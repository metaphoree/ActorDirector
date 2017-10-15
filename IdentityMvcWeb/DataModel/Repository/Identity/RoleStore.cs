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
    public class RoleStore<TRole> : IQueryableRoleStore<TRole, long>
        where TRole : IdentityRole
    {
        private bool _disposed=false;
        IConnectionStringProvider con = new ConfigurationConnectionStringProvider("DefaultConnection");
        IAdoNetProvider prov = new SqlServerAdoNetProvider();
        IDbSession dbSession;
        RoleStoreRepository roleStoreRepository = null;

        public RoleStoreRepository RoleRepository
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

        public IQueryable<TRole> Roles
        {
            get
            {
                List<TRole> result = RoleRepository.All() as List<TRole>;
                return result.AsQueryable();
            }
        }

        public RoleStore()
        {
            dbSession = new DbSession(con, prov);
        }

        public Task CreateAsync(TRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }

            RoleRepository.Add(role);
            

            return Task.FromResult<object>(null);
        }

        public Task UpdateAsync(TRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("user");
            }

            RoleRepository.Update(role);

            return Task.FromResult<Object>(null);
        }

        public Task DeleteAsync(TRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("user");
            }

            RoleRepository.Delete(role);

            return Task.FromResult<Object>(null);
        }

        public Task<TRole> FindByIdAsync(long roleId)
        {
            TRole result = RoleRepository.FindBy(roleId) as TRole;

            return Task.FromResult<TRole>(result);
        }

        public Task<TRole> FindById(long roleId)
        {
            TRole result = RoleRepository.FindBy(roleId) as TRole;

            return Task.FromResult<TRole>(result);
        }

        public Task<TRole> FindByNameAsync(string roleName)
        {
            TRole result = RoleRepository.GetRoleByName(roleName) as TRole;

            return Task.FromResult<TRole>(result);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        private void ThrowIfDisposed()
        {
            if (this._disposed)
                throw new ObjectDisposedException(this.GetType().Name);
        }
    }
}
