using Entities.Model.All;
using Entities.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO.User.UserCrud
{
    public class UserCreateDTO
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public DateTime birthday { get; set; }
        public string pinfl { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public RoleEnum role { get; set; }
        public OrganizationTypeEnum organizationtype { get; set; }


    }
}
