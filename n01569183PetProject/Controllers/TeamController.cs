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

        public ActionResult New()
        {
            return View();
        }

        // GET: Team
        public ActionResult List()
        {
            string url = "TeamData/ListTeams";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<Team> Teams = response.Content.ReadAsAsync<IEnumerable<Team>>().Result;
            return View(Teams);
        }

        public ActionResult Show(int id)
        {
            string url = "TeamData/FindTeamWithRoles/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            TeamDto Team = response.Content.ReadAsAsync<TeamDto>().Result;

            return View(Team);

        }

        public ActionResult Update(int id)
        {
            string url = "TeamData/FindTeamWithRoles/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            TeamDto Team = response.Content.ReadAsAsync<TeamDto>().Result;

            return View(Team);

        }

        public ActionResult ConfirmDelete(int id)
        {
            string url = "TeamData/FindTeam/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            TeamDto Team = response.Content.ReadAsAsync<TeamDto>().Result;

            return View(Team);
        }

        public ActionResult Delete(int TeamId)
        {
            string url = "TeamData/DeleteTeam/" + TeamId;
            HttpResponseMessage response = client.GetAsync(url).Result;
            return Redirect("/Team/List");
        }

        [HttpPost]
        public ActionResult Add(string TeamName, string TeamWinCondition, string TeamDescription, string TeamColor)
        {
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


        public ActionResult Save(Team Team, HttpPostedFileBase TeamIcon)
        {

        
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
    }
}