using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;

namespace IBS.Repositories
{
    public class ICBooksetFormRepository : I_ICBooksetFormRepository
    {
        private readonly ModelContext context;

        public ICBooksetFormRepository(ModelContext context)
        {
            this.context = context;
        }

        public IC_Bookset_FormModel FindByID(int Id)
        {
            IC_Bookset_FormModel model = new();
            T10IcBookset bookset = context.T10IcBooksets.Find(Id);

            if (bookset == null)
                return model;
            else
            {
                model.Id = bookset.Id;
                model.BkNo = bookset.BkNo;
                model.SetNoFr = bookset.SetNoFr;
                model.SetNoTo = bookset.SetNoTo;
                model.IssueDt = bookset.IssueDt;
                model.IssueToIecd = bookset.IssueToIecd;
                model.BkSubmitted = bookset.BkSubmitted;
                model.BkSubmitDt = bookset.BkSubmitDt;
                model.CutOffDt = bookset.CutOffDt;
                model.CutOffSet = bookset.CutOffSet;
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

            if (dtParameters.Order != null && dtParameters.Order.Length > 0)
            {
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                if (string.IsNullOrEmpty(orderCriteria)) orderCriteria = "IeName";
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                orderCriteria = "IeName";
                orderAscendingDirection = true;
            }

            string BookNo = !string.IsNullOrEmpty(dtParameters.AdditionalValues["BookNo"]) ? Convert.ToString(dtParameters.AdditionalValues["BookNo"]) : "";
            string SetNoFrom = !string.IsNullOrEmpty(dtParameters.AdditionalValues["SetNoFrom"]) ? Convert.ToString(dtParameters.AdditionalValues["SetNoFrom"]) : "";
            int? IssueToIecd = !string.IsNullOrEmpty(dtParameters.AdditionalValues["IssueToIecd"]) ? Convert.ToInt32(dtParameters.AdditionalValues["IssueToIecd"]) : null;

            query = from t10 in context.T10IcBooksets
                    join t09 in context.T09Ies on t10.IssueToIecd equals t09.IeCd
                    where t10.Isdeleted != 1
                     && (!string.IsNullOrEmpty(BookNo) ? t10.BkNo.ToLower().Contains(BookNo.ToLower()) : true)
                     && (!string.IsNullOrEmpty(SetNoFrom) ? t10.SetNoFr == SetNoFrom : true)
                      && ((IssueToIecd != null) ? t10.IssueToIecd == IssueToIecd : true)
                    select new IC_Bookset_FormModel
                    {
                        Id = t10.Id,
                        BkNo = t10.BkNo,
                        SetNoFr = t10.SetNoFr,
                        SetNoTo = t10.SetNoTo,
                        IssueDt = t10.IssueDt,
                        IeName = t09.IeName,
                        BkSubmitted = EnumUtility<Enums.BookSubmitted>.GetDescriptionByKey(t10.BkSubmitted),
                        BkSubmitDt = t10.BkSubmitDt,
                        Region = EnumUtility<Enums.Region>.GetDescriptionByKey(t10.Region),
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => (w.BkNo != null && w.BkNo.ToLower().Contains(searchBy.ToLower()))
                    || (w.SetNoFr != null && w.SetNoFr.ToLower().Contains(searchBy.ToLower()))
            );

            dTResult.recordsFiltered = query.Count();

            if (dtParameters.Length == -1) dtParameters.Length = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public string IsExists(IC_Bookset_FormModel model)
        {
            string returnMsg = string.Empty;

            if (model.Id == 0)
            {
                var query = from t10 in context.T10IcBooksets
                            where t10.BkNo.Trim() == model.BkNo.ToUpper()
                            && ((Convert.ToInt32(model.SetNoFr) >= Convert.ToInt32(t10.SetNoFr) && Convert.ToInt32(model.SetNoFr) <= Convert.ToInt32(t10.SetNoTo))
                                || (Convert.ToInt32(model.SetNoTo) >= Convert.ToInt32(t10.SetNoFr) && Convert.ToInt32(model.SetNoTo) <= Convert.ToInt32(t10.SetNoTo))
                                || (Convert.ToInt32(model.SetNoFr) < Convert.ToInt32(t10.SetNoFr) && Convert.ToInt32(model.SetNoTo) > Convert.ToInt32(t10.SetNoTo)))
                            && t10.Region == model.Region
                            select t10;

                int count = query.Count();

                if (count > 0)
                {
                    returnMsg = "Range of Entered Set_No_From to Set_No_To Already Present in Database!!";
                }
                else
                {
                    var ieStatus = context.T09Ies.Where(x => x.IeCd == model.IssueToIecd).Select(x => x.IeStatus ?? "W").FirstOrDefault();

                    if (ieStatus.ToUpper() != "W")
                    {
                        returnMsg = "You Cannot Issue a New Book and Set To a Retired or Left IE!!!";
                    }
                }
            }
            else
            {
                var query = from t10 in context.T10IcBooksets
                            where t10.BkNo.Trim() == model.BkNo.ToUpper() && t10.SetNoFr != model._SetNoFr && t10.SetNoTo != model._SetNoTo
                                && ((Convert.ToInt32(model.SetNoFr) >= Convert.ToInt32(t10.SetNoFr) && Convert.ToInt32(model.SetNoFr) <= Convert.ToInt32(t10.SetNoTo))
                                    || (Convert.ToInt32(model.SetNoTo) >= Convert.ToInt32(t10.SetNoFr) && Convert.ToInt32(model.SetNoTo) <= Convert.ToInt32(t10.SetNoTo))
                                    || (Convert.ToInt32(model.SetNoFr) < Convert.ToInt32(t10.SetNoFr) && Convert.ToInt32(model.SetNoTo) > Convert.ToInt32(t10.SetNoTo)))
                               && t10.Region == model.Region
                            select t10;

                int count = query.Count();

                if (count > 0)
                {
                    returnMsg = "Range of Entered Set_No_From to Set_No_To Already Present in Database!!";
                }
                else
                {
                    var query1 = from t20 in context.T20Ics
                                 where t20.BkNo.Trim() == model.BkNo.ToUpper() && t20.SetNo.CompareTo(model._SetNoFr) >= 0 && t20.SetNo.CompareTo(model._SetNoTo) <= 0
                                    && !(t20.SetNo.CompareTo(model.SetNoFr) >= 0 && t20.SetNo.CompareTo(model.SetNoTo) <= 0)
                                    && t20.CaseNo.Substring(0, 1) == model.Region
                                 select t20;

                    count = query1.Count();

                    if (count > 0)
                    {
                        returnMsg = "The Inspection Certificate is already been used in the range you are modifying!!";
                    }
                    else
                    {
                        if (model.CutOffDt != null && !string.IsNullOrEmpty(model.CutOffSet))
                        {
                            int _count1 = 0;
                            int _count2 = 0;

                            var query2 = from t20 in context.T20Ics
                                         where t20.BkNo.Trim() == model.BkNo.ToUpper() && t20.SetNo.CompareTo(model.CutOffSet) > 0 && t20.IeCd == model.IssueToIecd && t20.IcDt <= model.CutOffDt
                                         select t20;

                            _count1 = query2.Count();

                            var query3 = from t16 in context.T16IcCancels
                                         where t16.BkNo.Trim() == model.BkNo.ToUpper() && t16.SetNo.CompareTo(model.CutOffSet) > 0 && t16.IssueToIecd == model.IssueToIecd && t16.StatusDt <= model.CutOffDt
                                         select t16;
                            _count2 = query.Count();

                            if (_count1 > 0 && _count2 > 0)
                            {
                                returnMsg = "The Last set no. entered is invalid as Book-Sets after this have been Issued / Cancelled before the Cut off Date.";
                            }
                            else if (_count1 > 0)
                            {
                                returnMsg = "The Last set no. entered is invalid as Book-Set after this have been Issued before the Cut off Date.";
                            }
                            else if (_count2 > 0)
                            {
                                returnMsg = "The Last set no. entered is invalid as Book-Set after this have been Cancelled before the Cut off Date.";
                            }
                        }
                    }
                }
            }

            return returnMsg;
        }

        public string SaveDetails(IC_Bookset_FormModel model)
        {
            if (model.Id == 0)
            {
                T10IcBookset bookSet = new()
                {
                    BkNo = model.BkNo.ToUpper(),
                    SetNoFr = model.SetNoFr,
                    SetNoTo = model.SetNoTo,
                    IssueDt = model.IssueDt,
                    IssueToIecd = model.IssueToIecd,
                    BkSubmitted = model.BkSubmitted,
                    BkSubmitDt = model.BkSubmitDt,
                    Region = model.Region,
                    CutOffDt = model.CutOffDt,
                    CutOffSet = model.CutOffSet,
                    UserId = model.UserId,
                    Datetime = DateTime.Now.Date,
                    Createdby = model.Createdby,
                    Createddate = DateTime.Now,
                };

                context.T10IcBooksets.Add(bookSet);
                context.SaveChanges();
            }
            else
            {
                T10IcBookset bookSet = context.T10IcBooksets.Find(model.Id);

                if (bookSet != null)
                {
                    bookSet.SetNoFr = model.SetNoFr;
                    bookSet.SetNoTo = model.SetNoTo;
                    bookSet.IssueDt = model.IssueDt;
                    bookSet.IssueToIecd = model.IssueToIecd;
                    bookSet.BkSubmitted = model.BkSubmitted;
                    bookSet.BkSubmitDt = model.BkSubmitDt;
                    bookSet.CutOffDt = model.CutOffDt;
                    bookSet.CutOffDt = model.CutOffDt;
                    bookSet.UserId = model.UserId;
                    bookSet.Datetime = DateTime.Now.Date;
                    bookSet.Updatedby = model.Updatedby;
                    bookSet.Updateddate = DateTime.Now;

                    context.SaveChanges();
                }
            }

            return model.BkNo;
        }

        public bool Remove(int Id)
        {
            if (context.T10IcBooksets.Any(x => x.Id == Id))
            {
                context.T10IcBooksets.RemoveRange(context.T10IcBooksets.Where(x => x.Id == Id).ToList());
                context.SaveChanges();
            }
            return true;
        }
    }

}

