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
        public decimal TY_Days { get; set; }
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

    public class ProgressofChecksheetsModel
    {
        public string rn { get; set; }
        public string ITEM_DESC { get; set; }
        public string ie { get; set; }
        public string co_name { get; set; }
        public string creation_rev_dt { get; set; }
        public string region_cd { get; set; }
    }

    public class ComplaintsMainModel
    {
        public List<ComplaintsModel> complaintsModels { get; set; }
        public List<JIdisposedComplaintsModel> jIdisposedComplaintsModels { get; set; }
        public List<BreakupComplaintsModel> breakupComplaintsModels { get; set; }
        public List<OthercasesComplaintsModel> othercasesComplaintsModels { get; set; }
        public List<OthercasesComplaintsModel> othercasesComplaintsE { get; set; }
        public List<OthercasesComplaintsModel> othercasesComplaintsW { get; set; }
        public List<OthercasesComplaintsModel> othercasesComplaintsS { get; set; }
        public List<Long_pendingModel> long_PendingModels { get; set; }
        public List<CR_REJModel> cR_REJModels { get; set; }
    }

    public class ComplaintsModel
    {
        public string region { get; set; }
        public string Serial_Code { get; set; }
        public decimal Rec_All_Complaints { get; set; }
        public decimal Rec_Com_JI { get; set; }
        public decimal Des_All_Complaints { get; set; }
        public decimal Des_req_JI { get; set; }
        public decimal Des_AllFinal_Complaints { get; set; }
    }

    public class JIdisposedComplaintsModel
    {
        public string region { get; set; }
        public string Serial_Code { get; set; }
        public decimal Material_Accepted { get; set; }
        public decimal Finally_Rejected { get; set; }
        public decimal Sorting { get; set; }
        public decimal Rectification { get; set; }
        public decimal Price_Reduction { get; set; }
        public decimal Lifted_before_JI { get; set; }
        public decimal Not_on_Rites_Ac { get; set; }
        public decimal Transit_demand { get; set; }
        public decimal Unstamped { get; set; }
        public decimal deleted { get; set; }
    }

    public class BreakupComplaintsModel
    {
        public string region { get; set; }
        public string Serial_Code { get; set; }
        public decimal D { get; set; }
        public decimal S { get; set; }
        public decimal W { get; set; }
        public decimal E { get; set; }
        public decimal C_C { get; set; }
        public decimal P { get; set; }
        public decimal N { get; set; }
        public decimal M { get; set; }
        public decimal L { get; set; }
        public decimal O { get; set; }
    }

    public class OthercasesComplaintsModel
    {
        public string region { get; set; }
        public decimal Rec_Com_JI { get; set; }
        public decimal Des_req_JI { get; set; }
        public string region_cd { get; set; }

    }

    public class Long_pendingModel
    {
        public string region { get; set; }
        public string ji_sno { get; set; }
        public string Des_req_JI { get; set; }
        public string pending_date { get; set; }
        public string remarks { get; set; }
        public string region_cd { get; set; }
    }

    public class CR_REJModel
    {
        public string case_No { get; set; }
        public string consignee { get; set; }
        public string des_com { get; set; }
        public string conclusion { get; set; }
    }

    public class QualityofInspectionModel
    {
        public string R_NAME { get; set; }
        public decimal C1 { get; set; }
        public decimal C2 { get; set; }
        public decimal C3 { get; set; }
        public decimal C4 { get; set; }
        public decimal C5 { get; set; }
        public decimal C6 { get; set; }
        public decimal AVE_Rejection { get; set; }
        public decimal AVE_Cancell { get; set; }
        public decimal AVE_Upto7 { get; set; }
        public decimal AVE_Beyond7 { get; set; }
    }

    public class QualityofInspectionCentralMainModel
    {
        public List<QualityofInspectionCentralModel> qualityofInspectionCentralModels { get; set; }
        public List<QualityofInspectionCentral_RSMModel> qualityofInspectionCentral_RSMModels { get; set; }
        public List<QualityofInspectionCentral_URMModel> qualityofInspectionCentral_URMModels { get; set; }
        public List<QualityofInspectionCentral_JINDALModel> qualityofInspectionCentral_JINDALModels { get; set; }

    }

    public class QualityofInspectionCentralModel
    {
        public string Client_name { get; set; }
        public string Serial_cd { get; set; }
        public decimal IC_Issued { get; set; }
        public decimal IC_Issued_cum { get; set; }
        public decimal TQY_Dis { get; set; }
        public decimal TQY_Dis_cum { get; set; }
    }

    public class QualityofInspectionCentral_RSMModel
    {
        public string QOI_LEN { get; set; }
        public decimal ACC52 { get; set; }
        public decimal REJ52 { get; set; }
        public decimal ACC60 { get; set; }
        public decimal REJ60 { get; set; }
    }

    public class QualityofInspectionCentral_URMModel
    {
        public string QOI_LEN { get; set; }
        public decimal acc_urm { get; set; }
        public decimal rej_urm { get; set; }
    }

    public class QualityofInspectionCentral_JINDALModel
    {
        public string QOI_LEN { get; set; }
        public decimal acc_urm { get; set; }
        public decimal rej_urm { get; set; }
    }

    public class ImprovementInQualityofServiceMainModel
    {
        public List<ImprovementInQualityofServiceModel> improvementInQualityofServiceModels { get; set; }
        public List<ImprovementInQualityofService1Model> improvementInQualityofService1Models { get; set; }
        public List<ImprovementInQualityofService2Model> improvementInQualityofService2Models { get; set; }

    }

    public class ImprovementInQualityofServiceModel
    {
        public string Region { get; set; }
        public string Serial_Code { get; set; }
        public decimal total { get; set; }
    }
    public class ImprovementInQualityofService1Model
    {
        public string COD { get; set; }
        public string NameDesig { get; set; }
        public string IEName { get; set; }
        public string Firm { get; set; }
        public string Item { get; set; }
        public string PCR { get; set; }
        public string Disc { get; set; }
        public string otcome { get; set; }
        public string region_cd { get; set; }

    }

    public class ImprovementInQualityofService2Model
    {
        public string CO_NAME { get; set; }
        public string TOTAL_NO_CALLS { get; set; }
        public decimal TOTAL_NC { get; set; }
        public string NOIC { get; set; }
        public string co_cd { get; set; }
    }

    public class OutstandingRailwaysModel
    {
        public string bpo_orgn { get; set; }
        public string NR { get; set; }
        public string ER { get; set; }
        public string WR { get; set; }
        public string SR { get; set; }
        public string CR { get; set; }
        public string Total { get; set; }
    }

    public class OutstandingNonRailwaysModel
    {
        public string bpo_orgn { get; set; }
        public string NR { get; set; }
        public string ER { get; set; }
        public string WR { get; set; }
        public string SR { get; set; }
        public string CR { get; set; }
        public string Total { get; set; }
        public string rn { get; set; }
    }

    public class Top5OutstandingRailwayNonRailwaysMainModel
    {
        public List<Top5OutstandingRailwayNonRailwaysModel> top5OutstandingRailwayNonRailwaysModels { get; set; }
        public List<Top5OutstandingRailwayNonRailways1Model> top5OutstandingRailwayNonRailways1Models { get; set; }

    }

    public class Top5OutstandingRailwayNonRailwaysModel
    {
        public string bpo_orgn { get; set; }
        public string NR { get; set; }
        public string ER { get; set; }
        public string WR { get; set; }
        public string SR { get; set; }
        public string CR { get; set; }
        public string Total { get; set; }
        public string rn { get; set; }
    }

    public class Top5OutstandingRailwayNonRailways1Model
    {
        public string bpo_orgn { get; set; }
        public string NR { get; set; }
        public string ER { get; set; }
        public string WR { get; set; }
        public string SR { get; set; }
        public string CR { get; set; }
        public string Total { get; set; }
        public string rn { get; set; }
    }

    public class ClientContactModel
    {
        public string sn { get; set; }
        public string Visit_dt { get; set; }
        public string Detail_visit { get; set; }
        public string Rites_officer { get; set; }
        public string highlights { get; set; }
        public string overall_outcome { get; set; }
        public string region_cd { get; set; }
        public string out_amt { get; set; }
    }
    public class TrainingMainModel
    {
        public List<TrainingModel> trainingModels { get; set; }
        public List<Training1Model> training1Models { get; set; }

    }

    public class TrainingModel
    {
        public string region_code { get; set; }
        public string Man_days { get; set; }
        public string Trainee { get; set; }
        public string Cum_Mandays { get; set; }
        public string Cum_Trainee { get; set; }
        public string IMan_days { get; set; }
        public string ITrainee { get; set; }
        public string Icum_Mandays { get; set; }
        public string Icum_Trainee { get; set; }
    }

    public class Training1Model
    {
        public string region { get; set; }
        public string course_name { get; set; }
        public string course_dur_fr { get; set; }
        public string course_dur_to { get; set; }
        public string category { get; set; }
        public string name { get; set; }
    }

    public class TechnicalReferencesModel
    {
        public string sn { get; set; }
        public string cm_name { get; set; }
        public string ie_name { get; set; }
        public string item_des { get; set; }
        public string spec_drg { get; set; }
        public string letter_no { get; set; }
        public string tech_date { get; set; }
        public string ref_made { get; set; }
        public string tech_content { get; set; }
        public string region_cd { get; set; }
    }

    public class PCDOSummaryModel
    {
        public string Region_Code { get; set; }
        public string Serial_Code { get; set; }
        public string TURN_MONTH { get; set; }
        public string TURN_LAB_MONTH { get; set; }
        public string ADJ_MONTH { get; set; }
        public string TURN_UPTO { get; set; }
        public string TURN_LAB_UPTO { get; set; }
        public string ADJ_UPTO { get; set; }
        public string B_TARGET { get; set; }
        public string CONT_MONTH { get; set; }
        public string CONT_UPTO { get; set; }
        public string SUPER_UPTO { get; set; }
        public string TECH_UPTO { get; set; }
        public string CHECK_UPTO { get; set; }
        public string CCTM_EXP_FEE { get; set; }
        public string OR_Actual { get; set; }
    }
}
