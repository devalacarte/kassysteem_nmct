using nmct.ba.cashlessproject.api.Models;
using nmct.ba.cashlessproject.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace nmct.ba.cashlessproject.api.Controllers
{
    public class OrganisationController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<Organisation> Get()
        {
            return OrganisationDA.GetOrganisations();
        }

        // GET api/<controller>/5
        public Organisation Get(int id)
        {
            return OrganisationDA.GetOrganisationByID(id);
        }

        public Organisation Get(string user, string pass)
        {
            return OrganisationDA.CheckCredentials(user, pass);
        }

        // POST api/<controller>
        public int Post(Organisation org)
        {
            return OrganisationDA.InsertOrganisation(org);
        }

        // PUT api/<controller>/5
        public int Put(Organisation org)
        {
            return OrganisationDA.UpdateOrganisation(org);
        }

        [Route("changepass")]
        public HttpResponseMessage Put(string user, string pass)
        {
            OrganisationDA.ChangePassword(user, pass);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}