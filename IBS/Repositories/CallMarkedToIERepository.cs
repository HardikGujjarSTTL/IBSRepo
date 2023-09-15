using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;

namespace IBS.Repositories
{
    public class CallMarkedToIERepository : ICallMarkedToIERepository
    {
        private readonly ModelContext context;

        public CallMarkedToIERepository(ModelContext context)
        {
            this.context = context;
        }

        public DTResult<CallsMarkedToIEModel> GetDataList(DTParameters dtParameters, string GetRegionCode, string UserId, int GetIeCd)
        {

            DTResult<CallsMarkedToIEModel> dTResult = new() { draw = 0 };
            IQueryable<CallsMarkedToIEModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;
            string PType = Convert.ToString(dtParameters.AdditionalValues.ToArray().Where(x => x.Key == "PType").FirstOrDefault().Value);
            if (dtParameters.Order != null)
            {
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    if(PType == "C")
                    {
                        orderCriteria = "Vendor";
                    }
                    else
                    {
                        orderCriteria = "Vendor";
                    }
                    
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "Vendor";
                orderAscendingDirection = true;
            }

            DateTime CallMarkDtMax = Convert.ToDateTime("21-06-2023");
            DateTime CallMarkDtMin = Convert.ToDateTime("21-07-2023");

            query = from l in context.CallsmarkedtoieViews
                    join r in context.T91Railways on l.RlyCd equals r.RlyCd
                    where l.RlyCd == r.RlyCd && l.RlyCd == "NR"
                    && l.CallMarkDt >= CallMarkDtMax && l.CallMarkDt <= CallMarkDtMin && l.IeCd == GetIeCd
                    //where (l.Isdeleted == 0 || l.Isdeleted == null)
                    orderby l.Vendor, l.CallMarkDt, l.DtInspDesire
                    select new CallsMarkedToIEModel
                    {
                        Vendor = l.Vendor,
                        NewVendor = l.NewVendor,
                        Consignee = l.Consignee,
                        ItemDescPo = l.ItemDescPo,
                        ExtDelvDt = l.ExtDelvDt,
                        DtInspDesire = l.DtInspDesire,
                        CallMarkDt = l.CallMarkDt,
                        CallSno = l.CallSno,
                        callDocAny = "/RBS/Vendor/CALLS_DOCUMENTS/" + l.CaseNo + "-" + l.CallRecvDt.ToString().Substring(6, 4) + l.CallRecvDt.ToString().Substring(3, 2) + l.CallRecvDt.ToString().Substring(0, 2) + l.CallSno + ".PDF",
                        PoSource = l.PoSource.Equals("C") ? "https://www.ireps.gov.in/ireps/etender/pdfdocs/MMIS/PO/" + l.PoYr + "/" + r.ImmsRlyCd + "/" + l.PoNo + ".pdf" : "/RBS/CASE_NO/" + l.CaseNo + ".PDF",
                        CallStatus = l.CallStatus,
                        Remarks = l.Remarks,
                        PoNo = l.PoNo,
                        PoDt = l.PoDt,
                        MfgPers = l.MfgPers,
                        MfgPhone = l.MfgPhone,
                        UserId = l.UserId == UserId ? "" : l.UserId,
                        Datetime = l.Datetime,
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.Vendor).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }
    }
}
