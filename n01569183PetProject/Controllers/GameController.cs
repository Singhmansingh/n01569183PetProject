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
        [Authorize]
        public ActionResult Index()
        {
            GetApplicationCookie();
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