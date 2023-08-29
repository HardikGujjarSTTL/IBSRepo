using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace IBS.Repositories
{
    public class BankMaster : IBankMaster
    {
        private readonly ModelContext context;

        public BankMaster(ModelContext context)
        {
            this.context = context;
        }
        public BankMasterModel FindByID(int BankCd)
        {
            BankMasterModel model = new();
            T94Bank role = context.T94Banks.Find(BankCd);

            if (role == null)
                throw new Exception("Role Record Not found");
            else
            {
                model.BankCd = role.BankCd;
                model.BankName = role.BankName;
                model.FmisBankCd = (byte)role.FmisBankCd;
                model.UserId = role.UserId;
                model.Updatedby = role.Updatedby;
                model.Createdby = role.Createdby;
                model.Createddate = model.Createddate;
                model.Isdeleted = role.Isdeleted;
                return model;
            }
        }

        public DTResult<BankMasterModel> GetBankMasterList(DTParameters dtParameters)
        {

            DTResult<BankMasterModel> dTResult = new() { draw = 0 };
            IQueryable<BankMasterModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "BankCd";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "BankCd";
                orderAscendingDirection = true;
            }
            query = from l in context.T94Banks
                    where l.Isdeleted == 0 || l.Isdeleted == null
                    select new BankMasterModel
                    {
                        BankCd = l.BankCd,
                        BankName = l.BankName,
                        FmisBankCd = (byte)l.FmisBankCd,
                        UserId = l.UserId,
                        //Isdeleted = l.Isdeleted,
                        //Createddate = l.Createddate,
                        //Createdby = l.Createdby,
                        //Updateddate = l.Updateddate,
                        //Updatedby = l.Updatedby
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.BankName).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.FmisBankCd).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }
        public bool Remove(int BankCd, int UserID)
        {
            var roles = context.T94Banks.Find(BankCd);
            if (roles == null) { return false; }

            roles.Isdeleted = Convert.ToByte(true);
            roles.Updatedby = Convert.ToInt32(UserID);
            roles.Updateddate = DateTime.Now;
            context.SaveChanges();
            return true;
        }

        public int BankMasterDetailsInsertUpdate(BankMasterModel model)
        {
            int RoleId = 0;
            var BM = context.T94Banks.Where(x => x.BankCd == model.BankCd).FirstOrDefault();
            #region Role save
            if (BM == null || BM.BankCd == 0)
            {
                int maxNo = (from u in context.T94Banks
                             select u.BankCd).Max();
                T94Bank obj = new T94Bank();

                obj.BankCd = maxNo + 1;
                obj.BankName = model.BankName;
                obj.FmisBankCd = model.FmisBankCd;
                obj.Createdby = model.Createdby;
                obj.Isdeleted = Convert.ToByte(false);
                obj.Createddate = DateTime.Now;
                context.T94Banks.Add(obj);
                context.SaveChanges();
                RoleId = Convert.ToInt32(obj.BankCd);
            }
            else
            {
                BM.BankName = model.BankName;
                BM.FmisBankCd = model.FmisBankCd;
                BM.Updatedby = model.Updatedby;
                BM.Updateddate = DateTime.Now;
                context.SaveChanges();
                RoleId = Convert.ToInt32(BM.BankCd);
            }
            #endregion
            return RoleId;
        }

    }

}

