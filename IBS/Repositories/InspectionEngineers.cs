using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;

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

        public InspectionEngineersModel FindManageByID(int Id)
        {
            InspectionEngineersModel model = new();

            model = (from t09 in context.T09Ies
                     where t09.IeCd == Id
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
                         IeCityId = t09.IeCityCd ?? 0,
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
                         IeJobType = t09.IeJobType,
                         AltIe = t09.AltIe,
                         IeCallMarking = t09.IeCallMarking,
                         AltIeTwo = t09.AltIeTwo,
                         AltIeThree = t09.AltIeThree,
                         ContAltIe = t09.ContAltIe,
                         CallMarkingStoppingDt = t09.CallMarkingStoppingDt,
                         CallMarkingStartDt = t09.CallMarkingStartDt,
                         InspectionStartDt = t09.InspectionStartDt,
                         RepatriationDt = t09.RepatriationDt,
                         //Cluster = context.T101IeClusters.Where(x => x.IeCode == Convert.ToInt32(Id)).Select(x => x.ClusterCode).SingleOrDefault(),
                     }).FirstOrDefault();

            List<InspectionEngineersListModel> clst = (from T101 in context.T101IeClusters
                                                       join T09 in context.T09Ies on T101.IeCode equals T09.IeCd
                                                       join T99 in context.T99ClusterMasters on T101.ClusterCode equals T99.ClusterCode
                                                       where T101.IeCode == Id
                                                       select new
                                                       InspectionEngineersListModel
                                                       {
                                                           In_ID = Convert.ToInt32(T101.IeCode),
                                                           IeCd = Convert.ToInt32(T101.IeCode),
                                                           IeName = T09.IeName,
                                                           IeDepartment = T101.DepartmentCode,
                                                           Cluster = Convert.ToInt32(T101.ClusterCode),
                                                           ClusterID = Convert.ToString(T101.ClusterCode),
                                                           lstCluster = Convert.ToString(T99.ClusterName),
                                                       }
                                                 ).ToList();
            model.lstInspectionEClusterModel = clst;

            return model;
        }

        public DTResult<InspectionEngineersModel> GetInspectionEngineersList(DTParameters dtParameters, string Region)
        {

            DTResult<InspectionEngineersModel> dTResult = new() { draw = 0 };
            IQueryable<InspectionEngineersModel>? query = null;

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

            int? IeCd = !string.IsNullOrEmpty(dtParameters.AdditionalValues["IeCd"]) ? Convert.ToInt32(dtParameters.AdditionalValues["IeCd"]) : null;
            string IeSname = !string.IsNullOrEmpty(dtParameters.AdditionalValues["IeSname"]) ? Convert.ToString(dtParameters.AdditionalValues["IeSname"]) : "";
            string IeName = !string.IsNullOrEmpty(dtParameters.AdditionalValues["IeName"]) ? Convert.ToString(dtParameters.AdditionalValues["IeName"]) : "";
            int? IeCoCd = !string.IsNullOrEmpty(dtParameters.AdditionalValues["IeCoCd"]) ? Convert.ToInt32(dtParameters.AdditionalValues["IeCoCd"]) : null;

            string IeDepartment = !string.IsNullOrEmpty(dtParameters.AdditionalValues["IeDepartment"]) ? Convert.ToString(dtParameters.AdditionalValues["IeDepartment"]) : "";
            int? ClusterID = !string.IsNullOrEmpty(dtParameters.AdditionalValues["ClusterID"]) ? Convert.ToInt32(dtParameters.AdditionalValues["ClusterID"]) : null;
            int? IeCode = !string.IsNullOrEmpty(dtParameters.AdditionalValues["IeCode"]) ? Convert.ToInt32(dtParameters.AdditionalValues["IeCode"]) : null;

            query = from v in context.T09Ies
                    join T101 in context.T101IeClusters on v.IeCd equals T101.IeCode
                    join T99 in context.T99ClusterMasters on T101.ClusterCode equals T99.ClusterCode
                    join c in context.T03Cities on v.IeCityCd equals c.CityCd into cityJoin
                    from c in cityJoin.DefaultIfEmpty()
                    where ((IeCd != null) ? v.IeCd == IeCd : true)
                     && (!string.IsNullOrEmpty(IeSname) ? v.IeSname.ToLower().Contains(IeSname.ToLower()) : true)
                     && (!string.IsNullOrEmpty(IeName) ? v.IeName.ToLower().Contains(IeName.ToLower()) : true)
                     && ((IeCoCd != null) ? v.IeCoCd == IeCoCd : true)
                     && ((IeDepartment != "") ? v.IeDepartment == IeDepartment : true)
                     && ((ClusterID != null) ? T99.ClusterCode == Convert.ToInt32(ClusterID) : true)
                     && ((IeCode != null) ? v.IeCd == Convert.ToInt32(IeCode) : true)
                     && v.IeRegion == Region
                    select new InspectionEngineersModel
                    {
                        IeCd = v.IeCd,
                        IeName = v.IeName,
                        IeSname = v.IeSname,
                        IeEmpNo = v.IeEmpNo,
                        IeSealNo = v.IeSealNo,
                        IeCityCd = c.Location != null ? c.Location + " : " + c.City : c.City,
                        IeRegion = v.IeRegion,
                        Cluster = T99.ClusterCode,
                        ClusterName = T99.ClusterName + "-" + T99.GeographicalPartition
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
            string status = "";
            int code = new int();

            var T101IeClusters = (from T101 in context.T101IeClusters where T101.IeCode == model.IeCd select T101).ToList();
            //var ClusterCode = T101IeClusters.FirstOrDefault();
            //var ClustCount = context.T101IeClusters.Where(x => x.ClusterCode == ClusterCode.ClusterCode && x.DepartmentCode == ClusterCode.DepartmentCode).Count();

            //if (ClustCount >= 1)
            //{
            //    return status = "2";
            //}
            if (model.IeCd == 0)
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
                    obj.IeCityCd = Convert.ToInt32(model.IeCityId);
                    obj.IePhoneNo = model.IePhoneNo;
                    obj.IeEmail = model.IeEmail;
                    obj.IeCoCd = model.IeCoCd;
                    obj.IeStatus = model.IeStatus == "W" ? "" : model.IeStatus;
                    obj.IeStatusDt = model.IeStatusDt;
                    obj.IeType = model.IeType;
                    obj.IeRegion = model.IeRegion;
                    obj.IeJoinDt = model.IeJoinDt;
                    obj.IeDob = model.IeDob;
                    obj.IeJobType = model.IeJobType;
                    obj.AltIe = model.AltIe;
                    obj.AltIeTwo = model.AltIeTwo;
                    obj.AltIeThree = model.AltIeThree;
                    obj.ContAltIe = model.ContAltIe;
                    obj.IeCallMarking = model.IeCallMarking;
                    obj.IePwd = "WPg3mg2hKkFI3zwp72u8SA==";
                    obj.UserId = model.UserId;
                    obj.Datetime = DateTime.Now.Date;
                    obj.CallMarkingStoppingDt = model.CallMarkingStoppingDt;
                    obj.CallMarkingStartDt = model.CallMarkingStartDt;
                    obj.InspectionStartDt = model.InspectionStartDt;
                    obj.RepatriationDt = model.RepatriationDt;
                    obj.Createdby = model.Createdby;
                    obj.Createddate = DateTime.Now;
                    obj.Isdeleted = Convert.ToByte(false);

                    context.T09Ies.Add(obj);
                    context.SaveChanges();
                    status = Convert.ToString(obj.IeCd);

                    model.IeCd = obj.IeCd;
                }
                else
                {
                    status = "Exists";
                    return status;
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
                    IE.IeCityCd = Convert.ToInt32(model.IeCityId);
                    IE.IePhoneNo = model.IePhoneNo;
                    IE.IeCoCd = model.IeCoCd;
                    IE.IeStatus = model.IeStatus;
                    IE.IeStatusDt = model.IeStatusDt;
                    IE.IeType = model.IeType;
                    IE.IeRegion = model.IeRegion;
                    IE.IeJoinDt = model.IeJoinDt;
                    IE.IePwd = "WPg3mg2hKkFI3zwp72u8SA==";
                    IE.UserId = model.UserId;
                    IE.Datetime = DateTime.Now.Date;
                    IE.IeEmail = model.IeEmail;
                    IE.IeDob = model.IeDob;
                    IE.IeJobType = model.IeJobType;
                    IE.AltIe = model.AltIe;
                    IE.AltIeTwo = model.AltIeTwo;
                    IE.AltIeThree = model.AltIeThree;
                    IE.ContAltIe = model.ContAltIe;
                    IE.IeCallMarking = model.IeCallMarking;
                    IE.CallMarkingStoppingDt = model.CallMarkingStoppingDt;
                    IE.CallMarkingStartDt = model.CallMarkingStartDt;
                    IE.InspectionStartDt = model.InspectionStartDt;
                    IE.RepatriationDt = model.RepatriationDt;
                    IE.Updatedby = model.Updatedby;
                    IE.Updateddate = DateTime.Now;
                    context.SaveChanges();
                    status = Convert.ToString(IE.IeCd);
                }
                else
                {
                    status = "0";
                    return status;
                }
            }

            if (T101IeClusters.Count > 0 && T101IeClusters != null)
            {
                context.T101IeClusters.RemoveRange(T101IeClusters);
                context.SaveChanges();
            }
            if (model.lstInspectionEClusterModel != null)
            {
                foreach (var item in model.lstInspectionEClusterModel)
                {
                    T101IeCluster Clst = new T101IeCluster();
                    {
                        Clst.IeCode = model.IeCd;
                        Clst.DepartmentCode = item.IeDepartment;
                        Clst.ClusterCode = Convert.ToInt32(item.ClusterID);
                        Clst.UserId = model.UserId;
                        Clst.Datetime = DateTime.Now.Date;
                    }
                    context.T101IeClusters.Add(Clst);
                    context.SaveChanges();
                }

            }

            UserUpdate(model);
            return status;
        }

        public string GetMatch(int IeCd, string GetRegionCode)
        {
            var MCode = "";
            var item = context.T09Ies.Where(x => x.IeCd == IeCd).FirstOrDefault();
            if (item != null)
            {
                if (item.IeRegion == GetRegionCode)
                {
                    MCode = "2";
                }
            }
            return MCode;
        }

        public string DeleteIe(int IeCd)
        {
            string msg = "";
            var itemDelete = context.T09Ies.FirstOrDefault(x => x.IeCd == IeCd);

            context.T09Ies.Remove(itemDelete);
            context.SaveChanges();
            msg = Convert.ToString(itemDelete.IeCd);
            return msg;
        }

        public DTResult<InspectionEngineersListModel> GetClusterValueList(DTParameters dtParameters, List<InspectionEngineersListModel> ClusterModels)
        {
            DTResult<InspectionEngineersListModel> dTResult = new() { draw = 0 };
            IQueryable<InspectionEngineersListModel>? query = null;
            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "IeCd";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "IeCd";
                orderAscendingDirection = true;
            }

            query = (from u in ClusterModels.OrderBy(x => x.IeCd)
                     select new InspectionEngineersListModel
                     {
                         In_ID = u.In_ID,
                         IeCd = Convert.ToInt32(u.IeCd),
                         IeName = u.IeName,
                         IeDepartment = u.IeDepartment == "M" ? "Mechanical" : u.IeDepartment == "E" ? "Electrical" : u.IeDepartment == "C" ? "Civil" : u.IeDepartment == "L" ? "Metallurgy" : u.IeDepartment == "T" ? "Textiles" : "Power Engineering",
                         Cluster = u.Cluster,
                         lstCluster = u.lstCluster,
                         ClusterID = u.ClusterID
                     }).AsQueryable();

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => w.IeDepartment.ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;

        }

        public string GetUserID(string IeEmpNo)
        {
            string ID = "";
            var UserId = context.T02Users.Where(x => x.UserId == IeEmpNo).FirstOrDefault();
            if (UserId != null)
            {
                ID = "1";
            }
            else
            {
                ID = "0";
            }
            return ID;
        }

        public void UserUpdate(InspectionEngineersModel model)
        {
            var UserDetails = context.T02Users.Where(x => x.UserId == Convert.ToString(model.IeEmpNo)).FirstOrDefault();
            if (UserDetails == null)
            {
                T02User User = new();
                User.UserId = Convert.ToString(model.IeEmpNo);
                User.UserName = model.IeName;
                User.RitesEmp = "Y";
                User.EmpNo = Convert.ToString(model.IeEmpNo);
                User.Region = model.IeRegion;
                //User.Password = Convert.ToString(model.IeEmpNo);
                User.Password = "WPg3mg2hKkFI3zwp72u8SA==";
                User.Createdby = model.UserId;
                User.Createddate = DateTime.Now.Date;
                User.Isdeleted = 0;
                User.Migtype = "I";
                User.Mobile = model.IePhoneNo;

                context.T02Users.Add(User);
                context.SaveChanges();
            }
            else
            {
                //UserDetails.UserName = model.IeEmpNo;
                //UserDetails.RitesEmp = "Y";
                //UserDetails.EmpNo = Convert.ToString(model.IeEmpNo);
                //UserDetails.Region = model.IeRegion;
                //UserDetails.Password = Convert.ToString(model.IeEmpNo);
                //UserDetails.Updatedby = model.UserId;
                //UserDetails.Updateddate = DateTime.Now.Date;
                //UserDetails.Isdeleted = 0;
                //UserDetails.Migtype = "I";
                //UserDetails.Mobile = model.IePhoneNo;

                UserDetails.Password = "WPg3mg2hKkFI3zwp72u8SA==";
                context.SaveChanges();
            }
        }
    }

}

