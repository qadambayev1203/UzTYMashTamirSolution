using Entities.Model.All;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Model.User
{
    public class User
    {
        [Column("userid")]
        public int id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public DateTime birthday { get; set; }
        public string pinfl { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public RoleEnum role { get; set; }
        public OrganizationTypeEnum organizationtype { get; set; }
        public StatusEnum status { get; set; }
        public int datalog { get; set; }
        public string token { get; set; }


        public User()
        {

        }
        public User(DataRow row)
        {
            id = int.Parse("" + row["userid"]);
            firstname = row["firstname"].ToString();
            lastname = row["lastname"].ToString();
            birthday = Convert.ToDateTime(row["birthday"]);
            pinfl = row["pinfl"].ToString();
            login = row["login"].ToString();
            password = row["password"].ToString();
            role = (RoleEnum)row["role"];
            organizationtype = (OrganizationTypeEnum)row["organizationtype"]; 
            status = (StatusEnum)row["status"];
            datalog = (int)row["datalog"];
        }
    }
}
