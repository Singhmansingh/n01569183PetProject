using n01569183PetProject.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace n01569183PetProject.Controllers
{
    public class PlayerController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static PlayerController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44391/api/");
        }
        // GET: Player
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            string url = "PlayerData/ListPlayers";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<Player> Players = response.Content.ReadAsAsync<IEnumerable<Player>>().Result;
            return View(Players);
        }

        public ActionResult Show(int id)
        {
            string url = "PlayerData/FindPlayer/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            Player Player = response.Content.ReadAsAsync<Player>().Result;

            return View(Player);

        }

        public ActionResult New()
        {
            string url = "TeamData/ListTeamsWithRoles";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<TeamDto> Teams = response.Content.ReadAsAsync<IEnumerable<TeamDto>>().Result;
            return View(Teams);
        }

        public ActionResult Save(Player Player)
        {
            string url = "PlayerData/UpdatePlayer/" + Player.PlayerId;
            HttpContent content = Prepare(Player);
            Debug.WriteLine(content);
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            return Redirect("/Player/list");


        }

        public ActionResult Add(Player Player)
        {
            string url = "PlayerData/AddPlayer";
            HttpContent content = Prepare(Player);
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            return Redirect("/Player/list");

        }

        public ActionResult ConfirmDelete(int id)
        {
            string url = "PlayerData/FindPlayer/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            Player Player = response.Content.ReadAsAsync<Player>().Result;

            return View(Player);
        }

        public ActionResult Delete(int PlayerId)
        {
            string url = "PlayerData/DeletePlayer/" + PlayerId;
            HttpResponseMessage response = client.GetAsync(url).Result;
            return Redirect("/Player/List");
        }

        public ActionResult Submit(Player Player)
        {
            string url = "PlayerData/AddPlayer";
            HttpContent content = Prepare(Player);
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            return Redirect("/Player/list");
        }

        private HttpContent Prepare(Player Player)
        {
            string jsonpayload = jss.Serialize(Player);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            return content;
        }
    }
}