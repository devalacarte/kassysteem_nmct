using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace nmct.ba.cashlessproject.ui.management.ViewModel
{
    class ChangePasswordVM : ObservableObject, IPage
    {
        public string Name
        {
            get { return "ChangePassword"; }
        }


        public ChangePasswordVM()
        {
            Username = String.Empty;
            Password = String.Empty;
            PasswordR = String.Empty;
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

        private string _passwordr;
        public string PasswordR
        {
            get { return _passwordr; }
            set { _passwordr = value; OnPropertyChanged("PasswordR"); }
        }

        private string _error;
        public string Error
        {
            get { return _error; }
            set { _error = value; OnPropertyChanged("Error"); }
        }

        public ICommand ChangePasswordCommand
        {
            get { return new RelayCommand(updatePassword); }
        }


        ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;

        private async void updatePassword()
        {
            if (String.IsNullOrEmpty(Username) || String.IsNullOrEmpty(Password) || String.IsNullOrEmpty(PasswordR))
                return;

            if (!Password.Equals(PasswordR))
                return;

            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                //[Route("employee/{name}/{password}")]
                HttpResponseMessage response = client.GetAsync("http://localhost:27809/api/employee/changepass?user=" + Username + "&pass=" + Password).Result;
                if (response.IsSuccessStatusCode)
                {
                    appvm.ChangePage(new CustomerVM());
                }
                    
            }
        }
    }
}
