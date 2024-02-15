using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;

namespace IBS.Repositories
{
    public class HolidayMasterRepository : IHolidayMasterRepository
    {
        private readonly ModelContext context;
        public HolidayMasterRepository(ModelContext _context)
        {
            context = _context;
        }

        #region Holiday Master
        public DTResult<HolidayMasterModel> GetHolidayMasterList(DTParameters dtParameters, string Region)
        {
            DTResult<HolidayMasterModel> dTResult = new() { draw = 0 };
            IQueryable<HolidayMasterModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null && dtParameters.Order.Length > 0)
            {
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                if (string.IsNullOrEmpty(orderCriteria)) orderCriteria = "Finance_Year";
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                orderCriteria = "Finance_Year";
                orderAscendingDirection = true;
            }

            query = from a in context.T111HolidayMasters
                    join b in context.T01Regions on a.Region equals b.RegionCode into regionJoin
                    from b in regionJoin.DefaultIfEmpty()
                    where (a.Isdeleted ?? 0) == 0 && a.Region == Region
                    select new HolidayMasterModel
                    {
                        ID = a.Id,
                        Finance_Year = a.FinancialYear,
                        FY_FR_DT = Convert.ToDateTime(a.FyFromDt),
                        FY_TO_DT = Convert.ToDateTime(a.FyToDt),
                        Region = a.Region != null ? b.Region : null
                    };

            dTResult.recordsTotal = query.Count();

            dTResult.recordsFiltered = query.Count();

            if (dtParameters.Length == -1) dtParameters.Length = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public HolidayMasterModel FindByID(int id)
        {
            HolidayMasterModel model = new HolidayMasterModel();
            model = (from a in context.T111HolidayMasters
                     where a.Id == id
                     select new HolidayMasterModel
                     {
                         ID = a.Id,
                         Finance_Year = a.FinancialYear,
                         FY_FR_DT = Convert.ToDateTime(a.FyFromDt),
                         FY_TO_DT = Convert.ToDateTime(a.FyToDt),
                         Region = a.Region
                     }).FirstOrDefault();
            return model;
        }

        public int HolidayMasterSave(HolidayMasterModel model)
        {
            if (model.ID == 0 || model.ID == null)
            {
                T111HolidayMaster holiday = new()
                {
                    FinancialYear = model.Finance_Year,
                    FyFromDt = model.FY_FR_DT,
                    FyToDt = model.FY_TO_DT,
                    UserId = model.User_Name,
                    Region = model.Region,
                    Createdby = model.CreatedBy,
                    Createddate = DateTime.Now
                };
                context.T111HolidayMasters.Add(holiday);
                context.SaveChanges();
                model.ID = holiday.Id;
            }
            else
            {
                T111HolidayMaster holiday = context.T111HolidayMasters.Find(model.ID);
                if (holiday != null)
                {
                    holiday.FinancialYear = model.Finance_Year;
                    holiday.FyFromDt = model.FY_FR_DT;
                    holiday.FyToDt = model.FY_TO_DT;
                    holiday.UserId = model.User_Name;
                    holiday.Region= model.Region;
                    holiday.Updatedby = model.UpdatedBy;
                    holiday.Updateddate = DateTime.Now;

                    context.SaveChanges();
                }
            }
            return Convert.ToInt32(model.ID);
        }

        public int HolidayMasterDelete(int id, HolidayMasterModel model)
        {
            T111HolidayMaster holiday = (from a in context.T111HolidayMasters
                                         where a.Id == id
                                         select a).FirstOrDefault();
            if (holiday != null)
            {
                holiday.Isdeleted = 1;
                holiday.UserId = model.User_Name.Length > 8 ? model.User_Name.Substring(0, 8) : model.User_Name;
                holiday.Updatedby = model.UpdatedBy;
                holiday.Updateddate = DateTime.Now;

                context.SaveChanges();
            }
            else
            {
                id = 0;
            }
            return id;
        }
        #endregion

        #region Holiday Detail
        public DTResult<HolidayDetailModel> GetHolidayDetailList(DTParameters dtParameters, string Region)
        {
            DTResult<HolidayDetailModel> dTResult = new() { draw = 0 };
            IQueryable<HolidayDetailModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null && dtParameters.Order.Length > 0)
            {
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                if (string.IsNullOrEmpty(orderCriteria)) orderCriteria = "ID";
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                orderCriteria = "ID";
                orderAscendingDirection = true;
            }

            var Holiday_ID = !string.IsNullOrEmpty(dtParameters.AdditionalValues["HOLIDAY_ID"]) ? Convert.ToInt32(dtParameters.AdditionalValues["HOLIDAY_ID"]) : 0;

            query = (from t112 in context.T112HolidayDetails
                     join t111 in context.T111HolidayMasters on t112.HolidayId equals t111.Id
                     join t01 in context.T01Regions on t111.Region equals t01.RegionCode into regionJoin
                     from t01 in regionJoin.DefaultIfEmpty()
                     where t112.HolidayId == Holiday_ID && t111.Region == Region
                     select new HolidayDetailModel
                     {
                         ID = t112.Id,
                         FINANCIAL_YEAR = t111.FinancialYear,
                         HOLIDAY_DT = t112.HolidayDt,
                         HOLIDAY_DESC = t112.HolidayDesc,
                         USER_NAME = t112.UserId,
                         REGION = t01.Region,
                     });

            if (dtParameters.Length == -1) dtParameters.Length = query.Count();

            dTResult.recordsTotal = query.Count();
            dTResult.recordsFiltered = query.Count();
            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();
            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public HolidayDetailModel Detail_FindByID(int id)
        {
            HolidayDetailModel model = new HolidayDetailModel();
            model = (from a in context.T112HolidayDetails
                     where a.Id == id
                     select new HolidayDetailModel
                     {
                         HOLIDAY_ID = a.HolidayId,
                         HOLIDAY_DT = a.HolidayDt,
                         HOLIDAY_DESC = a.HolidayDesc
                     }).FirstOrDefault();
            return model;
        }

        public int HolidayDetailSave(HolidayDetailModel model)
        {
            if (model.ID == 0 || model.ID == null)
            {
                T112HolidayDetail holiday = new()
                {
                    //HolidayId = Convert.ToInt32(model.HOLIDAY_ID),
                    HolidayDt = model.HOLIDAY_DT,
                    HolidayDesc = model.HOLIDAY_DESC,
                    UserId = model.USER_NAME.Length > 8 ? model.USER_NAME.Substring(0, 8) : model.USER_NAME,
                    Createdby = model.USER_ID,
                    Createddate = DateTime.Now
                };
                context.T112HolidayDetails.Add(holiday);
                context.SaveChanges();
                model.ID = holiday.Id;
            }
            else
            {
                T112HolidayDetail holiday = context.T112HolidayDetails.Find(model.ID);
                if (holiday != null)
                {
                    //holiday.HolidayId = Convert.ToInt32(model.HOLIDAY_ID);
                    holiday.HolidayDt = model.HOLIDAY_DT;
                    holiday.HolidayDesc = model.HOLIDAY_DESC;
                    holiday.UserId = model.USER_NAME.Length > 8 ? model.USER_NAME.Substring(0, 8) : model.USER_NAME;
                    holiday.Updatedby = model.USER_ID;
                    holiday.Updateddate = DateTime.Now;

                    context.SaveChanges();
                }
            }
            return Convert.ToInt32(model.ID);
        }

        public int HolidayDetailDelete(int id, HolidayDetailModel model)
        {
            T112HolidayDetail holiday = (from a in context.T112HolidayDetails
                                         where a.Id == id
                                         select a).FirstOrDefault();
            if (holiday != null)
            {
                holiday.Isdeleted = 1;
                holiday.UserId = model.USER_NAME.Length > 8 ? model.USER_NAME.Substring(0, 8) : model.USER_NAME;
                holiday.Updatedby = model.USER_ID;
                holiday.Updateddate = DateTime.Now;

                context.SaveChanges();
            }
            else
            {
                id = 0;
            }
            return id;
        }
        #endregion
    }
}
