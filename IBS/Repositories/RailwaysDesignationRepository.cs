using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;

namespace IBS.Repositories
{
    public class RailwaysDesignationRepository : IRailwaysDesignationRepository
    {
        private readonly ModelContext context;

        public RailwaysDesignationRepository(ModelContext context)
        {
            this.context = context;
        }

        public Rly_Designation_FormModel FindByID(int id)
        {
            Rly_Designation_FormModel model = new();
            T90RlyDesignation railwayDesignation = context.T90RlyDesignations.Where(x => x.Id == id).FirstOrDefault();

            if (railwayDesignation == null)
                return model;
            else
            {
                model.ID = railwayDesignation.Id;
                model.RlyDesigCd = railwayDesignation.RlyDesigCd;
                model.RlyDesigDesc = railwayDesignation.RlyDesigDesc;
                return model;
            }
        }

        public DTResult<Rly_Designation_FormModel> GetRailwaysDesignationList(DTParameters dtParameters)
        {

            DTResult<Rly_Designation_FormModel> dTResult = new() { draw = 0 };
            IQueryable<Rly_Designation_FormModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null && dtParameters.Order.Length > 0)
            {
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                if (string.IsNullOrEmpty(orderCriteria)) orderCriteria = "RlyDesigCd";
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                orderCriteria = "RlyDesigCd";
                orderAscendingDirection = true;
            }

            string DesignationCode = !string.IsNullOrEmpty(dtParameters.AdditionalValues["DesignationCode"]) ? Convert.ToString(dtParameters.AdditionalValues["DesignationCode"]) : "";
            string Designation = !string.IsNullOrEmpty(dtParameters.AdditionalValues["Designation"]) ? Convert.ToString(dtParameters.AdditionalValues["Designation"]) : "";

            query = from t90 in context.T90RlyDesignations
                    where (!string.IsNullOrEmpty(DesignationCode) ? t90.RlyDesigCd.ToLower().Contains(DesignationCode.ToLower()) : true)
                    && (!string.IsNullOrEmpty(Designation) ? t90.RlyDesigDesc.ToLower().Contains(Designation.ToLower()) : true)
                    select new Rly_Designation_FormModel
                    {
                        ID = t90.Id,
                        RlyDesigCd = t90.RlyDesigCd,
                        RlyDesigDesc = t90.RlyDesigDesc,
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.RlyDesigCd).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.RlyDesigDesc).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            if (dtParameters.Length == -1) dtParameters.Length = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public bool IsDuplicate(Rly_Designation_FormModel model)
        {
            if (model.ID == 0)
            {
                return context.T90RlyDesignations.Any(x => x.RlyDesigCd.ToLower() == model.RlyDesigCd.ToLower());
            }
            else
            {
                return false;
            }
        }

        public void SaveDetails(Rly_Designation_FormModel model)
        {
            if (model.ID == 0)
            {

                T90RlyDesignation railwayDesignation = new()
                {
                    RlyDesigCd = model.RlyDesigCd,
                    RlyDesigDesc = model.RlyDesigDesc,
                    UserId = model.UserId,
                    Datetime = DateTime.Now.Date,
                    Createdby = model.Createdby,
                    Createddate = DateTime.Now,
                };

                context.T90RlyDesignations.Add(railwayDesignation);
                context.SaveChanges();
            }
            else
            {
                T90RlyDesignation railwayDesignation = context.T90RlyDesignations.Where(x => x.Id == model.ID).FirstOrDefault();

                if (railwayDesignation != null)
                {
                    railwayDesignation.RlyDesigDesc = model.RlyDesigDesc;
                    railwayDesignation.UserId = model.UserId;
                    railwayDesignation.Datetime = DateTime.Now.Date;
                    railwayDesignation.Updatedby = model.Updatedby;
                    railwayDesignation.Updateddate = DateTime.Now;

                    context.SaveChanges();
                }
            }
        }

        public bool Remove(int id)
        {
            if (context.T90RlyDesignations.Any(x => x.Id == id))
            {
                context.T90RlyDesignations.RemoveRange(context.T90RlyDesignations.Where(x => x.Id == id).ToList());
                context.SaveChanges();
            }
            return true;
        }

        public string IsExistsRailwayDesignationCode(int id)
        {
            T90RlyDesignation railwayDesignation = context.T90RlyDesignations.Where(x => x.Id == id).FirstOrDefault();

            if (railwayDesignation != null)
            {
                if (context.T06Consignees.Any(x => x.ConsigneeDesig == railwayDesignation.RlyDesigCd && x.ConsigneeType == "R"))
                {
                    return "Consignee Master";
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }
    }
}


