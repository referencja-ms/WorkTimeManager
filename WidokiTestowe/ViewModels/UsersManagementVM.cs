using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using WorkTimeManager.Models;
using WorkTimeManager.DBAccess;

namespace WorkTimeManager.ViewModels
{
    class UsersManagementVM : INotifyPropertyChanged
    {
        #region Fields
        private string _login { get; set; }
        private string _firstName { get; set; }
        private string _lastName { get; set; }
        private string _emailAddress { get; set; }
        private string _phoneNumber { get; set; }
        private string _address { get; set; }
        private string _position { get; set; }
        private float _salary { get; set; }
        private int _hours { get; set; }
        private string _password { get; set; }
        private int _selectedIndex { get; set; }
        private string _selectedLogin { get; set; }
        private User _selectedUser;
        private List<string> _userStringList = new List<string>();
        private List<User> _userObjectList = new List<User>();
        private string _message;
        #endregion

        #region Properties
        public string Login
        {
            get { return _login; }
            set
            {
                _login = value;
                OnPropertyChanged("Login");
            }
        }

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                OnPropertyChanged("FirstName");
            }
        }

        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                OnPropertyChanged("LastName");
            }
        }

        public string Email
        {
            get { return _emailAddress; }
            set
            {
                _emailAddress = value;
                OnPropertyChanged("Email");
            }
        }

        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set
            {
                _phoneNumber = value;
                OnPropertyChanged("PhoneNumber");
            }
        }

        public string Address
        {
            get { return _address; }
            set
            {
                _address = value;
                OnPropertyChanged("Address");
            }
        }


        public string Position
        {
            get { return _position; }
            set
            {
                _position = value;
                OnPropertyChanged("Position");
            }
        }


        public float Salary
        {
            get { return _salary; }
            set
            {
                _salary = value;
                OnPropertyChanged("Salary");
            }
        }

        public int Hours
        {
            get { return _hours; }
            set
            {
                _hours = value;
                OnPropertyChanged("Hours");
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged("Password");
            }
        }


        public List<string> UserStringList
        {
            get { return _userStringList; }
            set
            {
                _userStringList = value;
                OnPropertyChanged("UserStringList");
            }
        }

        public List<User> UserObjectList
        {
            get { return _userObjectList; }
            set
            {
                _userObjectList = value;
                OnPropertyChanged("UserObjectList");
            }
        }

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                _selectedIndex = value;
                OnPropertyChanged("SelectedIndex");
                _selectedUser = UserObjectList[_selectedIndex];
                _selectedLogin = _selectedUser.Login;
                SelectUser(this);
            }
        }

        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                OnPropertyChanged("Message");
            }
        }
        #endregion

        #region Commands
        public ICommand AddUserCommand { get; private set; }
        public ICommand UpdateUserCommand { get; private set; }
        public ICommand ClearFormCommand { get; private set; }
        public ICommand SelectUserCommand { get; private set; }
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        public UsersManagementVM()
        {
            AddUserCommand = new RelayCommand(AddUser);
            ClearFormCommand = new RelayCommand(ClearForm);
            UpdateUserCommand = new RelayCommand(UpdateUser);
            LoadUsersFromDatabase();
            RefreshUserList();
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void LoadUsersFromDatabase()
        {
            CommandsRepository.GET_ALL_USERS();
            UserObjectList = CommandsRepository.UsersList;
        }

        private void AddUser(object obj)
        {
            try
            {
                User user = new User(Login, FirstName, LastName, Email, PhoneNumber, Address, Position, Salary, Hours, Password);
                if (Validator.IsUserValid(user))
                {
                    if (CommandsRepository.ADD_USER(user))
                    {
                        UserObjectList.Add(user);
                        RefreshUserList();
                        Message = "";
                    }
                    else
                    {
                        Message = "Nie można dodać użytkownika";
                    }
                }
                else
                {
                    Message = "Niektóre pola są niepoprawne";
                }
            }
            catch (Exception)
            {
                Message = "Nie można dodać użytkownika";
            }
        }

        private void UpdateUser(object obj)
        {
            if (IsLoginCorrect())
            {
                _selectedUser.Login = Login;
                _selectedUser.FirstName = FirstName;
                _selectedUser.LastName = LastName;
                _selectedUser.Email = Email;
                _selectedUser.PhoneNumber = PhoneNumber;
                _selectedUser.Address = Address;
                _selectedUser.Position = Position;
                _selectedUser.Salary = Salary;
                _selectedUser.Hours = Hours;
                _selectedUser.Password = Password;
                if (Validator.IsUserValid(_selectedUser))
                {
                    if (CommandsRepository.UPDATE_USER(_selectedUser))
                    {
                        RefreshUserList();
                        Message = "";
                    }
                    else
                    {
                        Message = "Nie można zaktualizować użytkownika";
                    }
                }
                else
                {
                    Message = "Nie można zaktualizować użytkownika";
                }
            }
            else
            {
                Message = "Nie można zaktualizować użytkownika";
            }
        }

        private void SelectUser(object obj)
        {
            Login = _selectedUser.Login;
            FirstName = _selectedUser.FirstName;
            LastName = _selectedUser.LastName;
            Email = _selectedUser.Email;
            PhoneNumber = _selectedUser.PhoneNumber;
            Address = _selectedUser.Address;
            Position = _selectedUser.Position;
            Salary = _selectedUser.Salary;
            Hours = _selectedUser.Hours;
            Password = _selectedUser.Password;
        }

        private void ClearForm(object obj)
        {
            Login = "";
            FirstName = "";  
            LastName = "";   
            Email = "";      
            PhoneNumber = "";
            Address = "";    
            Position = "";
            Salary = 0;
            Hours = 0;       
            Password = "";
        }
        
        private bool IsLoginCorrect()
        {
            if (_selectedLogin == Login) return true;
            return false;
        }

        private void RefreshUserList()
        {
            List<User> tmp = new List<User>();
            foreach (User user in UserObjectList)
            {
                tmp.Add(user);
            }
            UserObjectList = tmp;
        }
    }
}

