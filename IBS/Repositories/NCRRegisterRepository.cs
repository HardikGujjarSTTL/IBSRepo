using Humanizer;
using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Numerics;

namespace IBS.Repositories
{
    public class NCRRegisterRepository : INCRRegisterRepository
    {
        private readonly ModelContext context;

        public NCRRegisterRepository(ModelContext context)
        {
            this.context = context;
        }
        public DTResult<NCRRegister> GetDataList(DTParameters dtParameters)
        {
            DTResult<NCRRegister> dTResult = new() { draw = 0 };
            IQueryable<NCRRegister>? query = null;
            try
            {
                var searchBy = dtParameters.Search?.Value;
                var orderCriteria = string.Empty;
                var orderAscendingDirection = true;

                if (dtParameters.Order != null)
                {
                    // in this example we just default sort on the 1st column
                    orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                    if (orderCriteria == "")
                    {
                        orderCriteria = "NCNO";
                    }
                    orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
                }
                else
                {
                    orderCriteria = "NCNO";
                    orderAscendingDirection = true;
                }

                string NCNO = "", CASENO = "", ToDate = null, FromDate = null, IENAME = "";

                if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["NCNO"]))
                {
                    NCNO = Convert.ToString(dtParameters.AdditionalValues["NCNO"]);
                }
                if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["CASENO"]))
                {
                    CASENO = Convert.ToString(dtParameters.AdditionalValues["CASENO"]);
                }
                if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["selectedValue"]))
                {
                    IENAME = Convert.ToString(dtParameters.AdditionalValues["selectedValue"]);
                }
                if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["FromDate"]))
                {
                    FromDate = Convert.ToString(dtParameters.AdditionalValues["FromDate"]);
                }
                if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["ToDate"]))
                {
                    ToDate = Convert.ToString(dtParameters.AdditionalValues["ToDate"]);
                }

                NCRRegister model = new NCRRegister();
                DataTable dt = new DataTable();
                List<NCRRegister> modelList = new List<NCRRegister>();
                DataSet ds;

                    OracleParameter[] par = new OracleParameter[6];
                    par[0] = new OracleParameter("p_case_no", OracleDbType.Varchar2, CASENO, ParameterDirection.Input);
                    par[1] = new OracleParameter("p_nc_no", OracleDbType.Varchar2, NCNO, ParameterDirection.Input);
                    par[2] = new OracleParameter("p_lstIE", OracleDbType.Varchar2, IENAME, ParameterDirection.Input);
                    par[3] = new OracleParameter("p_frmDt", OracleDbType.Date, FromDate, ParameterDirection.Input);
                    par[4] = new OracleParameter("p_toDt", OracleDbType.Date, ToDate, ParameterDirection.Input);
                    par[5] = new OracleParameter("p_result_cursor", OracleDbType.RefCursor, ParameterDirection.Output);
                     ds = DataAccessDB.GetDataSet("GetFilterNCR", par, 1);
               

                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];

                    List<NCRRegister> list = dt.AsEnumerable().Select(row => new NCRRegister
                    {
                        CaseNo = row.Field<string>("CASE_NO"),
                        BKNo = row.Field<string>("BK_NO"),
                        SetNo = row.Field<string>("SET_NO"),
                        NC_NO = row.Field<string>("NC_NO"),
                        CALL_SNO = row.Field<int>("CALL_SNO"),
                        IE_SNAME = row.Field<string>("IE_SNAME"),
                        CALL_RECV_DT = DateTime.TryParseExact(row.Field<string>("CALL_RECV_DATE"), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime callRecvDate)
                ? callRecvDate
                : (DateTime?)null,
                        CONSIGNEE = row.Field<string>("CONSIGNEE"),
                    }).ToList();

                    query = list.AsQueryable();

                    dTResult.recordsTotal = ds.Tables[0].Rows.Count;

                    if (!string.IsNullOrEmpty(searchBy))
                        query = query.Where(w => Convert.ToString(w.CaseNo).ToLower().Contains(searchBy.ToLower())
                        || Convert.ToString(w.NC_NO).ToLower().Contains(searchBy.ToLower())
                        );

                    dTResult.recordsFiltered = query.Count();

                    dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

                    dTResult.draw = dtParameters.Draw;

                }
                else
                {
                    return dTResult;
                }

            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
           
            return dTResult;
        }

        public NCRRegister FindByIDActionA(string CASE_NO, string BK_NO, string SET_NO, string NCNO)
        {
            NCRRegister model = new NCRRegister();
            DataTable dt = new DataTable();
           
                if (NCNO != "" && NCNO != null)
                {
                    OracleParameter[] par = new OracleParameter[2];
                    par[0] = new OracleParameter("p_nc_no", OracleDbType.Varchar2, NCNO, ParameterDirection.Input);
                    par[1] = new OracleParameter("p_result_cursor", OracleDbType.RefCursor, ParameterDirection.Output);
                    var ds = DataAccessDB.GetDataSet("GetForAction_M_NCR", par, 1);
                    dt = ds.Tables[0];
                }
                else
                {
                    OracleParameter[] par = new OracleParameter[4];
                    par[0] = new OracleParameter("p_case_no", OracleDbType.Varchar2, CASE_NO, ParameterDirection.Input);
                    par[1] = new OracleParameter("p_bk_no", OracleDbType.Varchar2, BK_NO, ParameterDirection.Input);
                    par[2] = new OracleParameter("p_set_no", OracleDbType.Varchar2, SET_NO, ParameterDirection.Input);
                    par[3] = new OracleParameter("RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
                    var ds = DataAccessDB.GetDataSet("GetForAction_A_NCR", par, 1);
                    dt = ds.Tables[0];
                }

                if (dt != null)
                {

                    DataRow firstRow = dt.Rows[0]; // Get the first row of the DataTable

                    if (NCNO != "" && NCNO != null)
                    {
                        model.QtyPassed = Convert.ToInt32(firstRow["QTY_PASSED"]);
                        model.Item = firstRow["ITEM_DESC_PO"].ToString();
                        if (!firstRow.IsNull("NC_DATE"))
                        {
                            model.NCRDate = Convert.ToDateTime(firstRow["NC_DATE"]);
                        }
                    }
                    model.CaseNo = firstRow["case_no"].ToString();
                    model.PO_NO = firstRow["po_no"].ToString();
                    model.BKNo = firstRow["bk_no"].ToString();
                    model.SetNo = firstRow["set_no"].ToString();
                    model.CONSIGNEE = firstRow["CONSIGNEE"].ToString();
                    model.CONSIGNEE_CD = Convert.ToInt32(firstRow["CONSIGNEE_CD"]);
                    model.Vendor = firstRow["vendor"].ToString();
                    model.CALL_SNO = Convert.ToInt32(firstRow["CALL_SNO"]);

                    // Parse CALL_RECV_DT if it's not null
                    if (!firstRow.IsNull("CALL_RECV_DT"))
                    {
                        DateTime callRecvDate;
                        if (DateTime.TryParseExact(firstRow["CALL_RECV_DT"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out callRecvDate))
                        {
                            model.CALL_RECV_DT = callRecvDate;
                        }
                    }

                    model.IeCd = Convert.ToString(firstRow["IE_CD"]);
                    model.IE_SNAME = firstRow["IE_NAME"].ToString();

                    // Parse IC_DATE if it's not null
                    if (!firstRow.IsNull("IC_DATE"))
                    {
                        model.ICDate = Convert.ToDateTime(firstRow["IC_DATE"]);
                    }

                    model.IC_NO = firstRow["IC_NO"].ToString();

                    // Parse PO_DT if it's not null
                    if (!firstRow.IsNull("PO_DT"))
                    {
                        model.PO_DT = Convert.ToDateTime(firstRow["PO_DT"]);
                    }

                }

                var query = from t69 in context.T69NcCodes
                            join t42 in context.T42NcDetails on t69.NcCd equals t42.NcCd
                            join t41 in context.T41NcMasters on t42.NcNo equals t41.NcNo
                            where t41.NcNo == NCNO
                            orderby t42.NcCdSno ascending
                            select new
                            {
                                t42.NcCd,
                                NC_DESC = t42.NcDescOthers != null
                                    ? t69.NcDesc + "-" + t42.NcDescOthers
                                    : t69.NcDesc,
                                t42.NcCdSno,
                                t42.IeAction1,
                                IE_ACTION1_DT = t42.IeAction1Dt,
                                t42.CoFinalRemarks1,
                                CO_FINAL_REMARKS1_DT = t42.CoFinalRemarks1Dt
                            };

                var result = query.ToList();
                
            var jsonData = JsonConvert.SerializeObject(result);
            return new NCRRegister
            {
                Model = model,
                JsonData = jsonData
            };
        }

        public int SaveRemarks(NCRRegister model)
        {
            int i = 1;
            var now = DateTime.Now;

            var ncDetail = context.T42NcDetails
                .FirstOrDefault(d => d.NcNo == model.NC_NO && d.NcCd == model.NCRCode);

            if (ncDetail != null)
            {
                ncDetail.CoFinalRemarks1 = model.CoFinalRemarks1;
                ncDetail.CoFinalRemarks1Dt = now;
                ncDetail.Datetime = now;

                context.SaveChanges();
            }

            return i;
        }
        public int Saveupdate(NCRRegister model)
        {
            int i = 0;
            var now = DateTime.Now;

            var NCRMaster = (from r in context.T41NcMasters where r.NcNo == model.NC_NO select r).FirstOrDefault();
            if (NCRMaster == null)
            {
                NCRRegister obj = new NCRRegister();
                obj.NC_NO = model.NC_NO;
                obj.NCRDate = model.NCRDate;
                obj.CaseNo = model.CaseNo;
                obj.CALL_RECV_DT = model.CALL_RECV_DT;
                obj.BKNo = model.BKNo;
                obj.SetNo = model.SetNo;
                obj.VEND_CD = model.VEND_CD;
                obj.CONSIGNEE_CD = model.CONSIGNEE_CD;
                obj.QtyPassed = model.QtyPassed;
                obj.PO_NO = model.PO_NO;
                obj.PO_DT = model.PO_DT;
                obj.IC_NO = model.IC_NO;
                obj.IC_DT = model.IC_DT;
                obj.IeCd = model.IeCd;
                obj.Date = DateTime.Now;
                obj.UserID = model.UserID;
                obj.RegionCode = model.RegionCode;
                //context.T41NcMasters.Add(obj);
                context.SaveChanges();
                i = Convert.ToInt32(obj.NC_NO);
            }
            else
            {
               NCRMaster.NcNo = model.NC_NO;
               NCRMaster.NcDt = model.NCRDate;
               NCRMaster.CaseNo = model.CaseNo;
               //NCRMaster.CallRecvDt = model.CALL_RECV_DT;
               NCRMaster.BkNo = model.BKNo;
               NCRMaster.SetNo = model.SetNo;
               NCRMaster.VendCd = model.VEND_CD;
               NCRMaster.ConsigneeCd = model.CONSIGNEE_CD;
               NCRMaster.QtyPassed = model.QtyPassed;
               NCRMaster.PoNo = model.PO_NO;
               NCRMaster.PoDt = model.PO_DT;
               NCRMaster.IcNo = model.IC_NO;
               NCRMaster.IcDt = model.IC_DT;
               //NCRMaster.IeCd = model.IeCd;
               NCRMaster.Datetime = DateTime.Now;
               NCRMaster.UserId = model.UserID;
               NCRMaster.RegionCode = model.RegionCode;
               context.SaveChanges();
               i = Convert.ToInt32(NCRMaster.NcNo);
            }
            return i;
        }
    }
}
