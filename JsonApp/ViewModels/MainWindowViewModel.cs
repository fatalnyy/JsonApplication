using JsonApp.Commands;
using JsonApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JsonApp.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private string _formattedJsonString;
        private ObservableCollection<Employee> _employees;

        public MainWindowViewModel()
        {
            _employees = new ObservableCollection<Employee>();
        }

        public string JsonString { get; set; }
        public bool ChangedOrder { get; set; }
        public string FormattedJsonString
        {
            get => _formattedJsonString;
            set
            {
                _formattedJsonString = value;
                RaisePropertyChanged(nameof(FormattedJsonString));
            }
        }

        public ObservableCollection<Employee> Employees
        {
            get => _employees;
            set
            {
                _employees = value;
                RaisePropertyChanged(nameof(Employees));
            }
        }
        public ICommand DownloadJsonStringCommand { get => new RelayCommand(o => DownloadJsonString(), o => true); }
        public ICommand ParseJsonToListOfObjectsCommand { get => new RelayCommand(o => ParseJsonToListOfObjects(), o => true); }
        public ICommand SortByFullNameCommand { get => new RelayCommand(o => SortByFullName(), o => true); }
        public ICommand SortByAgeCommand { get => new RelayCommand(o => SortByAge(), o => true); }
        public ICommand SortBySalaryCommand { get => new RelayCommand(o => SortBySalary(), o => true); }
        public ICommand SortByAvailabilityOnWeekendsCommand { get => new RelayCommand(o => SortByAvailabilityOnWeekends(), o => true); }

        public void DownloadJsonString()
        {
            JsonString = new WebClient().DownloadString("https://api.jsonbin.io/b/5cbb3d6f0a5cb2577c33aae3");
        }

        public void ParseJsonToListOfObjects()
        {
            foreach (var employee in Json<Employee>.ConvertJsonToCollectionOfObjects(JsonString))
            {
                Employees.Insert(0, employee);
            }
            FormattedJsonString = Json<Employee>.FormatJsonString(Employees);
        }

        private void SortByFullName()
        {
            if (ChangedOrder)
            {
                Employees = new ObservableCollection<Employee>(Employees.OrderBy(p => p.FullName));
                ChangedOrder = false;
            }
            else
            {
                Employees = new ObservableCollection<Employee>(Employees.OrderByDescending(p => p.FullName));
                ChangedOrder = true;
            }
        }
        private void SortByAge()
        {
            if(ChangedOrder)
            {
                Employees = new ObservableCollection<Employee>(Employees.OrderBy(p => p.Age));
                ChangedOrder = false;
            }
            else
            {
                Employees = new ObservableCollection<Employee>(Employees.OrderByDescending(p => p.Age));
                ChangedOrder = true;
            }
        }

        private void SortBySalary()
        {
            if (ChangedOrder)
            {
                Employees = new ObservableCollection<Employee>(Employees.OrderBy(p => p.Salary));
                ChangedOrder = false;
            }
            else
            {
                Employees = new ObservableCollection<Employee>(Employees.OrderByDescending(p => p.Salary));
                ChangedOrder = true;
            }
        }

        private void SortByAvailabilityOnWeekends()
        {
            if (ChangedOrder)
            {
                Employees = new ObservableCollection<Employee>(Employees.OrderBy(p => p.IsAvailableOnWeekends));
                ChangedOrder = true;
            }
            else
            {
                Employees = new ObservableCollection<Employee>(Employees.OrderByDescending(p => p.IsAvailableOnWeekends));
                ChangedOrder = true;
            }
        }

    }
}
