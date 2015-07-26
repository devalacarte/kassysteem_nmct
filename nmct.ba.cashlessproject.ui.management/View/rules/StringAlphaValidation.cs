using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace nmct.ba.cashlessproject.ui.management.View.rules
{
    class StringAlphaValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            string s = (string)value;
            if (string.IsNullOrEmpty(s))
                return new ValidationResult(false, "Verplicht");
            else if (s.ToCharArray().All(c => Char.IsLetter(c))==false)
                return new ValidationResult(false, "Enkel letters");
            else
                return new ValidationResult(true, null);
        }
    }
}
