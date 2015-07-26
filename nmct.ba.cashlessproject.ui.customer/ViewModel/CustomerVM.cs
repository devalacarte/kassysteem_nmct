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

namespace nmct.ba.cashlessproject.ui.customer.ViewModel
{
    class CustomerVM : ObservableObject, IPage
    {
        private const double MAX_BALANCE = 100;
        public string Name
        {
            get { return "Customer"; }
        }

        public CustomerVM(Customer c)
        {
            SelectedCustomer = c;
        }

        private Customer _selected;
        public Customer SelectedCustomer
        {
            get { return _selected; }
            set { _selected = value; OnPropertyChanged("SelectedCustomer"); }
        }

        private double _ammountToAdd;
        public double AmmountToAdd
        {
            get { return _ammountToAdd; }
            set { if (_ammountToAdd != value) { _ammountToAdd = value; OnPropertyChanged("AmmountToAdd"); SelectedCustomer.Balance = value; } }
        }

        public ICommand SaveCustomerCommand
        {
            get { return new RelayCommand(SaveCustomer, canExecuteSave); }
        }

        public ICommand CancelCommand
        {
            get { return new RelayCommand(CancelCustomer); }
        }

        private bool canExecuteSave()
        {
            if (SelectedCustomer != null)
            {
                if (SelectedCustomer.IsValid == true)
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

        private async void SaveCustomer()
        {
            string input = JsonConvert.SerializeObject(SelectedCustomer);
            // check insert (no ID assigned) or update (already an ID assigned)

            if (SelectedCustomer.ID == 0)
            {
                using (HttpClient client = new HttpClient())
                {
                    client.SetBearerToken(ApplicationVM.token.AccessToken);
                    HttpResponseMessage response = await client.PostAsync("http://localhost:27809/api/customer", new StringContent(input, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        string output = await response.Content.ReadAsStringAsync();
                        SelectedCustomer.ID = Int32.Parse(output);
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
                    HttpResponseMessage response = await client.PutAsync("http://localhost:27809/api/customer", new StringContent(input, Encoding.UTF8, "application/json"));
                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("error");
                    }
                    else {
                        
                    }
                }
            }
        }

        private void CancelCustomer()
        {
            ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
            appvm.CurrentPage = new ScanCardVM();
        }

    }
}
