using IBS.DataAccess;
using IBS.Interfaces.Vendor;
using IBS.Models;

namespace IBS.Repositories.Vendor
{
    public class VendorCallsMarkedForSpecificPORepository : IVendorCallsMarkedForSpecificPORepository
    {
        private readonly ModelContext context;

        public VendorCallsMarkedForSpecificPORepository(ModelContext context)
        {
            this.context = context;
        }

        public DTResult<VendorCallsMarkedForSpecificPOModel> GetDataList(DTParameters dtParameters, string UserName)
        {

            DTResult<VendorCallsMarkedForSpecificPOModel> dTResult = new() { draw = 0 };
            IQueryable<VendorCallsMarkedForSpecificPOModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "PoNo";
                }
                //orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
                orderAscendingDirection = true;
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "PoNo";
                orderAscendingDirection = true;
            }
            string RlyNorly = "", RlyCd = "", PoDt = "";
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["RlyNorly"]))
            {
                RlyNorly = Convert.ToString(dtParameters.AdditionalValues["RlyNorly"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["RlyCd"]))
            {
                RlyCd = Convert.ToString(dtParameters.AdditionalValues["RlyCd"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["PoDt"]))
            {
                PoDt = Convert.ToString(dtParameters.AdditionalValues["PoDt"]);
            }

            RlyNorly = RlyNorly.ToString() == "" ? string.Empty : RlyNorly.ToString();
            RlyCd = RlyCd.ToString() == "" ? string.Empty : RlyCd.ToString();
            DateTime? _PoDt = PoDt == "" ? null : DateTime.ParseExact(PoDt, "dd-MM-yyyy", null);

            query = from l in context.ViewVendorCallsMarkedForSpecificPos
                    where l.RlyNonrly.Equals(RlyNorly) && l.RlyCd.Equals(RlyCd) && l.PoDt.Equals(_PoDt) && l.VendCd.Equals(UserName)
                    select new VendorCallsMarkedForSpecificPOModel
                    {
                        PoNo = l.PoNo
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.PoNo).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }
    }
}
