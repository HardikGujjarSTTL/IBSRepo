using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using System.Composition;
using System;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;

namespace IBS.Repositories
{
    public class LabBillingRepository : ILabBillingRepository
    {
        private readonly ModelContext context;

        public LabBillingRepository(ModelContext context)
        {
            this.context = context;
        }
        public LabBillingModel FindByID(int Id)
        {
            LabBillingModel model = new();
            T59LabExp tenant = context.T59LabExps.Find(Convert.ToInt32(Id));

            if (tenant == null)
                throw new Exception("Bill Record Not found");
            else
            {
                model.Id = tenant.Id;
                model.LabBillPer = tenant.LabBillPer;
                model.LabExp = tenant.LabExp;
                //model.RegionCode = Region;                                 
                model.Datetime = tenant.Datetime;                 
                model.UserId= tenant.UserId;              
                //model.Isdeleted = tenant.Isdeleted;
                //model.Createddate = tenant.Createddate;
                //model.Createdby = tenant.Createdby;
                //model.Updateddate = tenant.Updateddate;
                //model.Updatedby = tenant.Updatedby;
                return model;
            }
        }

        public DTResult<LabBillingModel> GetLabBillingList(DTParameters dtParameters)
        { 
            DTResult<LabBillingModel> dTResult = new() { draw = 0 };
            IQueryable<LabBillingModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "LabBillPer";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "LabBillPer";
                orderAscendingDirection = true;
            }
            query = from l in context.T59LabExps
                        //where l.Isdeleted == 0 
                    select new LabBillingModel
                    {
                        Id = l.Id,                         
                        LabExp = l.LabExp,
                        LabBillPer = l.LabBillPer,
                        UserId = l.UserId,
                        Datetime = l.Datetime,                                                                     
                        //Isdeleted = l.Isdeleted,
                        //Createdby = l.Createdby,
                        //Createddate = l.Createddate,
                        //Updatedby= l.Updatedby,
                        //Updateddate= l.Updateddate,
            };

            dTResult.recordsTotal = query.Count();

            //if (!string.IsNullOrEmpty(searchBy))
            //    query = query.Where(w => Convert.ToString(w.Contractname).ToLower().Contains(searchBy.ToLower())
            //    || Convert.ToString(w.Contractdescription).ToLower().Contains(searchBy.ToLower())
            //    );

            dTResult.recordsFiltered = query.Count();
            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();
            dTResult.draw = dtParameters.Draw;
            return dTResult;
        }

        public bool Remove(int Id, int UserID)
        {
            var _contracts = context.T59LabExps.Find(Convert.ToInt32(Id));
            if (_contracts == null) { return false; }

            _contracts.Isdeleted = Convert.ToByte(true);
            _contracts.Updatedby = Convert.ToInt32(UserID);
            _contracts.Updateddate = DateTime.Now;
            context.SaveChanges();
            return true;
        }

        public int LabBillingDetailsInsertUpdate(LabBillingModel model)
        {
            int Id = 0;
             
            var _contract = context.T59LabExps.Find(model.Id);
            #region Contract save
            if (_contract == null)
            {
                model.LabBillPer = model.LabBillPerYear + model.LabBillPerMon;
                T59LabExp obj = new T59LabExp();
                //obj.Id = model.Id;
                obj.LabBillPer = model.LabBillPer;
                obj.LabExp = model.LabExp;
                obj.Datetime = model.Datetime;
                obj.UserId = model.UserId;
                obj.RegionCode = model.RegionCode; 
                obj.Isdeleted = Convert.ToByte(false);
                obj.Createdby = Convert.ToInt32(model.UserId);
                obj.Createddate = DateTime.Now;
                obj.Updatedby = Convert.ToInt32(model.UserId);
                obj.Updateddate = DateTime.Now;
                context.T59LabExps.Add(obj);
                context.SaveChanges();
                Id = Convert.ToInt32(obj.Id);
            }
            else
            {
                model.LabBillPer = model.LabBillPerYear + model.LabBillPerMon;
                //_contract.Id = model.Id;
               _contract.LabBillPer = model.LabBillPer;
               _contract.LabExp = model.LabExp; 
               //_contract.RegionCode = model.RegionCode;               
                //_contract.Updatedby = model.Updatedby;
               _contract.Updateddate = DateTime.Now;
                Id = Convert.ToInt32(_contract.Id);
                context.SaveChanges();
            }
            #endregion
            return Id;
        }
    }

}
