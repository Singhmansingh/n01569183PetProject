using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using n01569183PetProject.Models;

namespace n01569183PetProject.Controllers
{
    public class GameController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static GameController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44391/api/");
        }
        // GET: Game
        public ActionResult Index()
        {
            string TeamURL = "TeamData/ListTeams";
            string RoleURL = "RoleData/ListRoles";
            string PlayerURL = "PlayerData/ListPlayers";

            Data Data = new Data();

            HttpResponseMessage response = client.GetAsync(TeamURL).Result;
            Data.Teams = response.Content.ReadAsAsync<IEnumerable<Team>>().Result;

            response = client.GetAsync(RoleURL).Result;
            Data.Roles = response.Content.ReadAsAsync<IEnumerable<Role>>().Result;

            response = client.GetAsync(PlayerURL).Result;
            Data.Players = response.Content.ReadAsAsync<IEnumerable<Player>>().Result;

            return View(Data);
        }
        private HttpContent Prepare(Object Data)
        {
            string jsonpayload = jss.Serialize(Data);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            return content;
        }
    }
}