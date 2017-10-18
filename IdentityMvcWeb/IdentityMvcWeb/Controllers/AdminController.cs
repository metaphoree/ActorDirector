using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IdentityMvcWeb.App_Start;
using Microsoft.AspNet.Identity.Owin;
using Kloud21.DataModel.Identity;

namespace IdentityMvcWeb.Controllers
{

    [Authorize(Roles =CustomRoles.Admin)]
    public class AdminController : Controller
    {

        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }



        // GET: Admin
        public ActionResult Index()
        {
            IQueryable<IdentityRole> role = RoleManager.Roles;

            return View(role);
        }
    }
}