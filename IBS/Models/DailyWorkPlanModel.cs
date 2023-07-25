using IBS.DataAccess;

namespace IBS.Models
{
    public class DailyWorkPlanModel
    {
        public int IeCd { get; set; }

        public byte? CoCd { get; set; }

        public string? Reason { get; set; }

        public DateTime NwpDt { get; set; }

        public string? RegionCode { get; set; }

        public string? UserId { get; set; }

        public DateTime? Datetime { get; set; }

        public virtual T09Ie IeCdNavigation { get; set; } = null!;
    }
}
