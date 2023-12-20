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
                     where V22.BillNo == BillNo
                     select new CRISModel
                     {
                         BillNo = V22.BillNo,
                         BillDt = V22.BillDt,
                         BpoCd = V22.BpoCd,
                         BpoType = V22.BpoType,
                         BpoRly = V22.BpoRly,
                         BpoName = V22.BpoName,
                         BpoOrgn = V22.BpoOrgn,
                         BpoAdd = V22.BpoAdd,
                         BpoCity = V22.BpoCity,
                         CaseNo = V22.CaseNo,
                         RegionCode = V22.RegionCode,
                         PoNo = V22.PoNo,
                         PoDt = V22.PoDt,
                         VendCd = V22.VendCd,
                         VendName = V22.VendName,
                         VendAdd1 = V22.VendAdd1,
                         VendAdd2 = V22.VendAdd2,
                         VendorCity = V22.VendorCity,
                         ConsigneeCd = V22.ConsigneeCd,
                         Consignee = V22.Consignee,
                         ConsigneeAdd1 = V22.ConsigneeAdd1,
                         ConsigneeAdd2 = V22.ConsigneeAdd2,
                         ConsigneeCity = V22.ConsigneeCity,
                         IeCd = V22.IeCd,
                         IeCoCd = V22.IeCoCd,
                         IcNo = V22.IcNo,
                         IcDt = V22.IcDt,
                         BkNo = V22.BkNo,
                         SetNo = V22.SetNo,
                         CallInstalmentNo = V22.CallInstalmentNo,
                         MaterialValue = V22.MaterialValue,
                         Visits = V22.Visits,
                         InspFee = V22.InspFee,
                         Cgst = V22.Cgst,
                         Sgst = V22.Sgst,
                         Igst = V22.Igst,
                         BillAmount = V22.BillAmount,
                         InvoiceNo = V22.InvoiceNo,
                         RecipientGstinNo = V22.RecipientGstinNo,
                         Au = V22.Au,
                         GstinNo = T01.GstinNo,
                         RlyPartyCd = T01.RlyPartyCd,
                         PartyName = T01.PartyName,
                         BankAccNo = T01.BankAccNo,
                         IfscCode = T01.IfscCode,
                         BankName = T01.BankName,
                         BillResentCount = V22.BillResentCount,
                         IrfcFunded = V22.IrfcFunded,
                     }).FirstOrDefault();
            return model;
        }
    }
}
