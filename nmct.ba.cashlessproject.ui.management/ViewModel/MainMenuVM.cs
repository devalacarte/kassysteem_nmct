using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace nmct.ba.cashlessproject.ui.management.ViewModel
{
    class MainMenuVM : ObservableObject, IPage
    {
        public string Name
        {
            get { return "MainMenu page"; }
        }

        public MainMenuVM()
        {
            if (ApplicationVM.token != null)
            {

            }
        }

        public ICommand ManageProductsCommand
        {
            get { return new RelayCommand(GoToManageProducts); }
        }

        public ICommand ManageCustomersCommand
        {
            get { return new RelayCommand(GoToManageCustomers); }
        }

        public ICommand ManageRegistersCommand
        {
            get { return new RelayCommand(GoToManageRegisters); }
        }

        public ICommand StatisticsSalesCommand
        {
            get { return new RelayCommand(GoToStatisticsSales); }
        }

        public ICommand ManageEmployeesCommand
        {
            get { return new RelayCommand(GoToManageEmployees); }
        }

        public ICommand LogoutCommand
        {
            get { return new RelayCommand(Logout); }
        }

        public ICommand ChangePasswordCommand
        {
            get { return new RelayCommand(ChangePassword); }
        }







        


        private void GoToManageProducts()
        {
            ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
            appvm.ChangePage(new ProductsVM());
        }

        private void GoToManageCustomers()
        {
            ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
            appvm.ChangePage(new CustomerVM());
        }

        private void GoToManageRegisters()
        {
            ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
            appvm.ChangePage(new RegistersVM());
        }

        private void GoToStatisticsSales()
        {
            ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
            appvm.ChangePage(new SalesVM());
        }

        private void GoToManageEmployees()
        {
            ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
            appvm.ChangePage(new EmployeesVM());
        }

        private void Logout()
        {
            ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
            ApplicationVM.token = null;
            appvm.IsLoggedIn = false;
            appvm.ChangePage(new LoginVM());
        }

        private void ChangePassword()
        {
            ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
            appvm.ChangePage(new ChangePasswordVM());
        }
    }
}
