using Entities.Model.All;
using Entities.Model.QuarterPlan;
using Entities.Model.User;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Model.MonthPlanModel
{
    public class MonthPlanOne
    {
        public int month_id { get; set; }
        public int quarter_id { get; set; }
        public QuarterPlanReaded quarterPlanTwo { get; set; }
        public Designations designations { get; set; }
        public int designations_id { get; set; }
        public int section_true { get; set; }
        public StatusEnum status { get; set; }
        public int data_log { get; set; }


        public MonthPlanOne()
        {

        }
        public MonthPlanOne(DataRow row)
        {
            quarterPlanTwo = new QuarterPlanReaded
            {
                locomative_name = new LocomativeInformation
                {
                    loco_id = int.Parse("" + row["loco_id"]),
                    fuel_type = 0,
                    name = "" + row["name_loco"],
                },
                locomative_number = "" + row["locomative_number"],
                reprair_type = "" + row["type"],
                monthofreprair = "",
                section_num = int.Parse("" + row["section_num"]),
                locomativeFuelType = "" + row["fuel_type"],
                quarter = 0,
                plan_year = DateTime.Now,
            };
        }

        public MonthPlanOne(DataRow row, int a)
        {
            month_id = int.Parse("" + row["month_id"]);
            quarter_id = int.Parse("" + row["quarter_id"]);
            quarterPlanTwo = new QuarterPlanReaded
            {
                locomative_name = new LocomativeInformation
                {
                    loco_id = int.Parse("" + row["loco_id"]),
                    fuel_type = 0,
                    name = "" + row["name_loco"],
                },
                locomative_number = "" + row["locomative_number"],
                reprair_type = "" + row["type"],
                monthofreprair = "",
                section_num = 0,
                locomativeFuelType = "" + row["fuel_type"],
                quarter = 0,
                plan_year = DateTime.Now,
            };
            try
            {
                designations = new Designations
                {
                    designations_id = 0,
                    work_done = "" + row["work_done"],
                    unit_of_measure = "" + row["unit_of_measure"],
                    organization = new OrganizationType
                    {
                        org_id = int.Parse("" + row["org_id"]),
                        type = "" + row["name"]
                    }

                };
                section_true = int.Parse("" + row["section_true"]);
            }
            catch 
            {           
            }
        }

    }
}
