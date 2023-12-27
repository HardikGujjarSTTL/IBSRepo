using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces.WebsitePages;
using IBS.Models;
using System.Data;

namespace IBS.Repositories.WebsitePages
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly ModelContext context;
        #region Vendor feedback
        public FeedbackRepository(ModelContext context)
        {
            this.context = context;
        }

        public bool CheckAlreadyExist(VendorFeedbackModel model)
        {
            var AlreadyExist = (from vf in context.VendorFeedbacks where vf.VendCd == model.VendCd && vf.RegionCode == model.RegionCode select vf).ToList();
            if (AlreadyExist.Count > 0)
            {
                return true;
            }
            return false;
        }
        public int VendorFeedbackSave(VendorFeedbackModel model)
        {
            VendorFeedback obj = new VendorFeedback();
            obj.VendCd = model.VendCd;
            obj.RegionCode = model.RegionCode;
            obj.Field1 = model.Field1;
            obj.Field2 = model.Field2;
            obj.Field3 = model.Field3;
            obj.Field4 = model.Field4;
            obj.Field5 = model.Field5;
            obj.Field6 = model.Field6;
            obj.Field7 = model.Field7;
            obj.Field8 = model.Field8;
            obj.Field9 = model.Field9;
            obj.Field10 = model.Field10;
            obj.MthyrPeriod = DateTime.Now.ToString("yyyyMM");
            context.VendorFeedbacks.Add(obj);
            context.SaveChanges();
            return Convert.ToInt32(obj.VendCd);
        }
        #endregion

        #region Client feedback

        public bool CheckClientAlreadyExist(ClientFeedbackModel model)
        {
            var AlreadyExist = (from vf in context.ClientFeedbacks where vf.Mobile == model.Mobile && vf.RegionCode == model.RegionCode select vf).ToList();
            if (AlreadyExist.Count > 0)
            {
                return true;
            }
            return false;
        }
        public int ClientFeedbackSave(ClientFeedbackModel model)
        {
            ClientFeedback obj = new ClientFeedback();
            obj.RegionCode = model.RegionCode;
            obj.Field1 = model.Field1;
            obj.Field2 = model.Field2;
            obj.Field3 = model.Field3;
            obj.Field4 = model.Field4;
            obj.Field5 = model.Field5;
            obj.Field6 = model.Field6;
            obj.Field7 = model.Field7;
            obj.Field8 = model.Field8;
            obj.Field9 = model.Field9;
            obj.Field10 = model.Field10;
            obj.Field11 = model.Field11;
            obj.Field12 = model.Field12;
            obj.Client = model.Client;
            obj.OffName = model.OffName;
            obj.Mobile = model.Mobile;
            obj.Email = model.Email;
            obj.RegionCode = model.RegionCode;
            context.ClientFeedbacks.Add(obj);
            context.SaveChanges();
            return Convert.ToInt32(obj.Id);
        }
        #endregion
    }
}
