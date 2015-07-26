using Newtonsoft.Json;
using nmct.ba.cashlessproject.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.ui.management.ViewModel
{
    class RegistersVM : ObservableObject, IPage
    {
        public string Name
        {
            get { return "RegisterManagent page"; }
        }

        public RegistersVM()
        {
            if (ApplicationVM.token != null)
            {
                GetRegisters();
                GetEmployees();
            }
        }

        #region Registers
        private ObservableCollection<Register> _registers;
        public ObservableCollection<Register> Registers
        {
            get { return _registers; }
            set { _registers = value; OnPropertyChanged("Registers");}
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
            set { _selectedRegister = value; OnPropertyChanged("SelectedRegister"); FilterEmployees(); }
        }
        #endregion Registers

        #region Employees
        private ObservableCollection<RegisterEmployee> _employeesOriginal;
        private ObservableCollection<RegisterEmployee> _employees;
        public ObservableCollection<RegisterEmployee> Employees
        {
            get { return _employees; }
            set { _employees = value; OnPropertyChanged("Employees"); }
        }

        private async void GetEmployees()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.GetAsync("http://localhost:27809/api/registersemployee");
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    _employeesOriginal = JsonConvert.DeserializeObject<ObservableCollection<RegisterEmployee>>(json);
                    Employees = _employeesOriginal;
                }
            }
        }


        private RegisterEmployee _selected;
        public RegisterEmployee SelectedEmployee
        {
            get { return _selected; }
            set { _selected = value; OnPropertyChanged("SelectedEmployee"); }
        }
        #endregion Employees

        private void FilterEmployees()
        {
            Register r = SelectedRegister;
            Employees = _employeesOriginal;

            if (r != null && Employees != null)
            {
                var col = new ObservableCollection<RegisterEmployee>(Employees.Where(w => (w.RegisterID.ID == r.ID)));
                Employees = col;
            }
        }
    }
}
