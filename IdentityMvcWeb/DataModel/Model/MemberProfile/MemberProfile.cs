using Kloud21.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

 namespace ActorDirector.DataModel
{
  public class MemberProfile : Entity
    {
        
        public string FirstName { get; set; }
        public string LastName { get; set; }     
        public DateTime DOB { get; set; }
        public string HighestEducation { get; set; }     
        public string HomeDistrict { get; set; }
        public int Experience { get; set; }
        public string MobilePhone { get; set; }
        public long FK_FROM_IdentityUser { get; set; }
    }
}
