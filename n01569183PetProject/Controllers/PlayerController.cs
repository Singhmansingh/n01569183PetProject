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
            return Redirect("List");
        }

        // GET: Player/List
        public ActionResult List()
        {
            string url = "PlayerData/ListPlayers";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<Player> Players = response.Content.ReadAsAsync<IEnumerable<Player>>().Result;
            return View(Players);
        }

        // GET: Player/Show/3
        [Authorize]
        public ActionResult Show(int id)
        {
            string url = "PlayerData/FindPlayer/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            Player Player = response.Content.ReadAsAsync<Player>().Result;

            return View(Player);

        }

        // GET: Player/ToggleState/3
        public ActionResult ToggleState(int id)
        {
            string url = "PlayerData/ToggleLiveState/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            return Redirect("/Player/List");

        }

        // GET: Player/New

        [Authorize]
        public ActionResult New()
        {
            GetApplicationCookie();
            string url = "TeamData/ListTeamsWithRoles";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<TeamDto> Teams = response.Content.ReadAsAsync<IEnumerable<TeamDto>>().Result;
            return View(Teams);
        }

        // POST: Player/Save
        // BODY: Player
        [Authorize]
        [HttpPost]
        public ActionResult Save(Player Player)
        {
            GetApplicationCookie();
            string url = "PlayerData/UpdatePlayer/" + Player.PlayerId;
            HttpContent content = Prepare(Player);
            Debug.WriteLine(Player.PlayerScore);
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            return Redirect("/Player/list");


        }

        // POST: Player/Add
        // BODY: Player
        [Authorize]
        public ActionResult Add(Player Player)
        {
            GetApplicationCookie();
            string url = "PlayerData/AddPlayer";
            HttpContent content = Prepare(Player);
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            return Redirect("/Player/list");

        }


        // GET: Player/ConfirmDelete/3
        public ActionResult ConfirmDelete(int id)
        {
            string url = "PlayerData/FindPlayer/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            Player Player = response.Content.ReadAsAsync<Player>().Result;

            return View(Player);
        }

        // POST: Player/Delete/3
        [Authorize]
        [HttpPost]
        public ActionResult Delete(int PlayerId)
        {
            GetApplicationCookie();
            string url = "PlayerData/DeletePlayer/" + PlayerId;
            HttpResponseMessage response = client.GetAsync(url).Result;
            return Redirect("/Player/List");
        }

        // POST: Player/Submit
        // BODY: Player
        [Authorize]
        public ActionResult Submit(Player Player)
        {
            GetApplicationCookie();
            string url = "PlayerData/AddPlayer";
            HttpContent content = Prepare(Player);
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            return Redirect("/Player/list");
        }

        // Prepares a JSON playload
        private HttpContent Prepare(Player Player)
        {
            string jsonpayload = jss.Serialize(Player);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            return content;
        }

        private void GetApplicationCookie()
        {
            string token = "";
            //HTTP client is set up to be reused, otherwise it will exhaust server resources.
            //This is a bit dangerous because a previously authenticated cookie could be cached for
            //a follow-up request from someone else. Reset cookies in HTTP client before grabbing a new one.
            client.DefaultRequestHeaders.Remove("Cookie");
            if (!User.Identity.IsAuthenticated) return;

            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies.Get(".AspNet.ApplicationCookie");
            if (cookie != null) token = cookie.Value;

            //collect token as it is submitted to the controller
            //use it to pass along to the WebAPI.
            Debug.WriteLine("Token Submitted is : " + token);
            if (token != "") client.DefaultRequestHeaders.Add("Cookie", ".AspNet.ApplicationCookie=" + token);

            return;
        }
    }
}