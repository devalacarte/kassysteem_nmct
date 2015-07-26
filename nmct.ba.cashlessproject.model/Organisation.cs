using nmct.ba.cashlessproject.model.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace nmct.ba.cashlessproject.model
{
    public class Organisation : ValidationBase
    {
        #region fields
        private int _id;
        private string _login;
        private string _password;
        private string _dbName;
        private string _dbLogin;
        private string _dbPassword;
        private string _organisationName;
        private string _address;
        private string _email;
        private string _phone;
        #endregion fields

        #region properties
        public int ID
        {
            get { return _id; }
            set { if (_id != value) { _id = value; } }
        }
        [Required(ErrorMessage = "verplicht")]
        public string Login
        {
            get { return _login; }
            set { if (_login != value) { _login = value; ValidateProperty(value); } }
        }
        [Required(ErrorMessage = "verplicht")]
        public string Password
        {
            get { return _password; }
            set { if (_password != value) { _password = value; ValidateProperty(value); } }
        }
        [Required(ErrorMessage = "verplicht")]
        public string DbName
        {
            get { return _dbName; }
            set { if (_dbName != value) { _dbName = value; ValidateProperty(value); } }
        }
        [Required(ErrorMessage = "verplicht")]
        public string DbLogin
        {
            get { return _dbLogin; }
            set { if (_dbLogin != value) { _dbLogin = value; ValidateProperty(value); } }
        }
        [Required(ErrorMessage = "verplicht")]
        public string DbPassword
        {
            get { return _dbPassword; }
            set { if (_dbPassword != value) { _dbPassword = value; ValidateProperty(value); } }
        }
        [Required(ErrorMessage = "verplicht")]
        [RegularExpression(ValidationPaterns.ALPHANUMERICSPECIAL, ErrorMessage = "alfanumeriek + &-_'+")]
        [StringLength(50,MinimumLength = 3, ErrorMessage = "tussen 3 en 50 karakters")]
        public string OrganisationName
        {
            get { return _organisationName; }
            set { if (_organisationName != value) { _organisationName = value; ValidateProperty(value); } }
        }
        [Required(ErrorMessage = "verplicht")]
        [RegularExpression(ValidationPaterns.ADDRESS, ErrorMessage = "(Straat nr, )(code )gemeente")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "tussen 3 en 50 karakters")]
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
        #endregion properties

        #region constructor
        public Organisation()
        {
        }
        #endregion constructor

    }
}
