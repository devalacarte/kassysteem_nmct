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
    public class RegistersDA
    {

        #region withclaims for desktop
        private static ConnectionStringSettings CreateConnectionString(IEnumerable<Claim> claims)
        {
            string dblogin = claims.FirstOrDefault(c => c.Type == "dblogin").Value;
            string dbpass = claims.FirstOrDefault(c => c.Type == "dbpass").Value;
            string dbname = claims.FirstOrDefault(c => c.Type == "dbname").Value;

            return Database.CreateConnectionString("System.Data.SqlClient", @"LUNALAPPY\SQLEXPRESS", Cryptography.Decrypt(dbname), Cryptography.Decrypt(dblogin), Cryptography.Decrypt(dbpass));
        }

        public static List<Register> GetRegisters(IEnumerable<Claim> claims)
        {
            List<Register> list = new List<Register>();
            string sql = "SELECT * FROM Register";
            DbDataReader reader = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql);

            while (reader.Read())
            {
                list.Add(CreateClient(reader));
            }
            reader.Close();
            return list;
        }

        public static Register GetRegisterByID(int id, IEnumerable<Claim> claims)
        {
            Register reg = new Register();
            string sql = "SELECT * FROM Register WHERE ID=@ID";
            DbParameter parID = Database.AddParameter(Database.ADMIN_DB, "@ID", id);
            DbDataReader reader = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql, parID);
            reader.Read();
            reg = CreateClient(reader);
            reader.Close();
            return reg;
        }

        public static int InsertRegister(Register reg, IEnumerable<Claim> claims)
        {
            string sql = "INSERT INTO Register VALUES(@RegisterName,@Device,@PurchaseDate,@ExpiresDate)";
            DbParameter par1 = Database.AddParameter(Database.ADMIN_DB, "@RegisterName", reg.RegisterName);
            DbParameter par2 = Database.AddParameter(Database.ADMIN_DB, "@Device", reg.Device);
            return Database.InsertData(Database.GetConnection(CreateConnectionString(claims)), sql, par1, par2);
        }

        public static int UpdateRegister(Register reg, IEnumerable<Claim> claims)
        {
            string sql = "UPDATE Register SET RegisterName=@RegisterName, Device=@Device WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter(Database.ADMIN_DB, "@RegisterName", reg.RegisterName);
            DbParameter par2 = Database.AddParameter(Database.ADMIN_DB, "@Device", reg.Device);
            DbParameter parID = Database.AddParameter(Database.ADMIN_DB, "@ID", reg.ID);
            return Database.ModifyData(Database.GetConnection(CreateConnectionString(claims)), sql, par1, par2, parID);
        }

        public static int DeleteRegister(int id, IEnumerable<Claim> claims)
        {
            string sql = "DELETE FROM Register WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter(Database.ADMIN_DB, "@ID", id);
            return Database.ModifyData(Database.GetConnection(CreateConnectionString(claims)), sql, par1);
        }

        private static Register CreateClient(IDataRecord record)
        {
            return new Register()
            {
                ID = Int32.Parse(record["ID"].ToString()),
                RegisterName = record["RegisterName"].ToString(),
                Device = record["Device"].ToString()
            };
        }
        #endregion withclaims for desktop


        #region without claims for web
        public static List<Register> GetRegisters()
        {
            List<Register> list = new List<Register>();
            string sql = "SELECT * FROM Register";
            DbDataReader reader = Database.GetData(Database.GetConnection(Database.ADMIN_DB), sql);

            while (reader.Read())
            {
                list.Add(Create(reader));
            }
            reader.Close();
            return list;
        }

        public static Register GetRegisterByID(int id)
        {
            Register reg = new Register();
            string sql = "SELECT * FROM Register WHERE ID=@ID";
            DbParameter parID = Database.AddParameter(Database.ADMIN_DB, "@ID", id);
            DbDataReader reader = Database.GetData(Database.GetConnection(Database.ADMIN_DB), sql, parID);
            while (reader.Read())
            {
                reg = Create(reader);
            }
            reader.Close();
            return reg;
        }

        public static int InsertRegister(Register reg)
        {
            string sql = "INSERT INTO Register VALUES(@RegisterName,@Device,@PurchaseDate,@ExpiresDate)";
            DbParameter par1 = Database.AddParameter(Database.ADMIN_DB, "@RegisterName", reg.RegisterName);
            DbParameter par2 = Database.AddParameter(Database.ADMIN_DB, "@Device", reg.Device);
            DbParameter par3 = Database.AddParameter(Database.ADMIN_DB, "@PurchaseDate", reg.PurchaseDate);
            DbParameter par4 = Database.AddParameter(Database.ADMIN_DB, "@ExpiresDate", reg.ExpiresDate);
            return Database.InsertData(Database.GetConnection(Database.ADMIN_DB), sql, par1, par2, par3, par4);
        }

        public static int UpdateRegister(Register reg)
        {
            string sql = "UPDATE Register SET RegisterName=@RegisterName, Device=@Device, PurchaseDate=@PurchaseDate, ExpiresDate=@ExpiresDate WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter(Database.ADMIN_DB, "@RegisterName", reg.RegisterName);
            DbParameter par2 = Database.AddParameter(Database.ADMIN_DB, "@Device", reg.Device);
            DbParameter par3 = Database.AddParameter(Database.ADMIN_DB, "@PurchaseDate", reg.PurchaseDate);
            DbParameter par4 = Database.AddParameter(Database.ADMIN_DB, "@ExpiresDate", reg.ExpiresDate);
            DbParameter parID = Database.AddParameter(Database.ADMIN_DB, "@ID", reg.ID);
            return Database.ModifyData(Database.GetConnection(Database.ADMIN_DB), sql, par1, par2, par3, par4, parID);
        }

        public static int DeleteRegister(int id)
        {
            string sql = "DELETE FROM Register WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter(Database.ADMIN_DB, "@ID", id);
            return Database.ModifyData(Database.GetConnection(Database.ADMIN_DB), sql, par1);
        }

        private static Register Create(IDataRecord record)
        {
            return new Register()
            {
                ID = Int32.Parse(record["ID"].ToString()),
                RegisterName = record["RegisterName"].ToString(),
                Device = record["Device"].ToString(),
                PurchaseDate = Int32.Parse(record["PurchaseDate"].ToString()),
                ExpiresDate = Int32.Parse(record["ExpiresDate"].ToString())
            };
        }
        #endregion without claims for web
        
    }
}