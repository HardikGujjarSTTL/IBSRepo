using IBS.Controllers;
using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces.Vendor;
using IBS.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGeneration;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Data.Common;
using System.Xml;
using System.Xml.Linq;

namespace IBS.Repositories.Vendor
{
    public class VendorCallRegisterRepository : IVendorCallRegisterRepository
    {
        private readonly ModelContext context;

        public VendorCallRegisterRepository(ModelContext context)
        {
            this.context = context;
        }

        public VenderCallRegisterModel FindByID(string CaseNo, string CallRecvDt, int CallSno, string UserName)
        {
            VenderCallRegisterModel model = new();
            string CallRDt = Convert.ToDateTime(CallRecvDt).ToString("dd-MM-yyyy");
            //T17CallRegister user = context.T17CallRegisters.Find(Convert.ToString(CaseNo), CallRDt, Convert.ToString(CallSno));
            T17CallRegister user = context.T17CallRegisters.Where(X => X.CaseNo == CaseNo && X.CallRecvDt == Convert.ToDateTime(CallRDt) && X.CallSno == CallSno).FirstOrDefault();
            VendorCallPoDetailsView GetView = context.VendorCallPoDetailsViews.Where(X => X.CaseNo == CaseNo).FirstOrDefault();
            T05Vendor Vendor = context.T05Vendors.Where(x => x.VendCd == Convert.ToInt32(UserName)).FirstOrDefault();

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

                if (user.CallStatus.Equals("M") || user.CallStatus.Equals("C"))
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
                model.MfgCd = Convert.ToInt32(user.MfgCd);
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

                model.VendAdd1 = Vendor.VendAdd1;
                model.VendContactPer1 = Vendor.VendContactPer1;
                model.VendContactTel1 = Vendor.VendContactTel1;
                model.VendStatus = Vendor.VendStatus;
                model.VendStatusDtFr = Vendor.VendStatusDtFr;
                model.VendStatusDtTo = Vendor.VendStatusDtTo;
                model.VendEmail = Vendor.VendEmail;

                return model;
            }
        }

        public VenderCallRegisterModel FindByVenderDetail(int MfgCd)
        {
            VenderCallRegisterModel model = new();

            T05Vendor Vendor = context.T05Vendors.Where(x => x.VendCd == MfgCd).FirstOrDefault();

            if (Vendor == null)
                throw new Exception("Vender Record Not found");
            else
            {
                model.VendAdd1 = Vendor.VendAdd1;
                model.VendContactPer1 = Vendor.VendContactPer1;
                model.VendContactTel1 = Vendor.VendContactTel1;
                model.VendStatus = Vendor.VendStatus;
                model.VendStatusDtFr = Vendor.VendStatusDtFr;
                model.VendStatusDtTo = Vendor.VendStatusDtTo;
                model.VendEmail = Vendor.VendEmail;

                return model;
            }
        }

        public DTResult<VenderCallRegisterModel> FindByVenderDetail1(int MfgCd)
        {
            DTResult<VenderCallRegisterModel> dTResult = new() { draw = 0 };
            IQueryable<VenderCallRegisterModel>? query = null;

            query = from l in context.T05Vendors
                        //where l.VendCd == MfgCd
                    join c in context.T03Cities on l.VendCityCd equals (c.CityCd)
                    where l.VendCityCd == c.CityCd && l.VendName != null && l.VendCd == MfgCd

                    select new VenderCallRegisterModel
                    {
                        Vendor = Convert.ToString(l.VendName) + "/" + Convert.ToString(l.VendAdd1) + "/" + Convert.ToString(c.Location) + "/" + c.City,
                        VendCd = Convert.ToString(l.VendCd),
                        VendAdd1 = l.VendAdd1,
                        VendContactPer1 = l.VendContactPer1,
                        VendContactTel1 = l.VendContactTel1,
                        VendStatus = l.VendStatus,
                        VendStatusDtFr = l.VendStatusDtFr,
                        VendStatusDtTo = l.VendStatusDtTo,
                        VendEmail = l.VendEmail
                    };

            dTResult.data = query;
            return dTResult;
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
                    orderCriteria = "CaseNo";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "CaseNo";
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
                        CallStatus = l.CallStatus == null ? string.Empty : l.CallStatus,
                        CallLetterNo = l.CallLetterNo,
                        Remarks = l.Remarks == null ? string.Empty : l.Remarks,
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

        public DTResult<VenderCallRegisterModel> GetVenderList(DTParameters dtParameters, string UserName)
        {
            DTResult<VenderCallRegisterModel> dTResult = new() { draw = 0 };
            //IQueryable<VenderCallRegisterModel>? query = null;

            List<VenderCallRegisterModel>? query = null;

            var searchBy = dtParameters.Search?.Value;

            string CaseNo = "";
            string CallRecvDt = "";
            string CallSno = "";
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["CaseNo"]))
            {
                CaseNo = Convert.ToString(dtParameters.AdditionalValues["CaseNo"]);
            }

            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["CallRecvDt"]))
            {
                CallRecvDt = Convert.ToString(dtParameters.AdditionalValues["CallRecvDt"]);
            }

            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["CallSno"]))
            {
                CallSno = Convert.ToString(dtParameters.AdditionalValues["CallSno"]);
            }

            CaseNo = CaseNo.ToString() == "" ? string.Empty : CaseNo.ToString();
            CallRecvDt = Convert.ToDateTime(CallRecvDt).ToString("dd/MM/yyyyy");
            CallSno = CallSno.ToString() == "" ? string.Empty : CallSno.ToString();

            var ItemSrnoPo = (from a in context.T18CallDetails
                              where a.CaseNo == CaseNo && a.CallRecvDt == Convert.ToDateTime(CallRecvDt) && a.CallSno == Convert.ToInt16(CallSno)
                              select a.ItemSrnoPo).FirstOrDefault();

            query = (from l in context.VenderCallRegisterItemView1s
                     where l.CaseNo == CaseNo && l.CallRecvDt == Convert.ToDateTime(CallRecvDt) && l.CallSno == Convert.ToInt16(CallSno)

                     select new VenderCallRegisterModel
                     {
                         Status = l.Status,
                         ItemSrnoPo = l.ItemSrnoPo,
                         ItemDescPo = l.ItemDescPo,
                         QtyOrdered = l.QtyOrdered,
                         CumQtyPrevOffered = l.CumQtyPrevOffered,
                         CumQtyPrevPassed = l.CumQtyPrevPassed,
                         QtyToInsp = l.QtyToInsp,
                         QtyPassed = l.QtyPassed,
                         QtyRejected = l.QtyRejected,
                         QtyDue = l.QtyDue,
                         Consignee = l.Consignee,
                         DelvDate = l.DelvDate,
                         CaseNo = CaseNo,
                         CallRecvDt = Convert.ToDateTime(CallRecvDt),
                         CallSno = Convert.ToInt16(CallSno)
                     }).ToList();

            query.AddRange(from l in context.VenderCallRegisterItemView2s
                           where l.CaseNo == CaseNo && l.ItemSrnoPo != ItemSrnoPo

                           select new VenderCallRegisterModel
                           {
                               Status = l.Status,
                               ItemSrnoPo = l.ItemSrnoPo,
                               ItemDescPo = l.ItemDescPo,
                               QtyOrdered = l.QtyOrdered,
                               CumQtyPrevOffered = l.CumQtyPrevOffered,
                               CumQtyPrevPassed = l.CumQtyPrevPassed,
                               QtyToInsp = l.QtyToInsp,
                               QtyPassed = l.QtyPassed,
                               QtyRejected = l.QtyRejected,
                               QtyDue = l.QtyDue,
                               Consignee = l.Consignee,
                               DelvDate = l.DelvDate,
                               CaseNo = CaseNo,
                               CallRecvDt = Convert.ToDateTime(CallRecvDt),
                               CallSno = Convert.ToInt16(CallSno)
                           });


            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => w.Status.Contains(searchBy) || w.Consignee.Contains(searchBy)).ToList();

            dTResult.recordsFiltered = query.Count();

            //dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();
            dTResult.data = query.Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public int DetailsInsertUpdate(VenderCallRegisterModel model)
        {
            int ID = 0;
            using (var dbContext = context.Database.GetDbConnection())
            {
                OracleParameter[] par = new OracleParameter[4];
                par[0] = new OracleParameter("p_MfgCd", OracleDbType.Varchar2, model.MfgCd, ParameterDirection.Input);
                par[1] = new OracleParameter("p_VendContactPer1", OracleDbType.Varchar2, model.VendContactPer1, ParameterDirection.Input);
                par[2] = new OracleParameter("p_VendContactTel1", OracleDbType.Varchar2, model.VendContactTel1, ParameterDirection.Input);
                par[3] = new OracleParameter("p_VendEmail", OracleDbType.Varchar2, model.VendEmail, ParameterDirection.Input);

                var ds = DataAccessDB.ExecuteNonQuery("SP_UPDATE_VENDOR_INFO", par, 1);
                ID = model.MfgCd;
            }
            return ID;
        }

        public int RegiserCallSave(VenderCallRegisterModel model)
        {
            int ID = 0;
            using (var dbContext = context.Database.GetDbConnection())
            {
                OracleParameter[] par = new OracleParameter[14];
                par[0] = new OracleParameter("p_CALL_LETTER_NO", OracleDbType.Varchar2, model.CallLetterNo, ParameterDirection.Input);
                par[1] = new OracleParameter("p_CALL_LETTER_DT", OracleDbType.Date, model.CallLetterDt, ParameterDirection.Input);
                par[2] = new OracleParameter("p_CALL_MARK_DT", OracleDbType.Date, model.CallMarkDt, ParameterDirection.Input);
                par[3] = new OracleParameter("p_CALL_SNO", OracleDbType.Int32, model.CallSno, ParameterDirection.Input);
                par[4] = new OracleParameter("p_DT_INSP_DESIRE", OracleDbType.Date, model.DtInspDesire, ParameterDirection.Input);
                par[5] = new OracleParameter("p_CALL_STATUS_DT", OracleDbType.Date, model.CallStatusDt, ParameterDirection.Input);
                par[6] = new OracleParameter("p_CALL_REMARK_STATUS", OracleDbType.Varchar2, model.CallRemarkStatus, ParameterDirection.Input);
                par[7] = new OracleParameter("p_CALL_INSTALL_NO", OracleDbType.Varchar2, model.CallInstallNo, ParameterDirection.Input);
                par[8] = new OracleParameter("p_REMARKS", OracleDbType.Varchar2, model.Remarks, ParameterDirection.Input);
                par[9] = new OracleParameter("p_MFG_CD", OracleDbType.Varchar2, model.MfgCd, ParameterDirection.Input);
                par[10] = new OracleParameter("p_MFG_PLACE", OracleDbType.Varchar2, model.MfgPlace, ParameterDirection.Input);
                par[11] = new OracleParameter("p_USER_ID", OracleDbType.Varchar2, model.UserId, ParameterDirection.Input);
                par[12] = new OracleParameter("p_DATETIME", OracleDbType.Date, model.Datetime, ParameterDirection.Input);
                par[13] = new OracleParameter("p_CASE_NO", OracleDbType.Varchar2, model.CaseNo, ParameterDirection.Input);
                par[14] = new OracleParameter("p_CALL_RECV_DT", OracleDbType.Date, model.CallRecvDt, ParameterDirection.Input);

                var ds = DataAccessDB.ExecuteNonQuery("SP_UPDATE_CALL_REGISTER", par, 1);
                return ID;
            }
        }

    }
}
