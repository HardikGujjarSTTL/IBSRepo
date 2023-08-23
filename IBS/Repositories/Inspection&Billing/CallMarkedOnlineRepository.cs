using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Interfaces.Inspection_Billing;
using IBS.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Linq;

namespace IBS.Repositories.Inspection_Billing
{
    public class CallMarkedOnlineRepository: ICallMarkedOnlineRepository
    {
        private readonly ModelContext context;
        private readonly IConfiguration configuration;
        public CallMarkedOnlineRepository(ModelContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }
        public DTResult<CallMarkedOnlineModel> Get_Call_Marked_Online(DTParameters dtParameters)
        {
            DTResult<CallMarkedOnlineModel> dTResult = new() { draw = 0 };
            IQueryable<CallMarkedOnlineModel> query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;


            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "CASE_NO";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
            }
            else
            {
                orderCriteria = "CASE_NO";
                orderAscendingDirection = true;
            }

            string Date = "";
            bool RDB1 = false, RDB2 = false, RDB3 = false;
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["Date"]))
            {
                Date = Convert.ToString(dtParameters.AdditionalValues["Date"]);                
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["Rdb1"]))
            {
                RDB1 = Convert.ToString(dtParameters.AdditionalValues["Rdb1"]) == "0" ? false : true;
                
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["Rdb2"]))
            {
                RDB2 = Convert.ToString(dtParameters.AdditionalValues["Rdb2"]) == "0" ? false : true;

            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["Rdb3"]))
            {
                RDB3 = Convert.ToString(dtParameters.AdditionalValues["Rdb3"]) == "0" ? false : true;
            }

            Date = Date.ToString() == "" ? string.Empty : Date.ToString();

            //OracleParameter[] par = new OracleParameter[4];
            //par[0] = new OracleParameter("P_FROMDATE", OracleDbType.Varchar2, FromDate, ParameterDirection.Input);
            //par[1] = new OracleParameter("P_TODATE", OracleDbType.Varchar2, ToDate, ParameterDirection.Input);
            //par[2] = new OracleParameter("P_IECD", OracleDbType.Varchar2, IE_CD, ParameterDirection.Input);
            //par[3] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
            //var ds = DataAccessDB.GetDataSet("SP_GET_PENDING_JI_CASES", par, 1);
            //DataTable dt = ds.Tables[0];
            //List<CallMarkedOnlineModel> list = dt.AsEnumerable().Select(row => new CallMarkedOnlineModel
            //{                
            //    CASE_NO = Convert.ToString(row["CASE_NO"]),
            //    CALL_RECV_DT = Convert.ToString(row["CALL_RECV_DT"]),
            //    CALL_INSTALL_NO = Convert.ToString(row["CALL_INSTALL_NO"]),
            //    CALL_SNO = Convert.ToString(row["CALL_SNO"]),
            //    DATE_TIME = Convert.ToString(row["DATE_TIME"]),
            //    CALL_STATUS = Convert.ToString(row["CALL_STATUS"]),
            //    CALL_LETTER_NO = Convert.ToString(row["CALL_LETTER_NO"]),
            //    REMARKS = Convert.ToString(row["REMARKS"]),                
            //    PO_NO = Convert.ToString(row["PO_NO"]),
            //    PO_DT = Convert.ToString(row["PO_DATE"]),
            //    VENDOR = Convert.ToString(row["VENDOR"]),
            //    IE_NAME = Convert.ToString(row["IE_NAME"])
            //}).ToList();
            DateTime CHQ_DATE = Convert.ToDateTime(DateTime.ParseExact(Date, "dd/MM/yyyy", null).ToString("dd/MM/yyyy"));
            query = (from c in context.T17CallRegisters
                        join p in context.T13PoMasters on c.CaseNo equals p.CaseNo into cpJoin
                        from cp in cpJoin.DefaultIfEmpty()
                        join i in context.T09Ies on c.IeCd equals i.IeCd into ciJoin
                        from ci in ciJoin.DefaultIfEmpty()
                        join v in context.T05Vendors on cp.VendCd equals v.VendCd into cvJoin
                        from cv in cvJoin.DefaultIfEmpty()
                        join t in context.T03Cities on cv.VendCityCd equals t.CityCd into ctJoin
                        from ct in ctJoin.DefaultIfEmpty()
                        join s in context.T21CallStatusCodes on c.CallStatus equals s.CallStatusCd into csJoin
                        from cs in csJoin.DefaultIfEmpty()
                        where cp.RegionCode == "N" && c.OnlineCall == "Y"
                              && ((RDB3 && c.CallRecvDt == CHQ_DATE)
                                  || (RDB1 && c.CallRecvDt == CHQ_DATE && (ci.IeCd != 0))
                                  || (RDB3 || RDB2) && ci.IeCd == null
                                  || (RDB2 && c.CallRecvDt <= CHQ_DATE))                        
                        select new CallMarkedOnlineModel
                        {
                            CASE_NO = cp.CaseNo,
                            CALL_RECV_DT = Convert.ToString(c.CallRecvDt),
                            CALL_INSTALL_NO = Convert.ToString(c.CallInstallNo),
                            CALL_SNO = Convert.ToString(c.CallSno),
                            //DATE_TIME = Convert.ToDateTime(c.Datetime).ToString("dd/MM/yyyy-HH:mm:ss"),
                            CALL_STATUS = (cs.CallStatusDesc + (c.CallCancelStatus == "N" ? " (Non Chargeable)" : c.CallCancelStatus == "C" ? " (Chargeable)" : "")) ?? cs.CallStatusDesc,
                            CALL_LETTER_NO = c.CallLetterNo,
                            REMARKS = c.Remarks,
                            PO_NO = cp.PoNo,
                            //PO_DT = Convert.ToDateTime(cp.PoDt).ToString("dd/MM/yyyy"),
                            VENDOR = cv.VendName,// + "(" + (ct.LOCATION ?? (ct.LOCATION + " : " + ct.CITY)) + ")",
                            IE_NAME = ci.IeName
                        }).AsQueryable();

            if (RDB1 || RDB3)
            {
                query = query.OrderBy(item => item.CASE_NO)
                             .ThenBy(item => item.CALL_RECV_DT)
                             .ThenBy(item => item.CALL_SNO);
            }
            else
            {
                query = query.OrderByDescending(item => item.CALL_RECV_DT)
                             .ThenBy(item => item.CALL_SNO);
            }

            //list = query.ToList();


            //query = list.AsQueryable();

            dTResult.recordsTotal = query.ToList().Count(); //ds.Tables[0].Rows.Count;
            dTResult.recordsFiltered = query.ToList().Count();  //ds.Tables[0].Rows.Count;
            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();
            dTResult.draw = dtParameters.Draw;
            return dTResult;
        }
    }
}
