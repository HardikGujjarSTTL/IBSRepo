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
                                 where m.VendCd == VendCd
                                 select new VendorModel
                                 {
                                     VendCd= m.VendCd,
                                     VendName = m.VendName,
                                     VendAdd1 =  m.VendAdd1,
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

        public int VendorDetailsInsertUpdate(VendorModel model)
        {
            int VendCd = 0;
            var t05Vendors = context.T05Vendors.Find(model.VendCd);
            #region t05Vendors save
            if (t05Vendors == null)
            {
                T05Vendor obj = new T05Vendor();
                obj.VendCd = model.VendCd;
                obj.VendName = model.VendName;
                obj.VendAdd1 = model.VendAdd1;
                obj.VendAdd2 = model.VendAdd2;
                obj.VendCityCd = model.VendCityCd;
                obj.VendContactPer1 = model.VendContactPer1;
                obj.VendContactTel1 = model.VendContactTel1;
                obj.VendContactPer2 = model.VendContactPer2;
                obj.VendContactTel2 = model.VendContactTel2;
                obj.VendApproval = model.VendApproval;
                obj.VendApprovalFr = model.VendApprovalFr;
                obj.VendApprovalTo = model.VendApprovalTo;
                obj.VendRemarks = model.VendRemarks;
                obj.Datetime = DateTime.Now;
                obj.VendEmail = model.VendEmail;
                obj.VendPwd = model.VendPwd;

                context.T05Vendors.Add(obj);
                context.SaveChanges();
                VendCd = Convert.ToInt32(obj.VendCd);
            }
            else
            {
                //t05Vendors.VendName = model.VendName;
                t05Vendors.VendAdd1 = model.VendAdd1;
                t05Vendors.VendAdd2 = model.VendAdd2;
                t05Vendors.VendCityCd = model.VendCityCd;
                t05Vendors.VendContactPer1 = model.VendContactPer1;
                t05Vendors.VendContactTel1 = model.VendContactTel1;
                t05Vendors.VendContactPer2 = model.VendContactPer2;
                t05Vendors.VendContactTel2 = model.VendContactTel2;
                //t05Vendors.VendApproval = model.VendApproval;
                //t05Vendors.VendApprovalFr = model.VendApprovalFr;
                //t05Vendors.VendApprovalTo = model.VendApprovalTo;
                t05Vendors.VendRemarks = model.VendRemarks;
                t05Vendors.Datetime = DateTime.Now;
                t05Vendors.VendEmail = model.VendEmail;
                //t05Vendors.UserId = model.UserId;
                context.SaveChanges();
                VendCd = Convert.ToInt32(t05Vendors.VendCd);
            }
            #endregion
            return VendCd;
        }
    }

}

