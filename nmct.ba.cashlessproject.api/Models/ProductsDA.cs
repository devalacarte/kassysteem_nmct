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
    public class ProductsDA
    {
        private static ConnectionStringSettings CreateConnectionString(IEnumerable<Claim> claims)
        {
            string dblogin = claims.FirstOrDefault(c => c.Type == "dblogin").Value;
            string dbpass = claims.FirstOrDefault(c => c.Type == "dbpass").Value;
            string dbname = claims.FirstOrDefault(c => c.Type == "dbname").Value;

            return Database.CreateConnectionString("System.Data.SqlClient", @"LUNALAPPY\SQLEXPRESS", Cryptography.Decrypt(dbname), Cryptography.Decrypt(dblogin), Cryptography.Decrypt(dbpass));
        }

        public static List<Product> GetProducts(IEnumerable<Claim> claims)
        {
            List<Product> list = new List<Product>();
            string sql = "SELECT * FROM Product";
            DbDataReader reader = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql);
            while (reader.Read())
            {
                list.Add(Create(reader));
            }
            reader.Close();
            return list;
        }

        public static Product GetProductByID(int id, IEnumerable<Claim> claims)
        {
            string sql = "SELECT * FROM Product WHERE ID=@ID";
            DbParameter parID = Database.AddParameter(Database.ADMIN_DB, "@ID", id);
            DbDataReader reader = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql, parID);
            reader.Read();
            Product p = Create(reader);
            reader.Close();
            return p;
        }

        public static int InsertProduct(Product p , IEnumerable<Claim> claims)
        {
            string sql = "INSERT INTO Product VALUES(@ProductName,@Price)";
            DbParameter par1 = Database.AddParameter(Database.ADMIN_DB, "@ProductName", p.ProductName);
            DbParameter par2 = Database.AddParameter(Database.ADMIN_DB, "@Price", p.Price);
            return Database.InsertData(Database.GetConnection(CreateConnectionString(claims)), sql, par1, par2);
        }

        public static int UpdateProduct(Product p, IEnumerable<Claim> claims)
        {
            string sql = "UPDATE Product SET ProductName=@ProductName, Price=@Price WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter(Database.ADMIN_DB, "@ProductName", p.ProductName);
            DbParameter par2 = Database.AddParameter(Database.ADMIN_DB, "@Price", p.Price);
            DbParameter par3 = Database.AddParameter(Database.ADMIN_DB, "@ID", p.ID);
            return Database.ModifyData(Database.GetConnection(CreateConnectionString(claims)), sql, par1, par2, par3);
        }

        public static int DeleteProduct(int id, IEnumerable<Claim> claims)
        {
            string sql = "DELETE FROM Product WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter(Database.ADMIN_DB, "@ID", id);
            return Database.ModifyData(Database.GetConnection(CreateConnectionString(claims)), sql, par1);
        }

        private static Product Create(IDataRecord record)
        {
            return new Product()
            {
                ID = Int32.Parse(record["ID"].ToString()),
                ProductName = record["ProductName"].ToString(),
                Price = Double.Parse(record["Price"].ToString())
            };
        }
    }
}