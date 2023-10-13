using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using System.Drawing;

namespace IBS.Repositories
{
    public class Calls_Marked_ReportRepository : ICalls_Marked_ReportRepository
    {
        private readonly ModelContext context;
        public Calls_Marked_ReportRepository(ModelContext context)
        {
            this.context = context;
        }

        public Calls_Marked_ReportModel Query1(string pDtFr, string pDtTo, string pRegion, string pSortKey , int UserID , string wRgn_Name)
        {
            //string wSortHdr = "";
            Calls_Marked_ReportModel model = new();
            model.pDtFr = pDtFr;
            model.pDtTo = pDtTo;
            model.Uname = UserID;
            //string sortOrder;
            //string sortHeader;

            //if (pSortKey == "V")
            //{
            //    sortOrder = "VENDOR"; // Sort by vendor name
            //    sortHeader = "Report Sorted on Vendor Name";
            //}
            //else
            //{
            //    sortOrder = "CALL_MARK_DT"; // Sort by call date
            //    sortHeader = "Report Sorted on Call Date";
            //}

            // Perform the LINQ query with sorting
            var query = from t051 in context.T05Vendors
                        from t17 in context.T17CallRegisters
                        from t18 in context.T18CallDetails
                        from t13 in context.T13PoMasters
                        from t06 in context.T06Consignees.Where(c => c.ConsigneeCd == t13.PurchaserCd).DefaultIfEmpty()
                        from t09 in context.T09Ies
                        from t052 in context.T05Vendors.Where(v => v.VendCd == t17.MfgCd).DefaultIfEmpty()
                        from t03 in context.T03Cities.Where(c => c.CityCd == t051.VendCityCd)
                        from t032 in context.T03Cities.Where(c => c.CityCd == t052.VendCityCd).DefaultIfEmpty()
                        from t08 in context.T08IeControllOfficers.Where(o => o.CoCd == t09.IeCoCd).DefaultIfEmpty()
                        from t21 in context.T21CallStatusCodes.Where(s => s.CallStatusCd == t17.CallStatus)
                        from t15 in context.T15PoDetails.Where(d => d.CaseNo == t13.CaseNo)
                        where t051.VendCd == t13.VendCd &&
                              t13.CaseNo == t15.CaseNo &&
                              t15.CaseNo == t18.CaseNo &&
                              t15.ItemSrno == t18.ItemSrnoPo &&
                              t13.CaseNo == t17.CaseNo &&
                              t09.IeCd == t17.IeCd &&
                              t18.CallRecvDt == t17.CallRecvDt &&
                              t18.CallSno == t17.CallSno &&
                              (t17.CallMarkDt >= Convert.ToDateTime(pDtFr) && t17.CallMarkDt <= Convert.ToDateTime(pDtTo)) &&
                              t18.ItemSrnoPo == context.T18CallDetails
                                                       .Where(b => b.CaseNo == t18.CaseNo &&
                                                                   b.CallRecvDt == t18.CallRecvDt &&
                                                                   b.CallSno == t18.CallSno)
                                                       .Min(b => b.ItemSrnoPo) &&
                              t17.RegionCode == pRegion
                        orderby (pSortKey == "V" ? t051.VendName : ""), t17.CallMarkDt, t17.CallSno
                        select new Calls_Marked_ReportModel
                        {
                           
                            VENDOR = t051.VendName.Trim() + (t03.City != null ? "," + t03.City.Trim() : ""),
                            MANUFACTURER = t052.VendName.Trim() + (t032.City != null ? "," + t032.City.Trim() : ""),
                            VEND_CD = Convert.ToString(t051.VendCd),
                            MFG_CD = Convert.ToString(t052.VendCd),
                            CONSIGNEE = t06.ConsigneeDesig + " " + t06.ConsigneeFirm,
                            ITEM_DESC_PO = t18.ItemDescPo,
                            QTY_TO_INSP = Convert.ToDecimal(t18.QtyToInsp),
                            EXT_DELV_DT = Convert.ToString(t15.ExtDelvDt),
                            CALL_MARK_DT = Convert.ToString(t17.CallMarkDt),
                            IE_NAME = t09.IeName,
                            IE_PHONE_NO = t09.IePhoneNo,
                            PO_NO = t13.PoNo,
                            PO_DATE = Convert.ToString(t13.PoDt),
                            CASE_NO = t17.CaseNo,
                            REMARK = t17.Remarks,
                            COLOUR = t21.CallStatusColor,
                            MFG_PERS = t052.VendContactPer1,
                            MFG_PHONE = t052.VendContactTel1,
                            CALL_SNO = t17.CallSno,
                            COUNT = context.T18CallDetails.Count(a => a.CaseNo == t18.CaseNo && a.CallRecvDt == t18.CallRecvDt && a.CallSno == t18.CallSno),
                            CO_NAME = t08.CoPhoneNo != null ? t08.CoName.Trim() + " (Mob: " + t08.CoPhoneNo + ")" : t08.CoName.Trim(),
                            CALL_STATUS = (
                                t21.CallStatusDesc +
                                (t21.CallStatusCd == "A" ?
                                    (t17.BkNo != null ?
                                        " (BookSet-" + t17.BkNo + "/" + t17.SetNo + ") Dt: " + t17.CallStatusDt :
                                        "")
                                    : (t21.CallStatusCd == "B" ?
                                        " (Accepted on Dt:" + t17.CallStatusDt + ")"
                                        : (t21.CallStatusCd == "R" ?
                                            (t17.BkNo != null ?
                                                " (BookSet-" + t17.BkNo + "/" + t17.SetNo + ")"
                                                : "")
                                            : (t21.CallStatusCd == "G" ?
                                                " Dt: " + t17.CallStatusDt
                                                : " on " + t17.CallStatusDt))))
                                + (t17.CallCancelStatus == "N" ? " (Non Chargeable)" : (t17.CallCancelStatus == "C" ? " (Chargeable)" : "")
                           
                              ))

                        };


            // Execute the LINQ query
           

            model = query.FirstOrDefault();
            return model;

        }
    }
}
