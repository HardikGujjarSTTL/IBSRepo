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

        public bool DetailsUpdate(DEO_CRIS_PurchesOrderModel model)
        {
            bool objRet = false;
            var immsRitesPoHdr = (from m in context.ImmsRitesPoHdrs
                                  where m.ImmsPokey == Convert.ToInt32(model.IMMS_POKEY) && m.ImmsRlyCd == model.IMMS_RLY_CD
                                  select m).FirstOrDefault();

            #region save
            if (immsRitesPoHdr != null)
            {
                immsRitesPoHdr.RecvDate = model.RecvDt;
                immsRitesPoHdr.RegionCode = model.REGION_CODE;
                immsRitesPoHdr.PurchaserCd = Convert.ToInt32(model.PURCHASER_CD);
                immsRitesPoHdr.RlyCd = model.RLY_CD;
                immsRitesPoHdr.Remarks = model.REMARKS;
                immsRitesPoHdr.UserId = model.UserId;
                immsRitesPoHdr.PoId =Convert.ToDecimal(model.POI_CD);
                immsRitesPoHdr.VendCd = Convert.ToInt32(model.VEND_CD);
                immsRitesPoHdr.BpoCd = model.BPO_CD;
                immsRitesPoHdr.Datetime = DateTime.Now;
                context.SaveChanges();
                objRet = true;
            }
            #endregion
            return objRet;
        }

        public bool UpdateREMARKS(string REMARKS, int IMMS_POKEY, string IMMS_RLY_CD)
        {
            bool retVal = false;
            var immsRitesPoHdr = (from m in context.ImmsRitesPoHdrs
                              where m.ImmsPokey == IMMS_POKEY && m.ImmsRlyCd == IMMS_RLY_CD
                              select m).FirstOrDefault();

            #region save
            if (immsRitesPoHdr != null)
            {
                immsRitesPoHdr.Remarks = REMARKS;
                context.SaveChanges();
                retVal =true;
            }
            #endregion
            return retVal;
        }

        public string getVendorEmail(string CASE_NO)
        {
            string vendorEmail = "";
            OracleParameter[] par = new OracleParameter[2];
            par[0] = new OracleParameter("IN_CASE_NO", OracleDbType.Varchar2, CASE_NO, ParameterDirection.Input);
            par[1] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);
            var ds = DataAccessDB.GetDataSet("GET_VENDOR_INFO", par, 1);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                vendorEmail = ds.Tables[0].Rows[0]["VEND_EMAIL"].ToString();
            }
            return vendorEmail;
        }

        public string[] GenerateRealCaseNoCRIS(string REGION_CD, string IMMS_POKEY,string IMMS_RLY_CD, string USER_ID)
        {
            string[] result = new string[2];
            OracleParameter[] par = new OracleParameter[5];
            par[0] = new OracleParameter("IN_REGION_CD", OracleDbType.Char, REGION_CD, ParameterDirection.Input);
            par[1] = new OracleParameter("IN_TEMP_POKEY", OracleDbType.Char, IMMS_POKEY, ParameterDirection.Input);
            par[2] = new OracleParameter("IN_TEMP_RLY_CD", OracleDbType.Char, IMMS_RLY_CD, ParameterDirection.Input);
            par[3] = new OracleParameter("IN_TEMP_USER_ID", OracleDbType.Char, USER_ID, ParameterDirection.Input);
            par[4] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);
            var ds = DataAccessDB.GetDataSet("GENERATE_REAL_CASE_NO_CRIS_new", par, 1);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                result[0] = ds.Tables[0].Rows[0]["ERR_CD"].ToString();
                result[1] = ds.Tables[0].Rows[0]["CASE_NO"].ToString();
            }
            return result;
        }
    }
}
