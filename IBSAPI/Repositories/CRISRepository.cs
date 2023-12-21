using IBSAPI.DataAccess;
using IBSAPI.Interfaces;
using IBSAPI.Models;
using System.ComponentModel;
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
    }
}
