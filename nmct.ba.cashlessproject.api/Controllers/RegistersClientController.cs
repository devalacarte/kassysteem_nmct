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
    public class RegistersClientController : ApiController
    {
        public List<Register> Get()
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            return RegistersDA.GetRegisters(p.Claims);
        }

        public Register Get(int id)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            return RegistersDA.GetRegisterByID(id, p.Claims);
        }

        public HttpResponseMessage Post(Register r)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            int id = RegistersDA.InsertRegister(r, p.Claims);

            HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.OK);
            message.Content = new StringContent(id.ToString());
            return message;
        }

        public HttpResponseMessage Put(Register r)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            RegistersDA.UpdateRegister(r, p.Claims);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public HttpResponseMessage Delete(int id)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            RegistersDA.DeleteRegister(id, p.Claims);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}