using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace nmct.ba.cashlessproject.ui.management.View.rules
{
    class BalanceValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            double d;
            if(value==null)
                return new ValidationResult(false, "Verplicht");


            if (Double.TryParse((value.ToString().Replace('.',',')), out d))
            {
                if (d < 0)
                    return new ValidationResult(false, "Het saldo moet 0 of hoger zijn");
                else if (d.ToString().Length > 18)
                    return new ValidationResult(false, "max 18 cijfers");
                else if (CountDigitsAfterDecimal(d) >2)
                    return new ValidationResult(false, "max 2 cijfers na komma");
                else
                    return new ValidationResult(true, null);
            }
            else
                return new ValidationResult(false, "Saldo moet een (komma)getal zijn");
        }

        private int CountDigitsAfterDecimal(double value)
        {
            bool start = false;
            int count = 0;
            foreach (var s in value.ToString())
            {
                if (s == ',')
                {
                    start = true;
                }
                else if (start)
                {
                    count++;
                }
            }

            return count;
        }
    }
}
