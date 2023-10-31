using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace IBS.Repositories
{
    public class BillCheckPostingRepository : IBillCheckPostingRepository
    {
        private readonly ModelContext context;

        public BillCheckPostingRepository(ModelContext context)
        {
            this.context = context;
        }

        public BillCheckPostingModel FindByID(string ChqNo, DateTime ChqDt, string BankName, string BillNo, int AmountCleared, string Region)
        {
            BillCheckPostingModel model = new();
            var query1 = context.ViewChequePostingEditDetails.Where(x=>x.ChqNo == ChqNo && x.ChqDt == ChqDt && x.BankCd == Convert.ToInt32(BankName) && x.VchrNo.StartsWith(Region.Substring(0, 1))).FirstOrDefault();
            if (query1 != null)
            {
                model.VcharNo = query1.VchrNo;
                model.VcharDt = query1.VchrDt;
                model.ChqNo = query1.ChqNo;
                model.ChqDt = query1.ChqDt;
                model.BankName = Convert.ToString(query1.BankCd);
                model.BpoName = query1.Bpo;
                model.ChqAmount = query1.Amount;
                model.AmountAdjusted = query1.AmountAdjusted;
                model.AmtTransferred = query1.AmtTransferred;
                model.SuspenseAmt = query1.SuspenseAmt;
                model.AccCd = query1.AccCd;
                model.Bpo = query1.Bpo;
            }

            var ChqPosting = context.T26ChequePostings.Where(x=>x.BankCd == Convert.ToInt32(BankName) && x.ChqNo == ChqNo && x.ChqDt == ChqDt && x.BillNo == BillNo).FirstOrDefault();
            if (ChqPosting != null)
            {
                model.BillNo = ChqPosting.BillNo;
                model.BillAmount = ChqPosting.BillAmount;
                model.AmtTOClear = ChqPosting.AmountCleared;
                model.PostingDt = ChqPosting.PostingDt;
            }

            var BillDetails = context.V22Bills.Where(x=>x.BillNo == BillNo).FirstOrDefault();
            if (BillDetails != null)
            {
                model.BillDt = BillDetails.BillDt;
                model.AmtRecieved = BillDetails.AmountReceived;
                model.BillAmtCleared = BillDetails.BillAmtCleared;
                model.TDS = (BillDetails.Tds ?? 0) + (BillDetails.TdsSgst ?? 0) + (BillDetails.TdsCgst ?? 0) + (BillDetails.TdsIgst ?? 0);
                model.RetentionMoney = BillDetails.RetentionMoney ?? 0;
                model.CNoteAmt = BillDetails.CnoteAmount ?? 0;
                model.WriteOffAmt = BillDetails.WriteOffAmt;
                model.AmtToRec = (Convert.ToDecimal(model.BillAmount) - Convert.ToDecimal(model.AmtRecieved) + Convert.ToDecimal(model.TDS) + Convert.ToDecimal(model.RetentionMoney) + Convert.ToDecimal(model.WriteOffAmt) + +Convert.ToDecimal(model.CNoteAmt));
                model.CaseNo = BillDetails.CaseNo;
                model.BkNo = BillDetails.BkNo;
                model.SetNo = BillDetails.SetNo;
                model.BpoCd = BillDetails.BpoCd;
                model.BpoName = BillDetails.BpoCd + "-" + BillDetails.BpoName + "/" + BillDetails.BpoRly + "/" + (BillDetails.BpoAdd != null ? BillDetails.BpoAdd + "/" : "") + BillDetails.BpoCity;
            }
            return model;
        }

        public BillCheckPostingModel GetBankDetails(int BankCd, string ChqNo, DateTime ChqDt, string Region)
        {
            BillCheckPostingModel query = null;
            var query1 = from v in context.GetBankdetails
                         where v.ChqNo == ChqNo && v.ChqDt == Convert.ToDateTime(ChqDt) && v.BankCd == Convert.ToInt32(BankCd) && v.VchrNo.StartsWith(Region)
                         select new BillCheckPostingModel
                         {
                             VcharNo = v.VchrNo,
                             VcharDt = v.VchrDt,
                             ChqNo = v.ChqNo,
                             ChqDt = v.ChqDt,
                             BankName = Convert.ToString(v.BankCd),
                             Bpo = v.Bpo,
                             ChqAmount = v.Amount,
                             AmountAdjusted = v.AmountAdjusted,
                             AmtTransferred = v.AmtTransferred,
                             SuspenseAmt = v.SuspenseAmt,
                             AccCd = v.AccCd
                         };

            query = query1.FirstOrDefault();

            return query;
        }

        public BillCheckPostingModel GetBillDetails(string BillInvoiceNo, string BillTypes, string Region)
        {
            BillCheckPostingModel model = null;

            if (BillTypes == "B")
            {
                var query1 = from V22 in context.V22Bills
                             where V22.BillNo == BillInvoiceNo && V22.RegionCode == Region
                             select new BillCheckPostingModel
                             {
                                 BillNo = V22.BillNo,
                                 InvoiceNo = V22.InvoiceNo,
                                 BillDt = V22.BillDt,
                                 CaseNo = V22.CaseNo,
                                 BkNo = V22.BkNo,
                                 SetNo = V22.SetNo,
                                 BpoName = V22.BpoCd + "-" + V22.BpoName + "/" + V22.BpoRly + "/" + (V22.BpoAdd != null ? V22.BpoAdd + "/" : "") + V22.BpoCity,
                                 BillAmount = V22.BillAmount ?? 0,
                                 AmtRecieved = V22.AmountReceived ?? 0,
                                 BillAmtCleared = V22.BillAmtCleared ?? 0,
                                 TDS = (V22.Tds ?? 0) + (V22.TdsSgst ?? 0) + (V22.TdsCgst ?? 0) + (V22.TdsIgst ?? 0),
                                 RetentionMoney = V22.RetentionMoney ?? 0,
                                 WriteOffAmt = V22.WriteOffAmt ?? 0,
                                 CNoteAmt = V22.CnoteAmount ?? 0,
                                 BpoCd = V22.BpoCd,
                                 AmtToRec = Convert.ToDecimal((Convert.ToInt32(V22.BillAmount) - Convert.ToInt32(V22.AmountReceived)) + (Convert.ToInt32(V22.RetentionMoney) + Convert.ToInt32(V22.CnoteAmount) + Convert.ToInt32(V22.WriteOffAmt) + Convert.ToInt32(V22.Tds))),
                                 PostingDt = DateTime.Now.Date
                                 //AmtToRec = V22.BillAmount - V22.AmountReceived + V22.RetentionMoney + V22.CnoteAmount + V22.WriteOffAmt + V22.Tds
                             };
                model = query1.FirstOrDefault();
            }
            else
            {
                var query1 = from V22 in context.V22Bills
                             where V22.InvoiceNo == BillInvoiceNo && V22.RegionCode == Region
                             select new BillCheckPostingModel
                             {
                                 BillNo = V22.BillNo,
                                 InvoiceNo = V22.InvoiceNo,
                                 BillDt = V22.BillDt,
                                 CaseNo = V22.CaseNo,
                                 BkNo = V22.BkNo,
                                 SetNo = V22.SetNo,
                                 BpoName = V22.BpoCd + "-" + V22.BpoName + "/" + V22.BpoRly + "/" + (V22.BpoAdd != null ? V22.BpoAdd + "/" : "") + V22.BpoCity,
                                 BillAmount = V22.BillAmount ?? 0,
                                 AmtRecieved = V22.AmountReceived ?? 0,
                                 BillAmtCleared = V22.BillAmtCleared ?? 0,
                                 TDS = (V22.Tds ?? 0) + (V22.TdsSgst ?? 0) + (V22.TdsCgst ?? 0) + (V22.TdsIgst ?? 0),
                                 RetentionMoney = V22.RetentionMoney ?? 0,
                                 WriteOffAmt = V22.WriteOffAmt ?? 0,
                                 CNoteAmt = V22.CnoteAmount ?? 0,
                                 BpoCd = V22.BpoCd,
                                 AmtToRec = Convert.ToDecimal((Convert.ToInt32(V22.BillAmount) - Convert.ToInt32(V22.AmountReceived)) + (Convert.ToInt32(V22.RetentionMoney) + Convert.ToInt32(V22.CnoteAmount) + Convert.ToInt32(V22.WriteOffAmt) + Convert.ToInt32(V22.Tds))),
                                 PostingDt = DateTime.Now.Date
                             };
                model = query1.FirstOrDefault();
            }
            return model;
        }

        public DTResult<BillCheckPostingModelList> GetBillList(DTParameters dtParameters)
        {
            DTResult<BillCheckPostingModelList> dTResult = new() { draw = 0 };
            IQueryable<BillCheckPostingModelList>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null && dtParameters.Order.Length > 0)
            {
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                if (string.IsNullOrEmpty(orderCriteria)) orderCriteria = "Datetime";
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                orderCriteria = "Datetime";
                orderAscendingDirection = true;
            }

            string BankName = !string.IsNullOrEmpty(dtParameters.AdditionalValues["BankName"]) ? Convert.ToString(dtParameters.AdditionalValues["BankName"]) : "";
            string ChqNo = !string.IsNullOrEmpty(dtParameters.AdditionalValues["ChqNo"]) ? Convert.ToString(dtParameters.AdditionalValues["ChqNo"]) : "";
            string ChqDt = !string.IsNullOrEmpty(dtParameters.AdditionalValues["ChqDt"]) ? Convert.ToString(dtParameters.AdditionalValues["ChqDt"]) : "";

            query = from T26 in context.ViewChequePostingDetails
                    where T26.BankCd == Convert.ToInt32(BankName) && T26.ChqNo == ChqNo && T26.ChqDt == Convert.ToDateTime(ChqDt)
                    select new BillCheckPostingModelList
                    {
                        BankName = Convert.ToString(T26.BankCd),
                        ChqNo = T26.ChqNo,
                        ChqDt = T26.ChqDt,
                        BillNo = T26.BillNo,
                        BillAmount = T26.BillAmount,
                        AmountCleared = T26.AmountCleared,
                        PostingDt = T26.PostingDt,
                        BillAmtCleared = T26.BillAmtCleared,
                        BpoName = T26.BpoName,
                        Datetime = T26.Datetime,
                    };

            dTResult.recordsTotal = query.Count();

            //if (!string.IsNullOrEmpty(searchBy))
            //    query = query.Where(w => Convert.ToString(w.BankName).ToLower().Contains(searchBy.ToLower())
            //    );

            dTResult.recordsFiltered = query.Count();

            if (dtParameters.Length == -1) dtParameters.Length = query.Count();

            //dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            //dTResult.data = query.Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();
            var result = query.ToList();
            dTResult.data = result;

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public int BillDetailSave(BillCheckPostingModel model, string UserName)
        {
            int Id = 0;
            if (model.ActionType == "" || model.ActionType == null)
            {
                if ((Convert.ToInt32(model.ChqAmount) - (Convert.ToInt32(model.AmountAdjusted) + Convert.ToInt32(model.AmtTransferred))) > 0 && (Convert.ToInt32(model.ChqAmount) - (Convert.ToInt32(model.AmountAdjusted) + Convert.ToInt32(model.AmtTransferred))) >= Convert.ToInt32(model.AmtTOClear) && Convert.ToInt32(model.AmtTOClear) <= Convert.ToInt32(model.AmtToRec))
                {
                    # region Insert record for cheque posting 
                    T26ChequePosting T26 = new();
                    T26.BankCd = Convert.ToInt32(model.BankName);
                    T26.ChqNo = model.ChqNo;
                    T26.ChqDt = model.ChqDt;
                    T26.BillNo = model.BillNo;
                    T26.BillAmount = model.BillAmount;
                    T26.AmountCleared = model.AmtTOClear;
                    T26.PostingDt = model.PostingDt;
                    T26.BpoCd = model.BpoCd;
                    T26.UserId = UserName;
                    T26.Datetime = DateTime.Now.Date;
                    context.T26ChequePostings.Add(T26);
                    context.SaveChanges();
                    #endregion

                    double amtadj = Convert.ToDouble(model.AmountAdjusted);
                    amtadj = amtadj + Convert.ToDouble(model.AmtTOClear);
                    string BAmtRec = Convert.ToString(Convert.ToDouble(model.AmtRecieved) + Convert.ToDouble(model.AmtTOClear));
                    model.AmtRecieved = Convert.ToDecimal(BAmtRec);
                    string AmtToRec = Convert.ToString(Convert.ToDouble(model.AmtToRec) - (Convert.ToDouble(model.AmtTOClear) + Convert.ToDouble(model.TDS) + Convert.ToDouble(model.RetentionMoney) + Convert.ToDouble(model.WriteOffAmt) + Convert.ToDouble(model.CNoteAmt)));
                    model.AmtToRec = Convert.ToDecimal(AmtToRec);
                    string SAmt = Convert.ToString(Convert.ToDouble(model.SuspenseAmt) - Convert.ToDouble(model.AmtTOClear));
                    model.SuspenseAmt = Convert.ToDecimal(SAmt);

                    var RvDetails = context.T25RvDetails.Where(x => x.BankCd == Convert.ToInt32(model.BankName) && x.ChqNo == model.ChqNo && x.ChqDt == model.ChqDt).FirstOrDefault();
                    if (RvDetails != null)
                    {
                        RvDetails.AmountAdjusted = Convert.ToDecimal(amtadj);
                        RvDetails.SuspenseAmt = model.SuspenseAmt;
                        context.SaveChanges();
                    }
                    var BillDetails = context.T22Bills.Where(x => x.BillNo == model.BillNo).FirstOrDefault();
                    if (BillDetails != null)
                    {
                        BillDetails.AmountReceived = Convert.ToDecimal(BAmtRec);
                        BillDetails.WriteOffAmt = model.WriteOffAmt;
                        BillDetails.BillAmtCleared = Convert.ToDecimal(BAmtRec) + Convert.ToDecimal(model.TDS) + Convert.ToDecimal(model.RetentionMoney) + Convert.ToDecimal(model.WriteOffAmt) + Convert.ToDecimal(model.CNoteAmt);
                        context.SaveChanges();
                    }
                    Id = 1;
                }
                else
                {
                    Id = 2;
                }
            }

            else if (model.ActionType == "M")
            {
                decimal AMTCLR = 0;
                AMTCLR = Convert.ToDecimal(model.AmtTOClear);
                if ((Convert.ToDecimal(model.ChqAmount) - (Convert.ToDecimal(model.AmountAdjusted) + Convert.ToDecimal(model.AmtTransferred)) + AMTCLR) >= Convert.ToDecimal(model.AmtTOClear))
                {
                    var ChqPosting = context.T26ChequePostings.Where(x => x.BankCd == Convert.ToInt32(model.BankName) && x.ChqNo == model.ChqNo && x.ChqDt == model.ChqDt && x.BillNo == model.BillNo).FirstOrDefault();
                    if (ChqPosting != null)
                    {
                        ChqPosting.AmountCleared = model.AmtTOClear;
                        ChqPosting.PostingDt = model.PostingDt;
                        ChqPosting.UserId = UserName;
                        ChqPosting.Datetime = DateTime.Now.Date;
                        context.SaveChanges();
                    }
                    double amtadj = Convert.ToDouble(model.AmountAdjusted);
                    amtadj = amtadj - Convert.ToDouble(AMTCLR);
                    amtadj = amtadj + Convert.ToDouble(model.AmtTOClear);

                    string BAmtRec = Convert.ToString(Convert.ToDecimal(model.AmtRecieved) - AMTCLR);
                    BAmtRec = Convert.ToString(Convert.ToDouble(BAmtRec) + Convert.ToDouble(model.AmtTOClear));
                    model.BillAmount = Convert.ToDecimal(BAmtRec);
                    string AmtToRec = Convert.ToString(Convert.ToDouble(model.BillAmount) - (Convert.ToDouble(model.AmtRecieved) + Convert.ToDouble(model.TDS) + Convert.ToDouble(model.RetentionMoney) + Convert.ToDouble(model.WriteOffAmt) + Convert.ToDouble(model.CNoteAmt)));
                    model.AmtToRec = Convert.ToDecimal(AmtToRec);
                    string SAmt = Convert.ToString((Convert.ToDecimal(model.SuspenseAmt) + AMTCLR) - Convert.ToDecimal(model.AmtTOClear));
                    model.SuspenseAmt = Convert.ToDecimal(SAmt);

                    var RvDetails = context.T25RvDetails.Where(x => x.BankCd == Convert.ToInt32(model.BankName) && x.ChqNo == model.ChqNo && x.ChqDt == model.ChqDt).FirstOrDefault();
                    if (RvDetails != null)
                    {
                        RvDetails.AmountAdjusted = Convert.ToDecimal(amtadj);
                        RvDetails.SuspenseAmt = model.SuspenseAmt;
                        context.SaveChanges();
                    }
                    var BillDetails = context.T22Bills.Where(x => x.BillNo == model.BillNo).FirstOrDefault();
                    if (BillDetails != null)
                    {
                        BillDetails.AmountReceived = Convert.ToDecimal(model.BillAmount);
                        BillDetails.WriteOffAmt = model.WriteOffAmt;
                        BillDetails.BillAmtCleared = Convert.ToDecimal(model.BillAmount) + Convert.ToDecimal(model.TDS) + Convert.ToDecimal(model.RetentionMoney) + Convert.ToDecimal(model.WriteOffAmt) + Convert.ToDecimal(model.CNoteAmt);
                        context.SaveChanges();
                    }
                    Id = 1;
                }
                else
                {
                    Id = 3;
                }
            }

            return Id;
        }
    }
}
