using n01569183PetProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
    }
}