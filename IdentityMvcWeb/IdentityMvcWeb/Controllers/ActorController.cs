using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IdentityMvcWeb.App_Start;
namespace IdentityMvcWeb.Controllers
{
    [Authorize(Roles = CustomRoles.Actor)]
    public class ActorController : Controller
    {
        // GET: Actor
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TestingPost() {

            return View();
        }




    }
}