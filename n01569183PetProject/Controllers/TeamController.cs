using n01569183PetProject.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace n01569183PetProject.Controllers
{
    public class TeamController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static TeamController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44391/api/");
        }

        // GET: Team/New
        [Authorize]
        public ActionResult New()
        {
            GetApplicationCookie();
            return View();
        }

        // GET: Team/List
        public ActionResult List()
        {
            string url = "TeamData/ListTeams";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<Team> Teams = response.Content.ReadAsAsync<IEnumerable<Team>>().Result;
            return View(Teams);
        }
        // GET: Team/Show/3

        public ActionResult Show(int id)
        {
            string url = "TeamData/FindTeamWithRoles/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            TeamDto Team = response.Content.ReadAsAsync<TeamDto>().Result;

            return View(Team);

        }

        // GET: Team/Update/3

        [Authorize]
        public ActionResult Update(int id)
        {
            GetApplicationCookie();

            string url = "TeamData/FindTeamWithRoles/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            TeamDto Team = response.Content.ReadAsAsync<TeamDto>().Result;

            return View(Team);

        }

        // GET: Team/ConfirmDelete/3
        public ActionResult ConfirmDelete(int id)
        {
            string url = "TeamData/FindTeam/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            TeamDto Team = response.Content.ReadAsAsync<TeamDto>().Result;

            return View(Team);
        }

        // GET: Team/Delete/3
        [Authorize]
        public ActionResult Delete(int TeamId)
        {
            string url = "TeamData/DeleteTeam/" + TeamId;
            HttpResponseMessage response = client.GetAsync(url).Result;
            return Redirect("/Team/List");
        }

        // POST: Team/Add
        // BODY: Team

        [HttpPost]
        [Authorize]
        public ActionResult Add(string TeamName, string TeamWinCondition, string TeamDescription, string TeamColor)
        {
            GetApplicationCookie();

            string url = "TeamData/AddTeam";

            Team Team = new Team()
            {
                TeamName = TeamName,
                TeamWinCondition = TeamWinCondition,
                TeamColor = TeamColor,
                TeamDescription = TeamDescription,
            };

            string jsonpayload = jss.Serialize(Team);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return Redirect("/Team/List");
        }


        // POST: Team/Save
        // BODY: Team, TeamIcon

        [Authorize]

        public ActionResult Save(Team Team, HttpPostedFileBase TeamIcon)
        {

            GetApplicationCookie();

            string jsonpayload = jss.Serialize(Team);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            string url = "TeamData/UpdateTeam/" + Team.TeamId;
            HttpResponseMessage response = client.PostAsync(url, content).Result;


            if (TeamIcon != null)
            {
                Debug.Write("Uploading Team image...");
                url = "TeamData/UploadTeamImg/" + Team.TeamId;

                MultipartFormDataContent requestcontent = new MultipartFormDataContent();
                HttpContent imagecontent = new StreamContent(TeamIcon.InputStream);
                requestcontent.Add(imagecontent, "teamImg", TeamIcon.FileName);
                response = client.PostAsync(url, requestcontent).Result;

            }


            return Redirect("Show/"+ Team.TeamId);
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