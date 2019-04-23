using JsonApp.Commands;
using JsonApp.Models;
using JsonApp.Models.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace JsonApp.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        #region Fields
        
        private string _formattedJsonString;
        private ObservableCollection<Employee> _employees;

        #endregion

        #region Constructors

        public MainWindowViewModel()
        {
            _employees = new ObservableCollection<Employee>();
        }

        #endregion

        #region Properties

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

        public ICommand GetJsonStringCommand { get => new RelayCommand(o => GetJsonString(), o => true); }
        public ICommand ParseJsonToListOfObjectsCommand { get => new RelayCommand(o => ParseJsonToListOfObjects(), o => true); }
        public ICommand SortByFullNameCommand { get => new RelayCommand(o => SortByFullName(), o => true); }
        public ICommand SortByAgeCommand { get => new RelayCommand(o => SortByAge(), o => true); }
        public ICommand SortBySalaryCommand { get => new RelayCommand(o => SortBySalary(), o => true); }
        public ICommand SortByAvailabilityOnWeekendsCommand { get => new RelayCommand(o => SortByAvailabilityOnWeekends(), o => true); }
        public ICommand SaveResultsToFileCommand { get => new RelayCommand(o => SaveResultsToFile(), o => true); }
        public ICommand SaveResultsToDatabaseCommand { get => new RelayCommand(o => SaveResultsToDatabase(), o => true); }

        #endregion

        #region Methods

        private void GetJsonString()
        {
            try
            {
                JsonString = new WebClient().DownloadString("https://api.jsonbin.io/b/5cbf4a951f6d9a5478d00622");
                MessageBox.Show("Json was downloaded correctly.");
            }
            catch
            {
                MessageBox.Show("Check your internet connection!");
            }
        }

        private void ParseJsonToListOfObjects()
        {
            if(JsonString != null)
            {
                foreach (var employee in Json<Employee>.ConvertJsonToCollectionOfObjects(JsonString))
                {
                    Employees.Insert(0, employee);
                }
                FormattedJsonString = Json<Employee>.FormatJsonString(Employees);
            }
            else
            {
                MessageBox.Show("You have to download json first!");
            }
        }

        private void SaveResultsToFile()
        {
            if(Employees.Count != 0)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "JSON files (*.json)|*.json|XML files (*.xml)|*.xml";

                if (sfd.ShowDialog() == true)
                {
                    if (sfd.FilterIndex == 1)
                        File.WriteAllText(sfd.FileName, Json<Employee>.FormatJsonString(Employees));
                    else
                        Xml<ObservableCollection<Employee>>.SerializeObjectToXML(Employees, sfd.FileName);
                }
            }
            else
            {
                MessageBox.Show("You have to download json and parse it to list of objects first!");
            }
        }

        private void SaveResultsToDatabase()
        {
            if(Employees.Count != 0)
            {
                using (var db = new AppDbContext())
                {
                    db.Employees.AddRange(Employees);
                    db.SaveChanges();
                    Employees.Clear();
                    ParseJsonToListOfObjects();
                    MessageBox.Show("Results were saved in database correctly.");
                }
            }
            else
            {
                MessageBox.Show("You have to download json and parse it to list of objects first!");
            }
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

        #endregion
    }
}
