using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using WorkTimeManager.Models;

namespace WorkTimeManager.DBAccess {
    public static class CommandsRepository {

        static MySqlConnection conn = DBConnection.Instance.Connection;
		
		#region Properties
        public static List<User> UsersList { get; set; } = new List<User>();
        public static List<Customer> CustomersList { get; set; } = new List<Customer>();
        public static List<Project> ProjectsList { get; set; } = new List<Project>();
        #endregion
		
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
		
		
		//Admin commands
		#region Users commands
        public static string GET_ALL_USERS()
        {
            try
            {
                conn.Open();
                //utworzenie zapytania do bazy o pozycję użytkownika
                MySqlCommand command = conn.CreateCommand();
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "SELECT * FROM users";

                //sprawdzenie, czy baza zwróciła cokolwiek
                object result = command.ExecuteScalar();
                //jeśli zwróciła
                if (result != null)
                {
                    var reader = command.ExecuteReader();
                    reader.Read();
                    while (reader.Read())
                    {
                        UsersList.Add(new User(reader));
                    }
                    return "projects-loaded";
                }
                //jeśli nie zwróciła - brak klientów w bazie
                else
                    return "no-projects";

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

        public static bool ADD_USER(User user)
        {
            try
            {
                conn.Open();
                //utworzenie zapytania do bazy o pozycję użytkownika
                MySqlCommand command = conn.CreateCommand();
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "INSERT INTO users (login, firstName, lastName, emailAddress, phoneNumber, address, position, salary, hours, password) VALUES (@login, @firstName, @lastName, @email, @phoneNumber, @address, @position, @salary, @hours, @password);";
                command.Parameters.Add(new MySqlParameter("@login", user.Login));
                command.Parameters.Add(new MySqlParameter("@firstName", user.FirstName));
                command.Parameters.Add(new MySqlParameter("@lastName", user.LastName));
                command.Parameters.Add(new MySqlParameter("@email", user.Email));
                command.Parameters.Add(new MySqlParameter("@phoneNumber", user.PhoneNumber));
                command.Parameters.Add(new MySqlParameter("@address", user.Address));
                command.Parameters.Add(new MySqlParameter("@position", user.Position));
                command.Parameters.Add(new MySqlParameter("@salary", user.Salary));
                command.Parameters.Add(new MySqlParameter("@hours", user.Hours));
                command.Parameters.Add(new MySqlParameter("@password", user.Password));

                command.ExecuteNonQuery();
            }
            //obsługa błędu połączenia z bazą
            catch (MySqlException exc)
            {
                Console.WriteLine(exc);
                conn.Close();
                return false;
            }
            conn.Close();
            return true;
        }

        public static bool UPDATE_USER(User user)
        {
            try
            {
                conn.Open();
                //utworzenie zapytania do bazy o pozycję użytkownika
                MySqlCommand command = conn.CreateCommand();
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "UPDATE users SET firstName = @firstName, lastName = @lastName, emailAddress = @email, phoneNumber = @phoneNumber, address = @address, position = @position, salary = @salary, hours = @hours, password = @password WHERE login = @login;";
                command.Parameters.Add(new MySqlParameter("@login", user.Login));
                command.Parameters.Add(new MySqlParameter("@firstName", user.FirstName));
                command.Parameters.Add(new MySqlParameter("@lastName", user.LastName));
                command.Parameters.Add(new MySqlParameter("@email", user.Email));
                command.Parameters.Add(new MySqlParameter("@phoneNumber", user.PhoneNumber));
                command.Parameters.Add(new MySqlParameter("@address", user.Address));
                command.Parameters.Add(new MySqlParameter("@position", user.Position));
                command.Parameters.Add(new MySqlParameter("@salary", user.Salary));
                command.Parameters.Add(new MySqlParameter("@hours", user.Hours));
                command.Parameters.Add(new MySqlParameter("@password", user.Password));

                command.ExecuteNonQuery();
            }
            //obsługa błędu połączenia z bazą
            catch (MySqlException exc)
            {
                Console.WriteLine(exc);
                conn.Close();
                return false;
            }
            conn.Close();
            return true;
        }
        #endregion

        #region Projects commands
        public static string GET_ALL_PROJECTS()
        {
            try
            {
                conn.Open();
                //utworzenie zapytania do bazy o pozycję użytkownika
                MySqlCommand command = conn.CreateCommand();
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "SELECT * FROM projects";

                //sprawdzenie, czy baza zwróciła cokolwiek
                object result = command.ExecuteScalar();
                //jeśli zwróciła
                if (result != null)
                {
                    var reader = command.ExecuteReader();
                    reader.Read();
                    while (reader.Read())
                    {
                        ProjectsList.Add(new Project(reader, true));
                    }
                    return "projects-loaded";
                }
                //jeśli nie zwróciła - brak klientów w bazie
                else
                    return "no-projects";

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

        public static bool ADD_PROJECT(Project project)
        {
            try
            {
                conn.Open();
                //utworzenie zapytania do bazy o pozycję użytkownika
                MySqlCommand command = conn.CreateCommand();
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "INSERT INTO projects (name, status, budget, timeBudget, deadline, description, customerNIP) VALUES (@name, @status, @budget, @timeBudget, @deadline, @description, @customerNIP);";
                command.Parameters.Add(new MySqlParameter("@name", project.Name));
                command.Parameters.Add(new MySqlParameter("@status", project.Status));
                command.Parameters.Add(new MySqlParameter("@budget", project.Budget));
                command.Parameters.Add(new MySqlParameter("@timeBudget", project.TimeBudget));
                command.Parameters.Add(new MySqlParameter("@deadline", project.Deadline));
                command.Parameters.Add(new MySqlParameter("@description", project.Description));
                command.Parameters.Add(new MySqlParameter("@customerNIP", project.CustomerNIP));

                command.ExecuteNonQuery();
            }
            //obsługa błędu połączenia z bazą
            catch (MySqlException exc)
            {
                Console.WriteLine(exc);
                conn.Close();
                return false;
            }
            conn.Close();
            return true;
        }

        public static bool UPDATE_PROJECT(Project project)
        {
            try
            {
                conn.Open();
                //utworzenie zapytania do bazy o pozycję użytkownika
                MySqlCommand command = conn.CreateCommand();
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "UPDATE projects SET name = @name, status = @status, budget = @budget, timeBudget = @timeBudget, deadline = @deadline, description = @description, customerNIP = @customerNIP  WHERE id = @id;";
                command.Parameters.Add(new MySqlParameter("@name", project.Name));
                command.Parameters.Add(new MySqlParameter("@status", project.Status));
                command.Parameters.Add(new MySqlParameter("@budget", project.Budget));
                command.Parameters.Add(new MySqlParameter("@timeBudget", project.TimeBudget));
                command.Parameters.Add(new MySqlParameter("@deadline", project.Deadline));
                command.Parameters.Add(new MySqlParameter("@description", project.Description));
                command.Parameters.Add(new MySqlParameter("@customerNIP", project.CustomerNIP));
                command.Parameters.Add(new MySqlParameter("@id", project.Id));

                command.ExecuteNonQuery();
            }
            //obsługa błędu połączenia z bazą
            catch (MySqlException exc)
            {
                Console.WriteLine(exc);
                conn.Close();
                return false;
            }
            conn.Close();
            return true;
        }
        #endregion

        #region Customers commands
        public static string GET_ALL_CUSTOMERS()
        {
            try
            {
                conn.Open();
                //utworzenie zapytania do bazy o pozycję użytkownika
                MySqlCommand command = conn.CreateCommand();
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "SELECT * FROM customers";

                //sprawdzenie, czy baza zwróciła cokolwiek
                object result = command.ExecuteScalar();
                //jeśli zwróciła
                if (result != null)
                {
                    var reader = command.ExecuteReader();
                    reader.Read();
                    while (reader.Read())
                    {
                        CustomersList.Add(new Customer(reader));
                    }
                    return "customers-loaded";
                }
                //jeśli nie zwróciła - brak klientów w bazie
                else
                    return "no-customer";

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

        public static bool ADD_CUSTOMER(Customer customer)
        {
            try
            {
                conn.Open();
                //utworzenie zapytania do bazy o pozycję użytkownika
                MySqlCommand command = conn.CreateCommand();
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "INSERT INTO customers (NIP, name, emailAddress, phoneNumber, address) VALUES (@NIP, @Name, @Email, @PhoneNumber, @Address);";
                command.Parameters.Add(new MySqlParameter("@NIP", customer.NIP));
                command.Parameters.Add(new MySqlParameter("@Name", customer.Name));
                command.Parameters.Add(new MySqlParameter("@Email", customer.Email));
                command.Parameters.Add(new MySqlParameter("@PhoneNumber", customer.PhoneNumber));
                command.Parameters.Add(new MySqlParameter("@Address", customer.Address));

                command.ExecuteNonQuery();
            }
            //obsługa błędu połączenia z bazą
            catch (MySqlException exc)
            {
                Console.WriteLine(exc);
                conn.Close();
                return false;
            }
            conn.Close();
            return true;
        }

        public static bool UPDATE_CUSTOMER(Customer customer, string selectedCustomerNIP)
        {
            try
            {
                conn.Open();
                //utworzenie zapytania do bazy o pozycję użytkownika
                MySqlCommand command = conn.CreateCommand();
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "UPDATE Customers SET name = @Name, emailAddress = @Email, phoneNumber = @PhoneNumber, address = @Address WHERE NIP = @NIP;";
                command.Parameters.Add(new MySqlParameter("@Name", customer.Name));
                command.Parameters.Add(new MySqlParameter("@Email", customer.Email));
                command.Parameters.Add(new MySqlParameter("@PhoneNumber", customer.PhoneNumber));
                command.Parameters.Add(new MySqlParameter("@Address", customer.Address));
                command.Parameters.Add(new MySqlParameter("@NIP", selectedCustomerNIP));

                command.ExecuteNonQuery();
            }
            //obsługa błędu połączenia z bazą
            catch (MySqlException exc)
            {
                Console.WriteLine(exc);
                conn.Close();
                return false;
            }
            conn.Close();
            return true;
        }
        #endregion
    }
}
