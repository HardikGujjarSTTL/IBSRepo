using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace IBS.Repositories
{
    public class POMasterRepository : IPOMasterRepository
    {
        private readonly ModelContext context;

        public POMasterRepository(ModelContext context)
        {
            this.context = context;
        }
        public PO_MasterModel FindByID(int Id)
        {
            PO_MasterModel model = new();
            T13PoMaster POMaster = context.T13PoMasters.Find(Convert.ToInt32(Id));

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
                model.L5noPo = POMaster.L5noPo;
                model.PoDt = POMaster.PoDt;
                model.RecvDt = POMaster.RecvDt;
                model.VendCd = POMaster.VendCd;
                model.RlyCd = POMaster.RlyCd;
                model.RegionCode = POMaster.RegionCode;
                model.UserId = POMaster.UserId;
                model.Datetime = POMaster.Datetime;
                model.Remarks = POMaster.Remarks;
                model.InspectingAgency = POMaster.InspectingAgency;
                model.PoiCd = POMaster.PoiCd;
                model.PoSource = POMaster.PoSource;
                model.PendingCharges = POMaster.PendingCharges;
                model.Id = POMaster.Id;
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
                    select new PO_MasterModel
                    {
                        VendCd = POMaster.VendCd,
                        CaseNo = POMaster.CaseNo,
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
        public bool Remove(int Id, int UserID)
        {
            var POMasters = context.T13PoMasters.Find(Convert.ToInt32(Id));
            if (POMasters == null) { return false; }

            POMasters.Isdeleted = Convert.ToByte(true);
            POMasters.Updatedby = Convert.ToInt32(UserID);
            POMasters.Updateddate = DateTime.Now;
            context.SaveChanges();
            return true;
        }
        public int POMasterDetailsInsertUpdate(PO_MasterModel model)
        {
            int Id = 0;
            var POMaster = context.T13PoMasters.Find(model.Id);
            #region POMaster save
            if (POMaster == null)
            {
                T13PoMaster obj = new T13PoMaster();
                obj.CaseNo = model.CaseNo;
                obj.PurchaserCd = model.PurchaserCd;
                obj.StockNonstock = model.StockNonstock;
                obj.RlyNonrly = model.RlyNonrly;
                obj.PoOrLetter = model.PoOrLetter;
                obj.PoNo = model.PoNo;
                obj.L5noPo = model.L5noPo;
                obj.PoDt = model.PoDt;
                obj.RecvDt = model.RecvDt;
                obj.VendCd = model.VendCd;
                obj.RlyCd = model.RlyCd;
                obj.RegionCode = model.RegionCode;
                obj.UserId = model.UserId;
                obj.Datetime = model.Datetime;
                obj.Remarks = model.Remarks;
                obj.InspectingAgency = model.InspectingAgency;
                obj.PoiCd = model.PoiCd;
                obj.PoSource = model.PoSource;
                obj.PendingCharges = model.PendingCharges;
                obj.Createddate = DateTime.Now;
                obj.Createdby = model.Createdby;
                context.T13PoMasters.Add(obj);
                context.SaveChanges();
                Id = Convert.ToInt32(obj.Id);
            }
            else
            {
                POMaster.CaseNo = model.CaseNo;
                POMaster.PurchaserCd = model.PurchaserCd;
                POMaster.StockNonstock = model.StockNonstock;
                POMaster.RlyNonrly = model.RlyNonrly;
                POMaster.PoOrLetter = model.PoOrLetter;
                POMaster.PoNo = model.PoNo;
                POMaster.L5noPo = model.L5noPo;
                POMaster.PoDt = model.PoDt;
                POMaster.RecvDt = model.RecvDt;
                POMaster.VendCd = model.VendCd;
                POMaster.RlyCd = model.RlyCd;
                POMaster.RegionCode = model.RegionCode;
                POMaster.UserId = model.UserId;
                POMaster.Datetime = model.Datetime;
                POMaster.Remarks = model.Remarks;
                POMaster.InspectingAgency = model.InspectingAgency;
                POMaster.PoiCd = model.PoiCd;
                POMaster.PoSource = model.PoSource;
                POMaster.PendingCharges = model.PendingCharges;
                POMaster.Updatedby = model.Updatedby;
                POMaster.Updateddate = DateTime.Now;
                context.SaveChanges();
                Id = Convert.ToInt32(POMaster.Id);
            }
            #endregion
            return Id;
        }
        public PO_MasterModel FindCaseNo(string CaseNo)
        {
            PO_MasterModel model = new();
            T13PoMaster POMaster = context.T13PoMasters.Find(CaseNo);

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
                model.L5noPo = POMaster.L5noPo;
                model.PoDt = POMaster.PoDt;
                model.RecvDt = POMaster.RecvDt;
                model.VendCd = POMaster.VendCd;
                model.RlyCd = POMaster.RlyCd;
                model.RegionCode = POMaster.RegionCode;
                model.UserId = POMaster.UserId;
                model.Datetime = POMaster.Datetime;
                model.Remarks = POMaster.Remarks;
                model.InspectingAgency = POMaster.InspectingAgency;
                model.PoiCd = POMaster.PoiCd;
                model.PoSource = POMaster.PoSource;
                model.PendingCharges = POMaster.PendingCharges;
                model.Id = POMaster.Id;
                return model;
            }
        }

    }

}
