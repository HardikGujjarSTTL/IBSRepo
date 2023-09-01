using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Interfaces.WebsitePages;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Data.SqlClient;

namespace IBS.Repositories.WebsitePages
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly ModelContext context;

        public FeedbackRepository(ModelContext context)
        {
            this.context = context;
        }

        public bool CheckAlreadyExist(VendorFeedbackModel model)
        {
            var AlreadyExist = (from vf in context.VendorFeedbacks where vf.VendCd == model.VendCd && vf.RegionCode == model.RegionCode select vf).ToList();
            if (AlreadyExist.Count > 0 )
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
    }

}
