using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KluboviLige.Models
{
    public class League
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [MaxLength(4)]
        public string AbbName { get; set; }
        [Range(0, 2017)]
        public int YearOfEst { get; set; }
    }
}