using nmct.ba.cashlessproject.model.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace nmct.ba.cashlessproject.model
{
    public class RegisterEmployee : ValidationBase
    {
        #region fields
        private Register _registerID;
        private Employee _employeeID;
        private long _timeFrom;
        private long _timeTill;
        #endregion fields

        #region properties
        [Required(ErrorMessage = "verplicht")]
        public Register RegisterID
        {
            get { return _registerID; }
            set { if (_registerID != value) { _registerID = value; ValidateProperty(value); } }
        }
        [Required(ErrorMessage = "verplicht")]
        public Employee EmployeeID
        {
            get { return _employeeID; }
            set { if (_employeeID != value) { _employeeID = value; ValidateProperty(value); } }
        }
        [Required(ErrorMessage = "verplicht")]
        public long TimeFrom
        {
            get { return _timeFrom; }
            set { if (_timeFrom != value) { _timeFrom = value; ValidateProperty(value); } }
        }
        [Required(ErrorMessage = "verplicht")]
        public long TimeTill
        {
            get { return _timeTill; }
            set { if (_timeTill != value) { _timeTill = value; ValidateProperty(value); } }
        }
        #endregion properties

        #region constructor
        public RegisterEmployee()
        {
            RegisterID = null;
            EmployeeID = null;
            TimeFrom = 0;
            TimeTill = 0;
        }
        #endregion constructor
    }
}
