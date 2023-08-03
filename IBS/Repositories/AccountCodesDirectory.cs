using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace IBS.Repositories
{
    public class AccountCodesDirectory : IAccountCodesDirectory
    {
        private readonly ModelContext context;

        public AccountCodesDirectory(ModelContext context)
        {
            this.context = context;
        }
        public AccountCodesDirectoryModel FindByID(int AccCd)
        {
           AccountCodesDirectoryModel model = new();
            T95AccountCode role = context.T95AccountCodes.Find(AccCd);
           

            if (role == null)
                throw new Exception("Role Record Not found");
            else
            {
                model.AccCd = role.AccCd;
                model.AccDesc = role.AccDesc;
                //model.UserId = role.UserId;
                model.Updatedby = role.Updatedby;
                model.Createdby = role.Createdby;
                model.Createddate = model.Createddate;
                model.Isdeleted = role.Isdeleted;
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

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "AccCd";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "AccCd";
                orderAscendingDirection = true;
            }
            query = from l in context.T95AccountCodes
                    where l.Isdeleted == 0 || l.Isdeleted == null
                    select new AccountCodesDirectoryModel
                    {
                        AccCd = l.AccCd,
                        AccDesc = l.AccDesc,
                        //UserId = l.UserId,
                        //Isdeleted = l.Isdeleted,
                        //Createddate = l.Createddate,
                        //Createdby = l.Createdby,
                        //Updateddate = l.Updateddate,
                        //Updatedby = l.Updatedby
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.AccCd).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.AccDesc).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }
        public bool Remove(int AccCd)
        {
            var roles = context.T95AccountCodes.Find(AccCd);
            if (roles == null) { return false; }

            roles.Isdeleted = Convert.ToByte(true);
            //roles.Updatedby = Convert.ToInt32(UserID);
            roles.Updateddate = DateTime.Now;
            context.SaveChanges();
            return true;
        }

        public int AccountCodesDirectoryDetailsInsertUpdate(AccountCodesDirectoryModel model)
        {
            int RoleId = 0;
            var ACD = context.T95AccountCodes.Where(x => x.AccCd == model.AccCd).FirstOrDefault();
            #region Role save
            if (ACD == null || ACD.AccCd == 0)
            {
                int maxNo = (from u in context.T95AccountCodes
                             select u.AccCd).Max();
                T95AccountCode obj = new T95AccountCode();

                //obj.AccCd = maxNo + 1;
                obj.AccCd = model.AccCd;
                obj.AccDesc = model.AccDesc;
                obj.Createdby = model.Createdby;
                obj.Isdeleted = Convert.ToByte(false);
                obj.Createddate = DateTime.Now;
                context.T95AccountCodes.Add(obj);
                context.SaveChanges();
                RoleId = Convert.ToInt32(obj.AccCd);
            }
            else
            {
                ACD.AccDesc = model.AccDesc;
                ACD.Updatedby = model.Updatedby;
                ACD.Updateddate = DateTime.Now;
                context.SaveChanges();
                RoleId = Convert.ToInt32(ACD.AccCd);
            }
            #endregion
            return RoleId;
        }

        }
    }



