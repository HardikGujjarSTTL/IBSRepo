using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;

namespace IBS.Repositories
{
    public class VendorProfileRepository : IVendorProfileRepository
    {
        private readonly ModelContext context;

        public VendorProfileRepository(ModelContext context)
        {
            this.context = context;
        }


        public VendorModel FindByID(int VendCd)
        {
            VendorModel model = (from m in context.T05Vendors
                                 where m.VendCd == Convert.ToInt32(VendCd)
                                 select new VendorModel
                                 {
                                     VendCd = m.VendCd,
                                     VendName = m.VendName,
                                     VendAdd1 = m.VendAdd1,
                                     VendAdd2 = m.VendAdd2,
                                     VendCityCd = m.VendCityCd,
                                     VendApproval = m.VendApproval,
                                     VendApprovalFr = m.VendApprovalFr,
                                     VendApprovalTo = m.VendApprovalTo,
                                     VendContactPer1 = m.VendContactPer1,
                                     VendContactTel1 = m.VendContactTel1,
                                     VendContactPer2 = m.VendContactPer2,
                                     VendContactTel2 = m.VendContactTel2,
                                     VendEmail = m.VendEmail,
                                     VendRemarks = m.VendRemarks
                                 }).FirstOrDefault();

            return model;
        }
    }

}

