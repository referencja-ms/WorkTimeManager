using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkTimeManager.Models;

namespace WorkTimeManager.ViewModels {
    public class ProgrammerVM : INotifyPropertyChanged {
        #region Private fields
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
        public ICommand SelectionChanged { get; private set; }
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
            }
        }
        #endregion
        public ProgrammerVM() {
            _projects = new List<string>();
            _projectDetails = new List<string>();
            SelectionChanged = new RelayCommand(LoadProjectInformations);
            AddRecord = new RelayCommand(AddNewRecord);
            //Projects = CommandRepository.GetUserProjects(CurrentUser.Name);

        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        private void LoadProjectInformations(object obj) {
            //ProjectDetails = CommandRepository.GetProjectDetails(project, stanowisko);
            //ProjectHistory = CommandRepository.GetProjectHistoryForUser(project, user);
            //Colleagues = CommandRepository.GetProjectColleagues(project);
        }
        private void AddNewRecord(object obj) {

        }
    }
}
