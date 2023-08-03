using IBS.DataAccess;
using IBS.Interfaces;
using Microsoft.EntityFrameworkCore;
using IBS.Models;

namespace IBS.Repositories
{
    public class Bill_Paying_Officer_Form : IBill_Paying_Officer_Form
    {
        private readonly ModelContext context;

        public Bill_Paying_Officer_Form(ModelContext context)
        {
            this.context = context;
        }
        public Bill_Paying_Officer_FormModel FindByID(int BpoCd)
        {
            Bill_Paying_Officer_FormModel model = new();
            T12BillPayingOfficer role = context.T12BillPayingOfficers.Find(Convert.ToByte(BpoCd));

            if (role == null)
                throw new Exception("Role Record Not found");
            else
            {
                model.BpoCd = role.BpoCd;
                model.BpoName = role.BpoName;
                model.BpoCityCd = role.BpoCityCd;
                model.GstinNo = role.GstinNo;
                model.UserId = role.UserId;
                model.Updatedby = role.Updatedby;
                model.Createdby = role.Createdby;
                model.Createddate = model.Createddate;
                model.Isdeleted = role.Isdeleted;
                return model;
            }
        }

        public DTResult<Bill_Paying_Officer_FormModel>GetBPOList(DTParameters dtParameters)
        {

            DTResult<Bill_Paying_Officer_FormModel> dTResult = new() { draw = 0 };
            IQueryable<Bill_Paying_Officer_FormModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "BpoCd";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "BpoCd";
                orderAscendingDirection = true;
            }
            query = from l in context.T12BillPayingOfficers
                    where l.Isdeleted == 0 || l.Isdeleted == null
                    select new Bill_Paying_Officer_FormModel
                    {
                        BpoCd = l.BpoCd,
                        BpoName = l.BpoName,
                        BpoCityCd = l.BpoCityCd,
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
                query = query.Where(w => Convert.ToString(w.BpoName).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.BpoPhone).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }
        public bool Remove(int BpoCd, int UserID)
        {
            var roles = context.T12BillPayingOfficers.Find(Convert.ToByte(BpoCd));
            if (roles == null) { return false; }

            roles.Isdeleted = Convert.ToByte(true);
            roles.Updatedby = Convert.ToInt32(UserID);
            roles.Updateddate = DateTime.Now;
            context.SaveChanges();
            return true;
        }

        public int BPODetailsInsertUpdate(Bill_Paying_Officer_FormModel model)
        {
            int RoleId = 0;
            var BPO = context.T12BillPayingOfficers.Where(x => x.BpoCd == model.BpoCd).FirstOrDefault();
            #region Role save
            //if (BPO == null || BPO.BpoCd == 0)
                if (BPO == null )

                {
                T12BillPayingOfficer obj = new T12BillPayingOfficer();

                obj.BpoName = model.BpoName;
                obj.BpoCityCd = model.BpoCityCd;
                obj.GstinNo = model.GstinNo;
                obj.Createdby = model.Createdby;
                obj.Isdeleted = Convert.ToByte(false);
                obj.Createddate = DateTime.Now;
                context.T12BillPayingOfficers.Add(obj);
                context.SaveChanges();
                RoleId = Convert.ToInt32(obj.BpoCd);
            }
            else
            {
                BPO.BpoName = model.BpoName;
                BPO.BpoCityCd = model.BpoCityCd;
                BPO.GstinNo = model.GstinNo;
                BPO.Updatedby = model.Updatedby;
                BPO.Updateddate = DateTime.Now;
                context.SaveChanges();
                RoleId = Convert.ToInt32(BPO.BpoCd);
            }
            #endregion
            return RoleId;
        }
    }

}