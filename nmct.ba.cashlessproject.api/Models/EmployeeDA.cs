using nmct.ba.cashlessproject.api.Helper;
using nmct.ba.cashlessproject.model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace nmct.ba.cashlessproject.api.Models
{
    public class EmployeeDA
    {
        private static ConnectionStringSettings CreateConnectionString(IEnumerable<Claim> claims)
        {
            string dblogin = claims.FirstOrDefault(c => c.Type == "dblogin").Value;
            string dbpass = claims.FirstOrDefault(c => c.Type == "dbpass").Value;
            string dbname = claims.FirstOrDefault(c => c.Type == "dbname").Value;

            return Database.CreateConnectionString("System.Data.SqlClient", @"LUNALAPPY\SQLEXPRESS", Cryptography.Decrypt(dbname), Cryptography.Decrypt(dblogin), Cryptography.Decrypt(dbpass));
        }

        public static List<Employee> GetEmployees(IEnumerable<Claim> claims)
        {
            List<Employee> list = new List<Employee>();
            string sql = "SELECT * FROM Employee";
            DbDataReader reader = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql);
            while (reader.Read())
            {
                list.Add(Create(reader));
            }
            reader.Close();
            return list;
        }

        public static Employee GetEmployeeByID(int id, IEnumerable<Claim> claims)
        {
            string sql = "SELECT * FROM Employee WHERE ID=@ID";
            DbParameter parID = Database.AddParameter(Database.ADMIN_DB, "@ID", id);
            DbDataReader reader = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql, parID);
            reader.Read();
            Employee e = Create(reader);
            reader.Close();
            return e;
        }

        public static Employee GetEmployeeByNameAndPass(string name, string pass, IEnumerable<Claim> claims)
        {
            string sql = "SELECT * FROM Employee WHERE EmployeeName = @EmployeeName AND Password = @pass";
            DbParameter par1 = Database.AddParameter(Database.ADMIN_DB, "@EmployeeName", name);
            DbParameter par2 = Database.AddParameter(Database.ADMIN_DB, "@pass", pass);
            DbDataReader reader = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql, par1, par2);
            reader.Read();
            Employee e = Create(reader);
            reader.Close();
            return e;
        }

        public static int InsertEmployee(Employee e, IEnumerable<Claim> claims)
        {
            string sql = "INSERT INTO Employee VALUES(@EmployeeName,@Address,@Email,@Phone,@Pass)";
            DbParameter par1 = Database.AddParameter(Database.ADMIN_DB, "@EmployeeName", e.EmployeeName);
            DbParameter par2 = Database.AddParameter(Database.ADMIN_DB, "@Address", e.Address);
            DbParameter par3 = Database.AddParameter(Database.ADMIN_DB, "@Email", e.Email);
            DbParameter par4 = Database.AddParameter(Database.ADMIN_DB, "@Phone", e.Phone);
            DbParameter par5 = Database.AddParameter(Database.ADMIN_DB, "@Pass", e.Password);
            return Database.InsertData(Database.GetConnection(CreateConnectionString(claims)), sql, par1, par2, par3, par4, par5);
        }

        public static int UpdateEmployee(Employee e, IEnumerable<Claim> claims)
        {
            string sql = "UPDATE Employee SET EmployeeName=@EmployeeName, Address=@Address, Email=@Email, Phone=@Phone WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter(Database.ADMIN_DB, "@EmployeeName", e.EmployeeName);
            DbParameter par2 = Database.AddParameter(Database.ADMIN_DB, "@Address", e.Address);
            DbParameter par3 = Database.AddParameter(Database.ADMIN_DB, "@Email", e.Email);
            DbParameter par4 = Database.AddParameter(Database.ADMIN_DB, "@Phone", e.Phone);
            DbParameter par5 = Database.AddParameter(Database.ADMIN_DB, "@ID", e.ID);
            return Database.ModifyData(Database.GetConnection(CreateConnectionString(claims)), sql, par1, par2, par3, par4, par5);
        }
    
        public static int DeleteEmployee(int id, IEnumerable<Claim> claims)
        {
            string sql = "DELETE FROM Employee WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter(Database.ADMIN_DB, "@ID", id);
            return Database.ModifyData(Database.GetConnection(CreateConnectionString(claims)), sql, par1);
        }

        private static Employee Create(IDataRecord record)
        {
            return new Employee()
            {
                ID = Int32.Parse(record["ID"].ToString()),
                EmployeeName = record["EmployeeName"].ToString(),
                Address = record["Address"].ToString(),
                Email = record["Email"].ToString(),
                Phone = record["Phone"].ToString(),
                Password = record["Password"].ToString()
            };
        }
    }
}