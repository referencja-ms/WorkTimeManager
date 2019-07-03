using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace WorkTimeManager.DBAccess {
    public static class CommandsRepository {

        static MySqlConnection conn = DBConnection.Instance.Connection;

        //logowanie użytkownika
        public static string GET_LOGIN_WITH_PASSWORD(string login, string password) {
            try {
                conn.Open();
                //utworzenie zapytania do bazy o pozycję użytkownika
                MySqlCommand command = conn.CreateCommand();
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "SELECT position FROM users WHERE login=@UserLogin AND password=@UserPassword";
                command.Parameters.Add(new MySqlParameter("@UserLogin", login));
                command.Parameters.Add(new MySqlParameter("@UserPassword", password));

                //sprawdzenie, czy baza zwróciła cokolwiek
                object result = command.ExecuteScalar();
                //jeśli zwróciła
                if (result != null) {
                    var reader = command.ExecuteReader();
                    reader.Read();
                    Console.WriteLine(reader["position"].ToString());
                    DBConnection.LoggedUser = new Models.User(login, reader["position"].ToString());
                    return reader["position"].ToString();
                }
                //jeśli nie zwróciła - brak użytkownika w bazie
                else
                    return "no-user";

            }
            //obsługa błędu połączenia z bazą
            catch (MySqlException) {
                return "no-connection";
            }
            finally {
                conn.Close();
            }
        }
        public static List<string> GetUserProjects(string login) {
            List<Models.Project> projects = new List<Models.Project>();
            List<string> returanble = new List<string>();
            using (MySqlCommand command = conn.CreateCommand()) {
                conn.Open();
                command.CommandText = "SELECT name from projects, worktasks WHERE id=projectid and user=@parameter1";
                command.Parameters.AddWithValue("@parameter1", login);
                MySqlDataReader dr = command.ExecuteReader();
                while (dr.Read()) {
                    projects.Add(new Models.Project(dr));
                }
                conn.Close();
                foreach (var d in projects) {
                    returanble.Add(d.ToString());
                }
            }
            return returanble;
        }
        public static List<string> GetProjectDetails(string projectName, string position) {
            List<string> returnable = new List<string>();
            using (MySqlCommand command = conn.CreateCommand()) {
                conn.Open();
                command.CommandText = "Select id, status, budget, deadline, description from projects where name=@parameter1";
                command.Parameters.AddWithValue("@parameter1", projectName);
                MySqlDataReader dr = command.ExecuteReader();
                while (dr.Read()) {
                    returnable.Add("Id: " + dr["id"].ToString());
                    returnable.Add("Status: " + dr["status"].ToString());
                    returnable.Add("Budżet: " + dr["budget"].ToString());
                    returnable.Add("Deadline: " + dr["deadline"].ToString());
                    returnable.Add("Opis: " + dr["description"].ToString());
                }
                conn.Close();
            }
            return returnable;
        }
        public static List<string> GetProjectHistoryForUser(string login, string project) {
            List<string> returnable = new List<string>();
            using (MySqlCommand command = conn.CreateCommand()) {
                conn.Open();
                command.CommandText = "select registryDate, hours from registry where user=@parameter1 and projectid=(select id from projects where name=@parameter2)";
                command.Parameters.AddWithValue("@parameter1", login);
                command.Parameters.AddWithValue("@parameter2", project);
                MySqlDataReader dr = command.ExecuteReader();
                while (dr.Read()) {
                    returnable.Add("Data: "+dr["registrydate"].ToString() + " Godziny: " + dr["hours"]);
                }
                conn.Close();
            }
            return returnable;
        }
        public static List<string> GetProjectColleagues(string project) {
            List<string> returnable = new List<string>();
            using (MySqlCommand command = conn.CreateCommand()) {
                conn.Open();
                //command.CommandText = "select user from worktasks where projectid=(select id from projects where name=@parameter2)";
                command.CommandText = "select firstname, lastname from users, worktasks where worktasks.projectid=(select id from projects where name=@parameter2) and login=worktasks.user";
                command.Parameters.AddWithValue("@parameter2", project);
                MySqlDataReader dr = command.ExecuteReader();
                while (dr.Read()) {
                    returnable.Add(dr["firstname"].ToString()+" "+dr["lastname"].ToString());
                }
                conn.Close();
            }
            return returnable;
        }
        public static void AddRegistryNote(DateTime dt, string name, string project, string time) {
            int id = 0;
            using (MySqlCommand command = conn.CreateCommand()) {
                conn.Open();
                command.CommandText = "select id from projects where name=@parameter";
                command.Parameters.AddWithValue("@parameter", project);
                MySqlDataReader dr = command.ExecuteReader();
                while (dr.Read()) {
                    id = Int32.Parse(dr["id"].ToString());
                }
                conn.Close();
            }
            using (MySqlCommand command = conn.CreateCommand()) {
                conn.Open();
                command.CommandText = "INSERT into registry value(@dt, @name, @project, @time)";
                command.Parameters.AddWithValue("@dt", dt.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@project", id.ToString());
                command.Parameters.AddWithValue("@time", time);
                command.ExecuteNonQuery();
                conn.Close();
            }
        }
    }
}
