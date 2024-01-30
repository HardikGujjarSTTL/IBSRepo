using Humanizer.Localisation.TimeToClockNotation;
using NuGet.Protocol.Plugins;
using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class InspectionCertModel
    {
        public string Icno { get; set; }

        public string Caseno { get; set; } = null!;

        public string STS { get; set; }

        public string? Bkno { get; set; }

        public string? Setno { get; set; }

        public string Status { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? Callrecvdt { get; set; }

        public int? Callsno { get; set; }

        public string? Iesname { get; set; }

        public string Consignee { get; set; }

        public int ConsigneeCd { get; set; }

        public string? Callstatusdesc { get; set; }

        public string? Regioncode { get; set; }

        public string? Callstatus { get; set; }

        public string? CallCancelStatus { get; set; }

        public int? CallCancelCharges { get; set; }

        public string? CallCancelChargesStatus { get; set; }

        public decimal? CallCancelAmount { get; set; }

        public string CancellationStatus { get; set; }

        public string LocalOutstation { get; set; }

        public decimal? RejectionCharge { get; set; }

        public decimal? RejectMaterialValue { get; set; }

        public string Createdby { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int Updatedby { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string UserId { get; set; }

        public string IeName { get; set; }

        public int IcTypeId { get; set; }

        public int? IeCd { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? DtInspDesire { get; set; }

        public string RejCanCall { get; set; }

        public string Bpo { get; set; }

        public string BpoName { get; set; }

        public string BpoCd { get; set; }

        public string RecipientGstinNo { get; set; }

        public string IrfcBpo { get; set; }

        public string IrfcFunded { get; set; }

        public string PoNo { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? PoDt { get; set; }

        public string VendName { get; set; }

        public string PoSource { get; set; }

        public string RlyNonrly { get; set; }

        public string StockNonstock { get; set; }

        public bool? ConsigneeGST { get; set; }

        public string GstinNo { get; set; }

        public string LegalName { get; set; }

        public string BpoType { get; set; }

        public string Au { get; set; }

        public string OldAu { get; set; }

        public string BpoRly { get; set; }

        public decimal RlyBpoFee { get; set; }

        [Required]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Fee required.")]
        public decimal BpoFee { get; set; }

        [Required]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Adjustment Fee required.")]
        public decimal AdjustmentFee { get; set; }

        public string BpoFeeType { get; set; }

        public string BpoTaxType { get; set; }

        public string BillNo { get; set; }

        public string CnoteBillNo { get; set; }

        public decimal TMValue { get; set; }

        public decimal TMValueNew { get; set; }

        public decimal TMValueDiff { get; set; }

        public decimal TIFee { get; set; }

        public decimal TIFeeNew { get; set; }

        public decimal TIFeeDiff { get; set; }

        public decimal NetFee { get; set; }

        public decimal NetFeeNew { get; set; }

        public decimal NetFeeDiff { get; set; }

        public string InvoiceNo { get; set; }

        public string AdvBill { get; set; }

        public string CertNo { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? CertDt { get; set; }

        public int CallInstallNo { get; set; }

        public string FullPart { get; set; }

        public int NoOfInsp { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? CallDt { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? FirstInspDt { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? LastInspDt { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime LabTstRectDt { get; set; }

        public string OtherInspDt { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? ICSubmitDt { get; set; }

        public string StampPatternCd { get; set; }

        public string StampPattern { get; set; }

        public string ReasonReject { get; set; }

        public string Photo { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? BillDt { get; set; }

        public int MinFee { get; set; }

        public int MaxFee { get; set; }

        public string TaxType { get; set; }

        public string Remarks { get; set; }

        public string CanOrRejctionFee { get; set; }

        public string Pincode { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string StateID { get; set; }

        public string SelectRadio { get; set; }

        public string ActionType { get; set; }

        public string AccGroup { get; set; }

        public string BPOCall { get; set; }

        public string GSTINCall { get; set; }


        public int ItemSrnoPo { get; set; }

        public string ItemDescPo { get; set; }

        public string UomSDesc { get; set; }

        public decimal? QtyOrdered { get; set; }

        public decimal? CumQtyPrevOffered { get; set; }

        public decimal? CumQtyPrevPassed { get; set; }

        public decimal? QtyToInsp { get; set; }

        public decimal? QtyPassed { get; set; }

        public decimal? QtyRejected { get; set; }

        public decimal? QtyDue { get; set; }

        public decimal SuppNewRate { get; set; }

        public decimal? Rate { get; set; }

        public decimal? SalesTaxPer { get; set; }

        public decimal? SalesTax { get; set; }

        public string? ExciseType { get; set; }

        public decimal? ExcisePer { get; set; }

        public decimal? Excise { get; set; }

        public string? DiscountType { get; set; }

        public decimal? DiscountPer { get; set; }

        public decimal? Discount { get; set; }

        public string? OtChargeType { get; set; }

        public decimal? OtChargePer { get; set; }

        public decimal? OtherCharges { get; set; }

        public decimal? ServiceTax { get; set; }

        public decimal? EduCess { get; set; }

        public decimal? SheCess { get; set; }

        public decimal? AmountReceived { get; set; }

        public decimal? Tds { get; set; }

        public decimal? BillAmtCleared { get; set; }

        public string BillStatus { get; set; }

        public decimal? RetentionMoney { get; set; }

        public decimal? WriteOffAmt { get; set; }

        public decimal? ServTaxRate { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? TdsDt { get; set; }

        public string ScannedStatus { get; set; }

        public decimal? SwachhBharatCess { get; set; }

        public decimal? KrishiKalyanCess { get; set; }

        public decimal? Sgst { get; set; }

        public decimal? Cgst { get; set; }

        public decimal? Igst { get; set; }

        public decimal? TdsSgst { get; set; }

        public decimal? TdsCgst { get; set; }

        public decimal? TdsIgst { get; set; }

        public decimal? CnoteAmount { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? DigBillGenDt { get; set; }

        public string? BillResentStatus { get; set; }

        public int? BillResentCount { get; set; }

        public string? IrnNo { get; set; }

        public string? AckNo { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? AckDt { get; set; }

        public string? QrCode { get; set; }

        public string? SentToSap { get; set; }

        public string? BillFinalised { get; set; }

        public string? InvoiceSuppDocs { get; set; }

        public string? CreditDocId { get; set; }

        public string? LoRemarks { get; set; }

        public string? SapStatus { get; set; }

        public string chkABill { get; set; }

        public string UpdateStatus { get; set; }

        public string BillAdType { get; set; }

    }

    public class ICPopUpModel
    {
        public string Caseno { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? Callrecvdt { get; set; }

        public int? Callsno { get; set; }

        public string BillNo { get; set; }

        public decimal TMValue { get; set; }

        public decimal? TIFee { get; set; }

        public decimal? NetFee { get; set; }

        public decimal? BillAmount { get; set; }

        public string InvoiceNo { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? BillDt { get; set; }

        public string? Callstatus { get; set; }
    }

    public class InspectionCertItemListModel
    {
        public int ItemSrnoPo { get; set; }

        public string ItemDescPo { get; set; }

        public string UomSDesc { get; set; }

        public decimal? QtyOrdered { get; set; }

        public decimal? CumQtyPrevOffered { get; set; }

        public decimal? CumQtyPrevPassed { get; set; }

        public decimal? QtyToInsp { get; set; }

        public decimal? QtyPassed { get; set; }

        public decimal? QtyRejected { get; set; }

        public decimal? QtyDue { get; set; }

        public decimal SuppNewRate { get; set; }

        public decimal? Rate { get; set; }

        public decimal? SalesTaxPer { get; set; }

        public decimal? SalesTax { get; set; }

        public string? ExciseType { get; set; }

        public decimal? ExcisePer { get; set; }

        public decimal? Excise { get; set; }

        public string? DiscountType { get; set; }

        public decimal? DiscountPer { get; set; }

        public decimal? Discount { get; set; }

        public string? OtChargeType { get; set; }

        public decimal? OtChargePer { get; set; }

        public decimal? OtherCharges { get; set; }

        public decimal? ServiceTax { get; set; }

        public decimal MaterialValue { get; set; }
    }
}
