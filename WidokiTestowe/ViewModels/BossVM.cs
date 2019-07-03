using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkTimeManager.Models;
using WorkTimeManager.DBAccess;

namespace WorkTimeManager.ViewModels
{
    public class BossVM : INotifyPropertyChanged
    {
        #region PrivateFields
        private List<string> _requestsList;
        private List<string> _projectsNoManager;
        private List<string> _projectsJudgeReady;
        private List<string> _availableProgramers;
        private int _selectedIndex;
        private int _selectedIndexplus;
        private string _selectedRequestsList;
        private string _selectedProjectsNoManager;
        private string _selectedProjectsJudgeReady;
        private string _selectedAvailableProgramers;
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
                _projectsNoManager = value;
                OnPropertyChanged("ProjectsNoManager");
            }
        }

        public List<string> ProjectsJudgeReady
        {
            get { return _projectsJudgeReady; }
            set
            {
                _projectsJudgeReady = value;
                OnPropertyChanged("ProjectsJudgeReady");
            }
        }

        public List<string> AvailableProgramers
        {
            get { return _availableProgramers; }
            set
            {
                _availableProgramers = value;
                OnPropertyChanged("AvailableProgramers");
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

        public int IndexAvailableProgramers
        {
            get
            {
                return _selectedIndexplus;
            }
            set
            {
                _selectedIndexplus = value;
                OnPropertyChanged("IndexAvailableProgramers");
                SelectedAvailableProgramers(this);
            }
        }
        #endregion
        #region Commands
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
            _availableProgramers = new List<string>();

            //wypisywanie przy otwarciu
            AvailableProgramers = CommandsRepository.GET_PROGRAMER_WITH_NO_LEADED_PROJECT();
            ProjectsNoManager = CommandsRepository.GET_PROJECTS_WITHOUT_MANAGER();
            ProjectsJudgeReady = CommandsRepository.GET_PROJECTS_TO_JUDGE();

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
            if (IndexProjectsJudgeReady >= 0)
            {
                CommandsRepository.CHANGE_STATUS_TO_ACCEPTED(ProjectsJudgeReady[IndexProjectsJudgeReady], IndexProjectsJudgeReady);
                ProjectsJudgeReady = CommandsRepository.GET_PROJECTS_TO_JUDGE();
            }
           
        }
        private void DiscardProject(object obj)
        {
            if (IndexProjectsJudgeReady >= 0)
            {
                CommandsRepository.CHANGE_STATUS_TO_INPROGRESS(ProjectsJudgeReady[IndexProjectsJudgeReady], IndexProjectsJudgeReady);
                ProjectsJudgeReady = CommandsRepository.GET_PROJECTS_TO_JUDGE();
            }
        }
        private void AddManagerToProject(object obj)
        {
            if ((IndexAvailableProgramers >= 0) && (IndexProjectsNoManager >= 0))
            {
                CommandsRepository.ADD_MANAGER(ProjectsNoManager[IndexProjectsNoManager], IndexAvailableProgramers, IndexProjectsNoManager);
            }
            SelectedProjectsNoManager(this);
            SelectedAvailableProgramers(this);
            ProjectsNoManager = CommandsRepository.GET_PROJECTS_WITHOUT_MANAGER();
            AvailableProgramers = CommandsRepository.GET_PROGRAMER_WITH_NO_LEADED_PROJECT();
        }
        private void SelectedRequestsList(object obj)
        {
            _selectedRequestsList = RequestsList[IndexRequestsList];
        }
        private void SelectedProjectsNoManager(object obj)
        {
            if ((IndexAvailableProgramers >= 0) && (IndexProjectsNoManager >= 0))
            {
                _selectedProjectsNoManager = ProjectsNoManager[IndexProjectsNoManager];
            }
        }
        private void SelectedProjectsJudgeReady(object obj)
        {
            if (IndexProjectsJudgeReady >= 0)
            {
                _selectedProjectsJudgeReady = ProjectsJudgeReady[IndexProjectsJudgeReady];
            }
        }
        private void SelectedAvailableProgramers(object obj)
        {
            if ((IndexAvailableProgramers >= 0) && (IndexProjectsNoManager >= 0))
            {
                _selectedAvailableProgramers = AvailableProgramers[IndexAvailableProgramers];
            }
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
