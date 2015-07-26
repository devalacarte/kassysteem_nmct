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
    public class ProductsController : ApiController
    {
        public List<Product> Get()
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            return ProductsDA.GetProducts(p.Claims);
        }

        public Product Get(int id)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            return ProductsDA.GetProductByID(id, p.Claims);
        }

        public HttpResponseMessage Post(Product pr)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            int id = ProductsDA.InsertProduct(pr, p.Claims);

            HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.OK);
            message.Content = new StringContent(id.ToString());
            return message;
        }

        public HttpResponseMessage Put(Product pr)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            ProductsDA.UpdateProduct(pr, p.Claims);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public HttpResponseMessage Delete(int id)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            ProductsDA.DeleteProduct(id, p.Claims);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}