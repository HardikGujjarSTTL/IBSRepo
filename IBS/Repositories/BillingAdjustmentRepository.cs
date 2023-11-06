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
using System.Diagnostics.Metrics;

namespace IBS.Repositories
{
    public class BillingAdjustmentRepository : IBillingAdjustmentRepository
    {
        private readonly ModelContext context;

        public BillingAdjustmentRepository(ModelContext context)
        {
            this.context = context;
        }

        public BillingAdjustmentModel FindByID(string AdjusmentYrMth, string rgnCode)
        {
            using (var dbContext = context.Database.GetDbConnection())
            {
                OracleParameter[] par = new OracleParameter[3];
                par[0] = new OracleParameter("p_AdjusmentYrMth", OracleDbType.Decimal, AdjusmentYrMth, ParameterDirection.Input);
                par[1] = new OracleParameter("p_REGIONCODE", OracleDbType.Char, rgnCode, ParameterDirection.Input);
                par[2] = new OracleParameter("p_Cursor", OracleDbType.RefCursor, ParameterDirection.Output);
                var ds = DataAccessDB.GetDataSet("SP_GETDATA_T85BILLINGADJUSTEMENT", par, 1);

                BillingAdjustmentModel model = new();
                if (ds != null && ds.Tables.Count > 0)
                {
                    string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                    model = JsonConvert.DeserializeObject<List<BillingAdjustmentModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }).FirstOrDefault();
                }
                return model;
            }
        }

        public DTResult<BillingAdjustmentModel> GetBillingAdjustmentList(DTParameters dtParameters, string RgnCd)
        {
            DTResult<BillingAdjustmentModel> dTResult = new() { draw = 0 };
            IQueryable<BillingAdjustmentModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "Adjusment_Yr_Mth";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                //if we have an empty search then just order the results by Id ascending
                orderCriteria = "Adjusment_Yr_Mth";
                orderAscendingDirection = true;
            }
            query = from l in context.T85BillingAdjustementPcdos
                    where l.RegionCode == RgnCd && l.Isdeleted != true
                    select new BillingAdjustmentModel
                    {
                        //Id = l.Id,
                        Adjusment_Yr_Mth = l.AdjusmentYrMth,
                        Adjustment_Amt = l.AdjustmentAmt,
                        Remarks = l.Remarks,
                        User_Id = l.UserId,
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

        public bool Remove(string BePer, string strRgn)
        {

            var _BeTarget = (from m in context.T85BillingAdjustementPcdos
                             where m.AdjusmentYrMth == BePer
                             && m.RegionCode == strRgn
                             select m).FirstOrDefault();

            if (_BeTarget == null) { return false; }

            _BeTarget.Isdeleted = true;
            _BeTarget.Updateddate = DateTime.Now;
            context.SaveChanges();
            return true;
        }

        public string BillingAdjustmentDetailsInsertUpdate(BillingAdjustmentModel model)
        {
            var Id = "";

            var _BillingAdjustment = (from m in context.T85BillingAdjustementPcdos
                                      where m.AdjusmentYrMth == model.Adjusment_Yr_Mth
                                      && m.RegionCode == model.Region_Code
                                      select m).FirstOrDefault();

            #region BillingAdjustment save
            if (_BillingAdjustment == null)
            {
                T85BillingAdjustementPcdo obj = new T85BillingAdjustementPcdo();
                obj.AdjusmentYrMth = model.AdjusmentPerYear + model.AdjusmentPerMon;
                obj.AdjustmentAmt = model.Adjustment_Amt;
                obj.Remarks = model.Remarks;
                obj.UserId = model.User_Id;
                obj.RegionCode = model.Region_Code;
                obj.Isdeleted = false;
                obj.Createddate = DateTime.Now;
                obj.Updateddate = DateTime.Now;
                context.T85BillingAdjustementPcdos.Add(obj);
                context.SaveChanges();
                Id = (obj.AdjusmentYrMth);
            }
            else
            {
                _BillingAdjustment.AdjustmentAmt = model.Adjustment_Amt;
                _BillingAdjustment.Remarks = model.Remarks;
                _BillingAdjustment.Updateddate = DateTime.Now;
                Id = _BillingAdjustment.AdjusmentYrMth;
                context.SaveChanges();
            }

            return Id;
            #endregion
        }
    }

}
