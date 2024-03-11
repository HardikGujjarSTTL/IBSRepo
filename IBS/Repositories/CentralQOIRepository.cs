using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using System.Data;

namespace IBS.Repositories
{
    public class CentralQOIRepository : ICentralQOIRepository
    {
        private readonly ModelContext context;

        public CentralQOIRepository(ModelContext context)
        {
            this.context = context;
        }
        public CentralQOIModel FindByID(string Client, string QtyDate)
        {
            CentralQOIModel model = new();
            T78CentralQoi t78CentralQoi = (from t in context.T78CentralQois
                                           where t.Client == Client && t.QtyDate == QtyDate
                                           select t).FirstOrDefault();

            if (t78CentralQoi == null)
                throw new Exception("Central QOI Record Not found");
            else
            {
                model.TotalQtyDispatched = t78CentralQoi.TotalQtyDispatched;
                model.NoOfIcIssued = t78CentralQoi.NoOfIcIssued;
                model.QtyDate = t78CentralQoi.QtyDate;
                model.RegionCode = t78CentralQoi.RegionCode;
                model.UserId = t78CentralQoi.UserId;
                model.Datetime = t78CentralQoi.Datetime;
                model.Createdby = t78CentralQoi.Createdby;
                model.Createddate = t78CentralQoi.Createddate;
                model.Updatedby = t78CentralQoi.Updatedby;
                model.Updateddate = t78CentralQoi.Updateddate;
                model.Isdeleted = t78CentralQoi.Isdeleted;
                model.Year = t78CentralQoi.QtyDate.Substring(0, 4);
                model.Month = t78CentralQoi.QtyDate.Substring(t78CentralQoi.QtyDate.Length - 2);
                return model;
            }
        }

        public DTResult<CentralQOIModel> GetCentralQOIList(DTParameters dtParameters)
        {
            DTResult<CentralQOIModel> dTResult = new() { draw = 0 };
            IQueryable<CentralQOIModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "RegionCode";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                //if we have an empty search then just order the results by Id ascending
                orderCriteria = "RegionCode";
                orderAscendingDirection = true;
            }
            query = from l in context.T78CentralQois
                    where l.Isdeleted == 0 || l.Isdeleted == null
                    select new CentralQOIModel
                    {
                        TotalQtyDispatched = l.TotalQtyDispatched,
                        NoOfIcIssued = l.NoOfIcIssued,
                        Clients = l.Client,
                        QtyDate = l.QtyDate,
                        RegionCode = l.RegionCode,
                        UserId = l.UserId,
                        Datetime = l.Datetime,
                        Year = l.QtyDate.Substring(0, 4),
                        Month = l.QtyDate.Substring(5, 6),
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.QtyDate).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.RegionCode).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();
            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();
            dTResult.draw = dtParameters.Draw;
            return dTResult;
        }

        public bool Remove(string Client, string QtyDate, int UserID)
        {
            T78CentralQoi objDelete = (from t in context.T78CentralQois
                                       where t.Client == Client && t.QtyDate == QtyDate
                                       select t).FirstOrDefault();
            if (objDelete == null) { return false; }
            objDelete.Isdeleted = Convert.ToByte(true);
            objDelete.Updatedby = Convert.ToInt32(UserID);
            objDelete.Updateddate = DateTime.Now;
            context.SaveChanges();
            return true;
        }

        public string InsertUpdateCentralQOI(CentralQOIModel model)
        {
            string Client = "";

            T78CentralQoi objExist = (from t in context.T78CentralQois
                                      where t.Client == model.Clients && t.QtyDate == model.QtyDate && t.RegionCode == model.RegionCode
                                      select t).FirstOrDefault();
            if (model.IsEdited == false && objExist != null)
            {
                return "Already Exist";
            }
            T78CentralQoi objAdd = (from t in context.T78CentralQois
                                    where t.Client == model.Clients && t.QtyDate == model.QtyDate
                                    select t).FirstOrDefault();
            #region Contract save   
            if (objAdd == null)
            {
                T78CentralQoi obj = new T78CentralQoi();
                obj.TotalQtyDispatched = model.TotalQtyDispatched;
                obj.NoOfIcIssued = model.NoOfIcIssued;
                obj.Client = model.Clients;
                obj.QtyDate = model.QtyDate;
                obj.RegionCode = model.RegionCode;
                obj.UserId = model.UserId;
                obj.Datetime = DateTime.Now;
                obj.Isdeleted = Convert.ToByte(false);
                obj.Createdby = Convert.ToInt32(model.UserId);
                obj.Createddate = DateTime.Now;
                context.T78CentralQois.Add(obj);
                context.SaveChanges();
                Client = obj.Client;
            }
            else
            {
                objAdd.TotalQtyDispatched = model.TotalQtyDispatched;
                objAdd.NoOfIcIssued = model.NoOfIcIssued;
                objAdd.Client = model.Clients;
                objAdd.RegionCode = model.RegionCode;
                objAdd.UserId = model.UserId;
                objAdd.Datetime = DateTime.Now;
                objAdd.Updatedby = Convert.ToInt32(model.UserId);
                objAdd.Updateddate = DateTime.Now;
                context.SaveChanges();
                Client = objAdd.Client;
            }
            #endregion
            return Client;
        }
    }

}
