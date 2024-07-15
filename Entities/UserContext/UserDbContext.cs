using Entities.Model.User;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.UserContext
{
    public class UserDbContext : DatabaseConnection
    {
        public void CreateUser(User user, int loginiduser)
        {
            try
            {
                if (user != null)
                {
                    user.status = StatusEnum.active;

                    conn.Open();
                    string query = "INSERT INTO users (userid,firstname,lastname,birthday,pinfl,login,password,role,organizationtype,status,dataLog) " +
                        "VALUES " +
                        "(DEFAULT,@firstname,@lastname,@birthday,@pinfl,@login,@password,@role,@organizationtype,@status,@dataLog);";



                    string query1 = "INSERT INTO users (userid,firstname,lastname,birthday,pinfl,login,password,role,organizationtype) " +
                        "VALUES " +
                        $"(DEFAULT,{user.firstname},{user.lastname},{user.birthday},{user.pinfl},{user.login},{user.password},{user.role},{user.organizationtype});";
                    string logquery = "INSERT INTO data_log VALUES (DEFAULT,@user_id,@executeon_date,@request_text) RETURNING data_log_id;";
                    using (NpgsqlCommand cmd = new(logquery, conn))
                    {
                        cmd.Parameters.AddWithValue("@user_id", loginiduser);
                        cmd.Parameters.AddWithValue("@executeon_date", NpgsqlDbType.Date, DateTime.Now);
                        cmd.Parameters.AddWithValue("@request_text", query1);
                        var a = cmd.ExecuteScalar();
                        user.datalog = Convert.ToInt32(a);
                    }
                    conn.Close();
                    conn.Open();



                    using (NpgsqlCommand cmd = new(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@firstname", user.firstname);
                        cmd.Parameters.AddWithValue("@lastname", user.lastname);
                        cmd.Parameters.AddWithValue("@birthday", NpgsqlDbType.Date, user.birthday);
                        cmd.Parameters.AddWithValue("@pinfl", user.pinfl);
                        cmd.Parameters.AddWithValue("@login", user.login);
                        cmd.Parameters.AddWithValue("@password", user.password);
                        cmd.Parameters.AddWithValue("@role", (int)user.role);
                        cmd.Parameters.AddWithValue("@organizationtype", (int)user.organizationtype);
                        cmd.Parameters.AddWithValue("@status", (int)user.status);
                        cmd.Parameters.AddWithValue("@datalog", (int)user.datalog);
                        var a = cmd.ExecuteNonQuery();

                    }
                }
            }
            catch { }
            finally { conn.Close(); }

        }
        public void UpdateUser(int id, User user, int loginiduser)
        {
            if (user != null)
            {
                try
                {
                    user.status = StatusEnum.updated;

                    conn.Open();
                    string query = $"UPDATE users SET firstname=@firstname,lastname=@lastname,birthday=@birthday,pinfl=@pinfl,login=@login,password=@password,role=@role,organizationtype=@organizationtype, status=@status,dataLog=@dataLog WHERE userid=@userid;";


                    string query1 = $"UPDATE users SET firstname={user.firstname},lastname={user.lastname},birthday={user.birthday},pinfl={user.pinfl},login={user.login},password={user.password},role={user.role},organizationtype={user.organizationtype} WHERE userid={user.id}; " +
                        "VALUES " +
                        $"(DEFAULT,{user.firstname},{user.lastname},{user.birthday},{user.pinfl},{user.login},{user.password},{user.role},{user.organizationtype},{user.status},{user.datalog});";
                    string logquery = "INSERT INTO data_log VALUES (DEFAULT,@user_id,@executeon_date,@request_text) RETURNING data_log_id;";
                    using (NpgsqlCommand cmd = new(logquery, conn))
                    {
                        cmd.Parameters.AddWithValue("@user_id", loginiduser);
                        cmd.Parameters.AddWithValue("@executeon_date", NpgsqlDbType.Date, DateTime.Now);
                        cmd.Parameters.AddWithValue("@request_text", query1);
                        var a = cmd.ExecuteScalar();
                        user.datalog = Convert.ToInt32(a);
                    }

                    conn.Close();
                    conn.Open();

                    using (NpgsqlCommand cmd = new(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userid", id);
                        cmd.Parameters.AddWithValue("@firstname", user.firstname);
                        cmd.Parameters.AddWithValue("@lastname", user.lastname);
                        cmd.Parameters.AddWithValue("@birthday", NpgsqlDbType.Date, user.birthday);
                        cmd.Parameters.AddWithValue("@pinfl", user.pinfl);
                        cmd.Parameters.AddWithValue("@login", user.login);
                        cmd.Parameters.AddWithValue("@password", user.password);
                        cmd.Parameters.AddWithValue("@role", (int)user.role);
                        cmd.Parameters.AddWithValue("@organizationtype", (int)user.organizationtype);
                        cmd.Parameters.AddWithValue("@status", (int)user.status);
                        cmd.Parameters.AddWithValue("@datalog", (int)user.datalog);
                        var a = cmd.ExecuteNonQuery();
                    }
                }
                catch { }
                finally { conn.Close(); }
            }
        }
        public void DeleteUser(int id, int loginiduser)
        {

            try
            {
                int datalog;
                conn.Open();
                string query = $"UPDATE users SET status=@status, datalog=@datlog WHERE userid=@userid;";


                string query1 = $"UPDATE users SET status={StatusEnum.deleted} WHERE userid={id};";
                string logquery = "INSERT INTO data_log VALUES (DEFAULT,@user_id,@executeon_date,@request_text) RETURNING data_log_id;";
                using (NpgsqlCommand cmd = new(logquery, conn))
                {
                    cmd.Parameters.AddWithValue("@user_id", loginiduser);
                    cmd.Parameters.AddWithValue("@executeon_date", NpgsqlDbType.Date, DateTime.Now);
                    cmd.Parameters.AddWithValue("@request_text", query1);
                    var a = cmd.ExecuteScalar();
                    datalog = Convert.ToInt32(a);
                }

                conn.Close();
                conn.Open();

                using (NpgsqlCommand cmd = new(query, conn))
                {
                    cmd.Parameters.AddWithValue("@userid", id);
                    cmd.Parameters.AddWithValue("@status", (int)StatusEnum.deleted);
                    cmd.Parameters.AddWithValue("@datlog", datalog);
                    var a = cmd.ExecuteNonQuery();
                }
            }
            catch { }
            finally { conn.Close(); }
        }
        public User GetByIdUser(int id)
        {
            DataTable table = null;
            User user = null;
            string query = "SELECT * FROM users where userid=@userid;";

            try
            {
                conn.Open();

                using (NpgsqlCommand cmd = new(query, conn))
                {
                    cmd.Parameters.AddWithValue("@userid", id);
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
                    user = new(row);
                }
            }

            return user;
        }
        public IEnumerable<User> GetAllUser()
        {
            List<User> users = new();
            DataTable table = null;

            string query = "SELECT * FROM users WHERE status!=2;";

            try
            {
                conn.Open();

                using (NpgsqlCommand cmd = new(query, conn))
                {
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
                    User user = new(row);
                    users.Add(user);
                }
            }
            return users;
        }
    }
}
