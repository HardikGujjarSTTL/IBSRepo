using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;

namespace IBS.Repositories
{
    public class ProjectDetailsRepository : IProjectDetailsRepository
    {
        private readonly ModelContext context;

        public ProjectDetailsRepository(ModelContext context)
        {
            this.context = context;
        }

        public DTResult<ProjectDetails> GetProductDetailsList(DTParameters dtParameters, List<ProjectDetails> ProductDetailsModels)
        {
            DTResult<ProjectDetails> dTResult = new() { draw = 0 };
            IQueryable<ProjectDetails>? query = null;
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

            query = (from u in ProductDetailsModels.OrderBy(x => x.SancStrength)
                     select new ProjectDetails
                     {
                         In_ID = u.In_ID,
                         Numbers = Convert.ToInt32(u.Numbers),
                         Disc = EnumUtility<Enums.DiscDepartment>.GetDescriptionByKey(u.Disc),
                         SancStrength = EnumUtility<Enums.SanctionedStrength>.GetDescriptionByKey(u.SancStrength),
                         //Disc = u.Disc == "M" ? "Mechanical" : u.Disc == "E" ? "Electrical" : u.Disc == "C" ? "Civil" : u.Disc == "A" ? "M & C" : u.Disc == "O" ? "Others" : "",
                     }).AsQueryable();

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => w.Disc.ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;

        }

        public int SaveProductDetailsList(ProjectDetails model, List<ProjectDetails> ProductDetailsModels)
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
                ProjID = Convert.ToInt32(ProjMaster.Id);
            }
            #endregion

            #region Project Details

            foreach (var item in ProductDetailsModels)
            {
                int ProjDID = context.ProjectDetails.Any() ? context.ProjectDetails.Max(x => x.Id) + 1 : 1;
                ProjectDetail obj = new ProjectDetail();
                obj.Id = ProjDID;
                obj.ProdId = ProjectID;
                obj.Sanctionedstrength = EnumUtility<Enums.DiscDepartment>.GetDescriptionByKey(item.SancStrength);
                obj.Department = EnumUtility<Enums.DiscDepartment>.GetDescriptionByKey(item.Disc);
                obj.Nos = item.Numbers;
                context.ProjectDetails.Add(obj);
                context.SaveChanges();
            }
            #endregion

            return ProjID;
        }
    }
}
