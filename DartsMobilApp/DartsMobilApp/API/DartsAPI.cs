using DartsMobilApp.Classes;
using DartsMobilApp.Network;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DartsMobilApp.API
{
    public static class DartsAPI
    {

        public static async Task<StatisticModel> GetStatistic()
        {
            try
            {
                return await HTTPCommunication<StatisticModel>.Get("https://disciplinary-marj-feketemiklos222-91053eff.koyeb.app/api/users/friendlystat");

            }
            catch (Exception)
            {

                throw;
            }
        }
        public static async Task<LoginResponse> PostLogin(StringContent loginModel)
        {
            try
            {
                return await HTTPCommunication<LoginResponse>.Post("https://disciplinary-marj-feketemiklos222-91053eff.koyeb.app/api/users/login", loginModel);

            }
            catch (Exception)
            {

                throw;
            }
        }


        public static async Task<List<MatchModel>> GetUserMatches()
        {
            try
            {
                return await HTTPCommunication<List<MatchModel>>.Get("https://disciplinary-marj-feketemiklos222-91053eff.koyeb.app/api/users/tournament_matches");
            }
            catch (Exception)
            {

                throw;
            }
        }


        public static async Task<List<FriendlyMatchModel>> GetFriendlyMatches()
        {
            try
            {
                return await HTTPCommunication<List<FriendlyMatchModel>>.Get("https://disciplinary-marj-feketemiklos222-91053eff.koyeb.app/api/friendly_matches");
            }
            catch (Exception)
            {

                throw;
            }
        }


        public static async Task<AccessTokenResponse> PostRefreshAndGetNewAccess(StringContent refreshToken, string UserId)
        {
            try
            {
                var response = await HTTPCommunication<AccessTokenResponse>.PostAToken($"https://disciplinary-marj-feketemiklos222-91053eff.koyeb.app/api/token/refresh-token/{UserId}", refreshToken);

                return response;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static async Task<dynamic> PostNewFriendlyMatch(StringContent newfriendlymatch)
        {
            try
            {
                var response = await HTTPCommunication<dynamic>.PostAToken("https://disciplinary-marj-feketemiklos222-91053eff.koyeb.app/api/friendly_matches", newfriendlymatch);
                return response;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static async Task<LogoutResponse> PostLogout(StringContent content)
        {
            try
            {
                return await HTTPCommunication<LogoutResponse>.PostAToken("https://disciplinary-marj-feketemiklos222-91053eff.koyeb.app/api/users/logout", content);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static async Task<ValidationResponse> ValidatePassword(StringContent content, string matchId)
        {
            try
            {
                var response = await HTTPCommunication<ValidationResponse>.PostAToken($"https://disciplinary-marj-feketemiklos222-91053eff.koyeb.app/api/matches/tournament/join/{matchId}", content);
                return response;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}