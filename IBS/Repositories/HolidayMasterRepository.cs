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

        public DTResult<HolidayMasterModel> GetHolidayMasterList(DTParameters dtParameters)
        {
            DTResult<HolidayMasterModel> dTResult = new() { draw = 0 };
            IQueryable<HolidayMasterModel>? query = null;

            DTResult<BankMasterModel> dTResult1 = new() { draw = 0 };
            IQueryable<BankMasterModel>? query1 = null;

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

            //string BankName = !string.IsNullOrEmpty(dtParameters.AdditionalValues["BankName"]) ? Convert.ToString(dtParameters.AdditionalValues["BankName"]) : "";
            //int? FMISBankCD = !string.IsNullOrEmpty(dtParameters.AdditionalValues["FMISBankCD"]) ? Convert.ToInt32(dtParameters.AdditionalValues["FMISBankCD"]) : null;

            query = from a in context.T111HolidayMasters
                    select new HolidayMasterModel
                    {
                        ID = a.Id,
                        Finance_Year = a.FinancialYear,
                        FY_FR_DT = Convert.ToDateTime(a.FyFromDt),
                        FY_TO_DT = Convert.ToDateTime(a.FyToDt)
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
                     select new HolidayMasterModel { 
                         ID = a.Id,
                         Finance_Year = a.FinancialYear,
                         FY_FR_DT = Convert.ToDateTime(a.FyFromDt),
                         FY_TO_DT =  Convert.ToDateTime(a.FyToDt)
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
                    holiday.Updatedby = model.UpdatedBy;
                    holiday.Updateddate = DateTime.Now;

                    context.SaveChanges();
                }
            }
            return Convert.ToInt32(model.ID);
        }
    }
}
