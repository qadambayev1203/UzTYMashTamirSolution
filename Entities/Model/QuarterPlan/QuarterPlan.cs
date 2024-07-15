using Entities.Model.All;
using Entities.Model.User;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Model.QuarterPlan
{
    public class QuarterPlan
    {
        public int quarter_id { get; set; }
        public LocomativeInformation locomative_name { get; set; }
        public string locomative_number { get; set; }
        public OrganizationType organization { get; set; }
        public ReprairType reprair_id { get; set; }
        public string reprair_type { get; set; }
        public int month_of_reprair { get; set; }
        public string monthofreprair { get; set; }
        public int section_num { get; set; }
        public string locomativeFuelType { get; set; }
        public DateTime information_confirmed_date { get; set; }
        public DateTime information_entered_date { get; set; }
        public DateTime information_modified_date { get; set; }
        public int quarter { get; set; }
        public StatusEnum status { get; set; }
        public StatusEnum month_status { get; set; }
        public int data_log { get; set; }
        public DateTime plan_year { get; set; }
        public int section_0 { get; set; }

        public QuarterPlan()
        {

        }

        public QuarterPlan(DataRow row)
        {
            quarter_id = int.Parse("" + row["quarter_id"]);
            locomative_name = new LocomativeInformation
            {
                loco_id = int.Parse("" + row["loco_id"]),
                name = row["name_loco"].ToString(),
                fuel_type = 0,
            };
            locomativeFuelType = ("" + row["fuel_type"]);
            locomative_number = ("" + row["locomative_number"]);
            organization = new OrganizationType {
                org_id = 0,              
                type = row["name"].ToString(),
            };
            reprair_type = (row["type"]).ToString() ;
            monthofreprair = (row["month"]+"");
            month_of_reprair = int.Parse("" + row["month_of_reprair"]);
            section_num = int.Parse("" + row["section_num"]);
            information_confirmed_date = Convert.ToDateTime(row["information_confirmed_date"]);
            quarter = (Convert.ToInt32(row["quarter"]));
            plan_year = Convert.ToDateTime(row["plan_year"]);
            section_0 = int.Parse("" + row["section_0"]);
            status = (StatusEnum)(int.Parse("" + row["status_id"]));
            try
            {
                month_status = (StatusEnum)(int.Parse("" + row["month_status_id"]));
            }
            catch (Exception)
            {
                month_status = 0;
            }

        }
    }
}
