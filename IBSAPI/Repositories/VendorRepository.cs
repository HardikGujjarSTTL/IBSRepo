using IBSAPI.DataAccess;
using IBSAPI.Interfaces;
using IBSAPI.Models;

namespace IBSAPI.Repositories
{
    public class VendorRepository : IVendorRepository
    {

        private readonly ModelContext context;
        public VendorRepository(ModelContext context)
        {
            this.context = context;
        }

        public List<CallRegiModel> GetCaseDetailsforvendor(int UserID)
        {
            List<CallRegiModel> lst = new();

            lst = (from x in context.T17CallRegisters
                  join y in context.T13PoMasters on x.CaseNo equals y.CaseNo
                  join p in context.V06Consignees on y.PurchaserCd equals p.ConsigneeCd
                   join v in context.T05Vendors on y.VendCd equals v.VendCd
                   join cs in context.T21CallStatusCodes on x.CallStatus equals cs.CallStatusCd
                   join ie in context.T09Ies on x.IeCd equals ie.IeCd
                   where x.MfgCd == UserID
                   select new CallRegiModel
                   {
                       CaseNo = x.CaseNo,
                       CallDate = x.Datetime,
                       CallSNo = x.CallSno,
                       Purchaser = p.Consignee,
                       Vendor = v.VendName,
                       PurchaseOrderDate=y.PoDt,
                       PurchaseOrderNo = y.PoNo,
                       CallStatus = cs.CallStatusDesc,
                       Region = x.RegionCode,
                       PlaceofInspection = x.MfgPlace,
                       ContactPersonName = ie.IeName,
                       ManufacturerEmail = ie.IeEmail,
                       PhoneNumber = ie.IePhoneNo,
                   }).ToList();
            return lst;
        }
    }
}
