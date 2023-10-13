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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
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
            DateTime? _MaDt = MaDt == "" ? null : DateTime.ParseExact(MaDt, "dd/MM/yyyy", null);
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

        public VendorPOMAModel FindManageByID(string CaseNo, int UserName)
        {
            VendorPOMAModel model = new();
            var PODetails = context.T13PoMasters.Where(x => x.CaseNo == CaseNo).FirstOrDefault();
            if (PODetails == null)
                throw new Exception("Record Not found");
            else
            {
                if (PODetails != null)
                {
                    model.CASE_NO = PODetails.CaseNo;
                    model.PO_NO = PODetails.PoNo;
                    model.PO_DT = PODetails.PoDt;
                    model.RLY_CD = PODetails.RlyCd;
                    model.PO_OR_LETTER = PODetails.PoOrLetter;
                    model.RLY_NONRLY = PODetails.RlyNonrly;
                }
            }
            return model;
        }

        public int DetailsSave(VendorPOMAModel model, string CaseNo, string MaNo, string MaDt, string UserName)
        {
            //var GetValue = context.VendPoMaDetails.Find(CaseNo, CallRecvDt, CallSNo);
            int statusID = 0;

            //DateTime? _MaDt = MaDt == "" ? null : DateTime.ParseExact(MaDt, "dd/MM/yyyy", null);
            var MAcheckquery = context.VendPoMaMasters.Where(x => x.CaseNo == CaseNo && x.MaNo == MaNo && x.MaDt == model.MA_DT).FirstOrDefault();

            int maxRecordCount = context.VendPoMaDetails.Where(x => x.CaseNo == CaseNo && x.MaNo == MaNo && x.MaDt == model.MA_DT).Count();

            if (MAcheckquery != null)
            {
                statusID = 1;
            }
            else
            {
                #region Vend PO MA Master
                if (MAcheckquery == null)
                {
                    VendPoMaMaster obj = new VendPoMaMaster();
                    obj.CaseNo = model.CASE_NO;
                    obj.MaNo = model.MA_NO;
                    obj.MaDt = Convert.ToDateTime(model.MA_DT);
                    obj.PoNo = model.PO_NO;
                    obj.PoDt = model.PO_DT;
                    obj.RlyNonrly = model.RLY_NONRLY;
                    obj.RlyCd = model.RLY_CD;
                    obj.PoOrLetter = model.PO_OR_LETTER;
                    obj.PoSrc = "V";
                    context.VendPoMaMasters.Add(obj);
                    context.SaveChanges();
                    //Id = Convert.ToInt32(status);

                    VendPoMaDetail objD = new VendPoMaDetail();
                    objD.CaseNo = model.CASE_NO;
                    objD.MaSno = Convert.ToByte(maxRecordCount + 1);
                    objD.MaNo = model.MA_NO;
                    objD.MaDt = Convert.ToDateTime(model.MA_DT);
                    objD.MaField = model.MA_FIELD;
                    objD.MaDesc = model.MA_DESC;
                    objD.OldPoValue = model.OLD_PO_VALUE;
                    objD.NewPoValue = model.NEW_PO_VALUE;
                    objD.UserId = UserName;
                    context.VendPoMaDetails.Add(objD);
                    context.SaveChanges();

                    statusID = Convert.ToInt32(2);
                }

                #endregion
            }

            return statusID;
        }

        public int DetailsUpdate(VendorPOMAModel model, string UserName)
        {
            int Id = 0;
            var GetValue = context.VendPoMaDetails.Where(x => x.CaseNo == model.CASE_NO && x.MaNo == model.MA_NO && x.MaDt == model.MA_DT && x.MaSno == Convert.ToByte(model.MA_SNO)).FirstOrDefault();
            #region Update
            if (GetValue != null)
            {
                GetValue.MaField = model.MA_FIELD;
                GetValue.MaDesc = model.MA_DESC;
                GetValue.OldPoValue = model.OLD_PO_VALUE;
                GetValue.NewPoValue = model.NEW_PO_VALUE;
                GetValue.MaStatus = "P";
                context.SaveChanges();
                Id = Convert.ToInt32(GetValue.MaSno);
            }
            #endregion

            return Id;
        }

        public int GetDocument(VendorPOMAModel model)
        {
            string UNo = model.CASE_NO + "_" + model.MA_NO;
            int GetValue = context.IbsAppdocuments.Where(x => x.Applicationid == UNo).Count();

            return GetValue;
        }

        public DTResult<VendorPOMAModel> FindMatchDetailModify(string CaseNo, string MaNo, string MaDt, string UserName)
        {
            DTResult<VendorPOMAModel> dTResult = new() { draw = 0 };
            IQueryable<VendorPOMAModel>? query = null;

            DateTime? _MaDt = MaDt == "" ? null : DateTime.ParseExact(MaDt, "dd/MM/yyyy", null);

            query = from c in context.VendPoMaMasters
                    where c.CaseNo == CaseNo && c.MaNo == MaNo && c.MaDt == _MaDt
                    select new VendorPOMAModel
                    {
                        PO_SRC = Convert.ToString(c.PoSrc),
                        VEND_CD_S = UserName,
                    };

            dTResult.data = query;
            return dTResult;
        }

        public VendorPOMAModel FindManageModifyByID(string CaseNo, string MaNo, string MaDt, string MaStatus, byte MaSNo, int UserName)
        {
            VendorPOMAModel model = new();
            DateTime? _MaDt = MaDt == "" ? null : DateTime.ParseExact(MaDt, "dd/MM/yyyy", null);

            var PODetails = context.ViewMaDetailSearches.Where(x => x.VendCd == UserName && x.CaseNo == CaseNo && x.MaNo == MaNo && x.MaDt == _MaDt && x.MaSno == MaSNo).FirstOrDefault();
            if (PODetails == null)
                throw new Exception("Record Not found");
            else
            {
                if (PODetails != null)
                {
                    model.CASE_NO = PODetails.CaseNo;
                    model.PO_NO = PODetails.PoNo;
                    model.PO_DT = PODetails.PoDt;
                    model.RLY_CD = PODetails.RlyCd;
                    model.PO_OR_LETTER = PODetails.PoOrLetter;
                    model.MA_NO = PODetails.MaNo;
                    model.MA_DT = PODetails.MaDt;
                    model.RLY_NONRLY = PODetails.RlyNonrly;
                    model.MA_FIELD = PODetails.MaField;
                    model.MA_DESC = PODetails.MaDesc;
                    model.OLD_PO_VALUE = PODetails.OldPoValue;
                    model.NEW_PO_VALUE = PODetails.NewPoValue;
                    model.MA_REMARKS = PODetails.MaRemarks;
                    model.MA_SNO = Convert.ToString(PODetails.MaSno);
                }
            }
            return model;
        }

        public DTResult<VendorPOMAModel> GetSubDataList(DTParameters dtParameters, string UserName)
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
                    orderCriteria = "MA_NO";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
            }
            else
            {
                orderCriteria = "MA_NO";
                orderAscendingDirection = true;
            }

            string CaseNo = "", MaNo = "", MaDt = "";
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["CaseNo"]))
            {
                CaseNo = Convert.ToString(dtParameters.AdditionalValues["CaseNo"]);
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
            MaNo = MaNo.ToString() == "" ? string.Empty : MaNo.ToString();
            DateTime? _MaDt = MaDt == "" ? null : DateTime.ParseExact(MaDt, "dd/MM/yyyy", null);

            query = from l in context.ViewMaSubDetails
                    where l.UserId == UserName && l.CaseNo == CaseNo && l.MaNo == MaNo && l.MaDt == _MaDt && l.PoSrc == "V"
                    select new VendorPOMAModel
                    {
                        CASE_NO = l.CaseNo,
                        PO_NO = l.PoNo,
                        PO_DT = l.PoDt,
                        MA_NO = l.MaNo,
                        MA_DT = l.MaDt,
                        RLY_NONRLY = l.RlyNonrly,
                        RLY_CD = l.RlyCd,
                        PO_OR_LETTER = l.PoOrLetter,
                        MA_SNO = Convert.ToString(l.MaSno),
                        MA_STATUS = l.MaStatus,
                        PO_SRC = l.PoSrc,
                        MA_FIELD = l.MaField,
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.CASE_NO).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public VendorPOMAModel FindByReportID(string CaseNo, string UserName)
        {
            VendorPOMAModel model = new();

            var PODetails = context.ViewGetPoDetailsReports.Where(x => x.CaseNo == CaseNo).FirstOrDefault();
            if (PODetails == null)
                throw new Exception("Record Not found");
            else
            {
                if (PODetails != null)
                {
                    model.CASE_NO = PODetails.CaseNo;
                    model.PO_NO = PODetails.PoNo;
                    model.PO_DT = PODetails.PoDt;
                    model.RLY_CD = PODetails.RlyCd;
                    model.VENDOR = PODetails.Vendor;
                    model.PO_SOURCE = PODetails.PoSource;
                    model.IMMS_RLY_CD = PODetails.ImmsRlyCd;
                    model.REMARKS = PODetails.Remarks;
                    model.VEND_REMARKS = PODetails.VendRemarks;
                }
            }
            return model;
        }

        public DTResult<PODetailsModel> GetDataListPODetails(DTParameters dtParameters, string UserName)
        {
            DTResult<PODetailsModel> dTResult = new() { draw = 0 };
            IQueryable<PODetailsModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "" || orderCriteria == null)
                {
                    orderCriteria = "CaseNo";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
            }
            else
            {
                orderCriteria = "CaseNo";
                orderAscendingDirection = true;
            }

            string CaseNo = "";
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["CaseNo"]))
            {
                CaseNo = Convert.ToString(dtParameters.AdditionalValues["CaseNo"]);
            }

            CaseNo = CaseNo.ToString() == "" ? string.Empty : CaseNo.ToString();

            query = from p in context.ViewGetPoDetailsReportLists
                    where p.CaseNo == CaseNo
                    select new PODetailsModel
                    {
                        CaseNo = p.CaseNo,
                        ItemSrno = p.ItemSrno,
                        ItemDesc = p.ItemDesc,
                        Qty = p.Qty,
                        ExtDelvDt = p.ExtDelvDt,
                        Qty_Passed = p.Passed,
                        Qty_Rejected = p.Rejected,
                        Qty_Balance = p.Qty - p.Passed,
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.CaseNo).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public DTResult<POIREPSModel> GetDataListIREPS(DTParameters dtParameters, string UserName)
        {
            DTResult<POIREPSModel> dTResult = new() { draw = 0 };
            IQueryable<POIREPSModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "" || orderCriteria == null)
                {
                    orderCriteria = "RITES_CASE_NO";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
            }
            else
            {
                orderCriteria = "RITES_CASE_NO";
                orderAscendingDirection = true;
            }

            string CaseNo = "";
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["CaseNo"]))
            {
                CaseNo = Convert.ToString(dtParameters.AdditionalValues["CaseNo"]);
            }

            CaseNo = CaseNo.ToString() == "" ? string.Empty : CaseNo.ToString();

            query = from a in context.ImmsRitesPoHdrs
                    join b in context.MmpPomaHdrs on a.ImmsRlyCd equals b.Rly
                    join c in context.MmpPomaDtls on b.Rly equals c.Rly
                    where a.ImmsPokey == b.Pokey && b.Makey == c.Makey && a.RitesCaseNo == CaseNo
                    orderby c.Slno
                    select new POIREPSModel
                    {
                        RITES_CASE_NO = a.RitesCaseNo,
                        SLNO = c.Slno,
                        MAKEY = c.Makey,
                        MAKEY_DATE = b.MakeyDate,
                        MA_FLD_DESCR = c.MaFldDescr,
                        OLD_VALUE = c.OldValue,
                        NEW_VALUE = c.NewValue,
                        IMMS_RLY_CD = a.ImmsRlyCd,
                        IMMS_POKEY = a.ImmsPokey,
                        MA_NO = b.MaNo,
                        MA_DT = b.MaDate,
                        MA_STATUS = c.MaStatus == "A" ? "Approved" : c.MaStatus == "N" ? "Approved With No Change in IBS" : "Pending"
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.RITES_CASE_NO).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public DTResult<VendorPOMAModel> GetDataListPOVENDOR(DTParameters dtParameters, string UserName)
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
                    orderCriteria = "CASE_NO";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
            }
            else
            {
                orderCriteria = "CASE_NO";
                orderAscendingDirection = true;
            }

            string CaseNo = "";
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["CaseNo"]))
            {
                CaseNo = Convert.ToString(dtParameters.AdditionalValues["CaseNo"]);
            }

            CaseNo = CaseNo.ToString() == "" ? string.Empty : CaseNo.ToString();

            query = from m in context.VendPoMaMasters
                    join d in context.VendPoMaDetails on m.CaseNo equals d.CaseNo
                    where m.MaNo == d.MaNo && m.MaDt == d.MaDt && m.CaseNo == CaseNo && (d.MaStatus == "A" || d.MaStatus == "N")
                    orderby d.MaSno
                    select new VendorPOMAModel
                    {
                        CASE_NO = m.CaseNo,
                        MA_NO = m.MaNo,
                        MA_DT = m.MaDt,
                        MA_SNO = Convert.ToString(d.MaSno),
                        MA_FIELD = d.MaField,
                        MA_DESC = d.MaDesc,
                        OLD_PO_VALUE = d.OldPoValue,
                        NEW_PO_VALUE = d.NewPoValue,
                        MA_STATUS = d.MaStatus == "A" ? "Approved" : d.MaStatus == "N" ? "Approved With No Change in IBS" : "Pending"
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.CASE_NO).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public DTResult<PCallDetailsModel> GetDataListPCallDetails(DTParameters dtParameters, string UserName)
        {
            DTResult<PCallDetailsModel> dTResult = new() { draw = 0 };
            IQueryable<PCallDetailsModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "" || orderCriteria == null)
                {
                    orderCriteria = "CaseNo";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
            }
            else
            {
                orderCriteria = "CaseNo";
                orderAscendingDirection = true;
            }

            string CaseNo = "";
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["CaseNo"]))
            {
                CaseNo = Convert.ToString(dtParameters.AdditionalValues["CaseNo"]);
            }

            CaseNo = CaseNo.ToString() == "" ? string.Empty : CaseNo.ToString();

            query = from l in context.ViewGetPoDetailsReportPCalldetails
                    where l.CaseNo == CaseNo

                    select new PCallDetailsModel
                    {
                        CallRecvDt = l.CallRecvDt,
                        CallLetterDt = l.CallLetterDt,
                        CallSno = l.CallSno,
                        CallInstallNo = l.CallInstallNo,
                        IeName = l.IeName,
                        CallStatus = l.CallStatus,
                        ReasonReject = l.ReasonReject == "" || l.ReasonReject == null ? "" : l.ReasonReject,
                        Reason = l.Reason != "**" ? l.Reason : "",
                        CaseNo = l.CaseNo,
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.CaseNo).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public DTResult<CComplaintsModel> GetDataListCComplaints(DTParameters dtParameters, string UserName)
        {
            DTResult<CComplaintsModel> dTResult = new() { draw = 0 };
            IQueryable<CComplaintsModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "" || orderCriteria == null)
                {
                    orderCriteria = "BK_NO";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
            }
            else
            {
                orderCriteria = "BK_NO";
                orderAscendingDirection = true;
            }

            query = from c in context.V40ConsigneeComplaints
                    join s in context.T39JiStatusCodes on c.JiStatusCd equals s.JiStatusCd
                    where c.VendCd == Convert.ToInt32(UserName) && (c.JiRequired ?? "X") == "Y"
                    orderby c.RejMemoDt
                    select new CComplaintsModel
                    {
                        ITEM_DESC = c.ItemDesc,
                        REJ_MEMO_DT = c.RejMemoDt,
                        REJECTION_REASON = c.RejectionReason,
                        BK_NO = c.BkNo,
                        SET_NO = c.SetNo,
                        CONSIGNEE = c.ConsigneeName + "/" + c.ConsigneeAddr + "/" + c.ConsigneeCity,
                        JI_STATUS_DESC = s.JiStatusDesc,
                        BK_SET_NO = c.BkNo + "&" + c.SetNo,
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.BK_NO).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public DTResult<RVendorPlaceModel> GetDataListRVendorPlace(DTParameters dtParameters, string UserName)
        {
            DTResult<RVendorPlaceModel> dTResult = new() { draw = 0 };
            IQueryable<RVendorPlaceModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "" || orderCriteria == null)
                {
                    orderCriteria = "BkNo";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
            }
            else
            {
                orderCriteria = "BkNo";
                orderAscendingDirection = true;
            }

            string CaseNo = "";
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["CaseNo"]))
            {
                CaseNo = Convert.ToString(dtParameters.AdditionalValues["CaseNo"]);
            }

            CaseNo = CaseNo.ToString() == "" ? string.Empty : CaseNo.ToString();

            query = from p in context.ViewGetPoDetailsReportRVendorPlaces
                    where p.IcTypeId == "2" && p.VendCd == Convert.ToInt32(UserName) && p.CaseNo != CaseNo
                    select new RVendorPlaceModel
                    {
                        BillNo = p.BillNo,
                        IcDt = p.IcDt,
                        BkNo = p.BkNo,
                        SetNo = p.SetNo,
                        ReasonReject = p.ReasonReject,
                        IeName = p.IeName,
                        Vendor = p.Vendor,
                        ItemDescPo = p.ItemDescPo
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.BkNo).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }
    }
}
