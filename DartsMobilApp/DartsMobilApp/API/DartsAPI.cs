using DartsMobilApp.Classes;
using DartsMobilApp.Network;
using System;
using System.Collections.Generic;
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
    }
}
