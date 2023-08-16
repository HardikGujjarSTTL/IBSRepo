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
    public class HighlightsRepository : IHighlightsRepository
    {
        private readonly ModelContext context;

        public HighlightsRepository(ModelContext context)
        {
            this.context = context;
        }

        public HighlightsModel FindByID(string HighDt, string rgnCode)
        {  
            using (var dbContext = context.Database.GetDbConnection())
            {
                OracleParameter[] par = new OracleParameter[3];
                par[0] = new OracleParameter("p_HighDt", OracleDbType.Decimal, HighDt, ParameterDirection.Input);
                par[1] = new OracleParameter("p_REGIONCODE", OracleDbType.Char, rgnCode, ParameterDirection.Input);
                par[2] = new OracleParameter("p_Cursor", OracleDbType.RefCursor, ParameterDirection.Output);
                var ds = DataAccessDB.GetDataSet("SP_GetData_T67Highlight", par, 1);

                HighlightsModel model = new();
                if (ds != null && ds.Tables.Count > 0)
                {
                    string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                    model = JsonConvert.DeserializeObject<List<HighlightsModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }).FirstOrDefault();
                } 
                return model;
            } 
        }
 
        public DTResult<HighlightsModel> GetHighlightsList(DTParameters dtParameters, string RgnCd)
        { 
            DTResult<HighlightsModel> dTResult = new() { draw = 0 };
            IQueryable<HighlightsModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "High_Dt";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                //if we have an empty search then just order the results by Id ascending
                orderCriteria = "High_Dt";
                orderAscendingDirection = true;
            }
            query = from l in context.T67Highlights
                    where l.RegionCode == RgnCd
                    select new HighlightsModel
                    {
                        //Id = l.Id,                         
                        Hight_Text = l.HightText,
                        High_Dt = l.HighDt,
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

        public bool Remove(string HighDt, string strRgn)
        {
            //var _contracts = context.T59LabExps.Find(LabBillPer);
            var _contracts = (from m in context.T67Highlights
                              where m.HighDt == HighDt
                             && m.RegionCode == strRgn
                             select m).FirstOrDefault();

            if (_contracts == null) { return false; }

            //_contracts.Isdeleted = Convert.ToByte(true);
            //_contracts.Updatedby = Convert.ToInt32(UserID);
            //_contracts.Updateddate = DateTime.Now;
            context.SaveChanges();
            return true;
        }

        public string HighlightsDetailsInsertUpdate(HighlightsModel model)
        {
            var Id = "";
            model.High_Dt = model.HighDtYear + model.HighDtMon; 
            var _High = (from m in context.T67Highlights
                             where m.HighDt == model.High_Dt
                             && m.RegionCode == model.Region_Code
                             select m).FirstOrDefault();

            #region Contract save
            if (_High == null)
            {
                T67Highlight obj = new T67Highlight();
                obj.HighDt = model.High_Dt;
                obj.HightText = model.Hight_Text;
                obj.Datetime = model.Datetime;
                obj.UserId = model.User_Id;
                obj.RegionCode = model.Region_Code;
                //obj.Isdeleted = Convert.ToByte(false);
                //obj.Createdby = Convert.ToInt32(model.User_Id);
                //obj.Createddate = DateTime.Now;
                //obj.Updatedby = Convert.ToInt32(model.UserId);
                //obj.Updateddate = DateTime.Now;
                context.T67Highlights.Add(obj);
                context.SaveChanges();
                Id = (obj.HighDt);
            }
            else
            {
                _High.HightText = model.Hight_Text;
                //_High.Updatedby = model.UserId;
                //_High.Updateddate = DateTime.Now;
                Id = _High.HighDt;
                context.SaveChanges();
            }
            
            return Id;
            #endregion
        }
    }

}
