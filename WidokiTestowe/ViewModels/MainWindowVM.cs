using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkTimeManager.Models;
using MySql.Data.MySqlClient;
using System.ComponentModel;

namespace WorkTimeManager.ViewModels
{
    class MainWindowVM : INotifyPropertyChanged
    {
        private string _errortext;
        #region Properties
        public string Login { private get; set; }
        public string Password { private get; set; }
        public string ErrorText
        {
            get
            {
                return _errortext;
            }
            private set
            {
                _errortext = value;
                Console.WriteLine("Wchodzę do ErrorString: " + _errortext);
                OnPropertyChanged("ErrorText");
            }
        }
        #endregion
        #region Commands
        public ICommand LoginButtonClickedCommand { get; private set; }
        #endregion

        public MainWindowVM()
        {
            LoginButtonClickedCommand = new RelayCommand(LogIn);
        }

        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        private void LogIn(object obj) //Logowanie użytkownika - wyświetlenie odpowiedniego okna
        {
            string result = DBAccess.CommandsRepository.GET_LOGIN_WITH_PASSWORD(Login, Password);
            if (result == "no-user")
            {
                ErrorText="Brak użytkownika w bazie";
                Console.WriteLine("Brak użytkownika");
            }
            else if (result == "no-connection")
            {
                Console.WriteLine("Błąd połączenia");
            }
            else
            {
                Console.WriteLine("Zalogowano");
                Console.WriteLine(DBAccess.DBConnection.Instance.LoggedUser.Login);
                Password = "";
            }

        }

    }
}
