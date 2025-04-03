using Microsoft.AspNetCore.SignalR;

namespace Vizsga_Backend.SignalR
{
    public class MatchHub : Hub
    {
        private static Dictionary<string, List<string>> matchPlayers = new();

        public async Task JoinMatch(string matchId, string playerId)
        {
            if (!matchPlayers.ContainsKey(matchId))
            {
                matchPlayers[matchId] = new List<string>();
            }

            if (!matchPlayers[matchId].Contains(playerId))
            {
                matchPlayers[matchId].Add(playerId);
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, matchId);
            await Clients.Group(matchId).SendAsync("PlayerJoined", playerId);

            if (matchPlayers[matchId].Count == 2)
            {
                await Clients.Group(matchId).SendAsync("MatchStarted");
            }
        }

        public async Task LeaveMatch(string matchId, string playerId)
        {
            if (matchPlayers.ContainsKey(matchId))
            {
                matchPlayers[matchId].Remove(playerId);

                if (matchPlayers[matchId].Count == 0)
                {
                    matchPlayers.Remove(matchId);
                }
            }

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, matchId);
            await Clients.Group(matchId).SendAsync("PlayerLeft", playerId);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var match = matchPlayers.FirstOrDefault(m => m.Value.Contains(Context.ConnectionId));

            if (!string.IsNullOrEmpty(match.Key))
            {
                await LeaveMatch(match.Key, Context.ConnectionId);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
