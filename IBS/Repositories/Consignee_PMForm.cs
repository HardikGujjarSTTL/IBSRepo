using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;

namespace IBS.Repositories
{
    public class Consignee_PMForm : IConsignee_PMForm
    {
        private readonly ModelContext context;

        public Consignee_PMForm(ModelContext context)
        {
            this.context = context;
        }
        public ConsigneePurchaseModel FindByID(int ConsigneeCd)
        {
            ConsigneePurchaseModel model = new();
            T06Consignee role = context.T06Consignees.Find(Convert.ToByte(ConsigneeCd));

            if (role == null)
                throw new Exception("Role Record Not found");
            else
            {
                model.ConsigneeType = role.ConsigneeType;
                model.ConsigneeDesig = role.ConsigneeDesig;
                model.ConsigneeDept = role.ConsigneeDept;
                model.ConsigneeCd = role.ConsigneeCd;
                model.ConsigneeCity = role.ConsigneeCity;
                model.GstinNo = role.GstinNo;
                model.SapCustCdCon = role.SapCustCdCon;
                model.UserId = role.UserId;
                model.Updatedby = role.Updatedby;
                model.Createdby = role.Createdby;
                model.Createddate = model.Createddate;
                model.Isdeleted = role.Isdeleted;
                return model;
            }
        }

        public DTResult<ConsigneePurchaseModel> GetConsignee_PMFormList(DTParameters dtParameters)
        {

            DTResult<ConsigneePurchaseModel> dTResult = new() { draw = 0 };
            IQueryable<ConsigneePurchaseModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "ConsigneeCd";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "ConsigneeCd";
                orderAscendingDirection = true;
            }
            query = from l in context.T06Consignees
                    where l.Isdeleted == 0 || l.Isdeleted == null
                    select new ConsigneePurchaseModel
                    {
                        ConsigneeCd = l.ConsigneeCd,
                        ConsigneeDept = l.ConsigneeDept,
                        ConsigneeCity = l.ConsigneeCity,
                        SapCustCdCon = l.SapCustCdCon,
                        ConsigneeType = l.ConsigneeType,
                        GstinNo = l.GstinNo,
                        UserId = l.UserId,
                        Isdeleted = l.Isdeleted,
                        Createddate = l.Createddate,
                        Createdby = l.Createdby,
                        Updateddate = l.Updateddate,
                        Updatedby = l.Updatedby
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.ConsigneeCd).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.ConsigneeCity).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }
        public bool Remove(int ConsigneeCd, int UserID)
        {
            var roles = context.T06Consignees.Find(Convert.ToByte(ConsigneeCd));
            if (roles == null) { return false; }

            roles.Isdeleted = Convert.ToByte(true);
            roles.Updatedby = Convert.ToInt32(UserID);
            roles.Updateddate = DateTime.Now;
            context.SaveChanges();
            return true;
        }

        public int Consignee_PMFormDetailsInsertUpdate(ConsigneePurchaseModel model)
        {
            int RoleId = 0;
            var CPM = context.T06Consignees.Where(x => x.ConsigneeCd == model.ConsigneeCd).FirstOrDefault();
            #region Role save
            if (CPM == null || CPM.ConsigneeCd == 0)
            {
                T06Consignee obj = new T06Consignee();

                obj.ConsigneeDept = model.ConsigneeDept;
                obj.ConsigneeDesig = model.ConsigneeDesig;
                obj.GstinNo = model.GstinNo;
                obj.SapCustCdCon = model.SapCustCdCon;
                obj.ConsigneeCity = model.ConsigneeCity;
                obj.Createdby = model.Createdby;
                obj.Isdeleted = Convert.ToByte(false);
                obj.Createddate = DateTime.Now;
                context.T06Consignees.Add(obj);
                context.SaveChanges();
                RoleId = Convert.ToInt32(obj.ConsigneeCd);
            }
            else
            {
                CPM.ConsigneeDesig = model.ConsigneeDesig;
                CPM.ConsigneeCity = model.ConsigneeCity;
                CPM.ConsigneeDept = model.ConsigneeDept;
                CPM.Updatedby = model.Updatedby;
                CPM.Updateddate = DateTime.Now;
                context.SaveChanges();
                RoleId = Convert.ToInt32(CPM.ConsigneeCd);
            }
            #endregion
            return RoleId;
        }
    }

}

