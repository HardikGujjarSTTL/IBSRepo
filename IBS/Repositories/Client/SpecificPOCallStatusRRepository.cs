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
    public class SpecificPOCallStatusRRepository : ISpecificPOCallStatusRepository
    {
        private readonly ModelContext context;

        public SpecificPOCallStatusRRepository(ModelContext context)
        {
            this.context = context;
        }
        public DTResult<ClientCallRptModel> GetPOCallStatusIndex(DTParameters dtParameters)
        {

            DTResult<ClientCallRptModel> dTResult = new() { draw = 0 };
            IQueryable<ClientCallRptModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "PO_NO";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "PO_NO";
                orderAscendingDirection = true;
            }
            var Status = dtParameters.AdditionalValues?.GetValueOrDefault("ReportStatus");
            DataSet ds = new DataSet();
            //if(Status == "")
            //{
            //    OracleParameter[] par = new OracleParameter[5];
            //    par[0] = new OracleParameter("p_CLT", OracleDbType.NVarchar2, "R", ParameterDirection.Input);
            //    par[1] = new OracleParameter("p_RLYCD", OracleDbType.NVarchar2, "SR", ParameterDirection.Input);
            //    par[2] = new OracleParameter("p_wFrmDt", OracleDbType.Date, dtParameters.AdditionalValues?.GetValueOrDefault("FromDate"), ParameterDirection.Input);
            //    par[3] = new OracleParameter("p_wToDt", OracleDbType.Date, dtParameters.AdditionalValues?.GetValueOrDefault("ToDate"), ParameterDirection.Input);
            //    par[4] = new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

            //     ds = DataAccessDB.GetDataSet("ClientCallRpt", par, 4);
            //}
            //else
            //{
            OracleParameter[] par = new OracleParameter[4];
            par[0] = new OracleParameter("ORGN_TYPE_IN", OracleDbType.NVarchar2, "U", ParameterDirection.Input);
            par[1] = new OracleParameter("ORGN_IN", OracleDbType.NVarchar2, "SAIL", ParameterDirection.Input);
            par[2] = new OracleParameter("PODT_IN", OracleDbType.Date, dtParameters.AdditionalValues?.GetValueOrDefault("PODate"), ParameterDirection.Input);
            par[3] = new OracleParameter("RESULT", OracleDbType.RefCursor, ParameterDirection.Output);

            ds = DataAccessDB.GetDataSet("Get_POCallStatus", par, 3);
            //}


            List<ClientCallRptModel> modelList = new List<ClientCallRptModel>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    ClientCallRptModel model = new ClientCallRptModel
                    {
                        L5NO_PO = Convert.ToString(row["L5NO_PO"]),
                        PO_NO = Convert.ToString(row["PO_NO"]),
                        PO_DT = Convert.ToString(row["PO_DT"]),
                        RLY_NONRLY = Convert.ToString(row["RLY_NONRLY"]),
                        RLY_CD = Convert.ToString(row["RLY_CD"]),
                        
                    };

                    modelList.Add(model);
                }
            }


            query = modelList.AsQueryable();

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.PO_NO).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.PO_NO).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;


        }
        public DTResult<ClientCallRptModel> GetPOCallStatus(DTParameters dtParameters)
        {

            DTResult<ClientCallRptModel> dTResult = new() { draw = 0 };
            IQueryable<ClientCallRptModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "PO_NO";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "PO_NO";
                orderAscendingDirection = true;
            }
            var Status = dtParameters.AdditionalValues?.GetValueOrDefault("ReportStatus");
            DataSet ds = new DataSet();
            //if(Status == "")
            //{
            //    OracleParameter[] par = new OracleParameter[5];
            //    par[0] = new OracleParameter("p_CLT", OracleDbType.NVarchar2, "R", ParameterDirection.Input);
            //    par[1] = new OracleParameter("p_RLYCD", OracleDbType.NVarchar2, "SR", ParameterDirection.Input);
            //    par[2] = new OracleParameter("p_wFrmDt", OracleDbType.Date, dtParameters.AdditionalValues?.GetValueOrDefault("FromDate"), ParameterDirection.Input);
            //    par[3] = new OracleParameter("p_wToDt", OracleDbType.Date, dtParameters.AdditionalValues?.GetValueOrDefault("ToDate"), ParameterDirection.Input);
            //    par[4] = new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

            //     ds = DataAccessDB.GetDataSet("ClientCallRpt", par, 4);
            //}
            //else
            //{
            OracleParameter[] par = new OracleParameter[5];
            par[0] = new OracleParameter("PNO_IN", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("PONO"), ParameterDirection.Input);
            par[1] = new OracleParameter("PDT_IN", OracleDbType.Date, dtParameters.AdditionalValues?.GetValueOrDefault("PODT"), ParameterDirection.Input);
            par[2] = new OracleParameter("CLT_IN", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("RLYNONRLY"), ParameterDirection.Input);
            par[3] = new OracleParameter("RLYCD_IN", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("RLY_CD"), ParameterDirection.Input);
            par[4] = new OracleParameter("RESULT", OracleDbType.RefCursor, ParameterDirection.Output);

            ds = DataAccessDB.GetDataSet("GET_CALLPO", par, 4);
            //}


            List<ClientCallRptModel> modelList = new List<ClientCallRptModel>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    ClientCallRptModel model = new ClientCallRptModel
                    {
                        Vendor = Convert.ToString(row["Vendor"]),
                        Manufacturer = Convert.ToString(row["Manufacturer"]),
                        VEND_CD = Convert.ToString(row["VEND_CD"]),
                        MFG_CD = Convert.ToString(row["MFG_CD"]),
                        Consignee = Convert.ToString(row["Consignee"]),
                        ITEM_DESC_PO = Convert.ToString(row["ITEM_DESC_PO"]),
                        QTY_TO_INSP = Convert.ToString(row["QTY_TO_INSP"]),
                        CALL_MARK_DT = Convert.ToString(row["CALL_MARK_DT"]),
                        IE_NAME = Convert.ToString(row["IE_NAME"]),
                        IE_PHONE_NO = Convert.ToString(row["IE_PHONE_NO"]),
                        PO_NO = Convert.ToString(row["PO_NO"]),
                        PO_DATE = Convert.ToString(row["PO_DATE"]),
                        CASE_NO = Convert.ToString(row["CASE_NO"]),
                        REMARK = Convert.ToString(row["REMARK"]),
                        DESIRE_DT = Convert.ToString(row["DESIRE_DT"]),
                        CALL_STATUS = Convert.ToString(row["CALL_STATUS"]),
                        COLOUR = Convert.ToString(row["COLOUR"]),
                        MFG_PERS = Convert.ToString(row["MFG_PERS"]),
                        MFG_PHONE = Convert.ToString(row["MFG_PHONE"]),
                        CALL_SNO = Convert.ToString(row["CALL_SNO"]),
                        HOLOGRAM = Convert.ToString(row["HOLOGRAM"]),
                        IC_PHOTO = Convert.ToString(row["IC_PHOTO"]),
                        IC_PHOTO_A1 = Convert.ToString(row["IC_PHOTO_A1"]),
                        IC_PHOTO_A2 = Convert.ToString(row["IC_PHOTO_A2"]),
                        COUNT = Convert.ToInt32(row["COUNT"]),
                        CO_NAME = Convert.ToString(row["CO_NAME"]),
                    };

                    modelList.Add(model);
                }
            }


            query = modelList.AsQueryable();

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.Vendor).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.Vendor).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;


        }

    }
}
