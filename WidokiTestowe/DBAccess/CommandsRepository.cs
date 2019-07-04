using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using WorkTimeManager.Models;

namespace WorkTimeManager.DBAccess
{
    public static class CommandsRepository
    {
        static MySqlConnection conn = DBConnection.Instance.Connection;

        #region Properties
        public static List<User> UsersList { get; set; } = new List<User>();
        public static List<Customer> CustomersList { get; set; } = new List<Customer>();
        public static List<Project> ProjectsList { get; set; } = new List<Project>();
        #endregion

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
                        ProjectsList.Add(new Project(reader));
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
                command.CommandText = "UPDATE projects SET " +
                    "name = @name, status = @status, budget = @budget, timeBudget = @timeBudget, " +
                    "deadline = @deadline, description = @description, customerNIP = @customerNIP " +
                    "WHERE id = @id;";
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