using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Globalization;

namespace IBS.Repositories
{
    public class HologramAccountalRepository : IHologramAccountalRepository
    {
        private readonly ModelContext context;
        private readonly IConfiguration configuration;
        public HologramAccountalRepository(ModelContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }

        public DTResult<HologramAccountalModel> GetHologramAcountList(DTParameters dtParameters, string Region)
        {
            DTResult<HologramAccountalModel> dTResult = new() { draw = 0 };
            IQueryable<HologramAccountalModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;


            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
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

            string BK_NO = "", SET_NO = "", REGION = "";
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["BK_NO"]))
            {
                BK_NO = Convert.ToString(dtParameters.AdditionalValues["BK_NO"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["SET_NO"]))
            {
                SET_NO = Convert.ToString(dtParameters.AdditionalValues["SET_NO"]);
            }
            if (!string.IsNullOrEmpty(Region))
            {
                REGION = Region;
            }

            BK_NO = BK_NO.ToString() == "" ? string.Empty : BK_NO.ToString();
            SET_NO = SET_NO.ToString() == "" ? string.Empty : SET_NO.ToString();
            REGION = REGION.ToString() == "" ? string.Empty : REGION.ToString();

            OracleParameter[] par = new OracleParameter[4];
            par[0] = new OracleParameter("P_BK_NO", OracleDbType.Varchar2, BK_NO, ParameterDirection.Input);
            par[1] = new OracleParameter("P_SET_NO", OracleDbType.Varchar2, SET_NO, ParameterDirection.Input);
            par[2] = new OracleParameter("P_REGION", OracleDbType.Varchar2, REGION, ParameterDirection.Input);
            par[3] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("SP_GET_HOLOGRAM_ACCOUNTAL", par, 1);
            DataTable dt = ds.Tables[0];

            List<HologramAccountalModel> list = dt.AsEnumerable().Select(row => new HologramAccountalModel
            {
                CASE_NO = row["CASE_NO"].ToString(),
                BK_NO = row["BK_NO"].ToString(),
                SET_NO = row["SET_NO"].ToString(),
                HG_NO_MATERIAL = row["HG_NO_MATERIAL"].ToString(),
                HG_NO_SAMPLE = row["HG_NO_SAMPLE"].ToString(),
                HG_NO_TEST = row["HG_NO_TEST"].ToString(),
                HG_NO_IC = row["HG_NO_IC"].ToString(),
                HG_NO_IC_DOC = row["HG_NO_IC_DOC"].ToString(),
                HG_NO_OT = row["HG_NO_OT"].ToString()
            }).ToList();

            query = list.AsQueryable();

            dTResult.recordsTotal = ds.Tables[0].Rows.Count;
            dTResult.recordsFiltered = ds.Tables[0].Rows.Count;
            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();
            dTResult.draw = dtParameters.Draw;
            return dTResult;
        }

        public string GetRegionCode(string BK_NO, string SET_NO, string REGION)
        {
            //var region = context.T20Ics.Find();
            OracleParameter[] par = new OracleParameter[4];
            par[0] = new OracleParameter("P_BK_NO", OracleDbType.Varchar2, BK_NO, ParameterDirection.Input);
            par[1] = new OracleParameter("P_SET_NO", OracleDbType.Varchar2, SET_NO, ParameterDirection.Input);
            par[2] = new OracleParameter("P_REGION", OracleDbType.Varchar2, REGION, ParameterDirection.Input);
            par[3] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("SP_GET_REGION_CODE", par, 1);
            var sub = ds.Tables[0].Rows.Count == 0 ? "" : Convert.ToString(ds.Tables[0].Rows[0]["REGION_CODE"]);
            return sub;
        }

        public HologramAccountalModel GetHologramAccountalDetail(HologramAccountalFilter model)
        {
            var obj = new HologramAccountalModel();
            OracleParameter[] par = new OracleParameter[8];
            par[0] = new OracleParameter("P_CASE_NO", OracleDbType.Varchar2, model.CASE_NO, ParameterDirection.Input);
            par[1] = new OracleParameter("P_CALL_RECV_DT", OracleDbType.Varchar2, model.CALL_DT, ParameterDirection.Input);
            par[2] = new OracleParameter("P_BK_NO", OracleDbType.Varchar2, model.BK_NO, ParameterDirection.Input);
            par[3] = new OracleParameter("P_SET_NO", OracleDbType.Varchar2, model.SET_NO, ParameterDirection.Input);
            par[4] = new OracleParameter("P_CONSIGNEE_CD", OracleDbType.Varchar2, model.CONSIGNEE_CD, ParameterDirection.Input);
            par[5] = new OracleParameter("P_CALL_SNO", OracleDbType.Varchar2, model.CALL_SNO, ParameterDirection.Input);
            par[6] = new OracleParameter("P_REGION", OracleDbType.Varchar2, model.REGION, ParameterDirection.Input);
            par[7] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("SP_GET_HOLOGRAM_ACCOUNTAL_DETAIL", par, 1);

            obj.CASE_NO = Convert.ToString(ds.Tables[0].Rows[0]["CASE_NO"]);
            obj.CALL_DT = Convert.ToDateTime(ds.Tables[0].Rows[0]["CALL_DT"]).ToShortDateString().Replace('-', '/');
            obj.CALL_SNO = Convert.ToString(ds.Tables[0].Rows[0]["CALL_SNO"]);
            obj.BK_NO = Convert.ToString(ds.Tables[0].Rows[0]["BK_NO"]);
            obj.SET_NO = Convert.ToString(ds.Tables[0].Rows[0]["SET_NO"]);
            obj.CONSIGNEE_CD = Convert.ToString(ds.Tables[0].Rows[0]["CONSIGNEE_CD"]);
            obj.IE_NAME = Convert.ToString(ds.Tables[0].Rows[0]["IE_NAME"]);
            obj.IE_CD = Convert.ToString(ds.Tables[0].Rows[0]["IE_CD"]);
            return obj;
        }

        public DTResult<HologramAccountalModel> GetHologramAccountalDetails([FromBody] DTParameters dtParameters)
        {
            DTResult<HologramAccountalModel> dTResult = new() { draw = 0 };
            IQueryable<HologramAccountalModel>? query = null;

            string CASE_NO = "", CALL_DT = "", CONSIGNEE_CD = "", CALL_SNO = "";
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["CASE_NO"]))
            {
                CASE_NO = Convert.ToString(dtParameters.AdditionalValues["CASE_NO"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["CALL_DT"]))
            {
                CALL_DT = Convert.ToString(dtParameters.AdditionalValues["CALL_DT"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["CONSIGNEE_CD"]))
            {
                CONSIGNEE_CD = Convert.ToString(dtParameters.AdditionalValues["CONSIGNEE_CD"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["CALL_SNO"]))
            {
                CALL_SNO = Convert.ToString(dtParameters.AdditionalValues["CALL_SNO"]);
            }



            var obj = new HologramAccountalModel();
            OracleParameter[] par = new OracleParameter[5];
            par[0] = new OracleParameter("P_CASE_NO", OracleDbType.Varchar2, CASE_NO, ParameterDirection.Input);
            par[1] = new OracleParameter("P_CALL_RECV_DT", OracleDbType.Varchar2, CALL_DT, ParameterDirection.Input);
            par[2] = new OracleParameter("P_CONSIGNEE_CD", OracleDbType.Varchar2, CONSIGNEE_CD, ParameterDirection.Input);
            par[3] = new OracleParameter("P_CALL_SNO", OracleDbType.Varchar2, CALL_SNO, ParameterDirection.Input);
            par[4] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("SP_GET_HOLOGRAM_ACCOUNTAL_DETAILS", par, 1);
            DataTable dt = ds.Tables[0];

            List<HologramAccountalModel> list = dt.AsEnumerable().Select(row => new HologramAccountalModel
            {
                CASE_NO = row["CASE_NO"].ToString(),
                CONSIGNEE_CD = row["CONSIGNEE_CD"].ToString(),
                CALL_RECV_DT = row["CALL_RECV_DT"].ToString(),
                CALL_SNO = row["CALL_SNO"].ToString(),
                REC_NO = row["REC_NO"].ToString(),
                HG_NO_MATERIAL = row["HG_NO_MATERIAL"].ToString(),
                HG_NO_SAMPLE = row["HG_NO_SAMPLE"].ToString(),
                HG_NO_TEST = row["HG_NO_TEST"].ToString(),
                HG_NO_IC = row["HG_NO_IC"].ToString(),
                HG_NO_IC_DOC = row["HG_NO_IC_DOC"].ToString(),
                HG_NO_OT = row["HG_NO_OT"].ToString()
            }).ToList();

            query = list.AsQueryable();

            dTResult.recordsTotal = ds.Tables[0].Rows.Count;
            dTResult.recordsFiltered = ds.Tables[0].Rows.Count;
            //dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();
            dTResult.data = DbContextHelper.OrderByDynamic(query, "REC_NO", true).Select(p => p).ToList();
            dTResult.draw = dtParameters.Draw;
            return dTResult;
        }

        public HologramAccountalModel GetSelectedHologramAccountal(string CaseNo, string CallRecvDt, int CCD, int CallSNo, int RecNo)
        {
            string dt = Convert.ToDateTime(CallRecvDt).ToString("dd-MM-yy");
            DateTime parsedDate = DateTime.ParseExact(dt, "dd-MM-yy", CultureInfo.InvariantCulture);

            HologramAccountalModel query = null;
            query = (from c in context.T33HologramAccountals
                     where c.CaseNo == CaseNo && c.CallRecvDt == parsedDate && c.CallSno == CallSNo && c.ConsigneeCd == CCD && c.CallSno == Convert.ToInt16(CallSNo) && c.RecNo == Convert.ToByte(RecNo)
                     select new HologramAccountalModel
                     {
                         REC_NO = Convert.ToString(c.RecNo),
                         HG_NO_MATERIAL_FR = Convert.ToString(c.HgNoMaterialFr),
                         HG_NO_MATERIAL_TO = c.HgNoMaterialTo,
                         HG_NO_SAMPLE_FR = c.HgNoSampleFr,
                         HG_NO_SAMPLE_TO = c.HgNoSampleTo,
                         HG_NO_TEST_FR = c.HgNoTestFr,
                         HG_NO_TEST_TO = c.HgNoTestTo,
                         HG_NO_IC_FR = c.HgNoIcFr,
                         HG_NO_IC_TO = c.HgNoIcTo,
                         HG_NO_IC_DOC = c.HgNoIcDoc,
                         HG_OT_DESC = c.HgOtDesc,
                         HG_NO_OT_FR = c.HgNoOtFr,
                         HG_NO_OT_TO = c.HgNoOtTo
                     }).FirstOrDefault();

            return query;
        }

        public string CheckDuplicateHologram(HologramAccountalModel model)
        {
            var msg = string.Empty;

            OracleParameter[] par = new OracleParameter[15];
            par[0] = new OracleParameter("P_HG_NO_MATERIAL_FR", OracleDbType.Varchar2, model.HG_NO_MATERIAL_FR, ParameterDirection.Input);
            par[1] = new OracleParameter("P_HG_NO_MATERIAL_TO", OracleDbType.Varchar2, model.HG_NO_MATERIAL_TO, ParameterDirection.Input);
            par[2] = new OracleParameter("P_HG_NO_SAMPLE_FR", OracleDbType.Varchar2, model.HG_NO_SAMPLE_FR, ParameterDirection.Input);
            par[3] = new OracleParameter("P_HG_NO_SAMPLE_TO", OracleDbType.Varchar2, model.HG_NO_SAMPLE_TO, ParameterDirection.Input);
            par[4] = new OracleParameter("P_HG_NO_TEST_FR", OracleDbType.Varchar2, model.HG_NO_TEST_FR, ParameterDirection.Input);
            par[5] = new OracleParameter("P_HG_NO_TEST_TO", OracleDbType.Varchar2, model.HG_NO_TEST_TO, ParameterDirection.Input);
            par[6] = new OracleParameter("P_HG_NO_IC_FR", OracleDbType.Varchar2, model.HG_NO_IC_FR, ParameterDirection.Input);
            par[7] = new OracleParameter("P_HG_NO_IC_TO", OracleDbType.Varchar2, model.HG_NO_IC_TO, ParameterDirection.Input);
            par[8] = new OracleParameter("P_HG_NO_IC_DOC", OracleDbType.Varchar2, model.HG_NO_IC_DOC, ParameterDirection.Input);
            par[9] = new OracleParameter("P_HG_OT_DESC", OracleDbType.Varchar2, model.HG_OT_DESC, ParameterDirection.Input);
            par[10] = new OracleParameter("P_HG_NO_OT_FR", OracleDbType.Varchar2, model.HG_NO_OT_FR, ParameterDirection.Input);
            par[11] = new OracleParameter("P_HG_NO_OT_TO", OracleDbType.Varchar2, model.HG_NO_OT_TO, ParameterDirection.Input);
            par[12] = new OracleParameter("P_IE_CD", OracleDbType.Int32, Convert.ToInt32(model.IE_CD), ParameterDirection.Input);
            par[13] = new OracleParameter("P_REGION", OracleDbType.Varchar2, model.HG_REGION, ParameterDirection.Input);
            par[14] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("SP_CHECK_DUPLICATE_HOLOGRAM", par, 1);
            if (Convert.ToInt32(ds.Tables[0].Rows[0]["a"]) == 0 && model.HG_NO_MATERIAL_FR != "") { msg = msg + "- Material/Stores"; }
            if (Convert.ToInt32(ds.Tables[0].Rows[0]["b"]) == 0 && model.HG_NO_SAMPLE_FR != "") { msg = msg + "- Samples"; }
            if (Convert.ToInt32(ds.Tables[0].Rows[0]["c"]) == 0 && model.HG_NO_TEST_FR != "") { msg = msg + "- Test Request Slip"; }
            if (Convert.ToInt32(ds.Tables[0].Rows[0]["d"]) == 0 && model.HG_NO_IC_FR != "") { msg = msg + "- IC"; }
            if (Convert.ToInt32(ds.Tables[0].Rows[0]["e"]) == 0 && model.HG_NO_IC_DOC != "") { msg = msg + "- IC Documents"; }
            if (Convert.ToInt32(ds.Tables[0].Rows[0]["f"]) == 0 && model.HG_NO_OT_FR != "") { msg = msg + "- Other Locations"; }
            if (msg.Trim() == "") { return ("0"); }
            else { return ("Hologram Numbers entered for " + msg + " - are not issued to the IE!!!"); }
        }

        public int GetRecNo(string Case_No, string CallRecvDt, int CCD, int CallSNo)
        {
            string dt = Convert.ToDateTime(CallRecvDt).ToString("dd-MM-yy");
            DateTime parsedDate = DateTime.ParseExact(dt, "dd-MM-yy", CultureInfo.InvariantCulture);

            var recno = context.T33HologramAccountals
                        .Where(x => x.CaseNo == Case_No && x.CallRecvDt == parsedDate && x.CallSno == CallSNo && x.ConsigneeCd == CCD && x.CallSno == CallSNo)
                        .Select(x => Convert.ToInt32(x.RecNo)).Max() + 1;

            return recno;
        }

        public bool HologramInsertUpdate(HologramAccountalModel model, int RNO)
        {
            var flag = false;
            //var CallRecvDt = DateTime.ParseExact(model.CALL_DT, "dd/mm/yyyy", CultureInfo.InvariantCulture);
            //var currDate = DateTime.ParseExact(DateTime.Now.ToString(), "dd/mm/yyyy-HH24:MI:SS", CultureInfo.InvariantCulture);

            #region Hologram Accountal Detail Save 
            if (RNO == 0)
            {
                T33HologramAccountal obj = new T33HologramAccountal();
                obj.CaseNo = model.CASE_NO;
                obj.CallRecvDt = Convert.ToDateTime(model.CALL_DT);
                obj.ConsigneeCd = Convert.ToInt32(model.CONSIGNEE_CD);
                obj.CallSno = Convert.ToInt16(model.CALL_SNO);
                obj.RecNo = Convert.ToByte(model.REC_NO);
                obj.HgRegion = model.HG_REGION;
                obj.HgNoMaterialFr = model.HG_NO_MATERIAL_FR;
                obj.HgNoMaterialTo = model.HG_NO_MATERIAL_TO;
                obj.HgNoSampleFr = model.HG_NO_SAMPLE_FR;
                obj.HgNoSampleTo = model.HG_NO_SAMPLE_TO;
                obj.HgNoTestFr = model.HG_NO_TEST_FR;
                obj.HgNoTestTo = model.HG_NO_TEST_TO;
                obj.HgNoIcFr = model.HG_NO_IC_FR;
                obj.HgNoIcTo = model.HG_NO_IC_TO;
                obj.HgNoIcDoc = model.HG_NO_IC_DOC;
                obj.HgNoOtFr = model.HG_NO_OT_FR;
                obj.HgNoOtTo = model.HG_NO_OT_TO;
                obj.HgOtDesc = model.HG_OT_DESC;
                obj.UserId = model.USER_NAME;
                obj.Datetime = DateTime.Now;
                obj.Createdby = model.USER_ID;
                obj.Createddate = DateTime.Now;
                context.T33HologramAccountals.Add(obj);
                context.SaveChanges();
                flag = true;
                //ContractId = Convert.ToInt32(obj.ContractId);
            }
            else if (RNO > 0)
            {
                string dt = Convert.ToDateTime(model.CALL_DT).ToString("dd-MM-yy");
                DateTime callRecvDt = DateTime.ParseExact(dt, "dd-MM-yy", CultureInfo.InvariantCulture);

                var _data = context.T33HologramAccountals.Find(model.CASE_NO, callRecvDt, Convert.ToInt32(model.CONSIGNEE_CD), Convert.ToInt16(model.CALL_SNO), Convert.ToByte(model.REC_NO));
                _data.HgNoMaterialFr = model.HG_NO_MATERIAL_FR;
                _data.HgNoMaterialTo = model.HG_NO_MATERIAL_TO;
                _data.HgNoSampleFr = model.HG_NO_SAMPLE_FR;
                _data.HgNoSampleTo = model.HG_NO_SAMPLE_TO;
                _data.HgNoTestFr = model.HG_NO_TEST_FR;
                _data.HgNoTestTo = model.HG_NO_TEST_TO;
                _data.HgNoIcFr = model.HG_NO_IC_FR;
                _data.HgNoIcTo = model.HG_NO_IC_TO;
                _data.HgNoIcDoc = model.HG_NO_IC_DOC;
                _data.HgNoOtFr = model.HG_NO_OT_FR;
                _data.HgNoOtTo = model.HG_NO_OT_TO;
                _data.HgOtDesc = model.HG_OT_DESC;
                _data.UserId = model.USER_NAME;
                _data.Datetime = DateTime.Now;
                _data.Updatedby = model.USER_ID;
                _data.Updateddate = DateTime.Now;
                context.SaveChanges();
                flag = true;
            }
            #endregion

            return flag;
        }

        public bool HologramDelete(HologramAccountalModel model)
        {
            var flag = false;
            string dt = Convert.ToDateTime(model.CALL_DT).ToString("dd-MM-yy");
            DateTime callRecvDt = DateTime.ParseExact(dt, "dd-MM-yy", CultureInfo.InvariantCulture);

            var _data = context.T33HologramAccountals.Find(model.CASE_NO, callRecvDt, Convert.ToInt32(model.CONSIGNEE_CD), Convert.ToInt16(model.CALL_SNO), Convert.ToByte(model.REC_NO));
            if (_data != null)
            {
                _data.Isdeleted = 1;
                context.SaveChanges();
                flag = true;
            }
            return flag;
        }

    }
}
