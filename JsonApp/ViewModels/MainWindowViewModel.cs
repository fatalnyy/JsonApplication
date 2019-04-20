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
    }
}
