using IBSAPI.DataAccess;
using IBSAPI.Helper;
using IBSAPI.Interfaces;
using IBSAPI.Models;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.ComponentModel;
using System.Data;
using System.Drawing;

namespace IBSAPI.Repositories
{
    public class CRISRepository : ICRISRepository
    {
        private readonly ModelContext context;

        public CRISRepository(ModelContext context)
        {
            this.context = context;
        }

        public CRISModel FindBillDetails(string BillNo)
        {
            CRISModel model = new CRISModel();

            model = (from V22 in context.V22Bills
                     join T01 in context.T01Regions on V22.RegionCode equals T01.RegionCode
                     join V23 in context.V23BillItems on V22.BillNo equals V23.BillNo
                     where V22.BillNo == BillNo
                     && V22.BpoType == "R" && (V22.AmountReceived == 0 || V22.AmountReceived == null)
                     select new CRISModel
                     {
                         bill_no = V22.BillNo,
                         invoicedate = V22.BillDt,
                         bpo_cd = V22.BpoCd,
                         bpo_type = V22.BpoType,
                         bpo_rly = V22.BpoRly,
                         bpo_name = V22.BpoName,
                         bpo_orgn = V22.BpoOrgn,
                         bpo_add = V22.BpoAdd,
                         bpo_city = V22.BpoCity,
                         case_no = V22.CaseNo,
                         region_code = V22.RegionCode,
                         po_no = V22.PoNo,
                         po_dt = V22.PoDt,
                         vend_cd = Convert.ToString(V22.VendCd),
                         vend_name = V22.VendName,
                         vend_add1 = V22.VendAdd1,
                         vend_add2 = V22.VendAdd2,
                         vendor_city = V22.VendorCity,
                         consignee_cd = Convert.ToString(V22.ConsigneeCd),
                         consignee = V22.Consignee,
                         consignee_add1 = V22.ConsigneeAdd1,
                         consignee_add2 = V22.ConsigneeAdd2,
                         consignee_city = V22.ConsigneeCity,
                         ie_cd = Convert.ToString(V22.IeCd),
                         ie_co_cd = Convert.ToString(V22.IeCoCd),
                         ic_no = V22.IcNo,
                         ic_dt = V22.IcDt,
                         bk_no = V22.BkNo,
                         set_no = V22.SetNo,
                         call_instalment_no = V22.CallInstalmentNo,
                         material_value = Convert.ToString(V22.MaterialValue),
                         visits = Convert.ToString(V22.Visits),
                         gsttaxableamt = V22.InspFee,
                         cgstamt = V22.Cgst,
                         sgstamt = V22.Sgst,
                         igstamt = V22.Igst,
                         amount = V22.BillAmount,
                         invoiceno = V22.InvoiceNo,
                         rlygstin = V22.RecipientGstinNo,
                         au = V22.Au,
                         partygstin = T01.GstinNo,
                         partycode = T01.RlyPartyCd,
                         partyname = T01.PartyName,
                         bank_acc_no = T01.BankAccNo,
                         ifsc_code = T01.IfscCode,
                         bank_name = T01.BankName,
                         bill_resent_count = V22.BillResentCount == false ? "1" : "0",
                         irfc_funded = V22.IrfcFunded,

                         item_srno = Convert.ToString(V23.ItemSrno),
                         itemdesc = V23.ItemDesc,
                         qty = V23.Qty,
                         rate = V23.Rate,
                         uom_factor = V23.UomFactor,
                         basic_value = V23.BasicValue,
                         value = V23.Value,
                         INVOICE_PDF = "http://49.50.102.182/signed/" + V22.BillNo + ".pdf",
                         IC_PDF = "http://rites.ritesinsp.com/RBS/BILL_IC/" + V22.CaseNo + "-" + V22.BkNo + "-" + V22.SetNo + ".PDF",
                         PO_PDF = "http://rites.ritesinsp.com/RBS/CASE_NO/" + V22.CaseNo + ".PDF",
                         BILLDESC = "RITES INSPECTION BILL",
                         PARTYSTATE = T01.GstinNo.Substring(0, 2),
                         REVERSECHARGE = "N",
                         ISGSTREGISTERED = "Y",
                         GSTTDSDEDUCTION = "N",
                         COMPOSITETAXABLE = "N",
                         ITEMCATEGORY = "R",
                         HSNSAC = "S",
                         HSNSACCODE = 998346,
                         ITCELIGIBLE = null,
                         UNITCODE = V23.UomSDesc.Substring(0, 3),
                         SGSTRATE = V22.Sgst > 0 ? 9 : 0,
                         CGSTRATE = V22.Cgst > 0 ? 9 : 0,
                         UGSTRATE = null,
                         UGSTAMT = null,
                         IGSTRATE = V22.Igst > 0 ? 18 : 0,
                         STATESUPPLY = V22.RecipientGstinNo.Substring(0, 2),
                         BILLCOUNT = Convert.ToString(V22.BillResentCount),
                         IRFC_FUNDED1 = V22.IrfcFunded == null ? "" : V22.IrfcFunded,
                         INVOICE_SUPP_DOCS = "http://rites.ritesinsp.com/RBS/INVOICE_SUPP_DOCS/" + V22.BillNo + ".PDF",
                     }).FirstOrDefault();
            return model;
        }

        public List<CRISBillListing> FindListBillNo(DateTime frmdt, DateTime todt)
        {
            List<CRISBillListing> lstStatus = new();

            lstStatus = (from V22 in context.V22Bills
                         join T01 in context.T01Regions on V22.RegionCode equals T01.RegionCode
                         join V23 in context.V23BillItems on V22.BillNo equals V23.BillNo
                         where (V22.BillDt >= frmdt && V22.BillDt <= todt)
                         && V22.BpoType == "R" && (V22.AmountReceived == 0 || V22.AmountReceived == null) && (V22.CnoteAmount == 0 || V22.CnoteAmount == null)
                         select new CRISBillListing
                         {
                             bill_no = V22.BillNo,
                             region_code = V22.RegionCode,
                         }).ToList();
            return lstStatus;
        }

        public List<CRISGetBillListing> Findgetbill(DateTime frmdt, DateTime todt, string billno)
        {

            List<CRISGetBillListing> lst = new();

            OracleParameter[] par = new OracleParameter[4];
            par[0] = new OracleParameter("pStartDate", OracleDbType.Date, Convert.ToDateTime(frmdt), ParameterDirection.Input);
            par[1] = new OracleParameter("pEndDate", OracleDbType.Date, Convert.ToDateTime(todt), ParameterDirection.Input);
            par[2] = new OracleParameter("pBillNo", OracleDbType.Varchar2, billno, ParameterDirection.Input);
            par[3] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GET_V22_BILL_DETAILS", par, 1);
            //var ds = DataAccessDB.GetDataSet("GET_V22_BILL_DETAILS", par);
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                lst = dt.AsEnumerable().Select(row => new CRISGetBillListing
                {
                    bill_no = Convert.ToString(row["bill_no"]),
                    invoicedate = Convert.ToDateTime(row["invoicedate"]),
                    bpo_cd = Convert.ToString(row["bpo_cd"]) ?? "",
                    bpo_type = Convert.ToString(row["bpo_type"]) ?? "",
                    bpo_rly = Convert.ToString(row["bpo_rly"]) ?? "",
                    bpo_name = Convert.ToString(row["bpo_name"]) ?? "",
                    bpo_orgn = Convert.ToString(row["bpo_orgn"]) ?? "",
                    bpo_add = Convert.ToString(row["bpo_add"]) ?? "",
                    bpo_city = Convert.ToString(row["bpo_city"]) ?? "",
                    case_no = Convert.ToString(row["case_no"]),
                    region_code = Convert.ToString(row["region_code"]) ?? "",
                    po_no = Convert.ToString(row["po_no"]) ?? "",
                    po_dt = Convert.ToDateTime(row["po_dt"]),
                    vend_cd = Convert.ToString(row["vend_cd"]) ?? "",
                    vend_name = Convert.ToString(row["vend_name"]) ?? "",
                    vend_add1 = Convert.ToString(row["vend_add1"]) ?? "",
                    vend_add2 = Convert.ToString(row["vend_add2"]) ?? "",
                    vendor_city = Convert.ToString(row["vendor_city"]) ?? "",
                    consignee_cd = Convert.ToString(row["consignee_cd"]) ?? "",
                    consignee = Convert.ToString(row["consignee"]) ?? "",
                    consignee_add1 = Convert.ToString(row["consignee_add1"]) ?? "",
                    consignee_add2 = Convert.ToString(row["consignee_add2"]) ?? "",
                    consignee_city = Convert.ToString(row["consignee_city"]) ?? "",
                    ie_cd = Convert.ToString(row["ie_cd"]) ?? "",
                    ie_co_cd = Convert.ToString(row["ie_co_cd"]) ?? "",
                    ic_no = Convert.ToString(row["ic_no"]) ?? "",
                    ic_dt = Convert.ToDateTime(row["ic_dt"]),
                    bk_no = Convert.ToString(row["bk_no"]) ?? "",
                    set_no = Convert.ToString(row["set_no"]) ?? "",
                    call_instalment_no = Convert.ToString(row["call_instalment_no"]) ?? "",
                    material_value = Convert.ToString(row["material_value"]) ?? "",
                    visits = Convert.ToString(row["visits"]) ?? "",
                    gsttaxableamt = Convert.ToDecimal(row["gsttaxableamt"]),
                    cgstamt = Convert.ToDecimal(row["cgstamt"]),
                    sgstamt = Convert.ToDecimal(row["sgstamt"]),
                    igstamt = Convert.ToDecimal(row["igstamt"]),
                    amount = Convert.ToDecimal(row["amount"]),
                    invoiceno = Convert.ToString(row["invoiceno"]) ?? "",
                    rlygstin = Convert.ToString(row["rlygstin"]) ?? "",
                    au = Convert.ToString(row["au"]) ?? "",
                    partygstin = Convert.ToString(row["partygstin"]) ?? "",
                    partycode = Convert.ToString(row["partycode"]) ?? "",
                    partyname = Convert.ToString(row["partyname"]) ?? "",
                    bank_acc_no = Convert.ToString(row["bank_acc_no"]) ?? "",
                    ifsc_code = Convert.ToString(row["ifsc_code"]) ?? "",
                    bank_name = Convert.ToString(row["bank_name"]) ?? "",
                    bill_resent_count = Convert.ToBoolean(row["bill_resent_count"]) == false ? "1" : "0",
                    irfc_funded = Convert.ToString(row["irfc_funded"]) ?? "",

                    item_srno = Convert.ToString(row["item_srno"]) ?? "",
                    itemdesc = Convert.ToString(row["itemdesc"]) ?? "",
                    qty = Convert.ToDecimal(row["qty"]),
                    rate = Convert.ToDecimal(row["rate"]),
                    uom_factor = Convert.ToString(row["uom_factor"]),
                    basic_value = Convert.ToDecimal(row["basic_value"]),
                    value = Convert.ToDecimal(row["value"]),
                    INVOICE_PDF = "http://49.50.102.182/signed/" + Convert.ToString(row["bill_no"]) + ".pdf",
                    IC_PDF = "http://rites.ritesinsp.com/RBS/BILL_IC/" + Convert.ToString(row["case_no"]) + "-" + Convert.ToString(row["bk_no"]) + "-" + Convert.ToString(row["set_no"]) + ".PDF",
                    PO_PDF = "http://rites.ritesinsp.com/RBS/CASE_NO/" + Convert.ToString(row["case_no"]) + ".PDF",
                    BILLDESC = "RITES INSPECTION BILL",
                    PARTYSTATE = Convert.ToString(row["partygstin"]).Substring(0, 2) ?? "",
                    REVERSECHARGE = "N",
                    ISGSTREGISTERED = "Y",
                    GSTTDSDEDUCTION = "N",
                    COMPOSITETAXABLE = "N",
                    ITEMCATEGORY = "R",
                    HSNSAC = "S",
                    HSNSACCODE = 998346,
                    ITCELIGIBLE = null,
                    UNITCODE = Convert.ToString(row["UNITCODE"]).Length >= 3 ? Convert.ToString(row["UNITCODE"]).Substring(0, 3) : Convert.ToString(row["UNITCODE"]),
                    SGSTRATE = Convert.ToInt32(row["sgstrate"]) > 0 ? 9 : 0,
                    CGSTRATE = Convert.ToInt32(row["cgstrate"]) > 0 ? 9 : 0,
                    UGSTRATE = 0,
                    UGSTAMT = 0,
                    IGSTRATE = Convert.ToInt32(row["igstrate"]) > 0 ? 18 : 0,
                    STATESUPPLY = Convert.ToString(row["rlygstin"]) == "" ? "" : Convert.ToString(row["rlygstin"]).Substring(0, 2),
                    BILLCOUNT = Convert.ToString(row["bill_resent_count"]) ?? "0",
                    IRFC_FUNDED1 = Convert.ToString(row["irfc_funded"]) == "" ? "" : Convert.ToString(row["irfc_funded"]),
                    INVOICE_SUPP_DOCS = "http://rites.ritesinsp.com/RBS/INVOICE_SUPP_DOCS/" + Convert.ToString(row["bill_no"]) + ".PDF",
                }).ToList();
            }
            return lst;

        }
    }
}
