using IBS.Models.Reports;
using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class BillRaisedModel
    {
        public string Title { get; set; }

        public string Region { get; set; } = null!;

        public string? BillSummary { get; set; }
        
        public int FromMn { get; set; }
        
        public int FromYr { get; set; }

        public int ToMn { get; set; }

        public int ToYr { get; set; }

        public string FromMonthName { get; set; }

        public string ToMonthName { get; set; }

        public string IncRites { get; set; }

        public string ActionType { get; set; }

        public string BPO_TYPE { get; set; }

        public string BPO_RLY { get; set; }

        public string BPO_ORGN { get; set; }

        public decimal? INSP_FEE { get; set; }

        public decimal? TAX { get; set; }

        public decimal? BILL_AMOUNT { get; set; }

        public decimal? NO_OF_BILLS { get; set; }

        public List<BillRaisedListModel> lstBill { get; set; }

        //Sector Wise Billing Report
        public string? SECTOR { get; set; }

        public decimal? SERVICE_TAX { get; set; }

        public decimal? EDU_CESS { get; set; }

        public decimal? SHE_CESS { get; set; }

        public decimal? SWACHH_BHARAT_CESS { get; set; }

        public decimal? KRISHI_KALYAN_CESS { get; set; }

        public decimal? CGST { get; set; }

        public decimal? SGST { get; set; }

        public decimal? IGST { get; set; }

        public List<BillSectorListModel> lstBillSector { get; set; }

        //Railway online bills
        public string ClientType { get; set; }

        public string BpoType { get; set; }

        public string BpoRly { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? FromDt { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? ToDt { get; set; }

        public string chkRegion { get; set; }

        public string? PO_NO { get; set; }

        public string? PO_DT { get; set; }

        public string? CASE_NO { get; set; }

        public string? PO_OR_LETTER { get; set; }

        public string? BPO_NAME { get; set; }

        public string? BILL_NO { get; set; }

        public string? BILL_DT { get; set; }

        public string? INVOICE_NO { get; set; }

        public string? AU_DESC { get; set; }

        public string? IC_NO { get; set; }

        public string? IC_DT { get; set; }

        public string? BK_NO { get; set; }

        public string? SET_NO { get; set; }

        public string? AMOUNT_OUTSTANDING { get; set; }

        public string? DIG_BILL_GEN_DATE { get; set; }

        public string? ONLINE_INVOICE { get; set; }

        public string? IC_PHOTO { get; set; }

        public string? PO_SOURCE { get; set; }

        public string? INVOICE_SUPP_DOCS { get; set; }

        public string? PO_YR { get; set; }

        public string? IMMS_RLY_CD { get; set; }

        public List<RailwayOnlineListModel> lstBillRailway { get; set; }

        public string AU { get; set; }

        public string lstAU { get; set; }

        public string RlyTypes { get; set; }

        public decimal? AMT_CLEARED { get; set; }

        public decimal? AMT_RECEIVED { get; set; }

        public string BILL_GEN_DATE { get; set; }

        public string IRN_NO { get; set; }

        public List<BillsNotCrisListModel> lstBillCris { get; set; }

        public int? BILL_RESENT_COUNT { get; set; }

        public string? CO6_STATUS { get; set; }

        public string RETURN_DT { get; set; }

        public string RETURN_REASON { get; set; }

        public List<BillSubmittedCrisModel> lstBillCrisSubmitted { get; set; }

    }

    public class BillRaisedListModel
    {
        public string BPO_TYPE { get; set; }

        public string BPO_RLY { get; set; }

        public string BPO_ORGN { get; set; }

        public decimal? INSP_FEE { get; set; }

        public decimal? TAX { get; set; }

        public decimal? BILL_AMOUNT { get; set; }

        public decimal? NO_OF_BILLS { get; set; }
    }

    public class BillSectorListModel
    {
        public string? SECTOR { get; set; }

        public decimal? INSP_FEE { get; set; }

        public decimal? SERVICE_TAX { get; set; }

        public decimal? EDU_CESS { get; set; }

        public decimal? SHE_CESS { get; set; }

        public decimal? SWACHH_BHARAT_CESS { get; set; }

        public decimal? KRISHI_KALYAN_CESS { get; set; }

        public decimal? CGST { get; set; }

        public decimal? SGST { get; set; }

        public decimal? IGST { get; set; }

        public decimal? BILL_AMOUNT { get; set; }

        public decimal? NO_OF_BILLS { get; set; }
    }

    public class RailwayOnlineListModel
    {
        public string? BPO_RLY { get; set; }

        public string? PO_NO { get; set; }

        public string? PO_DT { get; set; }

        public string? CASE_NO { get; set; }

        public string? PO_OR_LETTER { get; set; }

        public string? BPO_NAME { get; set; }

        public string? BILL_NO { get; set; }

        public string? BILL_DT { get; set; }

        public string? INVOICE_NO { get; set; }

        public string? AU_DESC { get; set; }

        public string? IC_NO { get; set; }

        public string? IC_DT { get; set; }

        public string? BK_NO { get; set; }

        public string? SET_NO { get; set; }

        public string? BILL_AMOUNT { get; set; }

        public string? AMOUNT_OUTSTANDING { get; set; }

        public string? DIG_BILL_GEN_DATE { get; set; }

        public string? ONLINE_INVOICE { get; set; }

        public string? IC_PHOTO { get; set; }

        public string? PO_SOURCE { get; set; }

        public string? INVOICE_SUPP_DOCS { get; set; }

        public string? PO_YR { get; set; }

        public string? IMMS_RLY_CD { get; set; }
    }

    public class BillsNotCrisListModel
    {
        public string BILL_NO { get; set; }

        public string BILL_DT { get; set; }

        public string BPO_RLY { get; set; }

        public decimal? BILL_AMOUNT { get; set; }

        public decimal? AMT_CLEARED { get; set; }

        public decimal? AMT_RECEIVED { get; set; }

        public string AU { get; set; }

        public string BILL_GEN_DATE { get; set; }

        public string INVOICE_NO { get; set; }

        public string PO_OR_LETTER { get; set; }

        public string IRN_NO { get; set; }
    }

    public class BillSubmittedCrisModel
    {
        public string BILL_NO { get; set; }

        public string BILL_DT { get; set; }

        public string BPO_RLY { get; set; }

        public string? BK_NO { get; set; }

        public decimal? BILL_AMOUNT { get; set; }

        public string? SET_NO { get; set; }

        public int? BILL_RESENT_COUNT { get; set; }

        public string? CO6_STATUS { get; set; }

        public string RETURN_DT { get; set; }

        public string RETURN_REASON { get; set; }

        public string AU { get; set; }
    }
}
