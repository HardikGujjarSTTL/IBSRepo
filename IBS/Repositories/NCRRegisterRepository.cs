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
using System.Net.Mail;
using System.Net;
using System.Numerics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using static IBS.Helper.Enums;

namespace IBS.Repositories
{
    public class NCRRegisterRepository : INCRRegisterRepository
    {
        private readonly ModelContext context;
        private readonly ISendMailRepository pSendMailRepository;

        public NCRRegisterRepository(ModelContext context, ISendMailRepository pSendMailRepository)
        {
            this.context = context;
            this.pSendMailRepository = pSendMailRepository;
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
                        orderCriteria = "NC_NO";
                    }
                    orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
                }
                else
                {
                    orderCriteria = "NC_NO";
                    orderAscendingDirection = true;
                }

                string IENAME = !string.IsNullOrEmpty(dtParameters.AdditionalValues["selectedValue"]) ? Convert.ToString(dtParameters.AdditionalValues["selectedValue"]) : "";
                string FromDate = !string.IsNullOrEmpty(dtParameters.AdditionalValues["FromDate"]) ? Convert.ToString(dtParameters.AdditionalValues["FromDate"]) : "";
                string ToDate = !string.IsNullOrEmpty(dtParameters.AdditionalValues["ToDate"]) ? Convert.ToString(dtParameters.AdditionalValues["ToDate"]) : "";


                NCRRegister model = new NCRRegister();
                List<NCRRegister> modelList = new List<NCRRegister>();

                DataSet ds;
                DateTime parsedDate = DateTime.ParseExact(FromDate, "dd/mm/yyyy", CultureInfo.InvariantCulture);
                DateTime parsedDat1e = DateTime.ParseExact(ToDate, "dd/mm/yyyy", CultureInfo.InvariantCulture);

                string formattedDate = parsedDate.ToString("dd/mm/yyyy");
                string formattedtoDate = parsedDat1e.ToString("dd/mm/yyyy");

               
                OracleParameter[] par = new OracleParameter[4];
                par[0] = new OracleParameter("lst_IE", OracleDbType.Varchar2, IENAME, ParameterDirection.Input);
                par[1] = new OracleParameter("frm_Dt", OracleDbType.Varchar2, formattedDate, ParameterDirection.Input);
                par[2] = new OracleParameter("to_Dt", OracleDbType.Varchar2, formattedtoDate, ParameterDirection.Input);
                par[3] = new OracleParameter("p_result_cursor", OracleDbType.RefCursor, ParameterDirection.Output);
                ds = DataAccessDB.GetDataSet("GetFilterNCR", par, 1);

                DataTable dt = ds.Tables[0];


                List<NCRRegister> list = new();
                if (ds != null && ds.Tables.Count > 0)
                {
                    string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                    list = JsonConvert.DeserializeObject<List<NCRRegister>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                }

                query = list.AsQueryable();

                dTResult.recordsTotal = query.Count();

                if (!string.IsNullOrEmpty(searchBy))
                    query = query.Where(w => Convert.ToString(w.CaseNo).ToLower().Contains(searchBy.ToLower())
                    );

                dTResult.recordsFiltered = query.Count();

                dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

                dTResult.draw = dtParameters.Draw;

                //if (ds != null && ds.Tables.Count > 0)
                //{
                //    dt = ds.Tables[0];

                //    List<NCRRegister> list = dt.AsEnumerable().Select(row => new NCRRegister
                //    {
                //        CaseNo = row.Field<string>("CASE_NO"),
                //        BKNo = row.Field<string>("BK_NO"),
                //        SetNo = row.Field<string>("SET_NO"),
                //        NC_NO = row.Field<string>("NC_NO"),
                //        CALL_SNO = row.Field<int>("CALL_SNO"),
                //        IE_SNAME = row.Field<string>("IE_SNAME"),
                //        CALL_RECV_DT = DateTime.TryParseExact(row.Field<string>("CALL_RECV_DATE"), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime callRecvDate)
                //? callRecvDate
                //: (DateTime?)null,
                //        CONSIGNEE = row.Field<string>("CONSIGNEE"),
                //    }).ToList();

                //    query = list.AsQueryable();

                //    dTResult.recordsTotal = query.Count();

                //    if (!string.IsNullOrEmpty(searchBy))
                //        query = query.Where(w => Convert.ToString(w.CaseNo).ToLower().Contains(searchBy.ToLower())
                //        );

                //    dTResult.recordsFiltered = query.Count();

                //    dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

                //    dTResult.draw = dtParameters.Draw;

                //}
                //else
                //{
                //    return dTResult;
                //}

            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }

            return dTResult;
        }

        public DTResult<Remarks> GetRemarks(DTParameters dtParameters)
        {
            DTResult<Remarks> dTResult = new() { draw = 0 };
            IQueryable<Remarks>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null && dtParameters.Order.Length > 0)
            {
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                if (string.IsNullOrEmpty(orderCriteria)) orderCriteria = "NC_NO";
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                orderCriteria = "NC_NO";
                orderAscendingDirection = true;
            }

            string NCNO = !string.IsNullOrEmpty(dtParameters.AdditionalValues["NC_NO"]) ? Convert.ToString(dtParameters.AdditionalValues["NC_NO"]) : "";

           query = from t69 in context.T69NcCodes
                               join t42 in context.T42NcDetails on t69.NcCd equals t42.NcCd
                               join t41 in context.T41NcMasters on t42.NcNo equals t41.NcNo
                               where t41.NcNo == NCNO
                               orderby t42.NcCdSno
                               select new Remarks
                               {
                                   NcCd = t42.NcCd,
                                   NC_DESC = t42.NcDescOthers != null ? t69.NcDesc + "-" + t42.NcDescOthers : t69.NcDesc,
                                   NcCdSno = t42.NcCdSno,
                                   IeAction1 = t42.IeAction1,
                                   IE_ACTION1_DT = t42.IeAction1Dt,
                                   CoFinalRemarks1 = t42.CoFinalRemarks1,
                                   CO_FINAL_REMARKS1_DT = t42.CoFinalRemarks1Dt
                               };
            dTResult.recordsTotal = query.Count();


            dTResult.recordsFiltered = query.Count();

            if (dtParameters.Length == -1) dtParameters.Length = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public NCRRegister FindByIDActionA(string CASE_NO, string BK_NO, string SET_NO, string NCNO, string Actions)
        {
            NCRRegister model = new NCRRegister();
            DataTable dt = new DataTable();
            string msg = "";
            if (NCNO != "" && NCNO != null)
            {
                OracleParameter[] par = new OracleParameter[2];
                par[0] = new OracleParameter("p_nc_no", OracleDbType.Varchar2, NCNO, ParameterDirection.Input);
                par[1] = new OracleParameter("p_result_cursor", OracleDbType.RefCursor, ParameterDirection.Output);
                var ds = DataAccessDB.GetDataSet("GetForAction_M_NCR", par, 1);
                dt = ds.Tables[0];
            }

            if (CASE_NO != "" && CASE_NO != null)
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

                if (dt.Rows.Count > 0)
                {
                    DataRow firstRow = dt.Rows[0];

                    DateTime callRecvDate;

                    if (!firstRow.IsNull("CALL_RECV_DT"))
                    {
                        if (DateTime.TryParseExact(firstRow["CALL_RECV_DT"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out callRecvDate))
                        {
                            model.CALL_RECV_DT = callRecvDate;
                            model.CALLRECVDT = callRecvDate;
                        }
                    }

                    if (NCNO != "" && NCNO != null)
                    {
                        model.QtyPassed = firstRow.Table.Columns.Contains("QTY_PASSED") && firstRow["QTY_PASSED"] != DBNull.Value ? Convert.ToInt32(firstRow["QTY_PASSED"] as string) : 0;
                        model.Item = firstRow.Table.Columns.Contains("ITEM_DESC_PO") && firstRow["ITEM_DESC_PO"] != DBNull.Value
                            ? Convert.ToString(firstRow["ITEM_DESC_PO"])
                            : string.Empty;
                        if (firstRow.Table.Columns.Contains("NC_DATE") && firstRow["NC_DATE"] != DBNull.Value)
                        {
                            model.NCRDate = Convert.ToDateTime(firstRow["NC_DATE"]);
                        }
                        else
                        {
                            model.NCRDate = DateTime.MinValue;
                        }

                    }
                    if (CASE_NO != "" && CASE_NO != null)
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

                        model.CALLSNO = short.Parse(firstRow["CALL_SNO"].ToString());

                        model.QtyPassed = Convert.ToInt32(qtyPassed);
                        model.Item = descvalue.ItemDescPo;
                        model.Item_Srno_no = descvalue.ItemSrnoPo;
                        model.Ie_Cd = (byte?)(int)firstRow["IE_CD"];

                        model.NCRDate = DateTime.Now;
                    }
                    model.SetRegionCode = firstRow["REGION_CODE"].ToString();
                    model.UserID = firstRow["user_id"].ToString();

                    model.CaseNo = firstRow["case_no"].ToString();
                    model.PO_NO = firstRow["po_no"].ToString();
                    model.BKNo = firstRow["bk_no"].ToString();
                    model.SetNo = firstRow["set_no"].ToString();
                    model.CONSIGNEE = firstRow["CONSIGNEE"].ToString();
                    model.CONSIGNEE_CD = Convert.ToInt32(firstRow["CONSIGNEE_CD"]);
                    model.CONSIGNEECD = (byte?)(int)firstRow["CONSIGNEE_CD"];
                    model.Vendor = firstRow["vendor"].ToString();
                    model.VEND_CD = Convert.ToInt32(firstRow["VEND_CD"]);
                    model.CALL_SNO = Convert.ToInt32(firstRow["CALL_SNO"]);
                    model.NC_NO = NCNO;


                    model.IeCd = Convert.ToString(firstRow["IE_CD"]);
                    model.IE_SNAME = firstRow["IE_NAME"].ToString();

                    if (!firstRow.IsNull("IC_DATE"))
                    {
                        model.ICDate = Convert.ToDateTime(firstRow["IC_DATE"]);
                    }

                    model.IC_NO = firstRow["IC_NO"].ToString();
                    if (!firstRow.IsNull("PO_DT"))
                    {
                        model.PO_DT = Convert.ToDateTime(firstRow["PO_DT"]);
                    }
                }
                else
                {
                    msg = "Data Not Found";
                }
            }

            model.lstRemark = (from t69 in context.T69NcCodes
                              join t42 in context.T42NcDetails on t69.NcCd equals t42.NcCd
                              join t41 in context.T41NcMasters on t42.NcNo equals t41.NcNo
                              where t41.NcNo == NCNO
                              orderby t42.NcCdSno
                              select new Remarks
                              {
                                  NcCd = t42.NcCd,
                                  NC_DESC = t42.NcDescOthers != null ? t69.NcDesc + "-" + t42.NcDescOthers : t69.NcDesc,
                                  NcCdSno = t42.NcCdSno,
                                  IeAction1 = t42.IeAction1,
                                  IE_ACTION1_DT = t42.IeAction1Dt,
                                  CoFinalRemarks1 = t42.CoFinalRemarks1,
                                  CO_FINAL_REMARKS1_DT = t42.CoFinalRemarks1Dt
                              }).ToList();

            return model;
        }

        public string SaveRemarks(string NCNO, string UserID, List<Remarks> model)
        {
            string msg = "";

            foreach (var item in model)
            {
                var existingRecord = context.T42NcDetails.FirstOrDefault(record => record.NcNo == NCNO && record.NcCd == item.NcCd && record.NcCdSno == item.NcCdSno);

                if (existingRecord != null)
                {
                    if (!string.IsNullOrEmpty(item.CoFinalRemarks1))
                    {
                        existingRecord.CoFinalRemarks1 = item.CoFinalRemarks1;
                    }
                    existingRecord.CoFinalRemarks1Dt = DateTime.Now;
                    existingRecord.UserId = UserID;
                    if (!string.IsNullOrEmpty(item.IeAction1))
                    {
                        existingRecord.IeAction1 = item.IeAction1;
                    }
                    existingRecord.IeAction1Dt = DateTime.Now;
                    existingRecord.Datetime = DateTime.Now;

                    context.SaveChanges();
                }
                msg = "Remarks Save Successfully";

            }

            return msg;
        }

        public string GetItems(string CaseNo, string BKNo, string SetNo)
        {
            var Desc = context.T18CallDetails
                        .Where(t18 => context.T20Ics
                            .Any(t20 => t20.CaseNo == CaseNo.Trim()
                                        && t20.BkNo == BKNo.Trim()
                                        && t20.SetNo == SetNo.Trim()
                                        && t20.CaseNo == t18.CaseNo
                                        && t20.CallRecvDt == t18.CallRecvDt
                                        && t20.CallSno == t18.CallSno
                                        && t20.ConsigneeCd == t18.ConsigneeCd))
                        .Select(t18 => new
                        {
                            t18.ItemSrnoPo,
                            t18.ItemDescPo
                        })
                        .ToList();

            string json = JsonConvert.SerializeObject(Desc, Formatting.Indented);

            return json;
        }
        
        public string GetQtyByItems(string CaseNo, string CALLRECVDT, string CALLSNO, string ItemSno)
        {
            var result = context.T18CallDetails
                        .Where(t18 => t18.CaseNo == CaseNo
                                      && t18.CallRecvDt == Convert.ToDateTime(CALLRECVDT)
                                      && t18.CallSno == Convert.ToInt32(CALLSNO)
                                      && t18.ItemSrnoPo == Convert.ToInt32(ItemSno))
                        .Select(t18 => t18.QtyPassed ?? 0)
                        .FirstOrDefault();

            decimal qtyPassed = result != null ? result : 0;

            string json = Convert.ToString(qtyPassed);

            return json;
        }

        public NCRRegister Saveupdate(NCRRegister model, bool isRadioChecked, string extractedText)
        {
            string msg = "";

            DataTable dt = new DataTable();
            string genrate_NCNO = "";
            string ErrCD = "";

            if (model.NC_NO != "" || model.NC_NO != null)
            {
                OracleParameter[] par = new OracleParameter[3];
                par[0] = new OracleParameter("IN_REGION_CD", OracleDbType.Varchar2, model.SetRegionCode, ParameterDirection.Input);
                par[1] = new OracleParameter("IN_NC_DT", OracleDbType.Varchar2, model.NCRDate, ParameterDirection.Input);
                par[2] = new OracleParameter("p_result_cursor", OracleDbType.RefCursor, ParameterDirection.Output);
                var ds = DataAccessDB.GetDataSet("GENERATE_NC_NO_New", par, 1);
                dt = ds.Tables[0];

                DataRow firstRow = dt.Rows[0];
                ErrCD = firstRow["W_ERR_CD"].ToString();
                genrate_NCNO = firstRow["W_NC_NO"].ToString().Trim();
                model.NC_NO = genrate_NCNO;
            }


            var NCRMaster = context.T41NcMasters.FirstOrDefault(r => r.CaseNo == model.CaseNo && r.BkNo == model.BKNo && r.SetNo == model.SetNo);

            if (ErrCD == "-1")
            {
                msg = "NC Details not available";
            }
            else
            {
                if (NCRMaster == null)
                {
                    T41NcMaster obj = new T41NcMaster();
                    obj.NcNo = genrate_NCNO;
                    obj.NcDt = model.NCRDate;
                    obj.CaseNo = model.CaseNo;
                    obj.CallRecvDt = model.CALLRECVDT;
                    obj.ItemDescPo = model.Item;
                    obj.CallSno = model.CALLSNO;
                    obj.BkNo = model.BKNo;
                    obj.SetNo = model.SetNo;
                    obj.VendCd = model.VEND_CD;
                    obj.CoCd = model.CONSIGNEECD;
                    obj.QtyPassed = model.QtyPassed;
                    obj.PoNo = model.PO_NO;
                    obj.PoDt = model.PO_DT;
                    obj.IcNo = model.IC_NO;
                    obj.IcDt = model.ICDate;
                    obj.IeCd = model.Ie_Cd;
                    obj.ConsigneeCd = model.CONSIGNEECD;
                    obj.Datetime = DateTime.Now;
                    obj.ItemSrnoPo = model.Item_Srno_no;
                    obj.UserId = model.UserID;
                    obj.RegionCode = model.SetRegionCode;
                    context.T41NcMasters.Add(obj);
                    context.SaveChanges();
                    msg = "Record Saved Successfully";
                }
                else
                {
                    model.NC_NO = NCRMaster.NcNo;
                }
                if (extractedText != "-Select--")
                {
                    if (isRadioChecked == true)
                    {

                        T42NcDetail obj1 = new T42NcDetail();
                        obj1.NcNo = genrate_NCNO;
                        obj1.NcCd = "X01";
                        obj1.NcCdSno = 1;
                        obj1.NcDescOthers = "";
                        obj1.UserId = model.UserID;
                        obj1.Datetime = DateTime.Now;
                        context.T42NcDetails.Add(obj1);
                        context.SaveChanges();
                    }
                    else
                    {
                        var NCRemarkMaster = context.T42NcDetails.FirstOrDefault(r => r.NcNo == model.NC_NO);

                        if (NCRemarkMaster == null)
                        {
                            T42NcDetail obj1 = new T42NcDetail();
                            obj1.NcNo = genrate_NCNO;
                            obj1.NcCd = model.NcCdSno;
                            obj1.NcCdSno = 1;
                            obj1.NcDescOthers = extractedText;
                            obj1.UserId = model.UserID;
                            obj1.Datetime = DateTime.Now;
                            context.T42NcDetails.Add(obj1);
                            context.SaveChanges();
                        }
                    }
                }
            }

            return model;
        }

        public NCRRegister SaveMoreNC(NCRRegister model, string extractedText)
        {
            string msg = "";
            var maxNcCdSno = context.T42NcDetails
                        .Where(detail => detail.NcNo == model.NC_NO)
                        .Select(detail => (int?)detail.NcCdSno) // Project to nullable int
                        .Max();

            int nextNcCdSno = maxNcCdSno.HasValue ? maxNcCdSno.Value + 1 : 1;

            T42NcDetail obj1 = new T42NcDetail();
            obj1.NcNo = model.NC_NO;
            obj1.NcCd = model.NcCdSno;
            obj1.NcCdSno = nextNcCdSno;
            obj1.NcDescOthers = extractedText;
            obj1.UserId = model.UserID;
            obj1.Datetime = DateTime.Now;
            context.T42NcDetails.Add(obj1);
            context.SaveChanges();

            model.lstRemark = (from t69 in context.T69NcCodes
                               join t42 in context.T42NcDetails on t69.NcCd equals t42.NcCd
                               join t41 in context.T41NcMasters on t42.NcNo equals t41.NcNo
                               where t41.NcNo == model.NC_NO
                               orderby t42.NcCdSno
                               select new Remarks
                               {
                                   NcCd = t42.NcCd,
                                   NC_DESC = t42.NcDescOthers != null ? t69.NcDesc + "-" + t42.NcDescOthers : t69.NcDesc,
                                   NcCdSno = t42.NcCdSno,
                                   IeAction1 = t42.IeAction1,
                                   IE_ACTION1_DT = t42.IeAction1Dt,
                                   CoFinalRemarks1 = t42.CoFinalRemarks1,
                                   CO_FINAL_REMARKS1_DT = t42.CoFinalRemarks1Dt
                               }).ToList();

            return model;
        }

        public bool SendEmail(NCRRegister nCRRegister)
        {
            string region = nCRRegister.SetRegionCode;
            string wRegion = GetRegionDetails(region);
            string rsender = GetSenderEmail(region);

            DataTable dt = new DataTable();
            OracleParameter[] par = new OracleParameter[2];
            par[0] = new OracleParameter("p_NC_NO", OracleDbType.Varchar2, nCRRegister.NC_NO, ParameterDirection.Input);
            par[1] = new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output);
            var ds = DataAccessDB.GetDataSet("GET_NCR_Email", par, 1);
            dt = ds.Tables[0];

            int j = 0;
            string NC_REASONS = "";

            foreach (DataRow row in dt.Rows)
            {
                if (row["NC_CLASS"].ToString() == "C")
                {
                    j = 1;
                }
                if (string.IsNullOrEmpty(NC_REASONS))
                {
                    NC_REASONS = $"IC No. {row["IC_NO"]}, Dated: {row["IC_DATE"]}\n";
                    NC_REASONS += $"Case No. {row["CASE_NO"]}\n";
                    NC_REASONS += $"Item: {row["ITEM_DESC_PO"]}\n";
                    NC_REASONS += $"PO No. {row["PO_NO"]}, Dated: {row["PO_DATE"]}\n";
                    NC_REASONS += $"IE: {row["IE_NAME"]}\n";
                    NC_REASONS += $"CM: {row["CO_NAME"]}\n";
                }

                NC_REASONS += $"NCR Code: {row["NC_CD"]}-{row["NC_DESC"]}\n";
                if (!string.IsNullOrEmpty(row["IE_ACTION1"].ToString()))
                {
                    NC_REASONS += $"IE Corrective and Preventive Action: {row["IE_ACTION1"]}\n";
                }
                if (!string.IsNullOrEmpty(row["CO_FINAL_REMARKS1"].ToString()))
                {
                    NC_REASONS += $"Controlling Remarks: {row["CO_FINAL_REMARKS1"]}\n";
                }
            }

            int ieCdFromDataRow = Convert.ToInt32(ds.Tables[0].Rows[0]["IE_CD"]);

            var emailQuery = (from t09 in context.T09Ies
                              join t08 in context.T08IeControllOfficers on t09.IeCoCd equals t08.CoCd
                              where t09.IeCd == ieCdFromDataRow
                              select t09.IeEmail + ";" + t08.CoEmail).FirstOrDefault();

            string emailAddresses = emailQuery ?? string.Empty;

            MailMessage mail1 = new MailMessage();

           
            if (j == 1 && nCRRegister.SetRegionCode == "N")
            {
                mail1.CC.Add("sbu.ninsp@rites.com");
            }

            mail1.From = new MailAddress("nrinspn@gmail.com");
            mail1.Subject = "Non Conformities Register";
            mail1.Body = NC_REASONS + "\n" + wRegion;

            SendMailModel sendMailModel = new SendMailModel();
            //rsender = rsender;
            sendMailModel.From = rsender;
            sendMailModel.To = emailAddresses;
            sendMailModel.Subject = "Non Conformities Register";
            sendMailModel.Message = NC_REASONS + "\n" + wRegion; ;

            bool isSend = pSendMailRepository.SendMail(sendMailModel, null);

            return isSend;
        }

        private string GetRegionDetails(string region)
        {
            string wRegion = string.Empty;

            if (region == "N")
            {
                wRegion = "NORTHERN REGION \n 12th FLOOR,CORE-II,SCOPE MINAR,LAXMI NAGAR, DELHI - 110092 \n Phone : +918800018691-95 \n Fax : 011-22024665";
            }
            else if (region == "S")
            {
                wRegion = "SOUTHERN REGION \n CTS BUILDING - 2ND FLOOR, BSNL COMPLEX, NO. 16, GREAMS ROAD,  CHENNAI - 600 006 \n Phone : 044-28292807/044- 28292817 \n Fax : 044-28290359";
            }
            else if (region == "E")
            {
                wRegion = "EASTERN REGION \n CENTRAL STATION BUILDING(METRO), 56, C.R. AVENUE,3rd FLOOR,KOLKATA-700 012  \n Fax : 033-22348704";
            }
            else if (region == "W")
            {
                wRegion = "WESTERN REGION \n 5TH FLOOR, REGENT CHAMBER, ABOVE STATUS RESTAURANT,NARIMAN POINT,MUMBAI-400021 \n Phone : 022-68943400/68943445";
            }
            else if (region == "C")
            {
                wRegion = "Central Region";
            }

            return wRegion;
        }

        private string GetSenderEmail(string region)
        {
            if (region == "N") return "nrinspn@rites.com";
            if (region == "S") return "srinspn@rites.com";
            if (region == "E") return "erinspn@rites.com";
            if (region == "W") return "wrinspn@rites.com";
            // Return a default sender email for other regions
            return "default@example.com";
        }

        public int ShouldRemarkDisable(string ncNo)
        {
            if (!string.IsNullOrEmpty(ncNo))
            {
                int count = context.T42NcDetails
                            .Where(detail => detail.NcNo == ncNo && detail.NcCd.StartsWith("X"))
                            .Count();

                return count;
            }

            return 0;
        }

        public List<SelectListItem> GetNcrCd(string NCRClass)
        {
            List<SelectListItem> NCCODE = (from a in context.T69NcCodes
                                           where a.NcClass == NCRClass
                                           select
                                          new SelectListItem
                                          {
                                              Text = a.NcCd + " - " + a.NcDesc,
                                              Value = a.NcCd
                                          }).ToList();
            return NCCODE;
        }
    }
}
