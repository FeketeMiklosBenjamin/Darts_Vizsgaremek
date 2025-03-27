using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace DartsMobilApp.Classes
{
    public class FriendlyMatchModel
    {
        public string? id { get; set; }

        public string? name { get; set; }

        public string? playerLevel { get; set; }

        public bool?  levelLocked { get; set; }

        public int? setsCount { get; set; }

        public int? legsCount { get; set; }

        public int? startingPoint { get; set; }

        public string? joinPassword { get; set; }

    }
}
