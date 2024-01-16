using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class ContractEntry
    {
        public int ID { get; set; }
        public string? LETTER_NO { get; set; }
        public DateTime? LETTER_DATE { get; set; }
        public DateTime? TPFROM { get; set; }
        public DateTime? TPTO { get; set; }

        [Required(ErrorMessage ="Organization Type is required")]
        public string CLIENTTYPE { get; set; }

        [Required(ErrorMessage = "Organization Name is required")]
        public string? CLIENTNAME { get; set; }
        public int? INSPFEE { get; set; }
        public int? MANDAYBASIS { get; set; }
        public int? LOTOFINSP { get; set; }
        public int? MATERIALVALUE { get; set; }
        public int? MINPOVAL { get; set; }
        public int? MAXPOVAL { get; set; }
        public string? Materialdescription { get; set; }
        public int? CALLCANCELATION { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public int? IsDeleted { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? updatedDate { get; set; }
        public string CLIENTCODE { get; set; }

        public string? InspectionfeeType { get; set; }

        public int? PerBasisFlatfee { get; set; }

        public int? MandayFlatfee { get; set; }

        public int? LumpsumFlatfee { get; set; }

        public int? PerBasisCancellation { get; set; }

        public int? MandayCancellation { get; set; }

        public int? LumpsumCancellation { get; set; }

        public int? PerBasisRejection { get; set; }

        public int? MandayRejection { get; set; }

        public int? LumpsumRejection { get; set; }

        public int? PerBasis { get; set; }

        public int? Manday { get; set; }

        public int? Lumpsum { get; set; }

        public int? Fromrs { get; set; }

        public int? Tors { get; set; }

        public List<ContractEntryList> lstContractEntryList { get; set; }
    }

    public partial class ContractEntryList
    {
        public int Id { get; set; }

        public int? ContractId { get; set; }

        public int? PerBasis { get; set; }

        public int? Manday { get; set; }

        public int? Lumpsum { get; set; }

        public int? Fromrs { get; set; }

        public int? Tors { get; set; }

        public string? Createdby { get; set; }

        public string? Updatedby { get; set; }

        public DateTime? Createddate { get; set; }

        public DateTime? Updatedate { get; set; }

        public int? Isdeleted { get; set; }
    }
}
