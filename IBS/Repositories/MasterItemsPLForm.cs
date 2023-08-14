using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;

namespace IBS.Repositories
{
    public class MasterItemsPLForm : IMasterItemsPLForm
    {
        private readonly ModelContext context;

        public MasterItemsPLForm(ModelContext context)
        {
            this.context = context;
        }
        public MasterItemsPLFormModel FindByID(int ItemCd)
        {
            MasterItemsPLFormModel model = new();
            T62MasterItemPlno role = context.T62MasterItemPlnos.Find(Convert.ToByte(ItemCd));

            if (role == null)
                throw new Exception("Role Record Not found");
            else
            {
                model.ItemCd = role.ItemCd;
                model.PlNo = role.PlNo;
                model.UserId = role.UserId;
                model.Updatedby = role.Updatedby;
                model.Createdby = role.Createdby;
                model.Createddate = model.Createddate;
                model.Isdeleted = role.Isdeleted;
                return model;
            }
        }

        public DTResult<MasterItemsPLFormModel> GetMasterItemsPLFormList(DTParameters dtParameters)
        {

            DTResult<MasterItemsPLFormModel> dTResult = new() { draw = 0 };
            IQueryable<MasterItemsPLFormModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "ItemCd";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "ItemCd";
                orderAscendingDirection = true;
            }
            query = from l in context.T62MasterItemPlnos
                    where l.Isdeleted == 0 || l.Isdeleted == null
                    select new MasterItemsPLFormModel
                    {
                        ItemCd = l.ItemCd,
                        PlNo = l.PlNo,
                        UserId = l.UserId,
                        Isdeleted = l.Isdeleted,
                        Createddate = l.Createddate,
                        Createdby = l.Createdby,
                        Updateddate = l.Updateddate,
                        Updatedby = l.Updatedby
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.ItemCd).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.PlNo).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }
        public bool Remove(int ItemCd, int UserID)
        {
            var roles = context.T62MasterItemPlnos.Find(Convert.ToByte(ItemCd));
            if (roles == null) { return false; }

            roles.Isdeleted = Convert.ToByte(true);
            roles.Updatedby = Convert.ToInt32(UserID);
            roles.Updateddate = DateTime.Now;
            context.SaveChanges();
            return true;
        }

        public int MasterItemsPLFormDetailsInsertUpdate(MasterItemsPLFormModel model)
        {
            int RoleId = 0;
            var MSIL = context.T62MasterItemPlnos.Where(x => x.ItemCd == model.ItemCd).FirstOrDefault();
            #region Role save
           // if (MSIL == null || MSIL.ItemCd == 0)
                if (MSIL == null )
                {
                T62MasterItemPlno obj = new T62MasterItemPlno();
                obj.ItemCd = model.ItemCd;
                obj.PlNo = model.PlNo;
                obj.Createdby = model.Createdby;
                obj.Isdeleted = Convert.ToByte(false);
                obj.Createddate = DateTime.Now;
                context.T62MasterItemPlnos.Add(obj);
                context.SaveChanges();
                RoleId = Convert.ToInt32(obj.ItemCd);
            }
            else
            {
                MSIL.PlNo = model.PlNo;
                MSIL.Updatedby = model.Updatedby;
                MSIL.Updateddate = DateTime.Now;
                context.SaveChanges();
                RoleId = Convert.ToInt32(MSIL.ItemCd);
            }
            #endregion
            return RoleId;
        }
    }

}