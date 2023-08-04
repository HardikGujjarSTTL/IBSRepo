using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using Newtonsoft.Json;

namespace IBS.Repositories
{
    public class POMasterRepository : IPOMasterRepository
    {
        private readonly ModelContext context;

        public POMasterRepository(ModelContext context)
        {
            this.context = context;
        }
        public PO_MasterModel FindByID(string CaseNo)
        {
            PO_MasterModel model = new();
            T80PoMaster POMaster = context.T80PoMasters.Find(CaseNo);

            if (POMaster == null)
                throw new Exception("Po Master Record Not found");
            else
            {
                model.CaseNo = POMaster.CaseNo.Trim();
                model.PurchaserCd = POMaster.PurchaserCd;
                model.StockNonstock = POMaster.StockNonstock;
                model.RlyNonrly = POMaster.RlyNonrly;
                model.PoOrLetter = POMaster.PoOrLetter;
                model.PoNo = POMaster.PoNo;
                model.PoDt = POMaster.PoDt;
                model.RecvDt = POMaster.RecvDt;
                model.RlyCdDesc = POMaster.RlyCdDesc;
                model.VendCd = POMaster.VendCd;
                model.RlyCd = POMaster.RlyCd;
                model.RegionCode = POMaster.RegionCode;
                model.UserId = POMaster.UserId;
                model.Datetime = POMaster.Datetime;
                model.Remarks = POMaster.Remarks;
                model.PoiCd = POMaster.PoiCd;
                return model;
            }
        }
        public DTResult<PO_MasterModel> GetPOMasterList(DTParameters dtParameters,int VendCd)
        {

            DTResult<PO_MasterModel> dTResult = new() { draw = 0 };
            IQueryable<PO_MasterModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "RealCaseNo";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "RealCaseNo";
                orderAscendingDirection = true;
            }
            query = from POMaster in context.ViewPomasterlists
                    where POMaster.VendCd == VendCd
                    && POMaster.Isdeleted != Convert.ToByte(true)
                    select new PO_MasterModel
                    {
                        VendCd = POMaster.VendCd,
                        CaseNo = POMaster.CaseNo.Trim(),
                        PoNo = POMaster.PoNo,
                        PoDtDate = POMaster.PoDt,
                        RlyCd = POMaster.RlyCd,
                        VendorName = POMaster.VendName,
                        ConsigneeSName = POMaster.ConsigneeSName,
                        RealCaseNo = POMaster.RealCaseNo,
                        Remarks = POMaster.Remarks,
                    };


            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.CaseNo).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.PoNo).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.PoDtDate).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }
        public bool Remove(string CaseNo, int UserID)
        {
            var POMasters = context.T80PoMasters.Find(CaseNo);
            if (POMasters == null) { return false; }

            POMasters.Isdeleted = Convert.ToByte(true);
            POMasters.Updatedby = Convert.ToInt32(UserID);
            POMasters.Updateddate = DateTime.Now;
            context.SaveChanges();
            return true;
        }
        public string POMasterDetailsInsertUpdate(PO_MasterModel model)
        {
            
            string CaseNo = "";
            var POMaster = context.T80PoMasters.Find(model.CaseNo);
            #region POMaster save
            if (POMaster == null)
            {
                //var w_ctr= model.RegionCode + model.PoDt.ToString().Substring(8,2) + model.PoDt.ToString().Substring(0, 2);
                //var cn=(from m in context.T80PoMasters 
                //        where m.RegionCode==model.RegionCode && m.CaseNo.Substring(0,4)== w_ctr
                //        select m.CaseNo.Substring(6,4)).Max();

                string date = model.PoDt.ToString().Substring(0, 10);
                OracleParameter[] par = new OracleParameter[3];
                par[0] = new OracleParameter("IN_REGION_CD", OracleDbType.Char, model.RegionCode, ParameterDirection.Input);
                par[1] = new OracleParameter("IN_PO_DT", OracleDbType.Varchar2, date, ParameterDirection.Input);
                //par[2] = new OracleParameter("OUT_CASE_NO", OracleDbType.Char, ParameterDirection.Output);
                //par[3] = new OracleParameter("OUT_ERR_CD", OracleDbType.Int32, ParameterDirection.Output);
                par[2] = new OracleParameter("p_result_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

                var ds = DataAccessDB.GetDataSet("GENERATE_VEND_CASE_NO", par, 1);

                PO_MasterModel model1 = new();
                if (ds != null && ds.Tables.Count > 0)
                {
                    string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                    model1 = JsonConvert.DeserializeObject<List<PO_MasterModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }).FirstOrDefault();
                }

                T80PoMaster obj = new T80PoMaster();
                obj.CaseNo = model1.CaseNo.Trim();
                obj.PurchaserCd = model.PurchaserCd;
                obj.StockNonstock = model.StockNonstock;
                obj.PoOrLetter = model.PoOrLetter;
                obj.RlyNonrly = model.RlyNonrly;
                obj.PoNo = model.PoNo;
                obj.PoDt = model.PoDt;
                obj.RecvDt = model.RecvDt;
                obj.VendCd = model.VendCd;
                obj.RlyCd = model.RlyCd;
                obj.RlyCdDesc = model.RlyCdDesc;
                obj.RegionCode = model.RegionCode;
                obj.Remarks = model.Remarks;
                obj.Datetime = DateTime.Now;
                obj.PoiCd = model.PoiCd;
                context.T80PoMasters.Add(obj);
                context.SaveChanges();
                CaseNo = obj.CaseNo;
            }
            else
            {
                POMaster.PurchaserCd = model.PurchaserCd;
                POMaster.StockNonstock = model.StockNonstock;
                POMaster.PoOrLetter = model.PoOrLetter;
                POMaster.RlyNonrly = model.RlyNonrly;
                POMaster.PoNo = model.PoNo;
                POMaster.PoDt = model.PoDt;
                POMaster.RecvDt = model.RecvDt;
                POMaster.VendCd = model.VendCd;
                POMaster.RlyCd = model.RlyCd;
                POMaster.RlyCdDesc = model.RlyCdDesc;
                POMaster.RegionCode = model.RegionCode;
                POMaster.Remarks = model.Remarks;
                POMaster.Datetime = DateTime.Now;
                POMaster.PoiCd = model.PoiCd;
                context.SaveChanges();
                CaseNo = POMaster.CaseNo;
            }
            #endregion
            return CaseNo;
        }
        public PO_MasterModel FindCaseNo(string CaseNo,int VendCd)
        {
            PO_MasterModel model = new();
            //T13PoMaster POMaster = context.T13PoMasters.Find(CaseNo);
            T80PoMaster POMaster =(from l in context.T80PoMasters
                                   where l.CaseNo== CaseNo && l.VendCd == VendCd select l).FirstOrDefault();

            if (POMaster == null)
                throw new Exception("Po Master Record Not found");
            else
            {
                model.CaseNo = POMaster.CaseNo;
                model.PurchaserCd = POMaster.PurchaserCd;
                model.StockNonstock = POMaster.StockNonstock;
                model.RlyNonrly = POMaster.RlyNonrly;
                model.PoOrLetter = POMaster.PoOrLetter;
                model.PoNo = POMaster.PoNo;
                model.PoDt = POMaster.PoDt;
                model.RecvDt = POMaster.RecvDt;
                model.VendCd = POMaster.VendCd;
                model.RlyCd = POMaster.RlyCd;
                model.RegionCode = POMaster.RegionCode;
                model.UserId = POMaster.UserId;
                model.Datetime = POMaster.Datetime;
                model.Remarks = POMaster.Remarks;
                model.PoiCd = POMaster.PoiCd;
                return model;
            }
        }

    }

}
