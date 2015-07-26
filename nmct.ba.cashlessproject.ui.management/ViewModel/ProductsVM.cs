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
    class ProductsVM : ObservableObject, IPage
    {
        public string Name
        {
            get { return "ProductsManagement page"; }
        }

        public ProductsVM()
        {
            if (ApplicationVM.token != null)
            {
                GetProducts();
            }
        }

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
                    if (Products.Count > 0)
                        SelectedProduct = Products.First();
                }
            }
        }

        private Product _selected;
        public Product SelectedProduct
        {
            get { return _selected; }
            set { _selected = value; OnPropertyChanged("SelectedProduct"); }
        }

        public ICommand NewProductCommand
        {
            get { return new RelayCommand(NewProduct); }
        }

        public ICommand SaveProductCommand
        {
            get { return new RelayCommand(SaveProduct, canExecuteSave); }
        }

        private bool canExecuteSave()
        {
            if (SelectedProduct != null)
            {
                if (SelectedProduct.IsValid == true)
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

        public ICommand DeleteProductCommand
        {
            get { return new RelayCommand(DeleteProduct); }
        }


        private void NewProduct()
        {
            Product p = new Product();
            Products.Add(p);
            SelectedProduct = p;
        }

        private async void SaveProduct()
        {
            string input = JsonConvert.SerializeObject(SelectedProduct);
            // check insert (no ID assigned) or update (already an ID assigned)

            if (SelectedProduct.ID == 0)
            {
                using (HttpClient client = new HttpClient())
                {
                    client.SetBearerToken(ApplicationVM.token.AccessToken);
                    HttpResponseMessage response = await client.PostAsync("http://localhost:27809/api/products", new StringContent(input, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        string output = await response.Content.ReadAsStringAsync();
                        SelectedProduct.ID = Int32.Parse(output);
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
                    HttpResponseMessage response = await client.PutAsync("http://localhost:27809/api/products", new StringContent(input, Encoding.UTF8, "application/json"));
                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("error");
                    }
                }
            }
        }

        private async void DeleteProduct()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.DeleteAsync("http://localhost:27809/api/products/" + SelectedProduct.ID);
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("error");
                }
                else
                {
                    Products.Remove(SelectedProduct);
                }
            }
        }
    }
}
