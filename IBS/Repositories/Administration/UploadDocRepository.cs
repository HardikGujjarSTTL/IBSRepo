using IBS.DataAccess;
using IBS.Interfaces.Administration;
using IBS.Models;
using static IBS.Helper.Enums;

namespace IBS.Repositories.Administration
{
    public class UploadDocRepository : IUploadDocRepository
    {
        private readonly ModelContext context;

        public UploadDocRepository(ModelContext context)
        {
            this.context = context;
        }

        public string DetailsUpdate(UploadDocModel model)
        {
            var GetValuePO = context.T76DocumentCatalogs.Find(model.FileId);

            decimal? MAXid = (from x in context.DocumentCatalogViews
                              where x.DocType == model.DocType && (x.DocSubType == model.DocSubType || null == model.DocSubType)
                              select x.FId).Max();

            string FildID = model.DocType + model.DocSubType + (Convert.ToInt32(MAXid ?? 0) + 1).ToString("D4");
            if(string.IsNullOrEmpty(model.DocSubType))
            {
                FildID = model.DocType + (Convert.ToInt32(MAXid ?? 0) + 1).ToString("D5");
            }

            #region save
            if (GetValuePO == null)
            {
                T76DocumentCatalog obj = new T76DocumentCatalog();
                obj.DocType = model.DocType;
                obj.DocSubType = model.DocSubType;
                obj.FileId = FildID;
                obj.FileExt = model.FileExt;
                obj.DocumentName = model.DocumentName;
                obj.DocumentNo = model.DocumentNo;
                obj.Datetime = model.Datetime;
                context.T76DocumentCatalogs.Add(obj);
                context.SaveChanges();
                return obj.FileId;
            }
            //else
            //{
            //    //GetValuePO.MaStatus = model.MaStatus;
            //    //GetValuePO.ApprovedBy = model.ApprovedBy;
            //    //GetValuePO.ApprovedDatetime = DateTime.Now;
            //    context.SaveChanges();
            //    return GetValuePO.FileId;
            //}
            #endregion
            return "";
        }
    }
}
