using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces.InspectionBilling;
using IBS.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;

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
                    model.Consignee = Convert.ToString(GetDetails.C.ConsigneeCd);
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
                    //model.OtherInspDt = Convert.ToDateTime(GetDetails.C.OtherInspDt);
                    model.OtherInspDt = !string.IsNullOrEmpty(GetDetails.C.OtherInspDt) ? Convert.ToString(GetDetails.C.OtherInspDt) : null;

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
                        model.BpoFee = Convert.ToDecimal(query2.BpoFee);
                    }
                    //model.AdjustmentFee = query2.BpoFee;
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
                if (GetDetails.C.IrfcFunded == "Y")
                {
                    get_legalname(model, Convert.ToInt32(model.IrfcBpo), "B");
                }
                else
                {
                    get_legalname(model, Convert.ToInt32(model.ConsigneeCd), "C");
                }

            }
            if (Bill != null)
            {
                //Bill Details
                model.Caseno = Bill.CaseNo;
                model.BillNo = Bill.BillNo;
                model.BillDt = Bill.BillDt;
                model.TMValue = Convert.ToDecimal(Bill.MaterialValue);
                model.BpoFeeType = Bill.FeeType;
                model.Rate = Bill.FeeRate;
                model.RlyBpoFee = Convert.ToDecimal(Bill.FeeRate);
                model.AdjustmentFee = Convert.ToDecimal(Bill.FeeRate);
                model.TaxType = Bill.TaxType;
                model.TIFee = Convert.ToDecimal(Bill.InspFee);
                model.ServiceTax = Bill.ServiceTax;
                model.EduCess = Bill.EduCess;
                model.SheCess = Bill.SheCess;
                model.MinFee = Convert.ToInt32(Bill.MinFee);
                model.MaxFee = Convert.ToInt32(Bill.MaxFee);
                model.NetFee = Convert.ToDecimal(Bill.BillAmount);
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

                model.TMValueDiff = (Convert.ToDecimal(model.TMValue) - Convert.ToDecimal(model.TMValueNew));
                model.TIFeeDiff = (Convert.ToDecimal(model.TIFee) - Convert.ToDecimal(model.TIFeeNew));
                model.NetFeeDiff = (Convert.ToDecimal(model.NetFee) - Convert.ToDecimal(model.NetFeeNew));


            }
            return model;
        }

        public DTResult<InspectionCertModel> GetLoadTableDetails(DTParameters dtParameters, string Region)
        {
            DTResult<InspectionCertModel> dTResult = new() { draw = 0 };
            IQueryable<InspectionCertModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "" || orderCriteria == null)
                {
                    orderCriteria = "ItemSrnoPo";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
            }
            else
            {
                orderCriteria = "ItemSrnoPo";
                orderAscendingDirection = true;
            }

            string Caseno = "", Callrecvdt = "", Callsno = "", Consignee = "";

            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["Caseno"]))
            {
                Caseno = Convert.ToString(dtParameters.AdditionalValues["Caseno"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["Callrecvdt"]))
            {
                Callrecvdt = Convert.ToString(dtParameters.AdditionalValues["Callrecvdt"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["Callsno"]))
            {
                Callsno = Convert.ToString(dtParameters.AdditionalValues["Callsno"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["Consignee"]))
            {
                Consignee = Convert.ToString(dtParameters.AdditionalValues["Consignee"]);
            }

            Caseno = Caseno.ToString() == "" ? string.Empty : Caseno.ToString();
            //DateTime? _CallRecvDt = Callrecvdt == "" ? null : DateTime.ParseExact(Callrecvdt, "dd/MM/yyyy", null);
            Callsno = Callsno.ToString() == "" ? string.Empty : Callsno.ToString();
            Consignee = Consignee.ToString() == "" ? string.Empty : Consignee.ToString();

            query = from c in context.T18CallDetails
                    join p in context.T15PoDetails on c.CaseNo equals p.CaseNo
                    join u in context.T04Uoms on p.UomCd equals u.UomCd
                    where c.ItemSrnoPo == p.ItemSrno && c.CaseNo == Caseno
                           && c.CallRecvDt == Convert.ToDateTime(Callrecvdt)
                           && c.CallSno == Convert.ToInt16(Callsno)
                           && c.ConsigneeCd == Convert.ToInt32(Consignee)
                    select new InspectionCertModel
                    {
                        Caseno = c.CaseNo,
                        Callrecvdt = c.CallRecvDt,
                        Callsno = c.CallSno,
                        ItemSrnoPo = c.ItemSrnoPo,
                        ItemDescPo = c.ItemDescPo,
                        UomSDesc = u.UomSDesc,
                        QtyOrdered = c.QtyOrdered,
                        CumQtyPrevOffered = c.CumQtyPrevOffered,
                        CumQtyPrevPassed = c.CumQtyPrevPassed,
                        QtyToInsp = c.QtyToInsp,
                        QtyPassed = c.QtyPassed,
                        QtyRejected = c.QtyRejected,
                        QtyDue = c.QtyDue,
                        Rate = p.Rate,
                        SuppNewRate = Convert.ToDecimal(c.SuppNewRate),
                        SalesTaxPer = p.SalesTaxPer,
                        SalesTax = p.SalesTax,
                        ExcisePer = p.ExcisePer,
                        Excise = p.Excise,
                        DiscountPer = p.DiscountPer,
                        Discount = p.Discount,
                        OtherCharges = p.OtherCharges,
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.ItemSrnoPo).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
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
                if (GetLegalD != null)
                {
                    model.LegalName = GetLegalD.LegalName;
                    model.City = GetLegalD.City;
                    model.State = GetLegalD.State;
                    model.Pincode = GetLegalD.Pincode;
                    model.GstinNo = GetLegalD.GstinNo;
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
                    model.GstinNo = GetLegalD.GstinNo;
                }
            }
        }

        public InspectionCertModel FindByItemID(string Caseno, DateTime Callrecvdt, int Callsno, int ItemSrnoPo)
        {
            InspectionCertModel model = new();
            var query = (from c in context.T18CallDetails
                         join p in context.T15PoDetails on c.CaseNo equals p.CaseNo
                         join u in context.T04Uoms on p.UomCd equals u.UomCd
                         where c.CaseNo == Caseno && c.CallRecvDt == Callrecvdt && c.CallSno == Callsno && c.ItemSrnoPo == ItemSrnoPo

                         select new
                         {
                             c,
                             p,
                             u
                         }).FirstOrDefault();
            var CDetails = query;

            if (CDetails != null)
            {
                model.Caseno = CDetails.c.CaseNo;
                model.Callrecvdt = CDetails.c.CallRecvDt;
                model.Callsno = CDetails.c.CallSno;

                model.ItemSrnoPo = CDetails.c.ItemSrnoPo;
                model.ItemDescPo = CDetails.c.ItemDescPo;
                model.QtyOrdered = CDetails.c.QtyOrdered;
                model.QtyPassed = CDetails.c.QtyPassed;
                model.QtyToInsp = CDetails.c.QtyToInsp;
                model.QtyPassed = CDetails.c.QtyPassed;
                model.QtyRejected = CDetails.c.QtyRejected;
                model.QtyDue = CDetails.c.QtyDue;
                model.Rate = CDetails.p.Rate;
                model.SuppNewRate = Convert.ToDecimal(CDetails.c.SuppNewRate);
                model.SalesTaxPer = CDetails.p.SalesTaxPer;
                model.SalesTax = CDetails.p.SalesTax;
                model.ExcisePer = CDetails.p.ExcisePer;
                model.Excise = CDetails.p.Excise;
                model.DiscountPer = CDetails.p.DiscountPer;
                model.Discount = CDetails.p.Discount;
                model.OtherCharges = CDetails.p.OtherCharges;
                model.Consignee = Convert.ToString(CDetails.c.ConsigneeCd);
            }
            return model;
        }

        public string UpdateCallDetails(InspectionCertModel model, int ItemSrnoPo)
        {
            string ID = "";
            var CallDetails = context.T18CallDetails.Where(x => x.CaseNo == model.Caseno && x.CallRecvDt == model.Callrecvdt && x.CallSno == model.Callsno && x.ItemSrnoPo == ItemSrnoPo).FirstOrDefault();
            if (CallDetails != null)
            {
                CallDetails.SuppBFlag = "S";
                CallDetails.SuppNewRate = model.SuppNewRate;

                CallDetails.Updatedby = model.UserId;
                CallDetails.Updateddate = DateTime.Now.Date;

                context.SaveChanges();
                ID = Convert.ToString(CallDetails.ItemSrnoPo);

                var PODetails = context.T15PoDetails.Where(x => x.CaseNo == model.Caseno && x.ItemSrno == ItemSrnoPo && x.ConsigneeCd == Convert.ToInt32(model.Consignee)).FirstOrDefault();
                if (PODetails != null)
                {
                    PODetails.OtherCharges = model.OtherCharges;

                    PODetails.Updatedby = model.UserId;
                    PODetails.Updateddate = DateTime.Now.Date;
                    context.SaveChanges();
                }

            }
            return ID;
        }

        public ICPopUpModel FindByBillDetailsPopUp(string BillNo, string Region)
        {
            ICPopUpModel model = new();
            T22Bill Bill = context.T22Bills.Where(x => x.BillNo == BillNo).FirstOrDefault();

            if (Bill == null)
                throw new Exception("Call Record Not found");
            else
            {
                model.Caseno = Bill.CaseNo;
                model.BillNo = Bill.BillNo;
                model.TMValue = Convert.ToDecimal(Bill.MaterialValue);
                model.TIFee = Bill.InspFee;
                model.BillDt = Bill.BillDt;

                return model;
            }
        }

        public int financial_year_check(InspectionCertModel model)
        {
            var IcData = context.T20Ics.Where(ic => ic.BkNo.Trim() == model.Bkno && ic.SetNo == model.Setno && ic.CaseNo.Substring(0, 1) == model.Regioncode).Select(ic => ic.IcDt).FirstOrDefault();
            string myYear, myMonth;
            int fin_year_IC = 0;
            myYear = Convert.ToString(IcData).Substring(6, 4);
            myMonth = Convert.ToString(IcData).Substring(3, 2);

            if (Convert.ToInt16(myMonth) >= 4 && Convert.ToInt16(myMonth) <= 12)
            {
                fin_year_IC = Convert.ToInt16(myYear);
            }
            else
            {
                fin_year_IC = Convert.ToInt16(myYear) - 1;
            }

            string myYear1, myMonth1;
            int fin_year_BILL = 0;
            myYear1 = Convert.ToString(model.BillDt).Substring(6, 4);
            myMonth1 = Convert.ToString(model.BillDt).Substring(3, 2);
            if (Convert.ToInt16(myMonth1) >= 4 && Convert.ToInt16(myMonth1) <= 12)
            {
                fin_year_BILL = Convert.ToInt16(myYear1);
            }
            else
            {
                fin_year_BILL = Convert.ToInt16(myYear1) - 1;
            }
            if (fin_year_BILL == fin_year_IC)
            {
                return (0);
            }
            else
            {
                return (1);
            }

        }

        public string BillUpdate(InspectionCertModel model, string Region)
        {
            string str = "";
            if (model.BillNo == null || model.BillNo == "")
            {
                if (chk_bill_dt(Convert.ToString(model.BillDt), Region) == 1)
                {
                    if (model.IcTypeId == 9)
                    {
                        gen_credit_note(model);
                    }

                    if (model.CanOrRejctionFee == "Y" && model.BillNo == null)
                    {
                        var T13 = context.T13PoMasters.Where(x => x.CaseNo == model.Caseno && (x.PendingCharges == null || x.PendingCharges > 0)).FirstOrDefault();
                        if (T13 != null)
                        {
                            T13.PendingCharges = Convert.ToByte(Convert.ToInt32(T13.PendingCharges) - 1);
                            context.SaveChanges();
                        }
                        send_Vend_sms(model.Caseno, Region);
                    }
                }
            }
            else if (model.BillNo != null)
            {
                if (model.IcTypeId == 9)
                {
                    gen_credit_note(model);
                }
            }
            str = model.BillNo;
            return str;
        }

        int chk_bill_dt(string BillDt, string Region)
        {
            if (Region != "Q")
            {
                var allowstatus = context.T97ControlFiles.Where(x => x.Region == Region).Select(x => x.AllowOldBillDt).FirstOrDefault();
                var min_bill_dt = context.T87BillControls.Select(x => x.MinBillDt).FirstOrDefault();

                string myYear, myMonth, myDay;
                myYear = Convert.ToString(BillDt).Substring(6, 4);
                myMonth = Convert.ToString(BillDt).Substring(3, 2);
                myDay = Convert.ToString(BillDt).Substring(0, 2);
                string dt1 = myYear + myMonth + myDay;

                if (allowstatus == "N")
                {
                    //Bhavesh changes pending datetime.now.add - GraceDays
                    int? gDays = context.T97ControlFiles.Where(x => x.Region == Region).Select(x => x.GraceDays).FirstOrDefault();
                    string cDt = DateTime.Now.Date.ToString("yyyyMMdd");

                    int? grace_days = Convert.ToInt32(cDt) - Convert.ToInt32(gDays);

                    if (grace_days != null)
                    {
                        if (dt1.CompareTo(grace_days) > 0 || dt1.CompareTo(grace_days) == 0)
                        {
                            if (Convert.ToDateTime(BillDt).ToString("dd/MM/yyyy").CompareTo(Convert.ToDateTime(min_bill_dt).ToString("dd/MM/yyyy")) > 0)
                            {
                                return (1);
                            }
                            else
                            {
                                return (0);
                            }
                        }
                        else
                        {

                            return (0);
                        }
                    }
                    else
                    {
                        return (0);
                    }
                }
                else
                {
                    if (Convert.ToString(Convert.ToDateTime(BillDt).ToString("yyyyMMdd")).CompareTo(Convert.ToString(Convert.ToDateTime(min_bill_dt).ToString("yyyyMMdd"))) > 0)
                    {
                        return (1);
                    }
                    else
                    {
                        return (0);
                    }
                }
            }
            else
            {
                return (1);
            }
        }

        void gen_credit_note(InspectionCertModel model)
        {
            if (model.BpoCd != model.Bpo)
            {
                var T20 = context.T20Ics.Where(x => x.CaseNo == model.Caseno && x.CallRecvDt == model.Callrecvdt && x.CallSno == model.Callsno && x.ConsigneeCd == Convert.ToInt32(model.Consignee)).FirstOrDefault();
                if (T20 != null)
                {
                    T20.BpoCd = model.Bpo;
                    T20.IcTypeId = model.IcTypeId;
                    context.SaveChanges();
                }
            }
            string c_note_bno = "";
            if (model.BillNo != null)
            {
                //if (model.IcTypeId == 9)
                //{
                //    c_note_bno = model.CnoteBillNo;
                //}
                //else
                //{
                //    c_note_bno = model.BillNo;
                //}

                if (model.IcTypeId == 9)
                {
                    c_note_bno = model.BillNo;
                }
            }
            if (c_note_bno != null)
            {
                string myYear1, myMonth1, myDay1;

                myYear1 = Convert.ToString(model.CertDt).Substring(6, 4);
                myMonth1 = Convert.ToString(model.CertDt).Substring(3, 2);
                myDay1 = Convert.ToString(model.CertDt).Substring(0, 2);
                string certdt = myYear1 + myMonth1 + myDay1;

                string TaxType;
                if (Convert.ToInt32(certdt) >= 20170701)
                {
                    TaxType = model.TaxType;
                }
                else
                {
                    TaxType = model.BpoTaxType;
                }
                int NoOfInsp;
                if (model.NoOfInsp == 0)
                {
                    NoOfInsp = 1;
                }
                else
                {
                    NoOfInsp = model.NoOfInsp;
                }
                string InvoiceNo = null;
                if (Convert.ToInt32(certdt) >= 20170701)
                {

                    if (model.InvoiceNo == null || model.InvoiceNo.Length < 13 || model.CnoteBillNo == null)
                    {
                        if (model.Regioncode == "N")
                        {
                            InvoiceNo = "0708";
                        }
                        else if (model.Regioncode == "W")
                        {
                            InvoiceNo = "2705";
                        }
                        else if (model.Regioncode == "E")
                        {
                            InvoiceNo = "1906";
                        }
                        else if (model.Regioncode == "S")
                        {
                            InvoiceNo = "3307";
                        }
                        else if (model.Regioncode == "C")
                        {
                            InvoiceNo = "2210";
                        }
                        else if (model.Regioncode == "Q")
                        {
                            InvoiceNo = "0708";
                        }
                        model.BillDt = DateTime.Now.Date;
                    }
                    else
                    {
                        InvoiceNo = model.InvoiceNo;
                    }
                }
                int MaxFee;
                if (model.MaxFee == null || model.MaxFee == 0)
                {
                    MaxFee = -1;
                }
                else
                {
                    MaxFee = Convert.ToInt32(model.MaxFee);
                }
                int MinFee;
                if (model.MinFee == null || model.MinFee == 0)
                {
                    MinFee = 0;
                }
                else
                {
                    MinFee = Convert.ToInt32(model.MinFee);
                }
                string reg_cd = "";
                if (model.Regioncode == "N")
                {
                    reg_cd = "O";
                }
                else if (model.Regioncode == "S")
                {
                    reg_cd = "T";
                }
                else if (model.Regioncode == "W")
                {
                    reg_cd = "X";
                }
                else if (model.Regioncode == "E")
                {
                    reg_cd = "F";
                }
                else if (model.Regioncode == "C")
                {
                    reg_cd = "D";
                }
                else if (model.Regioncode == "Q")
                {
                    reg_cd = "R";
                }

                DataSet ds = new DataSet();

                OracleParameter[] parameter = new OracleParameter[22];
                parameter[0] = new OracleParameter("in_region_cd", OracleDbType.Varchar2, 1, reg_cd, ParameterDirection.Input);
                parameter[1] = new OracleParameter("in_case_no", OracleDbType.Varchar2, 10, model.Caseno, ParameterDirection.Input);
                parameter[2] = new OracleParameter("in_call_recv_dt", OracleDbType.Date, model.Callrecvdt, ParameterDirection.Input);
                parameter[3] = new OracleParameter("in_call_sno", OracleDbType.Int32, model.Callsno, ParameterDirection.Input);
                parameter[4] = new OracleParameter("in_consignee_cd", OracleDbType.Int32, model.Consignee, ParameterDirection.Input);
                parameter[5] = new OracleParameter("in_bill", OracleDbType.Varchar2, 10, model.CnoteBillNo == null ? "Z" : model.BillNo, ParameterDirection.Input);
                parameter[6] = new OracleParameter("in_fee_type", OracleDbType.Varchar2, 1, model.BpoFeeType, ParameterDirection.Input);
                parameter[7] = new OracleParameter("in_fee", OracleDbType.Decimal, model.AdjustmentFee, ParameterDirection.Input);
                parameter[8] = new OracleParameter("in_tax_type", OracleDbType.Varchar2, 1, TaxType, ParameterDirection.Input);
                parameter[9] = new OracleParameter("in_no_of_insp", OracleDbType.Int32, NoOfInsp, ParameterDirection.Input);
                parameter[10] = new OracleParameter("in_invoice", OracleDbType.Varchar2, InvoiceNo, ParameterDirection.Input);
                parameter[11] = new OracleParameter("in_max_fee", OracleDbType.Int32, MaxFee, ParameterDirection.Input);
                parameter[12] = new OracleParameter("in_min_fee", OracleDbType.Int32, MinFee, ParameterDirection.Input);
                parameter[13] = new OracleParameter("in_bill_dt", OracleDbType.Varchar2, Convert.ToDateTime(model.BillDt).ToString("ddMMyyyy"), ParameterDirection.Input);
                parameter[14] = new OracleParameter("in_adv_bill", OracleDbType.Varchar2, 1, model.AdvBill, ParameterDirection.Input);

                parameter[15] = new OracleParameter("in_material_value_new", OracleDbType.Int32, model.TMValueNew, ParameterDirection.Input);
                parameter[16] = new OracleParameter("in_insp_fee_new", OracleDbType.Int32, model.TIFeeNew, ParameterDirection.Input);
                parameter[17] = new OracleParameter("in_bill_amount_new", OracleDbType.Int32, model.NetFeeNew, ParameterDirection.Input);
                parameter[18] = new OracleParameter("in_material_value_diff", OracleDbType.Int32, model.TMValueDiff, ParameterDirection.Input);
                parameter[19] = new OracleParameter("in_insp_fee_diff", OracleDbType.Int32, model.TIFeeDiff, ParameterDirection.Input);
                parameter[20] = new OracleParameter("in_bill_amount_diff", OracleDbType.Int32, model.NetFeeDiff, ParameterDirection.Input);

                parameter[21] = new OracleParameter("p_result_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

                ds = DataAccessDB.GetDataSet("SP_GENERATE_CREDIT_NOTE_NEW", parameter);

                if (ds != null && ds.Tables.Count > 0)
                {
                    model.UpdateStatus = Convert.ToString(ds.Tables[0].Rows[0]["OUT_ERR_CD"]);
                }
                if (model.UpdateStatus == "0")
                {
                    var str3 = context.T22Bills.Where(x => x.BillNo == model.BillNo).FirstOrDefault();
                    if (str3 != null)
                    {
                        model.TMValue = Convert.ToDecimal(str3.MaterialValue);
                        model.TIFee = Convert.ToDecimal(str3.InspFee);
                        model.NetFee = Convert.ToDecimal(str3.BillAmount);
                        model.InvoiceNo = str3.InvoiceNo;
                        model.CnoteBillNo = Convert.ToString(ds.Tables[0].Rows[0]["OUT_BILL"]);
                    }
                    var Cnote_bill_dtls = context.T22Bills.Where(x => x.BillNo == model.BillNo).FirstOrDefault();
                    double w_bill_amt = 0, w_tds_amt = 0, w_amt_rec = 0, w_ret_amt = 0, w_writeoff_amt = 0;
                    if (Cnote_bill_dtls != null)
                    {
                        w_bill_amt = Convert.ToDouble(Cnote_bill_dtls.BillAmount);
                        w_tds_amt = Convert.ToDouble(Cnote_bill_dtls.Tds);
                        w_amt_rec = Convert.ToDouble(Cnote_bill_dtls.AmountReceived);
                        w_ret_amt = Convert.ToDouble(Cnote_bill_dtls.RetentionMoney);
                        w_writeoff_amt = Convert.ToDouble(Cnote_bill_dtls.WriteOffAmt);
                    }
                    decimal totalBillAmount = context.T22Bills.Where(x => x.BillNo == Convert.ToString(ds.Tables[0].Rows[0]["OUT_BILL"])).Select(x => x.BillAmount ?? 0).DefaultIfEmpty().Sum();
                    decimal cmdCNoteAmt = Math.Abs(totalBillAmount);
                    int w_cnote_amt = Convert.ToInt32(cmdCNoteAmt);

                    var str_cnote_bill = context.T22Bills.Where(b => b.BillNo == model.BillNo).FirstOrDefault();
                    if (str_cnote_bill != null)
                    {
                        //str_cnote_bill.CnoteAmount = w_cnote_amt;
                        str_cnote_bill.BillAmtCleared = Convert.ToDecimal(w_amt_rec + w_tds_amt + w_ret_amt + w_writeoff_amt + w_cnote_amt);
                        context.SaveChanges();
                    }

                    var str = context.T22Bills.Where(b => b.BillNo == Convert.ToString(ds.Tables[0].Rows[0]["OUT_BILL"])).FirstOrDefault();
                    if (str != null)
                    {
                        str.Remarks = model.Remarks;
                        str.UserId = model.UserId;
                        str.Datetime = DateTime.Now.Date;
                        str.CnoteAmount = w_cnote_amt;
                        str.CnoteBillNo = model.BillNo;
                        context.SaveChanges();
                    }



                    var AType = context.T22AdjustmentBills.Where(x => x.BillNoN == Convert.ToString(ds.Tables[0].Rows[0]["OUT_BILL"])).FirstOrDefault();
                    if (AType == null)
                    {
                        T22AdjustmentBill T22 = new()
                        {
                            BillNoN = Convert.ToString(ds.Tables[0].Rows[0]["OUT_BILL"]),
                            BillNoO = model.BillNo,
                            CaseNo = model.Caseno,
                            Billadtype = model.BillAdType,
                            UserId = model.UserId,
                            Datetime = DateTime.Now.Date,
                        };
                        context.T22AdjustmentBills.Add(T22);
                        context.SaveChanges();
                    }
                    else
                    {
                        AType.Billadtype = model.BillAdType;
                        //AType.ReferenceAid = AType.Aid;
                        context.SaveChanges();
                    }

                    var strUpdateCnoteAmt = context.T22Bills.Where(b => b.BillNo == Convert.ToString(ds.Tables[0].Rows[0]["OUT_BILL"])).FirstOrDefault();
                    if (strUpdateCnoteAmt != null)
                    {
                        strUpdateCnoteAmt.AmountReceived = w_cnote_amt;
                        strUpdateCnoteAmt.BillAmtCleared = w_cnote_amt;

                        strUpdateCnoteAmt.Billadtype = model.BillAdType;

                        int Aid = context.T22AdjustmentBills.Where(x => x.BillNoN == Convert.ToString(ds.Tables[0].Rows[0]["OUT_BILL"])).Select(x => x.Aid).FirstOrDefault();
                        strUpdateCnoteAmt.ReferenceAid = Aid;

                        context.SaveChanges();
                    }
                    model.BillNo = Convert.ToString(ds.Tables[0].Rows[0]["OUT_BILL"]);
                }
            }

        }

        public async Task<string> send_Vend_sms(string CaseNo, string Region)
        {
            string sms = "";
            string sender = "";
            string wVendor = "", wVendMobile = "";
            if (Region == "N")
                sender = "RITES/NR";
            else if (Region == "W")
                sender = "RITES/WR";
            else if (Region == "E")
                sender = "RITES/ER";
            else if (Region == "S")
                sender = "RITES/SR";
            else
                sender = "RITES";
            var query = from v in context.T05Vendors
                        join c in context.T03Cities on v.VendCityCd equals c.CityCd
                        join t13 in context.T13PoMasters on v.VendCd equals t13.VendCd
                        where t13.CaseNo == CaseNo
                        select new
                        {
                            VendName = (v.VendName.Substring(0, Math.Min(v.VendName.Length, 30)).Replace("&", "AND")) + "," + (c.City.Substring(0, Math.Min(c.City.Length, 12))),
                            VendTel = (v.VendContactTel1.Substring(0, Math.Min(v.VendContactTel1.Length, 10))).Trim()
                        };
            var result = query.FirstOrDefault();
            if (result != null)
            {
                wVendor = result.VendName;
                wVendMobile = result.VendTel;
            }
            string message = "FIRM NAME-" + wVendor + ". The Billing has generated for the Case No." + CaseNo + ", against Call Cancellation/Rejection Charges Submitted by you. Kindly try to register new call now." + " - RITES LTD" + sender;
            if (!string.IsNullOrEmpty(wVendMobile))
            {
                using (HttpClient client = new HttpClient())
                {
                    //string baseurl = $"http://apin.onex-aura.com/api/sms?key=QtPr681q&to={wVendMobile}&from=RITESI&body={message}&entityid=1501628520000011823&templateid=1707161588918541674";
                    string baseurl = $"http://apin.onex-aura.com/api/sms?key=QtPr681q&to={wVendMobile}&from=RITESI&body={message}&entityid=1501628520000011823&templateid=1707161648485350941";

                    HttpResponseMessage response = await client.GetAsync(baseurl);
                    response.EnsureSuccessStatusCode(); // Ensure a successful response

                    string responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseBody);

                    sms = "success";
                }
            }
            return sms;
        }

        //public DTResult<InspectionCertItemListModel> FindByFeesDetails(string Caseno, string Callrecvdt, int Callsno, string Consignee, string BillNo, decimal AdjustmentFee)
        //{
        //    DTResult<InspectionCertItemListModel> dTResult = new() { draw = 0 };
        //    int err = 0;

        //    var query = (from c in context.T18CallDetails
        //                 join p in context.T15PoDetails on c.CaseNo equals p.CaseNo
        //                 join u in context.T04Uoms on p.UomCd equals u.UomCd
        //                 where c.ItemSrnoPo == p.ItemSrno && c.CaseNo == Caseno
        //                        && c.CallRecvDt == Convert.ToDateTime(Callrecvdt)
        //                        && c.CallSno == Callsno
        //                        && c.ConsigneeCd == Convert.ToInt32(Consignee)
        //                 select new InspectionCertItemListModel
        //                 {
        //                     ItemSrnoPo = c.ItemSrnoPo,
        //                     ItemDescPo = c.ItemDescPo,
        //                     UomSDesc = u.UomSDesc,
        //                     QtyOrdered = c.QtyOrdered,
        //                     CumQtyPrevOffered = c.CumQtyPrevOffered,
        //                     CumQtyPrevPassed = c.CumQtyPrevPassed,
        //                     QtyToInsp = c.QtyToInsp,
        //                     QtyPassed = c.QtyPassed,
        //                     QtyRejected = c.QtyRejected,
        //                     QtyDue = c.QtyDue,
        //                     Rate = p.Rate,
        //                     SuppNewRate = Convert.ToDecimal(c.SuppNewRate),
        //                     SalesTaxPer = p.SalesTaxPer,
        //                     SalesTax = p.SalesTax,
        //                     ExciseType = p.ExciseType,
        //                     ExcisePer = p.ExcisePer,
        //                     Excise = p.Excise,
        //                     DiscountType = p.DiscountType,
        //                     DiscountPer = p.DiscountPer,
        //                     Discount = p.Discount,
        //                     OtherCharges = p.OtherCharges,
        //                     OtChargePer = p.OtChargePer,
        //                     OtChargeType = p.OtChargeType
        //                 }).ToList();

        //    decimal basevalue = 0;
        //    decimal discountamount = 0;
        //    decimal exciseamount = 0;
        //    decimal stamount = 0;
        //    decimal otheramount = 0;


        //    decimal totalvalue1 = 0;
        //    decimal totalvalue2 = 0;
        //    decimal totalvalue3 = 0;
        //    decimal totalvalue = 0;

        //    for (int i = 0; i < query.Count; i++)
        //    {
        //        InspectionCertItemListModel dataItem = query[i];
        //        err = 0;

        //        int srno = (byte)dataItem.ItemSrnoPo;

        //        decimal qty = Convert.ToDecimal(dataItem.QtyOrdered);
        //        decimal qtyOffNow = Convert.ToDecimal(dataItem.QtyPassed);

        //        decimal SuppNewRateP = Convert.ToDecimal(dataItem.SuppNewRate);

        //        decimal SalesTaxP = Convert.ToDecimal(dataItem.SalesTax);
        //        decimal SalesTaxPerP = Convert.ToDecimal(dataItem.SalesTaxPer);
        //        decimal ExcisePerP = Convert.ToDecimal(dataItem.ExcisePer);
        //        decimal DiscountPerP = Convert.ToDecimal(dataItem.DiscountPer);
        //        decimal OtChargePerP = Convert.ToDecimal(dataItem.OtChargePer);
        //        decimal OtherChargesP = Convert.ToDecimal(dataItem.OtherCharges);
        //        string DiscountTypeP = dataItem.DiscountType;
        //        string ExciseTypeP = dataItem.ExciseType;
        //        string OtChargeTypeP = dataItem.OtChargeType;

        //        int uom = 1;
        //        if (qtyOffNow > 0)
        //        {
        //            err = 1;
        //        }
        //        if (err == 1)
        //        {
        //            //Base value Calculation
        //            basevalue += Convert.ToDecimal((qtyOffNow * SuppNewRateP) / uom);
        //            //DisCount Calculation
        //            if (DiscountTypeP == "P")
        //            {
        //                discountamount += Convert.ToDecimal((basevalue * DiscountPerP) / 100);
        //            }
        //            else if (DiscountTypeP == "L")
        //            {
        //                discountamount += DiscountPerP;
        //            }
        //            else if (DiscountTypeP == "N")
        //            {
        //                discountamount += Convert.ToDecimal(DiscountPerP * qtyOffNow);
        //            }
        //            else
        //            {
        //                discountamount += 0;
        //            }
        //            //Exise Calculation
        //            if(qtyOffNow == qty)
        //            {
        //                if (ExciseTypeP == "P")
        //                {
        //                    exciseamount += Convert.ToDecimal(((basevalue - discountamount) * ExcisePerP) / 100);
        //                }
        //                else if (ExciseTypeP == "L")
        //                {
        //                    exciseamount += Convert.ToDecimal(ExcisePerP);
        //                }
        //                else
        //                {
        //                    exciseamount += 0;
        //                }
        //            }
        //            else
        //            {
        //                exciseamount += 0;
        //            }

        //            //Salse Tax
        //            if (SalesTaxPerP > 0)
        //            {
        //                //stamount += Convert.ToDecimal((qtyOffNow / qty) * SalesTaxP);
        //                stamount += Convert.ToDecimal((((basevalue - discountamount) + exciseamount) * SalesTaxPerP) / 100);
        //            }
        //            else
        //            {
        //                //stamount += Convert.ToDecimal(((basevalue - discountamount) * SalesTaxPerP) / 100);
        //                stamount += 0;
        //            }

        //            //Other Charges
        //            if (OtChargeTypeP == "P")
        //            {
        //                otheramount = Convert.ToDecimal((basevalue * OtChargePerP) / 100);
        //            }
        //            else if (OtChargeTypeP == "L")
        //            {
        //                otheramount += Convert.ToDecimal(OtChargePerP);
        //            }
        //            else if (OtChargeTypeP == "N")
        //            {
        //                otheramount += Convert.ToDecimal((qtyOffNow * OtChargePerP) / 100);
        //            }
        //            else
        //            {
        //                otheramount += 0;
        //            }

        //            //Total Calculation
        //            totalvalue1 = Convert.ToDecimal(basevalue - discountamount);
        //            totalvalue2 = Convert.ToDecimal(totalvalue1 + exciseamount);
        //            totalvalue3 = Convert.ToDecimal(totalvalue2 + stamount);
        //            totalvalue = Convert.ToDecimal(totalvalue3 + otheramount);
        //        }
        //    }

        //    decimal MaxFee = 0;
        //    decimal MinFee = 0;
        //    decimal in_fee = 0;

        //    var BillDetails = context.T22Bills.Where(x=>x.BillNo == BillNo).FirstOrDefault();
        //    if(BillDetails != null)
        //    {

        //    }

        //    dTResult.recordsTotal = Convert.ToInt32(totalvalue);
        //    //dTResult.data = query;
        //    return dTResult;
        //}

        public InspectionCertModel FindByFeesDetails(string Caseno, string Callrecvdt, int Callsno, string Consignee, string BillNo, decimal AdjustmentFee, int ConsigneeCd, string BillAdType)
        {
            //DTResult<InspectionCertItemListModel> dTResult = new() { draw = 0 };
            InspectionCertModel model = new();
            int err = 0;

            var query = (from c in context.T18CallDetails
                         join p in context.T15PoDetails on c.CaseNo equals p.CaseNo
                         join u in context.T04Uoms on p.UomCd equals u.UomCd
                         where c.ItemSrnoPo == p.ItemSrno && c.CaseNo == Caseno
                                && c.CallRecvDt == Convert.ToDateTime(Callrecvdt)
                                && c.CallSno == Callsno
                                && c.ConsigneeCd == Convert.ToInt32(Consignee)
                         select new InspectionCertItemListModel
                         {
                             ItemSrnoPo = c.ItemSrnoPo,
                             ItemDescPo = c.ItemDescPo,
                             UomSDesc = u.UomSDesc,
                             QtyOrdered = c.QtyOrdered,
                             CumQtyPrevOffered = c.CumQtyPrevOffered,
                             CumQtyPrevPassed = c.CumQtyPrevPassed,
                             QtyToInsp = c.QtyToInsp,
                             QtyPassed = c.QtyPassed,
                             QtyRejected = c.QtyRejected,
                             QtyDue = c.QtyDue,
                             Rate = p.Rate,
                             SuppNewRate = Convert.ToDecimal(c.SuppNewRate),
                             SalesTaxPer = p.SalesTaxPer,
                             SalesTax = p.SalesTax,
                             ExciseType = p.ExciseType,
                             ExcisePer = p.ExcisePer,
                             Excise = p.Excise,
                             DiscountType = p.DiscountType,
                             DiscountPer = p.DiscountPer,
                             Discount = p.Discount,
                             OtherCharges = p.OtherCharges,
                             OtChargePer = p.OtChargePer,
                             OtChargeType = p.OtChargeType
                         }).ToList();

            decimal basevalue = 0;
            decimal discountamount = 0;
            decimal exciseamount = 0;
            decimal stamount = 0;
            decimal otheramount = 0;


            decimal totalvalue1 = 0;
            decimal totalvalue2 = 0;
            decimal totalvalue3 = 0;
            decimal totalvalue = 0;
            decimal totalvalueFinal = 0;

            decimal w_insp_fee = 0;

            decimal w_igst = 0;
            decimal w_cgst = 0;
            decimal w_sgst = 0;

            decimal w_total_fee = 0;

            for (int i = 0; i < query.Count; i++)
            {
                InspectionCertItemListModel dataItem = query[i];
                err = 0;

                int srno = (byte)dataItem.ItemSrnoPo;

                decimal qty = Convert.ToDecimal(dataItem.QtyOrdered);
                decimal qtyOffNow = Convert.ToDecimal(dataItem.QtyPassed);

                decimal SuppNewRateP = Convert.ToDecimal(dataItem.SuppNewRate);

                decimal SalesTaxP = Convert.ToDecimal(dataItem.SalesTax);
                decimal SalesTaxPerP = Convert.ToDecimal(dataItem.SalesTaxPer);
                decimal ExcisePerP = Convert.ToDecimal(dataItem.ExcisePer);
                decimal DiscountPerP = Convert.ToDecimal(dataItem.DiscountPer);
                decimal OtChargePerP = Convert.ToDecimal(dataItem.OtChargePer);
                decimal OtherChargesP = Convert.ToDecimal(dataItem.OtherCharges);
                string DiscountTypeP = dataItem.DiscountType;
                string ExciseTypeP = dataItem.ExciseType;
                string OtChargeTypeP = dataItem.OtChargeType;

                int uom = 1;
                if (qtyOffNow > 0)
                {
                    err = 1;
                }
                if (err == 1)
                {
                    //Base value Calculation
                    basevalue = Convert.ToDecimal((qtyOffNow * SuppNewRateP) / uom);
                    //DisCount Calculation
                    if (DiscountTypeP == "P")
                    {
                        discountamount = Convert.ToDecimal((basevalue * DiscountPerP) / 100);
                    }
                    else if (DiscountTypeP == "L")
                    {
                        discountamount = DiscountPerP;
                    }
                    else if (DiscountTypeP == "N")
                    {
                        discountamount = Convert.ToDecimal(DiscountPerP * qtyOffNow);
                    }
                    else
                    {
                        discountamount = 0;
                    }
                    //Exise Calculation
                    if (BillAdType == "Credit")
                    {
                        if (qtyOffNow == qty)
                        {
                            if (ExciseTypeP == "P")
                            {
                                exciseamount = Convert.ToDecimal(((basevalue - discountamount) * ExcisePerP) / 100);
                            }
                            else if (ExciseTypeP == "L")
                            {
                                exciseamount = Convert.ToDecimal(ExcisePerP);
                            }
                            else
                            {
                                exciseamount = 0;
                            }
                        }
                        else
                        {
                            exciseamount = 0;
                        }
                    }
                    else
                    {
                        if (ExciseTypeP == "P")
                        {
                            exciseamount = Convert.ToDecimal(((basevalue - discountamount) * ExcisePerP) / 100);
                        }
                        else if (ExciseTypeP == "L")
                        {
                            exciseamount = Convert.ToDecimal(ExcisePerP);
                        }
                        else
                        {
                            exciseamount = 0;
                        }
                    }

                    //Salse Tax
                    if (SalesTaxPerP > 0)
                    {
                        //stamount += Convert.ToDecimal((qtyOffNow / qty) * SalesTaxP);
                        stamount = Convert.ToDecimal((((basevalue - discountamount) + exciseamount) * SalesTaxPerP) / 100);
                    }
                    else
                    {
                        //stamount += Convert.ToDecimal(((basevalue - discountamount) * SalesTaxPerP) / 100);
                        stamount = 0;
                    }

                    //Other Charges
                    if (OtChargeTypeP == "P")
                    {
                        otheramount = Convert.ToDecimal((basevalue * OtChargePerP) / 100);
                    }
                    else if (OtChargeTypeP == "L")
                    {
                        otheramount = Convert.ToDecimal(OtChargePerP);
                    }
                    else if (OtChargeTypeP == "N")
                    {
                        otheramount = Convert.ToDecimal((qtyOffNow * OtChargePerP) / 100);
                    }
                    else
                    {
                        otheramount = 0;
                    }

                    //Total Calculation
                    totalvalue1 = Convert.ToDecimal(basevalue - discountamount);
                    totalvalue2 = Convert.ToDecimal(totalvalue1 + exciseamount);
                    totalvalue3 = Convert.ToDecimal(totalvalue2 + stamount);
                    totalvalue = Convert.ToDecimal(totalvalue3 + otheramount);
                    totalvalueFinal += totalvalue;
                    model.TMValueNew = totalvalueFinal;
                }
            }

            var BillDetails = context.T22Bills.Where(x => x.BillNo == BillNo).FirstOrDefault();
            if (BillDetails != null)
            {
                model.MaxFee = Convert.ToInt32(BillDetails.MaxFee);
                model.MinFee = Convert.ToInt32(BillDetails.MinFee);
                model.TIFeeNew = Convert.ToDecimal(BillDetails.InspFee);
                model.BpoFeeType = BillDetails.FeeType;
                model.TaxType = BillDetails.TaxType;
            }
            var T20Details = context.T20Ics.Where(x => x.CaseNo == Caseno && x.CallRecvDt == Convert.ToDateTime(Callrecvdt) && x.CallSno == Callsno).FirstOrDefault();
            if (T20Details != null)
            {
                model.NoOfInsp = Convert.ToInt32(T20Details.NoOfInsp);
            }

            if (model.BpoFeeType == "D" || model.BpoFeeType == "H")
            {
                w_insp_fee = Convert.ToDecimal(model.NoOfInsp * AdjustmentFee);
            }
            else if (model.BpoFeeType == "P")
            {
                w_insp_fee = Convert.ToDecimal((totalvalue * AdjustmentFee) / 100);
            }
            else if (model.BpoFeeType == "L")
            {
                w_insp_fee = AdjustmentFee;
            }
            else
            {
                w_insp_fee = 0;
            }

            if ((model.MinFee > 0) && (model.MinFee > w_insp_fee))
            {
                w_insp_fee += Convert.ToDecimal(model.MinFee);
            }
            if ((model.MaxFee >= 0) && (model.MaxFee < w_insp_fee))
            {
                w_insp_fee += Convert.ToDecimal(model.MaxFee);
            }
            model.TIFeeNew = w_insp_fee;

            var Ic_Dt = context.T20Ics.Where(x => x.CaseNo == Caseno && x.CallRecvDt == Convert.ToDateTime(Callrecvdt) && x.CallSno == Callsno && x.ConsigneeCd == ConsigneeCd).Select(x => x.IcDt).FirstOrDefault();
            var igst_rate = context.T89Gsts.Where(x => x.DtFrom >= Ic_Dt && x.DtTo <= Ic_Dt).Select(x => x.IgstRate ?? 0).FirstOrDefault();
            var sgst_rate = context.T89Gsts.Where(x => x.DtFrom >= Ic_Dt && x.DtTo <= Ic_Dt).Select(x => x.SgstRate ?? 0).FirstOrDefault();
            var cgst_rate = context.T89Gsts.Where(x => x.DtFrom >= Ic_Dt && x.DtTo <= Ic_Dt).Select(x => x.CgstRate ?? 0).FirstOrDefault();
            if (model.TaxType == "I")
            {
                w_igst = Convert.ToDecimal((w_insp_fee * Convert.ToDecimal(Convert.ToInt32(igst_rate) / 100)) / 100);
                w_cgst = 0;
                w_sgst = 0;
            }
            else if (model.TaxType == "C")
            {
                w_cgst = Convert.ToDecimal((w_insp_fee * Convert.ToDecimal(Convert.ToInt32(cgst_rate) / 100)) / 100);
                w_sgst = Convert.ToDecimal((w_insp_fee * Convert.ToDecimal(Convert.ToInt32(sgst_rate) / 100)) / 100);
                w_igst = 0;
            }
            else if (model.TaxType == "X")
            {
                w_cgst = 0;
                w_sgst = 0;
                w_igst = 0;
            }
            else if (model.TaxType == "Y")
            {
                w_total_fee = w_insp_fee;
                w_insp_fee = w_total_fee / (1 + (Convert.ToInt32(igst_rate) / 100));
                w_igst = Convert.ToDecimal(w_insp_fee * Convert.ToDecimal(igst_rate));
                w_cgst = 0;
                w_sgst = 0;
                w_insp_fee = Convert.ToDecimal(w_total_fee - (w_igst));
            }
            else if (model.TaxType == "Z")
            {
                w_total_fee = w_insp_fee;
                w_insp_fee = w_total_fee / (1 + (Convert.ToInt32(cgst_rate) / 100) + (Convert.ToInt32(sgst_rate) / 100));
                w_igst = 0;
                w_cgst = Convert.ToDecimal(w_insp_fee * (Convert.ToInt32(cgst_rate) / 100));
                w_sgst = Convert.ToDecimal(w_insp_fee * (Convert.ToInt32(sgst_rate) / 100));
                w_insp_fee = w_total_fee - (w_cgst + w_sgst);
            }
            w_total_fee = Convert.ToDecimal(w_insp_fee + w_cgst + w_sgst + w_igst);

            model.NetFeeNew = w_total_fee;

            return model;
        }
    }
}
