using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using System.Drawing;

namespace IBS.Repositories
{
    public class DownloadBillingDocumentsforDigitalInvoiceRepository : IDownload_billing
    {
        private readonly ModelContext context;

        public DownloadBillingDocumentsforDigitalInvoiceRepository(ModelContext _context)
        {
            this.context = _context;
        }

        public DownloadBillingDocumentsForDigitalInvoiceModel FindByID(int Id)
        {
            DownloadBillingDocumentsForDigitalInvoiceModel model = new();


            if (model == null)
                throw new Exception("DownloadBillingDocumentsForDigitalInvoiceModel Not found");
            else
            {



                return model;
            }
        }

        public DTResult<DownloadBillingDocumentsForDigitalInvoiceModel> GetDownloadBillingList(DTParameters dtParameters)
        {
            DTResult<DownloadBillingDocumentsForDigitalInvoiceModel> dTResult = new() { draw = 0 };
            IQueryable<DownloadBillingDocumentsForDigitalInvoiceModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "Region";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "Region";
                orderAscendingDirection = true;
            }
            query = from l in context.T97ControlFiles
                    select new DownloadBillingDocumentsForDigitalInvoiceModel
                    {

                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.Month).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public int DownloadBillingInsertUpdate(DownloadBillingDocumentsForDigitalInvoiceModel model)
        {
            int ID = 0;

            var region = context.T97ControlFiles.Find(model.Month);
            #region User save

            return ID;
            #endregion
        }

        public bool Remove(int Id)
        {
            throw new NotImplementedException();
        }
    }
}
