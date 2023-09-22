using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Security.Claims;

namespace IBS.Repositories
{
    public class IEClaimFormRepository : IIEClaimFormRepository
    {
        private readonly ModelContext context;
        public IEClaimFormRepository(ModelContext context)
        {
            this.context = context;
        }
        public DTResult<IECliamFormModel> IE_List(DTParameters dtParameters)
        {

            DTResult<IECliamFormModel> dTResult = new() { draw = 0 };
            IQueryable<IECliamFormModel>? query = null;



            string CLAIM_NO = (dtParameters.AdditionalValues?.GetValueOrDefault("CLAIM_NO"));
            string CLAIM_DT = dtParameters.AdditionalValues?.GetValueOrDefault("CLAIM_DT");
            int IE = dtParameters.AdditionalValues?.GetValueOrDefault("IE") == "" ? 0  : Convert.ToInt32(dtParameters.AdditionalValues?.GetValueOrDefault("IE"));
  

            var query1 = (from t45 in context.T45ClaimMasters
                          join t09 in context.T09Ies on t45.IeCd equals t09.IeCd
                          where (string.IsNullOrEmpty(CLAIM_NO) || t45.ClaimNo == CLAIM_NO.Trim())
                             && (string.IsNullOrEmpty(CLAIM_DT) || t45.ClaimDt == DateTime.ParseExact(CLAIM_DT.Trim(), "dd/MM/yyyy", null))
                             && (IE==0 || t45.IeCd == IE)
                          select new IECliamFormModel
                          {
                              CLAIM_NO = t45.ClaimNo,
                              CLAIM_DT = Convert.ToString(t45.ClaimDt),
                              CLAIM_RECIEVE_DT = Convert.ToString(t45.ReceiveDt),
                              IE_NAME = t09.IeName,
                              ID = t45.Id,
                              PAYMENT_VOUCHER_NUMBER = t45.PaymentVchrNo,
                              PAYMENT_VOUCHER_DATE = Convert.ToString(t45.PaymentVchrDt)
                              
                          }).ToList();

            var result = query1.ToList();

            dTResult.recordsTotal = query1.Count();
            dTResult.data = query1;
            dTResult.recordsFiltered = query1.Count();
            return dTResult;
        }

        public DTResult<IECliamFormModel> Manage_grid(DTParameters dtParameters)
        {

            DTResult<IECliamFormModel> dTResult = new() { draw = 0 };
            IQueryable<IECliamFormModel>? query = null;
            string PAYMENT_VOUCHER_NUMBER = (dtParameters.AdditionalValues?.GetValueOrDefault("CLAIM_NO"));


            var claimDetails = context.T46ClaimDetails
               .Where(t => t.ClaimNo == PAYMENT_VOUCHER_NUMBER.Trim())
               .Select(t => new IECliamFormModel
               {
                   CLAIM_NO = t.ClaimNo,
                   CLAIM_HEAD = t.ClaimHead,
                   //CLAIM_HEAD = t.CLAIM_HEAD == 308 ? "308-Conveyance/Fare Charges" :
                   //             t.CLAIM_HEAD == 309 ? "309-Traveling Allowance" :
                   //             t.CLAIM_HEAD == 310 ? "310-Hotel Charges" :
                   //             t.CLAIM_HEAD == 608 ? "608-Telephone/Mobile/Internet Charges" :
                   //             t.CLAIM_HEAD == 629 ? "629-Others" : "Unknown",
                   AMOUNT_CLAIMED = Convert.ToInt32(t.AmtClaimed),
                   AMOUNT_ADMITTED = Convert.ToInt32(t.AmtAdmitted),
                   AMOUNT_DISALLOWED = Convert.ToInt32(t.AmtDisallowed),
                   REMARKS = t.Remarks
               }) .ToList();


            var result = claimDetails.ToList();

            dTResult.recordsTotal = claimDetails.Count();
            dTResult.data = claimDetails;
            dTResult.recordsFiltered = claimDetails.Count();
            return dTResult;
        }

        public string InsertIE(IECliamFormModel model, string Region, int uname)
            {
            var Period_From = string.Concat(model.PERIOD_FROM_YEAR, model.PERIOD_FROM_MONTH);
            var Period_To = string.Concat(model.PERIOD_TO_YEAR, model.PERIOD_TO_MONTH);

            var C_NO = GenerateIE(model, Region);


            var ss = model.CLAIM_DT;
            DateTime parsedDate;
            DateTime vdt;
            DateTime.TryParseExact(model.CLAIM_DT, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate);
            DateTime.TryParseExact(model.CLAIM_RECIEVE_DT, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out vdt);


            var GetValue = context.T45ClaimMasters.Find(Convert.ToDecimal(uname));
            var GetValue2 = context.T46ClaimDetails.Find(model.CLAIM_NO,model.CLAIM_HEAD);

            if (GetValue == null)
            {

                T45ClaimMaster newClaim = new T45ClaimMaster();

                newClaim.ClaimNo = C_NO;
                newClaim.ClaimDt = Convert.ToDateTime(model.CLAIM_DT);
                newClaim.ReceiveDt = Convert.ToDateTime(model.CLAIM_RECIEVE_DT);
                newClaim.IeCd = model.IE;
                newClaim.PeriodFrom = Convert.ToInt32(Period_From);
                newClaim.PeriodTo = Convert.ToInt32(Period_To);
                newClaim.RegionCode = Region;
                newClaim.UserId = Convert.ToString(uname);
                newClaim.Datetime = Convert.ToDateTime(ss);


                context.T45ClaimMasters.Add(newClaim);
                context.SaveChanges();


                T46ClaimDetail obj = new T46ClaimDetail();

                obj.ClaimNo = C_NO;
                obj.ClaimHead = Convert.ToString(model.CLAIM_HEAD);
                obj.AmtClaimed = model.AMOUNT_CLAIMED;
                obj.AmtAdmitted = model.AMOUNT_ADMITTED;
                obj.AmtDisallowed = model.AMOUNT_DISALLOWED;
                obj.Remarks = model.REMARKS;
                obj.UserId = Convert.ToString(uname);
                obj.Datetime = Convert.ToDateTime(ss);

                context.T46ClaimDetails.Add(obj);
                context.SaveChanges();

            }
            else 
            {
                T46ClaimDetail data = new T46ClaimDetail();
                data.ClaimNo = model.CLAIM_NO;
                data.ClaimHead = model.CLAIM_HEAD;
                data.AmtClaimed = model.AMOUNT_CLAIMED;
                data.AmtAdmitted = model.AMOUNT_ADMITTED;
                data.AmtDisallowed = model.AMOUNT_DISALLOWED;
                data.Remarks = model.REMARKS;
                data.UserId = Convert.ToString(uname);
                data.Datetime = Convert.ToDateTime(ss);
                context.T46ClaimDetails.Add(data);
                context.SaveChanges();
            }
            return C_NO;
        }

        public IECliamFormModel FindByID(string CLAIM_NO, string ACTION, decimal ID)
            {



            IECliamFormModel model = new();
            //DateTime dt = Convert.ToDateTime(model.claim);
            //T24Rv tenant2 = context.T24Rvs.Find(VCHR_NO);
            //T25RvDetail tenant = context.T25RvDetails.Find(BANK_CD, CHQ_NO, dt);
            T45ClaimMaster tenant1 = context.T45ClaimMasters.Find(ID);
            //T46ClaimDetail tenant = context.T46ClaimDetails.Find(CLAIM_NO);

            if (tenant1 == null)
            {

                throw new Exception(" Record Not found");
            }
            else
            {
                //model.CLAIM_NO = Convert.ToString(tenant1.ClaimNo);
                model.CLAIM_DT = Convert.ToString(tenant1.ClaimDt);
                model.CLAIM_RECIEVE_DT = Convert.ToString(tenant1.ReceiveDt);
                model.IE = Convert.ToInt32(tenant1.IeCd);
                model.PERIOD_FROM = Convert.ToInt32(tenant1.PeriodFrom);
                model.PERIOD_TO = Convert.ToInt32(tenant1.PeriodTo);
                


            }
            return model;

        }

        public string GenerateIE(IECliamFormModel model, string Region)
        {
            string w_ctr = Region + "/" + model.CLAIM_DT.Substring(8, 2) + "/" + model.CLAIM_DT.Substring(3, 2);

            var claimNos = context.T45ClaimMasters
                .Where(c => c.ClaimNo.Substring(0, 7) == w_ctr)
                .Select(c => c.ClaimNo)
                .ToList();

            var w_snoQuery = claimNos
                .Select(cn => int.TryParse(cn.Substring(8, 3), out int sno) ? sno : 0)
                .DefaultIfEmpty(0)
                .Max();

            int w_sno = w_snoQuery + 1;

            string OUT_CLAIM_NO = $"{w_ctr}/{w_sno:D3}";

            int OUT_ERR_CD = -1;

            if (OUT_CLAIM_NO.Length != 11)
            {
                OUT_ERR_CD = -2;
                // Handle the error condition
            }
            else
            {
                try
                {
                    // Commit the transaction
                }
                catch (Exception ex)
                {
                    if (ex is DbUpdateException || ex is DbUpdateConcurrencyException)
                    {
                        // Handle specific exception types
                    }
                    else
                    {
                        throw;
                    }
                }

                OUT_ERR_CD = 0;
            }

            return OUT_CLAIM_NO;
        }

        public string Payment_Save(string CLAIM_NO , string VCHR_NO , string VCHR_DT)
        {
            DateTime parsedDate;
            string date = Convert.ToString(DateTime.TryParseExact(VCHR_DT, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate));
            var claimMaster = context.T45ClaimMasters.FirstOrDefault(c => c.ClaimNo == CLAIM_NO);

            if(claimMaster != null)
            {
                claimMaster.PaymentVchrNo = VCHR_NO;
                claimMaster.PaymentVchrDt = parsedDate;
                context.SaveChanges();
            }
            return CLAIM_NO;
        }
    }
}
