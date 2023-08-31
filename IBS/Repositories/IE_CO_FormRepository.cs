using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace IBS.Repositories
{
    public class IE_CO_FormRepository : I_IE_CO_FormRepository
    {
        private readonly ModelContext context;

        public IE_CO_FormRepository(ModelContext context)
        {
            this.context = context;
        }

        public IE_CO_FormModel FindByID(int CoCd)
        {
            IE_CO_FormModel model = new();
            T08IeControllOfficer co = context.T08IeControllOfficers.Find(CoCd);

            if (co == null)
                return model;
            else
            {
                model.CoCd = co.CoCd;
                model.CoName = co.CoName;
                model.CoDesig = co.CoDesig;
                model.CoPhoneNo = co.CoPhoneNo;
                model.CoEmail = co.CoEmail;
                model.CoStatus = co.CoStatus;
                model.CoType = co.CoType;
                model.CoStatusDt = co.CoStatusDt;

                return model;
            }
        }

        public DTResult<IE_CO_FormModel> GetCOList(DTParameters dtParameters)
        {

            DTResult<IE_CO_FormModel> dTResult = new() { draw = 0 };
            IQueryable<IE_CO_FormModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null && dtParameters.Order.Length > 0)
            {
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                if (string.IsNullOrEmpty(orderCriteria)) orderCriteria = "CoCd";
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                orderCriteria = "CoCd";
                orderAscendingDirection = true;
            }

            query = from co in context.T08IeControllOfficers
                    join tmp1 in context.T07RitesDesigs on co.CoDesig equals tmp1.RDesigCd into _tmp1
                    from d in _tmp1.DefaultIfEmpty()
                    where co.Isdeleted != 1
                    select new IE_CO_FormModel
                    {
                        CoCd = co.CoCd,
                        CoName = co.CoName,
                        CoDesigName = d.RDesignation,
                        CoPhoneNo = co.CoPhoneNo,
                        CoEmail = co.CoEmail,
                        CoTypeName = EnumUtility<Enums.COType>.GetDescriptionByKey(co.CoType),
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.CoName).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.CoDesigName).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.CoPhoneNo).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.CoEmail).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            if (dtParameters.Length == -1) dtParameters.Length = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public int SaveDetails(IE_CO_FormModel model)
        {
            if (model.CoCd == 0)
            {
                int CoCd = GetMaxCoCd();
                CoCd += 1;

                T08IeControllOfficer co = new()
                {
                    CoCd = CoCd,
                    CoName = model.CoName,
                    CoDesig = model.CoDesig,
                    CoRegion = model.CoRegion,
                    CoPhoneNo = model.CoPhoneNo,
                    CoEmail = model.CoEmail,
                    CoStatus = model.CoStatus,
                    CoStatusDt = model.CoStatusDt,
                    CoType = model.CoType,
                    Createdby = model.Createdby,
                    Createddate = DateTime.Now,
                };

                context.T08IeControllOfficers.Add(co);
                context.SaveChanges();
            }
            else
            {
                T08IeControllOfficer co = context.T08IeControllOfficers.Find(model.CoCd);

                if (co != null)
                {
                    co.CoName = model.CoName;
                    co.CoDesig = model.CoDesig;
                    co.CoPhoneNo = model.CoPhoneNo;
                    co.CoEmail = model.CoEmail;
                    co.CoStatus = model.CoStatus;
                    co.CoStatusDt = model.CoStatusDt;
                    co.CoType = model.CoType;
                    co.Updatedby = model.Updatedby;
                    co.Updateddate = DateTime.Now;

                    context.SaveChanges();
                }
            }

            return model.CoCd;
        }

        public int GetMaxCoCd()
        {
            int CoCd = 0;

            using ModelContext context = new(DbContextHelper.GetDbContextOptions());
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                bool wasOpen = command.Connection.State == ConnectionState.Open;
                if (!wasOpen) command.Connection.Open();
                try
                {
                    command.CommandText = "SELECT NVL(MAX(CO_CD), 0) FROM T08_IE_CONTROLL_OFFICER";
                    CoCd = Convert.ToInt32(command.ExecuteScalar());
                }
                finally
                {
                    if (!wasOpen) command.Connection.Close();
                }
            }
            return CoCd;
        }

        public bool Remove(int CoCd, int UserID)
        {
            var co = context.T08IeControllOfficers.Find(CoCd);
            if (co == null) { return false; }

            co.Isdeleted = 1;
            co.Updatedby = Convert.ToInt32(UserID);
            co.Updateddate = DateTime.Now;

            context.SaveChanges();
            return true;
        }
    }
}
