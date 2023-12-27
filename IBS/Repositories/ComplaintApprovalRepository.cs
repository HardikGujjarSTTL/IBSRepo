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
    public class ComplaintApprovalRepository : IComplaintApprovalRepository
    {
        private readonly ModelContext context;
        private readonly ISendMailRepository pSendMailRepository;
        private readonly IConfiguration config;

        public ComplaintApprovalRepository(ModelContext context, ISendMailRepository pSendMailRepository, IConfiguration _config)
        {
            this.context = context;
            this.pSendMailRepository = pSendMailRepository;
            this.config = _config;
        }

        public string GetItems(string InspRegionDropdown)
        {
            var query = from ie in context.T09Ies
                        where ie.IeRegion == InspRegionDropdown
                        orderby ie.IeName
                        select new
                        {
                            ie.IeCd,
                            ie.IeName
                        };

            string json = JsonConvert.SerializeObject(query, Newtonsoft.Json.Formatting.Indented);

            return json;
        }

        public DTResult<OnlineComplaints> GetRejComplaints(DTParameters dtParameters)
        {

            DTResult<OnlineComplaints> dTResult = new() { draw = 0 };
            IQueryable<OnlineComplaints>? query = null;

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

            query = from t in context.TempOnlineComplaints
                    where t.Status == null
                    orderby t.TempComplaintId, t.TempComplaintId
                    select new OnlineComplaints
                    {
                        CaseNo = t.CaseNo,
                        TEMP_COMPLAINT_ID = t.TempComplaintId,
                        TempComplaintDt = t.TempComplaintDt,
                        Name = t.ConsigneeName,
                        Designation = t.ConsigneeDesig,
                        Email = t.ConsigneeEmail,
                        MobileNO = t.ConsigneeMobile,
                        BKNo = t.BkNo,
                        SetNo = t.SetNo,
                        InspRegion = t.InspRegion,
                        RejMemono = t.RejMemoNo,
                        //RejMemodate = t.RejMemoDt,
                        RejectionValue = t.RejectionValue,
                        RejectionReason = t.RejectionReason,
                        Remarks = t.Remarks,
                        //COMP_DOC = "Online_Complaints/" + t.TEMP_COMPLAINT_ID + ".pdf"
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.TEMP_COMPLAINT_ID).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.CaseNo).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public string SubmitAcceptRecord(OnlineComplaints model)
        {
            string msg = "";

            var ComplaintID = context.T40ConsigneeComplaints
                   .Where(c => c.CaseNo == model.CaseNo && c.BkNo == model.BKNo && c.SetNo == model.SetNo)
                   .Select(c => c.ComplaintId)
                   .FirstOrDefault();


            if (model.Regioncode != "N")
            {
                var existingComplaint = context.T40ConsigneeComplaints.FirstOrDefault(c => c.ComplaintId == ComplaintID);

                if (existingComplaint != null)
                {
                    DataTable dt = new DataTable();

                    OracleParameter[] par = new OracleParameter[4];
                    par[0] = new OracleParameter("IN_INSP_REGION", OracleDbType.Varchar2, model.Regioncode, ParameterDirection.Input);
                    par[1] = new OracleParameter("IN_JI_REGION", OracleDbType.Varchar2, model.InspRegion, ParameterDirection.Input);
                    par[2] = new OracleParameter("IN_COMPLAINT_DT", OracleDbType.Varchar2, DateTime.Now, ParameterDirection.Input);
                    par[3] = new OracleParameter("p_result_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

                    var ds = DataAccessDB.GetDataSet("GENERATE_JI_SNO", par, 1);
                    dt = ds.Tables[0];

                    DataRow firstRow = dt.Rows[0];
                    string JiSno = firstRow["W_JISNO"].ToString().Trim();

                    if (model.Regioncode == "N")
                    {
                        existingComplaint.JiRequired = model.AcceptRejornot;
                        existingComplaint.JiRegion = model.InspRegion;
                        existingComplaint.JiSno = JiSno;
                        existingComplaint.JiIeCd = model.JiIeCd;
                        existingComplaint.JiDt = model.JIDate;
                        existingComplaint.JiFixDt = model.JiFixDt;
                        existingComplaint.JiStatusCd = 0;
                        existingComplaint.UserId = model.UserId;
                        existingComplaint.Datetime = DateTime.Now;
                        existingComplaint.Updateddate = DateTime.Now;
                        existingComplaint.Updatedby = Convert.ToInt32(model.UserId);
                    }
                    else
                    {
                        existingComplaint.JiRequired = model.AcceptRejornot;
                        existingComplaint.JiRegion = model.InspRegion;
                        existingComplaint.JiSno = JiSno;
                        existingComplaint.JiDt = model.JIDate;
                        existingComplaint.JiStatusCd = 0;
                        existingComplaint.UserId = model.UserId;
                        existingComplaint.Datetime = DateTime.Now;
                        existingComplaint.Updateddate = DateTime.Now;
                        existingComplaint.Updatedby = Convert.ToInt32(model.UserId);
                    }
                    msg = "Data Saved.";
                    context.SaveChanges();
                }

                send_IE_Email(model);
            }
            else if (model.Regioncode == "N")
            {
                string no_ji_other = "";

                if (model.NoJIReason == "K")
                {
                    no_ji_other = model.NoJiOther;
                }
                else
                {
                    no_ji_other = "";
                }
                var existingComplaint = context.T40ConsigneeComplaints.FirstOrDefault(c => c.ComplaintId == ComplaintID);

                if (existingComplaint != null)
                {
                    existingComplaint.JiRequired = model.AcceptRejornot;
                    existingComplaint.NoJiReason = model.NoJIReason;
                    existingComplaint.NoJiOther = no_ji_other;
                    existingComplaint.UserId = model.UserId;
                    existingComplaint.Datetime = DateTime.Now;
                    existingComplaint.Updateddate = DateTime.Now;
                    existingComplaint.Updatedby = model.UpdatedBy;
                    context.SaveChanges();
                    msg = "Success";
                }
            }

            return msg;
        }
        public string AcceptComplaint(OnlineComplaints model)
        {
            string msg = "";

            var onlineComplaints = context.TempOnlineComplaints
                                    .Where(c => c.CaseNo == model.CaseNo && c.BkNo == model.BKNo && c.SetNo == model.SetNo)
                                    .ToList();
            var firstOnlineComplaint = onlineComplaints.First();

            string bscheck = context.T40ConsigneeComplaints
                            .Where(c => c.BkNo == model.BKNo && c.SetNo == model.SetNo && c.CaseNo == model.CaseNo)
                            .Select(c => c.ComplaintId)
                            .FirstOrDefault();

            if (bscheck == null)
            {
                DataSet ds;

                OracleParameter[] par = new OracleParameter[3];
                par[0] = new OracleParameter("IN_REGION_CD", OracleDbType.Varchar2, model.Regioncode, ParameterDirection.Input);
                par[1] = new OracleParameter("IN_COMPLAINT_DT", OracleDbType.Varchar2, DateTime.Now, ParameterDirection.Input);
                par[2] = new OracleParameter("p_result_cursor", OracleDbType.RefCursor, ParameterDirection.Output);
                ds = DataAccessDB.GetDataSet("GENERATE_COMPLAINT_NO", par, 1);

                DataTable dt = ds.Tables[0];

                DataRow firstRow = dt.Rows[0];
                string ComplaintID = firstRow["W_COMPID"].ToString().Trim();

                model.ComplaintID = ComplaintID;

                T40ConsigneeComplaint obj = new T40ConsigneeComplaint();
                obj.ComplaintId = ComplaintID;
                obj.ComplaintDt = DateTime.Now;
                obj.RejMemoNo = firstOnlineComplaint.RejMemoNo;
                obj.RejMemoDt = firstOnlineComplaint.RejMemoDt;
                obj.CaseNo = firstOnlineComplaint.CaseNo;
                obj.BkNo = firstOnlineComplaint.BkNo;
                obj.SetNo = firstOnlineComplaint.SetNo;
                obj.IeCoCd = (byte?)firstOnlineComplaint.CoCd;
                obj.IeCd = (byte?)firstOnlineComplaint.IeCd;
                obj.ConsigneeCd = firstOnlineComplaint.ConsigneeCd;
                obj.JiRegion = firstOnlineComplaint.InspRegion;
                obj.VendCd = firstOnlineComplaint.VendCd;
                obj.ItemSrnoPo = firstOnlineComplaint.ItemSrnoPo;
                obj.ItemDesc = firstOnlineComplaint.ItemDesc;
                obj.QtyOffered = firstOnlineComplaint.QtyOffered;
                obj.QtyRejected = firstOnlineComplaint.QtyRejected;
                obj.UomCd = firstOnlineComplaint.UomCd;
                obj.Rate = firstOnlineComplaint.Rate;
                obj.RejectionValue = firstOnlineComplaint.RejectionValue;
                obj.RejectionReason = firstOnlineComplaint.RejectionReason;
                obj.Datetime = DateTime.Now;
                obj.Createdby = model.createdBy;
                obj.Createddate = DateTime.Now;

                context.T40ConsigneeComplaints.Add(obj);
                context.SaveChanges();

                var tempOnlineComplaint = context.TempOnlineComplaints.FirstOrDefault(c => c.TempComplaintId == firstOnlineComplaint.TempComplaintId);

                if (tempOnlineComplaint != null)
                {
                    tempOnlineComplaint.Status = "A";
                    tempOnlineComplaint.ComplaintId = ComplaintID;
                    context.SaveChanges();
                }
                msg = "Success";
            }
            else
            {
                msg = "Complaint Already Registered with the Given BK_NO, SET_NO & CASE_NO";
            }

            return msg;
        }

        public OnlineComplaints FindByID(string TEMP_COMPLAINT_ID, string SetNo, string BKNo, string CaseNo)
        {
            OnlineComplaints model = new OnlineComplaints();
            DataTable dt = new DataTable();

            OracleParameter[] par = new OracleParameter[5];
            par[0] = new OracleParameter("p_CaseNo", OracleDbType.Varchar2, CaseNo, ParameterDirection.Input);
            par[1] = new OracleParameter("p_BkNo", OracleDbType.Varchar2, BKNo, ParameterDirection.Input);
            par[2] = new OracleParameter("p_SetNo", OracleDbType.Varchar2, SetNo, ParameterDirection.Input);
            par[3] = new OracleParameter("p_TempComplaintId", OracleDbType.Varchar2, TEMP_COMPLAINT_ID, ParameterDirection.Input);
            par[4] = new OracleParameter("p_ResultCursor", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GET_REJECTIONCOMPLAINT_DETAILS", par, 1);
            dt = ds.Tables[0];

            if (ds == null)
                throw new Exception("complaint Record Not found");
            else
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    model.TEMP_COMPLAINT_ID = dt.Rows[0]["TEMP_COMPLAINT_ID"].ToString();

                    if (DateTime.TryParse(dt.Rows[0]["TEMP_COMPLAINT_DATE"].ToString(), out DateTime tempComplaintDt))
                    {
                        model.TempComplaintDt = tempComplaintDt;
                    }

                    if (DateTime.TryParse(dt.Rows[0]["PO_DT"].ToString(), out DateTime poDt))
                    {
                        model.Date = poDt;
                    }

                    model.BKNo = dt.Rows[0]["BK_NO"].ToString();
                    model.SetNo = dt.Rows[0]["SET_NO"].ToString();
                    model.Regioncode = dt.Rows[0]["region_code"].ToString();
                    model.Contract = dt.Rows[0]["PO_NO"].ToString();

                    if (DateTime.TryParse(dt.Rows[0]["IC_DATE"].ToString(), out DateTime icDt))
                    {
                        model.IC_DT = icDt;
                    }

                    model.IC_NO = dt.Rows[0]["IC_NO"].ToString();
                    model.Consignee = dt.Rows[0]["CONSIGNEE"].ToString();
                    model.Vendor = dt.Rows[0]["VENDOR"].ToString();
                    model.InspER = dt.Rows[0]["IE_NAME"].ToString();
                    model.Item = dt.Rows[0]["ITEM_DESC"].ToString();

                    if (int.TryParse(dt.Rows[0]["QTY_OFFERED"].ToString(), out int qtyperIC))
                    {
                        model.QtyperIC = qtyperIC;
                    }

                    if (int.TryParse(dt.Rows[0]["QTY_REJECTED"].ToString(), out int qtyRejected))
                    {
                        model.QtyRejected = qtyRejected;
                    }

                    if (decimal.TryParse(dt.Rows[0]["REJECTION_VALUE"].ToString(), out decimal rejectionValue))
                    {
                        model.RejectionValue = rejectionValue;
                    }

                    model.RejectionReason = dt.Rows[0]["REJECTION_REASON"].ToString();

                    if (DateTime.TryParse(dt.Rows[0]["rej_memo_dt"].ToString(), out DateTime memodt))
                    {
                        // model.RejMemodate = memodt;
                    }
                    model.RejMemono = dt.Rows[0]["rej_memo_no"].ToString();

                }
                return model;
            }
        }

        public string RejectComp(OnlineComplaints model)
        {
            string msg = "";
            var complaint = context.TempOnlineComplaints.SingleOrDefault(c => c.TempComplaintId == model.TEMP_COMPLAINT_ID);

            if (complaint != null)
            {
                complaint.Status = "R";
                complaint.TempCompRejReason = model.Reasonforreject;
                context.SaveChanges();
                msg = "Reject Successfully!";
            }

            send_Consignee_Email_for_Rejected_Complaints(model);

            return msg;
        }

        public void send_Consignee_Email_for_Rejected_Complaints(OnlineComplaints model)
        {
            string wRegion = "";

            if (model.Regioncode == "N")
            {
                wRegion = "NORTHERN REGION \n 12th FLOOR,CORE-II,SCOPE MINAR,LAXMI NAGAR, DELHI - 110092 \n Phone : +918800018691-95 \n Fax : 011-22024665";
            }
            else if (model.Regioncode == "S")
            {
                wRegion = "SOUTHERN REGION \n CTS BUILDING - 2ND FLOOR, BSNL COMPLEX, NO. 16, GREAMS ROAD,  CHENNAI - 600 006 \n Phone : 044-28292807/044- 28292817 \n Fax : 044-28290359";
            }
            else if (model.Regioncode == "E")
            {
                wRegion = "EASTERN REGION \n CENTRAL STATION BUILDING(METRO), 56, C.R. AVENUE,3rd FLOOR,KOLKATA-700 012  \n Fax : 033-22348704";
            }
            else if (model.Regioncode == "W")
            {
                wRegion = "WESTERN REGION \n 5TH FLOOR, REGENT CHAMBER, ABOVE STATUS RESTAURANT, NARIMAN POINT,MUMBAI-400021 \n Phone : 022-68943400/68943445";
            }
            else if (model.Regioncode == "C")
            {
                wRegion = "Central Region";
            }

            var complaint = context.TempOnlineComplaints.SingleOrDefault(c => c.TempComplaintId == model.TEMP_COMPLAINT_ID);

            if (complaint != null)
            {
                string consignee = complaint.ConsigneeName;
                string consigneeEmail = complaint.ConsigneeEmail;
                string rejMemoNo = complaint.RejMemoNo;
                string callLetterDt = complaint.RejMemoDt.HasValue
                ? complaint.RejMemoDt.Value.ToString("dd/MM/yyyy")
                : "NIL";
            }

            string mailBody = "Dear Sir/Madam,\n\n Online Consignee Complaint vide Rej Memo Letter dated:  " + complaint.RejMemoNo + " for JI of material against PO No. - " + model.Contract + " dated - " + model.Date + ", on date: " + complaint.TempComplaintDt + ". The Complaint is rejected due to following Reason:- " + model.Reasonforreject + ", so Complaint not registered. \n\n Thanks for using RITES Inspection Services. \n NATIONAL INSPECTION HELP LINE NUMBER : 1800 425 7000 (TOLL FREE). \n\n" + wRegion + ".";

            bool isSend = false;
            if (Convert.ToBoolean(config.GetSection("AppSettings")["SendMail"]) == true)
            {
                SendMailModel SendMailModel = new SendMailModel();
                SendMailModel.To = complaint.ConsigneeEmail; ;
                SendMailModel.From = "nrinspn@gmail.com"; ;
                SendMailModel.Subject = "Your Consignee Complaint For RITES";
                SendMailModel.Message = mailBody;

                isSend = pSendMailRepository.SendMail(SendMailModel, null);
            }

        }

        public void send_IE_Email(OnlineComplaints model)
        {
            string wRegion = "";
            string ie_name = "", ie_email = "", co_email = "";
            SendMailModel SendMailModel = new SendMailModel();


            switch (model.Regioncode)
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
                default:
                    wRegion = "Unknown Region";
                    break;
            }

            var query = from t20 in context.T20Ics
                        join t09 in context.T09Ies on t20.IeCd equals t09.IeCd
                        join t08 in context.T08IeControllOfficers on t09.IeCoCd equals t08.CoCd
                        where t20.CaseNo == model.CaseNo && t20.BkNo == model.BKNo && t20.SetNo == model.SetNo
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

            if (model.CaseNo.Substring(0, 1) == model.Regioncode)
            {
                mail_body = $"Dear {model.InspER},\n\n Rejection Advice has Registered Vide Complaint No: {model.TEMP_COMPLAINT_ID}, Dated: {model.TempComplaintDt} \n Consignee: {model.Consignee} \n PO No. - {model.Contract} \n Book No -  {model.BKNo} & Set No - {model.SetNo} & \n Vendor - {model.Vendor} \n Item- {model.Item} \n Rejected Qty - {model.QtyRejected} \n Rejection Memo No. {model.RejMemono} \n Reason for Rejection - {model.RejectionReason}. \n\n You are requested to send your comments on the rejection at TOP Prioity.\n NATIONAL INSPECTION HELP LINE NUMBER : 1800 425 7000 (TOLL FREE). \n\n {wRegion}.";
            }
            else
            {
                switch (model.CaseNo.Substring(0, 1))
                {
                    case "N":
                        mail_body = "Controlling Manager (JI/NR), \n\n ";
                        break;
                    case "W":
                        mail_body = "Controlling Manager (JI/WR), \n\n ";
                        break;
                    case "E":
                        mail_body = "Controlling Manager (JI/ER), \n\n ";
                        break;
                    case "S":
                        mail_body = "Controlling Manager (JI/SR), \n\n ";
                        break;
                }

                mail_body += $"Rejection Advice has Registered Vide Complaint No: {model.TEMP_COMPLAINT_ID}, Dated: {model.TempComplaintDt} \n Consignee: {model.Consignee} \n PO No. - {model.Contract} \n Book No -  {model.BKNo} & Set No - {model.SetNo} \n Vendor - {model.Vendor} \n Item- {model.Item} \n Rejected Qty - {model.QtyRejected} \n Rejection Memo No. {model.RejMemono} \n Reason for Rejection - {model.RejectionReason}. \n\n You are requested to send complete Inspection Case with comments for arranging JI at TOP Prioity. \n\n {wRegion}.";
            }

            //string sender = "";
            string cc = "";

            switch (model.Regioncode)
            {
                case "N":
                    //sender = "nrinspn@rites.com";
                    cc = "nrinspn@rites.com;pklal@rites.com;sbu.ninsp@rites.com";
                    break;
                case "W":
                    //sender = "wrinspn@rites.com";
                    cc = "wrinspn@rites.com;harisankarprasad@rites.com;sbu.winsp@rites.com";
                    break;
                case "E":
                    //sender = "erinspn@rites.com";
                    cc = "erinspn@rites.com;dksinha1958@hotmail.com;sbu.einsp@rites.com";
                    break;
                case "S":
                    //sender = "srinspn@rites.com";
                    cc = "srinspn@rites.com;ravis@rites.com;ravis@rites.com;sbu.sinsp@rites.com";
                    break;
                default:
                    // Handle any default case
                    break;
            }

            var jiIeEmail = context.T09Ies
                      .Where(t09 => t09.IeCd.ToString() == model.InspER)
                      .Select(t09 => t09.IeEmail)
                      .FirstOrDefault();

            if (model.CaseNo.Substring(0, 1) == model.Regioncode)
            {
                SendMailModel.To = ie_email;
                SendMailModel.CC = co_email;
                SendMailModel.CC = cc;
                SendMailModel.CC = jiIeEmail;
                SendMailModel.From = "nrinspn@gmail.com"; ;
                SendMailModel.Subject = "Rejection Advice Has Been Registered for Joint Inspection (JI)";
                SendMailModel.Message = mail_body;
            }
            else
            {
                if (model.Regioncode == "N")
                {
                    cc = cc + ";nrinspn@rites.com;pklal@rites.com;sbu.ninsp@rites.com";
                }
                else if (model.Regioncode == "S")
                {
                    cc = cc + ";srinspn@rites.com;ravis@rites.com;ravis@rites.com;sbu.sinsp@rites.com";
                }
                else if (model.Regioncode == "W")
                {
                    cc = cc + ";wrinspn@rites.com;harisankarprasad@rites.com;sbu.winsp@rites.com";
                }
                else if (model.Regioncode == "E")
                {
                    cc = cc + ";erinspn@rites.com;ercomplaints@gmail.com;sbu.einsp@rites.com";
                }

                SendMailModel.To = cc + ";" + jiIeEmail;
                SendMailModel.CC = co_email;
                SendMailModel.CC = cc;
                SendMailModel.CC = jiIeEmail;
                SendMailModel.From = "nrinspn@gmail.com"; ;
                SendMailModel.Subject = "Rejection Advice Has Been Registered for Joint Inspection (JI)";
                SendMailModel.Message = mail_body;

            }

            bool isSend = pSendMailRepository.SendMail(SendMailModel, null);


        }
    }
}
