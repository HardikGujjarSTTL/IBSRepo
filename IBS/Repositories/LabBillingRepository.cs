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
    public class LabBillingRepository : ILabBillingRepository
    {
        private readonly ModelContext context;

        public LabBillingRepository(ModelContext context)
        {
            this.context = context;
        }

        public LabBillingModel FindByID(string LabBillPer,string rgnCode)
        {  
            using (var dbContext = context.Database.GetDbConnection())
            {
                OracleParameter[] par = new OracleParameter[3];
                par[0] = new OracleParameter("p_LabBillPer", OracleDbType.Decimal, LabBillPer, ParameterDirection.Input);
                par[1] = new OracleParameter("p_REGIONCODE", OracleDbType.Char, rgnCode, ParameterDirection.Input);
                par[2] = new OracleParameter("p_Cursor", OracleDbType.RefCursor, ParameterDirection.Output);
                var ds = DataAccessDB.GetDataSet("SP_GetData_T59LabExp", par, 1);

                LabBillingModel model = new();
                if (ds != null && ds.Tables.Count > 0)
                {
                    string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                    model = JsonConvert.DeserializeObject<List<LabBillingModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }).FirstOrDefault();
                } 
                return model;
            } 
        }
 
        public DTResult<LabBillingModel> GetLabBillingList(DTParameters dtParameters, string RgnCd)
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
                    orderCriteria = "Lab_Bill_Per";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "Lab_Bill_Per";
                orderAscendingDirection = true;
            }
            query = from l in context.T59LabExps
                    where l.RegionCode == RgnCd && (l.Isdeleted == 0 || l.Isdeleted == null)
                    select new LabBillingModel
                    {
                        //Id = l.Id,                         
                        Lab_Exp = l.LabExp,
                        Lab_Bill_Per = l.LabBillPer,
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

        public bool Remove(string LabBillPer, string strRgn,int UserID)
        {
            //var _contracts = context.T59LabExps.Find(LabBillPer);
            var _contracts = (from m in context.T59LabExps
                             where m.LabBillPer == LabBillPer
                             && m.RegionCode == strRgn
                             select m).FirstOrDefault();

            if (_contracts == null) { return false; }

            _contracts.Isdeleted = Convert.ToByte(true);
            _contracts.Updatedby = Convert.ToInt32(UserID);
            _contracts.Updateddate = DateTime.Now;
            context.SaveChanges();
            return true;
        }

        public string LabBillingDetailsInsertUpdate(LabBillingModel model)
        {
            var Id = "";
            model.Lab_Bill_Per = model.LabBillPerYear + model.LabBillPerMon;
              
            //var _contract = context.T59LabExps.Find(model.Lab_Bill_Per,model.Region_Code);
            var _contract = (from m in context.T59LabExps 
                             where m.LabBillPer == model.Lab_Bill_Per 
                             && m.RegionCode == model.Region_Code
                             select m).FirstOrDefault();

            #region Contract save
            if (_contract == null)
            {
                T59LabExp obj = new T59LabExp();
                obj.LabBillPer = model.Lab_Bill_Per;
                obj.LabExp = model.Lab_Exp;
                obj.Datetime = model.Datetime;
                obj.UserId = model.User_Id;
                obj.RegionCode = model.Region_Code;
                obj.Isdeleted = Convert.ToByte(false);
                obj.Createdby = Convert.ToInt32(model.Createdby);
                obj.Createddate = DateTime.Now;
                obj.Updatedby = Convert.ToInt32(model.Updatedby);
                obj.Updateddate = DateTime.Now;
                context.T59LabExps.Add(obj);
                context.SaveChanges();
                Id = (obj.LabBillPer);
            }
            else
            {
                //_contract.LabBillPer = model.Lab_Bill_Per;
                _contract.LabExp = model.Lab_Exp;
                _contract.Updatedby = model.Updatedby;
                _contract.Updateddate = DateTime.Now;
                Id = _contract.LabBillPer;
                context.SaveChanges();
            }
            
            return Id;
            #endregion
        }
    }

}
