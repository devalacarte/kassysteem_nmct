using GalaSoft.MvvmLight.CommandWpf;
using Newtonsoft.Json;
using nmct.ba.cashlessproject.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Thinktecture.IdentityModel.Client;

namespace nmct.ba.cashlessproject.ui.employee.ViewModel
{
    class LoginEmployeeVM : ObservableObject, IPage
    {
        public string Name
        {
            get { return "LoginEmployee"; }
        }


        public LoginEmployeeVM()
        {
            Username = String.Empty;
            Password = String.Empty;
        }

        private string _username;
        public string Username
        {
            get { return _username; }
            set { _username = value; OnPropertyChanged("Username"); }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged("Password"); }
        }

        private Employee _selectedEmployee;
        public Employee SelectedEmployee
        {
            get { return _selectedEmployee; }
            set { _selectedEmployee = value; OnPropertyChanged("SelectedEmployee"); }
        }

        private string _error;
        public string Error
        {
            get { return _error; }
            set { _error = value; OnPropertyChanged("Error"); }
        }


        public ICommand LoginCommand
        {
            get { return new RelayCommand(Login, canExecuteLogin); }
        }

        public bool canExecuteLogin()
        {
            if (!String.IsNullOrEmpty(Username) && !String.IsNullOrEmpty(Password))
                return true;
            else
                return false;
        }

        private void Login()
        {
            if (!String.IsNullOrEmpty(Username) && !String.IsNullOrEmpty(Password))
            {
               GetEmployeeFromLogin(Username, Cryptography.Encrypt(Password));
            }
        }
        

        /*
         * checken of er passwoord is ingevuld, zo ni eerst doorsture naar een usercontrol om wachtwoord in te stellen */
        ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
        private async void GetEmployeeFromLogin(string name, string password)
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                //[Route("employee/{name}/{password}")]
                HttpResponseMessage response = client.GetAsync("http://localhost:27809/api/employee/checkemployee?name="+name+"&password="+password).Result;
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Employee employee = JsonConvert.DeserializeObject<Employee>(json);
                    if (employee != null)
                    {
                        if (employee.EmployeeName == name)
                            SelectedEmployee = employee;
                    }
                }
            }
            if (SelectedEmployee != null)
                appvm.ChangePage(new EmployeeVM(SelectedEmployee));
            else
            {
                Error = "Gebruikersnaam of paswoord kloppen niet";
            }
        }

    }
}
