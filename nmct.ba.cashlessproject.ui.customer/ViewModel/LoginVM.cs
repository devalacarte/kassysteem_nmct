﻿using GalaSoft.MvvmLight.CommandWpf;
using nmct.ba.cashlessproject.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Thinktecture.IdentityModel.Client;
using nmct.ba.cashlessproject.model.Validation;


namespace nmct.ba.cashlessproject.ui.customer.ViewModel
{
    class LoginVM : ObservableObject, IPage
    {
        

        public string Name
        {
            get { return "Login"; }
        }

        public LoginVM()
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
            ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
            ApplicationVM.token = GetToken();

            if (!ApplicationVM.token.IsError)
            {
                appvm.ChangePage(new ScanCardVM());
                appvm.IsLoggedIn = true;
            }
            else
            {
                Error = "Gebruikersnaam of paswoord kloppen niet";
            }
        }

        private TokenResponse GetToken()
        {
            OAuth2Client client = new OAuth2Client(new Uri("http://localhost:27809/token"));
            return client.RequestResourceOwnerPasswordAsync(Username, Password).Result;
        }
    }
}
