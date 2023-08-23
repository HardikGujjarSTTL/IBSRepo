using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;

namespace IBS.Repositories
{
    public class RitesDesignationMaster : IRitesDesignationMaster
    {
        private readonly ModelContext context;

        public RitesDesignationMaster(ModelContext context)
        {
            this.context = context;
        }
        public RDMModel FindByID(int RdmCd)
        {
            RDMModel model = new();
            T07RitesDesig role = context.T07RitesDesigs.Find(RdmCd);

            if (role == null)
                throw new Exception("Role Record Not found");
            else
            {
                model.RDesigCd = (byte)role.RDesigCd;
                model.RDesignation = role.RDesignation;
                model.Updatedby = role.Updatedby;
                model.Createdby = role.Createdby;
                model.Createddate = model.Createddate;
                model.Updateddate = model.Updateddate;
                model.Isdeleted = role.Isdeleted;
                return model;
            }
        }

        public DTResult<RDMModel>GetRDMList(DTParameters dtParameters)
        {

            DTResult<RDMModel> dTResult = new() { draw = 0 };
            IQueryable<RDMModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "RDesigCd";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "RDesigCd";
                orderAscendingDirection = true;
            }
            query = from l in context.T07RitesDesigs
                    where l.Isdeleted == 0 || l.Isdeleted == null
                    select new RDMModel
                    {
                        RDesigCd = l.RDesigCd,
                        RDesignation = l.RDesignation,
                        Isdeleted = l.Isdeleted,
                        Createddate = l.Createddate,
                        Createdby = l.Createdby,
                        Updateddate = l.Updateddate,
                        Updatedby = l.Updatedby
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.RDesignation).ToLower().Contains(searchBy.ToLower())
               
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }
        public bool Remove(int RdmCd, int UserID)
        {
            var roles = context.T07RitesDesigs.Find(RdmCd);
            if (roles == null) { return false; }

            roles.Isdeleted = Convert.ToByte(true);
            roles.Updatedby = Convert.ToInt32(UserID);
            roles.Updateddate = DateTime.Now;
            context.SaveChanges();
            return true;
        }

        public int RDMDetailsInsertUpdate(RDMModel model)
        {
            int RoleId = 0;
            var RDM = context.T07RitesDesigs.Where(x => x.RDesigCd == model.RDesigCd).FirstOrDefault();
            #region Role save
            if (RDM == null || RDM.RDesigCd == 0)
            {
                T07RitesDesig obj = new T07RitesDesig();

                obj.RDesigCd = model.RDesigCd;
                obj.RDesignation = model.RDesignation;
                obj.Createdby = model.Createdby;
                obj.Isdeleted = Convert.ToByte(false);
                obj.Createddate = DateTime.Now;
                context.T07RitesDesigs.Add(obj);
                context.SaveChanges();
                RoleId = Convert.ToInt32(obj.RDesigCd);
            }
            else
            {
                RDM.RDesigCd = model.RDesigCd;
                RDM.RDesignation = model.RDesignation;
                RDM.Updatedby = model.Updatedby;
                RDM.Updateddate = DateTime.Now;
                context.SaveChanges();
                RoleId = Convert.ToInt32(RDM.RDesigCd);
            }
            #endregion
            return RoleId;
        }

    }

}
