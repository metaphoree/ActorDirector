using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kloud21.DataModel;
using System.Security.Claims;

namespace Kloud21.DataModel.Identity
{
    public class IdentityUserLoginInfo : Entity
    {
        //
        // Summary:
        //     Constructor
        //
        // Parameters:
        //   loginProvider:
        //
        //   providerKey:

        public IdentityUserLoginInfo()
        { }

        //
        // Summary:
        //     Provider for the linked login, i.e. Facebook, Google, etc.
        public string LoginProvider { get; set; }
        //
        // Summary:
        //     User specific key for the login provider
        public string ProviderKey { get; set; }
    }
}
