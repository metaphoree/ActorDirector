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
    public class UserRoleRepository : DbRepository<long,IdentityUserRole>
    {
        public UserRoleRepository(IDbSession dbSession)
            : base(dbSession)
        {
        }

        public IList<IdentityUserRole> GetAllRolesByUserID(long userID)
        {
            var spName = StoreProcPrefix + Pluralize(TableName) + "_by_UserId";
            return ExecuteStoreProcedureReader(spName, new Dictionary<string, object> { { "@UserId", userID } });
        }

        public void AddUserRole(IdentityUser user, string roleId)
        {
            var spName = "qsp_IdentityUserRole_Create";
            ExecuteStoreProcedureNonQuery(spName, new Dictionary<string, object> { { "@UserId", user.Id }, { "@RoleId", roleId } });
        }
    }
}
