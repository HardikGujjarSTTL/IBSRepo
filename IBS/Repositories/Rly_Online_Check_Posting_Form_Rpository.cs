using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace IBS.Repositories
{
    public class Rly_Online_Check_Posting_Form_Rpository : IRly_Online_Check_Posting_Form_Repository
    {
        private readonly ModelContext context;
        public Rly_Online_Check_Posting_Form_Rpository(ModelContext context)
        {
            this.context = context;
        }

        public Rly_Online_Check_Posting_Form_Model GetTextboxValues(string BankNameDropdown, string CHQ_NO, string CHQ_DT, string region)
        {

            Rly_Online_Check_Posting_Form_Model query = null;

            //int i = fill_grid(BankNameDropdown , CHQ_NO , CHQ_DT);


            var query1 = from t24 in context.T24Rvs
                        join t25 in context.T25RvDetails on t24.VchrNo equals t25.VchrNo
                        join b in context.T12BillPayingOfficers on t25.BpoCd equals b.BpoCd into bGroup
                        from b in bGroup.DefaultIfEmpty()
                        join c in context.T03Cities on b.BpoCityCd equals c.CityCd into cGroup
                        from c in cGroup.DefaultIfEmpty()
                        where t25.ChqNo == CHQ_NO
                            && t25.ChqDt == DateTime.ParseExact(CHQ_DT, "dd/MM/yyyy", CultureInfo.InvariantCulture)
                            && t25.BankCd == Convert.ToInt32(BankNameDropdown)
                            && t24.VchrNo.StartsWith(region)
                        select new Rly_Online_Check_Posting_Form_Model
                        {
                            VCHR_NO = t24.VchrNo,
                            VCHR_DT = Convert.ToString(t24.VchrDt),
                            CHQ_NO = t25.ChqNo,
                            CHQ_DT = t25.ChqDt.ToString("dd/MM/yyyy"),
                             BANK_CD =  t25.BankCd,
                            BPO_CD = t25.BpoCd != null
                                ? $"{b.BpoCd}-{b.BpoName}/{(b.BpoAdd != null ? b.BpoAdd + "/" : "")}{(c.Location != null ? c.Location : c.City + "/")}{b.BpoRly}"
                                : t25.Narration,
                            CHQ_AMOUNT = t25.Amount ?? 0,
                            AMOUNT_ADJUSTED = t25.AmountAdjusted ?? 0,
                            AMOUNT_TRANSFERRED = t25.AmtTransferred ?? 0,
                            SUSPENSE_AMOUNT = t25.SuspenseAmt ?? 0,
                            ACC_CD = Convert.ToInt32(t25.AccCd),
                            BPO_RLY = b.BpoRly
                        };

            query = query1.FirstOrDefault();

            return query;
        }

        public int fill_grid(string BankNameDropdown, string CHQ_NO, string CHQ_DT)
        {

            var count = context.T26ChequePostings
            .Where(c => c.ChqNo == CHQ_NO && c.BankCd == Convert.ToInt32(BankNameDropdown))
            .Count();
            return count;
        }


        public DTResult<Rly_Online_Check_Posting_Form_Model> BillList(DTParameters dtParameters , string Region)
        {
            Rly_Online_Check_Posting_Form_Model model = new();

            DateTime fromDate = Convert.ToDateTime(dtParameters.AdditionalValues?.GetValueOrDefault("fromDate"));
            DateTime toDate = Convert.ToDateTime(dtParameters.AdditionalValues?.GetValueOrDefault("toDate"));
            var bpoRly = dtParameters.AdditionalValues?.GetValueOrDefault("bpoRly");
          
            DTResult<Rly_Online_Check_Posting_Form_Model> dTResult = new() { draw = 0 };

            // Use the converted DateTime values in the LINQ query
            var query1 = (from t22 in context.V22Bills
                        join r in context.RitesBillDtls on t22.BillNo equals r.BillNo
                          //where r.BpoType == "R"
                          // && r.PaymentDt >= fromDate
                          // && r.PaymentDt <= toDate
                          // && r.Co6Status == "A"
                          // && r.BpoRly == bpoRly
                          //  && r.PassedAmt > 0
                          //  && t22.AmountReceived == 0
                          //  && r.RegionCode == "E"
                          orderby r.PaymentDt, t22.BillNo
                        select new Rly_Online_Check_Posting_Form_Model
                        {
                          Bill_NO  =  t22.BillNo,
                          INVOICE_NO =   t22.InvoiceNo,
                          BPO_RLY =   r.BpoRly,
                            BILL_AMOUNT = Convert.ToDecimal(t22.BillAmount),
                            AMOUNT_PASSED = Convert.ToDecimal(r.NetAmt),
                           C_07_NO = r.Co7No,
                            C_07_DT = Convert.ToDateTime( r.Co7Date),
                            PAYMENT_DATE = Convert.ToDateTime(r.PaymentDt),
                            BILL_AMOUNT_CLEARED = r.Amount ?? 0,
                            BPO_CD = t22.BpoCd
                        }).Take(20);

            var result = query1.ToList();

            dTResult.recordsTotal = query1.Count();
            dTResult.data = result;
            dTResult.recordsFiltered = query1.Count();
            return dTResult;
        }

        public string Submit(RequestDataModel requestData , string Uname)
        {
            
           
            List<Rly_Online_Check_Posting_Form_Model> selectedData = requestData.selectedData;
            Dictionary<string, string> additionalData = requestData.additionalData;

           foreach(var item in selectedData)
           {
                string billNo = item.Bill_NO;
                  int billAmount = Convert.ToInt32(item.BILL_AMOUNT);
                  double pass_amt = Convert.ToDouble(item.AMOUNT_PASSED);
                  double amtadj = Convert.ToDouble(item.AMOUNT_ADJUSTED);
                int bpo = Convert.ToInt32(item.BPO_CD);
                string ss = Convert.ToString(DateTime.Now);

                string BANK_CD = additionalData["BANK_CD"];
                string CHQ_NO = additionalData["CHQ_NO"];
                string CHQ_DATE = additionalData["CHQ_DATE"];
                string SUSPENSE_AMT = additionalData["SUSPENSE_AMT"];

                bool w_chk_bno = false;

                if(w_chk_bno == true)
                { 
                
                    var newChequePosting = new T26ChequePosting
                    {
                        BankCd = Convert.ToInt32(BANK_CD),
                        ChqNo = CHQ_NO,
                        ChqDt = DateTime.ParseExact(CHQ_DATE, "dd/MM/yyyy", null), // Parse date string to DateTime
                        BillNo = billNo,
                        BillAmount = billAmount,
                        AmountCleared = Convert.ToDecimal(pass_amt),
                        PostingDt = DateTime.ParseExact(ss.Substring(0, 10), "dd/MM/yyyy", null), // Parse date string to DateTime
                        BpoCd = Convert.ToString(bpo),
                        UserId = Uname,
                        Datetime = DateTime.Now
                    };

                    // Add the new record to the context and save changes
                    context.T26ChequePostings.Add(newChequePosting);
                    context.SaveChanges();



                    amtadj = amtadj + pass_amt;

                    var updatedBill = context.T22Bills.FirstOrDefault(b => b.BillNo == billNo);

                    if (updatedBill != null)
                    {
                        // Update AMOUNT_RECEIVED
                        updatedBill.AmountReceived = Convert.ToDecimal(pass_amt);

                        // Update BILL_AMT_CLEARED
                        updatedBill.BillAmtCleared = (updatedBill.BillAmtCleared ?? 0) + Convert.ToDecimal(pass_amt);

                        // Save changes to the database
                        context.SaveChanges();
                    }

                }


                if (amtadj <= Convert.ToDouble(SUSPENSE_AMT))
                {
                    var rvDetails = context.T25RvDetails
                    .Where(d => d.BankCd == Convert.ToInt32(BANK_CD) && d.ChqNo == CHQ_NO && d.ChqDt == Convert.ToDateTime(CHQ_DATE))
                    .FirstOrDefault();

                    if (rvDetails != null)
                    {
                        // Update AMOUNT_ADJUSTED
                        rvDetails.AmountAdjusted = Convert.ToDecimal(amtadj);

                        // Update SUSPENSE_AMT
                        rvDetails.SuspenseAmt = Convert.ToDecimal((rvDetails.SuspenseAmt) - Convert.ToDecimal(amtadj));

                        // Save changes to the database
                        context.SaveChanges();
                    }
                }
           }

           


                return Convert.ToString(true);
        }

        public string InsertT26()
        {
            return null;
        }

    }
}
