using Entities.Model.All;
using Entities.Model.User;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Model.AnualyPlan
{
    public class AnualyPlan
    {
        public int anualy_id { get; set; }
        public LocomativeInformation locomative_name { get; set; }
        public int sections_reprair_number { get; set; }
        public string reprairtype { get; set; }
        public ReprairType reprair_type { get; set; }
        public DateTime information_confirmed_date { get; set; }
        public DateTime information_entered_date { get; set; }
        public DateTime information_modified_date { get; set; }
        public double all_price { get; set; }
        public MonthPlan month_plan { get; set; }
        public StatusEnum status { get; set; }
        public int data_log { get; set; }
        public DateTime plan_year { get; set; }
        public int a_o_id { get; set; }

        public AnualyPlan()
        {

        }

        public AnualyPlan(DataRow row)
        {
            anualy_id = int.Parse("" + row["anualy_id"]); 
            locomative_name = new LocomativeInformation
            {
                name = row["name"].ToString(),
                fuel_type=(FuelType)(Convert.ToInt32(row["fuel_type_id"])),
                loco_id = int.Parse("" + row["loco_id"]), 
            };
            sections_reprair_number = int.Parse("" + row["sections_repraer_number"]);
            reprairtype = row["type"].ToString(); 
            information_confirmed_date = Convert.ToDateTime(row["information_confirmed_date"]);
            information_entered_date = Convert.ToDateTime(row["information_entered_date"]);
            try
            {
                information_modified_date = Convert.ToDateTime(row["information_modified_date"]);
            }
            catch (Exception)
            {

            }
            status = (StatusEnum)(Convert.ToInt32(row["status_id"]));
            data_log = Convert.ToInt32(row["data_log_id"]);
            plan_year = Convert.ToDateTime(row["plan_year"]);
            try
            {
                all_price = Convert.ToDouble(row["all_price"]);
            }
            catch (Exception)
            {     }


        }
        public AnualyPlan(DataRow row, int a)
        {

            locomative_name = new LocomativeInformation
            {
                loco_id = Convert.ToInt32(row["loco_id"]),
                fuel_type = (FuelType)(Convert.ToInt32(row["fuel_type_id"])),
                name = row["name"].ToString(),
            };
            sections_reprair_number = int.Parse("" + row["sections_repraer_number"]);
            a_o_id = int.Parse("" + row["a_o_id"]);
            anualy_id = int.Parse("" + row["anualy_id"]);
            reprairtype = row["type"].ToString(); 
            information_confirmed_date = Convert.ToDateTime(row["information_confirmed_date"]);
            month_plan = new MonthPlan
            {
                Yanvar = Convert.ToInt32(row["yanvar"]),
                Fevral = Convert.ToInt32(row["fevral"]),
                Mart = Convert.ToInt32(row["mart"]),
                Aprel = Convert.ToInt32(row["aprel"]),
                May = Convert.ToInt32(row["may"]),
                Iyun = Convert.ToInt32(row["iyun"]),
                Iyul = Convert.ToInt32(row["iyul"]),
                Avgust = Convert.ToInt32(row["avgust"]),
                Sentyabr = Convert.ToInt32(row["sentyabr"]),
                Oktyabr = Convert.ToInt32(row["oktyabr"]),
                Noyabr = Convert.ToInt32(row["noyabr"]),
                Dekabr = Convert.ToInt32(row["dekabr"])
            };
        }

        public AnualyPlan(DataRow row, string a)
        {
            a_o_id = Convert.ToInt32(row["a_o_id"]);
            locomative_name = new LocomativeInformation
            {
                loco_id = Convert.ToInt32(row["loco_id"]),
                name = row["name"].ToString(),
                fuel_type = (FuelType)(Convert.ToInt32(row["fuel_type_id"])),
            };
            sections_reprair_number = int.Parse("" + row["sections_repraer_number"]);
            reprairtype = row["type"].ToString(); 

        }
    }
}
