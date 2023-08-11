using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using System.Composition;
using System;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using IBS.Helper;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;

namespace IBS.Repositories
{
    public class ExpenditureRepository : IExpenditureRepository
    {
        private readonly ModelContext context;

        public ExpenditureRepository(ModelContext context)
        {
            this.context = context;
        }
        public ExpenditureModel FindByID(string ExpPer, string rgnCode)
        {
            ExpenditureModel model = new();
            //T63Exp tenant = context.T63Exps.Find(ExpPer);

            var tenant = (from m in context.T63Exps
                             where m.ExpPer == ExpPer
                             && m.RegionCode == rgnCode
                             select m).FirstOrDefault();

            if (tenant == null)
                throw new Exception("Record Not found");
            else
            {
                model.ExpPer = tenant.ExpPer;
                model.ExpAmt = tenant.ExpAmt;
                model.TaxAmt = tenant.TaxAmt;
                model.Datetime = tenant.Datetime;
                model.UserId = tenant.UserId;
                //model.Isdeleted = tenant.Isdeleted;
                //model.Createddate = tenant.Createddate;
                //model.Createdby = tenant.Createdby;
                //model.Updateddate = tenant.Updateddate;
                //model.Updatedby = tenant.Updatedby;
                return model;
            } 
        } 

        public DTResult<ExpenditureModel> GetExpenditureList(DTParameters dtParameters, string RgnCd)
        { 
            DTResult<ExpenditureModel> dTResult = new() { draw = 0 };
            IQueryable<ExpenditureModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "ExpPer";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "ExpPer";
                orderAscendingDirection = true;
            }
            query = from l in context.T63Exps
                    where l.RegionCode == RgnCd
                    select new ExpenditureModel
                    {  
                        ExpPer = l.ExpPer,
                        ExpAmt = l.ExpAmt,
                        TaxAmt = l.TaxAmt,
                        RegionCode = l.RegionCode,
                        UserId = l.UserId,
                        Datetime = l.Datetime,
                        //Isdeleted = l.Isdeleted,
                        //Createdby = l.Createdby,
                        //Createddate = l.Createddate,
                        //Updatedby= l.Updatedby,
                        //Updateddate= l.Updateddate,
                    };

            dTResult.recordsTotal = query.Count();
            dTResult.recordsFiltered = query.Count();
            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();
            dTResult.draw = dtParameters.Draw;
            return dTResult;
        }

        public bool Remove(string ExpPer, string rgnCode)
        {
            var _contract = (from m in context.T63Exps
                             where m.ExpPer == ExpPer
                             && m.RegionCode == rgnCode
                             select m).FirstOrDefault();

            if (_contract == null) { return false; }
            //_contract. = Convert.ToByte(true);
            //_contracts.Updatedby = Convert.ToInt32(UserID);
            //_contracts.Updateddate = DateTime.Now;
            context.SaveChanges();
            return true;
        }

        public string ExpenditureDetailsInsertUpdate(ExpenditureModel model)
        {
            var Id = "";
            model.ExpPer = model.ExpPerYear + model.ExpPerMonth;              
            var _contract = (from m in context.T63Exps
                             where m.ExpPer == model.ExpPer
                             && m.RegionCode == model.RegionCode
                             select m).FirstOrDefault();

            #region ExpPer save
            if (_contract == null)
            {
                T63Exp obj = new T63Exp();
                obj.ExpPer = model.ExpPer;
                obj.ExpAmt = model.ExpAmt;
                obj.TaxAmt = model.TaxAmt;
                obj.Datetime = model.Datetime;
                obj.UserId = model.UserId;
                obj.RegionCode = model.RegionCode;
                //obj.Isdeleted = Convert.ToByte(false);
                //obj.Createdby = Convert.ToInt32(model.User_Id);
                //obj.Createddate = DateTime.Now;
                //obj.Updatedby = Convert.ToInt32(model.UserId);
                //obj.Updateddate = DateTime.Now;
                context.T63Exps.Add(obj);
                context.SaveChanges();
                Id = (obj.ExpPer);
            }
            else
            {
                _contract.TaxAmt = model.TaxAmt;
                _contract.ExpAmt = model.ExpAmt;
                //_contract.Updatedby = model.UserId;
                //_contract.Updateddate = DateTime.Now;
                Id = _contract.ExpPer;
                context.SaveChanges();
            }
            #endregion
            return Id;
        }
    }

}
