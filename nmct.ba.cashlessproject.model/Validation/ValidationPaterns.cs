using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nmct.ba.cashlessproject.model.Validation
{
    public static class ValidationPaterns
    {
        public const string ADDRESS = @"^([a-zA-Z']{1}[a-zA-Z\'\-\s()]+\s{1}\d{1,3}[,]?[\s])?(\d{4}[\s])?[a-zA-Z'\s]+$";
        public const string ALPHA = @"^[a-zA-Z]+$";
        public const string ALPHANUMERIC = @"^[a-zA-Z0-9]+$";
        public const string ALPHANUMERICSPECIAL = @"^[a-zA-Z0-9\'\s\-\&_\+\.]+$";
        //public const string EMAIL = @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@ (?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+(?:[A-Z]{2}|com|org|net|int|edu|gov|mil|arpa|biz|info|mobi|name|aero|asia|jobs|museum|it|nl|fr|de|es|uk|be)\b";
        //public const string EMAIL = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*" + "@" + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))\z";
        public const string EMAIL = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";
        public const string GSM_BELGIUM_FULL_FORMATTED = @"^04\d{2}/\d{2}[.]\d{2}[.]\d{2}$";
        public const string GSM_BELGIUM_ZONE_FORMATTED = @"^04\d{2}/\d{6}$";
        public const string GSM_BELGIUM_NOT_FORMATTED = @"^04\d{8}$";
        public const string NAME = @"^[a-zA-Z\'\-\s]+$";
        public const string NUMERIC = @"^[0-9]+$";
        public const string PHONE = @"(^0\d{1,2}/\d{2}[.]\d{2}[.]\d{2}$)|(^0\d{1,2}/\d{6}$)|(^0\d{1,2}\d{6}$)|(^04\d{2}/\d{2}[.]\d{2}[.]\d{2}$)|(^04\d{2}/\d{6}$)|(^04\d{2}/\d{6}$)";
        public const string PHONE_BELGIUM_FULL_FORMATTED = @"^0\d{1,2}/\d{2}[.]\d{2}[.]\d{2}$";
        public const string PHONE_BELGIUM_ZONE_FORMATTED = @"^0\d{1,2}/\d{6}$";
        public const string PHONE_BELGIUM_NOT_FORMATTED = @"^0\d{1,2}\d{6}$";


        
    }
}
