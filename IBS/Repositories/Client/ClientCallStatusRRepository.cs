using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Reflection.Emit;
using static IBS.Helper.Enums;

namespace IBS.Repositories
{
    public class ClientCallStatusRRepository : IClientCallStatusRepository
    {
        private readonly ModelContext context;

        public ClientCallStatusRRepository(ModelContext context)
        {
            this.context = context;
        }
        //public DTResult<ClientCallRptModel> GetCallStatusR(DTParameters dtParameters, string OrgType, string Org)
        //{

        //    DTResult<ClientCallRptModel> dTResult = new() { draw = 0 };
        //    IQueryable<ClientCallRptModel>? query = null;

        //    var searchBy = dtParameters.Search?.Value;
        //    var orderCriteria = string.Empty;
        //    var orderAscendingDirection = true;

        //    if (dtParameters.Order != null)
        //    {
        //        // in this example we just default sort on the 1st column
        //        orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

        //        if (orderCriteria == "")
        //        {
        //            orderCriteria = "PO_NO";
        //        }
        //        orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
        //    }
        //    else
        //    {
        //        // if we have an empty search then just order the results by Id ascending
        //        orderCriteria = "PO_NO";
        //        orderAscendingDirection = true;
        //    }

        //    OracleParameter[] par = new OracleParameter[5];
        //    par[0] = new OracleParameter("p_CLT", OracleDbType.NVarchar2, OrgType, ParameterDirection.Input);
        //    par[1] = new OracleParameter("p_RLYCD", OracleDbType.NVarchar2, Org, ParameterDirection.Input);
        //    par[2] = new OracleParameter("p_wFrmDt", OracleDbType.Date, dtParameters.AdditionalValues?.GetValueOrDefault("FromDate"), ParameterDirection.Input);
        //    par[3] = new OracleParameter("p_wToDt", OracleDbType.Date, dtParameters.AdditionalValues?.GetValueOrDefault("ToDate"), ParameterDirection.Input);
        //    par[4] = new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

        //    var ds = DataAccessDB.GetDataSet("Reject_ClientCallRpt", par, 4);

        //    List<ClientCallRptModel> modelList = new List<ClientCallRptModel>();
        //    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //    {
        //        foreach (DataRow row in ds.Tables[0].Rows)
        //        {
        //            ClientCallRptModel model = new ClientCallRptModel
        //            {
        //                VendorName = Convert.ToString(row["Vendor"]),
        //                PO_NO = Convert.ToString(row["PO_NO"]),
        //                PO_DT = Convert.ToString(row["PO_DT"]),
        //                ITEM_DESC = Convert.ToString(row["ITEM_DESC"]),
        //                QTY_TO_INSP = Convert.ToString(row["QTY_TO_INSP"]),
        //                QTY_REJECTED = Convert.ToString(row["QTY_REJECTED"]),
        //                REASON_REJECT = Convert.ToString(row["REASON_REJECT"]),
        //                IC_DT = Convert.ToString(row["IC_DT"]),
        //                IE_NAME = Convert.ToString(row["IE_NAME"]),
        //                BK_NO = Convert.ToString(row["BK_NO"]),
        //                SET_NO = Convert.ToString(row["SET_NO"]),


        //            };

        //            modelList.Add(model);
        //        }
        //    }


        //    query = modelList.AsQueryable();

        //    dTResult.recordsTotal = query.Count();

        //    if (!string.IsNullOrEmpty(searchBy))
        //        query = query.Where(w => Convert.ToString(w.PO_NO).ToLower().Contains(searchBy.ToLower())
        //        || Convert.ToString(w.PO_NO).ToLower().Contains(searchBy.ToLower())
        //        );

        //    dTResult.recordsFiltered = query.Count();

        //    dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

        //    dTResult.draw = dtParameters.Draw;

        //    return dTResult;


        //}
        //public DTResult<ClientCallRptModel> GetCallStatusC(DTParameters dtParameters, string OrgType, string Org)
        //{

        //    DTResult<ClientCallRptModel> dTResult = new() { draw = 0 };
        //    IQueryable<ClientCallRptModel>? query = null;

        //    var searchBy = dtParameters.Search?.Value;
        //    var orderCriteria = string.Empty;
        //    var orderAscendingDirection = true;

        //    if (dtParameters.Order != null)
        //    {
        //        // in this example we just default sort on the 1st column
        //        orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

        //        if (orderCriteria == "")
        //        {
        //            orderCriteria = "PO_NO";
        //        }
        //        orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
        //    }
        //    else
        //    {
        //        // if we have an empty search then just order the results by Id ascending
        //        orderCriteria = "PO_NO";
        //        orderAscendingDirection = true;
        //    }
        //    var Status = dtParameters.AdditionalValues?.GetValueOrDefault("ReportStatus");
        //    DataSet ds = new DataSet();
        //    //if(Status == "")
        //    //{
        //    //    OracleParameter[] par = new OracleParameter[5];
        //    //    par[0] = new OracleParameter("p_CLT", OracleDbType.NVarchar2, "R", ParameterDirection.Input);
        //    //    par[1] = new OracleParameter("p_RLYCD", OracleDbType.NVarchar2, "SR", ParameterDirection.Input);
        //    //    par[2] = new OracleParameter("p_wFrmDt", OracleDbType.Date, dtParameters.AdditionalValues?.GetValueOrDefault("FromDate"), ParameterDirection.Input);
        //    //    par[3] = new OracleParameter("p_wToDt", OracleDbType.Date, dtParameters.AdditionalValues?.GetValueOrDefault("ToDate"), ParameterDirection.Input);
        //    //    par[4] = new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

        //    //     ds = DataAccessDB.GetDataSet("ClientCallRpt", par, 4);
        //    //}
        //    //else
        //    //{
        //    OracleParameter[] par = new OracleParameter[6];
        //    par[0] = new OracleParameter("p_CLT", OracleDbType.NVarchar2, OrgType, ParameterDirection.Input);
        //    par[1] = new OracleParameter("p_RLYCD", OracleDbType.NVarchar2, Org, ParameterDirection.Input);
        //    par[2] = new OracleParameter("p_wFrmDt", OracleDbType.Date, dtParameters.AdditionalValues?.GetValueOrDefault("FromDate"), ParameterDirection.Input);
        //    par[3] = new OracleParameter("p_wToDt", OracleDbType.Date, dtParameters.AdditionalValues?.GetValueOrDefault("ToDate"), ParameterDirection.Input);
        //    par[4] = new OracleParameter("p_status", OracleDbType.Varchar2, dtParameters.AdditionalValues?.GetValueOrDefault("ReportStatus"), ParameterDirection.Input);
        //    par[5] = new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

        //    ds = DataAccessDB.GetDataSet("DropdownSelected_ClientCallRpt", par, 5);
        //    //}


        //    List<ClientCallRptModel> modelList = new List<ClientCallRptModel>();
        //    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //    {
        //        foreach (DataRow row in ds.Tables[0].Rows)
        //        {
        //            ClientCallRptModel model = new ClientCallRptModel
        //            {
        //                Vendor = Convert.ToString(row["Vendor"]),
        //                Manufacturer = Convert.ToString(row["Manufacturer"]),
        //                VEND_CD = Convert.ToString(row["VEND_CD"]),
        //                MFG_CD = Convert.ToString(row["MFG_CD"]),
        //                Consignee = Convert.ToString(row["Consignee"]),
        //                ITEM_DESC_PO = Convert.ToString(row["ITEM_DESC_PO"]),
        //                QTY_TO_INSP = Convert.ToString(row["QTY_TO_INSP"]),
        //                CALL_MARK_DT = Convert.ToString(row["CALL_MARK_DT"]),
        //                IE_NAME = Convert.ToString(row["IE_NAME"]),
        //                IE_PHONE_NO = Convert.ToString(row["IE_PHONE_NO"]),
        //                PO_NO = Convert.ToString(row["PO_NO"]),
        //                PO_DATE = Convert.ToString(row["PO_DATE"]),
        //                CASE_NO = Convert.ToString(row["CASE_NO"]),
        //                REMARK = Convert.ToString(row["REMARK"]),
        //                DESIRE_DT = Convert.ToString(row["DESIRE_DT"]),
        //                CALL_STATUS = Convert.ToString(row["CALL_STATUS"]),
        //                COLOUR = Convert.ToString(row["COLOUR"]),
        //                MFG_PERS = Convert.ToString(row["MFG_PERS"]),
        //                MFG_PHONE = Convert.ToString(row["MFG_PHONE"]),
        //                CALL_SNO = Convert.ToString(row["CALL_SNO"]),
        //                HOLOGRAM = Convert.ToString(row["HOLOGRAM"]),
        //                IC_PHOTO = Convert.ToString(row["IC_PHOTO"]),
        //                IC_PHOTO_A1 = Convert.ToString(row["IC_PHOTO_A1"]),
        //                IC_PHOTO_A2 = Convert.ToString(row["IC_PHOTO_A2"]),
        //                COUNT = Convert.ToInt32(row["COUNT"]),
        //                CO_NAME = Convert.ToString(row["CO_NAME"]),
        //            };

        //            modelList.Add(model);
        //        }
        //    }


        //    query = modelList.AsQueryable();

        //    dTResult.recordsTotal = query.Count();

        //    if (!string.IsNullOrEmpty(searchBy))
        //        query = query.Where(w => Convert.ToString(w.Vendor).ToLower().Contains(searchBy.ToLower())
        //        || Convert.ToString(w.Vendor).ToLower().Contains(searchBy.ToLower())
        //        );

        //    dTResult.recordsFiltered = query.Count();

        //    dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

        //    dTResult.draw = dtParameters.Draw;

        //    return dTResult;


        //}
        public ClientCallRptModel GetCallStatusR(string FromDate, string ToDate, string ReportStatus, string OrgType, string Org)
        {

            ClientCallRptModel model = new();
            List<ClientCallRptModel> lstlab = new();


            OracleParameter[] par = new OracleParameter[5];
            par[0] = new OracleParameter("p_CLT", OracleDbType.NVarchar2, OrgType, ParameterDirection.Input);
            par[1] = new OracleParameter("p_RLYCD", OracleDbType.NVarchar2, Org, ParameterDirection.Input);
            par[2] = new OracleParameter("p_wFrmDt", OracleDbType.Date, FromDate, ParameterDirection.Input);
            par[3] = new OracleParameter("p_wToDt", OracleDbType.Date, ToDate, ParameterDirection.Input);
            par[4] = new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("Reject_ClientCallRpt", par, 4);


            if (ds.Tables[0].Rows.Count != 0)
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                    lstlab = JsonConvert.DeserializeObject<List<ClientCallRptModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                        model.VendorName = Convert.ToString(ds.Tables[0].Rows[0]["VendorName"]);
                        model.PO_NO = Convert.ToString(ds.Tables[0].Rows[0]["PO_NO"]);
                        model.PO_DT = Convert.ToString(ds.Tables[0].Rows[0]["PO_DT"]);
                        model.ITEM_DESC = Convert.ToString(ds.Tables[0].Rows[0]["ITEM_DESC"]);
                        model.QTY_TO_INSP = Convert.ToString(ds.Tables[0].Rows[0]["QTY_TO_INSP"]);
                        model.QTY_REJECTED = Convert.ToString(ds.Tables[0].Rows[0]["QTY_REJECTED"]);
                        model.REASON_REJECT = Convert.ToString(ds.Tables[0].Rows[0]["REASON_REJECT"]);
                        model.IC_DT = Convert.ToString(ds.Tables[0].Rows[0]["IC_DT"]);
                        model.IE_NAME = Convert.ToString(ds.Tables[0].Rows[0]["IE_NAME"]);
                        model.BK_NO = Convert.ToString(ds.Tables[0].Rows[0]["BK_NO"]);
                        model.SET_NO = Convert.ToString(ds.Tables[0].Rows[0]["SET_NO"]);
                }
                model.lstreport = lstlab;
            }
            return model;
        }
        public ClientCallRptModel GetCallStatusC(string FromDate, string ToDate, string ReportStatus, string OrgType, string Org)
        {

            ClientCallRptModel model = new();
            List<ClientCallRptModel> lstlab = new();


            OracleParameter[] par = new OracleParameter[6];
            par[0] = new OracleParameter("p_CLT", OracleDbType.NVarchar2, OrgType, ParameterDirection.Input);
            par[1] = new OracleParameter("p_RLYCD", OracleDbType.NVarchar2, Org, ParameterDirection.Input);
            par[2] = new OracleParameter("p_wFrmDt", OracleDbType.Date, FromDate, ParameterDirection.Input);
            par[3] = new OracleParameter("p_wToDt", OracleDbType.Date, ToDate, ParameterDirection.Input);
            par[4] = new OracleParameter("p_status", OracleDbType.Varchar2, ReportStatus, ParameterDirection.Input);
            par[5] = new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

           var ds = DataAccessDB.GetDataSet("DropdownSelected_ClientCallRpt", par, 5);


            if (ds.Tables[0].Rows.Count != 0)
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                    lstlab = JsonConvert.DeserializeObject<List<ClientCallRptModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                        model.Vendor = Convert.ToString(ds.Tables[0].Rows[0]["Vendor"]);
                        model.Manufacturer = Convert.ToString(ds.Tables[0].Rows[0]["Manufacturer"]);
                        model.VEND_CD = Convert.ToString(ds.Tables[0].Rows[0]["VEND_CD"]);
                        model.MFG_CD = Convert.ToString(ds.Tables[0].Rows[0]["MFG_CD"]);
                        model.Consignee = Convert.ToString(ds.Tables[0].Rows[0]["Consignee"]);
                        model.ITEM_DESC_PO = Convert.ToString(ds.Tables[0].Rows[0]["ITEM_DESC_PO"]);
                        model.QTY_TO_INSP = Convert.ToString(ds.Tables[0].Rows[0]["QTY_TO_INSP"]);
                        model.CALL_MARK_DT = Convert.ToString(ds.Tables[0].Rows[0]["CALL_MARK_DT"]);
                        model.IE_NAME = Convert.ToString(ds.Tables[0].Rows[0]["IE_NAME"]);
                        model.IE_PHONE_NO = Convert.ToString(ds.Tables[0].Rows[0]["IE_PHONE_NO"]);
                        model.PO_NO = Convert.ToString(ds.Tables[0].Rows[0]["PO_NO"]);
                        model.PO_DATE = Convert.ToString(ds.Tables[0].Rows[0]["PO_DATE"]);
                        model.CASE_NO = Convert.ToString(ds.Tables[0].Rows[0]["CASE_NO"]);
                        model.REMARK = Convert.ToString(ds.Tables[0].Rows[0]["REMARK"]);
                        model.DESIRE_DT = Convert.ToString(ds.Tables[0].Rows[0]["DESIRE_DT"]);
                        model.CALL_STATUS = Convert.ToString(ds.Tables[0].Rows[0]["CALL_STATUS"]);
                        model.COLOUR = Convert.ToString(ds.Tables[0].Rows[0]["COLOUR"]);
                        model.MFG_PERS = Convert.ToString(ds.Tables[0].Rows[0]["MFG_PERS"]);
                        model.MFG_PHONE = Convert.ToString(ds.Tables[0].Rows[0]["MFG_PHONE"]);
                        model.CALL_SNO = Convert.ToString(ds.Tables[0].Rows[0]["CALL_SNO"]);
                        model.HOLOGRAM = Convert.ToString(ds.Tables[0].Rows[0]["HOLOGRAM"]);
                        model.IC_PHOTO = Convert.ToString(ds.Tables[0].Rows[0]["IC_PHOTO"]);
                        model.IC_PHOTO_A1 = Convert.ToString(ds.Tables[0].Rows[0]["IC_PHOTO_A1"]);
                        model.IC_PHOTO_A2 = Convert.ToString(ds.Tables[0].Rows[0]["IC_PHOTO_A2"]);
                        //model.COUNT = Convert.ToInt32(ds.Tables[0].Rows[0]["COUNT"]);
                        model.CO_NAME = Convert.ToString(ds.Tables[0].Rows[0]["CO_NAME"]);
                }
                model.lstreport = lstlab;
            }
            return model;
        }

    }
}
