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
    public class RegistersEmployeeController : ApiController
    {
        public List<RegisterEmployee> Get()
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            return RegisterEmployeeDA.GetRegisterEmployee(p.Claims);
        }

        public List<RegisterEmployee> Get(int registerID)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            return RegisterEmployeeDA.GetRegisterEmployeeByRegisterID(registerID,p.Claims);
        }

        public HttpResponseMessage Post(RegisterEmployee re)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            int id = RegisterEmployeeDA.InsertRegisterEmployee(re, p.Claims);

            HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.OK);
            message.Content = new StringContent(id.ToString());
            return message;
        }

        public HttpResponseMessage Put(RegisterEmployee re)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            RegisterEmployeeDA.UpdateRegisterEmployee(re, p.Claims);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public HttpResponseMessage Delete(int registerID, int employeeID)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            RegisterEmployeeDA.DeleteRegisterEmployee(registerID,employeeID, p.Claims);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}