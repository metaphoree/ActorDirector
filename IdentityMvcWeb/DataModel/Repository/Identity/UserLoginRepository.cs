using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kloud21.GenericRepository;
using Kloud21.ADODAL;
using Kloud21.DataModel;
using Microsoft.AspNet.Identity;

namespace Kloud21.DataModel.Identity
{
    public class UserLoginRepository : DbRepository<long, IdentityUserLoginInfo>
    {
        public UserLoginRepository(IDbSession dbSession)
            : base(dbSession)
        {
        }

        public List<UserLoginInfo> FindByUserID(long Id)
        {
            List<UserLoginInfo> logins = new List<UserLoginInfo>();
            var spName = StoreProcPrefix + Pluralize(TableName) + "_by_UserID";
            var logList = ExecuteStoreProcedureReader(spName, new Dictionary<string, object> { { "@Id", Id } });

            foreach (var list in logList)
            {
                var login = new UserLoginInfo(list.LoginProvider, list.ProviderKey);
                logins.Add(login);
            }
            return logins;
        }

    
    }
}
