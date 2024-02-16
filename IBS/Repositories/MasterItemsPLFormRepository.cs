using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;

namespace IBS.Repositories
{
    public class MasterItemsPLFormRepository : IMasterItemsPLFormRepository
    {
        private readonly ModelContext context;

        public MasterItemsPLFormRepository(ModelContext context)
        {
            this.context = context;
        }

        public MasterItemsPLFormModel FindByID(string PlNo)
        {
            MasterItemsPLFormModel model = new();
            T62MasterItemPlno item = context.T62MasterItemPlnos.Find(PlNo);

            if (item == null)
                return model;
            else
            {
                model.ItemCd = item.ItemCd;
                model.PlNo = item.PlNo;
                model.DrawingNo = item.DrawingNo;
                model.SpecificationNo = item.SpecificationNo;
                model.IsNew = false;
                return model;
            }
        }

        public DTResult<MasterItemsPLFormModel> GetMasterItemsPLFormList(DTParameters dtParameters)
        {
            DTResult<MasterItemsPLFormModel> dTResult = new() { draw = 0 };
            IQueryable<MasterItemsPLFormModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null && dtParameters.Order.Length > 0)
            {
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                if (string.IsNullOrEmpty(orderCriteria)) orderCriteria = "ItemCd";
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                orderCriteria = "ItemCd";
                orderAscendingDirection = true;
            }

            string ItemDescription = !string.IsNullOrEmpty(dtParameters.AdditionalValues["ItemDescription"]) ? Convert.ToString(dtParameters.AdditionalValues["ItemDescription"]) : "";
            string PLNO = !string.IsNullOrEmpty(dtParameters.AdditionalValues["PLNO"]) ? Convert.ToString(dtParameters.AdditionalValues["PLNO"]) : "";

            query = from t62 in context.T62MasterItemPlnos
                    join t61 in context.T61ItemMasters on t62.ItemCd equals t61.ItemCd
                    where t62.Isdeleted != 1
                    && (!string.IsNullOrEmpty(ItemDescription) ? t61.ItemDesc.ToLower().Contains(ItemDescription.ToLower()) : true)
                    && (!string.IsNullOrEmpty(PLNO) ? t62.PlNo.ToLower().Contains(PLNO.ToLower()) : true)
                    select new MasterItemsPLFormModel
                    {
                        ItemCd = t62.ItemCd,
                        ItemDesc = t61.ItemDesc,
                        PlNo = t62.PlNo,
                        DrawingNo = t62.DrawingNo,
                        SpecificationNo = t62.SpecificationNo,
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => (w.ItemDesc != null && w.ItemDesc.ToLower().Contains(searchBy.ToLower()))
                                                || (w.PlNo != null && w.PlNo.ToLower().Contains(searchBy.ToLower()))
                                                || (w.DrawingNo != null && w.DrawingNo.ToLower().Contains(searchBy.ToLower()))
                                                || (w.SpecificationNo != null && w.SpecificationNo.ToLower().Contains(searchBy.ToLower()))
                                           );

            dTResult.recordsFiltered = query.Count();

            if (dtParameters.Length == -1) dtParameters.Length = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public void SaveDetails(MasterItemsPLFormModel model)
        {
            if (model.IsNew)
            {
                T62MasterItemPlno items = new()
                {
                    ItemCd = model.ItemCd,
                    PlNo = model.PlNo,
                    DrawingNo = model.DrawingNo,
                    SpecificationNo = model.SpecificationNo,
                    UserId = model.UserId,
                    Datetime = DateTime.Now.Date,
                    Createdby = model.Createdby,
                    Createddate = DateTime.Now,
                };

                context.T62MasterItemPlnos.Add(items);
                context.SaveChanges();
            }
            else
            {
                T62MasterItemPlno items = context.T62MasterItemPlnos.Find(model.PlNo);

                if (items != null)
                {
                    items.PlNo = model.PlNo;
                    items.DrawingNo = model.DrawingNo;
                    items.SpecificationNo = model.SpecificationNo;
                    items.UserId = model.UserId;
                    items.Datetime = DateTime.Now.Date;
                    items.Updatedby = model.Updatedby;
                    items.Updateddate = DateTime.Now;

                    context.SaveChanges();
                }
            }
        }

        public bool IsDuplicate(MasterItemsPLFormModel model)
        {
            if (model.IsNew)
            {
                return context.T62MasterItemPlnos.Any(x => x.PlNo.ToLower() == model.PlNo.ToLower());
            }
            else
            {
                return false;
            }
        }

        public bool Remove(string PlNo)
        {
            if (context.T62MasterItemPlnos.Any(x => x.PlNo == PlNo))
            {
                context.T62MasterItemPlnos.RemoveRange(context.T62MasterItemPlnos.Where(x => x.PlNo == PlNo).ToList());
                context.SaveChanges();
            }
            return true;
        }
    }

}