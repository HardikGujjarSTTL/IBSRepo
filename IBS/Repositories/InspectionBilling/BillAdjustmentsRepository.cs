using IBS.DataAccess;
using IBS.Interfaces.InspectionBilling;
using IBS.Models;
using System.Dynamic;

namespace IBS.Repositories.InspectionBilling
{
    public class BillAdjustmentsRepository : IBillAdjustmentsRepository
    {
        private readonly ModelContext context;

        public BillAdjustmentsRepository(ModelContext context)
        {
            this.context = context;
        }

        public InspectionCertModel FindByBillDetails(string BillNo, string Region)
        {
            InspectionCertModel model = new();
            T22Bill Bill = context.T22Bills.Where(x => x.BillNo == BillNo).FirstOrDefault();

            if (Bill != null)
            {
                var GetDetails = (from C in context.T20Ics
                                  join I in context.T09Ies on C.IeCd equals I.IeCd
                                  where C.CaseNo == Bill.CaseNo
                                  select new
                                  {
                                      C,
                                      I
                                  }).FirstOrDefault();
                //CallDetails
                if (GetDetails != null)
                {
                    model.Caseno = GetDetails.C.CaseNo;
                    model.Callrecvdt = GetDetails.C.CallRecvDt;
                    model.Callsno = GetDetails.C.CallSno;
                    model.IcTypeId = Convert.ToInt32(GetDetails.C.IcTypeId);
                    model.ConsigneeCd = GetDetails.C.ConsigneeCd;
                    model.BpoCd = GetDetails.C.BpoCd;
                    model.Bkno = GetDetails.C.BkNo;
                    model.Setno = GetDetails.C.SetNo;
                    model.IeName = GetDetails.I.IeName;
                    model.IeCd = GetDetails.C.IeCd;
                    model.CertNo = GetDetails.C.IcNo;
                    model.CertDt = GetDetails.C.IcDt;
                    model.CallDt = GetDetails.C.CallDt;
                    model.CallInstallNo = Convert.ToInt32(GetDetails.C.CallInstallNo);
                    model.FullPart = GetDetails.C.FullPart;
                    model.NoOfInsp = Convert.ToInt32(GetDetails.C.NoOfInsp);
                    model.FirstInspDt = Convert.ToDateTime(GetDetails.C.FirstInspDt);
                    model.LastInspDt = Convert.ToDateTime(GetDetails.C.LastInspDt);
                    model.OtherInspDt = Convert.ToDateTime(GetDetails.C.OtherInspDt);
                    model.StampPattern = GetDetails.C.StampPattern;
                    model.ReasonReject = GetDetails.C.ReasonReject;
                    model.BillNo = GetDetails.C.BillNo;
                    model.ICSubmitDt = GetDetails.C.IcSubmitDt.HasValue ? Convert.ToDateTime(GetDetails.C.IcSubmitDt.Value) : Convert.ToDateTime(null);
                    model.Photo = GetDetails.C.Photo;
                    model.StampPatternCd = GetDetails.C.StampPatternCd;
                    model.AccGroup = GetDetails.C.AccGroup ?? "XXXX";
                    model.IrfcBpo = GetDetails.C.IrfcBpoCd;
                    model.IrfcFunded = GetDetails.C.IrfcFunded;
                    model.GstinNo = GetDetails.C.RecipientGstinNo;
                }

                var PODetails = (from p in context.T13PoMasters
                                 join v in context.T05Vendors on p.VendCd equals v.VendCd
                                 join c in context.T03Cities on v.VendCityCd equals c.CityCd
                                 where p.CaseNo == Bill.CaseNo
                                 select new
                                 {
                                     p,
                                     v,
                                     c
                                 }).FirstOrDefault();
                if (PODetails != null)
                {
                    model.PoNo = PODetails.p.PoNo;
                    model.PoDt = PODetails.p.PoDt;
                    model.VendName = PODetails.v.VendName + "/" + PODetails.v.VendAdd1 + "/" + (PODetails.c.Location != PODetails.c.City + "/" + PODetails.c.City);
                    model.PoSource = PODetails.p.PoSource == "C" ? "CRIS" : (PODetails.p.PoSource == "V" ? "VENDOR" : "MANUAL");
                    model.StockNonstock = PODetails.p.StockNonstock == "S" ? "Stock" : "Non-Stock";
                }
                var query1 = (from p in context.T20Ics
                              join b in context.T12BillPayingOfficers on p.BpoCd equals b.BpoCd
                              join c in context.T03Cities on b.BpoCityCd equals c.CityCd
                              where p.CaseNo == model.Caseno && p.CallRecvDt == model.Callrecvdt && p.CallSno == model.Callsno
                                    && p.ConsigneeCd == model.ConsigneeCd
                              select new
                              {
                                  BpoCd = p.BpoCd,
                                  BpoName = b.BpoCd + "-" + b.BpoName + "/" + (b.BpoAdd ?? "/") + (c.Location != null ? c.City + "/" + c.Location : c.City) + "/" + b.BpoRly,
                                  b.BpoRly,
                                  b.Au
                              }).FirstOrDefault();
                if (query1 != null)
                {
                    model.BpoName = query1.BpoName;
                    model.Bpo = query1.BpoCd;
                    model.BpoCd = query1.BpoCd;
                    model.BpoRly = query1.BpoRly;
                    model.Au = query1.Au;
                }
                var query2 = (from p in context.T14PoBpos
                              join b in context.T12BillPayingOfficers on p.BpoCd equals b.BpoCd
                              join c in context.T03Cities on b.BpoCityCd equals c.CityCd
                              where p.CaseNo == model.Caseno &&
                                    p.ConsigneeCd == model.ConsigneeCd
                              select new
                              {
                                  BpoCd = p.BpoCd,
                                  BpoName = $"{b.BpoCd}-{b.BpoName}/{(b.BpoAdd ?? "/")}{(c.Location != null ? $"{c.City}/{c.Location}" : c.City)}/{b.BpoRly}",
                                  b.BpoRly,
                                  b.BpoFee,
                                  b.BpoFeeType,
                                  BpoTaxType = b.BpoTaxType ?? "X",
                                  b.Au,
                                  b.BpoType
                              }).FirstOrDefault();
                if (query2 != null)
                {
                    model.BpoName = query2.BpoName;
                    model.Bpo = query2.BpoCd;
                    model.BpoCd = query2.BpoCd;
                    model.BpoRly = query2.BpoRly;
                    model.BpoType = query2.BpoType;
                    if (query2.BpoType == "R" && query2.BpoFeeType == "P" && model.IcTypeId == 1)
                    {

                    }
                    else
                    {
                        model.BpoFee = query2.BpoFee;
                    }
                    model.BpoFeeType = query2.BpoFeeType;
                    model.TaxType = query2.BpoTaxType;
                    model.Au = query2.Au;
                }
                var query3 = (from c in context.T17CallRegisters
                              where c.CaseNo == model.Caseno && c.CallRecvDt == model.Callrecvdt && c.CallSno == model.Callsno
                              select new
                              {
                                  BPO = c.Bpo.ToUpper(),
                                  c.RecipientGstinNo
                              }).FirstOrDefault();
                if (query3 != null)
                {
                    model.BPOCall = query3.BPO;
                    model.GSTINCall = query3.RecipientGstinNo;
                }
                if (GetDetails.C.IrfcFunded == null || GetDetails.C.IrfcFunded == "")
                {
                    model.IrfcBpo = "N";
                }
                if(GetDetails.C.IrfcFunded == "Y")
                {
                    get_legalname(model, Convert.ToInt32(model.IrfcBpo), "B");
                }
                else
                {
                    get_legalname(model, Convert.ToInt32(model.ConsigneeCd), "C");
                }
                
            }
            if (Bill == null)
                throw new Exception("Bill Record Not found");
            else
            {
                //Bill Details
                model.Caseno = Bill.CaseNo;
                model.BillNo = Bill.BillNo;
                model.BillDt = Bill.BillDt;
                model.TMValue = Convert.ToDecimal(Bill.MaterialValue);
                model.BpoFeeType = Bill.FeeType;
                model.Rate = Bill.FeeRate;
                model.TaxType = Bill.TaxType;
                model.TIFee = Bill.InspFee;
                model.ServiceTax = Bill.ServiceTax;
                model.EduCess = Bill.EduCess;
                model.SheCess = Bill.SheCess;
                model.MinFee = Convert.ToInt32(Bill.MinFee);
                model.MaxFee = Convert.ToInt32(Bill.MaxFee);
                model.NetFee = Bill.BillAmount;
                model.AmountReceived = Bill.AmountReceived;
                model.Tds = Bill.Tds;
                model.BillAmtCleared = Bill.BillAmtCleared;
                model.BillStatus = Bill.BillStatus;
                model.Remarks = Bill.Remarks;
                model.RetentionMoney = Bill.RetentionMoney;
                model.WriteOffAmt = Bill.WriteOffAmt;
                model.ServTaxRate = Bill.ServTaxRate;
                model.TdsDt = Bill.TdsDt;
                model.AdvBill = Bill.AdvBill == "A" ? Bill.AdvBill : "X";
                model.ScannedStatus = Bill.ScannedStatus;
                model.SwachhBharatCess = Bill.SwachhBharatCess;
                model.KrishiKalyanCess = Bill.KrishiKalyanCess;
                model.Sgst = Bill.Sgst;
                model.Cgst = Bill.Cgst;
                model.Igst = Bill.Igst;
                model.InvoiceNo = Bill.InvoiceNo;
                model.TdsSgst = Bill.TdsSgst;
                model.CnoteBillNo = Bill.CnoteBillNo;
                model.CnoteAmount = Bill.CnoteAmount;
                model.DigBillGenDt = Bill.DigBillGenDt;
                model.BillResentStatus = Bill.BillResentStatus;
                model.BillResentCount = Convert.ToInt32(Bill.BillResentCount);
                model.IrnNo = Bill.IrnNo;
                model.AckNo = Bill.AckNo;
                model.AckDt = Bill.AckDt;
                model.QrCode = Bill.QrCode;
                model.SentToSap = Bill.SentToSap;
                model.BillFinalised = Bill.BillFinalised;
                model.InvoiceSuppDocs = Bill.InvoiceSuppDocs;
                model.CreditDocId = Bill.CreditDocId;
                model.LoRemarks = Bill.LoRemarks;
                model.SapStatus = Bill.SapStatus;

                
            }
            return model;
        }

        void get_legalname(InspectionCertModel model, int code, string type)
        {
            if (type == "C")
            {
                var GetLegalD = (from t06 in context.T06Consignees
                                 join t03 in context.T03Cities on t06.ConsigneeCity equals t03.CityCd
                                 join t92 in context.T92States on t03.StateCd equals t92.StateCd
                                 where t06.ConsigneeCd == code
                                 select new
                                 {
                                     GstinNo = t06.GstinNo ?? "X",
                                     LegalName = t06.LegalName,
                                     Pincode = t06.PinCode,
                                     City = t03.City,
                                     State = t92.StateCd + "-" + t92.StateName
                                 }).FirstOrDefault();
                if(GetLegalD != null)
                {
                    model.LegalName = GetLegalD.LegalName;
                    model.City = GetLegalD.City;
                    model.State = GetLegalD.State;
                    model.Pincode = GetLegalD.Pincode;
                }
            }
            else if (type == "B")
            {
                var GetLegalD = (from t12 in context.T12BillPayingOfficers
                                 join t03 in context.T03Cities on t12.BpoCityCd equals t03.CityCd
                                 join t92 in context.T92States on t03.StateCd equals t92.StateCd
                                 where t12.BpoCd == Convert.ToString(code)
                                 select new
                                 {
                                     GstinNo = t12.GstinNo ?? "X",
                                     LegalName = t12.LegalName,
                                     Pincode = t12.PinCode,
                                     City = t03.City,
                                     State = t92.StateCd + "-" + t92.StateName
                                 }).FirstOrDefault();
                if (GetLegalD != null)
                {
                    model.LegalName = GetLegalD.LegalName;
                    model.City = GetLegalD.City;
                    model.State = GetLegalD.State;
                    model.Pincode = GetLegalD.Pincode;
                }
            }
        }

        public InspectionCertModel FindByItemID(string CaseNo, DateTime CallRecvDt, int CallSno, int ItemSrnoPo, string Region)
        {
            InspectionCertModel model = new();
            T18CallDetail CDetails = context.T18CallDetails.Where(x => x.CaseNo == CaseNo && x.CallRecvDt == CallRecvDt && x.CallSno == CallSno && x.ItemSrnoPo == ItemSrnoPo).FirstOrDefault();

            if (CDetails == null)
                throw new Exception("Call Record Not found");
            else
            {
                model.Caseno = CDetails.CaseNo;
                model.Callrecvdt = CDetails.CallRecvDt;
                model.Callsno = CDetails.CallSno;

                model.ItemSrnoPo = CDetails.ItemSrnoPo;
                model.ItemDescPo = CDetails.ItemDescPo;
                model.QtyPassed = CDetails.QtyPassed;
                model.QtyToInsp = CDetails.QtyToInsp;
                model.QtyPassed = CDetails.QtyPassed;
                model.QtyRejected = CDetails.QtyRejected;
                model.QtyDue = CDetails.QtyDue;

                return model;
            }
        }
    }
}
