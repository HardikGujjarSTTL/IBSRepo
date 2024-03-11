using CrystalDecisions.CrystalReports.Engine;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;

namespace IBSReports.ReportClass
{
    public static class ICAccountal
    {
        public static OracleConnection conn1 = new OracleConnection(ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString);
        public static string Oracle_UserID = System.Configuration.ConfigurationManager.AppSettings["Oracle_UserID"].ToString();
        public static string Oracle_Password = System.Configuration.ConfigurationManager.AppSettings["Oracle_Password"].ToString();

        public static ReportDocument ICAccount(string FromDate, string ToDate, string Region, string lstYesNo, string rdbGIE, string lstIE, string rdbCancelYes,out DataSet dsCustom)
        {
            int wError = 0;
            
            
            ReportDocument rd = new ReportDocument();
            dsCustom = new DataSet();
            string wRecordSelectionFormula = "";
            try
            {
                find_missing_IC(FromDate, ToDate, Region, lstYesNo, rdbGIE, lstIE, rdbCancelYes);
                if (wError == -1)
                {
                    DisplayAlert("Unable to generate the report.");
                }
                else
                {
                    if (Convert.ToString(Region) == "N")
                    {
                        rd.Load(HttpContext.Current.Server.MapPath("~/Reports/nr_rptICAccountal.rpt"));
                    }
                    else
                    {
                        rd.Load(HttpContext.Current.Server.MapPath("~/Reports/rptICAccountal.rpt"));
                    }
                    rd.RecordSelectionFormula = "{T10_IC_BOOKSET.REGION}='" + Region + "'";
                    rd.SetParameterValue(rd.ParameterFields[0].Name, Region);
                    rd.SetParameterValue(rd.ParameterFields[1].Name, FromDate);
                    rd.SetParameterValue(rd.ParameterFields[2].Name, ToDate);

                    rd.RecordSelectionFormula = wRecordSelectionFormula;
                    rd.SetDatabaseLogon(Oracle_UserID, Oracle_Password);
                }
            }
            catch
            {

            }
            return rd;
        }
                
		private static void find_missing_IC(string FromDate, string ToDate, string Region,string lstYesNo,string rdbGIE,string lstIE,string rdbCancelYes)
		{
            
            try
			{
                conn1.Open();
                OracleCommand cmd = new OracleCommand("IC_ACCOUNTAL", conn1);
                cmd.CommandType = CommandType.StoredProcedure;

                // Add OracleParameters
                cmd.Parameters.Add("IN_REGION", OracleDbType.Char, Region, ParameterDirection.Input);
                cmd.Parameters.Add("IN_DATE_FR", OracleDbType.Char, FromDate, ParameterDirection.Input);
                cmd.Parameters.Add("IN_DATE_TO", OracleDbType.Char, ToDate, ParameterDirection.Input);
                cmd.Parameters.Add("IN_YES_NO", OracleDbType.Char, lstYesNo, ParameterDirection.Input);

                if (rdbGIE == "ParticularIE")
                {
                    cmd.Parameters.Add("IN_IE_CD", OracleDbType.Char, lstIE, ParameterDirection.Input);
                }
                else
                {
                    cmd.Parameters.Add("IN_IE_CD", OracleDbType.Char, "X", ParameterDirection.Input);
                }

                if (rdbCancelYes == "true")
                {
                    cmd.Parameters.Add("IN_CANCEL_LOST", OracleDbType.Char, "Y", ParameterDirection.Input);
                }
                else
                {
                    cmd.Parameters.Add("IN_CANCEL_LOST", OracleDbType.Char, "N", ParameterDirection.Input);
                }

                // Output parameter
                OracleParameter outParam = new OracleParameter("OUT_ERR_CD", OracleDbType.Int32);
                outParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outParam);

                cmd.ExecuteNonQuery();
                OracleDecimal oracleDecimalValue = (OracleDecimal)outParam.Value;
                int outErrCode = oracleDecimalValue.ToInt32();
            }
            catch (Exception ex)
			{
				string str;
				str = ex.Message;
				string str1 = str.Replace("\n", "");
				//Response.Redirect(("Error_Form.aspx?err=" + str1));
			}
			finally
			{
				conn1.Close();
			}
			//wError=Convert.ToInt16(cmd.Parameters["OUT_ERR_CD"].Value);
		}

        private static void DisplayAlert(string msg)
        {
            Page page = HttpContext.Current.CurrentHandler as Page;
            if (page != null)
            {
                string script = "alert('" + msg + "');";
                page.ClientScript.RegisterStartupScript(typeof(Page), "alert", script, true);
            }
        }
    }
}