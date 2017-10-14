using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IdentityMvcWeb.App_Start;
using Microsoft.Owin;

[assembly: OwinStartupAttribute(typeof(IdentityMvcWeb.Startup))]
namespace IdentityMvcWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}