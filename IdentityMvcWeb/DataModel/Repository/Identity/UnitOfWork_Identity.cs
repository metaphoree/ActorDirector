using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kloud21.DataModel.Identity;
using Kloud21.ADODAL;

namespace DataModel.Repository.Identity
{
   public  class UnitOfWork_Identity
    {

        IConnectionStringProvider con = new ConfigurationConnectionStringProvider("DefaultConnection");
        IAdoNetProvider prov = new SqlServerAdoNetProvider();
        IDbSession dbSession;

        public UnitOfWork_Identity()
        {
            dbSession = new DbSession(con, prov);
        }


        IdentityRepository identityRepository = null;
        RoleStoreRepository roleStoreRepository = null;
        UserClaimRepository userClaimRepository = null;
        UserLoginRepository userLoginRepository = null;
        UserRoleRepository userRoleRepository = null;



        public IdentityRepository IdentityRepository
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
        public RoleStoreRepository RoleStoreRepository
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
        public UserClaimRepository UserClaimRepository
        {
            get
            {
                if (userClaimRepository == null)
                {
                    userClaimRepository = new UserClaimRepository(dbSession);
                }
                return userClaimRepository;
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
        public UserRoleRepository UserRoleRepository
        {
            get
            {
                if (userRoleRepository == null)
                {
                    userRoleRepository = new UserRoleRepository(dbSession);
                }
                return userRoleRepository;
            }
        }














































    }
}
