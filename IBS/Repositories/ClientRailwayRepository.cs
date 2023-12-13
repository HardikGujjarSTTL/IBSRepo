using DocumentFormat.OpenXml.Office2010.Excel;
using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Globalization;

namespace IBS.Repositories
{
    public class ClientRailwayRepository : IClientRailwayRepository
    {
        private readonly ModelContext context;

        public ClientRailwayRepository(ModelContext context)
        {
            this.context = context;
        }
        public DTResult<Railway> GetRailways(DTParameters dtParameters)
        {
            DTResult<Railway> dTResult = new() { draw = 0 };
            IQueryable<Railway>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "Railway";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                orderCriteria = "Railway";
                orderAscendingDirection = true;
            }

            query = from railway in context.T91Railways
                    select new Railway
                    {
                        RLY_CD = railway.RlyCd,
                        RAILWAY = railway.Railway,
                        HEAD_QUARTER = railway.HeadQuarter
                    };

            query = query.AsQueryable();

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.RAILWAY).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public int RailwayInsertUpdate(Railway model)
        {
            int RailwayId = 0;
            var Railway = (from r in context.T91Railways where r.RlyCd == model.RLY_CD select r).FirstOrDefault();
            var existingRailway = context.T91Railways.FirstOrDefault(r => r.RlyCd == model.RLY_CD);

            if (existingRailway == null)
            {
                #region Railway save
                if (Railway == null)
                {
                    T91Railway obj = new T91Railway();
                    obj.RlyCd = model.RLY_CD;
                    obj.Railway = model.RAILWAY;
                    obj.HeadQuarter = model.HEAD_QUARTER;
                    obj.ImmsRlyCd = model.IMMS_RLY_CD;
                    obj.UserId = model.USERID;
                    obj.Createdby = model.Createdby;
                    obj.Createddate = DateTime.Now;
                    context.T91Railways.Add(obj);
                    context.SaveChanges();
                    RailwayId = 1;
                }
                else
                {
                    Railway.RlyCd = model.RLY_CD;
                    Railway.Railway = model.RAILWAY;
                    Railway.HeadQuarter = model.HEAD_QUARTER;
                    Railway.ImmsRlyCd = model.IMMS_RLY_CD;
                    Railway.UserId = model.USERID;
                    Railway.Updatedby = model.Updatedby;
                    Railway.Updateddate = DateTime.Now;
                    context.SaveChanges();
                    RailwayId = 2;
                }
                #endregion
            }

            return RailwayId;
        }

        public Railway FindRailwayByID(string id)
        {
            Railway model = new();
            T91Railway Railways = (from r in context.T91Railways where r.RlyCd == id select r).FirstOrDefault();

            if (Railways == null)
                throw new Exception("Railway Not found");
            else
            {
                model.RLY_CD = Railways.RlyCd;
                model.RAILWAY = Railways.Railway;
                model.HEAD_QUARTER = Railways.HeadQuarter;
                model.IMMS_RLY_CD = Railways.ImmsRlyCd;
                return model;
            }
        }
    }
}
