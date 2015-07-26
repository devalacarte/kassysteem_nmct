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
    public class ErrorlogDA
    {
        private static ConnectionStringSettings CreateConnectionString(IEnumerable<Claim> claims)
        {
            string dblogin = claims.FirstOrDefault(c => c.Type == "dblogin").Value;
            string dbpass = claims.FirstOrDefault(c => c.Type == "dbpass").Value;
            string dbname = claims.FirstOrDefault(c => c.Type == "dbname").Value;

            return Database.CreateConnectionString("System.Data.SqlClient", @"LUNALAPPY\SQLEXPRESS", Cryptography.Decrypt(dbname), Cryptography.Decrypt(dblogin), Cryptography.Decrypt(dbpass));
        }

        public static List<ErrorLog> GetErrorLogs(IEnumerable<Claim> claims)
        {
            List<ErrorLog> list = new List<ErrorLog>();
            string sql = "SELECT * FROM ErrorLog";
            DbDataReader reader = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql);

            while (reader.Read())
            {
                list.Add(Create(reader));
            }
            reader.Close();
            return list;
        }

        public static ErrorLog GetErrorLogsByRegID(int registerID, IEnumerable<Claim> claims)
        {
            ErrorLog reg = new ErrorLog();
            string sql = "SELECT * FROM ErrorLog WHERE ID=@ID";
            DbParameter parID = Database.AddParameter(Database.ADMIN_DB, "@ID", registerID);
            DbDataReader reader = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql, parID);
            while (reader.Read())
            {
                reg = Create(reader);
            }
            reader.Close();
            return reg;
        }

        public static int InsertErrorLog(ErrorLog err, IEnumerable<Claim> claims)
        {
            string sql = "INSERT INTO ErrorLog VALUES(@RegisterID,@Timestamp,@Message,@Stacktrace)";
            DbParameter par1 = Database.AddParameter(Database.ADMIN_DB, "@RegisterID", err.RegisterID);
            DbParameter par2 = Database.AddParameter(Database.ADMIN_DB, "@Timestamp", err.TimeStamp);
            DbParameter par3 = Database.AddParameter(Database.ADMIN_DB, "@Message", err.Message);
            DbParameter par4 = Database.AddParameter(Database.ADMIN_DB, "@Stacktrace", err.Stacktrace);
            return Database.InsertData(Database.GetConnection(CreateConnectionString(claims)), sql, par1, par2, par3, par4);
        }


        #region withoutclaimsforweb
        public static List<ErrorLog> GetErrorLogs()
        {
            List<ErrorLog> list = new List<ErrorLog>();
            string sql = "SELECT * FROM ErrorLog";
            DbDataReader reader = Database.GetData(Database.GetConnection(Database.ADMIN_DB), sql);

            while (reader.Read())
            {
                list.Add(Create(reader));
            }
            reader.Close();
            return list;
        }

        public static ErrorLog GetErrorLogsByRegID(int registerID)
        {
            ErrorLog reg = new ErrorLog();
            string sql = "SELECT * FROM ErrorLog WHERE ID=@ID";
            DbParameter parID = Database.AddParameter(Database.ADMIN_DB, "@ID", registerID);
            DbDataReader reader = Database.GetData(Database.GetConnection(Database.ADMIN_DB), sql, parID);
            while (reader.Read())
            {
                reg = Create(reader);
            }
            reader.Close();
            return reg;
        }

        public static int InsertErrorLog(ErrorLog err)
        {
            string sql = "INSERT INTO ErrorLog VALUES(@RegisterID,@Timestamp,@Message,@Stacktrace)";
            DbParameter par1 = Database.AddParameter(Database.ADMIN_DB, "@RegisterID", err.RegisterID);
            DbParameter par2 = Database.AddParameter(Database.ADMIN_DB, "@Timestamp", err.TimeStamp);
            DbParameter par3 = Database.AddParameter(Database.ADMIN_DB, "@Message", err.Message);
            DbParameter par4 = Database.AddParameter(Database.ADMIN_DB, "@Stacktrace", err.Stacktrace);
            return Database.InsertData(Database.GetConnection(Database.ADMIN_DB), sql, par1, par2, par3, par4);
        }
        #endregion withoutclaimsforweb

        private static ErrorLog Create(IDataRecord record)
        {
            return new ErrorLog()
            {
                RegisterID = Int32.Parse(record["RegisterID"].ToString()),
                TimeStamp = record["Timestamp"].ToString(),
                Message = record["Message"].ToString(),
                Stacktrace = record["Stacktrace"].ToString(),
            };
        }
    }
}