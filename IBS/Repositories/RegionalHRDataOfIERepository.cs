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
    public class RegionalHRDataOfIERepository : IRegionalHRDataOfIERepository
    {
        private readonly ModelContext context;

        public RegionalHRDataOfIERepository(ModelContext context)
        {
            this.context = context;
        }
        public RegionalHRDataOfIEModel FindByID(int ID)
        {
            RegionalHRDataOfIEModel model = new();
            Regionalhrdataofie regionalhrdataofie = context.Regionalhrdataofies.Find(ID);

            if (regionalhrdataofie == null)
                throw new Exception("Record Not found");
            else
            {
                model.Id = regionalhrdataofie.Id;
                model.IeCd = regionalhrdataofie.IeCd;
                model.Disclipline = regionalhrdataofie.Disclipline;
                model.Joiningdate = regionalhrdataofie.Joiningdate;
                model.Postingdate = regionalhrdataofie.Postingdate;
                model.Retirementdate = regionalhrdataofie.Retirementdate;
                model.Transferdate = regionalhrdataofie.Transferdate;
                model.Deputationfromdate = regionalhrdataofie.Deputationfromdate;
                model.Deputationtodate = regionalhrdataofie.Deputationtodate;
                model.Repetriationdate = regionalhrdataofie.Repetriationdate;
                model.Ietenurefromdate = regionalhrdataofie.Ietenurefromdate;
                model.Ietenuretodate = regionalhrdataofie.Ietenuretodate;
                model.Isdeleted = regionalhrdataofie.Isdeleted;
                return model;
            }
        }

        public DTResult<RegionalHRDataOfIEModel> GetList(DTParameters dtParameters)
        {

            DTResult<RegionalHRDataOfIEModel> dTResult = new() { draw = 0 };
            IQueryable<RegionalHRDataOfIEModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "IeCd";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "IeCd";
                orderAscendingDirection = true;
            }
            query = from l in context.Regionalhrdataofies
                    join i in context.T09Ies on l.IeCd equals i.IeCd
                    where l.Isdeleted == 0 
                    select new RegionalHRDataOfIEModel
                    {
                        IeCd = l.IeCd,
                        IE_NAME=i.IeName,
                        Disclipline = l.Disclipline,
                        Joiningdate = l.Joiningdate,
                        Postingdate = l.Postingdate,
                        Retirementdate = l.Retirementdate,
                        Transferdate = l.Transferdate,
                        Deputationfromdate = l.Deputationfromdate,
                        Deputationtodate = l.Deputationtodate,
                        Repetriationdate = l.Repetriationdate,
                        Ietenurefromdate = l.Ietenurefromdate,
                        Ietenuretodate = l.Ietenuretodate,
                        Isdeleted = l.Isdeleted,
                        EncryptedId = Common.EncryptQueryString(l.Id.ToString())
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.IeCd).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.IE_NAME).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.Disclipline).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }
        public bool Remove(int ID, int UserID)
        {
            var regionalhrdataofie = context.Regionalhrdataofies.Find(ID);
            if (regionalhrdataofie == null) { return false; }
            regionalhrdataofie.Isdeleted = Convert.ToByte(true);
            regionalhrdataofie.Updatedby = UserID;
            regionalhrdataofie.Updateddate = DateTime.Now;
            context.SaveChanges();
            return true;
        }

        public int InsertUpdate(RegionalHRDataOfIEModel model)
        {
            int ID = 0;
            var regionalhrdataofie = (from r in context.Regionalhrdataofies where r.Id == model.Id select r).FirstOrDefault();
            if (regionalhrdataofie == null)
            {
                Regionalhrdataofie obj = new Regionalhrdataofie();
                obj.IeCd = model.IeCd;
                obj.Disclipline = model.Disclipline;
                obj.Joiningdate = model.Joiningdate;
                obj.Postingdate = model.Postingdate;
                obj.Retirementdate = model.Retirementdate;
                obj.Transferdate = model.Transferdate;
                obj.Deputationfromdate = model.Deputationfromdate;
                obj.Deputationtodate = model.Deputationtodate;
                obj.Repetriationdate = model.Repetriationdate;
                obj.Ietenurefromdate = model.Ietenurefromdate;
                obj.Ietenuretodate = model.Ietenuretodate;
                obj.Isdeleted = Convert.ToByte(false);
                obj.Createdby = model.Createdby;
                obj.Createddate = DateTime.Now;
                context.Regionalhrdataofies.Add(obj);
                context.SaveChanges();
                ID = Convert.ToInt32(obj.Id);
            }
            else
            {
                regionalhrdataofie.Disclipline = model.Disclipline;
                regionalhrdataofie.Joiningdate = model.Joiningdate;
                regionalhrdataofie.Postingdate = model.Postingdate;
                regionalhrdataofie.Retirementdate = model.Retirementdate;
                regionalhrdataofie.Transferdate = model.Transferdate;
                regionalhrdataofie.Deputationfromdate = model.Deputationfromdate;
                regionalhrdataofie.Deputationtodate = model.Deputationtodate;
                regionalhrdataofie.Repetriationdate = model.Repetriationdate;
                regionalhrdataofie.Ietenurefromdate = model.Ietenurefromdate;
                regionalhrdataofie.Ietenuretodate = model.Ietenuretodate;
                regionalhrdataofie.Isdeleted = Convert.ToByte(false);
                regionalhrdataofie.Updatedby = model.Updatedby;
                regionalhrdataofie.Updateddate = DateTime.Now;
                context.SaveChanges();
                ID = Convert.ToInt32(regionalhrdataofie.Id);
            }
            return ID;
        }
        public RegionalHRDataOfIEModel GetIEDetails(int ID)
        {
            RegionalHRDataOfIEModel model = new();
            T09Ie regionalhrdataofie = context.T09Ies.Find(ID);

            if (regionalhrdataofie == null)
                throw new Exception("Record Not found");
            else
            {
                model.Joiningdate = regionalhrdataofie.IeJoinDt;
                model.Retirementdate = regionalhrdataofie.RepatriationDt;
                return model;
            }
        }
    }
}
