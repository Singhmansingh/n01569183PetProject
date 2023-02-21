using n01569183PetProject.Models;
using n01569183PetProject.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
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
        [HttpPost]
        [Route("api/TeamData/UploadTeamImg/{TeamId}")]
        public IHttpActionResult UploadTeamImg(int TeamId)
        {
            bool haspic = false;
            string picextension;
            if (Request.Content.IsMimeMultipartContent())
            {
                Debug.WriteLine("Received multipart form data.");

                int numfiles = HttpContext.Current.Request.Files.Count;
                Debug.WriteLine("Files Received: " + numfiles);

                //Check if a file is posted
                if (numfiles == 1 && HttpContext.Current.Request.Files[0] != null)
                {
                    var teamImg = HttpContext.Current.Request.Files[0];
                    //Check if the file is empty
                    if (teamImg.ContentLength > 0)
                    {
                        //establish valid file types (can be changed to other file extensions if desired!)
                        var valtypes = new[] { "jpeg", "jpg", "png", "gif" };
                        var extension = Path.GetExtension(teamImg.FileName).Substring(1);
                        //Check the extension of the file
                        if (valtypes.Contains(extension))
                        {
                            try
                            {
                                //file name is the id of the image
                                string fn = TeamId + "." + extension;

                                //get a direct file path to ~/Content/animals/{id}.{extension}
                                string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/Images/Teams/"), fn);

                                //save the file
                                teamImg.SaveAs(path);

                                //if these are all successful then we can set these fields
                                haspic = true;
                                picextension = extension;

                                //Update the animal haspic and picextension fields in the database
                                Team SelectedTeam = db.Teams.Find(TeamId);
                                SelectedTeam.TeamHasImg = haspic;
                                SelectedTeam.TeamImgExt = extension;
                                db.Entry(SelectedTeam).State = EntityState.Modified;

                                db.SaveChanges();

                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine("Team Image was not saved successfully.");
                                Debug.WriteLine("Exception:" + ex);
                                return BadRequest();
                            }
                        }
                    }

                }

                return Ok();
            }
            else
            {
                //not multipart form data
                return BadRequest();

            }
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
            Team.TeamHasImg = FoundTeam.TeamHasImg;
            Team.TeamImgExt = FoundTeam.TeamImgExt;
            Team.TeamRoles = db.Roles.Where(r=> r.TeamId == TeamId).ToList();


            return Team;
        }

        [HttpGet]
        [Route("api/TeamData/ListTeamsWithLivingPlayers")]
        public IEnumerable<TeamPlayers> ListTeamsWithLivingPlayers()
        {
            IEnumerable<Team> Teams = db.Teams.ToList();
            List<TeamPlayers> TeamPlayers = new List<TeamPlayers>();
            foreach(Team Team in Teams)
            {
                IEnumerable<Player> Players = db.Players.Where(player => player.Role.TeamId == Team.TeamId).ToList();
                if(Players.Count() > 0)
                {
                    TeamPlayers _tp = new TeamPlayers();
                    _tp.Team = Team;
                    _tp.Players = Players;
                    TeamPlayers.Add(_tp);
                }
                
            }
            return TeamPlayers;


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
