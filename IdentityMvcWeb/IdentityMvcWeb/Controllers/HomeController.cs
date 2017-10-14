using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IdentityMvcWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
           
            return View();
        }

        //1st tutorial

        //var config = new MapperConfiguration(cfg => {

        //    cfg.CreateMap<AuthorModel, AuthorDTO>();

        //});

        //IMapper iMapper = config.CreateMapper();

        //var source = new AuthorModel();

        //var destination = iMapper.Map<AuthorModel, AuthorDTO>(source);

        //2nd tutorial mapping two different names same variable

        //var config = new MapperConfiguration(cfg => {

        //    cfg.CreateMap<AuthorModel, AuthorDTO>()

        //    .ForMember(destination => destination.ContactDetails,

        //   opts => opts.MapFrom(source => source.Contact));

        //});

    }
}
