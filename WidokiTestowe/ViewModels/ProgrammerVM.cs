using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using WorkTimeManager.Models;

namespace WorkTimeManager.ViewModels {
    public class ProgrammerVM : INotifyPropertyChanged {
        #region Private fields
        private string _errorMessage;
        private List<string> _projects;
        private List<string> _projectDetails;
        private List<string> _projectHistory;
        private List<string> _colleagues;
        private string _hours = "";
        private int _selectedIndex;
        #endregion
        #region Properties
        public List<string> Projects {
            get { return _projects; }
            set {
                _projects = value;
                OnPropertyChanged("Projects");
            }
        }
        public List<string> ProjectDetails {
            get {
                return _projectDetails;
            }
            set {
                _projectDetails = value;
                OnPropertyChanged("ProjectDetails");
            }
        }
        public List<string> ProjectHistory {
            get {
                return _projectHistory;
            }
            set {
                _projectHistory = value;
                OnPropertyChanged("ProjectHistory");
            }
        }
        public List<string> Colleagues {
            get {
                return _colleagues;
            }
            set {
                _colleagues = value;
                OnPropertyChanged("Colleagues");
            }
        }
        public ICommand AddRecord { get; private set; }
        public string Hours {
            get {
                return _hours;
            }
            set {
                _hours = value;
                OnPropertyChanged("Hours");
            }
        }
        public int SelectedIndex {
            get {
                return _selectedIndex;
            }
            set {
                _selectedIndex = value;
                OnPropertyChanged("SelectedIndex");
                LoadProjectInformations(this);
            }
        }
        public string Text { get; set; }
        public string ErrorMessage {
            get { return _errorMessage; }
            private set {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }
        #endregion
        public ProgrammerVM() {
            _projects = new List<string>();
            _projectDetails = new List<string>();
            _projectHistory = new List<string>();
            _colleagues = new List<string>();
            SelectedIndex = -1;
            AddRecord = new RelayCommand(AddNewRecord);
            Projects = DBAccess.CommandsRepository.GetUserProjects(DBAccess.DBConnection.LoggedUser.Login);
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        private void LoadProjectInformations(object sender) {
            if (SelectedIndex >= 0) {
                ProjectDetails = DBAccess.CommandsRepository.GetProjectDetails(Projects[SelectedIndex], DBAccess.DBConnection.LoggedUser.Position);
                ProjectHistory = DBAccess.CommandsRepository.GetProjectHistoryForUser(DBAccess.DBConnection.LoggedUser.Login, Projects[SelectedIndex]);
                Colleagues = DBAccess.CommandsRepository.GetProjectColleagues(Projects[SelectedIndex]);
            }
        }
        private void AddNewRecord(object obj) {
            Regex rg = new Regex(@"^\d$");
            if (rg.IsMatch(Text)&&Int32.Parse(Text)>0) {
                DBAccess.CommandsRepository.AddRegistryNote(DateTime.Now, DBAccess.DBConnection.LoggedUser.Login, Projects[SelectedIndex], Text);
                LoadProjectInformations(this);
            }
            else
                ErrorMessage = "Proszę podać cyfrę!";
        }
    }
}
