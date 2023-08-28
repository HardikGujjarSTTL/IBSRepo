namespace IBS.Models
{
    public class ConsigneeModel
    {
        public string CaseNo { get; set; } = null!;
        public int ConsigneeCd { get; set; }
        public string? BpoCd { get; set; }
    }
    public class ConsigneeListModel
    {
        public string CASE_NO { get; set; }
        public string CONSIGNEE_CD { get; set; }
        public string BPO_CD { get; set; }
        public string CONSIGNEE_NAME { get; set; }
        public string BPO_NAME { get; set; }
    }
}
