using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IdentityMvcWeb.App_Start;
namespace IdentityMvcWeb.Controllers
{

    [Authorize(Roles=CustomRoles.Director)]
    public class DirectorController : Controller
    {
        // GET: Director



        public ActionResult Index()
        {
            return View();
        }
    }
}