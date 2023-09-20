using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;

namespace IBS.Repositories
{
    public class BillFinalisationFormRepository : IBillFinalisationFormRepository
    {
        private readonly ModelContext context;

        public BillFinalisationFormRepository(ModelContext context)
        {
            this.context = context;
        }

        public DTResult<BillFinalisationFormModel> GetBillFinalisationList(DTParameters dtParameters)
        {
            DTResult<BillFinalisationFormModel> dTResult = new() { draw = 0 };
            IQueryable<BillFinalisationFormModel>? query = null;

            string Region = !string.IsNullOrEmpty(dtParameters.AdditionalValues["Region"]) ? Convert.ToString(dtParameters.AdditionalValues["Region"]) : "";
            DateTime? FromDate = !string.IsNullOrEmpty(dtParameters.AdditionalValues["FromDate"]) ? Convert.ToDateTime(dtParameters.AdditionalValues["FromDate"]) : null;
            DateTime? ToDate = !string.IsNullOrEmpty(dtParameters.AdditionalValues["ToDate"]) ? Convert.ToDateTime(dtParameters.AdditionalValues["ToDate"]) : null;
            string Sector = !string.IsNullOrEmpty(dtParameters.AdditionalValues["Sector"]) ? Convert.ToString(dtParameters.AdditionalValues["Sector"]) : "";

            query = from v22 in context.V22Bills
                    where v22.BillDt >= new DateTime(2020, 11, 1) && v22.SentToSap == null && v22.BillFinalised == null && v22.RegionCode == Region
                    && v22.BillDt >= FromDate && v22.BillDt <= ToDate
                    && (Sector != "A" ? v22.BpoType == Sector : true)
                    orderby v22.BillNo
                    select new BillFinalisationFormModel
                    {
                        BillNo = v22.BillNo,
                        BillDt = v22.BillDt,
                        InspFee = v22.InspFee,
                        Cgst = v22.Cgst,
                        Sgst = v22.Sgst,
                        Igst = v22.Igst,
                        BillAmount = v22.BillAmount,
                        InvoiceNo = v22.InvoiceNo,
                        BPO = v22.BpoName + "/" + v22.BpoRly + "/" + v22.BpoCity,
                        Consignee = v22.Consignee + "/" + v22.ConsigneeCity,
                        RecipientGstinNo = v22.RecipientGstinNo
                    };

            dTResult.recordsTotal = query.Count();

            dTResult.recordsFiltered = query.Count();

            if (dtParameters.Length == -1) dtParameters.Length = query.Count();

            dTResult.data = query.Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public void UpdateBillFinalisationStatus(string[] BillNos)
        {
            if (BillNos.Length > 0)
            {
                foreach (var id in BillNos)
                {
                    T22Bill bill = context.T22Bills.Find(id);
                    if (bill != null)
                    {
                        bill.BillDt = DateTime.Now.Date;
                        bill.BillFinalised = "Y";
                        context.SaveChanges();
                    }
                }
            }
        }
    }

}