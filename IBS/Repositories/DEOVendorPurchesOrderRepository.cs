using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;

namespace IBS.Repositories
{
    public class DEOVendorPurchesOrderRepository : IDEOVendorPurchesOrderRepository
    {
        private readonly ModelContext context;

        public DEOVendorPurchesOrderRepository(ModelContext context)
        {
            this.context = context;
        }

        public DTResult<DEOVendorPurchesOrderModel> GetDataList(DTParameters dtParameters, string GetRegionCode)
        {

            DTResult<DEOVendorPurchesOrderModel> dTResult = new() { draw = 0 };
            IQueryable<DEOVendorPurchesOrderModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "CaseNo";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "CaseNo";
                orderAscendingDirection = true;
            }
            query = from p in context.T80PoMasters
                    join v in context.T05Vendors on p.VendCd equals v.VendCd
                    join c in context.T03Cities on v.VendCityCd equals c.CityCd
                    where (p.Isdeleted == 0 || p.Isdeleted == null) && p.RealCaseNo == null && p.RegionCode == GetRegionCode
                    //&& Convert.ToDateTime(p.RecvDt) > "01-12-2016"

                    select new DEOVendorPurchesOrderModel
                    {
                        CaseNo = p.CaseNo,
                        PurchaserCd = p.PurchaserCd,
                        StockNonstock = p.StockNonstock,
                        RlyNonrly = p.RlyNonrly.Equals("R") ? "Railway" : p.RlyNonrly.Equals("P") ? "Private" : p.RlyNonrly.Equals("S") ? "State Government" : p.RlyNonrly.Equals("F") ? "Foreign Railways" : p.RlyNonrly.Equals("U") ? "PSU" : p.RlyNonrly,
                        PoOrLetter = p.PoOrLetter,
                        PoNo = p.PoNo,
                        PoDt = p.PoDt,
                        RecvDt = p.RecvDt,
                        VendCd = p.VendCd,
                        RlyCd = p.RlyCd,
                        RegionCode = p.RegionCode,
                        UserId = p.UserId,
                        Datetime = p.Datetime,
                        Remarks = p.Remarks,
                        PoiCd = p.PoiCd,
                        Isdeleted = p.Isdeleted,
                        Createddate = p.Createddate,
                        //Createdby = p.Createdby,
                        Updateddate = p.Updateddate,
                        //Updatedby = p.Updatedby,
                        //VendCdNavigation = v,
                        VendName = v.VendName,
                        RealCaseNo=p.RealCaseNo

                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.CaseNo).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.Remarks).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }
    }
}
