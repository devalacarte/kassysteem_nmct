using GalaSoft.MvvmLight.CommandWpf;
using Newtonsoft.Json;
using nmct.ba.cashlessproject.model;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Windows.Input;

namespace nmct.ba.cashlessproject.ui.management.ViewModel
{
    class EmployeesVM : ObservableObject, IPage
    {
        public string Name
        {
            get { return "EmployeeManagement page"; }
        }

        public EmployeesVM()
        {
            if (ApplicationVM.token != null)
            {
                GetEmployees();
            }
        }

        private ObservableCollection<Employee> _employees;
        public ObservableCollection<Employee> Employees
        {
            get { return _employees; }
            set { _employees = value; OnPropertyChanged("Employees"); }
        }

        private async void GetEmployees()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.GetAsync("http://localhost:27809/api/employee");
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Employees = JsonConvert.DeserializeObject<ObservableCollection<Employee>>(json);
                    if (Employees.Count > 0)
                        SelectedEmployee = Employees.First();
                }
            }
        }


        private Employee _selected;
        public Employee SelectedEmployee
        {
            get { return _selected; }
            set { _selected = value; OnPropertyChanged("SelectedEmployee"); }
        }

        public ICommand NewEmployeeCommand
        {
            get { return new RelayCommand(NewEmployee); }
        }

        public ICommand SaveEmployeeCommand
        {
            get { return new RelayCommand(SaveEmployee, canExecuteSave); }
        }

        private bool canExecuteSave()
        {
            if (SelectedEmployee != null)
            {
                if (SelectedEmployee.IsValid == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        public ICommand DeleteEmployeeCommand
        {
            get { return new RelayCommand(DeleteEmployee); }
        }

        private void NewEmployee()
        {
            if (Employees == null)
                Employees = new ObservableCollection<Employee>();
            Employee e = new Employee();
            Employees.Add(e);
            SelectedEmployee = e;
        }

        private async void SaveEmployee()
        {
            string input = JsonConvert.SerializeObject(SelectedEmployee);
            // check insert (no ID assigned) or update (already an ID assigned)

            if (SelectedEmployee.ID == 0)
            {
                SelectedEmployee.Password = Cryptography.Encrypt(SelectedEmployee.Password);
                input = JsonConvert.SerializeObject(SelectedEmployee);
                using (HttpClient client = new HttpClient())
                {
                    client.SetBearerToken(ApplicationVM.token.AccessToken);
                    HttpResponseMessage response = await client.PostAsync("http://localhost:27809/api/employee", new StringContent(input, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        string output = await response.Content.ReadAsStringAsync();
                        SelectedEmployee.ID = Int32.Parse(output);
                    }
                    else
                    {
                        Console.WriteLine("error");
                    }
                }
            }
            else
            {
                using (HttpClient client = new HttpClient())
                {
                    client.SetBearerToken(ApplicationVM.token.AccessToken);
                    HttpResponseMessage response = await client.PutAsync("http://localhost:27809/api/employee", new StringContent(input, Encoding.UTF8, "application/json"));
                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("error");
                    }
                }
            }
        }

        private async void DeleteEmployee()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.DeleteAsync("http://localhost:27809/api/employee/" + SelectedEmployee.ID);
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("error");
                }
                else
                {
                    Employees.Remove(SelectedEmployee);
                }
            }
        } 
    }
}
