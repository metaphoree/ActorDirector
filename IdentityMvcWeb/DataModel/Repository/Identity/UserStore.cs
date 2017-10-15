using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Kloud21.GenericRepository;
using Kloud21.ADODAL;
using System.Security.Claims;

namespace Kloud21.DataModel.Identity
{
    public class UserStore<TUser> : IUserStore<TUser, long>, IUserPasswordStore<TUser,long>,
        IUserSecurityStampStore<TUser, long>, IQueryableUserStore<TUser,long>, IUserEmailStore<TUser, long>, IUserPhoneNumberStore<TUser, long>,
        IUserTwoFactorStore<TUser,long>, IUserLoginStore<TUser,long>, IUserLockoutStore<TUser, long>, IUserRoleStore<TUser, long>//, IUserClaimStore<TUser, long>
        where TUser : IdentityUser
    {
        private bool _disposed=false;
        IConnectionStringProvider con = new ConfigurationConnectionStringProvider("DefaultConnection");
        IAdoNetProvider prov = new SqlServerAdoNetProvider();
        IDbSession dbSession;
        IdentityRepository identityRepository = null;
        UserClaimRepository claimRepository = null;
        UserRoleRepository roleRepository = null;
        UserLoginRepository userLoginRepository = null;
        RoleStoreRepository roleStoreRepository = null;

        public IdentityRepository IdentityUserRepository
        {
            get
            {
                if (identityRepository == null)
                {
                    identityRepository = new IdentityRepository(dbSession);
                }
                return identityRepository;
            }
        }

        public UserClaimRepository ClaimRepository
        {
            get
            {
                if (claimRepository == null)
                {
                    claimRepository = new UserClaimRepository(dbSession);
                }
                return claimRepository;
            }
        }

        public UserRoleRepository RoleRepository
        {
            get
            {
                if (roleRepository == null)
                {
                    roleRepository = new UserRoleRepository(dbSession);
                }
                return roleRepository;
            }
        }

        public RoleStoreRepository RoleStoreReposit
        {
            get
            {
                if (roleStoreRepository == null)
                {
                    roleStoreRepository = new RoleStoreRepository(dbSession);
                }
                return roleStoreRepository;
            }
        }


        public UserLoginRepository UserLoginRepository
        {
            get
            {
                if (userLoginRepository == null)
                {
                    userLoginRepository = new UserLoginRepository(dbSession);
                }
                return userLoginRepository;
            }
        }

        public IQueryable<TUser> Users
        {
            get
            {
                List<TUser> result = IdentityUserRepository.All() as List<TUser>;
                return result.AsQueryable();
            }
        }

        public UserStore()
        {
            dbSession = new DbSession(con, prov);
        }

        private void ThrowIfDisposed()
        {
            if (this._disposed)
                throw new ObjectDisposedException(this.GetType().Name);
        }


        #region  IUser Definition Starts Here
        public Task CreateAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            TUser result = IdentityUserRepository.Add(user) as TUser;

            if (result != null)
            {
                return Task.FromResult<TUser>(result);
            }

            return Task.FromResult<TUser>(null);

            //return Task.FromResult<object>(null);
        }

        public Task DeleteAsync(TUser user)
        {
            if (user != null)
            {
                IdentityUserRepository.Delete(user);
            }

            return Task.FromResult<Object>(null);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public Task<TUser> FindByIdAsync(long userId)
        {
            //if (long.IsNullOrEmpty(userId))
            //{
            //    throw new ArgumentException("Null or empty argument: userId");
            //}

            TUser result = IdentityUserRepository.GetByUserID(userId) as TUser;
            if (result != null)
            {
                return Task.FromResult<TUser>(result);
            }

            return Task.FromResult<TUser>(null);
        }

        public Task<TUser> FindByNameAsync(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentException("Null or empty argument: userName");
            }

            TUser result = IdentityUserRepository.GetByUserName(userName) as TUser;

            // Should I throw if > 1 user?
            if (result != null)
            {
                return Task.FromResult<TUser>(result);
            }

            return Task.FromResult<TUser>(null);
        }

        public Task UpdateAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            IdentityUserRepository.Update(user);

            return Task.FromResult<object>(null);
        }

        #endregion IUser Definition Ends Here

        //#region IUserClaimStore Starts Here


        //public Task<IList<Claim>> GetClaimsAsync(TUser user)
        //{
        //    ClaimsIdentity identity = ClaimRepository.FindByUserId(user.Id);

        //    return Task.FromResult<IList<Claim>>(identity.Claims.ToList());
        //}

        //public Task AddClaimAsync(TUser user, Claim claim)
        //{
        //    if (user == null)
        //    {
        //        throw new ArgumentNullException("user");
        //    }

        //    if (claim == null)
        //    {
        //        throw new ArgumentNullException("user");
        //    }

        //    ClaimRepository.Insert(claim, user.Id);

        //    return Task.FromResult<object>(null);
        //}

        //public Task RemoveClaimAsync(TUser user, Claim claim)
        //{
        //    if (user == null)
        //    {
        //        throw new ArgumentNullException("user");
        //    }

        //    if (claim == null)
        //    {
        //        throw new ArgumentNullException("claim");
        //    }

        //    ClaimRepository.Delete(user, claim);

        //    return Task.FromResult<object>(null);
        //}

        //#endregion

        #region IUserRoleStore Starts Here
        public Task AddToRoleAsync(TUser user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (string.IsNullOrEmpty(roleName))
            {
                throw new ArgumentException("Argument cannot be null or empty: roleName.");
            }
            // Must Change

            //string roleId = ""; //roleTable.GetRoleId(roleName);
            string roleId = Convert.ToString(RoleStoreReposit.GetRoleByName(roleName).Id); //roleTable.GetRoleId(roleName);
            if (!string.IsNullOrEmpty(roleId))
            {
                RoleRepository.AddUserRole(user,roleId);
            }

            return Task.FromResult<object>(null);
        }

        public Task RemoveFromRoleAsync(TUser user, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task<IList<IdentityUserRole>> GetRolesAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            IList<IdentityUserRole> roles = RoleRepository.GetAllRolesByUserID(user.Id);// userRolesTable.FindByUserId(user.Id);
            {
                if (roles != null)
                {
                    return Task.FromResult<IList<IdentityUserRole>>(roles);
                }
            }

            return Task.FromResult<IList<IdentityUserRole>>(null);
        }

        public Task<bool> IsInRoleAsync(TUser user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (string.IsNullOrEmpty(roleName))
            {
                throw new ArgumentNullException("roleName");
            }

            //List<string> roles = userRolesTable.FindByUserId(user.Id);
            IList<IdentityUserRole> roles = RoleRepository.GetAllRolesByUserID(user.Id);
            {
                foreach (IdentityUserRole role in roles)
                {
                    if (role.Name == roleName)
                    {
                        return Task.FromResult<bool>(true);
                    }
                }

            }

            return Task.FromResult<bool>(false);
        }

        Task<IList<string>> IUserRoleStore<TUser, long>.GetRolesAsync(TUser user)
        {
            List<string> roleList = new List<string>();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            var roles = RoleRepository.GetAllRolesByUserID(user.Id);// userRolesTable.FindByUserId(user.Id);
            {
                if (roles != null)
                {
                    foreach (var value in roles)
                    {
                        roleList.Add(value.Name);
                    }
                       
                    return Task.FromResult<IList<string>>(roleList);
                }
            }

            return Task.FromResult<IList<string>>(null);
        }



        #endregion


        #region IUserPasswordStore Starts Here
        public Task SetPasswordHashAsync(TUser user, string passwordHash)
        {
            user.PasswordHash = passwordHash;

            return Task.FromResult<Object>(null);
        }

        public Task<string> GetPasswordHashAsync(TUser user)
        {
            string passwordHash = IdentityUserRepository.GetPasswordHash(user.Id);

            return Task.FromResult<string>(passwordHash);
        }

        public Task<bool> HasPasswordAsync(TUser user)
        {
            var hasPassword = !string.IsNullOrEmpty(IdentityUserRepository.GetPasswordHash(user.Id));

            return Task.FromResult<bool>(Boolean.Parse(hasPassword.ToString()));
        }


        #endregion

        #region IUserSecurityStampStore Starts Here
        public Task SetSecurityStampAsync(TUser user, string stamp)
        {
            user.SecurityStamp = stamp;

            return Task.FromResult(0);
        }

        public Task<string> GetSecurityStampAsync(TUser user)
        {
            return Task.FromResult(user.SecurityStamp);
        }

        #endregion

        #region  IUserEmailStore Starts Here
        public Task SetEmailAsync(TUser user, string email)
        {
            user.Email = email;
            IdentityUserRepository.Update(user);

            return Task.FromResult(0);
        }

        public Task<string> GetEmailAsync(TUser user)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(TUser user)
        {
            return Task.FromResult(user.IsEmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(TUser user, bool confirmed)
        {
            user.IsEmailConfirmed = confirmed;
            IdentityUserRepository.Update(user);

            return Task.FromResult(0);
        }

        public Task<TUser> FindByEmailAsync(string email)
        {
            if (String.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException("email");
            }

            TUser result = IdentityUserRepository.GetUserByEmail(email) as TUser;
            if (result != null)
            {
                return Task.FromResult<TUser>(result);
            }

            return Task.FromResult<TUser>(null);
        }


        #endregion

        #region IUserPhoneNumberStore Starts Here
        public Task SetPhoneNumberAsync(TUser user, string phoneNumber)
        {
            user.PhoneNumber = phoneNumber;
            IdentityUserRepository.Update(user);

            return Task.FromResult(0);
        }

        public Task<string> GetPhoneNumberAsync(TUser user)
        {
            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(TUser user)
        {
            return Task.FromResult(user.IsPhoneNumberConfirmed);
        }

        public Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed)
        {
            user.IsPhoneNumberConfirmed = confirmed;
            IdentityUserRepository.Update(user);

            return Task.FromResult(0);
        }



        #endregion

        #region IUserTwoFactorStored

        public Task SetTwoFactorEnabledAsync(TUser user, bool enabled)
        {
            user.TwoFactorAuthEnabled = enabled;
            IdentityUserRepository.Update(user);

            return Task.FromResult(0);
        }

        public Task<bool> GetTwoFactorEnabledAsync(TUser user)
        {
            return Task.FromResult(user.TwoFactorAuthEnabled);
        }





        #endregion

        #region 

        public Task<DateTimeOffset> GetLockoutEndDateAsync(TUser user)
        {


            return
                Task.FromResult(new DateTimeOffset());

            //Task.FromResult(user.LockoutEndDate.HasValue
            //        ? new DateTimeOffset(DateTime.SpecifyKind(user.LockoutEndDate.Value, DateTimeKind.Utc))
            //        : new DateTimeOffset());
        }

        public Task SetLockoutEndDateAsync(TUser user, DateTimeOffset lockoutEnd)
        {
            user.LockoutEndDate = lockoutEnd.UtcDateTime;
            IdentityUserRepository.Update(user);

            return Task.FromResult(0);
        }

        public Task<int> IncrementAccessFailedCountAsync(TUser user)
        {
            user.AccessFailedCount++;
            IdentityUserRepository.Update(user);

            return Task.FromResult(user.AccessFailedCount);
        }

        public Task ResetAccessFailedCountAsync(TUser user)
        {
            user.AccessFailedCount = 0;
            IdentityUserRepository.Update(user);

            return Task.FromResult(0);
        }

        public Task<int> GetAccessFailedCountAsync(TUser user)
        {
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task<bool> GetLockoutEnabledAsync(TUser user)
        {
            return Task.FromResult(user.LockoutEnabled);
        }

        public Task SetLockoutEnabledAsync(TUser user, bool enabled)
        {
            user.LockoutEnabled = enabled;
            IdentityUserRepository.Update(user);

            return Task.FromResult(0);
        }

        #endregion



        #region IUserLoginStore
        public Task AddLoginAsync(TUser user, UserLoginInfo login)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (login == null)
            {
                throw new ArgumentNullException("login");
            }

            //userLoginsTable.Insert(user, login);

            return Task.FromResult<object>(null);
        }

        public Task RemoveLoginAsync(TUser user, UserLoginInfo login)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (login == null)
            {
                throw new ArgumentNullException("login");
            }

            //userLoginsTable.Delete(user, login);

            return Task.FromResult<Object>(null);
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user)
        {
            List<UserLoginInfo> userLogins = new List<UserLoginInfo>();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            List<UserLoginInfo> logins = UserLoginRepository.FindByUserID(user.Id);
            if (logins != null)
            {
                return Task.FromResult<IList<UserLoginInfo>>(logins);
            }

            return Task.FromResult<IList<UserLoginInfo>>(null);
        }

        public Task<TUser> FindAsync(UserLoginInfo login)
        {
            if (login == null)
            {
                throw new ArgumentNullException("login");
            }

            //var userId = userLoginsTable.FindUserIdByLogin(login);
            //if (userId != null)
            //{
            //    TUser user = userTable.GetUserById(userId) as TUser;
            //    if (user != null)
            //    {
            //        return Task.FromResult<TUser>(user);
            //    }
            //}

            return Task.FromResult<TUser>(null);
        }
        #endregion

    }
}
