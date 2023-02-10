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

        [HttpGet]
        [Route("api/TeamData/ListTeamsWithRoles")]

        public IEnumerable<TeamDto> ListTeamsWithRoles()
        {
            List<Team> Teams = db.Teams.ToList();
            List<TeamDto> dtos = new List<TeamDto>();
            foreach(Team team in Teams)
            {
                TeamDto TeamDto = new TeamDto()
                {
                    TeamName = team.TeamName,
                    TeamDescription = team.TeamDescription,
                    TeamId = team.TeamId,
                    TeamColor = team.TeamColor,
                    TeamRoles = db.Roles.Where(r => r.TeamId == team.TeamId).ToList()
                };
                dtos.Add(TeamDto);

            }

            return dtos;
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
            Debug.Write("TeamData:"+(TeamData.TeamWinCondition));
            db.Teams.AddOrUpdate(TeamData);
            db.SaveChanges();

            return FindTeam(TeamId);
        }
    }
}
