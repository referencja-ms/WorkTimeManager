using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkTimeManager.Models;

namespace WorkTimeManager.ViewModels
{
    public class BossVM : INotifyPropertyChanged
    {
        #region PrivateFields
        private List<string> _requestsList;
        private List<string> _projectsNoManager;
        private List<string> _projectsJudgeReady;
        private List<string> _availableProgrammers;
        private int _selectedIndex;
        private string _selectedRequestsList;
        private string _selectedProjectsNoManager;
        private string _selectedProjectsJudgeReady;
        private string _selectedAvailableProgrammers;
        #endregion
        #region Properties
        public List<string> RequestsList
        {
            get { return _requestsList; }
            set
            {
                _requestsList = value;
                OnPropertyChanged("RequestedList");
            }
        }

        public List<string> ProjectsNoManager
        {
            get { return _projectsNoManager; }
            set
            {
                _requestsList = value;
                OnPropertyChanged("ProjectsNoManager");
            }
        }

        public List<string> ProjectsJudgeReady
        {
            get { return _projectsJudgeReady; }
            set
            {
                _requestsList = value;
                OnPropertyChanged("ProjectsJudgeReady");
            }
        }

        public List<string> AvailableProgrammers
        {
            get { return _availableProgrammers; }
            set

            {
                _requestsList = value;
                OnPropertyChanged("AvailableProgrammers");
            }
        }

        public int IndexRequestsList
        {
            get
            {
                return _selectedIndex;
            }
            set
            {
                _selectedIndex = value;
                OnPropertyChanged("IndexRequestsList");
                SelectedRequestsList(this);
            }
        }

        public int IndexProjectsNoManager
        {
            get
            {
                return _selectedIndex;
            }
            set
            {
                _selectedIndex = value;
                OnPropertyChanged("IndexProjectsNoManager");
                SelectedProjectsNoManager(this);
            }
        }

        public int IndexProjectsJudgeReady
        {
            get
            {
                return _selectedIndex;
            }
            set
            {
                _selectedIndex = value;
                OnPropertyChanged("IndexProjectsJudgeReady");
                SelectedProjectsJudgeReady(this);
            }
        }

        public int IndexAvailableProgrammers
        {
            get
            {
                return _selectedIndex;
            }
            set
            {
                _selectedIndex = value;
                OnPropertyChanged("IndexAvailableProgrammers");
                SelectedAvailableProgrammers(this);
            }
        }

        public ICommand GiveProgramer { get; private set; }
        public ICommand NotGiveProgramer { get; private set; }
        public ICommand GoodProject { get; private set; }
        public ICommand BadProject { get; private set; }
        public ICommand AddManager { get; private set; }
        #endregion

        #region Functions
        public BossVM()
        {
            _requestsList = new List<string>();
            _projectsNoManager = new List<string>();
            _projectsJudgeReady = new List<string>();
            _availableProgrammers = new List<string>();

            GiveProgramer = new RelayCommand(AcceptRequest);
            NotGiveProgramer = new RelayCommand(DiscardRequest);
            GoodProject = new RelayCommand(AcceptProject);
            BadProject = new RelayCommand(DiscardProject);
            AddManager = new RelayCommand(AddManagerToProject);
        }

        private void AcceptRequest(object obj)
        {

        }
        private void DiscardRequest(object obj)
        {

        }
        private void AcceptProject(object obj)
        {

        }
        private void DiscardProject(object obj)
        {

        }
        private void AddManagerToProject(object obj)
        {

        }
        private void SelectedRequestsList(object obj)
        {
            _selectedRequestsList = RequestsList[_selectedIndex];
        }
        private void SelectedProjectsNoManager(object obj)
        {
            _selectedProjectsNoManager = ProjectsNoManager[_selectedIndex];
        }
        private void SelectedProjectsJudgeReady(object obj)
        {
            _selectedProjectsJudgeReady = ProjectsJudgeReady[_selectedIndex];
        }
        private void SelectedAvailableProgrammers(object obj)
        {
            _selectedAvailableProgrammers = AvailableProgrammers[_selectedIndex];
        }
        #endregion


        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        #endregion
    }
}
