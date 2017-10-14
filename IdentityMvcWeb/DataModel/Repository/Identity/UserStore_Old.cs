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
    //public class UserStore<TUser> : IUserStore<TUser>, IUserLoginStore<TUser>, IUserClaimStore<TUser>, IUserRoleStore<TUser>,
    //      IUserPasswordStore<TUser>, IUserSecurityStampStore<TUser>, IUserEmailStore<TUser>, IUserLockoutStore<TUser, string>,
    //      IUserTwoFactorStore<TUser, string>, IUserPhoneNumberStore<TUser>
    //      where TUser : IdentityUser
    //{
    //    private bool _disposed;

    //    IConnectionStringProvider con = new ConfigurationConnectionStringProvider("DefaultConnection");
    //    IAdoNetProvider prov = new SqlServerAdoNetProvider();
    //    IDbSession dbSession;
    //    IdentityRepository identityRepository = null;

    //    //private IDocumentSession session
    //    //{
    //    //    get
    //    //    {
    //    //        if (_session == null)
    //    //        {
    //    //            _session = getSessionFunc();
    //    //            _session.Advanced.DocumentStore.Conventions.RegisterIdConvention<IdentityUser>((dbname, commands, user) => "IdentityUsers/" + user.Id);
    //    //        }
    //    //        return _session;
    //    //    }
    //    //}

    //    public IdentityRepository IdentityUserRepository
    //    {
    //        get
    //        {
    //            if (identityRepository == null)
    //            {
    //                identityRepository = new IdentityRepository(dbSession);
    //            }
    //            return identityRepository;
    //        }
    //    }

    //    public UserStore()
    //    {
    //        dbSession = new DbSession(con, prov);
    //    }



    //    public Task CreateAsync(TUser user)
    //    {
    //        this.ThrowIfDisposed();
    //        if (user == null)
    //            throw new ArgumentNullException("user");
    //        if (string.IsNullOrEmpty(user.Id))
    //            throw new InvalidOperationException("user.Id property must be specified before calling CreateAsync");

    //        this.IdentityUserRepository.Add(user);

    //        // This model allows us to lookup a user by name in order to get the id
    //        //var userByName = new IdentityUserByUserName(user.Id, user.UserName);
    //        //this.session.Store(userByName, Util.GetIdentityUserByUserNameId(user.UserName));

    //        return Task.FromResult(true);
    //    }




    //    public Task UpdateAsync(TUser user)
    //    {
    //        this.ThrowIfDisposed();
    //        if (user == null)
    //            throw new ArgumentNullException("user");

    //        return Task.FromResult(true);
    //    }

    //    private void ThrowIfDisposed()
    //    {
    //        if (this._disposed)
    //            throw new ObjectDisposedException(this.GetType().Name);
    //    }

    //    public void Dispose()
    //    {
    //        this._disposed = true;
    //    }

    //    public Task AddLoginAsync(TUser user, UserLoginInfo login)
    //    {
    //        //this.ThrowIfDisposed();
    //        //if (user == null)
    //        //    throw new ArgumentNullException("user");

    //        //if (!user.Logins.Any(x => x.LoginProvider == login.LoginProvider && x.ProviderKey == login.ProviderKey))
    //        //{
    //        //    user.Logins.Add(login);

    //        //    this.IdentityUserRepository.Add(new IdentityUserLogin
    //        //    {
    //        //        Id = Util.GetLoginId(login),
    //        //        UserId = user.Id,
    //        //        Provider = login.LoginProvider,
    //        //        ProviderKey = login.ProviderKey
    //        //    });
    //        //}

    //        //return Task.FromResult(true);

    //        throw new NotImplementedException();
    //    }

    //    public Task<TUser> FindAsync(UserLoginInfo login)
    //    {
    //        //string loginId = Util.GetLoginId(login);

    //        //var loginDoc = session.Include<IdentityUserLogin>(x => x.UserId)
    //        //    .Load(loginId);

    //        //TUser user = null;

    //        //if (loginDoc != null)
    //        //    user = this.session.Load<TUser>(loginDoc.UserId);

    //        //return Task.FromResult(user);

    //        throw new NotImplementedException();
    //    }

    //    public Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user)
    //    {
    //        this.ThrowIfDisposed();
    //        if (user == null)
    //            throw new ArgumentNullException("user");

    //        return Task.FromResult(user.Logins.ToIList());
    //    }

    //    public Task RemoveLoginAsync(TUser user, UserLoginInfo login)
    //    {
    //        //this.ThrowIfDisposed();
    //        //if (user == null)
    //        //    throw new ArgumentNullException("user");

    //        //string loginId = Util.GetLoginId(login);
    //        //var loginDoc = this.session.Load<IdentityUserLogin>(loginId);
    //        //if (loginDoc != null)
    //        //    this.session.Delete(loginDoc);

    //        //user.Logins.RemoveAll(x => x.LoginProvider == login.LoginProvider && x.ProviderKey == login.ProviderKey);

    //        //return Task.FromResult(0);

    //        throw new NotImplementedException();
    //    }

    //    public Task AddClaimAsync(TUser user, Claim claim)
    //    {
    //        this.ThrowIfDisposed();
    //        if (user == null)
    //            throw new ArgumentNullException("user");

    //        if (!user.Claims.Any(x => x.ClaimType == claim.Type && x.ClaimValue == claim.Value))
    //        {
    //            user.Claims.Add(new IdentityUserClaim
    //            {
    //                ClaimType = claim.Type,
    //                ClaimValue = claim.Value
    //            });
    //        }
    //        return Task.FromResult(0);
    //    }

    //    public Task<IList<Claim>> GetClaimsAsync(TUser user)
    //    {
    //        this.ThrowIfDisposed();
    //        if (user == null)
    //            throw new ArgumentNullException("user");

    //        IList<Claim> result = user.Claims.Select(c => new Claim(c.ClaimType, c.ClaimValue)).ToList();
    //        return Task.FromResult(result);
    //    }

    //    public Task RemoveClaimAsync(TUser user, Claim claim)
    //    {
    //        this.ThrowIfDisposed();
    //        if (user == null)
    //            throw new ArgumentNullException("user");

    //        user.Claims.RemoveAll(x => x.ClaimType == claim.Type && x.ClaimValue == claim.Value);
    //        return Task.FromResult(0);
    //    }

    //    public Task<string> GetPasswordHashAsync(TUser user)
    //    {
    //        this.ThrowIfDisposed();
    //        if (user == null)
    //            throw new ArgumentNullException("user");

    //        return Task.FromResult(user.PasswordHash);
    //    }

    //    public Task<bool> HasPasswordAsync(TUser user)
    //    {
    //        this.ThrowIfDisposed();
    //        if (user == null)
    //            throw new ArgumentNullException("user");

    //        return Task.FromResult<bool>(user.PasswordHash != null);
    //    }

    //    public Task SetPasswordHashAsync(TUser user, string passwordHash)
    //    {
    //        this.ThrowIfDisposed();
    //        if (user == null)
    //            throw new ArgumentNullException("user");

    //        user.PasswordHash = passwordHash;
    //        return Task.FromResult(0);
    //    }

    //    public Task<string> GetSecurityStampAsync(TUser user)
    //    {
    //        this.ThrowIfDisposed();
    //        if (user == null)
    //            throw new ArgumentNullException("user");

    //        return Task.FromResult(user.SecurityStamp);
    //    }

    //    public Task SetSecurityStampAsync(TUser user, string stamp)
    //    {
    //        this.ThrowIfDisposed();
    //        if (user == null)
    //            throw new ArgumentNullException("user");

    //        user.SecurityStamp = stamp;
    //        return Task.FromResult(0);
    //    }

    //    public Task AddToRoleAsync(TUser user, string role)
    //    {
    //        this.ThrowIfDisposed();
    //        if (user == null)
    //            throw new ArgumentNullException("user");

    //        if (!user.Roles.Contains(role, StringComparer.InvariantCultureIgnoreCase))
    //            user.Roles.Add(role);

    //        return Task.FromResult(0);
    //    }

    //    public Task<IList<string>> GetRolesAsync(TUser user)
    //    {
    //        this.ThrowIfDisposed();
    //        if (user == null)
    //            throw new ArgumentNullException("user");

    //        return Task.FromResult<IList<string>>(user.Roles);
    //    }

    //    public Task<bool> IsInRoleAsync(TUser user, string role)
    //    {
    //        this.ThrowIfDisposed();
    //        if (user == null)
    //            throw new ArgumentNullException("user");

    //        return Task.FromResult(user.Roles.Contains(role, StringComparer.InvariantCultureIgnoreCase));
    //    }

    //    public Task RemoveFromRoleAsync(TUser user, string role)
    //    {
    //        this.ThrowIfDisposed();
    //        if (user == null)
    //            throw new ArgumentNullException("user");

    //        user.Roles.RemoveAll(r => String.Equals(r, role, StringComparison.InvariantCultureIgnoreCase));

    //        return Task.FromResult(0);
    //    }

    //    public Task<TUser> FindByEmailAsync(string email)
    //    {
    //        //this.ThrowIfDisposed();
    //        //if (email == null)
    //        //    throw new ArgumentNullException("email");

    //        //var result = this.session.Query<TUser>().Where(u => u.Email == email).FirstOrDefault();
    //        //return Task.FromResult(result);

    //        throw new NotImplementedException();
    //    }

    //    public Task<string> GetEmailAsync(TUser user)
    //    {
    //        this.ThrowIfDisposed();
    //        if (user == null)
    //            throw new ArgumentNullException("user");

    //        return Task.FromResult(user.Email);
    //    }

    //    public Task<bool> GetEmailConfirmedAsync(TUser user)
    //    {
    //        this.ThrowIfDisposed();
    //        if (user == null)
    //            throw new ArgumentNullException("user");

    //        return Task.FromResult(user.IsEmailConfirmed);
    //    }

    //    public Task SetEmailAsync(TUser user, string email)
    //    {
    //        this.ThrowIfDisposed();
    //        if (user == null)
    //            throw new ArgumentNullException("user");

    //        user.Email = email;

    //        return Task.FromResult(0);
    //    }

    //    public Task SetEmailConfirmedAsync(TUser user, bool confirmed)
    //    {
    //        this.ThrowIfDisposed();
    //        if (user == null)
    //            throw new ArgumentNullException("user");

    //        user.IsEmailConfirmed = confirmed;

    //        return Task.FromResult(0);
    //    }

    //    public Task<int> GetAccessFailedCountAsync(TUser user)
    //    {
    //        this.ThrowIfDisposed();
    //        if (user == null)
    //            throw new ArgumentNullException("user");

    //        return Task.FromResult(user.AccessFailedCount);
    //    }

    //    public Task<bool> GetLockoutEnabledAsync(TUser user)
    //    {
    //        this.ThrowIfDisposed();
    //        if (user == null)
    //            throw new ArgumentNullException("user");

    //        return Task.FromResult(user.LockoutEnabled);
    //    }

    //    public Task<DateTimeOffset> GetLockoutEndDateAsync(TUser user)
    //    {
    //        this.ThrowIfDisposed();
    //        if (user == null)
    //            throw new ArgumentNullException("user");

    //        return Task.FromResult(user.LockoutEndDate);
    //    }

    //    public Task<int> IncrementAccessFailedCountAsync(TUser user)
    //    {
    //        this.ThrowIfDisposed();
    //        if (user == null)
    //            throw new ArgumentNullException("user");

    //        user.AccessFailedCount++;

    //        return Task.FromResult(user.AccessFailedCount);
    //    }

    //    public Task ResetAccessFailedCountAsync(TUser user)
    //    {
    //        this.ThrowIfDisposed();
    //        if (user == null)
    //            throw new ArgumentNullException("user");

    //        user.AccessFailedCount = 0;

    //        return Task.FromResult(0);
    //    }

    //    public Task SetLockoutEnabledAsync(TUser user, bool enabled)
    //    {
    //        this.ThrowIfDisposed();
    //        if (user == null)
    //            throw new ArgumentNullException("user");

    //        user.LockoutEnabled = enabled;

    //        return Task.FromResult(0);
    //    }

    //    public Task SetLockoutEndDateAsync(TUser user, DateTimeOffset lockoutEnd)
    //    {
    //        this.ThrowIfDisposed();
    //        if (user == null)
    //            throw new ArgumentNullException("user");

    //        user.LockoutEndDate = lockoutEnd;

    //        return Task.FromResult(0);
    //    }

    //    public Task<bool> GetTwoFactorEnabledAsync(TUser user)
    //    {
    //        this.ThrowIfDisposed();
    //        if (user == null)
    //            throw new ArgumentNullException("user");

    //        return Task.FromResult(user.TwoFactorAuthEnabled);
    //    }

    //    public Task SetTwoFactorEnabledAsync(TUser user, bool enabled)
    //    {
    //        this.ThrowIfDisposed();
    //        if (user == null)
    //            throw new ArgumentNullException("user");

    //        user.TwoFactorAuthEnabled = enabled;

    //        return Task.FromResult(0);
    //    }

    //    public Task<string> GetPhoneNumberAsync(TUser user)
    //    {
    //        this.ThrowIfDisposed();
    //        if (user == null)
    //            throw new ArgumentNullException("user");

    //        return Task.FromResult(user.PhoneNumber);
    //    }

    //    public Task<bool> GetPhoneNumberConfirmedAsync(TUser user)
    //    {
    //        this.ThrowIfDisposed();
    //        if (user == null)
    //            throw new ArgumentNullException("user");

    //        return Task.FromResult(user.IsPhoneNumberConfirmed);
    //    }

    //    public Task SetPhoneNumberAsync(TUser user, string phoneNumber)
    //    {
    //        this.ThrowIfDisposed();
    //        if (user == null)
    //            throw new ArgumentNullException("user");

    //        user.PhoneNumber = phoneNumber;

    //        return Task.FromResult(0);
    //    }

    //    public Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed)
    //    {
    //        this.ThrowIfDisposed();
    //        if (user == null)
    //            throw new ArgumentNullException("user");

    //        user.IsPhoneNumberConfirmed = confirmed;

    //        return Task.FromResult(0);
    //    }

    //    public Task DeleteAsync(TUser user)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task<TUser> FindByIdAsync(string userId)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task<TUser> FindByNameAsync(string userName)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    Task<IList<Claim>> IUserClaimStore<TUser, string>.GetClaimsAsync(TUser user)
    //    {
    //        throw new NotImplementedException();
    //    }

    //}


    //public class UserStoreOld : IUserStore<IdentityUser, long>, IUserPasswordStore<IdentityUser, long>, IUserSecurityStampStore<IdentityUser, long>
    //{
    //    private bool _disposed;
    //    IConnectionStringProvider con = new ConfigurationConnectionStringProvider("DefaultConnection");
    //    IAdoNetProvider prov = new SqlServerAdoNetProvider();
    //    IDbSession dbSession;
    //    IdentityRepository identityRepository = null;

    //    public IdentityRepository IdentityUserRepository
    //    {
    //        get
    //        {
    //            if (identityRepository == null)
    //            {
    //                identityRepository = new IdentityRepository(dbSession);
    //            }
    //            return identityRepository;
    //        }
    //    }

    //    public UserStore()
    //    {
    //        dbSession = new DbSession(con, prov);
    //    }

    //    private void ThrowIfDisposed()
    //    {
    //        if (this._disposed)
    //            throw new ObjectDisposedException(this.GetType().Name);
    //    }

    //    public void Dispose()
    //    {
    //        this._disposed = true;
    //    }

    //    public Task CreateAsync(IdentityUser user)
    //    {
    //        this.ThrowIfDisposed();
    //        if (user == null)
    //            throw new ArgumentNullException("user");
    //        if (string.IsNullOrEmpty(user.Id))
    //            throw new InvalidOperationException("user.Id property must be specified before calling CreateAsync");

    //        IdentityUserRepository.Add(user);

    //        // This model allows us to lookup a user by name in order to get the id
    //        //var userByName = new IdentityUserByUserName(user.Id, user.UserName);
    //        //this.session.Store(userByName, Util.GetIdentityUserByUserNameId(user.UserName));

    //        return Task.FromResult(true);
    //    }

    //    public Task UpdateAsync(IdentityUser user)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task DeleteAsync(IdentityUser user)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task<IdentityUser> FindByIdAsync(string userId)
    //    {
    //        var user = this.IdentityUserRepository.GetByUserID(userId);
    //        return Task.FromResult(user);
    //    }

    //    public Task<IdentityUser> FindByNameAsync(string userName)
    //    {
    //        throw new NotImplementedException();
    //    }


    //    public Task SetPasswordHashAsync(IdentityUser user, string passwordHash)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task<string> GetPasswordHashAsync(IdentityUser user)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task<bool> HasPasswordAsync(IdentityUser user)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task SetSecurityStampAsync(IdentityUser user, string stamp)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task<string> GetSecurityStampAsync(IdentityUser user)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
