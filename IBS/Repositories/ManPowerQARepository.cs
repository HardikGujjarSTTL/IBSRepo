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
                if (string.IsNullOrEmpty(orderCriteria)) orderCriteria = "BankCd";
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                orderCriteria = "BankCd";
                orderAscendingDirection = true;
            }

            string BankName = !string.IsNullOrEmpty(dtParameters.AdditionalValues["BankName"]) ? Convert.ToString(dtParameters.AdditionalValues["BankName"]) : "";
            int? FMISBankCD = !string.IsNullOrEmpty(dtParameters.AdditionalValues["FMISBankCD"]) ? Convert.ToInt32(dtParameters.AdditionalValues["FMISBankCD"]) : null;

            query = from a in context.T116ManpowerMasters
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
                    };
            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.EmpName).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();
            if (dtParameters.Length == -1) dtParameters.Length = query.Count();
            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();
            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public ManpowerModel FindByID(int id)
        {
            ManpowerModel model = new ManpowerModel();
            var data = (from a in context.T116ManpowerMasters
                        where a.Id == id
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

        public int SaveDetails(ManpowerModel model)
        {
            int res = 0;
            var data = (from a in context.T116ManpowerMasters
                        where a.Id == model.ID
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

            if (data == null)
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
                context.Add(obj);
                context.SaveChanges();
                res = obj.Id;
            }
            else
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
                context.SaveChanges();
                res = data.ID;
            }
            return res;
        }
    }
}
