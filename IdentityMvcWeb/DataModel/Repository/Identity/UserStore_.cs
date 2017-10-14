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
    public class UserStore_ : IUserStore<IdentityUser, long>
    {
        private bool _disposed;
        IConnectionStringProvider con = new ConfigurationConnectionStringProvider("DefaultConnection");
        IAdoNetProvider prov = new SqlServerAdoNetProvider();
        IDbSession dbSession;
        IdentityRepository identityRepository = null;

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

        public UserStore_()
        {
            dbSession = new DbSession(con, prov);
        }

        private void ThrowIfDisposed()
        {
            if (this._disposed)
                throw new ObjectDisposedException(this.GetType().Name);
        }

        public Task CreateAsync(IdentityUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            IdentityUserRepository.Add(user);

            return Task.FromResult<object>(null);
        }

        public Task DeleteAsync(IdentityUser user)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            this._disposed = true;
        }

        public Task<IdentityUser> FindByIdAsync(long userId)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityUser> FindByNameAsync(string userName)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(IdentityUser user)
        {
            throw new NotImplementedException();
        }
    }
}
