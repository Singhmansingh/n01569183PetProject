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
using System.Web.Http.Description;
using System.Web.Routing;

namespace n01569183PetProject.Controllers
{
    public class TeamDataController : ApiController
    {
        public ApplicationDbContext db = new ApplicationDbContext();
        

        /// <summary>
        /// List all Teams in the Database.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all teams in the database
        /// </returns>
        /// <example>
        /// GET: api/TeamData/ListTeams
        /// </example>
        [HttpGet]
        [ResponseType(typeof(Team))]
        public IHttpActionResult ListTeams()
        {
            List<Team> Teams = db.Teams.ToList();

            return Ok(Teams);
        }

        /// <summary>
        /// List all Teams with associated Roles
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: List of Team DTOs
        /// </returns>
        /// <example>
        /// GET: /api/TeamData/ListTeamsWithRoles
        /// </example>
        [HttpGet]
        [Route("api/TeamData/ListTeamsWithRoles")]
        [ResponseType(typeof(TeamDto))]
        public IHttpActionResult ListTeamsWithRoles()
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

            return Ok(dtos);
        }


        /// <summary>
        /// Delete a specific Team.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// </returns>
        /// <example>
        /// GET: /api/TeamData/DeleteTeam/1
        /// </example>
        [HttpGet]
        [Route("api/TeamData/DeleteTeam/{TeamId}")]
        public IHttpActionResult DeleteTeam(int TeamId)
        {
            Team FoundTeam = db.Teams.Find(TeamId);
            db.Teams.Remove(FoundTeam);
            db.SaveChanges();
            return Ok();
        }



        /// <summary>
        /// Update a specific Team.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: Team
        /// </returns>
        /// <param name="TeamId">Integer. ID of a Team.</param>
        /// <param name="TeamData">Team. Data for the Team</param>
        /// <example>
        /// POST: /api/TeamData/UpdateTeam/1
        /// BODY: Team
        /// </example>
        [HttpPost]
        [Route("api/TeamData/UpdateTeam/{TeamId}")]
        [ResponseType(typeof(Team))]
        public IHttpActionResult UpdateTeam(int TeamId, [FromBody] Team TeamData)
        {
            Team FoundTeam = db.Teams.Find(TeamId);
            if (FoundTeam != null)
            {
                if (!TeamData.TeamHasImg) TeamData.TeamHasImg = FoundTeam.TeamHasImg;
                if (TeamData.TeamImgExt == null) TeamData.TeamImgExt = FoundTeam.TeamImgExt;
            }
            db.Teams.AddOrUpdate(TeamData);
            db.SaveChanges();

            return Ok(db.Teams.Find(TeamId));
        }

        /// <summary>
        /// Add a new Team
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// </returns>
        /// <param name="TeamData">Team. Data for new Team.</param>
        /// <example>
        /// POST: /api/TeamData/AddTeam
        /// BODY: Team
        /// </example>
        [HttpPost]
        public IHttpActionResult AddTeam([FromBody] Team TeamData)
        {
            Debug.Write(TeamData.TeamName);
            db.Teams.Add(TeamData);
            db.SaveChanges();
            return Ok();
        }


        /// <summary>
        /// Find a specific Team
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: Team
        /// </returns>
        /// <example>
        /// GET: /api/TeamData/FindTeam/1
        /// </example>
        [HttpGet]
        [ResponseType(typeof(Team))]
        [Route("api/TeamData/FindTeam/{TeamId}")]
        public IHttpActionResult FindTeam(int TeamId)
        {
            Team FoundTeam = db.Teams.Find(TeamId);

            return Ok(FoundTeam);
        }

        /// <summary>
        /// Get a Team with all associated Roles.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: Team DTO
        /// </returns>
        /// <example>
        /// GET: /api/TeamData/FindTeamWithRoles/1
        /// </example>
        [HttpGet]
        [Route("api/TeamData/FindTeamWithRoles/{TeamId}")]
        [ResponseType(typeof(TeamDto))]
        public IHttpActionResult FindTeamWithRoles(int TeamId)
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


            return Ok(Team);
        }


        /// <summary>
        /// Get a list of Teams with associated Players that are currently Alive.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: TeamPlayers
        /// </returns>
        /// <example>
        /// GET: /api/TeamData/ListTeamsWithLivingPlayers
        /// </example>
        [HttpGet]
        [Route("api/TeamData/ListTeamsWithLivingPlayers")]
        [ResponseType(typeof(TeamPlayers))]
        public IHttpActionResult ListTeamsWithLivingPlayers()
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
            return Ok(TeamPlayers);


        }




        /// <summary>
        /// Upload an image for a specific Team.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// </returns>
        /// <param name="TeamId">Integer. ID of a Team.</param>
        /// <example>
        /// POST: /api/TeamData/UploadTeamImg/1
        /// BODY: Image File
        /// </example>
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
    }
}
