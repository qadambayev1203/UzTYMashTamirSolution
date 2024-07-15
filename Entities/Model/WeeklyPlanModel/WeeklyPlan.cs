using Entities.Model.All;
using Entities.Model.User;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Model.WeeklyPlanModel
{
    public class WeeklyPlan
    {
        public int weekly_id { get; set; }
        public int w_w_id { get; set; }
        public WeeklyWork weekly_work { get; set; }
        public int loco_id { get; set; }
        public LocomativeInformation loco_inf { get; set; }
        public string fuel_type { get; set; }
        public string locomative_number { get; set; }
        public int number_of_locomotives { get; set; }
        public StatusEnum status_id { get; set; }
        public int data_log_id { get; set; }
        public DateTime week_date { get; set; }

        public WeeklyPlan()
        {

        }

        public WeeklyPlan(DataRow row)
        {
            weekly_id = int.Parse("" + row["weekly_id"]);
            weekly_work = new WeeklyWork
            {
                w_w_id = int.Parse("" + row["w_w_id"]),
                w_work = ("" + row["w_work"]).ToString(),
            };
            loco_inf = new LocomativeInformation {
                loco_id = int.Parse("" + row["loco_id"]),
                name = ("" + row["name"]).ToString(),
                fuel_type = (FuelType)(int.Parse("" + row["fuel_type_id"])),
            };
            fuel_type = ("" + row["type"]).ToString();
            locomative_number = ("" + row["locomative_number"]).ToString();
            number_of_locomotives = int.Parse("" + row["number_of_locomotives"]);
            week_date = Convert.ToDateTime("" + row["week_date"]);
        }

    }
}
