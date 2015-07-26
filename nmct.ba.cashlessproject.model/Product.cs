using nmct.ba.cashlessproject.model.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace nmct.ba.cashlessproject.model
{
    public class Product : ValidationBase
    {
        #region fields
        private int _id;
        private string _productName;
        private double _price;
        #endregion fields

        #region properties
        public int ID
        {
            get { return _id; }
            set { if (_id != value) { _id = value; } }
        }
        [Required(ErrorMessage = "verplicht")]
        [RegularExpression(ValidationPaterns.ALPHANUMERICSPECIAL, ErrorMessage = "alfanumeriek + &-_'+")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "tussen 3 en 50 karakters")]
        public string ProductName
        {
            get { return _productName; }
            set { if (_productName != value) { _productName = value; ValidateProperty(value); } }
        }
        [Required(ErrorMessage = "verplicht")]
        [Range(0.01,100.00,ErrorMessage="tussen 0.01 en 100")]
        public double Price
        {
            get { return _price; }
            set { if (_price != value) { _price = value; ValidateProperty(value); } }
        }
        #endregion properties

        #region constructor
        public Product()
        {
            ProductName = String.Empty;
            Price = 0;
        }
        #endregion constructor
    }
}
