using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace IBS.Repositories
{
    public class IC_RPT_IntermediateRepository : IIC_RPT_IntermediateRepository
    {
        private readonly ModelContext context;
        public IC_RPT_IntermediateRepository(ModelContext context)
        {
            this.context = context;
        }

        public IC_RPT_IntermediateModel GetDetails(string Case_No, string Call_Recv_Dt, string Call_SNo, string ITEM_SRNO_PO, string CONSIGNEE_CD)
        {
            IC_RPT_IntermediateModel model = new();
            Call_Recv_Dt = Convert.ToDateTime(Call_Recv_Dt).ToString("dd/MM/yyyy");
            OracleParameter[] par = new OracleParameter[6];
            par[0] = new OracleParameter("p_case_no", OracleDbType.NVarchar2, Case_No, ParameterDirection.Input);
            par[1] = new OracleParameter("p_call_recv_dt", OracleDbType.NVarchar2, Call_Recv_Dt, ParameterDirection.Input);
            par[2] = new OracleParameter("p_call_sno", OracleDbType.NVarchar2, Call_SNo, ParameterDirection.Input);
            par[3] = new OracleParameter("p_item_srno_po", OracleDbType.NVarchar2, ITEM_SRNO_PO, ParameterDirection.Input);
            par[4] = new OracleParameter("p_consignee_cd", OracleDbType.NVarchar2, CONSIGNEE_CD, ParameterDirection.Input);
            par[5] = new OracleParameter("p_Result_Cursor", OracleDbType.RefCursor, ParameterDirection.Output);
            var ds = DataAccessDB.GetDataSet("sp_get_ic_rpt_intermediate", par, 1);
            DataTable dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                model = dt.AsEnumerable().Select(row => new IC_RPT_IntermediateModel
                {
                    CASE_NO = Convert.ToString(row["CASE_NO"]),
                    Call_SNO = Convert.ToString(row["Call_SNO"]),
                    Call_Recv_dt = Convert.ToDateTime(row["Call_Recv_dt"]),
                    Region_Code = Convert.ToString(row["Region_Code"]),
                    RLY_CD = Convert.ToString(row["RLY_CD"]),
                    Call_Install_No = Convert.ToString(row["Call_Install_No"]),
                    IE_Sname = Convert.ToString(row["IE_Sname"]),
                    Vend_Name = Convert.ToString(row["Vend_Name"]),
                    Vend_Add1 = Convert.ToString(row["Vend_Add1"]),
                    Vend_Add2 = Convert.ToString(row["Vend_Add2"]),
                    Vend_City = Convert.ToString(row["Vend_City"]),
                    MFG_Name = Convert.ToString(row["MFG_Name"]),
                    MFG_Add1 = Convert.ToString(row["MFG_Add1"]),
                    MFG_Add2 = Convert.ToString(row["MFG_Add2"]),
                    MFG_City = Convert.ToString(row["MFG_City"]),
                    MFG_PLACE = Convert.ToString(row["MFG_PLACE"]),
                    PO_NO = Convert.ToString(row["PO_NO"]),
                    PO_DT = Convert.ToString(row["PO_DT"]),
                    CONSIGNEE_DESIG = Convert.ToString(row["CONSIGNEE_DESIG"]),
                    CONSIGNEE_CD = Convert.ToString(row["CONSIGNEE_CD"]),
                    CONSIGNEE_CITYNAME = Convert.ToString(row["CONSIGNEE_CITYNAME"]),
                    CONSIGNEE_DEPT = Convert.ToString(row["CONSIGNEE_DEPT"]),
                    CONSIGNEE_FIRM = Convert.ToString(row["CONSIGNEE_FIRM"]),
                    PUR_DESIG = Convert.ToString(row["PUR_DESIG"]),
                    PUR_CD = Convert.ToString(row["PUR_CD"]),
                    PUR_DEPT = Convert.ToString(row["PUR_DEPT"]),
                    PUR_FIRM = Convert.ToString(row["PUR_FIRM"]),
                    PUR_City = Convert.ToString(row["PUR_City"]),
                    ITEM_SRNO_PO = Convert.ToString(row["ITEM_SRNO_PO"]),
                    ITEM_DESC_PO = Convert.ToString(row["ITEM_DESC_PO"]),
                    UOM_S_DESC = Convert.ToString(row["UOM_S_DESC"]),
                    QTY_ORDERED = Convert.ToString(row["QTY_ORDERED"]),
                    CUM_QTY_PREV_OFFERED = Convert.ToString(row["CUM_QTY_PREV_OFFERED"]),
                    CUM_QTY_PREV_PASSED = Convert.ToString(row["CUM_QTY_PREV_PASSED"]),
                    QTY_TO_INSP = Convert.ToString(row["QTY_TO_INSP"]),
                    QTY_PASSED = Convert.ToString(row["QTY_PASSED"]),
                    QTY_REJECTED = Convert.ToString(row["QTY_REJECTED"]),
                    QTY_DUE = Convert.ToString(row["QTY_DUE"]),
                    HOLOGRAM = Convert.ToString(row["HOLOGRAM"]),
                    NUM_VISITS = Convert.ToString(row["NUM_VISITS"]),
                    VISIT_DATES = Convert.ToString(row["VISIT_DATES"]),
                    BPO_NAME = Convert.ToString(row["BPO_NAME"]),
                    BPO_ORGN = Convert.ToString(row["BPO_ORGN"]),
                    City = Convert.ToString(row["City"]),
                    HOLOGRAMORG = Convert.ToString(row["HOLOGRAMORG"]),
                    REMARK = Convert.ToString(row["REMARK"]),
                    DT_INSP_DESIRE = Convert.ToString(row["DT_INSP_DESIRE"]),
                    ITEM_REMARK = Convert.ToString(row["ITEM_REMARK"]),
                    AMENDMENT_1 = Convert.ToString(row["AMENDMENT_1"]),
                    AMENDMENTDT_1 = Convert.ToString(row["AMENDMENTDT_1"]),
                    AMENDMENT_2 = Convert.ToString(row["AMENDMENT_2"]),
                    AMENDMENTDT_2 = Convert.ToString(row["AMENDMENTDT_2"]),
                    AMENDMENT_3 = Convert.ToString(row["AMENDMENT_3"]),
                    AMENDMENTDT_3 = Convert.ToString(row["AMENDMENTDT_3"]),
                    AMENDMENT_4 = Convert.ToString(row["AMENDMENT_4"]),
                    AMENDMENTDT_4 = Convert.ToString(row["AMENDMENTDT_4"]),
                    BK_NO = Convert.ToString(row["BK_NO"]),
                    SET_NO = Convert.ToString(row["SET_NO"]),
                    VISITS_DATES = Convert.ToString(row["VISITS_DATES"]),
                    //LAB_TST_RECT_DT = Convert.ToDateTime(row["LAB_TST_RECT_DT"]),
                    PASSED_INST_NO = Convert.ToString(row["PASSED_INST_NO"]),
                    CONSIGNEE_DTL = Convert.ToString(row["CONSIGNEE_DTL"]),
                    BPO_DTL = Convert.ToString(row["BPO_DTL"]),
                    PUR_DTL = Convert.ToString(row["PUR_DTL"]),
                    PUR_AUT_DTL = Convert.ToString(row["PUR_AUT_DTL"]),
                    OFF_INST_NO_DTL = Convert.ToString(row["OFF_INST_NO_DTL"]),
                    UNIT_DTL = Convert.ToString(row["UNIT_DTL"]),
                    DISPATCH_PACKING_NO = Convert.ToString(row["DISPATCH_PACKING_NO"]),
                    INVOICE_NO = Convert.ToString(row["INVOICE_NO"]),
                    NAME_OF_IE = Convert.ToString(row["NAME_OF_IE"]),
                    GOV_BILL_AUTH = Convert.ToString(row["GOV_BILL_AUTH"]),
                    MAN_TYPE = Convert.ToString(row["MAN_TYPE"]),
                    CONSIGNEE_DESG = Convert.ToString(row["CONSIGNEE_DESG"])
                }).FirstOrDefault();
            }


            if (!string.IsNullOrEmpty(model.CONSIGNEE_DTL)) { model.CONSIGNEE_DESC = model.CONSIGNEE_DTL; }
            else { model.CONSIGNEE_DESC = model.CONSIGNEE_DESIG + "/" + model.CONSIGNEE_DEPT + "/" + model.CONSIGNEE_FIRM + "/" + model.CONSIGNEE_CITYNAME; }

            if (!string.IsNullOrEmpty(model.BPO_DTL)) { model.BPO_DESC = model.BPO_DTL; }
            else { model.BPO_DESC = model.BPO_NAME + "/" + model.BPO_ORGN + "/" + model.City; }

            if (!string.IsNullOrEmpty(model.GOV_BILL_AUTH)) { model.GBPO_AUTH = model.GOV_BILL_AUTH; }
            else { model.GBPO_AUTH = model.BPO_NAME + "/" + model.BPO_ORGN + "/" + model.City; }

            if (!string.IsNullOrEmpty(model.PUR_DTL)) { model.MANUFAC_DESC = "M/s " + model.PUR_DTL; }
            else { model.MANUFAC_DESC = "M/s " + model.MFG_Name; }

            if (!string.IsNullOrEmpty(model.PUR_AUT_DTL)) { model.PUR_AUTH_DESC = model.PUR_AUT_DTL; }
            else { model.PUR_AUTH_DESC = model.PUR_DESIG + "/" + model.PUR_FIRM + "/" + model.PUR_City; }

            if (!string.IsNullOrEmpty(model.OFF_INST_NO_DTL)) { model.OFF_INST_NO = model.OFF_INST_NO_DTL; }
            else { model.OFF_INST_NO = model.Call_Install_No; }

            if (!string.IsNullOrEmpty(model.UNIT_DTL)) { model.ITEM_UNIT = model.UNIT_DTL; }
            else { model.ITEM_UNIT = model.UOM_S_DESC; }
            return model;
        }

        public IC_RPT_IntermediateModel AcceptedFun(string Case_No, string Call_Recv_Dt, string Call_SNo, string CONSIGNEE_CD)
        {
            IC_RPT_IntermediateModel model = new();

            OracleParameter[] par = new OracleParameter[4];
            par[0] = new OracleParameter("p_case_no", OracleDbType.NVarchar2, Case_No, ParameterDirection.Input);
            par[1] = new OracleParameter("p_call_recv_dt", OracleDbType.NVarchar2, Convert.ToDateTime(Call_Recv_Dt).ToString("dd/MM/yyyy"), ParameterDirection.Input);
            par[2] = new OracleParameter("p_call_sno", OracleDbType.NVarchar2, Call_SNo, ParameterDirection.Input);
            par[3] = new OracleParameter("p_result_cursor", OracleDbType.RefCursor, ParameterDirection.Output);
            var ds = DataAccessDB.GetDataSet("sp_get_acceptedfun", par, 1);
            DataTable dt = ds.Tables[0];

            model = dt.AsEnumerable().Select(row => new IC_RPT_IntermediateModel
            {
                Call_Recv_dt = Convert.ToDateTime(row["CALL_RECV_DT"]),
                PO_NO = Convert.ToString(row["PO_NO"]),
                PO_DT = Convert.ToString(row["PO_DATE"]),
                CASE_NO = Convert.ToString(row["CASE_NO"]),
                Call_SNO = Convert.ToString(row["Call_SNO"]),
                CALL_STATUS = Convert.ToString(row["CALL_STATUS"]),
                CALL_STATUS_DT = Convert.ToString(row["CALL_STATUS_DT"]),
            }).FirstOrDefault();

            if (model.CALL_STATUS == "A")
            {
                var query = Get_IcIntermediate(Case_No, Call_Recv_Dt, Call_SNo, CONSIGNEE_CD);

                if (query.Count() > 0)
                {
                    foreach (var row in query)
                    {
                        model.REMARK = row.REMARK;
                        model.HOLOGRAM = row.HOLOGRAM;
                        model.BK_NO = row.BK_NO;
                        model.SET_NO = row.SET_NO;
                        model.IE_STAMPS_DETAIL = row.IE_STAMPS_DETAIL;
                        model.IE_STAMPS_DETAIL2 = row.IE_STAMPS_DETAIL2;
                        model.LAB_TST_RECT_DT = string.IsNullOrEmpty(row.LAB_TST_RECT_DT) ? "" : Convert.ToDateTime(row.LAB_TST_RECT_DT).ToString("dd/MM/yyyy");
                        model.PASSED_INST_NO = row.PASSED_INST_NO;
                    }
                }
            }

            using ModelContext cont = new(DbContextHelper.GetDbContextOptions());
            using (var command = cont.Database.GetDbConnection().CreateCommand())
            {
                bool wasOpen = command.Connection.State == ConnectionState.Open;
                if (!wasOpen) command.Connection.Open();
                try
                {
                    command.CommandText = "Select to_char(sysdate,'dd/mm/yyyy') from dual";
                    model.CALL_STATUS_DT = Convert.ToString(command.ExecuteScalar());
                }
                finally
                {
                    if (!wasOpen) command.Connection.Close();
                }
            }
            return model;
        }

        public DTResult<PO_Amendments> GetPOAmendment(DTParameters dtParameters)
        {
            DTResult<PO_Amendments> dTResult = new() { draw = 0 };
            IQueryable<PO_Amendments>? query = null;

            //var searchBy = dtParameters.Search?.Value;
            //var orderCriteria = string.Empty;
            //var orderAscendingDirection = true;            

            var Case_No = "";

            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["Case_No"]))
            {
                Case_No = Convert.ToString(dtParameters.AdditionalValues["Case_No"]);
            }


            List<PO_Amendments> modelList = new List<PO_Amendments>();

            var AmendmentDetail = (from a in context.IcPoAmendments
                                   where a.CaseNo == Case_No
                                   select a.AmendmentDetail).FirstOrDefault();


            if (AmendmentDetail != null)
            {
                var arrAmd = AmendmentDetail.Split("#");
                for (int i = 0; i < arrAmd.Length; i++)
                {
                    var arrdetail = arrAmd[i].Split(";");

                    PO_Amendments obj = new PO_Amendments();
                    obj.Sno = i.ToString();
                    obj.Amendments = arrdetail[0];
                    obj.Date = arrdetail[1];
                    modelList.Add(obj);

                }
            }
            query = modelList.AsQueryable();

            dTResult.recordsTotal = query.Count();
            dTResult.recordsFiltered = query.Count();
            dTResult.data = modelList; //DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();
            dTResult.draw = dtParameters.Draw;
            return dTResult;
        }

        public int SetAccepted(string Case_No, string Call_Recv_Dt, string Call_SNo, string CONSIGNEE_CD)
        {
            Call_Recv_Dt = Convert.ToDateTime(Call_Recv_Dt).ToString("dd/MM/yyyy");
            var query = 0;
            query = (from ic in context.IcIntermediates
                     where ((ic.ConsgnCallStatus == "A" || ic.ConsgnCallStatus == "R")
                            && ic.CaseNo == Case_No
                             && ic.CallRecvDt == DateTime.ParseExact(Call_Recv_Dt, "dd/MM/yyyy", null)
                             && ic.CallSno == Convert.ToInt32(Call_SNo)
                             && ic.ConsigneeCd == Convert.ToInt32(CONSIGNEE_CD))
                     orderby ic.Datetime descending
                     select ic).Count();
            return query;
        }

        public string GetVisitsChanges(string Case_No, string Call_Recv_Dt, string Call_SNo, string VisitDate)
        {
            var VisitChange = VisitDate;
            //var txtVisitDate = "";
            var callRecvDt = DateTime.ParseExact(Convert.ToDateTime(Call_Recv_Dt).ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);

            var result = context.T47IeWorkPlans
                        .Where(item =>
                            item.CaseNo == Case_No &&
                            item.CallSno == Convert.ToInt32(Call_SNo) &&
                            item.CallRecvDt == callRecvDt)
                        .GroupBy(item => new { item.CaseNo, item.CallSno, item.CallRecvDt })
                        .Select(group => new
                        {
                            CASE_NO = group.Key.CaseNo,
                            CALL_SNO = group.Key.CallSno,
                            CALL_RECV_DT = group.Key.CallRecvDt,
                            NUM_VISITS = group.Count(),
                            VISIT_DATES = string.Join(", ", group.OrderBy(item => item.VisitDt).Select(item => item.VisitDt.ToString("dd.MM.yyyy")))
                        }).ToList();

            if (result.Count() > 0)
            {
                foreach (var item in result)
                {
                    VisitChange = item.VISIT_DATES;
                    VisitDate = SaveUpdateVisit(Case_No, Call_Recv_Dt, Call_SNo, VisitDate, VisitChange);
                }
            }
            else
            {
                VisitDate = SaveUpdateVisit(Case_No, Call_Recv_Dt, Call_SNo, VisitDate, VisitChange);
            }
            return VisitDate;
        }

        public IC_RPT_IntermediateModel FillItems(string Case_No, string Call_Recv_Dt, string Call_SNo, string CONSIGNEE_CD)
        {
            Call_Recv_Dt = Convert.ToDateTime(Call_Recv_Dt).ToString("dd/MM/yyyy");
            IC_RPT_IntermediateModel model = new();
            model = (from ic in context.IcIntermediates
                     where (ic.ConsigneeCd == Convert.ToInt32(CONSIGNEE_CD)
                             && ic.CaseNo == Case_No
                             && ic.CallRecvDt == DateTime.ParseExact(Call_Recv_Dt, "dd/MM/yyyy", null)
                             && ic.CallSno == Convert.ToInt32(Call_SNo))
                     orderby ic.Datetime descending
                     select new IC_RPT_IntermediateModel
                     {
                         BK_NO = ic.BkNo,
                         SET_NO = ic.SetNo,
                         IE_STAMPS_DETAIL = ic.IeStampsDetail,
                         IE_STAMPS_DETAIL2 = ic.IeStampsDetail2,
                         LAB_TST_RECT_DT = ic.LabTstRectDt == null ? "" : Convert.ToDateTime(ic.LabTstRectDt).ToString("dd/MM/yyyy"),
                         PASSED_INST_NO = ic.PassedInstNo,
                         REMARK = ic.Remark,
                         HOLOGRAM = ic.Hologram
                     }).FirstOrDefault();
            // Execute the query and retrieve the results
            //var results = query.ToList();
            return model;
        }

        public string SaveUpdateVisit(string Case_No, string Call_Recv_Dt, string Call_SNo, string VisitDate, string VisitChange)
        {
            Call_Recv_Dt = Convert.ToDateTime(Call_Recv_Dt).ToString("dd/MM/yyyy");

            var query = (from ic in context.IcIntermediates
                         where (ic.CaseNo == Case_No
                                 && ic.CallRecvDt == DateTime.ParseExact(Call_Recv_Dt, "dd/MM/yyyy", null)
                                 && ic.CallSno == Convert.ToInt32(Call_SNo)
                                 && ic.VisitsDates != null)
                         orderby ic.Datetime descending
                         select ic.VisitsDates).ToList();

            try
            {
                string sqlQuery = "";
                if (query.Count() > 0)
                {
                    if (!string.IsNullOrEmpty(VisitDate))
                    {
                        sqlQuery = "Update IC_INTERMEDIATE set NUM_VISITS = " + VisitDate.Trim().Split(',').Length + " , VISITS_DATES = '" + VisitDate.Trim() + "' where case_no = '" + Case_No + "' and CALL_SNO ='" + Call_SNo + "' AND CALL_RECV_DT = TO_date('" + Call_Recv_Dt + "', 'dd/mm/yyyy') ";
                    }
                    else
                    {
                        sqlQuery += "Update IC_INTERMEDIATE set VISITS_DATES = VISITS_DATES,NUM_VISITS=NUM_VISITS where case_no = '" + Case_No + "' and CALL_SNO ='" + Call_SNo + "' AND CALL_RECV_DT = TO_date('" + Call_Recv_Dt + "', 'dd/mm/yyyy') ";
                        VisitDate = query[0]; //Convert.ToString(ds.Tables[0].Rows[0]["VISITS_DATES"]);
                    }
                }
                else
                {
                    sqlQuery += "Update IC_INTERMEDIATE set NUM_VISITS = " + VisitChange.Split(',').Length + ", VISITS_DATES = '" + VisitChange + "' where case_no = '" + Case_No + "' and CALL_SNO ='" + Call_SNo + "' AND CALL_RECV_DT = TO_date('" + Call_Recv_Dt + "', 'dd/mm/yyyy') ";
                    VisitDate = VisitChange;
                }

                using ModelContext cont = new(DbContextHelper.GetDbContextOptions());
                using (var command = cont.Database.GetDbConnection().CreateCommand())
                {
                    var trans = cont.Database.BeginTransaction();
                    bool wasOpen = command.Connection.State == ConnectionState.Open;
                    if (!wasOpen) command.Connection.Open();
                    try
                    {
                        //command.Transaction = trans;
                        command.CommandText = sqlQuery;
                        var res = command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                    }
                    finally
                    {
                        trans.Commit();
                        if (!wasOpen) command.Connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return VisitDate;
        }

        public List<IC_RPT_IntermediateModel> Get_IcIntermediate(string Case_No, string Call_Recv_Dt, string Call_SNo, string CONSIGNEE_CD)
        {
            Call_Recv_Dt = Convert.ToDateTime(Call_Recv_Dt).ToString("dd/MM/yyyy");
            var query = (from ic in context.IcIntermediates
                         where (ic.ConsigneeCd == Convert.ToInt32(CONSIGNEE_CD)
                                 && ic.CaseNo == Case_No
                                 && ic.CallRecvDt == DateTime.ParseExact(Call_Recv_Dt, "dd/MM/yyyy", null)
                                 && ic.CallSno == Convert.ToInt32(Call_SNo))
                         orderby ic.Datetime descending
                         select new IC_RPT_IntermediateModel
                         {
                             BK_NO = ic.BkNo,
                             SET_NO = ic.SetNo,
                             IE_STAMPS_DETAIL = ic.IeStampsDetail,
                             IE_STAMPS_DETAIL2 = ic.IeStampsDetail2,
                             LAB_TST_RECT_DT = ic.LabTstRectDt == null ? "" : Convert.ToDateTime(ic.LabTstRectDt).ToString("dd/MM/yyyy"),
                             PASSED_INST_NO = ic.PassedInstNo,
                             REMARK = ic.Remark,
                             HOLOGRAM = ic.Hologram
                         }).ToList();
            return query;
        }

        public List<SelectListItem> GetItems(string Case_No, string Call_Recv_Dt, string Call_SNo, string CONSIGNEE_CD)
        {
            OracleParameter param = new OracleParameter();
            OracleParameter[] par = new OracleParameter[6];
            par[0] = new OracleParameter("P_CASE_NO", OracleDbType.NVarchar2, Case_No, ParameterDirection.Input);
            par[1] = new OracleParameter("P_CALL_RECV_DT", OracleDbType.NVarchar2, Convert.ToDateTime(Call_Recv_Dt).ToString("dd/MM/yyyy"), ParameterDirection.Input);
            par[2] = new OracleParameter("P_CALL_SNO", OracleDbType.NVarchar2, Call_SNo, ParameterDirection.Input);
            par[3] = new OracleParameter("P_CONSIGNEE_CD", OracleDbType.NVarchar2, CONSIGNEE_CD, ParameterDirection.Input);
            par[4] = new OracleParameter("P_DP_CONSIGNEE_CD", OracleDbType.NVarchar2, CONSIGNEE_CD, ParameterDirection.Input);
            par[5] = new OracleParameter("p_result_cursor", OracleDbType.RefCursor, ParameterDirection.Output);
            var ds = DataAccessDB.GetDataSet("SP_GET_IC_RPT_ITEMS", par, 1);

            DataTable dt = ds.Tables[0];

            List<SelectListItem> lst = dt.AsEnumerable().Select(row => new SelectListItem
            {
                Text = Convert.ToString(row["ITEM_SRNO_PO"]),
                Value = Convert.ToString(row["ITEM_SRNO_PO"])
            }).ToList();
            return lst;
        }

        public bool SaveDetail(IC_RPT_IntermediateModel model, UserSessionModel user)
        {
            DateTime callRecDt = DateTime.ParseExact(model.Display_Call_Recv_dt, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            #region Code Comment
            //using (var trans = context.Database.BeginTransaction())
            //{                
            //    try
            //    {
            //        if (user.Region != "C")
            //        {
            //            model.MAN_TYPE = "";
            //        }
            //        var _data = (from x in context.IcIntermediates
            //                     where x.ItemSrnoPo == Convert.ToByte(model.ITEM_SRNO_PO)
            //                     && x.CaseNo == model.CASE_NO
            //                     && x.BkNo == model.BK_NO
            //                     && x.SetNo == model.SET_NO
            //                     && x.CallRecvDt == callRecDt
            //                     && x.CallSno == Convert.ToInt16(model.Call_SNO)
            //                     && x.ConsigneeCd == Convert.ToInt32(model.DP_CONSIGNEE_CD)
            //                     select x).FirstOrDefault();                                        
            //        if (_data != null)
            //        {
            //            _data.ConsgnCallStatus = "Z";
            //            _data.ItemSrnoPo = Convert.ToByte(model.ITEM_SRNO_PO);
            //            _data.ItemDescPo = model.ITEM_DESC_PO;
            //            _data.ItemRemark = model.ITEM_REMARK;
            //            _data.QtyOrdered = Convert.ToDecimal(model.QTY_ORDERED);
            //            _data.CumQtyPrevOffered = Convert.ToDecimal(model.CUM_QTY_PREV_OFFERED);
            //            _data.CumQtyPrevPassed = Convert.ToDecimal(model.CUM_QTY_PREV_PASSED);
            //            _data.QtyToInsp = Convert.ToDecimal(model.QTY_TO_INSP);
            //            _data.QtyPassed = Convert.ToDecimal(model.QTY_PASSED);
            //            _data.QtyRejected = Convert.ToDecimal(model.QTY_REJECTED);
            //            _data.QtyDue = Convert.ToDecimal(model.QTY_DUE);
            //            _data.PoNo = model.PO_NO;
            //            _data.UnitDtl = model.UNIT_DTL;
            //            context.SaveChanges();
            //        }
            //        var _data1 = context.IcIntermediates
            //                    .Where(x => x.IeCd == Convert.ToInt32(user.IeCd))
            //                    .Select(x => x);
            //        if (_data1 != null)
            //        {
            //            foreach (var row in _data1)
            //            {
            //                row.IeStamp = "";
            //                row.IeStamp2 = "";
            //                row.IeStampsDetail = model.IE_STAMPS_DETAIL;
            //                row.IeStampsDetail2 = model.IE_STAMPS_DETAIL2;
            //                context.SaveChanges();
            //            }
            //        }

            //        var query = context.IcIntermediates
            //                    .Where(x => x.CaseNo == model.CASE_NO && x.BkNo == model.BK_NO && x.SetNo == model.SET_NO
            //                    && x.CallRecvDt == callRecDt && x.CallSno == Convert.ToInt32(model.Call_SNO))
            //                    .Select(x => x);
            //        if (query != null)
            //        {
            //            foreach (var row in query)
            //            {
            //                row.ReasonOfRejection = model.REMARK;
            //                row.PassedInstNo = model.PASSED_INST_NO;
            //                row.LabTstRectDt = string.IsNullOrEmpty(model.LAB_TST_RECT_DT) ? null : DateTime.ParseExact(model.LAB_TST_RECT_DT, "dd/MM/yyyy", null);
            //                row.Hologram = model.HOLOGRAM;
            //                row.Remark = model.REMARK;
            //                row.ManType = model.MAN_TYPE;
            //                row.DispatchPackingNo = model.DISPATCH_PACKING_NO;
            //                row.InvoiceNo = model.INVOICE_NO;
            //                row.NameOfIe = model.NAME_OF_IE;
            //                row.ConsigneeDesg = model.CONSIGNEE_DESG;

            //                row.ConsigneeDtl = model.CONSIGNEE_DTL;
            //                row.BpoDtl = model.BPO_DTL;
            //                row.GovBillAuth = model.GOV_BILL_AUTH;
            //                row.PurDtl = model.PUR_DTL;
            //                row.PurAutDtl = model.PUR_AUT_DTL;
            //                row.OffInstNoDtl = model.OFF_INST_NO_DTL;
            //                context.SaveChanges();
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        trans.Rollback();
            //        return false;
            //    }
            //    trans.Commit();
            //}
            #endregion

            if (user.Region != "C")
            {
                model.MAN_TYPE = "";
            }
            OracleParameter[] par = new OracleParameter[38];
            par[0] = new OracleParameter("P_CASE_NO", OracleDbType.Varchar2, model.CASE_NO, ParameterDirection.Input);
            par[1] = new OracleParameter("P_CALL_RECV_DT", OracleDbType.Varchar2, model.Display_Call_Recv_dt, ParameterDirection.Input);
            par[2] = new OracleParameter("P_CALL_SNO", OracleDbType.Varchar2, model.Call_SNO, ParameterDirection.Input);
            par[3] = new OracleParameter("P_BK_NO", OracleDbType.Varchar2, model.BK_NO, ParameterDirection.Input);
            par[4] = new OracleParameter("P_SET_NO", OracleDbType.Varchar2, model.SET_NO, ParameterDirection.Input);
            par[5] = new OracleParameter("P_CONSIGNEE_CD", OracleDbType.Int32, model.CONSIGNEE_CD, ParameterDirection.Input);
            par[6] = new OracleParameter("P_ITEM_SRNO_PO", OracleDbType.Int32, model.ITEM_SRNO_PO, ParameterDirection.Input);
            par[7] = new OracleParameter("P_ITEM_DESC_PO", OracleDbType.Varchar2, model.ITEM_DESC_PO, ParameterDirection.Input);
            par[8] = new OracleParameter("P_ITEM_REMARK", OracleDbType.Varchar2, model.ITEM_REMARK, ParameterDirection.Input);
            par[9] = new OracleParameter("P_QTY_ORDERED", OracleDbType.Varchar2, model.QTY_ORDERED, ParameterDirection.Input);
            par[10] = new OracleParameter("P_CUM_QTY_PREV_OFFERED", OracleDbType.Int32, model.CUM_QTY_PREV_OFFERED, ParameterDirection.Input);
            par[11] = new OracleParameter("P_CUM_QTY_PREV_PASSED", OracleDbType.Int32, model.CUM_QTY_PREV_PASSED, ParameterDirection.Input);
            par[12] = new OracleParameter("P_QTY_TO_INSP", OracleDbType.Int32, model.QTY_TO_INSP, ParameterDirection.Input);
            par[13] = new OracleParameter("P_QTY_PASSED", OracleDbType.Int32, model.QTY_PASSED, ParameterDirection.Input);
            par[14] = new OracleParameter("P_QTY_REJECTED", OracleDbType.Int32, model.QTY_REJECTED, ParameterDirection.Input);
            par[15] = new OracleParameter("P_QTY_DUE", OracleDbType.Int32, model.QTY_DUE, ParameterDirection.Input);
            par[16] = new OracleParameter("P_PO_NO", OracleDbType.Varchar2, model.PO_NO, ParameterDirection.Input);
            par[17] = new OracleParameter("P_UNIT_DTL", OracleDbType.Varchar2, model.UNIT_DTL, ParameterDirection.Input);
            par[18] = new OracleParameter("P_IE_STAMPS_DETAIL", OracleDbType.Varchar2, model.IE_STAMPS_DETAIL, ParameterDirection.Input);
            par[19] = new OracleParameter("P_IE_STAMPS_DETAIL2", OracleDbType.Varchar2, model.IE_STAMPS_DETAIL2, ParameterDirection.Input);
            par[20] = new OracleParameter("P_REMARK", OracleDbType.Varchar2, model.REMARK, ParameterDirection.Input);
            par[21] = new OracleParameter("P_PASSED_INST_NO", OracleDbType.Varchar2, model.PASSED_INST_NO, ParameterDirection.Input);
            par[22] = new OracleParameter("P_LAB_TST_RECT_DT", OracleDbType.Varchar2, model.LAB_TST_RECT_DT, ParameterDirection.Input);
            par[23] = new OracleParameter("P_HOLOGRAM", OracleDbType.Varchar2, model.HOLOGRAM, ParameterDirection.Input);
            par[24] = new OracleParameter("P_MAN_TYPE", OracleDbType.Varchar2, model.MAN_TYPE, ParameterDirection.Input);
            par[25] = new OracleParameter("P_DISPATCH_PACKING_NO", OracleDbType.Varchar2, model.DISPATCH_PACKING_NO, ParameterDirection.Input);
            par[26] = new OracleParameter("P_INVOICE_NO", OracleDbType.Varchar2, model.INVOICE_NO, ParameterDirection.Input);
            par[27] = new OracleParameter("P_NAME_OF_IE", OracleDbType.Varchar2, model.NAME_OF_IE, ParameterDirection.Input);
            par[28] = new OracleParameter("P_CONSIGNEE_DESG", OracleDbType.Varchar2, model.CONSIGNEE_DESG, ParameterDirection.Input);
            par[29] = new OracleParameter("P_CONSIGNEE_DTL", OracleDbType.Varchar2, model.CONSIGNEE_DTL, ParameterDirection.Input);
            par[30] = new OracleParameter("P_BPO_DTL", OracleDbType.Varchar2, model.BPO_DTL, ParameterDirection.Input);
            par[31] = new OracleParameter("P_GOV_BILL_AUTH", OracleDbType.Varchar2, model.GOV_BILL_AUTH, ParameterDirection.Input);
            par[32] = new OracleParameter("P_PUR_DTL", OracleDbType.Varchar2, model.PUR_DTL, ParameterDirection.Input);
            par[33] = new OracleParameter("P_PUR_AUT_DTL", OracleDbType.Varchar2, model.PUR_AUT_DTL, ParameterDirection.Input);
            par[34] = new OracleParameter("P_OFF_INST_NO_DTL", OracleDbType.Varchar2, model.OFF_INST_NO_DTL, ParameterDirection.Input);
            par[35] = new OracleParameter("P_IECD", OracleDbType.Varchar2, Convert.ToString(user.IeCd), ParameterDirection.Input);
            par[36] = new OracleParameter("P_IESTAMP_PATH", OracleDbType.Varchar2, model.IESTAMP_PATH, ParameterDirection.Input);
            par[37] = new OracleParameter("P_IESTAMP2_PATH", OracleDbType.Varchar2, model.IESTAMP2_PATH, ParameterDirection.Input);
            var ds = DataAccessDB.ExecuteNonQuery("SP_UPDATE_IC_INTERMEDIATE", par, 1);
            try
            {
                var date = SaveUpdateVisit(model.CASE_NO, model.Display_Call_Recv_dt, model.Call_SNO, model.VISIT_DATES, model.VISIT_DATES);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public int SaveAmendment(string CaseNo, string PO_NO, PO_Amendments model, List<PO_Amendments> lst, string Type)
        {
            var strUpdateSet = "";
            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    if (lst.Count() > 0)
                    {
                        foreach (var item in lst)
                        {
                            if (item.Sno == model.Sno)
                            {
                                item.Amendments = model.Amendments;
                                item.Date = model.Date;
                            }
                            strUpdateSet = strUpdateSet + item.Amendments.Trim().PadRight(100, ' ') + ";" + item.Date.Trim().PadRight(10, ' ') + ";" + item.IECD + "#";

                        }
                        if (!string.IsNullOrEmpty(model.Sno) && Convert.ToInt32(model.Sno) == -1)
                        {
                            strUpdateSet = strUpdateSet + model.Amendments.Trim().PadRight(100, ' ') + ";" + model.Date.Trim().PadRight(10, ' ') + ";" + model.IECD + "#";
                        }


                        var POAhmdetail = (from a in context.IcPoAmendments
                                           where a.CaseNo == CaseNo
                                           select a).FirstOrDefault();

                        if (POAhmdetail != null)
                        {
                            POAhmdetail.AmendmentDetail = strUpdateSet.Substring(0, strUpdateSet.Length - 1);
                            context.SaveChanges();
                        }
                    }
                    else if (Type == "Delete" && lst.Count() == 0)
                    {
                        var POAhmdetail = (from a in context.IcPoAmendments
                                           where a.CaseNo == CaseNo
                                           select a).FirstOrDefault();
                        if (POAhmdetail != null)
                        {
                            context.Remove(POAhmdetail);
                            context.SaveChanges();
                        }
                    }
                    else
                    {
                        strUpdateSet = strUpdateSet + model.Amendments.Trim().PadRight(100, ' ') + ";" + model.Date.Trim().PadRight(10, ' ') + ";" + model.IECD;
                        IcPoAmendment obj = new IcPoAmendment();
                        obj.CaseNo = CaseNo;
                        obj.PoNo = PO_NO;
                        obj.AmendmentDetail = strUpdateSet;
                        context.IcPoAmendments.Add(obj);
                        context.SaveChanges();
                    }
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    return 0;
                }
            }

            var data = UpdateAmend(CaseNo, PO_NO);
            return 1;
        }


        public DateTime[] getSorteddated(string[] arramend)
        {
            try
            {
                DateTime[] arramendSortedated = new DateTime[arramend.Length];

                for (int i = 0; i < arramend.Length; i++)
                {
                    //
                    if (arramend[i] == "") continue;
                    //
                    string[] dataamnd = arramend[i].Split(';')[1].Split('/');

                    //string actualdt = dataamnd[1] + "/" + dataamnd[0] + "/" + dataamnd[2];
                    string actualdt = dataamnd[0] + "/" + dataamnd[1] + "/" + dataamnd[2];

                    arramendSortedated[i] = Convert.ToDateTime(actualdt);
                    //arramendSortedated[i] = Convert.ToDateTime(arramend[i].Split(';')[1]);
                }

                if (arramendSortedated != null) { Array.Sort(arramendSortedated); }
                return arramendSortedated;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string[] Amendments(string[] amendments, DateTime[] amenddates)
        {
            string[] Arramendments = new string[amendments.Length];
            string[] ArramendmentsSelected = new string[amendments.Length];
            int i = 0;

            if (amenddates == null) return null;

            foreach (DateTime item in amenddates)
            {
                for (int j = 0; j < amendments.Length; j++)
                {
                    if (amendments[j] == "") continue;

                    string[] dataamnd = amendments[j].Split(';')[1].Split('/');

                    //string actualdt = dataamnd[1] + "/" + dataamnd[0] + "/" + dataamnd[2];
                    string actualdt = dataamnd[0] + "/" + dataamnd[1] + "/" + dataamnd[2];


                    string dtstr1 = Convert.ToDateTime(actualdt).ToShortDateString();


                    //string dtstr1 = Convert.ToDateTime(amendments[j].Split(';')[1]).ToShortDateString();
                    if (DateTime.Compare(Convert.ToDateTime(dtstr1), Convert.ToDateTime(item)) == 0 && getselectedamend(ArramendmentsSelected, j.ToString()))
                    {
                        Arramendments[i] = amendments[j];
                        ArramendmentsSelected[i] = j.ToString();
                    }
                }
                i++;
            }
            return Arramendments;
        }

        private bool getselectedamend(string[] selectedamend, string newamnd)
        {
            foreach (string item in selectedamend)
            {
                if (item == newamnd)
                {
                    return false;
                }
            }
            return true;
        }

        private string[] GetAmendment(string CaseNo, string PO_No)
        {
            var data = context.IcPoAmendments.Where(x => x.CaseNo == CaseNo && x.PoNo == PO_No).Select(x => x.AmendmentDetail).FirstOrDefault();
            string[] finalamend = null;

            if (data != null)
            {
                string[] arramend = Convert.ToString(data).Split('#');

                finalamend = new string[arramend.Length];
                finalamend = Amendments(arramend, getSorteddated(arramend));
            }

            return finalamend;

        }
        public bool UpdateAmend(string CaseNo, string PO_No)
        {
            var flag = true;
            string[] amend1 = GetAmendment(CaseNo, PO_No);
            string amend2 = "";
            string amend3 = "";
            string amend4 = "";
            string amend5 = "";

            if (amend1 == null) return false;

            if (amend1.Length >= 1)
            {
                amend2 = amend1[amend1.Length - 1];
                if (amend1.Length >= 2)
                {
                    amend3 = amend1[amend1.Length - 2];
                    if (amend1.Length >= 3)
                    {
                        amend4 = amend1[amend1.Length - 3];
                        if (amend1.Length >= 4)
                            amend5 = amend1[amend1.Length - 4];
                        //else amend3 = amend1[3];
                    }
                    //else amend3 = amend1[2];
                }
                //else amend3 = amend1[1];
            }

            using ModelContext cont = new(DbContextHelper.GetDbContextOptions());
            using (var command = cont.Database.GetDbConnection().CreateCommand())
            {
                bool wasOpen = command.Connection.State == ConnectionState.Open;
                if (!wasOpen) command.Connection.Open();
                var tran = command.Connection.BeginTransaction();
                try
                {
                    string updateQuery1 = "Update IC_INTERMEDIATE set ";
                    updateQuery1 += " AMENDMENT_1 = '" + GetFormatedAmend(amend2) + "'  ";
                    updateQuery1 += ", AMENDMENT_2 = '" + GetFormatedAmend(amend3) + "'  ";
                    updateQuery1 += ", AMENDMENT_3 = '" + GetFormatedAmend(amend4) + "'  ";
                    updateQuery1 += ", AMENDMENT_4 = '" + GetFormatedAmend(amend5) + "'  ";
                    updateQuery1 += " where CASE_NO = '" + CaseNo.Trim() + "'";
                    command.Transaction = tran;
                    command.CommandText = updateQuery1;
                    var res = Convert.ToString(command.ExecuteNonQuery());
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    flag = false;
                    tran.Rollback();
                }
                finally
                {
                    if (!wasOpen) command.Connection.Close();
                }
            }
            return flag;
        }

        private string GetFormatedAmend(string amend)
        {
            string formatedamend = "";
            string[] arrformatamenddt = null;
            if (amend != null && amend.Trim() != "")
            {
                string[] arrAmend = amend.Split(';');

                if (arrAmend != null)
                {
                    //for(int i=0;i<=1;i++)
                    {
                        formatedamend += arrAmend[0].PadRight(100, ' ');
                        formatedamend += " " + arrAmend[1];
                    }
                }
            }
            return formatedamend;
        }

        public string RefreshDetail(IC_RPT_IntermediateModel model, UserSessionModel user)
        {
            var flag = true;
            var caseNo = model.CASE_NO.Trim();
            var callRecvDt = DateTime.ParseExact(model.Display_Call_Recv_dt, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var callSno = model.Call_SNO;
            var consigneeCd = model.DP_CONSIGNEE_CD;
            string ErrorMsg = "";
            var query = from details in context.T18CallDetails
                        where details.CaseNo == caseNo &&
                              details.CallRecvDt == callRecvDt &&
                              details.CallSno == Convert.ToInt32(callSno) &&
                              details.ConsigneeCd == Convert.ToInt32(consigneeCd)
                        select new
                        {
                            CASE_NO = details.CaseNo,
                            CALL_RECV_DT = details.CallRecvDt.ToString("dd/MM/yyyy"),
                            CALL_SNO = details.CallSno,
                            CONSIGNEE_CD = details.ConsigneeCd,
                            ITEM_SRNO_PO = details.ItemSrnoPo,
                            ITEM_DESC_PO = details.ItemDescPo,
                            QTY_ORDERED = details.QtyOrdered,
                            CUM_QTY_PREV_OFFERED = details.CumQtyPrevOffered,
                            CUM_QTY_PREV_PASSED = details.CumQtyPrevPassed,
                            QTY_TO_INSP = details.QtyToInsp,
                            QTY_PASSED = details.QtyPassed,
                            QTY_REJECTED = details.QtyRejected,
                            QTY_DUE = details.QtyDue
                        };

            // Execute the query and retrieve the results
            var results = query.ToList();

            if (query.Count() > 0)
            {
                double dblQTY_TO_INSP = 0;
                double dblQTY_REJECTED = 0;
                double dblQTY_PASSED = 0;
                using ModelContext cont = new(DbContextHelper.GetDbContextOptions());

                using (var command = cont.Database.GetDbConnection().CreateCommand())
                {
                    var trans = cont.Database.BeginTransaction();
                    bool wasOpen = command.Connection.State == ConnectionState.Open;
                    if (!wasOpen) command.Connection.Open();
                    try
                    {
                        foreach (var itemDR in results)
                        {
                            string sqlQuery = "";
                            dblQTY_TO_INSP = itemDR.QTY_TO_INSP == null ? 0 : Convert.ToDouble(itemDR.QTY_TO_INSP);
                            dblQTY_REJECTED = itemDR.QTY_REJECTED == null ? 0 : Convert.ToDouble(itemDR.QTY_REJECTED);
                            dblQTY_PASSED = itemDR.QTY_PASSED == null ? 0 : Convert.ToDouble(itemDR.QTY_PASSED);
                            if (!IsValidQty(dblQTY_TO_INSP, dblQTY_REJECTED, dblQTY_PASSED))
                            {
                                ErrorMsg = "QTY REJECTED + QTY PASSED should not be greater than (>) QTY TO INSP !";
                                continue;
                            }

                            var ItemSrnoPo = (from a in context.IcIntermediates
                                              where a.CaseNo == caseNo && a.CallSno == Convert.ToInt32(callSno) && a.CallRecvDt == callRecvDt && a.BkNo == model.BK_NO
                                              && a.SetNo == model.SET_NO && a.ConsigneeCd == itemDR.CONSIGNEE_CD && a.ItemSrnoPo == itemDR.ITEM_SRNO_PO
                                              select a.ItemSrnoPo).FirstOrDefault();
                            if (!string.IsNullOrEmpty(Convert.ToString(ItemSrnoPo)))
                            {
                                if (itemDR.ITEM_SRNO_PO == Convert.ToInt32(model.ITEM_SRNO_PO))
                                {
                                    sqlQuery = "Update IC_INTERMEDIATE set QTY_ORDERED = '" + Convert.ToString(itemDR.QTY_ORDERED) + "',CUM_QTY_PREV_OFFERED = '" + Convert.ToString(itemDR.CUM_QTY_PREV_OFFERED) + "',CUM_QTY_PREV_PASSED  = '" + Convert.ToString(itemDR.CUM_QTY_PREV_PASSED) + "',QTY_TO_INSP = '" + Convert.ToString(itemDR.QTY_TO_INSP) + "',QTY_PASSED = '" + Convert.ToString(itemDR.QTY_PASSED) + "',QTY_REJECTED  = '" + Convert.ToString(itemDR.QTY_REJECTED) + "',QTY_DUE = '" + Convert.ToString(itemDR.QTY_DUE) + "',ITEM_DESC_PO = '" + Convert.ToString(itemDR.ITEM_DESC_PO) + "', ITEM_REMARK ='', UNIT_DTL = null , OFF_INST_NO_DTL = null, PUR_AUT_DTL=null, PUR_DTL= null, CONSIGNEE_DTL = null, BPO_DTL = null where CASE_NO = '" + model.CASE_NO + "' and BK_NO = '" + model.BK_NO + "' and SET_NO = '" + model.SET_NO + "' AND CALL_RECV_DT = TO_date('" + model.Display_Call_Recv_dt + "', 'dd/mm/yyyy') AND CALL_SNO = '" + model.Call_SNO + "' and consignee_cd = '" + Convert.ToString(model.DP_CONSIGNEE_CD) + "' AND ITEM_SRNO_PO = '" + model.ITEM_SRNO_PO + "'  ";
                                    //command.Transaction = trans;
                                    command.CommandText = sqlQuery;
                                }
                            }
                            else
                            {
                                sqlQuery = " Insert into IC_INTERMEDIATE(CASE_NO,CALL_RECV_DT,CALL_SNO,BK_NO,SET_NO,PO_NO,CONSIGNEE_CD,USER_ID,ITEM_SRNO_PO,ITEM_DESC_PO,QTY_ORDERED,CUM_QTY_PREV_OFFERED,CUM_QTY_PREV_PASSED,QTY_TO_INSP,QTY_PASSED,QTY_REJECTED,QTY_DUE,IE_CD,DATETIME,FILE_1,FILE_2,FILE_3,FILE_4,FILE_5,FILE_6,FILE_7,FILE_8,FILE_9,FILE_10,CONSGN_CALL_STATUS,LAB_TST_RECT_DT,REASON_OF_REJECTION,REMARK,HOLOGRAM,IE_STAMP,IE_STAMP2,IE_STAMP_CD,VISITS_DATES,AMENDMENT_1,AMENDMENT_2,AMENDMENT_3,AMENDMENT_4,ITEM_REMARK,IE_STAMP_IMAGE,IE_STAMP_IMAGE1,NUM_VISITS,IE_STAMPS_DETAIL,IE_STAMPS_DETAIL2,PASSED_INST_NO,BPO_DTL,PUR_DTL,PUR_AUT_DTL,OFF_INST_NO_DTL,UNIT_DTL,CONSIGNEE_DTL,DISPATCH_PACKING_NO,INVOICE_NO,NAME_OF_IE,GOV_BILL_AUTH,MAN_TYPE,CONSIGNEE_DESG) " +
                                        " select CASE_NO,CALL_RECV_DT,CALL_SNO,BK_NO,SET_NO,PO_NO,CONSIGNEE_CD,USER_ID,ITEM_SRNO_PO,ITEM_DESC_PO,QTY_ORDERED,CUM_QTY_PREV_OFFERED,CUM_QTY_PREV_PASSED,QTY_TO_INSP,QTY_PASSED,QTY_REJECTED,QTY_DUE,IE_CD,DATETIME,FILE_1,FILE_2,FILE_3,FILE_4,FILE_5,FILE_6,FILE_7,FILE_8,FILE_9,FILE_10,CONSGN_CALL_STATUS,LAB_TST_RECT_DT,REASON_OF_REJECTION,REMARK,HOLOGRAM,IE_STAMP,IE_STAMP2,IE_STAMP_CD,VISITS_DATES,AMENDMENT_1,AMENDMENT_2,AMENDMENT_3,AMENDMENT_4,ITEM_REMARK,IE_STAMP_IMAGE,IE_STAMP_IMAGE1,NUM_VISITS,IE_STAMPS_DETAIL,IE_STAMPS_DETAIL2,PASSED_INST_NO,BPO_DTL,PUR_DTL,PUR_AUT_DTL,OFF_INST_NO_DTL,UNIT_DTL,CONSIGNEE_DTL,DISPATCH_PACKING_NO,INVOICE_NO,NAME_OF_IE,GOV_BILL_AUTH,MAN_TYPE,CONSIGNEE_DESG " + " from (" +
                                        " select '" + Convert.ToString(itemDR.CASE_NO) + "' as CASE_NO,to_date('" + Convert.ToString(itemDR.CALL_RECV_DT) + "','dd/mm/yyyy') as CALL_RECV_DT ,'" + Convert.ToString(itemDR.CALL_SNO) + "' as CALL_SNO,'" + model.BK_NO + "' as BK_NO,'" + model.SET_NO + "' as SET_NO,'" + model.PO_NO + "' as PO_NO,'" + Convert.ToString(itemDR.CONSIGNEE_CD) + "' as CONSIGNEE_CD,USER_ID,'" + Convert.ToString(itemDR.ITEM_SRNO_PO) + "' as ITEM_SRNO_PO , '" + Convert.ToString(itemDR.ITEM_DESC_PO) + "' as ITEM_DESC_PO,'" + Convert.ToString(itemDR.QTY_ORDERED) + "' as QTY_ORDERED,'" + Convert.ToString(itemDR.CUM_QTY_PREV_OFFERED) + "' as CUM_QTY_PREV_OFFERED,'" + Convert.ToString(itemDR.CUM_QTY_PREV_PASSED) + "' as CUM_QTY_PREV_PASSED,'" + Convert.ToString(itemDR.QTY_TO_INSP) + "' as QTY_TO_INSP,'" + Convert.ToString(itemDR.QTY_PASSED) + "' as QTY_PASSED,'" + Convert.ToString(itemDR.QTY_REJECTED) + "' as QTY_REJECTED,'" + Convert.ToString(itemDR.QTY_DUE) + "' as QTY_DUE," + user.IeCd + " as IE_CD,to_date('" + DateTime.Now.ToString("dd/MM/yyyy") + "','dd/mm/yyyy') as DATETIME,FILE_1,FILE_2,FILE_3,FILE_4,FILE_5,FILE_6,FILE_7,FILE_8,FILE_9,FILE_10,CONSGN_CALL_STATUS,LAB_TST_RECT_DT,REASON_OF_REJECTION,REMARK,HOLOGRAM,IE_STAMP,IE_STAMP2,IE_STAMP_CD,VISITS_DATES,AMENDMENT_1,AMENDMENT_2,AMENDMENT_3,AMENDMENT_4,ITEM_REMARK,IE_STAMP_IMAGE,IE_STAMP_IMAGE1,NUM_VISITS,IE_STAMPS_DETAIL,IE_STAMPS_DETAIL2,PASSED_INST_NO,BPO_DTL,PUR_DTL,PUR_AUT_DTL,OFF_INST_NO_DTL,UNIT_DTL,CONSIGNEE_DTL,DISPATCH_PACKING_NO,INVOICE_NO,NAME_OF_IE,GOV_BILL_AUTH,MAN_TYPE,CONSIGNEE_DESG row_number() over (order by CASE_NO) as rn FROM IC_INTERMEDIATE WHERE CASE_NO = '" + Convert.ToString(itemDR.CASE_NO) + "' AND CALL_SNO = '" + Convert.ToString(itemDR.CALL_SNO) + "' AND CALL_RECV_DT =  TO_date('" + Convert.ToString(itemDR.CALL_RECV_DT) + "', 'dd/mm/yyyy') and BK_NO = '" + model.BK_NO + "' and SET_NO = '" + model.SET_NO + "' AND CONSIGNEE_CD = '" + Convert.ToString(itemDR.CONSIGNEE_CD) + "') where rn = 1";
                                //command.Transaction = trans;
                                command.CommandText = sqlQuery;
                            }
                        }

                        var res = command.ExecuteNonQuery();
                        if (trans != null)
                            trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                    }
                    if (!wasOpen) command.Connection.Close();

                }
            }
            return ErrorMsg;
        }

        public void DeleteNotReq(IC_RPT_IntermediateModel model)
        {
            string caseNo = model.CASE_NO;
            string bkNo = model.BK_NO;
            string setNo = model.SET_NO;
            DateTime callRecvDt = DateTime.ParseExact(model.Display_Call_Recv_dt, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string callSno = model.Call_SNO;
            string consigneeCd = Convert.ToString(model.DP_CONSIGNEE_CD);

            var query = from record in context.IcIntermediates
                        where record.CaseNo == caseNo
                              && record.BkNo == bkNo
                              && record.SetNo == setNo
                              && record.CallRecvDt == callRecvDt
                              && record.CallSno == Convert.ToInt32(callSno)
                              && record.ConsigneeCd == Convert.ToInt32(consigneeCd)
                        select new
                        {
                            CASE_NO = record.CaseNo,
                            CALL_RECV_DT = record.CallRecvDt.ToString("dd/MM/yyyy"),
                            CALL_SNO = record.CallSno,
                            CONSIGNEE_CD = record.ConsigneeCd,
                            ITEM_SRNO_PO = record.ItemSrnoPo
                        };


            if (query.Count() > 0)
            {
                using ModelContext cont = new(DbContextHelper.GetDbContextOptions());
                using (var command = cont.Database.GetDbConnection().CreateCommand())
                {
                    var trans = cont.Database.BeginTransaction();
                    bool wasOpen = command.Connection.State == ConnectionState.Open;
                    foreach (var item in query)
                    {
                        var ItemSrnoPo = (from a in context.IcIntermediates
                                          where a.CaseNo == item.CASE_NO && a.CallSno == Convert.ToInt32(item.CALL_SNO) && a.CallRecvDt == Convert.ToDateTime(item.CALL_RECV_DT)
                                          && a.ConsigneeCd == item.CONSIGNEE_CD && a.ItemSrnoPo == item.ITEM_SRNO_PO
                                          select a.ItemSrnoPo).FirstOrDefault();
                        if (string.IsNullOrEmpty(Convert.ToString(ItemSrnoPo)))
                        {
                            var sqlQuery = "";
                            if (!wasOpen) command.Connection.Open();
                            sqlQuery = "DELETE FROM IC_INTERMEDIATE WHERE CASE_NO = '" + Convert.ToString(item.CASE_NO) + "' AND CALL_SNO = '" + Convert.ToString(item.CALL_SNO) + "' AND CALL_RECV_DT =  TO_date('" + Convert.ToString(item.CALL_RECV_DT) + "', 'dd/mm/yyyy') and BK_NO = '" + model.BK_NO + "' and SET_NO = '" + model.SET_NO + "' AND CONSIGNEE_CD = '" + Convert.ToString(item.CONSIGNEE_CD) + "' AND ITEM_SRNO_PO = '" + Convert.ToString(item.ITEM_SRNO_PO) + "' ";
                            command.CommandText = sqlQuery;
                        }
                    }
                    try
                    {
                        var res = command.ExecuteNonQuery();
                        if (trans != null)
                            trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                    }
                    if (!wasOpen) command.Connection.Close();
                }
            }

        }

        private bool IsValidQty(double pinspqty, double prejectedqty, double ppassedqty)
        {
            double inspqty = 0;
            double rejectedqty = 0;
            double passedqty = 0;

            inspqty = Convert.ToDouble(pinspqty);
            rejectedqty = Convert.ToDouble(prejectedqty);
            passedqty = Convert.ToDouble(ppassedqty);

            if ((rejectedqty + passedqty) > inspqty)
            {
                return false;

            }
            return true;

        }


    }
}
