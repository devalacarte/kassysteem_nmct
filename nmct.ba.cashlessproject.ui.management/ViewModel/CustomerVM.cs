using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using Newtonsoft.Json;
using nmct.ba.cashlessproject.model;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Windows.Input;

namespace nmct.ba.cashlessproject.ui.management.ViewModel
{
    class CustomerVM : ObservableObject, IPage
    {
        public string Name
        {
            get { return "CustomerManagement page"; }
        }

        public CustomerVM()
        {
            if (ApplicationVM.token != null)
            {
                GetCustomers();
            }
        }

        private ObservableCollection<Customer> _customers;
        public ObservableCollection<Customer> Customers
        {
            get { return _customers; }
            set { _customers = value; OnPropertyChanged("Customers"); }
        }

        private async void GetCustomers()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.GetAsync("http://localhost:27809/api/customer");
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Customers = JsonConvert.DeserializeObject<ObservableCollection<Customer>>(json);
                    if (Customers.Count > 0)
                        SelectedCustomer = Customers.First();
                }
            }
        }

        private Customer _selected;
        public Customer SelectedCustomer
        {
            get { return _selected; }
            set { _selected = value; OnPropertyChanged("SelectedCustomer"); }
        }

        public ICommand NewCustomerCommand
        {
            get { return new RelayCommand(NewCustomer); }
        }
        
        public ICommand SaveCustomerCommand
        {
            get { return new RelayCommand(SaveCustomer,canExecuteSave); }
        }

        private bool canExecuteSave() {
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

        public ICommand DeleteCustomerCommand
        {
            get { return new RelayCommand(DeleteCustomer); }
        }

        public ICommand AddImageCommand
        {
            get { return new RelayCommand(AddImage); }
        }

        private void NewCustomer()
        {
            if (Customers == null)
                Customers = new ObservableCollection<Customer>();
            Customer c = new Customer();
            Customers.Add(c);
            SelectedCustomer = c;
        }

        private async void SaveCustomer()
        {
            if (SelectedCustomer.Picture == null)
                SelectedCustomer.Picture = GetPhoto();

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
                }
            }
        }

        private async void DeleteCustomer()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.DeleteAsync("http://localhost:27809/api/customer/" + SelectedCustomer.ID);
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("error");
                }
                else
                {
                    Customers.Remove(SelectedCustomer);
                }
            }
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


        private byte[] GetPhoto()
        {
            System.Windows.Resources.StreamResourceInfo sri = System.Windows.Application.GetResourceStream(new Uri(@"pack://application:,,,/nmct.ba.cashlessproject.ui.management;component/View/images/noimage.png"));
            Stream s = sri.Stream;
            byte[] data = new byte[s.Length];
            s.Read(data, 0, (int)s.Length);
            s.Close();
            return data;
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
