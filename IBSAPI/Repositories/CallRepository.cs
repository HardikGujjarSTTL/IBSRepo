using IBSAPI.DataAccess;
using IBSAPI.Helper;
using IBSAPI.Interfaces;
using IBSAPI.Models;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Net.Mail;
using System.Net;

namespace IBSAPI.Repositories
{
    public class CallRepository : ICallRepository
    {

        private readonly ModelContext context;
        public CallRepository(ModelContext context)
        {
            this.context = context;
        }

        public List<CallListModel> GetCallList()
        {
            List<CallListModel> lst = new();

            lst = (from x in context.T01Regions
                   select new CallListModel
                   {
                       ID = x.RegionCode,
                       Name = x.Region,
                       MobileNo = "+91 " + x.MobileNo,
                   }).ToList();
            return lst;
        }

        public int SheduleInspection(SheduleInspectionRequestModel sheduleInspectionRequestModel, int PlanDHours)
        {
            int ID = 0;
            string CallRecvDt = sheduleInspectionRequestModel.CallRecvDt.ToString("dd-MM-yy");
            var query = (from t17 in context.T17CallRegisters
                         join t05 in context.T05Vendors on t17.MfgCd equals t05.VendCd
                         join t03 in context.T03Cities on t05.VendCityCd equals t03.CityCd
                         where t17.CaseNo == sheduleInspectionRequestModel.CaseNo && t17.CallRecvDt.Date == sheduleInspectionRequestModel.CallRecvDt.Date
                         && t17.CallSno == sheduleInspectionRequestModel.CallSno
                         select new
                         {
                             t17.CaseNo,
                             t17.CallRecvDt,
                             t17.CallSno,
                             t17.CallStatus,
                             t17.MfgCd,
                             t17.MfgPlace,
                             t17.IeCd,
                             t17.CoCd,
                             t03.CityCd,
                             t03.City,
                             t17.DtInspDesire
                         }).FirstOrDefault();
            if (query != null)
            {
                if (sheduleInspectionRequestModel.CaseNo != null && sheduleInspectionRequestModel.CallRecvDt != null && sheduleInspectionRequestModel.CallSno > 0)
                {
                    DateTime CDate = DateTime.Now;
                    if (sheduleInspectionRequestModel.InspectionDay == "TD")
                    {
                        if (CDate.Hour > PlanDHours)
                        {
                            ID = 999;
                        }
                        else
                        {
                            T47IeWorkPlan obj = new T47IeWorkPlan();
                            obj.IeCd = query.IeCd;
                            obj.CoCd = Convert.ToByte(query.CoCd);
                            if (sheduleInspectionRequestModel.InspectionDay == "TD")
                            {
                                obj.VisitDt = Convert.ToDateTime(DateTime.Now);
                            }
                            else if (sheduleInspectionRequestModel.InspectionDay == "TM")
                            {
                                obj.VisitDt = Convert.ToDateTime(DateTime.Now.AddDays(1));
                            }
                            obj.CaseNo = query.CaseNo;
                            obj.CallRecvDt = query.CallRecvDt;
                            obj.CallSno = query.CallSno;
                            obj.MfgCd = query.MfgCd;
                            obj.MfgPlace = query.MfgPlace;
                            obj.RegionCode = sheduleInspectionRequestModel.RegionCode;
                            obj.UserId = sheduleInspectionRequestModel.UserId;
                            obj.Datetime = DateTime.Now;
                            context.T47IeWorkPlans.Add(obj);
                            context.SaveChanges();
                            ID = Convert.ToInt32(obj.CallSno);
                        }
                    }
                    else
                    {
                        T47IeWorkPlan obj = new T47IeWorkPlan();
                        obj.IeCd = query.IeCd;
                        obj.CoCd = Convert.ToByte(query.CoCd);
                        if (sheduleInspectionRequestModel.InspectionDay == "TD")
                        {
                            obj.VisitDt = Convert.ToDateTime(DateTime.Now);
                        }
                        else if (sheduleInspectionRequestModel.InspectionDay == "TM")
                        {
                            obj.VisitDt = Convert.ToDateTime(DateTime.Now.AddDays(1));
                        }
                        obj.CaseNo = query.CaseNo;
                        obj.CallRecvDt = query.CallRecvDt;
                        obj.CallSno = query.CallSno;
                        obj.MfgCd = query.MfgCd;
                        obj.MfgPlace = query.MfgPlace;
                        obj.RegionCode = sheduleInspectionRequestModel.RegionCode;
                        obj.UserId = sheduleInspectionRequestModel.UserId;
                        obj.Datetime = DateTime.Now;
                        context.T47IeWorkPlans.Add(obj);
                        context.SaveChanges();
                        ID = Convert.ToInt32(obj.CallSno);
                    }
                }
            }
            return ID;
        }

        public List<CallStatusModel> Get_Call_Status_List()
        {


            var allowedStatuses = new string[] { "M", "A", "R", "U", "S", "G", "W", "T", "PR" };
            List<CallStatusModel> lstStatus = new();

            lstStatus = (from x in context.T21CallStatusCodes
                         where allowedStatuses.Contains(x.CallStatusCd)
                         select new CallStatusModel
                         {
                             CallStatusCd = x.CallStatusCd,
                             CallStatusDesc = x.CallStatusDesc,
                             CallStatusColor = x.CallStatusColor,
                         }).ToList();
            return lstStatus;
        }
        public int CancelInspection(int IeCd, string CaseNo, DateTime PlanDt, DateTime CallRecvDt, int CallSno)
        {
            int ID = 0;
            var T47 = context.T47IeWorkPlans.Where(x => x.IeCd == IeCd && x.VisitDt.Date == PlanDt.Date && x.CaseNo == CaseNo && x.CallRecvDt.Date == CallRecvDt.Date && x.CallSno == CallSno).FirstOrDefault();
            if (T47 != null)
            {
                context.T47IeWorkPlans.RemoveRange(T47);
                context.SaveChanges();
                ID = Convert.ToInt32(T47.CallSno);
            }
            return ID;
        }

        public VenderCallStatusModel CallStatusAcceptRej(VenderCallStatusModel model)
        {
            var groupedResults = (from t49 in context.T49IcPhotoEncloseds
                                  join ic in context.IcIntermediates
                                  on new { t49.CaseNo, t49.BkNo, t49.SetNo } equals new { ic.CaseNo, ic.BkNo, ic.SetNo }
                                  where t49.CaseNo == model.CaseNo &&
                                        t49.CallRecvDt == model.CallRecvDt &&
                                        t49.CallSno == model.CallSno &&
                                        t49.IcPhoto == null
                                  select new { t49.CaseNo, t49.BkNo, t49.SetNo })
                            .ToList() // Fetch data from the database
                            .GroupBy(item => new { item.CaseNo, item.BkNo, item.SetNo }) // Group by in-memory
                            .ToList(); // Materialize the grouped results in-memory

            var no_ic_count = groupedResults.Count();

            var no_of_photo = context.T49IcPhotoEncloseds
                        .Where(t => t.CaseNo == model.CaseNo &&
                                    t.CallRecvDt == model.CallRecvDt &&
                                    t.CallSno == model.CallSno)
                        .Count();

            if (model.CallStatus.Trim() == "" || model.CallStatus == null)
            {
                model.AlertMsg = "Your Call Status is Blank, Kindly Goto Mainmenu and select the call again to update!!!";
                return model;
            }
            else if (model.CallStatus.Trim() == "R" && model.RejectionCharge == "" && model.RejectionCharge == null)
            {
                model.AlertMsg = "Kindly Enter Rejection Charges in Case of Rejection IC!!!";
                return model;
            }
            else if (model.ConsigneeFirm == "0")
            {
                model.AlertMsg = "Select Consignee from the List and then Click on Accepted/Rejected Button";
                return model;
            }
            else if (no_of_photo == 0)
            {
                model.AlertMsg = "Kindly upload the inspections photos and prepare the IC before updating the Call Status to Aceepted/Rejected!!!";
                return model;
            }
            else if (no_ic_count > 0)
            {
                model.AlertMsg = "Kindly upload the PDF file for all ICs, Before updating the Status to Aceepted/Rejected!!!";
                return model;
            }

            var callStatus = context.T17CallRegisters.Where(t => t.CaseNo == model.CaseNo && t.CallRecvDt == model.CallRecvDt && t.CallSno == model.CallSno).Select(t => t.CallStatus).FirstOrDefault();

            var result = context.IcIntermediates.Where(ic => ic.CaseNo == model.CaseNo && ic.CallRecvDt == model.CallRecvDt && ic.CallSno == model.CallSno).ToList();

            if (result.Count > 0)
            {
                if (model.CallStatus == "R" && callStatus != "R")
                {
                    foreach (var entity in result)
                    {
                        int len_item = 0;
                        if (!string.IsNullOrEmpty(entity.ItemDescPo))
                        {
                            if (entity.ItemDescPo.Length > 400)
                            {
                                len_item = 390;
                            }
                            else
                            {
                                len_item = entity.ItemDescPo.Length;
                            }

                            string formatedItem = entity.ItemDescPo.Substring(0, len_item);
                            var existingEntity = context.T18CallDetails.FirstOrDefault(e => e.ItemSrnoPo == entity.ItemSrnoPo && e.CaseNo == model.CaseNo && e.CallSno == model.CallSno && e.CallRecvDt == model.CallRecvDt);
                            if (existingEntity != null)
                            {
                                existingEntity.ItemDescPo = formatedItem;
                                existingEntity.QtyPassed = entity.QtyPassed;
                                existingEntity.QtyRejected = entity.QtyRejected;
                                existingEntity.QtyDue = entity.QtyDue;
                                existingEntity.Updatedby = Convert.ToString(model.UserId);
                                existingEntity.Updateddate = DateTime.Now;
                                context.SaveChanges();
                            }
                        }
                    }
                }

                double wRejCharges = 0;
                string wRejType = "";
                if (callStatus == "R")
                {
                    wRejCharges = Convert.ToDouble(model.RejectionCharge);

                }
                if (model.LocalOutstation != "" && model.LocalOutstation != null)
                {
                    wRejType = model.LocalOutstation;
                }

                if (model.CallStatus == "R" && callStatus != "R")
                {
                    var existingRecord2 = context.T13PoMasters.FirstOrDefault(po => po.CaseNo == model.CaseNo);

                    existingRecord2.PendingCharges = (byte?)((existingRecord2.PendingCharges ?? 0) + 1);
                    context.SaveChanges();
                }
                var existingRecord = context.T17CallRegisters.FirstOrDefault(c => c.CaseNo == model.CaseNo && c.CallRecvDt == model.CallRecvDt && c.CallSno == model.CallSno);

                if (existingRecord != null)
                {
                    existingRecord.CallStatus = model.CallStatus;
                    existingRecord.CallStatusDt = model.CallStatusDt;
                    existingRecord.BkNo = model.DocBkNo;
                    existingRecord.SetNo = model.DocSetNo;
                    existingRecord.UserId = model.UserName;
                    existingRecord.Datetime = DateTime.Now;
                    existingRecord.RejCharges = Convert.ToDecimal(wRejCharges);
                    existingRecord.FifoVoilateReason = model.ReasonFIFO;
                    existingRecord.LocalOrOuts = wRejType;
                    existingRecord.Updatedby = Convert.ToString(model.UserId);
                    existingRecord.Updateddate = DateTime.Now;
                    context.SaveChanges();
                }

                var existingRecord1 = context.IcIntermediates.FirstOrDefault(ic => ic.CaseNo == model.CaseNo && ic.BkNo == model.DocBkNo && ic.SetNo == model.DocSetNo && ic.CallRecvDt == model.CallRecvDt && ic.CallSno == model.CallSno && ic.ConsigneeCd == Convert.ToInt32(model.ConsigneeFirm));

                if (existingRecord1 != null)
                {
                    existingRecord1.ConsgnCallStatus = model.CallStatus;
                    context.SaveChanges();
                }
                if (model.CallStatus == "R")
                {
                    Vendor_Rej_Email(model);
                }
                model.AlertMsg = "Success";

            }
            else
            {
                model.AlertMsg = "Kindly upload the PDF file for all ICs, Before updating the Status to Aceepted/Rejected!!!";
                return model;
            }
            return model;
        }

        public void Vendor_Rej_Email(VenderCallStatusModel model)
        {
            string email = "";
            string Case_Region = model.CaseNo.ToString().Substring(0, 1);
            string wRegion = "";
            string sender = "";
            string wPCity = "";
            string manu_mail = "", mfg_cd = "", manu_name = "", manu_city = "";
            string ie_phone = "", ie_name = "", ie_email = "", ie_co_email = "";
            string vend_cd = "", vend_name = "", vend_email = "", rly_cd = "", vend_city = "";

            var querys = from t13 in context.T13PoMasters
                         join t05 in context.T05Vendors on t13.VendCd equals t05.VendCd
                         join t03 in context.T03Cities on t05.VendCityCd equals t03.CityCd
                         where t13.CaseNo == model.CaseNo.Trim()
                         select new
                         {
                             t13.VendCd,
                             t05.VendName,
                             VEND_ADDRESS = t05.VendAdd2 != null ? t05.VendAdd1 + "/" + t05.VendAdd2 : t05.VendAdd1,
                             t03.City,
                             t05.VendEmail,
                             t13.RegionCode,
                             t13.RlyCd
                         };

            var results = querys.ToList();

            foreach (var item in results)
            {
                vend_cd = item.VendCd.ToString();
                vend_name = item.VendName;
                vend_city = item.City;
                vend_email = item.VendEmail;
                rly_cd = item.RlyCd;

                if (Case_Region == "N") { wRegion = "NORTHERN REGION <BR>12th FLOOR,CORE-II,SCOPE MINAR,LAXMI NAGAR, DELHI - 110092 <BR>Phone : +918800018691-95 <BR>Fax : 011-22024665"; sender = "nrinspn@rites.com"; wPCity = "New Delhi"; }
                else if (Case_Region == "S") { wRegion = "SOUTHERN REGION <BR>CTS BUILDING - 2ND FLOOR, BSNL COMPLEX, NO. 16, GREAMS ROAD,  CHENNAI - 600 006 <BR>Phone : 044-28292807/044- 28292817 <BR>Fax : 044-28290359"; sender = "srinspn@rites.com"; wPCity = "Chennai"; }
                else if (Case_Region == "E") { wRegion = "EASTERN REGION <BR>CENTRAL STATION BUILDING(METRO), 56, C.R. AVENUE,3rd FLOOR,KOLKATA-700 012  <BR>Fax : 033-22348704"; sender = "erinspn@rites.com"; wPCity = "Kolkata"; wPCity = "Kolkata"; }
                else if (Case_Region == "W") { wRegion = "WESTERN REGION <BR>5TH FLOOR, REGENT CHAMBER, ABOVE STATUS RESTAURANT,NARIMAN POINT,MUMBAI-400021 <BR>Phone : 022-68943400/68943445 <BR>"; sender = "wrinspn@rites.com"; wPCity = "Mumbai"; }
                else if (Case_Region == "C") { wRegion = "Central Region"; sender = "crinspn@rites.com"; }
            }

            var query = from t05 in context.T05Vendors
                        join t17 in context.T17CallRegisters on t05.VendCd equals t17.MfgCd
                        join t03 in context.T03Cities on t05.VendCityCd equals t03.CityCd
                        where t17.CaseNo == model.CaseNo.Trim() &&
                              t17.CallRecvDt == model.CallRecvDt &&
                              t17.CallSno == model.CallSno
                        select new
                        {
                            MFG_NAME = t05.VendName,
                            MFG_CITY = t03.City,
                            t05.VendEmail,
                            t17.MfgCd
                        };
            var result = query.FirstOrDefault();

            manu_mail = result.VendEmail;
            mfg_cd = result.MfgCd.ToString();
            manu_name = result.MFG_NAME;
            manu_city = result.MFG_CITY;

            var query2 = from t09 in context.T09Ies
                         join t08 in context.T08IeControllOfficers
                         on t09.IeCoCd equals t08.CoCd
                         where t09.IeCd == Convert.ToInt32(model.IeCd)
                         select new
                         {
                             IE_PHONE_NO = t09.IePhoneNo,
                             CO_NAME = t08.CoName,
                             CO_PHONE_NO = t08.CoPhoneNo,
                             IE_NAME = t09.IeName,
                             IE_EMAIL = t09.IeEmail,
                             CO_Email = t08.CoEmail,
                         };

            var result2 = query2.FirstOrDefault();

            if (result2 != null)
            {
                ie_phone = result2.IE_PHONE_NO;
                ie_name = result2.IE_NAME;
                ie_email = result2.IE_EMAIL;
                ie_co_email = result2.CO_Email;
            }


            string call_letter_dt = "";
            if (Convert.ToString(model.CallLetterDt) == "")
            {
                call_letter_dt = "NIL";
            }
            else
            {
                call_letter_dt = Convert.ToString(model.CallLetterDt);
            }
            string mail_body = "";

            mail_body = vend_name + ", " + vend_city + " / " + manu_name + ", " + manu_city + ",<br><br> Your Call Letter Dated:  " + call_letter_dt + " for inspection of material against Agency.-" + rly_cd + ", PO No. - " + model.PoNo + " & Date - " + model.PoDt + ", Case NO. -" + model.CaseNo + ", registered on date: " + model.CallStatusDt + ", at SNo. " + model.CallSno + ". is Rejected on Date.-" + model.CallStatusDt + " by the concerned Inspection Engineer. - " + ie_name + " Contact No. " + ie_phone + "<br>";

            mail_body = mail_body + "You are requested to submit Rejection charges for the amount of Rs. " + model.CallCancelCharges + "/- + GST, through NEFT/RTGS/Credit card/Debit card/Net banking. </b> in f/o RITES LTD, Payble at " + wPCity + " along with next call.<br><b><u>Please note that call letter without Call Rejection charges will not be accepted.</u></b><br>";

            mail_body = mail_body + "This is for your information and necessary corrective measures please. <br><br> Thanks for using RITES Inspection Services.<br> NATIONAL INSPECTION HELP LINE NUMBER : 1800 425 7000 (TOLL FREE). <br><br>" + wRegion + ".";

            if (vend_cd == mfg_cd && manu_mail != "")
            {
                // Create a MailMessage object
                MailMessage mail = new MailMessage();
                mail.To.Add(manu_mail);
                mail.Bcc.Add("nrinspn@gmail.com");
                mail.From = new MailAddress("nrinspn@gmail.com");
                mail.Subject = "Your Call for Inspection By RITES";
                mail.IsBodyHtml = true; // Set to true if the body contains HTML content
                mail.Body = mail_body;

                // Create a SmtpClient
                SmtpClient smtpClient = new SmtpClient("10.60.50.81"); // Set your SMTP server address
                smtpClient.Credentials = new NetworkCredential("bhavesh.rathod@silvertouch.com", "RB_rathod@123"); // If authentication is required
                                                                                                                   // Send the email
                try
                {
                    smtpClient.Send(mail);
                }
                catch (Exception ex)
                {
                    // Handle the exception (log, display error message, etc.)
                }
                finally
                {
                    // Dispose of resources
                    mail.Dispose();
                    smtpClient.Dispose();
                }
            }
            else if (vend_cd != mfg_cd && vend_email != "" && manu_mail != "")
            {
                // Create a MailMessage object
                MailMessage mail = new MailMessage();
                mail.To.Add(vend_email);
                mail.To.Add(manu_mail);
                mail.Bcc.Add("nrinspn@gmail.com");
                mail.From = new MailAddress("nrinspn@gmail.com");
                mail.Subject = "Your Call for Inspection By RITES";
                mail.IsBodyHtml = true; // Set to true if the body contains HTML content
                mail.Body = mail_body;

                // Create a SmtpClient
                SmtpClient smtpClient = new SmtpClient("10.60.50.81"); // Set your SMTP server address
                smtpClient.Credentials = new NetworkCredential("bhavesh.rathod@silvertouch.com", "RB_rathod@123"); // If authentication is required

                // Send the email
                try
                {
                    smtpClient.Send(mail);
                }
                catch (Exception ex)
                {
                    // Handle the exception (log, display error message, etc.)
                }
                finally
                {
                    // Dispose of resources
                    mail.Dispose();
                    smtpClient.Dispose();
                }
            }
            else if (vend_cd != mfg_cd && (vend_email == "" || manu_mail == ""))
            {
                // Create a MailMessage object
                MailMessage mail = new MailMessage();

                if (string.IsNullOrEmpty(vend_email))
                {
                    mail.To.Add(manu_mail);
                }
                else if (string.IsNullOrEmpty(manu_mail))
                {
                    mail.To.Add(vend_email);
                }
                else
                {
                    mail.To.Add(vend_email);
                    mail.To.Add(manu_mail);
                }

                mail.Bcc.Add("nrinspn@gmail.com");
                mail.From = new MailAddress("nrinspn@gmail.com");
                mail.Subject = "Your Call for Inspection By RITES";
                mail.IsBodyHtml = true; // Set to true if the body contains HTML content
                mail.Body = mail_body;

                SmtpClient smtpClient = new SmtpClient("10.60.50.81"); // Set your SMTP server address
                smtpClient.Credentials = new NetworkCredential("bhavesh.rathod@silvertouch.com", "RB_rathod@123"); // If authentication is required

                try
                {
                    smtpClient.Send(mail);
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    mail.Dispose();
                    smtpClient.Dispose();
                }
            }

            if (vend_email == "" && manu_mail == "")
            {
                MailMessage mail = new MailMessage();
                mail_body = mail_body + "\n As their is no email-id available for Vendor/Manufacturer, So the email cannot be send to Vendor/Manufacturer.";

                mail.To.Add(ie_co_email);
                if (Case_Region == "N")
                {
                    mail.Bcc.Add(ie_email + ";nrinspn@gmail.com" + ";nrinspn.fin@rites.com");
                }
                else
                {
                    mail.Bcc.Add(ie_email + ";nrinspn@gmail.com");
                }
                mail.From = new MailAddress(sender);
                mail.Subject = "Your Call for Inspection By RITES has Rejected.";
                mail.Body = mail_body;
                SmtpClient smtpClient = new SmtpClient("10.60.50.81"); // Set your SMTP server address
                smtpClient.Credentials = new NetworkCredential("bhavesh.rathod@silvertouch.com", "RB_rathod@123"); // If authentication is required
                try
                {
                    smtpClient.Send(mail);
                    email = "success";
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    mail.Dispose();
                    smtpClient.Dispose();
                }
            }

            // return email;
        }

        public string Save(VenderCallStatusModel model, string document)
        {
            if (model.CallStatus == "G" || model.CallStatus == "T")
            {
                string bsCheck = "";
                if (!string.IsNullOrEmpty(model.CallStatus) && !string.IsNullOrEmpty(model.SetNo))
                {
                    bsCheck = context.T10IcBooksets
                                  .Where(bookset => bookset.BkNo.Trim().ToUpper() == model.BkNo
                                  && Convert.ToInt32(model.SetNo) >= Convert.ToInt32(bookset.SetNoFr)
                                  && Convert.ToInt32(model.SetNo) <= Convert.ToInt32(bookset.SetNoTo) && bookset.IssueToIecd == Convert.ToInt32(model.IeCd))
                                  .Select(bookset => Convert.ToString(bookset.IssueToIecd)).FirstOrDefault();

                    var ICTYPE = context.T10IcBooksets
                                .Where(item => item.BkNo == model.BkNo && item.IssueToIecd == Convert.ToInt32(model.IeCd))
                                .Select(item => item.Ictype)
                                .FirstOrDefault();

                    if (ICTYPE == "F")
                    {
                        model.AlertMsg = "This Book number and Set number are Finalized.";
                        return model.AlertMsg;
                    }
                }

                if (!string.IsNullOrEmpty(model.BkNo) && !string.IsNullOrEmpty(model.SetNo) && !string.IsNullOrEmpty(bsCheck) && document != "")
                {
                    var t17Detail = from a in context.T17CallRegisters
                                    where a.CaseNo == model.CaseNo && a.CallRecvDt == DateTime.ParseExact(Convert.ToDateTime(model.CallRecvDt).ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) && a.CallSno == model.CallSno
                                    select a;
                    if (t17Detail.Count() > 0)
                    {
                        foreach (var row in t17Detail)
                        {
                            row.CallStatus = model.CallStatus;
                            row.CallStatusDt = model.CallStatusDt;
                            row.CallCancelStatus = null;
                            row.BkNo = model.BkNo;
                            row.SetNo = model.SetNo;
                            row.UserId = model.UserName;
                            row.Datetime = DateTime.Now;
                            row.FifoVoilateReason = model.ReasonFIFO;
                            row.Updatedby = Convert.ToString(model.UserId);
                            row.Updateddate = DateTime.Now;
                            context.SaveChanges();
                        }
                        model.AlertMsg = "Success";
                    }
                }
                else if (!string.IsNullOrEmpty(model.BkNo) && !string.IsNullOrEmpty(model.SetNo) && string.IsNullOrEmpty(bsCheck))
                {
                    model.AlertMsg = "Book No. and Set No. specified is not issued to You!!!";
                }
                else if (string.IsNullOrEmpty(model.BkNo) || string.IsNullOrEmpty(model.SetNo) || document != " ")
                {
                    model.AlertMsg = "Book No. , Set No. OR Stage IC Photo cannot be left blank!!!";
                }
            }
            else
            {
                var detail = from a in context.T17CallRegisters
                             where a.CaseNo == model.CaseNo && a.CallRecvDt == DateTime.ParseExact(Convert.ToDateTime(model.CallRecvDt).ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) && a.CallSno == model.CallSno
                             select a;
                if (detail.Count() > 0)
                {
                    foreach (var item in detail)
                    {
                        item.CallStatus = model.CallStatus;
                        item.CallStatusDt = model.CallStatusDt;
                        //item.CallCancelStatus = w_call_cancel_status;
                        item.UserId = model.UserName;
                        item.Datetime = DateTime.Now;
                        item.FifoVoilateReason = model.ReasonFIFO;
                        item.Updatedby = Convert.ToString(model.UserId);
                        item.Updateddate = DateTime.Now;
                        context.SaveChanges();
                    }
                }
                model.AlertMsg = "Success";
            }
            return model.AlertMsg;
        }
    }
}
