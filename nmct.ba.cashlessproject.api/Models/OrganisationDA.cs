using nmct.ba.cashlessproject.api.Helper;
using nmct.ba.cashlessproject.model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Web;

namespace nmct.ba.cashlessproject.api.Models
{
    public class OrganisationDA
    {
        public static List<Organisation> GetOrganisations()
        {
            List<Organisation> list = new List<Organisation>();
            string sql = "SELECT * FROM Organisations";
            DbDataReader reader = Database.GetData(Database.GetConnection(Database.ADMIN_DB), sql);
            while (reader.Read())
            {
                list.Add(Create(reader));
            }
            reader.Close();
            return list;
        }

        public static Organisation GetOrganisationByID(int id)
        {
            Organisation org = new Organisation();
            string sql = "SELECT * FROM Organisations WHERE ID=@ID";
            DbParameter parID = Database.AddParameter(Database.ADMIN_DB, "@ID", id);
            DbDataReader reader = Database.GetData(Database.GetConnection(Database.ADMIN_DB), sql, parID);
            reader.Read();
            org = Create(reader);
            reader.Close();
            return org;
        }

        public static Organisation CheckCredentials(string username, string password)
        {
            string sql = "SELECT * FROM Organisation WHERE Login=@Login AND Password=@Password";
            DbParameter par1 = Database.AddParameter(Database.ADMIN_DB, "@Login", Cryptography.Encrypt(username));
            DbParameter par2 = Database.AddParameter(Database.ADMIN_DB, "@Password", Cryptography.Encrypt(password));
            try
            {
                DbDataReader reader = Database.GetData(Database.GetConnection(Database.ADMIN_DB), sql, par1, par2);
                reader.Read();
                return Create(reader);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }


        public static int InsertOrganisation(Organisation org)
        {
            string sql = "INSERT INTO Organisations VALUES(@Login,@Password,@DbName,@DbLogin,@DbPassword,@OrganisationName,@Address,@Email,@Phone)";
            DbParameter par1 = Database.AddParameter(Database.ADMIN_DB, "@Login", org.Login);
            DbParameter par2 = Database.AddParameter(Database.ADMIN_DB, "@Password", org.Password);
            DbParameter par3 = Database.AddParameter(Database.ADMIN_DB, "@DbName", org.DbName);
            DbParameter par4 = Database.AddParameter(Database.ADMIN_DB, "@DbLogin", org.DbLogin);
            DbParameter par5 = Database.AddParameter(Database.ADMIN_DB, "@DbPassword", org.DbPassword);
            DbParameter par6 = Database.AddParameter(Database.ADMIN_DB, "@OrganisationName", org.OrganisationName);
            DbParameter par7 = Database.AddParameter(Database.ADMIN_DB, "@Address", org.Address);
            DbParameter par8 = Database.AddParameter(Database.ADMIN_DB, "@Email", org.Email);
            DbParameter par9 = Database.AddParameter(Database.ADMIN_DB, "@Phone", org.Phone);
            return Database.InsertData(Database.GetConnection(Database.ADMIN_DB), sql, par1, par2, par3, par4, par5, par6, par7, par8, par9);
        }

        public static int UpdateOrganisation(Organisation org)
        {
            string sql = "UPDATE Organisations SET Login=@Login, Password=@Password, DbName=@DbName, DbLogin=@DbLogin, DbPassword=@DbPassword, OrganisationName=@OrganisationName, Address=@Address, Email=@Email, Phone=@Phone WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter(Database.ADMIN_DB, "@Login", org.Login);
            DbParameter par2 = Database.AddParameter(Database.ADMIN_DB, "@Password", org.Password);
            DbParameter par3 = Database.AddParameter(Database.ADMIN_DB, "@DbName", org.DbName);
            DbParameter par4 = Database.AddParameter(Database.ADMIN_DB, "@DbLogin", org.DbLogin);
            DbParameter par5 = Database.AddParameter(Database.ADMIN_DB, "@DbPassword", org.DbPassword);
            DbParameter par6 = Database.AddParameter(Database.ADMIN_DB, "@OrganisationName", org.OrganisationName);
            DbParameter par7 = Database.AddParameter(Database.ADMIN_DB, "@Address", org.Address);
            DbParameter par8 = Database.AddParameter(Database.ADMIN_DB, "@Email", org.Email);
            DbParameter par9 = Database.AddParameter(Database.ADMIN_DB, "@Phone", org.Phone);
            DbParameter parID = Database.AddParameter(Database.ADMIN_DB, "@ID", org.ID);
            return Database.ModifyData(Database.GetConnection(Database.ADMIN_DB), sql, par1, par2, par3, par4, par5, par6, par7, par8, par9, parID);
        }

        private static Organisation Create(IDataRecord record)
        {
            return new Organisation()
            {
                ID = Int32.Parse(record["ID"].ToString()),
                Login = record["Login"].ToString(),
                Password = record["Password"].ToString(),
                DbName = record["DbName"].ToString(),
                DbLogin = record["DbLogin"].ToString(),
                DbPassword = record["DbPassword"].ToString(),
                OrganisationName = record["OrganisationName"].ToString(),
                Address = record["Address"].ToString(),
                Email = record["Email"].ToString(),
                Phone = record["Phone"].ToString()
            };
        }



        private static void CreateDatabase(Organisation o)
        {
            // create the actual database
            //string create = File.ReadAllText(HostingEnvironment.MapPath(@"~/App_Data/create.txt")); only for the web
            string create = File.ReadAllText(@"..\..\Data\create.txt"); // only for desktop
            string sql = create.Replace("@@DbName", o.DbName).Replace("@@DbLogin", o.DbLogin).Replace("@@DbPassword", o.DbPassword);
            foreach (string commandText in RemoveGo(sql))
            {
                Database.ModifyData(Database.GetConnection(Database.ADMIN_DB), commandText);
            }

            // create login, user and tables
            DbTransaction trans = null;
            try
            {
                trans = Database.BeginTransaction(Database.ADMIN_DB);

                //string fill = File.ReadAllText(HostingEnvironment.MapPath(@"~/App_Data/fill.txt")); // only for the web
                string fill = File.ReadAllText(@"..\..\Data\fill.txt"); // only for desktop
                string sql2 = fill.Replace("@@DbName", o.DbName).Replace("@@DbLogin", o.DbLogin).Replace("@@DbPassword", o.DbPassword);

                foreach (string commandText in RemoveGo(sql2))
                {
                    Database.ModifyData(trans, commandText);
                }

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                Console.WriteLine(ex.Message);
            }
        }

        private static string[] RemoveGo(string input)
        {
            //split the script on "GO" commands
            string[] splitter = new string[] { "\r\nGO\r\n" };
            string[] commandTexts = input.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
            return commandTexts;
        }

        public static int ChangePassword(string user, string pass)
        {
            user = Cryptography.Encrypt(user);
            pass = Cryptography.Encrypt(pass);

            string sql = "UPDATE Organisations SET Password=@Password WHERE Login=@Login";
            DbParameter par1 = Database.AddParameter(Database.ADMIN_DB, "@Password", user);
            DbParameter par2 = Database.AddParameter(Database.ADMIN_DB, "@Login", pass);
            return Database.ModifyData(Database.GetConnection(Database.ADMIN_DB), sql, par1, par2);

        }
    }
}