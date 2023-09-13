using Humanizer;
using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace IBS.Repositories
{
    public class HologramSearchForm : IHologramSearchForm
    {
        private readonly ModelContext context;

        public HologramSearchForm(ModelContext context)
        {
            this.context = context;
        }
        public HologramSearchFormModel FindByID(string HgNoFr, string HgNoTo, string Region)
        {
            HologramSearchFormModel model = new();
            T31HologramIssued role = context.T31HologramIssueds
                                    .Where(x => x.HgNoFr == HgNoFr && x.HgNoTo == HgNoTo && x.HgRegion == Region)
                                    .Select(x => x).FirstOrDefault();


            if (role == null)
                throw new Exception("Role Record Not found");
            else
            {
                var status = context.T09Ies
                            .Where(x => x.IeCd == role.HgIecd)
                            .Select(x => x.IeStatus).First();

                model.HgNoFr = role.HgNoFr;
                model.lblHgNoFr = role.HgNoFr;
                model.HgNoTo = role.HgNoTo;
                model.lblHgNoTo = role.HgNoTo;
                model.HgIssueDt = role.HgIssueDt;
                model.HgIecd = role.HgIecd;
                model.UserId = role.UserId;
                model.Updatedby = role.Updatedby;
                model.Createdby = role.Createdby;
                model.Createddate = model.Createddate;
                model.IEStatus = status;
                return model;
            }
        }

        public DTResult<HologramSearchFormModel> GetHologramSearchFormList(DTParameters dtParameters, string region)
        {

            DTResult<HologramSearchFormModel> dTResult = new() { draw = 0 };
            IQueryable<HologramSearchFormModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "HgNoFr";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "HgNoFr";
                orderAscendingDirection = true;
            }
            query = from b in context.T31HologramIssueds
                    join i in context.T09Ies on b.HgIecd equals i.IeCd
                    where b.HgRegion == region && (b.Isdeleted == Convert.ToByte(0) || b.Isdeleted == null)
                    select new HologramSearchFormModel
                    {
                        HgNoFr = b.HgNoFr,//b.HgRegion + b.HgNoFr,
                        HgNoTo = b.HgNoTo,//b.HgRegion + b.HgNoTo,
                        HgIssueDt = b.HgIssueDt,
                        HgIecd = b.HgIecd,
                        HgIeName = i.IeName,
                        HgRegion = i.IeRegion == "N" ? "Northern" :
                                 i.IeRegion == "W" ? "Western" :
                                 i.IeRegion == "E" ? "Eastern" :
                                 i.IeRegion == "S" ? "Southern" :
                                 i.IeRegion == "C" ? "Central" : ""
                    };

            // Apply filters based on user input
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["HFromNo"]))
            {
                query = query.Where(item => item.HgNoFr.Trim() == dtParameters.AdditionalValues["HFromNo"].Trim()).OrderBy(x => x.HgNoFr);
            }

            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["HToNo"]))
            {
                query = query.Where(item => item.HgNoTo.Trim() == dtParameters.AdditionalValues["HToNo"].Trim()).OrderBy(x => x.HgNoFr);
            }

            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["IE"]))
            {
                query = query.Where(item => item.HgIecd == Convert.ToInt32(dtParameters.AdditionalValues["IE"].Trim())).OrderBy(x => x.HgNoFr);
            }

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.HgNoFr).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.HgNoTo).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }
        public bool Remove(HologramSearchFormModel model)
        {
            var roles = context.T31HologramIssueds
                        .Where(x => x.HgNoFr == model.HgNoFr && x.HgNoTo == model.HgNoTo && x.HgRegion == model.HgRegion)
                        .Select(x => x).FirstOrDefault();

            if (roles == null) { return false; }

            roles.Isdeleted = 1;
            roles.Updatedby = model.Updatedby;
            roles.Updateddate = DateTime.Now;
            context.SaveChanges();
            return true;
        }

        public int SaveDetails(HologramSearchFormModel model)
        {
            int res = 0;

            #region Role save
            // if (SOF == null || SOF.HgNoFr == 0)
            if (model.lblHgNoFr == null && model.lblHgNoTo == null)
            {
                T31HologramIssued obj = new T31HologramIssued();
                obj.HgRegion = model.HgRegion;
                obj.HgNoFr = model.HgNoFr;
                obj.HgNoTo = model.HgNoTo;
                obj.HgIssueDt = model.HgIssueDt;
                obj.HgIecd = model.HgIecd;
                obj.UserId = model.UserId;
                obj.Datetime = DateTime.Now;
                obj.Createdby = model.Createdby;
                obj.Createddate = DateTime.Now;
                context.T31HologramIssueds.Add(obj);
                context.SaveChanges();
                res = Convert.ToInt32(obj.HgNoFr);
            }
            else
            {
                var SOF = context.T31HologramIssueds
                        .Where(x => x.HgNoFr == model.lblHgNoFr && x.HgNoTo == model.lblHgNoTo && x.HgRegion == model.HgRegion)
                        .Select(x => x).FirstOrDefault();

                SOF.HgNoFr = model.HgNoFr;
                SOF.HgNoTo = model.HgNoTo;
                SOF.HgIssueDt = model.HgIssueDt;
                SOF.HgIecd = model.HgIecd;
                SOF.UserId = model.UserId;
                SOF.Datetime = DateTime.Now;
                SOF.Updatedby = model.Updatedby;
                SOF.Updateddate = DateTime.Now;
                context.SaveChanges();
                res = Convert.ToInt32(SOF.HgNoFr);
            }
            #endregion
            return res;
        }

        public int CheckDate(string IEDate)
        {
            int result = 0;
            
            using ModelContext context = new(DbContextHelper.GetDbContextOptions());
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                bool wasOpen = command.Connection.State == ConnectionState.Open;
                if (!wasOpen) command.Connection.Open();
                try
                {
                    command.CommandText = "SELECT sign(sysdate - TO_DATE('" + Convert.ToDateTime(IEDate).ToString("dd/MM/yyyy") + "','DD/MM/YYYY')) FROM dual";
                    result = Convert.ToInt32(command.ExecuteScalar());
                }
                finally
                {
                    if (!wasOpen) command.Connection.Close();
                }
            }
            return result;
        }

        public int CheckHologramNo(HologramSearchFormModel model)
        {
            int count = 0, sfr = 0, sto = 0;
            sfr = Convert.ToInt32(model.HgNoFr);
            sto = Convert.ToInt32(model.HgNoTo);
            if (model.lblHgNoFr == null && model.lblHgNoTo == null)
            {
                count = context.T31HologramIssueds
                        .Where(h =>
                            (sfr >= Convert.ToInt32(h.HgNoFr) && sfr <= Convert.ToInt32(h.HgNoTo)) ||
                            (sto >= Convert.ToInt32(h.HgNoFr) && sto <= Convert.ToInt32(h.HgNoTo)) ||
                            (sfr < Convert.ToInt32(h.HgNoFr) && sto > Convert.ToInt32(h.HgNoTo))
                        )
                        .Where(h => h.HgRegion == model.HgRegion)
                        .Select(x => x).Count();

            }
            else
            {
                count = context.T31HologramIssueds
                        .Where(h =>
                            h.HgNoFr.Trim() != model.lblHgNoFr &&
                            h.HgNoTo.Trim() != model.lblHgNoTo &&
                            ((sfr >= Convert.ToInt32(h.HgNoFr) && sfr <= Convert.ToInt32(h.HgNoTo)) ||
                            (sto >= Convert.ToInt32(h.HgNoFr) && sto <= Convert.ToInt32(h.HgNoTo)) ||
                            (sfr < Convert.ToInt32(h.HgNoFr) && sto > Convert.ToInt32(h.HgNoTo))) &&
                            h.HgRegion == model.HgRegion)
                        .Count();                
            }
            return count;
        }

        public string IEIssueOrNot(string IE)
        {
            var result = "";
            
            using ModelContext context = new(DbContextHelper.GetDbContextOptions());
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                bool wasOpen = command.Connection.State == ConnectionState.Open;
                if (!wasOpen) command.Connection.Open();
                try
                {
                    command.CommandText = "select NVL(IE_STATUS,'W') from T09_IE where IE_CD=" + IE;
                    result = Convert.ToString(command.ExecuteScalar());
                }
                finally
                {
                    if (!wasOpen) command.Connection.Close();
                }
            }

            return result;
        }

        public int CheckHologramCancel(HologramSearchFormModel model)
        {
            throw new NotImplementedException();
        }
        public int MatchHologram(string HgNoFr, string HgNoTo, string Region)
        {
            int count = 0;
            var result = "";            

            using ModelContext context = new(DbContextHelper.GetDbContextOptions());
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                bool wasOpen = command.Connection.State == ConnectionState.Open;
                if (!wasOpen) command.Connection.Open();
                try
                {
                    command.CommandText = "Select T31.HG_REGION FROM T31_HOLOGRAM_ISSUED T31,T09_IE T09 WHERE T31.HG_IECD=T09.IE_CD and trim(T31.HG_NO_FR) = '" + HgNoFr + "' and trim(T31.HG_NO_TO)= '" + HgNoTo + "'  and T31.HG_REGION='" + Region + "'";
                    result = Convert.ToString(command.ExecuteScalar());
                }
                finally
                {
                    if (!wasOpen) command.Connection.Close();
                }
            }
            if (result == "\0")
            {
                count = 0;
            }
            if (result == Region)
            {
                count = 2;
            }
            else
            {
                count = 1;
            }
            return count;
        }
    }
}