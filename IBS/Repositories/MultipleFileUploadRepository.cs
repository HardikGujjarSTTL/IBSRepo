using IBS.DataAccess;
using IBS.Interfaces;

namespace IBS.Repositories
{
    public class MultipleFileUploadRepository : IMultipleFileUploadRepository
    {
        private readonly ModelContext context;

        public MultipleFileUploadRepository(ModelContext context)
        {
            this.context = context;
        }

        public int InsertPDFDetails(string FileName, string Bill_NO, int CreatedBy)
        {
            var res = 0;
            var data = (from item in context.T114MultipleBillFileUploads
                        where item.BillNo == Bill_NO
                        select item).FirstOrDefault();
            try
            {
                if (data == null)
                {
                    int maxID = context.T114MultipleBillFileUploads.Max(x => x.Id) + 1;
                    T114MultipleBillFileUpload obj = new T114MultipleBillFileUpload();
                    obj.Id = maxID;
                    obj.BillNo = Bill_NO;
                    obj.FileName = FileName;
                    obj.Createdby = CreatedBy.ToString();
                    obj.Createddate = DateTime.Now.Date;
                    context.T114MultipleBillFileUploads.Add(obj);
                    context.SaveChanges();
                    res = 1;
                }
                else
                {
                    data.BillNo = Bill_NO;
                    data.FileName = FileName;
                    data.Updatedby = CreatedBy.ToString();
                    data.Updateddate = DateTime.Now.Date;
                    context.SaveChanges();
                    res = 1;
                }
            }
            catch (Exception)
            {
                res = 0;
            }
            return res;
        }
    }
}
