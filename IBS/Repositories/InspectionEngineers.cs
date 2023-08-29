using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Connections.Features;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Xml.Linq;

namespace IBS.Repositories
{
    public class InspectionEngineers : IInspectionEngineers
    {
        private readonly ModelContext context;

        public InspectionEngineers(ModelContext context)
        {
            this.context = context;
        }
        public InspectionEngineersModel FindByID(int IeCd)
        {
            InspectionEngineersModel model = new();

            model = (from v in context.T09Ies
                     join c in context.T03Cities on v.IeCityCd equals c.CityCd into cityJoin
                     from c in cityJoin.DefaultIfEmpty()
                     where v.IeCd == Convert.ToInt32(IeCd)
                     select new InspectionEngineersModel
                     {
                         IeCd = v.IeCd,
                         IeName = v.IeName,
                         IeSname = v.IeSname,
                         IeEmpNo = v.IeEmpNo,
                         IeSealNo = v.IeSealNo,
                         IeCoCd = Convert.ToByte(v.IeCoCd),
                         IeRegion = v.IeRegion
                     }).FirstOrDefault();
            return model;
        }

        public InspectionEngineersModel FindManageByID(int IeCd, string ActionType, string GetRegionCode)
        {
            InspectionEngineersModel model = new();
            if(ActionType == "M")
            {
                model = (from t09 in context.T09Ies
                         where t09.IeCd == Convert.ToInt32(IeCd)
                         select new InspectionEngineersModel
                         {
                             IeCd = t09.IeCd,
                             IeName = t09.IeName,
                             IeSname = t09.IeSname,
                             IeEmpNo = t09.IeEmpNo,
                             IeDesig = t09.IeDesig,
                             IeSealNo = t09.IeSealNo,
                             IeDepartment = t09.IeDepartment,
                             IeCityCd = Convert.ToString(t09.IeCityCd),
                             IeCityId = Convert.ToInt32(t09.IeCityCd),
                             IePhoneNo = t09.IePhoneNo,
                             IeCoCd = Convert.ToByte(t09.IeCoCd),
                             IeJoinDt = t09.IeJoinDt,
                             IeStatus = t09.IeStatus,
                             IeStatusDt = t09.IeStatusDt,
                             IeType = t09.IeType,
                             IeRegion = t09.IeRegion,
                             IePwd = t09.IePwd,
                             IeEmail = t09.IeEmail,
                             IeDob = t09.IeDob,
                             AltIe = t09.AltIe,
                             IeCallMarking = t09.IeCallMarking,
                             AltIeTwo = t09.AltIeTwo,
                             AltIeThree = t09.AltIeThree,
                             CallMarkingStoppingDt = t09.CallMarkingStoppingDt,

                         }).FirstOrDefault();
            }
            //else
            //{
            //    model = (from v in context.T09Ies
            //         join c in context.T03Cities on v.IeCityCd equals c.CityCd into cityJoin
            //         from c in cityJoin.DefaultIfEmpty()
            //         where v.IeCd == Convert.ToInt32(IeCd)
            //         select new InspectionEngineersModel
            //         {
            //             IeCd = v.IeCd,
            //             IeName = v.IeName,
            //             IeSname = v.IeSname,
            //             IeEmpNo = v.IeEmpNo,
            //             IeSealNo = v.IeSealNo,
            //             IeCoCd = Convert.ToByte(v.IeCoCd),
            //             IeRegion = v.IeRegion
            //         }).FirstOrDefault();
            //}
            model.IeRegion = GetRegionCode;
            return model;
        }

        public DTResult<InspectionEngineersModel> GetInspectionEngineersList(DTParameters dtParameters)
        {

            DTResult<InspectionEngineersModel> dTResult = new() { draw = 0 };
            IQueryable<InspectionEngineersModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "IeName";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "IeName";
                orderAscendingDirection = true;
            }

            string IeCd = "", IeSname = "", IeName = "", IeCoCd = "";

            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["IeCd"]))
            {
                IeCd = Convert.ToString(dtParameters.AdditionalValues["IeCd"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["IeSname"]))
            {
                IeSname = Convert.ToString(dtParameters.AdditionalValues["IeSname"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["IeName"]))
            {
                IeName = Convert.ToString(dtParameters.AdditionalValues["IeName"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["IeCoCd"]))
            {
                IeCoCd = Convert.ToString(dtParameters.AdditionalValues["IeCoCd"]);
            }

            IeCd = IeCd.ToString() == "" ? string.Empty : IeCd.ToString();
            IeSname = IeSname.ToString() == "" ? string.Empty : IeSname.ToString();
            IeName = IeName.ToString() == "" ? string.Empty : IeName.ToString();
            IeCoCd = IeCoCd.ToString() == "" ? string.Empty : IeCoCd.ToString();

            query = from v in context.T09Ies
                    join c in context.T03Cities on v.IeCityCd equals c.CityCd into cityJoin
                    from c in cityJoin.DefaultIfEmpty()
                    where (string.IsNullOrEmpty(IeCd) || v.IeCd == Convert.ToInt32(IeCd))
                        && (string.IsNullOrEmpty(IeName) || v.IeName.Equals(IeName))
                        && (string.IsNullOrEmpty(IeSname) || v.IeSname.Equals(IeSname))
                        && (string.IsNullOrEmpty(IeCoCd) || v.IeCoCd == Convert.ToInt32(IeCoCd))

                    select new InspectionEngineersModel
                    {
                        IeCd = v.IeCd,
                        IeName = v.IeName,
                        IeSname = v.IeSname,
                        IeEmpNo = v.IeEmpNo,
                        IeSealNo = v.IeSealNo,
                        IeCityCd = c.Location != null ? c.Location + " : " + c.City : c.City,
                        IeRegion = v.IeRegion
                    };



            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.IeCd).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.IeName).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public bool Remove(int IeCd, int UserID)
        {
            var roles = context.T09Ies.Find(Convert.ToByte(IeCd));
            if (roles == null) { return false; }

            roles.Isdeleted = Convert.ToByte(true);
            roles.Updatedby = Convert.ToInt32(UserID);
            roles.Updateddate = DateTime.Now;
            context.SaveChanges();
            return true;
        }

        public string DetailsInsertUpdate(InspectionEngineersModel model)
        {
            string status="";
            int code = new int();
            if(model.IeCd == null || model.IeCd == 0)
            {
                int count = context.T09Ies.Where(t09 => t09.IeRegion == model.IeRegion && (t09.IeSname == model.IeSname || t09.IeEmpNo == model.IeEmpNo)).Count();
                if (count == 0)
                {
                    var maxIECd = context.T09Ies.Max(t09 => (int?)t09.IeCd) ?? 0;
                    code = maxIECd + 1;

                    T09Ie obj = new T09Ie();
                    obj.IeCd = code;
                    obj.IeName = model.IeName;
                    obj.IeSname = model.IeSname;
                    obj.IeEmpNo = model.IeEmpNo;
                    obj.IeDesig = model.IeDesig;
                    obj.IeSealNo = model.IeSealNo;
                    obj.IeDepartment = model.IeDepartment;
                    obj.IeCityCd = Convert.ToInt32(model.IeCityCd);
                    obj.IePhoneNo = model.IePhoneNo;
                    obj.IeEmail = model.IeEmail;
                    obj.IeCoCd = model.IeCoCd;
                    obj.IeStatus = model.IeStatus == "W" ? "" : model.IeStatus;
                    obj.IeStatusDt = model.IeStatusDt;
                    obj.IeType = model.IeType;
                    obj.IeRegion = model.IeRegion;
                    obj.IeJoinDt = model.IeJoinDt;
                    obj.IeDob = model.IeDob;
                    obj.AltIe = model.AltIe;
                    obj.AltIeTwo = model.AltIeTwo;
                    obj.AltIeThree = model.AltIeThree;
                    obj.IeCallMarking = model.IeCallMarking;
                    obj.IePwd = model.IeEmpNo;
                    obj.UserId = model.UserId;
                    obj.Datetime = DateTime.Now.Date;
                    obj.CallMarkingStoppingDt = model.CallMarkingStoppingDt;
                    //obj.IEJOBTYPE = model.IEJOBTYPE;
                    //obj.CONTALTIE = model.CONTALTIE;

                    obj.Createdby = model.Createdby;
                    obj.Createddate = DateTime.Now;
                    obj.Isdeleted = Convert.ToByte(false);

                    context.T09Ies.Add(obj);
                    context.SaveChanges();
                    status = Convert.ToString(obj.IeCd);

                }
                else
                {
                    status = "Exists";
                }
            }
            else
            {
                var IE = context.T09Ies.Where(x => x.IeCd == model.IeCd).FirstOrDefault();
                if (IE != null)
                {
                    IE.IeName = model.IeName;
                    IE.IeSname = model.IeSname;
                    IE.IeEmpNo = model.IeEmpNo;
                    IE.IeDesig = model.IeDesig;
                    IE.IeSealNo = model.IeSealNo;
                    IE.IeDepartment = model.IeDepartment;
                    IE.IeCityCd = Convert.ToInt32(model.IeCityCd);
                    IE.IePhoneNo = model.IePhoneNo;
                    IE.IeCoCd = model.IeCoCd;
                    IE.IeStatus = model.IeStatus;
                    IE.IeStatusDt = model.IeStatusDt;
                    IE.IeType = model.IeType;
                    IE.IeRegion = model.IeRegion;
                    IE.IeJoinDt = model.IeJoinDt;
                    IE.IePwd = model.IeEmpNo;
                    IE.UserId = model.UserId;
                    IE.Datetime = DateTime.Now.Date;
                    IE.IeEmail = model.IeEmail;
                    IE.IeDob = model.IeDob;
                    IE.AltIe = model.AltIe;
                    IE.AltIeTwo = model.AltIeTwo;
                    IE.AltIeThree = model.AltIeThree;
                    IE.IeCallMarking = model.IeCallMarking;
                    IE.CallMarkingStoppingDt = model.CallMarkingStoppingDt;
                    //IE.IEJOBTYPE = model.IEJOBTYPE;
                    //IE.CONT_ALT_IE = model.CONT_ALT_IE;

                    IE.Updatedby = model.Updatedby;
                    IE.Updateddate = DateTime.Now;
                    context.SaveChanges();
                    status = Convert.ToString(IE.IeCd);
                }
                else
                {
                    status = "0";
                }
            }



            return status;
        }

        public string GetMatch(int IeCd, string GetRegionCode)
        {
            var MCode ="";
            var item = context.T09Ies.Where(x=>x.IeCd == IeCd).FirstOrDefault();
            if (item != null)
            {
                if(item.IeRegion == GetRegionCode)
                {
                    MCode = "2";
                }
            }
            return MCode;
        }
    }

}

