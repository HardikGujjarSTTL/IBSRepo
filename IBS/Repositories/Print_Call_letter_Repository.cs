using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace IBS.Repositories
{
    public class Print_Call_letter_Repository : IPrint_Call_letter_Repository
    {
        private readonly ModelContext context;
        public Print_Call_letter_Repository(ModelContext context)
        {
            this.context = context;
        }

      
        public Print_Call_letter_Model query1(string caseNo = "", string callRecvDate = "", string callSno = "")
        {
            string CASE_NO = caseNo;
            string CALL_RECV_DATE = callRecvDate;
            string CALL_SNO = callSno;
          
            Print_Call_letter_Model model = new();
            var result = from p in context.T13PoMasters
                         join t17 in context.T17CallRegisters on p.CaseNo equals t17.CaseNo
                         join m in context.T05Vendors on t17.MfgCd equals m.VendCd
                         join cm in context.T03Cities on m.VendCityCd equals cm.CityCd
                         join pu in context.V06Consignees on p.PurchaserCd equals pu.ConsigneeCd
                         join t18 in context.T18CallDetails on new { t17.CaseNo, t17.CallRecvDt, t17.CallSno } equals new { t18.CaseNo, t18.CallRecvDt, t18.CallSno }
                         join cn in context.V06Consignees on t18.ConsigneeCd equals cn.ConsigneeCd
                         join t14 in context.T14PoBpos on p.CaseNo equals t14.CaseNo
                         join v12 in context.V12BillPayingOfficers on new { t14.BpoCd } equals new { v12.BpoCd }
                         join t15 in context.T15PoDetails on new { CaseNo = t18.CaseNo, ItemSrnoPo = t18.ItemSrnoPo } equals new { CaseNo = t15.CaseNo, ItemSrnoPo = t15.ItemSrno }
                         join t09 in context.T09Ies on t17.IeCd equals t09.IeCd into t09Group
                         from t09 in t09Group.DefaultIfEmpty()
                         where t17.CaseNo == CASE_NO && t17.CallRecvDt == Convert.ToDateTime(CALL_RECV_DATE) && t17.CallSno == Convert.ToInt32(CALL_SNO)
                         select new Print_Call_letter_Model
                         {
                             PO_NO = p.PoNo,
                             PO_DT = Convert.ToString(p.PoDt),
                             MFG_NAME = m.VendName.Trim(),
                             MFG_ADD = cm.Location != null ? cm.Location + " / " + cm.City : cm.City,
                             PURCHASER = p.PurchaserCd + "-" + pu.Consignee,
                             
                            CONSIGNEE = t18.ConsigneeCd + "-" + cn.Consignee,
                             CASE_NO = t17.CaseNo,
                             CALL_RECV_DATE = t17.CallRecvDt.ToString("dd/MM/yyyy"),
                             CALL_SNO = t17.CallSno,
                             CALL_LETTER_NO = (t17.CallLetterNo)??"CallNo",
                             CALL_LETTER_DT = t17.CallLetterDt != null ? t17.CallLetterDt.Value.ToString("dd/MM/yyyy") : "01/01/2001",
                             CALL_INSTALL_NO = Convert.ToInt32(t17.CallInstallNo),
                             ONLINE_CALL = t17.OnlineCall ?? "X",
                             FINAL_OR_STAGE = t17.FinalOrStage ?? "X",
                             REMARKS = (t17.Remarks)?? "Remark",
                             ITEM_RDSO = t17.ItemRdso == "Y" ? "Yes" : t17.ItemRdso == "N" ? "No" : "",
                             VEND_RDSO = (t17.VendRdso == "Y" ? "Yes" : t17.VendRdso == "N" ? "No" : "")?? "Yes",
                             VEND_APP_FR = t17.VendApprovalFr != null ? t17.VendApprovalFr.Value.ToString("dd/MM/yyyy") : "01/01/2001",
                             VEND_APP_TO = t17.VendApprovalTo != null ? t17.VendApprovalTo.Value.ToString("dd/MM/yyyy") : "01/01/2001",
                             STAG_DP = t17.StaggeredDp == "Y" ? "YES" : t17.StaggeredDp == "N" ? "NO" : "N.A",
                             LOT_DP_1 = (t17.LotDp1)?? "00",
                             LOT_DP_2 = (t17.LotDp2)?? "00",
                             IE_NAME = t09.IeName,
                             ITEM_DESC_PO = t18.ItemDescPo,
                             QTY_ORDERED = Convert.ToDecimal(t18.QtyOrdered),
                             QTY_TO_INSP = Convert.ToDecimal(t18.QtyToInsp),
                             CUM_QTY_PREV_OFFERED = t18.CumQtyPrevOffered ?? 0,
                             CUM_QTY_PREV_PASSED = t18.CumQtyPrevPassed ?? 0,
                             VEND_CONTACT_PER_1 = m.VendContactPer1,
                             VEND_CONTACT_TEL_1 = m.VendContactTel1,
                             VEND_EMAIL = m.VendEmail,
                             BPO = v12.Bpo,
                             DELV_DT = t15.ExtDelvDt != null ? t15.ExtDelvDt.Value.ToString("dd/MM/yyyy") : null,
                             ITEM_CD = t15.ItemCd,
                             IRFC_FUNDED = Convert.ToString(t15.BasicValue),
                            
                         };

            //var result = query1.ToList();

            model = result.FirstOrDefault();
            // query2( caseNo = "",  callRecvDate = "",  callSno = "");
            //dTResult.recordsTotal = query1.Count();
            //dTResult.data = result;
            //dTResult.recordsFiltered = query1.Count();
            var result1 = result.ToList();
            model.printcalllater = result1;
            return model;
        }


        public Print_Call_letter_Model query2(string caseNo = "", string callRecvDate = "", string callSno = "")
        {
            string CASE_NO = caseNo;
            string CALL_RECV_DATE = callRecvDate;
            string CALL_SNO = callSno;
            //DTResult<Print_Call_letter_Model> dTResult = new() { draw = 0 };
            //IQueryable<Print_Call_letter_Model>? query = null;
            Print_Call_letter_Model model = new();

            var query1 = from t13 in context.T13PoMasters
                        join t05 in context.T05Vendors on t13.VendCd equals t05.VendCd
                        join t03 in context.T03Cities on t05.VendCityCd equals t03.CityCd
                        where t13.CaseNo == CASE_NO
                         select new Print_Call_letter_Model
                         {
                            VEND_CD = Convert.ToString(t13.VendCd),
                            VEND_NAME = t05.VendName.Trim(),
                            VEND_ADDRESS = (t05.VendAdd2 != null) ? t05.VendAdd1 + "/" + t05.VendAdd2 : t05.VendAdd1 + "/" + t03.City,
                            VEND_EMAIL = t05.VendEmail,
                            VEND_CONTACT_PER_1 = t05.VendContactPer1,
                            VEND_CONTACT_TEL_1 = t05.VendContactTel1,
                            SOURCE = (t13.PoSource == "V") ? "VENDOR" : (t13.PoSource == "M") ? "MANUAL" : (t13.PoSource == "C") ? "IREPS" : "OTHER"
                         };

          

            model = query1.FirstOrDefault();

            return model;
        }



        public Print_Call_letter_Model CombinedQuery(string caseNo = "", string callRecvDate = "", string callSno = "")
            {
            // Call query1 to get the initial model 
            Print_Call_letter_Model model = query1(caseNo, callRecvDate, callSno);
            

            // Call query2 to populate additional fields in the model
            var result2 = query2(caseNo, callRecvDate, callSno);
            if (result2 != null)
            {
                // Combine data from query2 into the model
                model.VEND_CD = result2.VEND_CD;
                model.VEND_NAME = result2.VEND_NAME;
                model.VEND_ADDRESS = result2.VEND_ADDRESS;
                model.VEND_EMAIL = result2.VEND_EMAIL;
                model.VEND_CONTACT_PER_1 = result2.VEND_CONTACT_PER_1;
                model.VEND_CONTACT_TEL_1 = result2.VEND_CONTACT_TEL_1;
                model.SOURCE = result2.SOURCE;
            }

            // Return the populated model

            return model;
        
        
        }
    }
}
