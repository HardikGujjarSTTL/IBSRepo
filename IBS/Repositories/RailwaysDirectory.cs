using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;

namespace IBS.Repositories
{
    public class RailwaysDirectory : IRailwaysDirectory
    {
        private readonly ModelContext context;
        public RailwaysDirectory(ModelContext context)
        {
            this.context = context;
        }
        public RailwaysDirectoryModel FindByID(int RlyCd)
        {
            RailwaysDirectoryModel model = new();
            T91Railway role = context.T91Railways.Find(Convert.ToByte(RlyCd));

            if (role == null)
                throw new Exception("Role Record Not found");
            else
            {
                model.Railway = role.Railway;
                model.HeadQuarter = role.HeadQuarter;
                model.RlyCd = role.RlyCd;
                model.UserId = role.UserId;
                model.Updatedby = role.Updatedby;
                model.Createdby = role.Createdby;
                model.Createddate = model.Createddate;
                model.Isdeleted = role.Isdeleted;
                return model;
            }
        }

        public DTResult<RailwaysDirectoryModel>GetRMList(DTParameters dtParameters)
        {

            DTResult<RailwaysDirectoryModel> dTResult = new() { draw = 0 };
            IQueryable<RailwaysDirectoryModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "RlyCd";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "RlyCd";
                orderAscendingDirection = true;
            }
            query = from l in context.T91Railways
                    where l.Isdeleted == 0 || l.Isdeleted == null
                    select new RailwaysDirectoryModel
                    {
                        Railway = l.Railway,
                        HeadQuarter = l.HeadQuarter,
                        RlyCd = l.RlyCd,
                        UserId = l.UserId,
                        Datetime = l.Datetime,
                        ImmsRlyCd = l.ImmsRlyCd,
                        Isdeleted = l.Isdeleted,
                        Createddate = l.Createddate,
                        Createdby = l.Createdby,
                        Updateddate = l.Updateddate,
                        Updatedby = l.Updatedby
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.RlyCd).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.Railway).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }
        public bool Remove(int RlyCd, int UserID)
        {
            var roles = context.T91Railways.Find(RlyCd);
            if (roles == null) { return false; }

            roles.Isdeleted = Convert.ToByte(true);
            roles.Updatedby = Convert.ToInt32(UserID);
            roles.Updateddate = DateTime.Now;
            context.SaveChanges();
            return true;
        }

        public int RailwaysDirectoryDetailsInsertUpdate(RailwaysDirectoryModel model)
        {
            int RoleId = 0;
            var ROM = context.T91Railways.Where(x => x.RlyCd == model.RlyCd).FirstOrDefault();
            #region Role save
            if (ROM == null || ROM.Id == 0)
            {
                T91Railway obj = new T91Railway();


                obj.RlyCd = model.RlyCd;
                obj.Railway = model.Railway;
                obj.HeadQuarter = model.HeadQuarter;
                obj.Createdby = model.Createdby;
                obj.Isdeleted = Convert.ToByte(false);
                obj.Createddate = DateTime.Now;
                context.T91Railways.Add(obj);
                context.SaveChanges();
                RoleId = Convert.ToInt32(obj.RlyCd);
            }
            else
            {
                ROM.Railway = model.Railway;
                ROM.HeadQuarter = model.HeadQuarter;
                ROM.Updatedby = model.Updatedby;
                ROM.Updateddate = DateTime.Now;
                context.SaveChanges();
                RoleId = Convert.ToInt32(ROM.RlyCd);
            }
            #endregion
            return RoleId;
        }

           }

}