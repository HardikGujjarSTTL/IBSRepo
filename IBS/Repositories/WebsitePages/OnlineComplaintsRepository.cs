using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Interfaces.WebsitePages;
using IBS.Models;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace IBS.Repositories.WebsitePages
{
    public class OnlineComplaintsRepository : IOnlineComplaintsRepository
    {
        private readonly ModelContext context;
        private readonly ISendMailRepository pSendMailRepository;

        public OnlineComplaintsRepository(ModelContext context, ISendMailRepository pSendMailRepository)
        {
            this.context = context;
            this.pSendMailRepository = pSendMailRepository;
        }

        public string GetItems(string ItemSno, string bkno, string setno, string InspRegionDropdown)
        {
            DataSet ds;

            OracleParameter[] par = new OracleParameter[5];
            par[0] = new OracleParameter("p_bk_no", OracleDbType.Varchar2, bkno, ParameterDirection.Input);
            par[1] = new OracleParameter("p_set_no", OracleDbType.Varchar2, setno, ParameterDirection.Input);
            par[2] = new OracleParameter("p_regioncode", OracleDbType.Varchar2, InspRegionDropdown, ParameterDirection.Input);
            par[3] = new OracleParameter("p_item_srno", OracleDbType.Varchar2, ItemSno, ParameterDirection.Input);
            par[4] = new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output);
            ds = DataAccessDB.GetDataSet("GET_ITEMS_PROC", par, 1);

            DataTable dt = ds.Tables[0];

            string json = JsonConvert.SerializeObject(dt, Formatting.Indented);

            return json;
        }

        public string SaveComplaints(OnlineComplaints onlineComplaints)
        {
            TempOnlineComplaint complaint = null;
            //if (complaintFile != null && Path.GetExtension(complaintFile.FileName).ToUpper() == ".PDF")
            //{
            //    // Save the uploaded file
            //    string uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            //    string uploadedFileName = Guid.NewGuid().ToString() + Path.GetExtension(complaintFile.FileName);
            //    string filePath = Path.Combine(uploadsPath, uploadedFileName);

            //using (var stream = new FileStream(filePath, FileMode.Create))
            //{
            //    complaintFile.CopyTo(stream);
            //}

            DataTable dt = new DataTable();

            OracleParameter[] par = new OracleParameter[2];
            par[0] = new OracleParameter("IN_TEMP_COMPLAINT_DT", OracleDbType.Varchar2, DateTime.Now, ParameterDirection.Input);
            par[1] = new OracleParameter("p_result_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GENERATE_TEMP_COMPLAINT_NO", par, 1);
            dt = ds.Tables[0];

            DataRow firstRow = dt.Rows[0];
            string Compid = firstRow["w_compid"].ToString().Trim();

            complaint = new TempOnlineComplaint
            {
                TempComplaintId = Compid,
                TempComplaintDt = DateTime.Now,
                //RejMemoDt = onlineComplaints.RejMemodate,
                ConsigneeName = onlineComplaints.Name,
                ConsigneeDesig = onlineComplaints.Designation,
                ConsigneeEmail = onlineComplaints.Email,
                ConsigneeMobile = onlineComplaints.MobileNO,
                BkNo = onlineComplaints.BKNo,
                SetNo = onlineComplaints.SetNo,
                InspRegion = onlineComplaints.InspRegion,
                RejMemoNo = onlineComplaints.RejMemono,
                ItemSrnoPo = onlineComplaints.ItemSrnoPo,
                ItemDesc = onlineComplaints.ITEM_DESC_PO,
                QtyOffered = onlineComplaints.QtyperIC,
                QtyRejected = onlineComplaints.QtyRejected,
                UomCd = onlineComplaints.UomCd,
                Rate = onlineComplaints.Rate,
                //RejectionValue = onlineComplaints.RejectionValue,
                RejectionValue = (onlineComplaints.Rate * onlineComplaints.QtyRejected),
                RejectionReason = onlineComplaints.RejectionReason,
                Remarks = onlineComplaints.Remarks,
                CaseNo = onlineComplaints.CaseNo,
                VendCd = onlineComplaints.VendCd,
                ConsigneeCd = onlineComplaints.ConsigneeCd,
                IeCd = onlineComplaints.IE_CD,
                CoCd = onlineComplaints.CoCd,
            };
            SendEmail(onlineComplaints);

            //}
            //else
            //{
            //    msg = "Please select a PDF file to upload.";
            //}


            context.TempOnlineComplaints.Add(complaint);
            context.SaveChanges();

            return Compid;
        }

        public void SendEmail(OnlineComplaints onlineComplaints)
        {
            string wRegion = "";
            if (onlineComplaints.InspRegion == "N")
            {
                wRegion = "CONTROLLING MANAGER (JI/NR) \n 12th FLOOR,CORE-II,SCOPE MINAR,LAXMI NAGAR, DELHI - 110092 \n Phone : 011-22029101 \n Fax : 011-22024665";
            }
            else if (onlineComplaints.InspRegion == "S")
            {
                wRegion = "CONTROLLING MANAGER (JI/SR) \n CTS BUILDING - 2ND FLOOR, BSNL COMPLEX, NO. 16, GREAMS ROAD,  CHENNAI - 600 006 \n Phone : 044-28292807/044- 28292817 \n Fax : 044-28290359";
            }
            else if (onlineComplaints.InspRegion == "E")
            {
                wRegion = "CONTROLLING MANAGER (JI/ER) \n CENTRAL STATION BUILDING(METRO), 56, C.R. AVENUE,3rd FLOOR,KOLKATA-700 012  \n Fax : 033-22348704";
            }
            else if (onlineComplaints.InspRegion == "W")
            {
                wRegion = "CONTROLLING MANAGER (JI/WR) \n 5TH FLOOR, REGENT CHAMBER, ABOVE STATUS RESTAURANT,NARIMAN POINT,MUMBAI-400021 \n Phone : 022-68943400/68943445";
            }
            else if (onlineComplaints.InspRegion == "C")
            {
                wRegion = "CONTROLLING MANAGER (JI/CR)";
            }

            string mail_body = $"Sir/Mam, \n\nOnline Rejection Advice has been Registered Vide Dated: {onlineComplaints.UomCd} \n Consignee Name: {onlineComplaints.Name} \n Book No - {onlineComplaints.BKNo} & Set No - {onlineComplaints.SetNo} \n Item- {onlineComplaints.ITEM_DESC_PO} \n Rejected Qty - {onlineComplaints.QtyRejected} \n Rejection Memo No. {onlineComplaints.RejMemodate} Dated: {onlineComplaints.RejMemodate} \n Reason for Rejection - {onlineComplaints.RejectionReason}.\n NATIONAL INSPECTION HELP LINE NUMBER : 1800 425 7000 (TOLL FREE) \n\n {wRegion}.";

            string sender = "";
            string cc = "";

            if (onlineComplaints.InspRegion == "N")
            {
                sender = "nrinspn@rites.com";
                cc = "nrinspn@rites.com;ramendrakumar@rites.com";
            }
            else if (onlineComplaints.InspRegion == "W")
            {
                sender = "wrinspn@rites.com";
                cc = "wrinspn@rites.com;";
            }
            else if (onlineComplaints.InspRegion == "E")
            {
                sender = "erinspn@rites.com";
                cc = "erinspn@rites.com;ercomplaints@gmail.com";
            }
            else if (onlineComplaints.InspRegion == "S")
            {
                sender = "srinspn@rites.com";
                cc = "srinspn@rites.com;ravis@rites.com;kbjayan@rites.com";
            }
            sender = "hardiksilvertouch007@outlook.com";
            SendMailModel SendMailModel = new SendMailModel();
            SendMailModel.To = cc;
            SendMailModel.CC = onlineComplaints.Email;
            SendMailModel.From = sender;
            SendMailModel.Subject = "Online Rejection Advice Has Been Registered";
            SendMailModel.Message = mail_body;
            try
            {
                bool isSend = pSendMailRepository.SendMail(SendMailModel, null);
            }
            catch (Exception ex)
            {
                // Handle the exception (log, display error message, etc.)
            }
        }
    }
}
