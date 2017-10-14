using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kloud21.GenericRepository;
using Kloud21.ADODAL;
using Kloud21.DataModel;

namespace Kloud21.DataModel.Identity
{
    public class IdentityRepository : DbRepository<long, IdentityUser>
    {
        public IdentityRepository(IDbSession dbSession)
            : base(dbSession)
        {
        }

        public override IdentityUser Add(IdentityUser user)
        {
            var result = new Dictionary<string, object>();
            result.Add("Email",user.Email);
            result.Add("EmailConfirmed", user.IsEmailConfirmed);
            result.Add("PasswordHash", user.PasswordHash);
            result.Add("SecurityStamp", user.SecurityStamp);
            result.Add("PhoneNumber", "");
            result.Add("PhoneNumberConfirmed", user.IsPhoneNumberConfirmed);
            result.Add("TwoFactorEnabled", user.TwoFactorAuthEnabled);
            //result.Add("LockoutEndDateUtc", ConvertFromDateTimeOffset(user.LockoutEndDate));
            result.Add("LockoutEnabled", user.LockoutEnabled);
            result.Add("AccessFailedCount", user.AccessFailedCount);
            result.Add("UserName", user.UserName);

            var spName = "qsp_IdentityUser_Create";
            ExecuteStoreProcedureNonQuery(spName, result);

            return GetByUserName(user.UserName);
        }

        public override void Update(IdentityUser user)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("@UserId", user.Id);
            parameters.Add("@Email", user.Email);
            parameters.Add("@EmailConfirmed", user.IsEmailConfirmed);
            parameters.Add("@PasswordHash", user.PasswordHash);
            parameters.Add("@SecurityStamp", user.SecurityStamp);
            parameters.Add("@PhoneNumber", user.PhoneNumber?? string.Empty);
            parameters.Add("@PhoneNumberConfirmed", user.IsPhoneNumberConfirmed);
            parameters.Add("@TwoFactorEnabled", user.TwoFactorAuthEnabled);
            ////parameters.Add("LockoutEndDateUtc", ConvertFromDateTimeOffset(user.LockoutEndDate));
            parameters.Add("@LockoutEnabled", user.LockoutEnabled);
            parameters.Add("@AccessFailedCount", user.AccessFailedCount);
            parameters.Add("@UserName", user.UserName);

            var spName = "qsp_IdentityUser_Update";
            ExecuteStoreProcedureNonQuery(spName, parameters);


        }

        static DateTime ConvertFromDateTimeOffset(DateTimeOffset dateTime)
        {
            if (dateTime.Offset.Equals(TimeSpan.Zero))
                return dateTime.UtcDateTime;
            else if (dateTime.Offset.Equals(TimeZoneInfo.Local.GetUtcOffset(dateTime.DateTime)))
                return DateTime.SpecifyKind(dateTime.DateTime, DateTimeKind.Local);
            else
                return dateTime.DateTime;
        }

        public IdentityUser GetByUserID(long userID)
        {
            var spName = StoreProcPrefix + TableName + "_by_Id";
            var paramId = "Id";
            var result = ExecuteStoreProcedureReader(spName, new Dictionary<string, object> { { paramId, userID } });
            return result.FirstOrDefault();
        }

        public IdentityUser GetByUserName(string userName)
        {
            var spName = StoreProcPrefix + TableName + "_by_UserName";
            var paramId = "UserName";
            var result = ExecuteStoreProcedureReader(spName, new Dictionary<string, object> { { paramId, userName } });
            return result.FirstOrDefault();
        }

        public IdentityUser GetUserByEmail(string userEmail)
        {
            var spName = StoreProcPrefix + TableName + "_by_Email";
            var paramId = "Email";
            var result = ExecuteStoreProcedureReader(spName, new Dictionary<string, object> { { paramId, userEmail } });
            return result.FirstOrDefault();
        }

        /// <summary>
        /// Return the user's password hash
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public string GetPasswordHash(long userId)
        {
            //string commandText = "Select PasswordHash from Users where Id = @id";
            //Dictionary<string, object> parameters = new Dictionary<string, object>();
            //parameters.Add("@id", userId);

            //var passHash = _database.GetStrValue(commandText, parameters);

            var spName = StoreProcPrefix + TableName + "_GetPasswordHash";
            var paramId = "Id";
            var passHash = ExecuteStoreProcedureScalar<string>(spName, new Dictionary<string, object> { { paramId, userId } });

            if (string.IsNullOrEmpty(passHash))
            {
                return null;
            }

            return passHash;
        }


    }
}
