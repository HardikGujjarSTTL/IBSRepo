using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using static IBS.Helper.Enums;
using System.Data;

namespace IBS.Repositories
{
    public class DEOCRISPurchesOrderWCaseNoRepository : IDEOCRISPurchesOrderWCaseNoRepository
    {
        private readonly ModelContext context;

        public DEOCRISPurchesOrderWCaseNoRepository(ModelContext context)
        {
            this.context = context;
        }

        public DEO_CRIS_PurchesOrderModel FindByID(string ImmsPokey, string ImmsRlyCd)
        {
            DEO_CRIS_PurchesOrderModel model = new();
            List<DEO_CRIS_PurchesOrderModel> model1 = new();

            OracleParameter[] par = new OracleParameter[3];
            par[0] = new OracleParameter("p_POKey", OracleDbType.Varchar2, ImmsPokey, ParameterDirection.Input);
            par[1] = new OracleParameter("p_RlyCode", OracleDbType.Varchar2, ImmsRlyCd, ParameterDirection.Input);
            par[2] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("SP_Get_DEO_CRIS_PurchesOrderData", par, 1);
            DataTable dt = ds.Tables[0];

            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                model1 = JsonConvert.DeserializeObject< List<DEO_CRIS_PurchesOrderModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                if (model1 != null)
                {
                    model = model1.FirstOrDefault();
                }
            }
            
            return model;
        }

        public DTResult<DEO_CRIS_PurchesOrderListModel> GetDataList(DTParameters dtParameters, string Region)
        {

            DTResult<DEO_CRIS_PurchesOrderListModel> dTResult = new() { draw = 0 };
            IQueryable<DEO_CRIS_PurchesOrderListModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "CASE_NO";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "CASE_NO";
                orderAscendingDirection = true;
            }

            OracleParameter[] par = new OracleParameter[2];
            par[0] = new OracleParameter("p_Region", OracleDbType.Varchar2, Region == "" ? DBNull.Value : Region.ToString(), ParameterDirection.Input);
            par[1] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("Get_SP_DEO_CRIS_PurchesOrderList", par, 1);
            DataTable dt = ds.Tables[0];


            List<DEO_CRIS_PurchesOrderListModel> list = new();
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                list = JsonConvert.DeserializeObject<List<DEO_CRIS_PurchesOrderListModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }

            query = list.AsQueryable();
            dTResult.recordsTotal = query.Count();
            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.CASE_NO).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.IMMS_POKEY).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.PO_NO).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public int DetailsUpdate(DEOCRISPurchesOrderMAModel model)
        {
            int Id = 0;
            var GetValuePO = context.MmpPomaDtls.Find(model.Rly, model.Makey, model.Slno);

            #region save
            if (GetValuePO == null)
            {
                MmpPomaDtl obj = new MmpPomaDtl();
                obj.MaStatus = model.MaStatus;
                obj.ApprovedBy = model.ApprovedBy;
                obj.ApprovedDatetime = DateTime.Now;
                context.MmpPomaDtls.Add(obj);
                context.SaveChanges();
                Id = Convert.ToInt32(obj.Makey);
            }
            else
            {
                GetValuePO.MaStatus = model.MaStatus;
                GetValuePO.ApprovedBy = model.ApprovedBy;
                GetValuePO.ApprovedDatetime = DateTime.Now;
                context.SaveChanges();
                Id = Convert.ToInt32(GetValuePO.Makey);
            }
            #endregion
            return Id;
        }
    }
}
