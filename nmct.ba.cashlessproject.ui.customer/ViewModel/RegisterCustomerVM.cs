using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using Newtonsoft.Json;
using nmct.ba.cashlessproject.model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace nmct.ba.cashlessproject.ui.customer.ViewModel
{
    class RegisterCustomerVM : ObservableObject, IPage
    {
        public string Name
        {
            get { return "RegisterCustomer"; }
        }

        public RegisterCustomerVM(Customer c)
        {
            SelectedCustomer = c;
        }

        private Customer _selected;
        public Customer SelectedCustomer
        {
            get { return _selected; }
            set { _selected = value; OnPropertyChanged("SelectedCustomer"); }
        }

        public ICommand SaveCustomerCommand
        {
            get { return new RelayCommand(SaveCustomer, canExecuteSave); }
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

        public ICommand CancelCommand
        {
            get { return new RelayCommand(CancelCustomer); }
        }

        
        public ICommand AddImageCommand
        {
            get { return new RelayCommand(AddImage); }
        }

        ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
        private async void SaveCustomer()
        {
            /*if (SelectedCustomer.Picture == null)
                SelectedCustomer.Picture = GetPhoto();*/

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
                        appvm.ChangePage(new CustomerVM(SelectedCustomer));
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
                }
            }
        }

        private void CancelCustomer()
        {
            ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
            appvm.CurrentPage = new ScanCardVM();
        }

        private void AddImage()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {
                SelectedCustomer.Picture = GetPhoto(ofd.FileName);
                OnPropertyChanged("SelectedCustomer");
            }
        }

        private byte[] GetPhoto(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            byte[] data = new byte[fs.Length];
            fs.Read(data, 0, (int)fs.Length);
            fs.Close();

            return data;
        }
         
    }
}
