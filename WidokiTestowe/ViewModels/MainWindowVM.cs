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
        public event Action CloseMainWindow;
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
                ErrorText="Błędny login lub hasło";
            }
            else if (result == "no-connection")
            {
                ErrorText="Błąd połączenia. Spróbuj ponownie później lub skontaktuj się z administratorem.";
            }
            else
            {
                //Wyświetlenie odpowiednich okien po zalogowaniu
                Console.WriteLine("Zalogowano");
                Password = "";
                switch (DBAccess.DBConnection.LoggedUser.Position)
                {
                    case "admin":
                        new AdminWindow().Show();
                        break;
                    case "szef":
                        new BossWindow().Show();
                        break;
                    default:
                        new ProgrammerWindow().Show();
                        break;
                }
                //Wywołanie eventu, który ma zamknąć okno
                CloseMainWindow?.Invoke();
            }

        }

    }
}
