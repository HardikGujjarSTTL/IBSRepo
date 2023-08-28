using IBS.DataAccess;
using IBS.Interfaces;
using NuGet.Protocol;
using IBS.Models;
using IBS.Helper;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Mvc.Rendering;   

namespace IBS.Repositories
{   
    public class CheckPostingFormRepository :  ICheckPostingFormRepository
    {
        private readonly ModelContext context;

        public CheckPostingFormRepository(ModelContext context)
        {
            this.context = context;
        }
        public DTResult<CheckPostingFormModel> BillList(DTParameters dtParameters)
        {
            CheckPostingFormModel model = new();
            int count = 0;

            int BankNameDropdown = Convert.ToInt32(dtParameters.AdditionalValues?.GetValueOrDefault("BankNameDropdown"));
            string CHQ_NO = dtParameters.AdditionalValues?.GetValueOrDefault("CHQ_NO");
            var DATE = dtParameters.AdditionalValues?.GetValueOrDefault("CHQ_DATE");
            DateTime CHQ_DATE = Convert.ToDateTime(DateTime.ParseExact(DATE, "dd/MM/yyyy", null).ToString("dd/MM/yyyy"));
            DTResult<CheckPostingFormModel> dTResult = new() { draw = 0 };
            IQueryable<CheckPostingFormModel>? query = null;


            // Use the converted DateTime values in the LINQ query
            var query1 = from t26 in context.T26ChequePostings
            join t22 in context.V22Bills on t26.BillNo equals t22.BillNo
            join b in context.T12BillPayingOfficers on t22.BpoCd equals b.BpoCd
                        join c in context.T03Cities on b.BpoCityCd equals c.CityCd
                        where t26.BankCd == BankNameDropdown &&
                              t26.ChqNo == CHQ_NO &&
                              t26.ChqDt == CHQ_DATE

                         select new CheckPostingFormModel
                        {
                          BANK_CD = Convert.ToInt32( t26.BankCd),
                          CHQ_NO =   t26.ChqNo,
                            CHQ_DATE = Convert.ToDateTime(t26.ChqDt),
                          BILL_NO =   t26.BillNo,
                           BILL_AMOUNT = Convert.ToDouble(t22.BillAmount),
                          AMOUNT_CLEARED = Convert.ToDouble(t26.AmountCleared),
                           POSTING_DATE = Convert.ToDateTime(t26.PostingDt),
                            BILL_AMOUNT_CLEARED = Convert.ToDouble(t22.BillAmtCleared) ,
                            BPO_CD = b.BpoCd + "-" + b.BpoName + "/" +
                                       (b.BpoAdd ?? "") + "/" +
                                       (c.Location ?? c.City) + "/" +
                                       b.BpoRly
                        };

            var result = query1.ToList();
                
            dTResult.recordsTotal = query1.Count();
            dTResult.data = result;
            dTResult.recordsFiltered = query1.Count();
            return dTResult;
        }

        public CheckPostingHeader GetTextboxValues(string BankNameDropdown , string CHQ_NO , string CHQ_DATE , string region)
        {

            CheckPostingHeader query = null;

           
            var query1 = from t24 in context.T24Rvs
                        join t25 in context.T25RvDetails on t24.VchrNo equals t25.VchrNo
                        join b in context.T12BillPayingOfficers on t25.BpoCd equals b.BpoCd into bGroup
                        from b in bGroup.DefaultIfEmpty()
                        join c in context.T03Cities on b.BpoCityCd equals c.CityCd into cGroup
                        from c in cGroup.DefaultIfEmpty()
                        where t25.ChqNo == CHQ_NO && t25.ChqDt == Convert.ToDateTime(CHQ_DATE) && t25.BankCd == Convert.ToInt32(BankNameDropdown) && t24.VchrNo.StartsWith(region)
                         select new CheckPostingHeader
                        {
                            VCHR_NO =  t24.VchrNo,
                            VCHR_DT = Convert.ToDateTime(t24.VchrDt),
                            CHQ_NO =  t25.ChqNo,
                            CHQ_DATE = t25.ChqDt,
                           BANK_CD =  t25.BankCd,
                            BPO_CD = t25.BpoCd != null? $"{b.BpoCd}-{b.BpoName}/{b.BpoAdd ?? ""}/{c.Location ?? c.City}/{b.BpoRly}"
                                : t25.Narration,
                            Cheaque_amount = Convert.ToDouble(t25.Amount),
                            Amount_Adjusted = Convert.ToDouble(t25.AmountAdjusted),
                            amount_transferred = Convert.ToDouble(t25.AmtTransferred),
                            Suspense_Amt =Convert.ToDouble(t25.SuspenseAmt),    
                            ACC_CD = Convert.ToInt32(t25.AccCd)
                        };

          query = query1.FirstOrDefault();
            
            return query;
        }

        public CheckPostingbillInvoice ChkBillNo(string RadioBill , string Region)
        {
            CheckPostingbillInvoice query = null;
            var query1 = from bill in context.V22Bills
                        select new CheckPostingbillInvoice
                        {
                           BILL_NO = bill.BillNo,
                           Invoice_NO =  bill.InvoiceNo,
                            Bill_DT= Convert.ToString( bill.BillDt),
                          CASE_NO =  bill.CaseNo,
                           BK_NO =  bill.BkNo,
                           SET_NO =  bill.SetNo,
                            BPO_NAME = $"{bill.BpoCd}-{bill.BpoName}/{bill.BpoRly}/{(bill.BpoAdd ?? "")}{bill.BpoCity}",
                            BILL_AMOUNT = bill.BillAmount ?? 0,
                            AMOUNT_RECIEVED = bill.AmountReceived ?? 0,
                            BILL_AMT_CLEARED = bill.BillAmtCleared ?? 0,
                            TDS = (bill.Tds ?? 0) + (bill.TdsSgst ?? 0) + (bill.TdsCgst ?? 0) + (bill.TdsIgst ?? 0),
                            RETENTION_MONEY = bill.RetentionMoney ?? 0,
                            WRITE_OFF_AMT = bill.WriteOffAmt ?? 0,
                            CNOTE_AMT = bill.CnoteAmount ?? 0,
                           BPO_CD = bill.BpoCd
                        };

            query = query1.FirstOrDefault();

            return query;
        }

        public CheckPostingFormModel FindByID( string billNo)
            {

           
            CheckPostingFormModel query = null;
            var query1 = from bill in context.V22Bills
                         where bill.BillNo == billNo
                         select new CheckPostingFormModel
                        {
                            BILL_DATE = Convert.ToDateTime(bill.BillDt),
                           CASE_NO =  bill.CaseNo,
                           BK_NO =  bill.BkNo,
                          SET_NO =  bill.SetNo,
                            BPO_NAME = $"{bill.BpoCd}-{bill.BpoName}/{bill.BpoRly}/{(bill.BpoAdd ?? "")}/{bill.BpoCity}",
                           BPO_CD = bill.BpoCd,
                           AMOUNT_RECIEVED = Convert.ToDecimal( bill.AmountReceived),
                            BILL_AMOUNT_CLEARED = Convert.ToDouble(bill.BillAmtCleared),
                            TDS = Convert.ToDouble((bill.Tds) + (bill.TdsSgst ) + (bill.TdsCgst) + (bill.TdsIgst)),
                            Retention_Money = Convert.ToDouble(bill.RetentionMoney),
                            WriteOffAmount = Convert.ToDouble(bill.WriteOffAmt),
                            Cnote = Convert.ToDouble(bill.CnoteAmount)
                        };

            query = query1.FirstOrDefault();

            return query;

        }
        public string UpdateData(CheckPostingFormModel model )
        {
           
            var GetValue = context.T26ChequePostings.Find(model.BANK_CD,model.CHQ_NO,model.CHQ_DATE);

            if (GetValue == null)
            {
                T26ChequePosting data = new T26ChequePosting();
                data.BankCd = model.BANK_CD;
                data.ChqNo = model.CHQ_NO;
                data.ChqDt = model.CHQ_DATE;
                data.BillNo = model.BILL_NO;
                data.BillAmount = Convert.ToDecimal(model.BILL_AMOUNT);
                data.AmountCleared = Convert.ToDecimal(model.AMOUNT_CLEARED);
                data.PostingDt = model.POSTING_DATE;
                data.BpoCd = model.BPO_CD;
                //data.UserId = Uname;
                data.Datetime = model.DATETIME;

                context.T26ChequePostings.Add(data);
                context.SaveChanges();
            }
            else
            {
                T26ChequePosting data = new T26ChequePosting();
                GetValue.BankCd = model.BANK_CD;
                GetValue.ChqNo = model.CHQ_NO;
                GetValue.ChqDt= model.CHQ_DATE;
                GetValue.BillNo = model.BILL_NO;
                GetValue.BillAmount = Convert.ToDecimal(model.BILL_AMOUNT);
                GetValue.AmountCleared = Convert.ToDecimal(model.AMOUNT_CLEARED);
                GetValue.PostingDt = model.POSTING_DATE; ;
                GetValue.BpoCd= model.BPO_CD;
                //GetValue.UserId = Uname;
                GetValue.Datetime = model.DATETIME;

                context.T26ChequePostings.Add(data);
                context.SaveChanges();
            }

            return model.BILL_NO;
        }


        public CheckPostingbillInvoice ChkInvoiceNo(string InvoiceBill , string Region)
        {
            CheckPostingbillInvoice query = null;
            var query1 = from bill in context.V22Bills
                         select new CheckPostingbillInvoice
                         {
                             BILL_NO = bill.BillNo,
                             Invoice_NO = bill.InvoiceNo,
                             Bill_DT = Convert.ToString(bill.BillDt),
                             CASE_NO = bill.CaseNo,
                             BK_NO = bill.BkNo,
                             SET_NO = bill.SetNo,
                             BPO_NAME = $"{bill.BpoCd}-{bill.BpoName}/{bill.BpoRly}/{(bill.BpoAdd ?? "")}{bill.BpoCity}",
                             BILL_AMOUNT = bill.BillAmount ?? 0,
                             AMOUNT_RECIEVED = bill.AmountReceived ?? 0,
                             BILL_AMT_CLEARED = bill.BillAmtCleared ?? 0,
                             TDS = (bill.Tds ?? 0) + (bill.TdsSgst ?? 0) + (bill.TdsCgst ?? 0) + (bill.TdsIgst ?? 0),
                             RETENTION_MONEY = bill.RetentionMoney ?? 0,
                             WRITE_OFF_AMT = bill.WriteOffAmt ?? 0,
                             CNOTE_AMT = bill.CnoteAmount ?? 0,
                             BPO_CD = bill.BpoCd
                         };

            query = query1.FirstOrDefault();

            return query;
        }

    }
}
