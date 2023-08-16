using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using System.Composition;
using System;
using System.Data;
using System.Diagnostics.Contracts;
using IBS.Helper;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using NuGet.Protocol.Core.Types;
using System.Drawing;
using System.Collections.Generic;

namespace IBS.Repositories
{
    public class TechReferenceRepository : ITechReferenceRepository
    {
        private readonly ModelContext context;

        public TechReferenceRepository(ModelContext context)
        {
            this.context = context;
        }
         
        public TechReferenceModel FindByID(string TechId, string rgnCode)
        {
            using (var dbContext = context.Database.GetDbConnection())
            {
                OracleParameter[] par = new OracleParameter[3];
                par[0] = new OracleParameter("in_tech_id", OracleDbType.Varchar2, TechId, ParameterDirection.Input);
                par[1] = new OracleParameter("in_region_cd", OracleDbType.Varchar2, rgnCode, ParameterDirection.Input);
                par[2] = new OracleParameter("out_result_set", OracleDbType.RefCursor, ParameterDirection.Output);
                var ds = DataAccessDB.GetDataSet("SP_GETDATA_T66TECHREF", par, 1);

                TechReferenceModel model = new();
                if (ds != null && ds.Tables.Count > 0)
                {
                    string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                    model = JsonConvert.DeserializeObject<List<TechReferenceModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }).FirstOrDefault();
                }
                return model;
            }

            // TechReferenceModel model = new();
            //T66TechRef tenant = context.T66TechRefs.Find(TechId);
            //if (tenant == null)
            //   throw new Exception("Tech Reference Record Not found");
            // else
            //{
                //model.ContractId = tenant.ContractId;
                //model.ContractNo = tenant.ContractNo;
                //model.ContractPanalty = tenant.ContractPanalty;
                //model.ContractCm = tenant.ContractCm;
                //model.ExpOr =  tenant.ExpOr;
                //model.ClientName = tenant.ClientName;
                //model.ContInspFee = tenant.ContInspFee;
                //model.ContPerFrom = Convert.ToDateTime(tenant.ContPerFrom);
                //model.Status =  tenant.Status;
                //model.ContractSpecialCondn = tenant.ContractSpecialCondn;
                //model.Datetime = tenant.Datetime;
                //model.OfferDt = tenant.OfferDt;
                //model.ContPerTo = tenant.ContPerTo;
                //model.RegionCode = tenant.RegionCode;
                //model.ContractFee = tenant.ContractFee;
                //model.ContractFeeNum = tenant.ContractFeeNum;
                //model.ScopeOfWork = tenant.ScopeOfWork;
                //model.UserId= tenant.UserId;
                //model.ContSignDt = tenant.ContSignDt;
                //model.Isdeleted = tenant.Isdeleted;
                //model.Createddate = tenant.Createddate;
                //model.Createdby = tenant.Createdby;
                //model.Updateddate = tenant.Updateddate;
                //model.Updatedby = tenant.Updatedby;
                //return model;
             //}
        }

        public DTResult<TechReferenceModel> GetTechReferenceList(DTParameters dtParameters, string strRgn)
        {
            DTResult<TechReferenceModel> dTResult = new() { draw = 0 };
            IQueryable<TechReferenceModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (string.IsNullOrEmpty(orderCriteria))
                {
                    orderCriteria = "TechId";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "TechId";
                orderAscendingDirection = true;
            }

            query = (from tr in context.T66TechRefs
                     join ie in context.T09Ies on tr.TechIeCd equals ie.IeCd
                     join co in context.T08IeControllOfficers on tr.TechCmCd equals co.CoCd
                     where tr.TechId != null && tr.RegionCd == strRgn
                     orderby tr.TechDate
                     select new TechReferenceModel
                     {
                         //tr.sn = context.T66TechRefs.OrderBy(t => t.TechDate).Count(t => t.TechDate <= tr.tech_date),
                         TechCmName = co.CoName,
                         TechIeName = ie.IeName,
                         TechItemDes = tr.TechItemDes,
                         TechSpecDrg = tr.TechSpecDrg,
                         TechLetterNo = tr.TechLetterNo,
                         TechDate = Convert.ToDateTime(tr.TechDate).Date,
                         TechRefMade = tr.TechRefMade,
                         TechContent = tr.TechContent,
                         TechId = tr.TechId
                     });

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
            {
                query = query.Where(w =>
                    w.TechCmName.ToLower().Contains(searchBy.ToLower()) ||
                    w.TechIeName.ToLower().Contains(searchBy.ToLower()) ||
                    w.TechItemDes.ToLower().Contains(searchBy.ToLower()) ||
                    w.TechSpecDrg.ToLower().Contains(searchBy.ToLower()) ||
                    w.TechLetterNo.ToLower().Contains(searchBy.ToLower()) ||
                    w.TechDate.ToString().Contains(searchBy.ToLower()) ||
                    w.TechRefMade.ToLower().Contains(searchBy.ToLower()) ||
                    w.TechContent.ToLower().Contains(searchBy.ToLower()) ||
                    w.TechId.ToLower().Contains(searchBy.ToLower())
                );
            }

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection)
                .Skip(dtParameters.Start)
                .Take(dtParameters.Length)
                .ToList();

            dTResult.draw = dtParameters.Draw;
            return dTResult;
        }

        private string GenerateTechRefID(string ToDt,string rgnCode)
        {
            try
            {
                string techDate = ToDt.Replace("/", "");
                string regionCode = rgnCode;

                using (var dbContext = context.Database.GetDbConnection())
                {
                        List<TechReferenceModel> result = new();
                    
                        OracleParameter[] par = new OracleParameter[3];
                        par[0] = new OracleParameter("IN_TECH_DT", OracleDbType.Decimal, techDate, ParameterDirection.Input);
                        par[1] = new OracleParameter("IN_REGION_CD", OracleDbType.Char, regionCode, ParameterDirection.Input);
                        par[2] = new OracleParameter("p_result_cursor", OracleDbType.RefCursor, ParameterDirection.Output);
                        //par[2] = new OracleParameter("OUT_TECH_ID", OracleDbType.Char, ParameterDirection.Output);
                        //par[3] = new OracleParameter("OUT_ERR_CD", OracleDbType.NChar, ParameterDirection.Output);
                        var ds = DataAccessDB.GetDataSet("TEST_SP ", par, 1);
                     
                        if (ds != null && ds.Tables.Count > 0)
                        {
                            string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                            result = JsonConvert.DeserializeObject<List<TechReferenceModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }).ToList();
                        }

                    //if (result.Count > 0 && result[0].OUT_ERR_CD == -1)
                    //{
                    //    return "-1";
                    //}
                    //else
                    //{
                    //    return result[0].OUT_TECH_ID;
                    //}
                    return result[0].ToString();
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
                string str1 = str.Replace("\n", "");
                //Response.Redirect(("Error_Form.aspx?err=" + str1));
                return "-1";
            }
        }
         
        public bool Remove(string TechId, string regn,int UserId)
        {
            var _TechReference = new TechReferenceModel();
            using (var dbContext = context.Database.GetDbConnection())
            {
                OracleParameter[] par = new OracleParameter[3];
                par[0] = new OracleParameter("in_tech_id", OracleDbType.Varchar2, TechId, ParameterDirection.Input);
                par[1] = new OracleParameter("in_region_cd", OracleDbType.Varchar2, regn, ParameterDirection.Input);
                par[2] = new OracleParameter("out_result_set", OracleDbType.RefCursor, ParameterDirection.Output);
                var ds = DataAccessDB.GetDataSet("SP_GETDATA_T66TECHREF", par, 1);
                if (ds != null && ds.Tables.Count > 0)
                {
                    string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                    _TechReference = JsonConvert.DeserializeObject<List<TechReferenceModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }).FirstOrDefault();
                }
            }
            
            if (_TechReference == null) { return false; }

            _TechReference.Isdeleted = Convert.ToByte(true);
            _TechReference.Updatedby = Convert.ToInt32(UserId);
            _TechReference.Updateddate = DateTime.Now;
            context.SaveChanges();
            return true;
        }

        public string TechRefDetailsInsertUpdate(TechReferenceModel model,string rgn)
        {
            string _TechRefId = string.Empty;

            //int count = context.T66TechRefs.Where(tr => tr.TechDate == model.TechDate &&
            //    tr.RegionCd == model.RegionCd && tr.TechCmCd == model.TechCmCd && tr.TechIeCd == model.TechIeCd 
            //    && tr.TechLetterNo == model.TechLetterNo).Count();

            var _TechRef = new TechReferenceModel();

            //var _TechRef = context.T66TechRefs.Find(model.TechId);

            using (var dbContext = context.Database.GetDbConnection())
            {
                OracleParameter[] par = new OracleParameter[3];
                par[0] = new OracleParameter("in_tech_id", OracleDbType.Varchar2, model.TechId, ParameterDirection.Input);
                par[1] = new OracleParameter("in_region_cd", OracleDbType.Varchar2, rgn, ParameterDirection.Input);
                par[2] = new OracleParameter("out_result_set", OracleDbType.RefCursor, ParameterDirection.Output);
                var ds = DataAccessDB.GetDataSet("SP_GETDATA_T66TECHREF", par, 1);                 
                if (ds != null && ds.Tables.Count > 0)
                {
                    string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                    _TechRef = JsonConvert.DeserializeObject<List<TechReferenceModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }).FirstOrDefault();
                }                
            }

            var TechId = "";             
            #region Tech save and update
            if (_TechRef == null)
            {
                TechId = GenerateTechRefID(model.TechDate.ToString(), model.RegionCd);

                T66TechRef obj = new T66TechRef();
                obj.RegionCd = model.RegionCd;
                obj.TechCmCd = model.TechCmCd;
                obj.TechIeCd = model.TechIeCd;
                obj.TechItemDes = model.TechItemDes;
                obj.TechSpecDrg = model.TechSpecDrg;
                obj.TechLetterNo = model.TechLetterNo;
                obj.TechDate = model.TechDate;
                obj.TechRefMade = model.TechRefMade;
                obj.TechContent = model.TechContent;
                obj.UserId = model.UserId;
                obj.Datetime = model.Datetime;
                obj.TechId = model.TechId;       
                //obj.Isdeleted = Convert.ToByte(false);
                //obj.Createdby = Convert.ToInt32(model.UserId);
                //obj.Createddate = DateTime.Now;
                //obj.Updatedby = Convert.ToInt32(model.UserId);
                //obj.Updateddate = DateTime.Now;
                context.T66TechRefs.Add(obj);
                context.SaveChanges();
                _TechRefId =  obj.TechId;
            }
            else
            {
                _TechRef.RegionCd = model.RegionCd;
                _TechRef.TechCmCd = model.TechCmCd;
                _TechRef.TechIeCd = model.TechIeCd;
                _TechRef.TechItemDes = model.TechItemDes;
                _TechRef.TechSpecDrg = model.TechSpecDrg;
                _TechRef.TechLetterNo = model.TechLetterNo;
                _TechRef.TechDate = Convert.ToDateTime(model.TechDate);
                _TechRef.TechRefMade = model.TechRefMade;
                _TechRef.TechContent = model.TechContent;
                _TechRef.UserId = model.UserId;
                _TechRef.Datetime = model.Datetime;
                _TechRef.TechId = model.TechId;
                _TechRefId = _TechRef.TechId;
                context.SaveChanges();
            }
            #endregion
            return _TechRefId;
        }
    }

}
