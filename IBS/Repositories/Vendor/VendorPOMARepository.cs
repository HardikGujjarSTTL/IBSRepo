using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces.Vendor;
using IBS.Models;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Drawing;
using System.Dynamic;
using System.Globalization;
using System.Net.NetworkInformation;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace IBS.Repositories.Vendor
{
    public class VendorPOMARepository : IVendorPOMARepository
    {
        private readonly ModelContext context;

        public VendorPOMARepository(ModelContext context)
        {
            this.context = context;
        }

        public DTResult<VendorPOMAModel> GetDataList(DTParameters dtParameters, string UserName)
        {
            DTResult<VendorPOMAModel> dTResult = new() { draw = 0 };
            IQueryable<VendorPOMAModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "" || orderCriteria == null)
                {
                    orderCriteria = "PO_NO";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
            }
            else
            {
                orderCriteria = "PO_NO";
                orderAscendingDirection = true;
            }

            string CaseNo = "", PoNo = "", PoDt = "", MaNo = "", MaDt = "", MaStatus = "", MaSNo = "";
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["CaseNo"]))
            {
                CaseNo = Convert.ToString(dtParameters.AdditionalValues["CaseNo"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["PoNo"]))
            {
                PoNo = Convert.ToString(dtParameters.AdditionalValues["PoNo"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["PoDt"]))
            {
                PoDt = Convert.ToString(dtParameters.AdditionalValues["PoDt"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["MaNo"]))
            {
                MaNo = Convert.ToString(dtParameters.AdditionalValues["MaNo"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["MaDt"]))
            {
                MaDt = Convert.ToString(dtParameters.AdditionalValues["MaDt"]);
            }
            CaseNo = CaseNo.ToString() == "" ? string.Empty : CaseNo.ToString();
            PoNo = PoNo.ToString() == "" ? string.Empty : PoNo.ToString();
            DateTime? _PoDt = PoDt == "" ? null : DateTime.ParseExact(PoDt, "dd/MM/yyyy", null);
            MaNo = MaNo.ToString() == "" ? string.Empty : MaNo.ToString();
            DateTime? _MaDt = MaDt == "" ? null : DateTime.ParseExact(MaDt, "dd/MM/yyyy", null);

            MaStatus = MaStatus.ToString() == "" ? string.Empty : MaStatus.ToString();
            MaSNo = MaSNo.ToString() == "" ? string.Empty : MaSNo.ToString();

            OracleParameter[] par = new OracleParameter[9];
            par[0] = new OracleParameter("p_vend_cd", OracleDbType.Varchar2, UserName, ParameterDirection.Input);
            par[1] = new OracleParameter("p_cs_no", OracleDbType.Varchar2, CaseNo, ParameterDirection.Input);
            par[2] = new OracleParameter("p_po_no", OracleDbType.Varchar2, PoNo, ParameterDirection.Input);
            par[3] = new OracleParameter("p_po_date", OracleDbType.Date, _PoDt, ParameterDirection.Input);
            par[4] = new OracleParameter("p_ma_no", OracleDbType.Varchar2, MaNo, ParameterDirection.Input);
            par[5] = new OracleParameter("p_ma_dt", OracleDbType.Date, _MaDt, ParameterDirection.Input);
            par[6] = new OracleParameter("p_ma_status", OracleDbType.Varchar2, MaStatus, ParameterDirection.Input);
            par[7] = new OracleParameter("p_ma_s_no", OracleDbType.Varchar2, MaSNo, ParameterDirection.Input);
            par[8] = new OracleParameter("p_result_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("SP_GET_PO_MA_DATA", par, 1);
            DataTable dt = ds.Tables[0];


            VendorPOMAModel model = new();
            List<VendorPOMAModel> list = new();
            if (ds != null && ds.Tables.Count > 0)
            {
                //string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                //model = JsonConvert.DeserializeObject<List<BillRegisterModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }).FirstOrDefault();

                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                list = JsonConvert.DeserializeObject<List<VendorPOMAModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }

            query = list.AsQueryable();

            dTResult.recordsTotal = ds.Tables[0].Rows.Count;

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.PO_NO).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = ds.Tables[0].Rows.Count;

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public DTResult<VendorPOMAModel> FindMatchDetail(string CaseNo, string UserName)
        {
            DTResult<VendorPOMAModel> dTResult = new() { draw = 0 };
            IQueryable<VendorPOMAModel>? query = null;

            query = from c in context.T13PoMasters
                    where c.CaseNo == CaseNo
                    select new VendorPOMAModel
                    {
                        VEND_CD = Convert.ToString(c.VendCd),
                        VEND_CD_S = UserName,
                    };

            dTResult.data = query;
            return dTResult;
        }

        public VendorPOMAModel FindByID(string CaseNo, string MaNo, string MaDt, string MaStatus, string MaSNo, string UserName)
        {
            VendorPOMAModel model = new();
            DataTable dt = new DataTable();
            string PoNo = "", PoDt = "";
            if (MaStatus == "Pending")
            {
                MaStatus = "P";
            }
            else if (MaStatus == "Approved")
            {
                MaStatus = "A";
            }
            else if (MaStatus == "Return")
            {
                MaStatus = "R";
            }
            else if (MaStatus == "Approved With No Change")
            {
                MaStatus = "N";
            }
            else
            {
                MaStatus = "";
            }
            CaseNo = CaseNo.ToString() == "" ? string.Empty : CaseNo.ToString();
            MaNo = MaNo.ToString() == "" ? string.Empty : MaNo.ToString();
            DateTime? _MaDt = MaDt == "" ? null : DateTime.ParseExact(MaDt, "dd-MM-yyyy", null);
            MaStatus = MaStatus.ToString() == "" ? string.Empty : MaStatus.ToString();
            MaSNo = MaSNo.ToString() == "" ? string.Empty : MaSNo.ToString();

            PoNo = PoNo.ToString() == "" ? string.Empty : PoNo.ToString();
            DateTime? _PoDt = PoDt == "" ? null : DateTime.ParseExact(PoDt, "dd-MM-yyyy", null);

            OracleParameter[] par = new OracleParameter[9];
            par[0] = new OracleParameter("p_vend_cd", OracleDbType.Varchar2, UserName, ParameterDirection.Input);
            par[1] = new OracleParameter("p_cs_no", OracleDbType.Varchar2, CaseNo, ParameterDirection.Input);
            par[2] = new OracleParameter("p_po_no", OracleDbType.Varchar2, PoNo, ParameterDirection.Input);
            par[3] = new OracleParameter("p_po_date", OracleDbType.Date, _PoDt, ParameterDirection.Input);
            par[4] = new OracleParameter("p_ma_no", OracleDbType.Varchar2, MaNo, ParameterDirection.Input);
            par[5] = new OracleParameter("p_ma_dt", OracleDbType.Date, _MaDt, ParameterDirection.Input);
            par[6] = new OracleParameter("p_ma_status", OracleDbType.Varchar2, MaStatus, ParameterDirection.Input);
            par[7] = new OracleParameter("p_ma_s_no", OracleDbType.Varchar2, MaSNo, ParameterDirection.Input);
            par[8] = new OracleParameter("p_result_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("SP_GET_PO_MA_DATA", par, 1);
            if (ds != null)
            {
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    List<VendorPOMAModel> list = new();
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        //string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                        //model = JsonConvert.DeserializeObject<List<BillRegisterModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }).FirstOrDefault();

                        string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                        list = JsonConvert.DeserializeObject<List<VendorPOMAModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                    }
                    if (list == null)
                        throw new Exception("Record Not found");
                    else
                    {
                        model.CASE_NO = list[0].CASE_NO;
                        model.PO_DT = list[0].PO_DT;
                        model.PO_NO = list[0].PO_NO;
                        model.MA_NO = list[0].MA_NO;
                        model.MA_DT = list[0].MA_DT;
                    }
                }

            }
            return model;
        }
    }
}
