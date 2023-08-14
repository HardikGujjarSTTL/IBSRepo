using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
namespace IBS.Repositories
{
    public class CityMaster : ICityMaster
    {
        private readonly ModelContext context;

        public CityMaster(ModelContext context)
        {
            this.context = context;
        }
        public CityMasterModel FindByID(int CityCd)
        {
            CityMasterModel model = new();
            T03City role = context.T03Cities.Find(Convert.ToByte(CityCd));

            if (role == null)
                throw new Exception("Role Record Not found");
            else
            {
                model.CityCd = role.CityCd;
                model.Location = role.Location;
                model.City = role.City;
                model.StateCd = role.StateCd;
                model.Country = role.Country;
                model.UserId = role.UserId;
                model.Updatedby = role.Updatedby;
                model.Createdby = role.Createdby;
                model.Createddate = model.Createddate;
                model.Isdeleted = role.Isdeleted;
                return model;
            }
        }

        public DTResult<CityMasterModel>GetCityMasterList(DTParameters dtParameters)
        {

            DTResult<CityMasterModel> dTResult = new() { draw = 0 };
            IQueryable<CityMasterModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "CityCd";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "CityCd";
                orderAscendingDirection = true;
            }
            query = from l in context.T03Cities
                    where l.Isdeleted == 0 || l.Isdeleted == null
                    select new CityMasterModel
                    {
                        CityCd = l.CityCd,
                        Location = l.Location,
                        StateCd = l.StateCd,
                        City = l.City,
                        Country = l.Country,
                        UserId = l.UserId,
                        Isdeleted = l.Isdeleted,
                        Createddate = l.Createddate,
                        Createdby = l.Createdby,
                        Updateddate = l.Updateddate,
                        Updatedby = l.Updatedby
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.Location).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.Country).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }
        public bool Remove(int CityCd, int UserID)
        {
            var roles = context.T03Cities.Find(Convert.ToByte(CityCd));
            if (roles == null) { return false; }

            roles.Isdeleted = Convert.ToByte(true);
            roles.Updatedby = Convert.ToInt32(UserID);
            roles.Updateddate = DateTime.Now;
            context.SaveChanges();
            return true;
        }

        public int CityMasterDetailsInsertUpdate(CityMasterModel model)
        {
            int RoleId = 0;
            var CM = context.T03Cities.Where(x => x.CityCd == model.CityCd).FirstOrDefault();
            #region Role save
            if (CM == null || CM.CityCd == 0)
            {
                T03City obj = new T03City();

                obj.Location = model.Location;
                obj.City = model.City;
                obj.StateCd = model.StateCd;
                obj.Country = model.Country;
                obj.Updateddate = DateTime.Now;
                obj.Createdby = model.Createdby;
                obj.Isdeleted = Convert.ToByte(false);
                obj.Createddate = DateTime.Now;
                context.T03Cities.Add(obj);
                context.SaveChanges();
                RoleId = Convert.ToInt32(obj.CityCd);
            }
            else
            {
                CM.Location = model.Location;
                CM.City = model.City;
                CM.StateCd = model.StateCd;
                CM.Updatedby = model.Updatedby;
                CM.Updateddate = DateTime.Now;
                context.SaveChanges();
                RoleId = Convert.ToInt32(CM.CityCd);
            }
            #endregion
            return RoleId;
        }
    }

}