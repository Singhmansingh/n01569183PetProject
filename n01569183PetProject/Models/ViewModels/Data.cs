using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace n01569183PetProject.Models
{
    public class Data
    {
        public IEnumerable<Role> Roles { get; set; }
        public IEnumerable<Team> Teams { get; set; }
        public IEnumerable<Player> Players { get; set; }
    }
}