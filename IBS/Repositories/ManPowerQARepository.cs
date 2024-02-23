using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;

namespace IBS.Repositories
{
    public class ManPowerQARepository : IManPowerQARepository
    {
        private readonly ModelContext context;

        public ManPowerQARepository(ModelContext context)
        {
            this.context = context;
        }

        public DTResult<ManpowerModel> GetMasterList(DTParameters dtParameters)
        {
            DTResult<ManpowerModel> dTResult = new() { draw = 0 };
            IQueryable<ManpowerModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null && dtParameters.Order.Length > 0)
            {
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                if (string.IsNullOrEmpty(orderCriteria)) orderCriteria = "EmpName";
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                orderCriteria = "EmpName";
                orderAscendingDirection = true;
            }

            string Region = !string.IsNullOrEmpty(dtParameters.AdditionalValues["Region"]) ? Convert.ToString(dtParameters.AdditionalValues["Region"]) : null;
            string EmpName = !string.IsNullOrEmpty(dtParameters.AdditionalValues["EmpName"]) ? Convert.ToString(dtParameters.AdditionalValues["EmpName"]) : null;
            string EmpNo = !string.IsNullOrEmpty(dtParameters.AdditionalValues["EmpNo"]) ? Convert.ToString(dtParameters.AdditionalValues["EmpNo"]) : null;

            query = from a in context.T116ManpowerMasters
                    where (Region == null || a.Region == Region)
                    && (EmpName == null || a.EmpName.ToLower().Contains(EmpName))
                    && (EmpNo == null || a.EmpNo.ToLower().Contains(EmpNo))
                    && (a.Isdeleted == 0 || a.Isdeleted == null)
                    select new ManpowerModel
                    {
                        ID = a.Id,
                        Region = a.Region == "N" ? "North" : a.Region == "S" ? "South" : a.Region == "W" ? "West" : a.Region == "E" ? "East" : a.Region == "C" ? "Central" : "",
                        EmpName = a.EmpName,
                        EmpNo = a.EmpNo,
                        Desig = a.Desig,
                        Cadre = a.Cadre,
                        Discp = a.Discp,
                        Status = a.Status,
                        Dob = a.Dob,
                        RitesDt = a.RitesDt,
                        RioDt = a.RioDt,
                        DrrtDt = a.DrrtDt
                    };
            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.EmpName).ToLower().Contains(searchBy.ToLower()) || w.EmpNo.ToLower().Contains(searchBy.ToLower()));

            dTResult.recordsFiltered = query.Count();
            if (dtParameters.Length == -1) dtParameters.Length = query.Count();
            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();
            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public ManpowerModel FindByID(int id)
        {
            ManpowerModel model = new ManpowerModel();
            model = (from a in context.T116ManpowerMasters
                     where a.Id == id && (a.Isdeleted == 0 || a.Isdeleted == null)
                     select new ManpowerModel
                     {
                         ID = a.Id,
                         Region = a.Region,
                         EmpName = a.EmpName,
                         EmpNo = a.EmpNo,
                         Desig = a.Desig,
                         Cadre = a.Cadre,
                         Discp = a.Discp,
                         Status = a.Status,
                         Dob = a.Dob,
                         RitesDt = a.RitesDt,
                         RioDt = a.RioDt,
                         DrrtDt = a.DrrtDt
                     }).FirstOrDefault();
            return model;
        }

        public int SaveMaster(ManpowerModel model)
        {
            int res = 0;            

            if (model.ID == 0)
            {
                T116ManpowerMaster obj = new T116ManpowerMaster();
                obj.Region = model.Region;
                obj.EmpName = model.EmpName; ;
                obj.EmpNo = model.EmpNo;
                obj.Desig = model.Desig;
                obj.Cadre = model.Cadre;
                obj.Discp = model.Discp;
                obj.Status = model.Status;
                obj.Dob = model.Dob;
                obj.RitesDt = model.RitesDt;
                obj.RioDt = model.RioDt;
                obj.DrrtDt = model.DrrtDt;
                obj.UserId = model.UserName;
                obj.Createdby = model.UserID;
                obj.Createddate = DateTime.Now;
                context.T116ManpowerMasters.Add(obj);
                context.SaveChanges();
                res = obj.Id;
            }
            else
            {
                var data = context.T116ManpowerMasters.Find(model.ID);  
                if(data != null)
                {
                    data.Region = model.Region;
                    data.EmpName = model.EmpName; ;
                    data.EmpNo = model.EmpNo;
                    data.Desig = model.Desig;
                    data.Cadre = model.Cadre;
                    data.Discp = model.Discp;
                    data.Status = model.Status;
                    data.Dob = model.Dob;
                    data.RitesDt = model.RitesDt;
                    data.RioDt = model.RioDt;
                    data.DrrtDt = model.DrrtDt;
                    data.UserId = model.UserName;
                    data.Updatedby = model.UserID;
                    data.Updateddate = DateTime.Now;
                    context.SaveChanges();
                    res = model.ID;
                }
            }
            return res;
        }

        public ManpowerDetailModel DetailFindByID(int id)
        {
            ManpowerDetailModel model = new ManpowerDetailModel();
            model = (from a in context.T117ManpowerDetails
                     where a.Id == id && (a.Isdeleted == 0 || a.Isdeleted == null)
                     select new ManpowerDetailModel
                     {
                         ID = a.Id,
                         Working = a.Working,
                         Staff = a.Staff,
                         PlacePosting = a.PlacePosting,
                         ProjectName = a.ProjectName
                     }).FirstOrDefault();
            return model;
        }

        public int SaveDetails(ManpowerDetailModel model)
        {
            int res = 0;

            if (model.ID == 0)
            {
                T117ManpowerDetail obj = new T117ManpowerDetail();
                obj.Working = model.Working;
                obj.Staff = model.Staff; ;
                obj.PlacePosting = model.PlacePosting;
                obj.ProjectName = model.ProjectName;                
                obj.UserId = model.UserName;
                obj.Createdby = model.UserID;
                obj.Createddate = DateTime.Now;
                context.T117ManpowerDetails.Add(obj);
                context.SaveChanges();
                res = obj.Id;
            }
            else
            {
                var data = context.T117ManpowerDetails.Find(model.ID);
                if (data != null)
                {
                    data.Working = model.Working;
                    data.Staff = model.Staff; ;
                    data.PlacePosting = model.PlacePosting;
                    data.ProjectName = model.ProjectName;
                    data.UserId = model.UserName;
                    data.Updatedby = model.UserID;
                    data.Updateddate = DateTime.Now;
                    context.SaveChanges();
                    res = model.ID;
                }
            }
            return res;
        }

        public DTResult<ManpowerDetailModel> GetDetailList(DTParameters dtParameters)
        {
            DTResult<ManpowerDetailModel> dTResult = new() { draw = 0 };
            IQueryable<ManpowerDetailModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null && dtParameters.Order.Length > 0)
            {
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                if (string.IsNullOrEmpty(orderCriteria)) orderCriteria = "EmpName";
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                orderCriteria = "EmpName";
                orderAscendingDirection = true;
            }

            string Working = !string.IsNullOrEmpty(dtParameters.AdditionalValues["Working"]) ? Convert.ToString(dtParameters.AdditionalValues["Working"]) : null;
            string Staff = !string.IsNullOrEmpty(dtParameters.AdditionalValues["Staff"]) ? Convert.ToString(dtParameters.AdditionalValues["Staff"]) : null;
            string PlacePosting = !string.IsNullOrEmpty(dtParameters.AdditionalValues["PlacePosting"]) ? Convert.ToString(dtParameters.AdditionalValues["PlacePosting"]) : null;
            string ProjectName = !string.IsNullOrEmpty(dtParameters.AdditionalValues["ProjectName"]) ? Convert.ToString(dtParameters.AdditionalValues["ProjectName"]) : null;

            query = from a in context.T117ManpowerDetails
                    join b in context.ProjectMasters on Convert.ToInt32(a.ProjectName) equals b.Id
                    where (Working == null || a.Working == Working)
                    && (Staff == null || a.Staff.ToLower().Contains(Staff))
                    && (PlacePosting == null || a.PlacePosting.ToLower().Contains(PlacePosting))
                    && (ProjectName == null || a.ProjectName == ProjectName)
                    && (b.Isdeleted == 0 || b.Isdeleted == null)
                    select new ManpowerDetailModel
                    {
                        ID = a.Id,
                        Working = a.Working == "S" ? "SBU" : a.Working == "H" ? "Head" : a.Working == "C" ? "C.M" : a.Working == "I" ? "I.E" : a.Working == "D" ? "DFO" : a.Working == "O" ?  "Other": "",
                        Staff = a.Staff == "T" ? "Technical" : a.Staff == "N" ? "Non Technical": "",
                        PlacePosting = a.PlacePosting,
                        ProjectName = b.Projectname
                    };
            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.Working).ToLower().Contains(searchBy.ToLower()) || w.Staff.ToLower().Contains(searchBy.ToLower()));

            dTResult.recordsFiltered = query.Count();
            if (dtParameters.Length == -1) dtParameters.Length = query.Count();
            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();
            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }
    }
}
