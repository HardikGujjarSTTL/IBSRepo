using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;

namespace IBS.Repositories
{
    public class IC_Bookset_Form : I_IC_Bookset_Form
    {
        private readonly ModelContext context;

        public IC_Bookset_Form(ModelContext context)
        {
            this.context = context;
        }
        public IC_Bookset_FormModel FindByID(int BkNo)
        {
            IC_Bookset_FormModel model = new();
            T10IcBookset role = context.T10IcBooksets.Find(Convert.ToByte(BkNo));

            if (role == null)
                throw new Exception("Role Record Not found");
            else
            {
                model.BkNo = role.BkNo;
                model.SetNoFr = role.SetNoFr;
                model.IssueToIecd = role.IssueToIecd;
                model.UserId = role.UserId;
                model.Updatedby = role.Updatedby;
                model.Createdby = role.Createdby;
                model.Createddate = model.Createddate;
                model.Isdeleted = role.Isdeleted;
                return model;
            }
        }

        public DTResult<IC_Bookset_FormModel> GetBooksetList(DTParameters dtParameters)
        {

            DTResult<IC_Bookset_FormModel> dTResult = new() { draw = 0 };
            IQueryable<IC_Bookset_FormModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "BkNo";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "BkNo";
                orderAscendingDirection = true;
            }
            query = from l in context.T10IcBooksets
                    where l.Isdeleted == 0 || l.Isdeleted == null
                    select new IC_Bookset_FormModel
                    {
                        BkNo = l.BkNo,
                        SetNoFr = l.SetNoFr,
                        IssueToIecd = l.IssueToIecd,
                        UserId = l.UserId,
                        Isdeleted = l.Isdeleted,
                        Createddate = l.Createddate,
                        Createdby = l.Createdby,
                        Updateddate = l.Updateddate,
                        Updatedby = l.Updatedby
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.SetNoFr).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.IssueToIecd).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }
        public bool Remove(int IeCd, int UserID)
        {
            var roles = context.T10IcBooksets.Find(Convert.ToByte(IeCd));
            if (roles == null) { return false; }

            roles.Isdeleted = Convert.ToByte(true);
            roles.Updatedby = Convert.ToInt32(UserID);
            roles.Updateddate = DateTime.Now;
            context.SaveChanges();
            return true;
        }

        public int BooksetDetailsInsertUpdate(IC_Bookset_FormModel model)
        {
            int RoleId = 0;
            var Bookset = context.T10IcBooksets.Where(x => x.BkNo == model.BkNo).FirstOrDefault();
            #region Role save
            //if (Bookset == null || Bookset.BkNo == 0)
            if (Bookset == null) 
            {
                T10IcBookset obj = new T10IcBookset();

                obj.SetNoFr = model.SetNoFr;
                obj.IssueToIecd = model.IssueToIecd;
                obj.Createdby = model.Createdby;
                obj.Isdeleted = Convert.ToByte(false);
                obj.Createddate = DateTime.Now;
                context.T10IcBooksets.Add(obj);
                context.SaveChanges();
                RoleId = Convert.ToInt32(obj.BkNo);
            }
            else
            {
                Bookset.SetNoFr = model.SetNoTo;
                Bookset.IssueToIecd = model.IssueToIecd;
                 Bookset.Updatedby = model.Updatedby;
                Bookset.Updateddate = DateTime.Now;
                context.SaveChanges();
                RoleId = Convert.ToInt32(Bookset.BkNo);
            }
            #endregion
            return RoleId;
        }
    }

}

