using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces.Transaction;
using IBS.Models;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace IBS.Repositories.Transaction
{
    public class CentralRegionBillingInformationRepository : ICentralRegionBillingInformationRepository
    {
        private readonly ModelContext context;

        public CentralRegionBillingInformationRepository(ModelContext context)
        {
            this.context = context;
        }
        public CentralRegionBillingInformationModel FindByID(string BILL_NO)
        {
            CentralRegionBillingInformationModel model = new();
            model = (from t in context.T36Bills
                     where t.BillNo == BILL_NO
                     select new CentralRegionBillingInformationModel
                     {
                         BillNo = t.BillNo,
                         BillDt = t.BillDt,
                         Region = t.Region,
                         RlyNonrly = t.RlyNonrly,
                         PoNo = t.PoNo,
                         PoDt = t.PoDt,
                         Purchaser = t.Purchaser,
                         Bpo = t.Bpo,
                         Consignee = t.Consignee,
                         RailDesc = t.RailDesc,
                         IcNo = t.IcNo,
                         IcDt = t.IcDt,
                         BasePrice = t.BasePrice,
                         Qty = t.Qty,
                         TotBaseValue = t.TotBaseValue,
                         Laying = t.Laying,
                         ExcisePer = t.ExcisePer,
                         Excise = t.Excise,
                         SalesTaxPer = t.SalesTaxPer,
                         SalesTax = t.SalesTax,
                         MaterialValue = t.MaterialValue,
                         FeeRate = t.FeeRate,
                         InspFee = t.InspFee,
                         ServTaxRate = t.ServTaxRate,
                         ServiceTax = t.ServiceTax,
                         EduCess = t.EduCess,
                         SheCess = t.SheCess,
                         BillAmount = t.BillAmount,
                         Remarks = t.Remarks,
                         UserId = t.UserId,
                         Datetime = t.Datetime
                     }).FirstOrDefault();
            if (model == null)
                throw new Exception("Billing Information Record Not found");
            else
            {
                return model;
            }
        }

        public DTResult<CentralRegionBillingInformationListModel> GetBillingInformationList(DTParameters dtParameters, string Region)
        {

            DTResult<CentralRegionBillingInformationListModel> dTResult = new() { draw = 0 };
            IQueryable<CentralRegionBillingInformationListModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "BILL_NO";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                orderCriteria = "BILL_NO";
                orderAscendingDirection = true;
            }

            string BillNo = "", BillFromDate = "", BillToDate = "";
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["BillNo"]))
            {
                BillNo = Convert.ToString(dtParameters.AdditionalValues["BillNo"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["BillFromDate"]))
            {
                BillFromDate = Convert.ToString(dtParameters.AdditionalValues["BillFromDate"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["BillToDate"]))
            {
                BillToDate = Convert.ToString(dtParameters.AdditionalValues["BillToDate"]);
            }
            OracleParameter[] par = new OracleParameter[5];
            par[0] = new OracleParameter("p_Region", OracleDbType.Varchar2, Region.ToString() == "" ? DBNull.Value : Region.ToString(), ParameterDirection.Input);
            par[1] = new OracleParameter("p_BillNo", OracleDbType.Varchar2, BillNo.ToString() == "" ? DBNull.Value : BillNo.ToString(), ParameterDirection.Input);
            par[2] = new OracleParameter("p_BillFromDate", OracleDbType.Varchar2, BillFromDate.ToString() == "" ? DBNull.Value : BillFromDate.ToString(), ParameterDirection.Input);
            par[3] = new OracleParameter("p_BillToDate", OracleDbType.Varchar2, BillToDate.ToString() == "" ? DBNull.Value : BillToDate.ToString(), ParameterDirection.Input);
            par[4] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("SP_GetBillData", par, 1);

            DataTable dt = ds.Tables[0];


            List<CentralRegionBillingInformationListModel> list = new();
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                list = JsonConvert.DeserializeObject<List<CentralRegionBillingInformationListModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }

            query = list.AsQueryable();
            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.BILL_NO).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.IC_NO).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();
            if (dtParameters.Length == -1) dtParameters.Length = query.Count();
            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }
        public bool Remove(string BILL_NO, int UserId)
        {
            var t36Bill = context.T36Bills.Find(BILL_NO);
            if (t36Bill == null) { return false; }
            t36Bill.Isdeleted = Convert.ToByte(true);
            t36Bill.Updatedby = UserId;
            t36Bill.Updateddate = DateTime.Now;
            context.SaveChanges();
            return true;
        }
        public bool AlreadyExist(string BILL_NO)
        {
            bool retVal = false;
            var t36Bills = (from r in context.T36Bills where r.BillNo == BILL_NO select r).FirstOrDefault();
            if (t36Bills != null)
            {
                retVal = true;
            }
            return retVal;
        }
        public string BillingInformationInsertUpdate(CentralRegionBillingInformationModel model)
        {
            string BillNo = "";
            var t36Bills = (from r in context.T36Bills where r.BillNo == model.BillNo select r).FirstOrDefault();
            if (t36Bills == null)
            {

                T36Bill newBill = new T36Bill();
                newBill.BillNo = model.BillNo;
                newBill.BillDt = model.BillDt;
                newBill.Region = model.Region;
                newBill.RlyNonrly = model.RlyNonrly;
                newBill.PoNo = model.PoNo;
                newBill.PoDt = model.PoDt;
                newBill.Purchaser = model.Purchaser;
                newBill.Bpo = model.Bpo;
                newBill.Consignee = model.Consignee;
                newBill.RailDesc = model.RailDesc;
                newBill.IcNo = model.IcNo;
                newBill.IcDt = model.IcDt;
                newBill.BasePrice = model.BasePrice;
                newBill.Qty = model.Qty;
                newBill.TotBaseValue = model.TotBaseValue;
                newBill.Laying = model.Laying;
                newBill.ExcisePer = model.ExcisePer;
                newBill.Excise = model.Excise;
                newBill.SalesTaxPer = model.SalesTaxPer;
                newBill.SalesTax = model.SalesTax;
                newBill.MaterialValue = model.MaterialValue;
                newBill.FeeRate = model.FeeRate;
                newBill.InspFee = model.InspFee;
                newBill.BillAmount = model.BillAmount;
                newBill.ServTaxRate = model.ServTaxRate;
                newBill.ServiceTax = model.ServiceTax;
                newBill.EduCess = model.EduCess;
                newBill.SheCess = model.SheCess;
                newBill.Remarks = model.Remarks;
                newBill.UserId = model.UserId;
                newBill.Datetime = DateTime.Now;
                newBill.Isdeleted = Convert.ToByte(false);
                newBill.Createdby = model.Createdby;
                newBill.Createddate = DateTime.Now;
                context.T36Bills.Add(newBill);
                context.SaveChanges();
                BillNo = newBill.BillNo;
            }
            else
            {
                t36Bills.BillDt = model.BillDt;
                t36Bills.RlyNonrly = model.RlyNonrly;
                t36Bills.PoNo = model.PoNo;
                t36Bills.PoDt = model.PoDt;
                t36Bills.Purchaser = model.Purchaser;
                t36Bills.Bpo = model.Bpo;
                t36Bills.Consignee = model.Consignee;
                t36Bills.RailDesc = model.RailDesc;
                t36Bills.IcNo = model.IcNo;
                t36Bills.IcDt = model.IcDt;
                t36Bills.BasePrice = model.BasePrice;
                t36Bills.Qty = model.Qty;
                t36Bills.TotBaseValue = model.TotBaseValue;
                t36Bills.Laying = model.Laying;
                t36Bills.ExcisePer = model.ExcisePer;
                t36Bills.Excise = model.Excise;
                t36Bills.SalesTaxPer = model.SalesTaxPer;
                t36Bills.SalesTax = model.SalesTax;
                t36Bills.MaterialValue = model.MaterialValue;
                t36Bills.FeeRate = model.FeeRate;
                t36Bills.InspFee = model.InspFee;
                t36Bills.BillAmount = model.BillAmount;
                t36Bills.ServTaxRate = model.ServTaxRate;
                t36Bills.ServiceTax = model.ServiceTax;
                t36Bills.EduCess = model.EduCess;
                t36Bills.SheCess = model.SheCess;
                t36Bills.Remarks = model.Remarks;
                t36Bills.UserId = model.UserId;
                t36Bills.Datetime = DateTime.Now;
                t36Bills.Updatedby = model.Updatedby;
                t36Bills.Updateddate = DateTime.Now;
                context.SaveChanges();
                BillNo = t36Bills.BillNo;
            }
            return BillNo;
        }

    }
}
