using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.CodeAnalysis;

namespace IBS.Repositories
{
    public class ProjectDetailsRepository : IProjectDetailsRepository
    {
        private readonly ModelContext context;

        public ProjectDetailsRepository(ModelContext context)
        {
            this.context = context;
        }

        public DTResult<ProjectModel> GetMasterList(DTParameters dtParameters)
        {
            DTResult<ProjectModel> dTResult = new() { draw = 0 };
            IQueryable<ProjectModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null && dtParameters.Order.Length > 0)
            {
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                if (string.IsNullOrEmpty(orderCriteria)) orderCriteria = "ProjectName";
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                orderCriteria = "ProjectName";
                orderAscendingDirection = true;
            }

            query = from a in context.ProjectMasters
                    where (a.Isdeleted == 0 || a.Isdeleted == null)
                    select new ProjectModel
                    {
                        Proj_ID = a.Id,
                        ProjectName = a.Projectname,
                        StartDate = a.Startdate,
                        CompletionDate = a.Completiondate
                    };
            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.ProjectName).ToLower().Contains(searchBy.ToLower()));

            dTResult.recordsFiltered = query.Count();
            if (dtParameters.Length == -1) dtParameters.Length = query.Count();
            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();
            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public ProjectModel FindByID(int id)
        {
            ProjectModel model = new ProjectModel();
            model = (from a in context.ProjectMasters
                     where a.Id == id && (a.Isdeleted == 0 || a.Isdeleted == null)
                     select new ProjectModel
                     {
                         Proj_ID = a.Id,
                         ProjectName = a.Projectname,
                         StartDate = a.Startdate,
                         CompletionDate = a.Completiondate
                     }).FirstOrDefault();
            if (model != null)
            {
                List<ProjectDetailsModel> clst = (from T117 in context.ProjectDetails
                                                  where T117.ProjId == model.Proj_ID
                                                  select new
                                                  ProjectDetailsModel
                                                  {
                                                      DetailID = Convert.ToInt32(T117.Id),
                                                      ProjId = Convert.ToInt32(T117.ProjId),
                                                      Sanctionedstrength=T117.Sanctionedstrength,
                                                      SanctionedstrengthText = T117.Sanctionedstrength == "R" ? "Regular" : T117.Sanctionedstrength == "C" ? "Contract" : T117.Sanctionedstrength == "T" ? "Through MPA" : "",
                                                      Department = T117.Department,
                                                      DepartmentText = T117.Department == "M" ? "Mechanical" : T117.Department == "E" ? "Electrical" : T117.Department == "C" ? "Civil" : T117.Department == "A" ? "M & C" : T117.Department == "O" ? "Others" : "",
                                                      Nos = T117.Nos,
                                                  }).ToList();
                model.lstProjectDetails = clst;
            }
            return model;
        }

        public DTResult<ProjectDetailsModel> GetProjectDetailsList(DTParameters dtParameters, List<ProjectDetailsModel> ProductDetailsModels)
        {
            DTResult<ProjectDetailsModel> dTResult = new() { draw = 0 };
            IQueryable<ProjectDetailsModel>? query = null;
            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "SancStrength";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "SancStrength";
                orderAscendingDirection = true;
            }

            query = (from u in ProductDetailsModels.OrderBy(x => x.Sanctionedstrength)
                     select new ProjectDetailsModel
                     {
                         DetailID = u.DetailID,
                         Nos = Convert.ToInt32(u.Nos),
                         Department = u.Department,
                         DepartmentText = u.DepartmentText,
                         Sanctionedstrength = u.Sanctionedstrength,
                         SanctionedstrengthText = u.SanctionedstrengthText,
                         //Disc = u.Disc == "M" ? "Mechanical" : u.Disc == "E" ? "Electrical" : u.Disc == "C" ? "Civil" : u.Disc == "A" ? "M & C" : u.Disc == "O" ? "Others" : "",
                     }).AsQueryable();

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => w.Department.ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;

        }

        public int SaveProject(ProjectModel model)
        {
            int ProjID = 0;
            int ProjectID = 0;
            var ProjMaster = (from r in context.ProjectMasters where r.Id == Convert.ToInt32(model.Proj_ID) select r).FirstOrDefault();

            #region Project Master
            if (ProjMaster == null)
            {
                ProjectID = context.ProjectMasters.Any() ? context.ProjectMasters.Max(x => x.Id) + 1 : 1;
                ProjectMaster obj = new ProjectMaster();
                obj.Id = ProjectID;
                obj.Projectname = model.ProjectName;
                obj.Startdate = model.StartDate;
                obj.Completiondate = model.CompletionDate;
                obj.SanctionedFile = model.SanctionedFile;
                obj.Isdeleted = Convert.ToByte(false);
                obj.Createdby = model.Createdby;
                obj.Createddate = DateTime.Now;
                context.ProjectMasters.Add(obj);
                context.SaveChanges();
                ProjectID = Convert.ToInt32(obj.Id);
            }
            else
            {
                ProjMaster.Projectname = model.ProjectName;
                ProjMaster.Startdate = model.StartDate;
                ProjMaster.Completiondate = model.CompletionDate;
                ProjMaster.SanctionedFile = model.SanctionedFile;
                ProjMaster.Isdeleted = Convert.ToByte(false);
                ProjMaster.Updatedby = model.UpdatedBy;
                ProjMaster.Updateddate = DateTime.Now;
                context.SaveChanges();
                ProjectID = Convert.ToInt32(ProjMaster.Id);
            }
            #endregion

            #region Project Details
            var projectDetails = (from T117 in context.ProjectDetails where T117.ProjId == model.Proj_ID select T117).ToList();
            if (projectDetails.Count > 0 && projectDetails != null)
            {
                context.ProjectDetails.RemoveRange(projectDetails);
                context.SaveChanges();
            }
            if (model.lstProjectDetails != null)
            {
                foreach (var item in model.lstProjectDetails)
                {
                    ProjectDetail objAdd = new ProjectDetail();
                    {
                        objAdd.ProjId = ProjectID;
                        objAdd.Sanctionedstrength = item.Sanctionedstrength;
                        objAdd.Department = item.Department;
                        objAdd.Nos = item.Nos;
                        objAdd.Isdeleted = Convert.ToByte(false);
                        objAdd.Createdby = model.Createdby;
                        objAdd.Createddate = DateTime.Now;
                    }
                    context.ProjectDetails.Add(objAdd);
                    context.SaveChanges();
                }
            }
            #endregion
            return ProjectID;
        }

        public int DeleteProject(int ID, int UserID)
        {
            int res = 0;
            var projectMasters = (from T116 in context.ProjectMasters where T116.Id == ID select T116).FirstOrDefault();
            if (projectMasters != null)
            {
                projectMasters.Isdeleted = Convert.ToByte(true);
                projectMasters.Updatedby = UserID;
                projectMasters.Updateddate = DateTime.Now;
                context.SaveChanges();
                res = projectMasters.Id;
            }
            return res;
        }

        public int SaveDetails(ProjectDetailsModel model)
        {
            int res = 0;
            if (model.DetailID == 0)
            {
                ProjectDetail objAdd = new ProjectDetail();
                objAdd.ProjId = model.ProjId;
                objAdd.Sanctionedstrength = model.Sanctionedstrength;
                objAdd.Department = model.Department;
                objAdd.Nos = model.Nos;
                context.ProjectDetails.Add(objAdd);
                context.SaveChanges();
                res = objAdd.Id;
            }
            else
            {
                var data = context.ProjectDetails.Find(model.DetailID);
                if (data != null)
                {
                    data.Sanctionedstrength = model.Sanctionedstrength;
                    data.Department = model.Department; ;
                    data.Nos = model.Nos;
                    context.SaveChanges();
                    res = data.Id;
                }
            }
            return res;
        }

        public int DeleteProjectDetails(int DetailID, int ProjectID)
        {
            int res = 0;
            var projectDetails = (from T117 in context.ProjectDetails where T117.Id == DetailID && T117.ProjId == ProjectID select T117).FirstOrDefault();
            if (projectDetails != null)
            {
                context.ProjectDetails.RemoveRange(projectDetails);
                context.SaveChanges();
                res = projectDetails.Id;
            }
            return res;
        }
    }
}
