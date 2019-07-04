using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace WorkTimeManager.Models
{
    public class User
    {
        public string Login { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Position { get; set; }
        public float Salary { get; set; }
        public int Hours { get; set; }
        public string Password { get; set; }
		
        public User(string login, string position)
        {
            Login = login;
            Position = position;
        }

        public User(string login, string firstName, string lastName, string email, string phoneNumber, string address, string position, float salary, int hours, string password)
        {
            Login = login;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            Address = address;
            Position = position;
            Salary = salary;
            Hours = hours;
            Password = password;
        }

        public User(MySqlDataReader reader)
        {
            Login = reader["login"].ToString();
            FirstName = reader["firstName"].ToString();
            LastName = reader["lastName"].ToString();
            Email = reader["emailAddress"].ToString();
            PhoneNumber = reader["phoneNumber"].ToString();
            Address = reader["address"].ToString();
            Position = reader["position"].ToString();
            Salary = (float)reader["salary"];
            Hours = Convert.ToInt32(reader["hours"]);
            Password = reader["password"].ToString();
        }

        public override string ToString()
        {
            return $"{Login} {FirstName} {LastName} {Email} {PhoneNumber} {Address} {Position} {Salary} {Hours}";
        }
    }
}
