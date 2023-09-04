using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Globalization;

namespace IBS.Repositories
{
    public class SearchRepository : ISearchRepository
    {
        private readonly ModelContext context;

        public SearchRepository(ModelContext context)
        {
            this.context = context;
        }

        public List<BPOdata> GetBPOList(string BPONO)
        {
            var query = context.T12BillPayingOfficers
                        .Where(bpo =>
                            bpo.BpoCd.Trim().ToUpper() == BPONO.ToUpper() ||
                            bpo.BpoName.Trim().ToUpper().StartsWith(BPONO.ToUpper()))
                        .OrderBy(bpo => bpo.BpoName)
                        .Select(bpo => new BPOdata
                        {
                            BPO_CD = bpo.BpoCd,
                            BPO_NAME = $"{bpo.BpoCd}-{bpo.BpoName}/{bpo.BpoAdd}/{bpo.BpoRly}"
                        })
                        .ToList();

            return query;
        }

        public List<Consignee> GetConsigneeList(string Consignee)
        {
            var query = context.V06Consignees
                .Where(consignee =>
                    consignee.Consignee.Trim().ToUpper().StartsWith(Consignee))
                .OrderBy(consignee => consignee.Consignee)
                .Select(consignee => new Consignee
                {
                    CONSIGNEE_CD = consignee.ConsigneeCd,
                    CONSIGNEE = $"{consignee.ConsigneeCd}-{consignee.Consignee}"
                })
                .ToList();

            return query;
        }

        public DTResult<Search> GetSearchList(DTParameters dtParameters)
        {
            DTResult<Search> dTResult = new() { draw = 0 };
            IQueryable<Search>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "CaseNo";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                orderCriteria = "CaseNo";
                orderAscendingDirection = true;
            }

            string caseno = "", pono = "", podt = "", bkno = "", setno = "", calldt = "", billno = "", billdt = "", billamount = "", bpo = "", consignee = "", vendor = "", IENAME = "", callsno = "";
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["caseno"]))
            {
                caseno = Convert.ToString(dtParameters.AdditionalValues["caseno"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["pono"]))
            {
                pono = Convert.ToString(dtParameters.AdditionalValues["pono"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["podt"]))
            {
                podt = Convert.ToString(dtParameters.AdditionalValues["podt"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["bkno"]))
            {
                bkno = Convert.ToString(dtParameters.AdditionalValues["bkno"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["setno"]))
            {
                setno = Convert.ToString(dtParameters.AdditionalValues["setno"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["calldt"]))
            {
                calldt = Convert.ToString(dtParameters.AdditionalValues["calldt"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["billno"]))
            {
                billno = Convert.ToString(dtParameters.AdditionalValues["billno"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["billdt"]))
            {
                billdt = Convert.ToString(dtParameters.AdditionalValues["billdt"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["billamount"]))
            {
                billamount = Convert.ToString(dtParameters.AdditionalValues["billamount"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["bpo"]))
            {
                bpo = Convert.ToString(dtParameters.AdditionalValues["bpo"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["consignee"]))
            {
                consignee = Convert.ToString(dtParameters.AdditionalValues["consignee"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["vendor"]))
            {
                vendor = Convert.ToString(dtParameters.AdditionalValues["vendor"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["IENAME"]))
            {
                IENAME = Convert.ToString(dtParameters.AdditionalValues["IENAME"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["callsno"]))
            {
                callsno = Convert.ToString(dtParameters.AdditionalValues["callsno"]);
            }

            DataTable dt = new DataTable();
            DataSet ds;

            OracleParameter[] par = new OracleParameter[15];
            par[0] = new OracleParameter("p_case_no", OracleDbType.Varchar2, caseno, ParameterDirection.Input);
            par[1] = new OracleParameter("p_po_no", OracleDbType.Varchar2, pono, ParameterDirection.Input);
            par[2] = new OracleParameter("p_po_dt", OracleDbType.Varchar2, podt, ParameterDirection.Input);
            par[3] = new OracleParameter("p_consignee_cd", OracleDbType.Varchar2, consignee, ParameterDirection.Input);
            par[4] = new OracleParameter("p_bpo_cd", OracleDbType.Varchar2, bpo, ParameterDirection.Input);
            par[5] = new OracleParameter("p_vend_cd", OracleDbType.Varchar2, vendor, ParameterDirection.Input);
            par[6] = new OracleParameter("p_dt_of_rec", OracleDbType.Varchar2, calldt, ParameterDirection.Input);
            par[7] = new OracleParameter("p_call_sno", OracleDbType.Varchar2, callsno, ParameterDirection.Input);
            par[8] = new OracleParameter("p_ie_cd", OracleDbType.Varchar2, IENAME, ParameterDirection.Input);
            par[9] = new OracleParameter("p_bk_no", OracleDbType.Varchar2, bkno, ParameterDirection.Input);
            par[10] = new OracleParameter("p_set_no", OracleDbType.Varchar2, setno, ParameterDirection.Input);
            par[11] = new OracleParameter("p_bill_no", OracleDbType.Varchar2, billno, ParameterDirection.Input);
            par[12] = new OracleParameter("p_bill_dt", OracleDbType.Varchar2, billdt, ParameterDirection.Input);
            par[13] = new OracleParameter("p_bill_amt", OracleDbType.Varchar2, billamount, ParameterDirection.Input);
            par[14] = new OracleParameter("p_ResultSet", OracleDbType.RefCursor, ParameterDirection.Output);
            ds = DataAccessDB.GetDataSet("GetSearchAllData", par, 1);

            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];

                List<Search> list = dt.AsEnumerable().Select(row => new Search
                {
                    CaseNo = row.Field<string?>("CASE_NO"),
                    PONO = row.Field<string?>("PO_NO"),
                    PODT = row.Field<DateTime?>("PO_DT"),
                    Consignee = row.Field<string>("CONSIGNEE"),
                    BPO = row.Field<string?>("BPO_CD"),
                    Calldt = row.Field<DateTime?>("CALL_RECV_DT"),
                    CallSno = row.Field<string?>("CALL_SNO"),
                    BKNO = row.Field<string?>("BK_NO"),
                    SetNo = row.Field<string?>("SET_NO"),
                    BillNo = row.Field<string?>("BILL_NO"),
                    BillDt = row.Field<DateTime?>("BILL_DT"),
                    IEName = row.Field<string?>("IE_SNAME"),
                    BillAmount = row.Field<decimal?>("BILL_AMOUNT"),
                    InspFee = row.Field<decimal?>("INSP_FEE"),
                }).ToList();

                query = list.AsQueryable();

                dTResult.recordsTotal = ds.Tables[0].Rows.Count;

                if (!string.IsNullOrEmpty(searchBy))
                    query = query.Where(w => Convert.ToString(w.PONO).ToLower().Contains(searchBy.ToLower())
                    || Convert.ToString(w.CaseNo).ToLower().Contains(searchBy.ToLower())
                    );

                dTResult.recordsFiltered = query.Count();

                dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

                dTResult.draw = dtParameters.Draw;

            }
            return dTResult;
        }
    }

    public class BPOdata
    {
        public string BPO_CD { get; set; }
        public string BPO_NAME { get; set; }
    }

    public class Consignee
    {
        public int CONSIGNEE_CD { get; set; }
        public string CONSIGNEE { get; set; }
    }

}
