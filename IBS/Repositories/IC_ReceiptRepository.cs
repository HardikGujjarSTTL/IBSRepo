using DocumentFormat.OpenXml.InkML;
using Humanizer;
using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;

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
                //orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
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
            List<SelectListItem> IE = (from a in context.T09Ies
                                       orderby a.IeName
                                       where a.IeRegion == region && a.IeStatus == null
                                       select new SelectListItem
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
            var _data = (from item in context.T30IcReceiveds
                         where item.BkNo.Trim() == model.BK_NO.Trim() && item.SetNo.Trim() == model.SET_NO.Trim() && item.Region.Trim() == model.REGION.Trim()
                         select item).FirstOrDefault();

            if (_data == null)
            {
                T30IcReceived obj = new T30IcReceived();
                obj.Region = model.REGION;
                obj.BkNo = model.BK_NO;
                obj.SetNo = model.SET_NO;
                obj.IeCd = model.IE_CD;
                obj.IcSubmitDt = string.IsNullOrEmpty(model.IC_SUBMIT_DT) ? null : Convert.ToDateTime(model.IC_SUBMIT_DT);
                obj.BillNo = model.BILL_NO;
                obj.UserId = model.USER_NAME;
                obj.Datetime = DateTime.Now;
                obj.Remarks = model.REMARKS;
                obj.RemarksDt = string.IsNullOrEmpty(model.REMARKS_DT) ? null : Convert.ToDateTime(model.REMARKS_DT);
                obj.Createdby = Convert.ToInt32(model.USER_ID);
                obj.Createddate = DateTime.Now;
                context.T30IcReceiveds.Add(obj);
                context.SaveChanges();
                flag = 1;
            }
            else
            {
                _data.BkNo = model.BK_NO;
                _data.SetNo = model.SET_NO;
                _data.IeCd = model.IE_CD;
                _data.IcSubmitDt = string.IsNullOrEmpty(model.IC_SUBMIT_DT) ? null : Convert.ToDateTime(model.IC_SUBMIT_DT);
                _data.UserId = model.USER_NAME;
                _data.Datetime = DateTime.Now;
                _data.Remarks = model.REMARKS;
                _data.RemarksDt = string.IsNullOrEmpty(model.REMARKS_DT) ? null : Convert.ToDateTime(model.REMARKS_DT);
                _data.Updatedby = Convert.ToInt32(model.USER_ID);
                _data.Updateddate = DateTime.Now;
                context.SaveChanges();
                flag = 2;
            }
            return flag;
        }

        public int IC_Receipt_Delete(IC_ReceiptModel model)
        {
            var flag = 0;
            var _data = (from item in context.T30IcReceiveds
                         where item.BkNo.Trim() == model.BK_NO.Trim() && item.SetNo.Trim() == model.SET_NO.Trim() && item.Region.Trim() == model.REGION.Trim()
                         select item).FirstOrDefault();
            if (_data != null)
            {
                _data.Isdeleted = 1;
                _data.Updatedby = model.USER_ID;
                _data.Updateddate = DateTime.Now;
                context.SaveChanges();
                flag = 1;
            }
            return flag;
        }

        public int CheckIC(IC_ReceiptModel model)
        {
            var bscheck = (from item in context.T10IcBooksets
                           where item.BkNo.Trim().ToUpper() == model.BK_NO.Trim() &&
                                 Convert.ToInt32(model.SET_NO) >= Convert.ToInt32(item.SetNoFr) && Convert.ToInt32(model.SET_NO) <= Convert.ToInt32(item.SetNoTo) &&
                                 item.IssueToIecd == 873
                           select new
                           {
                               IssueDate = Convert.ToDateTime(item.IssueDt).ToString("yyyyMMdd")
                           }).FirstOrDefault().IssueDate;

            string myYear = Convert.ToString(Convert.ToDateTime(model.IC_SUBMIT_DT).Year);
            string myMonth = Convert.ToString(Convert.ToDateTime(model.IC_SUBMIT_DT).Month);
            string myDay = Convert.ToString(Convert.ToDateTime(model.IC_SUBMIT_DT).Day);
            var dt = myYear + myMonth + myDay;


            int i = bscheck.CompareTo(dt);

            if (!string.IsNullOrEmpty(bscheck))
            {
                if (i > 0)
                {
                    return 3;
                }
                else
                {
                    var bscheck1 = (from item in context.T20Ics
                                    where item.BkNo.Trim().ToUpper() == model.BK_NO.Trim() &&
                                          item.SetNo.Trim().ToUpper() == model.SET_NO.Trim() &&
                                          item.CaseNo.Substring(0, 1) == model.REGION
                                    select item.BillNo).FirstOrDefault();
                    if (string.IsNullOrEmpty(bscheck1))
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

        public List<IC_Unbilled_List_Model> Get_UnBilled_IC(string FromDate, string ToDate, string Region)
        {
            string REGION = "";
            if (!string.IsNullOrEmpty(Region))
            {
                REGION = Region;
            }

            FromDate = FromDate.ToString() == "" ? string.Empty : FromDate.ToString();
            ToDate = ToDate.ToString() == "" ? string.Empty : ToDate.ToString();
            REGION = REGION.ToString() == "" ? string.Empty : REGION.ToString();

            OracleParameter[] par = new OracleParameter[4];
            par[0] = new OracleParameter("P_FROMDATE", OracleDbType.Varchar2, FromDate, ParameterDirection.Input);
            par[1] = new OracleParameter("P_TODATE", OracleDbType.Varchar2, ToDate, ParameterDirection.Input);
            par[2] = new OracleParameter("P_REGION", OracleDbType.Varchar2, REGION, ParameterDirection.Input);
            par[3] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("SP_GET_UNBILLED_IC", par, 1);
            DataTable dt = ds.Tables[0];


            List<IC_Unbilled_List_Model> list = dt.AsEnumerable().Select(row => new IC_Unbilled_List_Model
            {
                SUBMIT_DATE = row["SUBMIT_DATE"].ToString(),
                IC_SUBMIT_DATE = row["IC_SUBMIT_DATE"].ToString(),
                IE_NAME = row["IE_NAME"].ToString(),
                BK_NO = row["BK_NO"].ToString(),
                SET_NO = row["SET_NO"].ToString(),
                CLIENT_TYPE = row["CLIENT_TYPE"].ToString(),
                REMARKS = row["REMARKS"].ToString(),
                REMARKS_DATE = row["REMARKS_DATE"].ToString(),
                IC_DATE = row["IC_DATE"].ToString(),
            }).ToList();

            return list;
        }

        public List<ICIssueNotReceiveModel> Get_IC_Issue_Not_Receive(string FromDate, string ToDate, UserSessionModel model)
        {
            string REGION = "";
            if (!string.IsNullOrEmpty(model.Region))
            {
                REGION = model.Region;
            }

            FromDate = FromDate.ToString() == "" ? string.Empty : FromDate.ToString();
            ToDate = ToDate.ToString() == "" ? string.Empty : ToDate.ToString();
            REGION = REGION.ToString() == "" ? string.Empty : REGION.ToString();

            var IE_CD = (model.RoleName != "Inspection Engineer (IE)") ? null : Convert.ToString(model.IeCd);

            OracleParameter[] par = new OracleParameter[5];
            par[0] = new OracleParameter("P_FROMDATE", OracleDbType.Varchar2, FromDate, ParameterDirection.Input);
            par[1] = new OracleParameter("P_TODATE", OracleDbType.Varchar2, ToDate, ParameterDirection.Input);
            par[2] = new OracleParameter("P_REGION", OracleDbType.Varchar2, REGION, ParameterDirection.Input);
            par[3] = new OracleParameter("P_IE_CD", OracleDbType.Varchar2, IE_CD, ParameterDirection.Input);
            par[4] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);


            var ds = DataAccessDB.GetDataSet("SP_GET_IC_ISSUED_BY_IE", par, 1);
            DataTable dt = ds.Tables[0];
            dt.Columns.Add("IsTIF", typeof(bool));
            dt.Columns.Add("IsPDF", typeof(bool));

            List<ICIssueNotReceiveModel> list = dt.AsEnumerable().Select(row => new ICIssueNotReceiveModel
            {
                IC_ISSUED_DT = Convert.ToString(row["IC_ISSUED_DT"]),
                BK_NO = Convert.ToString(row["BK_NO"]),
                SET_NO = Convert.ToString(row["SET_NO"]),
                IE_NAME = Convert.ToString(row["IE_NAME"]),
                CO_NAME = Convert.ToString(row["CO_NAME"]),
                CASE_NO = Convert.ToString(row["CASE_NO"]),
                PO_SOURCE = Convert.ToString(row["PO_SOURCE"]),
                PO_YR = Convert.ToString(row["PO_YR"]),
                PO_NO = Convert.ToString(row["PO_NO"]),
                IMMS_RLY_CD = Convert.ToString(row["IMMS_RLY_CD"]),
                RLY_NONRLY = Convert.ToString(row["RLY_NONRLY"])
            }).ToList();

            return list;
        }

        public List<ICStatusListModel> Get_IC_Status(string FromDate, string ToDate, string IECD, string Region)
        {
            string REGION = "", IE_CD = null;

            FromDate = FromDate.ToString() == "" ? string.Empty : FromDate.ToString();
            ToDate = ToDate.ToString() == "" ? string.Empty : ToDate.ToString();
            IE_CD = IE_CD == "" ? null : IE_CD;
            REGION = REGION.ToString() == "" ? string.Empty : REGION.ToString();

            OracleParameter[] par = new OracleParameter[5];
            par[0] = new OracleParameter("P_FROMDATE", OracleDbType.Varchar2, FromDate, ParameterDirection.Input);
            par[1] = new OracleParameter("P_TODATE", OracleDbType.Varchar2, ToDate, ParameterDirection.Input);
            par[2] = new OracleParameter("P_REGION", OracleDbType.Varchar2, REGION, ParameterDirection.Input);
            par[3] = new OracleParameter("P_IE_CD", OracleDbType.Varchar2, IE_CD, ParameterDirection.Input);
            par[4] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("SP_GET_IC_STATUS", par, 1);
            DataTable dt = ds.Tables[0];

            List<ICStatusListModel> list = dt.AsEnumerable().Select(row => new ICStatusListModel
            {
                IC_SUBMIT_DT = Convert.ToString(row["IC_SUBMIT_DT"]),
                IE_NAME = Convert.ToString(row["IE_NAME"]),
                BK_NO = Convert.ToString(row["BK_NO"]),
                SET_NO = Convert.ToString(row["SET_NO"]),
                BILL_NO = Convert.ToString(row["BILL_NO"])
            }).ToList();

            return list;
        }
    }
}
