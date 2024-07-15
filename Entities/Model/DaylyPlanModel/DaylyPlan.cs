using Entities.Model.All;
using Entities.Model.User;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Model.DaylyPlanModel
{
    public class DaylyPlan
    {
        public int dayly_id { get; set; }
        public LocomativeInformation loco_inf { get; set; }
        public string fuel_type { get; set; }
        public int loco_id { get; set; }
        public string locomative_number { get; set; }
        public ReprairType reprair_id { get; set; }
        public string reprair_type { get; set; }
        public OrganizationType organization { get; set; }
        public int org_id { get; set; }
        public string do_work { get; set; }
        public string comment { get; set; }
        public StatusEnum status_id { get; set; }
        public int data_log_id { get; set; }
        public DateTime day_date { get; set; }

        public DaylyPlan()
        {

        }

        public DaylyPlan(DataRow row)
        {
            dayly_id = int.Parse("" + row["dayly_id"]);
            loco_inf = new LocomativeInformation    
            {
                loco_id = int.Parse("" + row["loco_id"]),
                name = ("" + row["name"]).ToString(),
                fuel_type = (FuelType)(int.Parse("" + row["fuel_type_id"])),
            };
            fuel_type = ("" + row["fuel_type"]).ToString();
            locomative_number = ("" + row["locomative_number"]).ToString();
            reprair_type = ("" + row["type"]);
            organization = new OrganizationType
            {
                org_id= int.Parse("" + row["org_id"]),
                type= "" + row["org_name"]
            };
            do_work = "" + row["do_work"];
            comment = "" + row["comment"];
            day_date = Convert.ToDateTime("" + row["day_date"]);
        }
    }

   
}
