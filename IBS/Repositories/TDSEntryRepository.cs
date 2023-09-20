using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using System.Drawing;
using System.Globalization;

namespace IBS.Repositories
{
    public class TDSEntryRepository : ITDSEntryRepository
    {
        private readonly ModelContext context;
        public TDSEntryRepository(ModelContext context)
        {
            this.context = context;
        }

        public TDSEntryModel GetTextboxValues(string txtBNO, string Region)
        {


            var query = from bill in context.T22Bills
                        where bill.BillNo == txtBNO && bill.CaseNo.Substring(0, 1) == Region
                        select new TDSEntryModel
                        {
                            BILL_NO = bill.BillNo,
                            CASE_NO = bill.CaseNo,
                            Bill_Amount = bill.BillAmount ?? 0,
                            TDS = bill.Tds ?? 0,
                            TDSCGST = bill.TdsCgst ?? 0,
                            TDSSGST = bill.TdsSgst ?? 0,
                            TDSIGST = bill.TdsIgst ?? 0,
                            Retention_Money = bill.RetentionMoney ?? 0,
                            WrtOffAmount = bill.WriteOffAmt ?? 0,
                            TDSposting_DT = Convert.ToString( bill.TdsDt)
                        };

            var result = query.FirstOrDefault();


            return result;

        }

        public string TDSdetailSave(TDSEntryModel model)
        {
            double wtotOld = Convert.ToDouble(model.TDS + model.Retention_Money + model.WrtOffAmount + model.TDSCGST + model.TDSIGST + model.TDSCGST);
            DateTime parsedDate;
            DateTime.TryParseExact(model.TDSposting_DT, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate);
            var GetValue = context.T22Bills.Find(model.BILL_NO);

            if (model.BILL_NO != null || model.BILL_NO != "")
            {
                T22Bill bill = new T22Bill();
                GetValue.BillNo = model.BILL_NO;
                GetValue.Tds = model.TDS;
                GetValue.Cgst = model.TDSCGST;
                GetValue.Sgst = model.TDSSGST;
                GetValue.Igst = model.TDSIGST;
                GetValue.RetentionMoney = model.Retention_Money;
                GetValue.WriteOffAmt = model.WrtOffAmount;
                GetValue.BillAmtCleared = (GetValue.BillAmtCleared ?? 0 - Convert.ToDecimal(wtotOld) + Convert.ToDecimal(wtotOld));
                GetValue.TdsDt = parsedDate;
                context.SaveChanges();


            }

           
            return model.BILL_NO;
        }
    }
}
