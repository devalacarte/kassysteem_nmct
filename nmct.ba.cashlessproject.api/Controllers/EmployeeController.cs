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
    [RoutePrefix("api/employee")]
    public class EmployeeController : ApiController
    {
        public List<Employee> Get()
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            return EmployeeDA.GetEmployees(p.Claims);
        }

        public Employee Get(int id)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            return EmployeeDA.GetEmployeeByID(id, p.Claims);
        }

        //api/{controller}/{id}
        [Route("checkemployee")]
        [HttpGet]
        public Employee CheckEmployee(string name, string password)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            return EmployeeDA.GetEmployeeByNameAndPass(name, password, p.Claims);
        }

        public HttpResponseMessage Post(Employee e)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            int id = EmployeeDA.InsertEmployee(e, p.Claims);

            HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.OK);
            message.Content = new StringContent(id.ToString());
            return message;
        }

        public HttpResponseMessage Put(Employee e)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            EmployeeDA.UpdateEmployee(e, p.Claims);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public HttpResponseMessage Delete(int id)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            EmployeeDA.DeleteEmployee(id, p.Claims);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}