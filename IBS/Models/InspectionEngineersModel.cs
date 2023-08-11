using IBS.DataAccess;

namespace IBS.Models
{
    public class InspectionEngineersModel
    {
        public int IeCd { get; set; }

        public string? IeName { get; set; }

        public string? IeSname { get; set; }

        public string? IeEmpNo { get; set; }

        public int? IeDesig { get; set; }

        public int? IeSealNo { get; set; }

        public string? IeDepartment { get; set; }

        public int? IeCityCd { get; set; }

        public string? IePhoneNo { get; set; }

        public byte? IeCoCd { get; set; }

        public DateTime? IeJoinDt { get; set; }

        public string? IeStatus { get; set; }

        public DateTime? IeStatusDt { get; set; }

        public string? IeType { get; set; }

        public string? IeRegion { get; set; }

        public string? IePwd { get; set; }

        public string? UserId { get; set; }

        public DateTime? Datetime { get; set; }

        public string? IeEmail { get; set; }

        public DateTime? IeDob { get; set; }

        public int? AltIe { get; set; }

        public string? IeCallMarking { get; set; }

        public int? AltIeTwo { get; set; }

        public int? AltIeThree { get; set; }

        public DateTime? CallMarkingStoppingDt { get; set; }

        public DateTime? DscExpiryDt { get; set; }
        public byte? Isdeleted { get; set; }

        public int? Createdby { get; set; }

        public int? Updatedby { get; set; }

        public DateTime? Createddate { get; set; }

        public DateTime? Updateddate { get; set; }

        public virtual T08IeControllOfficer? IeCoCdNavigation { get; set; }

        public virtual T07RitesDesig? IeDesigNavigation { get; set; }

        public virtual T01Region? IeRegionNavigation { get; set; }

        public virtual ICollection<NoIeWorkPlan> NoIeWorkPlans { get; set; } = new List<NoIeWorkPlan>();

        public virtual ICollection<T16IcCancel> T16IcCancels { get; set; } = new List<T16IcCancel>();

        public virtual ICollection<T17CallRegister> T17CallRegisters { get; set; } = new List<T17CallRegister>();

        public virtual ICollection<T20Ic> T20Ics { get; set; } = new List<T20Ic>();

        public virtual ICollection<T45ClaimMaster> T45ClaimMasters { get; set; } = new List<T45ClaimMaster>();

        public virtual ICollection<T47IeWorkPlan> T47IeWorkPlans { get; set; } = new List<T47IeWorkPlan>();

        public virtual ICollection<T48NiIeWorkPlan> T48NiIeWorkPlans { get; set; } = new List<T48NiIeWorkPlan>();

        public virtual T70UnregisteredCall? T70UnregisteredCall { get; set; }
    }
}
