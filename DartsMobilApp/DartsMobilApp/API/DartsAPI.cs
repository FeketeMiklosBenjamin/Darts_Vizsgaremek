using DartsMobilApp.Classes;
using DartsMobilApp.Network;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DartsMobilApp.API
{
    public static class DartsAPI
    {
        
        public static StatisticModel GetStatistic( )
        {
            try
            {
                return HTTPCommunication<StatisticModel>.Get("https://disciplinary-marj-feketemiklos222-91053eff.koyeb.app/api/users/friendlystat").Result;

            }
            catch (Exception)
            {

                throw;
            }
        }
        public static LoginResponse PostLogin(StringContent loginModel)
        {
            try
            {
                return HTTPCommunication<LoginResponse>.Post("https://disciplinary-marj-feketemiklos222-91053eff.koyeb.app/api/users/login", loginModel).Result;

            }
            catch (Exception)
            {

                throw;
            }
        }


        public static List<MatchModel> GetUserMatches()
        {
            try
            {
                return HTTPCommunication<List<MatchModel>>.Get("https://disciplinary-marj-feketemiklos222-91053eff.koyeb.app/api/users/tournament_matches")?.Result;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public static List<FriendlyMatchModel> GetFriendlyMatches() {
            try
            {
                return HTTPCommunication<List<FriendlyMatchModel>>.Get("https://disciplinary-marj-feketemiklos222-91053eff.koyeb.app/api/friendly_matches")?.Result;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public static AccessTokenResponse PostRefreshAndGetNewAccess(StringContent refreshToken, string UserId)
        {
            try
            {
                Debug.WriteLine($"RefreshToken:  {refreshToken} \n\n\n\n \t\t User Id:{UserId}");
                var response = HTTPCommunication<AccessTokenResponse>.PostAToken($"https://disciplinary-marj-feketemiklos222-91053eff.koyeb.app/api/token/refresh-token/{UserId}", refreshToken).Result;
                return response;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static dynamic PostNewFriendlyMatch(StringContent newfriendlymatch)
        {
            try
            {
                var response = HTTPCommunication<dynamic>.PostAToken("https://disciplinary-marj-feketemiklos222-91053eff.koyeb.app/api/friendly_matches", newfriendlymatch);
                return response;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static LogoutResponse PostLogout(StringContent content)
        {
            try
            {
                return HTTPCommunication<LogoutResponse>.PostAToken("https://disciplinary-marj-feketemiklos222-91053eff.koyeb.app/api/users/logout", content).Result;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
