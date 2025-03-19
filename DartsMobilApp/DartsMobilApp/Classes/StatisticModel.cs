using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DartsMobilApp.Classes
{

    public class StatisticModel
    {
        public string id { get; set; }
        public string userId { get; set; }
        public int matches { get; set; }
        public int matchesWon { get; set; }
        public int sets { get; set; }
        public int setsWon { get; set; }
        public int legs { get; set; }
        public int legsWon { get; set; }
        public float averages { get; set; }
        public int max180s { get; set; }
        public float checkoutPercentage { get; set; }
        public int highestCheckout { get; set; }
        public int nineDarter { get; set; }

    }

}
