using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.ui.management.View.rules
{
    public static class ValidationPaterns
    {

        public static string PHONE_BELGIUM_FULL_FORMATTED = @"^0\d{1,2}/\d{2}[.]\d{2}[.]\d{2}$";
        public static string PHONE_BELGIUM_ZONE_FORMATTED = @"^0\d{1,2}/\d{6}$";
        public static string PHONE_BELGIUM_NOT_FORMATTED = @"^0\d{1,2}\d{6}$";
        public static string GSM_BELGIUM_FULL_FORMATTED = @"^04\d{2}/\d{2}[.]\d{2}[.]\d{2}$";
        public static string GSM_BELGIUM_ZONE_FORMATTED = @"^04\d{2}/\d{6}$";
        public static string GSM_BELGIUM_NOT_FORMATTED = @"^04\d{8}$";

        //public static string EMAIL = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*" + "@" + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))\z";
        //public static string EMAIL = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";
        public static string EMAIL = @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@ (?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+(?:[A-Z]{2}|com|org|net|int|edu|gov|mil|arpa|biz|info|mobi|name|aero|asia|jobs|museum|it|nl|fr|de|es|uk|be)\b";
    }
}
