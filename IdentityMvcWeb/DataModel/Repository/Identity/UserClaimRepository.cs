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
    public class UserClaimRepository : DbRepository<long, IdentityUserClaim>
    {
        public UserClaimRepository(IDbSession dbSession)
            : base(dbSession)
        {
        }

        /// <summary>
        /// Returns a ClaimsIdentity instance given a userId
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public ClaimsIdentity FindByUserId(long userId)
        {
            ClaimsIdentity claims = new ClaimsIdentity();
            //string commandText = "Select * from UserClaims where UserId = @userId";
            //Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@UserId", userId } };

            //var rows = _database.Query(commandText, parameters);
            //foreach (var row in rows)
            //{
            //    Claim claim = new Claim(row["ClaimType"], row["ClaimValue"]);
            //    claims.AddClaim(claim);
            //}


            var spName = StoreProcPrefix + TableName + "_by_Id";
            var paramId = TableName.ToLower() + "Id";
            var result = ExecuteStoreProcedureReader(spName, new Dictionary<string, object> { { paramId, userId } });

            claims = result as ClaimsIdentity;
            return claims;
        }

        /// <summary>
        /// Deletes all claims from a user given a userId
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public int Delete(long userId)
        {
            var spName = StoreProcPrefix + TableName + "_by_Id";
            var paramId = TableName.ToLower() + "Id";
            var result = ExecuteStoreProcedureScalar <int>(spName, new Dictionary<string, object> { { paramId, userId } });
            return result;
        }

        /// <summary>
        /// Inserts a new claim in UserClaims table
        /// </summary>
        /// <param name="userClaim">User's claim to be added</param>
        /// <param name="userId">User's id</param>
        /// <returns></returns>
        public int Insert(Claim userClaim, long userId)
        {
            var spName = StoreProcPrefix + TableName + "_by_Id";
            var paramId = TableName.ToLower() + "Id";
            var result = ExecuteStoreProcedureScalar<int>(spName, new Dictionary<string, object> { { paramId, userId } });
            return result;
        }

        /// <summary>
        /// Deletes a claim from a user 
        /// </summary>
        /// <param name="user">The user to have a claim deleted</param>
        /// <param name="claim">A claim to be deleted from user</param>
        /// <returns></returns>
        public int Delete(IdentityUser user, Claim claim)
        {
            var spName = StoreProcPrefix + TableName + "_by_Id";
            var paramId = TableName.ToLower() + "Id";
            var result = ExecuteStoreProcedureScalar<int>(spName, new Dictionary<string, object> { { paramId, user.Id } });
            return result;
        }
    }
}
