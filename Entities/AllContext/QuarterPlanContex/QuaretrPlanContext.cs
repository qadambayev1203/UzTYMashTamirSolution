using Entities.Model.All;
using Entities.Model.QuarterPlan;
using Entities.Model.User;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.AllContext.QuarterPlanContex
{
    public class QuaretrPlanContext : DatabaseConnection
    {
        public QuaretrPlanContext()
        {

        }

        public string CreateQuarterPlan(QuarterPlan quarterPlan, int loginiduser)
        {
            if (quarterPlan != null)
            {

                quarterPlan.status = StatusEnum.active;
                quarterPlan.locomative_name.loco_id = locomativeGetId(quarterPlan.locomative_name.name);
                quarterPlan.locomative_name.fuel_type = (FuelType)locomativeGettypeId(quarterPlan.locomative_name.loco_id);


                try
                {
                    string query1 = "INSERT INTO public.quarter_plan(" +
                        "quarter_id, loco_id, locomative_number, organization_id, reprair_id, month_of_reprair, " +
                        "section_num, information_confirmed_date," +
                        "quarter, pan_year, section_0)" +
                        $"VALUES(DEFAULT,{quarterPlan.locomative_name.loco_id},'{quarterPlan.locomative_number}'," +
                        $"'{quarterPlan.organization.org_id}',{quarterPlan.month_of_reprair},{quarterPlan.section_num}," +
                        $"'{quarterPlan.information_confirmed_date}','{quarterPlan.quarter}','{quarterPlan.plan_year}'," +
                        $"'{quarterPlan.section_0}'); ";
                    quarterPlan.data_log = logWriting(loginiduser, query1);



                    string query = "INSERT INTO public.quarter_plan(" +
                        "quarter_id, loco_id, locomative_number, organization_id, reprair_id, month_of_reprair, " +
                        "section_num, information_confirmed_date, information_entered_date, " +
                        "quarter, status_id, data_log_id, plan_year, section_0)" +
                        " VALUES(DEFAULT, @loco_id, @locomative_number, @organization_id, @reprair_id," +
                        " @month_of_reprair," +
                        "@section_num, @information_confirmed_date, @information_entered_date," +
                        "@quarter, @status_id, @data_log_id, @plan_year, @section_0); ";
                    conn.Open();
                    using (NpgsqlCommand cmd = new(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@loco_id", quarterPlan.locomative_name.loco_id);
                        cmd.Parameters.AddWithValue("@locomative_number", quarterPlan.locomative_number);
                        cmd.Parameters.AddWithValue("@organization_id", quarterPlan.organization.org_id);
                        cmd.Parameters.AddWithValue("@reprair_id", (int)quarterPlan.reprair_id);
                        cmd.Parameters.AddWithValue("@month_of_reprair", quarterPlan.month_of_reprair);
                        cmd.Parameters.AddWithValue("@section_num", quarterPlan.section_num);
                        cmd.Parameters.AddWithValue("@information_confirmed_date", NpgsqlDbType.Date, quarterPlan.information_confirmed_date);
                        cmd.Parameters.AddWithValue("@information_entered_date", NpgsqlDbType.Date, DateTime.Now);
                        cmd.Parameters.AddWithValue("@quarter", quarterPlan.quarter);
                        cmd.Parameters.AddWithValue("@status_id", (int)quarterPlan.status);
                        cmd.Parameters.AddWithValue("@data_log_id", quarterPlan.data_log);
                        cmd.Parameters.AddWithValue("@plan_year", quarterPlan.plan_year);
                        cmd.Parameters.AddWithValue("@section_0", quarterPlan.section_0);
                        var a = cmd.ExecuteNonQuery();

                    }
                    return "Created";
                }
                catch (Exception ex) { return ex + ""; }
                finally
                {
                    conn.Close();
                }
            }
            return "";
        }

        public void DeleteQuarterPlan(int id, int loginiduser)
        {
            try
            {
                int datalog;
                string query = $"UPDATE quarter_plan SET status_id=@status_id, data_log_id=@data_log_id WHERE quarter_id=@quarter_id;";


                string query1 = $"UPDATE quarter_plan SET status={StatusEnum.deleted} WHERE quarter_id={id};";
                datalog = logWriting(loginiduser, query);

                conn.Open();

                using (NpgsqlCommand cmd = new(query, conn))
                {
                    cmd.Parameters.AddWithValue("@quarter_id", id);
                    cmd.Parameters.AddWithValue("@status_id", (int)StatusEnum.deleted);
                    cmd.Parameters.AddWithValue("@data_log_id", datalog);
                    var a = cmd.ExecuteNonQuery();
                }
            }
            catch { }
            finally { conn.Close(); }
        }

        public IEnumerable<QuarterPlan> GetAllYearQuarterPlan(int year, int quarter, int queryNum)
        {
            List<QuarterPlan> quarterPlans = new();
            DataTable table = null;
            try
            {
                string limit = queryNum != 0 ? " LIMIT @queryNum;" : ";";
                string query = "SELECT quarter_plan.quarter_id, locomative_information.loco_id,locomative_information.name AS name_loco," +
                    "fuel_type.type AS fuel_type," +
                    "quarter_plan.locomative_number, organization.name, reprair_type.type, " +
                    "month.month, quarter_plan.section_num, quarter_plan.information_confirmed_date," +
                    "quarter_plan.quarter, quarter_plan.plan_year, quarter_plan.section_0,quarter_plan.status_id,quarter_plan.month_status_id,quarter_plan.month_of_reprair" +
                    " FROM locomative_information" +
                    " INNER JOIN quarter_plan ON quarter_plan.loco_id = locomative_information.loco_id" +
                    " INNER JOIN fuel_type ON fuel_type.fuel_type_id = locomative_information.fuel_type_id" +
                    " INNER JOIN organization ON organization.org_id = quarter_plan.organization_id" +
                    " INNER JOIN reprair_type ON reprair_type.reprair_id = quarter_plan.reprair_id" +
                    " INNER JOIN public.month ON month.month_id=quarter_plan.month_of_reprair" +
                    " WHERE quarter_plan.status_id != 2 AND quarter_plan.quarter=@quarter AND EXTRACT(YEAR FROM quarter_plan.plan_year) = @year" + limit;



                conn.Open();

                using (NpgsqlCommand cmd = new(query, conn))
                {
                    cmd.Parameters.AddWithValue("@year", year);
                    cmd.Parameters.AddWithValue("@queryNum", queryNum);
                    cmd.Parameters.AddWithValue("@quarter", quarter);
                    using (NpgsqlDataAdapter da = new(cmd))
                    {
                        table = new DataTable();
                        da.Fill(table);
                    }
                }
            }
            catch { }
            finally { conn.Close(); }

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    QuarterPlan quarterPlan = new(row);
                    quarterPlans.Add(quarterPlan);
                }
            }
            return quarterPlans;
        }

        public QuarterPlan GetQuarterPlanByID(int id)
        {
            DataTable table = null;
            QuarterPlan quarterPlan = null;

            string query = "SELECT quarter_plan.quarter_id, locomative_information.loco_id,locomative_information.name AS name_loco," +
                "fuel_type.type AS fuel_type," +
                "quarter_plan.locomative_number, organization.name, reprair_type.type, " +
                "month.month, quarter_plan.section_num, quarter_plan.information_confirmed_date," +
                "quarter_plan.quarter, quarter_plan.plan_year, quarter_plan.section_0,quarter_plan.status_id,quarter_plan.month_status_id,quarter_plan.month_of_reprair" +
                " FROM locomative_information" +
                " INNER JOIN quarter_plan ON quarter_plan.loco_id = locomative_information.loco_id" +
                " INNER JOIN fuel_type ON fuel_type.fuel_type_id = locomative_information.fuel_type_id" +
                " INNER JOIN organization ON organization.org_id = quarter_plan.organization_id" +
                " INNER JOIN reprair_type ON reprair_type.reprair_id = quarter_plan.reprair_id" +
                " INNER JOIN public.month ON month.month_id=quarter_plan.month_of_reprair" +
                " WHERE quarter_plan.status_id != 2 AND quarter_plan.quarter_id=@quarter_id;";
            try
            {
                conn.Open();

                using (NpgsqlCommand cmd = new(query, conn))
                {
                    cmd.Parameters.AddWithValue("@quarter_id", id);
                    using (NpgsqlDataAdapter da = new(cmd))
                    {
                        table = new DataTable();
                        da.Fill(table);
                    }
                }
            }
            catch { }
            finally { conn.Close(); }

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    quarterPlan = new(row);
                }
            }

            return quarterPlan;
        }

        public void UpdateQuarterPlan(int id, QuarterPlan quarterPlan, int loginiduser)
        {
            if (quarterPlan != null)
            {

                quarterPlan.status = StatusEnum.updated;
                quarterPlan.locomative_name.loco_id = locomativeGetId(quarterPlan.locomative_name.name);
                quarterPlan.locomative_name.fuel_type = (FuelType)locomativeGettypeId(quarterPlan.locomative_name.loco_id);

                try
                {
                    string query1 = $"UPDATE public.quarter_plan" +
                        $"SET loco_id ={quarterPlan.locomative_name.loco_id}, locomative_number ={quarterPlan.locomative_number}, " +
                        $"organization_id ={quarterPlan.organization.org_id}," +
                        $"reprair_id ={quarterPlan.reprair_id}, month_of_reprair ={quarterPlan.month_of_reprair}," +
                        $" section_num ={quarterPlan.section_num}, information_confirmed_date ={quarterPlan.information_confirmed_date}," +
                        $"quarter ={quarterPlan.quarter}, plan_year ={quarterPlan.plan_year}, section_0 ={quarterPlan.section_0}" +
                        $" WHERE quarter_id = '{id}'; ";
                    quarterPlan.data_log = logWriting(loginiduser, query1);



                    string query = "UPDATE public.quarter_plan" +
                        " SET loco_id = @loco_id, locomative_number = @locomative_number, organization_id = @organization_id," +
                        "reprair_id = @reprair_id, month_of_reprair = @month_of_reprair, section_num = @section_num," +
                        "information_confirmed_date = @information_confirmed_date," +
                        "information_modifaed_date = @information_modifaed_date, quarter = @quarter," +
                        "status_id = @status_id, data_log_id = @data_log_id, plan_year = @plan_year, section_0 = @section_0" +
                        " WHERE quarter_id = @quarter_id; ";

                    conn.Open();
                    using (NpgsqlCommand cmd = new(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@loco_id", quarterPlan.locomative_name.loco_id);
                        cmd.Parameters.AddWithValue("@locomative_number", quarterPlan.locomative_number);
                        cmd.Parameters.AddWithValue("@organization_id", quarterPlan.organization.org_id);
                        cmd.Parameters.AddWithValue("@reprair_id", (int)quarterPlan.reprair_id);
                        cmd.Parameters.AddWithValue("@month_of_reprair", quarterPlan.month_of_reprair);
                        cmd.Parameters.AddWithValue("@section_num", quarterPlan.section_num);
                        cmd.Parameters.AddWithValue("@information_confirmed_date", NpgsqlDbType.Date, quarterPlan.information_confirmed_date);
                        cmd.Parameters.AddWithValue("@information_modifaed_date", NpgsqlDbType.Date, DateTime.Now);
                        cmd.Parameters.AddWithValue("@quarter", quarterPlan.quarter);
                        cmd.Parameters.AddWithValue("@status_id", (int)quarterPlan.status);
                        cmd.Parameters.AddWithValue("@data_log_id", quarterPlan.data_log);
                        cmd.Parameters.AddWithValue("@plan_year", NpgsqlDbType.Date, quarterPlan.plan_year);
                        cmd.Parameters.AddWithValue("@section_0", quarterPlan.section_0);
                        cmd.Parameters.AddWithValue("@quarter_id", id);
                        var a = cmd.ExecuteNonQuery();

                    }
                }
                catch { }
                finally { conn.Close(); }
            }
        }

        public void UpdateQuarterPlanAdd(int id)
        {
            int status = (int)StatusEnum.addition;

            try
            {

                string query = "UPDATE quarter_plan" +
                    " SET status_id = @status_id" +
                    " WHERE quarter_id = @quarter_id; ";

                conn.Open();
                using (NpgsqlCommand cmd = new(query, conn))
                {

                    cmd.Parameters.AddWithValue("@status_id", status);

                    cmd.Parameters.AddWithValue("@quarter_id", id);
                    var a = cmd.ExecuteNonQuery();

                }
            }
            catch { }
            finally { conn.Close(); }
        }
        public void UpdateQuarterPlanMonthAdd(int id)
        {
            int status = (int)StatusEnum.addition;

            try
            {

                string query = "UPDATE quarter_plan" +
                    " SET month_status_id = @month_status_id" +
                    " WHERE quarter_id = @quarter_id; ";

                conn.Open();
                using (NpgsqlCommand cmd = new(query, conn))
                {

                    cmd.Parameters.AddWithValue("@month_status_id", status);

                    cmd.Parameters.AddWithValue("@quarter_id", id);
                    var a = cmd.ExecuteNonQuery();

                }
            }
            catch { }
            finally { conn.Close(); }
        }


        //Two



        public IEnumerable<QuarterPlanTwo> GetAllYearQuarterPlanTwo(int year, int quarter, int queryNum)
        {
            List<QuarterPlanTwo> quarterPlans = new();
            DataTable table = null;
            try
            {
                string limit = queryNum != 0 ? " LIMIT @queryNum;" : ";";
                string query = "SELECT locomative_information.loco_id,locomative_information.name,fuel_type.type AS fuel_type," +
                    "quarter_plan.locomative_number, quarter_plan.section_num, quarter_plan.quarter," +
                    "reprair_type.type AS reprair_type," +
                    "month.month," +
                    "quarter_plan_two.q_pt_id," +
                    "quarter_plan_two.section_1," +
                    "quarter_plan_two.section_2, quarter_plan_two.section_3, quarter_plan_two.section_4, " +
                    "quarter_plan_two.information_confirmed_date, quarter_plan.plan_year, quarter_plan_two.quarter_id" +
                    " FROM fuel_type" +
                    " INNER JOIN locomative_information ON locomative_information.fuel_type_id = fuel_type.fuel_type_id" +
                    " INNER JOIN quarter_plan ON quarter_plan.loco_id = locomative_information.loco_id" +
                    " INNER JOIN reprair_type ON reprair_type.reprair_id = quarter_plan.reprair_id" +
                    " INNER JOIN month ON month.month_id = quarter_plan.month_of_reprair" +
                    " INNER JOIN quarter_plan_two ON quarter_plan_two.quarter_id = quarter_plan.quarter_id" +
                    " WHERE quarter_plan_two.status_id != 2 AND quarter_plan.quarter = @quarter AND EXTRACT(YEAR FROM quarter_plan.plan_year) = @year" + limit;



                conn.Open();

                using (NpgsqlCommand cmd = new(query, conn))
                {
                    cmd.Parameters.AddWithValue("@year", year);
                    cmd.Parameters.AddWithValue("@queryNum", queryNum);
                    cmd.Parameters.AddWithValue("@quarter", quarter);
                    using (NpgsqlDataAdapter da = new(cmd))
                    {
                        table = new DataTable();
                        da.Fill(table);
                    }
                }
            }
            catch { }
            finally { conn.Close(); }

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    QuarterPlanTwo quarterPlan = new(row);
                    quarterPlans.Add(quarterPlan);
                }
            }
            return quarterPlans;
        }

        public QuarterPlanTwo GetQuarterPlanTwoByID(int id)
        {
            DataTable table = null;
            QuarterPlanTwo quarterPlan = null;

            string query = "SELECT locomative_information.loco_id,locomative_information.name,fuel_type.type AS fuel_type," +
                "quarter_plan.locomative_number, quarter_plan.section_num, quarter_plan.quarter," +
                "reprair_type.type AS reprair_type," +
                "month.month," +
                "quarter_plan_two.q_pt_id," +
                "quarter_plan_two.section_1," +
                "quarter_plan_two.section_2, quarter_plan_two.section_3, quarter_plan_two.section_4, " +
                "quarter_plan_two.information_confirmed_date, quarter_plan.plan_year, quarter_plan_two.quarter_id " +
                " FROM fuel_type" +
                " INNER JOIN locomative_information ON locomative_information.fuel_type_id = fuel_type.fuel_type_id" +
                " INNER JOIN quarter_plan ON quarter_plan.loco_id = locomative_information.loco_id" +
                " INNER JOIN reprair_type ON reprair_type.reprair_id = quarter_plan.reprair_id" +
                " INNER JOIN month ON month.month_id = quarter_plan.month_of_reprair" +
                " INNER JOIN quarter_plan_two ON quarter_plan_two.quarter_id = quarter_plan.quarter_id" +
                " WHERE quarter_plan_two.status_id != 2 AND quarter_plan_two.q_pt_id=@q_pt_id;";
            try
            {
                conn.Open();

                using (NpgsqlCommand cmd = new(query, conn))
                {
                    cmd.Parameters.AddWithValue("@q_pt_id", id);
                    using (NpgsqlDataAdapter da = new(cmd))
                    {
                        table = new DataTable();
                        da.Fill(table);
                    }
                }
            }
            catch { }
            finally { conn.Close(); }

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    quarterPlan = new(row);
                }
            }

            return quarterPlan;
        }

        public string CreateQuarterPlanTwo(QuarterPlanTwo quarterPlan, int loginiduser)
        {
            if (quarterPlan != null)
            {


                quarterPlan.status = StatusEnum.active;
                try
                {
                    string query1 = "INSERT INTO quarter_plan_two(" +
                        "q_pt_id, quarter_id, section_1, section_2, section_3, section_4, information_confirmed_date, )" +
                        $" VALUES(DEFAULT,'{quarterPlan.quarter_id}','{quarterPlan.section_1}','{quarterPlan.section_2}'," +
                        $"'{quarterPlan.section_3}','{quarterPlan.section_4}','{quarterPlan.information_confirmed_date}'); ";
                    quarterPlan.data_log = logWriting(loginiduser, query1);



                    string query = "INSERT INTO public.quarter_plan_two(" +
                        "q_pt_id, quarter_id, section_1, section_2, section_3, section_4, information_confirmed_date," +
                        "information_entered_date, status_id, data_log_id)" +
                        " VALUES(DEFAULT, @quarter_id, @section_1, @section_2, @section_3, @section_4, " +
                        "@information_confirmed_date," +
                        "@information_entered_date, @status_id, @data_log_id); ";
                    conn.Open();
                    using (NpgsqlCommand cmd = new(query, conn))
                    {

                        cmd.Parameters.AddWithValue("@quarter_id", quarterPlan.quarter_id);
                        cmd.Parameters.AddWithValue("@section_1", quarterPlan.section_1);
                        cmd.Parameters.AddWithValue("@section_2", quarterPlan.section_2);
                        cmd.Parameters.AddWithValue("@section_3", quarterPlan.section_3);
                        cmd.Parameters.AddWithValue("@section_4", quarterPlan.section_4);
                        cmd.Parameters.AddWithValue("@information_confirmed_date", NpgsqlDbType.Date, quarterPlan.information_confirmed_date);
                        cmd.Parameters.AddWithValue("@information_entered_date", NpgsqlDbType.Date, DateTime.Now);
                        cmd.Parameters.AddWithValue("@status_id", (int)quarterPlan.status);
                        cmd.Parameters.AddWithValue("@data_log_id", quarterPlan.data_log);

                        var a = cmd.ExecuteNonQuery();

                    }
                    return "Created";
                }
                catch (Exception ex) { return ex + ""; }
                finally
                {
                    conn.Close();
                }
            }
            return "";
        }

        public void DeleteQuarterPlanTwo(int id, int loginiduser)
        {
            try
            {
                int datalog;
                string query = $"UPDATE quarter_plan_two SET status_id=@status_id, data_log_id=@data_log_id WHERE q_pt_id=@q_pt_id;";


                string query1 = $"UPDATE quarter_plan_two SET status={StatusEnum.deleted} WHERE q_pt_id={id};";
                datalog = logWriting(loginiduser, query);

                conn.Open();

                using (NpgsqlCommand cmd = new(query, conn))
                {
                    cmd.Parameters.AddWithValue("@q_pt_id", id);
                    cmd.Parameters.AddWithValue("@status_id", (int)StatusEnum.deleted);
                    cmd.Parameters.AddWithValue("@data_log_id", datalog);
                    var a = cmd.ExecuteNonQuery();
                }
            }
            catch { }
            finally { conn.Close(); }
        }

        public void UpdateQuarterPlanTwo(int id, QuarterPlanTwo quarterPlan, int loginiduser)
        {
            if (quarterPlan != null)
            {

                quarterPlan.status = StatusEnum.updated;


                try
                {
                    string query1 = $"UPDATE quarter_plan_two" +
                        $" SET quarter_id ={quarterPlan.quarter_id}, section_1 ={quarterPlan.section_1}, " +
                        $"section_2 ={quarterPlan.section_2}, section_3 ={quarterPlan.section_3}," +
                        $"section_4 ={quarterPlan.section_4}," +
                        $" WHERE q_pt_id ={id}; ";
                    quarterPlan.data_log = logWriting(loginiduser, query1);



                    string query = "UPDATE quarter_plan_two" +
                        " SET quarter_id = @quarter_id, section_1 = @section_1," +
                        "section_2 = @section_2, section_3 = @section_3, section_4 = @section_4, " +
                        "information_confirmed_date = @information_confirmed_date, " +
                        "information_modifaid_date = @information_modifaid_date," +
                        "status_id = @status_id, data_log_id = @data_log_id" +
                        " WHERE q_pt_id = @q_pt_id ; ";

                    conn.Open();
                    using (NpgsqlCommand cmd = new(query, conn))
                    {

                        cmd.Parameters.AddWithValue("@quarter_id", quarterPlan.quarter_id);
                        cmd.Parameters.AddWithValue("@section_1", quarterPlan.section_1);
                        cmd.Parameters.AddWithValue("@section_2", quarterPlan.section_2);
                        cmd.Parameters.AddWithValue("@section_3", quarterPlan.section_3);
                        cmd.Parameters.AddWithValue("@section_4", quarterPlan.section_4);
                        cmd.Parameters.AddWithValue("@information_confirmed_date", NpgsqlDbType.Date, quarterPlan.information_confirmed_date);
                        cmd.Parameters.AddWithValue("@information_modifaid_date", NpgsqlDbType.Date, DateTime.Now);
                        cmd.Parameters.AddWithValue("@status_id", (int)quarterPlan.status);
                        cmd.Parameters.AddWithValue("@data_log_id", quarterPlan.data_log);
                        cmd.Parameters.AddWithValue("@q_pt_id", id);
                        var a = cmd.ExecuteNonQuery();

                    }
                }
                catch { }
                finally { conn.Close(); }
            }
        }
    }
}
