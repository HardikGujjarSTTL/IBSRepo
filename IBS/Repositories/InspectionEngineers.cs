using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;

namespace IBS.Repositories
{
    public class InspectionEngineers : IInspectionEngineers
    {
        private readonly ModelContext context;

        public InspectionEngineers(ModelContext context)
        {
            this.context = context;
        }
        public InspectionEngineersModel FindByID(int IeCd)
        {
            InspectionEngineersModel model = new();
            T09Ie role = context.T09Ies.Find(IeCd);

            if (role == null)
                throw new Exception("Role Record Not found");
            else
            {
                model.IeCd = role.IeCd;
                model.IeName = role.IeName;
                model.IeSname = role.IeSname;
                model.IeEmpNo = role.IeEmpNo;
                model.IeDesig = role.IeDesig;
                model.IeSealNo = role.IeSealNo;
                model.IeCityCd = role.IeCityCd;
                model.IeDepartment = role.IeDepartment;
                model.UserId = role.UserId;
                model.Updatedby = role.Updatedby;
                model.Createdby = role.Createdby;
                model.Createddate = model.Createddate;
                model.Isdeleted = role.Isdeleted;
                return model;
            }
        }

        public DTResult<InspectionEngineersModel> GetInspectionEngineersList(DTParameters dtParameters)
        {

            DTResult<InspectionEngineersModel> dTResult = new() { draw = 0 };
            IQueryable<InspectionEngineersModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "IeCd";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "IeCd";
                orderAscendingDirection = true;
            }
            query = from l in context.T09Ies
                    where l.Isdeleted == 0 || l.Isdeleted == null
                    select new InspectionEngineersModel
                    {
                        IeCd = l.IeCd,
                        IeName = l.IeName,
                        IeSname = l.IeSname,
                        IeEmpNo = l.IeEmpNo,
                        IeDesig = l.IeDesig,
                        IeSealNo = l.IeSealNo,
                        IeCityCd = l.IeCityCd,
                        IeDepartment = l.IeDepartment,
                        UserId = l.UserId,
                        Isdeleted = l.Isdeleted,
                        Createddate = l.Createddate,
                        Createdby = l.Createdby,
                        Updateddate = l.Updateddate,
                        Updatedby = l.Updatedby
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.IeCd).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.IeName).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }
        public bool Remove(int IeCd, int UserID)
        {
            var roles = context.T09Ies.Find(Convert.ToByte(IeCd));
            if (roles == null) { return false; }

            roles.Isdeleted = Convert.ToByte(true);
            roles.Updatedby = Convert.ToInt32(UserID);
            roles.Updateddate = DateTime.Now;
            context.SaveChanges();
            return true;
        }

        public int InspectionEngineersDetailsInsertUpdate(InspectionEngineersModel model)
        {
            int RoleId = 0;
            var IE = context.T09Ies.Where(x => x.IeCd == model.IeCd).FirstOrDefault();
            #region Role save
            if (IE == null || IE.IeCd == 0)
            {
                T09Ie obj = new T09Ie();
                obj.IeCoCd = model.IeCoCd;
                obj.IeCityCd = model.IeCityCd;
                obj.IeDepartment = model.IeDepartment;
                obj.Createdby = model.Createdby;
                obj.Isdeleted = Convert.ToByte(false);
                obj.Createddate = DateTime.Now;
                context.T09Ies.Add(obj);
                context.SaveChanges();
                RoleId = Convert.ToInt32(obj.IeCd);
            }
            else
            {
                IE.IeSname = model.IeSname;
                IE.IeCityCd = model.IeCityCd;
                IE.IeDesig = model.IeDesig;
                IE.Updatedby = model.Updatedby;
                IE.Updateddate = DateTime.Now;
                context.SaveChanges();
                RoleId = Convert.ToInt32(IE.IeCd);
            }
            #endregion
            return RoleId;
        }
    }

}

