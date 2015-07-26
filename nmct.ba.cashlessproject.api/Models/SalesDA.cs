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
    public class SalesDA
    {
        private static ConnectionStringSettings CreateConnectionString(IEnumerable<Claim> claims)
        {
            string dblogin = claims.FirstOrDefault(c => c.Type == "dblogin").Value;
            string dbpass = claims.FirstOrDefault(c => c.Type == "dbpass").Value;
            string dbname = claims.FirstOrDefault(c => c.Type == "dbname").Value;

            return Database.CreateConnectionString("System.Data.SqlClient", @"LUNALAPPY\SQLEXPRESS", Cryptography.Decrypt(dbname), Cryptography.Decrypt(dblogin), Cryptography.Decrypt(dbpass));
        }

        public static List<Sale> GetSales(IEnumerable<Claim> claims)
        {
            List<Sale> list = new List<Sale>();
            string sql = "SELECT * FROM Sale";
            DbDataReader reader = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql);
            while (reader.Read())
            {
                list.Add(Create(reader));
            }
            reader.Close();
            return list;
        }

        public static Sale GetSalesByID(int id, IEnumerable<Claim> claims)
        {
            string sql = "SELECT * FROM Sale WHERE ID=@ID";
            DbParameter parID = Database.AddParameter(Database.ADMIN_DB, "@ID", id);
            DbDataReader reader = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql, parID);
            reader.Read();
            Sale s = Create(reader);
            reader.Close();
            return s;
        }

        public static List<Sale> GetSalesByCustomerID(Customer c, IEnumerable<Claim> claims)
        {
            List<Sale> list = new List<Sale>();
            string sql = "SELECT * FROM Sale WHERE CustomerID=@ID";
            DbParameter parID = Database.AddParameter(Database.ADMIN_DB, "@ID", c.ID);
            DbDataReader reader = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql, parID);
            while (reader.Read())
            {
                list.Add(Create(reader));
            }
            reader.Close();
            return list;
        }

        public static List<Sale> GetSalesByRegisterID(Register r, IEnumerable<Claim> claims)
        {
            List<Sale> list = new List<Sale>();
            string sql = "SELECT * FROM Sale WHERE RegisterID=@ID";
            DbParameter parID = Database.AddParameter(Database.ADMIN_DB, "@ID", r.ID);
            DbDataReader reader = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql,parID);
            while (reader.Read())
            {
                list.Add(Create(reader));
            }
            reader.Close();
            return list;
        }

        public static List<Sale> GetSalesByProductID(Product p, IEnumerable<Claim> claims)
        {
            List<Sale> list = new List<Sale>();
            string sql = "SELECT * FROM Sale WHERE ProductID=@ID";
            DbParameter parID = Database.AddParameter(Database.ADMIN_DB, "@ID", p.ID);
            DbDataReader reader = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql, parID);
            while (reader.Read())
            {
                list.Add(Create(reader));
            }
            reader.Close();
            return list;
        }

        public static List<Sale> GetSalesByCustRegProdID(Customer c, Register r, Product p, IEnumerable<Claim> claims)
        {
            List<Sale> list = new List<Sale>();
            string sql = "SELECT * FROM Sale WHERE CustomerID=@CID And RegisterID=@RID And ProductID=@PID";
            DbParameter parCID = Database.AddParameter(Database.ADMIN_DB, "@CID", c.ID);
            DbParameter parRID = Database.AddParameter(Database.ADMIN_DB, "@RID", r.ID);
            DbParameter parPID = Database.AddParameter(Database.ADMIN_DB, "@PID", p.ID);
            DbDataReader reader = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql, parCID, parRID, parPID);
            while (reader.Read())
            {
                list.Add(Create(reader));
            }
            reader.Close();
            return list;
        }

        public static int InsertSale(Sale s, IEnumerable<Claim> claims)
        {
            string sql = "INSERT INTO Sale VALUES(@Timestamp,@CustomerID,@RegisterID,@ProductID,@Amount,@TotalPrice)";
            DbParameter par1 = Database.AddParameter(Database.ADMIN_DB, "@Timestamp", s.Timestamp);
            DbParameter par2 = Database.AddParameter(Database.ADMIN_DB, "@CustomerID", s.CustomerID);
            DbParameter par3 = Database.AddParameter(Database.ADMIN_DB, "@RegisterID", s.RegisterID);
            DbParameter par4 = Database.AddParameter(Database.ADMIN_DB, "@ProductID", s.ProductID);
            DbParameter par5 = Database.AddParameter(Database.ADMIN_DB, "@Amount", s.Amount);
            DbParameter par6 = Database.AddParameter(Database.ADMIN_DB, "@TotalPrice", s.TotalPrice);
            return Database.InsertData(Database.GetConnection(CreateConnectionString(claims)), sql, par1, par2, par3, par4, par5, par6);
        }

        public static int UpdateSale(Sale s, IEnumerable<Claim> claims)
        {
            string sql = "UPDATE Sale SET Timestamp=@Timestamp, CustomerID=@CustomerID, RegisterID=@RegisterID, ProductID=@ProductID, Amount=@Amount, TotalPrice=@TotalPrice WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter(Database.ADMIN_DB, "@Timestamp", s.Timestamp);
            DbParameter par2 = Database.AddParameter(Database.ADMIN_DB, "@CustomerID", s.CustomerID);
            DbParameter par3 = Database.AddParameter(Database.ADMIN_DB, "@RegisterID", s.RegisterID);
            DbParameter par4 = Database.AddParameter(Database.ADMIN_DB, "@ProductID", s.ProductID);
            DbParameter par5 = Database.AddParameter(Database.ADMIN_DB, "@Amount", s.Amount);
            DbParameter par6 = Database.AddParameter(Database.ADMIN_DB, "@TotalPrice", s.TotalPrice);
            DbParameter par7 = Database.AddParameter(Database.ADMIN_DB, "@ID", s.ID);
            return Database.ModifyData(Database.GetConnection(CreateConnectionString(claims)), sql, par1, par2, par3, par4, par5, par6, par7);
        }

        public static int DeleteSale(int id, IEnumerable<Claim> claims)
        {
            string sql = "DELETE FROM Sale WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter(Database.ADMIN_DB, "@ID", id);
            return Database.ModifyData(Database.GetConnection(CreateConnectionString(claims)), sql, par1);
        }

        private static Sale Create(IDataRecord record)
        {
            return new Sale()
            {
                ID = Int32.Parse(record["ID"].ToString()),
                Timestamp = long.Parse(record["Timestamp"].ToString()),
                CustomerID = Int32.Parse(record["CustomerID"].ToString()),
                RegisterID = Int32.Parse(record["RegisterID"].ToString()),
                ProductID = Int32.Parse(record["ProductID"].ToString()),
                Amount = Int32.Parse(record["Amount"].ToString()),
                TotalPrice = Double.Parse(record["TotalPrice"].ToString()),
            };
        }
    }
}