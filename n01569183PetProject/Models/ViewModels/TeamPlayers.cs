using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace n01569183PetProject.Models.ViewModels
{
    public class TeamPlayers
    {
        public Team Team { get; set; }
        public IEnumerable<Player> Players { get; set; }
    }
}