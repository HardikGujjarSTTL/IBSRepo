using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using System.Composition;
using System;
using System.Data;
using System.Diagnostics.Contracts;

namespace IBS.Repositories
{
    public class CentralQOIIRepository : ICentralQOIIRepository
    {
        private readonly ModelContext context;

        public CentralQOIIRepository(ModelContext context)
        {
            this.context = context;
        }
        public CentralQOIIModel FindByID(string Client, string QtyDate, string Weight, string QoiLength)
        {
            CentralQOIIModel model = new();
            T79CentralQoinsp t79CentralQoinsp = (from t in context.T79CentralQoinsps
                                           where t.Client == Client && t.QoiDate == QtyDate && t.Weight == Weight && t.QoiLength == QoiLength
                                           select t).FirstOrDefault();

            if (t79CentralQoinsp == null)
                throw new Exception("Central QOII Record Not found");
            else
            {
                model.Clients = t79CentralQoinsp.Client;
                model.Weights = t79CentralQoinsp.Weight;
                model.QoiLengths = t79CentralQoinsp.QoiLength;
                model.Accepted = Convert.ToInt64(t79CentralQoinsp.Accepted);
                model.Rejected = Convert.ToInt64(t79CentralQoinsp.Rejected);
                model.QoiDate = t79CentralQoinsp.QoiDate;
                model.Region = t79CentralQoinsp.Region;
                model.UserId = t79CentralQoinsp.UserId;
                model.Createdby = t79CentralQoinsp.Createdby;
                model.Createddate = t79CentralQoinsp.Createddate;
                model.Updatedby = t79CentralQoinsp.Updatedby;
                model.Updateddate = t79CentralQoinsp.Updateddate;
                model.Isdeleted = t79CentralQoinsp.Isdeleted;
                model.Year = t79CentralQoinsp.QoiDate.Substring(0, 4);
                model.Month = t79CentralQoinsp.QoiDate.Substring(t79CentralQoinsp.QoiDate.Length - 2);
                return model;
            }
        }

        public DTResult<CentralQOIIModel> GetCentralQOIIList(DTParameters dtParameters)
        {
            DTResult<CentralQOIIModel> dTResult = new() { draw = 0 };
            IQueryable<CentralQOIIModel>? query = null;

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
            query = from l in context.T79CentralQoinsps
                    where l.Isdeleted == 0 || l.Isdeleted == null
                    select new CentralQOIIModel
                    {
                        Clients = l.Client,
                        Weights = l.Weight,
                        QoiLengths = l.QoiLength,
                        Accepted = Convert.ToInt64(l.Accepted),
                        Rejected = Convert.ToInt64(l.Rejected),
                        QoiDate = l.QoiDate,
                        UserId = l.UserId,
                        Region = l.Region,
                        Datetime = l.Datetime,
                        Year = l.QoiDate.Substring(0, 4),
                        Month = l.QoiDate.Substring(5, 6),
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.QoiDate).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.Region).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();
            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();
            dTResult.draw = dtParameters.Draw;
            return dTResult;
        }

        public bool Remove(string Client, string QtyDate, int UserID, string Weight, string QoiLength)
        {
            T79CentralQoinsp objDelete = (from t in context.T79CentralQoinsps
                                       where t.Client == Client && t.QoiDate == QtyDate && t.Weight == Weight && t.QoiLength == QoiLength
                                       select t).FirstOrDefault();
            if (objDelete == null) { return false; }
            objDelete.Isdeleted = Convert.ToByte(true);
            objDelete.Updatedby = Convert.ToInt32(UserID);
            objDelete.Updateddate = DateTime.Now;
            context.SaveChanges();
            return true;
        }

        public string InsertUpdateCentralQOII(CentralQOIIModel model)
        {
            string Client = "";

            T79CentralQoinsp objExist = (from t in context.T79CentralQoinsps
                                      where t.Client == model.Clients && t.QoiDate == model.QoiDate && t.Weight == model.Weights && t.QoiLength == model.QoiLengths
                                      select t).FirstOrDefault();
            if (model.IsEdited == false && objExist != null)
            {
                return "Already Exist";
            }
            T79CentralQoinsp objAdd = (from t in context.T79CentralQoinsps
                                    where t.Client == model.Clients && t.QoiDate == model.QoiDate && t.Weight == model.Weights && t.QoiLength == model.QoiLengths
                                    select t).FirstOrDefault();
            #region Contract save
            if (objAdd == null)
            {
                T79CentralQoinsp obj = new T79CentralQoinsp();
                obj.Client = model.Clients;
                obj.Weight = model.Weights;
                obj.QoiLength = model.QoiLengths;
                obj.Accepted = model.Accepted;
                obj.Rejected = model.Rejected;
                obj.QoiDate = model.QoiDate;
                obj.Region = model.Region;
                obj.UserId = model.UserId;
                obj.Datetime = DateTime.Now;
                obj.Isdeleted = Convert.ToByte(false);
                obj.Createdby = Convert.ToInt32(model.UserId);
                obj.Createddate = DateTime.Now;
                context.T79CentralQoinsps.Add(obj);
                context.SaveChanges();
                Client = obj.Client;
            }
            else
            {
                objAdd.UserId = model.UserId;
                objAdd.Accepted = model.Accepted;
                objAdd.Rejected = model.Rejected;
                objAdd.Region = model.Region;
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
