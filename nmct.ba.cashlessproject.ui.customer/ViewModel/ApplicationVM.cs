using be.belgium.eid;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using Thinktecture.IdentityModel.Client;

namespace nmct.ba.cashlessproject.ui.customer.ViewModel
{
    class ApplicationVM : ObservableObject
    {
        public static TokenResponse token = null;
        public EIDReader _reader;
        public ApplicationVM()
        {

            Messenger.Default.Register<NotificationMessage>(this, (message) => NotificationMessageHandler(message));  
            CurrentPage = new LoginVM();
            _reader = new EIDReader();
        }

        private Boolean _isLoggedIn = false;
        public Boolean IsLoggedIn
        {
            get { return _isLoggedIn; }
            set { if (_isLoggedIn != value) { _isLoggedIn = value; OnPropertyChanged("IsLoggedIn"); } }
        }

        private object currentPage;
        public object CurrentPage
        {
            get { return currentPage; }
            set { currentPage = value; OnPropertyChanged("CurrentPage"); }
        }

        private List<IPage> pages;
        public List<IPage> Pages
        {
            get
            {
                if (pages == null)
                    pages = new List<IPage>();
                return pages;
            }
        }

        public ICommand ChangePageCommand
        {
            get { return new RelayCommand<IPage>(ChangePage); }
        }

        public void ChangePage(IPage page)
        {
            if (token != null)
                CurrentPage = page;
            else
                CurrentPage = new LoginVM();
        }

        private void NotificationMessageHandler(NotificationMessage m)
        {
            // Checks the actual content of the message
            switch (m.Notification)
            {
                case EIDReader.CARDINSERTED:
                    //ChangePage(new ScanCardVM());
                    break;
                case EIDReader.CARDREMOVED:
                    if (token != null)
                    {
                        if(!CurrentPage.GetType().Name.Equals(typeof(ScanCardVM).Name))
                            ChangePage(new ScanCardVM());
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
