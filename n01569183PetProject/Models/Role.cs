using System;

using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace n01569183PetProject.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }

        public bool RoleHasImg { get; set; }
        public string RoleImgExt { get; set; }
        public int RoleMaxCount { get; set; }
        public bool RoleInPlay { get; set; }

        [ForeignKey("Team")]
        public int TeamId { get; set; }
        public virtual Team Team { get; set; }

    }

    public class RoleDto
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
        public int RoleMaxCount { get; set; }
        public bool RoleInPlay { get; set; }


        public Team Team { get; set; }  
        public IEnumerable<Team> Teams { get; set; }
        public bool RoleHasImg { get; set; }
        public string RoleImgExt { get; set; }
    }

    public class RoleIdSet
    {
        public int[] RoleIds { get; set; }
    }
}