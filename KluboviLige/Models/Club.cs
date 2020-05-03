using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KluboviLige.Models
{
    public class Club
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Town { get; set; }
        [Required]
        [Range(0, 2016)]
        public int YearOfEst { get; set; }
        [Required]
        [Range(0, 1000000)]
        public decimal Price { get; set; }

        //foreign key
        public int LeagueId { get; set; }
        public League League { get; set; }


    }
}