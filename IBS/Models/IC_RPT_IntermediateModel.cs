using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class IC_RPT_IntermediateModel
    {
        public string CASE_NO { get; set; }
        public string CONSIGNEE_CD { get; set; }
        public string DP_CONSIGNEE_CD { get; set; }
        public string ACTIONAR { get; set; }
        public string IE_CD { get; set; }

        public string PO_NO { get; set; }
        public string PO_DT { get; set; }

        public string Call_SNO { get; set; }
        public string CALL_STATUS { get; set; }
        public string CALL_STATUS_DT { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? Call_Recv_dt { get; set; }
        public string Display_Call_Recv_dt { get { return this.Call_Recv_dt != null ? Common.ConvertDateFormat(this.Call_Recv_dt.Value) : ""; } }

        public string Region_Code { get; set; }
        public string RLY_CD { get; set; }
        public string Call_Install_No { get; set; }
        public string IE_Sname { get; set; }
        public string Vend_Name { get; set; }
        public string Vend_Add1 { get; set; }
        public string Vend_Add2 { get; set; }
        public string Vend_City { get; set; }
        public string MFG_Name { get; set; }
        public string MFG_Add1 { get; set; }
        public string MFG_Add2 { get; set; }
        public string MFG_City { get; set; }
        public string MFG_PLACE { get; set; }
        public string CONSIGNEE_DESIG { get; set; }
        public string CONSIGNEE_CITYNAME { get; set; }
        public string CONSIGNEE_DEPT { get; set; }
        public string CONSIGNEE_FIRM { get; set; }
        public string PUR_DESIG { get; set; }
        public string PUR_CD { get; set; }
        public string PUR_DEPT { get; set; }
        public string PUR_FIRM { get; set; }
        public string PUR_City { get; set; }
        public string ITEM_SRNO_PO { get; set; }
        public string ITEM_DESC_PO { get; set; }
        public string UOM_S_DESC { get; set; }
        public string QTY_ORDERED { get; set; }
        public string CUM_QTY_PREV_OFFERED { get; set; }
        public string CUM_QTY_PREV_PASSED { get; set; }
        public string QTY_TO_INSP { get; set; }
        public string QTY_PASSED { get; set; }
        public string QTY_REJECTED { get; set; }
        public string QTY_DUE { get; set; }
        public string HOLOGRAM { get; set; }
        public string NUM_VISITS { get; set; }
        public string VISIT_DATES { get; set; }
        public string BPO_NAME { get; set; }
        public string BPO_ORGN { get; set; }
        public string City { get; set; }
        public string HOLOGRAMORG { get; set; }
        public string REMARK { get; set; }
        public string DT_INSP_DESIRE { get; set; }
        public string ITEM_REMARK { get; set; }
        public string AMENDMENT_1 { get; set; }
        public string AMENDMENTDT_1 { get; set; }
        public string AMENDMENT_2 { get; set; }
        public string AMENDMENTDT_2 { get; set; }
        public string AMENDMENT_3 { get; set; }
        public string AMENDMENTDT_3 { get; set; }
        public string AMENDMENT_4 { get; set; }
        public string AMENDMENTDT_4 { get; set; }
        public string BK_NO { get; set; }
        public string SET_NO { get; set; }
        public string VISITS_DATES { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime LAB_TST_RECT_DT { get; set; }
        public string PASSED_INST_NO { get; set; }
        public string CONSIGNEE_DTL { get; set; }
        public string BPO_DTL { get; set; }
        public string PUR_DTL { get; set; }
        public string PUR_AUT_DTL { get; set; }
        public string OFF_INST_NO_DTL { get; set; }
        public string UNIT_DTL { get; set; }
        public string DISPATCH_PACKING_NO { get; set; }
        public string INVOICE_NO { get; set; }
        public string NAME_OF_IE { get; set; }
        public string GOV_BILL_AUTH { get; set; }
        public string MAN_TYPE { get; set; }
        public string CONSIGNEE_DESG { get; set; }
        public string IE_STAMPS_DETAIL { get; set; }
        public string IE_STAMPS_DETAIL2 { get; set; }

        public virtual string CONSIGNEE_DESC { get; set; }
        public virtual string BPO_DESC { get; set; }
        public virtual string GBPO_AUTH { get; set; }
        public virtual string MANUFAC_DESC { get; set; }
        public virtual string PUR_AUTH_DESC { get; set; }
        public virtual string OFF_INST_NO { get; set; }
        public virtual string ITEM_UNIT { get; set; }

        public string Region { get; set; }
        List<PO_Amendments> lstAmendment { get; set; }
    }

    public class PO_Amendments
    {
        public int Sno { get; set; }
        public string Amendments { get; set; }
        public string Date { get; set; }
    }    
}
