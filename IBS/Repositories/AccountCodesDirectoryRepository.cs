using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;

namespace IBS.Repositories
{
    public class AccountCodesDirectoryRepository : IAccountCodesDirectoryRepository
    {
        private readonly ModelContext context;

        public AccountCodesDirectoryRepository(ModelContext context)
        {
            this.context = context;
        }

        public AccountCodesDirectoryModel FindByID(int AccCd)
        {
            AccountCodesDirectoryModel model = new();
            T95AccountCode role = context.T95AccountCodes.Find(AccCd);

            if (role == null)
                return model;
            else
            {
                model.AccCd = role.AccCd;
                model.AccDesc = role.AccDesc;
                model.IsNew = false;
                return model;
            }
        }

        public DTResult<AccountCodesDirectoryModel> GetAccountCodesDirectoryList(DTParameters dtParameters)
        {

            DTResult<AccountCodesDirectoryModel> dTResult = new() { draw = 0 };
            IQueryable<AccountCodesDirectoryModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null && dtParameters.Order.Length > 0)
            {
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                if (string.IsNullOrEmpty(orderCriteria)) orderCriteria = "AccCd";
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                orderCriteria = "AccCd";
                orderAscendingDirection = true;
            }

            query = from l in context.T95AccountCodes
                    where l.Isdeleted == 0 || l.Isdeleted == null
                    select new AccountCodesDirectoryModel
                    {
                        AccCd = l.AccCd,
                        AccountCode = Convert.ToString(l.AccCd),
                        AccDesc = l.AccDesc,
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.AccountCode).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.AccDesc).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            if (dtParameters.Length == -1) dtParameters.Length = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public void SaveDetails(AccountCodesDirectoryModel model)
        {
            if (model.IsNew)
            {
                T95AccountCode accCode = new()
                {
                    AccCd =   model.AccCd ?? 0,
                    AccDesc = model.AccDesc,
                    Createdby = model.Createdby,
                    Createddate = DateTime.Now,
                };

                context.T95AccountCodes.Add(accCode);
                context.SaveChanges();
            }
            else
            {
                T95AccountCode accCode = context.T95AccountCodes.Find(model.AccCd);

                if (accCode != null)
                {
                    accCode.AccCd = model.AccCd ?? 0;
                    accCode.AccDesc = model.AccDesc;
                    accCode.Updatedby = model.Updatedby;
                    accCode.Updateddate = DateTime.Now;

                    context.SaveChanges();
                }
            }
        }

        public bool IsDuplicate(AccountCodesDirectoryModel model)
        {
            if (model.IsNew)
            {
                return context.T95AccountCodes.Any(x => x.AccCd == model.AccCd);
            }
            else
            {
                return false;
            }
        }

        public bool Remove(int AccCd)
        {
            if (context.T95AccountCodes.Any(x => x.AccCd == AccCd))
            {
                context.T95AccountCodes.RemoveRange(context.T95AccountCodes.Where(x => x.AccCd == AccCd).ToList());
                context.SaveChanges();
            }
            return true;
        }

    }
}



