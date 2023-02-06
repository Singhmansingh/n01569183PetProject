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

        [ForeignKey("Team")]
        public int TeamId { get; set; }
        public virtual Team Team { get; set; }

    }
}