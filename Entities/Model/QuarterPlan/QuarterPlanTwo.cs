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
    public class QuarterPlanTwo
    {
        public int q_pt_id { get; set; }
        public QuarterPlanReaded quarter_plan { get; set; }
        public int quarter_id { get; set; }
        public int section_1 { get; set; }
        public int section_2 { get; set; }
        public int section_3 { get; set; }
        public int section_4 { get; set; }
        public DateTime information_confirmed_date { get; set; }
        public DateTime information_entered_date { get; set; }
        public DateTime information_modified_date { get; set; }
        public StatusEnum status { get; set; }
        public int data_log { get; set; }

        public QuarterPlanTwo()
        {

        }

        public QuarterPlanTwo(DataRow row)
        {
            q_pt_id = int.Parse("" + row["q_pt_id"]);
            quarter_id = int.Parse("" + row["quarter_id"]);
            quarter_plan = new QuarterPlanReaded
            {
                locomative_name = new LocomativeInformation
                {
                    loco_id = int.Parse("" + row["loco_id"]),
                    name = row["name"].ToString(),
                    fuel_type = 0,
                },
                locomativeFuelType = ("" + row["fuel_type"]),
                locomative_number = ("" + row["locomative_number"]),
                reprair_type = (row["reprair_type"]).ToString(),
                monthofreprair = (row["month"] + ""),
                section_num = int.Parse("" + row["section_num"]),
                quarter = (Convert.ToInt32(row["quarter"])),
                plan_year = Convert.ToDateTime(row["plan_year"])
            };
            information_confirmed_date = Convert.ToDateTime(row["information_confirmed_date"]);
            section_1 = int.Parse("" + row["section_1"]);
            section_2 = int.Parse("" + row["section_2"]);
            section_3 = int.Parse("" + row["section_3"]);
            section_4 = int.Parse("" + row["section_4"]);
        }
    }
}
