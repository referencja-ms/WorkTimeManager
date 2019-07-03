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
                    DBConnection.Instance.LoggedUser = new Models.User(login, reader["position"].ToString());
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

        public static List<string> GET_PROJECTS_WITHOUT_MANAGER()
        {
            List<string> result = new List<string>();
            try
            {
                conn.Open();
                //utworzenie zapytania do bazy o projekty bez projekt menadżera
                MySqlCommand command = conn.CreateCommand();
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "SELECT DISTINCT p.name FROM worktasks w,projects p WHERE p.id=w.projectid and projectID not in (SELECT DISTINCT projectID FROM worktasks WHERE roleID = \"Menedżer projektu\"); ";

                //sprawdzenie, czy baza zwróciła cokolwiek
                object isNotEmpty = command.ExecuteScalar();
                
                //jeśli zwróciła
                if (isNotEmpty != null)
                {
                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        result.Add(reader["name"].ToString());
                    }
                    return result;               
                }
                //jeśli nie zwróciła - wszystkie projekty mają menadżera
                else
                {
                    result.Add("Brak");
                    return result;
                }

            }
            //obsługa błędu połączenia z bazą
            catch (MySqlException)
            {
                result.Add("Brak połączenia");
                return result;
            }
            finally
            {
                conn.Close();
            }
        }

        public static List<string> GET_PROGRAMER_WITH_NO_LEADED_PROJECT()
        {
            List<string> result = new List<string>();
            List<Models.UsersList> temp = new List<Models.UsersList>();
            try
            {
                conn.Open();
                //utworzenie zapytania do bazy o pracowników nie będących menadzerami i nie szef i nie admin
                MySqlCommand command = conn.CreateCommand();
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "SELECT distinct concat(firstname,\" \",lastname) from users, worktasks WHERE users.login=worktasks.user and position!=\"szef\" and position!=\"admin\" and worktasks.roleID != \"Menedżer projektu\"; ";
                
                //sprawdzenie, czy baza zwróciła cokolwiek
                object isNotEmpty = command.ExecuteScalar();

                //jeśli zwróciła
                if (isNotEmpty != null)
                {
                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        temp.Add(new Models.UsersList(reader));
                    }
                    foreach (var item in temp)
                    {
                        result.Add(item.ToString());
                    }

                    return result;
                }
                //jeśli nie zwróciła - wszystkie projekty mają kierownika
                else
                {
                    result.Add("Brak dostępnych pracowników");
                    return result;
                }
            }
            //obsługa błędu połączenia z bazą
            catch (MySqlException)
            {
                result.Add("Brak połączenia");
                return result;
            }
            finally
            {
                conn.Close();
            }
        }

        public static List<string> GET_PROJECTS_TO_JUDGE()
        {
            List<string> result = new List<string>();
            try
            {
                conn.Open();
                //utworzenie zapytania do bazy o projekty bez projekt menadżera
                MySqlCommand command = conn.CreateCommand();
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "SELECT name FROM projects where status=\"done\"; ";

                //sprawdzenie, czy baza zwróciła cokolwiek
                object isNotEmpty = command.ExecuteScalar();

                //jeśli zwróciła
                if (isNotEmpty != null)
                {
                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        result.Add(reader["name"].ToString());
                    }
                    return result;
                }
                //jeśli nie zwróciła - wszystkie projekty mają menadżera
                else
                {
                    result.Add("Brak");
                    return result;
                }

            }
            //obsługa błędu połączenia z bazą
            catch (MySqlException)
            {
                result.Add("Brak połączenia");
                return result;
            }
            finally
            {
                conn.Close();
            }
        }

        public static void CHANGE_STATUS_TO_ACCEPTED(string projectn, int id)
        {
            if ((id >= 0) &&(projectn!="Brak"))
            {
                    conn.Open();
                    MySqlCommand command = conn.CreateCommand();
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = "update projects set status=\"Accepted\" where name=@npro;";
                    command.Parameters.Add(new MySqlParameter("@npro", projectn));
                    command.ExecuteReader();
                    conn.Close();
            }
        }

        public static void CHANGE_STATUS_TO_INPROGRESS(string projectn, int id)
        {
            if ((id >= 0) && (projectn != "Brak"))
            {
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "update projects set status=\"In progress\" where name=@npro;";
                command.Parameters.Add(new MySqlParameter("@npro", projectn));
                command.ExecuteReader();
                conn.Close();
            }
        }
        public static void ADD_MANAGER(string projectn, int programerid, int projectid)
        {
            if ((programerid >= 0) && (projectid>=0) && (projectn != "Brak"))
            {
                try
                {
                    conn.Open();
                    MySqlCommand command = conn.CreateCommand();
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = "insert into worktasks values (@user, @idpro, \"Menedżer projektu\")";
                    command.Parameters.Add(new MySqlParameter("@user", projectn));
                    command.Parameters.Add(new MySqlParameter("@idpro", programerid));
                    command.ExecuteReader();
                    Console.WriteLine("dodawanko");
                }
                catch
                {
                    MySqlCommand command = conn.CreateCommand();
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = "update worktasks set roleid=\"Menedżer projektu\" where user=@idprog and projectid=@idproj";
                    command.Parameters.Add(new MySqlParameter("@idprog", programerid));
                    command.Parameters.Add(new MySqlParameter("@idproj", projectid));
                    command.ExecuteReader();
                    Console.WriteLine("zamienianko");
                }
                
                conn.Close();
            }
        }
    }
}
