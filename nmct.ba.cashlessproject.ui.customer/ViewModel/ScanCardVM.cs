using be.belgium.eid;
using GalaSoft.MvvmLight.Messaging;
using Newtonsoft.Json;
using nmct.ba.cashlessproject.model;
using nmct.ba.cashlessproject.ui.customer.EID;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace nmct.ba.cashlessproject.ui.customer.ViewModel
{
    class ScanCardVM : ObservableObject, IPage
    {
        public string Name
        {
            get { return "ScanCard"; }
        }
        
        private string _error;
        public string Error
        {
            get { return _error; }
            set { _error = value; OnPropertyChanged("Error"); }
        }

        private string _connectionDescription;
        public string ConnectionDescription
        {
            get { return _connectionDescription; }
            set { _connectionDescription = value; OnPropertyChanged("ConnectionDescription"); }
        }

        private Customer _selected;
        public Customer SelectedCustomer
        {
            get { return _selected; }
            set { _selected = value; OnPropertyChanged("SelectedCustomer"); }
        }

        private String _selectedCard;
        public String SelectedCard
        {
            get { return _selectedCard; }
            set { _selectedCard = value; OnPropertyChanged("SelectedCard"); }
        }

        public ScanCardVM()
        {
            Messenger.Default.Register<NotificationMessage>(this, (message) => NotificationMessageHandler(message));
        }

        private void NotificationMessageHandler(NotificationMessage m)
        {
            // Checks the actual content of the message
            switch (m.Notification)
            {
                case EIDReader.CARDINSERTED:
                    LoginOrRegister();
                    break;
                case EIDReader.CARDREMOVED:
                    break;
                default:
                    break;
            }
        }

        private string _cardID;
        private void LoginOrRegister()
        {
            if (BEID_ReaderSet.instance().readerCount() <= 0)
                return;
             _cardID = Load_eid();
             if (String.IsNullOrEmpty(_cardID))
                return;

             GetCustomerByCardID(_cardID);
       }

        ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
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
                    if (customer!=null)
                    {
                        if (customer.CardID == id)
                            SelectedCustomer = customer;
                    }
                }
            }
            if (SelectedCustomer != null)
                appvm.ChangePage(new CustomerVM(SelectedCustomer));
            else
            {
                Customer c = Load_eidCustomer();
                if (c == null)
                    return;
                appvm.ChangePage(new RegisterCustomerVM(c));
            }
        }


        private String Load_eid()
        {
            ReadData rd =  new ReadData("beidpkcs11.dll");
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
 
    }
}
