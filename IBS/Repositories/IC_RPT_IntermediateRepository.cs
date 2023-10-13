using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Globalization;
using System.Linq;

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

            OracleParameter[] par = new OracleParameter[6];
            par[0] = new OracleParameter("p_case_no", OracleDbType.NVarchar2, Case_No, ParameterDirection.Input);
            par[1] = new OracleParameter("p_call_recv_dt", OracleDbType.NVarchar2, Call_Recv_Dt, ParameterDirection.Input);
            par[2] = new OracleParameter("p_call_sno", OracleDbType.NVarchar2, Call_SNo, ParameterDirection.Input);
            par[3] = new OracleParameter("p_item_srno_po", OracleDbType.NVarchar2, ITEM_SRNO_PO, ParameterDirection.Input);
            par[4] = new OracleParameter("p_consignee_cd", OracleDbType.NVarchar2, CONSIGNEE_CD, ParameterDirection.Input);
            par[5] = new OracleParameter("p_Result_Cursor", OracleDbType.RefCursor, ParameterDirection.Output);
            var ds = DataAccessDB.GetDataSet("sp_get_ic_rpt_intermediate", par, 1);
            DataTable dt = ds.Tables[0];

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

        public IC_RPT_IntermediateModel FillItems(string Case_No, string Call_Recv_Dt, string Call_SNo, string CONSIGNEE_CD)
        {
            IC_RPT_IntermediateModel model = new();
            //var query = from ic in context.IcIntermediates
            //            where (ic.ConsigneeCd == Convert.ToInt32(CONSIGNEE_CD)
            //                    && ic.CaseNo == Case_No
            //                    && ic.CallRecvDt == DateTime.ParseExact(Call_Recv_Dt, "dd/MM/yyyy", null)
            //                    && ic.CallSno == Convert.ToInt32(Call_SNo))
            //            orderby ic.Datetime descending
            //            select new IC_RPT_IntermediateModel
            //            {
            //                BK_NO = ic.BkNo,
            //                SET_NO = ic.SetNo,
            //                IE_STAMPS_DETAIL = ic.IeStampsDetail,
            //                IE_STAMPS_DETAIL2 = ic.IeStampsDetail2,
            //                LAB_TST_RECT_DT = Convert.ToDateTime(ic.LabTstRectDt),
            //                PASSED_INST_NO = ic.PassedInstNo,
            //                REMARK = ic.Remark,
            //                HOLOGRAM = ic.Hologram
            //            };

            //// Execute the query and retrieve the results
            //var results = query.ToList();
            return model;
        }

        public IC_RPT_IntermediateModel AcceptedFun(string Case_No, string Call_Recv_Dt, string Call_SNo, string CONSIGNEE_CD)
        {
            IC_RPT_IntermediateModel model = new();

            OracleParameter[] par = new OracleParameter[4];
            par[0] = new OracleParameter("p_case_no", OracleDbType.NVarchar2, Case_No, ParameterDirection.Input);
            par[1] = new OracleParameter("p_call_recv_dt", OracleDbType.NVarchar2, Call_Recv_Dt, ParameterDirection.Input);
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



        public bool GetVisitsChanges(string Case_No, string Call_Recv_Dt, string Call_SNo, string VisitDate)
        {
            var callRecvDt = DateTime.ParseExact(Call_Recv_Dt, "dd/MM/yyyy", CultureInfo.InvariantCulture);

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

                    var visitChange = item.VISIT_DATES;
                    using (var trans = context.Database.BeginTransaction())
                    {
                        var query = (from ic in context.IcIntermediates
                                     where (ic.CaseNo == Case_No
                                             && ic.CallRecvDt == DateTime.ParseExact(Call_Recv_Dt, "dd/MM/yyyy", null)
                                             && ic.CallSno == Convert.ToInt32(Call_SNo)
                                             && ic.VisitsDates != null)
                                     orderby ic.Datetime descending
                                     select ic.VisitsDates).ToList();

                        try
                        {
                            if(query.Count() > 0)
                            {

                            }
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                        }
                        trans.Commit();
                    }
                }
            }
            return true;
        }

        public List<IC_RPT_IntermediateModel> Get_IcIntermediate(string Case_No, string Call_Recv_Dt, string Call_SNo, string CONSIGNEE_CD)
        {
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
                             LAB_TST_RECT_DT = Convert.ToDateTime(ic.LabTstRectDt),
                             PASSED_INST_NO = ic.PassedInstNo,
                             REMARK = ic.Remark,
                             HOLOGRAM = ic.Hologram
                         }).ToList();
            return query;
        }
    }
}
