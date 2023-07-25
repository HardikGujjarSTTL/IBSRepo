using IBS.DataAccess;
using IBS.Interfaces.Vendor;
using IBS.Models;
using Microsoft.VisualStudio.Web.CodeGeneration;

namespace IBS.Repositories.Vendor
{
    public class VendorCallRegisterRepository : IVendorCallRegisterRepository
    {
        private readonly ModelContext context;

        public VendorCallRegisterRepository(ModelContext context)
        {
            this.context = context;
        }

        public VenderCallRegisterModel FindByID(string CaseNo, string CallRecvDt, int CallSno)
        {
            VenderCallRegisterModel model = new();
            string CallRDt = Convert.ToDateTime(CallRecvDt).ToString("dd-MM-yyyy");
            //T17CallRegister user = context.T17CallRegisters.Find(Convert.ToString(CaseNo), CallRDt, Convert.ToString(CallSno));
            T17CallRegister user = context.T17CallRegisters.Where(X => X.CaseNo == CaseNo && X.CallRecvDt == Convert.ToDateTime(CallRDt) && X.CallSno == CallSno).FirstOrDefault();
            VendorCallPoDetailsView GetView = context.VendorCallPoDetailsViews.Where(X => X.CaseNo == CaseNo).FirstOrDefault();

            if (user == null)
                throw new Exception("Vender Record Not found");
            else
            {
                model.CaseNo = CaseNo;
                model.CallSno = user.CallSno;
                model.CallRecvDt = user.CallRecvDt;
                model.CallLetterNo = user.CallLetterNo;
                model.CallLetterDt = user.CallLetterDt;
                model.CallMarkDt = user.CallMarkDt;
                model.IeCd = user.IeCd;
                model.DtInspDesire = user.DtInspDesire;
                
                if(user.CallStatus.Equals("M") || user.CallStatus.Equals("C"))
                {
                    model.CallStatus = user.CallCancelStatus.Equals("N") ? " (Non Chargeable)" : user.CallCancelStatus.Equals("C") ? " (Chargeable)" : "";
                }
                else
                {
                    model.CallStatus = user.CallStatus.Equals("M") ? "Marked" : user.CallStatus.Equals("C") ? "Cancelled" : user.CallStatus.Equals("A") ? "Accepted" : user.CallStatus.Equals("R") ? "Rejected" : user.CallStatus.Equals("U") ? "Under Lab Testing" : user.CallStatus.Equals("S") ? "Still Under Inspection" : user.CallStatus.Equals("G") ? "Stage Inspection" : "";
                }
                
                
                
                model.CallStatusDt = user.CallStatusDt;
                model.CallRemarkStatus = user.CallRemarkStatus;
                model.CallInstallNo = user.CallInstallNo;
                model.SetRegionCode = user.RegionCode;
                model.RegionCode = user.RegionCode.Equals("N") ? "Northern" : user.RegionCode.Equals("S") ? "Southern" : user.RegionCode.Equals("E") ? "Eastern" : user.RegionCode.Equals("W") ? "Western" : "Central";
                model.MfgCd = user.MfgCd;
                model.MfgPlace = user.MfgPlace;
                model.UpdateAllowed = user.UpdateAllowed == null ? "Y" : user.UpdateAllowed;
                model.Remarks = user.Remarks;
                model.FinalOrStage = user.FinalOrStage;
                model.Bpo = user.Bpo;
                model.RecipientGstinNo = user.RecipientGstinNo;
                model.PurchaserCd = GetView.PurchaserCd;
                model.VendCd = GetView.VendCd;
                model.PoNo = GetView.PoNo;
                model.PoDt = GetView.PoDt;
                model.Rly = GetView.Rly;
                model.L5noPo = GetView.L5noPo;
                model.RlyNonrly = GetView.RlyNonrly;

                return model;
            }
        }

        public DTResult<VenderCallRegisterModel> GetUserList(DTParameters dtParameters, string UserName)
        {
            DTResult<VenderCallRegisterModel> dTResult = new() { draw = 0 };
            IQueryable<VenderCallRegisterModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "CallRecvDt";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "CallRecvDt";
                orderAscendingDirection = true;
            }
            string CaseNo = "";
            string PoNo = "";
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["CaseNo"]))
            {
                CaseNo = Convert.ToString(dtParameters.AdditionalValues["CaseNo"]);
            }

            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["PoNo"]))
            {
                PoNo = Convert.ToString(dtParameters.AdditionalValues["PoNo"]);
            }

            CaseNo = CaseNo.ToString() == "" ? string.Empty : CaseNo.ToString();
            PoNo = PoNo.ToString() == "" ? string.Empty : PoNo.ToString();

            query = from l in context.T17CallRegisterSearchViews
                    where l.VendCd == Convert.ToInt32(UserName) && l.CaseNo.Contains(CaseNo) && l.PoNo.Contains(PoNo)

                    select new VenderCallRegisterModel
                    {
                        VendCd = Convert.ToString(l.VendCd),
                        CaseNo = l.CaseNo,
                        CallRecvDt = l.CallRecvDt,
                        CallInstallNo = l.CallInstallNo,
                        CallSno = l.CallSno,
                        CallStatus = l.CallStatus,
                        CallLetterNo = l.CallLetterNo,
                        Remarks = l.Remarks,
                        PoNo = l.PoNo,
                        PoDt = l.PoDt,
                        IeSname = l.IeSname,
                        Vendor = l.Vendor
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.CaseNo).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        //public VenderCallRegisterModel FindByID()
        //{
        //    VenderCallRegisterModel model = new();
        //    DateTime CDATE = DateTime.Now;


        //    if (user == null)
        //        throw new Exception("User Record Not found");
        //    else
        //    {
        //        model.ID = Convert.ToDecimal(user.Id);
        //        model.UserId = user.UserId;
        //        model.UserName = user.UserName;
        //        model.Password = user.Password;
        //        model.EmpNo = user.EmpNo;
        //        model.Region = user.Region;
        //        model.AuthLevl = user.AuthLevl;
        //        model.Status = user.Status;
        //        model.AllowPo = user.AllowPo;
        //        model.AllowUpChksht = user.AllowUpChksht;
        //        model.AllowDnChksht = user.AllowDnChksht;
        //        model.CallMarking = user.CallMarking;
        //        model.CallRemarking = user.CallRemarking;
        //        model.UserType = user.UserType;

        //        model.Isdeleted = user.Isdeleted;
        //        return model;
        //    }
        //}
    }
}
