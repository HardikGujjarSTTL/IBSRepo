using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Interfaces.Reports.ManPower;
using IBS.Models;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace IBS.Repositories.Reports.ManPower
{
    public class ManpowerMasterDataReportRepository : IManpowerMasterDataReportRepository
    {
        private readonly ModelContext context;

        public ManpowerMasterDataReportRepository(ModelContext context)
        {
            this.context = context;
        }

        public DTResult<ManpowerModel> GetManpowerMasterReportData(DTParameters dtParameters)
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

            //string Region = !string.IsNullOrEmpty(dtParameters.AdditionalValues["Region"]) ? Convert.ToString(dtParameters.AdditionalValues["Region"]) : null;

            query = from a in context.T116ManpowerMasters
                    join md in context.T117ManpowerDetails on a.Id equals md.Manpowerid into mdJoin
                    from md in mdJoin.DefaultIfEmpty()
                    join rd in context.T07RitesDesigs on Convert.ToInt32(a.Desig) equals rd.RDesigCd into rdJoin
                    from rd in rdJoin.DefaultIfEmpty()
                    join r in context.T01Regions on a.Cadre equals r.RegionCode into rJoin
                    from r in rJoin.DefaultIfEmpty()
                    join s in context.T115ManpowerStatuses on Convert.ToInt32(a.Status) equals s.Id into sJoin
                    from s in sJoin.DefaultIfEmpty()
                    select new ManpowerModel
                    {
                        ID = a.Id,
                        Region = a.Region == "N" ? "North" : a.Region == "S" ? "South" : a.Region == "W" ? "West" : a.Region == "E" ? "East" : a.Region == "C" ? "Central" : "",
                        EmpName = a.EmpName,
                        EmpNo = a.EmpNo,
                        Desig = rd.RDesignation,
                        Cadre = r.Region,
                        Discp = a.Discp == "M" ? "Mechanical" : a.Discp == "E" ? "Electrical" : a.Discp == "C" ? "Civil" : a.Discp == "L" ? "Metallurgy" : a.Discp == "T" ? "Textiles" : a.Discp == "P" ? "Power Engineering" : "",
                        Status = s.Status,
                        Dob = a.Dob,
                        RitesDt = a.RitesDt,
                        RioDt = a.RioDt,
                        DrrtDt = a.DrrtDt,
                        Working = md.Working == "S" ? "SBU" : md.Working == "H" ? "Head" : md.Working == "C" ? "C.M" : md.Working == "I" ? "I.E" : md.Working == "D" ? "DFO" : md.Working == "O" ? "Other" : "",
                        Staff = md.Staff == "T" ? "Technical" : md.Staff == "N" ? "Non Technical" : "",
                        PlacePosting = md.PlacePosting,
                        ProjectName = (from f in context.ProjectMasters where f.Id == md.ProjectName select f.Projectname).FirstOrDefault(),
                        Nameofcluster = md.Nameofcluster,
                        Fromdate = md.Fromdate,
                        Todate = md.Todate,
                    };
            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.EmpName).ToLower().Contains(searchBy.ToLower())
                || w.EmpNo.ToLower().Contains(searchBy.ToLower())
                || w.Desig.ToLower().Contains(searchBy.ToLower())
                || w.Working.ToLower().Contains(searchBy.ToLower())
                || w.ProjectName.ToLower().Contains(searchBy.ToLower())
                );

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
                OracleParameter[] par = new OracleParameter[2];
                par[0] = new OracleParameter("p_PROJ_ID", OracleDbType.Int32, model.Proj_ID, ParameterDirection.Input);
                par[1] = new OracleParameter("p_result_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

                var ds = DataAccessDB.GetDataSet("SP_GET_ProjectDetailsReport", par, 2);
                List<ProjectDetailsReport> list = new();
                if (ds != null && ds.Tables.Count > 0)
                {
                    string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                    list = JsonConvert.DeserializeObject<List<ProjectDetailsReport>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                }
                model.lstprojectDetailsReports = list;

                var query = (from a in context.T116ManpowerMasters
                             join d in context.T117ManpowerDetails on a.Id equals d.Manpowerid
                             join rd in context.T07RitesDesigs on Convert.ToInt32(a.Desig) equals rd.RDesigCd into rdJoin
                             from rd in rdJoin.DefaultIfEmpty()
                             where d.ProjectName == model.Proj_ID
                             select new ManpowerModel
                             {
                                 ID = a.Id,
                                 Region = a.Region == "N" ? "North" : a.Region == "S" ? "South" : a.Region == "W" ? "West" : a.Region == "E" ? "East" : a.Region == "C" ? "Central" : "",
                                 EmpName = a.EmpName,
                                 EmpNo = a.EmpNo,
                                 Discp = a.Discp == "M" ? "Mechanical" : a.Discp == "E" ? "Electrical" : a.Discp == "C" ? "Civil" : a.Discp == "L" ? "Metallurgy" : a.Discp == "T" ? "Textiles" : a.Discp == "P" ? "Power Engineering" : "",
                                 Status = a.Status,
                                 Desig = rd.RDesignation,
                             }).ToList();
                model.lstManpowerModels = query;
            }
            return model;
        }

        public List<IEAndCallReport> GetIEAndCallReport(string P_FROMDATE, string P_TODATE)
        {
            OracleParameter[] par = new OracleParameter[3];
            par[0] = new OracleParameter("P_FROMDATE", OracleDbType.Date, Convert.ToDateTime(P_FROMDATE), ParameterDirection.Input);
            par[1] = new OracleParameter("P_TODATE", OracleDbType.Date, Convert.ToDateTime(P_TODATE), ParameterDirection.Input);
            par[2] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);
            var ds = DataAccessDB.GetDataSet("SP__Get_IEAndCallReport", par, 1);
            List<IEAndCallReport> model = new();
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                model = JsonConvert.DeserializeObject<List<IEAndCallReport>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }
            return model;
        }
    }
}
