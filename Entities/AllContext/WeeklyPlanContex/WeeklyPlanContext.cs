using Entities.Model.User;
using Entities.Model.WeeklyPlanModel;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.AllContext.WeeklyPlanContex
{
    public class WeeklyPlanContext : DatabaseConnection
    {
        public WeeklyPlanContext()
        {

        }


        public string CreateWeekluPlan(WeeklyPlan weeklyPlan, int loginiduser)
        {
            if (weeklyPlan != null)
            {

                weeklyPlan.status_id = StatusEnum.active;

                try
                {
                    string query1 = "INSERT INTO weekly_plan(" +
                        " weekly_id, w_w_id, loco_id, locomative_number, week_date, number_of_locomotives)" +
                        $" VALUES(DEFAULT, {weeklyPlan.w_w_id}, {weeklyPlan.loco_id}, {weeklyPlan.locomative_number}, {weeklyPlan.week_date}, {weeklyPlan.number_of_locomotives}); ";
                    weeklyPlan.data_log_id = logWriting(loginiduser, query1);



                    string query = "INSERT INTO weekly_plan(" +
                        "weekly_id, w_w_id, loco_id, locomative_number, status_id, data_log_id, week_date, " +
                        "number_of_locomotives)" +
                        " VALUES (DEFAULT, @w_w_id, @loco_id, @locomative_number, @status_id," +
                        "@data_log_id, @week_date, @number_of_locomotives); ";

                    conn.Open();
                    using (NpgsqlCommand cmd = new(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@w_w_id", weeklyPlan.w_w_id);
                        cmd.Parameters.AddWithValue("@loco_id", weeklyPlan.loco_id);
                        cmd.Parameters.AddWithValue("@locomative_number", weeklyPlan.locomative_number);
                        cmd.Parameters.AddWithValue("@status_id", (int)weeklyPlan.status_id);
                        cmd.Parameters.AddWithValue("@data_log_id", weeklyPlan.data_log_id);
                        cmd.Parameters.AddWithValue("@week_date", NpgsqlDbType.Date, weeklyPlan.week_date);
                        cmd.Parameters.AddWithValue("@number_of_locomotives", weeklyPlan.number_of_locomotives);
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

        public void DeleteWeekluPlan(int id, int loginiduser)
        {
            try
            {
                int datalog;
                string query = $"UPDATE weekly_plan SET status_id=@status_id, data_log_id=@data_log_id WHERE weekly_id=@weekly_id;";


                string query1 = $"UPDATE quarter_plan SET status={StatusEnum.deleted} WHERE quarter_id={id};";
                datalog = logWriting(loginiduser, query);

                conn.Open();

                using (NpgsqlCommand cmd = new(query, conn))
                {
                    cmd.Parameters.AddWithValue("@weekly_id", id);
                    cmd.Parameters.AddWithValue("@status_id", (int)StatusEnum.deleted);
                    cmd.Parameters.AddWithValue("@data_log_id", datalog);
                    var a = cmd.ExecuteNonQuery();
                }
            }
            catch { }
            finally { conn.Close(); }
        }

        public IEnumerable<WeeklyPlan> GetAllYearWeekluPlan(DateTime week_date, int queryNum)
        {
            List<WeeklyPlan> weeklyPlans = new();
            DataTable table = null;
            try
            {
                string limit = queryNum != 0 ? " LIMIT @queryNum;" : ";";
                string query = "SELECT weekly_plan.weekly_id, weekly_work.w_w_id, weekly_work.w_work," +
                    "locomative_information.loco_id, locomative_information.name, locomative_information.fuel_type_id," +
                    "fuel_type.type, weekly_plan.locomative_number, weekly_plan.number_of_locomotives," +
                    "weekly_plan.week_date FROM fuel_type" +
                    " INNER JOIN locomative_information ON locomative_information.fuel_type_id = fuel_type.fuel_type_id" +
                    " INNER JOIN weekly_plan ON weekly_plan.loco_id = locomative_information.loco_id" +
                    " INNER JOIN weekly_work ON weekly_work.w_w_id = weekly_plan.w_w_id" +
                    " WHERE weekly_plan.status_id != 2 AND DATE(weekly_plan.week_date) = @week_date::date" + limit;



                conn.Open();

                using (NpgsqlCommand cmd = new(query, conn))
                {
                    cmd.Parameters.AddWithValue("@week_date", NpgsqlDbType.Date, week_date);
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
                    WeeklyPlan weeklyPlan = new(row);
                    weeklyPlans.Add(weeklyPlan);
                }
            }
            return weeklyPlans;
        }

        public WeeklyPlan GetWeekluPlanByID(int id)
        {
            DataTable table = null;
            WeeklyPlan weeklyPlan = null;

            string query = "SELECT weekly_plan.weekly_id, weekly_work.w_w_id, weekly_work.w_work," +
                    "locomative_information.loco_id, locomative_information.name, locomative_information.fuel_type_id," +
                    "fuel_type.type, weekly_plan.locomative_number, weekly_plan.number_of_locomotives," +
                    "weekly_plan.week_date FROM fuel_type" +
                    " INNER JOIN locomative_information ON locomative_information.fuel_type_id = fuel_type.fuel_type_id" +
                    " INNER JOIN weekly_plan ON weekly_plan.loco_id = locomative_information.loco_id" +
                    " INNER JOIN weekly_work ON weekly_work.w_w_id = weekly_plan.w_w_id" +
                    " WHERE weekly_plan.status_id != 2 AND weekly_plan.weekly_id=@weekly_plan;";
            try
            {
                conn.Open();

                using (NpgsqlCommand cmd = new(query, conn))
                {
                    cmd.Parameters.AddWithValue("@weekly_plan", id);
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
                    weeklyPlan = new(row);
                }
            }

            return weeklyPlan;
        }

        public void UpdateWeekluPlan(int id, WeeklyPlan weeklyPlan, int loginiduser)
        {
            if (weeklyPlan != null)
            {

                weeklyPlan.status_id = StatusEnum.updated;

                try
                {
                    string query1 = $"UPDATE weekly_plan" +
                        $" SET w_w_id = {weeklyPlan.w_w_id}, loco_id = {weeklyPlan.loco_id}, locomative_number = '{weeklyPlan.locomative_number}', " +
                        $" week_date = '{weeklyPlan.week_date}', number_of_locomotives = {weeklyPlan.number_of_locomotives}" +
                        $" WHERE weekly_id = {id}; ";
                    weeklyPlan.data_log_id = logWriting(loginiduser, query1);



                    string query = "UPDATE weekly_plan" +
                        " SET w_w_id = @w_w_id, loco_id = @loco_id, locomative_number = @locomative_number, " +
                        "status_id = @status_id," +
                        "data_log_id = @data_log_id, week_date = @week_date, " +
                        "number_of_locomotives = @number_of_locomotives" +
                        " WHERE weekly_id = @weekly_id";

                    conn.Open();
                    using (NpgsqlCommand cmd = new(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@weekly_id", id);
                        cmd.Parameters.AddWithValue("@w_w_id", weeklyPlan.w_w_id);
                        cmd.Parameters.AddWithValue("@loco_id", weeklyPlan.loco_id);
                        cmd.Parameters.AddWithValue("@locomative_number", weeklyPlan.locomative_number);
                        cmd.Parameters.AddWithValue("@status_id", (int)weeklyPlan.status_id);
                        cmd.Parameters.AddWithValue("@data_log_id", weeklyPlan.data_log_id);
                        cmd.Parameters.AddWithValue("@week_date", NpgsqlDbType.Date, weeklyPlan.week_date);
                        cmd.Parameters.AddWithValue("@number_of_locomotives", weeklyPlan.number_of_locomotives);
                        var a = cmd.ExecuteNonQuery();

                    }
                }
                catch { }
                finally { conn.Close(); }
            }
        }
    }
}
