using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using System.Dynamic;
using System.Globalization;

namespace IBS.Repositories
{
    public class CMIEWiseCancellationAcceptance_FormRepository : ICMIEWiseCancellationAcceptance_FormRepository
    {

            private readonly ModelContext context;
            public CMIEWiseCancellationAcceptance_FormRepository(ModelContext context)
            {
                this.context = context;
            }


        public CMIEWiseCancellationAcceptance_FormModel   GetIEsByRegionAndCO(string selectedValue , string region)
        {
            CMIEWiseCancellationAcceptance_FormModel query = null;

            var query1 =  context.T09Ies
                .Where(ie => ie.IeStatus == null && ie.IeRegion == region && ie.IeCoCd == Convert.ToInt32(selectedValue))
                .OrderBy(ie => ie.IeName)
                .Select(ie => new CMIEWiseCancellationAcceptance_FormModel
                {
                    IE_CD = Convert.ToInt32(ie.IeCoCd),
                    IE_NAME = ie.IeName
                })
                .ToList();

            query = query1.FirstOrDefault(); 
            return query;
        }

        public DTResult<CMIEWiseCancellationAcceptance_FormModel> CMIEWTable(DTParameters dtParameters , string Region)
        {
            int lstCO = Convert.ToInt32(dtParameters.AdditionalValues?.GetValueOrDefault("lstCO"));
            DTResult<CMIEWiseCancellationAcceptance_FormModel> dTResult = new() { draw = 0 };

            var query1 = from t17 in context.T17CallRegisters
                        join t19 in context.T19CallCancels on
                            new { t17.CaseNo, t17.CallRecvDt, t17.CallSno } equals new { t19.CaseNo, t19.CallRecvDt, t19.CallSno }
                        join t05 in context.T05Vendors on t17.MfgCd equals t05.VendCd
                         where t17.CallStatus == "C" &&
                               t19.CancelDate > DateTime.ParseExact("04/08/2019", "dd/MM/yyyy", CultureInfo.InvariantCulture) &&
                               t19.DocsSubmitted == null &&
                               t17.CaseNo.StartsWith(Region) &&
                               t17.CoCd == (int)lstCO
                         orderby t17.CallRecvDt, t05.VendName, t17.CallRecvDt, t17.CallSno
                         select new CMIEWiseCancellationAcceptance_FormModel
                         {
                           CASE_NO =  t17.CaseNo,
                             CALL_RECV_DATE = t17.CallRecvDt.ToString("dd/MM/yyyy"), // Format as "dd/mm/yyyy"
                             CALL_DT_CONCAT = t17.CallRecvDt.ToString("yyyyMMdd"), // Format as "yyyymmdd"
                             CALL_SNO =  t17.CallSno,
                           MFG_CD =  Convert.ToInt32(t17.MfgCd),
                           MFG_PLACE =  t17.MfgPlace,
                            CO_CD = t17.CoCd ?? 0, // Coalesce to 0 if CO_CD is null
                            MFG = t05.VendName,
                            DESIRE_DT = Convert.ToString(t17.DtInspDesire),
                            CANC_DOC = "CALL_CANCELLATION_DOCUMENTS/" + t17.CaseNo +
                                       "-" + t17.CallRecvDt +
                                       "-" + t17.CallSno + ".PDF"
                         };

            var result = query1.ToList();

            dTResult.recordsTotal = query1.Count();
            dTResult.data = result;
            dTResult.recordsFiltered = query1.Count();
            return dTResult;
        }

        public DTResult<CMIEWiseCancellationAcceptance_FormModel> CMIEWTable1(DTParameters dtParameters, string Region)
        {
                int lstIE = Convert.ToInt32(dtParameters.AdditionalValues?.GetValueOrDefault("lstIE"));
            DTResult<CMIEWiseCancellationAcceptance_FormModel> dTResult = new() { draw = 0 };

            var query1 = from t17 in context.T17CallRegisters
                         join t19 in context.T19CallCancels on
                             new { t17.CaseNo, t17.CallRecvDt, t17.CallSno } equals new { t19.CaseNo, t19.CallRecvDt, t19.CallSno }
                         join t05 in context.T05Vendors on t17.MfgCd equals t05.VendCd
                         where t17.CallStatus == "C" &&
                               t19.CancelDate > DateTime.ParseExact("04/08/2019", "dd/MM/yyyy", CultureInfo.InvariantCulture) &&
                               t19.DocsSubmitted == null &&
                               t17.CaseNo.StartsWith(Region) &&
                               t17.CoCd == (int)lstIE
                         orderby t17.CallRecvDt, t05.VendName, t17.CallRecvDt, t17.CallSno
                         select new CMIEWiseCancellationAcceptance_FormModel
                         {
                             CASE_NO = t17.CaseNo,
                             CALL_RECV_DATE = t17.CallRecvDt.ToString("dd/MM/yyyy"), // Format as "dd/mm/yyyy"
                             CALL_DT_CONCAT = t17.CallRecvDt.ToString("yyyyMMdd"), // Format as "yyyymmdd"
                             CALL_SNO = t17.CallSno,
                             MFG_CD = Convert.ToInt32(t17.MfgCd),
                             MFG_PLACE = t17.MfgPlace,
                             CO_CD = t17.CoCd ?? 0, // Coalesce to 0 if CO_CD is null
                             MFG = t05.VendName,
                             DESIRE_DT = Convert.ToString(t17.DtInspDesire),
                             CANC_DOC = "CALL_CANCELLATION_DOCUMENTS/" + t17.CaseNo +
                                       "-" + t17.CallRecvDt +
                                       "-" + t17.CallSno + ".PDF"
                         };

            var result = query1.ToList();

            dTResult.recordsTotal = query1.Count();
            dTResult.data = result;
            dTResult.recordsFiltered = query1.Count();
            return dTResult;
        }

        public string update(CMIEWiseCancellationAcceptance_FormModel model, string Uname)
        {
            string msg = "something went wrong";

            var query = context.T19CallCancels
           .Where(c => c.CaseNo == model.CASE_NO &&
                       c.CallRecvDt == Convert.ToDateTime(model.CALL_RECV_DATE) &&
                       c.CallSno == model.CALL_SNO)
           .FirstOrDefault();

            if (query != null)
            {
                query.DocsSubmitted = "Y";
                query.UserId = Uname;
                query.Datetime = DateTime.Now;

                context.SaveChanges();
                 msg = "UPDATED";
                return msg;
            }

            return msg;
        }


    }
}
