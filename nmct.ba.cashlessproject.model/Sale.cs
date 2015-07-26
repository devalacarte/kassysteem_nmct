using nmct.ba.cashlessproject.model.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace nmct.ba.cashlessproject.model
{
    public class Sale : ValidationBase
    {
        #region fields
        private int _id;
        private long _timestamp;
        private int _customerID;
        private int _registerID;
        private int _productID;
        private int _amount;
        private double _totalPrice;
        #endregion fields

        #region properties
        public int ID
        {
            get { return _id; }
            set { if (_id != value) { _id = value; } }
        }
        [Required(ErrorMessage = "verplicht")]
        public long Timestamp
        {
            get { return _timestamp; }
            set { if (_timestamp != value) { _timestamp = value; ValidateProperty(value); } }
        }
        [Required(ErrorMessage = "verplicht")]
        [MinValue(0, ErrorMessage = "hoger dan 0")]
        public int CustomerID
        {
            get { return _customerID; }
            set { if (_customerID != value) { _customerID = value; ValidateProperty(value); } }
        }
        [Required(ErrorMessage = "verplicht")]
        [MinValue(0, ErrorMessage = "hoger dan 0")]
        public int RegisterID
        {
            get { return _registerID; }
            set { if (_registerID != value) { _registerID = value; ValidateProperty(value); } }
        }
        [Required(ErrorMessage = "verplicht")]
        [MinValue(0, ErrorMessage = "hoger dan 0")]
        public int  ProductID
        {
            get { return _productID; }
            set { if (_productID != value) { _productID = value; ValidateProperty(value); } }
        }
        [Required(ErrorMessage = "verplicht")]
        [MinValue(0, ErrorMessage = "hoger dan 0")]
         public int  Amount
        {
            get { return _amount; }
            set { if (_amount != value) { _amount = value; ValidateProperty(value); } }
        }
        [Required(ErrorMessage = "verplicht")]
        [Range(0.01,100.00,ErrorMessage="tussen 0.01 en 100")]
        public double TotalPrice
        {
            get { return _totalPrice; }
            set { if (_totalPrice != value) { _totalPrice = value; ValidateProperty(value); } }
        }
        #endregion properties

        #region constructor
        public Sale()
        {
            Timestamp = 0;
            CustomerID = 0;
            RegisterID = 0;
            ProductID = 0;
            Amount = 0;
            TotalPrice = 0;
        }
        #endregion constructor
    }
}
