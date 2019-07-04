using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using WorkTimeManager.Models;
using WorkTimeManager.DBAccess;
using MySql.Data.MySqlClient;


namespace WorkTimeManager.ViewModels
{
    class CustomersManagementVM : INotifyPropertyChanged
    {
        #region Fields
        private string _NIP;
        private string _name;
        private string _email;
        private string _phoneNumber;
        private string _address;
        private int _selectedIndex;
        private string selectedNIP;
        private Customer _selectedCustomer;
        private List<string> _customerStringList = new List<string>();
        private List<Customer> _customerObjectsList = new List<Customer>();

        private string _message;
        #endregion

        #region Properties
        public string NIP
        {
            get { return _NIP; }
            set
            {
                _NIP = value;
                OnPropertyChanged("NIP");
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
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

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                _selectedIndex = value;
                OnPropertyChanged("SelectedIndex");
                _selectedCustomer = CustomerObjectList[_selectedIndex];
                selectedNIP = _selectedCustomer.NIP;
                SelectCustomer(this);
            }
        }

        public List<string> CustomerStringList
        {
            get { return _customerStringList; }
            set
            {
                _customerStringList = value;
                OnPropertyChanged("CustomerStringList");
            }
        }

        public List<Customer> CustomerObjectList
        {
            get { return _customerObjectsList; }
            set
            {
                _customerObjectsList = value;
                OnPropertyChanged("CustomerObjectList");
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
        public ICommand AddCustomerCommand { get; private set; }
        public ICommand UpdateCustomerCommand { get; private set; }
        public ICommand ClearFormCommand { get; private set; }
        public ICommand SelectCustomerCommand { get; private set; }
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        public CustomersManagementVM()
        {
            AddCustomerCommand = new RelayCommand(AddCustomer);
            ClearFormCommand = new RelayCommand(ClearForm);
            UpdateCustomerCommand = new RelayCommand(UpdateCustomer);
            SelectCustomerCommand = new RelayCommand(SelectCustomer);
            LoadCustomersFromDatabase();
            RefreshCustomerList();
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void LoadCustomersFromDatabase()
        {
            CommandsRepository.GET_ALL_CUSTOMERS();
            CustomerObjectList = CommandsRepository.CustomersList;
        }

        private void AddCustomer(object obj)
        {
            try
            {
                Customer customer = new Customer(NIP, Name, Email, PhoneNumber, Address);
                if (Validator.IsCustomerValid(customer))
                {
                    if (CommandsRepository.ADD_CUSTOMER(customer))
                    {
                        CustomerObjectList.Add(customer);
                        RefreshCustomerList();
                        Message = "";
                    }
                    else
                    {
                        Message = "Nie można dodać klienta";
                    }
                }
                else
                {
                    Message = "Niektóre pola są niepoprawne";
                }
            }
            catch (Exception)
            {
                Message = "Nie można dodać klienta";
            }
        }

        private void UpdateCustomer(object obj)
        {
            if (IsNIPCorrect())
            {
                _selectedCustomer.NIP = NIP;
                _selectedCustomer.Name = Name;
                _selectedCustomer.Email = Email;
                _selectedCustomer.PhoneNumber = PhoneNumber;
                _selectedCustomer.Address = Address;

                if (Validator.IsCustomerValid(_selectedCustomer))
                {
                    if (CommandsRepository.UPDATE_CUSTOMER(_selectedCustomer, selectedNIP))
                    {
                        RefreshCustomerList();
                        Message = "";
                    }
                    else
                    {
                        Message = "Nie można zaktualizować klienta";
                    }
                }
                else
                {
                    Message = "Niektóre pola są niepoprawne";
                }
            }
            else
            {
                Message = "Nie można zaktualizować klienta";
            }
        }

        private bool IsNIPCorrect()
        {
            if (NIP == selectedNIP) return true;
            return false;
        }

        private void SelectCustomer(object obj)
        {
            NIP = _selectedCustomer.NIP;
            Name = _selectedCustomer.Name;
            Email = _selectedCustomer.Email;
            PhoneNumber = _selectedCustomer.PhoneNumber;
            Address = _selectedCustomer.Address;
        }

        private void ClearForm(object obj)
        {
            NIP = "";
            Name = "";
            Email = "";
            PhoneNumber = "";
            Address = "";
        }

        private void RefreshCustomerList()
        {
            List<Customer> tmp = new List<Customer>();
            foreach (Customer customer in CustomerObjectList)
            {
                tmp.Add(customer);
            }
            CustomerObjectList = tmp;
        }
    }
}
