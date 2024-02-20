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
    }
}
