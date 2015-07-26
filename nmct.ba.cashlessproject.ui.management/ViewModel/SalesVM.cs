using GalaSoft.MvvmLight.CommandWpf;
using Newtonsoft.Json;
using nmct.ba.cashlessproject.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace nmct.ba.cashlessproject.ui.management.ViewModel
{
    class SalesVM : ObservableObject, IPage
    {
        public string Name
        {
            get { return "ManagementSales page"; }
        }

        public SalesVM()
        {
            if (ApplicationVM.token != null)
            {
                GetProducts();
                GetRegisters();
                GetSales();
                SelectedStartDate = DateTime.Now;
                SelectedEndDate = DateTime.Now;
            }
        }
        public ICommand ResetFiltersCommand
        {
            get { return new RelayCommand(ResetFilters); }
        }
        private void ResetFilters()
        {
            Sales = _salesOriginal;
        }

        #region products
        private ObservableCollection<Product> _products;
        public ObservableCollection<Product> Products
        {
            get { return _products; }
            set { _products = value; OnPropertyChanged("Products"); }
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
                    //if (Products.Count > 0)
                    //SelectedProduct = Products.First();
                }
            }
        }
        private Product _selectedProduct;
        public Product SelectedProduct
        {
            get { return _selectedProduct; }
            set { _selectedProduct = value; OnPropertyChanged("SelectedProduct"); FilterSales(); }
        }
        #endregion products

        #region registers
        private ObservableCollection<Register> _registers;
        public ObservableCollection<Register> Registers
        {
            get { return _registers; }
            set { _registers = value; OnPropertyChanged("Registers"); }
        }
        private async void GetRegisters()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.GetAsync("http://localhost:27809/api/registersclient");
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Registers = JsonConvert.DeserializeObject<ObservableCollection<Register>>(json);
                    //if (Products.Count > 0)
                    //SelectedRegister = Registers.First();
                }
            }
        }
        private Register _selectedRegister;
        public Register SelectedRegister
        {
            get { return _selectedRegister; }
            set { _selectedRegister = value; OnPropertyChanged("SelectedRegister"); FilterSales(); }
        }
        #endregion registers

        #region Sales
        private ObservableCollection<Sale> _salesOriginal;
        private ObservableCollection<Sale> _sales;
        public ObservableCollection<Sale> Sales
        {
            get { return _sales; }
            set { _sales = value; OnPropertyChanged("Sales"); }
        } 
        private async void GetSales()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.GetAsync("http://localhost:27809/api/sales");
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    _salesOriginal = JsonConvert.DeserializeObject<ObservableCollection<Sale>>(json);
                    Sales = _salesOriginal;
                }
            }
        }

        private void FilterSales()
        {
            Register r = SelectedRegister;
            Product p = SelectedProduct;
            long ts = DateTimeToUnixTimestamp(SelectedStartDate);
            long te = DateTimeToUnixTimestamp(SelectedEndDate);
            Sales = _salesOriginal;

            if (r != null && Sales != null)
            {
                var col = new ObservableCollection<Sale>(Sales.Where(w => (w.RegisterID == r.ID)));
                Sales = col;
            }

            if (p != null && Sales != null)
            {
                var col = new ObservableCollection<Sale>(Sales.Where(w => (w.ProductID == p.ID)));
                Sales = col;
            }

            if (SelectedStartDate!=null && SelectedEndDate != null && Sales!=null)
            {
                if (SelectedStartDate <= SelectedEndDate)
                {
                    var col = new ObservableCollection<Sale>(Sales.Where(w => (w.Timestamp >= ts && w.Timestamp <= te)));
                    Sales = col;
                }
            }
        }
        #endregion Sales

        #region dates
        private DateTime _selectedStartDate;
        public DateTime SelectedStartDate
        {
            get { return _selectedStartDate; }
            set
            {
                if (_selectedStartDate != value)
                {
                    DateTime dt = new DateTime(value.Year, value.Month, value.Day, 0, 0, 0, 1);
                    _selectedStartDate = dt; OnPropertyChanged("SelectedStartDate"); FilterSales();
                }
            }
        }

        private DateTime _selectedEndDate;
        public DateTime SelectedEndDate
        {
            get { return _selectedEndDate; }
            set { if (_selectedEndDate != value) {
                DateTime dt = new DateTime(value.Year, value.Month, value.Day, 23, 59, 59, 99);
                _selectedEndDate = dt; OnPropertyChanged("SelectedEndDate"); FilterSales(); } }
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

        #endregion dates
    }
}