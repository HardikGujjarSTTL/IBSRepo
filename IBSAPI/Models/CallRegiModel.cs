using IBSAPI.DataAccess;
using System.ComponentModel.DataAnnotations;

namespace IBSAPI.Models
{
    public class CallRegiModel
    {
        public string CaseNo { get; set; } 
        public DateTime? CallDate { get; set; }
        public int CallSNo { get; set; }

        public string? Purchaser { get; set; }
        public string? Vendor { get; set; }

        public DateTime? PurchaseOrderDate { get; set; }

        public string? PurchaseOrderNo { get; set; }

        public string? CallStatus { get; set; }
        public string? Region { get; set; }

        public string? PlaceofInspection { get; set; }

        public string? ContactPersonName { get; set; }

        public string? ManufacturerEmail { get; set; }

        public string? PhoneNumber { get; set; }

    }
}
