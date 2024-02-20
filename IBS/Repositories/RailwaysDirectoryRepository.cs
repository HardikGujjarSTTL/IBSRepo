using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;

namespace IBS.Repositories
{
    public class RailwaysDirectoryRepository : IRailwaysDirectoryRepository
    {
        private readonly ModelContext context;

        public RailwaysDirectoryRepository(ModelContext context)
        {
            this.context = context;
        }

        public RailwaysDirectoryModel FindByID(int Id)
        {
            RailwaysDirectoryModel model = new();
            T91Railway railway = context.T91Railways.Find(Id);

            if (railway == null)
                return model;
            else
            {
                model.Id = railway.Id;
                model.RlyCd = railway.RlyCd;
                model.Railway = railway.Railway;
                model.HeadQuarter = railway.HeadQuarter;
                return model;
            }
        }

        public DTResult<RailwaysDirectoryModel> GetRMList(DTParameters dtParameters)
        {

            DTResult<RailwaysDirectoryModel> dTResult = new() { draw = 0 };
            IQueryable<RailwaysDirectoryModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null && dtParameters.Order.Length > 0)
            {
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                if (string.IsNullOrEmpty(orderCriteria)) orderCriteria = "RlyCd";
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                orderCriteria = "RlyCd";
                orderAscendingDirection = true;
            }

            string RailwayCode = !string.IsNullOrEmpty(dtParameters.AdditionalValues["RailwayCode"]) ? Convert.ToString(dtParameters.AdditionalValues["RailwayCode"]) : "";
            string RailwaysProductionUnit = !string.IsNullOrEmpty(dtParameters.AdditionalValues["RailwaysProductionUnit"]) ? Convert.ToString(dtParameters.AdditionalValues["RailwaysProductionUnit"]) : "";
            string HeadQuarter = !string.IsNullOrEmpty(dtParameters.AdditionalValues["HeadQuarter"]) ? Convert.ToString(dtParameters.AdditionalValues["HeadQuarter"]) : "";

            query = from t91 in context.T91Railways
                    where (!string.IsNullOrEmpty(RailwayCode) ? t91.RlyCd.ToLower().Contains(RailwayCode.ToLower()) : true)
                    && (!string.IsNullOrEmpty(RailwaysProductionUnit) ? t91.Railway.ToLower().Contains(RailwaysProductionUnit.ToLower()) : true)
                    && (!string.IsNullOrEmpty(HeadQuarter) ? t91.HeadQuarter.ToLower().Contains(HeadQuarter.ToLower()) : true)
                    select new RailwaysDirectoryModel
                    {
                        Id = t91.Id,
                        RlyCd = t91.RlyCd,
                        Railway = t91.Railway,
                        HeadQuarter = t91.HeadQuarter,
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.RlyCd).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.Railway).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.HeadQuarter).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            if (dtParameters.Length == -1) dtParameters.Length = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public bool IsDuplicate(RailwaysDirectoryModel model)
        {
            return context.T91Railways.Any(x => x.Id != model.Id && x.RlyCd == model.RlyCd);
        }

        public int SaveDetails(RailwaysDirectoryModel model)
        {
            if (model.Id == 0)
            {
                var Cnt = context.T91Railways.Where(x => x.RlyCd.ToLower().Contains(model.RlyCd)).Count();
                if(Cnt > 0)
                {
                    return -1;
                }

                T91Railway railway = new()
                {
                    RlyCd = model.RlyCd,
                    Railway = model.Railway,
                    HeadQuarter = model.HeadQuarter,
                    UserId = model.UserId,
                    Datetime = DateTime.Now.Date,
                    Createdby = model.Createdby,
                    Createddate = DateTime.Now,
                };

                context.T91Railways.Add(railway);
                context.SaveChanges();
            }
            else
            {
                T91Railway railway = context.T91Railways.Find(model.Id);

                if (railway != null)
                {
                    railway.RlyCd = model.RlyCd;
                    railway.Railway = model.Railway;
                    railway.HeadQuarter = model.HeadQuarter;
                    railway.UserId = model.UserId;
                    railway.Datetime = DateTime.Now.Date;
                    railway.Updatedby = model.Updatedby;
                    railway.Updateddate = DateTime.Now;

                    context.SaveChanges();
                }
            }

            return model.Id;
        }

        public bool Remove(int Id)
        {
            if (context.T91Railways.Any(x => x.Id == Id))
            {
                context.T91Railways.RemoveRange(context.T91Railways.Where(x => x.Id == Id).ToList());
                context.SaveChanges();
            }
            return true;
        }

        public string IsExistsRailwayCode(int Id)
        {
            string RlyCd = context.T91Railways.Where(x => x.Id == Id).Select(x => x.RlyCd).FirstOrDefault();

            if (context.T06Consignees.Any(x => x.ConsigneeFirm == RlyCd && x.ConsigneeType == "R"))
            {
                return "Consignee Master";
            }
            else if (context.T12BillPayingOfficers.Any(x => x.BpoRly == RlyCd && x.BpoType == "R"))
            {
                return "BPO Master";
            }
            else
            {
                return "";
            }
        }
    }

}