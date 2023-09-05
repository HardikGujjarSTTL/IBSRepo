using IBS.DataAccess;
using IBS.Interfaces.InspectionBilling;
using IBS.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Oracle.ManagedDataAccess.Client;
using System.Dynamic;
using System.Globalization;

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
                    model.LabTstRectDt = GetIC.ic.LabTstRectDt;
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
                                model.LastInspDt = GetIC.ic.LabTstRectDt;
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
                        if (GetDetails == null)
                        {
                            return model;
                        }
                        else
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
                            model.FirstInspDt = GetDetails.C.FirstInspDt;
                            model.LastInspDt = GetDetails.C.LastInspDt;
                            model.OtherInspDt = Convert.ToDateTime(GetDetails.C.OtherInspDt);
                            model.StampPattern = GetDetails.C.StampPattern;
                            model.ReasonReject = GetDetails.C.ReasonReject;
                            model.BillNo = GetDetails.C.BillNo;
                            model.ICSubmitDt = GetDetails.C.IcSubmitDt.HasValue ? GetDetails.C.IcSubmitDt.Value : null;
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
                            }
                        }

                    }
                    else if (ActionType == "D")
                    {

                    }

                }

            }
            return model;
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
            string msg = "";

            var return_bill = context.RitesBillDtls.Where(x => x.BillNo == model.BillNo).FirstOrDefault();

            var return_bill_resent_cris = context.RitesBillDtls.Where(x => x.BillNo == model.BillNo && x.ReturnDate != null && x.Co6Status == "R").Max(x => x.BillResentCount);

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
            }

            return msg;
        }
    }
}
