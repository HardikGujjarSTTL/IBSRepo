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
    public class LabPaymentListRRepository : ILabPaymentListRepository
    {
        private readonly ModelContext context;

        public LabPaymentListRRepository(ModelContext context)
        {
            this.context = context;
        }
        public DTResult<LabPaymentListModel> GetLapPaymentList(DTParameters dtParameters,string Regin)
        {

            DTResult<LabPaymentListModel> dTResult = new() { draw = 0 };
            IQueryable<LabPaymentListModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "CaseNo";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "CaseNo";
                orderAscendingDirection = true;
            }

            OracleParameter[] par = new OracleParameter[2];
            par[0] = new OracleParameter("p_region", OracleDbType.NVarchar2, Regin, ParameterDirection.Input);
            par[1] = new OracleParameter("p_ref_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("LabPaymentList", par, 1);

            List<LabPaymentListModel> modelList = new List<LabPaymentListModel>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    LabPaymentListModel model = new LabPaymentListModel
                    {
                        CaseNo = Convert.ToString(row["case_no"]),
                        CallRecvDt = Convert.ToString(row["call_recv_dt"]),
                        CallSno = Convert.ToString(row["call_sno"]),
                        GrossTestingChargesLab = Convert.ToString(row["testing_charges_lab"]),
                        GrossVendor = Convert.ToString(row["testing_charges_vend"]),
                        NetTestingChargesVend = Convert.ToString(row["GROSS_VENDOR"]),
                        TDS = Convert.ToString(row["TDS"]),
                        PaymentRecUpload = Convert.ToString(row["RE_DOC"]),
                        DocStatusFin = Convert.ToString(row["doc_status_fin"]),
                        Vendor = Convert.ToString(row["VENDOR"]),
                        Mfg = Convert.ToString(row["MFG"]),

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
                query = query.Where(w => Convert.ToString(w.CallSno).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.CaseNo).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;

            //using (var dbContext = context.Database.GetDbConnection())
            //{
                
            //}

            //return dTResult;
        }
        public LabPaymentListModel LoadPayment(string CaseNo, string CallSno, string CallRecvDt, string Regin)
        {

            using (var dbContext = context.Database.GetDbConnection())
            {
                OracleParameter[] par = new OracleParameter[5];
                par[0] = new OracleParameter("p_Region", OracleDbType.NVarchar2, Regin, ParameterDirection.Input);
                par[1] = new OracleParameter("p_CaseNo", OracleDbType.NVarchar2, CaseNo, ParameterDirection.Input);
                par[2] = new OracleParameter("p_CallRecvDt", OracleDbType.Date, CallRecvDt, ParameterDirection.Input);
                par[3] = new OracleParameter("p_CallSno", OracleDbType.NVarchar2, CallSno, ParameterDirection.Input);
                par[4] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);

                var ds = DataAccessDB.GetDataSet("LoadLabPayment", par, 4);

                LabPaymentListModel model = new();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    DataRow row = ds.Tables[0].Rows[0];
                    model = new LabPaymentListModel
                    {
                        GrossTestingChargesLab = row["testing_charges_lab"] as string, // Replace "Property1" with the actual column name in the table
                        NetTestingChargesVend = Convert.ToString(row["testing_charges_vend"]),
                        TDS = Convert.ToString(row["TDS"]),
                        CaseNo = CaseNo,
                        CallRecvDt = CallRecvDt,
                        CallSno = CallSno,
                        GrossVendor = Convert.ToString(row["TOTAL_CHARGES"]),
                        UTRNO = Convert.ToString(row["UTR_NO"]),
                        UTRDT = Convert.ToString(row["UTR_DATE"]),
                        PaymentRecUpload = Convert.ToString(row["doc_status_fin"]),
                        //DocStatusFin = Convert.ToString(row["status"]),
                        //Remarks = Convert.ToString(row["remarks"]),
                    };
                }

                return model;
            }
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
        public bool SaveData(LabPaymentListModel LabPaymentListModel)
        {

            string ss;
            string sqlQuery = "Select to_char(sysdate,'mm/dd/yyyy') from dual";

            ss = GetDateString(sqlQuery);
            try
            {

                OracleParameter[] par = new OracleParameter[7];
                par[0] = new OracleParameter("p_CaseNo", OracleDbType.Varchar2, LabPaymentListModel.CaseNo, ParameterDirection.Input);
                par[1] = new OracleParameter("p_CallSno", OracleDbType.Varchar2, LabPaymentListModel.CallSno, ParameterDirection.Input);
                par[2] = new OracleParameter("p_CallRecvDt", OracleDbType.Date, LabPaymentListModel.CallRecvDt, ParameterDirection.Input);
                par[3] = new OracleParameter("p_DLStatus", OracleDbType.Varchar2, LabPaymentListModel.DocStatusFin, ParameterDirection.Input);
                par[4] = new OracleParameter("p_DocRejRemark", OracleDbType.Varchar2, LabPaymentListModel.Remarks, ParameterDirection.Input);
                par[5] = new OracleParameter("p_Username", OracleDbType.Varchar2, LabPaymentListModel.UName, ParameterDirection.Input);
                par[6] = new OracleParameter("p_SS1", OracleDbType.Date, ss, ParameterDirection.Input);
                
                var ds = DataAccessDB.ExecuteNonQuery("UpdateLabPayment", par, 1);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

    }
}
