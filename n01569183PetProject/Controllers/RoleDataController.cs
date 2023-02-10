using n01569183PetProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

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
        //FindRole
        [HttpGet]
        [Route("api/RoleData/FindRole/{RoleId}")]
        public Role FindRole(int RoleId)
        {
            Role FoundRole = db.Roles.Find(RoleId);

            return FoundRole;
        }

        //FindRoleWithTeam
        [HttpGet]
        [Route("api/RoleData/FindRoleWithTeam/{RoleId}")]
        public RoleDto FindRoleWithTeam(int RoleId)
        {
            Role FoundRole = db.Roles.Find(RoleId);

            RoleDto Role = new RoleDto();
            Role.RoleName = FoundRole.RoleName;
            Role.RoleDescription = FoundRole.RoleDescription;
            Role.TeamId = FoundRole.TeamId;
            Role.RoleId = FoundRole.RoleId;
            Role.TeamName = FoundRole.Team.TeamName;
            Role.TeamColor = FoundRole.Team.TeamColor;

            return Role;
        }

        //DeleteRole
        [HttpGet]
        [Route("api/RoleData/DeleteRole/{RoleId}")]
        public bool DeleteRole(int RoleId)
        {
            Role FoundRole = FindRole(RoleId);
            db.Roles.Remove(FoundRole);
            db.SaveChanges();
            return true;
        }
        //UpdateRole
        [HttpPost]
        [Route("api/RoleData/UpdateRole/{RoleId}")]
        public Role UpdateRole(int RoleId, [FromBody] Role RoleData)
        {
            db.Roles.AddOrUpdate(RoleData);
            db.SaveChanges();
            return FindRole(RoleId);
        }
    }
}
