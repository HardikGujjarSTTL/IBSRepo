using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces.InspectionBilling;
using IBS.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Drawing;

namespace IBS.Repositories.InspectionBilling
{
    public class InspectionCertRepository : IInspectionCertRepository
    {
        private readonly ModelContext context;

        public InspectionCertRepository(ModelContext context)
        {
            this.context = context;
        }

        public DTResult<InspectionCertModel> GetDataList(DTParameters dtParameters, string GetRegionCode)
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
                    orderCriteria = "Caseno";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
            }
            else
            {
                orderCriteria = "Caseno";
                orderAscendingDirection = true;
            }

            string Caseno = "", Callrecvdt = "", Callsno = "";

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

            Caseno = Caseno.ToString() == "" ? string.Empty : Caseno.ToString();
            DateTime? _CallRecvDt = Callrecvdt == "" ? null : DateTime.ParseExact(Callrecvdt, "dd/MM/yyyy", null);
            Callsno = Callsno.ToString() == "" ? string.Empty : Callsno.ToString();

            query = from l in context.ViewGetInspectionCertDetails
                    where l.Regioncode == GetRegionCode
                          && (Caseno == null || Caseno == "" || l.Caseno == Caseno)
                          && (Callrecvdt == null || Callrecvdt == "" || l.Callrecvdt == _CallRecvDt)
                          && (Callsno == null || Callsno == "" || l.Callsno == Convert.ToInt32(Callsno))
                    select new InspectionCertModel
                    {
                        Caseno = l.Caseno,
                        Callrecvdt = l.Callrecvdt,
                        Callsno = Convert.ToInt32(l.Callsno),
                        Icno = l.Icno,
                        Bkno = l.Bkno,
                        Setno = l.Setno,
                        Status = l.Status,
                        Iesname = l.Iesname,
                        Consignee = l.Consignee,
                        Callstatusdesc = l.Callstatusdesc,
                        Regioncode = l.Regioncode,
                        Callstatus = l.Callstatus,
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.Caseno).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public InspectionCertModel FindByID(string CaseNo, DateTime? CallRecvDt, int CallSno, string Bkno, string Setno, string GetRegionCode)
        {
            InspectionCertModel model = new();
            if (Bkno != "" || Setno != "")
            {
                var CallData = context.ViewGetInspectionCertDetails.Where(x => x.Caseno == CaseNo && x.Callrecvdt == CallRecvDt && x.Callsno == CallSno && x.Bkno == Bkno && x.Setno == Setno).FirstOrDefault();
                if (CallData == null)
                    return model;
                else
                {
                    model.Caseno = CallData.Caseno;
                    model.Callsno = CallData.Callsno;
                    model.Callrecvdt = CallData.Callrecvdt;

                    model.Bkno = CallData.Bkno;
                    model.Setno = CallData.Setno;
                }
            }
            else
            {
                var CallData = context.ViewGetInspectionCertDetails.Where(x => x.Caseno == CaseNo && x.Callrecvdt == CallRecvDt && x.Callsno == CallSno).FirstOrDefault();
                if (CallData == null)
                    return model;
                else
                {
                    model.Caseno = CallData.Caseno;
                    model.Callsno = CallData.Callsno;
                    model.Callrecvdt = CallData.Callrecvdt;
                }
            }

            return model;
        }

        public InspectionCertModel FindByInspDetailsID(string CaseNo, DateTime? CallRecvDt, int CallSno, string Bkno, string Setno, string ActionType, string GetRegionCode, int RoleId)
        {
            InspectionCertModel model = new();
            if (ActionType == "A")
            {
                var CallData = (from c in context.T17CallRegisters
                                join i in context.T09Ies on c.IeCd equals i.IeCd
                                where c.CaseNo == CaseNo && c.CallRecvDt == CallRecvDt && c.CallSno == CallSno
                                select new
                                {
                                    c,
                                    i
                                }).FirstOrDefault();

                var PODetails = (from p in context.T13PoMasters
                                 join v in context.T05Vendors on p.VendCd equals v.VendCd
                                 join c in context.T03Cities on v.VendCityCd equals c.CityCd
                                 where p.CaseNo == CaseNo
                                 select new
                                 {
                                     p,
                                     v,
                                     c
                                 }).FirstOrDefault();

                var GetConsigneeCd = GetConsigneeID(CaseNo, CallRecvDt, CallSno);



                var query1 = (from p in context.T14PoBpos
                              join b in context.T12BillPayingOfficers on p.BpoCd equals b.BpoCd
                              join c in context.T03Cities on b.BpoCityCd equals c.CityCd
                              where b.BpoCityCd == c.CityCd && p.CaseNo == CaseNo && p.ConsigneeCd == Convert.ToInt32(GetConsigneeCd)
                              select new
                              {
                                  p,
                                  b,
                                  c
                              }).FirstOrDefault();


                var GetBPO = (from b in context.T12BillPayingOfficers
                              join c in context.T03Cities on b.BpoCityCd equals c.CityCd
                              where (b.BpoCd.Trim().ToUpper() == query1.b.BpoCd.ToUpper() ||
                                     b.BpoName.Trim().ToUpper().StartsWith(query1.b.BpoCd.ToUpper()))
                              select new
                              {
                                  b,
                                  c
                              }).FirstOrDefault();

                if (CallData == null)
                    return model;
                else
                {
                    model.Caseno = CallData.c.CaseNo;
                    model.Callrecvdt = CallData.c.CallRecvDt;
                    model.Callsno = CallData.c.CallSno;
                    model.Bkno = CallData.c.BkNo;
                    model.Setno = CallData.c.SetNo;

                    model.IeName = CallData.i.IeName;
                    model.Iesname = CallData.i.IeSname;
                    model.IeCd = CallData.c.IeCd;
                    model.DtInspDesire = CallData.c.DtInspDesire;
                    model.RejCanCall = CallData.c.RejCanCall;
                    //model.Bpo = CallData.c.Bpo;
                    model.RecipientGstinNo = CallData.c.RecipientGstinNo;
                    model.IrfcFunded = CallData.c.IrfcFunded;
                }

                if (PODetails == null)
                    return model;
                else
                {
                    model.PoNo = PODetails.p.PoNo;
                    model.PoDt = PODetails.p.PoDt;
                    model.VendName = PODetails.v.VendName + "/" + PODetails.v.VendAdd1 + "/" + (PODetails.c.Location != PODetails.c.City + "/" + PODetails.c.City);
                    model.PoSource = PODetails.p.PoSource == "C" ? "CRIS" : (PODetails.p.PoSource == "V" ? "VENDOR" : "MANUAL");
                    model.StockNonstock = PODetails.p.StockNonstock == "S" ? "Stock" : "Non-Stock";
                }

                if (GetBPO == null)
                    return model;
                else
                {
                    model.BpoType = GetBPO.b.BpoType;
                    model.Au = GetBPO.b.Au;
                }

                if (query1 == null)
                    return model;
                else
                {
                    model.Bpo = query1.b.BpoName;
                    model.BpoCd = query1.b.BpoCd;
                    model.BpoType = query1.b.BpoType;
                    model.BpoRly = query1.b.BpoRly;
                    model.BpoFee = query1.b.BpoFee;
                    model.BpoFeeType = query1.b.BpoFeeType;
                    model.BpoTaxType = query1.b.BpoTaxType == null ? "X" : query1.b.BpoTaxType;
                    //model.BpoName = query1.b.BpoCd + "-" + query1.b.BpoName + "/";
                    model.BpoName = query1.b.BpoCd + "-" + query1.b.BpoName + "/" + (query1.b.BpoAdd != null ? (query1.b.BpoAdd + "/") : "") +
                               (query1.c.Location != null ? (query1.c.City + "/" + query1.c.Location) : query1.c.City) + "/" + query1.b.BpoRly;

                }
                model.CertNo = GetRegionCode + "/" + model.BpoRly + "/" + model.Caseno + "/" + model.Iesname;
                model.CertDt = DateTime.Now.Date;
                model.ICSubmitDt = DateTime.Now.Date;

                var GetIC = (from ic in context.IcIntermediates
                             where ic.CaseNo == CaseNo && ic.CallRecvDt == CallRecvDt && ic.CallSno == CallSno && ic.ConsigneeCd == Convert.ToInt32(GetConsigneeCd)
                             select new
                             {
                                 ic
                             }).FirstOrDefault();
                if (GetIC != null)
                {
                    model.Consignee = Convert.ToString(GetIC.ic.ConsigneeCd);
                    model.Bkno = GetIC.ic.BkNo;
                    model.Setno = GetIC.ic.SetNo;
                    model.LabTstRectDt = Convert.ToDateTime(GetIC.ic.LabTstRectDt);
                    model.NoOfInsp = Convert.ToInt32(GetIC.ic.NumVisits);
                    model.CertDt = GetIC.ic.Datetime;
                }

                var WorkPlans = (from w in context.T47IeWorkPlans
                                 where w.CaseNo == CaseNo && w.CallRecvDt == CallRecvDt && w.CallSno == CallSno
                                 group w by 1 into g
                                 select new
                                 {
                                     FIRST_INSP_DT = g.Min(w => w.VisitDt).ToString("dd/MM/yyyy"),
                                     LAST_INSP_DT = g.Max(w => w.VisitDt).ToString("dd/MM/yyyy")
                                 }).FirstOrDefault();
                if (WorkPlans != null)
                {
                    if (WorkPlans.FIRST_INSP_DT != null)
                    {
                        model.FirstInspDt = Convert.ToDateTime(WorkPlans.FIRST_INSP_DT);
                        model.LastInspDt = Convert.ToDateTime(WorkPlans.LAST_INSP_DT);
                        if (GetIC.ic.LabTstRectDt != null && model.LastInspDt != null)
                        {
                            string myYear, myMonth, myDay;
                            myYear = Convert.ToString(GetIC.ic.LabTstRectDt).Substring(6, 4);
                            myMonth = Convert.ToString(GetIC.ic.LabTstRectDt).Substring(3, 2);
                            myDay = Convert.ToString(GetIC.ic.LabTstRectDt).Substring(0, 2);
                            string dt = myYear + myMonth + myDay;

                            string myYear1, myMonth1, myDay1;
                            myYear1 = Convert.ToString(model.LastInspDt).Substring(6, 4);
                            myMonth1 = Convert.ToString(model.LastInspDt).Substring(3, 2);
                            myDay1 = Convert.ToString(model.LastInspDt).Substring(0, 2);
                            string dt1 = myYear1 + myMonth1 + myDay1;
                            int i = dt.CompareTo(dt1);
                            if (i > 0)
                            {
                                model.LastInspDt = Convert.ToDateTime(GetIC.ic.LabTstRectDt);
                            }
                        }
                    }
                }

                var icReceived = (from i in context.T30IcReceiveds
                                  where i.BkNo.Trim() == model.Bkno && i.SetNo.Trim() == model.Setno && i.Region == GetRegionCode
                                  select new
                                  {
                                      SUBMIT_DT = i.IcSubmitDt != null ? i.IcSubmitDt.Value.ToString("dd/MM/yyyy") : DateTime.Now.ToString("dd/MM/yyyy")
                                  }).FirstOrDefault();
                if (icReceived != null)
                {
                    model.ICSubmitDt = Convert.ToDateTime(icReceived.SUBMIT_DT);
                }

                var MValue = context.T14aPoNonrlies.Where(x => x.CaseNo == CaseNo).FirstOrDefault();
                if (MValue != null)
                {
                    model.MinFee = Convert.ToInt32(MValue.MinFee);
                    model.MaxFee = Convert.ToInt32(MValue.MaxFee);
                }
            }
            else
            {
                if (ActionType == "M")
                {
                    show(model, CaseNo, CallRecvDt, CallSno, Bkno, Setno, ActionType, GetRegionCode, RoleId);
                }
                else if (ActionType == "D")
                {
                    show(model, CaseNo, CallRecvDt, CallSno, Bkno, Setno, ActionType, GetRegionCode, RoleId);
                }

            }
            return model;
        }

        void get_legalname(InspectionCertModel model, int code, string type)
        {
            if (type == "C")
            {
                var GetLegalD = (from consignee in context.T06Consignees
                                 join city in context.T03Cities on consignee.ConsigneeCity equals city.CityCd
                                 join state in context.T92States on city.StateCd equals state.StateCd into stateGroup
                                 from stateItem in stateGroup.DefaultIfEmpty()
                                 where consignee.ConsigneeCd == code
                                 select new
                                 {
                                     LegalName = consignee.LegalName,
                                     PinCode = consignee.PinCode,
                                     City = city.City,
                                     State = stateItem != null ? Convert.ToString(stateItem.StateCd).PadLeft(2, '0') + "-" + stateItem.StateName : null
                                 }).FirstOrDefault();

                model.LegalName = GetLegalD.LegalName;
                model.City = GetLegalD.City;
                model.State = GetLegalD.State;
                model.Pincode = GetLegalD.PinCode;
            }
            else if (type == "B")
            {
                var GetLegalD = (from bpo in context.T12BillPayingOfficers
                                 join city in context.T03Cities on bpo.BpoCityCd equals city.CityCd
                                 join state in context.T92States on city.StateCd equals state.StateCd into stateGroup
                                 from stateItem in stateGroup.DefaultIfEmpty()
                                 where bpo.BpoCd == Convert.ToString(code)
                                 select new
                                 {
                                     LegalName = bpo.LegalName,
                                     PinCode = bpo.PinCode,
                                     City = city.City,
                                     State = stateItem != null ? Convert.ToString(stateItem.StateCd).PadLeft(2, '0') + "-" + stateItem.StateName : null
                                 }).FirstOrDefault();
                if (GetLegalD != null)
                {
                    model.LegalName = GetLegalD.LegalName;
                    model.City = GetLegalD.City;
                    model.State = GetLegalD.State;
                    model.Pincode = GetLegalD.PinCode;
                }
            }
        }

        void getconsignee_gstno(InspectionCertModel model)
        {
            if (model.SelectRadio == "rdbConsignee")
            {
                var query = (from t06 in context.T06Consignees
                             join t03 in context.T03Cities on t06.ConsigneeCity equals t03.CityCd
                             join t92 in context.T92States on t03.StateCd equals t92.StateCd
                             where t06.ConsigneeCd == Convert.ToInt32(model.Consignee)
                             select new
                             {
                                 GstinNo = t06.GstinNo ?? "X",
                                 t06.LegalName,
                                 t03.PinCode,
                                 t03.City,
                                 State = Convert.ToString(t92.StateCd).PadLeft(2, '0') + "-" + t92.StateName
                             }).FirstOrDefault();
                if (query != null)
                {
                    if (model.GstinNo != "X")
                    {
                        if (model.LegalName != "")
                        {
                            model.LegalName = query.LegalName;
                        }
                        else
                        {
                            model.LegalName = "";
                        }
                        model.GstinNo = query.GstinNo;
                        model.City = query.City;
                        model.State = query.State;
                        model.Pincode = query.PinCode;

                    }
                }

            }
            if (model.SelectRadio == "rdbBPO")
            {
                var query = (from t12 in context.T12BillPayingOfficers
                             join t03 in context.T03Cities on t12.BpoCityCd equals t03.CityCd
                             join t92 in context.T92States on t03.StateCd equals t92.StateCd
                             where t12.BpoCd == model.BpoCd
                             select new
                             {
                                 GstinNo = t12.GstinNo ?? "X",
                                 t12.LegalName,
                                 t12.PinCode,
                                 t03.City,
                                 State = Convert.ToString(t92.StateCd).PadLeft(2, '0') + "-" + t92.StateName
                             }).FirstOrDefault();
                if (query != null)
                {
                    if (model.GstinNo != "X")
                    {
                        if (model.LegalName != "")
                        {
                            model.LegalName = query.LegalName;
                        }
                        else
                        {
                            model.LegalName = "";
                        }
                        model.GstinNo = query.GstinNo;
                        model.City = query.City;
                        model.State = query.State;
                        model.Pincode = query.PinCode;
                    }
                }
            }
        }

        void get_irfc_bpo_gst_detail(InspectionCertModel model)
        {
            var query = (from t12 in context.T12BillPayingOfficers
                         join t03 in context.T03Cities on t12.BpoCityCd equals t03.CityCd
                         join t92 in context.T92States on t03.StateCd equals t92.StateCd into stateJoin
                         from t92 in stateJoin.DefaultIfEmpty() // Perform a left outer join
                         where t12.BpoCd == model.BpoCd
                         select new
                         {
                             GstinNo = t12.GstinNo ?? "X",
                             t12.LegalName,
                             t12.PinCode,
                             t03.City,
                             State = (t92 != null) ? (Convert.ToString(t92.StateCd).PadLeft(2, '0') + "-" + t92.StateName) : null
                         }).FirstOrDefault();
            if (query != null)
            {
                if (query.GstinNo != "X")
                {
                    if (query.LegalName != "" || query.LegalName != null)
                    {
                        model.LegalName = query.LegalName;
                    }
                    else
                    {
                        model.LegalName = "";
                    }
                    model.GstinNo = query.GstinNo;
                    model.City = query.City;
                    model.State = query.State;
                    model.Pincode = query.PinCode;
                }
                else
                {
                    model.GstinNo = "";
                }
            }
        }

        void showmaxamt(InspectionCertModel model)
        {
            var MaxVal = context.T14aPoNonrlies.Where(x => x.CaseNo == model.Caseno).FirstOrDefault();
            if (MaxVal != null)
            {
                model.MaxFee = Convert.ToInt16(MaxVal.MaxFee);
                model.MinFee = Convert.ToInt16(MaxVal.MinFee);
            }
        }

        void show3(InspectionCertModel model)
        {
            var query = (from p in context.T13PoMasters
                         join v in context.T05Vendors on p.VendCd equals v.VendCd
                         join c in context.T03Cities on v.VendCityCd equals c.CityCd
                         where p.CaseNo == model.Caseno
                         select new
                         {
                             p.PoNo,
                             PoDt = p.PoDt,
                             VendName = v.VendName.Trim() + "/" + v.VendAdd1.Trim() + "/" + (c.Location != null ? c.Location.Trim() + "/" + c.City.Trim() : c.City.Trim()),
                             PoSource = (p.PoSource == "C") ? "CRIS" : (p.PoSource == "V") ? "VENDOR" : "MANUAL",
                             p.RlyNonrly,
                             StockNonstock = (p.StockNonstock == "S") ? "Stock" : "Non-Stock"
                         }).FirstOrDefault();
            if (query != null)
            {
                model.PoNo = query.PoNo;
                model.PoDt = query.PoDt;
                model.VendName = query.VendName;
                model.StockNonstock = query.StockNonstock;
                model.PoSource = query.PoSource;
            }
        }

        void show(InspectionCertModel model, string CaseNo, DateTime? CallRecvDt, int CallSno, string Bkno, string Setno, string ActionType, string GetRegionCode, int RoleId)
        {
            if (Bkno != null && Setno != null && CaseNo != null)
            {
                var GetDetails = (from C in context.T20Ics
                                  join I in context.T09Ies on C.IeCd equals I.IeCd
                                  where C.BkNo == Bkno &&
                                        C.SetNo == Setno &&
                                        C.CaseNo.Substring(0, 1) == GetRegionCode
                                  select new
                                  {
                                      C,
                                      I
                                  }).FirstOrDefault();
                if (GetDetails != null)
                {
                    model.Caseno = GetDetails.C.CaseNo;
                    model.Callrecvdt = GetDetails.C.CallRecvDt;
                    model.Callsno = GetDetails.C.CallSno;
                    model.IcTypeId = Convert.ToInt32(GetDetails.C.IcTypeId);
                    model.Consignee = Convert.ToString(GetDetails.C.ConsigneeCd);
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

                    string myYear1 = "", myMonth1 = "", myDay1 = "";
                    if (model.CertDt != null)
                    {
                        myYear1 = Convert.ToString(model.CertDt).Substring(6, 4);
                        myMonth1 = Convert.ToString(model.CertDt).Substring(3, 2);
                        myDay1 = Convert.ToString(model.CertDt).Substring(0, 2);
                    }
                    string certdt = myYear1 + myMonth1 + myDay1;
                    if (certdt != "")
                    {
                        if (Convert.ToInt32(certdt) >= 20170701)
                        {
                            if (GetDetails.C.RecipientGstinNo != "")
                            {
                                if (GetRegionCode == "N" && GetDetails.C.RecipientGstinNo.Substring(0, 2) == "07")
                                {
                                    model.BpoTaxType = "C";
                                }
                                else if (GetRegionCode == "S" && GetDetails.C.RecipientGstinNo.Substring(0, 2) == "33")
                                {
                                    model.BpoTaxType = "C";
                                }
                                else if (GetRegionCode == "E" && GetDetails.C.RecipientGstinNo.Substring(0, 2) == "19")
                                {
                                    model.BpoTaxType = "C";
                                }
                                else if (GetRegionCode == "W" && GetDetails.C.RecipientGstinNo.Substring(0, 2) == "27")
                                {
                                    model.BpoTaxType = "C";
                                }
                                else if (GetRegionCode == "C" && GetDetails.C.RecipientGstinNo.Substring(0, 2) == "22")
                                {
                                    model.BpoTaxType = "C";
                                }
                                else if (GetRegionCode == "Q" && GetDetails.C.RecipientGstinNo.Substring(0, 2) == "06")
                                {
                                    model.BpoTaxType = "C";
                                }
                                else
                                {
                                    model.BpoTaxType = "I";
                                }
                            }
                        }
                        else
                        {

                        }
                        //Bhavesh Pending work
                    }
                    if (GetDetails.C.IrfcFunded == null || GetDetails.C.IrfcFunded == "")
                    {
                        model.IrfcBpo = "N";
                    }
                    if (GetDetails.C.AccGroup == "Z006")
                    {
                        get_legalname(model, Convert.ToInt32(GetDetails.C.BpoCd), "B");
                        getconsignee_gstno(model);
                    }
                    else if (GetDetails.C.AccGroup == "Z007")
                    {
                        get_legalname(model, Convert.ToInt32(GetDetails.C.ConsigneeCd), "C");
                        getconsignee_gstno(model);
                    }
                    else
                    {
                        get_legalname(model, Convert.ToInt32(GetDetails.C.BpoCd), "B");
                        get_irfc_bpo_gst_detail(model);
                    }

                    if (GetDetails.C.BillNo != null || GetDetails.C.BillNo != "")
                    {
                        model.BillNo = GetDetails.C.BillNo;
                        var query1 = (from p in context.T20Ics
                                      join b in context.T12BillPayingOfficers on p.BpoCd equals b.BpoCd
                                      join c in context.T03Cities on b.BpoCityCd equals c.CityCd
                                      where p.CaseNo == CaseNo && p.CallRecvDt == CallRecvDt && p.CallSno == CallSno
                                            && p.ConsigneeCd == Convert.ToInt32(model.Consignee)
                                      select new
                                      {
                                          BpoCd = p.BpoCd,
                                          //BpoName = $"{b.BpoCd}-{b.BpoName}/{(b.BpoAdd ?? "/")}{(c.Location != null ? $"{c.City}/{c.Location}" : c.City)}/{b.BpoRly}",
                                          BpoName = b.BpoCd + "-" + b.BpoName + "/" + (b.BpoAdd ?? "/") + (c.Location != null ? c.City + "/" + c.Location : c.City) + "/" + b.BpoRly,
                                          b.BpoRly,
                                          b.Au
                                      }).FirstOrDefault();
                        if (query1 != null)
                        {
                            model.Bpo = query1.BpoName;
                            model.BpoCd = query1.BpoCd;
                            model.BpoRly = query1.BpoRly;
                            model.Au = query1.Au;
                        }
                        var query3 = (from b in context.T22Bills
                                      where b.BillNo == model.BillNo
                                      select new
                                      {
                                          BillDt = b.BillDt,
                                          FeeRate = b.FeeRate ?? 0,
                                          b.FeeType,
                                          TaxType = b.TaxType ?? "X",
                                          MaterialValue = b.MaterialValue ?? 0,
                                          b.InspFee,
                                          b.MinFee,
                                          b.MaxFee,
                                          b.BillAmount,
                                          b.Remarks,
                                          AdvBill = b.AdvBill ?? "X",
                                          b.InvoiceNo,
                                          b.CnoteBillNo,
                                          b.SentToSap,
                                          b.BillFinalised
                                      }).FirstOrDefault();
                        if (query3 != null)
                        {
                            model.BpoFee = query3.FeeRate;
                            model.BpoFeeType = query3.FeeType;
                            if (Convert.ToInt32(certdt) >= 20170701)
                            {
                                model.TaxType = query3.TaxType;
                            }
                            else
                            {
                                model.BpoTaxType = query3.TaxType;
                            }
                            model.TMValue = query3.MaterialValue;
                            model.TIFee = query3.InspFee;
                            model.NetFee = query3.BillAmount;
                            model.MaxFee = Convert.ToInt16(query3.MaxFee);
                            model.MinFee = Convert.ToInt16(query3.MinFee);
                            model.BillDt = query3.BillDt;
                            model.Remarks = query3.Remarks;
                            model.InvoiceNo = query3.InvoiceNo;
                            if (query3.AdvBill == "A")
                            {

                            }
                            else
                            {

                            }
                            if (model.IcTypeId == 9)
                            {
                                model.BillNo = query3.CnoteBillNo;
                            }
                            if (query3.SentToSap == "X" || query3.BillFinalised == "Y")
                            {
                                model.STS = "X";
                            }

                        }
                    }
                    else
                    {
                        var query1 = (from p in context.T14PoBpos
                                      join b in context.T12BillPayingOfficers on p.BpoCd equals b.BpoCd
                                      join c in context.T03Cities on b.BpoCityCd equals c.CityCd
                                      where p.CaseNo == CaseNo &&
                                            p.ConsigneeCd == Convert.ToInt32(model.Consignee)
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
                        if (query1 != null)
                        {
                            model.Bpo = query1.BpoName;
                            model.BpoCd = query1.BpoCd;
                            model.BpoRly = query1.BpoRly;
                            model.BpoType = query1.BpoType;
                            if (query1.BpoType == "R" && query1.BpoFeeType == "P" && model.IcTypeId == 1)
                            {

                            }
                            else
                            {
                                model.BpoFee = query1.BpoFee;
                            }
                            model.BpoFeeType = query1.BpoFeeType;
                            model.TaxType = query1.BpoTaxType;
                            model.Au = query1.Au;
                        }

                        showmaxamt(model);
                    }

                    var query2 = (from c in context.T17CallRegisters
                                  where c.CaseNo == CaseNo && c.CallRecvDt == CallRecvDt && c.CallSno == CallSno
                                  select new
                                  {
                                      BPO = c.Bpo.ToUpper(),
                                      c.RecipientGstinNo
                                  }).FirstOrDefault();
                    if (query2 != null)
                    {
                        model.BPOCall = query2.BPO;
                        model.GSTINCall = query2.RecipientGstinNo;
                    }

                    show3(model);
                }

            }
            else if (ActionType == "D")
            {

            }
        }

        public string GetConsigneeID(string CaseNo, DateTime? CallRecvDt, int CallSno)
        {
            string Cd = "";
            var ConsigneeCd = from cd in context.T18CallDetails
                              where cd.CaseNo == CaseNo &&
                                    cd.CallRecvDt == CallRecvDt &&
                                    cd.CallSno == Convert.ToInt32(CallSno)
                              select cd.ConsigneeCd;

            var GetConsigneeCd = (from c in context.T06Consignees
                                  join d in context.T03Cities on c.ConsigneeCity equals d.CityCd
                                  where ConsigneeCd.Contains(c.ConsigneeCd)
                                  select new
                                  {
                                      ConsigneeCd = c.ConsigneeCd,
                                  }).FirstOrDefault();
            Cd = Convert.ToString(GetConsigneeCd.ConsigneeCd);
            return Cd;
        }

        public DTResult<InspectionCertModel> GetBillDetails(string BillNo)
        {
            DTResult<InspectionCertModel> dTResult = new() { draw = 0 };
            IQueryable<InspectionCertModel>? query = null;

            query = from p in context.T22Bills
                    where p.BillNo == BillNo
                    select new InspectionCertModel
                    {
                        Caseno = Convert.ToString(p.CaseNo)
                    };

            dTResult.data = query;
            return dTResult;
        }

        public DTResult<InspectionCertModel> GetConsignee(int ConsigneeCd)
        {
            DTResult<InspectionCertModel> dTResult = new() { draw = 0 };
            IQueryable<InspectionCertModel>? query = null;

            query = from t06 in context.T06Consignees
                    join t03 in context.T03Cities on t06.ConsigneeCity equals t03.CityCd
                    join t92 in context.T92States on t03.StateCd equals t92.StateCd
                    where t06.ConsigneeCd == ConsigneeCd

                    select new InspectionCertModel
                    {
                        GstinNo = t06.GstinNo ?? "X",
                        LegalName = t06.LegalName,
                        Pincode = t06.PinCode,
                        City = t03.City,
                        State = t92.StateCd + "-" + t92.StateName
                    };

            dTResult.data = query;
            return dTResult;
        }

        public DTResult<InspectionCertModel> GetBPO(string BPOCd)
        {
            DTResult<InspectionCertModel> dTResult = new() { draw = 0 };
            IQueryable<InspectionCertModel>? query = null;

            query = from t12 in context.T12BillPayingOfficers
                    join t03 in context.T03Cities on t12.BpoCityCd equals t03.CityCd
                    join t92 in context.T92States on t03.StateCd equals t92.StateCd
                    where t12.BpoCd == BPOCd
                    select new InspectionCertModel
                    {
                        GstinNo = t12.GstinNo ?? "X",
                        LegalName = t12.LegalName,
                        Pincode = t12.PinCode,
                        City = t03.City,
                        State = t92.StateCd + "-" + t92.StateName
                    };

            dTResult.data = query;
            return dTResult;
        }

        public int UpdateGSTDetails(InspectionCertModel model, string UserName)
        {
            int Id = 0;
            if (model.SelectRadio == "rdbConsignee")
            {
                var GetUpdate = context.T06Consignees.Where(x => x.ConsigneeCd == Convert.ToInt32(model.Consignee)).FirstOrDefault();

                if (GetUpdate != null)
                {
                    GetUpdate.GstinNo = model.GstinNo;
                    GetUpdate.LegalName = model.LegalName;

                    GetUpdate.Updatedby = model.Updatedby;
                    GetUpdate.Updateddate = DateTime.Now.Date;

                    context.SaveChanges();
                    Id = Convert.ToInt32(model.Consignee);
                }
            }
            if (model.SelectRadio == "rdbBPO")
            {
                var GetUpdate = context.T12BillPayingOfficers.Where(x => x.BpoCd == model.BpoCd).FirstOrDefault();

                if (GetUpdate != null)
                {
                    GetUpdate.GstinNo = model.GstinNo;
                    GetUpdate.LegalName = model.LegalName;

                    GetUpdate.Updatedby = model.Updatedby;
                    GetUpdate.Updateddate = DateTime.Now.Date;

                    context.SaveChanges();
                    Id = Convert.ToInt32(model.Consignee);
                }
            }

            return Id;
        }

        public string InspectionCertSave(InspectionCertModel model, string GetRegionCode)
        {
            string msg = "";
            var bscheck = (context.T10IcBooksets.Where(x => x.BkNo.Trim().ToUpper() == model.Bkno
                        && Convert.ToInt32(model.Setno) >= Convert.ToInt32(x.SetNoFr) && Convert.ToInt32(model.Setno) <= Convert.ToInt32(x.SetNoTo)
                        && x.IssueToIecd == model.IeCd).Select(x => x.IssueToIecd)).FirstOrDefault();

            var bscheck1 = (context.T16IcCancels.Where(x => x.BkNo.Trim().ToUpper() == model.Bkno
                        && x.SetNo.Trim() == model.Setno && x.Region == GetRegionCode).Select(x => x.IssueToIecd)).FirstOrDefault();
            string bscheck2 = "";
            if (model.GstinNo.Substring(0, 2) != model.State.Substring(0, 2))
            {
                bscheck2 = "N";
            }
            int bscheck3 = 0;
            if ((model.SelectRadio != "rdbConsignee" && model.SelectRadio != "rdbBPO" && model.IrfcFunded == "N") || (model.SelectRadio != "rdbConsignee" && model.SelectRadio != "rdbBPO" && model.IrfcFunded == "Y" && model.IrfcBpo == ""))
            {
                bscheck3 = 1;
            }
            if (bscheck != null && bscheck1 == null && bscheck2 == "" && bscheck3 == 0)
            {
                string acc_group = "";
                if (model.SelectRadio == "rdbConsignee")
                {
                    acc_group = "Z007";
                }
                else if (model.SelectRadio == "rdbBPO")
                {
                    acc_group = "Z006";
                }

                var check1 = context.T20Ics.Where(x => x.BkNo == model.Bkno && x.SetNo == model.Setno && x.CaseNo.Substring(0, 1) == GetRegionCode).Select(x => x.BkNo).FirstOrDefault();
                string Cstatus = "";
                string upQuery = "";
                string w_irfc_funded = "";
                string w_irfc_bpo = "";
                if (model.ActionType == "A")
                {
                    if (check1 == "")
                    {
                        if (model.IrfcFunded == "Y")
                        {
                            w_irfc_funded = model.IrfcFunded;
                            w_irfc_bpo = model.IrfcBpo;
                            acc_group = "";
                        }
                        else
                        {
                            w_irfc_funded = "N";
                        }
                        var Co = context.T17CallRegisters.Where(x => x.CaseNo == model.Caseno && x.CallRecvDt == model.Callrecvdt && x.CallSno == Convert.ToInt32(model.Callsno) && x.IeCd == model.IeCd).Select(x => x.CoCd).FirstOrDefault();

                        T20Ic obj = new T20Ic();
                        obj.CaseNo = model.Caseno;
                        obj.CallRecvDt = Convert.ToDateTime(model.Callrecvdt);
                        obj.CallSno = Convert.ToInt16(model.Callsno);
                        obj.IcTypeId = model.IcTypeId;
                        obj.ConsigneeCd = Convert.ToInt32(model.Consignee);
                        obj.BpoCd = model.BpoCd;
                        obj.BkNo = model.Bkno;
                        obj.SetNo = model.Setno;
                        obj.IeCd = model.IeCd;
                        obj.IcNo = model.CertNo;
                        obj.IcDt = model.CertDt;
                        obj.CallDt = model.DtInspDesire;
                        obj.CallInstallNo = model.CallInstallNo;
                        obj.FullPart = model.FullPart;
                        obj.ReasonReject = model.ReasonReject;
                        obj.NoOfInsp = model.NoOfInsp;
                        obj.FirstInspDt = model.FirstInspDt;
                        obj.LastInspDt = model.LastInspDt;
                        obj.OtherInspDt = Convert.ToString(model.OtherInspDt);
                        obj.IcSubmitDt = model.ICSubmitDt;
                        obj.UserId = model.UserId;
                        obj.Datetime = DateTime.Now.Date;
                        obj.Photo = model.Photo;
                        obj.CoCd = Co;
                        obj.StampPatternCd = model.StampPatternCd;
                        obj.StampPattern = model.StampPattern;
                        obj.RecipientGstinNo = model.GstinNo;
                        obj.AccGroup = acc_group;
                        obj.IrfcBpoCd = w_irfc_bpo;
                        obj.IrfcFunded = w_irfc_funded;

                        context.T20Ics.Add(obj);
                        context.SaveChanges();

                        Cstatus = setCallStatus(model);

                        var GetReg = context.T17CallRegisters.Where(x => x.CaseNo == model.Caseno && x.CallRecvDt == model.Callrecvdt && x.CallSno == Convert.ToInt32(model.Callsno)).FirstOrDefault();
                        if (GetReg != null)
                        {
                            GetReg.CallStatus = Cstatus;

                            GetReg.Updatedby = model.UserId;
                            GetReg.Updateddate = DateTime.Now.Date;
                            context.SaveChanges();
                        }
                    }
                }
                else if (model.ActionType == "M")
                {
                    if (model.IrfcFunded == "Y")
                    {
                        w_irfc_funded = model.IrfcFunded;
                        w_irfc_bpo = model.IrfcBpo;
                        acc_group = "";
                    }
                    else
                    {
                        w_irfc_funded = "N";
                    }
                    var GetCall = context.T20Ics.Where(x => x.CaseNo == model.Caseno && x.CallRecvDt == model.Callrecvdt && x.CallSno == model.Callsno && x.ConsigneeCd == Convert.ToInt32(model.Consignee)).FirstOrDefault();
                    #region Details update
                    if (GetCall != null)
                    {
                        GetCall.IcTypeId = model.IcTypeId;
                        GetCall.BkNo = model.Bkno;
                        GetCall.SetNo = model.Setno;
                        GetCall.IcNo = model.Icno;
                        GetCall.IcDt = model.CertDt;
                        GetCall.CallDt = model.CallDt;
                        GetCall.CallInstallNo = model.CallInstallNo;
                        GetCall.FullPart = model.FullPart;
                        GetCall.StampPattern = model.StampPattern;
                        GetCall.ReasonReject = model.ReasonReject;
                        GetCall.NoOfInsp = model.NoOfInsp;
                        GetCall.FirstInspDt = model.FirstInspDt;
                        GetCall.LastInspDt = model.LastInspDt;

                        GetCall.OtherInspDt = Convert.ToString(model.OtherInspDt);
                        GetCall.IcSubmitDt = model.ICSubmitDt;
                        GetCall.UserId = model.UserId;
                        GetCall.Datetime = DateTime.Now.Date;
                        GetCall.Photo = model.Photo;
                        GetCall.StampPatternCd = model.StampPatternCd;
                        GetCall.RecipientGstinNo = model.RecipientGstinNo;
                        GetCall.AccGroup = model.AccGroup;
                        GetCall.IrfcFunded = model.IrfcFunded;
                        GetCall.IrfcBpoCd = model.IrfcBpo;

                        context.SaveChanges();
                        msg = model.Caseno;
                    }
                    #endregion
                    Cstatus = setCallStatus(model);
                    var T17 = context.T17CallRegisters.Where(x => x.CaseNo == model.Caseno && x.CallRecvDt == model.Callrecvdt && x.CallSno == model.Callsno).FirstOrDefault();
                    #region Details update
                    if (T17 != null)
                    {
                        T17.CallStatus = Cstatus;
                        T17.UpdateAllowed = "N";
                        T17.Datetime = DateTime.Now.Date;
                        T17.UserId = model.UserId;
                        T17.Updatedby = model.UserId;
                        T17.Updateddate = DateTime.Now.Date;

                        context.SaveChanges();
                        msg = model.Caseno;
                    }
                    #endregion
                }
            }

            string myYear1, myMonth1, myDay1;

            myYear1 = Convert.ToString(model.CertDt).Substring(6, 4);
            myMonth1 = Convert.ToString(model.CertDt).Substring(3, 2);
            myDay1 = Convert.ToString(model.CertDt).Substring(0, 2);
            string certdt = myYear1 + myMonth1 + myDay1;
            if (Convert.ToInt32(certdt) >= 20170701)
            {
                if (GetRegionCode == "N" && model.GstinNo.Substring(0, 2) == "07")
                {
                    model.TaxType = "C";
                }
                else if (GetRegionCode == "S" && model.GstinNo.Substring(0, 2) == "33")
                {
                    model.TaxType = "C";
                }
                else if (GetRegionCode == "E" && model.GstinNo.Substring(0, 2) == "19")
                {
                    model.TaxType = "C";
                }
                else if (GetRegionCode == "W" && model.GstinNo.Substring(0, 2) == "27")
                {
                    model.TaxType = "C";
                }
                else if (GetRegionCode == "C" && model.GstinNo.Substring(0, 2) == "22")
                {
                    model.TaxType = "C";
                }
                else if (GetRegionCode == "Q" && model.GstinNo.Substring(0, 2) == "06")
                {
                    model.TaxType = "C";
                }
                else
                {
                    model.TaxType = "I";
                }
            }
            return msg;
        }

        public string setCallStatus(InspectionCertModel model)
        {
            if (model.IcTypeId == 2)
            {
                return "R";
            }
            else if (model.IcTypeId == 6)
            {
                return "C";
            }
            else
            {
                return "B";
            }
        }

        public string ReturnBillSubmit(InspectionCertModel model, string GetRegionCode)
        {
            string msg = "N";

            var return_bill = context.RitesBillDtls.Where(x => x.BillNo == model.BillNo).FirstOrDefault();

            //var return_bill_resent_cris = context.RitesBillDtls.Where(x => x.BillNo == model.BillNo && x.ReturnDate != null && x.Co6Status == "R").Max(x => x.BillResentCount);

            var query = context.RitesBillDtls.Where(b => b.BillNo == model.BillNo && b.Co6Status == "R");

            int? return_bill_resent_cris = query.Any() ? query.Max(b => (int?)Convert.ToInt32(b.BillResentCount)) : 0;

            var return_bill_resent_ibs = context.V22Bills.Where(x => x.BillNo == model.BillNo).Select(x => (int?)Convert.ToInt32(x.BillResentCount) ?? 0).FirstOrDefault();

            if (return_bill != null && Convert.ToInt32(return_bill_resent_cris) == Convert.ToInt32(return_bill_resent_ibs))
            {
                var SetBill = context.T22Bills.Where(x => x.BillNo == model.BillNo).FirstOrDefault();
                if (SetBill != null)
                {
                    SetBill.BillResentStatus = "R";
                    SetBill.Remarks = model.Remarks;
                    SetBill.UserId = model.UserId;
                    SetBill.Datetime = DateTime.Now.Date;
                    context.SaveChanges();
                }
                msg = "U";
            }

            return msg;
        }

        public string Validation(InspectionCertModel model, string GetRegionCode)
        {
            string msg = "";
            checkManDay(model);
            var bscheck = (context.T10IcBooksets.Where(x => x.BkNo.Trim().ToUpper() == model.Bkno
                        && Convert.ToInt32(model.Setno) >= Convert.ToInt32(x.SetNoFr) && Convert.ToInt32(model.Setno) <= Convert.ToInt32(x.SetNoTo)
                        && x.IssueToIecd == model.IeCd).Select(x => x.IssueToIecd)).FirstOrDefault();

            var bscheck1 = (context.T16IcCancels.Where(x => x.BkNo.Trim().ToUpper() == model.Bkno
                        && x.SetNo.Trim() == model.Setno && x.Region == GetRegionCode).Select(x => x.IssueToIecd)).FirstOrDefault();

            string bscheck2 = "";
            if (model.GstinNo.Substring(0, 2) != model.State.Substring(0, 2))
            {
                bscheck2 = "N";
            }
            int bscheck3 = 0;
            if ((model.SelectRadio != "rdbConsignee" && model.SelectRadio != "rdbBPO" && model.IrfcFunded == "N") || (model.SelectRadio != "rdbConsignee" && model.SelectRadio != "rdbBPO" && model.IrfcFunded == "Y" && model.IrfcBpo == ""))
            {
                bscheck3 = 1;
            }

            if (bscheck == null)
            {
                msg = "Book No. and Set No. specified is not issued to the Selected Inspection Engineer!!!";
                return msg;
            }
            else if (bscheck1 != null)
            {
                msg = "Book No. and Set No. specified is being Cancelled or Missed!!!";
                return msg;
            }
            else if (bscheck2 != "")
            {
                msg = "Recipient GST No. State Code Does not match with the State Code of the Recipient!!!";
                return msg;
            }
            else if (bscheck3 == 1)
            {
                msg = "Select the one of the Options for GST Recipient, i.e. Consignee, BPO or IRFC BPO!!!";
                return msg;
            }
            return msg;
        }

        string checkManDay(InspectionCertModel model)
        {
            string msg = "";
            var PurchaserCd = context.T13PoMasters.Where(x => x.CaseNo == model.Caseno).FirstOrDefault();
            var query = from m in context.T13PoMasters
                        join i in context.T20Ics on m.CaseNo equals i.CaseNo
                        where i.IeCd == model.IeCd && m.CaseNo == model.Caseno && m.RlyNonrly != "R"
                                && (i.FirstInspDt <= model.FirstInspDt && (i.LastInspDt == null || i.LastInspDt >= model.LastInspDt))
                                && m.PurchaserCd == Convert.ToInt32(PurchaserCd.PurchaserCd)
                        group i by new { i.BillNo, i.BkNo, i.SetNo } into g
                        select new
                        {
                            g.Key.BillNo,
                            g.Key.BkNo,
                            g.Key.SetNo
                        };
            var results = query.ToList();
            if (results.Count > 1)
            {
                msg = "IC With the given inspection date, IE and Purchaser Already Exists: \n";
                int j = 1;
                foreach (var result in results)
                {
                    if (result.BkNo + result.SetNo != model.Bkno + model.Setno)
                    {
                        msg += j + ") Bill No: " + result.BillNo + " vide Book NO=" + result.BkNo + " & Set No.=" + result.SetNo + ". \n";
                        j++;
                    }
                }
                msg += " So Plz Check These Cases, Before Proceeding !!!";
            }
            return msg;
        }

        public DTResult<InspectionCertModel> GetLoadTableDetails(DTParameters dtParameters, string GetRegionCode)
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

        public int financial_year_check(InspectionCertModel model)
        {
            int Id = 0;
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

        public string BillUpdate(InspectionCertModel model, string GetRegionCode)
        {
            string str = "";
            if (model.BillNo == null || model.BillNo == "")
            {
                if (chk_bill_dt(Convert.ToString(model.BillDt), GetRegionCode) == 1)
                {
                    if (model.IcTypeId == 9)
                    {
                        gen_credit_note(model);
                    }
                    else
                    {
                        gen_bill(model);
                    }
                }
            }
            else if (model.BillNo != null)
            {
                if (model.IcTypeId == 9)
                {
                    gen_credit_note(model);
                }
                else
                {
                    gen_bill(model);
                }
            }
            str = model.BillNo;
            return str;
        }

        public string BillDateUpdate(InspectionCertModel model, string Region)
        {
            string str = "";
            int oldMonth = Convert.ToInt32(Convert.ToString(model.BillDt).Substring(3, 2));
            string frm = "", to = "";
            if (oldMonth >= 4)
            {
                frm = Convert.ToString(model.BillDt).Substring(6, 4) + "04";
                to = Convert.ToString(Convert.ToInt32(Convert.ToString(model.BillDt).Substring(6, 4)) + 1) + "03";
            }
            else
            {
                frm = Convert.ToString(Convert.ToInt32(Convert.ToString(model.BillDt).Substring(6, 4)) - 1) + "04";
                to = Convert.ToString(model.BillDt).Substring(6, 4) + "03";

            }
            string newdt = Convert.ToString(model.BillDt).Substring(6, 4) + Convert.ToString(model.BillDt).Substring(3, 2);
            if (chk_bill_dt(Convert.ToString(model.BillDt), Region) == 1)
            {
                if (Convert.ToInt32(newdt) >= Convert.ToInt32(frm) && Convert.ToInt32(newdt) <= Convert.ToInt32(to))
                {
                    var BillDtUpdate = context.T22Bills.Where(x => x.BillNo == model.BillNo).First();
                    if (BillDtUpdate != null)
                    {
                        BillDtUpdate.BillDt = model.BillDt;
                        BillDtUpdate.Updatedby = model.UserId;
                        BillDtUpdate.Updateddate = DateTime.Now.Date;

                        context.SaveChanges();
                        str = "1";
                    }
                }
                else
                {
                    str = "2";
                }
            }
            else
            {
                str = "3";
            }
            return str;
        }

        int chk_bill_dt(string BillDt, string GetRegionCode)
        {
            if (GetRegionCode != "Q")
            {
                var allowstatus = context.T97ControlFiles.Where(x => x.Region == GetRegionCode).Select(x => x.AllowOldBillDt).FirstOrDefault();
                var min_bill_dt = context.T87BillControls.Select(x => x.MinBillDt).FirstOrDefault();

                string myYear, myMonth, myDay;
                myYear = Convert.ToString(BillDt).Substring(6, 4);
                myMonth = Convert.ToString(BillDt).Substring(3, 2);
                myDay = Convert.ToString(BillDt).Substring(0, 2);
                string dt1 = myYear + myMonth + myDay;

                if (allowstatus == "N")
                {
                    //Bhavesh changes pending datetime.now.add - GraceDays
                    int? gDays = context.T97ControlFiles.Where(x => x.Region == GetRegionCode).Select(x => x.GraceDays).FirstOrDefault();
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
                    if (Convert.ToDateTime(BillDt).ToString("dd/MM/yyyy").CompareTo(Convert.ToDateTime(min_bill_dt).ToString("dd/MM/yyyy")) > 0)
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
                    context.SaveChanges();
                }
            }
            string c_note_bno = "";
            if (model.BillNo != null)
            {
                c_note_bno = model.BillNo;
            }
            if (c_note_bno != "")
            {
                string myYear1, myMonth1, myDay1;

                myYear1 = Convert.ToString(model.CertDt).Substring(6, 4);
                myMonth1 = Convert.ToString(model.CertDt).Substring(3, 2);
                myDay1 = Convert.ToString(model.CertDt).Substring(0, 2);
                string certdt = myYear1 + myMonth1 + myDay1;
            }

        }

        void gen_bill(InspectionCertModel model)
        {
            if (model.BpoCd != model.Bpo)
            {
                var T20 = context.T20Ics.Where(x => x.CaseNo == model.Caseno && x.CallRecvDt == model.Callrecvdt && x.CallSno == model.Callsno && x.ConsigneeCd == Convert.ToInt32(model.Consignee)).FirstOrDefault();
                if (T20 != null)
                {
                    T20.BpoCd = model.BpoCd;
                    T20.Updatedby = model.UserId;
                    T20.Updateddate = DateTime.Now.Date;
                    context.SaveChanges();
                }
            }
            string myYear1, myMonth1, myDay1;
            myYear1 = Convert.ToString(model.CertDt).Substring(6, 4);
            myMonth1 = Convert.ToString(model.CertDt).Substring(3, 2);
            myDay1 = Convert.ToString(model.CertDt).Substring(0, 2);
            string certdt = myYear1 + myMonth1 + myDay1;

            decimal in_fee;
            if (model.BpoType == "R" && model.IcTypeId == 1)
            {
                in_fee = model.RlyBpoFee;
            }
            else
            {
                in_fee = Convert.ToDecimal(model.BpoFee);
            }
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
            if (model.NoOfInsp == null)
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
                if (model.InvoiceNo == "" || model.InvoiceNo.Length < 13)
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

            DataSet ds = new DataSet();
            if (Convert.ToInt32(certdt) >= 20170701)
            {
                OracleParameter[] parameter = new OracleParameter[16];
                parameter[0] = new OracleParameter("in_region_cd", OracleDbType.Char, model.Regioncode, ParameterDirection.Input);
                parameter[1] = new OracleParameter("in_case_no", OracleDbType.Varchar2, model.Caseno, ParameterDirection.Input);
                parameter[2] = new OracleParameter("in_call_recv_dt", OracleDbType.Varchar2, Convert.ToDateTime(model.Callrecvdt).ToString("dd/MM/yyyy"), ParameterDirection.Input);
                parameter[3] = new OracleParameter("in_call_sno", OracleDbType.Int32, model.Callsno, ParameterDirection.Input);
                parameter[4] = new OracleParameter("in_consignee_cd", OracleDbType.Int32, model.Consignee, ParameterDirection.Input);
                parameter[5] = new OracleParameter("in_bill", OracleDbType.Varchar2, model.BillNo, ParameterDirection.Input);
                parameter[6] = new OracleParameter("in_fee_type", OracleDbType.Varchar2, model.BpoFeeType, ParameterDirection.Input);
                parameter[7] = new OracleParameter("in_fee", OracleDbType.Decimal, in_fee, ParameterDirection.Input);
                parameter[8] = new OracleParameter("in_tax_type", OracleDbType.Varchar2, TaxType, ParameterDirection.Input);
                parameter[9] = new OracleParameter("in_no_of_insp", OracleDbType.Int32, NoOfInsp, ParameterDirection.Input);
                parameter[10] = new OracleParameter("in_invoice", OracleDbType.Varchar2, InvoiceNo, ParameterDirection.Input);
                parameter[11] = new OracleParameter("in_max_fee", OracleDbType.Int32, MaxFee, ParameterDirection.Input);
                parameter[12] = new OracleParameter("in_min_fee", OracleDbType.Int32, MinFee, ParameterDirection.Input);
                parameter[13] = new OracleParameter("in_bill_dt", OracleDbType.Varchar2, Convert.ToDateTime(model.BillDt).ToString("dd/MM/yyyy"), ParameterDirection.Input);
                parameter[14] = new OracleParameter("in_adv_bill", OracleDbType.Varchar2, model.chkABill, ParameterDirection.Input);
                parameter[15] = new OracleParameter("p_result_cursor", OracleDbType.RefCursor, ParameterDirection.Output);
                //parameter[15] = new OracleParameter("out_err_cd", OracleDbType.Int32, 1, ParameterDirection.Output);
                //parameter[16] = new OracleParameter("out_bill", OracleDbType.Varchar2, 20, ParameterDirection.Output);
                //parameter[17] = new OracleParameter("out_fee", OracleDbType.Int32, 15, ParameterDirection.Output);

                ds = DataAccessDB.GetDataSet("SP_GENERATE_BILL_GST_NEW", parameter);
            }
            else
            {
                OracleParameter[] parameter = new OracleParameter[15];
                parameter[0] = new OracleParameter("in_region_cd", OracleDbType.Char, model.Regioncode, ParameterDirection.Input);
                parameter[1] = new OracleParameter("in_case_no", OracleDbType.Varchar2, model.Caseno, ParameterDirection.Input);
                parameter[2] = new OracleParameter("in_call_recv_dt", OracleDbType.Varchar2, Convert.ToDateTime(model.Callrecvdt).ToString("dd/MM/yyyy"), ParameterDirection.Input);
                parameter[3] = new OracleParameter("in_call_sno", OracleDbType.Int32, model.Callsno, ParameterDirection.Input);
                parameter[4] = new OracleParameter("in_consignee_cd", OracleDbType.Int32, model.Consignee, ParameterDirection.Input);
                parameter[5] = new OracleParameter("in_bill", OracleDbType.Varchar2, model.BillNo, ParameterDirection.Input);
                parameter[6] = new OracleParameter("in_fee_type", OracleDbType.Varchar2, model.BpoFeeType, ParameterDirection.Input);
                parameter[7] = new OracleParameter("in_fee", OracleDbType.Decimal, in_fee, ParameterDirection.Input);
                parameter[8] = new OracleParameter("in_tax_type", OracleDbType.Varchar2, TaxType, ParameterDirection.Input);
                parameter[9] = new OracleParameter("in_no_of_insp", OracleDbType.Int32, NoOfInsp, ParameterDirection.Input);
                parameter[10] = new OracleParameter("in_max_fee", OracleDbType.Int32, MaxFee, ParameterDirection.Input);
                parameter[11] = new OracleParameter("in_min_fee", OracleDbType.Int32, MinFee, ParameterDirection.Input);
                parameter[12] = new OracleParameter("in_bill_dt", OracleDbType.Varchar2, Convert.ToDateTime(model.BillDt).ToString("dd/MM/yyyy"), ParameterDirection.Input);
                parameter[13] = new OracleParameter("in_adv_bill", OracleDbType.Varchar2, model.chkABill, ParameterDirection.Input);
                parameter[14] = new OracleParameter("p_result_cursor", OracleDbType.RefCursor, ParameterDirection.Output);
                //parameter[15] = new OracleParameter("out_err_cd", OracleDbType.Int32, ParameterDirection.Output);
                //parameter[16] = new OracleParameter("out_bill", OracleDbType.Varchar2, ParameterDirection.Output);
                //parameter[17] = new OracleParameter("out_fee", OracleDbType.Int32, ParameterDirection.Output);


                ds = DataAccessDB.GetDataSet("SP_GENERATE_BILL_NEW", parameter);

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
                        model.TIFee = str3.InspFee;
                        model.NetFee = str3.BillAmount;
                        model.InvoiceNo = str3.InvoiceNo;
                    }
                    var str = context.T22Bills.Where(x => x.BillNo == Convert.ToString(ds.Tables[0].Rows[0]["OUT_BILL"])).FirstOrDefault();
                    if (str != null)
                    {
                        str.Remarks = model.Remarks;
                        str.UserId = model.UserId;
                        str.Datetime = DateTime.Now.Date;

                        context.SaveChanges();
                    }
                    var str_ic = context.T30IcReceiveds.Where(x => x.BkNo == model.Bkno && x.SetNo == model.Setno && x.IeCd == model.IeCd && x.Region == model.Regioncode).FirstOrDefault();
                    if (str_ic != null)
                    {
                        str_ic.BillNo = Convert.ToString(ds.Tables[0].Rows[0]["OUT_BILL"]);

                        context.SaveChanges();
                    }
                }
                else
                {
                    model.UpdateStatus = Convert.ToString(ds.Tables[0].Rows[0]["OUT_ERR_CD"]);
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

        public ICPopUpModel FindByBillDetails(string BillNo, string Region)
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

        public string UpdateCallDetails(InspectionCertModel model, int ItemSrnoPo, string CaseNo, DateTime CallRecvDt, int CallSno)
        {
            string ID = "";
            var CallDetails = context.T18CallDetails.Where(x => x.CaseNo == CaseNo && x.CallRecvDt == CallRecvDt && x.CallSno == CallSno && x.ItemSrnoPo == ItemSrnoPo).FirstOrDefault();
            if (CallDetails != null)
            {
                CallDetails.ItemDescPo = model.ItemDescPo;
                CallDetails.QtyToInsp = model.QtyToInsp;
                CallDetails.QtyPassed = model.QtyPassed;
                CallDetails.QtyRejected = model.QtyRejected;
                CallDetails.QtyDue = model.QtyDue;

                CallDetails.Updatedby = model.UserId;
                CallDetails.Updateddate = DateTime.Now.Date;

                context.SaveChanges();
                ID = Convert.ToString(CallDetails.ItemSrnoPo);
            }
            return ID;
        }

        public string DocUpdate(string BillNo, string UserId)
        {
            InspectionCertModel model = new();
            T22Bill Bill = context.T22Bills.Where(x => x.BillNo == BillNo).FirstOrDefault();

            if (Bill == null)
                throw new Exception("Bill Record Not found");
            else
            {
                Bill.InvoiceSuppDocs = "Y";
                Bill.Updatedby = UserId;
                Bill.Updateddate = DateTime.Now.Date;

                context.SaveChanges();

            }
            return BillNo;
        }
    }
}
