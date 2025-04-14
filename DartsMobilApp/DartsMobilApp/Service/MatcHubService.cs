using DartsMobilApp.Classes;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DartsMobilApp.Services
{
    public class SignalRService
    {
        private HubConnection _hubConnection;
        private readonly string _hubUrl = "https://disciplinary-marj-feketemiklos222-91053eff.koyeb.app/matchHub";

        public event Action<StartFriendlyMatchModel>? OnFriendlyMatchStarted;
        public event Action<string, string, string>? OnFriendlyPlayerJoined;
        public event Action<string>? OnFriendlyPlayerRemoved;
        public event Action<string>? OnFriendlyPlayerLeft;
        public event Action<StartFriendlyMatchModel>? OnTournamentMatchStarted;
        public event Action<int> OnGetPoints;

        public async Task ConnectAsync(string jwtToken)
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(_hubUrl, options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(jwtToken);
                })
                .WithAutomaticReconnect()
                .Build();

            RegisterEvents();

            try
            {
                await _hubConnection.StartAsync();
                Console.WriteLine("SignalR connected");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SignalR connection failed: {ex.Message}");
            }
        }

        private void RegisterEvents()
        {
            _hubConnection.On<StartFriendlyMatchModel>("FriendlyMatchStarted", (startingSetup) =>
            {
                OnFriendlyMatchStarted?.Invoke(startingSetup);
            });

            _hubConnection.On< int>("GetPoints", (points) =>
            {
                OnGetPoints?.Invoke(points);
            });

            _hubConnection.On<string, string, string>("FriendlyPlayerJoined", (playerId, username, dartsPoint) =>
            {
                OnFriendlyPlayerJoined?.Invoke(playerId, username, dartsPoint);
            });

            _hubConnection.On<string>("FriendlyPlayerRemoved", (removerId) =>
            {
                OnFriendlyPlayerRemoved?.Invoke(removerId);
            });

            _hubConnection.On<string>("FriendlyPlayerLeft", (leaverId) =>
            {
                OnFriendlyPlayerLeft?.Invoke(leaverId);
            });

            _hubConnection.On<StartFriendlyMatchModel>("TournamentMatchStarted", (startingSetup) =>
            {
                OnTournamentMatchStarted?.Invoke(startingSetup);
            });
        }


        public async Task EndFriendlyMatch(string matchId, string playerId, EndMatchModel stats)
        {
                if (_hubConnection.State == HubConnectionState.Connected)
                {
                    await _hubConnection.InvokeAsync("EndFriendlyMatch", matchId, playerId, stats);
                }
        }
        public async Task PassPoints(string matchId,string playerId, int points)
        {
            if (_hubConnection.State == HubConnectionState.Connected)
            {
                await _hubConnection.InvokeAsync("PassPoints", matchId, playerId,points);
            }

        }
        public async Task JoinFriendlyMatch(string matchId, string playerId, string username, string dartsPoint)
        {
            if (_hubConnection.State == HubConnectionState.Connected)
            {
                await _hubConnection.InvokeAsync("JoinFriendlyMatch", matchId, playerId, username, dartsPoint);
            }
        }

        public async Task StartFriendlyMatch(string matchId, string playerId, string secondPlayerId)
        {
            if (_hubConnection.State == HubConnectionState.Connected)
            {
                await _hubConnection.InvokeAsync("StartFriendlyMatch", matchId, playerId, secondPlayerId);
            }
        }

        public async Task RemovePlayerFromFriendlyMatch(string matchId, string playerId, string removablePlayerId)
        {
            if (_hubConnection.State == HubConnectionState.Connected)
            {
                await _hubConnection.InvokeAsync("RemovePlayerFromFriendlyMatch", matchId, playerId, removablePlayerId);
            }
        }

        public async Task LeaveFriendlyMatch(string matchId, string playerId)
        {
            if (_hubConnection.State == HubConnectionState.Connected)
            {
                await _hubConnection.InvokeAsync("LeaveFriendlyMatch", matchId, playerId);
            }
        }

        public async Task JoinTournamentMatch(string matchId, string playerId)
        {
            if (_hubConnection.State == HubConnectionState.Connected)
            {
                await _hubConnection.InvokeAsync("JoinTournamentMatch", matchId, playerId);
            }
        }

        public async Task DisconnectAsync()
        {
            if (_hubConnection != null)
            {
                await _hubConnection.StopAsync();
                await _hubConnection.DisposeAsync();
            }
        }
    }
}
