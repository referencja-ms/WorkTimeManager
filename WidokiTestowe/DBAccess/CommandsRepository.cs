using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace WorkTimeManager.DBAccess
{
    public static class CommandsRepository
    {

        static MySqlConnection conn = DBConnection.Instance.Connection;

        //logowanie użytkownika
        public static string GET_LOGIN_WITH_PASSWORD(string login, string password)
        {
            try
            {
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
                if (result != null)
                {
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
            catch (MySqlException)
            {
                return "no-connection";
            }
            finally
            {
                conn.Close();
            }
        }
        public static List<string> GetUserProjects(string login) {
            List<Models.Project> projects = new List<Models.Project>();
            List<string> returanble = new List<string>();
            using(MySqlCommand command = conn.CreateCommand()) {
                conn.Open();
                command.CommandText = "SELECT name from projects, worktasks WHERE id=projectid and user=@parameter1";
                command.Parameters.AddWithValue("@parameter1", login);
                MySqlDataReader dr = command.ExecuteReader();
                while (dr.Read()) {
                    projects.Add(new Models.Project(dr));
                }
                conn.Close();
                foreach(var d in projects) {
                    returanble.Add(d.ToString());
                }
            }
            return returanble;
        }
    }
}
