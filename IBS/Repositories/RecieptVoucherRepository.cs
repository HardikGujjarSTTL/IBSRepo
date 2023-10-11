using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;

namespace IBS.Repositories
{
    public class RecieptVoucherRepository : IRecieptVoucherRepository
    {
        public string VNO, Action, VTYPE;
        private readonly ModelContext context;

        public RecieptVoucherRepository(ModelContext context)
        {
            this.context = context;
        }

        public RecieptVoucherModel FindByID(string VCHR_NO, int BANK_CD, string CHQ_NO, string CHQ_DT)
        {

            RecieptVoucherModel model = new();
            DateTime dt = Convert.ToDateTime(CHQ_DT);
            T24Rv tenant2 = context.T24Rvs.Find(VCHR_NO);
            T25RvDetail tenant = context.T25RvDetails.Find(BANK_CD, CHQ_NO, dt);

            if (tenant == null && tenant2 == null)
            {

                throw new Exception("Voucher Record Not found");
            }
            else
            {
                model.VCHR_DT = Convert.ToString(tenant2.VchrDt);
                model.BANK_CD = tenant.BankCd;
                model.CHQ_NO = tenant.ChqNo;
                model.CHQ_DT = Convert.ToString(tenant.ChqDt);
                model.BANK_NAME = Convert.ToString(tenant.BankCd);
                model.AMOUNT = Convert.ToDouble(tenant.Amount);
                model.SAMPLE_NO = tenant.SampleNo;
                model.ACC_CD = Convert.ToString(tenant.AccCd);
                model.CASE_NO = tenant.CaseNo;
                model.NARRATION = tenant.Narration;
                model.BPO_CD = tenant.BpoCd;

            }
            return model;
        }

        public DTResult<RecieptVoucherModel> GetVoucherList(DTParameters dtParameters)
        {
            DTResult<RecieptVoucherModel> dTResult = new() { draw = 0 };
            IQueryable<RecieptVoucherModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "VCHR_NO";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "VCHR_NO";
                orderAscendingDirection = true;
            }
            query = from l in context.ViewVoucherLists
                        //join i in context.T25RvDetails on l.VchrNo equals i.VchrNo
                        //join j in context.T95AccountCodes on i.AccCd equals j.AccCd
                        //join k in context.T12BillPayingOfficers on i.BpoCd equals k.BpoCd
                        //join m in context.T94Banks on i.BankCd equals m.BankCd
                        //join c in context.T03Cities on k.BpoCityCd equals c.CityCd
                        //where l.Isdeleted == 0  (nvl(T24.VCHR_TYPE, 'X') <> 'I')

                    select new RecieptVoucherModel
                    {
                        VCHR_NO = l.VchrNo,
                        //SNO = Convert.ToInt32(l.Sno),
                        CHQ_NO = l.ChqNo,
                        CHQ_DT = l.ChqDt,
                        AMOUNT = Convert.ToDouble(l.Amount),
                        BANK_NAME = l.BankName,
                        BANK_CD = l.BankCd,
                        BPO_CD = l.BpoName,
                        ACC_CD = Convert.ToString(l.AccDesc),
                        CASE_NO = l.CaseNo,
                        NARRATION = l.Narration,
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.VCHR_NO).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.CHQ_NO).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();
            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();
            dTResult.draw = dtParameters.Draw;
            return dTResult;
        }

        public string VoucherDetailsSave(RecieptVoucherModel model, string Region)
        {
            DTResult<RecieptVoucherModel> dTResult = new() { draw = 0 };
            IQueryable<RecieptVoucherModel>? query = null;

            string VCHR_NO = "";
            string CASE_NO = "";

            if (model.VCHR_NO == null)
            {


                string vchr_dt = model.VCHR_DT.ToString() ?? string.Empty;
                string ss = Region + vchr_dt.Substring(8, 2) + vchr_dt.Substring(3, 2);
                string ss1 = ss.Substring(Convert.ToInt32(model.VCHR_NO), 5);
                var voucher1 = context.T24Rvs
                  .Where(r => r.VchrNo.StartsWith(ss))
                  .Select(r => r.VchrNo.Substring(5, 8))
                  .AsEnumerable()
                  .Select(substring => int.TryParse(substring, out int parsedInt) ? parsedInt : 0)
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
            }
            var GetValue = context.T24Rvs.Find(model.VCHR_NO);

            var GetValue2 = context.T25RvDetails.Find(Convert.ToInt32(model.BANK_CD), model.CHQ_NO, Convert.ToDateTime(model.CHQ_DT));

            var GetValue3 = context.T13PoMasters.Find(model.CASE_NO);

            if (GetValue == null)
            {
                T24Rv data = new T24Rv();
                data.VchrNo = Convert.ToString(VCHR_NO);
                data.VchrDt = Convert.ToDateTime(DateTime.ParseExact(model.VCHR_DT, "MM/dd/yyyy", null).ToString("dd/MM/yyyy"));
                data.BankCd = Convert.ToByte(model.BANK_CD);
                data.VchrType = model.VCHR_TYPE;
                context.T24Rvs.Add(data);
                context.SaveChanges();
                VCHR_NO = Convert.ToString(data.VchrNo);

                T13PoMaster insert = new T13PoMaster();
                insert.CaseNo = model.CASE_NO;
                context.T13PoMasters.Add(insert);
                context.SaveChanges();

                T25RvDetail obj = new T25RvDetail();
                obj.VchrNo = Convert.ToString(VCHR_NO);
                obj.ChqNo = model.CHQ_NO;
                obj.BankCd = Convert.ToByte(model.BANK_NAME);
                obj.ChqDt = Convert.ToDateTime(DateTime.ParseExact(model.CHQ_DT, "MM/dd/yyyy", null).ToString("dd/MM/yyyy"));
                obj.Amount = Convert.ToDecimal(model.AMOUNT);
                obj.SampleNo = model.SAMPLE_NO;
                //obj.AccCd = Convert.ToByte(model.ACC_CD);
                obj.CaseNo = model.CASE_NO;
                obj.Narration = model.NARRATION;
                obj.BpoCd = model.BPO_CD;
                //  obj.CaseNo = model.CASE_NO;
                obj.BpoCd = model.BPO_CD;
                obj.BpoType = model.BPO_TYPE;

                context.T25RvDetails.Add(obj);
                context.SaveChanges();





            }
            else
            {
                DateTime parsedDate;
                DateTime vdt;
                DateTime.TryParseExact(model.CHQ_DT, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate);
                DateTime.TryParseExact(model.VCHR_DT, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out vdt);
                VCHR_NO = model.VCHR_NO;

                T24Rv data = new T24Rv();
                GetValue.VchrNo = Convert.ToString(VCHR_NO);
                // GetValue.VchrDt = Convert.ToDateTime(DateTime.ParseExact(model.VCHR_DT, "MM/dd/yyyy", null).ToString("dd/MM/yyyy"));
                GetValue.BankCd = Convert.ToByte(model.BANK_CD);
                GetValue.VchrType = model.VCHR_TYPE;
                // context.T24Rvs.Add(data);
                context.SaveChanges();
                VCHR_NO = Convert.ToString(GetValue.VchrNo);

                T25RvDetail obj = new T25RvDetail();
                GetValue2.VchrNo = Convert.ToString(VCHR_NO);
                GetValue2.BankCd = Convert.ToByte(model.BANK_NAME);
                obj.ChqDt = parsedDate;
                GetValue2.Amount = Convert.ToDecimal(model.AMOUNT);
                GetValue2.SampleNo = model.SAMPLE_NO;
                GetValue2.AccCd = Convert.ToInt32(model.ACC_CD);
                GetValue2.CaseNo = model.CASE_NO;
                GetValue2.Narration = model.NARRATION;
                GetValue2.BpoCd = model.BPO_CD;
                // GetValue2.CaseNo = model.CASE_NO;
                GetValue2.BpoCd = model.BPO_CD;
                GetValue2.BpoType = model.BPO_TYPE;
                GetValue2.Narration = model.NARRATION;

                // context.T25RvDetails.Add(obj);
                context.SaveChanges();


            }
            return VCHR_NO;
        }

        public string ChkCSNO(string txtCSNO, string lstBPO, out string Narrt)
        {

            var query = from p in context.T13PoMasters
                        join b in context.T14PoBpos on p.CaseNo equals b.CaseNo into bpoJoin
                        from bpo in bpoJoin.DefaultIfEmpty()
                        join v in context.T05Vendors on p.VendCd equals v.VendCd
                        where p.CaseNo == txtCSNO
                        group new { p, bpo, v } by new { p.CaseNo, bpo.BpoCd, v.VendName } into g
                        select new
                        {
                            CaseNo = g.Key.CaseNo,
                            BpoCd = g.Key.BpoCd,
                            VendName = g.Key.VendName
                        };
            var result = query.FirstOrDefault();
            if (query == null)
            {
                Narrt = "";
                return "0";
            }
            else
            {
                Narrt = query.FirstOrDefault().VendName;
            }


            return Narrt;
        }

        public List<BPOlist> GetDistinctBPOsByCaseNo(string txtCSNO)
        {



            var query = from b in context.T12BillPayingOfficers
                        join p in context.T14PoBpos on b.BpoCd equals p.BpoCd
                        join d in context.T03Cities on b.BpoCityCd equals d.CityCd
                        where p.CaseNo == txtCSNO.ToUpper()
                        orderby b.BpoName
                        select new
                        {
                            BpoCd = b.BpoCd,
                            BpoName = b.BpoName + "/" + (b.BpoAdd ?? "") + "/" + (d.Location ?? d.City) + "/" + b.BpoRly
                        };

            var distinctQuery = query.Distinct();

            var DropdownValues = distinctQuery.AsEnumerable().Select(item => new BPOlist
            {
                value = item.BpoCd.ToString(),
                text = item.BpoName
            }).ToList();

            return DropdownValues;

        }

        public string Insert(RecieptVoucherModel model, string VoucherDate, string Bank_Code, string VoucherType, string Region)
        {
            string VCHR_NO = "";

            string vchr_dt = VoucherDate.ToString() ?? string.Empty;
            string ss = Region + vchr_dt.Substring(8, 2) + vchr_dt.Substring(4, 2);
            string ss1 = ss.Substring(Convert.ToInt32(model.VCHR_NO), 5);
            var voucher1 = context.T24Rvs
                .Where(r => r.VchrNo.StartsWith(ss))
                .Select(r => r.VchrNo.Substring(5, 8))
                .AsEnumerable()
                .Select(substring => int.TryParse(substring, out int parsedInt) ? parsedInt : 0)
                .DefaultIfEmpty(0)
                .Max() + 1;
            if (voucher1 != null)
            {
                VCHR_NO = ss + "00" + voucher1.ToString();
            }
            else
            {
                VCHR_NO = ss + "001";
            }
            return null;
        }

        public string GetAccountName(int AccCd)
        {
            return context.T95AccountCodes.Where(x => x.AccCd == AccCd).Select(x => x.AccDesc.ToString() + ":" + x.AccCd.ToString()).FirstOrDefault();
        }

        public string GetBankName(int BankCd)
        {
            return context.T94Banks.Where(x => x.BankCd == BankCd).Select(x => x.BankName).FirstOrDefault();
        }

        public string GetBPOName(string BpoCd)
        {
            string BpoName = string.Empty;
            var obj = (from bpo in context.T12BillPayingOfficers
                           join city in context.T03Cities on bpo.BpoCityCd equals city.CityCd
                           where bpo.BpoCd == BpoCd
                           select new
                           {
                               BPO_NAME = bpo.BpoName + "/" + (bpo.BpoAdd != null ? bpo.BpoAdd + "/" : "") + (city.Location != null ? city.City + "/" + city.Location : city.City) + "/" + bpo.BpoRly
                           }).FirstOrDefault();

            if (obj != null) BpoName = obj.BPO_NAME.ToString();

            return BpoName;

        }

        public IEnumerable<SelectListItem> GetBPO(int Acc_cd, string BpoType, string BPO_cd)
        {
            int[] Acc_cds = new int[] { 2201, 2202, 2203, 2204, 2205 };

            if (!string.IsNullOrEmpty(BPO_cd))
            {
                return (from bpo in context.T12BillPayingOfficers
                        join city in context.T03Cities on bpo.BpoCityCd equals city.CityCd
                        where bpo.BpoCd == BPO_cd
                        select new SelectListItem
                        {
                            Value = bpo.BpoCd.ToString(),
                            Text = bpo.BpoName + "/" + (bpo.BpoAdd != null ? bpo.BpoAdd + "/" : "") + (city.Location != null ? city.City + "/" + city.Location : city.City) + "/" + bpo.BpoRly
                        }).ToList();
            }
            else
            {
                if (Acc_cds.Contains(Acc_cd))
                {
                    return (from bpo in context.T12BillPayingOfficers
                            join city in context.T03Cities on bpo.BpoCityCd equals city.CityCd
                            where bpo.BpoType == BpoType
                            orderby bpo.BpoName
                            select new SelectListItem
                            {
                                Value = bpo.BpoCd.ToString(),
                                Text = bpo.BpoName + "/" + (bpo.BpoAdd != null ? bpo.BpoAdd + "/" : "") + (city.Location != null ? city.City + "/" + city.Location : city.City) + "/" + bpo.BpoRly
                            }).ToList();
                }
                else
                {
                    return (from bpo in context.T12BillPayingOfficers
                            join city in context.T03Cities on bpo.BpoCityCd equals city.CityCd
                            orderby bpo.BpoName
                            select new SelectListItem
                            {
                                Value = bpo.BpoCd.ToString(),
                                Text = bpo.BpoName + "/" + (bpo.BpoAdd != null ? bpo.BpoAdd + "/" : "") + (city.Location != null ? city.City + "/" + city.Location : city.City) + "/" + bpo.BpoRly
                            }).ToList();
                }
            }
        }

        public BPODetailsModel FindBPODetails(string CaseNo)
        {
            return (from p in context.T13PoMasters
                    join b in context.T14PoBpos on p.CaseNo equals b.CaseNo into bpoGroup
                    from bpo in bpoGroup.DefaultIfEmpty()
                    join v in context.T05Vendors on p.VendCd equals v.VendCd
                    where p.CaseNo == CaseNo
                    group new { p.CaseNo, bpo.BpoCd, v.VendName } by new { p.CaseNo, bpo.BpoCd, v.VendName } into grouped
                    select new BPODetailsModel
                    {
                        CASE_NO = grouped.Key.CaseNo,
                        BPO_CD = grouped.Key.BpoCd,
                        VEND_NAME = grouped.Key.VendName
                    }).FirstOrDefault();
        }

    }
}