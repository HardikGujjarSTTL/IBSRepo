using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Xml.Linq;
using static IBS.Helper.Enums;

namespace IBS.Repositories
{
    public class SuperSurpirseFormRRepository : ISuperSurpirseFormRepository
    {
        private readonly ModelContext context;

        public SuperSurpirseFormRRepository(ModelContext context)
        {
            this.context = context;
        }
        public DTResult<SuperSurpirseFormModel> GetSuperFormData(DTParameters dtParameters, string Regin)
        {
            var Caseno = dtParameters.AdditionalValues?.GetValueOrDefault("CaseNo");
            var CallDt = DateTime.Parse(dtParameters.AdditionalValues?.GetValueOrDefault("CallDate"));
            var CallSNo = int.Parse(dtParameters.AdditionalValues?.GetValueOrDefault("CallSNo"));

            // Assuming context is a database context for Entity Framework
            var exists = context.T44SuperSurprises
                .FirstOrDefault(s => s.CaseNo == Caseno && s.CallRecvDt == CallDt && s.CallSno == CallSNo);
            int count = 0;
            if(exists != null)
            {
                count = 1;
            }

            DTResult<SuperSurpirseFormModel> dTResult = new() { draw = 0 };
            IQueryable<SuperSurpirseFormModel>? query = null;

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
            var dt = dtParameters.AdditionalValues?.GetValueOrDefault("CallDate");
            var calldt = Convert.ToDateTime(dt).ToString("MM/dd/yyyy");
            OracleParameter[] par = new OracleParameter[5];
            par[0] = new OracleParameter("CASE_NO_PARAM", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("CaseNo"), ParameterDirection.Input);
            par[1] = new OracleParameter("CALL_DT_PARAM", OracleDbType.Date, calldt, ParameterDirection.Input);
            par[2] = new OracleParameter("CALL_SNO_PARAM", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("CallSNo"), ParameterDirection.Input);
            par[3] = new OracleParameter("REGION_PARAM", OracleDbType.NVarchar2, Regin, ParameterDirection.Input);
            par[4] = new OracleParameter("CUR_OUT", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("Get_Super_FormData", par, 4);

            List<SuperSurpirseFormModel> modelList = new List<SuperSurpirseFormModel>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {


                    SuperSurpirseFormModel model = new SuperSurpirseFormModel
                    {
                        VEND_CD = Convert.ToString(row["VEND_CD"]),
                        MFG = Convert.ToString(row["MFG"]),
                        IE_NAME = Convert.ToString(row["IE_NAME"]),
                        IE_CD = Convert.ToString(row["IE_CD"]),
                        Case_No = Convert.ToString(row["Case_No"]),
                        PO_NO = Convert.ToString(row["PO_NO"]),
                        PO_DT = Convert.ToString(row["PO_DT"]),
                        CO_CD = Convert.ToString(row["CO_CD"]),
                        CallDt = Convert.ToString(row["call_recv_dt"]),
                        CallSNo = Convert.ToString(row["call_sno"]),
                        Count = count,
                    };

                    modelList.Add(model);
                }
            }

            //query = from l in context.Roles
            //        where l.Isdeleted == 0
            //        select new RoleModel
            //        {
            //            RoleId = l.RoleId,
            //            Rolename = l.Rolename,
            //            Roledescription = l.Roledescription,
            //            Issysadmin = Convert.ToBoolean(l.Issysadmin),
            //            Isactive = Convert.ToBoolean(l.Isactive),
            //            Isdeleted = l.Isdeleted,
            //            Createddate = l.Createddate,
            //            Createdby = l.Createdby,
            //            Updateddate = l.Updateddate,
            //            Updatedby = l.Updatedby
            //        };

            query = modelList.AsQueryable();

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.IE_NAME).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.IE_NAME).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();
            //dTResult.data = query.ToList();
            dTResult.draw = dtParameters.Draw;

            return dTResult;

            //using (var dbContext = context.Database.GetDbConnection())
            //{

            //}

            //return dTResult;
        }

        public static string GetDateString(string sqlQuery)
        {
            ModelContext context = new ModelContext(DbContextHelper.GetDbContextOptions());
            string dateResult = null;
            try
            {
                var conn = (OracleConnection)context.Database.GetDbConnection();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sqlQuery;

                    context.Database.OpenConnection();

                    // Execute the SQL query and fetch the date result
                    var result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        dateResult = result.ToString();
                    }

                    context.Database.CloseConnection();
                }
            }
            catch (Exception)
            {
                context.Database.CloseConnection();
            }

            return dateResult;
        }
        public SuperSurpirseFormModel LoadSuperData(SuperSurpirseFormModel SuperSurpirseFormModel, string Case_No, string CallDt, string CallSNo)
        {
            var calldt = Convert.ToDateTime(CallDt).ToString("MM/dd/yyyy");
            using (var dbContext = context.Database.GetDbConnection())
            {
                OracleParameter[] par = new OracleParameter[5];
                par[0] = new OracleParameter("CASE_NO_PARAM", OracleDbType.NVarchar2, Case_No, ParameterDirection.Input);
                par[1] = new OracleParameter("CALL_DT_PARAM", OracleDbType.Date, calldt, ParameterDirection.Input);
                par[2] = new OracleParameter("CALL_SNO_PARAM", OracleDbType.NVarchar2, CallSNo, ParameterDirection.Input);
                par[3] = new OracleParameter("REGION_PARAM", OracleDbType.NVarchar2, SuperSurpirseFormModel.Regin, ParameterDirection.Input);
                par[4] = new OracleParameter("CUR_OUT", OracleDbType.RefCursor, ParameterDirection.Output);

                var ds = DataAccessDB.GetDataSet("Load_Super_FormData", par, 4);

                SuperSurpirseFormModel model = new();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    DataRow row = ds.Tables[0].Rows[0];
                    model = new SuperSurpirseFormModel
                    {
                        VEND_CD = Convert.ToString(row["VEND_CD"]),
                        MFG = Convert.ToString(row["MFG"]),
                        IE_NAME = Convert.ToString(row["IE_NAME"]),
                        IE_CD = Convert.ToString(row["IE_CD"]),
                        Case_No = Convert.ToString(row["Case_No"]),
                        CO_CD = Convert.ToString(row["PO_NO"]),
                        ContractNo = Convert.ToString(row["PO_NO"]),
                        Date = Convert.ToString(row["PO_DT"]),
                        SuperSurpriseCM = Convert.ToString(row["CO_CD"]),
                        CallDt = Convert.ToDateTime(row["call_recv_dt"]).ToString("dd/MM/yyyy"),
                        CallSNo = Convert.ToString(row["call_sno"]),
                        VenderNm = Convert.ToString(row["VEND_CD"]),
                        Vcode = Convert.ToString(row["vcode"]),
                        CallDtAndSrno = Convert.ToString(row["calldtandsrno"]),
                        SuperSurpriseDt = Convert.ToString(row["super_surprise_dt"]),
                        NameofScope = Convert.ToString(row["name_scope_item"]),
                        ItemDesc = Convert.ToString(row["item_desc"]),
                        PreviousInternal = Convert.ToString(row["pre_int_rej"]),
                        Discrepancy = Convert.ToString(row["discrepancy"]),
                        Overall = Convert.ToString(row["outcome"]),
                        SBUHead = Convert.ToString(row["sbu_head_remarks"]),
                    };
                }

                return model;
            }

        }
        public bool Save(SuperSurpirseFormModel SuperSurpirseFormModel)
        {
            var CallDt = Convert.ToDateTime(SuperSurpirseFormModel.CallDt).ToString("MM/dd/yyyy");
            var SuperDt = Convert.ToDateTime(SuperSurpirseFormModel.SuperSurpriseDt).ToString("MM/dd/yyyy");

            
            if (SuperSurpirseFormModel.SuperSurpriseNo == null && SuperSurpirseFormModel.SuperSurpriseDt != null)
            {

                string sup_sur_no = generate_super_surprise_id(SuperSurpirseFormModel);
                if (sup_sur_no == "-1")
                {
                    return false;
                }
                else
                {
                    try
                    {

                        OracleParameter[] par = new OracleParameter[15];
                        par[0] = new OracleParameter("p_SUPER_SURPRISE_NO", OracleDbType.Varchar2, sup_sur_no, ParameterDirection.Input);
                        par[1] = new OracleParameter("p_SUPER_SURPRISE_DT", OracleDbType.Date, SuperDt, ParameterDirection.Input);
                        par[2] = new OracleParameter("p_CASE_NO", OracleDbType.Varchar2, SuperSurpirseFormModel.Case_No, ParameterDirection.Input);
                        par[3] = new OracleParameter("p_CALL_RECV_DT", OracleDbType.Date, CallDt, ParameterDirection.Input);
                        par[4] = new OracleParameter("p_CALL_SNO", OracleDbType.Varchar2, SuperSurpirseFormModel.CallSNo, ParameterDirection.Input);
                        par[5] = new OracleParameter("p_IE_CD", OracleDbType.Varchar2, SuperSurpirseFormModel.IE_CD, ParameterDirection.Input);
                        par[6] = new OracleParameter("p_CO_CD", OracleDbType.Varchar2, SuperSurpirseFormModel.SuperSurpriseCM, ParameterDirection.Input);
                        par[7] = new OracleParameter("p_ITEM_DESC", OracleDbType.Varchar2, SuperSurpirseFormModel.ItemDesc, ParameterDirection.Input);
                        par[8] = new OracleParameter("p_DISCREPANCY", OracleDbType.Varchar2, SuperSurpirseFormModel.Discrepancy, ParameterDirection.Input);
                        par[9] = new OracleParameter("p_OUTCOME", OracleDbType.Varchar2, SuperSurpirseFormModel.Overall, ParameterDirection.Input);
                        par[10] = new OracleParameter("p_PRE_INT_REJ", OracleDbType.Varchar2, SuperSurpirseFormModel.PreviousInternal, ParameterDirection.Input);
                        par[11] = new OracleParameter("p_NAME_SCOPE_ITEM", OracleDbType.Varchar2, SuperSurpirseFormModel.NameofScope, ParameterDirection.Input);
                        par[12] = new OracleParameter("p_USER_ID", OracleDbType.Varchar2, SuperSurpirseFormModel.UserName, ParameterDirection.Input);
                        par[13] = new OracleParameter("p_DATETIME", OracleDbType.Date, DateTime.Now, ParameterDirection.Input);
                        par[14] = new OracleParameter("p_SBU_HEAD_REMARKS", OracleDbType.Varchar2, SuperSurpirseFormModel.SBUHead, ParameterDirection.Input);

                        var ds = DataAccessDB.ExecuteNonQuery("Insert_Super_Surprise", par, 1);
                    }

                    catch (Exception ex)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public string generate_super_surprise_id(SuperSurpirseFormModel SuperSurpirseFormModel)
        {
            var SuperDt = Convert.ToDateTime(SuperSurpirseFormModel.SuperSurpriseDt).ToString("MM/dd/yyyy");
            using (var dbContext = context.Database.GetDbConnection())
            {
                OracleParameter[] par = new OracleParameter[3];
                par[0] = new OracleParameter("IN_REGION_CD", OracleDbType.NVarchar2, SuperSurpirseFormModel.Regin, ParameterDirection.Input);
                par[1] = new OracleParameter("IN_SUPER_SURPRISE_DT", OracleDbType.Date, SuperDt, ParameterDirection.Input);
                par[2] = new OracleParameter("p_result_cursor", OracleDbType.RefCursor, ParameterDirection.Output);
                
                var ds = DataAccessDB.GetDataSet("GENERATE_SUPER_SURPRISE_NO3", par, 2);

                SuperSurpirseFormModel model = new();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    DataRow row = ds.Tables[0].Rows[0];
                    model = new SuperSurpirseFormModel
                    {
                        SuperSurpriseNo = Convert.ToString(row["SUPER_SURPRISE_NO"]),                       
                    };
                    
                }
                var SuperSurpriseNo = model.SuperSurpriseNo.Trim();
                return SuperSurpriseNo;
            }

        }
    }
}
