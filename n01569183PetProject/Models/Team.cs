using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace n01569183PetProject.Models
{
    public class Team
    {
        [Key]
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public string TeamColor { get; set; }
        public string TeamDescription { get; set; }
        public string TeamWinCondition { get; set; }
        public bool TeamHasImg { get; set; }
        public string TeamImgExt { get; set; }

    }

    public class TeamDto
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public string TeamColor { get; set; }
        public string TeamDescription { get; set; }
        public string TeamWinCondition { get; set; }
        public bool TeamHasImg { get; set; }
        public string TeamImgExt { get; set; }
        public List<Role> TeamRoles { get; set; }
    }

}