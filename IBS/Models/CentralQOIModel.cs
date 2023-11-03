using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class CentralQOIModel
    {
        [Required(ErrorMessage = "Total Qty Dispatched is required")]
        public long? TotalQtyDispatched { get; set; }

        public long? NoOfIcIssued { get; set; }
        [Required(ErrorMessage = "Client is required")]
        public string Clients { get; set; }
        public string ClientName { get; set; }

        public string QtyDate { get; set; }
        [Required(ErrorMessage = "For The Period Month is required")]
        public string Month { get; set; }
        [Required(ErrorMessage = "For The Period Year is required")]
        public string Year { get; set; } 

        public string? RegionCode { get; set; }

        public string? UserId { get; set; }

        public DateTime? Datetime { get; set; }

        public int? Createdby { get; set; }

        public DateTimeOffset? Createddate { get; set; }

        public int? Updatedby { get; set; }

        public DateTimeOffset? Updateddate { get; set; }

        public byte? Isdeleted { get; set; }
        public bool IsEdited { get; set; }
    }
}
