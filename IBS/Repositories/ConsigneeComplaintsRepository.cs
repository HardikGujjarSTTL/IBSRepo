using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace IBS.Repositories
{
    public class ConsigneeComplaintsRepository : IConsigneeComplaintsRepository
    {
        private readonly ModelContext context;
        private readonly ISendMailRepository pSendMailRepository;
        private readonly IConfiguration config;

        public ConsigneeComplaintsRepository(ModelContext context, ISendMailRepository pSendMailRepository, IConfiguration _config)
        {
            this.context = context;
            this.pSendMailRepository = pSendMailRepository;
            this.config = _config;
        }

        public ConsigneeComplaints FindByID(string CASE_NO, string BK_NO, string SET_NO)
        {
            ConsigneeComplaints model = new ConsigneeComplaints();
            DataTable dt = new DataTable();

            OracleParameter[] par = new OracleParameter[4];
            par[0] = new OracleParameter("p_case_no", OracleDbType.Varchar2, CASE_NO, ParameterDirection.Input);
            par[1] = new OracleParameter("p_bk_no", OracleDbType.Varchar2, BK_NO, ParameterDirection.Input);
            par[2] = new OracleParameter("p_set_no", OracleDbType.Varchar2, SET_NO, ParameterDirection.Input);
            par[3] = new OracleParameter("RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);



            var ds = DataAccessDB.GetDataSet("GetConsigneeDetails", par, 1);
            dt = ds.Tables[0];

            string regiondata = "";
            string wRegion = dt.Rows[0].Field<string>("region_code");
            if (wRegion == "N") { regiondata = "Northern Region"; }
            else if (wRegion == "E") { regiondata = "Eastern Region"; }
            else if (wRegion == "W") { regiondata = "Western Region"; }
            else if (wRegion == "S") { regiondata = "Southern Region"; }
            else if (wRegion == "C") { regiondata = "Central Region"; }
            else { regiondata = ""; }

            List<ConsigneeComplaints> list = dt.AsEnumerable().Select(row => new ConsigneeComplaints
            {
                CASE_NO = row.Field<string>("case_no"),
                PO_NO = row.Field<string>("po"),
                BK_NO = row.Field<string>("bk_no"),
                SET_NO = row.Field<string>("set_no"),
                IC_NO = row.Field<string>("ic_no"),
                Consignee = row.Field<string>("CONSIGNEE"),
                VEND_NAME = row.Field<string>("vendor"),
                FormattedIC_DATE = row.Field<string>("IC_Dt"),
                Railway = row.Field<string>("rly_cd"),
                CoName = row.Field<string>("co_name"),
                VendCd = row.Field<int>("vend_cd"),
                ie_name = row.Field<string>("ie_name"),
                ConsigneeCd = row.Field<int>("Consignee_cd"),
            }).ToList();

            if (ds != null)
            {
                model.CASE_NO = list[0].CASE_NO;
                model.ComplaintDate = list[0].ComplaintDate;
                model.PO_DT = list[0].PO_DT;
                model.ComplaintId = list[0].ComplaintId;
                model.FormattedPO_DT = list[0].PO_DT?.ToString("MM-dd-yyyy");
                model.FormattedComplaintDate = list[0].ComplaintDate?.ToString("MM-dd-yyyy");
                model.PO_NO = list[0].PO_NO;
                model.VEND_NAME = list[0].VEND_NAME;
                model.ConsigneeCd = list[0].ConsigneeCd;
                model.VendCd = list[0].VendCd;
                model.InspRegion = regiondata;
                model.BK_NO = list[0].BK_NO;
                model.SET_NO = list[0].SET_NO;
                model.ie_name = list[0].ie_name;
                model.CoName = list[0].CoName;
                model.Consignee = list[0].Consignee;
                model.FormattedIC_DATE = list[0].FormattedIC_DATE;
                model.RejMemoDt = list[0].RejMemoDt;
                model.RejMemoNo = list[0].RejMemoNo;
                model.Railway = list[0].Railway;
                model.IC_NO = list[0].IC_NO;
                model.ItemDesc = list[0].ItemDesc;
                model.QtyOffered = list[0].QtyOffered;
                model.QtyRejected = list[0].QtyRejected;
                model.Rate = list[0].Rate;
                model.RejectionReason = list[0].RejectionReason;
                model.InspectionBy = list[0].InspectionBy;
            }
            return model;
        }

        public ConsigneeComplaints FindByCompID(string ComplaintId)
        {
            ConsigneeComplaints model = new ConsigneeComplaints();
            DataTable dt = new DataTable();

            OracleParameter[] par = new OracleParameter[2];
            par[0] = new OracleParameter("p_complaint_id", OracleDbType.Varchar2, ComplaintId, ParameterDirection.Input);
            par[1] = new OracleParameter("p_result", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GetConsigneeComplaintDetails", par, 1);
            dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                model.CASE_NO = dt.Rows[0]["CASE_NO"].ToString();
                model.ComplaintDate = Convert.ToDateTime(dt.Rows[0]["COMPLAINT_DATE"]);
                model.ComplaintId = ComplaintId;
                model.PO_NO = dt.Rows[0]["PO"].ToString();
                model.VEND_NAME = dt.Rows[0]["VEND_NAME"].ToString();
                model.BK_NO = dt.Rows[0]["BK_NO"].ToString();
                model.SET_NO = dt.Rows[0]["SET_NO"].ToString();
                //model.ie_name = dt.Rows[0]["IE_NAME"].ToString();
                model.Consignee = dt.Rows[0]["CONSIGNEE"].ToString();
                model.FormattedIC_DATE = dt.Rows[0]["IC_DT"].ToString();
                model.RejMemoDt = dt.Rows[0]["REJ_MEMO_DT"] is DBNull ? default(DateTime) : Convert.ToDateTime(dt.Rows[0]["REJ_MEMO_DT"]);
                //model.RejMemoDt = Convert.ToDateTime(dt.Rows[0]["REJ_MEMO_DT"]);
                model.RejMemoNo = dt.Rows[0]["REJ_MEMO_NO"].ToString();
                model.Railway = dt.Rows[0]["rly_cd"].ToString();
                model.ItemDesc = dt.Rows[0]["ITEM_DESC"].ToString();
                model.QtyOffered = Convert.ToDecimal(dt.Rows[0]["QTY_OFFERED"]);
                model.QtyRejected = Convert.ToDecimal(dt.Rows[0]["QTY_REJECTED"]);
                model.rejectionValue = Convert.IsDBNull(dt.Rows[0]["REJECTION_VALUE"]) ? 0 : Convert.ToDecimal(dt.Rows[0]["REJECTION_VALUE"]);
                //model.rejectionValue = Convert.ToDecimal(dt.Rows[0]["REJECTION_VALUE"]);
                model.Rate = Convert.IsDBNull(dt.Rows[0]["RATE"]) ? 0 : Convert.ToDecimal(dt.Rows[0]["RATE"]);
                model.RejectionReason = dt.Rows[0]["REJECTION_REASON"].ToString();
                model.InspRegion = dt.Rows[0]["region_code"].ToString();
                model.CoName = dt.Rows[0]["IE_CO_CD"].ToString();
                model.ITEM_SRNO_PO = dt.Rows[0]["ITEM_SRNO_PO"].ToString();
                model.VendCd = Convert.ToInt32(dt.Rows[0]["vend_cd"]);
                model.ConsigneeCd = Convert.ToInt32(dt.Rows[0]["consignee_cd"]);
                model.ie_cd = Convert.ToInt32(dt.Rows[0]["ie_cd"] is DBNull ? 0 : dt.Rows[0]["ie_cd"]);
                model.ie_co_cd = Convert.ToInt32(dt.Rows[0]["ie_co_cd"] is DBNull ? 0 : dt.Rows[0]["ie_co_cd"]);
                model.UserId = dt.Rows[0]["user_id"].ToString();
                model.unitofM = "Per" + dt.Rows[0]["UOM_S_DESC"].ToString();
                model.uom_cd = dt.Rows[0]["uom_cd"].ToString();
                model.Remarks = dt.Rows[0]["REMARKS"].ToString();
                model.JiStatusCd = dt.Rows[0]["JI_STATUS_CD"].ToString();
                model.COMP_RECV_REGION = dt.Rows[0]["COMP_RECV_REGION"].ToString();
            }
            return model;
        }

        public DTResult<ConsigneeComplaints> GetDataListComplaint(DTParameters dtParameters)
        {
            DTResult<ConsigneeComplaints> dTResult = new() { draw = 0 };
            IQueryable<ConsigneeComplaints>? query = null;

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

            string PoNo = "", PoDt = "";


            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["PoNo"]))
            {
                PoNo = Convert.ToString(dtParameters.AdditionalValues["PoNo"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["PoDt"]))
            {
                PoDt = Convert.ToString(dtParameters.AdditionalValues["PoDt"]);
            }

            DateTime? dtPo = !string.IsNullOrEmpty(dtParameters.AdditionalValues["PoDt"]) ? Convert.ToDateTime(dtParameters.AdditionalValues["PoDt"]) : null;


            PoNo = PoNo.ToString() == "" ? string.Empty : PoNo.ToString();
            //DateTime? _PoDt = PoDt == "" ? null : DateTime.ParseExact(PoDt, "dd-MM-yyyy", null);


            OracleParameter[] par1 = new OracleParameter[6];
            par1[0] = new OracleParameter("p_PO_No", OracleDbType.Varchar2, PoNo, ParameterDirection.Input);
            par1[1] = new OracleParameter("p_PO_Date", OracleDbType.Varchar2, PoDt, ParameterDirection.Input);
            par1[2] = new OracleParameter("p_page_start", OracleDbType.Int32, dtParameters.Start + 1, ParameterDirection.Input);
            par1[3] = new OracleParameter("p_page_end", OracleDbType.Int32, (dtParameters.Start + dtParameters.Length), ParameterDirection.Input);
            par1[4] = new OracleParameter("p_ResultSet", OracleDbType.RefCursor, ParameterDirection.Output);
            par1[5] = new OracleParameter("p_result_records", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds2 = DataAccessDB.GetDataSet("GetConsigneeComplaint", par1, 2);
            DataTable dt2 = ds2.Tables[0];

            List<ConsigneeComplaints> list = dt2.AsEnumerable().Select(row => new ConsigneeComplaints
            {
                CASE_NO = row.Field<string>("case_no"),
                PO_NO = row.Field<string>("po_no"),
                BK_NO = row.Field<string>("bk_no"),
                SET_NO = row.Field<string>("set_no"),
                IC_NO = row.Field<string>("IC_NO"),
                JiSno = row.Field<string>("ji_sno"),
                ComplaintId = row.Field<string>("complaint_id"),
                RejMemoNo = row.Field<string>("rej_memo_no"),
                PO_DT = row.Field<DateTime?>("PO_DT"),
                IC_DATE = row.Field<DateTime?>("IC_DT"),
                RejMemoDt = row.Field<DateTime?>("rej_memo_dt"),
            }).ToList();

            query = list.AsQueryable();

            int recordsTotal = 0;
            if (ds2 != null && ds2.Tables[1].Rows.Count > 0)
            {
                recordsTotal = Convert.ToInt32(ds2.Tables[1].Rows[0]["total_records"]);
            }
            dTResult.recordsTotal = recordsTotal;
            dTResult.recordsFiltered = recordsTotal;
            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Select(p => p).ToList();
            dTResult.draw = dtParameters.Draw;
            return dTResult;
        }


        public DTResult<ConsigneeComplaints> GetDataListConsignee(DTParameters dtParameters)
        {

            DTResult<ConsigneeComplaints> dTResult = new() { draw = 0 };
            IQueryable<ConsigneeComplaints>? query = null;

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

            string PoNo = "", PoDt = "";


            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["PoNo"]))
            {
                PoNo = Convert.ToString(dtParameters.AdditionalValues["PoNo"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["PoDt"]))
            {
                PoDt = Convert.ToString(dtParameters.AdditionalValues["PoDt"]);
            }

            DateTime? dtPo = !string.IsNullOrEmpty(dtParameters.AdditionalValues["PoDt"]) ? Convert.ToDateTime(dtParameters.AdditionalValues["PoDt"]) : null;


            PoNo = PoNo.ToString() == "" ? string.Empty : PoNo.ToString();
            //DateTime? _PoDt = PoDt == "" ? null : DateTime.ParseExact(PoDt, "dd-MM-yyyy", null);


            OracleParameter[] par = new OracleParameter[3];
            par[0] = new OracleParameter("p_po_no_param", OracleDbType.Varchar2, PoNo, ParameterDirection.Input);
            par[1] = new OracleParameter("p_po_date_param", OracleDbType.Date, dtPo, ParameterDirection.Input);
            par[2] = new OracleParameter("RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GetFilteredConsigneeComplaints", par, 1);
            DataTable dt = ds.Tables[0];

            ConsigneeComplaints model = new();
            List<ConsigneeComplaints> list = new();
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                list = JsonConvert.DeserializeObject<List<ConsigneeComplaints>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }

            List<ConsigneeComplaints> lst = new();

            query = list.AsQueryable();

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.PO_NO).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }


        public string ComplaintsDetailsInsertUpdate(ConsigneeComplaints model)
        {
            string Complaintid = "";
            var Complaint = (from r in context.T40ConsigneeComplaints where r.ComplaintId == model.ComplaintId select r).FirstOrDefault();
            #region Complaint save
            if (Complaint == null)
            {
                DataSet ds;
                string inspRegionFirstChar = model.InspRegion.Substring(0, 1);
                OracleParameter[] par = new OracleParameter[3];
                par[0] = new OracleParameter("IN_REGION_CD", OracleDbType.Varchar2, inspRegionFirstChar, ParameterDirection.Input);
                par[1] = new OracleParameter("IN_COMPLAINT_DT", OracleDbType.Varchar2, DateTime.Now, ParameterDirection.Input);
                par[2] = new OracleParameter("p_result_cursor", OracleDbType.RefCursor, ParameterDirection.Output);
                ds = DataAccessDB.GetDataSet("GENERATE_COMPLAINT_NO", par, 1);

                DataTable dt = ds.Tables[0];

                DataRow firstRow = dt.Rows[0];
                string ComplaintID = firstRow["W_COMPID"].ToString().Trim();

                T40ConsigneeComplaint obj = new T40ConsigneeComplaint();
                obj.ComplaintId = ComplaintID;
                obj.RejMemoNo = model.RejMemoNo;
                obj.CaseNo = model.CASE_NO;
                obj.BkNo = model.BK_NO;
                obj.SetNo = model.SET_NO;
                obj.ConsigneeCd = model.ConsigneeCd;
                obj.VendCd = model.VendCd;
                obj.ItemDesc = model.ItemDesc;
                obj.IeCd = Convert.ToByte(model.ie_cd);
                obj.IeCoCd = Convert.ToByte(model.ie_co_cd);
                obj.CompRecvRegion = model.COMP_RECV_REGION;
                obj.ItemSrnoPo = Convert.ToByte(model.ITEM_SRNO_PO);
                obj.QtyOffered = model.QtyOffered;
                obj.QtyRejected = model.QtyRejected;
                obj.UomCd = Convert.ToInt32(model.uom_cd);
                obj.Rate = model.Rate;
                obj.RejectionValue = model.rejectionValue;
                obj.RejectionReason = model.RejectionReason;
                obj.UserId = model.UserId;
                obj.ComplaintDt = DateTime.Now;
                obj.RejMemoDt = model.RejMemoDt;
                obj.Datetime = DateTime.Now;
                obj.Createdby = Convert.ToInt32(model.UserId);
                obj.Createddate = DateTime.Now;
                context.T40ConsigneeComplaints.Add(obj);
                context.SaveChanges();
                Complaintid = obj.ComplaintId;
            }
            else
            {
                try
                {
                    Complaint.ComplaintId = model.ComplaintId;
                    Complaint.RejMemoNo = model.RejMemoNo;
                    Complaint.CaseNo = model.CASE_NO;
                    Complaint.BkNo = model.BK_NO;
                    Complaint.SetNo = model.SET_NO;
                    Complaint.ConsigneeCd = model.ConsigneeCd;
                    Complaint.VendCd = model.VendCd;
                    Complaint.ItemDesc = model.ItemDesc;
                    Complaint.IeCd = model.ie_cd;
                    Complaint.IeCoCd = model.ie_co_cd;
                    Complaint.CompRecvRegion = model.COMP_RECV_REGION;
                    Complaint.ItemSrnoPo = Convert.ToByte(model.ITEM_SRNO_PO);
                    Complaint.QtyOffered = model.QtyOffered;
                    Complaint.QtyRejected = model.QtyRejected;
                    Complaint.UomCd = Convert.ToInt32(model.uom_cd);
                    Complaint.Rate = model.Rate;
                    Complaint.RejectionValue = model.rejectionValue;
                    Complaint.RejectionReason = model.RejectionReason;
                    Complaint.UserId = model.UserId;
                    Complaint.ComplaintDt = DateTime.Now;
                    Complaint.RejMemoDt = model.RejMemoDt;
                    Complaint.Datetime = DateTime.Now;
                    Complaint.Updatedby = Convert.ToInt32(model.UserId);
                    Complaint.Updateddate = DateTime.Now;
                    context.SaveChanges();
                    Complaintid = Complaint.ComplaintId;
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            #endregion
            return Complaintid;
        }

        public string CancelJI(ConsigneeComplaints model)
        {

            if (model.AcceptRejornot != "C")
            {
                if (model.Remarks != "" && model.Remarks != null)
                {
                    var complaint = context.T40ConsigneeComplaints.FirstOrDefault(c => c.ComplaintId == model.ComplaintId);

                    if (complaint != null)
                    {
                        complaint.JiRequired = "C";
                        complaint.Remarks = model.Remarks;
                        complaint.UserId = model.UserId;
                        complaint.Datetime = DateTime.Now;
                        complaint.Updatedby = Convert.ToInt32(model.UserId);
                        complaint.Updateddate = DateTime.Now;
                        context.SaveChanges();
                    }
                }
            }
            return model.ComplaintId;
        }

        public string FinalDisposal(ConsigneeComplaints model)
        {
            if (model.DARPurpose == "I" || model.DARPurpose == "J")
            {
                var existingRecord = context.T40ConsigneeComplaints.FirstOrDefault(c => c.ComplaintId == model.ComplaintId);

                if (existingRecord != null)
                {
                    existingRecord.RootCauseAnalysis = model.RootCause;
                    existingRecord.TechRef = model.TechnicalReference;
                    existingRecord.ChksheetStatus = model.Checksheet;
                    existingRecord.AnyOther = model.AnyOther;
                    existingRecord.CapaStatus = model.StatusCAPA;
                    existingRecord.DandarStatus = model.DARStatus;
                    existingRecord.ActionProposed = model.DARPurpose;
                    existingRecord.ActionProposedDt = model.DARDate;
                    existingRecord.PenaltyType = model.Penaltytype;
                    existingRecord.PenaltyDt = model.PenaltyDate;
                    existingRecord.Action = model.FinalRemarks;
                    existingRecord.UserId = model.UserId;
                    existingRecord.Datetime = DateTime.Now;
                    existingRecord.Updatedby = Convert.ToInt32(model.UserId);
                    existingRecord.Updateddate = DateTime.Now;
                    context.SaveChanges();
                }
            }
            else
            {
                var existingRecord = context.T40ConsigneeComplaints.FirstOrDefault(c => c.ComplaintId == model.ComplaintId);

                if (existingRecord != null)
                {
                    existingRecord.RootCauseAnalysis = model.RootCause;
                    existingRecord.TechRef = model.TechnicalReference;
                    existingRecord.ChksheetStatus = model.Checksheet;
                    existingRecord.AnyOther = model.AnyOther;
                    existingRecord.CapaStatus = model.StatusCAPA;
                    existingRecord.DandarStatus = model.DARStatus;
                    existingRecord.ActionProposed = model.DARPurpose;
                    existingRecord.ActionProposedDt = model.DARDate;
                    existingRecord.Action = model.FinalRemarks;
                    existingRecord.UserId = model.UserId;
                    existingRecord.Datetime = DateTime.Now;
                    existingRecord.Updatedby = Convert.ToInt32(model.UserId);
                    existingRecord.Updateddate = DateTime.Now;
                    context.SaveChanges();
                }
            }
            return model.ComplaintId;
        }

        public string JIOutCome(ConsigneeComplaints model)
        {
            if (model.JIDateConclusion != null)
            {
                var complaint = context.T40ConsigneeComplaints.FirstOrDefault(c => c.ComplaintId == model.ComplaintId);

                if (complaint != null)
                {
                    complaint.RejectionReason = model.RejectionReason;
                    complaint.JiDt = model.JIDate;
                    complaint.DefectCd = model.DefectDesc;
                    if (ushort.TryParse(model.JiStatusCd, out ushort parsedValue))
                    {
                        byte? byteValue = parsedValue > byte.MaxValue ? null : (byte)parsedValue;
                        complaint.JiStatusCd = byteValue;
                    }
                    complaint.Status = model.Status;
                    complaint.ConclusionDt = model.JIDateConclusion;
                    complaint.UserId = model.UserId;
                    complaint.Datetime = DateTime.Now;
                    complaint.Updatedby = Convert.ToInt32(model.UserId);
                    complaint.Updateddate = DateTime.Now;
                    context.SaveChanges();
                }
                send_Conclusion_Email(model);
            }

            return model.ComplaintId;
        }

        public string JIChoice(ConsigneeComplaints model)
        {
            string Complaintid = "";

            if (model.AcceptRejornot != "C")
            {
                if ((model.AcceptRejornot == "Y") && (model.JIInspRegion != ""))
                {
                    DataTable dt = new DataTable();
                    string inspRegionFirstChar = model.JIInspRegion.Substring(0, 1);

                    OracleParameter[] par = new OracleParameter[4];
                    par[0] = new OracleParameter("IN_INSP_REGION", OracleDbType.Varchar2, inspRegionFirstChar, ParameterDirection.Input);
                    par[1] = new OracleParameter("IN_JI_REGION", OracleDbType.Varchar2, model.JIInspRegion, ParameterDirection.Input);
                    par[2] = new OracleParameter("IN_COMPLAINT_DT", OracleDbType.Varchar2, DateTime.Now, ParameterDirection.Input);
                    par[3] = new OracleParameter("p_result_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

                    var ds = DataAccessDB.GetDataSet("GENERATE_JI_SNO", par, 1);
                    dt = ds.Tables[0];

                    DataRow firstRow = dt.Rows[0];
                    string JiSno = firstRow["W_JISNO"].ToString().Trim();
                    model.JiSno = JiSno;
                    int year = model.ComplaintDate.Value.Year;
                    if (year >= 2013)
                    {
                        send_IE_Email(model);
                    }
                }

                if (model.AcceptRejornot == "Y")
                {
                    if (model.JiStatusDesc != "" && model.JiStatusDesc != null)
                    {
                        var complaint = context.T40ConsigneeComplaints.FirstOrDefault(c => c.ComplaintId == model.ComplaintId);

                        if (complaint != null)
                        {
                            complaint.JiRequired = model.AcceptRejornot;
                            complaint.JiRegion = model.JIInspRegion;
                            complaint.Remarks = model.Remarks;
                            complaint.JiSno = model.JiSno;
                            complaint.JiIeCd = byte.Parse(model.InspER);
                            complaint.JiDt = model.JIDate;
                            complaint.JiFixDt = model.JiFixDt;
                            complaint.UserId = model.UserId;
                            complaint.Datetime = DateTime.Now;
                            complaint.Updatedby = Convert.ToInt32(model.UserId);
                            complaint.Updateddate = DateTime.Now;
                            context.SaveChanges();
                        }
                    }
                    else
                    {
                        var complaint = context.T40ConsigneeComplaints.FirstOrDefault(c => c.ComplaintId == model.ComplaintId);

                        if (complaint != null)
                        {
                            complaint.JiRequired = model.AcceptRejornot;
                            complaint.JiRegion = model.JIInspRegion;
                            complaint.Remarks = model.Remarks;
                            complaint.JiSno = model.JiSno;
                            //complaint.JiIeCd = byte.Parse(model.InspER);
                            if (ushort.TryParse(model.InspER, out ushort parsedValue))
                            {
                                complaint.JiIeCd = parsedValue;
                            }
                            complaint.JiDt = model.JIDate;
                            complaint.JiFixDt = model.JiFixDt;
                            complaint.JiStatusCd = 0;
                            complaint.UserId = model.UserId;
                            complaint.Datetime = DateTime.Now;
                            complaint.Updatedby = Convert.ToInt32(model.UserId);
                            complaint.Updateddate = DateTime.Now;
                            context.SaveChanges();
                        }
                    }
                }
                else
                {
                    var complaint = context.T40ConsigneeComplaints.FirstOrDefault(c => c.ComplaintId == model.ComplaintId);

                    if (complaint != null)
                    {
                        complaint.JiRequired = model.AcceptRejornot;
                        complaint.NoJiReason = model.NoJIReason;
                        complaint.JiRegion = model.JIInspRegion;
                        complaint.Remarks = model.Remarks;
                        complaint.JiSno = model.JiSno;
                        complaint.JiIeCd = model.JiIeCd;
                        complaint.JiDt = model.JIDate;
                        complaint.JiFixDt = DateTime.Now;
                        complaint.UserId = model.UserId;
                        complaint.Datetime = DateTime.Now;
                        complaint.Updatedby = Convert.ToInt32(model.UserId);
                        complaint.Updateddate = DateTime.Now;
                        context.SaveChanges();
                    }
                }
            }
            else
            {
                return Complaintid;
            }

            return model.ComplaintId;
        }

        public void send_IE_Email(ConsigneeComplaints model)
        {
            string wRegion = "";
            string ie_name = "", ie_email = "", co_email = "";
            SendMailModel SendMailModel = new SendMailModel();

            string inspRegionFirstChar = model.InspRegion.Substring(0, 1);

            switch (inspRegionFirstChar)
            {
                case "N":
                    wRegion = "CONTROLLING MANAGER (JI/NR) \n 12th FLOOR,CORE-II,SCOPE MINAR,LAXMI NAGAR, DELHI - 110092 \n Phone : 011-22029101 \n Fax : 011-22024665";
                    break;
                case "S":
                    wRegion = "CONTROLLING MANAGER (JI/SR) \n CTS BUILDING - 2ND FLOOR, BSNL COMPLEX, NO. 16, GREAMS ROAD,  CHENNAI - 600 006 \n Phone : 044-28292807/044- 28292817 \n Fax : 044-28290359";
                    break;
                case "E":
                    wRegion = "CONTROLLING MANAGER (JI/ER) \n CENTRAL STATION BUILDING(METRO), 56, C.R. AVENUE,3rd FLOOR,KOLKATA-700 012  \n Fax : 033-22348704";
                    break;
                case "W":
                    wRegion = "CONTROLLING MANAGER (JI/WR) \n 5TH FLOOR, REGENT CHAMBER, ABOVE STATUS RESTAURANT,NARIMAN POINT,MUMBAI-400021 \n Phone : 022-68943400/68943445";
                    break;
                case "C":
                    wRegion = "CONTROLLING MANAGER (JI/CR)";
                    break;
            }

            var query = from t20 in context.T20Ics
                        join t09 in context.T09Ies on t20.IeCd equals t09.IeCd
                        join t08 in context.T08IeControllOfficers on t09.IeCoCd equals t08.CoCd
                        where t20.CaseNo == model.CASE_NO && t20.BkNo == model.BK_NO && t20.SetNo == model.SET_NO
                        select new
                        {
                            t09.IeName,
                            t09.IeEmail,
                            t08.CoEmail
                        };


            var result = query.FirstOrDefault();
            if (result != null)
            {
                ie_name = result.IeName;
                ie_email = result.IeEmail;
                co_email = result.CoEmail;
            }

            string mail_body = "";
            if (model.CASE_NO.Substring(0, 1) == inspRegionFirstChar)
            {
                mail_body = $"Dear {ie_name},\n\n Complaint has Registered Vide Complaint No: {model.ComplaintId}, Dated: {model.ComplaintDate} \n Consignee: {model.Consignee} \n PO No. - {model.PO_NO} \n Book No - {model.BK_NO} & Set No - {model.SET_NO} \n Vendor - {model.VEND_NAME} \n Item - {model.ItemDesc} \n Rejected Qty - {model.QtyRejected} \n Rejection Memo No. {model.RejMemoNo} Dated: {model.RejMemoDt} \n Reason for Rejection - {model.RejectionReason}. \n\n You are requested to send your comments on the rejection at TOP Priority. \n NATIONAL INSPECTION HELP LINE NUMBER : 1800 425 7000 (TOLL FREE). \n\n {wRegion}.";
            }
            else
            {
                if (inspRegionFirstChar.Substring(0, 1) == "N")
                {
                    mail_body = "Controlling Manager (JI/NR), \n\n ";
                }
                else if (inspRegionFirstChar.Substring(0, 1) == "W")
                {
                    mail_body = "Controlling Manager (JI/WR), \n\n ";
                }
                else if (inspRegionFirstChar.Substring(0, 1) == "E")
                {
                    mail_body = "Controlling Manager (JI/ER), \n\n ";
                }
                else if (inspRegionFirstChar.Substring(0, 1) == "S")
                {
                    mail_body = "Controlling Manager (JI/SR), \n\n ";
                }
                mail_body = $"\n\n Complaint has Registered Vide Complaint No: {model.ComplaintId}, Dated: {model.ComplaintDate} \n Consignee: {model.Consignee} \n PO No. - {model.PO_NO} \n Book No - {model.BK_NO} & Set No - {model.SET_NO} \n Vendor - {model.VEND_NAME} \n Item - {model.ItemDesc} \n Rejected Qty - {model.QtyRejected} \n Rejection Memo No. {model.RejMemoNo} Dated: {model.RejMemoDt} \n Reason for Rejection - {model.RejectionReason}. \n\n You are requested to send complete Inspection Case with comments for arranging JI at TOP Priority. \n NATIONAL INSPECTION HELP LINE NUMBER : 1800 425 7000 (TOLL FREE). \n\n {wRegion}.\n\n THIS IS AN AUTO GENERATED EMAIL. PLEASE DO NOT REPLY. USE EMAIL GIVEN IN THE REGION ADDRESS";
            }

            string senderEmail = "";

            if (inspRegionFirstChar == "N")
            {
                senderEmail = "nrcomplaints@gmail.com";
            }
            else if (inspRegionFirstChar == "W")
            {
                senderEmail = "wrinspn@rites.com";
            }
            else if (inspRegionFirstChar == "E")
            {
                senderEmail = "erinspn@rites.com";
            }
            else if (inspRegionFirstChar == "S")
            {
                senderEmail = "srinspn@rites.com";
            }

            string cc = "";

            if (model.CASE_NO.StartsWith("N"))
            {
                cc = "nrcomplaints@gmail.com;pklal@rites.com";
            }
            else if (model.CASE_NO.StartsWith("W"))
            {
                cc = "wrinspn@rites.com;harisankarprasad@rites.com";
            }
            else if (model.CASE_NO.StartsWith("E"))
            {
                cc = "erinspn@rites.com;ercomplaints@gmail.com";
            }
            else if (model.CASE_NO.StartsWith("S"))
            {
                cc = "srinspn@rites.com;ravis@rites.com;ravis@rites.com";
            }

            string JI_IE = context.T09Ies.Where(t09 => t09.IeCd == model.ie_cd).Select(t09 => t09.IeEmail).FirstOrDefault();

            if (model.CASE_NO.Substring(0, 1) == inspRegionFirstChar)
            {
                //SendMailModel.To = ie_email + ";" + co_email;
                //SendMailModel.CC = cc + ";" + JI_IE ;
                //SendMailModel.From = "nrinspn@gmail.com"; ;
                //SendMailModel.Subject = "Consignee Complaint Has Been Registered for Joint Inspection (JI)";
                //SendMailModel.Message = mail_body;

                // sender for local mail testing
                senderEmail = "hardiksilvertouch007@outlook.com";
                SendMailModel.From = senderEmail;
                SendMailModel.To = ie_email;
                SendMailModel.To = co_email;
                SendMailModel.Subject = "Consignee Complaint Has Been Registered for Joint Inspection (JI)";
                SendMailModel.Message = mail_body;
            }
            else
            {
                switch (inspRegionFirstChar)
                {
                    case "N":
                        cc = cc + ";nrcomplaints@gmail.com;pklal@rites.com";
                        break;
                    case "S":
                        cc = cc + ";srinspn@rites.com;ravis@rites.com";
                        break;
                    case "W":
                        cc = cc + ";wrinspn@rites.com;ravis@rites.com";
                        break;
                    case "E":
                        cc = cc + ";erinspn@rites.com;ercomplaints@gmail.com";
                        break;
                }


                // sender for local mail testing
                senderEmail = "hardiksilvertouch007@outlook.com";
                SendMailModel.CC = cc;
                SendMailModel.CC = JI_IE;
                SendMailModel.To = ie_email;
                SendMailModel.To = co_email;
                SendMailModel.From = senderEmail;
                SendMailModel.Subject = "Consignee Complaint Has Been Registered for Joint Inspection (JI)";
                SendMailModel.Message = mail_body;

                //SendMailModel.CC = cc;
                //SendMailModel.CC = JI_IE;
                //SendMailModel.To = ie_email;
                //SendMailModel.To = co_email;
                //SendMailModel.From = "nrinspn@gmail.com"; ;
                //SendMailModel.Subject = "Consignee Complaint Has Been Registered for Joint Inspection (JI)";
                //SendMailModel.Message = mail_body;
            }
            bool isSend = false;
            if (Convert.ToString(config.GetSection("MailConfig")["SendMail"]) == "1")
            {
                isSend = pSendMailRepository.SendMail(SendMailModel, null);
            }
        }

        public void send_Conclusion_Email(ConsigneeComplaints model)
        {
            string wRegion = "";
            string sender = "";
            string ie_name = "", ie_email = "", co_email = "";
            SendMailModel SendMailModel = new SendMailModel();

            string inspRegionFirstChar = model.InspRegion.Substring(0, 1);

            if (inspRegionFirstChar == "N")
            {
                wRegion = "NORTHERN REGION \n 12th FLOOR,CORE-II,SCOPE MINAR,LAXMI NAGAR, DELHI - 110092 \n Phone : +918800018691-95 \n Fax : 011-22024665";
                sender = "nrcomplaints@gmail.com";
            }
            else if (inspRegionFirstChar == "S")
            {
                wRegion = "SOUTHERN REGION \n CTS BUILDING - 2ND FLOOR, BSNL COMPLEX, NO. 16, GREAMS ROAD,  CHENNAI - 600 006 \n Phone : 044-28292807/044- 28292817 \n Fax : 044-28290359";
                sender = "srinspn@rites.com";
            }
            else if (inspRegionFirstChar == "E")
            {
                wRegion = "EASTERN REGION \n CENTRAL STATION BUILDING(METRO), 56, C.R. AVENUE,3rd FLOOR,KOLKATA-700 012  \n Fax : 033-22348704";
                sender = "erinspn@rites.com";
            }
            else if (inspRegionFirstChar == "W")
            {
                wRegion = "WESTERN REGION \n 5TH FLOOR, REGENT CHAMBER, ABOVE STATUS RESTAURANT, NARIMAN POINT,MUMBAI-400021 \n Phone : 022-68943400/68943445";
                sender = "wrinspn@rites.com";
            }
            else if (inspRegionFirstChar == "C")
            {
                wRegion = "\"CONTROLLING MANAGER (JI/CR)";
            }

            var query = from t20 in context.T20Ics
                        join t09 in context.T09Ies on t20.IeCd equals t09.IeCd
                        join t08 in context.T08IeControllOfficers on t09.IeCoCd equals t08.CoCd
                        where t20.CaseNo == model.CASE_NO && t20.BkNo == model.BK_NO && t20.SetNo == model.SET_NO
                        select new
                        {
                            IeName = t09.IeName,
                            IeEmail = t09.IeEmail,
                            CoEmail = t08.CoEmail
                        };

            var result = query.FirstOrDefault();

            if (result != null)
            {
                ie_name = result.IeName;
                ie_email = result.IeEmail;
                co_email = result.CoEmail;
            }

            string mail_body = $@"Dear Sir,<br><br> Complaint No: {model.ComplaintId}, Dated: {model.ComplaintDate} <br> Consignee: {model.Consignee} <br> PO No. - {model.PO_NO} <br> Book No -  {model.BK_NO} & Set No - {model.SET_NO} <br> Vendor - {model.VEND_NAME} <br> Item- {model.ItemDesc} <br> Rejected Qty - {model.QtyRejected} <br> Rejection Memo No. {model.RejMemoNo} Dated: {model.RejMemoDt} <br> Reason for Rejection - {model.RejectionReason}. <br><br> The JI case No. {model.JiSno} has been concluded as {model.JiStatusDesc}. <br>Details of the case have been uploaded on the following link: <a href='http://rites.ritesinsp.com/RBS/COMPLAINTS_REPORT/{""}'><b>JI Report</b></a> <br> NATIONAL INSPECTION HELP LINE NUMBER: 1800 425 7000 (TOLL FREE). <br><br> {wRegion}.";
            mail_body = mail_body + "\n\n THIS IS AN AUTO GENERATED EMAIL. PLEASE DO NOT REPLY. USE EMAIL GIVEN IN THE REGION ADDRESS";

            string cc = string.Empty;
            string JI_IE = string.Empty;

            switch (model.CASE_NO.Substring(0, 1))
            {
                case "N":
                    cc = "nrcomplaints@gmail.com;pklal@rites.com;sbu.ninsp@rites.com";
                    break;
                case "W":
                    cc = "wrinspn@rites.com;harisankarprasad@rites.com;sbu.winsp@rites.com";
                    break;
                case "E":
                    cc = "erinspn@rites.com;ercomplaints@gmail.com;sbu.einsp@rites.com";
                    break;
                case "S":
                    cc = "srinspn@rites.com;ravis@rites.com;ravis@rites.com;k.sbu.sinsp@rites.com";
                    break;
            }

            JI_IE = context.T09Ies.Where(t09 => t09.IeCd == model.ie_cd).Select(t09 => t09.IeEmail).FirstOrDefault();

            if (model.CASE_NO.Substring(0, 1) == inspRegionFirstChar)
            {
                // sender for local mail testing
                sender = "hardiksilvertouch007@outlook.com";
                SendMailModel.CC = cc;
                SendMailModel.CC = JI_IE;
                SendMailModel.To = ie_email;
                SendMailModel.To = co_email;
                SendMailModel.To = "nrinspn@gmail.com";
                SendMailModel.From = sender;
                SendMailModel.Subject = "Consignee Complaint Has Concluded";
                SendMailModel.Message = mail_body;

                //SendMailModel.CC = cc;
                //SendMailModel.CC = JI_IE;
                //SendMailModel.To = ie_email;
                //SendMailModel.To = co_email;
                //SendMailModel.From = "nrinspn@gmail.com"; ;
                //SendMailModel.Subject = "Consignee Complaint Has Concluded";
                //SendMailModel.Message = mail_body;
            }
            else
            {
                switch (inspRegionFirstChar)
                {
                    case "N":
                        cc += ";nrcomplaints@gmail.com;pklal@rites.com";
                        break;
                    case "S":
                        cc += ";srinspn@rites.com;ravis@rites.com";
                        break;
                    case "W":
                        cc += ";wrinspn@rites.com;ravis@rites.com";
                        break;
                    case "E":
                        cc += ";erinspn@rites.com;ercomplaints@gmail.com";
                        break;
                }

                // sender for local mail testing
                sender = "hardiksilvertouch007@outlook.com";
                SendMailModel.CC = ie_email;
                SendMailModel.CC = co_email;
                SendMailModel.CC = "nrinspn@gmail.com";
                SendMailModel.To = cc;
                SendMailModel.To = JI_IE;
                SendMailModel.To = "nrinspn@gmail.com";
                SendMailModel.From = sender;
                SendMailModel.Subject = "Consignee Complaint Has Concluded";
                SendMailModel.Message = mail_body;

                //SendMailModel.CC = ie_email;
                //SendMailModel.CC = co_email;
                //SendMailModel.CC = "nrinspn@gmail.com";
                //SendMailModel.To = cc;
                //SendMailModel.To = JI_IE;
                //SendMailModel.To = "nrinspn@gmail.com"; 
                //SendMailModel.From = "nrinspn@gmail.com"; ;
                //SendMailModel.Subject = "Consignee Complaint Has Concluded";
                //SendMailModel.Message = mail_body;
            }
            bool isSend = false;
            if (Convert.ToString(config.GetSection("MailConfig")["SendMail"]) == "1")
            {
                isSend = pSendMailRepository.SendMail(SendMailModel, null);
            }
        }
    }
}
