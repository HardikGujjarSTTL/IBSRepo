using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace IBS.Repositories
{
    public class LastYearOutstandingRepository : ILastYearOutstandingRepository
    {
        private readonly ModelContext context;

        public LastYearOutstandingRepository(ModelContext context)
        {
            this.context = context;
        }

        public LastYearOutstandingModel FindByID(string LYPer, string rgnCode)
        {
            using (var dbContext = context.Database.GetDbConnection())
            {
                OracleParameter[] par = new OracleParameter[3];
                par[0] = new OracleParameter("p_LYPer", OracleDbType.Decimal, LYPer, ParameterDirection.Input);
                par[1] = new OracleParameter("p_REGIONCODE", OracleDbType.Char, rgnCode, ParameterDirection.Input);
                par[2] = new OracleParameter("p_Cursor", OracleDbType.RefCursor, ParameterDirection.Output);
                var ds = DataAccessDB.GetDataSet("SP_GETDATA_T84OUTStandingLY", par, 1);

                LastYearOutstandingModel model = new();
                if (ds != null && ds.Tables.Count > 0)
                {
                    string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                    model = JsonConvert.DeserializeObject<List<LastYearOutstandingModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }).FirstOrDefault();
                }
                return model;
            }
        }

        public DTResult<LastYearOutstandingModel> GetLastYearOutstandingList(DTParameters dtParameters, string RgnCd)
        {
            DTResult<LastYearOutstandingModel> dTResult = new() { draw = 0 };
            IQueryable<LastYearOutstandingModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "Ly_Per";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                //if we have an empty search then just order the results by Id ascending
                orderCriteria = "Ly_Per";
                orderAscendingDirection = true;
            }
            query = from l in context.T84OutsLies
                    where l.RegionCode == RgnCd && l.Isdeleted != true
                    select new LastYearOutstandingModel
                    {
                        //Id = l.Id,
                        Ly_Per = l.LyPer,
                        Ly_Outs = l.LyOuts,
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

        public bool Remove(string LyPer, string strRgn)
        {

            var _LyPer = (from m in context.T84OutsLies
                          where m.LyPer == LyPer
                         && m.RegionCode == strRgn
                          select m).FirstOrDefault();

            if (_LyPer == null) { return false; }

            _LyPer.Isdeleted = true;
            _LyPer.Updateddate = DateTime.Now;
            context.SaveChanges();
            return true;
        }

        public string LastYearOutstandingDetailsInsertUpdate(LastYearOutstandingModel model)
        {
            var Id = "";
            model.Ly_Per = model.LyPerYear + model.LyPerMon;
            var _BeTarget = (from m in context.T84OutsLies
                             where m.LyPer == model.Ly_Per
                             && m.RegionCode == model.Region_Code
                             select m).FirstOrDefault();

            #region LastYearOutstanding save
            if (_BeTarget == null)
            {
                T84OutsLy obj = new T84OutsLy();
                obj.LyPer = model.Ly_Per;
                obj.LyOuts = model.Ly_Outs;
                obj.UserId = model.User_Id;
                obj.RegionCode = model.Region_Code;
                obj.Isdeleted = false;
                //obj.Createdby = model.UserId;
                //obj.Createddate = DateTime.Now;
                // obj.Updatedby = model.UserId;
                // obj.Updateddate = DateTime.Now;
                context.T84OutsLies.Add(obj);
                context.SaveChanges();
                Id = (obj.LyPer);
            }
            else
            {
                //_BeTarget.LyPer = model.LyPer;
                _BeTarget.LyOuts = model.Ly_Outs;
                //_BeTarget.Updatedby = model.UserId;
                //_BeTarget.Updateddate = DateTime.Now;
                Id = _BeTarget.LyPer;
                context.SaveChanges();
            }

            return Id;
            #endregion
        }
    }

}
