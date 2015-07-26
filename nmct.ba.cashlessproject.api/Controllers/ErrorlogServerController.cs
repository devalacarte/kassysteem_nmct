using nmct.ba.cashlessproject.api.Models;
using nmct.ba.cashlessproject.model;
using System.Collections.Generic;
using System.Web.Http;

namespace nmct.ba.cashlessproject.api.Controllers
{
    public class ErrorlogServerController : ApiController
    {
        public List<ErrorLog> Get()
        {
            return ErrorlogDA.GetErrorLogs();
        }

        public ErrorLog Get(int id)
        {
            return ErrorlogDA.GetErrorLogsByRegID(id);
        }

        public int Post(ErrorLog e)
        {
            return ErrorlogDA.InsertErrorLog(e);
        }

        /*
        public int Put(ErrorLog e)
        {
            return ErrorlogDA.UpdateRegister(e);
        }

        public int Delete(int id)
        {
            return ErrorlogDA.DeleteRegister(id);
        }
         * */
    }
}