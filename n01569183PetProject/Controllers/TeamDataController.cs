using n01569183PetProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Diagnostics;
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
            Debug.Write(TeamData.TeamName);
            db.Teams.Add(TeamData);
            db.SaveChanges();
        }
        //FindTeam
        [HttpGet]
        [Route("api/TeamData/FindTeam/{TeamId}")]
        public Team FindTeam(int TeamId)
        {
            Team FoundTeam = db.Teams.Find(TeamId);

            return FoundTeam;
        }

        //FindTeamWithRoles
        [HttpGet]
        [Route("api/TeamData/FindTeamWithRoles/{TeamId}")]
        public TeamDto FindTeamWithRoles(int TeamId)
        {
            Team FoundTeam = db.Teams.Find(TeamId);

            TeamDto Team = new TeamDto();
            Team.TeamName = FoundTeam.TeamName;
            Team.TeamDescription = FoundTeam.TeamDescription;
            Team.TeamWinCondition = FoundTeam.TeamWinCondition;
            Team.TeamId = FoundTeam.TeamId;
            Team.TeamColor = FoundTeam.TeamColor;

            Team.TeamRoles = db.Roles.Where(r=> r.TeamId == TeamId).ToList();


            return Team;
        }


        //DeleteTeam
        [HttpGet]
        [Route("api/TeamData/DeleteTeam/{TeamId}")]
        public bool DeleteTeam(int TeamId)
        {
            Team FoundTeam = FindTeam(TeamId);
            db.Teams.Remove(FoundTeam);
            db.SaveChanges();
            return true;
        }
        //UpdateTeam
        [HttpPost]
        [Route("api/TeamData/UpdateTeam/{TeamId}")]
        public Team UpdateTeam(int TeamId, [FromBody] Team TeamData)
        {
            db.Teams.AddOrUpdate(TeamData);

            return FindTeam(TeamId);
        }
    }
}
