using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nmct.ba.cashlessproject.model
{
    public class ErrorLog
    {
        #region fields
        private int _registerId;
        private string _timeStamp;
        private string _message;
        private string _stacktrace;
        #endregion fields

        #region properties
        public int RegisterID
        {
            get { return _registerId; }
            set { if (_registerId != value) { _registerId = value; } }
        }
        public string TimeStamp
        {
            get { return _timeStamp; }
            set { if (_timeStamp != value) { _timeStamp = value; } }
        }
        public string Message
        {
            get { return _message; }
            set { if (_message != value) { _message = value; } }
        }
        public string Stacktrace
        {
            get { return _stacktrace; }
            set { if (_stacktrace != value) { _stacktrace = value; } }
        }
        #endregion properties

        #region constructors
        public ErrorLog()
        {

        }
        #endregion constructors
    }
}
