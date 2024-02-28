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
            if (model != null)
            {
                List<ManpowerDetailModel> clst = (from T117 in context.T117ManpowerDetails
                                                  where T117.Manpowerid == model.ID
                                                  select new
                                                  ManpowerDetailModel
                                                  {
                                                      DetailID = Convert.ToInt32(T117.Id),
                                                      ManpowerID = Convert.ToInt32(T117.Manpowerid),
                                                      Working = T117.Working,
                                                      WorkingText = T117.Working == "S" ? "SBU" : T117.Working == "H" ? "Head" : T117.Working == "C" ? "C.M" : T117.Working == "I" ? "I.E" : T117.Working == "D" ? "DFO" : T117.Working == "O" ? "Other" : "",
                                                      Staff = T117.Staff,
                                                      StaffText = T117.Staff == "T" ? "Technical" : T117.Staff == "N" ? "Non Technical" : "",
                                                      PlacePosting = T117.PlacePosting,
                                                      ProjectName = T117.ProjectName,
                                                      ProjectNameText =(from f in context.ProjectMasters where f.Id == T117.ProjectName select f.Projectname).FirstOrDefault()
                                                  }).ToList();
                model.lstManpowerDetailModel = clst;
            }
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
                obj.Isdeleted = Convert.ToByte(false);
                context.T116ManpowerMasters.Add(obj);
                context.SaveChanges();
                res = obj.Id;
            }
            else
            {
                var data = context.T116ManpowerMasters.Find(model.ID);
                if (data != null)
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
                    data.Isdeleted = Convert.ToByte(false);
                    context.SaveChanges();
                    res = model.ID;
                }
            }

            var t117ManpowerDetails = (from T117 in context.T117ManpowerDetails where T117.Manpowerid == model.ID select T117).ToList();
            if (t117ManpowerDetails.Count > 0 && t117ManpowerDetails != null)
            {
                context.T117ManpowerDetails.RemoveRange(t117ManpowerDetails);
                context.SaveChanges();
            }
            if (model.lstManpowerDetailModel != null)
            {
                foreach (var item in model.lstManpowerDetailModel)
                {
                    T117ManpowerDetail objAdd = new T117ManpowerDetail();
                    {
                        objAdd.Manpowerid = res;
                        objAdd.Working = item.Working;
                        objAdd.Staff = item.Staff;
                        objAdd.PlacePosting = item.PlacePosting;
                        objAdd.ProjectName = item.ProjectName;
                        objAdd.Createdby = model.UserID;
                        objAdd.Createddate = DateTime.Now;
                        objAdd.Isdeleted = Convert.ToByte(false);
                    }
                    context.T117ManpowerDetails.Add(objAdd);
                    context.SaveChanges();
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
                         DetailID = a.Id,
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
            if (model.DetailID == 0)
            {
                T117ManpowerDetail objAdd = new T117ManpowerDetail();
                objAdd.Manpowerid = model.ManpowerID;
                objAdd.Working = model.Working;
                objAdd.Staff = model.Staff;
                objAdd.PlacePosting = model.PlacePosting;
                objAdd.ProjectName = model.ProjectName;
                objAdd.Createdby = model.UserID;
                objAdd.Createddate = DateTime.Now;
                objAdd.Isdeleted = Convert.ToByte(false);
                context.T117ManpowerDetails.Add(objAdd);
                context.SaveChanges();
                res = objAdd.Id;
            }
            else
            {
                var data = context.T117ManpowerDetails.Find(model.DetailID);
                if (data != null)
                {
                    data.Working = model.Working;
                    data.Staff = model.Staff; ;
                    data.PlacePosting = model.PlacePosting;
                    data.ProjectName = model.ProjectName;
                    data.Updatedby = model.UserID;
                    data.Updateddate = DateTime.Now;
                    context.SaveChanges();
                    res = data.Id;
                }
            }
            return res;
        }

        public DTResult<ManpowerDetailModel> GetManpowerDetailList(DTParameters dtParameters, List<ManpowerDetailModel> manpowerDetailModels)
        {
            DTResult<ManpowerDetailModel> dTResult = new() { draw = 0 };
            IQueryable<ManpowerDetailModel>? query = null;
            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "DetailID";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "DetailID";
                orderAscendingDirection = true;
            }

            query = (from u in manpowerDetailModels.OrderBy(x => x.DetailID)
                     select new ManpowerDetailModel
                     {
                         DetailID = u.DetailID,
                         ManpowerID = Convert.ToInt32(u.ManpowerID),
                         Working = u.Working,
                         WorkingText = u.WorkingText,
                         Staff = u.Staff,
                         StaffText = u.StaffText,
                         PlacePosting = u.PlacePosting,
                         ProjectName = u.ProjectName,
                         ProjectNameText = u.ProjectNameText
                     }).AsQueryable();

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => w.Working.ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;

        }



        public int DeleteManpower(int ID, int UserID)
        {
            int res = 0;
            var t116ManpowerMasters = (from T116 in context.T116ManpowerMasters where T116.Id == ID select T116).FirstOrDefault();
            if (t116ManpowerMasters != null)
            {
                t116ManpowerMasters.Isdeleted = Convert.ToByte(true);
                t116ManpowerMasters.Updatedby = UserID;
                t116ManpowerMasters.Updateddate = DateTime.Now;
                context.SaveChanges();
                res = t116ManpowerMasters.Id;
            }
            return res;
        }

        public int DeleteManpowerDetail(int DetailID, int ManpowerID)
        {
            int res = 0;
            var t117ManpowerDetails = (from T117 in context.T117ManpowerDetails where T117.Id == DetailID && T117.Manpowerid == ManpowerID select T117).FirstOrDefault();
            if (t117ManpowerDetails != null)
            {
                context.T117ManpowerDetails.RemoveRange(t117ManpowerDetails);
                context.SaveChanges();
                res = t117ManpowerDetails.Id;
            }
            return res;
        }
    }
}
