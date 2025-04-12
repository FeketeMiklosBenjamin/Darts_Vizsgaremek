using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using Vizsga_Backend.Models.SignalRModels;
using Vizsga_Backend.Services;
using VizsgaBackend.Services;

namespace Vizsga_Backend.SignalR
{
    public class MatchHub : Hub
    {
        private static readonly ConcurrentDictionary<string, List<string>> matchPlayers = new(); // matchId, List<playerId>

        private static readonly ConcurrentDictionary<string, string> playerConnections = new(); // playerId, connectionId

        private readonly MatchHeaderService _matchHeaderService;
        private readonly UserService _userService;

        public MatchHub(MatchHeaderService matchHeaderService, UserService userService)
        {
            _matchHeaderService = matchHeaderService;
            _userService = userService;
        }



        // StartMatch

        public async Task JoinFriendlyMatch(string matchId, string playerId, string username, string dartsPoint)
        {
            playerConnections[playerId] = Context.ConnectionId;

            if (!matchPlayers.ContainsKey(matchId))
            {
                matchPlayers[matchId] = new List<string>();
            }

            if (!matchPlayers[matchId].Contains(playerId))
            {
                matchPlayers[matchId].Add(playerId);
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, matchId);

            if (matchPlayers[matchId].Count > 1)
            {
                string firstPlayerId = matchPlayers[matchId][0];
                if (firstPlayerId != playerId)
                {
                    await Clients.User(firstPlayerId).SendAsync("FriendlyPlayerJoined", playerId, username, dartsPoint);
                }
            }
        }



        public async Task StartFriendlyMatch(string matchId, string playerId, string secondPlayerId)
        {
            if (matchPlayers.TryGetValue(matchId, out var players))
            {
                if (players.Contains(playerId) && players.Contains(secondPlayerId))
                {
                    var allowedPlayers = new HashSet<string> { playerId, secondPlayerId };

                    foreach (var pId in players.ToList())
                    {
                        if (!allowedPlayers.Contains(pId) && playerConnections.TryGetValue(pId, out var connId))
                        {
                            await Groups.RemoveFromGroupAsync(connId, matchId);
                        }
                    }
                    matchPlayers[matchId] = new List<string>() { playerId, secondPlayerId };

                    var rnd = new Random();
                    var startingPlayer = rnd.Next(0, 2) == 0 ? true : false;

                    var match = await _matchHeaderService.GetByIdAsync(matchId);

                    var PlayerOne = await _userService.GetByIdAsync(playerId);

                    var PlayerTwo = await _userService.GetByIdAsync(secondPlayerId);

                    StartFriendlyMatchModel startFriendlyMatchModel = new StartFriendlyMatchModel() { 
                        StartingPlayer = startingPlayer,
                        PlayerOneName = (startingPlayer ? PlayerOne!.Username : PlayerTwo!.Username),
                        PlayerTwoName = (!startingPlayer ? PlayerOne!.Username : PlayerTwo!.Username),
                        LegCount = match!.LegsCount,
                        SetCount = (match.SetsCount == null ? 1 : (int)match.SetsCount),
                        StartingPoint = match.StartingPoint
                    };

                    await Clients.Group(matchId).SendAsync("FriendlyMatchStarted", startFriendlyMatchModel);
                }
                else
                {
                    await Clients.Caller.SendAsync("MatchStartFailed", "One or both players are not in the match.");
                }
            }
            else
            {
                await Clients.Caller.SendAsync("MatchStartFailed", "Match not found.");
            }
        }


        public async Task RemovePlayerFromFriendlyMatch(string matchId, string playerId, string removablePlayerId)
        {
            if (matchPlayers.ContainsKey(matchId) && matchPlayers[matchId][0] == playerId)
            {
                if (playerConnections.TryGetValue(removablePlayerId, out string? connectionId))
                {
                    await Groups.RemoveFromGroupAsync(connectionId, matchId);
                    matchPlayers[matchId].Remove(removablePlayerId);
                    playerConnections.TryRemove(removablePlayerId, out _);
                    await Clients.Client(connectionId).SendAsync("FriendlyPlayerRemoved", playerId);
                }
            }
        }

        public async Task LeaveFriendlyMatch(string matchId, string playerId)
        {
            if (matchPlayers.ContainsKey(matchId))
            {
                matchPlayers[matchId].Remove(playerId);

                if (matchPlayers[matchId].Count == 0)
                {
                    matchPlayers.TryRemove(matchId, out _);
                }
            }

            if (playerConnections.TryRemove(playerId, out string? connectionId))
            {
                await Groups.RemoveFromGroupAsync(connectionId, matchId);
            }

            if (matchPlayers.ContainsKey(matchId) && matchPlayers[matchId].Count > 0)
            {
                string firstPlayerId = matchPlayers[matchId][0];
                await Clients.User(firstPlayerId).SendAsync("FriendlyPlayerLeft", playerId);
            }
        }

        public async Task JoinTournamentMatch(string matchId, string playerId)
        {
            playerConnections[playerId] = Context.ConnectionId;

            if (!matchPlayers.ContainsKey(matchId))
            {
                matchPlayers[matchId] = new List<string>();
            }

            if (!matchPlayers[matchId].Contains(playerId))
            {
                matchPlayers[matchId].Add(playerId);
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, matchId);

            if (matchPlayers[matchId].Count == 2)
            {
                await Clients.Group(matchId).SendAsync("TournamentMatchStarted");
            }
        }

        public async Task PassPoints(string matchId, string senderplayerId, int points)
        {
            if (matchPlayers.TryGetValue(matchId, out var players))
            {
                var toPlayerId = players.Where(x => x != senderplayerId).First();
                await Clients.User(toPlayerId).SendAsync("GetPoints", points);
            }
        }


        // Communication


        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var player = playerConnections.FirstOrDefault(x => x.Value == Context.ConnectionId);
            string playerId = player.Key;

            if (!string.IsNullOrEmpty(playerId))
            {
                var match = matchPlayers.FirstOrDefault(m => m.Value.Contains(playerId));
                if (!string.IsNullOrEmpty(match.Key))
                {
                    await LeaveFriendlyMatch(match.Key, playerId);
                }
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
