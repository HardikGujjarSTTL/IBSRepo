using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;

namespace IBS.Repositories
{
    public class Allow_Old_Bill_DateRepository : IAllow_Old_Bill_DateRepository
    {
        private readonly ModelContext context;

        public Allow_Old_Bill_DateRepository(ModelContext context)
        {
            this.context = context;
        }

        public Allow_Old_Bill_DateModel FindByID(string region)
        {
            Allow_Old_Bill_DateModel model = new();
            T97ControlFile message = context.T97ControlFiles.Find(region);

            if (message == null)
                throw new Exception("Allow Old Bill Date Not found");
            else
            {
                model.Region = message.Region;
                model.AllowOldBillDt = message.AllowOldBillDt;
                model.GraceDays = message.GraceDays;
                model.Isdeleted = message.Isdeleted;

                return model;
            }
        }

        public DTResult<Allow_Old_Bill_DateModel> GetMessageList(DTParameters dtParameters, string GetRegionCode)
        {

            DTResult<Allow_Old_Bill_DateModel> dTResult = new() { draw = 0 };
            IQueryable<Allow_Old_Bill_DateModel>? query = null;

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
                    where l.Region == GetRegionCode
                    select new Allow_Old_Bill_DateModel
                    {
                        Region = l.Region,
                        AllowOldBillDt = l.AllowOldBillDt,
                        GraceDays = l.GraceDays,
                        Isdeleted = Convert.ToByte(false),
                        Createddate = l.Createddate,
                        Createdby = l.Createdby,
                        Updateddate = l.Updateddate,
                        Updatedby = l.Updatedby
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.Region).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public int DetailsInsertUpdate(Allow_Old_Bill_DateModel model)
        {
            int ID = 0;
            var region = context.T97ControlFiles.Find(model.Region);
            #region User save
            if (region == null)
            {
                //T97ControlFile obj = new T97ControlFile();
                //obj.Message = model.MESSAGE;
                //obj.UserId = model.Createdby;
                //obj.Datetime = DateTime.Now;
                //obj.Isdeleted = Convert.ToByte(false);
                //obj.Createdby = model.Createdby;
                //obj.Createddate = DateTime.Now;
                //context.T96Messages.Add(obj);
                //context.SaveChanges();
                //MESSAGE_ID = Convert.ToInt32(obj.MessageId);
            }
            else
            {
                region.AllowOldBillDt = model.AllowOldBillDt;
                region.GraceDays = model.GraceDays;
                region.Isdeleted = Convert.ToByte(false);
                region.Updatedby = model.Updatedby;
                region.Updateddate = DateTime.Now;
                context.SaveChanges();
                ID = Convert.ToInt32(1);
            }
            #endregion
            return ID;
        }
    }
}
