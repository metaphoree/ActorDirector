using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kloud21.DataModel
{
    public class Entity : IIdentityKey<long>
    {
        public virtual long Id { get; set; }
    }
}
