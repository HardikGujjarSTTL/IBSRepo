using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using System.Globalization;

namespace IBS.Repositories
{
    public class LaboratoryMstRepository : ILaboratoryMstRepository
    {
        private readonly ModelContext context;

        public LaboratoryMstRepository(ModelContext context)
        {
            this.context = context;
        }

        public DTResult<LaboratoryMstModel> GetLaboratoryMstList(DTParameters dtParameters)
        {

            DTResult<LaboratoryMstModel> dTResult = new() { draw = 0 };
            IQueryable<LaboratoryMstModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "LabId";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "LabId";
                orderAscendingDirection = true;
            }
            query = from l in context.ViewLaboratories
                        //where (l.Isdeleted == 0 || l.Isdeleted == null)
                    select new LaboratoryMstModel
                    {
                        LabId = l.LabId,
                        LabName = l.LabName,
                        LabAddress = l.LabAddress,
                        LabCity = l.City
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.LabId).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }
        public LaboratoryMstModel FindByID(int LabId)
        {

            LaboratoryMstModel model = new();
            var Lab = context.T65LaboratoryMasters.Find(LabId);

            //var lb = (from l in context.T65LaboratoryMasters where l.LabId == LabId select l).FirstOrDefault();
            string labApprovalFrString = Lab.LabApprovalFr?.ToString("dd/MM/yyyy");
            string labApprovalToString = Lab.LabApprovalTo?.ToString("dd/MM/yyyy");
            if (Lab == null)
                throw new Exception("Laboratory Record Not found");
            else
            {
                //model.ID = Convert.ToDecimal(user.Id);
                model.LabId = Lab.LabId;
                model.LabName = Lab.LabName;
                model.LabAddress = Lab.LabAddress;
                model.LabCity = Convert.ToString(Lab.LabCity);
                model.LabContactPer = Lab.LabContactPer;
                model.LabContactTel = Lab.LabContactTel;
                model.LabApproval = Lab.LabApproval;
                model.LabApprovalFr = labApprovalFrString;
                model.LabApprovalTo = labApprovalToString;
                model.LabEmail = Lab.LabEmail;
                return model;
            }
            return model;
        }
        public int LabDetailsInsertUpdate(LaboratoryMstModel model)
        {
            //DateTime from = Convert.ToDateTime(model.LabApprovalFr);
            //DateTime to = Convert.ToDateTime(model.LabApprovalTo);

            int Id = 0;
            //byte labid = Convert.ToByte("-1");
            ////var Lab = context.T65LaboratoryMasters.Find(Convert.ToInt32(model.LabId));
            //var Lab = (from r in context.T65LaboratoryMasters where r.LabId == labid select r).FirstOrDefault();

            //byte labid = Convert.ToByte(null);

            var Lab = context.T65LaboratoryMasters.Find(model.LabId);

            #region Lab save
            if (Lab == null)
            {
                T65LaboratoryMaster obj = new T65LaboratoryMaster();
                //obj.LabId = Convert.ToByte(model.LabId);
                obj.LabName = model.LabName;
                obj.LabAddress = model.LabAddress;
                obj.LabCity = Convert.ToInt32(model.LabCity);
                obj.LabApproval = Convert.ToString(model.LabApproval);
                //obj.LabApprovalFr = Convert.ToDateTime(model.LabApprovalFr);
                //obj.LabApprovalTo = Convert.ToDateTime(model.LabApprovalTo);
                obj.LabApprovalFr = DateTime.ParseExact(model.LabApprovalFr, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                obj.LabApprovalTo = DateTime.ParseExact(model.LabApprovalTo, "MM/dd/yyyy", CultureInfo.InvariantCulture);

                obj.LabContactPer = model.LabContactPer;
                obj.LabContactTel = model.LabContactTel;
                obj.LabEmail = model.LabEmail;
                //obj.CallRemarking = model.CallRemarking;
                //obj.Isdeleted = Convert.ToByte(false);
                obj.UserId = model.UserId;
                obj.Datetime = DateTime.Now;
                context.T65LaboratoryMasters.Add(obj);
                context.SaveChanges();
                Id = Convert.ToInt32(obj.LabId);
            }
            else
            {

                // Lab.LabId = Convert.ToByte(model.LabId);
                Lab.LabName = model.LabName;
                Lab.LabAddress = model.LabAddress;
                Lab.LabCity = Convert.ToInt32(model.LabCity);
                Lab.LabApproval = Convert.ToString(model.LabApproval);
                Lab.LabApprovalFr = Convert.ToDateTime(model.LabApprovalFr);
                Lab.LabApprovalTo = Convert.ToDateTime(model.LabApprovalTo);
                Lab.LabContactPer = model.LabContactPer;
                Lab.LabContactTel = model.LabContactTel;
                Lab.LabEmail = model.LabEmail;
                //obj.CallRemarking = model.CallRemarking;
                //obj.Isdeleted = Convert.ToByte(false);
                //obj.Createdby = model.UserId;
                //obj.Createddate = DateTime.Now;
                //context.T65LaboratoryMasters.Add(Lab);
                context.SaveChanges();
                Id = Convert.ToInt32(Lab.LabId);
            }
            #endregion
            return Id;
        }

    }

}
