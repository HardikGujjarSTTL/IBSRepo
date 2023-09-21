namespace IBS.Models.Reports
{
    public class PCDOReportsModel
    {
        public string sn { get; set; }
        public string Region_Code { get; set; }
        public decimal Hight_text { get; set; }
    }
    public class HighlightReportsModel
    {
        public string sn { get; set; }
        public string Region_Code { get; set; }
        public string Hight_text { get; set; }
    }
    public class FinancialBillingModel
    {
        public string Region_Code { get; set; }
        public string Serial_Code { get; set; }
        public decimal DR_FEE { get; set; }
        public decimal DNR_FEE { get; set; }
        public decimal DRI_FEE { get; set; }
        public decimal DCR_FEE { get; set; }
        public decimal DCNR_FEE { get; set; }
        public decimal DCRI_FEE { get; set; }
        public decimal R_FEE { get; set; }
        public decimal NR_FEE { get; set; }
        public decimal RI_FEE { get; set; }
        public decimal CR_FEE { get; set; }
        public decimal CNR_FEE { get; set; }
        public decimal CRI_FEE { get; set; }
        public decimal DTM_FEE { get; set; }
        public decimal CTM_FEE { get; set; }
        public decimal CDTM_FEE { get; set; }
        public decimal CCTM_FEE { get; set; }
        public decimal B_TARGET { get; set; }
        public decimal CCTM_adj_Fee { get; set; }
        public decimal CUTM_adj_Fee { get; set; }
        public decimal CCTM_adj_Fee_past { get; set; }
        public decimal CUTM_adj_Fee_past { get; set; }
    }

    public class FinancialExpenditureRealizationMainModel
    {
        public List<FinancialExpenditureRealizationModel> financialExpenditureRealizationModels { get; set; }
        public List<RealisationModel> realisationModel { get; set; }
        public List<Realisation1Model> realisation1Model { get; set; }
        public List<Realisation2Model> realisation2Model { get; set; }
    }
    public class FinancialExpenditureRealizationModel
    {
        public string Region_Code { get; set; }
        public string Serial_Code { get; set; }
        public decimal DTM_EXP_FEE { get; set; }
        public decimal DTM_TAX_FEE { get; set; }
        public decimal CTM_EXP_FEE { get; set; }
        public decimal CTM_TAX_FEE { get; set; }
        public decimal CDTM_EXP_FEE { get; set; }
        public decimal CDTM_TAX_FEE { get; set; }
        public decimal CCTM_EXP_FEE { get; set; }
        public decimal CCTM_TAX_FEE { get; set; }
        public decimal TOA1 { get; set; }
        public decimal TOA2 { get; set; }
        public decimal OR_Actual { get; set; }
        public decimal TOR { get; set; }
        public decimal CUTM_adj_Fee { get; set; }
        
    }

    public class RealisationModel
    {
        public string Region { get; set; }
        public string Serial_Code { get; set; }
        public decimal real_amt { get; set; }
    }
    public class Realisation1Model
    {
        public string Region { get; set; }
        public string Serial_Code { get; set; }
        public decimal real_amt { get; set; }
    }
    public class Realisation2Model
    {
        public string region_code { get; set; }
        public string Serial_Code { get; set; }
        public decimal bill_am { get; set; }
        public decimal cubill_amt { get; set; }
    }
}
