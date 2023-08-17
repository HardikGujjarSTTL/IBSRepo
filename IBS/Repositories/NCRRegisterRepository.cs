using Humanizer;
using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
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

                //var result = from t18 in context.T18CallDetails
                //             join t20 in context.T20Ics
                //             on new
                //             {
                //                 CaseNo = t18.CaseNo,
                //                 CallRecvDt = t18.CallRecvDt,
                //                 CallSno = t18.CallSno,
                //                 ConsigneeCd = t18.ConsigneeCd
                //             } equals new
                //             {
                //                 CaseNo = t20.CaseNo,
                //                 CallRecvDt = t20.CallRecvDt,
                //                 CallSno = t20.CallSno,
                //                 ConsigneeCd = t20.ConsigneeCd
                //             } into t20Joined
                //             from t20 in t20Joined.DefaultIfEmpty()
                //             join t20i in context.T20Ics on t18.CaseNo equals t20i.CaseNo
                //             where t20.CaseNo == "N18120364" && t20.BkNo == "3510" && t20.SetNo == "036"
                //             select new
                //             {
                //                 t18.ItemSrnoPo,
                //                 t20.ItemDescPo,
                //             };


                DataRow firstRow = dt.Rows[0]; // Get the first row of the DataTable
                DateTime callRecvDate;

                if (!firstRow.IsNull("CALL_RECV_DT"))
                {
                    if (DateTime.TryParseExact(firstRow["CALL_RECV_DT"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out callRecvDate))
                    {
                        model.CALL_RECV_DT = callRecvDate;
                    }
                }

                if (NCNO != "" && NCNO != null)
                {
                    model.QtyPassed = Convert.ToInt32(firstRow["QTY_PASSED"]);
                    model.Item = firstRow["ITEM_DESC_PO"].ToString();
                    if (!firstRow.IsNull("NC_DATE"))
                    {
                        model.NCRDate = Convert.ToDateTime(firstRow["NC_DATE"]);
                    }
                }
                else
                {
                    var qtyresult = context.T18CallDetails
                              .Where(cd =>
                                  cd.CaseNo == CASE_NO &&
                                  cd.CallRecvDt == DateTime.ParseExact(firstRow["CALL_RECV_DT"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture) &&
                                  cd.CallSno == Convert.ToInt32(firstRow["CALL_SNO"]))
                              .Select(cd => new
                              {
                                  QTY_PASSED = cd.QtyPassed ?? 0
                              })
                              .FirstOrDefault();

                    decimal qtyPassed = qtyresult?.QTY_PASSED ?? 0;

                    var Desc = from t18 in context.T18CallDetails
                                 join t20 in context.T20Ics on t18.CaseNo equals t20.CaseNo
                                 where t20.CaseNo == CASE_NO && t20.BkNo == BK_NO && t20.SetNo == SET_NO
                                 select new
                                 {
                                     t18.ItemSrnoPo,
                                     t18.ItemDescPo
                                 };
                    var descvalue = Desc.FirstOrDefault();

                    model.SetRegionCode = firstRow["REGION_CODE"].ToString();

                    model.QtyPassed = Convert.ToInt32(qtyPassed);
                    model.Item = descvalue.ItemDescPo;
                    model.Item_Srno_no = descvalue.ItemSrnoPo;
                    model.Ie_Cd = (byte?)(int)firstRow["IE_CD"];

                    model.NCRDate = DateTime.Now;
                }


                model.CaseNo = firstRow["case_no"].ToString();
                model.PO_NO = firstRow["po_no"].ToString();
                model.BKNo = firstRow["bk_no"].ToString();
                model.SetNo = firstRow["set_no"].ToString();
                model.CONSIGNEE = firstRow["CONSIGNEE"].ToString();
                model.CONSIGNEE_CD = Convert.ToInt32(firstRow["CONSIGNEE_CD"]);
                model.Vendor = firstRow["vendor"].ToString();
                model.VEND_CD = Convert.ToInt32(firstRow["VEND_CD"]);
                model.CALL_SNO = Convert.ToInt32(firstRow["CALL_SNO"]);

                // Parse CALL_RECV_DT if it's not null
               

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
        public string Saveupdate(NCRRegister model,bool isRadioChecked,string extractedText)
        {
            var now = DateTime.Now;

            string msg = "";

            DataTable dt = new DataTable();

            OracleParameter[] par = new OracleParameter[3];
            par[0] = new OracleParameter("IN_REGION_CD", OracleDbType.Varchar2, model.SetRegionCode, ParameterDirection.Input);
            par[1] = new OracleParameter("IN_NC_DT", OracleDbType.Varchar2, model.NCRDate, ParameterDirection.Input);
            par[2] = new OracleParameter("p_result_cursor", OracleDbType.RefCursor, ParameterDirection.Output);
            var ds = DataAccessDB.GetDataSet("GENERATE_NC_NO_New", par, 1);
            dt = ds.Tables[0];
            DataRow firstRow = dt.Rows[0];
            string ErrCD = firstRow["W_ERR_CD"].ToString();
            string genrate_NCNO = firstRow["W_NC_NO"].ToString();

            if (ErrCD == "-1")
            {
                msg = "NC Details not available";
            }
            else
            {
                var NCRMaster = (from r in context.T41NcMasters where r.NcNo == model.NC_NO select r).FirstOrDefault();
                if (NCRMaster == null)
                {
                    T41NcMaster obj = new T41NcMaster();
                    obj.NcNo = genrate_NCNO;
                    obj.NcDt = model.NCRDate;
                    obj.CaseNo = model.CaseNo;
                    obj.CallRecvDt = model.CALLRECVDT;
                    obj.CallSno = model.CALLSNO;
                    obj.BkNo = model.BKNo;
                    obj.SetNo = model.SetNo;
                    obj.VendCd = model.VEND_CD;
                    obj.ConsigneeCd = model.CONSIGNEE_CD;
                    obj.QtyPassed = model.QtyPassed;
                    obj.PoNo = model.PO_NO;
                    obj.PoDt = model.PO_DT;
                    obj.IcNo = model.IC_NO;
                    obj.IcDt = model.ICDate;
                    obj.IeCd = model.Ie_Cd;
                    obj.Datetime = DateTime.Now;
                    obj.ItemSrnoPo = model.Item_Srno_no;
                    obj.UserId = model.UserID;
                    obj.RegionCode = model.SetRegionCode;
                    context.T41NcMasters.Add(obj);
                    context.SaveChanges();
                    msg = "Record Save Successfully";
                }

                if (isRadioChecked == true)
                {
                    var NCDetail = (from r in context.T42NcDetails where r.NcNo == model.NC_NO select r).FirstOrDefault();
                    if (NCDetail == null)
                    {
                        T42NcDetail obj = new T42NcDetail();
                        obj.NcNo = genrate_NCNO;
                        obj.NcCd = "X01";
                        obj.NcCdSno = true;
                        obj.NcDescOthers = "";
                        obj.UserId = model.UserID;
                        obj.Datetime = DateTime.Now;
                        context.T42NcDetails.Add(obj);
                        context.SaveChanges();
                    }
                }
                else
                {
                    var NCDetail = (from r in context.T42NcDetails where r.NcNo == model.NC_NO select r).FirstOrDefault();
                    if (NCDetail == null)
                    {
                        T42NcDetail obj = new T42NcDetail();
                        obj.NcNo = genrate_NCNO;
                        obj.NcCd = model.NCRCode;
                        obj.NcCdSno = true;
                        obj.NcDescOthers = extractedText;
                        obj.UserId = model.UserID;
                        obj.Datetime = DateTime.Now;
                        context.T42NcDetails.Add(obj);
                        context.SaveChanges();
                    }
                }
            }      
           
            return msg;
        }

    }
}
