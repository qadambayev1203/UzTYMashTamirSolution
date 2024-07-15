using Entities.Model.MonthPlanModel;
using Entities.Model.User;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.AllContext.MonthPlanContext
{
    public class MonthPlanContex : DatabaseConnection
    {
        public MonthPlanContex()
        {

        }

        //Month
        public IEnumerable<MonthPlanOne> GetAllYearMonthPlan(int year, int quarter, int month, int queryNum)
        {
            List<MonthPlanOne> monthPlans = new();
            DataTable table = null;

            try
            {
                string limit = queryNum != 0 ? " LIMIT @queryNum;" : ";";
                string query = "SELECT locomative_information.loco_id,locomative_information.name AS name_loco," +
                    "fuel_type.type AS fuel_type," +
                    "quarter_plan.locomative_number, reprair_type.type," +
                    "quarter_plan.section_num" +
                    " FROM locomative_information" +
                    " INNER JOIN quarter_plan ON quarter_plan.loco_id = locomative_information.loco_id" +
                    " INNER JOIN fuel_type ON fuel_type.fuel_type_id = locomative_information.fuel_type_id" +
                    " INNER JOIN reprair_type ON reprair_type.reprair_id = quarter_plan.reprair_id" +
                    " WHERE quarter_plan.status_id != 2 AND quarter_plan.quarter=@quarter AND quarter_plan.month_of_reprair=@month" +
                    " AND EXTRACT(YEAR FROM quarter_plan.plan_year) = @year" + limit;
                conn.Open();

                using (NpgsqlCommand cmd = new(query, conn))
                {
                    cmd.Parameters.AddWithValue("@year", year);
                    cmd.Parameters.AddWithValue("@queryNum", queryNum);
                    cmd.Parameters.AddWithValue("@quarter", quarter);
                    cmd.Parameters.AddWithValue("@month", month);
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
                    MonthPlanOne monthPlan = new(row);
                    monthPlans.Add(monthPlan);
                }
            }
            return monthPlans;
        }



        //Month One
        public string CreateMonthPlanOne(MonthPlanOne monthPlan, int loginiduser)
        {
            if (monthPlan != null)
            {

                monthPlan.status = StatusEnum.active;



                string query1 = "INSERT INTO month_plan(" +
                    "month_id, quarter_id)" +
                    $"VALUES(DEFAULT,{monthPlan.quarter_id}); ";
                monthPlan.data_log = logWriting(loginiduser, query1);

                try
                {

                    string query = "INSERT INTO month_plan(" +
                        "month_id, quarter_id,status_id, data_log_id)" +
                        " VALUES (DEFAULT, @quarter_id, @status_id, @data_log_id);";
                    conn.Open();
                    using (NpgsqlCommand cmd = new(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@quarter_id", monthPlan.quarter_id);
                        cmd.Parameters.AddWithValue("@status_id", (int)monthPlan.status);
                        cmd.Parameters.AddWithValue("@data_log_id", monthPlan.data_log);
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

        public void DeleteMonthPlanOne(int id, int loginiduser)
        {
            try
            {
                int datalog;
                string query = $"UPDATE month_plan SET status_id=@status_id, data_log_id=@data_log_id WHERE month_id=@month_id;";


                string query1 = $"UPDATE month_plan SET status={StatusEnum.deleted} WHERE month_id={id};";
                datalog = logWriting(loginiduser, query);

                conn.Open();

                using (NpgsqlCommand cmd = new(query, conn))
                {
                    cmd.Parameters.AddWithValue("@month_id", id);
                    cmd.Parameters.AddWithValue("@status_id", (int)StatusEnum.deleted);
                    cmd.Parameters.AddWithValue("@data_log_id", datalog);
                    var a = cmd.ExecuteNonQuery();
                }
            }
            catch { }
            finally { conn.Close(); }
        }

        public IEnumerable<MonthPlanOne> GetAllYearMonthPlanOne(int year, int quarter, int month, int queryNum)
        {
            List<MonthPlanOne> monthPlans = new();
            DataTable table = null;


            try
            {
                string query = "SELECT month_plan.month_id,quarter_plan.quarter_id," +
                "month_plan.section_true," +
                "locomative_information.loco_id,locomative_information.name AS name_loco," +
                "fuel_type.type AS fuel_type,quarter_plan.locomative_number," +
                "reprair_type.type FROM locomative_information" +
                " INNER JOIN quarter_plan ON quarter_plan.loco_id = locomative_information.loco_id" +
                " INNER JOIN fuel_type ON fuel_type.fuel_type_id = locomative_information.fuel_type_id" +
                " INNER JOIN reprair_type ON reprair_type.reprair_id = quarter_plan.reprair_id" +
                " INNER JOIN month_plan ON month_plan.quarter_id = quarter_plan.quarter_id" +
                " WHERE month_plan.status_id != 2 AND quarter_plan.quarter = @quarter AND quarter_plan.month_of_reprair = @month" +
                " AND EXTRACT(YEAR FROM quarter_plan.plan_year) = @year AND month_plan.designations_id IS NULL";
            conn.Open();

            using (NpgsqlCommand cmd = new(query, conn))
            {
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@quarter", quarter);
                cmd.Parameters.AddWithValue("@month", month);
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
                    MonthPlanOne monthPlan = new(row, 0);
                    monthPlans.Add(monthPlan);
                }
            }
            try
            {
                string limit = queryNum != 0 ? " LIMIT @queryNum;" : ";";
                string query1 = "SELECT month_plan.month_id,quarter_plan.quarter_id," +
                    "organization.org_id, organization.name,designations.work_done," +
                    "designations.unit_of_measure,month_plan.section_true," +
                    "locomative_information.loco_id,locomative_information.name AS name_loco," +
                    "fuel_type.type AS fuel_type,quarter_plan.locomative_number," +
                    "reprair_type.type FROM locomative_information" +
                    " INNER JOIN quarter_plan ON quarter_plan.loco_id = locomative_information.loco_id" +
                    " INNER JOIN fuel_type ON fuel_type.fuel_type_id = locomative_information.fuel_type_id" +
                    " INNER JOIN reprair_type ON reprair_type.reprair_id = quarter_plan.reprair_id" +
                    " INNER JOIN month_plan ON month_plan.quarter_id = quarter_plan.quarter_id" +
                    " INNER JOIN designations ON designations.designations_id = month_plan.designations_id" +
                    " INNER JOIN organization ON organization.org_id = designations.org_id" +
                    " WHERE month_plan.status_id != 2 AND quarter_plan.quarter=@quarter AND quarter_plan.month_of_reprair=@month" +
                    " AND EXTRACT(YEAR FROM quarter_plan.plan_year) = @year" + limit;
                conn.Open();

                using (NpgsqlCommand cmd = new(query1, conn))
                {
                    cmd.Parameters.AddWithValue("@year", year);
                    cmd.Parameters.AddWithValue("@queryNum", queryNum);
                    cmd.Parameters.AddWithValue("@quarter", quarter);
                    cmd.Parameters.AddWithValue("@month", month);
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
                    MonthPlanOne monthPlan = new(row, 0);
                    monthPlans.Add(monthPlan);
                }
            }

            return monthPlans;
        }

        public MonthPlanOne GetMonthPlanOneByID(int id)
        {
            DataTable table = null;
            MonthPlanOne monthPlan = null;

            string query = "SELECT month_plan.month_id,quarter_plan.quarter_id," +
                    "organization.org_id, organization.name,designations.work_done," +
                    "designations.unit_of_measure,month_plan.section_true," +
                    "locomative_information.loco_id,locomative_information.name AS name_loco," +
                    "fuel_type.type AS fuel_type,quarter_plan.locomative_number," +
                    "reprair_type.type FROM locomative_information" +
                    " INNER JOIN quarter_plan ON quarter_plan.loco_id = locomative_information.loco_id" +
                    " INNER JOIN fuel_type ON fuel_type.fuel_type_id = locomative_information.fuel_type_id" +
                    " INNER JOIN reprair_type ON reprair_type.reprair_id = quarter_plan.reprair_id" +
                    " INNER JOIN month_plan ON month_plan.quarter_id = quarter_plan.quarter_id" +
                    " INNER JOIN designations ON designations.designations_id = month_plan.designations_id" +
                    " INNER JOIN organization ON organization.org_id = designations.org_id" +
                " WHERE month_plan.status_id != 2 AND month_plan.month_id=@month_id;";
            try
            {
                conn.Open();

                using (NpgsqlCommand cmd = new(query, conn))
                {
                    cmd.Parameters.AddWithValue("@month_id", id);
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
                    monthPlan = new(row, 0);
                }
            }
            else
            {
                string query1 = "SELECT month_plan.month_id,quarter_plan.quarter_id," +
                "month_plan.section_true," +
                "locomative_information.loco_id,locomative_information.name AS name_loco," +
                "fuel_type.type AS fuel_type,quarter_plan.locomative_number," +
                "reprair_type.type FROM locomative_information" +
                " INNER JOIN quarter_plan ON quarter_plan.loco_id = locomative_information.loco_id" +
                " INNER JOIN fuel_type ON fuel_type.fuel_type_id = locomative_information.fuel_type_id" +
                " INNER JOIN reprair_type ON reprair_type.reprair_id = quarter_plan.reprair_id" +
                " INNER JOIN month_plan ON month_plan.quarter_id = quarter_plan.quarter_id" +
                " WHERE month_plan.status_id != 2 AND month_plan.month_id=@month_id;";
                try
                {
                    conn.Open();

                    using (NpgsqlCommand cmd = new(query1, conn))
                    {
                        cmd.Parameters.AddWithValue("@month_id", id);
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
                        monthPlan = new(row, 0);
                    }
                }
            }

            return monthPlan;
        }

        public void UpdateMonthPlanOne(int id, MonthPlanOne monthPlan, int loginiduser)
        {
            if (monthPlan != null)
            {

                monthPlan.status = StatusEnum.updated;

                try
                {
                    string query1 = $"UPDATE month_plan" +
                        $" SET quarter_id ={monthPlan.quarter_id}, designations_id ={monthPlan.designations_id}, " +
                        $"section_true ={monthPlan.section_true}" +
                        $" WHERE month_id ={id}; ";
                    monthPlan.data_log = logWriting(loginiduser, query1);



                    string query = "UPDATE public.month_plan" +
                        " SET quarter_id = @quarter_id, designations_id = @designations_id," +
                        "section_true = @section_true, status_id = @status_id, data_log_id = @data_log_id" +
                        " WHERE month_id = @month_id; ";

                    conn.Open();
                    using (NpgsqlCommand cmd = new(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@quarter_id", monthPlan.quarter_id);
                        cmd.Parameters.AddWithValue("@designations_id", monthPlan.designations_id);
                        cmd.Parameters.AddWithValue("@section_true", monthPlan.section_true);
                        cmd.Parameters.AddWithValue("@status_id", (int)monthPlan.status);
                        cmd.Parameters.AddWithValue("@data_log_id", monthPlan.data_log);
                        cmd.Parameters.AddWithValue("@month_id", id);
                        var a = cmd.ExecuteNonQuery();

                    }
                }
                catch { }
                finally { conn.Close(); }
            }
        }
    }
}
