using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace IBS.Repositories
{
    public class CityRepository : ICityRepository
    {
        private readonly ModelContext context;

        public CityRepository(ModelContext context)
        {
            this.context = context;
        }

        public CityMasterModel FindByID(int CityCd)
        {
            CityMasterModel model = new();
            T03City city = context.T03Cities.Find(CityCd);

            if (city == null)
                return model;
            else
            {
                model.CityCd = city.CityCd;
                model.Location = city.Location;
                model.City = city.City;
                model.StateCd = city.StateCd;
                model.PinCode = city.PinCode;
                model.CountryCd = city.CountryCd;
                model.UserId = city.UserId;
                return model;
            }
        }

        public DTResult<CityMasterModel> GetCityMasterList(DTParameters dtParameters)
        {

            DTResult<CityMasterModel> dTResult = new() { draw = 0 };
            IQueryable<CityMasterModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null && dtParameters.Order.Length > 0)
            {
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                if (string.IsNullOrEmpty(orderCriteria)) orderCriteria = "CityCd";
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                orderCriteria = "CityCd";
                orderAscendingDirection = true;
            }

            query = from c in context.T03Cities
                    join tmp1 in context.T92States on c.StateCd equals tmp1.StateCd into _tmp1
                    from s in _tmp1.DefaultIfEmpty()
                    join tmp2 in context.CountryMasters on c.CountryCd equals tmp2.CountryCode into _tmp2
                    from cn in _tmp2.DefaultIfEmpty()
                    select new CityMasterModel
                    {
                        CityCd = c.CityCd,
                        Location = c.Location,
                        PinCode = c.PinCode,
                        City = c.City,
                        State = s.StateName,
                        Country = cn.CountryName
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.Location).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.Country).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            if (dtParameters.Length == -1) dtParameters.Length = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public int SaveDetails(CityMasterModel model)
        {
            if (model.CityCd == 0)
            {
                int CityCd = GetMaxCityCd();
                CityCd += 1;

                T03City city = new()
                {
                    CityCd = CityCd,
                    Location = model.Location,
                    City = model.City,
                    StateCd = model.StateCd,
                    UserId = model.UserId,
                    Datetime = DateTime.Now.Date,
                    PinCode = model.PinCode,
                    Createdby = model.Createdby,
                    Createddate = DateTime.Now,
                    CountryCd = model.CountryCd,
                };

                context.T03Cities.Add(city);
                context.SaveChanges();
            }
            else
            {
                T03City city = context.T03Cities.Find(model.CityCd);

                if (city != null)
                {
                    city.Location = model.Location;
                    city.City = model.City;
                    city.StateCd = model.StateCd;
                    city.UserId = model.UserId;
                    city.Datetime = DateTime.Now.Date;
                    city.PinCode = model.PinCode;
                    city.Updatedby = model.Updatedby;
                    city.Updateddate = DateTime.Now;
                    city.CountryCd = model.CountryCd;

                    context.SaveChanges();
                }
            }

            return model.CityCd;
        }

        public int GetMaxCityCd()
        {
            int CityCd = 0;

            using ModelContext context = new(DbContextHelper.GetDbContextOptions());
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                bool wasOpen = command.Connection.State == ConnectionState.Open;
                if (!wasOpen) command.Connection.Open();
                try
                {
                    command.CommandText = "SELECT NVL(MAX(CITY_CD), 0) FROM T03_CITY";
                    CityCd = Convert.ToInt32(command.ExecuteScalar());
                }
                finally
                {
                    if (!wasOpen) command.Connection.Close();
                }
            }
            return CityCd;
        }

        public bool Remove(int CityCd)
        {
            if (context.T03Cities.Any(x => x.CityCd == CityCd))
            {
                context.T03Cities.RemoveRange(context.T03Cities.Where(x => x.CityCd == CityCd).ToList());
                context.SaveChanges();
            }
            return true;
        }
    }
}