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
    public class PlayerDataController : ApiController
    {
        public ApplicationDbContext db = new ApplicationDbContext();
        //ListPlayers
        [HttpGet]
        [Route("api/PlayerData/ListPlayers")]

        public IEnumerable<Player> ListPlayers()
        {
            List<Player> Players = db.Players.ToList();

            return Players;
        }
        //AddPlayer
        [HttpPost]
        public void AddPlayer([FromBody] Player PlayerData)
        {
            Debug.Write(PlayerData.PlayerName);
            db.Players.Add(PlayerData);
            db.SaveChanges();
        }
        //FindPlayer
        [HttpGet]
        [Route("api/PlayerData/FindPlayer/{PlayerId}")]
        public Player FindPlayer(int PlayerId)
        {
            Player FoundPlayer = db.Players.Find(PlayerId);

            return FoundPlayer;
        }

        //FindPlayer
        [HttpGet]
        [Route("api/PlayerData/FindPlayerByName/{PlayerName}")]
        public Player FindPlayerByName(string PlayerName)
        {
            Player FoundPlayer = new Player();

            try
            {
               FoundPlayer = db.Players.Where(player=>player.PlayerName==PlayerName).First();
            }
            catch
            {
                return null;
            }

            return FoundPlayer;
        }




        //DeletePlayer
        [HttpGet]
        [Route("api/PlayerData/DeletePlayer/{PlayerId}")]
        public bool DeletePlayer(int PlayerId)
        {
            Player FoundPlayer = FindPlayer(PlayerId);
            db.Players.Remove(FoundPlayer);
            db.SaveChanges();
            return true;
        }
        //UpdatePlayer
        [HttpPost]
        [Route("api/PlayerData/UpdatePlayer/{PlayerId}")]
        public Player UpdatePlayer(int PlayerId, [FromBody] Player PlayerData)
        {
            db.Players.AddOrUpdate(PlayerData);
            db.SaveChanges();
            return FindPlayer(PlayerId);
        }

        // TogglePlayerState
        [HttpGet]
        [Route("api/PlayerData/ToggleLiveState/{PlayerId}")]
        public Player ToggleLiveState(int PlayerId)
        {
            Debug.WriteLine(PlayerId);
            Player Player = FindPlayer(PlayerId);
            Player.PlayerAlive = !Player.PlayerAlive;
            Player = UpdatePlayer(PlayerId, Player);
            return Player;
        }

        [HttpGet]
        [Route("api/PlayerData/ListPlayersForTeam/{TeamId}")]
        public IEnumerable<Player> ListPlayersForTeam(int TeamId)
        {
            List<Player> Players = db.Players.Where(p => p.Role.TeamId == TeamId).ToList();
            return Players;
        }

        [HttpGet]
        [Route("api/PlayerData/SetPlayerRole/{PlayerId}/{RoleId}")]

        public void SetPlayerRole(int PlayerId, int RoleId)
        {
            Player Player = FindPlayer(PlayerId);
            Player.RoleId = RoleId;
            UpdatePlayer(PlayerId,Player);
        }

        [HttpGet]
        [Route("api/PlayerData/ShufflePlayerRoles")]
        public IEnumerable<Player> ShufflePlayerRoles()
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

            return db.Players.ToList();
        }
    }
}
