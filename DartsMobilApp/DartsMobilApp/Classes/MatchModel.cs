using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DartsMobilApp.Classes
{

    public class MatchModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public string level { get; set; }
        public DateTime startDate { get; set; }
        public string opponentName { get; set; }

        public DateTime? formattedTime { get
            {
                return startDate.ToLocalTime();
            }
        }
    }

}
