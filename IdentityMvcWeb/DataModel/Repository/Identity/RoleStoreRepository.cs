using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kloud21.GenericRepository;
using Kloud21.ADODAL;
using Kloud21.DataModel;
using System.Security.Claims;

namespace Kloud21.DataModel.Identity
{
    public class RoleStoreRepository : DbRepository<long,IdentityRole>
    {
        public RoleStoreRepository(IDbSession dbSession)
            : base(dbSession)
        {
        }

        public IdentityRole GetRoleByName(string roleName)
        {
            var spName = StoreProcPrefix + TableName + "_by_Name";
            var paramId = "Name";
            var result = ExecuteStoreProcedureReader(spName, new Dictionary<string, object> { { paramId, roleName } });
            return result.FirstOrDefault();
        }
    }
}
