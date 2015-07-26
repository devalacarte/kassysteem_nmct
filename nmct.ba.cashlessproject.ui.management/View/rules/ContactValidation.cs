using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace nmct.ba.cashlessproject.ui.management.View.rules
{
    class AddressValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            string s = (string)value;
            char[] allowedChars = new char[]{'-', ' ', ',', '\'','(',')',':'};
            if (string.IsNullOrEmpty(s))
                return new ValidationResult(false, "Verplicht");
            else if (s.ToCharArray().All(c => Char.IsLetterOrDigit(c) || allowedChars.Contains(c)) == false)
                return new ValidationResult(false, "Geen speciale tekens");
            else
                return new ValidationResult(true, null);
        }
    }

    class FullNameValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            string s = (string)value;
            char[] allowedChars = new char[] { '-', ' ', ',','\''};
            if (string.IsNullOrEmpty(s))
                return new ValidationResult(false, "Verplicht");
            else if (s.ToCharArray().All(c => Char.IsLetter(c) || allowedChars.Contains(c)) == false)
                return new ValidationResult(false, "Alpha, spatie, - en '");
            else
                return new ValidationResult(true, null);
        }
    }

    class FistNameValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            string s = (string)value;
            char[] allowedChars = new char[] { '-' };
            if (string.IsNullOrEmpty(s))
                return new ValidationResult(false, "Verplicht");
            else if (s.ToCharArray().All(c => Char.IsLetter(c) || allowedChars.Contains(c)) == false)
                return new ValidationResult(false, "Alpha en -");
            else
                return new ValidationResult(true, null);
        }
    }

    class LastNameValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            string s = (string)value;
            char[] allowedChars = new char[] { '-', ' ', '\''};
            if (string.IsNullOrEmpty(s))
                return new ValidationResult(false, "Verplicht");
            else if (s.ToCharArray().All(c => Char.IsLetter(c) || allowedChars.Contains(c)) == false)
                return new ValidationResult(false, "Alphanumeriek, spatie - en'");
            else
                return new ValidationResult(true, null);
        }
    }

    class PhoneNumberValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            string s = (string)value;
            if (string.IsNullOrEmpty(s))
                return new ValidationResult(false, "Verplicht");
            //else if (s.ToCharArray().All(c => Char.IsDigit(c)) == false)
                //return new ValidationResult(false, "Numeriek");
            else if(IsValid(s)==false)
                return new ValidationResult(false, "Foutief formaat");
            else
                return new ValidationResult(true, null);
        }

        private bool IsValid(string s)
        {
            string[] regexes = new string[] { ValidationPaterns.PHONE_BELGIUM_FULL_FORMATTED, ValidationPaterns.PHONE_BELGIUM_ZONE_FORMATTED, ValidationPaterns.PHONE_BELGIUM_NOT_FORMATTED, ValidationPaterns.GSM_BELGIUM_FULL_FORMATTED, ValidationPaterns.GSM_BELGIUM_ZONE_FORMATTED, ValidationPaterns.GSM_BELGIUM_NOT_FORMATTED };
            foreach (string regex in regexes)
            {
                if(Regex.IsMatch(s,regex))
                    return true;
            }
            return false;
        }
    }

    class EmailAddressValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            string s = (string)value;
            if (string.IsNullOrEmpty(s))
                return new ValidationResult(false, "Verplicht");
            else if (Regex.IsMatch(s, ValidationPaterns.EMAIL)==false)
                return new ValidationResult(false, "Foutief formaat");
            else
                return new ValidationResult(true, null);
        }
    }

}
