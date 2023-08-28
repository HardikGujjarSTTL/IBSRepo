using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using System.Data;
using System.Globalization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace IBS.Repositories
{
    public class InterUnitRecieptRepository : IInterUnitRecieptRepository
    {
        private readonly ModelContext context;
        public InterUnitRecieptRepository(ModelContext context)
        {
            this.context = context;
        }

        public List<BPOlist> GetDistinctBPOsByCaseNo(string txtCSNO, string txtBPOtype,string BPOCD)
        {

                    var bpoNameList = (
                 from bpo in context.T12BillPayingOfficers
                 join city in context.T03Cities on bpo.BpoCityCd equals city.CityCd
                 //where bpo.BpoCd == Convert.ToString(city.CityCd) || bpo.BpoRly.Trim().ToUpper().StartsWith(bpo.BpoRly.Trim().ToUpper())
                 //orderby bpo.BpoName
                 where
                                (bpo.BpoCd.Trim().ToUpper() == BPOCD.Trim().ToUpper() ||
                                bpo.BpoRly.Trim().ToUpper().StartsWith(BPOCD.Trim().ToUpper()))
                 orderby bpo.BpoName
                 select new
                 {
                     bpo.BpoCd,
                     BPOName = $"{bpo.BpoName}/{(bpo.BpoAdd ?? "")}/{(city.Location ?? city.City + "/" + city.Location)}/{bpo.BpoRly}"
                 }
             ).Take(100).ToList();
            //var result =  bpoNameList.FirstOrDefault();

            //  return Convert.ToString(result);

            var distinctQuery = bpoNameList.Distinct();

            var DropdownValues = distinctQuery.AsEnumerable().Select(item => new BPOlist
            {
                value = item.BpoCd.ToString(),
                text = item.BPOName
            }).ToList();

            return DropdownValues;

        }

        public List<BPOlist> fill_BPO(string txtCSNO, string lstBPO, string txtBPOtype)
        {

                        var bpoNameList = (
                from bpo in context.T12BillPayingOfficers
                join city in context.T03Cities on bpo.BpoCityCd equals city.CityCd
                where (bpo.BpoCityCd == city.CityCd || bpo.BpoRly.Trim().ToUpper().StartsWith(bpo.BpoRly.Trim().ToUpper()))
                      && bpo.BpoType == txtBPOtype
                orderby bpo.BpoName ascending
                select new
                {
                    bpo.BpoCd,
                    BPOName = bpo.BpoName + "/" + (bpo.BpoAdd ?? "") + "/" + (city.Location ?? city.City + "/" + city.Location) + "/" + bpo.BpoRly
                }
            ).ToList();
          //var result =  bpoNameList.FirstOrDefault();

          //  return Convert.ToString(result);

            var distinctQuery = bpoNameList.Distinct();

            var DropdownValues = distinctQuery.AsEnumerable().Select(item => new BPOlist
            {
                value = item.BpoCd.ToString(),
                text = item.BPOName
            }).ToList();

            return DropdownValues;
        }

        public List<BPOlist> fill_BPO01(string txtCSNO, string lstBPO, string txtBPOtype)
        {

                        var bpoNameList = (
                from bpo in context.T12BillPayingOfficers
                join city in context.T03Cities on bpo.BpoCityCd equals city.CityCd
                where bpo.BpoCd == Convert.ToString(city.CityCd) || bpo.BpoRly.Trim().ToUpper().StartsWith(bpo.BpoRly.Trim().ToUpper())
                orderby bpo.BpoName
                select new
                {
                    bpo.BpoCd,
                    BPOName = $"{bpo.BpoName}/{(bpo.BpoAdd ?? "")}/{(city.Location ?? city.City + "/" + city.Location)}/{bpo.BpoRly}"
                }
            ).ToList();


            var distinctQuery = bpoNameList.Distinct();

            var DropdownValues = distinctQuery.AsEnumerable().Select(item => new BPOlist
            {
                value = item.BpoCd.ToString(),
                text = item.BPOName
            }).ToList();

            return DropdownValues;
        }



        public List<BPOlist> ChkCSNO(string txtCSNO)
        {

                        var query = (
                  from p in context.T13PoMasters
                  join b in context.T14PoBpos on p.CaseNo equals b.CaseNo into bpoGroup
                  from b in bpoGroup.DefaultIfEmpty()
                  join v in context.T05Vendors on p.VendCd equals v.VendCd
                  where p.CaseNo == txtCSNO
                  group new { p, b, v } by new { p.CaseNo, b.BpoCd, v.VendName } into grouped
                  select new
                  {
                      grouped.Key.CaseNo,
                      grouped.Key.BpoCd,
                      grouped.Key.VendName
                  }
              ).ToList();


            //var result = query.FirstOrDefault();

            //return Convert.ToString(result);
            var distinctQuery = query.Distinct();

            var DropdownValues = distinctQuery.AsEnumerable().Select(item => new BPOlist
            {
                value = item.BpoCd.ToString(),
                text = item.VendName.ToString()
            }).ToList();

            return DropdownValues;
        }



        public InterUnitRecieptModel FindByID(string VCHR_NO, int BANK_CD, string CHQ_NO, string CHQ_DT, string VCHR_DT)
        {

            InterUnitRecieptModel model = new();
            DateTime dt = Convert.ToDateTime(CHQ_DT);
            T24Rv tenant2 = context.T24Rvs.Find(VCHR_NO);
            T25RvDetail tenant = context.T25RvDetails.Find(BANK_CD, CHQ_NO, dt);

            if (tenant == null && tenant2 == null)
            {

                throw new Exception("Voucher Record Not found");
            }
            else
            {
                
                model.BANK_CD = Convert.ToString(tenant.BankCd);
                model.CHQ_NO = tenant.ChqNo;
                model.CHQ_DT = Convert.ToString(tenant.ChqDt);
                model.BANK_NAME = Convert.ToString(tenant.BankCd);
                model.AMOUNT = Convert.ToDouble(tenant.Amount);
                
                model.ACC_CD = Convert.ToString(tenant.AccCd);
                
                model.NARRATION = tenant.Narration;
                model.BPO_CD = tenant.BpoCd;

            }
            return model;
        }


        public string InterUnitRecieptSave(InterUnitRecieptModel model, string Region)
        {
            DTResult<InterUnitRecieptModel> dTResult = new() { draw = 0 };
            IQueryable<InterUnitRecieptModel>? query = null;
           
            string VCHR_NO = "";
            string CASE_NO = "";

            if (model.VCHR_NO == null)
            {


                string vchr_dt = model.VCHR_DT.ToString() ?? string.Empty;
                string ss = Region + vchr_dt.Substring(8, 2) + vchr_dt.Substring(3, 2);
                string ss1 = ss.Substring(Convert.ToInt32(model.VCHR_NO), 5);


                var voucher1 = context.T24Rvs
                  .Where(r => r.VchrNo.StartsWith(ss))
                  .Select(r => r.VchrNo.Substring(5, 8)) // Extract the portion after 'N23161'
                  .AsEnumerable()
                  .Select(substring => int.TryParse(substring, out int parsedInt) ? parsedInt : 0) // Parse to integer
                  .DefaultIfEmpty(0)
                  .Max() + 1;

                if (voucher1 != null)
                {
                    VCHR_NO = ss + 00 + voucher1.ToString();// ss + (Convert.ToInt32(voucher1) + 1);
                }
                else
                {
                    VCHR_NO = ss + (Convert.ToInt32(0) + 1);
                }

                
                var count = context.T24Rvs.Count(rv => rv.VchrNo == VCHR_NO);
                if(count == 0)
                {

                }
            }
            var GetValue = context.T24Rvs.Find(model.VCHR_NO);

            var GetValue2 = context.T25RvDetails.Find(Convert.ToInt32(model.BANK_CD), model.CHQ_NO, Convert.ToDateTime(model.CHQ_DT));

            if (GetValue == null)
            {
                DateTime parsedDate;
                DateTime vdt;
                DateTime.TryParseExact(model.iu_Advice_DT, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate);
                DateTime.TryParseExact(model.VCHR_DT, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out vdt);

                T24Rv data = new T24Rv();
                {
                    data.VchrNo = VCHR_NO;
                    data.VchrDt = vdt;
                    data.VchrType = "I";
                };
                context.T24Rvs.Add(data);
                context.SaveChanges();

                var maxSNO = context.T25RvDetails
                   .Where(rv => rv.VchrNo == VCHR_NO)
                   .Select(rv => (int?)rv.Sno)
                   .Max() ?? 0;

                maxSNO = maxSNO + 1;
               

               T25RvDetail obj = new T25RvDetail();
               obj.VchrNo = Convert.ToString(VCHR_NO);
                obj.Sno = maxSNO;
                obj.AccCd = Convert.ToInt32(model.ACC_CD);
                obj.BankCd = Convert.ToInt32(model.BANK_CD);
                obj.ChqNo = model.iu_Advice_no.Trim();
                obj.ChqDt = parsedDate;
                obj.Narration = model.NARRATION;
                obj.Amount = Convert.ToDecimal(model.AMOUNT);
                obj.SuspenseAmt = Convert.ToDecimal(model.AMOUNT);
                obj.BpoCd = model.BPO_CD;
                obj.CaseNo = model.CASE_NO;
                obj.BpoType = model.BPO_TYPE;
                obj.AmountAdjusted = 0;

                context.T25RvDetails.Add(obj);
                context.SaveChanges();





            }
            else
            {
                DateTime parsedDate;
                DateTime.TryParseExact(model.CHQ_DT, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate);
                VCHR_NO = model.VCHR_NO;

                GetValue2.AccCd = Convert.ToInt32(model.ACC_CD);
                GetValue2.BankCd = Convert.ToInt32(model.BANK_CD);
                GetValue2.ChqNo = model.CHQ_NO.Trim();
               // GetValue2.ChqDt = parsedDate;
                GetValue2.Narration = model.NARRATION;
                GetValue2.Amount = Convert.ToDecimal(model.AMOUNT);
                GetValue2.SuspenseAmt = Convert.ToDecimal(model.AMOUNT);
                GetValue2.BpoCd = model.BPO_CD;
                GetValue2.CaseNo = model.CASE_NO;
                GetValue2.BpoType = model.BPO_TYPE;
                GetValue2.AmountAdjusted = 0;

                // context.T25RvDetails.Add(obj);
                context.SaveChanges();


            }
            return VCHR_NO;
        }


        public DTResult<InterUnitRecieptModel> RecieptList(DTParameters dtParameters , string Region)
        {
            string VCHR_NO = dtParameters.AdditionalValues?.GetValueOrDefault("VCHR_NO");
            DTResult<InterUnitRecieptModel> dTResult = new() { draw = 0 };
            IQueryable<InterUnitRecieptModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            var result = (
             from t24 in context.T24Rvs
             join r in context.T25RvDetails on t24.VchrNo equals r.VchrNo
             join a in context.T95AccountCodes on r.AccCd equals a.AccCd into accJoin
             from acc in accJoin.DefaultIfEmpty()
             join b in context.T12BillPayingOfficers on r.BpoCd equals b.BpoCd into bpoJoin
             from bpo in bpoJoin.DefaultIfEmpty()
             join d in context.T94Banks on r.BankCd equals d.BankCd
             join c in context.T03Cities on bpo.BpoCityCd equals c.CityCd into cityJoin
             from city in cityJoin.DefaultIfEmpty()
             where t24.VchrNo.Substring(0, 1) == Region && t24.VchrNo == VCHR_NO && t24.VchrType == "I"

             select new InterUnitRecieptModel
             {
                 VCHR_NO = t24.VchrNo,
                 VCHR_DT = Convert.ToString(t24.VchrDt),
                 ACC_CD = r.AccCd + "-" + (acc != null ? acc.AccDesc : ""),
                 AMOUNT = Convert.ToDouble(r.Amount),
                 BPO_CD = (r.BpoCd != null ? (r.BpoCd + "-" + (bpo.BpoName ?? "") + "/" + (bpo.BpoAdd ?? "") + "/" + (city != null ? (city.Location ?? city.City + "/" + city.Location) : "") + "/" + (bpo.BpoRly ?? "")) : ""),
                 CHQ_NO = r.ChqNo,
                 CHQ_DT = r.ChqDt.ToString("dd/MM/yyyy"),
                 NARRATION = r.Narration,
                 BANK_CD = d.BankName,
                 BANK_NAME = Convert.ToString(d.BankCd)
                 
             }).ToList();

            var query1  = result.ToList();

            dTResult.recordsTotal = result.Count();
            dTResult.data = query1;
            dTResult.recordsFiltered = result.Count();
            return dTResult;

        }

       


    }
}
