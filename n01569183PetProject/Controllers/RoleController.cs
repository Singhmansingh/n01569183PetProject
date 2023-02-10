using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Web.Script.Serialization;
using n01569183PetProject.Models;
using System.Diagnostics;

namespace n01569183PetProject.Controllers
{
    public class RoleController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static RoleController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44391/api/");
        }
        // GET: Role
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            string url = "RoleData/ListRoles";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<Role> Roles = response.Content.ReadAsAsync<IEnumerable<Role>>().Result;
            return View(Roles);
        }

        public ActionResult Show(int id)
        {
            string url = "RoleData/FindRoleWithTeam/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            RoleDto Role = response.Content.ReadAsAsync<RoleDto>().Result;

            return View(Role);

        }   

        public ActionResult New()
        {
            string url = "TeamData/ListTeams";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<Team> Teams = response.Content.ReadAsAsync<IEnumerable<Team>>().Result;
            return View(Teams);
        }

        public ActionResult Save(Role Role)
        {
            string url = "RoleData/UpdateRole/" + Role.RoleId;
            HttpContent content = Prepare(Role);
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            return Redirect("/Role/list");


        }

        public ActionResult ConfirmDelete(int id)
        {
            string url = "RoleData/FindRole/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            Role Role = response.Content.ReadAsAsync<Role>().Result;

            return View(Role);
        }

        public ActionResult Delete(int RoleId)
        {
            string url = "RoleData/DeleteRole/" + RoleId;
            HttpResponseMessage response = client.GetAsync(url).Result;
            return Redirect("/Role/List");
        }

        public ActionResult Submit(Role role)
        {
            string url = "RoleData/AddRole";
            HttpContent content = Prepare(role);
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            return Redirect("/Role/list");
        }

        private HttpContent Prepare(Role role)
        {
            string jsonpayload = jss.Serialize(role);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            return content;
        }
    }
}