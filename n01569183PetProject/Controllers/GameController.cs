using System;
using System.Collections.Generic;
using System.Diagnostics;
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


            HttpResponseMessage response = client.GetAsync(TeamURL).Result;
            IEnumerable<Team> Teams = response.Content.ReadAsAsync<IEnumerable<Team>>().Result;

            return View(Teams);
        }

        public HttpContent GetTeamPlayers(int id)
        {
            string url = "PlayerData/GetPlayersByTeam" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<Player> Players = response.Content.ReadAsAsync<IEnumerable<Player>>().Result;

            HttpContent package = Prepare(Players);
            return package;

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