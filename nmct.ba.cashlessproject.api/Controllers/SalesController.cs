using nmct.ba.cashlessproject.api.Models;
using nmct.ba.cashlessproject.model;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace nmct.ba.cashlessproject.api.Controllers
{
    public class SalesController : ApiController
    {
        public List<Sale> Get()
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            return SalesDA.GetSales(p.Claims);
        }

        /*
        public List<Sale> Get(Customer c)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            return SalesDA.GetSalesByCustomerID(c, p.Claims);
        }

        public List<Sale> Get(Register r)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            return SalesDA.GetSalesByRegisterID(r, p.Claims);
        }

        public List<Sale> Get(Product pr)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            return SalesDA.GetSalesByProductID(pr, p.Claims);
        }

        public List<Sale> Get(Customer c, Register r, Product pr)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            return SalesDA.GetSalesByCustRegProdID(c,r,pr,p.Claims);
        }
        */
        public HttpResponseMessage Post(Sale s)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            int id = SalesDA.InsertSale(s, p.Claims);

            HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.OK);
            message.Content = new StringContent(id.ToString());
            return message;
        }

        public HttpResponseMessage Put(Sale s)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            SalesDA.UpdateSale(s, p.Claims);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public HttpResponseMessage Delete(int id)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            SalesDA.DeleteSale(id, p.Claims);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}