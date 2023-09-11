using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace IBS.Repositories
{
    public class RitesDesignationMasterRepository : IRitesDesignationMasterRepository
    {
        private readonly ModelContext context;

        public RitesDesignationMasterRepository(ModelContext context)
        {
            this.context = context;
        }

        public RDMModel FindByID(int RdmCd)
        {
            RDMModel model = new();
            T07RitesDesig role = context.T07RitesDesigs.Find(RdmCd);

            if (role == null)
                return model;
            else
            {
                model.RDesigCd = role.RDesigCd;
                model.RDesignation = role.RDesignation;
                return model;
            }
        }

        public DTResult<RDMModel> GetRDMList(DTParameters dtParameters)
        {

            DTResult<RDMModel> dTResult = new() { draw = 0 };
            IQueryable<RDMModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null && dtParameters.Order.Length > 0)
            {
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                if (string.IsNullOrEmpty(orderCriteria)) orderCriteria = "RDesigCd";
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                orderCriteria = "RDesigCd";
                orderAscendingDirection = true;
            }

            string Designation = !string.IsNullOrEmpty(dtParameters.AdditionalValues["Designation"]) ? Convert.ToString(dtParameters.AdditionalValues["Designation"]) : "";

            query = from t07 in context.T07RitesDesigs
                    where t07.Isdeleted != 1
                    && (!string.IsNullOrEmpty(Designation) ? t07.RDesignation.ToLower().Contains(Designation.ToLower()) : true)
                    select new RDMModel
                    {
                        RDesigCd = t07.RDesigCd,
                        RDesignation = t07.RDesignation,
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.RDesigCd).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.RDesignation).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            if (dtParameters.Length == -1) dtParameters.Length = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public int SaveDetails(RDMModel model)
        {
            if (model.RDesigCd == 0)
            {
                int Design_Cd = GetMaxDesign_Cd();
                Design_Cd += 1;

                T07RitesDesig ritesDesg = new()
                {
                    RDesigCd = Design_Cd,
                    RDesignation = model.RDesignation,
                    Createdby = model.Createdby,
                    Createddate = DateTime.Now,
                    Isdeleted = model.Isdeleted,
                };

                context.T07RitesDesigs.Add(ritesDesg);
                context.SaveChanges();
            }
            else
            {
                T07RitesDesig ritesDesg = context.T07RitesDesigs.Find(model.RDesigCd);

                if (ritesDesg != null)
                {
                    ritesDesg.RDesignation = model.RDesignation;
                    ritesDesg.Updatedby = model.Updatedby;
                    ritesDesg.Updateddate = DateTime.Now;

                    context.SaveChanges();
                }
            }

            return model.RDesigCd;
        }

        public bool Remove(int RdmCd, int UserID)
        {
            var roles = context.T07RitesDesigs.Find(RdmCd);
            if (roles == null) { return false; }

            roles.Isdeleted = 1;
            roles.Updatedby = UserID;
            roles.Updateddate = DateTime.Now;

            context.SaveChanges();
            return true;
        }

        public int GetMaxDesign_Cd()
        {
            int Design_Cd = 0;

            using ModelContext context = new(DbContextHelper.GetDbContextOptions());
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                bool wasOpen = command.Connection.State == ConnectionState.Open;
                if (!wasOpen) command.Connection.Open();
                try
                {
                    command.CommandText = "SELECT NVL(MAX(R_DESIG_CD), 0) FROM T07_RITES_DESIG";
                    Design_Cd = Convert.ToInt32(command.ExecuteScalar());
                }
                finally
                {
                    if (!wasOpen) command.Connection.Close();
                }
            }
            return Design_Cd;
        }
    }

}
