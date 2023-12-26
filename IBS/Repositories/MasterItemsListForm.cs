using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
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

        public MasterItemsListFormModel FindByID(string ItemCd, string Region)
        {
            MasterItemsListFormModel model = new();
            T61ItemMaster role = context.T61ItemMasters.Find(ItemCd);

            if (role == null)
                throw new Exception("Role Record Not found");
            else
            {
                model.ItemCd = role.ItemCd;
                model.ItemDesc = role.ItemDesc;
                model.Department = role.Department;
                model.IeCd = role.Ie;
                model.CoCd = role.Cm;
                model.CreationRevDt = role.CreationRevDt;
                model.TimeForInsp = role.TimeForInsp;
                model.UserId = role.UserId;
                model.Datetime = role.Datetime;
                model.Region = Region;

                model.Isdeleted = role.Isdeleted;
                model.Updatedby = role.Updatedby;
                model.Createdby = role.Createdby;
                model.Createddate = model.Createddate;
                model.Updateddate = role.Updateddate;
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
            //query = from l in context.T61ItemMasters
            //        join a in context.IbsAppdocuments on l.ItemCd equals a.Applicationid
            //        where l.Isdeleted == 0 || l.Isdeleted == null

            //query = from a in context.IbsAppdocuments
            //        join l in context.T61ItemMasters
            //            on a.Applicationid equals l.ItemCd into joinedItems
            //        from l in joinedItems.DefaultIfEmpty()
            //        where l == null || l.Isdeleted == 0
            query = from l in context.T61ItemMasters
                    join a in context.IbsAppdocuments on l.ItemCd equals a.Applicationid into joinedItems
                    from a in joinedItems.DefaultIfEmpty()
                    where l.Isdeleted == 0 || l.Isdeleted == null

                    select new MasterItemsListFormModel
                    {
                        ItemCd = l.ItemCd,
                        ItemDesc = l.ItemDesc,
                        UserId = l.UserId,
                        Department = l.Department,
                        TimeForInsp = l.TimeForInsp,
                        Checksheet = "/ReadWriteData/MASTER_ITEMS_CHECKSHEETS/" + l.ItemCd + ".RAR",
                        FilePath = "/ReadWriteData/MASTER_ITEMS_CHECKSHEETS/" + a.Fileid,

                        //Isdeleted = l.Isdeleted,
                        //Createddate = l.Createddate,
                        //Createdby = l.Createdby,
                        //Updateddate = l.Updateddate,
                        //Updatedby = l.Updatedby
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

        public string DtlInsertUpdate(MasterItemsListFormModel model, string Region, int GetIeCd)
        {
            string RoleId = "";
            var checkit = "N001704";
            var substring = checkit.Substring(0, 1);

            var itemsWithWctr = (from v in context.T61ItemMasters
                                 where v.ItemCd.Substring(0, 1) == Region
                                 select new MasterItemsListFormModel
                                 {
                                     ItemCd = v.ItemCd,
                                 }).ToList();


            //var itemsWithWctr = context.T61ItemMasters.Where(x => GetRegionCode.Contains(x.ItemCd.ToString().Substring(0, 1))).ToList();
            var maxW_Sno = itemsWithWctr.Select(item => int.TryParse(item.ItemCd.Substring(1, 6), out var parsed) ? parsed : 0).Max();
            var w_sno = (maxW_Sno + 1).ToString().PadLeft(6, '0');

            var RDF = context.T61ItemMasters.Where(x => x.ItemCd == model.ItemCd).FirstOrDefault();
            #region Role save
            if (RDF == null)
            {
                T61ItemMaster obj = new T61ItemMaster();
                obj.ItemCd = Region + w_sno;
                obj.ItemDesc = model.ItemDesc;
                obj.Department = model.Department;
                obj.TimeForInsp = model.TimeForInsp;
                obj.Checksheet = Region + w_sno;
                obj.UserId = Convert.ToString(model.Createdby);
                obj.Datetime = DateTime.Now;
                obj.Ie = model.IeCd;
                obj.Cm = model.CoCd;
                obj.CreationRevDt = model.CreationRevDt;

                obj.Isdeleted = Convert.ToByte(false);
                obj.Createdby = model.Createdby;
                obj.Createddate = DateTime.Now;
                context.T61ItemMasters.Add(obj);
                context.SaveChanges();
                RoleId = Convert.ToString(obj.ItemCd);
            }
            else
            {
                RDF.ItemCd = model.ItemCd;
                RDF.ItemDesc = model.ItemDesc;

                RDF.Department = model.Department;
                RDF.TimeForInsp = model.TimeForInsp;
                RDF.Checksheet = model.ItemCd;
                RDF.UserId = Convert.ToString(model.Updatedby);
                RDF.Ie = model.IeCd;
                RDF.Cm = model.CoCd;
                RDF.CreationRevDt = model.CreationRevDt;

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


