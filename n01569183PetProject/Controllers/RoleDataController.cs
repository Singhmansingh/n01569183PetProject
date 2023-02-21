using Microsoft.Ajax.Utilities;
using n01569183PetProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;

namespace n01569183PetProject.Controllers
{
    public class RoleDataController : ApiController
    {
        public ApplicationDbContext db = new ApplicationDbContext();
        //ListRoles
        [HttpGet]
        public IEnumerable<Role> ListRoles()
        {
            List<Role> Roles = db.Roles.ToList();
            Roles = Roles.OrderBy(role => role.TeamId).ToList();

            return Roles;
        }
        //AddRole
        [HttpPost]
        public void AddRole([FromBody] Role RoleData)
        {
            Debug.Write(RoleData.RoleName);
            db.Roles.Add(RoleData);
            db.SaveChanges();
        }

        [HttpGet]
        [Route("api/RoleData/ListRolesforTeam/{TeamId}")]
        public IEnumerable<Role> ListRolesforTeam(int TeamId)
        {
            List<Role> Roles = db.Roles.Where(role => role.TeamId == TeamId).ToList();
            return Roles;
        }

        //FindRole
        [HttpGet]
        [Route("api/RoleData/FindRole/{RoleId}")]
        public RoleDto FindRole(int RoleId)
        {
            Role FoundRole = db.Roles.Find(RoleId);
            if (FoundRole == null) return null;
            RoleDto dto = new RoleDto()
            {
                RoleId = FoundRole.RoleId,
                RoleDescription = FoundRole.RoleDescription,
                RoleHasImg = FoundRole.RoleHasImg,
                RoleName = FoundRole.RoleName,
                RoleImgExt = FoundRole.RoleImgExt,
                RoleMaxCount = FoundRole.RoleMaxCount,
                Team = FoundRole.Team,
                Teams = db.Teams.ToList()

            };

            return dto;
        }

        //DeleteRole
        [HttpGet]
        [Route("api/RoleData/DeleteRole/{RoleId}")]
        public bool DeleteRole(int RoleId)
        {
            Role FoundRole = db.Roles.Find(RoleId);
            db.Roles.Remove(FoundRole);
            db.SaveChanges();
            return true;
        }
        //UpdateRole
        [HttpPost]
        [Route("api/RoleData/UpdateRole/{RoleId}")]
        public RoleDto UpdateRole(int RoleId, [FromBody] Role RoleData)
        {
            Role FoundRole = db.Roles.Find(RoleId);
            if (FoundRole != null)
            {
                if (!RoleData.RoleHasImg) RoleData.RoleHasImg = FoundRole.RoleHasImg;
                if (RoleData.RoleImgExt == null) RoleData.RoleImgExt = FoundRole.RoleImgExt;
                if (!RoleData.RoleInPlay) RoleData.RoleInPlay = FoundRole.RoleInPlay;
            }

            
            db.Roles.AddOrUpdate(RoleData);
            db.SaveChanges();
            return FindRole(RoleId);
        }

        [HttpPost]
        [Route("api/RoleData/UseRoles")]

        public IHttpActionResult UseRoles([FromBody] RoleIdSet RoleIdSet)
        {
            db.Roles.ForEach(Role => Role.RoleInPlay = false);
            db.SaveChanges();
            foreach(int RoleId in RoleIdSet.RoleIds)
            {
                db.Roles.Where(Role => Role.RoleId == RoleId).First().RoleInPlay = true;
            }
            db.SaveChanges();

            return Ok();
        }

        [HttpPost]
        [Route("api/RoleData/UploadRoleImg/{RoleId}")]
        public IHttpActionResult UploadRoleImg(int RoleId)
        {
            bool haspic = false;
            string picextension;
            if (Request.Content.IsMimeMultipartContent())
            {
                Debug.WriteLine("Received multipart form data.");

                int numfiles = HttpContext.Current.Request.Files.Count;
                Debug.WriteLine("Files Received: " + numfiles);

                //Check if a file is posted
                if (numfiles == 1 && HttpContext.Current.Request.Files[0] != null)
                {
                    var roleImg = HttpContext.Current.Request.Files[0];
                    //Check if the file is empty
                    if (roleImg.ContentLength > 0)
                    {
                        //establish valid file types (can be changed to other file extensions if desired!)
                        var valtypes = new[] { "jpeg", "jpg", "png", "gif" };
                        var extension = Path.GetExtension(roleImg.FileName).Substring(1);
                        //Check the extension of the file
                        if (valtypes.Contains(extension))
                        {
                            try
                            {
                                //file name is the id of the image
                                string fn = RoleId + "." + extension;

                                //get a direct file path to ~/Content/animals/{id}.{extension}
                                string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/Images/Roles/"), fn);

                                //save the file
                                roleImg.SaveAs(path);

                                //if these are all successful then we can set these fields
                                haspic = true;
                                picextension = extension;

                                //Update the animal haspic and picextension fields in the database
                                Role SelectedRole = db.Roles.Find(RoleId);
                                SelectedRole.RoleHasImg = haspic;
                                SelectedRole.RoleImgExt = extension;
                                db.Entry(SelectedRole).State = EntityState.Modified;

                                db.SaveChanges();

                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine("Role Image was not saved successfully.");
                                Debug.WriteLine("Exception:" + ex);
                                return BadRequest();
                            }
                        }
                    }

                }

                return Ok();
            }
            else
            {
                //not multipart form data
                return BadRequest();

            }
        }

    }
}
