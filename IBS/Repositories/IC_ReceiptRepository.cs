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
                obj.UserId = model.USER_NAME;
                obj.Datetime = DateTime.Now;
                obj.Remarks = model.REMARKS;
                obj.RemarksDt = Convert.ToDateTime(model.REMARKS_DT);
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
                _data.IeCd = Convert.ToByte(model.IE_CD);
                _data.IcSubmitDt = Convert.ToDateTime(model.IC_SUBMIT_DT);
                _data.UserId = model.USER_NAME;
                _data.Datetime = DateTime.Now;
                _data.Remarks = model.REMARKS;
                _data.RemarksDt = Convert.ToDateTime(model.REMARKS_DT);
                _data.Updatedby = Convert.ToInt32(model.USER_ID);
                context.SaveChanges();
                flag = 1;
            }
            return flag;
        }

        public int IC_Receipt_Delete(IC_ReceiptModel model)
        {
            var flag = 0;
            var _data = context.T30IcReceiveds.Where(x => x.BkNo == model.BK_NO && x.SetNo == model.SET_NO && x.Region == model.REGION).Select(x => x).FirstOrDefault();
            //var _data = context.T30IcReceiveds.Find(model.REGION, model.BK_NO, model.SET_NO);
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
            var res = context.T10IcBooksets
                        .Where(x => x.BkNo == model.BK_NO && Convert.ToInt32(x.SetNoFr) >= Convert.ToInt32(model.SET_NO) && Convert.ToInt32(x.SetNoTo) <= Convert.ToInt32(model.SET_NO) && x.IssueToIecd == model.IE_CD)
                        .Select(x => DateTime.ParseExact(x.IssueDt.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture).ToString()).FirstOrDefault();
            string myYear = Convert.ToString(Convert.ToDateTime(model.IC_SUBMIT_DT).Year);
            string myMonth = Convert.ToString(Convert.ToDateTime(model.IC_SUBMIT_DT).Month);
            string myDay = Convert.ToString(Convert.ToDateTime(model.IC_SUBMIT_DT).Day);
            var dt = myYear + myMonth + myDay;

            int i = res == dt ? 0 : 1;

            if (res != "" && res != null)
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
                    if (res1 == "" || res1 == null)
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

        public DTResult<ICReportModel> Get_UnBilled_IC([FromBody] DTParameters dtParameters, string Region)
        {
            DTResult<ICReportModel> dTResult = new() { draw = 0 };
            IQueryable<ICReportModel>? query = null;

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

            string FromDate = "", ToDate = "", REGION = "";
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["FromDate"]))
            {
                FromDate = Convert.ToString(dtParameters.AdditionalValues["FromDate"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["ToDate"]))
            {
                ToDate = Convert.ToString(dtParameters.AdditionalValues["ToDate"]);
            }
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


            List<ICReportModel> list = dt.AsEnumerable().Select(row => new ICReportModel
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

            query = list.AsQueryable();

            dTResult.recordsTotal = ds.Tables[0].Rows.Count;
            dTResult.recordsFiltered = ds.Tables[0].Rows.Count;
            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();
            dTResult.draw = dtParameters.Draw;
            return dTResult;
        }

        public List<ICIssueNotReceiveModel> Get_IC_Issue_Not_Receive(string FromDate, string ToDate, UserSessionModel model)
        //public DTResult<ICIssueNotReceiveModel> Get_IC_Issue_Not_Receive([FromBody] DTParameters dtParameters, string Region, string UserName, string Ic_Cd)
        {
            //DTResult<ICIssueNotReceiveModel> dTResult = new() { draw = 0 };
            //IQueryable<ICIssueNotReceiveModel>? query = null;

            //var searchBy = dtParameters.Search?.Value;
            //var orderCriteria = string.Empty;
            //var orderAscendingDirection = false;//true;


            //if (dtParameters.Order != null)
            //{
            //    // in this example we just default sort on the 1st column
            //    orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

            //    if (orderCriteria == "")
            //    {
            //        orderCriteria = "CO_NAME"; //"CASE_NO";// "BK_NO";
            //    }
            //    //orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
            //}
            //else
            //{
            //    orderCriteria = "CO_NAME"; //"CASE_NO";// "BK_NO";
            //    orderAscendingDirection = true;
            //}

            //string FromDate = "", ToDate = "", REGION = "";
            //if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["FromDate"]))
            //{
            //    FromDate = Convert.ToString(dtParameters.AdditionalValues["FromDate"]);
            //}
            //if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["ToDate"]))
            //{
            //    ToDate = Convert.ToString(dtParameters.AdditionalValues["ToDate"]);
            //}
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
            //query = list.AsQueryable();

            //dTResult.recordsTotal = ds.Tables[0].Rows.Count;
            //dTResult.recordsFiltered = ds.Tables[0].Rows.Count;
            //if (dtParameters.Length == -1) dtParameters.Length = query.Count();
            //dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();
            //dTResult.draw = dtParameters.Draw;
            //return dTResult;
        }

        public DTResult<IC_ReceiptModel> Get_IC_Status([FromBody] DTParameters dtParameters, string Region)
        {
            DTResult<IC_ReceiptModel> dTResult = new() { draw = 0 };
            IQueryable<IC_ReceiptModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = false;//true;


            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "IC_SUBMIT_DT"; //"CASE_NO";// "BK_NO";
                }
                //orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
            }
            else
            {
                orderCriteria = "IC_SUBMIT_DT"; //"CASE_NO";// "BK_NO";
                orderAscendingDirection = true;
            }

            string FromDate = "", ToDate = "", REGION = "", IE_CD = null;
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["FromDate"]))
            {
                FromDate = Convert.ToString(dtParameters.AdditionalValues["FromDate"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["ToDate"]))
            {
                ToDate = Convert.ToString(dtParameters.AdditionalValues["ToDate"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["IECD"]))
            {
                IE_CD = Convert.ToString(dtParameters.AdditionalValues["IECD"]);
            }
            if (!string.IsNullOrEmpty(Region))
            {
                REGION = Region;
            }

            FromDate = FromDate.ToString() == "" ? string.Empty : FromDate.ToString();
            ToDate = ToDate.ToString() == "" ? string.Empty : ToDate.ToString();
            REGION = REGION.ToString() == "" ? string.Empty : REGION.ToString();


            OracleParameter[] par = new OracleParameter[5];
            par[0] = new OracleParameter("P_FROMDATE", OracleDbType.Varchar2, FromDate, ParameterDirection.Input);
            par[1] = new OracleParameter("P_TODATE", OracleDbType.Varchar2, ToDate, ParameterDirection.Input);
            par[2] = new OracleParameter("P_REGION", OracleDbType.Varchar2, REGION, ParameterDirection.Input);
            par[3] = new OracleParameter("P_IE_CD", OracleDbType.Varchar2, IE_CD, ParameterDirection.Input);
            par[4] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("SP_GET_IC_STATUS", par, 1);
            DataTable dt = ds.Tables[0];


            List<IC_ReceiptModel> list = dt.AsEnumerable().Select(row => new IC_ReceiptModel
            {
                IC_SUBMIT_DT = Convert.ToString(row["IC_SUBMIT_DT"]),
                IE_NAME = Convert.ToString(row["IE_NAME"]),
                BK_NO = Convert.ToString(row["BK_NO"]),
                SET_NO = Convert.ToString(row["SET_NO"]),
                BILL_NO = Convert.ToString(row["BILL_NO"])
            }).ToList();

            query = list.AsQueryable();

            dTResult.recordsTotal = ds.Tables[0].Rows.Count;
            dTResult.recordsFiltered = ds.Tables[0].Rows.Count;
            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();
            dTResult.draw = dtParameters.Draw;
            return dTResult;
        }
    }
}
