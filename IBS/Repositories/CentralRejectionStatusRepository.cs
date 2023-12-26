using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using System.Data;

namespace IBS.Repositories
{
    public class CentralRejectionStatusRepository : ICentralRejectionStatusRepository
    {
        private readonly ModelContext context;

        public CentralRejectionStatusRepository(ModelContext context)
        {
            this.context = context;
        }
        public CentralRejectionStatusModel FindByID(int ID)
        {
            CentralRejectionStatusModel model = new();
            T81CrRej t81CrRej = (from t in context.T81CrRejs
                                 where t.Id == ID
                                 select t).FirstOrDefault();

            if (t81CrRej == null)
                throw new Exception("Central Rejection Status Record Not found");
            else
            {
                model.Id = t81CrRej.Id;
                model.RejDt = t81CrRej.RejDt;
                model.CaseNo = t81CrRej.CaseNo;
                model.Consignee = t81CrRej.Consignee;
                model.DesCom = t81CrRej.DesCom;
                model.Conclusion = t81CrRej.Conclusion;
                model.Region = t81CrRej.Region;
                model.UserId = t81CrRej.UserId;
                model.Datetime = DateTime.Now;
                model.Year = t81CrRej.RejDt.Substring(0, 4);
                model.Month = t81CrRej.RejDt.Substring(t81CrRej.RejDt.Length - 2);
                return model;
            }
        }

        public DTResult<CentralRejectionStatusModel> GetCentralRejectionStatusList(DTParameters dtParameters)
        {
            DTResult<CentralRejectionStatusModel> dTResult = new() { draw = 0 };
            IQueryable<CentralRejectionStatusModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "Region";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                //if we have an empty search then just order the results by Id ascending
                orderCriteria = "Region";
                orderAscendingDirection = true;
            }
            query = from l in context.T81CrRejs
                    where l.Isdeleted == 0 || l.Isdeleted == null
                    select new CentralRejectionStatusModel
                    {
                        Id = l.Id,
                        RejDt = l.RejDt,
                        CaseNo = l.CaseNo,
                        Consignee = l.Consignee,
                        DesCom = l.DesCom,
                        Conclusion = l.Conclusion,
                        Region = l.Region,
                        UserId = l.UserId,
                        Datetime = l.Datetime,
                        Year = l.RejDt.Substring(0, 4),
                        Month = l.RejDt.Substring(5, 6),
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.CaseNo).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.Consignee).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();
            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();
            dTResult.draw = dtParameters.Draw;
            return dTResult;
        }

        public bool Remove(int ID, int UserID)
        {
            T81CrRej objDelete = (from t in context.T81CrRejs
                                  where t.Id == ID
                                  select t).FirstOrDefault();
            if (objDelete == null) { return false; }
            objDelete.Isdeleted = Convert.ToByte(true);
            objDelete.Updatedby = Convert.ToInt32(UserID);
            objDelete.Updateddate = DateTime.Now;
            context.SaveChanges();
            return true;
        }

        public int InsertUpdateCentralRejectionStatus(CentralRejectionStatusModel model)
        {
            int id = 0;
            T81CrRej objAdd = (from t in context.T81CrRejs
                               where t.Id == model.Id
                               select t).FirstOrDefault();
            #region Contract save
            if (objAdd == null)
            {
                T81CrRej obj = new T81CrRej();
                obj.RejDt = model.RejDt;
                obj.CaseNo = model.CaseNo;
                obj.Consignee = model.Consignee;
                obj.DesCom = model.DesCom;
                obj.Conclusion = model.Conclusion;
                obj.Region = model.Region;
                obj.UserId = model.UserId;
                obj.Datetime = DateTime.Now;
                obj.Isdeleted = Convert.ToByte(false);
                obj.Createdby = Convert.ToInt32(model.UserId);
                obj.Createddate = DateTime.Now;
                context.T81CrRejs.Add(obj);
                context.SaveChanges();
                id = obj.Id;
            }
            else
            {
                objAdd.RejDt = model.RejDt;
                objAdd.CaseNo = model.CaseNo;
                objAdd.Consignee = model.Consignee;
                objAdd.DesCom = model.DesCom;
                objAdd.Conclusion = model.Conclusion;
                objAdd.Region = model.Region;
                objAdd.UserId = model.UserId;
                objAdd.Updatedby = Convert.ToInt32(model.UserId);
                objAdd.Updateddate = DateTime.Now;
                context.SaveChanges();
                id = objAdd.Id;
            }
            #endregion
            return id;
        }
    }

}
