﻿using Kloud21.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActorDirector.DataModel;
using Kloud21.ADODAL;

namespace DataModel.Repository.DomainRepository.Member
{
   public class MemberProfileRepository : DbRepository<long, MemberProfile>
    {

        public MemberProfileRepository(IDbSession dbSession)
            : base(dbSession)
        {
           // SchemaName = "member";
        }


    }
}
