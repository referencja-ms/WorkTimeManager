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
    class ProjectsManagementVM : INotifyPropertyChanged
    {
        #region Fields
        private string _name;
        private string _status;
        private int _budget;
        private int _timeBudget;
        private DateTime _deadline;
        private string _description;
        private string _customerNIP;
        private int _selectedIndex;
        private int _selectedId;
        private Project _selectedProject;
        private List<string> _projectStringList = new List<string>();
        private List<Project> _projectObjectList = new List<Project>();
        private string _message;
        #endregion

        #region Properties
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged("Status");
            }
        }

        public int Budget
        {
            get { return _budget; }
            set
            {
                _budget = value;
                OnPropertyChanged("Budget");
            }
        }

        public int TimeBudget
        {
            get { return _timeBudget; }
            set
            {
                _timeBudget = value;
                OnPropertyChanged("TimeBudget");
            }
        }

        public DateTime Deadline
        {
            get { return _deadline; }
            set
            {
                _deadline = value;
                OnPropertyChanged("Deadline");
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged("Description");
            }
        }


        public string CustomerNIP
        {
            get { return _customerNIP; }
            set
            {
                _customerNIP = value;
                OnPropertyChanged("CustomerNIP");
            }
        }

        public List<string> ProjectStringList
        {
            get { return _projectStringList; }
            set
            {
                _projectStringList = value;
                OnPropertyChanged("ProjectStringList");
            }
        }

        public List<Project> ProjectObjectList
        {
            get { return _projectObjectList; }
            set
            {
                _projectObjectList = value;
                OnPropertyChanged("ProjectObjectList");
            }
        }

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                _selectedIndex = value;
                OnPropertyChanged("SelectedIndex");
                _selectedProject = ProjectObjectList[_selectedIndex];
                _selectedId = _selectedProject.Id;
                SelectProject(this);
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
        public ICommand AddProjectCommand { get; private set; }
        public ICommand UpdateProjectCommand { get; private set; }
        public ICommand ClearFormCommand { get; private set; }
        public ICommand SelectProjectCommand { get; private set; }
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        public ProjectsManagementVM()
        {
            AddProjectCommand = new RelayCommand(AddProject);
            ClearFormCommand = new RelayCommand(ClearForm);
            UpdateProjectCommand = new RelayCommand(UpdateProject);
            LoadProjectsFromDatabase();
            RefreshProjectList();
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void LoadProjectsFromDatabase()
        {
            CommandsRepository.GET_ALL_PROJECTS();
            ProjectObjectList = CommandsRepository.ProjectsList;
        }

        private void AddProject(object obj)
        {
            try
            {
                Project project = new Project(Name, Status, Budget, TimeBudget, Deadline, Description, CustomerNIP);
                if (Validator.IsProjectValid(project))
                {
                    if (CommandsRepository.ADD_PROJECT(project))
                    {
                        ProjectObjectList.Add(project);
                        RefreshProjectList();
                        Message = "";
                    }
                    else
                    {
                        Message = "Nie można dodać projektu";
                    }
                }
                else
                {
                    Message = "Niektóe pola są niepoprawne";
                }
            }
            catch
            {
                Message = "Nie można dodać projektu";
            }   
        }

        private void UpdateProject(object obj)
        {
            try
            {
                _selectedProject.Name = Name;
                _selectedProject.Status = Status;
                _selectedProject.Budget = Budget;
                _selectedProject.TimeBudget = TimeBudget;
                _selectedProject.Deadline = Deadline;
                _selectedProject.Description = Description;
                _selectedProject.CustomerNIP = CustomerNIP;

                if (Validator.IsProjectValid(_selectedProject))
                {
                    if (Validator.IsProjectValid(_selectedProject))
                    {
                        if (CommandsRepository.UPDATE_PROJECT(_selectedProject))
                        {
                            RefreshProjectList();
                            Message = "";
                        }
                        else
                        {
                            Message = "Nie można zaktualizować projektu";
                        }
                    }
                    else
                    {
                        Message = "Niektóre pola są niepoprawne";
                    }
                }
            }
            catch
            {
                Message = "Nie można zaktualizować projektu";
            }
        }

        private void SelectProject(object obj)
        {
            Name = _selectedProject.Name;
            Status = _selectedProject.Status;
            Budget = _selectedProject.Budget;
            TimeBudget = _selectedProject.TimeBudget;
            Description = _selectedProject.Description;
            Deadline = _selectedProject.Deadline;
            CustomerNIP = _selectedProject.CustomerNIP;
        }

        private void ClearForm(object obj)
        {
            Name = "";
            Status = "";
            Budget = 0;
            TimeBudget = 0;
            Deadline = DateTime.Now;
            Description = "";
            CustomerNIP = "";
        }

        private void RefreshProjectList()
        {
            List<Project> tmp = new List<Project>();
            foreach (Project project in ProjectObjectList)
            {
                tmp.Add(project);
            }
            ProjectObjectList = tmp;
        }
    }
}
