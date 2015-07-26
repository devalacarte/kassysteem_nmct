using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace nmct.ba.cashlessproject.model.Validation
{
    sealed public class ExactNumbers : ValidationAttribute
    {
        private int exactNumbers;
        public ExactNumbers(int exactNumbers)
        {
            this.exactNumbers = exactNumbers;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string pattern = @"^[0-9]{" + exactNumbers + "}$";
            bool result = Regex.IsMatch(value.ToString(), pattern);
            if (result)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("");
            }
        }
    }

    sealed public class MinValue : ValidationAttribute
    {
        private int minValue;
        public MinValue(int minValue)
        {
            this.minValue = minValue;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return (int.Parse(value.ToString()) > minValue) ? ValidationResult.Success : new ValidationResult("");
        }
    }

    sealed public class MaxValue : ValidationAttribute
    {
        private int maxValue;
        public MaxValue(int maxValue)
        {
            this.maxValue = maxValue;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return (int.Parse(value.ToString()) < maxValue) ? ValidationResult.Success : new ValidationResult("");
        }
    }
}
