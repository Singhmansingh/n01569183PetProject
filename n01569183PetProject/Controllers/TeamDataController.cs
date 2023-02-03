using n01569183PetProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace n01569183PetProject.Controllers
{
    public class TeamDataController : ApiController
    {
        public ApplicationDbContext db = new ApplicationDbContext();
        //ListTeams
        [HttpGet]
        public IEnumerable<Team> ListTeams()
        {
            List<Team> Teams = db.Teams.ToList();

            return Teams;
        }
        //AddTeam
        [HttpPost]
        public void AddTeam([FromBody] Team TeamData)
        {
           db.Teams.Add(TeamData);
        }
        //FindTeam
        //DeleteTeam
        //UpdateTeam
    }
}
