using nmct.ba.cashlessproject.model.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace nmct.ba.cashlessproject.model
{
    public class Employee : ValidationBase
    {
        #region fields
        private int _id;
        private string _employeeName;
        private string _address;
        private string _email;
        private string _phone;
        private string _pass;
        #endregion fields

        #region properties
        public int ID
        {
            get { return _id; }
            set { if (_id != value) { _id = value; } }
        }
        [Required(ErrorMessage = "verplicht")]
        [RegularExpression(ValidationPaterns.NAME, ErrorMessage = "geen speciale tekens")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "tussen de 3 en 50 karakters")]
        public string EmployeeName
        {
            get { return _employeeName; }
            set { if (_employeeName != value) { _employeeName = value; ValidateProperty(value); } }
        }
        [Required(ErrorMessage = "verplicht")]
        [RegularExpression(ValidationPaterns.ADDRESS, ErrorMessage = "(Straat nr, )(code )gemeente")]
        public string Address
        {
            get { return _address; }
            set { if (_address != value) { _address = value; ValidateProperty(value); } }
        }
        [Required(ErrorMessage = "verplicht")]
        [RegularExpression(ValidationPaterns.EMAIL, ErrorMessage = "format: xxx(.)(-)(xxx)@xxx.com")]
        public string Email
        {
            get { return _email; }
            set { if (_email != value) { _email = value; ValidateProperty(value); } }
        }
        [Required(ErrorMessage = "verplicht")]
        [RegularExpression(ValidationPaterns.PHONE, ErrorMessage = "format: xx(x)(/)xx(.)xx(.)xx")]
        public string Phone
        {
            get { return _phone; }
            set { if (_phone != value) { _phone = value; ValidateProperty(value); } }
        }
        [Required(ErrorMessage = "verplicht")]
        public string Password
        {
            get { return _pass; }
            set { if (_pass != value) { _pass = value; ValidateProperty(value); } }
        }
        #endregion properties

        #region constructor
        public Employee()
        {
            EmployeeName = String.Empty;
            Address = String.Empty;
            Email = String.Empty;
            Phone = String.Empty;
            Password = String.Empty;
        }
        #endregion constructor
    }
}
