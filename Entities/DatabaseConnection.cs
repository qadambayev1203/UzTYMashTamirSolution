using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class DatabaseConnection
    {
        protected readonly NpgsqlConnection conn = new("Server=localhost;Port=5432;Database=UzTYMashTamirDB;User Id=postgres;Password=1203");
        protected int locomativeGetId(string name)
        {

            int id = 0;
            try
            {
                conn.Open();
                string locopuery = "SELECT loco_id FROM locomative_information WHERE name = @name";

                using (NpgsqlCommand cmd = new(locopuery, conn))
                {
                    cmd.Parameters.AddWithValue("@name", name);
                    var a = cmd.ExecuteScalar();
                    id = Convert.ToInt32(a);
                }


            }
            catch
            { }
            finally { conn.Close(); }
            return id;
        }

        protected int locomativeGettypeId(int id)
        {

            try

            {
                conn.Open();
                string locopuery = "SELECT fuel_type_id FROM locomative_information WHERE loco_id = @loco_id";

                using (NpgsqlCommand cmd = new(locopuery, conn))
                {
                    cmd.Parameters.AddWithValue("@loco_id", id);
                    var a = cmd.ExecuteScalar();
                    id = Convert.ToInt32(a);
                }


            }
            catch
            { }
            finally { conn.Close(); }
            return id;
        }

        protected int logWriting(int loginiduser, string query1)
        {
            int log_id = 0;
            try
            {
                string logquery = "INSERT INTO data_log VALUES (DEFAULT,@user_id,@executeon_date,@request_text) RETURNING data_log_id;";
                conn.Open();
                using (NpgsqlCommand cmd = new(logquery, conn))
                {
                    cmd.Parameters.AddWithValue("@user_id", loginiduser);
                    cmd.Parameters.AddWithValue("@executeon_date", NpgsqlDbType.Date, DateTime.Now);
                    cmd.Parameters.AddWithValue("@request_text", query1);
                    var a = cmd.ExecuteScalar();
                    log_id = Convert.ToInt32(a);
                }

            }
            catch
            {


            }
            finally { conn.Close(); }
            return log_id;
        }
    }
}
