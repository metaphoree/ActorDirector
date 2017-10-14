using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kloud21.DataModel
{
    public interface IUserIdentity
    {
        long UserId { get; set; }

        long ClinicId { get; set; }
    }
}
