using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;

namespace IBS.Repositories
{
    public class TDSEntryRepository : ITDSEntryRepository
    {
        private readonly ModelContext context;

        public TDSEntryRepository(ModelContext context)
        {
            this.context = context;
        }

        public TDSEntryModel GetBillDetails(string BillNo, string Region)
        {
            TDSEntryModel model = new();
            T22Bill t22Bill = context.T22Bills.Where(x => x.BillNo == BillNo && x.CaseNo.Substring(0, 1) == Region).FirstOrDefault();

            if (t22Bill == null)
                return null;
            else
            {
                model.BILL_NO = t22Bill.BillNo;
                model.CASE_NO = t22Bill.CaseNo;
                model.Bill_Amount = t22Bill.BillAmount ?? 0;
                model.TDS = t22Bill.Tds ?? 0;
                model.TDSCGST = t22Bill.TdsCgst ?? 0;
                model.TDSSGST = t22Bill.TdsSgst ?? 0;
                model.TDSIGST = t22Bill.TdsIgst ?? 0;
                model.Retention_Money = t22Bill.RetentionMoney ?? 0;
                model.WrtOffAmount = t22Bill.WriteOffAmt ?? 0;
                model.TdsDt = t22Bill.TdsDt;

                return model;
            }
        }

        public bool SaveDetails(TDSEntryModel model)
        {
            T22Bill t22Bill = context.T22Bills.Find(model.BILL_NO);

            if (t22Bill != null)
            {
                decimal wtotOld = t22Bill.Tds ?? 0 + t22Bill.RetentionMoney ?? 0 + t22Bill.WriteOffAmt ?? 0 + t22Bill.TdsCgst ?? 0 + t22Bill.TdsSgst ?? 0 + t22Bill.TdsIgst ?? 0;
                decimal wtotNew = model.TDS + model.Retention_Money + model.WrtOffAmount + model.TDSCGST + model.TDSSGST + model.TDSIGST;
                decimal BillAmtCleared = (t22Bill.BillAmtCleared ?? 0 - wtotOld) + wtotNew;

                t22Bill.Tds = model.TDS;
                t22Bill.TdsCgst = model.TDSCGST;
                t22Bill.TdsSgst = model.TDSSGST;
                t22Bill.TdsIgst = model.TDSIGST;
                t22Bill.RetentionMoney = model.Retention_Money;
                t22Bill.WriteOffAmt = model.WrtOffAmount;
                t22Bill.BillAmtCleared = BillAmtCleared;
                t22Bill.TdsDt = model.TdsDt;

                context.SaveChanges();

                TdsHistory tdsHistory = new()
                {
                    BillNo = model.BILL_NO,
                    CaseNo = model.CASE_NO,
                    Tds = model.TDS,
                    TdsCgst = model.TDSCGST,
                    TdsSgst = model.TDSSGST,
                    TdsIgst = model.TDSIGST,
                    RetentionMoney = model.Retention_Money,
                    WriteOffAmt = model.WrtOffAmount,
                    BillAmtCleared = BillAmtCleared,
                    TdsDt = model.TdsDt,
                    Createdby = model.Createdby,
                    Createddate = DateTime.Now,
                };

                context.TdsHistories.Add(tdsHistory);
                context.SaveChanges();
            }

            return true;
        }

        public DTResult<TDSEntryModel> GetTDSHistroyList(DTParameters dtParameters)
        {
            DTResult<TDSEntryModel> dTResult = new() { draw = 0 };
            IQueryable<TDSEntryModel>? query = null;

            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null && dtParameters.Order.Length > 0)
            {
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                if (string.IsNullOrEmpty(orderCriteria)) orderCriteria = "ID";
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                orderCriteria = "ID";
                orderAscendingDirection = true;
            }

            string BillNo = !string.IsNullOrEmpty(dtParameters.AdditionalValues["BillNo"]) ? Convert.ToString(dtParameters.AdditionalValues["BillNo"]) : "";
            string Region = !string.IsNullOrEmpty(dtParameters.AdditionalValues["Region"]) ? Convert.ToString(dtParameters.AdditionalValues["Region"]) : "";

            query = from t in context.TdsHistories
                    where t.BillNo == BillNo && t.CaseNo.Substring(0, 1) == Region
                    select new TDSEntryModel
                    {
                        ID = t.Id,
                        TDS = t.Tds ?? 0,
                        TDSCGST = t.TdsCgst ?? 0,
                        TDSSGST = t.TdsSgst ?? 0,
                        TDSIGST = t.TdsIgst ?? 0,
                        Retention_Money = t.RetentionMoney ?? 0,
                        WrtOffAmount = t.WriteOffAmt ?? 0,
                        TdsDt = t.TdsDt,
                        Bill_Amount = t.BillAmtCleared ?? 0,
                    };

            dTResult.recordsTotal = query.Count();

            dTResult.recordsFiltered = query.Count();

            if (dtParameters.Length == -1) dtParameters.Length = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }
    }
}
