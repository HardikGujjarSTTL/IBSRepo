using IBS.DataAccess;
using IBS.Interfaces.InspectionBilling;
using IBS.Models;

namespace IBS.Repositories.InspectionBilling
{
    public class InspectionCertRepository : IInspectionCertRepository
    {
        private readonly ModelContext context;

        public InspectionCertRepository(ModelContext context)
        {
            this.context = context;
        }

        public DTResult<InspectionCertModel> GetDataList(DTParameters dtParameters, string GetRegionCode)
        {
            DTResult<InspectionCertModel> dTResult = new() { draw = 0 };
            IQueryable<InspectionCertModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "" || orderCriteria == null)
                {
                    orderCriteria = "Caseno";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
            }
            else
            {
                orderCriteria = "Caseno";
                orderAscendingDirection = true;
            }

            string Caseno = "", Callrecvdt = "", Callsno = "";

            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["Caseno"]))
            {
                Caseno = Convert.ToString(dtParameters.AdditionalValues["Caseno"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["Callrecvdt"]))
            {
                Callrecvdt = Convert.ToString(dtParameters.AdditionalValues["Callrecvdt"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["Callsno"]))
            {
                Callsno = Convert.ToString(dtParameters.AdditionalValues["Callsno"]);
            }

            Caseno = Caseno.ToString() == "" ? string.Empty : Caseno.ToString();
            DateTime? _CallRecvDt = Callrecvdt == "" ? null : DateTime.ParseExact(Callrecvdt, "dd/MM/yyyy", null);
            Callsno = Callsno.ToString() == "" ? string.Empty : Callsno.ToString();

            query = from l in context.ViewGetInspectionCertDetails
                    where l.Regioncode == GetRegionCode
                          && (Caseno == null || Caseno == "" || l.Caseno == Caseno)
                          && (Callrecvdt == null || Callrecvdt == "" || l.Callrecvdt == _CallRecvDt)
                          && (Callsno == null || Callsno == "" || l.Callsno == Convert.ToInt32(Callsno))
                    select new InspectionCertModel
                    {
                        Caseno = l.Caseno,
                        Callrecvdt = l.Callrecvdt,
                        Callsno = l.Callsno,
                        Icno = l.Icno,
                        Bkno = l.Bkno,
                        Setno = l.Setno,
                        Status = l.Status,
                        Iesname = l.Iesname,
                        Consignee = l.Consignee,
                        Callstatusdesc = l.Callstatusdesc,
                        Regioncode = l.Regioncode,
                        Callstatus = l.Callstatus,
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.Caseno).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }
    }
}
