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
    public class RegisterEmployeeDA
    {
        private static ConnectionStringSettings CreateConnectionString(IEnumerable<Claim> claims)
        {
            string dblogin = claims.FirstOrDefault(c => c.Type == "dblogin").Value;
            string dbpass = claims.FirstOrDefault(c => c.Type == "dbpass").Value;
            string dbname = claims.FirstOrDefault(c => c.Type == "dbname").Value;

            return Database.CreateConnectionString("System.Data.SqlClient", @"LUNALAPPY\SQLEXPRESS", Cryptography.Decrypt(dbname), Cryptography.Decrypt(dblogin), Cryptography.Decrypt(dbpass));
        }

        public static List<RegisterEmployee> GetRegisterEmployee(IEnumerable<Claim> claims)
        {
            List<RegisterEmployee> list = new List<RegisterEmployee>();
            string sql = "SELECT * FROM Register_Employee";
            DbDataReader reader = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql);
            while (reader.Read())
            {
                RegisterEmployee re = new RegisterEmployee();
                int rID = Int32.Parse(reader["RegisterID"].ToString());
                int eID = Int32.Parse(reader["EmployeeID"].ToString());
                re.TimeFrom = Int32.Parse(reader["FromTime"].ToString());
                re.TimeTill = Int32.Parse(reader["UntilTime"].ToString());
                re.EmployeeID = EmployeeDA.GetEmployeeByID(eID, claims);
                re.RegisterID = RegistersDA.GetRegisterByID(rID, claims);
                list.Add(re);
            }
            reader.Close();
            return list;
        }

        public static List<RegisterEmployee> GetRegisterEmployeeByRegisterID(int id, IEnumerable<Claim> claims)
        {
            List<RegisterEmployee> list = new List<RegisterEmployee>();
            string sql = "SELECT * FROM Register_Employee WHERE RegisterID=@ID";
            DbParameter parID = Database.AddParameter(Database.ADMIN_DB, "@ID", id);
            DbDataReader reader = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql, parID);
            while (reader.Read())
            {
                RegisterEmployee re = new RegisterEmployee();
                int rID = Int32.Parse(reader["RegisterID"].ToString());
                int eID = Int32.Parse(reader["EmployeeID"].ToString());
                re.TimeFrom = Int32.Parse(reader["FromTime"].ToString());
                re.TimeTill = Int32.Parse(reader["UntilTime"].ToString());
                re.EmployeeID = EmployeeDA.GetEmployeeByID(eID, claims);
                re.RegisterID = RegistersDA.GetRegisterByID(rID, claims);
                list.Add(re);
            }
            reader.Close();
            return list;
        }

        public static List<RegisterEmployee> GetRegisterEmployeeByEmployeeID(int id, IEnumerable<Claim> claims)
        {
            List<RegisterEmployee> list = new List<RegisterEmployee>();
            string sql = "SELECT * FROM Register_Employee WHERE EmployeeID=@ID";
            DbParameter parID = Database.AddParameter(Database.ADMIN_DB, "@ID", id);
            DbDataReader reader = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql, parID);
            while (reader.Read())
            {
                RegisterEmployee re = new RegisterEmployee();
                int rID = Int32.Parse(reader["RegisterID"].ToString());
                int eID = Int32.Parse(reader["EmployeeID"].ToString());
                re.TimeFrom = Int32.Parse(reader["FromTime"].ToString());
                re.TimeTill = Int32.Parse(reader["UntilTime"].ToString());
                re.EmployeeID = EmployeeDA.GetEmployeeByID(eID, claims);
                re.RegisterID = RegistersDA.GetRegisterByID(rID, claims);
                list.Add(re);
            }
            reader.Close();
            return list;
        }

        public static RegisterEmployee GetEmployeeByRegAndEmployeeID(int regID, int emplID, IEnumerable<Claim> claims)
        {
            string sql = "SELECT * FROM Register_Employee WHERE RegisterID=@ID AND EmployeeID=@ID2";
            DbParameter parID = Database.AddParameter(Database.ADMIN_DB, "@ID", regID);
            DbParameter parID2 = Database.AddParameter(Database.ADMIN_DB, "@ID2", regID);
            DbDataReader reader = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql, parID, parID2);
            reader.Read();
            RegisterEmployee re = new RegisterEmployee();
            int rID = Int32.Parse(reader["RegisterID"].ToString());
            int eID = Int32.Parse(reader["EmployeeID"].ToString());
            re.TimeFrom = Int32.Parse(reader["FromTime"].ToString());
            re.TimeTill = Int32.Parse(reader["UntilTime"].ToString());
            re.EmployeeID = EmployeeDA.GetEmployeeByID(eID, claims);
            re.RegisterID = RegistersDA.GetRegisterByID(rID, claims);
            reader.Close();
            return re;
        }

        public static int InsertRegisterEmployee(RegisterEmployee re, IEnumerable<Claim> claims)
        {
            string sql = "INSERT INTO Register_Employee VALUES(@RegisterID,@EmployeeID,@From,@Until)";
            DbParameter par1 = Database.AddParameter(Database.ADMIN_DB, "@RegisterID", re.RegisterID.ID);
            DbParameter par2 = Database.AddParameter(Database.ADMIN_DB, "@EmployeeID", re.EmployeeID.ID);
            DbParameter par3 = Database.AddParameter(Database.ADMIN_DB, "@From", re.TimeFrom);
            DbParameter par4 = Database.AddParameter(Database.ADMIN_DB, "@Until", re.TimeTill);
            return Database.InsertData(Database.GetConnection(CreateConnectionString(claims)), sql, par1, par2, par3, par4);
        }

        public static int UpdateRegisterEmployee(RegisterEmployee re, IEnumerable<Claim> claims)
        {
            string sql = "UPDATE Register_Employee SET RegisterID=@RegisterID, EmployeeID=@EmployeeID, From=@From, Until=@Until WHERE RegisterID=@ID AND EmployeeID=@ID2";
            DbParameter par1 = Database.AddParameter(Database.ADMIN_DB, "@RegisterID", re.RegisterID.ID);
            DbParameter par2 = Database.AddParameter(Database.ADMIN_DB, "@EmployeeID", re.EmployeeID.ID);
            DbParameter par3 = Database.AddParameter(Database.ADMIN_DB, "@From", re.TimeFrom);
            DbParameter par4 = Database.AddParameter(Database.ADMIN_DB, "@Until", re.TimeTill);
            DbParameter par5 = Database.AddParameter(Database.ADMIN_DB, "@ID", re.RegisterID);
            DbParameter par6 = Database.AddParameter(Database.ADMIN_DB, "@ID2", re.EmployeeID);
            return Database.ModifyData(Database.GetConnection(CreateConnectionString(claims)), sql, par1, par2, par3, par4, par5, par6);
        }

        public static int DeleteRegisterEmployee(int regID, int empID, IEnumerable<Claim> claims)
        {
            string sql = "DELETE FROM Register_Employee WHERE RegisterID=@ID AND EmployeeID=@ID2";
            DbParameter par1 = Database.AddParameter(Database.ADMIN_DB, "@RegisterID", regID);
            DbParameter par2 = Database.AddParameter(Database.ADMIN_DB, "@EmployeeID", empID);
            return Database.ModifyData(Database.GetConnection(CreateConnectionString(claims)), sql, par1, par2);
        }
    }
}