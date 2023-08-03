using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace IBS.Repositories
{
    public class MasterItemsListForm : IMasterItemsListForm
    {
        private readonly ModelContext context;

        public MasterItemsListForm(ModelContext context)
        {
            this.context = context;
        }
        public MasterItemsListFormModel FindByID(string ItemCd)
        {
            MasterItemsListFormModel model = new();
            T61ItemMaster role = context.T61ItemMasters.Find(ItemCd);

            if (role == null)
                throw new Exception("Role Record Not found");
            else
            {
                model.ItemCd = role.ItemCd;
                model.ItemDesc = role.ItemDesc;
                model.UserId = role.UserId;
                model.Updatedby = role.Updatedby;
                model.Createdby = role.Createdby;
                model.Createddate = model.Createddate;
                model.Isdeleted = role.Isdeleted;
                model.Updateddate = role.Updateddate;
                model.Datetime = role.Datetime;
                return model;
            }
        }

        public DTResult<MasterItemsListFormModel> GetMasterItemsListFormList(DTParameters dtParameters)
        {

            DTResult<MasterItemsListFormModel> dTResult = new() { draw = 0 };
            IQueryable<MasterItemsListFormModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "ItemCd";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "ItemCd";
                orderAscendingDirection = true;
            }
            query = from l in context.T61ItemMasters
                    where l.Isdeleted == 0 || l.Isdeleted == null
                    select new MasterItemsListFormModel
                    {
                        ItemCd = l.ItemCd,
                        ItemDesc = l.ItemDesc,
                        UserId = l.UserId,
                        Department = l.Department,
                        Isdeleted = l.Isdeleted,
                        Createddate = l.Createddate,
                        Createdby = l.Createdby,
                        Updateddate = l.Updateddate,
                        Updatedby = l.Updatedby
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.ItemCd).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.ItemDesc).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }
        public bool Remove(string ItemCd, int UserID)
        {
            var roles = context.T61ItemMasters.Where(x => x.ItemCd == ItemCd).FirstOrDefault();
            if (roles == null) { return false; }

            roles.Isdeleted = Convert.ToByte(true);
            roles.Updatedby = Convert.ToInt32(UserID);
            roles.Updateddate = DateTime.Now;
            context.SaveChanges();
            return true;
        }


        public string MasterItemsListFormInsertUpdate(MasterItemsListFormModel model)
        {
            string RoleId = "";
            var RDF = context.T61ItemMasters.Where(x => x.ItemCd == model.ItemCd).FirstOrDefault();
            #region Role save
            if (RDF == null)
            {
                T61ItemMaster obj = new T61ItemMaster();

                obj.ItemCd = model.ItemCd;
                obj.ItemDesc = model.ItemDesc;
                obj.Createdby = model.Createdby;
                obj.Isdeleted = Convert.ToByte(false);
                obj.Createddate = DateTime.Now;
                context.T61ItemMasters.Add(obj);
                context.SaveChanges();
                RoleId = Convert.ToString(obj.ItemCd);
            }
            else
            {
                RDF.ItemCd = model.ItemCd;
                RDF.ItemDesc = model.ItemDesc;
                RDF.Updatedby = model.Updatedby;
                RDF.Updateddate = DateTime.Now;
                context.SaveChanges();
                RoleId = Convert.ToString(RDF.ItemCd);
            }
            #endregion
            return RoleId;
        }
    }

}


