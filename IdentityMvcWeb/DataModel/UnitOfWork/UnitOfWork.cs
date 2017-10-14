using Kloud21.ADODAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kloud21.DataModel.Identity;
using DataModel.Repository.DomainRepository.Member;

namespace DataModel.UnitOfWork
{
    public class UnitOfWork : IDisposable
    {
        IConnectionStringProvider con = new ConfigurationConnectionStringProvider("DefaultConnection");
        IAdoNetProvider prov = new SqlServerAdoNetProvider();
        IDbSession dbSession;

        public UnitOfWork()
        {
            dbSession = new DbSession(con, prov);
        }


        MemberProfileRepository memberProfileRepository = null;
        public MemberProfileRepository MemberProfileRepo
        {
            get
            {
                if (memberProfileRepository == null)
                {
                    memberProfileRepository = new MemberProfileRepository(dbSession);
                }
                return memberProfileRepository;
            }
        }





        //UserRoleRepository userRoleRepository = null;
        //public UserRoleRepository UserRoleRepository
        //{
        //    get
        //    {
        //        if (userRoleRepository == null)
        //        {
        //            userRoleRepository = new UserRoleRepository(dbSession);
        //        }
        //        return userRoleRepository;
        //    }
        //}
            #region(Old Code)
            //Member Repository

            //MemberRepository memberRepository = null;
            //MemberAddressRepository memberAddressRepository = null;
            //SpouseRepository spouseRepository = null;
            //KidRepository kidRepository = null;

            ////Address Repository added by vishal 18 Oct 2016
            //AddressRepository addressRepository = null;
            //CityRepository cityRepository = null;
            //StateRepository stateRepository = null;
            //CountryRepository countryRepository = null;



            //// Lookup Repository

            //LookupTableListRepository lookupTableListRepository = null;
            //LookupValueListRepository lookupValueListRepository = null;



            ////Document
            //DocumentRepository documentRepository = null;

            ////System
            //SystemLogRepository systemLogRepository = null;

            //// Add all the repositroy getters here

            //#region Members
            //public MemberRepository MEMBERRepository
            //{
            //    get
            //    {
            //        if (memberRepository == null)
            //        {
            //            memberRepository = new MemberRepository(dbSession);
            //        }
            //        return memberRepository;
            //    }
            //}
            //public MemberAddressRepository MEMBERAddressRepository
            //{
            //    get
            //    {
            //        if (memberAddressRepository == null)
            //        {
            //            memberAddressRepository = new MemberAddressRepository(dbSession);
            //        }
            //        return memberAddressRepository;
            //    }
            //}
            //public SpouseRepository SPOUSERepository
            //{
            //    get
            //    {
            //        if (spouseRepository == null)
            //        {
            //            spouseRepository = new SpouseRepository(dbSession);
            //        }
            //        return spouseRepository;
            //    }
            //}
            //public KidRepository KIDRepository
            //{
            //    get
            //    {
            //        if (kidRepository == null)
            //        {
            //            kidRepository = new KidRepository(dbSession);
            //        }
            //        return kidRepository;
            //    }
            //}
            //#endregion
            //public CityRepository CITYRepository
            //{
            //    get
            //    {
            //        if (cityRepository == null)
            //        {
            //            cityRepository = new CityRepository(dbSession);
            //        }
            //        return cityRepository;
            //    }
            //}

            ////added by vishal on 22 Oct 2016
            //public StateRepository STATERepository
            //{
            //    get
            //    {
            //        if (stateRepository == null)
            //        {
            //            stateRepository = new StateRepository(dbSession);
            //        }
            //        return stateRepository;
            //    }
            //}
            //public CountryRepository COUNTRYRepository
            //{
            //    get
            //    {
            //        if (countryRepository == null)
            //        {
            //            countryRepository = new CountryRepository(dbSession);
            //        }
            //        return countryRepository;
            //    }
            //}


            ////Added by vishal Y 10-Oct-2016
            //#region Address
            //public AddressRepository ADDRESSRepository
            //{
            //    get
            //    {
            //        if (addressRepository == null)
            //        {
            //            addressRepository = new AddressRepository(dbSession);
            //        }
            //        return addressRepository;
            //    }
            //}
            //#endregion



            //#region Lookup


            //public LookupTableListRepository LOOKUPTABLELISTRepository
            //{
            //    get
            //    {
            //        if (lookupTableListRepository == null)
            //        {
            //            lookupTableListRepository = new LookupTableListRepository(dbSession);
            //        }
            //        return lookupTableListRepository;
            //    }
            //}
            //public LookupValueListRepository LOOKUPVALUELISTRepository
            //{
            //    get
            //    {
            //        if (lookupValueListRepository == null)
            //        {
            //            lookupValueListRepository = new LookupValueListRepository(dbSession);
            //        }
            //        return lookupValueListRepository;
            //    }
            //}




            //#endregion Lookup

            //#region Document
            //public DocumentRepository GetDocumentRepository
            //{
            //    get
            //    {
            //        if (documentRepository == null)
            //        {
            //            documentRepository = new DocumentRepository(dbSession);
            //        }
            //        return documentRepository;
            //    }
            //}
            //#endregion Document End

            //#region System
            //public SystemLogRepository GetSystemLogRepository
            //{
            //    get
            //    {
            //        if (systemLogRepository == null)
            //        {
            //            systemLogRepository = new SystemLogRepository(dbSession);
            //        }
            //        return systemLogRepository;
            //    }
            //}
            //#endregion System End

            //private bool disposed = false;
            #endregion


        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    dbSession.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }


}

