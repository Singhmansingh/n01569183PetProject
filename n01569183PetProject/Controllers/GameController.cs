using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using n01569183PetProject.Models;
using n01569183PetProject.Models.ViewModels;

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
            string TeamURL = "TeamData/ListTeamsWithLivingPlayers";

            HttpResponseMessage response = client.GetAsync(TeamURL).Result;
            IEnumerable<TeamPlayers> Teams = response.Content.ReadAsAsync<IEnumerable<TeamPlayers>>().Result;

            return View(Teams);
        }

        public ActionResult RoleSelect()
        {
            string url = "TeamData/ListTeamsWithRoles";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<TeamDto> Teams = response.Content.ReadAsAsync<IEnumerable<TeamDto>>().Result;
            return View(Teams);
        }
        public ActionResult Player()
        {
            return View();
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