using nmct.ba.cashlessproject.model.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace nmct.ba.cashlessproject.model
{
    public class Register : ValidationBase
    {
        #region fields
        private int _id;
        private string _registerName;
        private string _device;
        private int _purchaseDate;
        private int _expiresDate;
        #endregion fields

        #region properties
        public int ID
        {
            get { return _id; }
            set { if (_id != value) { _id = value; } }
        }
        [Required(ErrorMessage = "verplicht")]
        [RegularExpression(ValidationPaterns.ALPHANUMERIC, ErrorMessage = "alfanumeriek")]
        public string RegisterName
        {
            get { return _registerName; }
            set { if (_registerName != value) { _registerName = value; ValidateProperty(value); } }
        }
        [Required(ErrorMessage = "verplicht")]
        [RegularExpression(ValidationPaterns.ALPHANUMERIC, ErrorMessage = "alfanumeriek")]
        public string Device
        {
            get { return _device; }
            set { if (_device != value) { _device = value; ValidateProperty(value); } }
        }
        [Required(ErrorMessage = "verplicht")]
        [MinValue(0,ErrorMessage="Niet 0")]
        public int PurchaseDate
        {
            get { return _purchaseDate; }
            set { if (_purchaseDate != value) { _purchaseDate = value; ValidateProperty(value); } }
        }
        [Required(ErrorMessage = "verplicht")]
        [MinValue(0, ErrorMessage = "Niet 0")]
        public int ExpiresDate
        {
            get { return _expiresDate; }
            set { if (_expiresDate != value) { _expiresDate = value; ValidateProperty(value); } }
        }
        #endregion properties

        #region consturctors
        public Register()
        {
            RegisterName = String.Empty;
            Device = String.Empty;
            PurchaseDate = 0;
            ExpiresDate = 0;
        }
        #endregion constructors
    }
}
