using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Globalization;

namespace IBS.Repositories
{
    public class CallsReportRepository : ICallsReportRepository
    {
        private readonly ModelContext context;
        public CallsReportRepository(ModelContext context)
        {
            this.context = context;
        }

        public List<railway_dropdown1> GetValue(string selectedValue)
        {
            Calls_Marked_For_Specific_POModel result = null;
            var query = from railway in context.T91Railways
                        where railway.RlyCd != "CORE"
                        orderby railway.RlyCd
                        select new railway_dropdown1
                        {
                            RLY_CD = railway.RlyCd,
                            RAILWAY_ORGN = railway.Railway
                        };


            return query.ToList();
        }

        public List<railway_dropdown1> GetValue2(string selectedValue)
        {
            Calls_Marked_For_Specific_POModel result = null;
            var query = context.T12BillPayingOfficers
             .Where(bpo => bpo.BpoType == selectedValue)
             .Select(bpo => new railway_dropdown1
             {
                 //BPO_RLY = bpo.BPO_RLY,
                 RLY_CD = bpo.BpoRly,
                 RAILWAY_ORGN = bpo.BpoOrgn
             })
             .OrderBy(item => item.RLY_CD)
             .ToList();



            return query.ToList();
        }


        public DTResult<Statement_IeVendorWiseModel> gridData(DTParameters dtParameters)
        {
            DTResult<Statement_IeVendorWiseModel> dTResult = new() { draw = 0 };
            IQueryable<Statement_IeVendorWiseModel>? query = null;

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
                     select new Statement_IeVendorWiseModel
                     {
                         L5NO_PO = grouped.Key.L5noPo,
                         PO_NO = grouped.Key.PoNo,
                         PO_DT = Convert.ToDateTime(grouped.Key.PoDt),
                         RLY_NONRLY = grouped.Key.RlyNonrly,
                         RLY_CD = grouped.Key.RlyCd
                     });

            query = query.Distinct();


            var result = query.ToList();
            dTResult.data = result;

            return dTResult;
        }


        public Statement_IeVendorWiseModel Statement_IeVendorWise(string ReportType, string frmDate, string toDate , string Region)
        {

            Statement_IeVendorWiseModel model = new();
            List<Statement_IeVendorWiseModel> statement_IeVendorWiseModels = new();
            model.FromDate = frmDate;
            model.ToDate = toDate;
            model.ReportType = ReportType;
            var query = from t17 in context.T17CallRegisters
                        join t19 in context.T19CallCancels on new { t17.CaseNo, t17.CallRecvDt, t17.CallSno } equals new { t19.CaseNo, t19.CallRecvDt, t19.CallSno }
                        join t13 in context.T13PoMasters on t19.CaseNo equals t13.CaseNo
                        join t09 in context.T09Ies on t17.IeCd equals t09.IeCd
                        join v05 in context.V05Vendors on t13.VendCd equals v05.VendCd
                        //join v05 in context.V05_VENDOR on t13.VEND_CD equals v05.VEND_CD
                        join t11_01 in context.T11CallCancelCodes on t19.CancelCd1 equals t11_01.CancelCd into leftJoin01
                        from t11_01 in leftJoin01.DefaultIfEmpty()
                        join t11_02 in context.T11CallCancelCodes on t19.CancelCd2 equals t11_02.CancelCd into leftJoin02
                        from t11_02 in leftJoin02.DefaultIfEmpty()
                        join t11_03 in context.T11CallCancelCodes on t19.CancelCd3 equals t11_03.CancelCd into leftJoin03
                        from t11_03 in leftJoin03.DefaultIfEmpty()
                        join t11_04 in context.T11CallCancelCodes on t19.CancelCd4 equals t11_04.CancelCd into leftJoin04
                        from t11_04 in leftJoin04.DefaultIfEmpty()
                        join t11_05 in context.T11CallCancelCodes on t19.CancelCd5 equals t11_05.CancelCd into leftJoin05
                        from t11_05 in leftJoin05.DefaultIfEmpty()
                        join t11_06 in context.T11CallCancelCodes on t19.CancelCd6 equals t11_06.CancelCd into leftJoin06
                        from t11_06 in leftJoin06.DefaultIfEmpty()
                        join t11_07 in context.T11CallCancelCodes on t19.CancelCd7 equals t11_07.CancelCd into leftJoin07
                        from t11_07 in leftJoin07.DefaultIfEmpty()
                        join t11_08 in context.T11CallCancelCodes on t19.CancelCd8 equals t11_08.CancelCd into leftJoin08
                        from t11_08 in leftJoin08.DefaultIfEmpty()
                        join t11_09 in context.T11CallCancelCodes on t19.CancelCd9 equals t11_09.CancelCd into leftJoin09
                        from t11_09 in leftJoin09.DefaultIfEmpty()
                        join t11_10 in context.T11CallCancelCodes on t19.CancelCd10 equals t11_10.CancelCd into leftJoin10
                        from t11_10 in leftJoin10.DefaultIfEmpty()
                        join t11_11 in context.T11CallCancelCodes on t19.CancelCd11 equals t11_11.CancelCd into leftJoin11
                        from t11_11 in leftJoin11.DefaultIfEmpty()
                        where t19.CancelDate >= Convert.ToDateTime(frmDate) && t19.CancelDate <= Convert.ToDateTime(toDate) && t19.CaseNo.StartsWith(Region)
                        orderby t17.CallCancelStatus, t09.IeName, v05.Vendor, t19.CancelDate
                        select new Statement_IeVendorWiseModel
                        {
                          RLY_CD = t13.RlyCd,
                          IE_NAME = t09.IeName,
                           VENDOR =  v05.Vendor,
                           CASE_NO = t19.CaseNo,
                           CALL_RECV_DT = t19.CallRecvDt,
                           CANCEL_DT = Convert.ToDateTime(t19.CancelDate),
                            CALL_CANCEL_STATUS = t17.CallCancelStatus == "C" ? "Chargeable" : "Non-Chargeable",
                           CALL_CANCEL_CHARGES = t17.CallCancelCharges,
                            CANCEL_REASON = string.Join(" ", new[]
                            {
                                (t19.CancelCd1 == 0 ? " " : $"(1) {t11_01.CancelDesc}"),
                                (t19.CancelCd2 == 0 ? " " : $"(2) {t11_02.CancelDesc}"),
                                (t19.CancelCd3 == 0 ? " " : $"(3) {t11_03.CancelDesc}"),
                                (t19.CancelCd4 == 0 ? " " : $"(4) {t11_04.CancelDesc}"),
                                (t19.CancelCd5 == 0 ? " " : $"(5) {t11_05.CancelDesc}"),
                                (t19.CancelCd6 == 0 ? " " : $"(6) {t11_06.CancelDesc}"),
                                (t19.CancelCd7 == 0 ? " " : $"(7) {t11_07.CancelDesc}"),
                                (t19.CancelCd8 == 0 ? " " : $"(8) {t11_08.CancelDesc}"),
                                (t19.CancelCd9 == 0 ? " " : $"(9) {t11_09.CancelDesc}"),
                                (t19.CancelCd10 == 0 ? " " : $"(10) {t11_10.CancelDesc}"),
                                (t19.CancelCd11== 0 ? " " : $"(11) {t11_11.CancelDesc}"),
                                t19.CancelDesc == "" ? " " : $" --> {t19.CancelDesc}"
                            })
                        };

           
            var result = query.ToList();

           model.statement_IeVendorWiseModels = result;




            return model;
        }

        public Statement_IeVendorWiseModel Statement_OverdueCalls(string ReportType, string WiseRadio, string IeStatus, int Days, string includeNSIC, string pendingCallsOnly , string Region)
        {

            Statement_IeVendorWiseModel model = new();
            List<Statement_IeVendorWiseModel> statement_IeVendorWiseModels = new();
            //model.FromDate = frmDate;
            //model.ToDate = toDate;
            model.ToDate = Convert.ToString(DateTime.Now);
            model.ReportType = ReportType;
            var query = (from t17 in context.T17CallRegisters
                        join t09 in context.T09Ies on t17.IeCd equals t09.IeCd
                        join v05 in context.V05Vendors on t17.CoCd equals v05.VendCd
                        join t21 in context.T21CallStatusCodes on t17.CallStatus equals t21.CallStatusCd
                        join t13 in context.T13PoMasters on t17.CaseNo equals t13.CaseNo
                        join t08 in context.T08IeControllOfficers on t17.CoCd equals t08.CoCd
                         where t17.CallRecvDt > new DateTime(2013, 1, 1) 
                               && t17.RegionCode == Region
                         select new Statement_IeVendorWiseModel
                        {

                            ReportType = ReportType,
                            CASE_NO = t17.CaseNo,
                            CALL_RECV_DT = Convert.ToDateTime(t17.CallRecvDt),
                            CALL_SNO = t17.CallSno,
                            CALL_STATUS_DESC = t21.CallStatusDesc,
                            VENDOR = v05.Vendor,
                            IE_NAME = t09.IeName,
                            CALL_STATUS_DATE = Convert.ToDateTime(t17.CallStatusDt),
                            CO_NAME = t08.CoName,
                            RLY_CD = t13.RlyCd,
                            DESIRE_DT = Convert.ToDateTime(t13.Datetime)
                        }).Take(10000);



            var result = query.ToList();

            model.statement_IeVendorWiseModels = result;




            return model;

        }

        public Statement_IeVendorWiseModel Statement_ApprovalReport(string ReportType, string frmDate, string toDate, string Region)
        {

            Statement_IeVendorWiseModel model = new();
            List<Statement_IeVendorWiseModel> statement_IeVendorWiseModels = new();
            model.FromDate = frmDate;
            model.ToDate = toDate;
            model.ToDate = Convert.ToString(DateTime.Now);
            model.ReportType = ReportType;

            DateTime parsedDate = DateTime.ParseExact(frmDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime parsedDate1 = DateTime.ParseExact(toDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            // Format the date as "yyyyMMdd"
            string formattedDate = parsedDate.ToString("yyyyMMdd");
            string formattedDate1 = parsedDate1.ToString("yyyyMMdd");

            var query = from t17 in context.T17CallRegisters
                         join t19 in context.T19CallCancels on new { t17.CaseNo, t17.CallRecvDt, t17.CallSno } equals new { t19.CaseNo, t19.CallRecvDt, t19.CallSno }
                         join t05 in context.T05Vendors on t17.MfgCd equals t05.VendCd
                        where t17.CallStatus == "C" && t19.DocsSubmitted == "Y" &&
                              t19.Datetime >= DateTime.ParseExact(formattedDate, "yyyyMMdd", CultureInfo.InvariantCulture) &&
                              t19.Datetime <= DateTime.ParseExact(formattedDate1, "yyyyMMdd", CultureInfo.InvariantCulture) &&
                              t17.CaseNo.StartsWith(Region)
                        orderby t19.CancelDate
                         select new Statement_IeVendorWiseModel
                         {
                             CASE_NO = t17.CaseNo,
                             CALL_RECV_DT = t17.CallRecvDt,
                             CALL_DT_CONCAT = t17.CallRecvDt,
                             CALL_SNO = t17.CallSno,
                             MFG_CD = Convert.ToString(t17.MfgCd),
                             MFG_PLACE = t17.MfgPlace,
                             CO_CD = t17.CoCd ?? 0,
                             MFG = t05.VendName,
                             DESIRE_DATE = Convert.ToString(t17.Datetime),
                             USER_ID = t19.UserId,
                             APPROVAL_DATE = Convert.ToDateTime(t19.Datetime),
                             CANC_DOC = $"CALL_CANCELLATION_DOCUMENTS/{t17.CaseNo}-{t17.CallRecvDt.ToString("yyyyMMdd")}-{t17.CallSno}.PDF"
                         };




            var result = query.ToList();

            model.statement_IeVendorWiseModels = result;




            return model;
           
        }

        public Statement_IeVendorWiseModel Statement_SpecificPO(string ReportType, string PO_NO, string PO_DT, string RLY_NONRLY, string RLY_CD)
        {

            Statement_IeVendorWiseModel model = new();
            List<Statement_IeVendorWiseModel> statement_IeVendorWiseModels = new();
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
                        select new Statement_IeVendorWiseModel
                        {
                            VENDOR = t051.VendName.Trim() + (t03 != null ? ", " + t03.City : ""),
                            MANUFACTURER = (t052 != null ? t052.VendName.Trim() + (t032 != null ? ", " + t032.City : "") : ""),
                            VEND_CD = t051.VendCd,
                            MFG_CD = Convert.ToString(t052.VendCd),
                            CONSIGNEE = t06.ConsigneeDesig + " " + t06.ConsigneeFirm,
                            ITEM_DESC_PO = t18.ItemDescPo,
                            QTY_TO_INSP = Convert.ToString(t18.QtyToInsp),
                            CALL_MARK_DT = Convert.ToDateTime(t17.CallMarkDt),
                            IE_NAME = t09.IeName,
                            IE_PHONE_NO = t09.IePhoneNo,
                            PO_NO = t13.PoNo,
                            PO_DT = Convert.ToDateTime(t13.PoDt),
                            CASE_NO = t17.CaseNo,
                            REMARK = t17.Remarks,
                            COLOUR = t21.CallStatusColor,
                            MFG_PERS = t052.VendContactPer1,
                            MFG_PHONE = t052.VendContactTel1,
                            CALL_SNO = Convert.ToInt32(t17.CallSno),
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
            model.statement_IeVendorWiseModels = result;

            return model;

        }


    }
}
