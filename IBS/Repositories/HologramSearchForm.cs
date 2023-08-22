using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;

namespace IBS.Repositories
{
    public class HologramSearchForm : IHologramSearchForm
    {
        private readonly ModelContext context;

        public HologramSearchForm(ModelContext context)
        {
            this.context = context;
        }
        public HologramSearchFormModel FindByID(int HgNoFr)
        {
            HologramSearchFormModel model = new();
            T31HologramIssued role = context.T31HologramIssueds.Find(Convert.ToByte(HgNoFr));

            if (role == null)
                throw new Exception("Role Record Not found");
            else
            {
                model.HgNoFr = role.HgNoFr;
                model.HgNoTo = role.HgNoTo;
                model.HgIssueDt = role.HgIssueDt;
                model.HgIecd = role.HgIecd;
                model.UserId = role.UserId;
                model.Updatedby = role.Updatedby;
                model.Createdby = role.Createdby;
                model.Createddate = model.Createddate;
                model.Isdeleted = role.Isdeleted;
                return model;
            }
        }

        public DTResult<HologramSearchFormModel>GetHologramSearchFormList(DTParameters dtParameters)
        {

            DTResult<HologramSearchFormModel> dTResult = new() { draw = 0 };
            IQueryable<HologramSearchFormModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "HgNoFr";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "HgNoFr";
                orderAscendingDirection = true;
            }
            query = from l in context.T31HologramIssueds
                    where l.Isdeleted == 0 || l.Isdeleted == null
                    select new HologramSearchFormModel
                    {
                        HgNoFr = l.HgNoFr,
                        HgNoTo = l.HgNoTo,
                        HgIssueDt = l.HgIssueDt,
                        HgIecd = l.HgIecd,
                        
                        UserId = l.UserId,
                        Isdeleted = l.Isdeleted,
                        Createddate = l.Createddate,
                        Createdby = l.Createdby,
                        Updateddate = l.Updateddate,
                        Updatedby = l.Updatedby
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.HgNoFr).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.HgNoTo).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }
        public bool Remove(string HgNoFr, int UserID)
        {
            var roles = context.T31HologramIssueds.Find(Convert.ToByte(HgNoFr));
            if (roles == null) { return false; }

            roles.Isdeleted = Convert.ToByte(true);
            roles.Updatedby = Convert.ToInt32(UserID);
            roles.Updateddate = DateTime.Now;
            context.SaveChanges();
            return true;
        }

        public int HologramSearchFormDetailsInsertUpdate(HologramSearchFormModel model)
        {
            int RoleId = 0;
            var SOF = context.T31HologramIssueds.Where(x => x.HgNoFr == model.HgNoFr).FirstOrDefault();
            #region Role save
           // if (SOF == null || SOF.HgNoFr == 0)
                if (SOF == null )
                {
                T31HologramIssued obj = new T31HologramIssued();

                obj.HgNoFr = model.HgNoFr;
                obj.HgNoTo = model.HgNoTo;
                obj.HgIssueDt = model.HgIssueDt;
                obj.HgIecd = model.HgIecd;
                obj.Createdby = model.Createdby;
                obj.Isdeleted = Convert.ToByte(false);
                obj.Createddate = DateTime.Now;
                context.T31HologramIssueds.Add(obj);
                context.SaveChanges();
                RoleId = Convert.ToInt32(obj.HgNoFr);
            }
            else
            {
                SOF.HgNoFr = model.HgNoFr;
                SOF.HgNoTo = model.HgNoTo;
                SOF.HgIssueDt = model.HgIssueDt;
                SOF.Updatedby = model.Updatedby;
                SOF.Updateddate = DateTime.Now;
                context.SaveChanges();
                RoleId = Convert.ToInt32(SOF.HgNoFr);
            }
            #endregion
            return RoleId;
        }
    }

}