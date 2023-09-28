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

        public string? Callstatusdesc { get; set; }

        public string? Regioncode { get; set; }

        public string? Callstatus { get; set; }

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

        public string BpoRly { get; set; }

        public decimal RlyBpoFee { get; set; }

        public decimal? BpoFee { get; set; }

        public string BpoFeeType { get; set; }

        public string BpoTaxType { get; set; }

        public string BillNo { get; set; }

        public decimal TMValue { get; set; }

        public decimal? TIFee { get; set; }

        public decimal? NetFee { get; set; }

        public string InvoiceNo { get; set; }

        public string AdvBill { get; set; }


        public string CertNo { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? CertDt { get; set; }

        public int CallInstallNo { get; set; }

        public string FullPart { get; set; }

        public int NoOfInsp { get; set; }

        public DateTime? CallDt { get; set; }

        public DateTime FirstInspDt { get; set; }

        public DateTime LastInspDt { get; set; }

        public DateTime LabTstRectDt { get; set; }

        public DateTime OtherInspDt { get; set; }

        public DateTime ICSubmitDt { get; set; }

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
        public decimal? Rate { get; set; }
        public decimal? SalesTaxPer { get; set; }
        public decimal? SalesTax { get; set; }
        public decimal? ExcisePer { get; set; }
        public decimal? Excise { get; set; }
        public decimal? DiscountPer { get; set; }
        public decimal? Discount { get; set; }
        public decimal? OtherCharges { get; set; }

        public string chkABill { get; set; }

        public string UpdateStatus { get; set; }

    }

    public class ICPopUpModel
    {
        public string Caseno { get; set; }

        public string BillNo { get; set; }

        public decimal TMValue { get; set; }

        public decimal? TIFee { get; set; }

        public decimal? NetFee { get; set; }

        public string InvoiceNo { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? BillDt { get; set; }
    }
}
