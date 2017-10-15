using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel.UnitOfWork;
using AutoMapper;
using BusinessEntities;
using ActorDirector.DataModel;
namespace BusinessServices
{
  public  class MemberProfileService
    {

        UnitOfWork uow;

        public MemberProfileService()
        {
            uow = new UnitOfWork();
        }

        public MemberProfileService(UnitOfWork _uow)
        {
            this.uow = _uow;
        }

        //Added by vishal 16-Oct-2016
        #region Member
        public void AddMember(MemberProfileDTO addressDto)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<MemberProfileDTO, MemberProfile>());

            uow.MemberProfileRepo.Add(Mapper.Map<MemberProfileDTO, MemberProfile>(addressDto));

        }
        public void UpdateMember(MemberProfileDTO addressDto)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<MemberProfileDTO, MemberProfile>());

            uow.MemberProfileRepo.Update(Mapper.Map<MemberProfileDTO, MemberProfile>(addressDto));

        }
        public MemberProfileDTO FindByIdMember(Int64 Id)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<MemberProfile, MemberProfileDTO>());

            return Mapper.Map<MemberProfile, MemberProfileDTO>(uow.MemberProfileRepo.FindBy(Id));
        }
    }
}
#endregion