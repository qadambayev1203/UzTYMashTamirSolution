using Entities.Model.All;
using Entities.Model.AnualyPlan;
using Entities.Model.User;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.AllContext.AnualyPlanContext
{
    public class AnualyPlanContext : DatabaseConnection
    {
        //Anualy Plan
        public string CreateAnualyPlan(AnualyPlan anualyPlan, int loginiduser)
        {
            if (anualyPlan != null)
            {
                try
                {
                    anualyPlan.status = StatusEnum.active;
                    anualyPlan.locomative_name.loco_id = locomativeGetId(anualyPlan.locomative_name.name);
                    anualyPlan.locomative_name.fuel_type = (FuelType)locomativeGettypeId(anualyPlan.locomative_name.loco_id);



                    string query1 = "INSERT INTO anualy_plan (anualy_id,locomative_id,sections_repraer_number,information_confirmed_date,information_entered_date,status_id,data_log_id,plan_year,reprair_id,all_price) " +
                        "VALUES " +
                        $"(DEFAULT,{anualyPlan.locomative_name.name},{anualyPlan.sections_reprair_number},{anualyPlan.information_confirmed_date},{anualyPlan.information_entered_date},{anualyPlan.status},{anualyPlan.plan_year},{anualyPlan.reprair_type},{anualyPlan.all_price});";
                    anualyPlan.data_log = logWriting(loginiduser, query1);



                    string query = "INSERT INTO anualy_plan (anualy_id,locomative_id,sections_repraer_number,information_confirmed_date,information_entered_date,status_id,data_log_id,plan_year,reprair_id,all_price) " +
                        "VALUES " +
                        "(DEFAULT,@locomative_id,@sections_repraer_number,@information_confirmed_date,@information_entered_date,@status_id,@data_log_id,@plan_year,@reprair_id,@all_price);";
                    conn.Open();
                    using (NpgsqlCommand cmd = new(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@locomative_id", anualyPlan.locomative_name.loco_id);
                        cmd.Parameters.AddWithValue("@sections_repraer_number", anualyPlan.sections_reprair_number);
                        cmd.Parameters.AddWithValue("@information_confirmed_date", NpgsqlDbType.Date, anualyPlan.information_confirmed_date);
                        cmd.Parameters.AddWithValue("@information_entered_date", NpgsqlDbType.Date, DateTime.Now);
                        cmd.Parameters.AddWithValue("@status_id", (int)anualyPlan.status);
                        cmd.Parameters.AddWithValue("@data_log_id", anualyPlan.data_log);
                        cmd.Parameters.AddWithValue("@plan_year", NpgsqlDbType.Date, DateTime.Now);
                        cmd.Parameters.AddWithValue("@all_price", NpgsqlDbType.Double, anualyPlan.all_price);
                        cmd.Parameters.AddWithValue("@reprair_id", (int)anualyPlan.reprair_type);
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

        public void DeleteAnualyPlan(int id, int loginiduser)
        {
            try
            {
                int datalog;
                string query = $"UPDATE anualy_plan SET status_id=@status_id, data_log_id=@data_log_id WHERE anualy_id=@anualy_id;";


                string query1 = $"UPDATE anualy_plan SET status={StatusEnum.deleted} WHERE anualy_id={id};";
                datalog = logWriting(loginiduser, query);

                conn.Open();

                using (NpgsqlCommand cmd = new(query, conn))
                {
                    cmd.Parameters.AddWithValue("@anualy_id", id);
                    cmd.Parameters.AddWithValue("@status_id", (int)StatusEnum.deleted);
                    cmd.Parameters.AddWithValue("@data_log_id", datalog);
                    var a = cmd.ExecuteNonQuery();
                }
            }
            catch { }
            finally { conn.Close(); }
        }

        public IEnumerable<AnualyPlan> GetAllYearAnualyPlan(int year, int queryNum)
        {


            List<AnualyPlan> anualyPlans = new();
            DataTable table = null;
            try
            {
                string limit = queryNum != 0 ? " LIMIT @queryNum;" : ";";
                string query = "SELECT anualy_plan.anualy_id ,locomative_information.loco_id, " +
                    "locomative_information.name,locomative_information.fuel_type_id," +
                    "anualy_plan.sections_repraer_number, anualy_plan.information_confirmed_date," +
                    "anualy_plan.information_entered_date," +
                    "anualy_plan.information_modified_date, anualy_plan.status_id, anualy_plan.data_log_id," +
                    "anualy_plan.plan_year, reprair_type.type, anualy_plan.all_price FROM locomative_information" +
                    " INNER JOIN" +
                    " anualy_plan ON anualy_plan.locomative_id = locomative_information.loco_id" +
                    " INNER JOIN reprair_type ON reprair_type.reprair_id = anualy_plan.reprair_id " +
                    " WHERE anualy_plan.status_id != 2 AND EXTRACT(YEAR FROM anualy_plan.plan_year) = @year" + limit;



                conn.Open();

                using (NpgsqlCommand cmd = new(query, conn))
                {
                    cmd.Parameters.AddWithValue("@year", year);
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
                    AnualyPlan anualyPlan = new(row);
                    anualyPlans.Add(anualyPlan);
                }
            }
            return anualyPlans;
        }

        public AnualyPlan GetAnualyPlanByID(int id)
        {
            DataTable table = null;
            AnualyPlan anualyPlan = null;

            string query = "SELECT anualy_plan.anualy_id ,locomative_information.loco_id, " +
                "locomative_information.name,locomative_information.fuel_type_id," +
                "anualy_plan.sections_repraer_number, anualy_plan.information_confirmed_date," +
                "anualy_plan.information_entered_date," +
                "anualy_plan.information_modified_date, anualy_plan.status_id, anualy_plan.data_log_id," +
                "anualy_plan.plan_year, reprair_type.type, anualy_plan.all_price FROM locomative_information" +
                " INNER JOIN" +
                " anualy_plan ON anualy_plan.locomative_id = locomative_information.loco_id" +
                " INNER JOIN reprair_type ON reprair_type.reprair_id = anualy_plan.reprair_id " +
                " WHERE anualy_plan.anualy_id=@anualy_id;";
            try
            {
                conn.Open();

                using (NpgsqlCommand cmd = new(query, conn))
                {
                    cmd.Parameters.AddWithValue("@anualy_id", id);
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
                    anualyPlan = new(row);
                }
            }

            return anualyPlan;
        }

        public void UpdateAnualyPlan(int id, AnualyPlan anualyPlan, int loginiduser)
        {
            if (anualyPlan != null)
            {
                try
                {
                    anualyPlan.status = StatusEnum.updated;
                    anualyPlan.locomative_name.loco_id = locomativeGetId(anualyPlan.locomative_name.name);
                    anualyPlan.locomative_name.fuel_type = (FuelType)locomativeGettypeId(anualyPlan.locomative_name.loco_id);


                    string query1 = $"UPDATE anualy_plan SET locomative_id='{anualyPlan.locomative_name.loco_id}',sections_repraer_number='{anualyPlan.sections_reprair_number}',information_confirmed_date='{anualyPlan.information_confirmed_date}',reprair_id='{anualyPlan.reprair_type}', all_price='{anualyPlan.all_price}' WHERE anualy_id='{id}';";
                    anualyPlan.data_log = logWriting(loginiduser, query1);



                    string query = "UPDATE anualy_plan " +
                    "SET locomative_id = @locomative_id, sections_repraer_number = @sections_repraer_number," +
                    "information_confirmed_date = @information_confirmed_date," +
                    "information_modified_date = @information_modified_date, status_id = @status_id," +
                    "data_log_id = @data_log_id, plan_year = @plan_year, reprair_id = @reprair_id, all_price=@all_price" +
                    " WHERE anualy_id = @anualy_id; ";

                    conn.Open();
                    using (NpgsqlCommand cmd = new(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@anualy_id", id);
                        cmd.Parameters.AddWithValue("@locomative_id", anualyPlan.locomative_name.loco_id);
                        cmd.Parameters.AddWithValue("@sections_repraer_number", anualyPlan.sections_reprair_number);
                        cmd.Parameters.AddWithValue("@information_confirmed_date", NpgsqlDbType.Date, anualyPlan.information_confirmed_date);
                        cmd.Parameters.AddWithValue("@information_modified_date", NpgsqlDbType.Date, DateTime.Now);
                        cmd.Parameters.AddWithValue("@status_id", (int)anualyPlan.status);
                        cmd.Parameters.AddWithValue("@data_log_id", anualyPlan.data_log);
                        cmd.Parameters.AddWithValue("@plan_year", NpgsqlDbType.Date, DateTime.Now);
                        cmd.Parameters.AddWithValue("@reprair_id", (int)anualyPlan.reprair_type);
                        cmd.Parameters.AddWithValue("@all_price", NpgsqlDbType.Double, anualyPlan.all_price);
                        var a = cmd.ExecuteNonQuery();

                    }
                }
                catch { }
                finally { conn.Close(); }
            }
        }

        public void UpdateAnualyPlanOneAdd(int id)
        {
            try
            {
                int status = (int)StatusEnum.addition;

                string query = "UPDATE anualy_plan " +
                "SET status_id = @status_id" +
                " WHERE anualy_id = @anualy_id; ";

                conn.Open();
                using (NpgsqlCommand cmd = new(query, conn))
                {
                    cmd.Parameters.AddWithValue("@anualy_id", id);
                    cmd.Parameters.AddWithValue("@status_id", status);

                    var a = cmd.ExecuteNonQuery();

                }
            }
            catch { }
            finally { conn.Close(); }

        }

        //AnualyPlan Plan One


        public string CreateAnualyOnePlan(AnualyPlan anualyPlan, int loginiduser)
        {
            if (anualyPlan != null)
            {

                try
                {
                    anualyPlan.status = StatusEnum.active;

                    string query1 = "INSERT INTO anualy_plan_one(a_o_id, anualy_id, " +
                        "information_confirmed_date, status_id, data_log_id, yanvar, " +
                        "fevral, mart, aprel, may, iyun, iyul, avgust, sentyabr, oktyabr, noyabr, " +
                        "dekabr) " +
                        $"VALUES(DEFAULT,'{anualyPlan.anualy_id}','{anualyPlan.information_confirmed_date}','{anualyPlan.status}','{anualyPlan.data_log}','{anualyPlan.month_plan.Yanvar}'," +
                        $"'{anualyPlan.month_plan.Fevral}','{anualyPlan.month_plan.Mart}','{anualyPlan.month_plan.Aprel}','{anualyPlan.month_plan.May}'," +
                        $"'{anualyPlan.month_plan.Iyun}','{anualyPlan.month_plan.Iyul}','{anualyPlan.month_plan.Avgust}','{anualyPlan.month_plan.Sentyabr}','{anualyPlan.month_plan.Oktyabr}','{anualyPlan.month_plan.Noyabr}','{anualyPlan.month_plan.Dekabr}'" +
                        "); ";
                    anualyPlan.data_log = logWriting(loginiduser, query1);


                    string query = "INSERT INTO public.anualy_plan_one(" +
                        "a_o_id, anualy_id, information_confirmed_date, information_entered_date," +
                        "status_id, data_log_id, yanvar, fevral, mart, aprel, may, iyun," +
                        "iyul, avgust, sentyabr, oktyabr, noyabr, dekabr)" +
                        " VALUES(DEFAULT, @anualy_id, @information_confirmed_date, @information_entered_date," +
                        "@status_id, @data_log_id, @yanvar, @fevral, @mart, @aprel, @may," +
                        "@iyun, @iyul, @avgust, @sentyabr, @oktyabr, @noyabr, @dekabr); ";
                    conn.Open();
                    using (NpgsqlCommand cmd = new(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@anualy_id", anualyPlan.anualy_id);
                        cmd.Parameters.AddWithValue("@information_confirmed_date", NpgsqlDbType.Date, anualyPlan.information_confirmed_date);
                        cmd.Parameters.AddWithValue("@information_entered_date", NpgsqlDbType.Date, DateTime.Now);
                        cmd.Parameters.AddWithValue("@status_id", (int)anualyPlan.status);
                        cmd.Parameters.AddWithValue("@data_log_id", anualyPlan.data_log);
                        cmd.Parameters.AddWithValue("@yanvar", anualyPlan.month_plan.Yanvar);
                        cmd.Parameters.AddWithValue("@fevral", anualyPlan.month_plan.Fevral);
                        cmd.Parameters.AddWithValue("@mart", anualyPlan.month_plan.Mart);
                        cmd.Parameters.AddWithValue("@aprel", anualyPlan.month_plan.Aprel);
                        cmd.Parameters.AddWithValue("@may", anualyPlan.month_plan.May);
                        cmd.Parameters.AddWithValue("@iyun", anualyPlan.month_plan.Iyun);
                        cmd.Parameters.AddWithValue("@iyul", anualyPlan.month_plan.Iyul);
                        cmd.Parameters.AddWithValue("@avgust", anualyPlan.month_plan.Avgust);
                        cmd.Parameters.AddWithValue("@sentyabr", anualyPlan.month_plan.Sentyabr);
                        cmd.Parameters.AddWithValue("@oktyabr", anualyPlan.month_plan.Oktyabr);
                        cmd.Parameters.AddWithValue("@noyabr", anualyPlan.month_plan.Noyabr);
                        cmd.Parameters.AddWithValue("@dekabr", anualyPlan.month_plan.Dekabr);
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

        public void DeleteAnualyOnePlan(int id, int loginiduser)
        {
            try
            {
                int datalog;
                string query = $"UPDATE anualy_plan_one SET status_id=@status_id, data_log_id=@data_log_id WHERE a_o_id=@a_o_id;";


                string query1 = $"UPDATE anualy_plan SET status={StatusEnum.deleted} WHERE a_o_id={id};";
                datalog = logWriting(loginiduser, query);

                conn.Open();

                using (NpgsqlCommand cmd = new(query, conn))
                {
                    cmd.Parameters.AddWithValue("@a_o_id", id);
                    cmd.Parameters.AddWithValue("@status_id", (int)StatusEnum.deleted);
                    cmd.Parameters.AddWithValue("@data_log_id", datalog);
                    var a = cmd.ExecuteNonQuery();
                }
            }
            catch { }
            finally { conn.Close(); }
        }

        public IEnumerable<AnualyPlan> GetAllYearAnualyOnePlan(int year, int queryNum)
        {
            List<AnualyPlan> anualyPlans = new();
            DataTable table = null;
            try
            {
                string limit = queryNum != 0 ? " LIMIT @queryNum;" : ";";
                string query = "SELECT  locomative_information.loco_id, locomative_information.name, " +
                    "locomative_information.fuel_type_id, " +
                    "anualy_plan.sections_repraer_number, reprair_type.type, anualy_plan_one.a_o_id, " +
                    "anualy_plan_one.information_confirmed_date, " +
                    "anualy_plan_one.yanvar, anualy_plan_one.fevral, anualy_plan_one.mart, anualy_plan_one.aprel," +
                    "anualy_plan_one.may, anualy_plan_one.iyun, anualy_plan_one.iyul, anualy_plan_one.avgust, " +
                    "anualy_plan_one.sentyabr, anualy_plan_one.oktyabr, anualy_plan_one.noyabr, " +
                    "anualy_plan_one.dekabr,anualy_plan_one.anualy_id" +
                    " FROM locomative_information" +
                    " INNER JOIN anualy_plan ON anualy_plan.locomative_id = locomative_information.loco_id" +
                    " INNER JOIN reprair_type ON reprair_type.reprair_id = anualy_plan.reprair_id" +
                    " INNER JOIN anualy_plan_one ON anualy_plan_one.anualy_id = anualy_plan.anualy_id" +
                    " WHERE anualy_plan_one.status_id != 2 AND EXTRACT(YEAR FROM anualy_plan.plan_year) = @year" + limit;


                conn.Open();

                using (NpgsqlCommand cmd = new(query, conn))
                {
                    cmd.Parameters.AddWithValue("@year", year);
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
                    AnualyPlan anualyPlan = new(row, 1);
                    anualyPlans.Add(anualyPlan);
                }
            }
            return anualyPlans;
        }

        public AnualyPlan GetAnualyPlanOneByID(int id)
        {
            DataTable table = null;
            AnualyPlan anualyPlan = null;

            string query = "SELECT  locomative_information.loco_id, locomative_information.name, " +
                    "locomative_information.fuel_type_id, " +
                    "anualy_plan.sections_repraer_number, reprair_type.type, anualy_plan_one.a_o_id, " +
                    "anualy_plan_one.information_confirmed_date, " +
                    "anualy_plan_one.yanvar, anualy_plan_one.fevral, anualy_plan_one.mart, anualy_plan_one.aprel," +
                    "anualy_plan_one.may, anualy_plan_one.iyun, anualy_plan_one.iyul, anualy_plan_one.avgust, " +
                    "anualy_plan_one.sentyabr, anualy_plan_one.oktyabr, anualy_plan_one.noyabr, " +
                    "anualy_plan_one.dekabr,anualy_plan_one.anualy_id" +
                    " FROM locomative_information" +
                    " INNER JOIN anualy_plan ON anualy_plan.locomative_id = locomative_information.loco_id" +
                    " INNER JOIN reprair_type ON reprair_type.reprair_id = anualy_plan.reprair_id" +
                    " INNER JOIN anualy_plan_one ON anualy_plan_one.anualy_id = anualy_plan.anualy_id" +
                " WHERE anualy_plan_one.a_o_id=@a_o_id AND anualy_plan_one.status_id != 2;";
            try
            {
                conn.Open();

                using (NpgsqlCommand cmd = new(query, conn))
                {
                    cmd.Parameters.AddWithValue("@a_o_id", id);
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
                    anualyPlan = new(row, 1);
                }
            }

            return anualyPlan;
        }

        public void UpdateAnualyOnePlan(int id, AnualyPlan anualyPlan, int loginiduser)
        {
            if (anualyPlan != null)
            {

                try
                {
                    anualyPlan.status = StatusEnum.updated;

                    string query1 = $"UPDATE public.anualy_plan_one" +
                        $" SET information_confirmed_date ='{anualyPlan.information_confirmed_date}'," +
                        $"yanvar ='{anualyPlan.month_plan.Yanvar}', fevral ='{anualyPlan.month_plan.Fevral}', mart ='{anualyPlan.month_plan.Mart}', aprel ='{anualyPlan.month_plan.Aprel}'," +
                        $" may ='{anualyPlan.month_plan.May}', iyun ='{anualyPlan.month_plan.Iyun}', iyul ='{anualyPlan.month_plan.Iyul}', avgust ='{anualyPlan.month_plan.Avgust}', sentyabr ='{anualyPlan.month_plan.Sentyabr}', oktyabr ='{anualyPlan.month_plan.Oktyabr}', noyabr ='{anualyPlan.month_plan.Noyabr}', " +
                        $"dekabr ='{anualyPlan.month_plan.Dekabr}' WHERE a_o_id ={id}; ";
                    anualyPlan.data_log = logWriting(loginiduser, query1);



                    string query = "UPDATE anualy_plan_one" +
                        " SET information_confirmed_date = @information_confirmed_date," +
                        "information_modifaed_date = @information_modifaed_date, status_id = @status_id," +
                        "data_log_id = @data_log_id, yanvar = @yanvar, fevral = @fevral, mart = @mart," +
                        "aprel = @aprel, may = @may, iyun = @iyun, iyul = @iyul, avgust = @avgust," +
                        "sentyabr = @sentyabr, oktyabr = @oktyabr," +
                        "noyabr = @noyabr, dekabr = @dekabr WHERE a_o_id = @a_o_id; ";

                    conn.Open();
                    using (NpgsqlCommand cmd = new(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@a_o_id", id);
                        cmd.Parameters.AddWithValue("@information_confirmed_date", NpgsqlDbType.Date, anualyPlan.information_confirmed_date);
                        cmd.Parameters.AddWithValue("@information_modifaed_date", NpgsqlDbType.Date, DateTime.Now);
                        cmd.Parameters.AddWithValue("@status_id", (int)anualyPlan.status);
                        cmd.Parameters.AddWithValue("@data_log_id", anualyPlan.data_log);
                        cmd.Parameters.AddWithValue("@yanvar", anualyPlan.month_plan.Yanvar);
                        cmd.Parameters.AddWithValue("@fevral", anualyPlan.month_plan.Fevral);
                        cmd.Parameters.AddWithValue("@mart", anualyPlan.month_plan.Mart);
                        cmd.Parameters.AddWithValue("@aprel", anualyPlan.month_plan.Aprel);
                        cmd.Parameters.AddWithValue("@may", anualyPlan.month_plan.May);
                        cmd.Parameters.AddWithValue("@iyun", anualyPlan.month_plan.Iyun);
                        cmd.Parameters.AddWithValue("@iyul", anualyPlan.month_plan.Iyul);
                        cmd.Parameters.AddWithValue("@avgust", anualyPlan.month_plan.Avgust);
                        cmd.Parameters.AddWithValue("@sentyabr", anualyPlan.month_plan.Sentyabr);
                        cmd.Parameters.AddWithValue("@oktyabr", anualyPlan.month_plan.Oktyabr);
                        cmd.Parameters.AddWithValue("@noyabr", anualyPlan.month_plan.Noyabr);
                        cmd.Parameters.AddWithValue("@dekabr", anualyPlan.month_plan.Dekabr);
                        var a = cmd.ExecuteNonQuery();

                    }
                }
                catch { }
                finally { conn.Close(); }
            }
        }



        //Anualy Plan Two
        public IEnumerable<AnualyPlan> GetAllYearAnualyTwoPlan(int year, int queryNum)
        {
            List<AnualyPlan> anualyPlans = new();
            DataTable table = null;
            try
            {
                string limit = queryNum != 0 ? " LIMIT @queryNum;" : ";";
                string query = "SELECT  anualy_plan_one.a_o_id, locomative_information.loco_id, " +
                    "locomative_information.name," +
                    "locomative_information.fuel_type_id," +
                    "anualy_plan.sections_repraer_number, reprair_type.type" +
                    " FROM locomative_information" +
                    " INNER JOIN anualy_plan ON anualy_plan.locomative_id = locomative_information.loco_id" +
                    " INNER JOIN reprair_type ON reprair_type.reprair_id = anualy_plan.reprair_id" +
                    " INNER JOIN anualy_plan_one ON anualy_plan_one.anualy_id = anualy_plan.anualy_id" +
                    " WHERE anualy_plan_one.status_id != 2 AND EXTRACT(YEAR FROM anualy_plan.plan_year) = @year" + limit;


                conn.Open();

                using (NpgsqlCommand cmd = new(query, conn))
                {
                    cmd.Parameters.AddWithValue("@year", year);
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
                    AnualyPlan anualyPlan = new(row, "");
                    anualyPlans.Add(anualyPlan);
                }
            }
            return anualyPlans;
        }




    }
}
