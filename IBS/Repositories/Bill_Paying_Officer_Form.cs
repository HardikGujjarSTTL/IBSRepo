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

        public Bill_Paying_Officer_FormModel FindByID(string BpoCd)
        {
            Bill_Paying_Officer_FormModel model = new();
            T12BillPayingOfficer BPO = context.T12BillPayingOfficers.Where(x=>x.BpoCd == BpoCd).FirstOrDefault();

            if (BPO == null)
                return model;
            else
            {
                model.BpoCd = BPO.BpoCd;
                model.BpoName = BPO.BpoName;
                model.BpoCityCd = Convert.ToInt32(BPO.BpoCityCd);
                model.GstinNo = BPO.GstinNo;
                model.UserId = BPO.UserId;
                model.Updatedby = BPO.Updatedby;
                model.Createdby = BPO.Createdby;
                model.Createddate = model.Createddate;
                model.Isdeleted = BPO.Isdeleted;
                return model;
            }
        }

        public DTResult<Bill_Paying_Officer_FormModel> GetBPOList(DTParameters dtParameters)
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
                    orderCriteria = "BpoName";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "BpoName";
                orderAscendingDirection = true;
            }

            string BpoCd = "", BpoName = "", BpoRly = "", BpoCity = "", SapCustCdBpo = "", GstinNo = "";
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["BpoCd"]))
            {
                BpoCd = Convert.ToString(dtParameters.AdditionalValues["BpoCd"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["BpoName"]))
            {
                BpoName = Convert.ToString(dtParameters.AdditionalValues["BpoName"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["BpoRly"]))
            {
                BpoRly = Convert.ToString(dtParameters.AdditionalValues["BpoRly"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["BpoCity"]))
            {
                BpoCity = Convert.ToString(dtParameters.AdditionalValues["BpoCity"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["SapCustCdBpo"]))
            {
                SapCustCdBpo = Convert.ToString(dtParameters.AdditionalValues["SapCustCdBpo"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["GstinNo"]))
            {
                GstinNo = Convert.ToString(dtParameters.AdditionalValues["GstinNo"]);
            }

            BpoCd = BpoCd.ToString() == "" ? string.Empty : BpoCd.ToString();
            BpoName = BpoName.ToString() == "" ? string.Empty : BpoName.ToString();
            BpoRly = BpoRly.ToString() == "" ? string.Empty : BpoRly.ToString();
            BpoCity = BpoCity.ToString() == "" ? string.Empty : BpoCity.ToString();
            SapCustCdBpo = SapCustCdBpo.ToString() == "" ? string.Empty : SapCustCdBpo.ToString();
            GstinNo = GstinNo.ToString() == "" ? string.Empty : GstinNo.ToString();

            //query = from t12 in context.T12BillPayingOfficers
            //        join t03 in context.T03Cities on t12.BpoCityCd equals t03.CityCd into cityGroup
            //        from city in cityGroup.DefaultIfEmpty()
            //        join a in context.AuCris on t12.Au equals a.Au into auGroup
            //        from au in auGroup.DefaultIfEmpty()
            //        where (string.IsNullOrEmpty(BpoCd) || t12.BpoCd == BpoCd) &&
            //              (string.IsNullOrEmpty(BpoName) || t12.BpoName.ToUpper().StartsWith(BpoName.ToUpper())) &&
            //              (string.IsNullOrEmpty(BpoRly) || t12.BpoRly.ToUpper() == BpoRly.Trim().ToUpper()) &&
            //              (string.IsNullOrEmpty(BpoCity) || (city != null && city.City.ToUpper().StartsWith(BpoCity.Trim().ToUpper()))) &&
            //              (string.IsNullOrEmpty(SapCustCdBpo) || t12.SapCustCdBpo == SapCustCdBpo.Trim()) &&
            //              (string.IsNullOrEmpty(GstinNo) || t12.GstinNo.ToUpper() == GstinNo.Trim().ToUpper())
            //        orderby t12.BpoName, t12.BpoRly,
            //                (t12.BpoAdd + "," + (city != null ? city.Location + " : " + city.City : city.City))
            //        select new Bill_Paying_Officer_FormModel
            //        {
            //            BpoCd = t12.BpoCd,
            //            BpoName =t12.BpoName,
            //            BpoRly = t12.BpoRly,
            //            BpoAdd = t12.BpoAdd + "," + (city != null ? city.Location + " : " + city.City : city.City),
            //            Au = au != null ? au.Au + "-" + au.Audesc : null,
            //            BpoCityCd = t12.BpoCityCd,
            //            GstinNo = t12.GstinNo,
            //        };

            query = from l in context.ViewGetBpodetails
                    where (string.IsNullOrEmpty(BpoCd) || l.BpoCd == BpoCd)
                    && (string.IsNullOrEmpty(BpoName) || l.BpoName == BpoName)
                    && (string.IsNullOrEmpty(BpoRly) || l.BpoRly == BpoRly)
                    && (string.IsNullOrEmpty(BpoCity) || l.City.Equals(BpoCity))
                    && (string.IsNullOrEmpty(SapCustCdBpo) || l.SapCustCdBpo == SapCustCdBpo)
                    && (string.IsNullOrEmpty(GstinNo) || l.GstinNo == GstinNo)

                    select new Bill_Paying_Officer_FormModel
                    {
                        BpoCd = l.BpoCd,
                        BpoName = l.BpoName,
                        BpoRly = l.BpoRly,
                        BpoAdd = l.BpoAdd,
                        Au = l.Audesc,
                        BpoCity = l.City,
                        GstinNo = l.GstinNo
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.BpoCd).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.BpoName).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.BpoRly).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.BpoAdd).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.Au).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.GstinNo).ToLower().Contains(searchBy.ToLower())
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
            if (BPO == null)

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