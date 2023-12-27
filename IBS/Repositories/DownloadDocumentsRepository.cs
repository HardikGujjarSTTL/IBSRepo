using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;

namespace IBS.Repositories
{
    public class DownloadDocumentsRepository : IDownloadDocumentsRepository
    {
        private readonly ModelContext context;

        public DownloadDocumentsRepository(ModelContext context)
        {
            this.context = context;
        }

        public DTResult<DownloadDocumentsModel> GetMessageList(DTParameters dtParameters)
        {

            DTResult<DownloadDocumentsModel> dTResult = new() { draw = 0 };
            IQueryable<DownloadDocumentsModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "IssueDt";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "IssueDt";
                orderAscendingDirection = true;
            }
            string DocType = "";
            string DocSubType = "";
            string DocSearch = "";
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["DocType"]))
            {
                DocType = Convert.ToString(dtParameters.AdditionalValues["DocType"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["DocSubType"]))
            {
                DocSubType = Convert.ToString(dtParameters.AdditionalValues["DocSubType"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["DocSearch"]))
            {
                DocSearch = Convert.ToString(dtParameters.AdditionalValues["DocSearch"]);
            }
            DocType = DocType.ToString() == null ? string.Empty : DocType.ToString();
            DocSubType = DocSubType.ToString() == null ? string.Empty : DocSubType.ToString();
            DocSearch = DocSearch.ToString() == null ? string.Empty : DocSearch;

            query = (from l in context.T76DocumentCatalogs
                    where l.DocType.Contains(DocType) && l.DocSubType.Contains(DocSubType) && l.DocumentName.Contains(DocSearch)
                    select new DownloadDocumentsModel
                    {
                        DocType = l.DocType,
                        DocSubType = l.DocSubType,
                        FileId = l.FileId,
                        FileExt = l.FileExt,
                        FILE_NAME = l.FileId + "." + l.FileExt,
                        FILE_LOCATION = "Documents/" + l.FileId + "." + l.FileExt,
                        DocumentName = l.DocumentName,
                        DocumentNo = l.DocumentNo,
                        IssueDt = l.IssueDt,
                        UserId = l.UserId,
                        Datetime = l.Datetime,
                        Region = l.Region
                    }).OrderByDescending(l => l.IssueDt);

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.DocumentName).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

    }
}
