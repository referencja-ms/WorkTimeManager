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
        public static MySqlCommand GET_LOGIN_WITH_PASSWORD(string login, string password)
        {
            MySqlCommand command = new MySqlCommand();
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = "SELECT login FROM users WHERE login=@UserLogin AND password=@UserPassword";
            command.Parameters.Add(new MySqlParameter("@UserLogin", login));
            command.Parameters.Add(new MySqlParameter("@UserPassword", password));
            return command;
        }
        //Pobranie wszystkich projektów zalogowanego użytkownika (id, name)
        public static MySqlCommand GET_ALL_USERS_PROJECTS(string login)
        {
            MySqlCommand command = new MySqlCommand();
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = "SELECT id, name FROM projects, workTasks WHERE id = projectID AND user = @parametr1";
            command.Parameters.Add(new MySqlParameter("@parametr1", login));
            return command;
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
