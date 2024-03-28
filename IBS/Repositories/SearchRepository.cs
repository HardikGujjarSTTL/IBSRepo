using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;

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
                    consignee.Consignee.Trim().ToUpper().StartsWith(Consignee.ToUpper()) && consignee.Status == null)
                .OrderBy(consignee => consignee.Consignee)
                .Select(consignee => new Consignee
                {
                    CONSIGNEE_CD = consignee.ConsigneeCd,
                    CONSIGNEE = $"{consignee.ConsigneeCd}-{consignee.Consignee}"
                })
                .ToList();

            return query;
        }

        public List<VendorCls> GetVendorList(string Vendor)
        {
            var query = from v in context.T05Vendors
                        join c in context.T03Cities on v.VendCityCd equals c.CityCd
                        where v.VendName != null &&
                              v.VendName.Trim().ToUpper().StartsWith(Vendor.ToUpper())
                        orderby v.VendName
                        select new VendorCls
                        {
                            VEND_CD = v.VendCd,
                            VEND_NAME = v.VendCd + "-" +
                                        v.VendName.Trim() + "/" +
                                        v.VendAdd1.Trim() + "/" +
                                        (c.Location != null ?
                                        c.Location.Trim() + " / " + c.City.Trim() :
                                        c.City.Trim())
                        };
            return query.ToList();
        }


        public DTResult<Search> GetSearchList(DTParameters dtParameters, string region)
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

            string caseno = null, pono = null, podt = null, bkno = null, setno = null, calldt = null, billno = null, billdt = null, billamount = null, bpo = null, consignee = null, vendor = null, IENAME = null, callsno = null;
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
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["bpocd"]))
            {
                bpo = Convert.ToString(dtParameters.AdditionalValues["bpocd"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["consigneecd"]))
            {
                consignee = Convert.ToString(dtParameters.AdditionalValues["consigneecd"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["vendcd"]))
            {
                vendor = Convert.ToString(dtParameters.AdditionalValues["vendcd"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["IENAME"]))
            {
                IENAME = Convert.ToString(dtParameters.AdditionalValues["IENAME"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["callsno"]))
            {
                callsno = Convert.ToString(dtParameters.AdditionalValues["callsno"]);
            }
            DateTime? calldt1 = !string.IsNullOrEmpty(dtParameters.AdditionalValues["calldt"]) ? Convert.ToDateTime(dtParameters.AdditionalValues["calldt"]) : null;
            DateTime? billdt1 = !string.IsNullOrEmpty(dtParameters.AdditionalValues["billdt"]) ? Convert.ToDateTime(dtParameters.AdditionalValues["billdt"]) : null;
            DataTable dt = new DataTable();
            DataSet ds;

            OracleParameter[] par = new OracleParameter[19];
            par[0] = new OracleParameter("p_case_no", OracleDbType.Varchar2, caseno, ParameterDirection.Input);
            par[1] = new OracleParameter("p_region", OracleDbType.Varchar2, region, ParameterDirection.Input);
            par[2] = new OracleParameter("p_po_no", OracleDbType.Varchar2, pono, ParameterDirection.Input);
            par[3] = new OracleParameter("p_po_dt", OracleDbType.Varchar2, podt, ParameterDirection.Input);
            par[4] = new OracleParameter("p_consignee_cd", OracleDbType.Varchar2, consignee, ParameterDirection.Input);
            par[5] = new OracleParameter("p_bpo_cd", OracleDbType.Varchar2, bpo, ParameterDirection.Input);
            par[6] = new OracleParameter("p_vend_cd", OracleDbType.Varchar2, vendor, ParameterDirection.Input);
            par[7] = new OracleParameter("p_call_dt", OracleDbType.Date, calldt1, ParameterDirection.Input);
            par[8] = new OracleParameter("p_call_sno", OracleDbType.Varchar2, callsno, ParameterDirection.Input);
            par[9] = new OracleParameter("p_ie_cd", OracleDbType.Varchar2, IENAME, ParameterDirection.Input);
            par[10] = new OracleParameter("p_bk_no", OracleDbType.Varchar2, bkno, ParameterDirection.Input);
            par[11] = new OracleParameter("p_set_no", OracleDbType.Varchar2, setno, ParameterDirection.Input);
            par[12] = new OracleParameter("p_bill_no", OracleDbType.Varchar2, billno, ParameterDirection.Input);
            par[13] = new OracleParameter("p_bill_dt", OracleDbType.Date, billdt1, ParameterDirection.Input);
            par[14] = new OracleParameter("p_bill_amt", OracleDbType.Int32, billamount, ParameterDirection.Input);
            par[15] = new OracleParameter("p_page_start", OracleDbType.Int32, dtParameters.Start + 1, ParameterDirection.Input);
            par[16] = new OracleParameter("p_page_end", OracleDbType.Int32, (dtParameters.Start + dtParameters.Length), ParameterDirection.Input);
            par[17] = new OracleParameter("p_ResultSet", OracleDbType.RefCursor, ParameterDirection.Output);
            par[18] = new OracleParameter("p_result_records", OracleDbType.RefCursor, ParameterDirection.Output);
            ds = DataAccessDB.GetDataSet("GetSearchAllData", par, 2);

            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
                List<Search> list = dt.AsEnumerable().Select(row => new Search
                {
                    CaseNo = row["CASE_NO"].ToString(),
                    PONO = row["PO_NO"].ToString(),
                    PODT = DateTime.TryParse(row["PO_DT"].ToString(), out var poDt) ? poDt : null,
                    Consignee = row["CONSIGNEE"].ToString(),
                    BPO = row["BPO_CD"] != DBNull.Value ? Convert.ToInt32(row["BPO_CD"]) : null,
                    Calldt = DateTime.TryParse(row["CALL_RECV_DT"].ToString(), out var calldt) ? calldt : null,
                    CallSno = row["CALL_SNO"] != DBNull.Value ? Convert.ToInt32(row["CALL_SNO"]) : null,
                    BKNO = row["BK_NO"].ToString(),
                    SetNo = row["SET_NO"].ToString(),
                    BillNo = row["BILL_NO"].ToString(),
                    BillDt = DateTime.TryParse(row["BILL_DT"].ToString(), out var billDt) ? billDt : null,
                    IEName = row["IE_SNAME"].ToString(),
                    BillAmount = row["BILL_AMOUNT"] != DBNull.Value ? Convert.ToDecimal(row["BILL_AMOUNT"]) : null,
                    InspFee = row["INSP_FEE"] != DBNull.Value ? Convert.ToDecimal(row["INSP_FEE"]) : null
                }).ToList();

                query = list.AsQueryable();

                int recordsTotal = 0;
                if (ds != null && ds.Tables[1].Rows.Count > 0)
                {
                    recordsTotal = Convert.ToInt32(ds.Tables[1].Rows[0]["total_records"]);
                }

                dTResult.recordsTotal = recordsTotal;
                dTResult.recordsFiltered = recordsTotal;
                dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Select(p => p).ToList();
                dTResult.draw = dtParameters.Draw;

                //dTResult.recordsTotal = ds.Tables[0].Rows.Count;
                //if (!string.IsNullOrEmpty(searchBy))
                //    query = query.Where(w => Convert.ToString(w.PONO).ToLower().Contains(searchBy.ToLower())
                //    || Convert.ToString(w.CaseNo).ToLower().Contains(searchBy.ToLower())
                //    );
                //dTResult.recordsFiltered = query.Count();
                //dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();
                //dTResult.draw = dtParameters.Draw;

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

    public class VendorCls
    {
        public int VEND_CD { get; set; }
        public string VEND_NAME { get; set; }
    }

}
