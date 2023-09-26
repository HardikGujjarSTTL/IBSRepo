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

    public class FinancialOutstandingMainModel
    {
        public List<FinancialOutstandingModel> financialOutstandingModels { get; set; }
        public List<FinancialOutstanding1Model> financialOutstanding1Models { get; set; }
    }
    public class FinancialOutstandingModel
    {
        public string region_code { get; set; }
        public string Serial_Code { get; set; }
        public decimal lm_amt { get; set; }
        public decimal sinc_amt { get; set; }
        public decimal cum_amt { get; set; }
        public decimal Pri_amt { get; set; }
        public decimal amt_out { get; set; }
        public decimal tot_exp { get; set; }
        public decimal bill_amt { get; set; }
        public string days { get; set; }
    }
    public class FinancialOutstanding1Model
    {
        public string region_code { get; set; }
        public string Serial_Code { get; set; }
        public decimal amtR { get; set; }
        public decimal amtNR { get; set; }
        public decimal total { get; set; }
    }

    public class COHighlightMainModel
    {
        public List<COHighlightModel> cOHighlightModels { get; set; }
        public List<COHighlight1Model> cOHighlight1Models { get; set; }
    }
    public class COHighlightModel
    {
        public string Region_Code { get; set; }
        public string Serial_Code { get; set; }
        public decimal TY_Turn_T { get; set; }
        public decimal TY_Turnl_T { get; set; }
        public decimal T_B_TARGET { get; set; }
        public decimal TY_real_amt { get; set; }
        public decimal TY_Out_amt { get; set; }
        public decimal TY_tot_sus { get; set; }
        public decimal TY_bill_amt { get; set; }
        public decimal TY_EXP_FEE { get; set; }
        public decimal TY_TOA2 { get; set; }
        public int TY_Days { get; set; }
        public decimal TY_OR_Actual { get; set; }
        public decimal TY_BRO_TARGET { get; set; }
        public decimal LY_turn { get; set; }
        public decimal LY_turnl { get; set; }
        public decimal TOR { get; set; }
        public decimal Exp_pro_target { get; set; }
        public decimal LY_EXP_FEE { get; set; }
        public decimal LY_TOA1 { get; set; }
        public decimal LY_TOA2 { get; set; }
        public decimal LY_OR_Actual { get; set; }
    }

    public class COHighlight1Model
    {
        public string sn { get; set; }
        public string Region_Code { get; set; }
        public string Hight_text { get; set; }
    }
    public class EOIPricedOfferSentModel
    {
        public string CONTRACT_ID { get; set; }
        public string CLIENT_NAME { get; set; }
        public string CONTRACT_NO { get; set; }
        public string PER_FROM { get; set; }
        public string PER_TO { get; set; }
        public string CONTRACT_FEE_NUM { get; set; }
        public string CO_NAME { get; set; }
        public string CONTRACT_SPECIAL_CONDN { get; set; }
        public string CONTRACT_PANALTY { get; set; }
        public string CONT_INSP_FEE { get; set; }
        public string SCOPE_OF_WORK { get; set; }
        public string OFFER_DATE { get; set; }
        public string EXP_OR { get; set; }
        public string REGION { get; set; }
        public string DURATION { get; set; }
    }

    public class BDEffortsModel
    {
        public string sn { get; set; }
        public string Visit_dt { get; set; }
        public string Rites_officer { get; set; }
        public string Organisation_visited { get; set; }
        public string Detail_visit { get; set; }
        public string highlights { get; set; }
        public string overall_outcome { get; set; }
        public string region_cd { get; set; }
    }

}
