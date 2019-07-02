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

    }
}
