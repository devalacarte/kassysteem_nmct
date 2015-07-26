using be.belgium.eid;
using GalaSoft.MvvmLight.CommandWpf;
using Newtonsoft.Json;
using nmct.ba.cashlessproject.model;
using nmct.ba.cashlessproject.ui.employee.EID;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace nmct.ba.cashlessproject.ui.employee.ViewModel
{
    class EmployeeVM : ObservableObject, IPage
    {
        public string Name
        {
            get { return "EmployeeVM"; }
        }


        public EmployeeVM(Employee e)
        {
            SelectedEmployee = e;
            GetProducts();
            NewOrder();
            //ReadEID();
        }

        private void NewOrder()
        {
            SelectedSale = new Sale();
            if (Order != null)
                Order.Clear();
            Order = new ObservableCollection<Product>();
            SelectedCustomer = null;
        }

        private Employee _selectedEmployee;
        public Employee SelectedEmployee
        {
            get { return _selectedEmployee; }
            set { _selectedEmployee = value; OnPropertyChanged("SelectedEmployee"); }
        }

        private Customer _selectedCustomer;
        public Customer SelectedCustomer
        {
            get { return _selectedCustomer; }
            set { _selectedCustomer = value; OnPropertyChanged("SelectedCustomer"); }
        }

        private Product _selectedProduct;
        public Product SelectedProduct
        {
            get { return _selectedProduct; }
            set { _selectedProduct = value; OnPropertyChanged("SelectedProduct"); }
        }

        private ObservableCollection<Product> _products;
        public ObservableCollection<Product> Products
        {
            get { return _products; }
            set { _products = value; OnPropertyChanged("Products"); }
        }

        private Product _selectedProductInOrder;
        public Product SelectedProductInOrder
        {
            get { return _selectedProductInOrder; }
            set { _selectedProductInOrder = value; OnPropertyChanged("SelectedProductInOrder"); }
        }

        private ObservableCollection<Product> _order;
        public ObservableCollection<Product> Order
        {
            get { return _order; }
            set { _order = value; OnPropertyChanged("Order"); }
        } 

        private async void GetProducts()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.GetAsync("http://localhost:27809/api/products");
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Products = JsonConvert.DeserializeObject<ObservableCollection<Product>>(json);
                    if (Products.Count > 0)
                        SelectedProduct = Products.First();
                }
            }
        }

        private Sale _selectedSale;
        public Sale SelectedSale
        {
            get { return _selectedSale; }
            set { _selectedSale = value; OnPropertyChanged("SelectedSale"); }
        }


        private string _cardID = string.Empty;
        public string CardID
        {
          get { return _cardID; }
          set { if(_cardID != value) {_cardID = value; OnPropertyChanged("CardID");} }
        }

        public ICommand ReadEIDCommand
        {
            get { return new RelayCommand(ReadEID, CanReadCard); }
        }
        private bool CanReadCard()
        {
            if (SelectedCustomer == null)
                return true;
            else
                return false;
        }
        public ICommand SaveSaleCommand
        {
            get { return new RelayCommand(SaveSaleClicked, CanExecuteSave); }
        }
        private bool CanExecuteSave()
        {
            if (SelectedSale == null || SelectedCustomer == null || Products == null)
                return false;
            return SelectedSale.IsValid;
        }
        public ICommand NewSaleCommand
        {
            get { return new RelayCommand(NewSale); }
        }

        public ICommand LogoutCommand
        {
            get { return new RelayCommand(LogOut); }
        }

        public ICommand RemoveFromListCommand
        {
            get { return new RelayCommand(RemoveFromList, CanRemove); }
        }
        private bool CanRemove()
        {
            if (Order != null && Order.Count > 0  && SelectedProductInOrder != null)
                return true;
            else
                return false;
        }
        public ICommand AddToListCommand
        {
            get { return new RelayCommand(AddToList, CanAdd); }
        }
        private bool CanAdd()
        {
            if (SelectedProduct != null && Products.Count > 0 && SelectedCustomer != null)
                return true;
            else
                return false;
        }
        
        private void ReadEID()
        {
            /*BEID_ReaderSet.initSDK();
            // access the eID card here
            if (BEID_ReaderSet.instance().readerCount() > 0)
            {
                BEID_ReaderContext readerContext = readerContext = BEID_ReaderSet.instance().getReader();
                if (readerContext != null)
                {
                    if (readerContext.getCardType() == BEID_CardType.BEID_CARDTYPE_EID)
                    {
                        BEID_EIDCard card = readerContext.getEIDCard();
                        string cardID = card.getID().getNationalNumber();
                        GetCustomerByCardID(cardID);
                    }
                }
            }
            BEID_ReaderSet.releaseSDK();*/
            /*Fucking bugged dll, bollet*/

            GetCustomerByCardID(Load_eid());
        }

        private String Load_eid()
        {
            ReadData rd = new ReadData("beidpkcs11.dll");
            string res = rd.GetCardNumber();
            return res;
        }

        private Customer Load_eidCustomer()
        {

            Customer c = new Customer();
            c.Address = new ReadData("beidpkcs11.dll").GetStreet() + ", " + new ReadData("beidpkcs11.dll").GetZip() + " " + new ReadData("beidpkcs11.dll").GetCity();
            c.CardID = new ReadData("beidpkcs11.dll").GetCardNumber();
            c.CustomerName = new ReadData("beidpkcs11.dll").GetFirstName() + " " + new ReadData("beidpkcs11.dll").GetSurname();
            c.Picture = new ReadData("beidpkcs11.dll").GetPhotoFile();
            c.Balance = 0;

            return c;
        }
        private async void GetCustomerByCardID(string id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = client.GetAsync("http://localhost:27809/api/customer/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Customer customer = JsonConvert.DeserializeObject<Customer>(json);
                    if (customer != null)
                    {
                        if (customer.CardID == id)
                            SelectedCustomer = customer;
                    }
                }
            }
        }


        private void NewSale()
        {
            NewOrder();
        }

        private void LogOut()
        {
            ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
            appvm.ChangePage(new LoginEmployeeVM());
            //token niet op nul zetten, is voor de database
        }


        private async void SaveSaleClicked()
        {
            var query = Order.Select(prod => prod.ID).Distinct().ToList();
            int amount = 0;
            double totalPrice = 0;

            Sale sale = new Sale();

            foreach (int id in query)
            {
                var products = Order.Where(prod => prod.ID == id);
                foreach (var product in products)
                {
                    amount++;
                    totalPrice = totalPrice + product.Price;
                }

                sale.Timestamp = GetUnixTimeStamp();
                sale.CustomerID = SelectedCustomer.ID;
                sale.ProductID = id;
                sale.Amount = amount;
                sale.TotalPrice = totalPrice;
                SelectedSale = sale;
                SaveSale();
            }
        }

        private async void SaveSale()
        {

            string input = JsonConvert.SerializeObject(SelectedSale);

            if (SelectedSale.ID == 0)
            {
                input = JsonConvert.SerializeObject(SelectedSale);
                using (HttpClient client = new HttpClient())
                {
                    client.SetBearerToken(ApplicationVM.token.AccessToken);
                    HttpResponseMessage response = await client.PostAsync("http://localhost:27809/api/sales", new StringContent(input, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        string output = await response.Content.ReadAsStringAsync();
                        SelectedSale.ID = Int32.Parse(output);
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
                    HttpResponseMessage response = await client.PutAsync("http://localhost:27809/api/sales", new StringContent(input, Encoding.UTF8, "application/json"));
                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("error");
                    }
                }
            }
        }

        

        private void RemoveFromList()
        {
            if (Order != null && SelectedProductInOrder != null)
                Order.Remove(SelectedProductInOrder);
        }

        private void AddToList()
        {
            if (Order == null)
                Order = new ObservableCollection<Product>();
            if (SelectedProduct != null)
                Order.Add(SelectedProduct);
        }

        private long GetUnixTimeStamp()
        {
            DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);
            return DateTimeToUnixTimestamp(dt);
        }

        private DateTime UnixTimestampToDateTime(long unix)
        {
            return (new DateTime(1970, 1, 1, 0, 0, 0)).AddSeconds(unix);
        }

        private long DateTimeToUnixTimestamp(DateTime dateTime)
        {
            TimeSpan unix = (dateTime - new DateTime(1970, 1, 1, 0, 0, 0));
            return (long)unix.TotalSeconds;
        }
    }
}
