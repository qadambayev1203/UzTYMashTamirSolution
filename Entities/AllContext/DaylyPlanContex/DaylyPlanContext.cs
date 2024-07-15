using Entities.Model.DaylyPlanModel;
using Entities.Model.User;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.AllContext.DaylyPlanContex
{
    public class DaylyPlanContext : DatabaseConnection
    {
        public DaylyPlanContext()
        {

        }

        public string CreateDaylyPlan(DaylyPlan daylyPlan, int loginiduser)
        {
            if (daylyPlan != null)
            {

                daylyPlan.status_id = StatusEnum.active;

                try
                {
                    string query1 = "INSERT INTO dayly_plan(" +
                        "dayly_id, loco_id, locomative_number, reprair_id, org_id, do_work, comment, day_date)" +
                        $"VALUES(DEFAULT, {daylyPlan.loco_id}, {daylyPlan.locomative_number}, {daylyPlan.reprair_id}, {daylyPlan.org_id}, {daylyPlan.do_work}, {daylyPlan.comment},{daylyPlan.day_date}); ";
                    daylyPlan.data_log_id = logWriting(loginiduser, query1);



                    string query = "INSERT INTO dayly_plan(" +
                        "dayly_id, loco_id, locomative_number, reprair_id, org_id, do_work," +
                        "comment, status_id, data_log_id,day_date)" +
                        "VALUES (DEFAULT, @loco_id, @locomative_number, @reprair_id, @org_id, @do_work, @comment," +
                        "@status_id, @data_log_id,@day_date); ";

                    conn.Open();
                    using (NpgsqlCommand cmd = new(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@loco_id", daylyPlan.loco_id);
                        cmd.Parameters.AddWithValue("@locomative_number", daylyPlan.locomative_number);
                        cmd.Parameters.AddWithValue("@reprair_id", (int)daylyPlan.reprair_id);
                        cmd.Parameters.AddWithValue("@org_id", daylyPlan.org_id);
                        cmd.Parameters.AddWithValue("@do_work", daylyPlan.do_work);
                        cmd.Parameters.AddWithValue("@comment", daylyPlan.comment);
                        cmd.Parameters.AddWithValue("@status_id", (int)daylyPlan.status_id);
                        cmd.Parameters.AddWithValue("@data_log_id", daylyPlan.data_log_id);
                        cmd.Parameters.AddWithValue("@day_date", NpgsqlDbType.Date, daylyPlan.day_date);

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

        public void DeleteDaylyPlan(int id, int loginiduser)
        {
            try
            {
                int datalog;
                string query = $"UPDATE dayly_plan SET status_id=@status_id, data_log_id=@data_log_id WHERE dayly_id=@dayly_id;";


                string query1 = $"UPDATE dayly_plan SET status={StatusEnum.deleted} WHERE dayly_id={id};";
                datalog = logWriting(loginiduser, query);

                conn.Open();

                using (NpgsqlCommand cmd = new(query, conn))
                {
                    cmd.Parameters.AddWithValue("@dayly_id", id);
                    cmd.Parameters.AddWithValue("@status_id", (int)StatusEnum.deleted);
                    cmd.Parameters.AddWithValue("@data_log_id", datalog);
                    var a = cmd.ExecuteNonQuery();
                }
            }
            catch { }
            finally { conn.Close(); }
        }

        public IEnumerable<DaylyPlan> GetAllYearDaylyPlan(DateTime day_date, int queryNum)
        {
            List<DaylyPlan> daylyPlans = new();
            DataTable table = null;
            try
            {
                string limit = queryNum != 0 ? " LIMIT @queryNum;" : ";";
                string query = "SELECT dayly_plan.dayly_id, locomative_information.loco_id, locomative_information.name, locomative_information.fuel_type_id," +
                    "fuel_type.type AS fuel_type, dayly_plan.locomative_number, reprair_type.type, organization.org_id," +
                    "organization.name AS org_name, dayly_plan.do_work, dayly_plan.comment, dayly_plan.day_date" +
                    " FROM fuel_type" +
                    " INNER JOIN locomative_information ON locomative_information.fuel_type_id = fuel_type.fuel_type_id" +
                    " INNER JOIN dayly_plan ON dayly_plan.loco_id = locomative_information.loco_id" +
                    " INNER JOIN reprair_type ON reprair_type.reprair_id = dayly_plan.reprair_id" +
                    " INNER JOIN organization ON organization.org_id = dayly_plan.org_id" +
                    " WHERE dayly_plan.status_id != 2 AND DATE(dayly_plan.day_date) = @day_date::date" + limit;



                conn.Open();

                using (NpgsqlCommand cmd = new(query, conn))
                {
                    cmd.Parameters.AddWithValue("@day_date", NpgsqlDbType.Date, day_date);
                    cmd.Parameters.AddWithValue("@queryNum", queryNum);
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
                    DaylyPlan daylyPlan = new(row);
                    daylyPlans.Add(daylyPlan);
                }
            }
            return daylyPlans;
        }

        public DaylyPlan GetDaylyPlanByID(int id)
        {
            DataTable table = null;
            DaylyPlan daylyPlan = null;

            string query = "SELECT dayly_plan.dayly_id, locomative_information.loco_id, locomative_information.name, locomative_information.fuel_type_id," +
                "fuel_type.type AS fuel_type, dayly_plan.locomative_number, reprair_type.type, organization.org_id," +
                "organization.name, dayly_plan.do_work, dayly_plan.comment, dayly_plan.day_date" +
                " FROM fuel_type" +
                " INNER JOIN locomative_information ON locomative_information.fuel_type_id = fuel_type.fuel_type_id" +
                " INNER JOIN dayly_plan ON dayly_plan.loco_id = locomative_information.loco_id" +
                " INNER JOIN reprair_type ON reprair_type.reprair_id = dayly_plan.reprair_id" +
                " INNER JOIN organization ON organization.org_id = dayly_plan.org_id" +
                " WHERE dayly_plan.status_id != 2 AND dayly_plan.dayly_id=@dayly_id;";
            try
            {
                conn.Open();

                using (NpgsqlCommand cmd = new(query, conn))
                {
                    cmd.Parameters.AddWithValue("@dayly_id", id);
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
                    daylyPlan = new(row);
                }
            }

            return daylyPlan;
        }

        public void UpdateDaylyPlan(int id, DaylyPlan daylyPlan, int loginiduser)
        {
            if (daylyPlan != null)
            {

                daylyPlan.status_id = StatusEnum.updated;

                try
                {
                    string query1 = $"UPDATE dayly_plan" +
                        $"SET loco_id ={daylyPlan.loco_id}, locomative_number ={daylyPlan.locomative_number}, reprair_id ={daylyPlan.reprair_id}, org_id ={daylyPlan.org_id}, do_work ={daylyPlan.do_work}, " +
                        $"comment ={daylyPlan.comment}, day_date ={daylyPlan.day_date}" +
                        $" WHERE dayly_id ={id}; ";
                    daylyPlan.data_log_id = logWriting(loginiduser, query1);



                    string query = "UPDATE dayly_plan" +
                        " SET loco_id = @loco_id, locomative_number = @locomative_number, reprair_id = @reprair_id," +
                        "org_id = @org_id, do_work = @do_work, comment = @comment, status_id = @status_id," +
                        "data_log_id = @data_log_id, day_date = @day_date" +
                        " WHERE dayly_id = @dayly_id; ";

                    conn.Open();
                    using (NpgsqlCommand cmd = new(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@dayly_id", id);
                        cmd.Parameters.AddWithValue("@loco_id", daylyPlan.loco_id);
                        cmd.Parameters.AddWithValue("@locomative_number", daylyPlan.locomative_number);
                        cmd.Parameters.AddWithValue("@reprair_id", (int)daylyPlan.reprair_id);
                        cmd.Parameters.AddWithValue("@org_id", daylyPlan.org_id);
                        cmd.Parameters.AddWithValue("@do_work", daylyPlan.do_work);
                        cmd.Parameters.AddWithValue("@comment", daylyPlan.comment);
                        cmd.Parameters.AddWithValue("@status_id", (int)daylyPlan.status_id);
                        cmd.Parameters.AddWithValue("@data_log_id", daylyPlan.data_log_id);
                        cmd.Parameters.AddWithValue("@day_date", NpgsqlDbType.Date, daylyPlan.day_date);
                        var a = cmd.ExecuteNonQuery();

                    }
                }
                catch { }
                finally { conn.Close(); }
            }
        }
    }
}
