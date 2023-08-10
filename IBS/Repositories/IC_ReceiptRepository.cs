using Humanizer;
using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Drawing;
using System.Globalization;

namespace IBS.Repositories
{
    public class IC_ReceiptRepository : IIC_ReceiptRepository
    {
        private readonly ModelContext context;
        private readonly IConfiguration configuration;

        public IC_ReceiptRepository(ModelContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }

        public DTResult<IC_Receipt_Grid_Model> Get_IC_Receipt([FromBody] DTParameters dtParameters, string region)
        {
            DTResult<IC_Receipt_Grid_Model> dTResult = new() { draw = 0 };
            IQueryable<IC_Receipt_Grid_Model>? query = null;

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
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
            }
            else
            {
                orderCriteria = "BK_NO";
                orderAscendingDirection = true;
            }

            string BK_NO = "", SET_NO = "", IE_CD = "", REGION = "";
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["BK_NO"]))
            {
                BK_NO = Convert.ToString(dtParameters.AdditionalValues["BK_NO"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["SET_NO"]))
            {
                SET_NO = Convert.ToString(dtParameters.AdditionalValues["SET_NO"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["IE_CD"]))
            {
                IE_CD = Convert.ToString(dtParameters.AdditionalValues["IE_CD"]);
            }
            if (!string.IsNullOrEmpty(region))
            {
                REGION = region;
            }

            BK_NO = BK_NO.ToString() == "" ? string.Empty : BK_NO.ToString();
            SET_NO = SET_NO.ToString() == "" ? string.Empty : SET_NO.ToString();
            REGION = REGION.ToString() == "" ? string.Empty : REGION.ToString();

            OracleParameter[] par = new OracleParameter[5];
            par[0] = new OracleParameter("P_BK_NO", OracleDbType.Varchar2, BK_NO, ParameterDirection.Input);
            par[1] = new OracleParameter("P_SET_NO", OracleDbType.Varchar2, SET_NO, ParameterDirection.Input);
            par[2] = new OracleParameter("P_IE_CD", OracleDbType.Varchar2, IE_CD, ParameterDirection.Input);
            par[3] = new OracleParameter("P_REGION", OracleDbType.Varchar2, REGION, ParameterDirection.Input);
            par[4] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("SP_GET_IC_RECEIPT", par, 1);
            DataTable dt = ds.Tables[0];


            List<IC_Receipt_Grid_Model> list = dt.AsEnumerable().Select(row => new IC_Receipt_Grid_Model
            {
                BK_NO = row["BK_NO"].ToString(),
                SET_NO = row["SET_NO"].ToString(),
                IE_NAME = row["IE_NAME"].ToString(),
                IC_SUBMIT_DT = row["IC_SUBMIT_DT"].ToString(),
            }).ToList();

            query = list.AsQueryable();

            dTResult.recordsTotal = ds.Tables[0].Rows.Count;
            dTResult.recordsFiltered = ds.Tables[0].Rows.Count;
            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();
            dTResult.draw = dtParameters.Draw;
            return dTResult;
        }

        public List<SelectListItem> Get_IE_Whome_Issued(string region)
        {
            var data = context.T09Ies.Where(x => x.IeStatus == null && x.IeRegion == region).Select(x => x).OrderBy(x => x.IeName).ToList();
            List<SelectListItem> IE = (from a in context.T09Ies
                                       orderby a.IeName
                                       where a.IeRegion == region && a.IeStatus == null && a.IeStatus == null
                                       select
                                  new SelectListItem
                                  {
                                      Text = Convert.ToString(a.IeName),
                                      Value = Convert.ToString(a.IeCd)
                                  }).ToList();
            return IE;
        }

        public IC_ReceiptModel Get_Selected_IC_Receipt(string BK_NO, string SET_NO, string REGION)
        {
            var data = new IC_ReceiptModel();
            OracleParameter[] par = new OracleParameter[4];
            par[0] = new OracleParameter("P_BK_NO", OracleDbType.Varchar2, BK_NO, ParameterDirection.Input);
            par[1] = new OracleParameter("P_SET_NO", OracleDbType.Varchar2, SET_NO, ParameterDirection.Input);
            par[2] = new OracleParameter("P_REGION", OracleDbType.Varchar2, REGION, ParameterDirection.Input);
            par[3] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("SP_GET_SELECTED_IC_RECEIPT", par, 1);


            if (ds.Tables[0].Rows.Count > 0)
            {
                data.REGION = Convert.ToString(ds.Tables[0].Rows[0]["REGION"]);
                data.BK_NO = Convert.ToString(ds.Tables[0].Rows[0]["BK_NO"]);
                data.SET_NO = Convert.ToString(ds.Tables[0].Rows[0]["SET_NO"]);
                data.IE_CD = Convert.ToInt32(ds.Tables[0].Rows[0]["IE_CD"]);
                data.IC_SUBMIT_DT = Convert.ToString(ds.Tables[0].Rows[0]["SUBMIT_DT"]);
                data.REMARKS = Convert.ToString(ds.Tables[0].Rows[0]["REMARKS"]);
                data.REMARKS_DT = Convert.ToString(ds.Tables[0].Rows[0]["REMARKS_DATE"]);
            }
            return data;
        }

        public int IC_Receipt_InsertUpdate(IC_ReceiptModel model)
        {
            int flag = 0;
            var _data = context.T30IcReceiveds.Find(model.BK_NO, model.SET_NO, model.REGION);
            if (_data == null)
            {
                T30IcReceived obj = new T30IcReceived();
                obj.Region = model.REGION;
                obj.BkNo = model.BK_NO;
                obj.SetNo = model.SET_NO;
                obj.IeCd = Convert.ToByte(model.IE_CD);
                obj.IcSubmitDt = Convert.ToDateTime(model.IC_SUBMIT_DT);
                obj.BillNo = model.BILL_NO;
                obj.UserId = model.USER_ID;
                obj.Datetime = DateTime.Now;
                obj.Remarks = model.REMARKS;
                obj.RemarksDt = Convert.ToDateTime(model.REMARKS_DT);
                context.T30IcReceiveds.Add(obj);
                context.SaveChanges();
                flag = 1;
            }
            else
            {
                _data.BkNo = model.BK_NO;
                _data.SetNo = model.SET_NO;
                _data.IeCd = Convert.ToByte(model.IE_CD);
                _data.IcSubmitDt = Convert.ToDateTime(model.IC_SUBMIT_DT);
                _data.UserId = model.USER_ID;
                _data.Datetime = DateTime.Now;
                _data.Remarks = model.REMARKS;
                _data.RemarksDt = Convert.ToDateTime(model.REMARKS_DT);
                context.SaveChanges();
                flag = 1;
            }
            return flag;
        }

        public int CheckIC(IC_ReceiptModel model)
        {
            var res = context.T10IcBooksets
                        .Where(x => x.BkNo == model.BK_NO && Convert.ToInt32(x.SetNoFr) >= Convert.ToInt32(model.SET_NO) && Convert.ToInt32(x.SetNoTo) <= Convert.ToInt32(model.SET_NO) && x.IssueToIecd == model.IE_CD)
                        .Select(x => DateTime.ParseExact(x.IssueDt.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture)).FirstOrDefault().ToString();
            var myYear = Convert.ToDateTime(model.IC_SUBMIT_DT).Year;
            var myMonth = Convert.ToDateTime(model.IC_SUBMIT_DT).Month;
            var myDay = Convert.ToDateTime(model.IC_SUBMIT_DT).Day;
            var dt = myYear + myMonth + myDay;

            int i = Convert.ToInt32(res) == dt ? 1 : 0;

            if (res != "")
            {
                if (i > 0)
                {
                    return 3;
                }
                else
                {
                    var res1 = context.T20Ics
                                .Where(x => x.BkNo == model.BK_NO && x.SetNo == model.SET_NO && x.CaseNo.Remove(1) == model.REGION)
                                .Select(x => x.BillNo).FirstOrDefault().ToString();
                    if(res1 == "" || res1 == null)
                    {
                        return 1;
                    }
                    else
                    {   
                        return 2;
                    }
                }
            }
            else
            {
                return 0;
            }
        }
    }
}
