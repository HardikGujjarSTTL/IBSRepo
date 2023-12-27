using IBS.DataAccess;
using IBS.Interfaces.IE;
using IBS.Models;

namespace IBS.Repositories.IE
{
    public class IEJIRemarksPendingRepository : IIEJIRemarksPendingRepository
    {
        private readonly ModelContext context;

        public IEJIRemarksPendingRepository(ModelContext context)
        {
            this.context = context;
        }

        public DTResult<IEJIRemarksPendingModel> GetDataList(DTParameters dtParameters, string GetRegionCode, string UserId, int GetIeCd)
        {

            DTResult<IEJIRemarksPendingModel> dTResult = new() { draw = 0 };
            IQueryable<IEJIRemarksPendingModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "" || orderCriteria == null)
                {
                    orderCriteria = "ComplaintId";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
            }
            else
            {
                orderCriteria = "ComplaintId";
                orderAscendingDirection = true;
            }


            query = from l in context.ViewGetPendingJiComplaints
                    where l.IeCd == GetIeCd
                    select new IEJIRemarksPendingModel
                    {
                        ComplaintId = l.ComplaintId,
                        ComplaintDt = l.ComplaintDt,
                        CaseNo = l.CaseNo,
                        BkNo = l.BkNo,
                        SetNo = l.SetNo,
                        IeName = l.IeName,
                        Consignee = l.Consignee,
                        Vendor = l.Vendor,
                        ItemDesc = l.ItemDesc,
                        QtyOff = l.QtyOff,
                        QtyRejected = l.QtyRejected,
                        RejectionValue = l.RejectionValue,
                        RejectionReason = l.RejectionReason,
                        Status = l.Status,
                        JiDt = l.JiDt,
                        PoNo = l.PoNo,
                        PoDt = l.PoDt,
                        IcDt = l.IcDt,

                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.CaseNo).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }
    }
}
