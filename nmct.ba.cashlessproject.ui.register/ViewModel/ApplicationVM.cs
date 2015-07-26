using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Thinktecture.IdentityModel.Client;

namespace nmct.ba.cashlessproject.ui.register.ViewModel
{
    class ApplicationVM : ObservableObject
    {
        public static TokenResponse token = null;

        public ApplicationVM()
        {
            CurrentPage = new LoginVM();
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
            CurrentPage = page;
        }
    }
}
