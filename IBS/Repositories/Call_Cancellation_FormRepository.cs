using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Dynamic;
using System.Globalization;

namespace IBS.Repositories
{
    public class Call_Cancellation_FormRepository : ICall_Cancellation_FormRepository
    {
        private readonly ModelContext context;
        public Call_Cancellation_FormRepository(ModelContext context)
        {
            this.context = context;
        }
        public Call_Cancellation_FormModel GetPOS(string caseNo = "", string callRecvDate = "", string callSno = "")
        {
           
            Call_Cancellation_FormModel model = new();

            var query = from po in context.T13PoMasters
                        join vendor in context.T05Vendors on po.VendCd equals vendor.VendCd
                        where po.CaseNo == caseNo
                        select new Call_Cancellation_FormModel
                        {
                           PO_NO = po.PoNo,
                            PO_DT = Convert.ToString(po.PoDt),
                           VENDOR =  vendor.VendName,
                           CASE_NO = caseNo,
                           CALL_DATE = callRecvDate,
                           CALL_SNO = callSno
                           
                        };

            // Execute the query and retrieve the results
             model = query.FirstOrDefault();
            return model;
        }

        public Call_Cancellation_FormModel GetINSP(string caseNo = "", string callRecvDate = "", string callSno = "")
        {
            Call_Cancellation_FormModel model = new();
            var query = from callRegister in context.T17CallRegisters
                        join ie in context.T09Ies on callRegister.IeCd equals ie.IeCd
                        where callRegister.CaseNo == caseNo &&
                              callRegister.CallRecvDt == Convert.ToDateTime(callRecvDate) &&
                              callRegister.CallSno == Convert.ToByte(callSno)
                        select new Call_Cancellation_FormModel
                        {
                            Inspection_Engineer = ie.IeSname
                        };

            // Execute the query and retrieve the results
            model = query.FirstOrDefault();
            return model;
        }

        public Call_Cancellation_FormModel Combined(string caseNo = "", string callRecvDate = "", string callSno = "")
        {
            Call_Cancellation_FormModel model = GetPOS(caseNo, callRecvDate, callSno);
            var result2 = GetINSP(caseNo, callRecvDate, callSno);
            var result3 = GetCallDetails(caseNo, callRecvDate, callSno);
            if(result3 != null)
            {
                model.CANCEL_CD_1 = result3.CANCEL_CD_1;
                //model.CANCEL_CD_2 = result3.CANCEL_CD_2;
                //model.CANCEL_CD_3 = result3.CANCEL_CD_3;
                //model.CANCEL_CD_4 = result3.CANCEL_CD_4;
                //model.CANCEL_CD_5 = result3.CANCEL_CD_5;
                //model.CANCEL_CD_6 = result3.CANCEL_CD_6;
                //model.CANCEL_CD_7 = result3.CANCEL_CD_7;
                //model.CANCEL_CD_8 = result3.CANCEL_CD_8;
                //model.CANCEL_CD_9 = result3.CANCEL_CD_9;
                //model.CANCEL_CD_10 = result3.CANCEL_CD_10;
                //model.CANCEL_CD_11 = result3.CANCEL_CD_11;
                model.CANCEL_DATE = result3.CANCEL_DATE;
                model.DOCS_SUBMITTED = result3.DOCS_SUBMITTED;
                model.CALL_CANCEL_STATUS = result3.CALL_CANCEL_STATUS;
                model.CANCEL_DESC = result3.CANCEL_DESC;
                if(result3.CANCEL_CD_1 == Convert.ToString(1))
                {
                    model.Chk1 = true;
                }
                else if(result3.CANCEL_CD_1 == Convert.ToString(2))
                {
                    model.Chk2 = true;
                }
                else if (result3.CANCEL_CD_1 == Convert.ToString(3))
                {
                    model.Chk3 = true;
                }
                else if (result3.CANCEL_CD_1 == Convert.ToString(4))
                {
                    model.Chk4 = true;
                }
                else if (result3.CANCEL_CD_1 == Convert.ToString(5))
                {
                    model.Chk5 = true;
                }
                else if (result3.CANCEL_CD_1 == Convert.ToString(6))
                {
                    model.Chk6 = true;
                }
                else if (result3.CANCEL_CD_1 == Convert.ToString(7))
                {
                    model.Chk7 = true;
                }
                else if (result3.CANCEL_CD_1 == Convert.ToString(8))
                {
                    model.Chk8 = true;
                }
                else if (result3.CANCEL_CD_1 == Convert.ToString(9))
                {
                    model.Chk9 = true;
                }
                else if (result3.CANCEL_CD_1 == Convert.ToString(10))
                {
                    model.Chk10 = true;
                }
                else if (result3.CANCEL_CD_1 == Convert.ToString(11))
                {
                    model.Chk11 = true;
                }
                else if (result3.CANCEL_CD_1 == Convert.ToString(12))
                {
                    model.Chk12 = true;
                }
            }
            if(result2 != null)
            {
                model.Inspection_Engineer = result2.Inspection_Engineer;
            }
            return model;
        }
        public Call_Cancellation_FormModel GetCallDetails(string caseNo = "", string callRecvDate = "", string callSno = "")
        {
            Call_Cancellation_FormModel model = new();
            var query = from t19 in context.T19CallCancels
                        join t17 in context.T17CallRegisters
                        on new { t19.CaseNo, t19.CallSno, t19.CallRecvDt } equals new { t17.CaseNo, t17.CallSno, t17.CallRecvDt }
                        where t19.CaseNo == caseNo &&
                              t19.CallRecvDt == Convert.ToDateTime(callRecvDate) &&
                              t19.CallSno == Convert.ToByte(callSno)
                        select new Call_Cancellation_FormModel
                        {
                            CASE_NO = t19.CaseNo,
                            CALL_DATE = t19.CallRecvDt.ToString("dd/MM/yyyy"),
                            CALL_SNO = Convert.ToString(t19.CallSno),
                            CANCEL_CD_1 = Convert.ToString(t19.CancelCd1),
                            CANCEL_CD_2 = Convert.ToString(t19.CancelCd2),
                            CANCEL_CD_3 = Convert.ToString(t19.CancelCd3),
                            CANCEL_CD_4 = Convert.ToString(t19.CancelCd4),
                            CANCEL_CD_5 = Convert.ToString(t19.CancelCd5),
                            CANCEL_CD_6 = Convert.ToString(t19.CancelCd6),
                            CANCEL_CD_7 = Convert.ToString(t19.CancelCd7),
                            CANCEL_CD_8 = Convert.ToString(t19.CancelCd8),
                            CANCEL_CD_9 = Convert.ToString(t19.CancelCd9),
                            CANCEL_CD_10 = Convert.ToString(t19.CancelCd10),
                            CANCEL_CD_11 = Convert.ToString(t19.CancelCd11),
                            CANCEL_DESC = t19.CancelDesc,
                            CANCEL_DATE = Convert.ToString(t19.CancelDate),
                            DOCS_SUBMITTED = t19.DocsSubmitted ?? "", 
                            CALL_CANCEL_STATUS = t17.CallCancelStatus ?? ""
                        };

          


            model = query.FirstOrDefault();

            return model;

        }
        public string SaveDetails(Call_Cancellation_FormModel model,string selectedvalues , string Uname)
        {

            int chkbox = 0;
            var caseNo = model.CASE_NO;
            var callRecvDt = DateTime.ParseExact(model.CALL_DATE, "dd/MM/yyyy", null); // Parse date

            // Fetch the entity you want to update
            var callCancelEntity = context.T19CallCancels
                .Where(e => e.CaseNo == caseNo && e.CallRecvDt == callRecvDt && e.CallSno == Convert.ToByte(model.CALL_SNO))
                .FirstOrDefault();
            if(model.Chk1 == true)
            {
                chkbox = 1;
            }
            else if (model.Chk2 == true)
            {
                chkbox = 2;
            }
            else if (model.Chk3 == true)
            {
                chkbox = 3;
            }
            else if (model.Chk4 == true)
            {
                chkbox = 4;
            }
            else if (model.Chk5 == true)
            {
                chkbox = 5;
            }
            else if (model.Chk6 == true)
            {
                chkbox = 6;
            }
            else if (model.Chk7 == true)
            {
                chkbox = 7;
            }
            else if (model.Chk8 == true)
            {
                chkbox = 8;
            }
            else if (model.Chk9 == true)
            {
                chkbox = 9;
            }
            else if (model.Chk10 == true)
            {
                chkbox = 10;
            }
            else if (model.Chk11 == true)
            {
                chkbox = 11;
            }
            else if (model.Chk12 == true)
            {
                chkbox = 12;
            }
            

            if (callCancelEntity != null)
            {
                callCancelEntity.CancelCd1 = Convert.ToByte(chkbox);
                //callCancelEntity.CancelCd2 = Convert.ToByte(model.Chk2);
                //callCancelEntity.CancelCd3 = Convert.ToByte(model.Chk3);
                //callCancelEntity.CancelCd4 = Convert.ToByte(model.Chk4);
                //callCancelEntity.CancelCd5 = Convert.ToByte(model.Chk5);
                //callCancelEntity.CancelCd6 = Convert.ToByte(model.Chk6);
                //callCancelEntity.CancelCd7 = Convert.ToByte(model.Chk7);
                //callCancelEntity.CancelCd8 = Convert.ToByte(model.Chk8);
                //callCancelEntity.CancelCd9 = Convert.ToByte(model.Chk9);
                //callCancelEntity.CancelCd10 = Convert.ToByte(model.Chk10);
                //callCancelEntity.CancelCd11 = Convert.ToByte(model.Chk11);
                callCancelEntity.CancelDesc = model.CANCEL_DESC;
                callCancelEntity.UserId = Uname;
                callCancelEntity.Datetime = DateTime.Now;
                callCancelEntity.DocsSubmitted = model.DOCS_SUBMITTED;

                // Save changes to the database
                context.SaveChanges();

                SaveDetails2( model,  selectedvalues,  Uname);
            }

            return caseNo;
        }

        public string SaveDetails2(Call_Cancellation_FormModel model, string selectedvalues, string Uname)
        {
            string callCancelStatus = model.CALL_CANCEL_STATUS;
            string caseNo = model.CASE_NO;
            string callRecvDate = model.CALL_DATE;




            var query = context.T17CallRegisters
         .Where(callRegister => callRegister.CaseNo == caseNo
                             && callRegister.CallRecvDt == Convert.ToDateTime(callRecvDate)
                             && callRegister.CallSno == Convert.ToByte(model.CALL_SNO));

            foreach (var record in query)
            {
                record.CallStatus = "C";
                record.CallCancelStatus = callCancelStatus;
            }

            context.SaveChanges();
            return caseNo;
        }

        public string delete_details(string caseNo, string calldate, string callsno)
        {
          




            var query = context.T19CallCancels
         .Where(callCancel => callCancel.CaseNo == caseNo
                           && callCancel.CallRecvDt ==Convert.ToDateTime(calldate)
                           && callCancel.CallSno == Convert.ToByte(callsno));

            context.T19CallCancels.RemoveRange(query);
            context.SaveChanges();
            return caseNo;
        }

    }
}
