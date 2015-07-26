using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace nmct.ba.cashlessproject.ui.customer.View.converters
{
    class UnixConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                DateTime t = UnixTimestampToDateTime(long.Parse(value.ToString()));
                return t;
            }
            else
                return "not set";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }

        public DateTime UnixTimestampToDateTime(long unix)
        {
            return (new DateTime(1970, 1, 1, 0, 0, 0)).AddSeconds(unix);
        }

        public long DateTimeToUnixTimestamp(DateTime dateTime)
        {
            TimeSpan unix = (dateTime - new DateTime(1970, 1, 1, 0, 0, 0));
            return (long)unix.TotalSeconds;
        }
    }
}
