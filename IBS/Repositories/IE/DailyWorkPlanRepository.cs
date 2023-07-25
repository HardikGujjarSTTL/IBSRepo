using IBS.DataAccess;
using IBS.Interfaces.IE;
using IBS.Models;

namespace IBS.Repositories.IE
{
    public class DailyWorkPlanRepository : IDailyWorkPlanRepository
    {
        private readonly ModelContext context;

        public DailyWorkPlanRepository(ModelContext context)
        {
            this.context = context;
        }

        public DTResult<DailyWorkPlanModel> GetMessageList(DTParameters dtParameters, int GetIeCd)
        {

            DTResult<DailyWorkPlanModel> dTResult = new() { draw = 0 };
            IQueryable<DailyWorkPlanModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "Reason";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "Reason";
                orderAscendingDirection = true;
            }
            query = from l in context.NoIeWorkPlans
                    where l.IeCd == GetIeCd
                    select new DailyWorkPlanModel
                    {
                        IeCd = l.IeCd,
                        CoCd = l.CoCd,
                        Reason = l.Reason,
                        NwpDt = l.NwpDt,
                        RegionCode = l.RegionCode,
                        UserId = l.UserId,
                        Datetime = l.Datetime
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.Reason).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public int DetailsInsertUpdate(DailyWorkPlanModel model)
        {
            int ID = 0;
            var G_Message = context.NoIeWorkPlans.Find(model.IeCd);
            #region User save
            if (G_Message == null)
            {
                NoIeWorkPlan obj = new NoIeWorkPlan();
                //obj.Message = model.MESSAGE;
                //obj.UserId = model.Createdby;
                //obj.Datetime = DateTime.Now;
                //obj.Isdeleted = Convert.ToByte(false);
                //obj.Createdby = model.Createdby;
                //obj.Createddate = DateTime.Now;
                context.NoIeWorkPlans.Add(obj);
                context.SaveChanges();
                ID = Convert.ToInt32(obj.IeCd);
            }
            else
            {
                //G_Message.Message = model.MESSAGE;
                //G_Message.Isdeleted = Convert.ToByte(false);
                //G_Message.Updatedby = model.Updatedby;
                //G_Message.Updateddate = DateTime.Now;
                context.SaveChanges();
                ID = Convert.ToInt32(G_Message.IeCd);
            }
            #endregion
            return ID;
        }
    }
}
