using n01569183PetProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace n01569183PetProject.Controllers
{
    public class PlayerDataController : ApiController
    {
        public ApplicationDbContext db = new ApplicationDbContext();
        

        /// <summary>
        /// List all players in the Database
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: List of all Players in the Database
        /// </returns>
        /// <example>
        /// GET: /api/PlayerData/ListPlayers
        /// </example>
        [HttpGet]
        [Route("api/PlayerData/ListPlayers")]
        [ResponseType(typeof(Player))]
        public IHttpActionResult ListPlayers()
        {
            List<Player> Players = db.Players.ToList();

            return Ok(Players);
        }


        /// <summary>
        /// Add a new Player
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// </returns>
        /// <param name="PlayerData">Player. New Player Data.</param>
        /// <example>
        /// POST: /api/PlayerData/AddPlayer
        /// BODY: PlayerData
        /// </example>
        [HttpPost]
        public IHttpActionResult AddPlayer([FromBody] Player PlayerData)
        {
            Debug.Write(PlayerData.PlayerName);
            db.Players.Add(PlayerData);
            db.SaveChanges();
            return Ok();
        }


        /// <summary>
        /// Finds a specific Player by Id
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: Player Data with the specified ID
        /// </returns>
        /// <param name="PlayerId">Integer. ID of the Player.</param>
        /// <example>
        /// GET: /api/PlayerData/FindPlayer/2
        /// </example>          
        [HttpGet]
        [Route("api/PlayerData/FindPlayer/{PlayerId}")]
        [ResponseType(typeof(Player))]
        public IHttpActionResult FindPlayer(int PlayerId)
        {
            Player FoundPlayer = db.Players.Find(PlayerId);

            return Ok(FoundPlayer);
        }

        /// <summary>
        /// Finds a specific Player by Name
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: Player Data with the specified Name
        /// </returns>
        /// <param name="PlayerName">String. Name of the Player.</param>
        /// <example>
        /// GET: /api/PlayerData/FindPlayer/John
        /// </example>          
        [HttpGet]
        [Route("api/PlayerData/FindPlayerByName/{PlayerName}")]
        [ResponseType(typeof(Player))]
        public IHttpActionResult FindPlayerByName(string PlayerName)
        {
            Player FoundPlayer = new Player();

            try
            {
               FoundPlayer = db.Players.Where(player=>player.PlayerName==PlayerName).First();
            }
            catch
            {
                return BadRequest();
            }

            return Ok(FoundPlayer);
        }

        /// <summary>
        /// Removes a specific Player from the Database
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// </returns>
        /// <param name="PlayerId">Integer. ID of the Player.</param>
        /// <example>
        /// GET: /api/PlayerData/DeletePlayer/2
        /// </example>     
        [HttpGet]
        [Route("api/PlayerData/DeletePlayer/{PlayerId}")]
        public IHttpActionResult DeletePlayer(int PlayerId)
        {
            Player FoundPlayer = db.Players.Find(PlayerId);
            db.Players.Remove(FoundPlayer);
            db.SaveChanges();
            return Ok();
        }


        /// <summary>
        /// Updates the data for a specific Player.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// </returns>
        /// <param name="PlayerId">Integer. ID of the Player.</param>
        /// <param name="PlayerData">Player. Data for the Player.</param>
        /// <example>
        /// POST: /api/PlayerData/UpdatePlayer/2
        /// BODY: New Player Data for the player at ID 2
        /// </example> 
        [HttpPost]
        [Route("api/PlayerData/UpdatePlayer/{PlayerId}")]
        [ResponseType(typeof(Player))]
        public IHttpActionResult UpdatePlayer(int PlayerId, [FromBody] Player PlayerData)
        {
            db.Players.AddOrUpdate(PlayerData);
            db.SaveChanges();
            return Ok(db.Players.Find(PlayerId));
        }

        /// <summary>
        /// Toggles the Living/Dead State of the Player.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// </returns>
        /// <param name="PlayerId">Integer. ID of the Player.</param>
        /// <example>
        /// GET: /api/PlayerData/ToggleLiveState/2
        /// </example> 
        [HttpGet]
        [Route("api/PlayerData/ToggleLiveState/{PlayerId}")]
        [ResponseType(typeof(Player))]
        public IHttpActionResult ToggleLiveState(int PlayerId)
        {
            Debug.WriteLine(PlayerId);
            Player Player = db.Players.Find(PlayerId);
            Player.PlayerAlive = !Player.PlayerAlive;
            db.Players.AddOrUpdate(Player);
            db.SaveChanges();
            return Ok(db.Players.Find(PlayerId));
        }

        /// <summary>
        /// Reset all player's living state to True.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// </returns>
        /// <example>
        /// GET: /api/PlayerData/ResetAllLiveState
        /// </example> 
        [HttpGet]
        [Route("api/PlayerData/ResetAllLiveState")]
        public IHttpActionResult ResetAllLiveState()
        {
            db.Players.ToList().ForEach(Player =>
            {
                Player.PlayerAlive = true;
            });
            db.SaveChanges();
            return Ok();
        }

        /// <summary>
        /// Lists all the players on a specific Team.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: List of Players
        /// </returns>
        /// <param name="TeamId">Integer. ID of the Team.</param>
        /// <example>
        /// GET: /api/PlayerData/ListPlayersForTeam/1
        /// </example> 
        [HttpGet]
        [Route("api/PlayerData/ListPlayersForTeam/{TeamId}")]
        [ResponseType(typeof(Player))]
        public IHttpActionResult ListPlayersForTeam(int TeamId)
        {
            List<Player> Players = db.Players.Where(p => p.Role.TeamId == TeamId).ToList();
            return Ok(Players);
        }

        /// <summary>
        /// Associates a Player with a specified Role.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// </returns>
        /// <param name="PlayerId">Integer. ID of the Player.</param>
        /// <param name="RoleId">Integer. ID of the Role.</param>
        /// <example>
        /// GET: /api/PlayerData/SetPlayerRole/2/1
        /// </example> 
        [HttpGet]
        [Route("api/PlayerData/SetPlayerRole/{PlayerId}/{RoleId}")]

        public IHttpActionResult SetPlayerRole(int PlayerId, int RoleId)
        {
            Player Player = db.Players.Find(PlayerId);
            Player.RoleId = RoleId;
            UpdatePlayer(PlayerId,Player);
            return Ok();

        }

        /// <summary>
        /// Randomly Assigns all Players a usable Role.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: List of Players
        /// </returns>
        /// <example>
        /// GET: /api/PlayerData/ShufflePlayerRoles
        /// </example> 
        [HttpGet]
        [Route("api/PlayerData/ShufflePlayerRoles")]
        [ResponseType(typeof(Player))]

        public IHttpActionResult ShufflePlayerRoles()
        {
            List<Role> Roles = db.Roles.Where(Role => Role.RoleInPlay).ToList();
            IEnumerable<Player> Players = db.Players.ToList();
            //src : https://stackoverflow.com/questions/108819/best-way-to-randomize-an-array-with-net
            var rng = new Random();

            foreach (Player player in Players)
            {
                int index = rng.Next(0, Roles.Count());

                SetPlayerRole(player.PlayerId, Roles[index].RoleId);
                Debug.WriteLine("Currently at index " + index + " with Role " + Roles[index].RoleName);
                int CurrentRoleId = Roles[index].RoleId;
                List<Player> PlayersUsingThisRole = db.Players.Where(p => p.RoleId == CurrentRoleId).ToList();
                Debug.WriteLine("Currently at index " + index + " with Role " + Roles[index].RoleName + ". " + PlayersUsingThisRole.Count() + "players have this role.");

                if (Roles[index].RoleMaxCount > 0 && PlayersUsingThisRole.Count() >= Roles[index].RoleMaxCount)
                {
                    Debug.WriteLine("Removing at index " + index + " Role " + Roles[index].RoleName);
                    Roles.RemoveAt(index);
                }

            }

            return Ok(db.Players.ToList());
        }
    }
}
