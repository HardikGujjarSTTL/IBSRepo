using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Interfaces.Inspection_Billing;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using static IBS.Helper.Enums;

namespace IBS.Repositories.Inspection_Billing
{
    public class ICCancellationRepository : IICCancellationRepository
    {
        private readonly ModelContext context;

        public ICCancellationRepository(ModelContext context)
        {
            this.context = context;
        }
        public ICCancellationModel FindByID(string REGION, string BK_NO, string SET_NO)
        {
            ICCancellationModel model = new();
            T16IcCancel t16IcCancel = context.T16IcCancels.Where(x => x.Region == REGION && x.BkNo == BK_NO && x.SetNo == SET_NO).FirstOrDefault();
            if (t16IcCancel == null)
                return model;
            else
            {
                model.BkNo = t16IcCancel.BkNo;
                model.SetNo = t16IcCancel.SetNo;
                model.IssueToIecd = t16IcCancel.IssueToIecd;
                model.IcStatus = t16IcCancel.IcStatus;
                model.StatusDt = t16IcCancel.StatusDt;
                model.Region = t16IcCancel.Region;
                model.Remarks = t16IcCancel.Remarks;
                return model;
            }
        }

        public DTResult<ICCancellationListModel> GetCancellationList(DTParameters dtParameters,string Region)
        {

            DTResult<ICCancellationListModel> dTResult = new() { draw = 0 };
            IQueryable<ICCancellationListModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "BK_NO";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                orderCriteria = "BK_NO";
                orderAscendingDirection = true;
            }
            string p_BKNo = "", p_SetNo = "";
            int p_ISSUE_TO_IECD = 0;
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["BKNo"]))
            {
                p_BKNo = Convert.ToString(dtParameters.AdditionalValues["BKNo"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["SetNo"]))
            {
                p_SetNo = Convert.ToString(dtParameters.AdditionalValues["SetNo"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["ISSUE_TO_IECD"]))
            {
                p_ISSUE_TO_IECD = Convert.ToInt32(dtParameters.AdditionalValues["ISSUE_TO_IECD"]);
            }
            OracleParameter[] par = new OracleParameter[5];
            par[0] = new OracleParameter("p_BKNo", OracleDbType.Varchar2, p_BKNo.ToString() == "" ? DBNull.Value : p_BKNo.ToString(), ParameterDirection.Input);
            par[1] = new OracleParameter("p_SetNo", OracleDbType.Varchar2, p_SetNo.ToString() == "" ? DBNull.Value : p_SetNo.ToString(), ParameterDirection.Input);
            par[2] = new OracleParameter("p_ISSUE_TO_IECD", OracleDbType.Int32, p_ISSUE_TO_IECD, ParameterDirection.Input);
            par[3] = new OracleParameter("p_Region", OracleDbType.Varchar2, Region.ToString() == "" ? DBNull.Value : Region.ToString(), ParameterDirection.Input);
            par[4] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);
            var ds = DataAccessDB.GetDataSet("SP_Get_ICCancelData", par, 1);
            DataTable dt = ds.Tables[0];
            List<ICCancellationListModel> list = new();
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                list = JsonConvert.DeserializeObject<List<ICCancellationListModel>>(serializeddt, new JsonSerializerSettings { });
            }
            query = list.AsQueryable();

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.BK_NO).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.SET_NO).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }
        public bool Remove(string REGION, string BK_NO, string SET_NO)
        {
            T16IcCancel t16IcCancel = context.T16IcCancels.Where(x => x.Region == REGION && x.BkNo == BK_NO && x.SetNo == SET_NO).FirstOrDefault();
            if (t16IcCancel == null) { return false; }
            context.T16IcCancels.Remove(t16IcCancel);
            context.SaveChanges();
            return true;
        }

        public string ICCancellationSave(ICCancellationModel model)
        {
            string BkNo = "";
            T16IcCancel t16IcCancel = context.T16IcCancels.Where(x => x.Region == model.Region && x.BkNo == model.BkNo && x.SetNo == model.SetNo).FirstOrDefault();
            if (t16IcCancel == null)
            {
                T16IcCancel obj = new T16IcCancel();
                obj.BkNo = model.BkNo;
                obj.SetNo = model.SetNo;
                obj.IssueToIecd = model.IssueToIecd;
                obj.IcStatus = model.IcStatus;
                obj.StatusDt = model.StatusDt;
                obj.Region = model.Region;
                obj.Remarks = model.Remarks;
                context.T16IcCancels.Add(obj);
                context.SaveChanges();
                BkNo = obj.BkNo;
            }
            else
            {
                t16IcCancel.IcStatus = model.IcStatus;
                t16IcCancel.StatusDt = model.StatusDt;
                t16IcCancel.Remarks = model.Remarks;
                context.SaveChanges();
                BkNo = t16IcCancel.BkNo;
            }
            return BkNo;
        }

    }

}
