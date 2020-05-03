using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KluboviLige.Models
{
    public class ClubDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Town { get; set; }
        public string LeagueAbb { get; set; }
        public int YearOfEst { get; set; }
    }
}