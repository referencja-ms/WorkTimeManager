using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkTimeManager.Models;
using MySql.Data.MySqlClient;

namespace WorkTimeManager.ViewModels
{
    class MainWindowVM
    {
        #region Properties
        public string Login { private get; set; }
        public string Password { private get; set; }
        #endregion
        #region Commands
        public ICommand LoginButtonClickedCommand { get; private set; }
        #endregion

        public MainWindowVM()
        {
            LoginButtonClickedCommand = new RelayCommand(LogIn);
        }
        private void LogIn(object obj) //Logowanie użytkownika - wyświetlenie odpowiedniego okna
        {
            new ProgrammerWindow().Show();
            try
            {
                MySqlConnection conn = DBAccess.DBConnection.Instance.Connection;
                conn.Open();
                MySqlCommand getUser = conn.CreateCommand();
                getUser.CommandType = System.Data.CommandType.Text;
                getUser.CommandText = "SELECT login FROM users WHERE login=@UserLogin AND password=@UserPassword";
                getUser.Parameters.Add(new MySqlParameter("@UserLogin", Login));
                getUser.Parameters.Add(new MySqlParameter("@UserPassword", Password.ToString()));
                Console.WriteLine(Login);
                Console.WriteLine(Password.ToString());
                object result = getUser.ExecuteScalar();
                if (result != null)
                    DBAccess.DBConnection.Instance.LoggedUser = new User(Login);
                else
                    Console.WriteLine("Nie zalogowano");
                conn.Close();
            }
            catch (MySql.Data.MySqlClient.MySqlException)
                { Console.WriteLine("Błąd połączenia"); }
        }

    }
}
