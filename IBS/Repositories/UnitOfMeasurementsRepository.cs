using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace IBS.Repositories
{
    public class UnitOfMeasurementsRepository : IUnitOfMeasurementsRepository
    {
        private readonly ModelContext context;

        public UnitOfMeasurementsRepository(ModelContext context)
        {
            this.context = context;
        }

        public DTResult<UOMModel> GetUOMList(DTParameters dtParameters)
        {
            DTResult<UOMModel> dTResult = new() { draw = 0 };
            IQueryable<UOMModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null && dtParameters.Order.Length > 0)
            {
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                if (string.IsNullOrEmpty(orderCriteria)) orderCriteria = "UomSDesc";
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                orderCriteria = "UomSDesc";
                orderAscendingDirection = true;
            }

            string UOMShortDescription = !string.IsNullOrEmpty(dtParameters.AdditionalValues["UOMShortDescription"]) ? Convert.ToString(dtParameters.AdditionalValues["UOMShortDescription"]) : "";
            string UOMLongDescription = !string.IsNullOrEmpty(dtParameters.AdditionalValues["UOMLongDescription"]) ? Convert.ToString(dtParameters.AdditionalValues["UOMLongDescription"]) : "";
            decimal? DivisionFactor = !string.IsNullOrEmpty(dtParameters.AdditionalValues["DivisionFactor"]) ? Convert.ToDecimal(dtParameters.AdditionalValues["DivisionFactor"]) : null;

            query = from t04 in context.T04Uoms
                    where t04.Isdeleted != 1
                    && (!string.IsNullOrEmpty(UOMShortDescription) ? t04.UomSDesc.ToLower().Contains(UOMShortDescription.ToLower()) : true)
                    && (!string.IsNullOrEmpty(UOMLongDescription) ? t04.UomLDesc.ToLower().Contains(UOMLongDescription.ToLower()) : true)
                    && ((DivisionFactor != null) ? t04.UomFactor == DivisionFactor : true)
                    select new UOMModel
                    {
                        UomCd = t04.UomCd,
                        UomSDesc = t04.UomSDesc,
                        UomLDesc = t04.UomLDesc,
                        UomFactor = t04.UomFactor,
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.UomCd).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.UomSDesc).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.UomLDesc).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            if (dtParameters.Length == -1) dtParameters.Length = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public UOMModel FindByID(int id)
        {
            UOMModel model = new();
            T04Uom umo = context.T04Uoms.Find(Convert.ToByte(id));

            if (umo == null)
                return model;
            else
            {
                model.UomCd = umo.UomCd;
                model.UomSDesc = umo.UomSDesc;
                model.UomLDesc = umo.UomLDesc;
                model.UomFactor = umo.UomFactor;
                model.UserId = umo.UserId;
                return model;
            }
        }

        public int SaveDetails(UOMModel model)
        {
            if (model.UomCd == 0)
            {
                var Cnt = (from a in context.T04Uoms
                           where (a.UomSDesc == model.UomSDesc && a.UomFactor == model.UomFactor) || (a.UomLDesc == model.UomLDesc && a.UomFactor == model.UomFactor)
                           select a).Count();

                if (Cnt > 0) { return -1; }

                int UomCd = GetMaxUOM_CD();
                UomCd += 1;

                T04Uom uom = new()
                {
                    UomCd = Convert.ToByte(UomCd),
                    UomSDesc = model.UomSDesc,
                    UomLDesc = model.UomLDesc,
                    UomFactor = model.UomFactor,
                    UserId = model.UserId,
                    Datetime = DateTime.Now.Date,
                    Createdby = model.Createdby,
                    Createddate = DateTime.Now,
                    Isdeleted = model.Isdeleted,
                };

                context.T04Uoms.Add(uom);
                context.SaveChanges();
            }
            else
            {
                T04Uom uom = context.T04Uoms.Find(Convert.ToByte(model.UomCd));

                if (uom != null)
                {
                    uom.UomSDesc = model.UomSDesc;
                    uom.UomLDesc = model.UomLDesc;
                    uom.UomFactor = model.UomFactor;
                    uom.UserId = model.UserId;
                    uom.Datetime = DateTime.Now.Date;
                    uom.Updatedby = model.Updatedby;
                    uom.Updateddate = DateTime.Now;

                    context.SaveChanges();
                }
            }

            return model.UomCd;
        }

        public int GetMaxUOM_CD()
        {
            int UOM_CD = 0;

            using ModelContext context = new(DbContextHelper.GetDbContextOptions());
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                bool wasOpen = command.Connection.State == ConnectionState.Open;
                if (!wasOpen) command.Connection.Open();
                try
                {
                    command.CommandText = "select NVL(MAX(UOM_CD),0) from T04_UOM";
                    UOM_CD = Convert.ToInt32(command.ExecuteScalar());
                }
                finally
                {
                    if (!wasOpen) command.Connection.Close();
                }
            }
            return UOM_CD;
        }

        public bool Remove(int UomCd, int UserID)
        {
            var uom = context.T04Uoms.Find(Convert.ToByte(UomCd));
            if (uom == null) { return false; }

            uom.Isdeleted = 1;
            uom.Updatedby = UserID;
            uom.Updateddate = DateTime.Now;

            context.SaveChanges();
            return true;
        }
    }

}
