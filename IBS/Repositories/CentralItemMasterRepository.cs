using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Data.SqlClient;

namespace IBS.Repositories
{
    public class CentralItemMasterRepository : ICentralItemMasterRepository
    {
        private readonly ModelContext context;

        #region Master
        public CentralItemMasterRepository(ModelContext context)
        {
            this.context = context;
        }
        public CentralItemMasterModel FindByID(int ID)
        {
            CentralItemMasterModel model = new();
            T34CentralItemMaster t34CentralItemMaster = context.T34CentralItemMasters.Find(ID);

            if (t34CentralItemMaster == null)
                throw new Exception("Record Not found");
            else
            {
                model.RailCd = t34CentralItemMaster.RailCd;
                model.RailDesc = t34CentralItemMaster.RailDesc;
                model.RailLengthMeter = t34CentralItemMaster.RailLengthMeter;
                model.UserId = t34CentralItemMaster.UserId;
                return model;
            }
        }
        public DTResult<CentralItemMasterModel> GetList(DTParameters dtParameters)
        {

            DTResult<CentralItemMasterModel> dTResult = new() { draw = 0 };
            IQueryable<CentralItemMasterModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "Id";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "Id";
                orderAscendingDirection = true;
            }
            query = from l in context.T34CentralItemMasters
                    where l.Isdeleted == 0 
                    select new CentralItemMasterModel
                    {
                        Id = l.Id,
                        RailCd = l.RailCd,
                        RailDesc = l.RailDesc,
                        RailLengthMeter = l.RailLengthMeter
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.RailCd).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.RailDesc).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }
        public bool Remove(int Id, int UserID)
        {
            var t34CentralItemMaster = context.T34CentralItemMasters.Find(Id);
            if (t34CentralItemMaster == null) { return false; }

            t34CentralItemMaster.Isdeleted = Convert.ToByte(true);
            t34CentralItemMaster.Updatedby = UserID;
            t34CentralItemMaster.Updateddate = DateTime.Now;
            context.SaveChanges();
            return true;
        }
        public int InsertUpdate(CentralItemMasterModel model)
        {
            int ID = 0;
            var t34CentralItemMaster = context.T34CentralItemMasters.Find(model.Id);
            #region Role save
            if (t34CentralItemMaster == null)
            {
                T34CentralItemMaster obj = new T34CentralItemMaster();
                obj.RailCd = model.RailCd;
                obj.RailDesc = model.RailDesc;
                obj.RailLengthMeter = model.RailLengthMeter;
                obj.UserId =model.UserId;
                obj.Isdeleted = Convert.ToByte(false);
                obj.Createdby = model.Createdby;
                obj.Createddate = DateTime.Now;
                context.T34CentralItemMasters.Add(obj);
                context.SaveChanges();
                ID = Convert.ToInt32(obj.Id);
            }
            else
            {
                t34CentralItemMaster.RailCd = model.RailCd;
                t34CentralItemMaster.RailDesc = model.RailDesc;
                t34CentralItemMaster.RailLengthMeter = model.RailLengthMeter;
                t34CentralItemMaster.Updatedby = model.Updatedby;
                t34CentralItemMaster.Updateddate = DateTime.Now;
                context.SaveChanges();
                ID = Convert.ToInt32(t34CentralItemMaster.Id);
            }
            #endregion
            return ID;
        }

        public bool CheckAlreadyExist(CentralItemMasterModel model)
        {
            var t34CentralItemMaster =(from m in context.T34CentralItemMasters
                                       where m.RailCd == model.RailCd && m.Id != model.Id select m).ToList();
            if(t34CentralItemMaster != null && t34CentralItemMaster.Count != 0) 
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Details
        public CentralItemDetailModel FindDetailsByID(int ID)
        {
            CentralItemDetailModel model = new();
            T35CentralItemDetail t35CentralItemDetail = context.T35CentralItemDetails .Find(ID);

            if (t35CentralItemDetail == null)
                throw new Exception("Record Not found");
            else
            {
                model.RailId = t35CentralItemDetail.RailId;
                model.RailPricePerMt = t35CentralItemDetail.RailPricePerMt;
                model.PackingCharge = t35CentralItemDetail.PackingCharge;
                model.PriceDateFr = t35CentralItemDetail.PriceDateFr;
                model.PriceDateTo = t35CentralItemDetail.PriceDateTo;
                model.Isactive = Convert.ToBoolean(t35CentralItemDetail.Isactive);
                return model;
            }
        }
        public DTResult<CentralItemDetailModel> GetDetailsList(DTParameters dtParameters)
        {

            DTResult<CentralItemDetailModel> dTResult = new() { draw = 0 };
            IQueryable<CentralItemDetailModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "Id";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "Id";
                orderAscendingDirection = true;
            }
            int RailId=0;
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["RailId"]))
            {
                RailId = Convert.ToInt32(dtParameters.AdditionalValues["RailId"]);
            }

            query = from l in context.T35CentralItemDetails
                    where l.Isdeleted == 0 && l.RailId == RailId
                    select new CentralItemDetailModel
                    {
                        Id = l.Id,
                        RailId = l.RailId,
                        RailPricePerMt = l.RailPricePerMt,
                        PackingCharge = l.PackingCharge,
                        PriceDateFr = l.PriceDateFr,
                        PriceDateTo = l.PriceDateTo,
                        Isactive = Convert.ToBoolean(l.Isactive)
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.RailId).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.RailPricePerMt).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }
        public bool RemoveDetails(int Id, int UserID)
        {
            var t35CentralItemDetail = context.T35CentralItemDetails.Find(Id);
            if (t35CentralItemDetail == null) { return false; }
            t35CentralItemDetail.Isdeleted = Convert.ToByte(true);
            t35CentralItemDetail.Updatedby = UserID;
            t35CentralItemDetail.Updateddate = DateTime.Now;
            context.SaveChanges();
            return true;
        }
        public int DetailsInsertUpdate(CentralItemDetailModel model)
        {
            int ID = 0;
            var t35CentralItemDetail = context.T35CentralItemDetails.Find(model.Id);
            #region Role save
            if (t35CentralItemDetail == null)
            {
                T35CentralItemDetail obj = new T35CentralItemDetail();
                obj.RailId = model.RailId;
                obj.RailPricePerMt = model.RailPricePerMt;
                obj.PackingCharge = model.PackingCharge;
                obj.PriceDateFr = model.PriceDateFr;
                obj.PriceDateTo = model.PriceDateTo;
                obj.Isactive = Convert.ToByte(model.Isactive);
                obj.Isdeleted = Convert.ToByte(false);
                obj.Createdby = model.Createdby;
                obj.Createddate = DateTime.Now;
                context.T35CentralItemDetails.Add(obj);
                context.SaveChanges();
                ID = Convert.ToInt32(obj.Id);
            }
            else
            {
                t35CentralItemDetail.RailPricePerMt = model.RailPricePerMt;
                t35CentralItemDetail.PackingCharge = model.PackingCharge;
                t35CentralItemDetail.PriceDateFr = model.PriceDateFr;
                t35CentralItemDetail.PriceDateTo = model.PriceDateTo;
                t35CentralItemDetail.Isactive = Convert.ToByte(model.Isactive);
                t35CentralItemDetail.Updatedby = model.Updatedby;
                t35CentralItemDetail.Updateddate = DateTime.Now;
                context.SaveChanges();
                ID = Convert.ToInt32(t35CentralItemDetail.Id);
            }
            #endregion
            return ID;
        }
        #endregion

    }

}
