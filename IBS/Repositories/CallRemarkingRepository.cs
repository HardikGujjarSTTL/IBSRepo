using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace IBS.Repositories
{
    public class CallRemarkingRepository : ICallRemarkingRepository
    {
        private readonly ModelContext context;
        private readonly IConfiguration config;
        private readonly ISendMailRepository pSendMailRepository;

        public CallRemarkingRepository(ModelContext context, IConfiguration _config, ISendMailRepository _pSendMailRepository)
        {
            this.context = context;
            config = _config;
            pSendMailRepository = _pSendMailRepository;
        }

        public int GetPendingCallsFromIE(string Region, int IeCd)
        {
            return context.T17CallRegisters.Where(x => x.CallStatus == "M" && x.CaseNo.Substring(0, 1) == Region && x.IeCd == IeCd).Count();
        }

        public DTResult<PendingCallsListModel> GetPendingCallsList1(DTParameters dtParameters)
        {
            DTResult<PendingCallsListModel> dTResult = new() { draw = 0 };
            IQueryable<PendingCallsListModel>? query = null;

            string Region = !string.IsNullOrEmpty(dtParameters.AdditionalValues["Region"]) ? Convert.ToString(dtParameters.AdditionalValues["Region"]) : "";
            int IeCd = !string.IsNullOrEmpty(dtParameters.AdditionalValues["IeCd"]) ? int.Parse(dtParameters.AdditionalValues["IeCd"]) : 0;

            query = from t17 in context.T17CallRegisters
                    join t05 in context.T05Vendors on t17.MfgCd equals t05.VendCd
                    join t03 in context.T03Cities on t05.VendCityCd equals t03.CityCd
                    join t108 in context.T108RemarkedCalls on new { t17.CaseNo, t17.CallRecvDt, t17.CallSno }
                           equals new { CaseNo = Convert.ToString(t108.CaseNo), CallRecvDt = (DateTime)t108.CallRecvDt, CallSno = (int)t108.CallSno } into t108Group
                    from t108 in t108Group.Where(x => x.RemarkingStatus == "P").DefaultIfEmpty()
                    where t108.CaseNo == null && t108.CallRecvDt == null && t108.CallSno == null
                    && t17.CaseNo.Substring(0, 1) == Region
                    && t17.IeCd == IeCd
                    && t17.CallStatus == "M"
                    select new PendingCallsListModel
                    {
                        CaseNo = t17.CaseNo,
                        CallRecvDt = t17.CallRecvDt,
                        CallSno = t17.CallSno,
                        CallStatus = t17.CallStatus == "M" ? "Pending" : null,
                        MfgCd = t17.MfgCd,
                        MfgPlace = t17.MfgPlace,
                        Mfg_CityCd = t03.CityCd,
                        Mfg_City = t03.City,
                        CoCd = t17.CoCd ?? 0,
                        VendName = t05.VendName,
                        DtInspDesire = t17.DtInspDesire,
                        CallRemarkStatus = t17.CallRemarkStatus ?? "0"
                    };

            dTResult.recordsTotal = query.Count();

            dTResult.recordsFiltered = query.Count();

            if (dtParameters.Length == -1) dtParameters.Length = query.Count();

            dTResult.data = query.Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;

        }

        public DTResult<PendingCallsListModel> GetPendingCallsList2(DTParameters dtParameters)
        {
            DTResult<PendingCallsListModel> dTResult = new() { draw = 0 };
            IQueryable<PendingCallsListModel>? query = null;

            string Region = !string.IsNullOrEmpty(dtParameters.AdditionalValues["Region"]) ? Convert.ToString(dtParameters.AdditionalValues["Region"]) : "";
            int IeCd = !string.IsNullOrEmpty(dtParameters.AdditionalValues["IeCd"]) ? int.Parse(dtParameters.AdditionalValues["IeCd"]) : 0;

            query = from t108 in context.T108RemarkedCalls
                    join t09From in context.T09Ies on (int)t108.FrIeCd equals t09From.IeCd
                    join t10To in context.T09Ies on (int)t108.ToIeCd equals t10To.IeCd
                    join t02 in context.T02Users on t108.RemInitBy equals t02.UserId
                    where t108.RemarkingStatus == "P" && t108.CaseNo.Substring(0, 1) == Region && t108.FrIeCd == IeCd
                    select new PendingCallsListModel
                    {
                        CaseNo = t108.CaseNo,
                        CallRecvDt = t108.CallRecvDt,
                        CallSno = t108.CallSno,
                        CallRemarkStatus = t108.RemarkingStatus == "P" ? "Pending" : null,
                        FrIeName = t09From.IeName,
                        ToIeName = t10To.IeName,
                        UserName = t02.UserName,
                        RemInitDatetime = t108.RemInitDatetime
                    };

            dTResult.recordsTotal = query.Count();

            dTResult.recordsFiltered = query.Count();

            if (dtParameters.Length == -1) dtParameters.Length = query.Count();

            dTResult.data = query.Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;

        }

        public void SaveDetails(CallRemarkingModel model)
        {
            if (string.IsNullOrEmpty(model.CaseNos)) return;

            string[] CaseNos = model.CaseNos.Split(",");

            foreach (var details in CaseNos)
            {
                string[] data = details.Split("##");
                if (data.Length == 3)
                {
                    T108RemarkedCall remarkedCall = new()
                    {
                        CaseNo = data[0],
                        CallRecvDt = Convert.ToDateTime(data[1]),
                        CallSno = int.Parse(data[2]),
                        RemarkReason = model.RemarkingStatus,
                        FrIeCd = model.FrIeCd,
                        ToIeCd = model.ToIeCd,
                        RemInitBy = model.RemInitBy,
                        RemInitDatetime = DateTime.Now,
                        RemarkingStatus = "P",
                        FrIePendingCalls = model.FrIePendingCalls,
                        ToIePendingCalls = model.ToIePendingCalls
                    };

                    context.T108RemarkedCalls.Add(remarkedCall);
                }
            }

            context.SaveChanges();
        }

        public DTResult<PendingCallsListModel> GetCallRemarkingListForApproval(DTParameters dtParameters)
        {
            DTResult<PendingCallsListModel> dTResult = new() { draw = 0 };
            IQueryable<PendingCallsListModel>? query = null;

            string Region = !string.IsNullOrEmpty(dtParameters.AdditionalValues["Region"]) ? Convert.ToString(dtParameters.AdditionalValues["Region"]) : "";
            int UserId = !string.IsNullOrEmpty(dtParameters.AdditionalValues["UserId"]) ? int.Parse(dtParameters.AdditionalValues["UserId"]) : 0;

            var CallRemarking = context.T02Users.Where(x => x.Id == UserId).Select(x => x.CallRemarking).FirstOrDefault();

            if (CallRemarking != "Y")
            {
                List<PendingCallsListModel> lst = new();
                dTResult.recordsTotal = 0;
                dTResult.recordsFiltered = 0;
                dtParameters.Length = 0;
                dTResult.data = lst.ToList();
                dTResult.draw = dtParameters.Draw;
                return dTResult;
            }

            var UserType = context.T02Users.Where(x => x.Id == UserId).Select(x => x.UserType).FirstOrDefault();

            if (UserType == "S")
            {
                query = from t108 in context.T108RemarkedCalls
                        join t09From in context.T09Ies on t108.FrIeCd equals t09From.IeCd
                        join t10To in context.T09Ies on t108.ToIeCd equals t10To.IeCd
                        join t02 in context.T02Users on t108.RemInitBy equals t02.UserId
                        join t17 in context.T17CallRegisters on new { t108.CaseNo, CallRecvDt = (DateTime)t108.CallRecvDt, CallSno = (int)t108.CallSno }
                            equals new { t17.CaseNo, t17.CallRecvDt, t17.CallSno }
                        where t108.RemarkingStatus == "P" && t108.CaseNo.Substring(0, 1) == Region
                        select new PendingCallsListModel
                        {
                            Id = t108.Id,
                            CaseNo = t108.CaseNo,
                            CallRecvDt = t108.CallRecvDt,
                            CallSno = t108.CallSno,
                            CallRemarkStatus = t108.RemarkingStatus == "P" ? "Pending" : null,
                            RemarkReason = t108.RemarkReason,
                            FrIeName = t09From.IeName,
                            ToIeName = t10To.IeName,
                            FrIePendingCalls = t108.FrIePendingCalls,
                            ToIePendingCalls = t108.ToIePendingCalls,
                            UserName = t02.UserName,
                            RemInitDatetime = t108.RemInitDatetime,
                        };
            }
            else
            {
                query = from t108 in context.T108RemarkedCalls
                        join t09From in context.T09Ies on t108.FrIeCd equals t09From.IeCd
                        join t10To in context.T09Ies on t108.ToIeCd equals t10To.IeCd
                        join t02 in context.T02Users on t108.RemInitBy equals t02.UserId
                        join t17 in context.T17CallRegisters on new { t108.CaseNo, CallRecvDt = (DateTime)t108.CallRecvDt, CallSno = (int)t108.CallSno }
                            equals new { t17.CaseNo, t17.CallRecvDt, t17.CallSno }
                        where t108.RemarkingStatus == "P" && t108.CaseNo.Substring(0, 1) == Region
                         && (t17.CallRemarkStatus == null || t17.CallRemarkStatus == "0")
                        select new PendingCallsListModel
                        {
                            Id = t108.Id,
                            CaseNo = t108.CaseNo,
                            CallRecvDt = t108.CallRecvDt,
                            CallSno = t108.CallSno,
                            CallRemarkStatus = t108.RemarkingStatus == "P" ? "Pending" : null,
                            RemarkReason = t108.RemarkReason,
                            FrIeName = t09From.IeName,
                            ToIeName = t10To.IeName,
                            FrIePendingCalls = t108.FrIePendingCalls,
                            ToIePendingCalls = t108.ToIePendingCalls,
                            UserName = t02.UserName,
                            RemInitDatetime = t108.RemInitDatetime,
                        };
            }

            dTResult.recordsTotal = query.Count();

            dTResult.recordsFiltered = query.Count();

            if (dtParameters.Length == -1) dtParameters.Length = query.Count();

            dTResult.data = query.Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;

        }

        public CallRemarkingApprovalModel FindCallRemarkingDetailsForApproval(int id)
        {
            CallRemarkingApprovalModel model = new();

            model = (from t108 in context.T108RemarkedCalls
                     join t09From in context.T09Ies on t108.FrIeCd equals t09From.IeCd
                     join t10To in context.T09Ies on t108.ToIeCd equals t10To.IeCd
                     join t02 in context.T02Users on t108.RemInitBy equals t02.UserId
                     join t08 in context.T08IeControllOfficers on t10To.IeCoCd equals t08.CoCd
                     join t17 in context.T17CallRegisters on new { t108.CaseNo, CallRecvDt = (DateTime)t108.CallRecvDt, CallSno = (int)t108.CallSno }
                         equals new { t17.CaseNo, t17.CallRecvDt, t17.CallSno }
                     join v05 in context.V05Vendors on t17.MfgCd equals v05.VendCd
                     join t18 in context.T18CallDetails on new { t17.CaseNo, t17.CallRecvDt, t17.CallSno }
                         equals new { t18.CaseNo, t18.CallRecvDt, t18.CallSno }
                     where t108.RemarkingStatus == "P" && t108.Id == id
                        && t18.ItemSrnoPo == (from b in context.T18CallDetails
                                              where b.CaseNo == t18.CaseNo && b.CallRecvDt == t18.CallRecvDt && b.CallSno == t18.CallSno
                                              select b.ItemSrnoPo).Min()
                     select new CallRemarkingApprovalModel
                     {
                         Id = t108.Id,
                         CaseNo = t108.CaseNo,
                         CallRecvDt = t108.CallRecvDt,
                         CallSno = t108.CallSno,
                         RemarkingStatus = t108.RemarkingStatus == "P" ? "Pending" : null,
                         RemarkReason = t108.RemarkReason,
                         FrIeName = t09From.IeName,
                         ToIeName = t10To.IeName,
                         FrIePendingCalls = t108.FrIePendingCalls,
                         ToIePendingCalls = t108.ToIePendingCalls,
                         IeCd = t10To.IeCd,
                         CoCd = t08.CoCd,
                         UserName = t02.UserName,
                         CALL_DES_DT = t17.DtInspDesire,
                         Mfg = v05.VendCd + "-" + v05.Vendor,
                         ItemDescPo = t18.ItemDescPo,
                         CallRemarkStatus = t108.CallRemarkStatus ?? "0",
                         COUNT = (from a in context.T18CallDetails
                                  where a.CaseNo == t18.CaseNo
                                     && a.CallRecvDt == t18.CallRecvDt
                                     && a.CallSno == t18.CallSno
                                  select a).Count()
                     }).FirstOrDefault();

            if (model != null)
            {
                if (model.COUNT > 1)
                {
                    model.ItemDescPo = model.ItemDescPo + " and more items as per call";
                }

                OracleParameter[] par = new OracleParameter[4];
                par[0] = new OracleParameter("p_call_recv_dt", OracleDbType.Date, model.CallRecvDt, ParameterDirection.Input);
                par[1] = new OracleParameter("p_case_no", OracleDbType.Varchar2, model.CaseNo, ParameterDirection.Input);
                par[2] = new OracleParameter("p_call_sno", OracleDbType.Int32, model.CallSno, ParameterDirection.Input);
                par[3] = new OracleParameter("p_result_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

                var ds = DataAccessDB.GetDataSet("SP_GET_CALL_REMARK_APPROVAL_DETAILS", par, 1);

                if (ds != null && ds.Tables.Count > 0)
                {
                    string RLY = string.Empty;
                    string CLIENT_RLY = string.Empty;
                    decimal? MAT_VALUE = null;
                    DateTime? EXT_DELV_DT = null;
                    DateTime? INSP_DESIRE_DT = null;
                    string PENDING_SINCE = string.Empty;

                    if (ds.Tables[0].Rows[0]["RLY"] != null) RLY = Convert.ToString(ds.Tables[0].Rows[0]["RLY"]);
                    if (ds.Tables[0].Rows[0]["CLIENT_RLY"] != null) CLIENT_RLY = Convert.ToString(ds.Tables[0].Rows[0]["CLIENT_RLY"]);
                    if (ds.Tables[0].Rows[0]["MAT_VALUE"] != null) MAT_VALUE = Convert.ToDecimal(ds.Tables[0].Rows[0]["MAT_VALUE"]);
                    if (ds.Tables[0].Rows[0]["EXT_DELV_DT"] != null) EXT_DELV_DT = Convert.ToDateTime(ds.Tables[0].Rows[0]["EXT_DELV_DT"]);
                    if (ds.Tables[0].Rows[0]["INSP_DESIRE_DT"] != null) INSP_DESIRE_DT = Convert.ToDateTime(ds.Tables[0].Rows[0]["INSP_DESIRE_DT"]);
                    if (ds.Tables[0].Rows[0]["PENDING_SINCE"] != null) PENDING_SINCE = Convert.ToString(ds.Tables[0].Rows[0]["PENDING_SINCE"]);

                    model.RLY = RLY + " & " + CLIENT_RLY;
                    model.MAT_VALUE = MAT_VALUE;
                    model.ExtDelvDt = EXT_DELV_DT;
                    model.DtInspDesire = INSP_DESIRE_DT;
                    model.pending_since = PENDING_SINCE + " Days";
                }
            }

            return model;
        }

        public void SaveDetails(CallRemarkingApprovalModel model)
        {
            if (model.Action == "Approve")
            {
                model.CallRemarkStatus = Convert.ToString(Convert.ToInt32(model.CallRemarkStatus) + 1);

                T17CallRegister callRegister = context.T17CallRegisters.Where(x => x.CaseNo == model.CaseNo && x.CallRecvDt == model.CallRecvDt && x.CallSno == model.CallSno).FirstOrDefault();

                if (callRegister != null)
                {
                    callRegister.IeCd = model.IeCd;
                    callRegister.CoCd = model.CoCd;
                    callRegister.CallRemarkStatus = model.CallRemarkStatus;
                    callRegister.DtInspDesire = model.DtInspDesire;

                    context.SaveChanges();
                }

                T108RemarkedCall remarkedCall = context.T108RemarkedCalls.Find(model.Id);

                if (remarkedCall != null)
                {
                    remarkedCall.CallRemarkStatus = model.CallRemarkStatus;
                    remarkedCall.RemarkingStatus = "A";
                    remarkedCall.RemAppBy = model.UserId;
                    remarkedCall.RemAppDatetime = DateTime.Now;
                    remarkedCall.RemRejRemark = model.Remark;

                    context.SaveChanges();
                }
            }
            else if (model.Action == "Reject")
            {
                T108RemarkedCall remarkedCall = context.T108RemarkedCalls.Find(model.Id);

                if (remarkedCall != null)
                {
                    remarkedCall.RemarkingStatus = "R";
                    remarkedCall.RemAppBy = model.UserId;
                    remarkedCall.RemAppDatetime = DateTime.Now;
                    remarkedCall.RemRejRemark = model.Remark;

                    context.SaveChanges();
                }
            }

            Send_Email(model);
        }

        public string Send_Email(CallRemarkingApprovalModel model)
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
                            VEND_EMAIL = t05.VendEmail,
                            PoNo = t13.PoNo,
                            PoDt = t13.PoDt
                        };

            var result = query.FirstOrDefault();

            int vend_cd = 0;
            string vend_add = "";
            string vend_email = "";
            string vend_name = "";
            string vend_city = "";
            string PoNo = "";
            string PoDt = "";

            if (result != null)
            {
                vend_cd = Convert.ToInt32(result.VEND_CD);
                vend_add = result.VEND_ADDRESS;
                vend_email = result.VEND_EMAIL;
                vend_name = result.VEND_NAME;
                PoNo = Convert.ToString(result.PoNo);
                PoDt = Convert.ToString(result.PoDt);
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
            string manu_name = "", manu_add = "", CallLetterDt = "";
            var manufacturerInfo = (from t17 in context.T17CallRegisters
                                    join t05 in context.T05Vendors on t17.MfgCd equals t05.VendCd
                                    join t03 in context.T03Cities on t05.VendCityCd equals t03.CityCd
                                    where t17.CaseNo == model.CaseNo &&
                                    t17.CallRecvDt == model.CallRecvDt &&
                                    t17.CallSno == model.CallSno
                                    select new
                                    {
                                        manu_name = t05.VendName,
                                        manu_add = t03.City,
                                        CallLetterDt = t17.CallLetterDt
                                    }).FirstOrDefault();
            string call_letter_dt = "";
            if (Convert.ToString(manufacturerInfo.CallLetterDt) == null)
            {
                call_letter_dt = "NIL";
            }
            else
            {
                call_letter_dt = Convert.ToString(manufacturerInfo.CallLetterDt);
            }
            string mail_body = "Dear Sir/Madam,<br><br> In Reference to your Call Letter dated:  " + Convert.ToDateTime(call_letter_dt).ToString("dd/MM/yyyy") + " for inspection of material against PO No. - " + PoNo + " & date - " + Convert.ToDateTime(PoDt).ToString("dd/MM/yyyy") + ", Call has been registered vide Case No -  " + model.CaseNo + ", on date: " + Convert.ToDateTime(model.CallRecvDt).ToString("dd/MM/yyyy") + ", at SNo. " + model.CallSno + ".<br> ";
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
            
            string ActionSubject="";
            if(model.Action == "A")
            {
                ActionSubject = "Your Call Remarked Approved for Inspection By RITES";
            }
            else
            {
                ActionSubject = "Your Call Remarked Rejected for Inspection By RITES";
            }
            if (vend_cd == mfg_cd && manu_mail != "")
            {
                if (Convert.ToString(config.GetSection("MailConfig")["SendMail"]) == "1")
                {
                    SendMailModel sendMailModel = new SendMailModel();
                    // sender for local mail testing
                    sendMailModel.From = sender;
                    sendMailModel.To = manu_mail;
                    sendMailModel.Bcc = "nrinspn@gmail.com";
                    sendMailModel.Subject = ActionSubject;
                    sendMailModel.Message = mail_body;
                    isSend = pSendMailRepository.SendMail(sendMailModel, null);
                }
            }
            else if (vend_cd != mfg_cd && vend_email != null && manu_mail != null)
            {
                if (Convert.ToString(config.GetSection("MailConfig")["SendMail"]) == "1")
                {
                    SendMailModel sendMailModel = new SendMailModel();
                    // sender for local mail testing
                    sendMailModel.From = sender;
                    sendMailModel.To = vend_email + ";" + manu_mail;
                    sendMailModel.Bcc = "nrinspn@gmail.com";
                    sendMailModel.Subject = ActionSubject;
                    sendMailModel.Message = mail_body;
                    isSend = pSendMailRepository.SendMail(sendMailModel, null);
                }
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
                    sendMailModel.Subject = ActionSubject;
                    sendMailModel.Message = mail_body;
                    isSend = pSendMailRepository.SendMail(sendMailModel, null);
                }
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
            }
            return email;
        }
    }
}

