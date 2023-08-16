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
    public class BillingOperatingRepository : IBillingOperatingTargetRepository
    {
        private readonly ModelContext context;

        public BillingOperatingRepository(ModelContext context)
        {
            this.context = context;
        }

        public BillingOperatingTargetModel FindByID(string BePer, string rgnCode)
        {  
            using (var dbContext = context.Database.GetDbConnection())
            {
                OracleParameter[] par = new OracleParameter[3];
                par[0] = new OracleParameter("p_BePer", OracleDbType.Decimal, BePer, ParameterDirection.Input);
                par[1] = new OracleParameter("p_REGIONCODE", OracleDbType.Char, rgnCode, ParameterDirection.Input);
                par[2] = new OracleParameter("p_Cursor", OracleDbType.RefCursor, ParameterDirection.Output);
                var ds = DataAccessDB.GetDataSet("SP_GetData_T83BeTarget", par, 1);

                BillingOperatingTargetModel model = new();
                if (ds != null && ds.Tables.Count > 0)
                {
                    string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                    model = JsonConvert.DeserializeObject<List<BillingOperatingTargetModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }).FirstOrDefault();
                } 
                return model;
            } 
        }
 
        public DTResult<BillingOperatingTargetModel> GetBillingOperatingTargetList(DTParameters dtParameters, string RgnCd)
        { 
            DTResult<BillingOperatingTargetModel> dTResult = new() { draw = 0 };
            IQueryable<BillingOperatingTargetModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "Be_Per";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                //if we have an empty search then just order the results by Id ascending
                orderCriteria = "Be_Per";
                orderAscendingDirection = true;
            }
            query = from l in context.T83BeTargets
                    where l.RegionCode == RgnCd
                    select new BillingOperatingTargetModel
                    {
                        //Id = l.Id,
                        Be_Per = l.BePer,
                        B_Target = l.BTarget,
                        E_Target = l.ETarget,
                        Ex_Target = l.ExTarget,
                        User_Id = l.UserId,
                        Datetime = l.Datetime,                                                                     
                        Isdeleted = l.Isdeleted,
                        Createdby = l.Createdby,
                        Createddate = l.Createddate,
                        Updatedby= l.Updatedby,
                        Updateddate= l.Updateddate,
            };

            dTResult.recordsTotal = query.Count();
            dTResult.recordsFiltered = query.Count();
            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();
            dTResult.draw = dtParameters.Draw;
            return dTResult;
        }

        public bool Remove(string BePer, string strRgn)
        {
             
            var _BeTarget = (from m in context.T83BeTargets
                              where m.BePer == BePer
                             && m.RegionCode == strRgn
                             select m).FirstOrDefault();

            if (_BeTarget == null) { return false; }

            _BeTarget.Isdeleted = Convert.ToByte(true);
            //_BeTarget.Updatedby = UserID;
            _BeTarget.Updateddate = DateTime.Now;
            context.SaveChanges();
            return true;
        }

        public string BillingOperatingDetailsInsertUpdate(BillingOperatingTargetModel model)
        {
            var Id = "";
              
            var _BeTarget = (from m in context.T83BeTargets
                             where m.BePer == model.Be_Per
                             && m.RegionCode == model.Region_Code
                             select m).FirstOrDefault();

            #region Contract save
            if (_BeTarget == null)
            {
                T83BeTarget obj = new T83BeTarget();
                obj.BePer = model.Be_Per;
                obj.BTarget = model.B_Target;
                obj.ETarget = model.E_Target;
                obj.ExTarget = model.Ex_Target;
                obj.UserId = model.User_Id;
                obj.RegionCode = model.Region_Code;
                obj.Isdeleted = Convert.ToByte(false);
                obj.Createdby = model.User_Id;
                obj.Createddate = DateTime.Now;
                obj.Updatedby = model.User_Id;
                obj.Updateddate = DateTime.Now;
                context.T83BeTargets.Add(obj);
                context.SaveChanges();
                Id = (obj.BePer);
            }
            else
            {
                _BeTarget.BTarget = model.B_Target;
                _BeTarget.ETarget = model.E_Target;
                _BeTarget.ExTarget = model.Ex_Target;
                _BeTarget.Updatedby = model.User_Id;
                _BeTarget.Updateddate = DateTime.Now;
                Id = _BeTarget.BePer;
                context.SaveChanges();
            }
            
            return Id;
            #endregion
        }
    }

}
