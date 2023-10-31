using CrystalDecisions.Shared;
using System.Configuration;

namespace IBSReports
{
    public static class Common
    {
        public static ConnectionInfo GetReportConn()
        {
            ConnectionInfo con = null;
            string ServerNM = null;
            string DatabaseNM = null;
            string UserNM = null;
            string Pass = null;

            //string[] strArray = ConfigurationManager.AppSettings["ConnectionString"].Split(new char[] { ';' });

            //for (int i = 0; i <= strArray.Length - 1; i++)
            //{
            //    string[] strValue = strArray[i].Split('=');

            //    if (strValue[0].ToUpper() == "USER ID")
            //    {
            //        UserNM = strValue[1];
            //    }
            //    if (strValue[0].ToUpper() == "PWD")
            //    {
            //        Pass = strValue[1];
            //    }
            //    if (strValue[0].ToUpper() == "INITIAL CATALOG")
            //    {
            //        DatabaseNM = strValue[1];
            //    }
            //    if (strValue[0].ToUpper() == "DATA SOURCE")
            //    {
            //        ServerNM = strValue[1];
            //    }
            //}

            //ServerNM = "192.168.0.215";
            ServerNM = "192.168.0.215:1521/orcl.silvertouch.local";
            DatabaseNM = "IBSDev";
            UserNM = "IBSDev";
            Pass = "IBSDev";

            con = new ConnectionInfo();
            con.ServerName = ServerNM;
            con.DatabaseName = DatabaseNM;
            con.UserID = UserNM;
            con.Password = Pass;

            return con;
        }
    }
}