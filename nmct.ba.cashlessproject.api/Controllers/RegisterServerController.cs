using nmct.ba.cashlessproject.api.Models;
using nmct.ba.cashlessproject.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace nmct.ba.cashlessproject.api.Controllers
{
    public class RegisterServerController : ApiController
    {
        public List<Register> Get()
        {
            return RegistersDA.GetRegisters();
        }

        public Register Get(int id)
        {
            return RegistersDA.GetRegisterByID(id);
        }

        public int Post(Register r)
        {
            return RegistersDA.InsertRegister(r);
        }

        public int Put(Register r)
        {
            return RegistersDA.UpdateRegister(r);
        }

        public int Delete(int id)
        {
            return RegistersDA.DeleteRegister(id);
        }
    }
}