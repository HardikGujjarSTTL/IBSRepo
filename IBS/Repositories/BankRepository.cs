using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace IBS.Repositories
{
    public class BankRepository : IBankRepository
    {
        private readonly ModelContext context;

        public BankRepository(ModelContext context)
        {
            this.context = context;
        }

        public BankMasterModel FindByID(int Id)
        {
            BankMasterModel model = new();
            T94Bank bank = context.T94Banks.Find(Id);

            if (bank == null)
                return model;
            else
            {
                model.BankCd = bank.BankCd;
                model.BankName = bank.BankName;
                model.FmisBankCd = bank.FmisBankCd;
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

            if (dtParameters.Order != null && dtParameters.Order.Length > 0)
            {
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                if (string.IsNullOrEmpty(orderCriteria)) orderCriteria = "BankCd";
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                orderCriteria = "BankCd";
                orderAscendingDirection = true;
            }

            string BankName = !string.IsNullOrEmpty(dtParameters.AdditionalValues["BankName"]) ? Convert.ToString(dtParameters.AdditionalValues["BankName"]) : "";
            int? FMISBankCD = !string.IsNullOrEmpty(dtParameters.AdditionalValues["FMISBankCD"]) ? Convert.ToInt32(dtParameters.AdditionalValues["FMISBankCD"]) : null;

            query = from t94 in context.T94Banks
                    where t94.Isdeleted != 1
                     && (!string.IsNullOrEmpty(BankName) ? t94.BankName.ToLower().Contains(BankName.ToLower()) : true)
                     && ((FMISBankCD != null) ? t94.FmisBankCd == FMISBankCD : true)
                    select new BankMasterModel
                    {   
                        BankCd = t94.BankCd,
                        BankName = t94.BankName,
                        FmisBankCd = t94.FmisBankCd
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.BankName).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            if (dtParameters.Length == -1) dtParameters.Length = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public int SaveDetails(BankMasterModel model)
        {
            if (model.BankCd == 0)
            {
                int BankCd = GetMaxBankCd();
                BankCd += 1;

                T94Bank bank = new()
                {
                    BankCd = BankCd,
                    BankName = model.BankName,
                    FmisBankCd = model.FmisBankCd,
                    UserId = model.UserId,
                    Datetime = DateTime.Now.Date,
                    Createdby = model.Createdby,
                    Createddate = DateTime.Now,
                };

                context.T94Banks.Add(bank);
                context.SaveChanges();
            }
            else
            {
                T94Bank bank = context.T94Banks.Find(model.BankCd);

                if (bank != null)
                {
                    bank.BankName = model.BankName;
                    bank.FmisBankCd = model.FmisBankCd;
                    bank.UserId = model.UserId;
                    bank.Datetime = DateTime.Now.Date;
                    bank.Updatedby = model.Updatedby;
                    bank.Updateddate = DateTime.Now;

                    context.SaveChanges();
                }
            }

            return model.BankCd;
        }

        public int GetMaxBankCd()
        {
            int BankCd = 0;

            using ModelContext context = new(DbContextHelper.GetDbContextOptions());
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                bool wasOpen = command.Connection.State == ConnectionState.Open;
                if (!wasOpen) command.Connection.Open();
                try
                {
                    command.CommandText = "SELECT NVL(MAX(BANK_CD), 0) FROM T94_BANK WHERE BANK_CD < 990";
                    BankCd = Convert.ToInt32(command.ExecuteScalar());
                }
                finally
                {
                    if (!wasOpen) command.Connection.Close();
                }
            }
            return BankCd;
        }

        public bool Remove(int Id, int UserID)
        {
            T94Bank bank = context.T94Banks.Find(Id);

            if (bank == null) { return false; }

            bank.Isdeleted = 1;
            bank.Updatedby = UserID;
            bank.Updateddate = DateTime.Now;

            context.SaveChanges();
            return true;
        }

    }

}

