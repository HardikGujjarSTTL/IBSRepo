namespace IBS.Models
{
    public class AdministratorPurchaseOrderModel
    {
        public string CaseNo { get; set; } = null!;

        public int? PurchaserCd { get; set; }

        public string? StockNonstock { get; set; }

        public string? RlyNonrly { get; set; }

        public string? PoOrLetter { get; set; }

        public string? PoNo { get; set; }

        public string? L5noPo { get; set; }

        public DateTime? PoDt { get; set; }

        public DateTime? RecvDt { get; set; }

        public int? VendCd { get; set; }

        public string? RlyCd { get; set; }

        public string? RegionCode { get; set; }

        public string? UserId { get; set; }

        public DateTime? Datetime { get; set; }

        public string? Remarks { get; set; }

        public string? InspectingAgency { get; set; }

        public int? PoiCd { get; set; }

        public string? PoSource { get; set; }

        public byte? PendingCharges { get; set; }

        public DateTime? Createddate { get; set; }

        public int? Createdby { get; set; }

        public DateTime? Updateddate { get; set; }

        public int? Updatedby { get; set; }

        public byte? Isdeleted { get; set; }

        public int? Id { get; set; }
        public int? ddlManufac { get; set; }

        public string? ContractNo { get; set; }

        public DateTime? ContractDt { get; set; }

        public string? ProjectRef { get; set; }

        public long? MinFee { get; set; }

        public long? MaxFee { get; set; }

        public string? WithServTax { get; set; }
        public string? MPOI { get; set; }

    }

    public class AdministratorPurchaseOrderListModel
    {
        public string CASE_NO { get; set; }
        public string PO_NO { get; set; }
        public string PO_DT { get; set; }
        public string RLY_CD { get; set; }
        public string VEND_NAME { get; set; }
        public string CONSIGNEE_S_NAME { get; set; }
        public string INSPECTING_AGENCY { get; set; }
        public string SOURCE { get; set; }
        public string PO_YR { get; set; }
        public string PO_DOC { get; set; }
        public string PO_DOC1 { get; set; }
    }
}
