using nmct.ba.cashlessproject.api.Models;
using nmct.ba.cashlessproject.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace nmct.ba.cashlessproject.api.Controllers
{
    public class ErrorlogClientController : ApiController
    {
        public List<ErrorLog> Get()
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            return ErrorlogDA.GetErrorLogs(p.Claims);
        }

        public ErrorLog Get(int id)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            return ErrorlogDA.GetErrorLogsByRegID(id, p.Claims);
        }

        public HttpResponseMessage Post(ErrorLog e)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            int id = ErrorlogDA.InsertErrorLog(e, p.Claims);

            HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.OK);
            message.Content = new StringContent(id.ToString());
            return message;
        }
        /*
        public HttpResponseMessage Put(ErrorLog e)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            ErrorlogDA.UpdateErrorLog(e, p.Claims);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
        */

        /*
        public HttpResponseMessage Delete(int id)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            ErrorlogDA.DeleteErrorLog(id, p.Claims);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
        */
    }
}