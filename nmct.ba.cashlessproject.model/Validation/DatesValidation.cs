using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace nmct.ba.cashlessproject.model.Validation
{
    sealed public class DateStartToday : ValidationAttribute
    {
        private DateTime date;
        public DateStartToday(DateTime date)
        {
            this.date = date;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
           if (value == null)
                new ValidationResult("ConversionError");
            try
            {
                date = Convert.ToDateTime(value);
            }
            catch (FormatException)
            {
                return new ValidationResult("ConversionError");
            }
            if (date > DateTime.Now)
                return ValidationResult.Success;
            else
                return new ValidationResult("Early date");
        }
        }
    
}
