using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;

namespace IBS.Repositories
{
    public class IE_MaximumCallLimitForm : I_IE_MaximumCallLimitForm
    {
        private readonly ModelContext context;

        public IE_MaximumCallLimitForm(ModelContext context)
        {
            this.context = context;
        }
        public IE_MaximumCallLimitFormModel FindByID(string RegionCode)
        {
            IE_MaximumCallLimitFormModel model = new();
            var role = context.T102IeMaximumCallLimits.Where(x => x.RegionCode == RegionCode).FirstOrDefault();

            if (role == null)
                throw new Exception("Role Record Not found");
            else
            {
                model.RegionCode = role.RegionCode;
                model.MaximumCall = role.MaximumCall;
                model.UserId = role.UserId;
                model.Updatedby = role.Updatedby;
                model.Createdby = role.Createdby;
                model.Createddate = model.Createddate;
                model.Isdeleted = role.Isdeleted;
                return model;
            }
        }

        public DTResult<IE_MaximumCallLimitFormModel> GetIE_MaximumCallLimitFormList(DTParameters dtParameters, string GetRegionCode)
        {

            DTResult<IE_MaximumCallLimitFormModel> dTResult = new() { draw = 0 };
            IQueryable<IE_MaximumCallLimitFormModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "RegionCode";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "RegionCode";
                orderAscendingDirection = true;
            }
            query = from l in context.T102IeMaximumCallLimits
                    where l.Isdeleted == 0 || l.Isdeleted == null && l.RegionCode == GetRegionCode
                    select new IE_MaximumCallLimitFormModel
                    {
                        RegionCode = l.RegionCode,
                        MaximumCall = l.MaximumCall,
                        UserId = l.UserId,
                        Isdeleted = l.Isdeleted,
                        Createddate = l.Createddate,
                        Createdby = l.Createdby,
                        Updateddate = l.Updateddate,
                        Updatedby = l.Updatedby
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.RegionCode).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.MaximumCall).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }
        public bool Remove(int RegionCode, int UserID)
        {
            var roles = context.T102IeMaximumCallLimits.Find(Convert.ToByte(RegionCode));
            if (roles == null) { return false; }

            roles.Isdeleted = Convert.ToByte(true);
            roles.Updatedby = Convert.ToInt32(UserID);
            roles.Updateddate = DateTime.Now;
            context.SaveChanges();
            return true;
        }

        public string IE_MaximumCallLimitFormDetailsInsertUpdate(IE_MaximumCallLimitFormModel model)
        {
            string RegionID = "";
            var MCL = context.T102IeMaximumCallLimits.Where(x => x.RegionCode == model.RegionCode).FirstOrDefault();

            #region Region save
            if (MCL == null)
            {
                T102IeMaximumCallLimit obj = new T102IeMaximumCallLimit();

                obj.RegionCode = model.RegionCode;
                obj.MaximumCall = model.MaximumCall;
                obj.UserId = Convert.ToString(model.Createdby);
                obj.Datetime = DateTime.Now;
                obj.Isdeleted = 0;
                obj.Createdby = model.Createdby;
                obj.Createddate = DateTime.Now;
                context.T102IeMaximumCallLimits.Add(obj);
                context.SaveChanges();
                RegionID = obj.RegionCode;
            }
            else
            {
                MCL.MaximumCall = model.MaximumCall;
                MCL.Isdeleted = 0;
                MCL.Updatedby = model.Updatedby;
                MCL.Updateddate = DateTime.Now;
                context.SaveChanges();

                T102IeMaximumCallLimitHistory obj1 = new T102IeMaximumCallLimitHistory();
                obj1.RegionCode = model.RegionCode;
                obj1.MaximumCall = model.MaximumCall;
                obj1.UserId = Convert.ToString(model.Createdby);
                obj1.Datetime = DateTime.Now;
                obj1.Isdeleted = 0;
                obj1.Createdby = model.Createdby;
                obj1.Createddate = DateTime.Now;
                context.T102IeMaximumCallLimitHistories.Add(obj1);
                context.SaveChanges();
                RegionID = MCL.RegionCode;
            }
            #endregion
            return RegionID;
        }
    }

}

