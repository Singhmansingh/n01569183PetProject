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
        [Route("api/PlayerData/SetPlayerRole/{PlayerId}/{RoleId}")]

        public void SetPlayerRole(int PlayerId, int RoleId)
        {
            Player Player = FindPlayer(PlayerId);
            Player.RoleId = RoleId;
            UpdatePlayer(PlayerId,Player);
        }
    }
}
