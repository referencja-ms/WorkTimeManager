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
            try
            {
                MySqlConnection conn = DBAccess.DBConnection.Instance.Connection;
                conn.Open();
                MySqlCommand getUser = DBAccess.CommandsRepository.GET_LOGIN_WITH_PASSWORD(Login, Password);
                getUser.Connection = conn;
                Console.WriteLine(Login);
                Console.WriteLine(Password.ToString());
                object result = getUser.ExecuteScalar();
                if (result != null) {
                    DBAccess.DBConnection.LoggedUser = new User(Login);
                    conn.Close();
                    new ProgrammerWindow().Show();
                }
                    
                else
                    Console.WriteLine("Nie zalogowano");
                conn.Close();
            }
            catch (MySql.Data.MySqlClient.MySqlException)
                { Console.WriteLine("Błąd połączenia"); }
        }

    }
}
