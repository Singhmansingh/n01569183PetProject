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
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                //cookies are manually set in RequestHeader
                UseCookies = false
            };

            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://localhost:44391/api/");
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
            string url = "RoleData/FindRole/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            RoleDto Role = response.Content.ReadAsAsync<RoleDto>().Result;
            return View(Role);

        }
        [HttpGet]
        [Authorize]
        public ActionResult Update(int id)
        {
            GetApplicationCookie();
            string url = "RoleData/FindRole/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            RoleDto Role = response.Content.ReadAsAsync<RoleDto>().Result;

            return View(Role);

        }

        [Authorize]
        public ActionResult New()
        {
            GetApplicationCookie();
            string url = "TeamData/ListTeams";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<Team> Teams = response.Content.ReadAsAsync<IEnumerable<Team>>().Result;
            return View(Teams);
        }

        [Authorize]
        public ActionResult Save(Role Role, HttpPostedFileBase RoleIcon)
        {
            GetApplicationCookie();
            string url = "RoleData/UpdateRole/" + Role.RoleId;
            HttpContent content = Prepare(Role);
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if(RoleIcon != null)
            {
                url = "RoleData/UploadRoleImg/" + Role.RoleId;

                MultipartFormDataContent requestcontent = new MultipartFormDataContent();
                HttpContent imagecontent = new StreamContent(RoleIcon.InputStream);
                requestcontent.Add(imagecontent, "roleImg", RoleIcon.FileName);
                response = client.PostAsync(url, requestcontent).Result;

            }


            return Redirect("/Role/list");


        }

        public ActionResult ConfirmDelete(int id)
        {
            string url = "RoleData/FindRole/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            Role Role = response.Content.ReadAsAsync<Role>().Result;

            return View(Role);
        }
        [Authorize]

        public ActionResult Delete(int RoleId)
        {
            string url = "RoleData/DeleteRole/" + RoleId;
            HttpResponseMessage response = client.GetAsync(url).Result;
            return Redirect("/Role/List");
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