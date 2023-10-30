using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace CrystalReportProject.ReportClass
{
    public static class CustomerAdvance
    {
        public static ReportDocument CustomerAdv(string BPOType, string AccCD, string FromDate, string ToDate, string Region, out DataSet dsCustom)
        {
            String wRegion = "";
            if (Region == "N") { wRegion = "Northern Region"; }
            else if (Region == "S") { wRegion = "Southern Region"; }
            else if (Region == "E") { wRegion = "Eastern Region"; }
            else if (Region == "W") { wRegion = "Western Region"; }
            else if (Region == "C") { wRegion = "Central Region"; }
            String wHdr = wRegion + "(Customer Advance details for the period 01/04/2008 to " + ToDate + ")";

            ReportDocument rd = new ReportDocument();
            string formula = "((IsNull({T24_RV.VCHR_DT}) and ToText({T25_RV_DETAILS.CHQ_DT},'yyyyMMdd')>='" + dateconcate(FromDate) + "' and ToText({T25_RV_DETAILS.CHQ_DT},'yyyyMMdd')<='" + dateconcate(ToDate) + "') or ";
            formula = formula + "(ToText({T24_RV.VCHR_DT},'yyyyMMdd')>='" + dateconcate(FromDate) + "' and ToText({T24_RV.VCHR_DT},'yyyyMMdd')<='" + dateconcate(ToDate) + "'))";
            wHdr = wRegion + " ( Un-Posted/Partly Posted Cheques/EFT/DD received during " + FromDate + " to " + ToDate + ")";
            dsCustom = new DataSet();
            try
            {


                if (BPOType == "R")
                {
                    if (AccCD == "O")
                    {
                        formula = formula + " and ({T12_BILL_PAYING_OFFICER.BPO_TYPE}='R') and ({T25_RV_DETAILS.SUSPENSE_AMT}<>0) and ({T25_RV_DETAILS.ACC_CD}<>2709) and ({T25_RV_DETAILS.ACC_CD}<>2210) and ({T25_RV_DETAILS.ACC_CD}<>2212) and Left ({T25_RV_DETAILS.VCHR_NO},1)='" + Region + "'";
                        wHdr = wHdr + " - Railways(Other than Advance & Lab testing charges)";
                    }
                    else if (AccCD == "2709")
                    {
                        formula = formula + " and ({T12_BILL_PAYING_OFFICER.BPO_TYPE}='R') and ({T25_RV_DETAILS.SUSPENSE_AMT}<>0) and ({T25_RV_DETAILS.ACC_CD}=2709) and Left ({T25_RV_DETAILS.VCHR_NO},1)='" + Region + "'";
                        wHdr = wHdr + " - Railways(Advances)";
                    }
                    else if (AccCD == "2710")
                    {
                        formula = formula + " and ({T12_BILL_PAYING_OFFICER.BPO_TYPE}='R') and ({T25_RV_DETAILS.SUSPENSE_AMT}<>0) and ({T25_RV_DETAILS.ACC_CD}=2210) and ({T25_RV_DETAILS.ACC_CD}=2212) and Left ({T25_RV_DETAILS.VCHR_NO},1)='" + Region + "'";
                        wHdr = wHdr + " - Railways(Testing Charges)";
                    }
                    else
                    {
                        formula = formula + " and ({T12_BILL_PAYING_OFFICER.BPO_TYPE}='R') and ({T25_RV_DETAILS.SUSPENSE_AMT}<>0) and Left ({T25_RV_DETAILS.VCHR_NO},1)='" + Region + "'";
                        wHdr = wHdr + " - Railways";
                    }

                }
                else
                {
                    if (AccCD == "O")
                    {
                        formula = formula + " and ({T12_BILL_PAYING_OFFICER.BPO_TYPE}<>'R') and ({T25_RV_DETAILS.SUSPENSE_AMT}<>0) and ({T25_RV_DETAILS.ACC_CD}<>2709) and ({T25_RV_DETAILS.ACC_CD}<>2210) and ({T25_RV_DETAILS.ACC_CD}<>2212) and Left ({T25_RV_DETAILS.VCHR_NO},1)='" + Region + "'";
                        wHdr = wHdr + " - Non-Railwayss(Other than Advance & Lab testing charges)";
                    }
                    else if (AccCD == "2709")
                    {
                        formula = formula + " and ({T12_BILL_PAYING_OFFICER.BPO_TYPE}<>'R') and ({T25_RV_DETAILS.SUSPENSE_AMT}<>0) and ({T25_RV_DETAILS.ACC_CD}=2709) and Left ({T25_RV_DETAILS.VCHR_NO},1)='" + Region + "'";
                        wHdr = wHdr + " - Non-Railways(Advances)";
                    }
                    else if (AccCD == "2710")
                    {
                        formula = formula + " and ({T12_BILL_PAYING_OFFICER.BPO_TYPE}<>'R') and ({T25_RV_DETAILS.SUSPENSE_AMT}<>0) and ({T25_RV_DETAILS.ACC_CD}=2210) and ({T25_RV_DETAILS.ACC_CD}=2212) and Left ({T25_RV_DETAILS.VCHR_NO},1)='" + Region + "'";
                        wHdr = wHdr + " - Non-Railways(Testing Charges)";
                    }
                    else
                    {
                        formula = formula + " and ({T12_BILL_PAYING_OFFICER.BPO_TYPE}<>'R') and ({T25_RV_DETAILS.SUSPENSE_AMT}<>0) and Left ({T25_RV_DETAILS.VCHR_NO},1)='" + Region + "'";
                        wHdr = wHdr + " - Non-Railways";
                    }
                }
                

                rd.Load(HttpContext.Current.Server.MapPath("~/Reports/rptChequeStatus.rpt"));
                rd.RecordSelectionFormula = formula;
                rd.SetDatabaseLogon("QA", "QA");
                // rd.SetParameterValue(0, wHdr);
            }
            catch (Exception ex)
            {

            }
            return rd;
        }
        static string dateconcate(string dt)
        {
            string myYear, myMonth, myDay;
            myYear = dt.Substring(6, 4);
            myMonth = dt.Substring(3, 2);
            myDay = dt.Substring(0, 2);
            string dt1 = myYear + myMonth + myDay;
            return (dt1);
        }
    }
}