using IBS.Controllers;
using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces.Vendor;
using IBS.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGeneration;
using Newtonsoft.Json;
using NuGet.Packaging;
using NuGet.Protocol.Plugins;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Data.Common;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Reflection.Emit;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Xml;
using System.Xml.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace IBS.Repositories.Vendor
{
    public class VendorCallRegisterRepository : IVendorCallRegisterRepository
    {
        private readonly ModelContext context;

        public VendorCallRegisterRepository(ModelContext context)
        {
            this.context = context;
        }

        public string CNO, DT, Action, CSNO, cstatus, wFOS;
        int callval = 0;
        int e_status = 0;

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
                model.CallSno = (short)user.CallSno;
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

        public string RegiserCallSave(VenderCallRegisterModel model)
        {


            string IE_name = null;
            int ie_officer_code = 0;
            string automaticCallMarked = null;

            string ID = "";
            int CD = 0;
            if (model.ActionType == "A")
            {
                int cmdCL = context.T17CallRegisters.Where(x => x.CaseNo == model.CaseNo && x.CallRecvDt == model.CallRecvDt && x.RegionCode == model.SetRegionCode).Count();
                if (cmdCL == 0)
                {
                    var w_item_rdso = "";
                    var w_vend_rdso = "";
                    var w_stag = "";
                    var w_stage_or_final = "";

                    var str3 = context.T17CallRegisters.Where(x => x.CallRecvDt == model.CallRecvDt && x.RegionCode == model.SetRegionCode).FirstOrDefault();
                    CD = str3.CallSno + 1;
                    if (model.ItemRdso == "Y")
                    {
                        w_item_rdso = "Y";
                        if (model.VendRdso == "Y")
                        {
                            w_vend_rdso = "Y";
                        }
                        else
                        {
                            w_vend_rdso = "N";
                        }
                    }
                    else
                    {
                        w_item_rdso = "N";
                        w_vend_rdso = "";
                    }
                    if (model.StaggeredDp == "Y")
                    {
                        w_stag = "Y";
                    }
                    else
                    {
                        w_stag = "N";
                    }
                    if (model.FOS == "S")
                    {
                        w_stage_or_final = "S";
                    }
                    else
                    {
                        w_stage_or_final = "F";
                    }
                    var w_New_Vendor = "";
                    if (model.IsNewVender == "Y")
                    {
                        w_New_Vendor = "Y";
                    }
                    callval = FindIeCODE(model);
                    if (callval == 0)
                    {
                        //DisplayAlert("Master data not entered.So please enter master data cluster/vender/ie");
                    }
                    else
                    {
                        var ieInfo = context.T09Ies.Where(ie => ie.IeCd == callval).Select(ie => new { IeName = ie.IeName, IeCoCode = ie.IeCoCd }).FirstOrDefault();

                        if (ieInfo != null)
                        {
                            string ieName = ieInfo.IeName;
                            int ieOfficerCode = Convert.ToInt32(ieInfo.IeCoCode);
                            IE_name = ieName;
                            ie_officer_code = ieOfficerCode;
                            automaticCallMarked = "Y";
                        }
                    }
                    string w_irfc_funded = "";
                    if (model.SetRegionCode == "R")
                    {
                        w_irfc_funded = Convert.ToString(model.IrfcFunded);
                    }
                    else
                    {
                        w_irfc_funded = "N";
                    }
                    if (callval == 0)
                    {
                        T17CallRegister obj = new T17CallRegister();
                        obj.CaseNo = model.CaseNo;
                        obj.CallRecvDt = Convert.ToDateTime(model.CallRecvDt);
                        obj.CallSno = (short)CD;
                        obj.CallLetterNo = model.CallLetterNo;
                        obj.CallLetterDt = model.CallLetterDt;
                        obj.CallMarkDt = model.CallMarkDt;
                        obj.DepartmentCode = model.DepartmentCode;
                        obj.DtInspDesire = model.DtInspDesire;
                        obj.CallStatus = "M";
                        obj.CallStatusDt = model.CallStatusDt;
                        obj.CallRemarkStatus = model.CallRemarkStatus;
                        obj.CallInstallNo = model.CallInstallNo;
                        obj.RegionCode = model.SetRegionCode;
                        obj.MfgCd = model.MfgCd;
                        obj.UserId = model.UserId;
                        obj.Datetime = DateTime.Now;
                        obj.MfgPlace = model.MfgPlace;
                        obj.Remarks = model.Remarks;
                        obj.OnlineCall = "Y";
                        obj.ItemRdso = w_item_rdso;
                        obj.VendRdso = w_vend_rdso;
                        obj.VendApprovalFr = model.VendApprovalFr;
                        obj.VendApprovalTo = model.VendApprovalTo;
                        obj.StaggeredDp = w_stag;
                        obj.LotDp1 = model.LotDp1;
                        obj.LotDp2 = model.LotDp2;
                        obj.FinalOrStage = w_stage_or_final;
                        obj.Bpo = model.Bpo;
                        obj.RecipientGstinNo = model.RecipientGstinNo;
                        obj.NewVendor = w_New_Vendor;
                        obj.IrfcFunded = w_irfc_funded;

                        obj.Createdby = model.Createdby;
                        obj.Createddate = DateTime.Now;
                        context.T17CallRegisters.Add(obj);
                        context.SaveChanges();
                        ID = obj.CaseNo;
                    }
                    else
                    {
                        T17CallRegister obj = new T17CallRegister();
                        obj.CaseNo = model.CaseNo;
                        obj.CallRecvDt = Convert.ToDateTime(model.CallRecvDt);
                        obj.CallSno = (short)CD;
                        obj.CallLetterNo = model.CallLetterNo;
                        obj.CallLetterDt = model.CallLetterDt;
                        obj.CallMarkDt = model.CallMarkDt;
                        obj.IeCd = callval;
                        obj.CoCd = (byte)ie_officer_code;
                        obj.AutomaticCall = automaticCallMarked;
                        obj.DepartmentCode = model.DepartmentCode;
                        obj.DtInspDesire = model.DtInspDesire;
                        obj.CallStatus = "M";
                        obj.CallStatusDt = model.CallStatusDt;
                        obj.CallRemarkStatus = model.CallRemarkStatus;
                        obj.CallInstallNo = model.CallInstallNo;
                        obj.RegionCode = model.SetRegionCode;
                        obj.MfgCd = model.MfgCd;
                        obj.UserId = model.UserId;
                        obj.Datetime = DateTime.Now;
                        obj.MfgPlace = model.MfgPlace;
                        obj.Remarks = model.Remarks;
                        obj.OnlineCall = "Y";
                        obj.ItemRdso = w_item_rdso;
                        obj.VendRdso = w_vend_rdso;
                        obj.VendApprovalFr = model.VendApprovalFr;
                        obj.VendApprovalTo = model.VendApprovalTo;
                        obj.StaggeredDp = w_stag;
                        obj.LotDp1 = model.LotDp1;
                        obj.LotDp2 = model.LotDp2;
                        obj.FinalOrStage = w_stage_or_final;
                        obj.Bpo = model.Bpo;
                        obj.RecipientGstinNo = model.RecipientGstinNo;
                        obj.NewVendor = w_New_Vendor;
                        obj.IrfcFunded = w_irfc_funded;
                        obj.ClusterCode = model.ClusterCode;

                        obj.Createdby = model.Createdby;
                        obj.Createddate = DateTime.Now;
                        context.T17CallRegisters.Add(obj);
                        context.SaveChanges();
                        ID = obj.CaseNo;
                    }
                    decimal wMat_value = 0;
                    string ext_delv_dt = "";
                    int desire_dt = 0;

                    GetDtList(model);

                }
                else
                {
                    model.CaseNoNoFound = "NoFound";
                }
            }
            else if (model.ActionType == "M")
            {
                var GetCall = context.T17CallRegisters.Where(x => x.CaseNo == model.CaseNo && x.CallRecvDt == model.CallRecvDt && x.CallSno == model.CallSno).FirstOrDefault();
                #region Details save
                if (GetCall != null)
                {
                    GetCall.CallLetterNo = model.CallLetterNo;
                    GetCall.CallLetterDt = model.CallLetterDt;
                    GetCall.CallMarkDt = model.CallMarkDt;
                    GetCall.CallSno = (short)model.CallSno;
                    GetCall.DtInspDesire = model.DtInspDesire;
                    GetCall.CallStatusDt = model.CallStatusDt;
                    GetCall.CallRemarkStatus = model.CallRemarkStatus;
                    GetCall.CallInstallNo = model.CallInstallNo;
                    GetCall.Remarks = model.Remarks;
                    GetCall.MfgCd = model.MfgCd;
                    GetCall.MfgPlace = model.VendAdd1;
                    GetCall.UserId = model.UserId;
                    GetCall.Datetime = DateTime.Now;

                    context.SaveChanges();
                    ID = model.CaseNo;
                }
                #endregion
            }
            return ID;
        }

        int GetDtList(VenderCallRegisterModel model)
        {
            int err = 0;
            decimal qty_off_now = 0;

            List<VenderCallRegisterModel>? query = null;

            var ItemSrnoPo = (from a in context.T18CallDetails
                              where a.CaseNo == model.CaseNo && a.CallRecvDt == Convert.ToDateTime(model.CallRecvDt) && a.CallSno == Convert.ToInt16(model.CallSno)
                              select a.ItemSrnoPo).FirstOrDefault();

            query = (from l in context.VenderCallRegisterItemView1s
                     where l.CaseNo == model.CaseNo && l.CallRecvDt == Convert.ToDateTime(model.CallRecvDt) && l.CallSno == Convert.ToInt16(model.CallSno)

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
                         CaseNo = l.CaseNo,
                         CallRecvDt = Convert.ToDateTime(l.CallRecvDt),
                         CallSno = Convert.ToInt16(l.CallSno)
                     }).ToList();

            query.AddRange(from l in context.VenderCallRegisterItemView2s
                           where l.CaseNo == model.CaseNo && l.ItemSrnoPo != ItemSrnoPo

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
                               CaseNo = l.CaseNo,
                               CallRecvDt = Convert.ToDateTime(l.CallRecvDt),
                               CallSno = Convert.ToInt16(l.CallSno)
                           });
            decimal wMat_value = 0;
            string ext_delv_dt = "";
            int desire_dt = 0;

            for (int i = 0; i < query.Count; i++)
            {
                VenderCallRegisterModel dataItem = query[i];
                err = 0;

                int srno = (byte)dataItem.ItemSrnoPo;
                decimal qtyOffNow = Convert.ToDecimal(dataItem.QtyToInsp);

                if (qtyOffNow > 0)
                {
                    err = 1;
                }
                if (err == 1)
                {
                    var query1 = from detail in context.T15PoDetails
                                 where detail.CaseNo == model.CaseNo && detail.ItemSrno == model.ItemSrnoPo
                                 select new
                                 {
                                     detail.ConsigneeCd,
                                     detail.Qty,
                                     detail.Value,
                                     EXT_DELV_DATE = detail.ExtDelvDt.HasValue ? detail.ExtDelvDt.Value.ToString("dd/MM/yyyy") : "01-JAN-01"
                                 };
                    long ccd = 0;
                    foreach (var record in query1)
                    {
                        ccd = (long)record.ConsigneeCd;
                        wMat_value += Convert.ToDecimal((record.Value / record.Qty) * qty_off_now);
                        model.wMat_value = wMat_value;
                        ext_delv_dt = record.EXT_DELV_DATE;
                    }

                    T18CallDetail obj = new T18CallDetail();
                    obj.CaseNo = model.CaseNo;
                    obj.CallRecvDt = Convert.ToDateTime(model.CallRecvDt);
                    obj.CallSno = (short)model.CallSno;
                    obj.ItemSrnoPo = (byte)srno;
                    obj.ItemDescPo = dataItem.ItemDescPo;
                    obj.ConsigneeCd = Convert.ToInt32(ccd);
                    obj.QtyOrdered = dataItem.QtyOrdered;
                    obj.CumQtyPrevOffered = dataItem.CumQtyPrevOffered;
                    obj.CumQtyPrevPassed = dataItem.CumQtyPrevPassed;
                    obj.QtyToInsp = Convert.ToDecimal(qtyOffNow);
                    obj.QtyPassed = null;
                    obj.QtyRejected = null;
                    obj.QtyDue = null;
                    obj.UserId = model.Createdby;
                    obj.Datetime = DateTime.Now;
                    obj.Createdby = model.Createdby;
                    obj.Createddate = DateTime.Now;

                    obj.Isdeleted = Convert.ToByte(false);
                    context.T18CallDetails.Add(obj);
                    context.SaveChanges();
                    err = Convert.ToInt32(obj.ItemSrnoPo);

                    if (desire_dt == 0)
                    {
                        desire_dt = check_desire_dt(model, ext_delv_dt);
                        model.desire_dt = desire_dt;
                    }

                }
            }

            if ((model.RlyNonrly == "R" && wMat_value > 1000 && desire_dt == 0) || (model.RlyNonrly != "R" && wMat_value > 1000 && desire_dt == 0 && model.Bpo != "" && model.RecipientGstinNo != ""))
            {
                show_items(model);

                if (wMat_value < 5000000)
                {
                    var contAltIeCode = (from t09 in context.T09Ies
                                         where t09.IeCd == Convert.ToInt32(model.IeCd)
                                         select t09.AltIe ?? 0).FirstOrDefault();

                    var regMaxLimit = (from t102 in context.T102IeMaximumCallLimits
                                       where t102.RegionCode == Convert.ToString(model.SetRegionCode)
                                       select t102.MaximumCall).FirstOrDefault();
                    int reg_max_limit = regMaxLimit.HasValue ? regMaxLimit.Value : 0;

                    var iePendCalls = (from a in context.T17CallRegisters
                                       join b in context.T09Ies on a.IeCd equals b.IeCd
                                       where a.CallStatus == "M" || a.CallStatus == "S" &&
                                             a.IeCd == contAltIeCode &&
                                             a.CallRecvDt > DateTime.Parse("2022-04-01")
                                       select a.CallStatus).Count();
                    int ie_pend_calls = iePendCalls;

                    var ieCallMarking = (from t09 in context.T09Ies
                                         where t09.IeCd == contAltIeCode
                                         select t09.IeCallMarking).FirstOrDefault();

                    string ie_call_marking = Convert.ToString(ieCallMarking);
                    if (contAltIeCode != 0 && ie_pend_calls < reg_max_limit && ie_call_marking == "Y")
                    {
                        var contAltIeNameQuery = from t09 in context.T09Ies
                                                 where t09.IeCd == contAltIeCode
                                                 select new { t09.IeName, t09.IeCoCd };

                        var contAltIeInfo = contAltIeNameQuery.FirstOrDefault();

                        string IE_name = contAltIeInfo?.IeName ?? "";
                        model.IE_name = IE_name;
                        int cont_ieofficercode = contAltIeInfo?.IeCoCd ?? 0;

                        var CallRegisters1 = context.T17CallRegisters.Where(x => x.CaseNo == model.CaseNo && x.CallRecvDt == model.CallRecvDt && x.CallSno == model.CallSno).FirstOrDefault();
                        if (CallRegisters1 != null)
                        {
                            CallRegisters1.IeCd = contAltIeCode;
                            CallRegisters1.CoCd = (byte)cont_ieofficercode;
                            CallRegisters1.Updatedby = model.Createdby;
                            CallRegisters1.Updateddate = DateTime.Now;
                            context.SaveChanges();
                            err = Convert.ToInt32(CallRegisters1.IeCd);
                        }
                        model.callval = contAltIeCode;
                    }
                }
                e_status = 1;
                model.e_status = e_status;
            }

            return err;
        }

        void show_items(VenderCallRegisterModel model)
        {
            try
            {
                var query11 = (from t18 in context.T18CallDetails
                               join t06 in context.T06Consignees on t18.ConsigneeCd equals t06.ConsigneeCd
                               join t03 in context.T03Cities on t06.ConsigneeCity equals t03.CityCd
                               where t18.CaseNo == CNO &&
                                     t18.CallRecvDt == DateTime.ParseExact(DT, "dd/MM/yyyy", CultureInfo.InvariantCulture) &&
                                     t18.CallSno == (int)model.CallSno
                               select new VenderCallRegisterModel
                               {
                                   Status = "Marked",
                                   ItemSrnoPo = t18.ItemSrnoPo,
                                   ItemDescPo = t18.ItemDescPo,
                                   QtyOrdered = t18.QtyOrdered,
                                   CumQtyPrevOffered = t18.CumQtyPrevOffered,
                                   CumQtyPrevPassed = t18.CumQtyPrevPassed,
                                   QtyToInsp = t18.QtyToInsp,
                                   QtyPassed = t18.QtyPassed,
                                   QtyRejected = t18.QtyRejected,
                                   QtyDue = t18.QtyDue,
                                   Consignee = $"{t06.ConsigneeCd}-" + (string.IsNullOrEmpty(t06.ConsigneeDesig) ? "" : t06.ConsigneeDesig + "/") +
                                               (string.IsNullOrEmpty(t06.ConsigneeDept) ? "" : t06.ConsigneeDept + "/") +
                                               (string.IsNullOrEmpty(t06.ConsigneeFirm) ? "" : t06.ConsigneeFirm + "/") +
                                               (string.IsNullOrEmpty(t06.ConsigneeAdd1) ? "" : t06.ConsigneeAdd1 + "/") +
                                               (string.IsNullOrEmpty(t03.Location) ? "" : t03.Location + " : " + t03.City),
                                   DelvDate = "01-JAN-01"
                               }).ToList();

                var query22 = (from t15 in context.T15PoDetails
                               join t06 in context.T06Consignees on t15.ConsigneeCd equals t06.ConsigneeCd
                               join t03 in context.T03Cities on t06.ConsigneeCity equals t03.CityCd
                               where t15.CaseNo == CNO &&
                                     !(from t18 in context.T18CallDetails
                                       where t18.CaseNo == CNO &&
                             t18.CallRecvDt == DateTime.ParseExact(DT, "dd/MM/yyyy", CultureInfo.InvariantCulture) &&
                             t18.CallSno == (int)model.CallSno
                                       select t18.ItemSrnoPo).Contains(t15.ItemSrno)
                               select new VenderCallRegisterModel
                               {
                                   Status = "Available",
                                   ItemSrnoPo = t15.ItemSrno,
                                   ItemDescPo = t15.ItemDesc,
                                   QtyOrdered = t15.Qty,
                                   CumQtyPrevOffered = 0,
                                   CumQtyPrevPassed = 0,
                                   QtyToInsp = 0,
                                   QtyPassed = 0,
                                   QtyRejected = 0,
                                   QtyDue = 0,
                                   Consignee = $"{t06.ConsigneeCd}-" + (string.IsNullOrEmpty(t06.ConsigneeDesig) ? "" : t06.ConsigneeDesig + "/") +
                                               (string.IsNullOrEmpty(t06.ConsigneeDept) ? "" : t06.ConsigneeDept + "/") +
                                               (string.IsNullOrEmpty(t06.ConsigneeFirm) ? "" : t06.ConsigneeFirm + "/") +
                                               (string.IsNullOrEmpty(t06.ConsigneeAdd1) ? "" : t06.ConsigneeAdd1 + "/") +
                                               (string.IsNullOrEmpty(t03.Location) ? "" : t03.Location + " : " + t03.City),
                                   DelvDate = t15.ExtDelvDt.HasValue ? t15.ExtDelvDt.Value.ToString("dd/MM/yyyy") : "01-JAN-01"
                               }).ToList();

                //query11.AddRange(query22);

                var combinedQuery = query11.Union(query22).OrderByDescending(item => item.Status).ThenBy(item => item.ItemSrnoPo);

                var results = combinedQuery.ToList();
            }
            catch (Exception ex)
            {
            }
            finally
            {
            }


        }

        int check_desire_dt(VenderCallRegisterModel model, string ext_delv_dt)
        {
            if (ext_delv_dt == "01-JAN-01")
            {
                return (2);
            }
            else
            {
                System.DateTime w_dt1 = new System.DateTime(Convert.ToInt32(ext_delv_dt.Substring(6, 4)), Convert.ToInt32(ext_delv_dt.Substring(3, 2)), Convert.ToInt32(ext_delv_dt.Substring(0, 2)));
                System.DateTime w_dt2 = new System.DateTime(Convert.ToInt32(model.DtInspDesire.ToString().Substring(6, 4)), Convert.ToInt32(model.DtInspDesire.ToString().Substring(3, 2)), Convert.ToInt32(model.DtInspDesire.ToString().Substring(0, 2)));
                TimeSpan ts = w_dt1 - w_dt2;
                int differenceInDays = ts.Days;
                if (differenceInDays < 5)
                {
                    return (1);
                }
                else
                {
                    return (0);
                }
            }

        }

        int FindIeCODE(VenderCallRegisterModel model)
        {
            string department1 = string.Empty;
            int strval = 0;
            int Clustercode;
            int vcode = 0;
            string region = model.SetRegionCode.ToString();

            if (region == "N")
            {
                department1 = model.CallRemarkStatus;
                if (department1 == "M")
                {
                    department1 = "M";
                }
                else if (department1 == "E")
                {
                    department1 = "E";
                }
                else
                {
                    department1 = "M";
                }
            }
            else
            {
                department1 = model.CallRemarkStatus;
            }

            vcode = Convert.ToInt32(model.MfgCd);
            var distinctClusterCodes = (from a in context.T100VenderClusters
                                        join b in context.T99ClusterMasters on a.ClusterCode equals (byte)b.ClusterCode
                                        where a.VendorCode == vcode && a.DepartmentName == department1 && b.RegionCode == region
                                        select b.ClusterCode
                                        ).Distinct().ToList();
            if (distinctClusterCodes.Count > 0)
            {
                Clustercode = distinctClusterCodes[0];
                if (Clustercode == 0)
                {
                    strval = 0;
                }
                else
                {
                    var GetIeCodes = context.T101IeClusters.Where(x => x.ClusterCode == Clustercode && x.DepartmentCode == department1).FirstOrDefault();
                    if (GetIeCodes != null)
                    {
                        int ieCode = Convert.ToInt32(GetIeCodes.IeCode);
                        DateTime startDate = new DateTime(2017, 1, 1);

                        int callStatusCount = context.T17CallRegisters
                            .Join(context.T09Ies,
                                a => a.IeCd,
                                b => b.IeCd,
                                (a, b) => new { CallRegister = a, IE = b })
                            .Where(joined => joined.CallRegister.CallStatus == "M" || joined.CallRegister.CallStatus == "S")
                            .Where(joined => joined.CallRegister.IeCd == ieCode)
                            .Where(joined => joined.CallRegister.CallRecvDt > startDate)
                            .Count();

                        if (callStatusCount > 0)
                        {
                            int countcalls = callStatusCount;
                            var ieCallMarking = context.T09Ies.Where(ie => ie.IeCd == ieCode).Select(ie => ie.IeCallMarking).FirstOrDefault();
                            if (Convert.ToInt32(ieCode) == 0)
                            {
                                strval = 0;
                            }
                            else
                            {
                                string callmarking = ieCallMarking;
                                if (callmarking == "")
                                {
                                    strval = 0;
                                }
                                if (callmarking == "N")
                                {

                                }
                                var maximumCall = context.T102IeMaximumCallLimits.Where(limit => limit.RegionCode == region).Select(limit => limit.MaximumCall).FirstOrDefault();
                                int Maximumcalls = Convert.ToInt32(maximumCall);
                                if (countcalls < Maximumcalls && callmarking == "Y")
                                {
                                    strval = ieCode;
                                }
                                else
                                {
                                    var altIe = context.T09Ies.Where(ie => ie.IeCd == ieCode).Select(ie => ie.AltIe).FirstOrDefault();
                                    if (altIe != null)
                                    {
                                        int Alt_ieCode = Convert.ToInt32(altIe);
                                        DateTime startDate1 = new DateTime(2017, 1, 1);

                                        int callStatusCount1 = context.T17CallRegisters
                                            .Join(context.T09Ies,
                                                a => a.IeCd,
                                                b => b.IeCd,
                                                (a, b) => new { CallRegister = a, IE = b })
                                            .Where(joined => joined.CallRegister.CallStatus == "M" || joined.CallRegister.CallStatus == "S")
                                            .Where(joined => joined.CallRegister.IeCd == Alt_ieCode)
                                            .Where(joined => joined.CallRegister.CallRecvDt > startDate1)
                                            .Count();
                                        if (callStatusCount1 > 0)
                                        {
                                            int countcalls123 = callStatusCount1;
                                            var ieCallMarking1 = context.T09Ies.Where(ie => ie.IeCd == Alt_ieCode).Select(ie => ie.IeCallMarking).FirstOrDefault();

                                            if (Alt_ieCode == 0)
                                            {
                                                strval = 0;
                                            }
                                            else
                                            {
                                                string callmarkings = ieCallMarking1;
                                                if (callmarkings == "")
                                                {
                                                    strval = 0;
                                                }
                                                if (callmarkings == "N")
                                                {

                                                }
                                                var maximumCall1 = context.T102IeMaximumCallLimits.Where(limit => limit.RegionCode == region).Select(limit => limit.MaximumCall).FirstOrDefault();
                                                if (maximumCall1 != null)
                                                {
                                                    int Maximumcalls1 = Convert.ToInt32(maximumCall1);
                                                    if (countcalls123 < Maximumcalls1 && callmarkings == "Y")
                                                    {
                                                        strval = Alt_ieCode;
                                                    }
                                                    else
                                                    {
                                                        var altIeTwo = context.T09Ies.Where(ie => ie.IeCd == ieCode).Select(ie => ie.AltIeTwo).FirstOrDefault();
                                                        if (altIeTwo != null)
                                                        {
                                                            strval = 0;
                                                        }
                                                        else
                                                        {
                                                            int Alt_ieCode_TWO = Convert.ToInt32(altIeTwo);
                                                            DateTime startDate2 = new DateTime(2017, 1, 1);

                                                            int callStatusCount2 = context.T17CallRegisters
                                                                .Join(context.T09Ies,
                                                                    a => a.IeCd,
                                                                    b => b.IeCd,
                                                                    (a, b) => new { CallRegister = a, IE = b })
                                                                .Where(joined => joined.CallRegister.CallStatus == "M" || joined.CallRegister.CallStatus == "S")
                                                                .Where(joined => joined.CallRegister.IeCd == Alt_ieCode_TWO)
                                                                .Where(joined => joined.CallRegister.CallRecvDt > startDate2)
                                                                .Count();
                                                            if (callStatusCount2 > 0)
                                                            {
                                                                int countcalls1234 = callStatusCount2;
                                                                var ieCallMarking2 = context.T09Ies.Where(ie => ie.IeCd == Alt_ieCode_TWO).Select(ie => ie.IeCallMarking).FirstOrDefault();
                                                                if (Convert.ToInt32(Alt_ieCode_TWO) == 0)
                                                                {
                                                                    strval = 0;
                                                                }
                                                                else
                                                                {
                                                                    string callmarkings1 = ieCallMarking2;
                                                                    if (callmarkings1 == "")
                                                                    {
                                                                        strval = 0;
                                                                    }
                                                                    if (callmarkings1 == "N")
                                                                    {

                                                                    }
                                                                    var maximumCall2 = context.T102IeMaximumCallLimits.Where(limit => limit.RegionCode == region).Select(limit => limit.MaximumCall).FirstOrDefault();
                                                                    if (maximumCall2 != null)
                                                                    {
                                                                        int Maximumcalls12 = Convert.ToInt32(maximumCall2);
                                                                        if (countcalls1234 < Maximumcalls12 && callmarkings1 == "Y")
                                                                        {
                                                                            strval = Alt_ieCode_TWO;
                                                                        }
                                                                        else
                                                                        {
                                                                            var altIeThree = context.T09Ies.Where(ie => ie.IeCd == ieCode).Select(ie => ie.AltIeThree).FirstOrDefault();
                                                                            if (altIeThree != null)
                                                                            {
                                                                                strval = 0;
                                                                            }
                                                                            else
                                                                            {
                                                                                int Alt_ieCode_THREE = Convert.ToInt32(altIeThree);
                                                                                DateTime startDate3 = new DateTime(2017, 1, 1);

                                                                                int callStatusCount3 = context.T17CallRegisters
                                                                                    .Join(context.T09Ies,
                                                                                        a => a.IeCd,
                                                                                        b => b.IeCd,
                                                                                        (a, b) => new { CallRegister = a, IE = b })
                                                                                    .Where(joined => joined.CallRegister.CallStatus == "M" || joined.CallRegister.CallStatus == "S")
                                                                                    .Where(joined => joined.CallRegister.IeCd == Alt_ieCode_THREE)
                                                                                    .Where(joined => joined.CallRegister.CallRecvDt > startDate3)
                                                                                    .Count();
                                                                                if (callStatusCount3 > 0)
                                                                                {
                                                                                    int countcalls1233 = callStatusCount3;
                                                                                    var ieCallMarking3 = context.T09Ies.Where(ie => ie.IeCd == Alt_ieCode_THREE).Select(ie => ie.IeCallMarking).FirstOrDefault();
                                                                                    if (Alt_ieCode_THREE == 0)
                                                                                    {
                                                                                        strval = 0;
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        string callmarkings123 = ieCallMarking3;
                                                                                        if (callmarkings123 == "")
                                                                                        {
                                                                                            strval = 0;
                                                                                        }
                                                                                        if (callmarkings123 == "N")
                                                                                        {
                                                                                        }
                                                                                        var maximumCall3 = context.T102IeMaximumCallLimits.Where(limit => limit.RegionCode == region).Select(limit => limit.MaximumCall).FirstOrDefault();
                                                                                        if (maximumCall3 != null)
                                                                                        {
                                                                                            int Maximumcalls131 = Convert.ToInt32(maximumCall3);
                                                                                            if (countcalls1233 < Maximumcalls131 && callmarkings123 == "Y")
                                                                                            {
                                                                                                strval = Alt_ieCode_THREE;
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                strval = 0;
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                    return strval;
                                                                                }
                                                                            }
                                                                            return strval;
                                                                        }
                                                                        return strval;
                                                                    }
                                                                }
                                                                return strval;
                                                            }
                                                        }
                                                    }
                                                    return strval;
                                                }
                                            }
                                            return strval;
                                        }
                                    }
                                }
                                return strval;
                            }
                        }
                        return strval;
                    }

                }
            }
            return strval;
        }

        public async Task<string> send_IE_smsAsync(VenderCallRegisterModel model)
        {
            string sms = "";
            string sender = "";
            string wIEMobile = "", wIEName = "", wVendor = "", wCOMobile = "", wVendMobile = "", wIEMobile_for_SMS = "";
            if (model.CaseNo.ToString().Substring(0, 1) == "N") { sender = "NR"; }
            else if (model.CaseNo.ToString().Substring(0, 1) == "W") { sender = "WR"; }
            else if (model.CaseNo.ToString().Substring(0, 1) == "E") { sender = "ER"; }
            else if (model.CaseNo.ToString().Substring(0, 1) == "S") { sender = "SR"; }
            else if (model.CaseNo.ToString().Substring(0, 1) == "C") { sender = "CR"; }
            else { sender = "RITES"; }

            var query = from t09 in context.T09Ies
                        join t08 in context.T08IeControllOfficers
                        on t09.IeCoCd equals t08.CoCd into t08Group
                        from t08 in t08Group.DefaultIfEmpty()
                        where t09.IeCd == model.IeCd
                        select new
                        {
                            IE_NAME = t09.IeName.Trim().Substring(0, Math.Min(t09.IeName.Trim().Length, 20)),
                            IE_PHONE_NO = t09.IePhoneNo.Trim().Substring(0, Math.Min(t09.IePhoneNo.Trim().Length, 10)),
                            CO_PHONE_NO = t08 != null ? t08.CoPhoneNo.Trim().Substring(0, Math.Min(t08.CoPhoneNo.Trim().Length, 10)) : ""
                        };

            var result = query.FirstOrDefault();

            if (result != null)
            {
                wIEName = result.IE_NAME;
                wIEMobile = result.IE_PHONE_NO;
                wIEMobile_for_SMS = result.IE_PHONE_NO;
                wCOMobile = result.CO_PHONE_NO;
            }

            var queryNew = from v in context.T05Vendors
                           join c in context.T03Cities
                           on v.VendCityCd equals c.CityCd
                           where v.VendCd == model.MfgCd
                           select new
                           {
                               VEND_NAME = v.VendName.Substring(0, Math.Min(v.VendName.Length, 30)).Replace("&", "AND"),
                               VEND_TEL = v.VendContactTel1.Trim().Substring(0, Math.Min(v.VendContactTel1.Trim().Length, 10))
                           };

            var resultNew = queryNew.FirstOrDefault();

            if (resultNew != null)
            {
                wVendor = resultNew.VEND_NAME;
                wVendMobile = resultNew.VEND_TEL;
            }

            if (wCOMobile != "") { wIEMobile = wIEMobile + "," + wCOMobile; }
            if (wVendMobile != "") { wIEMobile = wIEMobile + "," + wVendMobile; }
            string message = "RITES LTD - QA Call Marked, IE-" + wIEName + ",Contact No.:" + wIEMobile_for_SMS + ",RLY-" + model.Rly + ",PO-" + model.PoNo + ",DT- " + model.PoDt + ", Firm Name-" + wVendor + ", Call Sno - " + model.CallSno + ",DT- " + model.CallRecvDt + "- RITES/" + sender;

            using (HttpClient client = new HttpClient())
            {
                string baseurl = $"http://apin.onex-aura.com/api/sms?key=QtPr681q&to={wIEMobile}&from=RITESI&body={message}&entityid=1501628520000011823&templateid=1707161588918541674";

                HttpResponseMessage response = await client.GetAsync(baseurl);
                response.EnsureSuccessStatusCode(); // Ensure a successful response

                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);

                sms = "success";
            }


            return sms;
        }

        public string send_Vendor_Email(VenderCallRegisterModel model)
        {
            string email = "";
            string Case_Region = model.CaseNo.ToString().Substring(0, 1);
            string wRegion = "";
            string sender = "";

            if (Case_Region == "N") { wRegion = "NORTHERN REGION <BR>12th FLOOR,CORE-II,SCOPE MINAR,LAXMI NAGAR, DELHI - 110092 <BR>Phone : +918800018691-95 <BR>Fax : 011-22024665"; sender = "nrinspn@rites.com"; }
            else if (Case_Region == "S") { wRegion = "SOUTHERN REGION <BR>CTS BUILDING - 2ND FLOOR, BSNL COMPLEX, NO. 16, GREAMS ROAD,  CHENNAI - 600 006 <BR>Phone : 044-28292807/044- 28292817 <BR>Fax : 044-28290359"; sender = "srinspn@rites.com"; }
            else if (Case_Region == "E") { wRegion = "EASTERN REGION <BR>CENTRAL STATION BUILDING(METRO), 56, C.R. AVENUE,3rd FLOOR,KOLKATA-700 012  <BR>Fax : 033-22348704"; sender = "erinspn@rites.com"; }
            else if (Case_Region == "W") { wRegion = "WESTERN REGION <BR>5TH FLOOR, REGENT CHAMBER, ABOVE STATUS RESTAURANT,NARIMAN POINT,MUMBAI-400021 <BR>Phone : 022-68943400/68943445 <BR>"; sender = "wrinspn@rites.com"; }
            else if (Case_Region == "C") { wRegion = "Central Region"; }

            var query = from t13 in context.T13PoMasters
                        join t05 in context.T05Vendors on t13.VendCd equals t05.VendCd
                        join t03 in context.T03Cities on t05.VendCityCd equals t03.CityCd
                        where t13.CaseNo == model.CaseNo
                        select new
                        {
                            VEND_CD = t13.VendCd,
                            VEND_NAME = t05.VendName,
                            VEND_ADDRESS = t05.VendAdd2 != null ? $"{t05.VendAdd1}/{t05.VendAdd2}" : t05.VendAdd1 + "/" + t03.City,
                            VEND_EMAIL = t05.VendEmail
                        };

            var result = query.FirstOrDefault();

            int vend_cd = 0;
            string vend_add = "";
            string vend_email = "";

            if (result != null)
            {
                vend_cd = Convert.ToInt32(result.VEND_CD);
                vend_add = result.VEND_ADDRESS;
                vend_email = result.VEND_EMAIL;
            }

            var query1 = from t05 in context.T05Vendors
                         join t17 in context.T17CallRegisters
                         on t05.VendCd equals t17.MfgCd
                         where t17.CaseNo == model.CaseNo &&
                               t17.CallRecvDt == model.CallRecvDt &&
                               t17.CallSno == model.CallSno
                         select new
                         {
                             VEND_EMAIL = t05.VendEmail,
                             MFG_CD = t17.MfgCd,
                             DESIRE_DT = t17.DtInspDesire
                         };

            var result1 = query1.FirstOrDefault();

            string manu_mail = "";
            int mfg_cd = 0;
            string desire_dt = null;

            if (result1 != null)
            {
                manu_mail = result1.VEND_EMAIL;
                mfg_cd = Convert.ToInt32(result1.MFG_CD);
                desire_dt = Convert.ToString(result1.DESIRE_DT);

            }
            var query2 = from t09 in context.T09Ies
                         join t08 in context.T08IeControllOfficers
                         on t09.IeCoCd equals t08.CoCd
                         where t09.IeCd == model.IeCd
                         select new
                         {
                             IE_PHONE_NO = t09.IePhoneNo,
                             CO_NAME = t08.CoName,
                             CO_PHONE_NO = t08.CoPhoneNo,
                             IE_NAME = t09.IeName,
                             IE_EMAIL = t09.IeEmail
                         };

            var result2 = query2.FirstOrDefault();

            string ie_phone = "";
            string co_name = "";
            string co_mobile = "";
            string ie_name = "";
            string ie_email = "";

            if (result2 != null)
            {
                ie_phone = result2.IE_PHONE_NO;
                co_name = result2.CO_NAME;
                co_mobile = result2.CO_PHONE_NO;
                ie_name = result2.IE_NAME;
                ie_email = result2.IE_EMAIL;

                // Use ie_phone, co_name, co_mobile, ie_name, ie_email as needed
            }

            var subquery = from t17 in context.T17CallRegisters
                           where t17.CallRecvDt > DateTime.ParseExact("01-APR-2017", "dd-MM-yyyy", null) &&
                                 (t17.CallStatus == "M" || t17.CallStatus == "S") &&
                                 t17.IeCd == model.IeCd
                           select t17;

            var query3 = from t17 in context.T17CallRegisters
                         where t17.CaseNo == model.CaseNo &&
                               t17.CallRecvDt == model.CallRecvDt &&
                               t17.CallSno == model.CallSno
                         select new
                         {
                             INSP_DATE = Convert.ToDateTime(t17.DtInspDesire).AddDays(subquery.Count() / 1.5).ToString("dd/MM/yyyy")
                         };

            var result3 = query3.FirstOrDefault();
            string dateto_attend = "";
            if (result3 != null)
            {
                dateto_attend = result3.INSP_DATE;
            }

            var recordToUpdate = context.T17CallRegisters.FirstOrDefault(t17 => t17.CaseNo == model.CaseNo &&
                            t17.CallRecvDt == model.CallRecvDt && t17.CallSno == model.CallSno);

            if (recordToUpdate != null)
            {
                recordToUpdate.ExpInspDt = DateTime.ParseExact(dateto_attend, "dd/MM/yyyy", null);
                context.SaveChanges();
            }

            var query4 = from t18 in context.T18CallDetails
                         join t15 in context.T15PoDetails on t18.CaseNo equals t15.CaseNo
                         join t61 in context.T61ItemMasters on t15.ItemCd equals t61.ItemCd
                         where t18.ItemSrnoPo == t15.ItemSrno && t15.CaseNo == model.CaseNo
                         group new { t61.TimeForInsp, t61.ItemCd } by t61.ItemCd into grouped
                         select new
                         {
                             ItemCd = grouped.Key,
                             DaysToIc = grouped.Max(g => g.TimeForInsp)
                         };

            var result4 = query4.ToList();

            int days_to_ic = 0;
            string item_cd = "";

            if (result4.Count > 0)
            {
                days_to_ic = Convert.ToInt32(result4[0].DaysToIc);
                item_cd = result4[0].ItemCd;
            }
            string call_letter_dt = "";
            if (Convert.ToString(model.CallLetterDt) == "")
            {
                call_letter_dt = "NIL";
            }
            else
            {
                call_letter_dt = Convert.ToString(model.CallLetterDt);
            }

            string mail_body = "Dear Sir/Madam,<br><br> In Reference to your Call Letter dated:  " + call_letter_dt + " for inspection of material against PO No. - " + model.PoNo + " & date - " + model.PoDt + ", Call has been registered vide Case No -  " + model.CaseNo + ", on date: " + model.CallRecvDt + ", at SNo. " + model.CallSno + ".<br> ";
            if (model.CallRecvDt != Convert.ToDateTime(desire_dt.Trim()))
            {
                mail_body = mail_body + "The Desired Inspection Date of this call shall be on or after: " + Convert.ToDateTime(desire_dt.Trim()) + ".<br>";
            }
            if (days_to_ic == 0)
            {
                mail_body = mail_body + "The inspection call has been assigned to Inspecting Engineer Sh. " + ie_name + ", Contact No. " + ie_phone + ", Email ID: " + ie_email + ". Based on the current workload with the IE, Inspection is likely to be attended on or before " + dateto_attend + " or next working day (In case the above date happens to be a holiday). Dates are subject to last minute changes due to  exigencies of work and overriding Client priorities. <br> Name of Controlling Manager of concerned IE Sh.: " + co_name + ", Contact No." + co_mobile + ". <br>Offered Material as per registration should be readily available on the indicated date along with all related documents and internal test reports.<br><a href='http://rites.ritesinsp.com/RBS/Guidelines for Vendors.pdf'>Guidelines for Vendors</a>.<br>For Inspection related information please visit : http://ritesinsp.com. <br> For any correspondence in future, please quote Case No. only.<br><br> Thanks for using RITES Inspection Services. <br><br>" + wRegion + ".";
            }
            else if (days_to_ic > 0)
            {
                System.DateTime w_dt1 = new System.DateTime(Convert.ToInt32(dateto_attend.Substring(6, 4)), Convert.ToInt32(dateto_attend.Substring(3, 2)), Convert.ToInt32(dateto_attend.Substring(0, 2)));
                System.DateTime w_dt2 = w_dt1.AddDays(days_to_ic);
                string date_to_ic = w_dt2.ToString("dd/MM/yyyy");
                mail_body = mail_body + "The inspection call has been assigned to Inspecting Engineer Sh. " + ie_name + ", Contact No. " + ie_phone + ", Email ID: " + ie_email + ". Based on the current workload with the IE, Inspection is likely to be attended on or before " + dateto_attend + " or next working day (In case the above date happens to be a holiday) and Inspection certificate is likely to issued by " + date_to_ic + ". Dates are subject to last minute changes due to  exigencies of work and overriding Client priorities. <br> Name of Controlling Manager of concerned IE Sh.: " + co_name + ", Contact No." + co_mobile + ". <br>Offered Material as per registration should be readily available on the indicated date along with all related documents and internal test reports. Inspection is proposed to be conducted as per inspection plan: <a href='http://rites.ritesinsp.com/RBS/MASTER_ITEMS_CHECKSHEETS/" + item_cd + ".RAR'>Inspection Plan</a>.<br><a href='http://rites.ritesinsp.com/RBS/Guidelines for Vendors.pdf'>Guidelines for Vendors</a>.<br>For Inspection related information please visit : http://ritesinsp.com. <br> For any correspondence in future, please quote Case No. only. <br><br> Thanks for using RITES Inspection Services. <br> NATIONAL INSPECTION HELP LINE NUMBER : 1800 425 7000 (TOLL FREE).<br><br>" + wRegion + ".";
            }
            mail_body = mail_body + "<br><br> THIS IS AN AUTO GENERATED EMAIL. PLEASE DO NOT REPLY. USE EMAIL GIVEN IN THE REGION ADDRESS.";
            if (Case_Region == "N")
            {
                sender = "nrinspn@rites.com";
            }
            else if (Case_Region == "W")
            {
                sender = "wrinspn@rites.com";
            }
            else if (Case_Region == "E")
            {
                sender = "erinspn@rites.com";
            }
            else if (Case_Region == "S")
            {
                sender = "srinspn@rites.com";
            }
            else if (Case_Region == "C")
            {
                sender = "crinspn@rites.com";
            }

            if (vend_cd == mfg_cd && manu_mail != "")
            {
                // Create a MailMessage object
                MailMessage mail = new MailMessage();
                mail.To.Add(manu_mail);
                mail.Bcc.Add("nrinspn@gmail.com");
                mail.From = new MailAddress("nrinspn@gmail.com");
                mail.Subject = "Your Call for Inspection By RITES";
                mail.IsBodyHtml = true; // Set to true if the body contains HTML content
                mail.Body = mail_body;

                // Create a SmtpClient
                SmtpClient smtpClient = new SmtpClient("10.60.50.81"); // Set your SMTP server address
                smtpClient.Credentials = new NetworkCredential("bhavesh.rathod@silvertouch.com", "RB_rathod@123"); // If authentication is required
                                                                                                                   // Send the email
                try
                {
                    smtpClient.Send(mail);
                }
                catch (Exception ex)
                {
                    // Handle the exception (log, display error message, etc.)
                }
                finally
                {
                    // Dispose of resources
                    mail.Dispose();
                    smtpClient.Dispose();
                }
            }
            else if (vend_cd != mfg_cd && vend_email != "" && manu_mail != "")
            {
                // Create a MailMessage object
                MailMessage mail = new MailMessage();
                mail.To.Add(vend_email);
                mail.To.Add(manu_mail);
                mail.Bcc.Add("nrinspn@gmail.com");
                mail.From = new MailAddress("nrinspn@gmail.com");
                mail.Subject = "Your Call for Inspection By RITES";
                mail.IsBodyHtml = true; // Set to true if the body contains HTML content
                mail.Body = mail_body;

                // Create a SmtpClient
                SmtpClient smtpClient = new SmtpClient("10.60.50.81"); // Set your SMTP server address
                smtpClient.Credentials = new NetworkCredential("bhavesh.rathod@silvertouch.com", "RB_rathod@123"); // If authentication is required

                // Send the email
                try
                {
                    smtpClient.Send(mail);
                }
                catch (Exception ex)
                {
                    // Handle the exception (log, display error message, etc.)
                }
                finally
                {
                    // Dispose of resources
                    mail.Dispose();
                    smtpClient.Dispose();
                }
            }
            else if (vend_cd != mfg_cd && (vend_email == "" || manu_mail == ""))
            {
                // Create a MailMessage object
                MailMessage mail = new MailMessage();

                if (string.IsNullOrEmpty(vend_email))
                {
                    mail.To.Add(manu_mail);
                }
                else if (string.IsNullOrEmpty(manu_mail))
                {
                    mail.To.Add(vend_email);
                }
                else
                {
                    mail.To.Add(vend_email);
                    mail.To.Add(manu_mail);
                }

                mail.Bcc.Add("nrinspn@gmail.com");
                mail.From = new MailAddress("nrinspn@gmail.com");
                mail.Subject = "Your Call for Inspection By RITES";
                mail.IsBodyHtml = true; // Set to true if the body contains HTML content
                mail.Body = mail_body;

                // Create a SmtpClient
                SmtpClient smtpClient = new SmtpClient("10.60.50.81"); // Set your SMTP server address
                smtpClient.Credentials = new NetworkCredential("bhavesh.rathod@silvertouch.com", "RB_rathod@123"); // If authentication is required

                // Send the email
                try
                {
                    smtpClient.Send(mail);
                }
                catch (Exception ex)
                {
                    // Handle the exception (log, display error message, etc.)
                }
                finally
                {
                    // Dispose of resources
                    mail.Dispose();
                    smtpClient.Dispose();
                }
            }

            var controllingEmail = (from t08 in context.T08IeControllOfficers
                                    join t09 in context.T09Ies on t08.CoCd equals t09.IeCoCd
                                    where t09.IeCd == model.IeCd
                                    select t08.CoEmail
                                    ).FirstOrDefault();

            string manu_name = "", manu_add = "";
            var manufacturerInfo = (from t17 in context.T17CallRegisters
                                    join t05 in context.T05Vendors on t17.MfgCd equals t05.VendCd
                                    join t03 in context.T03Cities on t05.VendCityCd equals t03.CityCd
                                    where t17.CaseNo == model.CaseNo &&
                                    t17.CallRecvDt == model.CallRecvDt &&
                                    t17.CallSno == model.CallSno
                                    select new
                                    {
                                        manu_name = t05.VendName,
                                        manu_add = t03.City
                                    }).FirstOrDefault();
            if (controllingEmail != "")
            {
                MailMessage mail2 = new MailMessage();

                mail2.To.Add(controllingEmail);
                mail2.Bcc.Add("nrinspn@gmail.com");
                if (!string.IsNullOrEmpty(ie_email))
                {
                    mail2.CC.Add(ie_email);
                }
                mail2.From = new MailAddress("nrinspn@gmail.com");
                mail2.Subject = "Your Call (" + manu_name + " - " + manu_add + ") for Inspection By RITES";
                mail2.IsBodyHtml = true;
                mail2.Body = mail_body;

                // Create a SmtpClient
                SmtpClient smtpClient2 = new SmtpClient("10.60.50.81"); // Set your SMTP server address
                smtpClient2.Credentials = new NetworkCredential("bhavesh.rathod@silvertouch.com", "RB_rathod@123"); // If authentication is required

                try
                {
                    smtpClient2.Send(mail2);
                    email = "success";
                }
                catch (Exception ex)
                {
                    // Handle the exception (log, display error message, etc.)
                }
                finally
                {
                    // Dispose of resources
                    mail2.Dispose();
                    smtpClient2.Dispose();
                }
            }
            return email;
        }

        public string RegiserCallDelete(VenderCallRegisterModel model)
        {
            string msg = "";
            var CallReg = context.T17CallRegisters.FirstOrDefault(x => x.CaseNo == model.CaseNo && x.CallRecvDt == model.CallRecvDt && x.CallSno == model.CallSno);

            if (CallReg != null)
            {
                context.T17CallRegisters.Remove(CallReg);
                context.SaveChanges();
                msg = CallReg.CaseNo;
            }


            return msg;
        }

        public VendorCallRegPrintReport FindByPrintReport(string CaseNo, string CallRecvDt, int CallSno, string UserName)
        {
            VendorCallRegPrintReport model = new();

            DateTime? _CallRecvDt = CallRecvDt == "" ? null : DateTime.ParseExact(CallRecvDt, "dd-MM-yyyy", null);

            var GetReport = context.ViewGetCallinspectionPrintReports.Where(x => x.CaseNo == CaseNo && x.CallRecvDt == _CallRecvDt && x.CallSno == CallSno).FirstOrDefault();
            if (GetReport == null)
                throw new Exception("Vender Record Not found");
            else
            {
                model.PoNo = GetReport.PoNo;
                model.PoDt = GetReport.PoDt;
                model.MfgName = GetReport.MfgName;
                model.MfgAdd = GetReport.MfgAdd;
                model.Purchaser = GetReport.Purchaser;
                model.Consignee = GetReport.Consignee;
                model.CaseNo = GetReport.CaseNo;
                model.CallRecvDt = GetReport.CallRecvDt;
                model.CallSno = GetReport.CallSno;
                model.CallLetterNo = GetReport.CallLetterNo;
                model.CallLetterDt = GetReport.CallLetterDt;
                model.CallInstallNo = GetReport.CallInstallNo;
                model.OnlineCall = GetReport.OnlineCall;
                model.FinalOrStage = GetReport.FinalOrStage;
                model.Remarks = GetReport.Remarks;
                model.ItemRdso = GetReport.ItemRdso;
                model.VendRdso = GetReport.VendRdso;
                model.VendAppFr = GetReport.VendAppFr;
                model.VendAppTo = GetReport.VendAppTo;
                model.StagDp = GetReport.StagDp;
                model.LotDp1 = GetReport.LotDp1;
                model.LotDp2 = GetReport.LotDp2;
                model.IeName = GetReport.IeName;
                model.ItemDescPo = GetReport.ItemDescPo;
                model.QtyOrdered = GetReport.QtyOrdered;
                model.QtyToInsp = GetReport.QtyToInsp;
                model.CumQtyPrevOffered = GetReport.CumQtyPrevOffered;
                model.CumQtyPrevPassed = GetReport.CumQtyPrevPassed;
                model.VendContactPer1 = GetReport.VendContactPer1;
                model.VendContactTel1 = GetReport.VendContactTel1;
                model.VendEmail = GetReport.VendEmail;
                model.Bpo = GetReport.Bpo;
                model.DelvDt = GetReport.DelvDt;
                model.ItemCd = GetReport.ItemCd;
                model.IrfcFunded = GetReport.IrfcFunded;

            }

            var GetPOReport = context.ViewGetCallinspectionPrintSubReports.Where(x => x.CaseNo == CaseNo).FirstOrDefault();
            if (GetPOReport == null)
                throw new Exception("Vender Record Not found");
            else
            {
                model.VendCd = GetPOReport.VendCd;
                model.VendName = GetPOReport.VendName;
                model.VendAdd = GetPOReport.VendAddress;
                model.VendPOEmail = GetPOReport.VendEmail;
                model.VendContactPerPO1 = GetPOReport.VendContactPer1;
                model.VendContactTelPO1 = GetPOReport.VendContactTel1;
                model.Source = GetPOReport.Source;
            }


            return model;
        }

        public DTResult<VenderCallRegisterModel> GetDataListReport(DTParameters dtParameters)
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
            string CaseNo = "", CallRecvDt = "", CallSno="";
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
            DateTime? _CallRecvDt = CallRecvDt == "" ? null : DateTime.ParseExact(CallRecvDt, "dd-MM-yyyy", null);
            CallSno = CallSno.ToString() == "" ? string.Empty : CallSno.ToString();

            query = from l in context.ViewGetCallinspectionPrintReports
                    where l.CaseNo == CaseNo && l.CallRecvDt == _CallRecvDt && l.CallSno == Convert.ToInt32(CallSno)

                    select new VenderCallRegisterModel
                    {
                        CaseNo = l.CaseNo,
                        Consignee = l.Consignee,
                        ItemDescPo = l.ItemDescPo,
                        QtyOrdered = l.QtyOrdered,
                        QtyToInsp = l.QtyToInsp,
                        CumQtyPrevOffered = l.CumQtyPrevOffered,
                        CumQtyPrevPassed = l.CumQtyPrevPassed,
                        DelvDt = l.DelvDt,
                        Bpo = l.Bpo,
                        ItemCd = l.ItemCd,
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
    }
}
