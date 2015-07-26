using nmct.ba.cashlessproject.model.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace nmct.ba.cashlessproject.model
{
    public class Customer : ValidationBase
    {
        #region fields
        private int _id;
        private string _customerName;
        private string _address;
        private byte[] _picture;
        private double _balance;
        private string _cardid;
        #endregion fields

        #region properties
        public int ID
        {
            get { return _id; }
            set { if (_id != value) { _id = value; } }
        }

        [Required(ErrorMessage = "verplicht")]
        [RegularExpression(ValidationPaterns.NAME,ErrorMessage = "geen speciale tekens")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "tussen de 3 en 50 karakters")]
        public string CustomerName
        {
            get { return _customerName; }
            set { if (_customerName != value) { _customerName = value; ValidateProperty(value);} }
        }
        [Required(ErrorMessage = "verplicht")]
        [RegularExpression(ValidationPaterns.ADDRESS, ErrorMessage = "(Straat nr, )(code )gemeente")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "tussen de 3 en 50 karakters")]
        public string Address
        {
            get { return _address; }
            set { if (_address != value) { _address = value; ValidateProperty(value); } }
        }
        public byte[] Picture
        {
            get { return _picture; }
            set { if (_picture != value) { _picture = value; } }
        }
        [Required(ErrorMessage = "verplicht")]
        [Range(0.00,100.00,ErrorMessage = "tussen 0 en 100")]
        public double Balance
        {
            get { return _balance; }
            set { if (_balance != value) { _balance = value; ValidateProperty(value); } }
        }
        [Required(ErrorMessage = "verplicht")]
        public string CardID
        {
            get { return _cardid; }
            set { if (_cardid != value) { _cardid = value; ValidateProperty(value); } }
        }
        #endregion properties

        #region constructor
        public Customer()
        {
            CustomerName = String.Empty;
            Address = String.Empty;
            Balance = 0;
        }
        #endregion constructor
    }
}
