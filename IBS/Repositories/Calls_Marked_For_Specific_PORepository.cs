using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;

using System.Drawing;
using System.Reflection.PortableExecutable;

namespace IBS.Repositories
{
    public class Calls_Marked_For_Specific_PORepository : ICalls_Marked_For_Specific_PORepository
    {
        private readonly ModelContext context;  
        public Calls_Marked_For_Specific_PORepository(ModelContext context)
        {
            this.context = context;
        }

        public DTResult<Calls_Marked_For_Specific_POModel> gridData(DTParameters dtParameters)
        {
            DTResult<Calls_Marked_For_Specific_POModel> dTResult = new() { draw = 0 };
            IQueryable<Calls_Marked_For_Specific_POModel>? query = null;

            string railwaytypes = dtParameters.AdditionalValues?.GetValueOrDefault("railwaytypes");
            string railwaytypes1 = dtParameters.AdditionalValues?.GetValueOrDefault("railwaytypes1");
            DateTime podt = Convert.ToDateTime(dtParameters.AdditionalValues?.GetValueOrDefault("podt"));



             query = (from t13 in context.T13PoMasters
                         join t17 in context.T17CallRegisters on t13.CaseNo equals t17.CaseNo
                         where t13.RlyNonrly == railwaytypes &&
                               t13.RlyCd == railwaytypes1 &&
                               t13.PoDt == podt
                         group new { t13, t17 } by new
                         {
                             t13.L5noPo,
                             t13.PoNo,
                             t13.PoDt,
                             t13.RlyNonrly,
                             t13.RlyCd
                         } into grouped
                         orderby grouped.Key.PoDt
                         select new Calls_Marked_For_Specific_POModel
                         {
                             L5NO_PO = grouped.Key.L5noPo,
                             PO_NO = grouped.Key.PoNo,
                             PO_DT = Convert.ToString(grouped.Key.PoDt),
                             RLY_NONRLY = grouped.Key.RlyNonrly,
                             RLY_CD = grouped.Key.RlyCd
                         });

            query = query.Distinct();

           
            var result = query.ToList();
            dTResult.data = result;
          
            return dTResult;
        }

        public List<railway_dropdown> GetValue(string selectedValue)
        {
            Calls_Marked_For_Specific_POModel result = null;
            var query = from railway in context.T91Railways
                        where railway.RlyCd != "CORE"
                        orderby railway.RlyCd
                        select new railway_dropdown
                        {
                            RLY_CD = railway.RlyCd, 
                            RAILWAY_ORGN = railway.Railway
                        };

           
            return query.ToList();
        }

        public List<railway_dropdown> GetValue2(string selectedValue)
        {
            Calls_Marked_For_Specific_POModel result = null;
            var query = context.T12BillPayingOfficers
             .Where(bpo => bpo.BpoType == selectedValue)
             .Select(bpo => new railway_dropdown
             {
                 //BPO_RLY = bpo.BPO_RLY,
                 RLY_CD = bpo.BpoRly,
                 RAILWAY_ORGN = bpo.BpoOrgn
             })
             .OrderBy(item => item.RLY_CD)
             .ToList();



            return query.ToList();
        }

        public Calls_Marked_For_Specific_POModel edit(string PO_NO, string PO_DT, string RLY_NONRLY, string RLY_CD)
        {
            Calls_Marked_For_Specific_POModel model = new();
            string trimmedPO_NO = PO_NO.Substring(PO_NO.Length - 5);

            var query = from t051 in context.T05Vendors
                        join t13 in context.T13PoMasters on t051.VendCd equals t13.VendCd
                        join t06 in context.T06Consignees on t13.PurchaserCd equals t06.ConsigneeCd into consigneeGroup
                        from t06 in consigneeGroup.DefaultIfEmpty()
                        join t17 in context.T17CallRegisters on t13.CaseNo equals t17.CaseNo
                        join t21 in context.T21CallStatusCodes on t17.CallStatus equals t21.CallStatusCd
                        join t18 in context.T18CallDetails on new { t17.CaseNo, t17.CallRecvDt, t17.CallSno } equals new { t18.CaseNo, t18.CallRecvDt, t18.CallSno }
                        join t09 in context.T09Ies on t17.IeCd equals t09.IeCd
                        join t052 in context.T05Vendors on t17.MfgCd equals t052.VendCd into manufacturerGroup
                        from t052 in manufacturerGroup.DefaultIfEmpty()
                        join t03 in context.T03Cities on t051.VendCityCd equals t03.CityCd
                        join t032 in context.T03Cities on t052.VendCityCd equals t032.CityCd into manufacturerCityGroup
                        from t032 in manufacturerCityGroup.DefaultIfEmpty()
                        join t08 in context.T08IeControllOfficers on t09.IeCoCd equals t08.CoCd into officerGroup
                        from t08 in officerGroup.DefaultIfEmpty()
                        where
                            t13.L5noPo.Trim().ToUpper() == trimmedPO_NO.Trim().ToUpper() &&
                            t13.PoDt == Convert.ToDateTime(PO_DT) &&
                            t13.RlyNonrly == RLY_NONRLY &&
                            t13.RlyCd == RLY_CD
                        orderby t17.CallMarkDt descending, t051.VendName
                        select new lstSpecificPO
                        {
                            VENDOR = t051.VendName.Trim() + (t03 != null ? ", " + t03.City : ""),
                            MANUFACTURER = (t052 != null ? t052.VendName.Trim() + (t032 != null ? ", " + t032.City : "") : ""),
                            VEND_CD = t051.VendCd,
                            MFG_CD = t052.VendCd,
                            CONSIGNEE = t06.ConsigneeDesig + " " + t06.ConsigneeFirm,
                            ITEM_DESC_PO = t18.ItemDescPo,
                            QTY_TO_INSP = Convert.ToDecimal(t18.QtyToInsp),
                            CALL_MARK_DT = Convert.ToString(t17.CallMarkDt),
                            IeName = t09.IeName,
                            IE_PHONE_NO = t09.IePhoneNo,
                            PO_NO = t13.PoNo,
                            PO_DT = Convert.ToString(t13.PoDt),
                            CASE_NO = t17.CaseNo,
                            REMARK = t17.Remarks,
                            COLOUR = t21.CallStatusColor,
                            MFG_PERS = t052.VendContactPer1,
                            MFG_PHONE = t052.VendContactTel1,
                            CALL_SNO = Convert.ToString(t17.CallSno),
                            Hologram = t17.Hologram,
                            ICPhoto = t17.CaseNo + "-" + t17.BkNo + "-" + t17.SetNo,
                            ICPhotoA1 = t17.CaseNo + "-" + t17.BkNo + "-" + t17.SetNo + "-A1",
                            ICPhotoA2 = t17.CaseNo + "-" + t17.BkNo + "-" + t17.SetNo + "-A2",
                            COUNT = context.T18CallDetails.Count(a => a.CaseNo == t18.CaseNo && a.CallRecvDt == t18.CallRecvDt && a.CallSno == t18.CallSno),
                            CO_NAME = (t08 != null ? (t08.CoPhoneNo != null ? t08.CoName.Trim() + " (Mob: " + t08.CoPhoneNo + ")" : t08.CoName) : null),
                            CALL_STATUS = (
                                t21.CallStatusCd == "A" ? (t17.BkNo != null ? " (BookSet-" + t17.BkNo + "/" + t17.SetNo + ") Dt: " + t17.CallStatusDt : "")
                                : (t21.CallStatusCd == "B" ? " (Accepted on Dt:" + t17.CallStatusDt + ")"
                                : (t21.CallStatusCd == "R" ? (t17.BkNo != null ? " (BookSet-" + t17.BkNo + "/" + t17.SetNo + ")" : "")
                                : (t21.CallStatusCd == "G" ? " Dt: " + t17.CallStatusDt
                                : (t21.CallStatusCd == "C" ? " on " + t17.CallStatusDt : ""))))
                                + (t17.CallCancelStatus == "N" ? " (Non Chargeable)" : (t17.CallCancelStatus == "C" ? " (Chargeable)" : "")))
                           
                        };

            // Execute the query and retrieve the results
            var result = query.ToList();

            model.lstSpecificPOs = result;
            return model;
            
        }
    }
}
