using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using static IBS.Helper.Enums;

namespace IBS.Repositories
{
    public class RecieptVoucherRepository : IRecieptVoucherRepository
    {
        private readonly ModelContext context;

        public RecieptVoucherRepository(ModelContext context)
        {
            this.context = context;
        }

        public RecieptVoucherModel FindByID(string VoucherNo)
        {
            RecieptVoucherModel model = new();
            T24Rv rv = context.T24Rvs.Where(x => x.VchrNo == VoucherNo).FirstOrDefault();

            if (rv != null)
            {
                model.IsNew = false;
                model.VCHR_NO = rv.VchrNo;
                model.VCHR_DT = rv.VchrDt;
                model.BANK_CD = rv.BankCd;
                model.lstVoucherDetails = (from t25 in context.T25RvDetails
                                           where t25.VchrNo == model.VCHR_NO
                                           select new VoucherDetailsModel
                                           {
                                               ID = t25.Sno ?? 0,
                                               CHQ_NO = t25.ChqNo,
                                               CHQ_DT = t25.ChqDt,
                                               BANK_CD = t25.BankCd,
                                               AMOUNT = t25.Amount,
                                               SAMPLE_NO = t25.SampleNo,
                                               ACC_CD = t25.AccCd,
                                               CASE_NO = t25.CaseNo,
                                               BPO_CD = t25.BpoCd,
                                               BPO_TYPE = t25.BpoType,
                                               NARRATION = t25.Narration,
                                           }).ToList();
            }

            return model;
        }

        public DTResult<RecieptVoucherModel> GetVoucherList(DTParameters dtParameters)
        {
            DTResult<RecieptVoucherModel> dTResult = new() { draw = 0 };
            IQueryable<RecieptVoucherModel>? query = null;
            List<RecieptVoucherModel>? lstQuery = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null && dtParameters.Order.Length > 0)
            {
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                if (string.IsNullOrEmpty(orderCriteria)) orderCriteria = "VCHR_DT";
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                orderCriteria = "VCHR_DT";
                orderAscendingDirection = true;
            }

            lstQuery = (from t24 in context.T24Rvs
                        join t94 in context.T94Banks on t24.BankCd equals t94.BankCd
                        where t24.Isdeleted != 1
                        select new RecieptVoucherModel
                        {
                            VCHR_NO = t24.VchrNo,
                            VCHR_DT = t24.VchrDt,
                            BANK_CD = t24.BankCd,
                            BANK_NAME = t94.FmisBankCd.ToString().PadLeft(4, '0') + "-" + t94.BankName,
                            
                        }).ToList();

            query = lstQuery.AsQueryable();

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => (w.VCHR_NO != null && w.VCHR_NO.ToLower().Contains(searchBy.ToLower()))
                    || (w.BANK_NAME != null && w.BANK_NAME.ToLower().Contains(searchBy.ToLower()))
            );

            dTResult.recordsFiltered = query.Count();

            if (dtParameters.Length == -1) dtParameters.Length = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public string GetAccountName(int AccCd)
        {
            return context.T95AccountCodes.Where(x => x.AccCd == AccCd).Select(x => x.AccDesc.ToString() + ":" + x.AccCd.ToString()).FirstOrDefault();
        }

        public string GetBankName(int BankCd)
        {
            return context.T94Banks.Where(x => x.BankCd == BankCd).Select(x => x.BankName).FirstOrDefault();
        }

        public string GetFMISBankCd(int BankCd)
        {
            return context.T94Banks.Where(x => x.BankCd == BankCd).Select(x => x.FmisBankCd.ToString().PadLeft(4, '0')).FirstOrDefault();
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

        public bool SaveDetails(RecieptVoucherModel model)
        {
            int wUnit_Cd = 0, wSBU_Cd = 0;

            if (model.Region == "N") { wUnit_Cd = 8; wSBU_Cd = 20; }
            else if (model.Region == "W") { wUnit_Cd = 5; wSBU_Cd = 17; }
            else if (model.Region == "E") { wUnit_Cd = 6; wSBU_Cd = 18; }
            else if (model.Region == "S") { wUnit_Cd = 7; wSBU_Cd = 19; }
            else if (model.Region == "C") { wUnit_Cd = 10; wSBU_Cd = 23; }
            else { wUnit_Cd = 0; wSBU_Cd = 0; }

            if (model.IsNew)
            {
                model.VCHR_NO = GenerateVoucherNo(model.Region, model.VCHR_DT ?? DateTime.Now.Date);

                T24Rv rv = new()
                {
                    VchrNo = model.VCHR_NO,
                    VchrDt = model.VCHR_DT,
                    BankCd = model.BANK_CD,
                    VchrType = model.VCHR_TYPE,
                    Createdby = model.Createdby,
                    Createddate = DateTime.Now,
                    Isdeleted = 0
                };

                context.T24Rvs.Add(rv);
                context.SaveChanges();

                if (model.lstVoucherDetails != null && model.lstVoucherDetails.Count > 0)
                {
                    int index = 0;
                    model.lstVoucherDetails.ForEach(i => { index = index + 1; i.ID = index; });

                    foreach (var item in model.lstVoucherDetails)
                    {
                        T25RvDetail rvDetails = new()
                        {
                            VchrNo = model.VCHR_NO,
                            Sno = item.ID,
                            BankCd = item.BANK_CD,
                            ChqNo = item.CHQ_NO,
                            ChqDt = item.CHQ_DT,
                            Amount = item.AMOUNT,
                            AccCd = item.ACC_CD,
                            AmountAdjusted = 0,
                            SuspenseAmt = item.AMOUNT,
                            Narration = item.NARRATION,
                            SampleNo = item.SAMPLE_NO,
                            BpoCd = item.BPO_CD,
                            BpoType = item.BPO_TYPE,
                            CaseNo = item.CASE_NO,
                            AmtTransferred = 0,
                            UserId = model.UserName,
                            Createdby = Convert.ToString(model.Createdby),
                            Createddate = DateTime.Now,
                        };
                        context.T25RvDetails.Add(rvDetails);

                        string wAcc_Cd = "", wProject_Cd = "", wSub_Cd = "";

                        if (item.ACC_CD == 2709) { wAcc_Cd = "2709"; wProject_Cd = "2203"; wSub_Cd = "1"; }
                        else if (item.ACC_CD > 2000 & item.ACC_CD < 2300) { wAcc_Cd = "2203"; wProject_Cd = item.ACC_CD.ToString(); wSub_Cd = "0"; }
                        else if (item.ACC_CD > 3000 & item.ACC_CD < 3100) { wAcc_Cd = item.ACC_CD.ToString(); wProject_Cd = "2204"; wSub_Cd = "1"; }
                        else if (item.ACC_CD == 2210) { wAcc_Cd = "2093"; wProject_Cd = "2203"; wSub_Cd = "0"; }
                        else if (item.ACC_CD == 2212) { wAcc_Cd = "2094"; wProject_Cd = "2203"; wSub_Cd = "0"; }
                        else { wAcc_Cd = "0000"; wProject_Cd = "0000"; wSub_Cd = ""; }

                        GeneralFile gnrFile = new()
                        {
                            UnitCode = wUnit_Cd,
                            CurryCode = 0,
                            VchrNumb = Convert.ToInt32(model.VCHR_NO.Substring(5, 3)),
                            VchrDate = model.VCHR_DT,
                            Tc = 2,
                            AccCode = Convert.ToInt32(wAcc_Cd),
                            SubCode = wSub_Cd,
                            RefNo = 1,
                            ProjectCode = Convert.ToInt32(wProject_Cd),
                            SbuCode = Convert.ToByte(wSBU_Cd),
                            Narration = item.NARRATION.Length > 30 ? item.NARRATION.Substring(0, 30) : item.NARRATION,
                            Amount = item.AMOUNT,
                            ChequeNo = item.CHQ_NO.Length > 6 ? item.CHQ_NO.Substring(0, 6) : item.CHQ_NO,
                            BankCode = Convert.ToInt32(GetFMISBankCd(model.BANK_CD ?? 0)),
                            PartyName = GetBankName(item.BANK_CD) + " || " + Common.ConvertDateFormat(item.CHQ_DT),
                            Region = model.Region,
                            VchrNoT25 = model.VCHR_NO,
                            SnoT25 = Convert.ToInt32(item.ID)
                        };

                        context.GeneralFiles.Add(gnrFile);
                    }
                    context.SaveChanges();
                }
            }
            else
            {
                T24Rv rv = context.T24Rvs.Where(x => x.VchrNo == model.VCHR_NO).FirstOrDefault();

                if (rv != null)
                {
                    rv.VchrDt = model.VCHR_DT;
                    rv.BankCd = model.BANK_CD;
                    rv.Updatedby = model.Updatedby;
                    rv.Updateddate = DateTime.Now;

                    context.SaveChanges();
                }

                if (model.lstVoucherDetails != null && model.lstVoucherDetails.Count > 0)
                {
                    foreach (var item in model.lstVoucherDetails)
                    {
                        string wAcc_Cd = "", wProject_Cd = "", wSub_Cd = "";

                        if (item.ACC_CD == 2709) { wAcc_Cd = "2709"; wProject_Cd = "2203"; wSub_Cd = "1"; }
                        else if (item.ACC_CD > 2000 & item.ACC_CD < 2300) { wAcc_Cd = "2203"; wProject_Cd = item.ACC_CD.ToString(); wSub_Cd = "0"; }
                        else if (item.ACC_CD > 3000 & item.ACC_CD < 3100) { wAcc_Cd = item.ACC_CD.ToString(); wProject_Cd = "2204"; wSub_Cd = "1"; }
                        else if (item.ACC_CD == 2210) { wAcc_Cd = "2093"; wProject_Cd = "2203"; wSub_Cd = "0"; }
                        else if (item.ACC_CD == 2212) { wAcc_Cd = "2094"; wProject_Cd = "2203"; wSub_Cd = "0"; }
                        else { wAcc_Cd = "0000"; wProject_Cd = "0000"; wSub_Cd = ""; }

                        if (item.IsNew)
                        {
                            T25RvDetail rvDetails = new()
                            {
                                VchrNo = model.VCHR_NO,
                                Sno = item.ID,
                                BankCd = item.BANK_CD,
                                ChqNo = item.CHQ_NO,
                                ChqDt = item.CHQ_DT,
                                Amount = item.AMOUNT,
                                AccCd = item.ACC_CD,
                                AmountAdjusted = 0,
                                SuspenseAmt = item.AMOUNT,
                                Narration = item.NARRATION,
                                SampleNo = item.SAMPLE_NO,
                                BpoCd = item.BPO_CD,
                                BpoType = item.BPO_TYPE,
                                CaseNo = item.CASE_NO,
                                AmtTransferred = 0,
                                UserId = model.UserName,
                                Createdby = Convert.ToString(model.Createdby),
                                Createddate = DateTime.Now,
                            };
                            context.T25RvDetails.Add(rvDetails);

                            GeneralFile gnrFile = new()
                            {
                                UnitCode = wUnit_Cd,
                                CurryCode = 0,
                                VchrNumb = Convert.ToInt32(model.VCHR_NO.Substring(5, 3)),
                                VchrDate = model.VCHR_DT,
                                Tc = 2,
                                AccCode = Convert.ToInt32(wAcc_Cd),
                                SubCode = wSub_Cd,
                                RefNo = 1,
                                ProjectCode = Convert.ToInt32(wProject_Cd),
                                SbuCode = Convert.ToByte(wSBU_Cd),
                                Narration = item.NARRATION.Length > 30 ? item.NARRATION.Substring(0, 30) : item.NARRATION,
                                Amount = item.AMOUNT,
                                ChequeNo = item.CHQ_NO.Length > 6 ? item.CHQ_NO.Substring(0, 6) : item.CHQ_NO,
                                BankCode = Convert.ToInt32(GetFMISBankCd(model.BANK_CD ?? 0)),
                                PartyName = GetBankName(item.BANK_CD) + " || " + Common.ConvertDateFormat(item.CHQ_DT),
                                Region = model.Region,
                                VchrNoT25 = model.VCHR_NO,
                                SnoT25 = Convert.ToInt32(item.ID)
                            };
                        }
                        else
                        {
                            T25RvDetail rvDetails = context.T25RvDetails.Where(x => x.VchrNo == model.VCHR_NO && x.Sno == item.ID).FirstOrDefault();

                            if (rvDetails != null)
                            {
                                rvDetails.BankCd = item.BANK_CD;
                                rvDetails.ChqNo = item.CHQ_NO;
                                rvDetails.ChqDt = item.CHQ_DT;
                                rvDetails.Amount = item.AMOUNT;
                                rvDetails.AccCd = item.ACC_CD;
                                rvDetails.AmountAdjusted = 0;
                                rvDetails.SuspenseAmt = item.AMOUNT;
                                rvDetails.Narration = item.NARRATION;
                                rvDetails.SampleNo = item.SAMPLE_NO;
                                rvDetails.BpoCd = item.BPO_CD;
                                rvDetails.BpoType = item.BPO_TYPE;
                                rvDetails.CaseNo = item.CASE_NO;
                                rvDetails.AmtTransferred = 0;
                                rvDetails.UserId = model.UserName;
                                rvDetails.Updatedby = Convert.ToString(model.Updatedby);
                                rvDetails.Updateddate = DateTime.Now;
                            }

                            GeneralFile gnrFile = context.GeneralFiles.Where(x => x.VchrNoT25 == model.VCHR_NO && x.SnoT25 == item.ID && x.PostingStatus == null).FirstOrDefault();

                            if(gnrFile != null)
                            {
                                gnrFile.AccCode = Convert.ToInt32(wAcc_Cd);
                                gnrFile.PartyName = GetBankName(item.BANK_CD) + " || " + Common.ConvertDateFormat(item.CHQ_DT);
                                gnrFile.ChequeNo = item.CHQ_NO.Length > 6 ? item.CHQ_NO.Substring(0, 6) : item.CHQ_NO;
                                gnrFile.Narration = item.NARRATION.Length > 30 ? item.NARRATION.Substring(0, 30) : item.NARRATION;
                                gnrFile.Amount = item.AMOUNT;
                            }

                        }
                    }
                    context.SaveChanges();
                }
            }

            return true;
        }

        public string GenerateVoucherNo(string Region, DateTime VoucherDate)
        {
            string strVoucherDate = Common.ConvertDateFormat(VoucherDate);
            string ss = Region + strVoucherDate.Substring(8, 2) + strVoucherDate.Substring(3, 2);

            var query = context.T24Rvs.Where(r => r.VchrNo.StartsWith(ss)).Select(r => r.VchrNo.Substring(5, 8)).AsEnumerable()
                          .Select(substring => int.TryParse(substring, out int parsedInt) ? parsedInt : 0).DefaultIfEmpty(0)
                          .Max() + 1;

            return ss + query.ToString("000");

        }
    }
}