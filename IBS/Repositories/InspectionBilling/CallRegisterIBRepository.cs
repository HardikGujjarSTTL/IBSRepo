using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Interfaces.InspectionBilling;
using IBS.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
//using NuGet.Protocol.Plugins;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Globalization;
using System.Net;
using System.Net.Mail;

namespace IBS.Repositories.InspectionBilling
{
    public class CallRegisterIBRepository : ICallRegisterIBRepository
    {
        private readonly ModelContext context;
        private readonly IConfiguration config;
        private readonly ISendMailRepository pSendMailRepository;


        public CallRegisterIBRepository(ModelContext context, IConfiguration _config, ISendMailRepository _pSendMailRepository)
        {
            this.context = context;
            config = _config;
            pSendMailRepository = _pSendMailRepository;
        }

        public string CNO, DT, Action, CSNO, cstatus, wFOS;
        int callval = 0;
        int e_status = 0;


        public DTResult<VenderCallRegisterModel> GetDataList(DTParameters dtParameters, string RegionCode)
        {
            DTResult<VenderCallRegisterModel> dTResult = new() { draw = 0 };
            IQueryable<VenderCallRegisterModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "" || orderCriteria == null)
                {
                    orderCriteria = "CaseNo";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
            }
            else
            {
                orderCriteria = "CaseNo";
                orderAscendingDirection = true;
            }

            string CaseNo = "", CallRecvDt = "", PoNo = "", PoDt = "", Vendor = "", CallLetterNo = "", CallSno = "";

            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["CaseNo"]))
            {
                CaseNo = Convert.ToString(dtParameters.AdditionalValues["CaseNo"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["CallRecvDt"]))
            {
                CallRecvDt = Convert.ToString(dtParameters.AdditionalValues["CallRecvDt"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["PoNo"]))
            {
                PoNo = Convert.ToString(dtParameters.AdditionalValues["PoNo"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["PoDt"]))
            {
                PoDt = Convert.ToString(dtParameters.AdditionalValues["PoDt"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["Vendor"]))
            {
                Vendor = Convert.ToString(dtParameters.AdditionalValues["Vendor"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["CallLetterNo"]))
            {
                CallLetterNo = Convert.ToString(dtParameters.AdditionalValues["CallLetterNo"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["CallSno"]))
            {
                CallSno = Convert.ToString(dtParameters.AdditionalValues["CallSno"]);
            }

            CaseNo = CaseNo.ToString() == "" ? string.Empty : CaseNo.ToString();
            DateTime? _CallRecvDt = CallRecvDt == "" ? null : DateTime.ParseExact(CallRecvDt, "dd/MM/yyyy", null);
            PoNo = PoNo.ToString() == "" ? string.Empty : PoNo.ToString();
            DateTime? _PoDt = PoDt == "" ? null : DateTime.ParseExact(PoDt, "dd/MM/yyyy", null);
            Vendor = Vendor.ToString() == "" ? string.Empty : Vendor.ToString();
            CallLetterNo = CallLetterNo.ToString() == "" ? string.Empty : CallLetterNo.ToString();
            CallSno = CallSno.ToString() == "" ? string.Empty : CallSno.ToString();

            string str1 = "";
            if (CallRecvDt != "" && PoNo == "" && PoDt == "" && CaseNo == "")
            {
                str1 = "l.CallRecvDt,l.CaseNo";
            }
            else
            {
                str1 = "l.CaseNo,l.CallRecvDt";
            }

            OracleParameter[] par = new OracleParameter[12];
            par[0] = new OracleParameter("p_regioncode", OracleDbType.Varchar2, RegionCode, ParameterDirection.Input);
            par[1] = new OracleParameter("p_caseno", OracleDbType.Varchar2, CaseNo, ParameterDirection.Input);
            par[2] = new OracleParameter("p_callrecvdt", OracleDbType.Date, _CallRecvDt, ParameterDirection.Input);
            par[3] = new OracleParameter("p_pono", OracleDbType.Varchar2, PoNo, ParameterDirection.Input);
            par[4] = new OracleParameter("p_podt", OracleDbType.Date, _PoDt, ParameterDirection.Input);
            par[5] = new OracleParameter("p_vendor", OracleDbType.Varchar2, Vendor, ParameterDirection.Input);
            par[6] = new OracleParameter("p_callletterno", OracleDbType.Varchar2, CallLetterNo, ParameterDirection.Input);
            par[7] = new OracleParameter("p_callsno", OracleDbType.Varchar2, CallSno, ParameterDirection.Input);
            par[8] = new OracleParameter("p_page_start", OracleDbType.Int32, dtParameters.Start + 1, ParameterDirection.Input);
            par[9] = new OracleParameter("p_page_end", OracleDbType.Int32, (dtParameters.Start + dtParameters.Length), ParameterDirection.Input);
            par[10] = new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output);
            par[11] = new OracleParameter("p_result_records", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GET_CALL_REG_CANCELLATION", par, 2);

            //List<VenderCallRegisterModel> modelList = new List<VenderCallRegisterModel>();
            //if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //{
            //    foreach (DataRow row in ds.Tables[0].Rows)
            //    {
            //        VenderCallRegisterModel model = new VenderCallRegisterModel
            //        {
            //            CaseNo = Convert.ToString(row["CASE_NO"]),
            //            CallRecvDt = Convert.ToString(row["CALL_RECV_DT"]) == "" ? null : Convert.ToDateTime(row["CALL_RECV_DT"]),
            //            CallInstallNo = Convert.ToInt32(row["CALL_INSTALL_NO"]) == null ? 0 : Convert.ToInt32(row["CALL_INSTALL_NO"]),
            //            CallSno = Convert.ToInt32(row["CALL_SNO"]) == null ? 0 : Convert.ToInt32(row["CALL_SNO"]),
            //            CallStatus = Convert.ToString(row["CALL_STATUS"]),
            //            CallLetterNo = Convert.ToString(row["CALL_LETTER_NO"]),
            //            Remarks = Convert.ToString(row["REMARKS"]),
            //            PoNo = Convert.ToString(row["PO_NO"]),
            //            PoDt = Convert.ToString(row["PO_DT"]) == "" ? null : Convert.ToDateTime(row["PO_DT"]),
            //            IeSname = Convert.ToString(row["IE_SNAME"]),
            //            Vendor = Convert.ToString(row["VENDOR"]),
            //            RegionCode = Convert.ToString(row["REGION_CODE"]),

            //        };
            //        modelList.Add(model);
            //    }
            //}

            List<VenderCallRegisterModel> list = new();
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                list = JsonConvert.DeserializeObject<List<VenderCallRegisterModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }

            query = list.AsQueryable();

            int recordsTotal = 0;
            if (ds != null && ds.Tables[1].Rows.Count > 0)
            {
                recordsTotal = Convert.ToInt32(ds.Tables[1].Rows[0]["total_records"]);
            }

            dTResult.recordsTotal = recordsTotal;
            dTResult.recordsFiltered = recordsTotal;
            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Select(p => p).ToList();
            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public VenderCallRegisterModel FindByID(string CaseNo, DateTime? CallRecvDt, string CallSno, string GetRegionCode)
        {
            VenderCallRegisterModel model = new();

            CaseNo = CaseNo.ToString() == "" ? string.Empty : CaseNo.ToString();
            //DateTime? _CallRecvDt = CallRecvDt == null ? null : DateTime.ParseExact(CallRecvDt, "dd-MM-yyyy", null);
            CallSno = CallSno.ToString() == "" ? string.Empty : CallSno.ToString();

            var CallData = context.ViewGetCallRegCancellations.Where(x => x.CaseNo == CaseNo && x.CallRecvDt == CallRecvDt && x.CallSno == Convert.ToInt32(CallSno)).FirstOrDefault();

            if (CallData == null)
                throw new Exception("Record Not found");
            else
            {
                model.CaseNo = CallData.CaseNo;
                model.PoNo = CallData.PoNo;
                model.PoDt = CallData.PoDt;
                model.CallSno = Convert.ToInt16(CallData.CallSno);
                model.CallRecvDt = CallData.CallRecvDt;
                model.Vendor = CallData.Vendor;
                model.CallLetterNo = CallData.CallLetterNo;
            }
            return model;
        }

        public DTResult<VenderCallRegisterModel> FindByModifyDetail(string CaseNo, string CallRecvDt, int CallSNo, string GetRegionCode)
        {
            DTResult<VenderCallRegisterModel> dTResult = new() { draw = 0 };
            IQueryable<VenderCallRegisterModel>? query = null;

            query = from p in context.T13PoMasters
                    join v in context.T05Vendors on p.VendCd equals v.VendCd
                    where p.CaseNo == CaseNo
                    select new VenderCallRegisterModel
                    {
                        VendCd = Convert.ToString(v.VendCd),
                        VendInspStopped = v.VendInspStopped,
                        Remarks = v.VendRemarks,
                    };

            dTResult.data = query;
            return dTResult;
        }

        public DTResult<VenderCallRegisterModel> FindMatchDetail(string CaseNo, string CallRecvDt, int CallSno, string GetRegionCode)
        {
            DTResult<VenderCallRegisterModel> dTResult = new() { draw = 0 };
            IQueryable<VenderCallRegisterModel>? query = null;

            string dt = Convert.ToDateTime(CallRecvDt).ToString("dd-MM-yyyy");
            DateTime parsedDate = DateTime.ParseExact(dt, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            query = from c in context.T17CallRegisters
                    where c.CaseNo == CaseNo && c.CallRecvDt == parsedDate && c.CallSno == CallSno
                    select new VenderCallRegisterModel
                    {
                        RegionCode = c.RegionCode,
                        SetRegionCode = GetRegionCode,
                    };

            dTResult.data = query;
            return dTResult;
        }

        public string GetRegionValue(string CaseNo, string CallRecvDt, string CallSno)
        {
            string SetRegion = "";
            CaseNo = CaseNo.ToString() == "" ? string.Empty : CaseNo.ToString();
            DateTime? _CallRecvDt = CallRecvDt == "" ? null : DateTime.ParseExact(CallRecvDt, "dd-MM-yyyy", null);
            CallSno = CallSno.ToString() == "" ? string.Empty : CallSno.ToString();

            var CallData = context.ViewGetCallRegCancellations.Where(x => x.CaseNo == CaseNo && x.CallRecvDt == _CallRecvDt && x.CallSno == Convert.ToInt32(CallSno)).FirstOrDefault();

            if (CallData == null)
                throw new Exception("Record Not found");
            else
            {
                SetRegion = CallData.RegionCode;
            }
            return SetRegion;
        }

        public VenderCallRegisterModel FindByManageID(string CaseNo, DateTime? CallRecvDt, int CallSno, string ActionType, string Region)
        {
            VenderCallRegisterModel model = new();
            //DateTime? _CallRecvDt = CallRecvDt == "" ? null : DateTime.ParseExact(CallRecvDt, "dd/MM/yyyy", null);

            VendorCallPoDetailsView GetView = context.VendorCallPoDetailsViews.Where(X => X.CaseNo == CaseNo).FirstOrDefault();


            if (ActionType == "A")
            {
                model.CallRecvDt = CallRecvDt;
                model.CallStatusDt = CallRecvDt;
                model.DtInspDesire = CallRecvDt;
                model.CallMarkDt = CallRecvDt;
                model.RegionCode = Region;

                //var maxCallSno = context.T17CallRegisters
                //    .Where(call => call.CallRecvDt == CallRecvDt && call.RegionCode == Region)
                //    .Max(call => (int?)call.CallSno) ?? 0;
                int cmdCL = context.T17CallRegisters.Where(x => x.CaseNo == CaseNo && x.CallRecvDt == Convert.ToDateTime(CallRecvDt) && x.RegionCode == Region).Count();

                var callSno = cmdCL + 1;
                model.CallSno = Convert.ToInt32(callSno);

                var result = from c in context.T17CallRegisters
                             join i in context.T09Ies on c.IeCd equals i.IeCd into iGroup
                             from i in iGroup.DefaultIfEmpty()
                             where c.CaseNo == CaseNo && c.CallRecvDt == CallRecvDt
                             select new
                             {
                                 CallMarkDt = c.CallMarkDt,
                                 CallSno = c.CallSno,
                                 IeName = i != null ? i.IeName : null
                             };

                var queryResult = result.ToList();

                if (queryResult.Count != 0)
                {
                    string msg = "The Call Already Present for the Given Case No and Call Date -: \\n";
                    for (int i = 0; i < queryResult.Count; i++)
                    {
                        msg += $"{i + 1}) Marked To: {queryResult[i].IeName} vide Call Serial No.={queryResult[i].CallSno} and Call Mark Date={Convert.ToDateTime(queryResult[i].CallMarkDt).ToString()}. \\n";
                    }
                    model.MsgStatus = msg;
                }

            }
            else if (ActionType == "M" || ActionType == "D")
            {


                var CallDetails = (from t17 in context.T17CallRegisters
                                   join t21 in context.T21CallStatusCodes on t17.CallStatus equals t21.CallStatusCd
                                   where t17.CaseNo == CaseNo && t17.CallRecvDt == CallRecvDt && t17.CallSno == CallSno
                                   select new
                                   {
                                       CaseNo = t17.CaseNo,
                                       CallRecvDt = t17.CallRecvDt,
                                       CallLetterNo = t17.CallLetterNo,
                                       CallLetterDt = t17.CallLetterDt,
                                       CallMarkDt = t17.CallMarkDt,
                                       CallSno = t17.CallSno,
                                       IeCd = t17.IeCd,
                                       DtInspDesire = t17.DtInspDesire,
                                       CallStatus = t21.CallStatusDesc + (t17.CallCancelStatus == "N" ? " (Non Chargeable)" : t17.CallCancelStatus == "C" ? " (Chargeable)" : ""),
                                       CallStatusDt = t17.CallStatusDt,
                                       CallRemarkStatus = t17.CallRemarkStatus,
                                       CallInstallNo = t17.CallInstallNo,
                                       RegionCode = t17.RegionCode,
                                       MfgCd = t17.MfgCd,
                                       MfgPlace = t17.MfgPlace,
                                       UpdateAllowed = t17.UpdateAllowed ?? "Y",
                                       Remarks = t17.Remarks,
                                       RejCanCall = t17.RejCanCall,
                                       FinalOrStage = t17.FinalOrStage,
                                       NewVendor = t17.NewVendor ?? "X",
                                       //COUNT_DT = t17.CountDt ?? 0,
                                       IrfcFunded = t17.IrfcFunded,
                                       DepartmentCode = t17.DepartmentCode,
                                       ClusterCode = t17.ClusterCode,
                                       Isfinalizedstatus = t17.Isfinalizedstatus
                                   }).FirstOrDefault();
                if (CallDetails == null)
                    throw new Exception("Record Not found");
                else
                {
                    model.CaseNo = CallDetails.CaseNo;
                    model.CallRecvDt = CallDetails.CallRecvDt;
                    model.CallLetterNo = CallDetails.CallLetterNo;
                    model.CallLetterDt = CallDetails.CallLetterDt;
                    model.CallMarkDt = CallDetails.CallMarkDt;
                    model.CallSno = (short)CallDetails.CallSno;
                    model.IeCd = CallDetails.IeCd;
                    model.DtInspDesire = CallDetails.DtInspDesire;
                    model.CallStatus = CallDetails.CallStatus;
                    //if (CallDetails.CallStatus != null)
                    //{
                    //    model.CallStatus = CallDetails.CallStatus.Equals("M") ? "Marked" : CallDetails.CallStatus.Equals("C") ? "Cancelled" : CallDetails.CallStatus.Equals("A") ? "Accepted" : CallDetails.CallStatus.Equals("R") ? "Rejected" : CallDetails.CallStatus.Equals("U") ? "Under Lab Testing" : CallDetails.CallStatus.Equals("S") ? "Still Under Inspection" : CallDetails.CallStatus.Equals("G") ? "Stage Inspection" : "";
                    //}
                    model.CallStatusDt = CallDetails.CallStatusDt;
                    model.CallRemarkStatus = CallDetails.CallRemarkStatus;
                    model.CallInstallNo = CallDetails.CallInstallNo;
                    model.RegionCode = CallDetails.RegionCode;
                    model.MfgCd = Convert.ToInt32(CallDetails.MfgCd);
                    model.MfgPlace = CallDetails.MfgPlace;
                    model.UpdateAllowed = CallDetails.UpdateAllowed ?? "Y";
                    model.Remarks = CallDetails.Remarks;
                    model.RejCanCall = CallDetails.RejCanCall;
                    model.FinalOrStage = CallDetails.FinalOrStage;
                    model.IsNewVender = CallDetails.NewVendor == "Y" ? true : false;
                    //model.CountDt = t17.CountDt ?? 0;
                    model.IrfcFunded = CallDetails.IrfcFunded;
                    model.DepartmentCode = CallDetails.DepartmentCode;
                    model.ClusterCode = CallDetails.ClusterCode;
                    model.IsFinalizedStatus = CallDetails.Isfinalizedstatus == "F" ? true : false;

                    T05Vendor Vendor = context.T05Vendors.Where(x => x.VendCd == Convert.ToInt32(CallDetails.MfgCd)).FirstOrDefault();
                    if (Vendor != null)
                    {
                        model.VendAdd1 = Vendor.VendAdd1;
                        model.VendContactPer1 = Vendor.VendContactPer1;
                        model.VendContactTel1 = Vendor.VendContactTel1;
                        model.VendStatus = Vendor.VendStatus;
                        model.VendStatusDtFr = Vendor.VendStatusDtFr;
                        model.VendStatusDtTo = Vendor.VendStatusDtTo;
                        model.VendEmail = Vendor.VendEmail;
                    }
                }
            }
            else
            {

            }

            if (GetView == null)
                throw new Exception("Record Not found");
            else
            {
                model.PurchaserCd = GetView.PurchaserCd;
                model.VendCd = GetView.VendCd;
                model.PoNo = GetView.PoNo;
                model.PoDt = GetView.PoDt;
                model.L5noPo = GetView.L5noPo;
                model.Rly = GetView.Rly;
                model.RlyNonrly = GetView.RlyNonrly;
                model.MfgCd = Convert.ToInt32(GetView.PoiCd);
            }

            var MfgDetails = context.ViewGetmanufvends.Where(x => x.VendCd == Convert.ToInt32(GetView.PoiCd)).FirstOrDefault();

            if (MfgDetails != null)
            {
                model.Vendor = MfgDetails.VendName;
                model.VendAdd1 = MfgDetails.VendAdd1;
                model.VendContactPer1 = MfgDetails.VendContactPer1;
                model.VendContactTel1 = MfgDetails.VendContactTel1;
                model.VendStatus = MfgDetails.VendStatus;
                model.VendStatusDtFr = Convert.ToDateTime(MfgDetails.VendStatusFr);
                model.VendStatusDtTo = Convert.ToDateTime(MfgDetails.VendStatusTo);
                model.VendEmail = MfgDetails.VendEmail;
            }
            //var ManufactureDetails = (from m in context.ViewGetmanufvends
            //                          where m.VendCd == Convert.ToInt32(GetView.PoiCd)
            //                          select new
            //                          {
            //                              VendCd = Convert.ToString(m.VendCd),
            //                              Vendor = m.VendName,
            //                              VendAdd1 = m.VendAdd1,
            //                              VendContactPer1 = m.VendContactPer1,
            //                              VendContactTel1 = m.VendContactTel1,
            //                              VendStatus = m.VendStatus,
            //                              VendStatusDtFr = Convert.ToDateTime(m.VendStatusFr),
            //                              VendStatusDtTo = Convert.ToDateTime(m.VendStatusTo),
            //                              VendEmail = m.VendEmail,
            //                          }).FirstOrDefault();


            model.Region = EnumUtility<Enums.Region>.GetDescriptionByKey(CaseNo.Substring(0, 1));
            model.RegionCode = Region;

            return model;
        }

        public DTResult<VenderCallRegisterModel> FindByVenderDetail1(int MfgCd, string CaseNo)
        {
            DTResult<VenderCallRegisterModel> dTResult = new() { draw = 0 };
            IQueryable<VenderCallRegisterModel>? query = null;


            if (MfgCd > 0)
            {
                query = from l in context.T05Vendors
                            //where l.VendCd == MfgCd
                        join c in context.T03Cities on l.VendCityCd equals (c.CityCd)
                        where l.VendCityCd == c.CityCd && l.VendName != null && l.VendCd == MfgCd

                        select new VenderCallRegisterModel
                        {
                            Vendor = Convert.ToString(l.VendName) + "/" + Convert.ToString(l.VendAdd1) + "/" + Convert.ToString(c.Location) + "/" + c.City,
                            VendCd = Convert.ToString(l.VendCd),
                            VendAdd1 = l.VendAdd1,
                            VendContactPer1 = l.VendContactPer1,
                            VendContactTel1 = l.VendContactTel1,
                            VendStatus = l.VendStatus,
                            VendStatusDtFr = l.VendStatusDtFr,
                            VendStatusDtTo = l.VendStatusDtTo,
                            VendEmail = l.VendEmail
                        };
            }
            else
            {
                var VendCd = (from l in context.T13PoMasters
                              join c in context.T05Vendors on l.VendCd equals (c.VendCd)
                              where l.CaseNo == CaseNo

                              select new VenderCallRegisterModel
                              {
                                  CaseNo = l.CaseNo,
                                  VendCd = Convert.ToString(c.VendCd)
                              }).FirstOrDefault();

                if (VendCd != null)
                {
                    query = from l in context.T05Vendors
                                //where l.VendCd == MfgCd
                            join c in context.T03Cities on l.VendCityCd equals (c.CityCd)
                            where l.VendCityCd == c.CityCd && l.VendName != null && l.VendCd == Convert.ToInt32(VendCd.VendCd)

                            select new VenderCallRegisterModel
                            {
                                Vendor = Convert.ToString(l.VendName) + "/" + Convert.ToString(l.VendAdd1) + "/" + Convert.ToString(c.Location) + "/" + c.City,
                                VendCd = Convert.ToString(l.VendCd),
                                VendAdd1 = l.VendAdd1,
                                VendContactPer1 = l.VendContactPer1,
                                VendContactTel1 = l.VendContactTel1,
                                VendStatus = l.VendStatus,
                                VendStatusDtFr = l.VendStatusDtFr,
                                VendStatusDtTo = l.VendStatusDtTo,
                                VendEmail = l.VendEmail
                            };
                }
            }


            dTResult.data = query;
            return dTResult;
        }

        public string RegiserCallSave(VenderCallRegisterModel model)
        {
            string ID = "";
            //ie_cd = FindIeCODE(model);
            string department1 = model.DepartmentCode;
            if (department1 == "M")
            {
                department1 = "M";
            }
            else if (department1 == "E")
            {
                department1 = "E";
            }
            else if (department1 == "C")
            {
                department1 = "C";
            }
            else
            {
                department1 = "M";
            }

            var IeCd = context.T101IeClusters.Where(x => x.ClusterCode == model.ClusterCode && x.DepartmentCode == department1).Select(x => x.IeCode).FirstOrDefault();
            model.IeCd = Convert.ToInt32(IeCd);

            var Co = context.T09Ies.Where(x => x.IeCd == Convert.ToInt32(IeCd)).Select(x => x.IeCoCd).FirstOrDefault();
            model.CoCd = Convert.ToByte(Co);

            if (model.ActionType == "A")
            {
                int cmdCL = context.T17CallRegisters.Where(x => x.CaseNo == model.CaseNo && x.CallLetterNo == model.CallLetterNo && x.RegionCode == model.SetRegionCode).Count();
                if (cmdCL == 0)
                {
                    var w_stage_or_final = "";

                    //var str3 = context.T17CallRegisters.Where(x => x.CallRecvDt == model.CallRecvDt && x.RegionCode == model.SetRegionCode).FirstOrDefault();
                    //CD = str3 + 1;
                    string rej_can_call = "";
                    if (model.CHKRejCan == "true")
                    {
                        rej_can_call = "Y";
                    }
                    if (model.CallStage == "S")
                    {
                        w_stage_or_final = "S";
                    }
                    else
                    {
                        w_stage_or_final = "F";
                    }
                    var w_New_Vendor = "";
                    if (model.IsNewVender == true)
                    {
                        w_New_Vendor = "Y";
                    }

                    string w_irfc_funded = "";
                    if (model.RlyNonrly == "R")
                    {
                        w_irfc_funded = Convert.ToString(model.IrfcFunded);
                    }
                    else
                    {
                        w_irfc_funded = "N";
                    }
                    model.e_status = 1;
                    if (IeCd > 0)
                    {
                        try
                        {
                            T17CallRegister obj = new T17CallRegister();
                            obj.CaseNo = model.CaseNo;
                            obj.CallRecvDt = Convert.ToDateTime(model.CallRecvDt);
                            obj.CallSno = Convert.ToInt32(model.CallSno);
                            obj.CallLetterNo = model.CallLetterNo;
                            obj.CallLetterDt = model.CallLetterDt;
                            obj.CallMarkDt = model.CallMarkDt;
                            obj.IeCd = model.IeCd;
                            obj.CoCd = Convert.ToInt32(Co);
                            obj.DtInspDesire = model.DtInspDesire;
                            obj.CallStatus = "M";
                            obj.CallStatusDt = model.CallStatusDt;
                            obj.CallRemarkStatus = model.CallRemarkStatus;
                            obj.CallInstallNo = model.CallInstallNo;
                            obj.RegionCode = model.SetRegionCode;
                            obj.MfgCd = model.MfgCd;
                            obj.UserId = model.UserId;
                            obj.Datetime = DateTime.Now;
                            obj.MfgPlace = model.VendAdd1;
                            obj.Remarks = model.Remarks;
                            obj.RejCanCall = rej_can_call;
                            obj.FinalOrStage = w_stage_or_final;
                            obj.NewVendor = w_New_Vendor;
                            obj.IrfcFunded = w_irfc_funded;
                            obj.ClusterCode = model.ClusterCode;
                            obj.DepartmentCode = model.DepartmentCode;
                            obj.Isfinalizedstatus = model.IsFinalizedStatus == true ? "F" : "N";

                            obj.Createdby = model.Createdby;
                            obj.Createddate = DateTime.Now;
                            context.T17CallRegisters.Add(obj);
                            context.SaveChanges();
                            ID = obj.CaseNo;
                        }
                        catch (Exception ex)
                        {
                            var msg = ex.Message;
                        }
                    }
                    //GetDtList(model);
                }
                else
                {
                    model.CaseNoNoFound = "NoFound";
                    ID = model.CaseNoNoFound;
                }
            }
            else if (model.ActionType == "M")
            {
                var w_New_Vendor = "";
                if (model.IsNewVender == true)
                {
                    w_New_Vendor = "Y";
                }
                var GetCall = context.T17CallRegisters.Where(x => x.CaseNo == model.CaseNo && x.CallRecvDt == model.CallRecvDt && x.CallSno == model.CallSno).FirstOrDefault();

                if (model.MfgCd > 0)
                {
                    model.e_status = 1;
                    #region Details save
                    if (model.DtInspDesire != null)
                    {
                        if (GetCall != null)
                        {
                            GetCall.CallLetterNo = model.CallLetterNo;
                            GetCall.CallLetterDt = model.CallLetterDt;
                            GetCall.CallMarkDt = model.CallMarkDt;
                            GetCall.CallSno = Convert.ToInt32(model.CallSno);
                            GetCall.DtInspDesire = model.DtInspDesire;
                            GetCall.CallStatusDt = model.CallStatusDt;
                            GetCall.CallRemarkStatus = model.CallRemarkStatus;
                            GetCall.CallInstallNo = model.CallInstallNo;
                            GetCall.Remarks = model.Remarks;
                            GetCall.MfgCd = model.MfgCd;
                            GetCall.MfgPlace = model.VendAdd1;
                            GetCall.IeCd = model.IeCd;
                            GetCall.CoCd = Convert.ToInt32(Co);
                            GetCall.DepartmentCode = model.DepartmentCode;
                            GetCall.NewVendor = w_New_Vendor;
                            GetCall.Isfinalizedstatus = model.IsFinalizedStatus == true ? "F" : "N";
                            GetCall.Updatedby = model.UserId;
                            GetCall.Updateddate = DateTime.Now;

                            context.SaveChanges();
                            ID = model.CaseNo;
                        }

                    }
                    else
                    {
                        if (GetCall != null)
                        {
                            GetCall.CallLetterNo = model.CallLetterNo;
                            GetCall.CallLetterDt = model.CallLetterDt;
                            GetCall.CallMarkDt = model.CallMarkDt;
                            GetCall.CallSno = Convert.ToInt32(model.CallSno);
                            GetCall.DtInspDesire = model.DtInspDesire;
                            GetCall.CallStatusDt = model.CallStatusDt;
                            GetCall.CallRemarkStatus = model.CallRemarkStatus;
                            GetCall.CallInstallNo = model.CallInstallNo;
                            GetCall.Remarks = model.Remarks;
                            GetCall.MfgCd = model.MfgCd;
                            GetCall.MfgPlace = model.VendAdd1;
                            GetCall.CountDt = Convert.ToBoolean(1);
                            GetCall.IeCd = model.IeCd;
                            GetCall.CoCd = Convert.ToInt32(Co);
                            GetCall.DepartmentCode = model.DepartmentCode;
                            GetCall.NewVendor = w_New_Vendor;
                            GetCall.Isfinalizedstatus = model.IsFinalizedStatus == true ? "F" : "N";
                            GetCall.Updatedby = model.UserId;
                            GetCall.Updateddate = DateTime.Now;

                            context.SaveChanges();
                            ID = model.CaseNo;
                        }
                    }
                    #endregion
                }
                else
                {
                    #region Details save
                    if (model.DtInspDesire != null)
                    {
                        if (GetCall != null)
                        {
                            GetCall.CallLetterNo = model.CallLetterNo;
                            GetCall.CallLetterDt = model.CallLetterDt;
                            GetCall.CallMarkDt = model.CallMarkDt;
                            GetCall.CallSno = Convert.ToInt32(model.CallSno);
                            GetCall.DtInspDesire = model.DtInspDesire;
                            GetCall.CallStatusDt = model.CallStatusDt;
                            GetCall.CallRemarkStatus = model.CallRemarkStatus;
                            GetCall.CallInstallNo = model.CallInstallNo;
                            GetCall.Remarks = model.Remarks;
                            GetCall.MfgCd = model.MfgCd;
                            GetCall.MfgPlace = model.VendAdd1;
                            GetCall.IeCd = model.IeCd;
                            GetCall.CoCd = Convert.ToInt32(Co);
                            GetCall.DepartmentCode = model.DepartmentCode;
                            GetCall.NewVendor = w_New_Vendor;
                            GetCall.Isfinalizedstatus = model.IsFinalizedStatus == true ? "F" : "N";
                            GetCall.Updatedby = model.UserId;
                            GetCall.Updateddate = DateTime.Now;
                            context.SaveChanges();
                            ID = model.CaseNo;
                        }

                    }
                    else
                    {
                        if (GetCall != null)
                        {
                            GetCall.CallLetterNo = model.CallLetterNo;
                            GetCall.CallLetterDt = model.CallLetterDt;
                            GetCall.CallMarkDt = model.CallMarkDt;
                            GetCall.CallSno = Convert.ToInt32(model.CallSno);
                            GetCall.DtInspDesire = model.DtInspDesire;
                            GetCall.CallStatusDt = model.CallStatusDt;
                            GetCall.CallRemarkStatus = model.CallRemarkStatus;
                            GetCall.CallInstallNo = model.CallInstallNo;
                            GetCall.Remarks = model.Remarks;
                            GetCall.MfgCd = model.MfgCd;
                            GetCall.MfgPlace = model.VendAdd1;
                            GetCall.CountDt = Convert.ToBoolean(1);
                            GetCall.IeCd = model.IeCd;
                            GetCall.CoCd = Convert.ToInt32(Co);
                            GetCall.DepartmentCode = model.DepartmentCode;
                            GetCall.NewVendor = w_New_Vendor;
                            GetCall.Isfinalizedstatus = model.IsFinalizedStatus == true ? "F" : "N";
                            GetCall.Updatedby = model.UserId;
                            GetCall.Updateddate = DateTime.Now;
                            context.SaveChanges();
                            ID = model.CaseNo;
                        }
                    }
                    #endregion
                }
            }
            return ID;
        }

        int FindIeCODE(VenderCallRegisterModel model)
        {
            string department1 = string.Empty;
            int Clustercode = 0;
            int vcode = 0;
            int cl_exist = 0;
            string region = model.SetRegionCode.ToString();

            department1 = model.DepartmentCode;
            if (department1 == "M")
            {
                department1 = "M";
            }
            else if (department1 == "E")
            {
                department1 = "E";
            }
            else if (department1 == "C")
            {
                department1 = "C";
            }
            else
            {
                department1 = "M";
            }

            vcode = Convert.ToInt32(model.MfgCd);

            var distinctClusterCodes = (from a in context.T100VenderClusters
                                        join b in context.T99ClusterMasters
                                        on a.ClusterCode equals b.ClusterCode
                                        where a.VendorCode == vcode && a.DepartmentName == department1 && b.RegionCode == region
                                        select b.ClusterCode).Distinct().ToList();
            if (distinctClusterCodes.Count > 0)
            {
                Clustercode = Convert.ToInt32(model.IeCd);
            }
            else
            {
                cl_exist = 1;
            }
            int ieCode = 0;
            var GetIeCodes = context.T101IeClusters.Where(x => x.ClusterCode == Clustercode && x.DepartmentCode == department1).FirstOrDefault();
            if (GetIeCodes != null)
            {
                ieCode = Convert.ToInt32(GetIeCodes.IeCode);
                //model.IeCd = ieCode;
                model.ClusterCode = ieCode;
            }
            var CoCd = context.T09Ies.Where(x => x.IeCd == model.ClusterCode).FirstOrDefault();
            if (CoCd != null)
            {
                model.CoCd = Convert.ToByte(CoCd.IeCoCd);
            }
            if (cl_exist == 1 && model.CHKRejCan == "false")
            {
                T100VenderCluster T100 = new T100VenderCluster();
                T100.VendorCode = model.MfgCd;
                T100.DepartmentName = department1;
                T100.ClusterCode = model.ClusterCode;
                T100.UserId = model.UserId;
                T100.Datetime = DateTime.Now.Date;
                context.T100VenderClusters.Add(T100);
                context.SaveChanges();
                Clustercode = Convert.ToInt16(model.ClusterCode);
            }
            return ieCode;
        }

        int FindIeCODE_OLD(VenderCallRegisterModel model)
        {
            string department1 = string.Empty;
            int strval = 0;
            int Clustercode;
            int vcode = 0;
            int cl_exist = 0;
            string region = model.SetRegionCode.ToString();

            department1 = model.DepartmentCode;
            if (department1 == "M")
            {
                department1 = "M";
            }
            else if (department1 == "E")
            {
                department1 = "E";
            }
            else if (department1 == "C")
            {
                department1 = "C";
            }
            else
            {
                department1 = "M";
            }

            vcode = Convert.ToInt32(model.MfgCd);

            var distinctClusterCodes = (from a in context.T100VenderClusters
                                        join b in context.T99ClusterMasters
                                        on a.ClusterCode equals b.ClusterCode
                                        where a.VendorCode == vcode && a.DepartmentName == department1 && b.RegionCode == region
                                        select b.ClusterCode).Distinct().ToList();
            if (distinctClusterCodes.Count > 0)
            {
                Clustercode = Convert.ToInt16(model.ClusterCode);
            }
            else
            {
                cl_exist = 1;
            }


            if (distinctClusterCodes.Count > 0)
            {
                Clustercode = distinctClusterCodes[0];
                if (Clustercode == 0)
                {
                    strval = 0;
                }
                else
                {
                    var GetIeCodes = context.T101IeClusters.Where(x => x.ClusterCode == Clustercode && x.DepartmentCode == department1).FirstOrDefault();
                    if (GetIeCodes != null)
                    {
                        int ieCode = Convert.ToInt32(GetIeCodes.IeCode);
                        DateTime startDate = new DateTime(2017, 1, 1);

                        int callStatusCount = context.T17CallRegisters
                            .Join(context.T09Ies,
                                a => a.IeCd,
                                b => b.IeCd,
                                (a, b) => new { CallRegister = a, IE = b })
                            .Where(joined => joined.CallRegister.CallStatus == "M" || joined.CallRegister.CallStatus == "S")
                            .Where(joined => joined.CallRegister.IeCd == ieCode)
                            .Where(joined => joined.CallRegister.CallRecvDt > startDate)
                            .Count();

                        if (callStatusCount > 0)
                        {
                            int countcalls = callStatusCount;
                            var ieCallMarking = context.T09Ies.Where(ie => ie.IeCd == ieCode).Select(ie => ie.IeCallMarking).FirstOrDefault();
                            if (Convert.ToInt32(ieCode) == 0)
                            {
                                strval = 0;
                            }
                            else
                            {
                                string callmarking = ieCallMarking;
                                if (callmarking == "")
                                {
                                    strval = 0;
                                }
                                if (callmarking == "N")
                                {

                                }
                                var maximumCall = context.T102IeMaximumCallLimits.Where(limit => limit.RegionCode == region).Select(limit => limit.MaximumCall).FirstOrDefault();
                                int Maximumcalls = Convert.ToInt32(maximumCall);
                                if (countcalls < Maximumcalls && callmarking == "Y")
                                {
                                    strval = ieCode;
                                }
                                else
                                {
                                    var altIe = context.T09Ies.Where(ie => ie.IeCd == ieCode).Select(ie => ie.AltIe).FirstOrDefault();
                                    if (altIe != null)
                                    {
                                        int Alt_ieCode = Convert.ToInt32(altIe);
                                        DateTime startDate1 = new DateTime(2017, 1, 1);

                                        int callStatusCount1 = context.T17CallRegisters
                                            .Join(context.T09Ies,
                                                a => a.IeCd,
                                                b => b.IeCd,
                                                (a, b) => new { CallRegister = a, IE = b })
                                            .Where(joined => joined.CallRegister.CallStatus == "M" || joined.CallRegister.CallStatus == "S")
                                            .Where(joined => joined.CallRegister.IeCd == Alt_ieCode)
                                            .Where(joined => joined.CallRegister.CallRecvDt > startDate1)
                                            .Count();
                                        if (callStatusCount1 > 0)
                                        {
                                            int countcalls123 = callStatusCount1;
                                            var ieCallMarking1 = context.T09Ies.Where(ie => ie.IeCd == Alt_ieCode).Select(ie => ie.IeCallMarking).FirstOrDefault();

                                            if (Alt_ieCode == 0)
                                            {
                                                strval = 0;
                                            }
                                            else
                                            {
                                                string callmarkings = ieCallMarking1;
                                                if (callmarkings == "")
                                                {
                                                    strval = 0;
                                                }
                                                if (callmarkings == "N")
                                                {

                                                }
                                                var maximumCall1 = context.T102IeMaximumCallLimits.Where(limit => limit.RegionCode == region).Select(limit => limit.MaximumCall).FirstOrDefault();
                                                if (maximumCall1 != null)
                                                {
                                                    int Maximumcalls1 = Convert.ToInt32(maximumCall1);
                                                    if (countcalls123 < Maximumcalls1 && callmarkings == "Y")
                                                    {
                                                        strval = Alt_ieCode;
                                                    }
                                                    else
                                                    {
                                                        var altIeTwo = context.T09Ies.Where(ie => ie.IeCd == ieCode).Select(ie => ie.AltIeTwo).FirstOrDefault();
                                                        if (altIeTwo != null)
                                                        {
                                                            //    strval = 0;
                                                            //}
                                                            //else
                                                            //{
                                                            int Alt_ieCode_TWO = Convert.ToInt32(altIeTwo);
                                                            DateTime startDate2 = new DateTime(2017, 1, 1);

                                                            int callStatusCount2 = context.T17CallRegisters
                                                                .Join(context.T09Ies,
                                                                    a => a.IeCd,
                                                                    b => b.IeCd,
                                                                    (a, b) => new { CallRegister = a, IE = b })
                                                                .Where(joined => joined.CallRegister.CallStatus == "M" || joined.CallRegister.CallStatus == "S")
                                                                .Where(joined => joined.CallRegister.IeCd == Alt_ieCode_TWO)
                                                                .Where(joined => joined.CallRegister.CallRecvDt > startDate2)
                                                                .Count();
                                                            if (callStatusCount2 > 0)
                                                            {
                                                                int countcalls1234 = callStatusCount2;
                                                                var ieCallMarking2 = context.T09Ies.Where(ie => ie.IeCd == Alt_ieCode_TWO).Select(ie => ie.IeCallMarking).FirstOrDefault();
                                                                if (Convert.ToInt32(Alt_ieCode_TWO) == 0)
                                                                {
                                                                    strval = 0;
                                                                }
                                                                else
                                                                {
                                                                    string callmarkings1 = ieCallMarking2;
                                                                    if (callmarkings1 == "")
                                                                    {
                                                                        strval = 0;
                                                                    }
                                                                    if (callmarkings1 == "N")
                                                                    {

                                                                    }
                                                                    var maximumCall2 = context.T102IeMaximumCallLimits.Where(limit => limit.RegionCode == region).Select(limit => limit.MaximumCall).FirstOrDefault();
                                                                    if (maximumCall2 != null)
                                                                    {
                                                                        int Maximumcalls12 = Convert.ToInt32(maximumCall2);
                                                                        if (countcalls1234 < Maximumcalls12 && callmarkings1 == "Y")
                                                                        {
                                                                            strval = Alt_ieCode_TWO;
                                                                        }
                                                                        else
                                                                        {
                                                                            var altIeThree = context.T09Ies.Where(ie => ie.IeCd == ieCode).Select(ie => ie.AltIeThree).FirstOrDefault();
                                                                            if (altIeThree != null)
                                                                            {
                                                                                strval = 0;
                                                                            }
                                                                            else
                                                                            {
                                                                                int Alt_ieCode_THREE = Convert.ToInt32(altIeThree);
                                                                                DateTime startDate3 = new DateTime(2017, 1, 1);

                                                                                int callStatusCount3 = context.T17CallRegisters
                                                                                    .Join(context.T09Ies,
                                                                                        a => a.IeCd,
                                                                                        b => b.IeCd,
                                                                                        (a, b) => new { CallRegister = a, IE = b })
                                                                                    .Where(joined => joined.CallRegister.CallStatus == "M" || joined.CallRegister.CallStatus == "S")
                                                                                    .Where(joined => joined.CallRegister.IeCd == Alt_ieCode_THREE)
                                                                                    .Where(joined => joined.CallRegister.CallRecvDt > startDate3)
                                                                                    .Count();
                                                                                if (callStatusCount3 > 0)
                                                                                {
                                                                                    int countcalls1233 = callStatusCount3;
                                                                                    var ieCallMarking3 = context.T09Ies.Where(ie => ie.IeCd == Alt_ieCode_THREE).Select(ie => ie.IeCallMarking).FirstOrDefault();
                                                                                    if (Alt_ieCode_THREE == 0)
                                                                                    {
                                                                                        strval = 0;
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        string callmarkings123 = ieCallMarking3;
                                                                                        if (callmarkings123 == "")
                                                                                        {
                                                                                            strval = 0;
                                                                                        }
                                                                                        if (callmarkings123 == "N")
                                                                                        {
                                                                                        }
                                                                                        var maximumCall3 = context.T102IeMaximumCallLimits.Where(limit => limit.RegionCode == region).Select(limit => limit.MaximumCall).FirstOrDefault();
                                                                                        if (maximumCall3 != null)
                                                                                        {
                                                                                            int Maximumcalls131 = Convert.ToInt32(maximumCall3);
                                                                                            if (countcalls1233 < Maximumcalls131 && callmarkings123 == "Y")
                                                                                            {
                                                                                                strval = Alt_ieCode_THREE;
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                strval = 0;
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                    return strval;
                                                                                }
                                                                            }
                                                                            return strval;
                                                                        }
                                                                        return strval;
                                                                    }
                                                                }
                                                                return strval;
                                                            }
                                                        }
                                                    }
                                                    return strval;
                                                }
                                            }
                                            return strval;
                                        }
                                    }
                                }
                                return strval;
                            }
                        }
                        return strval;
                    }

                }
            }
            else
            {
                cl_exist = 1;
            }
            if (cl_exist == 1 && model.CHKRejCan == "false")
            {
                T100VenderCluster T100 = new T100VenderCluster();
                T100.VendorCode = model.MfgCd;
                T100.DepartmentName = department1;
                T100.ClusterCode = model.ClusterCode;
                T100.UserId = model.UserId;
                T100.Datetime = DateTime.Now.Date;
                context.T100VenderClusters.Add(T100);
                context.SaveChanges();
                Clustercode = Convert.ToInt16(model.ClusterCode);
            }
            return strval;
        }

        int GetDtList(VenderCallRegisterModel model)
        {
            int err = 0;
            decimal qty_off_now = 0;

            List<VenderCallRegisterModel>? query = null;

            var ItemSrnoPo = (from a in context.T18CallDetails
                              where a.CaseNo == model.CaseNo && a.CallRecvDt == Convert.ToDateTime(model.CallRecvDt) && a.CallSno == Convert.ToInt16(model.CallSno)
                              select a.ItemSrnoPo).FirstOrDefault();

            //query = (from l in context.VenderCallRegisterItemView1s
            //         where l.CaseNo == model.CaseNo && l.CallRecvDt == Convert.ToDateTime(model.CallRecvDt) && l.CallSno == Convert.ToInt16(model.CallSno)

            //         select new VenderCallRegisterModel
            //         {
            //             Status = l.Status,
            //             ItemSrnoPo = l.ItemSrnoPo,
            //             ItemDescPo = l.ItemDescPo,
            //             QtyOrdered = l.QtyOrdered,
            //             CumQtyPrevOffered = l.CumQtyPrevOffered,
            //             CumQtyPrevPassed = l.CumQtyPrevPassed,
            //             QtyToInsp = l.QtyToInsp,
            //             QtyPassed = l.QtyPassed,
            //             QtyRejected = l.QtyRejected,
            //             QtyDue = l.QtyDue,
            //             Consignee = l.Consignee,
            //             DelvDate = l.DelvDate,
            //             CaseNo = l.CaseNo,
            //             CallRecvDt = Convert.ToDateTime(l.CallRecvDt),
            //             CallSno = Convert.ToInt16(l.CallSno)
            //         }).ToList();

            //query.AddRange(from l in context.VenderCallRegisterItemView2s
            //               where l.CaseNo == model.CaseNo && l.ItemSrnoPo != ItemSrnoPo

            //               select new VenderCallRegisterModel
            //               {
            //                   Status = l.Status,
            //                   ItemSrnoPo = l.ItemSrnoPo,
            //                   ItemDescPo = l.ItemDescPo,
            //                   QtyOrdered = l.QtyOrdered,
            //                   CumQtyPrevOffered = l.CumQtyPrevOffered,
            //                   CumQtyPrevPassed = l.CumQtyPrevPassed,
            //                   QtyToInsp = l.QtyToInsp,
            //                   QtyPassed = l.QtyPassed,
            //                   QtyRejected = l.QtyRejected,
            //                   QtyDue = l.QtyDue,
            //                   Consignee = l.Consignee,
            //                   DelvDate = l.DelvDate,
            //                   CaseNo = l.CaseNo,
            //                   CallRecvDt = Convert.ToDateTime(l.CallRecvDt),
            //                   CallSno = Convert.ToInt16(l.CallSno)
            //               });
            query = (from t15 in context.T15PoDetails
                     join t06 in context.T06Consignees on t15.ConsigneeCd equals t06.ConsigneeCd
                     join t18 in context.T18CallDetails on t15.CaseNo equals t18.CaseNo
                     join t03 in context.T03Cities on t06.ConsigneeCity equals t03.CityCd
                     join t14 in context.T14PoBpos on new { t15.CaseNo, ConsigneeCd = t15.ConsigneeCd ?? 0 } equals new { t14.CaseNo, t14.ConsigneeCd }
                     join b in context.T12BillPayingOfficers on t14.BpoCd equals b.BpoCd into bpoGroup
                     from bpo in bpoGroup.DefaultIfEmpty()
                     join c in context.T03Cities on bpo.BpoCityCd equals c.CityCd into cityGroup
                     from city in cityGroup.DefaultIfEmpty()
                     where t15.CaseNo == model.CaseNo && t18.ItemSrnoPo == ItemSrnoPo
                     select new VenderCallRegisterModel
                     {
                         Status = "Available",
                         ItemSrnoPo = t18.ItemSrnoPo,
                         ItemDescPo = t18.ItemDescPo,
                         QtyOrdered = t18.QtyOrdered,
                         CumQtyPrevOffered = t18.CumQtyPrevOffered,
                         CumQtyPrevPassed = t18.CumQtyPrevPassed,
                         QtyToInsp = t18.QtyToInsp,
                         QtyPassed = t18.QtyPassed,
                         QtyRejected = t18.QtyRejected,
                         QtyDue = t18.QtyDue,
                         Consignee = t06.ConsigneeCd + "-" +
                                    t06.ConsigneeDesig + "/" +
                                    t06.ConsigneeDept + "/" +
                                    t06.ConsigneeFirm + "/" +
                                    t06.ConsigneeAdd1 + "/" +
                                    t03.Location + " : " + t03.City,
                         DelvDt = Convert.ToDateTime(t15.ExtDelvDt),
                         CaseNo = t18.CaseNo,
                         CallRecvDt = t18.CallRecvDt,
                         CallSno = t18.CallSno,
                         Bpo = bpo.BpoCd + '-' +
                                bpo.BpoName + '/' +
                                bpo.BpoRly + '/' +
                                bpo.BpoAdd + '/' +
                                city.Location + '/' +
                                city.City,
                         ConsigneeCd = t06.ConsigneeCd
                     }).ToList();
            decimal wMat_value = 0;
            string ext_delv_dt = "";
            int desire_dt = 0;

            for (int i = 0; i < query.Count; i++)
            {
                VenderCallRegisterModel dataItem = query[i];
                err = 0;

                int srno = (byte)dataItem.ItemSrnoPo;
                decimal qtyOffNow = Convert.ToDecimal(dataItem.QtyToInsp);

                if (qtyOffNow > 0)
                {
                    err = 1;
                }
                if (err == 1)
                {
                    var query1 = from detail in context.T15PoDetails
                                 where detail.CaseNo == model.CaseNo && detail.ItemSrno == model.ItemSrnoPo
                                 select new
                                 {
                                     detail.ConsigneeCd,
                                     detail.Qty,
                                     detail.Value,
                                     EXT_DELV_DATE = detail.ExtDelvDt.HasValue ? detail.ExtDelvDt.Value.ToString("dd/MM/yyyy") : "01-JAN-01"
                                 };
                    long ccd = 0;
                    foreach (var record in query1)
                    {
                        ccd = (long)record.ConsigneeCd;
                        wMat_value += Convert.ToDecimal((record.Value / record.Qty) * qty_off_now);
                        model.wMat_value = wMat_value;
                        ext_delv_dt = record.EXT_DELV_DATE;
                    }
                    var CallDetails = context.T18CallDetails.Where(x => x.CaseNo == model.CaseNo && x.CallRecvDt == Convert.ToDateTime(model.CallRecvDt) && x.CallSno == model.CallSno && x.ItemSrnoPo == srno).FirstOrDefault();

                    if (CallDetails == null)
                    {
                        T18CallDetail obj = new T18CallDetail();
                        obj.CaseNo = model.CaseNo;
                        obj.CallRecvDt = Convert.ToDateTime(model.CallRecvDt);
                        obj.CallSno = (short)model.CallSno;
                        obj.ItemSrnoPo = (byte)srno;
                        obj.ItemDescPo = dataItem.ItemDescPo;
                        obj.ConsigneeCd = Convert.ToInt32(ccd);
                        obj.QtyOrdered = dataItem.QtyOrdered;
                        obj.CumQtyPrevOffered = dataItem.CumQtyPrevOffered;
                        obj.CumQtyPrevPassed = dataItem.CumQtyPrevPassed;
                        obj.QtyToInsp = Convert.ToDecimal(qtyOffNow);
                        obj.QtyPassed = null;
                        obj.QtyRejected = null;
                        obj.QtyDue = null;
                        obj.UserId = model.Createdby;
                        obj.Datetime = DateTime.Now;
                        obj.Createdby = model.Createdby;
                        obj.Createddate = DateTime.Now;

                        obj.Isdeleted = Convert.ToByte(false);
                        context.T18CallDetails.Add(obj);
                        context.SaveChanges();
                        err = Convert.ToInt32(obj.ItemSrnoPo);
                    }
                    else
                    {
                        CallDetails.QtyToInsp = Convert.ToDecimal(qtyOffNow);
                        context.SaveChanges();
                        err = Convert.ToInt32(CallDetails.ItemSrnoPo);
                    }
                    //T18CallDetail obj = new T18CallDetail();
                    //obj.CaseNo = model.CaseNo;
                    //obj.CallRecvDt = Convert.ToDateTime(model.CallRecvDt);
                    //obj.CallSno = (int)model.CallSno;
                    //obj.ItemSrnoPo = (byte)srno;
                    //obj.ItemDescPo = dataItem.ItemDescPo;
                    //obj.ConsigneeCd = Convert.ToInt32(ccd);
                    //obj.QtyOrdered = dataItem.QtyOrdered;
                    //obj.CumQtyPrevOffered = dataItem.CumQtyPrevOffered;
                    //obj.CumQtyPrevPassed = dataItem.CumQtyPrevPassed;
                    //obj.QtyToInsp = Convert.ToDecimal(qtyOffNow);
                    //obj.QtyPassed = null;
                    //obj.QtyRejected = null;
                    //obj.QtyDue = null;
                    //obj.UserId = model.Createdby;
                    //obj.Datetime = DateTime.Now;
                    //obj.Createdby = model.Createdby;
                    //obj.Createddate = DateTime.Now;

                    //obj.Isdeleted = Convert.ToByte(false);
                    //context.T18CallDetails.Add(obj);
                    //context.SaveChanges();
                    //err = Convert.ToInt32(obj.ItemSrnoPo);

                    if (desire_dt == 0)
                    {
                        desire_dt = check_desire_dt(model, ext_delv_dt);
                        model.desire_dt = desire_dt;
                    }

                }
            }

            if ((model.RlyNonrly == "R" && wMat_value > 1000 && desire_dt == 0) || (model.RlyNonrly != "R" && wMat_value > 1000 && desire_dt == 0 && model.Bpo != "" && model.RecipientGstinNo != ""))
            {
                show_items(model);

                if (wMat_value < 5000000)
                {
                    var contAltIeCode = (from t09 in context.T09Ies
                                         where t09.IeCd == Convert.ToInt32(model.IeCd)
                                         select t09.AltIe ?? 0).FirstOrDefault();

                    var regMaxLimit = (from t102 in context.T102IeMaximumCallLimits
                                       where t102.RegionCode == Convert.ToString(model.SetRegionCode)
                                       select t102.MaximumCall).FirstOrDefault();
                    int reg_max_limit = regMaxLimit.HasValue ? regMaxLimit.Value : 0;

                    var iePendCalls = (from a in context.T17CallRegisters
                                       join b in context.T09Ies on a.IeCd equals b.IeCd
                                       where a.CallStatus == "M" || a.CallStatus == "S" &&
                                             a.IeCd == contAltIeCode &&
                                             a.CallRecvDt > DateTime.Parse("2022-04-01")
                                       select a.CallStatus).Count();
                    int ie_pend_calls = iePendCalls;

                    var ieCallMarking = (from t09 in context.T09Ies
                                         where t09.IeCd == contAltIeCode
                                         select t09.IeCallMarking).FirstOrDefault();

                    string ie_call_marking = Convert.ToString(ieCallMarking);
                    if (contAltIeCode != 0 && ie_pend_calls < reg_max_limit && ie_call_marking == "Y")
                    {
                        var contAltIeNameQuery = from t09 in context.T09Ies
                                                 where t09.IeCd == contAltIeCode
                                                 select new { t09.IeName, t09.IeCoCd };

                        var contAltIeInfo = contAltIeNameQuery.FirstOrDefault();

                        string IE_name = contAltIeInfo?.IeName ?? "";
                        model.IE_name = IE_name;
                        int cont_ieofficercode = contAltIeInfo?.IeCoCd ?? 0;

                        var CallRegisters1 = context.T17CallRegisters.Where(x => x.CaseNo == model.CaseNo && x.CallRecvDt == model.CallRecvDt && x.CallSno == model.CallSno).FirstOrDefault();
                        if (CallRegisters1 != null)
                        {
                            CallRegisters1.IeCd = contAltIeCode;
                            CallRegisters1.CoCd = (byte)cont_ieofficercode;
                            CallRegisters1.Updatedby = model.Createdby;
                            CallRegisters1.Updateddate = DateTime.Now;
                            context.SaveChanges();
                            err = Convert.ToInt32(CallRegisters1.IeCd);
                        }
                        model.callval = contAltIeCode;
                    }
                }
                e_status = 1;
                model.e_status = e_status;
            }

            return err;
        }

        int check_desire_dt(VenderCallRegisterModel model, string ext_delv_dt)
        {
            if (ext_delv_dt == "01-01-2001")
            {
                return (2);
            }
            else
            {
                System.DateTime w_dt1 = new System.DateTime(Convert.ToInt32(ext_delv_dt.Substring(6, 4)), Convert.ToInt32(ext_delv_dt.Substring(3, 2)), Convert.ToInt32(ext_delv_dt.Substring(0, 2)));
                System.DateTime w_dt2 = new System.DateTime(Convert.ToInt32(model.DtInspDesire.ToString().Substring(6, 4)), Convert.ToInt32(model.DtInspDesire.ToString().Substring(3, 2)), Convert.ToInt32(model.DtInspDesire.ToString().Substring(0, 2)));
                TimeSpan ts = w_dt1 - w_dt2;
                int differenceInDays = ts.Days;
                if (differenceInDays < 5)
                {
                    return (1);
                }
                else
                {
                    return (0);
                }
            }

        }

        void show_items(VenderCallRegisterModel model)
        {
            try
            {
                var query11 = (from t18 in context.T18CallDetails
                               join t06 in context.T06Consignees on t18.ConsigneeCd equals t06.ConsigneeCd
                               join t03 in context.T03Cities on t06.ConsigneeCity equals t03.CityCd
                               where t18.CaseNo == CNO &&
                                     t18.CallRecvDt == DateTime.ParseExact(DT, "dd/MM/yyyy", CultureInfo.InvariantCulture) &&
                                     t18.CallSno == (int)model.CallSno
                               select new VenderCallRegisterModel
                               {
                                   Status = "Marked",
                                   ItemSrnoPo = t18.ItemSrnoPo,
                                   ItemDescPo = t18.ItemDescPo,
                                   QtyOrdered = t18.QtyOrdered,
                                   CumQtyPrevOffered = t18.CumQtyPrevOffered,
                                   CumQtyPrevPassed = t18.CumQtyPrevPassed,
                                   QtyToInsp = t18.QtyToInsp,
                                   QtyPassed = t18.QtyPassed,
                                   QtyRejected = t18.QtyRejected,
                                   QtyDue = t18.QtyDue,
                                   Consignee = $"{t06.ConsigneeCd}-" + (string.IsNullOrEmpty(t06.ConsigneeDesig) ? "" : t06.ConsigneeDesig + "/") +
                                               (string.IsNullOrEmpty(t06.ConsigneeDept) ? "" : t06.ConsigneeDept + "/") +
                                               (string.IsNullOrEmpty(t06.ConsigneeFirm) ? "" : t06.ConsigneeFirm + "/") +
                                               (string.IsNullOrEmpty(t06.ConsigneeAdd1) ? "" : t06.ConsigneeAdd1 + "/") +
                                               (string.IsNullOrEmpty(t03.Location) ? "" : t03.Location + " : " + t03.City),
                                   DelvDate = "01-01-2001"
                               }).ToList();

                var query22 = (from t15 in context.T15PoDetails
                               join t06 in context.T06Consignees on t15.ConsigneeCd equals t06.ConsigneeCd
                               join t03 in context.T03Cities on t06.ConsigneeCity equals t03.CityCd
                               where t15.CaseNo == CNO &&
                                     !(from t18 in context.T18CallDetails
                                       where t18.CaseNo == CNO &&
                             t18.CallRecvDt == DateTime.ParseExact(DT, "dd/MM/yyyy", CultureInfo.InvariantCulture) &&
                             t18.CallSno == (int)model.CallSno
                                       select t18.ItemSrnoPo).Contains(t15.ItemSrno)
                               select new VenderCallRegisterModel
                               {
                                   Status = "Available",
                                   ItemSrnoPo = t15.ItemSrno,
                                   ItemDescPo = t15.ItemDesc,
                                   QtyOrdered = t15.Qty,
                                   CumQtyPrevOffered = 0,
                                   CumQtyPrevPassed = 0,
                                   QtyToInsp = 0,
                                   QtyPassed = 0,
                                   QtyRejected = 0,
                                   QtyDue = 0,
                                   Consignee = $"{t06.ConsigneeCd}-" + (string.IsNullOrEmpty(t06.ConsigneeDesig) ? "" : t06.ConsigneeDesig + "/") +
                                               (string.IsNullOrEmpty(t06.ConsigneeDept) ? "" : t06.ConsigneeDept + "/") +
                                               (string.IsNullOrEmpty(t06.ConsigneeFirm) ? "" : t06.ConsigneeFirm + "/") +
                                               (string.IsNullOrEmpty(t06.ConsigneeAdd1) ? "" : t06.ConsigneeAdd1 + "/") +
                                               (string.IsNullOrEmpty(t03.Location) ? "" : t03.Location + " : " + t03.City),
                                   DelvDate = t15.ExtDelvDt.HasValue ? t15.ExtDelvDt.Value.ToString("dd/MM/yyyy") : "01-01-2001"
                               }).ToList();

                //query11.AddRange(query22);

                var combinedQuery = query11.Union(query22).OrderByDescending(item => item.Status).ThenBy(item => item.ItemSrnoPo);

                var results = combinedQuery.ToList();
            }
            catch (Exception ex)
            {
            }
            finally
            {
            }


        }

        public async Task<string> send_IE_smsAsync(VenderCallRegisterModel model)
        {
            string sms = "";
            string sender = "";
            string wIEMobile = "", wIEName = "", wVendor = "", wCOMobile = "", wVendMobile = "", wIEMobile_for_SMS = "";
            if (model.CaseNo.ToString().Substring(0, 1) == "N") { sender = "NR"; }
            else if (model.CaseNo.ToString().Substring(0, 1) == "W") { sender = "WR"; }
            else if (model.CaseNo.ToString().Substring(0, 1) == "E") { sender = "ER"; }
            else if (model.CaseNo.ToString().Substring(0, 1) == "S") { sender = "SR"; }
            else if (model.CaseNo.ToString().Substring(0, 1) == "C") { sender = "CR"; }
            else { sender = "RITES"; }

            var query = from t09 in context.T09Ies
                        join t08 in context.T08IeControllOfficers
                        on t09.IeCoCd equals t08.CoCd into t08Group
                        from t08 in t08Group.DefaultIfEmpty()
                        where t09.IeCd == model.IeCd
                        select new
                        {
                            IE_NAME = t09.IeName.Trim().Substring(0, Math.Min(t09.IeName.Trim().Length, 20)),
                            IE_PHONE_NO = t09.IePhoneNo.Trim().Substring(0, Math.Min(t09.IePhoneNo.Trim().Length, 10)),
                            CO_PHONE_NO = t08 != null ? t08.CoPhoneNo.Trim().Substring(0, Math.Min(t08.CoPhoneNo.Trim().Length, 10)) : ""
                        };

            var result = query.FirstOrDefault();

            if (result != null)
            {
                wIEName = result.IE_NAME;
                wIEMobile = result.IE_PHONE_NO;
                wIEMobile_for_SMS = result.IE_PHONE_NO;
                wCOMobile = result.CO_PHONE_NO;
            }

            var queryNew = from v in context.T05Vendors
                           join c in context.T03Cities
                           on v.VendCityCd equals c.CityCd
                           where v.VendCd == model.MfgCd
                           select new
                           {
                               VEND_NAME = v.VendName.Substring(0, Math.Min(v.VendName.Length, 30)).Replace("&", "AND"),
                               VEND_TEL = v.VendContactTel1.Trim().Substring(0, Math.Min(v.VendContactTel1.Trim().Length, 10))
                           };

            var resultNew = queryNew.FirstOrDefault();

            if (resultNew != null)
            {
                wVendor = resultNew.VEND_NAME;
                wVendMobile = resultNew.VEND_TEL;
            }

            if (wCOMobile != "") { wIEMobile = wIEMobile + "," + wCOMobile; }
            if (wVendMobile != "") { wIEMobile = wIEMobile + "," + wVendMobile; }
            string message = "RITES LTD - QA Call Marked, IE-" + wIEName + ",Contact No.:" + wIEMobile_for_SMS + ",RLY-" + model.Rly + ",PO-" + model.PoNo + ",DT- " + model.PoDt + ", Firm Name-" + wVendor + ", Call Sno - " + model.CallSno + ",DT- " + model.CallRecvDt + "- RITES/" + sender;

            using (HttpClient client = new HttpClient())
            {
                string baseurl = $"http://apin.onex-aura.com/api/sms?key=QtPr681q&to={wIEMobile}&from=RITESI&body={message}&entityid=1501628520000011823&templateid=1707161588918541674";

                HttpResponseMessage response = await client.GetAsync(baseurl);
                response.EnsureSuccessStatusCode(); // Ensure a successful response

                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);

                sms = "success";
            }


            return sms;
        }

        //public void Cancellation_Email(VenderCallStatusModel model)
        //{
        //    string MailID = Convert.ToString(config.GetSection("MailConfig")["MailID"]);
        //    string MailPass = Convert.ToString(config.GetSection("MailConfig")["MailPass"]);
        //    string MailSmtpClient = Convert.ToString(config.GetSection("MailConfig")["MailSmtpClient"]);

        //    string Case_Region = model.CaseNo.ToString().Substring(0, 1);
        //    string wRegion = "";
        //    string sender = "";
        //    string wPCity = "";
        //    string manu_mail = "", mfg_cd = "", manu_name = "", manu_city = "";
        //    string ie_phone = "", ie_name = "", ie_email = "", ie_co_email = "";
        //    string vend_cd = "", vend_name = "", vend_email = "", rly_cd = "", vend_city = "";

        //    var querys = from t13 in context.T13PoMasters
        //                 join t05 in context.T05Vendors on t13.VendCd equals t05.VendCd
        //                 join t03 in context.T03Cities on t05.VendCityCd equals t03.CityCd
        //                 where t13.CaseNo == model.CaseNo.Trim()
        //                 select new
        //                 {
        //                     t13.VendCd,
        //                     t05.VendName,
        //                     VEND_ADDRESS = t05.VendAdd2 != null ? t05.VendAdd1 + "/" + t05.VendAdd2 : t05.VendAdd1,
        //                     t03.City,
        //                     t05.VendEmail,
        //                     t13.RegionCode,
        //                     t13.RlyCd
        //                 };

        //    var results = querys.ToList();

        //    foreach (var item in results)
        //    {
        //        vend_cd = item.VendCd.ToString();
        //        vend_name = item.VendName;
        //        vend_city = item.City;
        //        vend_email = item.VendEmail;
        //        rly_cd = item.RlyCd;

        //        if (Case_Region == "N") { wRegion = "NORTHERN REGION <BR>12th FLOOR,CORE-II,SCOPE MINAR,LAXMI NAGAR, DELHI - 110092 <BR>Phone : +918800018691-95 <BR>Fax : 011-22024665"; sender = "nrinspn@rites.com"; wPCity = "New Delhi"; }
        //        else if (Case_Region == "S") { wRegion = "SOUTHERN REGION <BR>CTS BUILDING - 2ND FLOOR, BSNL COMPLEX, NO. 16, GREAMS ROAD,  CHENNAI - 600 006 <BR>Phone : 044-28292807/044- 28292817 <BR>Fax : 044-28290359"; sender = "srinspn@rites.com"; wPCity = "Chennai"; }
        //        else if (Case_Region == "E") { wRegion = "EASTERN REGION <BR>CENTRAL STATION BUILDING(METRO), 56, C.R. AVENUE,3rd FLOOR,KOLKATA-700 012  <BR>Fax : 033-22348704"; sender = "erinspn@rites.com"; wPCity = "Kolkata"; wPCity = "Kolkata"; }
        //        else if (Case_Region == "W") { wRegion = "WESTERN REGION <BR>5TH FLOOR, REGENT CHAMBER, ABOVE STATUS RESTAURANT,NARIMAN POINT,MUMBAI-400021 <BR>Phone : 022-68943400/68943445 <BR>"; sender = "wrinspn@rites.com"; wPCity = "Mumbai"; }
        //        else if (Case_Region == "C") { wRegion = "Central Region"; sender = "crinspn@rites.com"; }
        //    }

        //    var query = from t05 in context.T05Vendors
        //                join t17 in context.T17CallRegisters on t05.VendCd equals t17.MfgCd
        //                join t03 in context.T03Cities on t05.VendCityCd equals t03.CityCd
        //                where t17.CaseNo == model.CaseNo.Trim() &&
        //                      t17.CallRecvDt == model.CallRecvDt &&
        //                      t17.CallSno == model.CallSno
        //                select new
        //                {
        //                    MFG_NAME = t05.VendName,
        //                    MFG_CITY = t03.City,
        //                    t05.VendEmail,
        //                    t17.MfgCd
        //                };
        //    var result = query.FirstOrDefault();

        //    manu_mail = result.VendEmail;
        //    mfg_cd = result.MfgCd.ToString();
        //    manu_name = result.MFG_NAME;
        //    manu_city = result.MFG_CITY;

        //    var query2 = from t09 in context.T09Ies
        //                 join t08 in context.T08IeControllOfficers
        //                 on t09.IeCoCd equals t08.CoCd
        //                 where t09.IeCd == model.IeCd
        //                 select new
        //                 {
        //                     IE_PHONE_NO = t09.IePhoneNo,
        //                     CO_NAME = t08.CoName,
        //                     CO_PHONE_NO = t08.CoPhoneNo,
        //                     IE_NAME = t09.IeName,
        //                     IE_EMAIL = t09.IeEmail,
        //                     CO_Email = t08.CoEmail,
        //                 };

        //    var result2 = query2.FirstOrDefault();

        //    if (result2 != null)
        //    {
        //        ie_phone = result2.IE_PHONE_NO;
        //        ie_name = result2.IE_NAME;
        //        ie_email = result2.IE_EMAIL;
        //        ie_co_email = result2.CO_Email;
        //    }


        //    string call_letter_dt = "";
        //    if (Convert.ToString(model.CallLetterDt) == "")
        //    {
        //        call_letter_dt = "NIL";
        //    }
        //    else
        //    {
        //        call_letter_dt = Convert.ToString(model.CallLetterDt);
        //    }
        //    string mail_body = "";

        //    mail_body = vend_name + ", " + vend_city + " / " + manu_name + ", " + manu_city + ",<br><br> Your Call Letter Dated:  " + call_letter_dt + " for inspection of material against Agency.-" + rly_cd + ", PO No. - " + model.PoNo + " & Date - " + model.PoDt + ", Case NO. -" + model.CaseNo + ", registered on date: " + model.CallStatusDt + ", at SNo. " + model.CallSno + ". is Cancelled " + model.CallCancelStatusdrp + " on Date.-" + model.CallStatusDt + " by the concerned Inspection Engineer. - " + ie_name + " Contact No. " + ie_phone + ", Due to the following reasons.<br>";

        //    mail_body = mail_body + "=> Packing list showing quantities offered item wise and consignee wise not read (Whichever applicable) <br/> => 12 - Others (Specify)(Call cancelled on chargeable basis) <br><br>";

        //    mail_body = mail_body + "You are requested to submit cancellation charges for the amount of Rs. " + model.CanCharges + "/- + GST, through NEFT/RTGS/Credit card/Debit card/Net banking. </b> in f/o RITES LTD, Payble at " + wPCity + " along with next call.<br><b><u>Please note that call letter without Call Cancellation charges will not be accepted.</u></b><br>";

        //    mail_body = mail_body + "This is for your information and necessary corrective measures please. <br><br> Thanks for using RITES Inspection Services.<br> NATIONAL INSPECTION HELP LINE NUMBER : 1800 425 7000 (TOLL FREE). <br><br>" + wRegion + ".";

        //    bool isSend = false;
        //    if (vend_cd == mfg_cd && manu_mail != "")
        //    {
        //        if (Convert.ToString(config.GetSection("MailConfig")["SendMail"]) == "1")
        //        {
        //            SendMailModel sendMailModel = new SendMailModel();
        //            sendMailModel.From = sender;
        //            sendMailModel.To = manu_mail;
        //            sendMailModel.Bcc = "nrinspn@gmail.com";
        //            sendMailModel.Subject = "Your Call for Inspection By RITES";
        //            sendMailModel.Message = mail_body;
        //            isSend = pSendMailRepository.SendMail(sendMailModel, null);
        //        }
        //    }
        //    else if (vend_cd != mfg_cd && vend_email != "" && manu_mail != "")
        //    {
        //        if (Convert.ToString(config.GetSection("MailConfig")["SendMail"]) == "1")
        //        {
        //            SendMailModel sendMailModel = new SendMailModel();
        //            // sender for local mail testing
        //            sendMailModel.From = sender;
        //            sendMailModel.To = vend_email + ";" + manu_mail;
        //            sendMailModel.Bcc = "nrinspn@gmail.com";
        //            sendMailModel.Subject = "Your Call for Inspection By RITES";
        //            sendMailModel.Message = mail_body;
        //            isSend = pSendMailRepository.SendMail(sendMailModel, null);
        //        }
        //    }
        //    else if (vend_cd != mfg_cd && (vend_email == "" || manu_mail == ""))
        //    {
        //        if (Convert.ToString(config.GetSection("MailConfig")["SendMail"]) == "1")
        //        {
        //            SendMailModel sendMailModel = new SendMailModel();
        //            sendMailModel.From = sender;
        //            if (string.IsNullOrEmpty(vend_email))
        //            {
        //                sendMailModel.To = manu_mail;
        //            }
        //            else if (string.IsNullOrEmpty(manu_mail))
        //            {
        //                sendMailModel.To = vend_email;
        //            }
        //            else
        //            {
        //                sendMailModel.To = vend_email;
        //                sendMailModel.To = manu_mail;
        //            }
        //            sendMailModel.Bcc = "nrinspn@gmail.com";
        //            sendMailModel.Subject = "Your Call for Inspection By RITES";
        //            sendMailModel.Message = mail_body;
        //            isSend = pSendMailRepository.SendMail(sendMailModel, null);
        //        }
        //    }

        //    if (vend_email == "" && manu_mail == "")
        //    {
        //        if (Convert.ToString(config.GetSection("MailConfig")["SendMail"]) == "1")
        //        {
        //            mail_body = mail_body + "\n As their is no email-id available for Vendor/Manufacturer, So the email cannot be send to Vendor/Manufacturer.";

        //            SendMailModel sendMailModel = new SendMailModel();
        //            sendMailModel.From = sender;
        //            sendMailModel.To = vend_email + ";" + manu_mail;
        //            sendMailModel.Bcc = "nrinspn@gmail.com";
        //            if (Case_Region == "N")
        //            {
        //                sendMailModel.Bcc = ie_email + ";" + ";nrinspn@gmail.com" + ";nrinspn.fin@rites.com";
        //            }
        //            else
        //            {
        //                sendMailModel.Bcc = ie_email + ";" + ";nrinspn@gmail.com";
        //            }
        //            sendMailModel.Subject = "Your Call for Inspection By RITES has Cancelled..";
        //            sendMailModel.Message = mail_body;
        //            isSend = pSendMailRepository.SendMail(sendMailModel, null);
        //        }
        //    }
        //}

        public void Vendor_Rej_Email(VenderCallStatusModel model)
        {
            string MailID = Convert.ToString(config.GetSection("MailConfig")["MailID"]);
            string MailPass = Convert.ToString(config.GetSection("MailConfig")["MailPass"]);
            string MailSmtpClient = Convert.ToString(config.GetSection("MailConfig")["MailSmtpClient"]);

            string Case_Region = model.CaseNo.ToString().Substring(0, 1);
            string wRegion = "";
            string sender = "";
            string wPCity = "";
            string manu_mail = "", mfg_cd = "", manu_name = "", manu_city = "";
            string ie_phone = "", ie_name = "", ie_email = "", ie_co_email = "";
            string vend_cd = "", vend_name = "", vend_email = "", rly_cd = "", vend_city = "";

            var querys = from t13 in context.T13PoMasters
                         join t05 in context.T05Vendors on t13.VendCd equals t05.VendCd
                         join t03 in context.T03Cities on t05.VendCityCd equals t03.CityCd
                         where t13.CaseNo == model.CaseNo.Trim()
                         select new
                         {
                             t13.VendCd,
                             t05.VendName,
                             VEND_ADDRESS = t05.VendAdd2 != null ? t05.VendAdd1 + "/" + t05.VendAdd2 : t05.VendAdd1,
                             t03.City,
                             t05.VendEmail,
                             t13.RegionCode,
                             t13.RlyCd
                         };

            var results = querys.ToList();

            foreach (var item in results)
            {
                vend_cd = item.VendCd.ToString();
                vend_name = item.VendName;
                vend_city = item.City;
                vend_email = item.VendEmail;
                rly_cd = item.RlyCd;

                if (Case_Region == "N") { wRegion = "NORTHERN REGION <BR>12th FLOOR,CORE-II,SCOPE MINAR,LAXMI NAGAR, DELHI - 110092 <BR>Phone : +918800018691-95 <BR>Fax : 011-22024665"; sender = "nrinspn@rites.com"; wPCity = "New Delhi"; }
                else if (Case_Region == "S") { wRegion = "SOUTHERN REGION <BR>CTS BUILDING - 2ND FLOOR, BSNL COMPLEX, NO. 16, GREAMS ROAD,  CHENNAI - 600 006 <BR>Phone : 044-28292807/044- 28292817 <BR>Fax : 044-28290359"; sender = "srinspn@rites.com"; wPCity = "Chennai"; }
                else if (Case_Region == "E") { wRegion = "EASTERN REGION <BR>CENTRAL STATION BUILDING(METRO), 56, C.R. AVENUE,3rd FLOOR,KOLKATA-700 012  <BR>Fax : 033-22348704"; sender = "erinspn@rites.com"; wPCity = "Kolkata"; wPCity = "Kolkata"; }
                else if (Case_Region == "W") { wRegion = "WESTERN REGION <BR>5TH FLOOR, REGENT CHAMBER, ABOVE STATUS RESTAURANT,NARIMAN POINT,MUMBAI-400021 <BR>Phone : 022-68943400/68943445 <BR>"; sender = "wrinspn@rites.com"; wPCity = "Mumbai"; }
                else if (Case_Region == "C") { wRegion = "Central Region"; sender = "crinspn@rites.com"; }
            }

            var query = from t05 in context.T05Vendors
                        join t17 in context.T17CallRegisters on t05.VendCd equals t17.MfgCd
                        join t03 in context.T03Cities on t05.VendCityCd equals t03.CityCd
                        where t17.CaseNo == model.CaseNo.Trim() &&
                              t17.CallRecvDt == model.CallRecvDt &&
                              t17.CallSno == model.CallSno
                        select new
                        {
                            MFG_NAME = t05.VendName,
                            MFG_CITY = t03.City,
                            t05.VendEmail,
                            t17.MfgCd
                        };
            var result = query.FirstOrDefault();

            manu_mail = result.VendEmail;
            mfg_cd = result.MfgCd.ToString();
            manu_name = result.MFG_NAME;
            manu_city = result.MFG_CITY;

            var query2 = from t09 in context.T09Ies
                         join t08 in context.T08IeControllOfficers
                         on t09.IeCoCd equals t08.CoCd
                         where t09.IeCd == Convert.ToInt32(model.IeCd)
                         select new
                         {
                             IE_PHONE_NO = t09.IePhoneNo,
                             CO_NAME = t08.CoName,
                             CO_PHONE_NO = t08.CoPhoneNo,
                             IE_NAME = t09.IeName,
                             IE_EMAIL = t09.IeEmail,
                             CO_Email = t08.CoEmail,
                         };

            var result2 = query2.FirstOrDefault();

            if (result2 != null)
            {
                ie_phone = result2.IE_PHONE_NO;
                ie_name = result2.IE_NAME;
                ie_email = result2.IE_EMAIL;
                ie_co_email = result2.CO_Email;
            }


            string call_letter_dt = "";
            if (Convert.ToString(model.CallLetterDt) == "")
            {
                call_letter_dt = "NIL";
            }
            else
            {
                call_letter_dt = Convert.ToString(model.CallLetterDt);
            }
            string mail_body = "";
            string MailSubject = "";
            if (model.CallStatus == "R")
            {
                MailSubject = "Your Call for Inspection By RITES has Rejected.";
            }
            else
            {
                MailSubject = "Your Call for Inspection By RITES";
            }
            mail_body = vend_name + ", " + vend_city + " / " + manu_name + ", " + manu_city + ",<br><br> Your Call Letter Dated:  " + Convert.ToDateTime(call_letter_dt).ToString("dd/MM/yyyy") + " for inspection of material against Agency.-" + rly_cd + ", PO No. - " + model.PoNo + " & Date - " + Convert.ToDateTime(model.PoDt).ToString("dd/MM/yyyy") + ", Case NO. -" + model.CaseNo + ", registered on date: " + Convert.ToDateTime(model.CallStatusDt).ToString("dd/MM/yyyy") + ", at SNo. " + model.CallSno + ". is Rejected on Date.-" + Convert.ToDateTime(model.CallStatusDt).ToString("dd/MM/yyyy") + " by the concerned Inspection Engineer. - " + ie_name + " Contact No. " + ie_phone + "<br>";

            mail_body = mail_body + "You are requested to submit Rejection charges for the amount of Rs. " + model.RejectionCharge + "/- + GST, through NEFT/RTGS/Credit card/Debit card/Net banking. </b> in f/o RITES LTD, Payble at " + wPCity + " along with next call.<br><b><u>Please note that call letter without Call Rejection charges will not be accepted.</u></b><br>";

            mail_body = mail_body + "This is for your information and necessary corrective measures please. <br><br> Thanks for using RITES Inspection Services.<br> NATIONAL INSPECTION HELP LINE NUMBER : 1800 425 7000 (TOLL FREE). <br><br>" + wRegion + ".";

            bool isSend = false;
            if (vend_cd == mfg_cd && manu_mail != "")
            {
                if (Convert.ToString(config.GetSection("MailConfig")["SendMail"]) == "1")
                {
                    SendMailModel sendMailModel = new SendMailModel();
                    sendMailModel.From = sender;
                    sendMailModel.To = manu_mail;
                    sendMailModel.Bcc = "nrinspn@gmail.com";
                    sendMailModel.Subject = MailSubject;
                    sendMailModel.Message = mail_body;
                    isSend = pSendMailRepository.SendMail(sendMailModel, null);
                }
            }
            else if (vend_cd != mfg_cd && vend_email != "" && manu_mail != "")
            {
                if (Convert.ToString(config.GetSection("MailConfig")["SendMail"]) == "1")
                {
                    SendMailModel sendMailModel = new SendMailModel();
                    // sender for local mail testing
                    sendMailModel.From = sender;
                    sendMailModel.To = vend_email + ";" + manu_mail;
                    sendMailModel.Bcc = "nrinspn@gmail.com";
                    sendMailModel.Subject = MailSubject;
                    sendMailModel.Message = mail_body;
                    isSend = pSendMailRepository.SendMail(sendMailModel, null);
                }
            }
            else if (vend_cd != mfg_cd && (vend_email == "" || manu_mail == ""))
            {
                if (Convert.ToString(config.GetSection("MailConfig")["SendMail"]) == "1")
                {
                    SendMailModel sendMailModel = new SendMailModel();
                    sendMailModel.From = sender;
                    if (string.IsNullOrEmpty(vend_email))
                    {
                        sendMailModel.To = manu_mail;
                    }
                    else if (string.IsNullOrEmpty(manu_mail))
                    {
                        sendMailModel.To = vend_email;
                    }
                    else
                    {
                        sendMailModel.To = vend_email;
                        sendMailModel.To = manu_mail;
                    }
                    sendMailModel.Bcc = "nrinspn@gmail.com";
                    sendMailModel.Subject = MailSubject;
                    sendMailModel.Message = mail_body;
                    isSend = pSendMailRepository.SendMail(sendMailModel, null);
                }
            }

            if (vend_email == "" && manu_mail == "")
            {
                if (Convert.ToString(config.GetSection("MailConfig")["SendMail"]) == "1")
                {
                    mail_body = mail_body + "\n As their is no email-id available for Vendor/Manufacturer, So the email cannot be send to Vendor/Manufacturer.";

                    SendMailModel sendMailModel = new SendMailModel();
                    sendMailModel.From = sender;
                    sendMailModel.To = vend_email + ";" + manu_mail;
                    sendMailModel.Bcc = "nrinspn@gmail.com";
                    if (Case_Region == "N")
                    {
                        sendMailModel.Bcc = ie_email + ";" + ";nrinspn@gmail.com" + ";nrinspn.fin@rites.com";
                    }
                    else
                    {
                        sendMailModel.Bcc = ie_email + ";" + ";nrinspn@gmail.com";
                    }
                    sendMailModel.Subject = "Your Call for Inspection By RITES has Rejected.";
                    sendMailModel.Message = mail_body;
                    isSend = pSendMailRepository.SendMail(sendMailModel, null);
                }
            }
        }

        public string send_Vendor_Mail(VenderCallStatusModel model)
        {
            string MailID = Convert.ToString(config.GetSection("AppSettings")["MailID"]);
            string MailPass = Convert.ToString(config.GetSection("AppSettings")["MailPass"]);
            string MailSmtpClient = Convert.ToString(config.GetSection("AppSettings")["MailSmtpClient"]);

            string email = "";
            string Case_Region = model.CaseNo.ToString().Substring(0, 1);
            string wRegion = "";

            if (Case_Region == "N")
            {
                wRegion = "NORTHERN REGION <BR>12th FLOOR,CORE-II,SCOPE MINAR,LAXMI NAGAR, DELHI - 110092 <BR>Phone : +918800018691-95 <BR>Fax : 011-22024665";
            }
            else if (Case_Region == "S")
            {
                wRegion = "SOUTHERN REGION <BR>CTS BUILDING - 2ND FLOOR, BSNL COMPLEX, NO. 16, GREAMS ROAD,  CHENNAI - 600 006 <BR>Phone : 044-28292807/044- 28292817 <BR>Fax : 044-28290359";
            }
            else if (Case_Region == "E")
            {
                wRegion = "EASTERN REGION <BR>CENTRAL STATION BUILDING(METRO), 56, C.R. AVENUE,3rd FLOOR,KOLKATA-700 012  <BR>Fax : 033-22348704";
            }
            else if (Case_Region == "W")
            {
                wRegion = "WESTERN REGION <BR>5TH FLOOR, REGENT CHAMBER, ABOVE STATUS RESTAURANT,NARIMAN POINT,MUMBAI-400021 <BR>Phone : 022-68943400/68943445 <BR>";
            }
            else if (Case_Region == "C")
            {
                wRegion = "Central Region";
            }

            var query = from t13 in context.T13PoMasters
                        join t05 in context.T05Vendors on t13.VendCd equals t05.VendCd
                        join t03 in context.T03Cities on t05.VendCityCd equals t03.CityCd
                        where t13.CaseNo == model.CaseNo
                        select new
                        {
                            VEND_CD = t13.VendCd,
                            VEND_NAME = t05.VendName,
                            VEND_ADDRESS = t05.VendAdd2 != null ? $"{t05.VendAdd1}/{t05.VendAdd2}" : t05.VendAdd1 + "/" + t03.City,
                            VEND_EMAIL = t05.VendEmail
                        };

            var result = query.FirstOrDefault();

            int vend_cd = 0;
            string vend_add = "";
            string vend_email = "";
            string vend_name = "";
            string vend_city = "";

            if (result != null)
            {
                vend_cd = Convert.ToInt32(result.VEND_CD);
                vend_add = result.VEND_ADDRESS;
                vend_email = result.VEND_EMAIL;
                vend_name = result.VEND_NAME;
            }

            var query1 = from t05 in context.T05Vendors
                         join t17 in context.T17CallRegisters
                         on t05.VendCd equals t17.MfgCd
                         where t17.CaseNo == model.CaseNo &&
                               t17.CallRecvDt == model.CallRecvDt &&
                               t17.CallSno == model.CallSno
                         select new
                         {
                             VEND_EMAIL = t05.VendEmail,
                             MFG_CD = t17.MfgCd,
                             DESIRE_DT = t17.DtInspDesire
                         };

            var result1 = query1.FirstOrDefault();

            string manu_mail = "";
            int mfg_cd = 0;
            string desire_dt = null;

            if (result1 != null)
            {
                manu_mail = result1.VEND_EMAIL;
                mfg_cd = Convert.ToInt32(result1.MFG_CD);
                desire_dt = Convert.ToString(result1.DESIRE_DT);

            }
            var query2 = from t09 in context.T09Ies
                         join t08 in context.T08IeControllOfficers
                         on t09.IeCoCd equals t08.CoCd
                         where t09.IeCd == Convert.ToInt32(model.IeCd)
                         select new
                         {
                             IE_PHONE_NO = t09.IePhoneNo,
                             CO_NAME = t08.CoName,
                             CO_PHONE_NO = t08.CoPhoneNo,
                             IE_NAME = t09.IeName,
                             IE_EMAIL = t09.IeEmail
                         };

            var result2 = query2.FirstOrDefault();

            string ie_phone = "";
            string co_name = "";
            string co_mobile = "";
            string ie_name = "";
            string ie_email = "";
            string manu_city = "";
            string rly_cd = "";

            if (result2 != null)
            {
                ie_phone = result2.IE_PHONE_NO;
                co_name = result2.CO_NAME;
                co_mobile = result2.CO_PHONE_NO;
                ie_name = result2.IE_NAME;
                ie_email = result2.IE_EMAIL;

                // Use ie_phone, co_name, co_mobile, ie_name, ie_email as needed
            }

            var subquery = from t17 in context.T17CallRegisters
                           where t17.CallRecvDt > DateTime.ParseExact("01-APR-2017", "dd-MMM-yyyy", null) &&
                                 (t17.CallStatus == "M" || t17.CallStatus == "S") &&
                                 t17.IeCd == Convert.ToInt32(model.IeCd)
                           select t17;

            var query3 = from t17 in context.T17CallRegisters
                         where t17.CaseNo == model.CaseNo &&
                               t17.CallRecvDt == model.CallRecvDt &&
                               t17.CallSno == model.CallSno
                         select new
                         {
                             INSP_DATE = Convert.ToDateTime(t17.DtInspDesire).AddDays(subquery.Count() / 1.5).ToString("dd/MM/yyyy")
                         };

            var result3 = query3.FirstOrDefault();
            string dateto_attend = "";
            if (result3 != null)
            {
                dateto_attend = result3.INSP_DATE;
            }

            var recordToUpdate = context.T17CallRegisters.FirstOrDefault(t17 => t17.CaseNo == model.CaseNo &&
                            t17.CallRecvDt == model.CallRecvDt && t17.CallSno == model.CallSno);

            if (recordToUpdate != null)
            {
                recordToUpdate.ExpInspDt = DateTime.ParseExact(dateto_attend, "dd/MM/yyyy", null);
                context.SaveChanges();
            }

            var query4 = from t18 in context.T18CallDetails
                         join t15 in context.T15PoDetails on t18.CaseNo equals t15.CaseNo
                         join t61 in context.T61ItemMasters on t15.ItemCd equals t61.ItemCd
                         where t18.ItemSrnoPo == t15.ItemSrno && t15.CaseNo == model.CaseNo
                         group new { t61.TimeForInsp, t61.ItemCd } by t61.ItemCd into grouped
                         select new
                         {
                             ItemCd = grouped.Key,
                             DaysToIc = grouped.Max(g => g.TimeForInsp)
                         };

            var result4 = query4.ToList();

            int days_to_ic = 0;
            string item_cd = "";

            if (result4.Count > 0)
            {
                days_to_ic = Convert.ToInt32(result4[0].DaysToIc);
                item_cd = result4[0].ItemCd;
            }
            string manu_name = "", manu_add = "";
            var manufacturerInfo = (from t17 in context.T17CallRegisters
                                    join t05 in context.T05Vendors on t17.MfgCd equals t05.VendCd
                                    join t03 in context.T03Cities on t05.VendCityCd equals t03.CityCd
                                    where t17.CaseNo == model.CaseNo &&
                                    t17.CallRecvDt == model.CallRecvDt &&
                                    t17.CallSno == model.CallSno
                                    select new
                                    {
                                        manu_name = t05.VendName,
                                        manu_add = t03.City
                                    }).FirstOrDefault();
            string call_letter_dt = "";
            if (Convert.ToString(model.CallLetterDt) == "")
            {
                call_letter_dt = "NIL";
            }
            else
            {
                call_letter_dt = Convert.ToString(model.CallLetterDt);
            }
            string mail_body = "";
            string Callsubject = "";
            if (model.CallStatus == "C")
            {
                mail_body = vend_name + ", " + vend_add + " / " + manufacturerInfo.manu_name + ", " + manufacturerInfo.manu_add + ",<br><br> Your Call Letter Dated:  " + Convert.ToDateTime(call_letter_dt).ToString("dd/MM/yyyy") + " for inspection of material against Agency.-" + rly_cd + ", PO No. - " + model.PoNo + " & Date - " + Convert.ToDateTime(model.PoDt).ToString("dd/MM/yyyy") + ", Case NO. -" + model.CaseNo + ", registered on date: " + Convert.ToDateTime(model.CallRecvDt).ToString("dd/MM/yyyy") + ", at SNo. " + model.CallSno + ". is Cancelled (" + model.CallCancelStatusDesc + ") on Date.-" + model.CallStatusDt + " by the concerned Inspection Engineer. - " + ie_name + " Contact No. " + ie_phone + "<br>";

                mail_body = mail_body + "You are requested to submit call cancellation charges for the amount of Rs. " + model.CallCancelCharges + "/- + GST, through NEFT/RTGS/Credit card/Debit card/Net banking. </b> in f/o RITES LTD, Payble at " + manufacturerInfo.manu_add + " along with next call.<br><b><u>Please note that call letter without call cancellation charges will not be accepted.</u></b><br>";

                mail_body = mail_body + "This is for your information and necessary corrective measures please. <br><br> Thanks for using RITES Inspection Services.<br> NATIONAL INSPECTION HELP LINE NUMBER : 1800 425 7000 (TOLL FREE). <br><br>" + wRegion + ".";
                Callsubject = "Your Call for Inspection By RITES has Cancelled.";
            }
            else
            {
                mail_body = vend_name + ", " + vend_add + " / " + manufacturerInfo.manu_name + ", " + manufacturerInfo.manu_add + ",<br><br> Your Call Letter Dated:  " + call_letter_dt + " for inspection of material against Agency.-" + rly_cd + ", PO No. - " + model.PoNo + " & Date - " + Convert.ToDateTime(model.PoDt).ToString("dd/MM/yyyy") + ", Case NO. -" + model.CaseNo + ", registered on date: " + Convert.ToDateTime(model.CallRecvDt).ToString("dd/MM/yyyy") + ", at SNo. " + model.CallSno + ". is Cancelled (" + model.CallCancelStatus + ") on Date.-" + Convert.ToDateTime(model.CallStatusDt).ToString("dd/MM/yyyy") + " by the concerned Inspection Engineer. - " + ie_name + " Contact No. " + ie_phone + "<br>";
                mail_body = mail_body + "This is for your information and necessary corrective measures please.<br> NATIONAL INSPECTION HELP LINE NUMBER : 1800 425 7000 (TOLL FREE). <br><br> Thanks for using RITES Inspection Services. <br><br>" + wRegion + ".";
                Callsubject = "Your Call for Inspection By RITES";
            }
            bool isSend = false;
            string sender = "nrinspn@gmail.com";
            if (vend_cd == mfg_cd && manu_mail != "")
            {
                if (Convert.ToString(config.GetSection("MailConfig")["SendMail"]) == "1")
                {
                    SendMailModel sendMailModel = new SendMailModel();
                    // sender for local mail testing
                    sendMailModel.From = sender;
                    sendMailModel.To = manu_mail;
                    sendMailModel.Bcc = "nrinspn@gmail.com";
                    sendMailModel.Subject = Callsubject;
                    sendMailModel.Message = mail_body;
                    isSend = pSendMailRepository.SendMail(sendMailModel, null);
                }
            }
            else if (vend_cd != mfg_cd && vend_email != "" && manu_mail != "")
            {
                if (Convert.ToString(config.GetSection("MailConfig")["SendMail"]) == "1")
                {
                    SendMailModel sendMailModel = new SendMailModel();
                    // sender for local mail testing
                    sendMailModel.From = sender;
                    sendMailModel.To = vend_email + ";" + manu_mail;
                    sendMailModel.Bcc = "nrinspn@gmail.com";
                    sendMailModel.Subject = Callsubject;
                    sendMailModel.Message = mail_body;
                    isSend = pSendMailRepository.SendMail(sendMailModel, null);
                }
            }
            else if (vend_cd != mfg_cd && (vend_email == "" || manu_mail == ""))
            {
                if (Convert.ToString(config.GetSection("MailConfig")["SendMail"]) == "1")
                {
                    SendMailModel sendMailModel = new SendMailModel();
                    sendMailModel.From = sender;

                    if (string.IsNullOrEmpty(vend_email))
                    {
                        sendMailModel.To = manu_mail;
                    }
                    else if (string.IsNullOrEmpty(manu_mail))
                    {
                        sendMailModel.To = vend_email;
                    }
                    else
                    {
                        sendMailModel.To = vend_email + ";" + manu_mail;
                    }
                    sendMailModel.Bcc = "nrinspn@gmail.com";
                    sendMailModel.Subject = Callsubject;
                    sendMailModel.Message = mail_body;
                    isSend = pSendMailRepository.SendMail(sendMailModel, null);
                }
            }
            var controllingEmail = (from t08 in context.T08IeControllOfficers
                                    join t09 in context.T09Ies on t08.CoCd equals t09.IeCoCd
                                    where t09.IeCd == Convert.ToInt32(model.IeCd)
                                    select t08.CoEmail
                                    ).FirstOrDefault();
            if (controllingEmail != null)
            {
                if (Convert.ToString(config.GetSection("MailConfig")["SendMail"]) == "1")
                {
                    SendMailModel sendMailModel = new SendMailModel();
                    sendMailModel.From = sender;
                    sendMailModel.To = controllingEmail;
                    sendMailModel.Bcc = "nrinspn@gmail.com";
                    if (!string.IsNullOrEmpty(ie_email))
                    {
                        sendMailModel.CC = ie_email;
                    }
                    sendMailModel.Subject = "Your Call (" + manufacturerInfo.manu_name + " - " + manufacturerInfo.manu_add + ") for Inspection By RITES";
                    sendMailModel.Message = mail_body;
                    isSend = pSendMailRepository.SendMail(sendMailModel, null);
                }
            }
            return email;
        }

        public string send_Vendor_Email(VenderCallRegisterModel model)
        {
            string MailID = Convert.ToString(config.GetSection("AppSettings")["MailID"]);
            string MailPass = Convert.ToString(config.GetSection("AppSettings")["MailPass"]);
            string MailSmtpClient = Convert.ToString(config.GetSection("AppSettings")["MailSmtpClient"]);

            string email = "";
            string Case_Region = model.CaseNo.ToString().Substring(0, 1);
            string wRegion = "";

            if (Case_Region == "N")
            {
                wRegion = "NORTHERN REGION <BR>12th FLOOR,CORE-II,SCOPE MINAR,LAXMI NAGAR, DELHI - 110092 <BR>Phone : +918800018691-95 <BR>Fax : 011-22024665";
            }
            else if (Case_Region == "S")
            {
                wRegion = "SOUTHERN REGION <BR>CTS BUILDING - 2ND FLOOR, BSNL COMPLEX, NO. 16, GREAMS ROAD,  CHENNAI - 600 006 <BR>Phone : 044-28292807/044- 28292817 <BR>Fax : 044-28290359";
            }
            else if (Case_Region == "E")
            {
                wRegion = "EASTERN REGION <BR>CENTRAL STATION BUILDING(METRO), 56, C.R. AVENUE,3rd FLOOR,KOLKATA-700 012  <BR>Fax : 033-22348704";
            }
            else if (Case_Region == "W")
            {
                wRegion = "WESTERN REGION <BR>5TH FLOOR, REGENT CHAMBER, ABOVE STATUS RESTAURANT,NARIMAN POINT,MUMBAI-400021 <BR>Phone : 022-68943400/68943445 <BR>";
            }
            else if (Case_Region == "C")
            {
                wRegion = "Central Region";
            }

            var query = from t13 in context.T13PoMasters
                        join t05 in context.T05Vendors on t13.VendCd equals t05.VendCd
                        join t03 in context.T03Cities on t05.VendCityCd equals t03.CityCd
                        where t13.CaseNo == model.CaseNo
                        select new
                        {
                            VEND_CD = t13.VendCd,
                            VEND_NAME = t05.VendName,
                            VEND_ADDRESS = t05.VendAdd2 != null ? $"{t05.VendAdd1}/{t05.VendAdd2}" : t05.VendAdd1 + "/" + t03.City,
                            VEND_EMAIL = t05.VendEmail
                        };

            var result = query.FirstOrDefault();

            int vend_cd = 0;
            string vend_add = "";
            string vend_email = "";
            string vend_name = "";
            string vend_city = "";

            if (result != null)
            {
                vend_cd = Convert.ToInt32(result.VEND_CD);
                vend_add = result.VEND_ADDRESS;
                vend_email = result.VEND_EMAIL;
                vend_name = result.VEND_NAME;
            }

            var query1 = from t05 in context.T05Vendors
                         join t17 in context.T17CallRegisters
                         on t05.VendCd equals t17.MfgCd
                         where t17.CaseNo == model.CaseNo &&
                               t17.CallRecvDt == model.CallRecvDt &&
                               t17.CallSno == model.CallSno
                         select new
                         {
                             VEND_EMAIL = t05.VendEmail,
                             MFG_CD = t17.MfgCd,
                             DESIRE_DT = t17.DtInspDesire
                         };

            var result1 = query1.FirstOrDefault();

            string manu_mail = "";
            int mfg_cd = 0;
            string desire_dt = null;

            if (result1 != null)
            {
                manu_mail = result1.VEND_EMAIL;
                mfg_cd = Convert.ToInt32(result1.MFG_CD);
                desire_dt = Convert.ToString(result1.DESIRE_DT);

            }
            var query2 = from t09 in context.T09Ies
                         join t08 in context.T08IeControllOfficers
                         on t09.IeCoCd equals t08.CoCd
                         where t09.IeCd == Convert.ToInt32(model.IeCd)
                         select new
                         {
                             IE_PHONE_NO = t09.IePhoneNo,
                             CO_NAME = t08.CoName,
                             CO_PHONE_NO = t08.CoPhoneNo,
                             IE_NAME = t09.IeName,
                             IE_EMAIL = t09.IeEmail
                         };

            var result2 = query2.FirstOrDefault();

            string ie_phone = "";
            string co_name = "";
            string co_mobile = "";
            string ie_name = "";
            string ie_email = "";
            string manu_city = "";
            string rly_cd = "";

            if (result2 != null)
            {
                ie_phone = result2.IE_PHONE_NO;
                co_name = result2.CO_NAME;
                co_mobile = result2.CO_PHONE_NO;
                ie_name = result2.IE_NAME;
                ie_email = result2.IE_EMAIL;

                // Use ie_phone, co_name, co_mobile, ie_name, ie_email as needed
            }

            var subquery = from t17 in context.T17CallRegisters
                           where t17.CallRecvDt > DateTime.ParseExact("01-APR-2017", "dd-MMM-yyyy", null) &&
                                 (t17.CallStatus == "M" || t17.CallStatus == "S") &&
                                 t17.IeCd == Convert.ToInt32(model.IeCd)
                           select t17;

            var query3 = from t17 in context.T17CallRegisters
                         where t17.CaseNo == model.CaseNo &&
                               t17.CallRecvDt == model.CallRecvDt &&
                               t17.CallSno == model.CallSno
                         select new
                         {
                             INSP_DATE = Convert.ToDateTime(t17.DtInspDesire).AddDays(subquery.Count() / 1.5).ToString("dd/MM/yyyy")
                         };

            var result3 = query3.FirstOrDefault();
            string dateto_attend = "";
            if (result3 != null)
            {
                dateto_attend = result3.INSP_DATE;
            }

            var recordToUpdate = context.T17CallRegisters.FirstOrDefault(t17 => t17.CaseNo == model.CaseNo &&
                            t17.CallRecvDt == model.CallRecvDt && t17.CallSno == model.CallSno);

            if (recordToUpdate != null)
            {
                recordToUpdate.ExpInspDt = DateTime.ParseExact(dateto_attend, "dd/MM/yyyy", null);
                context.SaveChanges();
            }

            var query4 = from t18 in context.T18CallDetails
                         join t15 in context.T15PoDetails on t18.CaseNo equals t15.CaseNo
                         join t61 in context.T61ItemMasters on t15.ItemCd equals t61.ItemCd
                         where t18.ItemSrnoPo == t15.ItemSrno && t15.CaseNo == model.CaseNo
                         group new { t61.TimeForInsp, t61.ItemCd } by t61.ItemCd into grouped
                         select new
                         {
                             ItemCd = grouped.Key,
                             DaysToIc = grouped.Max(g => g.TimeForInsp)
                         };

            var result4 = query4.ToList();

            int days_to_ic = 0;
            string item_cd = "";

            if (result4.Count > 0)
            {
                days_to_ic = Convert.ToInt32(result4[0].DaysToIc);
                item_cd = result4[0].ItemCd;
            }
            string manu_name = "", manu_add = "";
            var manufacturerInfo = (from t17 in context.T17CallRegisters
                                    join t05 in context.T05Vendors on t17.MfgCd equals t05.VendCd
                                    join t03 in context.T03Cities on t05.VendCityCd equals t03.CityCd
                                    where t17.CaseNo == model.CaseNo &&
                                    t17.CallRecvDt == model.CallRecvDt &&
                                    t17.CallSno == model.CallSno
                                    select new
                                    {
                                        manu_name = t05.VendName,
                                        manu_add = t03.City
                                    }).FirstOrDefault();
            string call_letter_dt = "";
            if (Convert.ToString(model.CallLetterDt) == "")
            {
                call_letter_dt = "NIL";
            }
            else
            {
                call_letter_dt = Convert.ToString(model.CallLetterDt);
            }
            //string mail_body = "";
            //if (model.CallCancelStatus == "C")
            //{
            //    mail_body = vend_name + ", " + vend_add + " / " + manufacturerInfo.manu_name + ", " + manufacturerInfo.manu_add + ",<br><br> Your Call Letter Dated:  " + call_letter_dt + " for inspection of material against Agency.-" + rly_cd + ", PO No. - " + model.PoNo + " & Date - " + model.PoDt + ", Case NO. -" + model.CaseNo + ", registered on date: " + model.CallRecvDt + ", at SNo. " + model.CallSno + ". is Cancelled (" + model.CallCancelStatus + ") on Date.-" + model.CallStatusDt + " by the concerned Inspection Engineer. - " + ie_name + " Contact No. " + ie_phone + "<br>";

            //    mail_body = mail_body + "You are requested to submit call cancellation charges for the amount of Rs. " + model.CallCancelCharges + "/- + GST, through NEFT/RTGS/Credit card/Debit card/Net banking. </b> in f/o RITES LTD, Payble at " + manufacturerInfo.manu_add + " along with next call.<br><b><u>Please note that call letter without call cancellation charges will not be accepted.</u></b><br>";

            //    mail_body = mail_body + "This is for your information and necessary corrective measures please. <br><br> Thanks for using RITES Inspection Services.<br> NATIONAL INSPECTION HELP LINE NUMBER : 1800 425 7000 (TOLL FREE). <br><br>" + wRegion + ".";
            //}
            //else
            //{
            //    mail_body = vend_name + ", " + vend_add + " / " + manufacturerInfo.manu_name + ", " + manufacturerInfo.manu_add + ",<br><br> Your Call Letter Dated:  " + call_letter_dt + " for inspection of material against Agency.-" + rly_cd + ", PO No. - " + model.PoNo + " & Date - " + model.PoDt + ", Case NO. -" + model.CaseNo + ", registered on date: " + model.CallRecvDt + ", at SNo. " + model.CallSno + ". is Cancelled (" + model.CallCancelStatus + ") on Date.-" + model.CallStatusDt + " by the concerned Inspection Engineer. - " + ie_name + " Contact No. " + ie_phone + "<br>";
            //    mail_body = mail_body + "This is for your information and necessary corrective measures please.<br> NATIONAL INSPECTION HELP LINE NUMBER : 1800 425 7000 (TOLL FREE). <br><br> Thanks for using RITES Inspection Services. <br><br>" + wRegion + ".";
            //}
            string mail_body = "Dear Sir/Madam,<br><br> In Reference to your Call Letter dated:  " + call_letter_dt + " for inspection of material against PO No. - " + model.PoNo + " & date - " + model.PoDt + ", Call has been registered vide Case No -  " + model.CaseNo + ", on date: " + model.CallRecvDt + ", at SNo. " + model.CallSno + ".<br> ";
            if (model.CallRecvDt != Convert.ToDateTime(desire_dt.Trim()))
            {
                mail_body = mail_body + "The Desired Inspection Date of this call shall be on or after: " + Convert.ToDateTime(desire_dt.Trim()) + ".<br>";
            }
            if (days_to_ic == 0)
            {
                mail_body = mail_body + "The inspection call has been assigned to Inspecting Engineer Sh. " + ie_name + ", Contact No. " + ie_phone + ", Email ID: " + ie_email + ". Based on the current workload with the IE, Inspection is likely to be attended on or before " + dateto_attend + " or next working day (In case the above date happens to be a holiday). Dates are subject to last minute changes due to  exigencies of work and overriding Client priorities. <br> Name of Controlling Manager of concerned IE Sh.: " + co_name + ", Contact No." + co_mobile + ". <br>Offered Material as per registration should be readily available on the indicated date along with all related documents and internal test reports.<br><a href='http://rites.ritesinsp.com/RBS/Guidelines for Vendors.pdf'>Guidelines for Vendors</a>.<br>For Inspection related information please visit : http://ritesinsp.com. <br> For any correspondence in future, please quote Case No. only.<br><br> Thanks for using RITES Inspection Services. <br><br>" + wRegion + ".";
            }
            else if (days_to_ic > 0)
            {
                System.DateTime w_dt1 = new System.DateTime(Convert.ToInt32(dateto_attend.Substring(6, 4)), Convert.ToInt32(dateto_attend.Substring(3, 2)), Convert.ToInt32(dateto_attend.Substring(0, 2)));
                System.DateTime w_dt2 = w_dt1.AddDays(days_to_ic);
                string date_to_ic = w_dt2.ToString("dd/MM/yyyy");
                mail_body = mail_body + "The inspection call has been assigned to Inspecting Engineer Sh. " + ie_name + ", Contact No. " + ie_phone + ", Email ID: " + ie_email + ". Based on the current workload with the IE, Inspection is likely to be attended on or before " + dateto_attend + " or next working day (In case the above date happens to be a holiday) and Inspection certificate is likely to issued by " + date_to_ic + ". Dates are subject to last minute changes due to  exigencies of work and overriding Client priorities. <br> Name of Controlling Manager of concerned IE Sh.: " + co_name + ", Contact No." + co_mobile + ". <br>Offered Material as per registration should be readily available on the indicated date along with all related documents and internal test reports. Inspection is proposed to be conducted as per inspection plan: <a href='http://rites.ritesinsp.com/RBS/MASTER_ITEMS_CHECKSHEETS/" + item_cd + ".RAR'>Inspection Plan</a>.<br><a href='http://rites.ritesinsp.com/RBS/Guidelines for Vendors.pdf'>Guidelines for Vendors</a>.<br>For Inspection related information please visit : http://ritesinsp.com. <br> For any correspondence in future, please quote Case No. only. <br><br> Thanks for using RITES Inspection Services. <br> NATIONAL INSPECTION HELP LINE NUMBER : 1800 425 7000 (TOLL FREE).<br><br>" + wRegion + ".";
            }
            mail_body = mail_body + "<br><br> THIS IS AN AUTO GENERATED EMAIL. PLEASE DO NOT REPLY. USE EMAIL GIVEN IN THE REGION ADDRESS.";
            string sender = "";
            if (Case_Region == "N")
            {
                sender = "nrinspn@rites.com";
            }
            else if (Case_Region == "W")
            {
                sender = "wrinspn@rites.com";
            }
            else if (Case_Region == "E")
            {
                sender = "erinspn@rites.com";
            }
            else if (Case_Region == "S")
            {
                sender = "srinspn@rites.com";
            }
            else if (Case_Region == "C")
            {
                sender = "crinspn@rites.com";
            }

            bool isSend = false;


            if (vend_cd == mfg_cd && manu_mail != "")
            {
                if (Convert.ToString(config.GetSection("MailConfig")["SendMail"]) == "1")
                {
                    SendMailModel sendMailModel = new SendMailModel();
                    // sender for local mail testing
                    sendMailModel.From = sender;
                    sendMailModel.To = manu_mail;
                    sendMailModel.Bcc = "nrinspn@gmail.com";
                    sendMailModel.Subject = "Your Call for Inspection By RITES";
                    sendMailModel.Message = mail_body;
                    isSend = pSendMailRepository.SendMail(sendMailModel, null);
                }
                //MailMessage mail = new MailMessage();
                //mail.To.Add(manu_mail);
                //mail.Bcc.Add("nrinspn@gmail.com");
                //mail.From = new MailAddress("nrinspn@gmail.com");
                //mail.Subject = "Your Call for Inspection By RITES";
                //mail.IsBodyHtml = true; // Set to true if the body contains HTML content
                //mail.Body = mail_body;

                //SmtpClient smtpClient = new SmtpClient(MailSmtpClient); // Set your SMTP server address
                //smtpClient.Credentials = new NetworkCredential(MailID, MailPass); // If authentication is required
                //                                                                  // Send the email
                //try
                //{
                //    smtpClient.Send(mail);
                //}
                //catch (Exception ex)
                //{
                //}
                //finally
                //{
                //    mail.Dispose();
                //    smtpClient.Dispose();
                //}
            }
            else if (vend_cd != mfg_cd && vend_email != "" && manu_mail != "")
            {
                if (Convert.ToString(config.GetSection("MailConfig")["SendMail"]) == "1")
                {
                    SendMailModel sendMailModel = new SendMailModel();
                    // sender for local mail testing
                    sendMailModel.From = sender;
                    sendMailModel.To = vend_email + ";" + manu_mail;
                    sendMailModel.Bcc = "nrinspn@gmail.com";
                    sendMailModel.Subject = "Your Call for Inspection By RITES";
                    sendMailModel.Message = mail_body;
                    isSend = pSendMailRepository.SendMail(sendMailModel, null);
                }

                //MailMessage mail = new MailMessage();
                //mail.To.Add(vend_email);
                //mail.To.Add(manu_mail);
                //mail.Bcc.Add("nrinspn@gmail.com");
                //mail.From = new MailAddress("nrinspn@gmail.com");
                //mail.Subject = "Your Call for Inspection By RITES";
                //mail.IsBodyHtml = true; // Set to true if the body contains HTML content
                //mail.Body = mail_body;

                //SmtpClient smtpClient = new SmtpClient(MailSmtpClient); // Set your SMTP server address
                //smtpClient.Credentials = new NetworkCredential(MailID, MailPass); // If authentication is required
                //try
                //{
                //    smtpClient.Send(mail);
                //}
                //catch (Exception ex)
                //{
                //}
                //finally
                //{
                //    mail.Dispose();
                //    smtpClient.Dispose();
                //}
            }
            else if (vend_cd != mfg_cd && (vend_email == "" || manu_mail == ""))
            {
                if (Convert.ToString(config.GetSection("MailConfig")["SendMail"]) == "1")
                {
                    SendMailModel sendMailModel = new SendMailModel();
                    // sender for local mail testing
                    sendMailModel.From = sender;

                    if (string.IsNullOrEmpty(vend_email))
                    {
                        sendMailModel.To = manu_mail;
                    }
                    else if (string.IsNullOrEmpty(manu_mail))
                    {
                        sendMailModel.To = vend_email;
                    }
                    else
                    {
                        sendMailModel.To = vend_email + ";" + manu_mail;
                    }
                    sendMailModel.Bcc = "nrinspn@gmail.com";
                    sendMailModel.Subject = "Your Call for Inspection By RITES";
                    sendMailModel.Message = mail_body;
                    isSend = pSendMailRepository.SendMail(sendMailModel, null);
                }
                //MailMessage mail = new MailMessage();
                //if (string.IsNullOrEmpty(vend_email))
                //{
                //    mail.To.Add(manu_mail);
                //}
                //else if (string.IsNullOrEmpty(manu_mail))
                //{
                //    mail.To.Add(vend_email);
                //}
                //else
                //{
                //    mail.To.Add(vend_email);
                //    mail.To.Add(manu_mail);
                //}

                //mail.Bcc.Add("nrinspn@gmail.com");
                //mail.From = new MailAddress("nrinspn@gmail.com");
                //mail.Subject = "Your Call for Inspection By RITES";
                //mail.IsBodyHtml = true; // Set to true if the body contains HTML content
                //mail.Body = mail_body;
                //SmtpClient smtpClient = new SmtpClient(MailSmtpClient); // Set your SMTP server address
                //smtpClient.Credentials = new NetworkCredential(MailID, MailPass); // If authentication is required
                //try
                //{
                //    smtpClient.Send(mail);
                //}
                //catch (Exception ex)
                //{
                //}
                //finally
                //{
                //    mail.Dispose();
                //    smtpClient.Dispose();
                //}
            }

            var controllingEmail = (from t08 in context.T08IeControllOfficers
                                    join t09 in context.T09Ies on t08.CoCd equals t09.IeCoCd
                                    where t09.IeCd == Convert.ToInt32(model.IeCd)
                                    select t08.CoEmail
                                    ).FirstOrDefault();


            if (controllingEmail != "")
            {

                if (Convert.ToString(config.GetSection("MailConfig")["SendMail"]) == "1")
                {
                    SendMailModel sendMailModel = new SendMailModel();
                    // sender for local mail testing
                    sendMailModel.From = sender;
                    sendMailModel.To = controllingEmail;
                    sendMailModel.Bcc = "nrinspn@gmail.com";
                    if (!string.IsNullOrEmpty(ie_email))
                    {
                        sendMailModel.CC = ie_email;
                    }
                    sendMailModel.Subject = "Your Call (" + manufacturerInfo.manu_name + " - " + manufacturerInfo.manu_add + ") for Inspection By RITES";
                    sendMailModel.Message = mail_body;
                    isSend = pSendMailRepository.SendMail(sendMailModel, null);
                }
                //MailMessage mail2 = new MailMessage();
                //mail2.To.Add(controllingEmail);
                //mail2.Bcc.Add("nrinspn@gmail.com");
                //if (!string.IsNullOrEmpty(ie_email))
                //{
                //    mail2.CC.Add(ie_email);
                //}
                //mail2.From = new MailAddress("nrinspn@gmail.com");
                //mail2.Subject = "Your Call (" + manu_name + " - " + manu_add + ") for Inspection By RITES";
                //mail2.IsBodyHtml = true;
                //mail2.Body = mail_body;

                //SmtpClient smtpClient2 = new SmtpClient(MailSmtpClient); // Set your SMTP server address
                //smtpClient2.Credentials = new NetworkCredential(MailID, MailPass); // If authentication is required
                //try
                //{
                //    smtpClient2.Send(mail2);
                //    email = "success";
                //}
                //catch (Exception ex)
                //{
                //}
                //finally
                //{
                //    mail2.Dispose();
                //    smtpClient2.Dispose();
                //}
            }
            return email;
        }

        public string RegiserCallDelete(VenderCallRegisterModel model)
        {
            string msg = "";
            var CallCancalltion = context.T19CallCancels.FirstOrDefault(x => x.CaseNo == model.CaseNo && x.CallRecvDt == model.CallRecvDt && x.CallSno == model.CallSno);
            var CallDetails = context.T18CallDetails.FirstOrDefault(x => x.CaseNo == model.CaseNo && x.CallRecvDt == model.CallRecvDt && x.CallSno == model.CallSno);
            var CallReg = context.T17CallRegisters.FirstOrDefault(x => x.CaseNo == model.CaseNo && x.CallRecvDt == model.CallRecvDt && x.CallSno == model.CallSno);

            //int T47Count = context.T47IeWorkPlans.Count(x => x.CaseNo == model.CaseNo && x.CallRecvDt == model.CallRecvDt && x.CallSno == model.CallSno);
            var T47 = context.T47IeWorkPlans.FirstOrDefault(x => x.CaseNo == model.CaseNo && x.CallRecvDt == model.CallRecvDt && x.CallSno == model.CallSno);

            int delstatus = context.T20Ics.Where(x => x.CaseNo == model.CaseNo && x.CallRecvDt == model.CallRecvDt && x.CallSno == model.CallSno).Count();

            if (delstatus == 0)
            {
                if (CallCancalltion != null)
                {
                    context.T19CallCancels.Remove(CallCancalltion);
                    context.SaveChanges();

                    msg = CallCancalltion.CaseNo;
                }
                if (CallDetails != null)
                {
                    //context.T18CallDetails.Remove(CallDetails);
                    //context.SaveChanges();
                    if (context.T18CallDetails.Any(x => x.CaseNo == model.CaseNo && x.CallRecvDt == model.CallRecvDt && x.CallSno == model.CallSno))
                    {
                        context.T18CallDetails.RemoveRange(context.T18CallDetails.Where(x => x.CaseNo == model.CaseNo && x.CallRecvDt == model.CallRecvDt && x.CallSno == model.CallSno).ToList());
                        context.SaveChanges();
                    }
                }
                if (T47 != null)
                {
                    if (context.T47IeWorkPlans.Any(x => x.CaseNo == model.CaseNo && x.CallRecvDt == model.CallRecvDt && x.CallSno == model.CallSno))
                    {
                        context.T47IeWorkPlans.RemoveRange(context.T47IeWorkPlans.Where(x => x.CaseNo == model.CaseNo && x.CallRecvDt == model.CallRecvDt && x.CallSno == model.CallSno).ToList());
                        context.SaveChanges();
                    }
                    //context.T47IeWorkPlans.Remove(T47);
                    //context.SaveChanges();
                }
                if (CallReg != null)
                {
                    context.T17CallRegisters.Remove(CallReg);
                    context.SaveChanges();

                    msg = CallReg.CaseNo;
                }
            }
            return msg;
        }

        public VenderCallRegisterModel FindAddDetails(string CaseNo)
        {
            VenderCallRegisterModel model = new();
            var POMaster = (from l in context.T13PoMasters
                            join v in context.T05Vendors on l.VendCd equals (v.VendCd)
                            where l.CaseNo == CaseNo
                            select new VenderCallRegisterModel
                            {
                                InspectingAgency = l.InspectingAgency,
                                Remarks = l.Remarks,
                                VendInspStopped = v.VendInspStopped,
                                VendRemarks = v.VendRemarks,
                                RlyNonrly = l.RlyNonrly,
                                OnlineCallStatus = v.OnlineCallStatus,
                                OfflineCallStatus = v.OfflineCallStatus
                            }).FirstOrDefault();

            if (POMaster == null)
                throw new Exception("Record Not found");
            else
            {
                model.InspectingAgency = POMaster.InspectingAgency == null ? "X" : POMaster.InspectingAgency;
                model.Remarks = POMaster.Remarks;
                model.VendInspStopped = POMaster.VendInspStopped;
                model.VendRemarks = POMaster.VendRemarks;
                model.RlyNonrly = POMaster.RlyNonrly;
                model.OnlineCallStatus = POMaster.OnlineCallStatus;
                model.OfflineCallStatus = POMaster.OfflineCallStatus;
                return model;
            }
        }

        public string GetMatch(string CaseNo, string GetRegionCode)
        {
            var val = "";
            var Data = (from l in context.T13PoMasters
                        where l.CaseNo == CaseNo
                        select new
                        {
                            test = l.RegionCode
                        }).FirstOrDefault();

            if (Data.test == "")
            {
                val = "0";
            }
            else if (Data.test == GetRegionCode)
            {
                val = "2";
            }
            else
            {
                val = "1";
            }
            return val;
        }

        public string GetCaseNoFind(string CaseNo, string CallRecvDt, int CallSno)
        {
            var val = "";
            DateTime _CallRecvDt = DateTime.ParseExact(CallRecvDt, "dd/MM/yyyy", null);
            var Data = (from l in context.T19CallCancels
                        where l.CaseNo == CaseNo && l.CallRecvDt == _CallRecvDt && l.CallSno == CallSno
                        select new
                        {
                            CaseNo = l.CaseNo
                        }).FirstOrDefault();

            if (Data == null)
            {
                val = "";
            }
            else
            {
                val = Data.CaseNo;
            }
            return val;
        }

        public int show2(string CaseNo)
        {
            int val = 0;
            string ext_delv_dt = null;
            string INSP_DATE = "";
            var result = context.T15PoDetails.Where(x => x.CaseNo == CaseNo).ToList() // Retrieve data from the database into memory
                        .Select(l => new
                        {
                            ExtDelvDt = l.ExtDelvDt != null ? l.ExtDelvDt.Value.ToString("dd/MM/yyyy") : "01/01/2001"
                        }).OrderByDescending(l => l.ExtDelvDt).FirstOrDefault();

            if (result == null)
            {
                ext_delv_dt = "01/01/2001";
            }
            else
            {
                ext_delv_dt = result.ExtDelvDt;
            }

            INSP_DATE = Convert.ToString(DateTime.Now.Date);
            if (ext_delv_dt == "01/01/2001")
            {
                val = 2;
            }
            else
            {
                System.DateTime w_dt1 = new System.DateTime(Convert.ToInt32(ext_delv_dt.Substring(6, 4)), Convert.ToInt32(ext_delv_dt.Substring(3, 2)), Convert.ToInt32(ext_delv_dt.Substring(0, 2)));
                System.DateTime w_dt2 = new System.DateTime(Convert.ToInt32(INSP_DATE.ToString().Substring(6, 4)), Convert.ToInt32(INSP_DATE.ToString().Substring(3, 2)), Convert.ToInt32(INSP_DATE.ToString().Substring(0, 2)));
                TimeSpan ts = w_dt1 - w_dt2;
                int differenceInDays = ts.Days;
                if (differenceInDays < 5)
                {
                    val = 0;
                }
                else
                {
                    val = 1;
                }
            }
            return val;
        }

        public VenderCallCancellationModel CancelCallFindByID(string CaseNo, string CallRecvDt, int CallSno, string ActionType)
        {
            VenderCallCancellationModel model = new();
            DateTime? _CallRecvDt = CallRecvDt == "" ? null : DateTime.ParseExact(CallRecvDt, "dd/MM/yyyy", null);
            var CallData = (from p in context.T13PoMasters
                            join v in context.T05Vendors on p.VendCd equals v.VendCd
                            where p.CaseNo == CaseNo
                            select new
                            {
                                PoNo = p.PoNo,
                                PoDt = p.PoDt,
                                Vendor = v.VendName
                            }).FirstOrDefault();

            var GetData = (from p in context.T17CallRegisters
                           join v in context.T09Ies on p.IeCd equals v.IeCd
                           where p.CaseNo == CaseNo && p.CallRecvDt == _CallRecvDt && p.CallSno == CallSno
                           select new
                           {
                               CaseNo = p.CaseNo,
                               CallRecvDt = p.CallRecvDt,
                               CallSno = p.CallSno,
                               IeSname = v.IeSname
                           }).FirstOrDefault();

            if (CallData == null)
                throw new Exception("Record Not found");
            else
            {
                model.PoNo = CallData.PoNo;
                model.PoDt = CallData.PoDt;
                model.Vendor = CallData.Vendor;
                if (GetData != null)
                {
                    model.CaseNo = GetData.CaseNo;
                    model.CallRecvDt = GetData.CallRecvDt;
                    model.CallSno = (short)GetData.CallSno;
                    model.IeSname = GetData.IeSname;
                }
            }

            return model;
        }

        public VenderCallCancellationModel CancelCallFindByIDM(string CaseNo, string CallRecvDt, int CallSno, string ActionType)
        {
            VenderCallCancellationModel model = new();
            DateTime? _CallRecvDt = CallRecvDt == "" ? null : DateTime.ParseExact(CallRecvDt, "dd/MM/yyyy", null);

            var CancelData = (from l in context.T19CallCancels
                              join c in context.T17CallRegisters on new { l.CaseNo, l.CallSno, l.CallRecvDt } equals new { c.CaseNo, c.CallSno, c.CallRecvDt }
                              where l.CaseNo == CaseNo && l.CallRecvDt == _CallRecvDt && l.CallSno == CallSno
                              select new VenderCallCancellationModel
                              {
                                  CaseNo = l.CaseNo,
                                  CallRecvDt = l.CallRecvDt,
                                  CallSno = (short)l.CallSno,
                                  Cdesc = l.CancelDesc,
                                  CancelDt = l.CancelDate,
                                  DocRec = l.DocsSubmitted,
                                  CallCancelStatus = c.CallCancelStatus,
                                  chk1 = Convert.ToInt32(l.CancelCd1),
                                  chk2 = Convert.ToInt32(l.CancelCd2),
                                  chk3 = Convert.ToInt32(l.CancelCd3),
                                  chk4 = Convert.ToInt32(l.CancelCd4),
                                  chk5 = Convert.ToInt32(l.CancelCd5),
                                  chk6 = Convert.ToInt32(l.CancelCd6),
                                  chk7 = Convert.ToInt32(l.CancelCd7),
                                  chk8 = Convert.ToInt32(l.CancelCd8),
                                  chk9 = Convert.ToInt32(l.CancelCd9),
                                  chk10 = Convert.ToInt32(l.CancelCd10),
                                  chk11 = Convert.ToInt32(l.CancelCd11),
                                  chk12 = Convert.ToInt32(l.CancelCd12),
                              }).FirstOrDefault();

            var CallData = (from p in context.T13PoMasters
                            join v in context.T05Vendors on p.VendCd equals v.VendCd
                            where p.CaseNo == CaseNo
                            select new
                            {
                                PoNo = p.PoNo,
                                PoDt = p.PoDt,
                                Vendor = v.VendName
                            }).FirstOrDefault();

            var GetData = (from p in context.T17CallRegisters
                           join v in context.T09Ies on p.IeCd equals v.IeCd
                           where p.CaseNo == CaseNo && p.CallRecvDt == _CallRecvDt && p.CallSno == CallSno
                           select new
                           {
                               CaseNo = p.CaseNo,
                               CallRecvDt = p.CallRecvDt,
                               CallSno = p.CallSno,
                               IeSname = v.IeSname
                           }).FirstOrDefault();

            if (CancelData == null)
                throw new Exception("Record Not found");
            else
            {
                model.CaseNo = CancelData.CaseNo;
                model.CallRecvDt = CancelData.CallRecvDt;
                model.CallSno = CancelData.CallSno;
                model.Cdesc = CancelData.Cdesc;
                model.CancelDt = CancelData.CancelDt;
                model.DocRec = CancelData.DocRec;
                model.CallCancelStatus = CancelData.CallCancelStatus;

                bool[] chk = new bool[12];

                for (int i = 1; i <= 12; i++)
                {
                    if (CancelData.chk1 == i)
                    {
                        chk[i - 1] = true;
                    }
                    else if (CancelData.chk2 == i)
                    {
                        chk[i - 1] = true;
                    }
                    else if (CancelData.chk3 == i)
                    {
                        chk[i - 1] = true;
                    }
                    else if (CancelData.chk4 == i)
                    {
                        chk[i - 1] = true;
                    }
                    else if (CancelData.chk5 == i)
                    {
                        chk[i - 1] = true;
                    }
                    else if (CancelData.chk6 == i)
                    {
                        chk[i - 1] = true;
                    }
                    else if (CancelData.chk7 == i)
                    {
                        chk[i - 1] = true;
                    }
                    else if (CancelData.chk8 == i)
                    {
                        chk[i - 1] = true;
                    }
                    else if (CancelData.chk9 == i)
                    {
                        chk[i - 1] = true;
                    }
                    else if (CancelData.chk10 == i)
                    {
                        chk[i - 1] = true;
                    }
                    else if (CancelData.chk11 == i)
                    {
                        chk[i - 1] = true;
                    }
                    else if (CancelData.chk12 == i)
                    {
                        chk[i - 1] = true;
                    }

                }

                model.chkItems = chk;

                if (CallData != null)
                {
                    model.PoNo = CallData.PoNo;
                    model.PoDt = CallData.PoDt;
                    model.Vendor = CallData.Vendor;
                }
                if (GetData != null)
                {
                    model.CaseNo = GetData.CaseNo;
                    model.CallRecvDt = GetData.CallRecvDt;
                    model.CallSno = (short)GetData.CallSno;
                    model.IeSname = GetData.IeSname;
                }
            }

            return model;
        }

        int[] reasons(VenderCallCancellationModel model)
        {
            int[] chk = new int[11];
            int j = 0;

            for (int i = 0; i < 1; i++)
            {
                if (model.chk_1 == true)
                {
                    chk[j] = Convert.ToInt32(model.chk1);
                    j++;
                }
                if (model.chk_2 == true)
                {
                    chk[j] = Convert.ToInt32(model.chk2);
                    j++;
                }
                if (model.chk_3 == true)
                {
                    chk[j] = Convert.ToInt32(model.chk3);
                    j++;
                }
                if (model.chk_4 == true)
                {
                    chk[j] = Convert.ToInt32(model.chk4);
                    j++;
                }
                if (model.chk_5 == true)
                {
                    chk[j] = Convert.ToInt32(model.chk5);
                    j++;
                }
                if (model.chk_6 == true)
                {
                    chk[j] = Convert.ToInt32(model.chk6);
                    j++;
                }
                if (model.chk_7 == true)
                {
                    chk[j] = Convert.ToInt32(model.chk7);
                    j++;
                }
                if (model.chk_8 == true)
                {
                    chk[j] = Convert.ToInt32(model.chk8);
                    j++;
                }
                if (model.chk_9 == true)
                {
                    chk[j] = Convert.ToInt32(model.chk9);
                    j++;
                }
                if (model.chk_10 == true)
                {
                    chk[j] = Convert.ToInt32(model.chk10);
                    j++;
                }
                if (model.chk_11 == true)
                {
                    chk[j] = Convert.ToInt32(model.chk11);
                    j++;
                }
                if (model.chk_12 == true)
                {
                    chk[j] = Convert.ToInt32(model.chk12);
                    j++;
                }
            }
            return (chk);
        }

        public string CallCancellationSave(VenderCallCancellationModel model, string UserName)
        {
            string ID = "";
            //DateTime? _CallRecvDt = CallRecvDt == "" ? null : DateTime.ParseExact(CallRecvDt, "dd/MM/yyyy", null);
            var CallCancalltion = context.T19CallCancels.FirstOrDefault(x => x.CaseNo == model.CaseNo && x.CallRecvDt == model.CallRecvDt && x.CallSno == model.CallSno);

            //int[] chk1 = reasons(model);

            if (model.ActionType == "A")
            {
                if (CallCancalltion == null)
                {
                    T19CallCancel obj = new T19CallCancel();
                    obj.CaseNo = model.CaseNo;
                    obj.CallRecvDt = Convert.ToDateTime(model.CallRecvDt);
                    obj.CallSno = (int)model.CallSno;
                    obj.CancelDesc = model.Cdesc;
                    obj.CancelDate = model.CancelDt;
                    obj.UserId = model.Createdby;
                    obj.Datetime = DateTime.Now.Date;
                    obj.DocsSubmitted = model.DocRec;

                    obj.Createdby = UserName;
                    obj.Createddate = DateTime.Now.Date;


                    obj.CancelCd1 = 0;
                    obj.CancelCd2 = 0;
                    obj.CancelCd3 = 0;
                    obj.CancelCd4 = 0;
                    obj.CancelCd5 = 0;
                    obj.CancelCd6 = 0;
                    obj.CancelCd7 = 0;
                    obj.CancelCd8 = 0;
                    obj.CancelCd9 = 0;
                    obj.CancelCd10 = 0;
                    obj.CancelCd11 = 0;
                    obj.CancelCd12 = 0;

                    var indexes = model.chkItems.Select((v, i) => new { v, i }).Where(x => x.v == true).Select(x => x.i);
                    int count = indexes.Count();

                    if (count == 1)
                    {
                        obj.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                    }
                    else if (count == 2)
                    {
                        obj.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                        obj.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                    }
                    else if (count == 3)
                    {
                        obj.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                        obj.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                        obj.CancelCd3 = Convert.ToByte(indexes.ElementAt(2) + 1);
                    }
                    else if (count == 4)
                    {
                        obj.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                        obj.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                        obj.CancelCd3 = Convert.ToByte(indexes.ElementAt(2) + 1);
                        obj.CancelCd4 = Convert.ToByte(indexes.ElementAt(3) + 1);
                    }
                    else if (count == 5)
                    {
                        obj.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                        obj.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                        obj.CancelCd3 = Convert.ToByte(indexes.ElementAt(2) + 1);
                        obj.CancelCd4 = Convert.ToByte(indexes.ElementAt(3) + 1);
                        obj.CancelCd5 = Convert.ToByte(indexes.ElementAt(4) + 1);
                    }
                    else if (count == 6)
                    {
                        obj.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                        obj.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                        obj.CancelCd3 = Convert.ToByte(indexes.ElementAt(2) + 1);
                        obj.CancelCd4 = Convert.ToByte(indexes.ElementAt(3) + 1);
                        obj.CancelCd5 = Convert.ToByte(indexes.ElementAt(4) + 1);
                        obj.CancelCd6 = Convert.ToByte(indexes.ElementAt(5) + 1);
                    }
                    else if (count == 7)
                    {
                        obj.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                        obj.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                        obj.CancelCd3 = Convert.ToByte(indexes.ElementAt(2) + 1);
                        obj.CancelCd4 = Convert.ToByte(indexes.ElementAt(3) + 1);
                        obj.CancelCd5 = Convert.ToByte(indexes.ElementAt(4) + 1);
                        obj.CancelCd6 = Convert.ToByte(indexes.ElementAt(5) + 1);
                        obj.CancelCd7 = Convert.ToByte(indexes.ElementAt(6) + 1);
                    }
                    else if (count == 8)
                    {
                        obj.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                        obj.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                        obj.CancelCd3 = Convert.ToByte(indexes.ElementAt(2) + 1);
                        obj.CancelCd4 = Convert.ToByte(indexes.ElementAt(3) + 1);
                        obj.CancelCd5 = Convert.ToByte(indexes.ElementAt(4) + 1);
                        obj.CancelCd6 = Convert.ToByte(indexes.ElementAt(5) + 1);
                        obj.CancelCd7 = Convert.ToByte(indexes.ElementAt(6) + 1);
                        obj.CancelCd8 = Convert.ToByte(indexes.ElementAt(7) + 1);
                    }
                    else if (count == 9)
                    {
                        obj.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                        obj.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                        obj.CancelCd3 = Convert.ToByte(indexes.ElementAt(2) + 1);
                        obj.CancelCd4 = Convert.ToByte(indexes.ElementAt(3) + 1);
                        obj.CancelCd5 = Convert.ToByte(indexes.ElementAt(4) + 1);
                        obj.CancelCd6 = Convert.ToByte(indexes.ElementAt(5) + 1);
                        obj.CancelCd7 = Convert.ToByte(indexes.ElementAt(6) + 1);
                        obj.CancelCd8 = Convert.ToByte(indexes.ElementAt(7) + 1);
                        obj.CancelCd9 = Convert.ToByte(indexes.ElementAt(8) + 1);
                    }
                    else if (count == 10)
                    {
                        obj.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                        obj.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                        obj.CancelCd3 = Convert.ToByte(indexes.ElementAt(2) + 1);
                        obj.CancelCd4 = Convert.ToByte(indexes.ElementAt(3) + 1);
                        obj.CancelCd5 = Convert.ToByte(indexes.ElementAt(4) + 1);
                        obj.CancelCd6 = Convert.ToByte(indexes.ElementAt(5) + 1);
                        obj.CancelCd7 = Convert.ToByte(indexes.ElementAt(6) + 1);
                        obj.CancelCd8 = Convert.ToByte(indexes.ElementAt(7) + 1);
                        obj.CancelCd9 = Convert.ToByte(indexes.ElementAt(8) + 1);
                        obj.CancelCd10 = Convert.ToByte(indexes.ElementAt(9) + 1);
                    }
                    else if (count == 11)
                    {
                        obj.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                        obj.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                        obj.CancelCd3 = Convert.ToByte(indexes.ElementAt(2) + 1);
                        obj.CancelCd4 = Convert.ToByte(indexes.ElementAt(3) + 1);
                        obj.CancelCd5 = Convert.ToByte(indexes.ElementAt(4) + 1);
                        obj.CancelCd6 = Convert.ToByte(indexes.ElementAt(5) + 1);
                        obj.CancelCd7 = Convert.ToByte(indexes.ElementAt(6) + 1);
                        obj.CancelCd8 = Convert.ToByte(indexes.ElementAt(7) + 1);
                        obj.CancelCd9 = Convert.ToByte(indexes.ElementAt(8) + 1);
                        obj.CancelCd10 = Convert.ToByte(indexes.ElementAt(9) + 1);
                        obj.CancelCd11 = Convert.ToByte(indexes.ElementAt(10) + 1);
                    }
                    else if (count == 12)
                    {
                        obj.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                        obj.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                        obj.CancelCd3 = Convert.ToByte(indexes.ElementAt(2) + 1);
                        obj.CancelCd4 = Convert.ToByte(indexes.ElementAt(3) + 1);
                        obj.CancelCd5 = Convert.ToByte(indexes.ElementAt(4) + 1);
                        obj.CancelCd6 = Convert.ToByte(indexes.ElementAt(5) + 1);
                        obj.CancelCd7 = Convert.ToByte(indexes.ElementAt(6) + 1);
                        obj.CancelCd8 = Convert.ToByte(indexes.ElementAt(7) + 1);
                        obj.CancelCd9 = Convert.ToByte(indexes.ElementAt(8) + 1);
                        obj.CancelCd10 = Convert.ToByte(indexes.ElementAt(9) + 1);
                        obj.CancelCd11 = Convert.ToByte(indexes.ElementAt(10) + 1);
                        obj.CancelCd12 = Convert.ToByte(indexes.ElementAt(11) + 1);
                    }

                    context.T19CallCancels.Add(obj);
                    context.SaveChanges();
                    ID = obj.CaseNo;
                }
            }
            else if (model.ActionType == "M")
            {
                if (CallCancalltion != null)
                {
                    CallCancalltion.CancelCd1 = 0;
                    CallCancalltion.CancelCd2 = 0;
                    CallCancalltion.CancelCd3 = 0;
                    CallCancalltion.CancelCd4 = 0;
                    CallCancalltion.CancelCd5 = 0;
                    CallCancalltion.CancelCd6 = 0;
                    CallCancalltion.CancelCd7 = 0;
                    CallCancalltion.CancelCd8 = 0;
                    CallCancalltion.CancelCd9 = 0;
                    CallCancalltion.CancelCd10 = 0;
                    CallCancalltion.CancelCd11 = 0;
                    CallCancalltion.CancelCd12 = 0;

                    var indexes = model.chkItems.Select((v, i) => new { v, i }).Where(x => x.v == true).Select(x => x.i);
                    int count = indexes.Count();

                    if (count == 1)
                    {
                        CallCancalltion.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                    }
                    else if (count == 2)
                    {
                        CallCancalltion.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                        CallCancalltion.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                    }
                    else if (count == 3)
                    {
                        CallCancalltion.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                        CallCancalltion.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                        CallCancalltion.CancelCd3 = Convert.ToByte(indexes.ElementAt(2) + 1);
                    }
                    else if (count == 4)
                    {
                        CallCancalltion.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                        CallCancalltion.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                        CallCancalltion.CancelCd3 = Convert.ToByte(indexes.ElementAt(2) + 1);
                        CallCancalltion.CancelCd4 = Convert.ToByte(indexes.ElementAt(3) + 1);
                    }
                    else if (count == 5)
                    {
                        CallCancalltion.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                        CallCancalltion.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                        CallCancalltion.CancelCd3 = Convert.ToByte(indexes.ElementAt(2) + 1);
                        CallCancalltion.CancelCd4 = Convert.ToByte(indexes.ElementAt(3) + 1);
                        CallCancalltion.CancelCd5 = Convert.ToByte(indexes.ElementAt(4) + 1);
                    }
                    else if (count == 6)
                    {
                        CallCancalltion.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                        CallCancalltion.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                        CallCancalltion.CancelCd3 = Convert.ToByte(indexes.ElementAt(2) + 1);
                        CallCancalltion.CancelCd4 = Convert.ToByte(indexes.ElementAt(3) + 1);
                        CallCancalltion.CancelCd5 = Convert.ToByte(indexes.ElementAt(4) + 1);
                        CallCancalltion.CancelCd6 = Convert.ToByte(indexes.ElementAt(5) + 1);
                    }
                    else if (count == 7)
                    {
                        CallCancalltion.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                        CallCancalltion.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                        CallCancalltion.CancelCd3 = Convert.ToByte(indexes.ElementAt(2) + 1);
                        CallCancalltion.CancelCd4 = Convert.ToByte(indexes.ElementAt(3) + 1);
                        CallCancalltion.CancelCd5 = Convert.ToByte(indexes.ElementAt(4) + 1);
                        CallCancalltion.CancelCd6 = Convert.ToByte(indexes.ElementAt(5) + 1);
                        CallCancalltion.CancelCd7 = Convert.ToByte(indexes.ElementAt(6) + 1);
                    }
                    else if (count == 8)
                    {
                        CallCancalltion.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                        CallCancalltion.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                        CallCancalltion.CancelCd3 = Convert.ToByte(indexes.ElementAt(2) + 1);
                        CallCancalltion.CancelCd4 = Convert.ToByte(indexes.ElementAt(3) + 1);
                        CallCancalltion.CancelCd5 = Convert.ToByte(indexes.ElementAt(4) + 1);
                        CallCancalltion.CancelCd6 = Convert.ToByte(indexes.ElementAt(5) + 1);
                        CallCancalltion.CancelCd7 = Convert.ToByte(indexes.ElementAt(6) + 1);
                        CallCancalltion.CancelCd8 = Convert.ToByte(indexes.ElementAt(7) + 1);
                    }
                    else if (count == 9)
                    {
                        CallCancalltion.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                        CallCancalltion.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                        CallCancalltion.CancelCd3 = Convert.ToByte(indexes.ElementAt(2) + 1);
                        CallCancalltion.CancelCd4 = Convert.ToByte(indexes.ElementAt(3) + 1);
                        CallCancalltion.CancelCd5 = Convert.ToByte(indexes.ElementAt(4) + 1);
                        CallCancalltion.CancelCd6 = Convert.ToByte(indexes.ElementAt(5) + 1);
                        CallCancalltion.CancelCd7 = Convert.ToByte(indexes.ElementAt(6) + 1);
                        CallCancalltion.CancelCd8 = Convert.ToByte(indexes.ElementAt(7) + 1);
                        CallCancalltion.CancelCd9 = Convert.ToByte(indexes.ElementAt(8) + 1);
                    }
                    else if (count == 10)
                    {
                        CallCancalltion.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                        CallCancalltion.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                        CallCancalltion.CancelCd3 = Convert.ToByte(indexes.ElementAt(2) + 1);
                        CallCancalltion.CancelCd4 = Convert.ToByte(indexes.ElementAt(3) + 1);
                        CallCancalltion.CancelCd5 = Convert.ToByte(indexes.ElementAt(4) + 1);
                        CallCancalltion.CancelCd6 = Convert.ToByte(indexes.ElementAt(5) + 1);
                        CallCancalltion.CancelCd7 = Convert.ToByte(indexes.ElementAt(6) + 1);
                        CallCancalltion.CancelCd8 = Convert.ToByte(indexes.ElementAt(7) + 1);
                        CallCancalltion.CancelCd9 = Convert.ToByte(indexes.ElementAt(8) + 1);
                        CallCancalltion.CancelCd10 = Convert.ToByte(indexes.ElementAt(9) + 1);
                    }
                    else if (count == 11)
                    {
                        CallCancalltion.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                        CallCancalltion.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                        CallCancalltion.CancelCd3 = Convert.ToByte(indexes.ElementAt(2) + 1);
                        CallCancalltion.CancelCd4 = Convert.ToByte(indexes.ElementAt(3) + 1);
                        CallCancalltion.CancelCd5 = Convert.ToByte(indexes.ElementAt(4) + 1);
                        CallCancalltion.CancelCd6 = Convert.ToByte(indexes.ElementAt(5) + 1);
                        CallCancalltion.CancelCd7 = Convert.ToByte(indexes.ElementAt(6) + 1);
                        CallCancalltion.CancelCd8 = Convert.ToByte(indexes.ElementAt(7) + 1);
                        CallCancalltion.CancelCd9 = Convert.ToByte(indexes.ElementAt(8) + 1);
                        CallCancalltion.CancelCd10 = Convert.ToByte(indexes.ElementAt(9) + 1);
                        CallCancalltion.CancelCd11 = Convert.ToByte(indexes.ElementAt(10) + 1);
                    }
                    else if (count == 12)
                    {
                        CallCancalltion.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                        CallCancalltion.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                        CallCancalltion.CancelCd3 = Convert.ToByte(indexes.ElementAt(2) + 1);
                        CallCancalltion.CancelCd4 = Convert.ToByte(indexes.ElementAt(3) + 1);
                        CallCancalltion.CancelCd5 = Convert.ToByte(indexes.ElementAt(4) + 1);
                        CallCancalltion.CancelCd6 = Convert.ToByte(indexes.ElementAt(5) + 1);
                        CallCancalltion.CancelCd7 = Convert.ToByte(indexes.ElementAt(6) + 1);
                        CallCancalltion.CancelCd8 = Convert.ToByte(indexes.ElementAt(7) + 1);
                        CallCancalltion.CancelCd9 = Convert.ToByte(indexes.ElementAt(8) + 1);
                        CallCancalltion.CancelCd10 = Convert.ToByte(indexes.ElementAt(9) + 1);
                        CallCancalltion.CancelCd11 = Convert.ToByte(indexes.ElementAt(10) + 1);
                        CallCancalltion.CancelCd12 = Convert.ToByte(indexes.ElementAt(11) + 1);
                    }
                    CallCancalltion.CancelDesc = model.Cdesc;
                    CallCancalltion.UserId = model.Createdby;
                    CallCancalltion.Datetime = DateTime.Now.Date;
                    CallCancalltion.DocsSubmitted = model.DocRec;
                    CallCancalltion.Updatedby = UserName;
                    CallCancalltion.Updateddate = DateTime.Now.Date;

                    context.SaveChanges();
                    ID = model.CaseNo;
                }
            }

            var CallReg = context.T17CallRegisters.FirstOrDefault(x => x.CaseNo == model.CaseNo && x.CallRecvDt == model.CallRecvDt && x.CallSno == model.CallSno);
            if (CallReg != null)
            {
                CallReg.CallStatus = "C";
                CallReg.CallCancelStatus = model.CallCancelStatus;

                context.SaveChanges();
            }
            //if (CallReg.CallStatus == "C")
            //{
            //    Cancellation_Email(model);
            //}

            return ID;
        }

        public string CallCancelDelete(string CaseNo, string CallRecvDt, int CallSno)
        {
            string msg = "";

            var CallCancalltion = context.T19CallCancels.FirstOrDefault(x => x.CaseNo == CaseNo && x.CallRecvDt == Convert.ToDateTime(CallRecvDt) && x.CallSno == CallSno);
            var CallReg = context.T17CallRegisters.FirstOrDefault(x => x.CaseNo == CaseNo && x.CallRecvDt == Convert.ToDateTime(CallRecvDt) && x.CallSno == CallSno);


            if (CallCancalltion != null)
            {
                context.T19CallCancels.Remove(CallCancalltion);
                context.SaveChanges();

                msg = CallCancalltion.CaseNo;
            }
            if (CallReg != null)
            {
                CallReg.CallStatus = "M";
                context.SaveChanges();
            }

            return msg;
        }

        public VenderCallStatusModel FindCallStatus(string CaseNo, DateTime? CallRecvDt, int CallSno, int IE_CD)
        {
            VenderCallStatusModel model = new();
            //DateTime? _CallRecvDt = CallRecvDt == null ? null : DateTime.ParseExact(CallRecvDt, "dd-MM-yyyy", null);

            try
            {
                var Status = context.ViewGetCallStatusDetails.Where(x => x.CaseNo == CaseNo && x.CallRecvDt == CallRecvDt && x.CallSno == Convert.ToInt32(CallSno)).FirstOrDefault();

                if (IE_CD > 0)
                {
                    var ic_book = (from item in context.T10IcBooksets
                                   orderby item.IssueDt descending
                                   where item.IssueToIecd == IE_CD
                                   select item).FirstOrDefault();
                    if (ic_book != null)
                    {
                        var dlt_IC = (from x in context.IcIntermediates
                                      orderby x.SetNo descending
                                      where x.BkNo.Trim() == ic_book.BkNo.Trim() && x.IeCd == IE_CD
                                      select x).FirstOrDefault();

                        if (dlt_IC != null)
                        {
                            int setNo = Convert.ToInt32(dlt_IC.SetNo) + 1;

                            string incrementedSetNo = setNo.ToString("D3");

                            var ic_bookset = (from item in context.T10IcBooksets
                                              orderby item.IssueDt descending
                                              where item.BkNo.Trim().ToUpper() == dlt_IC.BkNo &&
                                                    Convert.ToInt32(incrementedSetNo) >= Convert.ToInt32(item.SetNoFr) && Convert.ToInt32(incrementedSetNo) <= Convert.ToInt32(item.SetNoTo) &&
                                                    item.IssueToIecd == dlt_IC.IeCd
                                              select item).FirstOrDefault();

                            if (ic_bookset != null)
                            {
                                model.DocBkNo = ic_bookset.BkNo;
                                model.DocSetNo = Convert.ToString(incrementedSetNo);
                            }
                            else
                            {
                                model.DocBkNo = "";
                                model.DocSetNo = "";
                            }
                        }
                        else
                        {
                            model.DocBkNo = ic_book.BkNo;
                            model.DocSetNo = Convert.ToString(ic_book.SetNoFr);
                        }
                    }
                }
                DateTime CallStatusDt = DateTime.Now.Date;
                if (Status == null)
                    return model;
                else
                {
                    model.VendName = Status.VendName;
                    model.Consignee = Status.Consignee;
                    model.ItemDescPo = Status.ItemDescPo;
                    model.CallRecvDt = Status.CallRecvDt;
                    model.IeName = Status.IeName;
                    model.IePhoneNo = Status.IePhoneNo;
                    model.PoNo = Status.PoNo;
                    model.PoDt = Status.PoDt;
                    model.CaseNo = Status.CaseNo;
                    model.MfgPers = Status.MfgPers;
                    model.MfgPhone = Status.MfgPhone;
                    model.CallSno = Convert.ToInt16(Status.CallSno);
                    model.CallStatus1 = Status.CallStatus1;
                    model.CallStatus = Status.CallStatus;
                    model.UpdateAllowed = Status.UpdateAllowed;
                    model.CallCancelStatus = Status.CallCancelStatus;
                    model.BkNo = Status.BkNo;
                    model.SetNo = Status.SetNo;
                    model.DesireDt = Status.DesireDt;
                    model.CallStatusDt = CallStatusDt;
                    model.CallLetterDt = Convert.ToDateTime(Status.CallLetterDt);
                    model.CallCancelChargesStatus = Status.CallCancelChargesStatus;
                    model.CallCancelAmount = Status.CallCancelAmount;
                    model.RlyNonrly = Status.RlyNonrly;
                    model.RejectionCharge = Convert.ToString(Status.RejCharges);
                    model.LocalOutstation = Convert.ToString(Status.LocalOrOuts);
                }

                if (string.IsNullOrEmpty(model.BkNo) && string.IsNullOrEmpty(model.SetNo))
                {
                    var bookdetail = (from item in context.T10IcBooksets
                                      orderby item.IssueDt descending
                                      where item.IssueToIecd == IE_CD
                                      select item).FirstOrDefault();

                    var calldetail = (from x in context.T17CallRegisters
                                      orderby x.SetNo descending
                                      where x.BkNo.Trim() == bookdetail.BkNo.Trim() && x.IeCd == IE_CD
                                      select x).FirstOrDefault();

                    if (calldetail != null)
                    {
                        int setNo = Convert.ToInt32(calldetail.SetNo) + 1;

                        string incrementedSetNo = setNo.ToString("D3");

                        var ic_bookset = (from item in context.T10IcBooksets
                                          orderby item.IssueDt descending
                                          where item.BkNo.Trim().ToUpper() == calldetail.BkNo &&
                                                Convert.ToInt32(incrementedSetNo) >= Convert.ToInt32(item.SetNoFr) && Convert.ToInt32(incrementedSetNo) <= Convert.ToInt32(item.SetNoTo) &&
                                                item.IssueToIecd == calldetail.IeCd
                                          select item).FirstOrDefault();

                        if (ic_bookset != null)
                        {
                            model.BkNo = ic_bookset.BkNo;
                            model.SetNo = Convert.ToString(incrementedSetNo);
                        }
                    }
                }

                if (Status.CallStatus == "M" || Status.CallStatus == "U" || Status.CallStatus == "S")
                {
                    model.Remarks = "";
                    model.Remarkslbl = Convert.ToString(Status.Remarks);
                }
                else
                {
                    model.Remarks = Convert.ToString(Status.Remarks);
                    model.Hologram = Convert.ToString(Status.Hologram);
                }

                string formattedCallRecvDt = "";
                if (CallRecvDt != null && CallRecvDt != DateTime.MinValue)
                {
                    DateTime parsedFromDate = DateTime.ParseExact(CallRecvDt.ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                    formattedCallRecvDt = parsedFromDate.ToString("dd/MM/yyyy");
                }

                var secondQuery = (from cdt in context.T18CallDetails
                                   join csn in context.V06Consignees
                                   on cdt.ConsigneeCd equals csn.ConsigneeCd
                                   where cdt.CaseNo == CaseNo &&
                                         cdt.CallRecvDt == Convert.ToDateTime(formattedCallRecvDt) &&
                                         cdt.CallSno == CallSno
                                   select new SelectListItem
                                   {
                                       Value = csn.ConsigneeCd.ToString(),
                                       Text = csn.ConsigneeCd + "-" + csn.Consignee
                                   }).Distinct().ToList();

                model.ConsigneeFirmList = secondQuery.ToList();

                var CancelData = (from l in context.T19CallCancels
                                  join c in context.T17CallRegisters on new { l.CaseNo, l.CallSno, l.CallRecvDt } equals new { c.CaseNo, c.CallSno, c.CallRecvDt }
                                  where l.CaseNo == CaseNo && l.CallRecvDt == Convert.ToDateTime(formattedCallRecvDt) && l.CallSno == CallSno
                                  select new VenderCallCancellationModel
                                  {
                                      CaseNo = l.CaseNo,
                                      CallRecvDt = l.CallRecvDt,
                                      CallSno = (short)l.CallSno,
                                      Cdesc = l.CancelDesc,
                                      CancelDt = l.CancelDate,
                                      DocRec = l.DocsSubmitted,
                                      CallCancelStatus = c.CallCancelStatus,
                                      chk1 = Convert.ToInt32(l.CancelCd1),
                                      chk2 = Convert.ToInt32(l.CancelCd2),
                                      chk3 = Convert.ToInt32(l.CancelCd3),
                                      chk4 = Convert.ToInt32(l.CancelCd4),
                                      chk5 = Convert.ToInt32(l.CancelCd5),
                                      chk6 = Convert.ToInt32(l.CancelCd6),
                                      chk7 = Convert.ToInt32(l.CancelCd7),
                                      chk8 = Convert.ToInt32(l.CancelCd8),
                                      chk9 = Convert.ToInt32(l.CancelCd9),
                                      chk10 = Convert.ToInt32(l.CancelCd10),
                                      chk11 = Convert.ToInt32(l.CancelCd11),
                                      chk12 = Convert.ToInt32(l.CancelCd12),
                                  }).FirstOrDefault();
                if (CancelData != null)
                {
                    model.CaseNo = CancelData.CaseNo;
                    model.CallRecvDt = CancelData.CallRecvDt;
                    model.CallSno = CancelData.CallSno;
                    model.CancellationDescription = CancelData.Cdesc;
                    model.CallCancelStatus = CancelData.CallCancelStatus;

                    bool[] chk = new bool[12];

                    for (int i = 1; i <= 12; i++)
                    {
                        if (CancelData.chk1 == i)
                        {
                            chk[i - 1] = true;
                        }
                        else if (CancelData.chk2 == i)
                        {
                            chk[i - 1] = true;
                        }
                        else if (CancelData.chk3 == i)
                        {
                            chk[i - 1] = true;
                        }
                        else if (CancelData.chk4 == i)
                        {
                            chk[i - 1] = true;
                        }
                        else if (CancelData.chk5 == i)
                        {
                            chk[i - 1] = true;
                        }
                        else if (CancelData.chk6 == i)
                        {
                            chk[i - 1] = true;
                        }
                        else if (CancelData.chk7 == i)
                        {
                            chk[i - 1] = true;
                        }
                        else if (CancelData.chk8 == i)
                        {
                            chk[i - 1] = true;
                        }
                        else if (CancelData.chk9 == i)
                        {
                            chk[i - 1] = true;
                        }
                        else if (CancelData.chk10 == i)
                        {
                            chk[i - 1] = true;
                        }
                        else if (CancelData.chk11 == i)
                        {
                            chk[i - 1] = true;
                        }
                        else if (CancelData.chk12 == i)
                        {
                            chk[i - 1] = true;
                        }

                    }

                    model.chkItems = chk;

                }
            }
            catch (Exception ex)
            {
                //Common.AddException(ex.ToString(), ex.Message.ToString(), "CallStatus", "CallRegisterIB", 1, GetIPAddress());
            }
            return model;
        }

        public string Save(VenderCallStatusModel model, List<APPDocumentDTO> DocumentsList)
        {
            string w_call_cancel_status = "";
            var wFifoVoilateReason = model.ReasonFIFO;
            var document = "";
            if (DocumentsList != null && DocumentsList.Count > 0)
            {
                document = DocumentsList[0].DocName;
            }

            if (model.CallStatus1 == "C" && model.CallStatus != "C")
            {
                var t19 = (from a in context.T19CallCancels
                           where a.CaseNo == model.CaseNo && a.CallRecvDt == model.CallRecvDt && a.CallSno == model.CallSno
                           select a).FirstOrDefault();
                if (t19 != null)
                {
                    t19.Isdeleted = 1;
                    t19.Updatedby = Convert.ToString(model.UserId);
                    t19.Updateddate = DateTime.Now;
                    context.SaveChanges();
                }
            }
            else if (model.CallStatus == "C" && model.CallCancelStatus == "C")
            {
                w_call_cancel_status = "C";
            }
            else if (model.CallStatus == "C" && model.CallCancelStatus == "N")
            {
                w_call_cancel_status = "N";
            }
            else
            {
                w_call_cancel_status = "";
            }

            if (model.CallStatus == "A" || model.CallStatus == "R")
            {
                string bscheck = null;
                string formattedCallRecvDt = "";

                if (model.CallRecvDt != null && model.CallRecvDt != DateTime.MinValue)
                {
                    DateTime parsedFromDate = DateTime.ParseExact(model.CallRecvDt.ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                    formattedCallRecvDt = parsedFromDate.ToString("dd/MM/yyyy");
                }
                if (model.BkNo != "" && model.SetNo != "")
                {
                    var FinalOrStage = context.T17CallRegisters
                                       .Where(cr => cr.CaseNo == model.CaseNo && cr.CallSno == model.CallSno && cr.CallRecvDt == Convert.ToDateTime(formattedCallRecvDt))
                                       .Select(cr => cr.FinalOrStage)
                                       .FirstOrDefault() ?? "F";

                    var docSetNo = Convert.ToInt32(model.DocSetNo);

                    var bsCheck = (from a in context.T10IcBooksets
                                   where a.BkNo.Trim() == model.DocBkNo.Trim()
                                    && docSetNo >= Convert.ToInt32(a.SetNoFr)
                                   && docSetNo <= Convert.ToInt32(a.SetNoTo)
                                   && a.IssueToIecd == Convert.ToInt32(model.IeCd) && a.Ictype == FinalOrStage
                                   select a.IssueToIecd).FirstOrDefault();
                }

                if (!string.IsNullOrEmpty(model.BkNo.Trim()) && !string.IsNullOrEmpty(model.SetNo.Trim()) && !string.IsNullOrEmpty(bscheck) && !string.IsNullOrEmpty(model.Hologram) && document == "IC Image 1")
                {
                    var count = (from item in context.T49IcPhotoEncloseds
                                 where item.CaseNo == model.CaseNo && item.BkNo == model.BkNo && item.SetNo == model.SetNo
                                 select item).Count();
                    if (count > 0)
                    {
                        using (var trans = context.Database.BeginTransaction())
                        {
                            try
                            {
                                var T17Details = from x in context.T17CallRegisters
                                                 where x.CaseNo == model.CaseNo.Trim() && x.CallRecvDt == DateTime.ParseExact(Convert.ToDateTime(model.CallRecvDt).ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) && x.CallSno == model.CallSno
                                                 select x;
                                if (T17Details.Count() > 0) //!= null)
                                {
                                    if (model.CallStatus == "A" || model.CallStatus == "R")
                                    {
                                        foreach (var row in T17Details)
                                        {
                                            row.CallStatus = model.CallStatus;
                                            row.CallStatusDt = DateTime.ParseExact(Convert.ToDateTime(model.CallStatusDt).ToString("dd/MM/yyyy"), "dd/MM/yyyy", null);
                                            row.CallCancelStatus = null;
                                            row.BkNo = model.BkNo;
                                            row.SetNo = model.SetNo;
                                            if (model.CallStatus == "R")
                                            {
                                                if (!string.IsNullOrEmpty(model.Remarkslbl))
                                                {
                                                    row.Remarks = model.Remarkslbl.Trim() + ", " + model.Remarks.Trim();
                                                }
                                                else
                                                {
                                                    row.Remarks = model.Remarks.Trim();
                                                }
                                            }
                                            row.UserId = model.UserName;
                                            row.Datetime = DateTime.Now;
                                            row.Hologram = model.Hologram;
                                            row.FifoVoilateReason = wFifoVoilateReason;
                                            context.SaveChanges();
                                        }
                                    }
                                    if (model.CallStatus == "R")
                                    {
                                        var recordToUpdate = context.T13PoMasters.Where(x => x.CaseNo == model.CaseNo);
                                        if (recordToUpdate.Count() > 0) //!= null)
                                        {
                                            foreach (var item in recordToUpdate)
                                            {
                                                var pendingCharge = 0;
                                                pendingCharge = item.PendingCharges == null ? 0 + 1 : Convert.ToInt16(item.PendingCharges) + 1;
                                                item.PendingCharges = Convert.ToByte(pendingCharge);
                                                context.SaveChanges();
                                            }
                                        }
                                    }
                                    trans.Commit();
                                }
                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();
                                model.AlertMsg = "Error";
                            }
                        }
                        model.AlertMsg = "Success";
                    }
                    else
                    {
                        model.AlertMsg = "Photos against given Case No, Book No & Set No are not uploaded, So Upload Photos before changing the Call Status to [Aceepted OR Rejection]!!!";
                    }
                }
                else if (!string.IsNullOrEmpty(model.BkNo) && !string.IsNullOrEmpty(model.SetNo) && string.IsNullOrEmpty(bscheck))
                {
                    model.AlertMsg = "Book No. and Set No. specified is not issued to You!!!'";
                }
                else if (string.IsNullOrEmpty(model.BkNo) && string.IsNullOrEmpty(model.SetNo) && string.IsNullOrEmpty(model.Hologram) && document == "")
                {
                    model.AlertMsg = "Book No. , Set No., Holograms OR IC Photo cannot be left blank!!!";
                }
            }
            else if (model.CallStatus == "G" || model.CallStatus == "T")
            {
                string bsCheck = "";
                if (!string.IsNullOrEmpty(model.CallStatus) && !string.IsNullOrEmpty(model.SetNo))
                {
                    bsCheck = context.T10IcBooksets
                                  .Where(bookset => bookset.BkNo.Trim().ToUpper() == model.BkNo
                                  && Convert.ToInt32(model.SetNo) >= Convert.ToInt32(bookset.SetNoFr)
                                  && Convert.ToInt32(model.SetNo) <= Convert.ToInt32(bookset.SetNoTo) && bookset.IssueToIecd == Convert.ToInt32(model.IeCd))
                                  .Select(bookset => Convert.ToString(bookset.IssueToIecd)).FirstOrDefault();

                    var ICTYPE = context.T10IcBooksets
                                .Where(item => item.BkNo == model.BkNo && item.IssueToIecd == Convert.ToInt32(model.IeCd))
                                .Select(item => item.Ictype)
                                .FirstOrDefault();

                    if (ICTYPE == "F")
                    {
                        model.AlertMsg = "This Book number and Set number are Finalized.";
                        return model.AlertMsg;
                    }
                }

                if (!string.IsNullOrEmpty(model.BkNo) && !string.IsNullOrEmpty(model.SetNo) && !string.IsNullOrEmpty(bsCheck) && document != "")
                {
                    var t17Detail = from a in context.T17CallRegisters
                                    where a.CaseNo == model.CaseNo && a.CallRecvDt == DateTime.ParseExact(Convert.ToDateTime(model.CallRecvDt).ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) && a.CallSno == model.CallSno
                                    select a;
                    if (t17Detail.Count() > 0)
                    {
                        foreach (var row in t17Detail)
                        {
                            row.CallStatus = model.CallStatus;
                            row.CallStatusDt = model.CallStatusDt;
                            row.CallCancelStatus = null;
                            row.BkNo = model.BkNo;
                            row.SetNo = model.SetNo;
                            row.UserId = model.UserName;
                            row.Datetime = DateTime.Now;
                            row.FifoVoilateReason = wFifoVoilateReason;
                            context.SaveChanges();
                        }
                        model.AlertMsg = "Success";
                    }
                }
                else if (!string.IsNullOrEmpty(model.BkNo) && !string.IsNullOrEmpty(model.SetNo) && string.IsNullOrEmpty(bsCheck))
                {
                    model.AlertMsg = "Book No. and Set No. specified is not issued to You!!!";
                }
                else if (string.IsNullOrEmpty(model.BkNo) || string.IsNullOrEmpty(model.SetNo) || document != " ")
                {
                    model.AlertMsg = "Book No. , Set No. OR Stage IC Photo cannot be left blank!!!";
                }
            }
            else
            {
                var detail = from a in context.T17CallRegisters
                             where a.CaseNo == model.CaseNo && a.CallRecvDt == DateTime.ParseExact(Convert.ToDateTime(model.CallRecvDt).ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) && a.CallSno == model.CallSno
                             select a;
                if (detail.Count() > 0)
                {
                    foreach (var item in detail)
                    {
                        item.CallStatus = model.CallStatus;
                        item.CallStatusDt = model.CallStatusDt;
                        item.CallCancelStatus = w_call_cancel_status;
                        item.UserId = model.UserName;
                        item.Datetime = DateTime.Now;
                        item.FifoVoilateReason = wFifoVoilateReason;
                        context.SaveChanges();
                    }
                }
                model.AlertMsg = "Success";
            }

            if (model.CallStatus == "C")
            {
                if (string.IsNullOrEmpty(model.CallCancelStatus) || (model.CallCancelStatus == "C" && string.IsNullOrEmpty(model.CallCancelCharges)))
                {
                    model.AlertMsg = "Mention Call Chargeable/Call Non-Chargeable & Select One of the Given Call Cancellation Charges in Case the Call is Chargeable!!!";
                }
                else
                {
                    wFifoVoilateReason = "";
                    if (!string.IsNullOrEmpty(model.ChkFIFO))
                    {
                        wFifoVoilateReason = model.ReasonFIFO;
                    }

                    var CCd = (from x in context.T20Ics
                               where x.CaseNo == model.CaseNo && x.CallRecvDt == model.CallRecvDt && x.CallSno == model.CallSno
                               select x.CaseNo).FirstOrDefault();

                    var Action = (from x in context.T19CallCancels
                                  where x.CaseNo == model.CaseNo && x.CallRecvDt == model.CallRecvDt && x.CallSno == model.CallSno
                                  select x.CaseNo).FirstOrDefault();

                    var w_IE_EMPNO = (from x in context.T09Ies
                                      where x.IeCd == Convert.ToInt32(model.IeCd) // Request Parameter
                                      select x.IeEmpNo).FirstOrDefault();

                    if (string.IsNullOrEmpty(CCd))
                    {
                        using (var trans = context.Database.BeginTransaction())
                        {
                            try
                            {
                                if (string.IsNullOrEmpty(Action))
                                {
                                    T19CallCancel obj = new T19CallCancel();
                                    obj.CaseNo = model.CaseNo;
                                    obj.CallRecvDt = Convert.ToDateTime(model.CallRecvDt);
                                    obj.CallSno = Convert.ToInt32(model.CallSno);
                                    obj.CancelDesc = model.CancellationDescription;
                                    obj.UserId = w_IE_EMPNO; //model.UserId;
                                    obj.Datetime = DateTime.Now.Date;
                                    obj.Createdby = Convert.ToString(model.UserId);
                                    obj.Createddate = DateTime.Now.Date;
                                    obj.CancelCd1 = 0;
                                    obj.CancelCd2 = 0;
                                    obj.CancelCd3 = 0;
                                    obj.CancelCd4 = 0;
                                    obj.CancelCd5 = 0;
                                    obj.CancelCd6 = 0;
                                    obj.CancelCd7 = 0;
                                    obj.CancelCd8 = 0;
                                    obj.CancelCd9 = 0;
                                    obj.CancelCd10 = 0;
                                    obj.CancelCd11 = 0;
                                    obj.CancelCd12 = 0;

                                    var indexes = model.chkItems.Select((v, i) => new { v, i }).Where(x => x.v == true).Select(x => x.i);
                                    int count = indexes.Count();

                                    if (count == 1)
                                    {
                                        obj.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                                    }
                                    else if (count == 2)
                                    {
                                        obj.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                                        obj.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                                    }
                                    else if (count == 3)
                                    {
                                        obj.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                                        obj.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                                        obj.CancelCd3 = Convert.ToByte(indexes.ElementAt(2) + 1);
                                    }
                                    else if (count == 4)
                                    {
                                        obj.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                                        obj.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                                        obj.CancelCd3 = Convert.ToByte(indexes.ElementAt(2) + 1);
                                        obj.CancelCd4 = Convert.ToByte(indexes.ElementAt(3) + 1);
                                    }
                                    else if (count == 5)
                                    {
                                        obj.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                                        obj.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                                        obj.CancelCd3 = Convert.ToByte(indexes.ElementAt(2) + 1);
                                        obj.CancelCd4 = Convert.ToByte(indexes.ElementAt(3) + 1);
                                        obj.CancelCd5 = Convert.ToByte(indexes.ElementAt(4) + 1);
                                    }
                                    else if (count == 6)
                                    {
                                        obj.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                                        obj.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                                        obj.CancelCd3 = Convert.ToByte(indexes.ElementAt(2) + 1);
                                        obj.CancelCd4 = Convert.ToByte(indexes.ElementAt(3) + 1);
                                        obj.CancelCd5 = Convert.ToByte(indexes.ElementAt(4) + 1);
                                        obj.CancelCd6 = Convert.ToByte(indexes.ElementAt(5) + 1);
                                    }
                                    else if (count == 7)
                                    {
                                        obj.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                                        obj.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                                        obj.CancelCd3 = Convert.ToByte(indexes.ElementAt(2) + 1);
                                        obj.CancelCd4 = Convert.ToByte(indexes.ElementAt(3) + 1);
                                        obj.CancelCd5 = Convert.ToByte(indexes.ElementAt(4) + 1);
                                        obj.CancelCd6 = Convert.ToByte(indexes.ElementAt(5) + 1);
                                        obj.CancelCd7 = Convert.ToByte(indexes.ElementAt(6) + 1);
                                    }
                                    else if (count == 8)
                                    {
                                        obj.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                                        obj.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                                        obj.CancelCd3 = Convert.ToByte(indexes.ElementAt(2) + 1);
                                        obj.CancelCd4 = Convert.ToByte(indexes.ElementAt(3) + 1);
                                        obj.CancelCd5 = Convert.ToByte(indexes.ElementAt(4) + 1);
                                        obj.CancelCd6 = Convert.ToByte(indexes.ElementAt(5) + 1);
                                        obj.CancelCd7 = Convert.ToByte(indexes.ElementAt(6) + 1);
                                        obj.CancelCd8 = Convert.ToByte(indexes.ElementAt(7) + 1);
                                    }
                                    else if (count == 9)
                                    {
                                        obj.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                                        obj.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                                        obj.CancelCd3 = Convert.ToByte(indexes.ElementAt(2) + 1);
                                        obj.CancelCd4 = Convert.ToByte(indexes.ElementAt(3) + 1);
                                        obj.CancelCd5 = Convert.ToByte(indexes.ElementAt(4) + 1);
                                        obj.CancelCd6 = Convert.ToByte(indexes.ElementAt(5) + 1);
                                        obj.CancelCd7 = Convert.ToByte(indexes.ElementAt(6) + 1);
                                        obj.CancelCd8 = Convert.ToByte(indexes.ElementAt(7) + 1);
                                        obj.CancelCd9 = Convert.ToByte(indexes.ElementAt(8) + 1);
                                    }
                                    else if (count == 10)
                                    {
                                        obj.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                                        obj.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                                        obj.CancelCd3 = Convert.ToByte(indexes.ElementAt(2) + 1);
                                        obj.CancelCd4 = Convert.ToByte(indexes.ElementAt(3) + 1);
                                        obj.CancelCd5 = Convert.ToByte(indexes.ElementAt(4) + 1);
                                        obj.CancelCd6 = Convert.ToByte(indexes.ElementAt(5) + 1);
                                        obj.CancelCd7 = Convert.ToByte(indexes.ElementAt(6) + 1);
                                        obj.CancelCd8 = Convert.ToByte(indexes.ElementAt(7) + 1);
                                        obj.CancelCd9 = Convert.ToByte(indexes.ElementAt(8) + 1);
                                        obj.CancelCd10 = Convert.ToByte(indexes.ElementAt(9) + 1);
                                    }
                                    else if (count == 11)
                                    {
                                        obj.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                                        obj.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                                        obj.CancelCd3 = Convert.ToByte(indexes.ElementAt(2) + 1);
                                        obj.CancelCd4 = Convert.ToByte(indexes.ElementAt(3) + 1);
                                        obj.CancelCd5 = Convert.ToByte(indexes.ElementAt(4) + 1);
                                        obj.CancelCd6 = Convert.ToByte(indexes.ElementAt(5) + 1);
                                        obj.CancelCd7 = Convert.ToByte(indexes.ElementAt(6) + 1);
                                        obj.CancelCd8 = Convert.ToByte(indexes.ElementAt(7) + 1);
                                        obj.CancelCd9 = Convert.ToByte(indexes.ElementAt(8) + 1);
                                        obj.CancelCd10 = Convert.ToByte(indexes.ElementAt(9) + 1);
                                        obj.CancelCd11 = Convert.ToByte(indexes.ElementAt(10) + 1);
                                    }
                                    else if (count == 12)
                                    {
                                        obj.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                                        obj.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                                        obj.CancelCd3 = Convert.ToByte(indexes.ElementAt(2) + 1);
                                        obj.CancelCd4 = Convert.ToByte(indexes.ElementAt(3) + 1);
                                        obj.CancelCd5 = Convert.ToByte(indexes.ElementAt(4) + 1);
                                        obj.CancelCd6 = Convert.ToByte(indexes.ElementAt(5) + 1);
                                        obj.CancelCd7 = Convert.ToByte(indexes.ElementAt(6) + 1);
                                        obj.CancelCd8 = Convert.ToByte(indexes.ElementAt(7) + 1);
                                        obj.CancelCd9 = Convert.ToByte(indexes.ElementAt(8) + 1);
                                        obj.CancelCd10 = Convert.ToByte(indexes.ElementAt(9) + 1);
                                        obj.CancelCd11 = Convert.ToByte(indexes.ElementAt(10) + 1);
                                        obj.CancelCd12 = Convert.ToByte(indexes.ElementAt(11) + 1);
                                    }
                                    context.T19CallCancels.Add(obj);
                                    context.SaveChanges();


                                    var t17Detail = from x in context.T17CallRegisters
                                                    where x.CaseNo == model.CaseNo && x.CallRecvDt == model.CallRecvDt && x.CallSno == model.CallSno
                                                    select x;
                                    if (t17Detail.Count() > 0)
                                    {
                                        foreach (var row in t17Detail)
                                        {
                                            row.CallStatus = "C";
                                            row.CallStatusDt = model.CallStatusDt;
                                            row.CallCancelStatus = model.CallCancelStatus;
                                            if (model.CallCancelStatus == "C")
                                            {
                                                row.CallCancelCharges = Convert.ToInt16(model.CallCancelCharges);
                                            }
                                            row.FifoVoilateReason = wFifoVoilateReason;
                                            context.SaveChanges();
                                        }
                                    }

                                    var t13Detail = from x in context.T13PoMasters
                                                    where x.CaseNo == model.CaseNo
                                                    select x;
                                    if (t13Detail.Count() > 0)
                                    {
                                        foreach (var row in t13Detail)
                                        {
                                            var PendCharge = row.PendingCharges == null ? 0 + 1 : Convert.ToInt16(row.PendingCharges) + 1;
                                            row.PendingCharges = Convert.ToByte(PendCharge);
                                            context.SaveChanges();
                                        }
                                    }
                                    model.AlertMsg = "Success";
                                }
                                else if (!string.IsNullOrEmpty(Action))
                                {
                                    var t19Detail = from x in context.T19CallCancels
                                                    where x.CaseNo == model.CaseNo && x.CallRecvDt == model.CallRecvDt && x.CallSno == model.CallSno
                                                    select x;
                                    if (t19Detail.Count() > 0)
                                    {
                                        foreach (var row in t19Detail)
                                        {
                                            row.CancelDate = model.CallStatusDt;
                                            row.CancelDesc = model.CancellationDescription;
                                            row.UserId = w_IE_EMPNO; //model.UserId;
                                            row.Datetime = DateTime.Now.Date;
                                            row.Updatedby = Convert.ToString(model.UserId);
                                            row.Updateddate = DateTime.Now.Date;
                                            row.CancelCd1 = 0;
                                            row.CancelCd2 = 0;
                                            row.CancelCd3 = 0;
                                            row.CancelCd4 = 0;
                                            row.CancelCd5 = 0;
                                            row.CancelCd6 = 0;
                                            row.CancelCd7 = 0;
                                            row.CancelCd8 = 0;
                                            row.CancelCd9 = 0;
                                            row.CancelCd10 = 0;
                                            row.CancelCd11 = 0;
                                            row.CancelCd12 = 0;

                                            var indexes = model.chkItems.Select((v, i) => new { v, i }).Where(x => x.v == true).Select(x => x.i);
                                            int count = indexes.Count();

                                            if (count == 1)
                                            {
                                                row.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                                            }
                                            else if (count == 2)
                                            {
                                                row.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                                                row.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                                            }
                                            else if (count == 3)
                                            {
                                                row.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                                                row.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                                                row.CancelCd3 = Convert.ToByte(indexes.ElementAt(2) + 1);
                                            }
                                            else if (count == 4)
                                            {
                                                row.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                                                row.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                                                row.CancelCd3 = Convert.ToByte(indexes.ElementAt(2) + 1);
                                                row.CancelCd4 = Convert.ToByte(indexes.ElementAt(3) + 1);
                                            }
                                            else if (count == 5)
                                            {
                                                row.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                                                row.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                                                row.CancelCd3 = Convert.ToByte(indexes.ElementAt(2) + 1);
                                                row.CancelCd4 = Convert.ToByte(indexes.ElementAt(3) + 1);
                                                row.CancelCd5 = Convert.ToByte(indexes.ElementAt(4) + 1);
                                            }
                                            else if (count == 6)
                                            {
                                                row.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                                                row.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                                                row.CancelCd3 = Convert.ToByte(indexes.ElementAt(2) + 1);
                                                row.CancelCd4 = Convert.ToByte(indexes.ElementAt(3) + 1);
                                                row.CancelCd5 = Convert.ToByte(indexes.ElementAt(4) + 1);
                                                row.CancelCd6 = Convert.ToByte(indexes.ElementAt(5) + 1);
                                            }
                                            else if (count == 7)
                                            {
                                                row.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                                                row.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                                                row.CancelCd3 = Convert.ToByte(indexes.ElementAt(2) + 1);
                                                row.CancelCd4 = Convert.ToByte(indexes.ElementAt(3) + 1);
                                                row.CancelCd5 = Convert.ToByte(indexes.ElementAt(4) + 1);
                                                row.CancelCd6 = Convert.ToByte(indexes.ElementAt(5) + 1);
                                                row.CancelCd7 = Convert.ToByte(indexes.ElementAt(6) + 1);
                                            }
                                            else if (count == 8)
                                            {
                                                row.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                                                row.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                                                row.CancelCd3 = Convert.ToByte(indexes.ElementAt(2) + 1);
                                                row.CancelCd4 = Convert.ToByte(indexes.ElementAt(3) + 1);
                                                row.CancelCd5 = Convert.ToByte(indexes.ElementAt(4) + 1);
                                                row.CancelCd6 = Convert.ToByte(indexes.ElementAt(5) + 1);
                                                row.CancelCd7 = Convert.ToByte(indexes.ElementAt(6) + 1);
                                                row.CancelCd8 = Convert.ToByte(indexes.ElementAt(7) + 1);
                                            }
                                            else if (count == 9)
                                            {
                                                row.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                                                row.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                                                row.CancelCd3 = Convert.ToByte(indexes.ElementAt(2) + 1);
                                                row.CancelCd4 = Convert.ToByte(indexes.ElementAt(3) + 1);
                                                row.CancelCd5 = Convert.ToByte(indexes.ElementAt(4) + 1);
                                                row.CancelCd6 = Convert.ToByte(indexes.ElementAt(5) + 1);
                                                row.CancelCd7 = Convert.ToByte(indexes.ElementAt(6) + 1);
                                                row.CancelCd8 = Convert.ToByte(indexes.ElementAt(7) + 1);
                                                row.CancelCd9 = Convert.ToByte(indexes.ElementAt(8) + 1);
                                            }
                                            else if (count == 10)
                                            {
                                                row.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                                                row.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                                                row.CancelCd3 = Convert.ToByte(indexes.ElementAt(2) + 1);
                                                row.CancelCd4 = Convert.ToByte(indexes.ElementAt(3) + 1);
                                                row.CancelCd5 = Convert.ToByte(indexes.ElementAt(4) + 1);
                                                row.CancelCd6 = Convert.ToByte(indexes.ElementAt(5) + 1);
                                                row.CancelCd7 = Convert.ToByte(indexes.ElementAt(6) + 1);
                                                row.CancelCd8 = Convert.ToByte(indexes.ElementAt(7) + 1);
                                                row.CancelCd9 = Convert.ToByte(indexes.ElementAt(8) + 1);
                                                row.CancelCd10 = Convert.ToByte(indexes.ElementAt(9) + 1);
                                            }
                                            else if (count == 11)
                                            {
                                                row.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                                                row.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                                                row.CancelCd3 = Convert.ToByte(indexes.ElementAt(2) + 1);
                                                row.CancelCd4 = Convert.ToByte(indexes.ElementAt(3) + 1);
                                                row.CancelCd5 = Convert.ToByte(indexes.ElementAt(4) + 1);
                                                row.CancelCd6 = Convert.ToByte(indexes.ElementAt(5) + 1);
                                                row.CancelCd7 = Convert.ToByte(indexes.ElementAt(6) + 1);
                                                row.CancelCd8 = Convert.ToByte(indexes.ElementAt(7) + 1);
                                                row.CancelCd9 = Convert.ToByte(indexes.ElementAt(8) + 1);
                                                row.CancelCd10 = Convert.ToByte(indexes.ElementAt(9) + 1);
                                                row.CancelCd11 = Convert.ToByte(indexes.ElementAt(10) + 1);
                                            }
                                            else if (count == 12)
                                            {
                                                row.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                                                row.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                                                row.CancelCd3 = Convert.ToByte(indexes.ElementAt(2) + 1);
                                                row.CancelCd4 = Convert.ToByte(indexes.ElementAt(3) + 1);
                                                row.CancelCd5 = Convert.ToByte(indexes.ElementAt(4) + 1);
                                                row.CancelCd6 = Convert.ToByte(indexes.ElementAt(5) + 1);
                                                row.CancelCd7 = Convert.ToByte(indexes.ElementAt(6) + 1);
                                                row.CancelCd8 = Convert.ToByte(indexes.ElementAt(7) + 1);
                                                row.CancelCd9 = Convert.ToByte(indexes.ElementAt(8) + 1);
                                                row.CancelCd10 = Convert.ToByte(indexes.ElementAt(9) + 1);
                                                row.CancelCd11 = Convert.ToByte(indexes.ElementAt(10) + 1);
                                                row.CancelCd12 = Convert.ToByte(indexes.ElementAt(11) + 1);
                                            }
                                            context.SaveChanges();
                                        }
                                    }

                                    var t17Detail = from x in context.T17CallRegisters
                                                    where x.CaseNo == model.CaseNo && x.CallRecvDt == model.CallRecvDt && x.CallSno == model.CallSno
                                                    select x;
                                    if (t17Detail.Count() > 0)
                                    {
                                        foreach (var row in t17Detail)
                                        {
                                            row.CallStatus = "C";
                                            row.CallStatusDt = model.CallStatusDt;
                                            row.CallCancelStatus = model.CallCancelStatus;
                                            if (model.CallCancelStatus == "C")
                                            {
                                                row.CallCancelCharges = Convert.ToInt16(model.CallListByRly);
                                            }
                                            row.FifoVoilateReason = wFifoVoilateReason;
                                            context.SaveChanges();
                                        }
                                    }
                                    model.AlertMsg = "Success";
                                }
                                trans.Commit();
                            }
                            catch (Exception)
                            {
                                trans.Rollback();
                            }
                        }
                    }
                    else
                    {
                        model.AlertMsg = "The IC is Present For give CASE_NO, CALL_RECV_DT and CALL_SNO, So it can not be cancelled!!!";
                    }
                    //send_Vendor_Mail(model);
                }
            }
            return model.AlertMsg;//str;
        }

        public VendrorCallDetailsModel CallDetailsFindByID(string CaseNo, string CallRecvDt, int CallSno, int ItemSrNoPo)
        {
            VendrorCallDetailsModel model = new();
            string dt = Convert.ToDateTime(CallRecvDt).ToString("dd-MM-yyyy");
            DateTime parsedDate = DateTime.ParseExact(dt, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var GetValue = (from v in context.T09Ies
                            join d in context.T17CallRegisters on v.IeCd equals d.IeCd
                            where d.CaseNo == CaseNo && d.CallRecvDt == parsedDate && d.CallSno == CallSno
                            select new
                            {
                                v,
                                d
                            }
                  ).FirstOrDefault();

            var PODetails = context.T13PoMasters.Where(x => x.CaseNo == CaseNo).FirstOrDefault();

            var T15PO = context.T15PoDetails.Where(x => x.CaseNo == CaseNo && x.ItemSrno == ItemSrNoPo).FirstOrDefault();
            if (T15PO != null)
            {
                model.ItemDescPo = T15PO.ItemDesc;
                model.ItemSrNoPo = T15PO.ItemSrno;
                model.QtyOrdered = T15PO.Qty;
                model.Consignee = Convert.ToString(T15PO.ConsigneeCd);
            }

            var CallDetails = context.T18CallDetails.Where(x => x.CaseNo == CaseNo && x.CallRecvDt == parsedDate && x.CallSno == CallSno && x.ItemSrnoPo == ItemSrNoPo).FirstOrDefault();
            if (GetValue != null)
            {
                model.IESName = GetValue.v.IeSname;
                model.CallRecvDt = GetValue.d.CallRecvDt;
                model.CallSno = (short)GetValue.d.CallSno;

                if (PODetails != null)
                {
                    model.PoNo = PODetails.PoNo;
                    model.PoDt = PODetails.PoDt;
                }
            }
            if (CallDetails != null)
            {
                model.ItemSrNoPo = CallDetails.ItemSrnoPo;
                model.ItemDescPo = CallDetails.ItemDescPo;
                model.QtyOrdered = CallDetails.QtyOrdered;
                model.CumQtyPrevOffered = CallDetails.CumQtyPrevOffered;
                model.CumQtyPrevPassed = CallDetails.CumQtyPrevPassed;
                model.QtyToInsp = CallDetails.QtyToInsp;
            }
            return model;
        }

        public DTResult<VendrorCallDetailsModel> GetCallDetailsList(DTParameters dtParameters)
        {

            DTResult<VendrorCallDetailsModel> dTResult = new() { draw = 0 };
            IQueryable<VendrorCallDetailsModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = false;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "Status";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
            }
            else
            {
                orderCriteria = "Status";
                orderAscendingDirection = true;
            }

            string CaseNo = "", CallRecvDt = "", CallSNo = "";

            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["CaseNo"]))
            {
                CaseNo = Convert.ToString(dtParameters.AdditionalValues["CaseNo"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["CallRecvDt"]))
            {
                CallRecvDt = Convert.ToString(dtParameters.AdditionalValues["CallRecvDt"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["CallSNo"]))
            {
                CallSNo = Convert.ToString(dtParameters.AdditionalValues["CallSNo"]);
            }

            CaseNo = CaseNo.ToString() == "" ? string.Empty : CaseNo.ToString();
            //DateTime? _CallRecvDt = CallRecvDt == "" ? null : DateTime.ParseExact(CallRecvDt, "dd/MM/yyyy", null);
            CallSNo = CallSNo.ToString() == "" ? string.Empty : CallSNo.ToString();



            OracleParameter[] par = new OracleParameter[4];
            par[0] = new OracleParameter("p_CNO", OracleDbType.Varchar2, CaseNo, ParameterDirection.Input);
            par[1] = new OracleParameter("p_DT", OracleDbType.Date, Convert.ToDateTime(CallRecvDt), ParameterDirection.Input);
            par[2] = new OracleParameter("p_CSNO", OracleDbType.Int32, CallSNo, ParameterDirection.Input);

            par[3] = new OracleParameter("p_result_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("SP_GET_CALL_DETAILS", par, 1);
            DataTable dt = ds.Tables[0];

            //VendrorCallDetailsModel model = new();
            //List<VendrorCallDetailsModel> list = new List<VendrorCallDetailsModel>();

            //if (ds != null && ds.Tables.Count > 0)
            //{
            //    string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);

            //    JArray jsonArray = JArray.Parse(serializeddt);
            //    list = jsonArray.Select(item =>
            //    {
            //        var lst = item.ToObject<VendrorCallDetailsModel>();

            //        if (lst.QTYPASSED is int doubleValue)
            //        {
            //            lst.QTYPASSED = (int)doubleValue;
            //        }

            //        return lst;
            //    }).ToList();
            //}

            VendrorCallDetailsModel model = new();
            List<VendrorCallDetailsModel> list = new();
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                list = JsonConvert.DeserializeObject<List<VendrorCallDetailsModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }


            query = list.AsQueryable();

            dTResult.recordsTotal = ds.Tables[0].Rows.Count;

            dTResult.recordsFiltered = ds.Tables[0].Rows.Count;

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public int CallDetailsSave(VendrorCallDetailsModel model, string UserName)
        {
            int Id = 0;
            int status = 0;
            decimal reader = 0;
            decimal qty = 0;

            var CallDetails = (from c in context.T17CallRegisters
                               join d in context.T18CallDetails on c.CaseNo equals d.CaseNo
                               where c.CaseNo.Equals(d.CaseNo) && c.CallRecvDt.Equals(d.CallRecvDt) && c.CallSno.Equals(d.CallSno)
                               && c.CaseNo == model.CaseNo && d.ItemSrnoPo == model.ItemSrNoPo && !new[] { "R", "C" }.Contains(c.CallStatus)
                               select new
                               {
                                   d.QtyToInsp
                               }
                  ).FirstOrDefault();
            if (CallDetails != null)
            {
                reader = Convert.ToDecimal(CallDetails.QtyToInsp);
            }
            if (reader > 0)
            {
                qty = (reader + Convert.ToDecimal(model.QtyToInsp)) - Convert.ToDecimal(model.QtyToInsp);
                if (reader > Convert.ToDecimal(model.QtyOrdered))
                {
                    status = 2;
                }
                else
                {
                    if (qty > Convert.ToDecimal(model.QtyOrdered))
                    {
                        status = 1;
                    }
                }
            }
            if (status != 2)
            {
                if (status == 1)
                {
                    return 1;
                }
                else
                {
                    var CallDetailsUpdate = context.T18CallDetails.Where(x => x.CaseNo == model.CaseNo && x.CallRecvDt == model.CallRecvDt && x.CallSno == model.CallSno && x.ItemSrnoPo == model.ItemSrNoPo).FirstOrDefault();

                    #region CallDetailsUpdate

                    if (CallDetailsUpdate == null)
                    {
                        T18CallDetail obj = new();
                        obj.CaseNo = model.CaseNo;
                        obj.CallRecvDt = Convert.ToDateTime(model.CallRecvDt);
                        obj.CallSno = Convert.ToInt32(model.CallSno);
                        obj.ItemSrnoPo = model.ItemSrNoPo;
                        obj.ItemDescPo = model.ItemDescPo;
                        obj.ConsigneeCd = Convert.ToInt32(model.Consignee);
                        obj.QtyOrdered = model.QtyOrdered != null ? model.QtyOrdered : 0;
                        obj.CumQtyPrevOffered = model.CumQtyPrevOffered != null ? model.CumQtyPrevOffered : 0;
                        obj.CumQtyPrevPassed = model.CumQtyPrevPassed != null ? model.CumQtyPrevPassed : 0;
                        obj.QtyToInsp = model.QtyToInsp != null ? model.QtyToInsp : 0;
                        obj.UserId = UserName;
                        obj.Datetime = DateTime.Now;
                        obj.Updatedby = model.Updatedby;
                        obj.Updateddate = DateTime.Now;
                        context.T18CallDetails.Add(obj);
                        context.SaveChanges();
                        Id = Convert.ToInt32(model.ItemSrNoPo);

                    }
                    else
                    {
                        CallDetailsUpdate.ItemDescPo = model.ItemDescPo;
                        CallDetailsUpdate.QtyOrdered = model.QtyOrdered != null ? model.QtyOrdered : 0;
                        CallDetailsUpdate.CumQtyPrevOffered = model.CumQtyPrevOffered != null ? model.CumQtyPrevOffered : 0;
                        CallDetailsUpdate.CumQtyPrevPassed = model.CumQtyPrevPassed != null ? model.CumQtyPrevPassed : 0;
                        CallDetailsUpdate.QtyToInsp = model.QtyToInsp != null ? model.QtyToInsp : 0;
                        CallDetailsUpdate.UserId = UserName;
                        CallDetailsUpdate.Datetime = DateTime.Now;
                        CallDetailsUpdate.Updatedby = model.Updatedby;
                        CallDetailsUpdate.Updateddate = DateTime.Now;
                        context.SaveChanges();
                        Id = Convert.ToInt32(model.ItemSrNoPo);
                    }
                    #endregion

                    var PODetailsUpdate = context.T15PoDetails.Where(x => x.CaseNo == model.CaseNo && x.ItemSrno == model.ItemSrNoPo).FirstOrDefault();

                    #region PODetailsUpdate 
                    if (PODetailsUpdate != null)
                    {
                        PODetailsUpdate.ItemDesc = model.ItemDescPo;
                        context.SaveChanges();
                        Id = Convert.ToInt32(3);
                    }
                    #endregion
                }
            }
            else if (status == 2)
            {
                return 2;
            }

            return Id;
        }

        public VenderCallStatusModel CallStatusFilesSave(VenderCallStatusModel model, List<APPDocumentDTO> DocumentsList)
        {
            int consignee_cd = 0;
            ImageFiles filesimg = new ImageFiles();
            string formattedCallRecvDt = "";

            if (model.CallRecvDt != null && model.CallRecvDt != DateTime.MinValue)
            {
                DateTime parsedFromDate = DateTime.ParseExact(model.CallRecvDt.ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                formattedCallRecvDt = parsedFromDate.ToString("dd/MM/yyyy");
            }

            if (model.DocBkNo != null && model.DocSetNo != null)
            {
                var FinalOrStage = context.T17CallRegisters
                    .Where(cr => cr.CaseNo == model.CaseNo && cr.CallSno == model.CallSno && cr.CallRecvDt == Convert.ToDateTime(formattedCallRecvDt))
                    .Select(cr => cr.FinalOrStage)
                    .FirstOrDefault() ?? "F";

                //var bsCheck = context.T10IcBooksets.Where(bookset => bookset.BkNo.Trim().ToUpper() == model.DocBkNo && Convert.ToInt32(model.DocSetNo) >= Convert.ToInt32(bookset.SetNoFr) &&
                //              Convert.ToInt32(model.DocSetNo) <= Convert.ToInt32(bookset.SetNoTo) && bookset.IssueToIecd == Convert.ToInt32(model.IeCd)).Select(bookset => bookset.IssueToIecd).FirstOrDefault();

                var docSetNo = Convert.ToInt32(model.DocSetNo);


                var bsCheckIC = (from a in context.T10IcBooksets
                                 where a.BkNo.Trim() == model.DocBkNo.Trim()
                                 && (docSetNo >= Convert.ToInt32(a.SetNoFr) && docSetNo <= Convert.ToInt32(a.SetNoTo))
                                 && a.IssueToIecd == Convert.ToInt32(model.IeCd)
                                 && (a.Ictype ?? "F") == FinalOrStage
                                 select a.IssueToIecd).FirstOrDefault();

                //var bsCheckIC = (from a in context.T10IcBooksets
                //                 where a.BkNo.Trim() == model.DocBkNo.Trim()
                //                 //&& (docSetNo >= Convert.ToInt32(a.SetNoFr) && docSetNo <= Convert.ToInt32(a.SetNoTo))
                //                 && a.IssueToIecd == Convert.ToInt32(model.IeCd)
                //                 //&& a.Ictype == FinalOrStage
                //                 && FinalOrStage == "F" ? (a.Ictype == null || a.Ictype == FinalOrStage) : a.Ictype == FinalOrStage
                //                 select a.IssueToIecd).FirstOrDefault();

                if (bsCheckIC != null)
                {
                    var query = context.IcIntermediates
                            .Where(ici => ici.CaseNo == model.CaseNo &&
                                          ici.CallRecvDt == Convert.ToDateTime(formattedCallRecvDt) &&
                                          ici.CallSno == model.CallSno &&
                                          ici.BkNo == model.DocBkNo &&
                                          ici.SetNo == model.DocSetNo)
                            .OrderBy(ici => ici.Datetime)
                            .Select(ici => ici.ConsigneeCd)
                            .FirstOrDefault();

                    consignee_cd = query;

                    if (consignee_cd > 0)
                    {
                        if (consignee_cd.ToString() != model.ConsigneeFirm)
                        {
                            model.AlertMsg = "Please Enter other book no. or set no. same is used for consignee " + consignee_cd + " !!!";
                            return model;
                        }
                    }

                    string FileName = model.CaseNo + "-" + model.DocBkNo + "-" + model.DocSetNo + "_0";

                    ImageFiles imageFiles = new ImageFiles();

                    for (int i = 0; i < DocumentsList.Count && i < 10; i++)
                    {
                        var document = DocumentsList[i];
                        string displayName = document.DocName;

                        if (displayName == "IC Image 1")
                        {
                            filesimg.File_1 = FileName + "1.JPG";
                        }
                        else if (displayName == "IC Image 2")
                        {
                            filesimg.File_2 = FileName + "2.JPG";
                        }
                        else if (displayName == "IC Image 3")
                        {
                            filesimg.File_3 = FileName + "3.JPG";
                        }
                        else if (displayName == "IC Image 4")
                        {
                            filesimg.File_4 = FileName + "4.JPG";
                        }
                        else if (displayName == "IC Image 5")
                        {
                            filesimg.File_5 = FileName + "5.JPG";
                        }
                        else if (displayName == "IC Image 6")
                        {
                            filesimg.File_6 = FileName + "6.JPG";
                        }
                        else if (displayName == "IC Image 7")
                        {
                            filesimg.File_7 = FileName + "7.JPG";
                        }
                        else if (displayName == "IC Image 8")
                        {
                            filesimg.File_8 = FileName + "8.JPG";
                        }
                        else if (displayName == "IC Image 9")
                        {
                            filesimg.File_9 = FileName + "9.JPG";
                        }
                        else if (displayName == "IC Image 10")
                        {
                            filesimg.File_10 = FileName + "10.JPG";
                        }
                    }

                    DateTime CallRecvDate = DateTime.ParseExact(Convert.ToDateTime(model.CallRecvDt).ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    var IcDetail = (from item in context.IcIntermediates
                                    where item.CaseNo == model.CaseNo.Trim() &&
                                          item.CallSno == model.CallSno &&
                                          item.CallRecvDt == CallRecvDate &&
                                          item.ConsigneeCd == Convert.ToInt32(model.ConsigneeFirm)
                                    select item).FirstOrDefault();

                    if (IcDetail == null)
                    {

                        var CallDetails = (from c in context.T18CallDetails
                                           where c.CaseNo == model.CaseNo && c.CallRecvDt == CallRecvDate
                                           && c.CallSno == model.CallSno && c.ConsigneeCd == Convert.ToInt32(model.ConsigneeFirm)
                                           select c).ToList();
                        if (CallDetails.Count() > 0)
                        {
                            foreach (var i in CallDetails)
                            {
                                IcIntermediate obj = new IcIntermediate();
                                obj.CaseNo = model.CaseNo;
                                obj.CallRecvDt = CallRecvDate;
                                obj.CallSno = Convert.ToInt16(model.CallSno);
                                obj.BkNo = model.DocBkNo;
                                obj.SetNo = model.DocSetNo;
                                obj.PoNo = model.PoNo;
                                obj.ConsigneeCd = Convert.ToInt32(model.ConsigneeFirm);
                                obj.UserId = model.UserName;
                                obj.ItemSrnoPo = i.ItemSrnoPo;
                                obj.ItemDescPo = i.ItemDescPo;
                                obj.QtyPassed = i.QtyPassed;
                                obj.QtyRejected = i.QtyRejected;
                                obj.QtyDue = i.QtyDue;
                                obj.IeCd = Convert.ToInt32(model.IeCd);
                                obj.Datetime = DateTime.Now;
                                obj.Createddate = DateTime.Now;
                                obj.Createdby = model.UserId;
                                context.IcIntermediates.Add(obj);
                                context.SaveChanges();

                            }
                        }


                    }
                    else
                    {
                        using (var command = context.Database.GetDbConnection().CreateCommand())
                        {
                            bool wasOpen = command.Connection.State == System.Data.ConnectionState.Open;
                            if (!wasOpen) command.Connection.Open();
                            try
                            {
                                command.CommandText = "UPDATE IC_INTERMEDIATE SET BK_NO = '" + model.DocBkNo + "', SET_NO = '" + model.DocSetNo + "', UPDATEDBY =" + model.UserId + ",UPDATEDDATE=TO_date('" + DateTime.Now.ToString("dd/MM/yyyy") + "', 'dd/mm/yyyy') WHERE CASE_NO = '" + model.CaseNo + "' AND CALL_SNO = '" + model.CallSno + "' AND CALL_RECV_DT = TO_date('" + CallRecvDate.ToString("dd/MM/yyyy") + "', 'dd/mm/yyyy') AND CONSIGNEE_CD = " + Convert.ToInt32(model.ConsigneeFirm);
                                command.ExecuteNonQuery();
                            }
                            finally
                            {
                                if (!wasOpen) command.Connection.Close();
                            }
                        }
                    }

                    var recordExists = context.T49IcPhotoEncloseds.Where(x => x.CaseNo == model.CaseNo && x.BkNo == model.DocBkNo && x.SetNo == model.DocSetNo && x.CallSno == model.CallSno && x.CallRecvDt == Convert.ToDateTime(formattedCallRecvDt)).FirstOrDefault();

                    if (recordExists == null)
                    {
                        T49IcPhotoEnclosed obj = new T49IcPhotoEnclosed();

                        obj.CaseNo = model.CaseNo;
                        obj.CallRecvDt = model.CallRecvDt;
                        obj.CallSno = (short?)model.CallSno;
                        obj.BkNo = model.DocBkNo;
                        obj.SetNo = model.DocSetNo;
                        obj.ConsigneeCd = Convert.ToInt32(model.ConsigneeFirm);
                        obj.Datetime = DateTime.Now;
                        obj.UserId = model.UserName;
                        obj.File1 = filesimg.File_1;
                        obj.File2 = filesimg.File_2;
                        obj.File3 = filesimg.File_3;
                        obj.File4 = filesimg.File_4;
                        obj.File5 = filesimg.File_5;
                        obj.File6 = filesimg.File_6;
                        obj.File7 = filesimg.File_7;
                        obj.File8 = filesimg.File_8;
                        obj.File9 = filesimg.File_9;
                        obj.File10 = filesimg.File_10;
                        context.T49IcPhotoEncloseds.Add(obj);
                        context.SaveChanges();
                    }
                    else
                    {
                        recordExists.CaseNo = model.CaseNo;
                        recordExists.CallRecvDt = model.CallRecvDt;
                        recordExists.CallSno = (short?)model.CallSno;
                        recordExists.BkNo = model.DocBkNo;
                        recordExists.SetNo = model.DocSetNo;
                        recordExists.ConsigneeCd = Convert.ToInt32(model.ConsigneeFirm);
                        recordExists.Datetime = DateTime.Now;
                        recordExists.UserId = model.UserName;
                        recordExists.File1 = filesimg.File_1;
                        recordExists.File2 = filesimg.File_2;
                        recordExists.File3 = filesimg.File_3;
                        recordExists.File4 = filesimg.File_4;
                        recordExists.File5 = filesimg.File_5;
                        recordExists.File6 = filesimg.File_6;
                        recordExists.File7 = filesimg.File_7;
                        recordExists.File8 = filesimg.File_8;
                        recordExists.File9 = filesimg.File_9;
                        recordExists.File10 = filesimg.File_10;
                        context.SaveChanges();
                    }

                    var IcDetail1 = (from ic in context.IcIntermediates
                                     where ic.CaseNo == model.CaseNo && ic.BkNo == model.DocBkNo && ic.SetNo == model.DocSetNo && ic.ConsigneeCd == Convert.ToInt32(model.ConsigneeFirm)
                                     select ic).FirstOrDefault();

                    if (IcDetail1 != null)
                    {
                        IcDetail1.File1 = filesimg.File_1;
                        IcDetail1.File2 = filesimg.File_2;
                        IcDetail1.File3 = filesimg.File_3;
                        IcDetail1.File4 = filesimg.File_4;
                        IcDetail1.File5 = filesimg.File_5;
                        IcDetail1.File6 = filesimg.File_6;
                        IcDetail1.File7 = filesimg.File_7;
                        IcDetail1.File8 = filesimg.File_8;
                        IcDetail1.File9 = filesimg.File_9;
                        IcDetail1.File10 = filesimg.File_10;
                        context.SaveChanges();
                    }
                    model.AlertMsg = "Success";
                }
                else if (model.DocBkNo != "" && model.DocSetNo != "" && (bsCheckIC == null || bsCheckIC == 0))
                {
                    model.AlertMsg = "Book No. and Set No. specified is not issued to You!!!";
                    return model;
                }
            }
            else
            {
                model.AlertMsg = "Please enter valid book no. and set no. !";
                return model;
            }

            return model;
        }

        public VenderCallStatusModel RefreshAllDlt(VenderCallStatusModel model)
        {
            var query = context.IcIntermediates
        .Where(i => i.CaseNo == model.CaseNo &&
                    i.CallRecvDt == model.CallRecvDt &&
                    i.CallSno == model.CallSno &&
                    (i.ConsgnCallStatus != "A" && i.ConsgnCallStatus != "R") || i.ConsgnCallStatus == null);

            context.IcIntermediates.RemoveRange(query);

            context.SaveChanges();

            model.AlertMsg = "Success";

            return model;
        }

        public VenderCallStatusModel CallCancellationSave(VenderCallStatusModel model, List<APPDocumentDTO> DocumentsList)
        {
            if (model.CallStatus == null || (model.CallStatus == "C" && model.CallStatus == ""))
            {
                model.AlertMsg = "Mention Call Chargeable/Call Non-Chargeable & Select One of the Given Call Cancellation Charges in Case the Call is Chargeable!!!";
                return model;
            }
            else
            {
                string wFifoVoilateReason = "";
                if (model.ReasonFIFO != null)
                {
                    wFifoVoilateReason = model.ReasonFIFO.Trim();
                }
                var CCd = context.T20Ics.Where(ic => ic.CaseNo == model.CaseNo && ic.CallRecvDt == model.CallRecvDt && ic.CallSno == model.CallSno).Select(ic => ic.CaseNo).FirstOrDefault();

                var Action = context.T19CallCancels.Where(cancel => cancel.CaseNo == model.CaseNo && cancel.CallRecvDt == model.CallRecvDt && cancel.CallSno == model.CallSno).Select(cancel => cancel.CaseNo).FirstOrDefault();

                var w_IE_EMPNO = context.T09Ies.Where(ie => ie.IeCd == Convert.ToInt32(model.IeCd)).Select(ie => ie.IeEmpNo).FirstOrDefault();

                if (CCd == null)
                {
                    if (Action == null)
                    {
                        T19CallCancel obj = new T19CallCancel();
                        obj.CaseNo = model.CaseNo;
                        obj.CallRecvDt = Convert.ToDateTime(model.CallRecvDt);
                        obj.CallSno = (int)model.CallSno;
                        obj.CancelDesc = model.CancellationDescription;
                        obj.UserId = model.UserName;
                        obj.Datetime = DateTime.Now.Date;

                        obj.Createdby = Convert.ToString(model.UserId);
                        obj.Createddate = DateTime.Now.Date;


                        obj.CancelCd1 = 0;
                        obj.CancelCd2 = 0;
                        obj.CancelCd3 = 0;
                        obj.CancelCd4 = 0;
                        obj.CancelCd5 = 0;
                        obj.CancelCd6 = 0;
                        obj.CancelCd7 = 0;
                        obj.CancelCd8 = 0;
                        obj.CancelCd9 = 0;
                        obj.CancelCd10 = 0;
                        obj.CancelCd11 = 0;
                        obj.CancelCd12 = 0;

                        var indexes = model.chkItems.Select((v, i) => new { v, i }).Where(x => x.v == true).Select(x => x.i);
                        int count = indexes.Count();

                        if (count == 1)
                        {
                            obj.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                        }
                        else if (count == 2)
                        {
                            obj.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                            obj.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                        }
                        else if (count == 3)
                        {
                            obj.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                            obj.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                            obj.CancelCd3 = Convert.ToByte(indexes.ElementAt(2) + 1);
                        }
                        else if (count == 4)
                        {
                            obj.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                            obj.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                            obj.CancelCd3 = Convert.ToByte(indexes.ElementAt(2) + 1);
                            obj.CancelCd4 = Convert.ToByte(indexes.ElementAt(3) + 1);
                        }
                        else if (count == 5)
                        {
                            obj.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                            obj.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                            obj.CancelCd3 = Convert.ToByte(indexes.ElementAt(2) + 1);
                            obj.CancelCd4 = Convert.ToByte(indexes.ElementAt(3) + 1);
                            obj.CancelCd5 = Convert.ToByte(indexes.ElementAt(4) + 1);
                        }
                        else if (count == 6)
                        {
                            obj.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                            obj.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                            obj.CancelCd3 = Convert.ToByte(indexes.ElementAt(2) + 1);
                            obj.CancelCd4 = Convert.ToByte(indexes.ElementAt(3) + 1);
                            obj.CancelCd5 = Convert.ToByte(indexes.ElementAt(4) + 1);
                            obj.CancelCd6 = Convert.ToByte(indexes.ElementAt(5) + 1);
                        }
                        else if (count == 7)
                        {
                            obj.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                            obj.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                            obj.CancelCd3 = Convert.ToByte(indexes.ElementAt(2) + 1);
                            obj.CancelCd4 = Convert.ToByte(indexes.ElementAt(3) + 1);
                            obj.CancelCd5 = Convert.ToByte(indexes.ElementAt(4) + 1);
                            obj.CancelCd6 = Convert.ToByte(indexes.ElementAt(5) + 1);
                            obj.CancelCd7 = Convert.ToByte(indexes.ElementAt(6) + 1);
                        }
                        else if (count == 8)
                        {
                            obj.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                            obj.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                            obj.CancelCd3 = Convert.ToByte(indexes.ElementAt(2) + 1);
                            obj.CancelCd4 = Convert.ToByte(indexes.ElementAt(3) + 1);
                            obj.CancelCd5 = Convert.ToByte(indexes.ElementAt(4) + 1);
                            obj.CancelCd6 = Convert.ToByte(indexes.ElementAt(5) + 1);
                            obj.CancelCd7 = Convert.ToByte(indexes.ElementAt(6) + 1);
                            obj.CancelCd8 = Convert.ToByte(indexes.ElementAt(7) + 1);
                        }
                        else if (count == 9)
                        {
                            obj.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                            obj.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                            obj.CancelCd3 = Convert.ToByte(indexes.ElementAt(2) + 1);
                            obj.CancelCd4 = Convert.ToByte(indexes.ElementAt(3) + 1);
                            obj.CancelCd5 = Convert.ToByte(indexes.ElementAt(4) + 1);
                            obj.CancelCd6 = Convert.ToByte(indexes.ElementAt(5) + 1);
                            obj.CancelCd7 = Convert.ToByte(indexes.ElementAt(6) + 1);
                            obj.CancelCd8 = Convert.ToByte(indexes.ElementAt(7) + 1);
                            obj.CancelCd9 = Convert.ToByte(indexes.ElementAt(8) + 1);
                        }
                        else if (count == 10)
                        {
                            obj.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                            obj.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                            obj.CancelCd3 = Convert.ToByte(indexes.ElementAt(2) + 1);
                            obj.CancelCd4 = Convert.ToByte(indexes.ElementAt(3) + 1);
                            obj.CancelCd5 = Convert.ToByte(indexes.ElementAt(4) + 1);
                            obj.CancelCd6 = Convert.ToByte(indexes.ElementAt(5) + 1);
                            obj.CancelCd7 = Convert.ToByte(indexes.ElementAt(6) + 1);
                            obj.CancelCd8 = Convert.ToByte(indexes.ElementAt(7) + 1);
                            obj.CancelCd9 = Convert.ToByte(indexes.ElementAt(8) + 1);
                            obj.CancelCd10 = Convert.ToByte(indexes.ElementAt(9) + 1);
                        }
                        else if (count == 11)
                        {
                            obj.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                            obj.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                            obj.CancelCd3 = Convert.ToByte(indexes.ElementAt(2) + 1);
                            obj.CancelCd4 = Convert.ToByte(indexes.ElementAt(3) + 1);
                            obj.CancelCd5 = Convert.ToByte(indexes.ElementAt(4) + 1);
                            obj.CancelCd6 = Convert.ToByte(indexes.ElementAt(5) + 1);
                            obj.CancelCd7 = Convert.ToByte(indexes.ElementAt(6) + 1);
                            obj.CancelCd8 = Convert.ToByte(indexes.ElementAt(7) + 1);
                            obj.CancelCd9 = Convert.ToByte(indexes.ElementAt(8) + 1);
                            obj.CancelCd10 = Convert.ToByte(indexes.ElementAt(9) + 1);
                            obj.CancelCd11 = Convert.ToByte(indexes.ElementAt(10) + 1);
                        }
                        else if (count == 12)
                        {
                            obj.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                            obj.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                            obj.CancelCd3 = Convert.ToByte(indexes.ElementAt(2) + 1);
                            obj.CancelCd4 = Convert.ToByte(indexes.ElementAt(3) + 1);
                            obj.CancelCd5 = Convert.ToByte(indexes.ElementAt(4) + 1);
                            obj.CancelCd6 = Convert.ToByte(indexes.ElementAt(5) + 1);
                            obj.CancelCd7 = Convert.ToByte(indexes.ElementAt(6) + 1);
                            obj.CancelCd8 = Convert.ToByte(indexes.ElementAt(7) + 1);
                            obj.CancelCd9 = Convert.ToByte(indexes.ElementAt(8) + 1);
                            obj.CancelCd10 = Convert.ToByte(indexes.ElementAt(9) + 1);
                            obj.CancelCd11 = Convert.ToByte(indexes.ElementAt(10) + 1);
                            obj.CancelCd12 = Convert.ToByte(indexes.ElementAt(11) + 1);
                        }

                        context.T19CallCancels.Add(obj);
                        context.SaveChanges();

                        if (model.CallStatus == "C" && model.CallCancelStatus == "C")
                        {
                            var callRegister = context.T17CallRegisters.FirstOrDefault(cr => cr.CaseNo == model.CaseNo && cr.CallRecvDt == model.CallRecvDt && cr.CallSno == model.CallSno);

                            if (callRegister != null)
                            {
                                short? cancelCharges = string.IsNullOrEmpty(model.CallCancelCharges) ? null : Convert.ToInt16(model.CallCancelCharges);
                                callRegister.CallStatus = model.CallStatus;
                                callRegister.CallStatusDt = model.CallStatusDt;
                                callRegister.CallCancelStatus = model.CallCancelStatus;
                                callRegister.CallCancelCharges = cancelCharges;
                                callRegister.FifoVoilateReason = wFifoVoilateReason;
                                callRegister.CallCancelChargesStatus = model.CallCancelChargesStatus;
                                callRegister.CallCancelAmount = model.CallCancelAmount;
                                context.SaveChanges();
                            }
                        }
                        else
                        {
                            var callRegister = context.T17CallRegisters.FirstOrDefault(cr => cr.CaseNo == model.CaseNo && cr.CallRecvDt == model.CallRecvDt && cr.CallSno == model.CallSno);

                            if (callRegister != null)
                            {
                                callRegister.CallStatus = model.CallStatus;
                                callRegister.CallStatusDt = model.CallStatusDt;
                                callRegister.CallCancelStatus = model.CallCancelStatus;
                                callRegister.FifoVoilateReason = wFifoVoilateReason;
                                context.SaveChanges();
                            }
                        }

                        if (model.CallStatus == "C" && model.CallCancelStatus == "C")
                        {
                            var poMaster = context.T13PoMasters.FirstOrDefault(pm => pm.CaseNo == model.CaseNo);
                            poMaster.PendingCharges = (byte?)((poMaster.PendingCharges ?? 0) + 1);

                            context.SaveChanges();
                        }
                        model.AlertMsg = "Success";
                    }
                    else if (Action != null)
                    {
                        var CallCancalltion = context.T19CallCancels.FirstOrDefault(cc => cc.CaseNo == model.CaseNo && cc.CallRecvDt == model.CallRecvDt && cc.CallSno == model.CallSno);

                        if (CallCancalltion != null)
                        {
                            CallCancalltion.CancelCd1 = 0;
                            CallCancalltion.CancelCd2 = 0;
                            CallCancalltion.CancelCd3 = 0;
                            CallCancalltion.CancelCd4 = 0;
                            CallCancalltion.CancelCd5 = 0;
                            CallCancalltion.CancelCd6 = 0;
                            CallCancalltion.CancelCd7 = 0;
                            CallCancalltion.CancelCd8 = 0;
                            CallCancalltion.CancelCd9 = 0;
                            CallCancalltion.CancelCd10 = 0;
                            CallCancalltion.CancelCd11 = 0;
                            CallCancalltion.CancelCd12 = 0;

                            var indexes = model?.chkItems?.Select((v, i) => new { v, i })?.Where(x => x.v == true)?.Select(x => x.i) ?? Enumerable.Empty<int>();

                            int count = indexes.Count();

                            if (count == 1)
                            {
                                CallCancalltion.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                            }
                            else if (count == 2)
                            {
                                CallCancalltion.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                                CallCancalltion.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                            }
                            else if (count == 3)
                            {
                                CallCancalltion.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                                CallCancalltion.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                                CallCancalltion.CancelCd3 = Convert.ToByte(indexes.ElementAt(2) + 1);
                            }
                            else if (count == 4)
                            {
                                CallCancalltion.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                                CallCancalltion.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                                CallCancalltion.CancelCd3 = Convert.ToByte(indexes.ElementAt(2) + 1);
                                CallCancalltion.CancelCd4 = Convert.ToByte(indexes.ElementAt(3) + 1);
                            }
                            else if (count == 5)
                            {
                                CallCancalltion.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                                CallCancalltion.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                                CallCancalltion.CancelCd3 = Convert.ToByte(indexes.ElementAt(2) + 1);
                                CallCancalltion.CancelCd4 = Convert.ToByte(indexes.ElementAt(3) + 1);
                                CallCancalltion.CancelCd5 = Convert.ToByte(indexes.ElementAt(4) + 1);
                            }
                            else if (count == 6)
                            {
                                CallCancalltion.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                                CallCancalltion.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                                CallCancalltion.CancelCd3 = Convert.ToByte(indexes.ElementAt(2) + 1);
                                CallCancalltion.CancelCd4 = Convert.ToByte(indexes.ElementAt(3) + 1);
                                CallCancalltion.CancelCd5 = Convert.ToByte(indexes.ElementAt(4) + 1);
                                CallCancalltion.CancelCd6 = Convert.ToByte(indexes.ElementAt(5) + 1);
                            }
                            else if (count == 7)
                            {
                                CallCancalltion.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                                CallCancalltion.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                                CallCancalltion.CancelCd3 = Convert.ToByte(indexes.ElementAt(2) + 1);
                                CallCancalltion.CancelCd4 = Convert.ToByte(indexes.ElementAt(3) + 1);
                                CallCancalltion.CancelCd5 = Convert.ToByte(indexes.ElementAt(4) + 1);
                                CallCancalltion.CancelCd6 = Convert.ToByte(indexes.ElementAt(5) + 1);
                                CallCancalltion.CancelCd7 = Convert.ToByte(indexes.ElementAt(6) + 1);
                            }
                            else if (count == 8)
                            {
                                CallCancalltion.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                                CallCancalltion.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                                CallCancalltion.CancelCd3 = Convert.ToByte(indexes.ElementAt(2) + 1);
                                CallCancalltion.CancelCd4 = Convert.ToByte(indexes.ElementAt(3) + 1);
                                CallCancalltion.CancelCd5 = Convert.ToByte(indexes.ElementAt(4) + 1);
                                CallCancalltion.CancelCd6 = Convert.ToByte(indexes.ElementAt(5) + 1);
                                CallCancalltion.CancelCd7 = Convert.ToByte(indexes.ElementAt(6) + 1);
                                CallCancalltion.CancelCd8 = Convert.ToByte(indexes.ElementAt(7) + 1);
                            }
                            else if (count == 9)
                            {
                                CallCancalltion.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                                CallCancalltion.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                                CallCancalltion.CancelCd3 = Convert.ToByte(indexes.ElementAt(2) + 1);
                                CallCancalltion.CancelCd4 = Convert.ToByte(indexes.ElementAt(3) + 1);
                                CallCancalltion.CancelCd5 = Convert.ToByte(indexes.ElementAt(4) + 1);
                                CallCancalltion.CancelCd6 = Convert.ToByte(indexes.ElementAt(5) + 1);
                                CallCancalltion.CancelCd7 = Convert.ToByte(indexes.ElementAt(6) + 1);
                                CallCancalltion.CancelCd8 = Convert.ToByte(indexes.ElementAt(7) + 1);
                                CallCancalltion.CancelCd9 = Convert.ToByte(indexes.ElementAt(8) + 1);
                            }
                            else if (count == 10)
                            {
                                CallCancalltion.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                                CallCancalltion.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                                CallCancalltion.CancelCd3 = Convert.ToByte(indexes.ElementAt(2) + 1);
                                CallCancalltion.CancelCd4 = Convert.ToByte(indexes.ElementAt(3) + 1);
                                CallCancalltion.CancelCd5 = Convert.ToByte(indexes.ElementAt(4) + 1);
                                CallCancalltion.CancelCd6 = Convert.ToByte(indexes.ElementAt(5) + 1);
                                CallCancalltion.CancelCd7 = Convert.ToByte(indexes.ElementAt(6) + 1);
                                CallCancalltion.CancelCd8 = Convert.ToByte(indexes.ElementAt(7) + 1);
                                CallCancalltion.CancelCd9 = Convert.ToByte(indexes.ElementAt(8) + 1);
                                CallCancalltion.CancelCd10 = Convert.ToByte(indexes.ElementAt(9) + 1);
                            }
                            else if (count == 11)
                            {
                                CallCancalltion.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                                CallCancalltion.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                                CallCancalltion.CancelCd3 = Convert.ToByte(indexes.ElementAt(2) + 1);
                                CallCancalltion.CancelCd4 = Convert.ToByte(indexes.ElementAt(3) + 1);
                                CallCancalltion.CancelCd5 = Convert.ToByte(indexes.ElementAt(4) + 1);
                                CallCancalltion.CancelCd6 = Convert.ToByte(indexes.ElementAt(5) + 1);
                                CallCancalltion.CancelCd7 = Convert.ToByte(indexes.ElementAt(6) + 1);
                                CallCancalltion.CancelCd8 = Convert.ToByte(indexes.ElementAt(7) + 1);
                                CallCancalltion.CancelCd9 = Convert.ToByte(indexes.ElementAt(8) + 1);
                                CallCancalltion.CancelCd10 = Convert.ToByte(indexes.ElementAt(9) + 1);
                                CallCancalltion.CancelCd11 = Convert.ToByte(indexes.ElementAt(10) + 1);
                            }
                            else if (count == 12)
                            {
                                CallCancalltion.CancelCd1 = Convert.ToByte(indexes.ElementAt(0) + 1);
                                CallCancalltion.CancelCd2 = Convert.ToByte(indexes.ElementAt(1) + 1);
                                CallCancalltion.CancelCd3 = Convert.ToByte(indexes.ElementAt(2) + 1);
                                CallCancalltion.CancelCd4 = Convert.ToByte(indexes.ElementAt(3) + 1);
                                CallCancalltion.CancelCd5 = Convert.ToByte(indexes.ElementAt(4) + 1);
                                CallCancalltion.CancelCd6 = Convert.ToByte(indexes.ElementAt(5) + 1);
                                CallCancalltion.CancelCd7 = Convert.ToByte(indexes.ElementAt(6) + 1);
                                CallCancalltion.CancelCd8 = Convert.ToByte(indexes.ElementAt(7) + 1);
                                CallCancalltion.CancelCd9 = Convert.ToByte(indexes.ElementAt(8) + 1);
                                CallCancalltion.CancelCd10 = Convert.ToByte(indexes.ElementAt(9) + 1);
                                CallCancalltion.CancelCd11 = Convert.ToByte(indexes.ElementAt(10) + 1);
                                CallCancalltion.CancelCd12 = Convert.ToByte(indexes.ElementAt(11) + 1);
                            }
                            CallCancalltion.CancelDesc = model.CancellationDescription;
                            CallCancalltion.UserId = model.Createdby;
                            CallCancalltion.Datetime = DateTime.Now.Date;
                            CallCancalltion.Updatedby = Convert.ToString(model.UserId);
                            CallCancalltion.Updateddate = DateTime.Now.Date;

                            context.SaveChanges();
                        }
                        if (model.CallStatus == "C" && model.CallCancelStatus == "C")
                        {
                            var callRegister = context.T17CallRegisters.FirstOrDefault(cr => cr.CaseNo == model.CaseNo && cr.CallRecvDt == model.CallRecvDt && cr.CallSno == model.CallSno);

                            if (callRegister != null)
                            {
                                short? cancelCharges = string.IsNullOrEmpty(model.CallCancelCharges) ? null : Convert.ToInt16(model.CallCancelCharges);
                                callRegister.CallStatus = model.CallStatus;
                                callRegister.CallStatusDt = model.CallStatusDt;
                                callRegister.CallCancelStatus = model.CallCancelStatus;
                                callRegister.CallCancelCharges = cancelCharges;
                                callRegister.FifoVoilateReason = wFifoVoilateReason;
                                callRegister.CallCancelChargesStatus = model.CallCancelChargesStatus;
                                callRegister.CallCancelAmount = model.CallCancelAmount;
                                context.SaveChanges();
                            }
                        }
                        else
                        {
                            var callRegister = context.T17CallRegisters.FirstOrDefault(cr => cr.CaseNo == model.CaseNo && cr.CallRecvDt == model.CallRecvDt && cr.CallSno == model.CallSno);

                            if (callRegister != null)
                            {
                                callRegister.CallStatus = model.CallStatus;
                                callRegister.CallStatusDt = model.CallStatusDt;
                                callRegister.CallCancelStatus = model.CallCancelStatus;
                                callRegister.FifoVoilateReason = wFifoVoilateReason;
                                context.SaveChanges();
                            }
                        }
                        model.AlertMsg = "Success";
                    }
                }
                else
                {
                    model.AlertMsg = "The IC is Present For give CASE_NO, CALL_RECV_DT and CALL_SNO, So it can not be cancelled!!!";
                    return model;
                }
                // send_Vendor_Email(model);
            }

            if (model.CallStatus == "C")
            {
                //Cancellation_Email(model);
                send_Vendor_Mail(model);
            }
            return model;
        }

        public VenderCallStatusModel CallStatusUploadSave(VenderCallStatusModel model, List<APPDocumentDTO> DocumentsList)
        {
            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    if (model.CallStatus == "A" || model.CallStatus == "R" || model.CallStatus == "G")
                    {
                        var count = context.T49IcPhotoEncloseds.Where(t => t.CaseNo == model.CaseNo && t.CallRecvDt == model.CallRecvDt && t.CallSno == model.CallSno && t.BkNo == model.DocBkNo && t.SetNo == model.DocSetNo).Count();
                        if (count > 0)
                        {
                            string FileName = model.CaseNo.Trim() + "-" + model.DocBkNo + "-" + model.DocSetNo;
                            var recordExists = context.T49IcPhotoEncloseds.Where(x => x.CaseNo == model.CaseNo && x.BkNo == model.DocBkNo && x.SetNo == model.DocSetNo).FirstOrDefault();
                            if (recordExists != null)
                            {
                                recordExists.IcPhoto = FileName + ".pdf";
                                recordExists.IcPhotoA1 = FileName + "-A1.PDF";
                                recordExists.IcPhotoA2 = FileName + "-A2.PDF";
                                context.SaveChanges();
                            }
                            model.AlertMsg = "Success";
                        }
                        else if (count == 0)
                        {
                            model.AlertMsg = "The Inspection Photos should be uploaded first against the given Case and then Upload the Files";
                            return model;
                        }

                    }
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                }
            }
            return model;
        }

        public VenderCallStatusModel CallStatusAcceptRej(VenderCallStatusModel model)
        {
            var groupedResults = (from t49 in context.T49IcPhotoEncloseds
                                  join ic in context.IcIntermediates
                                  on new { t49.CaseNo, t49.BkNo, t49.SetNo } equals new { ic.CaseNo, ic.BkNo, ic.SetNo }
                                  where t49.CaseNo == model.CaseNo &&
                                        t49.CallRecvDt == model.CallRecvDt &&
                                        t49.CallSno == model.CallSno &&
                                        t49.IcPhoto == null
                                  select new { t49.CaseNo, t49.BkNo, t49.SetNo })
                            .ToList() // Fetch data from the database
                            .GroupBy(item => new { item.CaseNo, item.BkNo, item.SetNo }) // Group by in-memory
                            .ToList(); // Materialize the grouped results in-memory

            var no_ic_count = groupedResults.Count();

            var no_of_photo = context.T49IcPhotoEncloseds
                        .Where(t => t.CaseNo == model.CaseNo &&
                                    t.CallRecvDt == model.CallRecvDt &&
                                    t.CallSno == model.CallSno)
                        .Count();

            if (model.CallStatus.Trim() == "" || model.CallStatus == null)
            {
                model.AlertMsg = "Your Call Status is Blank, Kindly Goto Mainmenu and select the call again to update!!!";
                return model;
            }
            else if (model.CallStatus.Trim() == "R" && model.RejectionCharge == "" || model.RejectionCharge == null)
            {
                model.AlertMsg = "Kindly Enter Rejection Charges in Case of Rejection IC!!!";
                return model;
            }
            if (model.CallStatus != "R")
            {
                if (model.ConsigneeFirm == "0")
                {
                    model.AlertMsg = "Select Consignee from the List and then Click on Accepted Button";
                    return model;
                }
                else if (no_of_photo == 0)
                {
                    model.AlertMsg = "Kindly upload the inspections photos and prepare the IC before updating the Call Status to Aceepted!!!";
                    return model;
                }
                else if (no_ic_count > 0)
                {
                    model.AlertMsg = "Kindly upload the PDF file for all ICs, Before updating the Status to Aceepted!!!";
                    return model;
                }
            }


            var callStatus = context.T17CallRegisters.Where(t => t.CaseNo == model.CaseNo && t.CallRecvDt == model.CallRecvDt && t.CallSno == model.CallSno).Select(t => t.CallStatus).FirstOrDefault();

            if (model.CallStatus != "R")
            {
                var result = context.IcIntermediates.Where(ic => ic.CaseNo == model.CaseNo && ic.CallRecvDt == model.CallRecvDt && ic.CallSno == model.CallSno).ToList();

                if (result.Count > 0)
                {
                    if (model.CallStatus == "R" && callStatus != "R")
                    {
                        foreach (var entity in result)
                        {
                            int len_item = 0;
                            if (!string.IsNullOrEmpty(entity.ItemDescPo))
                            {
                                if (entity.ItemDescPo.Length > 400)
                                {
                                    len_item = 390;
                                }
                                else
                                {
                                    len_item = entity.ItemDescPo.Length;
                                }

                                string formatedItem = entity.ItemDescPo.Substring(0, len_item);
                                var existingEntity = context.T18CallDetails.FirstOrDefault(e => e.ItemSrnoPo == entity.ItemSrnoPo && e.CaseNo == model.CaseNo && e.CallSno == model.CallSno && e.CallRecvDt == model.CallRecvDt);
                                if (existingEntity != null)
                                {
                                    existingEntity.ItemDescPo = formatedItem;
                                    existingEntity.QtyPassed = entity.QtyPassed;
                                    existingEntity.QtyRejected = entity.QtyRejected;
                                    existingEntity.QtyDue = entity.QtyDue;
                                    context.SaveChanges();
                                }
                            }
                        }

                        var existingRecord2 = context.T13PoMasters.FirstOrDefault(po => po.CaseNo == model.CaseNo);

                        existingRecord2.PendingCharges = (byte?)((existingRecord2.PendingCharges ?? 0) + 1);
                        context.SaveChanges();
                    }

                    var existingRecord = context.T17CallRegisters.FirstOrDefault(c => c.CaseNo == model.CaseNo && c.CallRecvDt == model.CallRecvDt && c.CallSno == model.CallSno);

                    if (existingRecord != null)
                    {
                        existingRecord.CallStatus = model.CallStatus;
                        existingRecord.CallStatusDt = model.CallStatusDt;
                        existingRecord.BkNo = model.DocBkNo;
                        existingRecord.SetNo = model.DocSetNo;
                        existingRecord.UserId = model.UserName;
                        existingRecord.Datetime = DateTime.Now;
                        //existingRecord.RejCharges = Convert.ToDecimal(wRejCharges);
                        existingRecord.FifoVoilateReason = model.ReasonFIFO;
                        //existingRecord.LocalOrOuts = wRejType;

                        context.SaveChanges();
                    }

                    var existingRecord1 = context.IcIntermediates.FirstOrDefault(ic => ic.CaseNo == model.CaseNo && ic.BkNo == model.DocBkNo && ic.SetNo == model.DocSetNo && ic.CallRecvDt == model.CallRecvDt && ic.CallSno == model.CallSno && ic.ConsigneeCd == Convert.ToInt32(model.ConsigneeFirm));

                    if (existingRecord1 != null)
                    {
                        existingRecord1.ConsgnCallStatus = model.CallStatus;
                        context.SaveChanges();
                    }
                    //if (model.CallStatus == "R")
                    //{
                    //    Vendor_Rej_Email(model);
                    //}
                    model.AlertMsg = "Success";

                }
                else
                {
                    model.AlertMsg = "Kindly upload the PDF file for all ICs, Before updating the Status to Aceepted/Rejected!!!";
                    return model;
                }
            }
            else
            {
                double wRejCharges = 0;
                string wRejType = "";
                //if (callStatus == "R")
                //{
                //    wRejCharges = Convert.ToDouble(model.RejectionCharge);
                //}
                wRejCharges = Convert.ToDouble(model.RejectionCharge);
                if (model.LocalOutstation != "" || model.LocalOutstation != null)
                {
                    wRejType = model.LocalOutstation;
                }
                if (model.CallStatus == "R" && callStatus != "R")
                {
                    var existingRecord2 = context.T13PoMasters.FirstOrDefault(po => po.CaseNo == model.CaseNo);

                    existingRecord2.PendingCharges = (byte?)((existingRecord2.PendingCharges ?? 0) + 1);
                    context.SaveChanges();
                }
                var existingRecord = context.T17CallRegisters.FirstOrDefault(c => c.CaseNo == model.CaseNo && c.CallRecvDt == model.CallRecvDt && c.CallSno == model.CallSno);

                if (existingRecord != null)
                {
                    existingRecord.CallStatus = model.CallStatus;
                    existingRecord.CallStatusDt = model.CallStatusDt;
                    //existingRecord.BkNo = model.DocBkNo;
                    //existingRecord.SetNo = model.DocSetNo;
                    existingRecord.UserId = model.UserName;
                    existingRecord.Datetime = DateTime.Now;
                    existingRecord.RejCharges = Convert.ToDecimal(wRejCharges);
                    existingRecord.FifoVoilateReason = model.ReasonFIFO;
                    existingRecord.LocalOrOuts = wRejType;

                    context.SaveChanges();
                }

                var existingRecord1 = context.IcIntermediates.FirstOrDefault(ic => ic.CaseNo == model.CaseNo && ic.BkNo == model.DocBkNo && ic.SetNo == model.DocSetNo && ic.CallRecvDt == model.CallRecvDt && ic.CallSno == model.CallSno && ic.ConsigneeCd == Convert.ToInt32(model.ConsigneeFirm));

                if (existingRecord1 != null)
                {
                    existingRecord1.ConsgnCallStatus = model.CallStatus;
                    context.SaveChanges();
                }
                if (model.CallStatus == "R")
                {
                    Vendor_Rej_Email(model);
                }
                model.AlertMsg = "Success";
            }


            return model;
        }

        public VenderCallStatusModel GetBkNoAndSetNoByConsignee(string CaseNo, DateTime? DesireDt, int CallSno, VenderCallStatusModel model, int selectedConsigneeCd, int IE_CD)
        {

            var ic_book = (from item in context.T10IcBooksets
                           orderby item.IssueDt descending
                           where item.IssueToIecd == IE_CD
                           select item).FirstOrDefault();

            var ICInter = context.IcIntermediates.Where(ic => ic.CaseNo == CaseNo.Trim() && ic.CallRecvDt == Convert.ToDateTime(DesireDt)
                         && ic.CallSno == CallSno).OrderByDescending(ic => ic.Datetime).ToList();

            if (ICInter.Count > 0)
            {
                foreach (var item in ICInter)
                {
                    if (selectedConsigneeCd == item.ConsigneeCd)
                    {
                        model.DocBkNo = item.BkNo;
                        model.DocSetNo = item.SetNo;
                        model.Consignee = Convert.ToString(item.ConsigneeCd);
                    }
                }
            }
            else
            {
                if (ic_book != null)
                {
                    var dlt_IC = (from x in context.IcIntermediates
                                  orderby x.SetNo descending
                                  where x.BkNo.Trim() == ic_book.BkNo.Trim() && x.IeCd == IE_CD
                                  select x).FirstOrDefault();

                    if (dlt_IC != null)
                    {
                        int setNo = Convert.ToInt32(dlt_IC.SetNo) + 1;

                        string incrementedSetNo = setNo.ToString("D3");
                        var ic_bookset = (from item in context.T10IcBooksets
                                          orderby item.IssueDt descending
                                          where item.BkNo.Trim().ToUpper() == dlt_IC.BkNo &&
                                                Convert.ToInt32(incrementedSetNo) >= Convert.ToInt32(item.SetNoFr) && Convert.ToInt32(incrementedSetNo) <= Convert.ToInt32(item.SetNoTo) &&
                                                item.IssueToIecd == dlt_IC.IeCd
                                          select item).FirstOrDefault();

                        if (ic_bookset != null)
                        {
                            model.DocBkNo = ic_bookset.BkNo;
                            model.DocSetNo = Convert.ToString(incrementedSetNo);
                        }
                        else
                        {
                            model.DocBkNo = "";
                            model.DocSetNo = "";
                        }
                    }
                    else
                    {
                        model.DocBkNo = ic_book.BkNo;
                        model.DocSetNo = Convert.ToString(ic_book.SetNoFr);
                    }
                }
            }

            return model;
        }

        public VenderCallStatusModel GetCancelChargeByStatus(string CaseNo, DateTime? DesireDt, int CallSno, string selectedValue)
        {
            double w_cancharges = 0;
            VenderCallStatusModel model = new();
            string formattedCallRecvDt = "";
            if (DesireDt != null && DesireDt != DateTime.MinValue)
            {
                DateTime parsedFromDate = DateTime.ParseExact(DesireDt.ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                formattedCallRecvDt = parsedFromDate.ToString("dd/MM/yyyy");
            }

            var rly_nonrly = context.T13PoMasters.Where(po => po.CaseNo == CaseNo).Select(po => po.RlyNonrly).FirstOrDefault();

            if (rly_nonrly == "R")
            {
                var result = (from t18 in context.T18CallDetails
                              join t15 in context.T15PoDetails on new { t18.CaseNo, t18.ItemSrnoPo } equals new { t15.CaseNo, ItemSrnoPo = t15.ItemSrno }
                              where t18.CaseNo == CaseNo && t18.CallRecvDt == Convert.ToDateTime(formattedCallRecvDt) && t18.CallSno == CallSno
                              select new
                              {
                                  t18.CaseNo,
                                  t18.CallRecvDt,
                                  t18.CallSno,
                                  Value = t15.Value != null && t15.Qty != null && t18.QtyToInsp != null
                          ? ((decimal)t15.Value / (decimal)t15.Qty) * (decimal)t18.QtyToInsp
                          : 0
                              })
                          .GroupBy(x => new { x.CaseNo, x.CallRecvDt, x.CallSno })
                          .Select(group => new { Value = Math.Round(group.Sum(x => (decimal)x.Value), 2) })
                          .FirstOrDefault();

                if (result != null)
                {
                    model.MaterialValue = Convert.ToString(result.Value);
                    decimal callCancelCharges = result.Value * (decimal)0.9 / 100;
                    model.CallCancelCharges = Math.Round(callCancelCharges).ToString();
                }
                else
                {
                    model.MaterialValue = "";
                    model.CallCancelCharges = "";
                }
                if (selectedValue == "A")
                {

                    w_cancharges = Math.Round(Convert.ToDouble(model.CallCancelCharges));
                    if (w_cancharges < 22000)
                    {
                        model.CallCancelCharges = Convert.ToString(w_cancharges);
                    }
                    else
                    {
                        model.CallCancelCharges = Convert.ToString(22000);
                    }
                }
                //else if(selectedValue == "B")
                else
                {
                    w_cancharges = Math.Round(Convert.ToDouble(model.CallCancelCharges) / 2);
                    if (w_cancharges < 11000)
                    {
                        model.CallCancelCharges = Convert.ToString(w_cancharges);
                    }
                    else
                    {
                        model.CallCancelCharges = Convert.ToString(11000);
                    }
                }
            }
            else
            {
                model.CallCancelCharges = Convert.ToString(selectedValue);
            }
            model.RlyNonrly = rly_nonrly;
            return model;
        }

        public VenderCallStatusModel GetRlyDrp(string CaseNo, DateTime? DesireDt, int CallSno, string selectedValue, string IeCd, string Region)
        {
            string formattedCallRecvDt = "";
            VenderCallStatusModel model = new();
            if (DesireDt != null && DesireDt != DateTime.MinValue)
            {
                DateTime parsedFromDate = DateTime.ParseExact(DesireDt.ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                formattedCallRecvDt = parsedFromDate.ToString("dd/MM/yyyy");
            }
            if (selectedValue == "C")
            {
                var rly_nonrly = context.T13PoMasters.Where(po => po.CaseNo == CaseNo).Select(po => po.RlyNonrly).FirstOrDefault();


                if (rly_nonrly == "R")
                {
                    model = new VenderCallStatusModel
                    {
                        CallRlyFirmList = new List<SelectListItem>
                        {
                            new SelectListItem { Value = "B", Text = "Before Visit of IE to Vendor's premises (AS per Railway Board Order No. 99/RS(G)/709/4 Dated: 12-02/2016)" },
                            new SelectListItem { Value = "A", Text = "After Visit of IE to Vendor's Premises (As per Railway Board Order No. 99/RS(G)/709/4 Dated: 12-02/2016)" }
                        }
                    };
                }
                else
                {
                    model = new VenderCallStatusModel
                    {
                        CallRlyFirmList = new List<SelectListItem>
                        {
                            new SelectListItem { Value = "3000", Text = "Before Visit of IE to Vendor's premises" },
                            new SelectListItem { Value = "10000", Text = "After Visit of IE to Vendor Premises - Local" },
                            new SelectListItem { Value = "15000", Text = "After Visit of IE to Vendor Premises - Out Station" }
                        }
                    };
                }
            }
            if (selectedValue != "A" && selectedValue != "R")
            {
                var callCount = context.T17CallRegisters.Where(t => t.DtInspDesire < Convert.ToDateTime(formattedCallRecvDt) && t.CallStatus == "M" && t.IeCd == Convert.ToInt32(IeCd) &&
                            t.RegionCode == Region && t.CallRecvDt > DateTime.ParseExact("01/04/2021", "dd/MM/yyyy", null)).Count();

                if (callCount > 0)
                {
                    model.AlertMsg = "You are Voilating the FIFO for attending Calls, kindly enter the reason for voilating FIFO!!!";
                    model.ChkFIFO = "true";
                    return model;
                }
                else
                {
                    model.ChkFIFO = "false";
                }
            }
            model.AlertMsg = "Success!!!";
            return model;
        }

        public VenderCallStatusModel GetLocalOutstation(string CaseNo, DateTime? DesireDt, int CallSno, string selectedValue)
        {
            VenderCallStatusModel model = new();
            string formattedCallRecvDt = "";
            if (DesireDt != null && DesireDt != DateTime.MinValue)
            {
                DateTime parsedFromDate = DateTime.ParseExact(DesireDt.ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                formattedCallRecvDt = parsedFromDate.ToString("dd/MM/yyyy");
            }
            var rly_nonrly = context.T13PoMasters.Where(po => po.CaseNo == CaseNo).Select(po => po.RlyNonrly).FirstOrDefault();

            var SumValue = (from t18 in context.T18CallDetails
                            join t15 in context.T15PoDetails on new { t18.CaseNo, t18.ItemSrnoPo } equals new { t15.CaseNo, ItemSrnoPo = t15.ItemSrno }
                            where t18.CaseNo == CaseNo && t18.CallRecvDt == Convert.ToDateTime(formattedCallRecvDt) && t18.CallSno == CallSno
                            select new
                            {
                                t18.CaseNo,
                                t18.CallRecvDt,
                                t18.CallSno,
                                Value = t15.Value != null && t15.Qty != null && t18.QtyToInsp != null
                        ? ((decimal)t15.Value / (decimal)t15.Qty) * (decimal)t18.QtyToInsp
                        : 0
                            })
                              .GroupBy(x => new { x.CaseNo, x.CallRecvDt, x.CallSno })
                              .Select(group => new { Value = Math.Round(group.Sum(x => (decimal)x.Value), 2) })
                              .FirstOrDefault();
            model.RejectMaterialValue = Convert.ToString(SumValue.Value);
            double w_cancharges = 0;
            if (rly_nonrly == "R")
            {
                decimal callCancelCharges = SumValue.Value * (decimal)0.9 / 100;
                model.RejectionCharge = callCancelCharges.ToString();

                w_cancharges = Math.Round(Convert.ToDouble(model.RejectionCharge), 2);
                if (w_cancharges < 5000)
                {
                    model.RejectionCharge = "5000";
                }
            }
            else if (rly_nonrly != "R")
            {
                model.RejectionCharge = Convert.ToString(SumValue.Value * 1 / 100);
                w_cancharges = Math.Round(Convert.ToDouble(model.RejectionCharge), 2);

                var no_of_visits = context.T47IeWorkPlans.Where(t => t.CaseNo == CaseNo && t.CallRecvDt == Convert.ToDateTime(DesireDt) && t.CallSno == CallSno).Count();
                double w_rejcharges = 0;
                if (selectedValue == "L")
                {
                    w_rejcharges = no_of_visits * 10000;
                    if (w_rejcharges > w_cancharges)
                    {
                        model.RejectionCharge = Convert.ToString(w_rejcharges);
                    }
                }
                if (selectedValue == "O")
                {
                    w_rejcharges = no_of_visits * 15000;
                    if (w_rejcharges > w_cancharges)
                    {
                        model.RejectionCharge = Convert.ToString(w_rejcharges);
                    }
                }
            }


            return model;
        }

        public bool CallDetailsRemove(VendrorCallDetailsModel model)
        {
            var itemDelete = context.T18CallDetails.FirstOrDefault(x => x.CaseNo == model.CaseNo && x.CallRecvDt == model.CallRecvDt && x.CallSno == model.CallSno);

            context.T18CallDetails.Remove(itemDelete);
            context.SaveChanges();
            return true;
        }

        public bool SaveRPTPRMInspectionCertificate(string CASE_NO, string CALL_RECV_DT, string CALL_SNO, string CONSIGNEE_CD)
        {
            var flag = true;
            CALL_RECV_DT = Convert.ToDateTime(CALL_RECV_DT).ToString("MM/dd/yyyy");
            using ModelContext cont = new(DbContextHelper.GetDbContextOptions());
            using (var command = cont.Database.GetDbConnection().CreateCommand())
            {
                var trans = cont.Database.BeginTransaction();
                bool wasOpen = command.Connection.State == System.Data.ConnectionState.Open;
                if (!wasOpen) command.Connection.Open();
                try
                {
                    //command.Transaction = trans;
                    command.CommandText = "select COUNT(*) from RPT_PRM_Inspection_Certificate where CASE_NO= '" + CASE_NO + "' and  CALL_SNO= '" + CALL_SNO + "' and CALL_RECV_DT= to_date('" + CALL_RECV_DT + "','mm/dd/yyyy') and CONSIGNEE_CD = '" + CONSIGNEE_CD + "' ";
                    var res = Convert.ToInt32(command.ExecuteScalar());

                    var qry = "";
                    if (res <= 0)
                    {
                        command.CommandText = "INSERT INTO RPT_PRM_Inspection_Certificate VALUES ('" + CASE_NO + "', to_date('" + CALL_RECV_DT + "','mm/dd/yyyy'), " + CALL_SNO + " , NULL, NULL , CURRENT_TIMESTAMP,'" + CONSIGNEE_CD + "')";
                        res = command.ExecuteNonQuery();
                    }

                    qry = "MERGE INTO RPT_PRM_Inspection_Certificate RP USING ";
                    qry += "( SELECT CASE_NO, CALL_SNO, CALL_RECV_DT, COUNT(*) as NUM_VISITS, LISTAGG(TO_CHAR(Visit_DT, 'DD.MM.YYYY'), ', ') within group (order by Visit_DT ) as VISIT_DATES ";
                    qry += "    FROM T47_IE_WORK_PLAN ";
                    qry += "   WHERE CASE_NO = '" + CASE_NO + "' and CALL_SNO = " + CALL_SNO + " and CALL_RECV_DT = to_date('" + CALL_RECV_DT + "','mm/dd/yyyy') ";
                    qry += "  GROUP BY CASE_NO, CALL_SNO, CALL_RECV_DT) WP ";
                    qry += "ON(RP.CASE_NO = WP.CASE_NO AND RP.CALL_SNO = WP.CALL_SNO AND RP.CALL_RECV_DT = WP.CALL_RECV_DT) ";
                    qry += "WHEN MATCHED THEN UPDATE SET ";
                    qry += "RP.NUM_VISITS = WP.NUM_VISITS, ";
                    qry += "RP.VISIT_DATES = WP.VISIT_DATES";

                    command.CommandText = qry;
                    res = command.ExecuteNonQuery();
                    trans.Commit();
                    flag = true;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    flag = false;
                }
                finally
                {
                    if (!wasOpen) command.Connection.Close();
                }
            }
            return flag;
        }
    }
}
