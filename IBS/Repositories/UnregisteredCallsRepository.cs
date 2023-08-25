using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using System.Data;

namespace IBS.Repositories
{
    public class UnregisteredCallsRepository : IUnregisteredCallsRepository
    {
        private readonly ModelContext context;

        public UnregisteredCallsRepository(ModelContext context)
        {
            this.context = context;
        }

        public DTResult<UnregisteredCallsModel> GetUnregisteredCallsList(DTParameters dtParameters)
        {
            DTResult<UnregisteredCallsModel> dTResult = new() { draw = 0 };
            IQueryable<UnregisteredCallsModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null && dtParameters.Order.Length > 0)
            {
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                if (string.IsNullOrEmpty(orderCriteria)) orderCriteria = "IeName";
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                orderCriteria = "IeName";
                orderAscendingDirection = true;
            }

            string Region = !string.IsNullOrEmpty(dtParameters.AdditionalValues["Region"]) ? Convert.ToString(dtParameters.AdditionalValues["Region"]) : "";

            query = from u in context.T70UnregisteredCalls
                    join i in context.T09Ies on u.IeCd equals i.IeCd
                    where u.Region == Region
                    select new UnregisteredCallsModel
                    {
                        IeCd = u.IeCd,
                        IeName = i.IeName,
                        YrMth = u.YrMth,
                        UnregCalls = u.UnregCalls,
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => w.IeName.ToLower().Contains(searchBy.ToLower())
                                        || w.YrMth.ToLower().Contains(searchBy.ToLower())
                                    );

            dTResult.recordsFiltered = query.Count();

            if (dtParameters.Length == -1) dtParameters.Length = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;

        }

        public bool IsExists(int IeCd)
        {
            return context.T70UnregisteredCalls.Any(x => x.IeCd == IeCd);
        }

        public UnregisteredCallsModel FindByID(int id)
        {
            UnregisteredCallsModel model = new();
            T70UnregisteredCall unregisteredCall = context.T70UnregisteredCalls.Find(id);

            if (unregisteredCall == null)
                throw new Exception("Unregistered Calls Record Not found");
            else
            {
                model.IeCd = unregisteredCall.IeCd;
                model.Year = Convert.ToInt32(unregisteredCall.YrMth.ToString().Substring(0, 4));
                model.Month = Convert.ToInt32(unregisteredCall.YrMth.ToString().Substring(4, 2));
                model.UnregCalls = unregisteredCall.UnregCalls;
                model.Region = unregisteredCall.Region;
                model.IsNew = false;

                return model;
            }
        }

        public int SaveDetails(UnregisteredCallsModel model)
        {
            T70UnregisteredCall unregisteredCall = context.T70UnregisteredCalls.Find(model.IeCd);

            if (unregisteredCall != null)
            {
                unregisteredCall.YrMth = string.Format("{0}{1}", model.Year, model.Month.ToString("00"));
                unregisteredCall.UnregCalls = model.UnregCalls;

                context.SaveChanges();
            }
            else
            {
                unregisteredCall = new()
                {
                    IeCd = model.IeCd,
                    YrMth = string.Format("{0}{1}", model.Year, model.Month.ToString("00")),
                    UnregCalls = model.UnregCalls,
                    Region = model.Region,
                };

                context.T70UnregisteredCalls.Add(unregisteredCall);
                context.SaveChanges();

                model.IeCd = unregisteredCall.IeCd;
            }

            return model.IeCd;
        }

        public bool Remove(int id)
        {
            if (context.T70UnregisteredCalls.Any(x => x.IeCd == id))
            {
                context.T70UnregisteredCalls.RemoveRange(context.T70UnregisteredCalls.Where(x => x.IeCd == id).ToList());
                context.SaveChanges();
            }
            return true;
        }
    }
}
