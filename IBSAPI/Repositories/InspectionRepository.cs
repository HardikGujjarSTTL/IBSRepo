using IBSAPI.DataAccess;
using IBSAPI.Interfaces;
using IBSAPI.Models;
using System.Globalization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Numerics;
using Oracle.ManagedDataAccess.Client;
using IBSAPI.Helper;
using Newtonsoft.Json;
using System.Data;
using System.Dynamic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace IBSAPI.Repositories
{
    public class InspectionRepository : IInspectionRepository
    {
        private readonly ModelContext context;
        public InspectionRepository(ModelContext context)
        {
            this.context = context;
        }

        #region IE Methods
        public List<TodayInspectionModel> GetToDayInspection(int IeCd)
        {
            List<TodayInspectionModel> todayList = new();
            var currDate = DateTime.Now.ToString("dd/MM/yyyy");

            OracleParameter[] par = new OracleParameter[3];
            par[0] = new OracleParameter("P_IECD", OracleDbType.Varchar2, IeCd, ParameterDirection.Input);
            par[1] = new OracleParameter("P_TODAYDATE", OracleDbType.Varchar2, currDate, ParameterDirection.Input);
            par[2] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GET_IE_TODAY_INSPECTION_API", par, 1);
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                todayList = JsonConvert.DeserializeObject<List<TodayInspectionModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }).ToList();
            }
            return todayList;
        }

        public List<TomorrowInspectionModel> GetTomorrowInspection(int IeCd)
        {
            var tomoDate = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
            List<TomorrowInspectionModel> tomorrowList = new();

            OracleParameter[] par = new OracleParameter[3];
            par[0] = new OracleParameter("P_IECD", OracleDbType.Varchar2, IeCd, ParameterDirection.Input);
            par[1] = new OracleParameter("P_TOMORROWDATE", OracleDbType.Varchar2, tomoDate, ParameterDirection.Input);
            par[2] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GET_IE_TOMORROW_INSPECTION_API", par, 1);
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                tomorrowList = JsonConvert.DeserializeObject<List<TomorrowInspectionModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }).ToList();
            }
            return tomorrowList;
        }

        public List<PendingInspectionModel> GetPendingInspection(int IeCd, string Region, DateTime CurrDate)
        {
            List<PendingInspectionModel> pendingList = new();
            var CurrentDate = CurrDate.ToString("dd/MM/yyyy");

            OracleParameter[] par = new OracleParameter[3];
            par[0] = new OracleParameter("P_IECD", OracleDbType.Varchar2, IeCd, ParameterDirection.Input);
            par[1] = new OracleParameter("P_DATE", OracleDbType.Varchar2, CurrentDate, ParameterDirection.Input);
            par[2] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GET_IE_PENDING_INSPECTION_API", par, 1);
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                pendingList = JsonConvert.DeserializeObject<List<PendingInspectionModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }).ToList();
            }
            return pendingList;
        }

        public CaseDetailIEModel GetCaseDetailForIE(string Case_No, DateTime CallRecvDt, int CallSNo, int IeCd)
        {
            CaseDetailIEModel caseDetailIEModel = new CaseDetailIEModel();

            OracleParameter[] par = new OracleParameter[5];
            par[0] = new OracleParameter("p_IeCd", OracleDbType.Int32, IeCd, ParameterDirection.Input);
            par[1] = new OracleParameter("p_Case_No", OracleDbType.Varchar2, Case_No, ParameterDirection.Input);
            par[2] = new OracleParameter("p_CALL_RECV_DT", OracleDbType.Date, CallRecvDt, ParameterDirection.Input);
            par[3] = new OracleParameter("p_CALL_SNO", OracleDbType.Int32, CallSNo, ParameterDirection.Input);
            par[4] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GetCaseDetailForIE_API", par, 1);
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                caseDetailIEModel = JsonConvert.DeserializeObject<List<CaseDetailIEModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }).FirstOrDefault();
            }
            var secondQuery = (from cdt in context.T18CallDetails
                               join csn in context.V06Consignees
                               on cdt.ConsigneeCd equals csn.ConsigneeCd
                               where cdt.CaseNo == Case_No &&
                                     cdt.CallRecvDt == Convert.ToDateTime(CallRecvDt) &&
                                     cdt.CallSno == CallSNo
                               select new SelectListItem
                               {
                                   Value = csn.ConsigneeCd.ToString(),
                                   Text = csn.ConsigneeCd + "-" + csn.Consignee
                               }).Distinct().ToList();
            if (secondQuery != null)
            {
                caseDetailIEModel.ConsigneeFirmList = secondQuery.ToList();
            }

            var ic_book = (from item in context.T10IcBooksets
                           orderby item.IssueDt descending
                           where item.IssueToIecd == IeCd
                           select item).FirstOrDefault();

            var ICInter = context.T17CallRegisters.Where(ic => ic.CaseNo == Case_No.Trim() && ic.CallRecvDt == Convert.ToDateTime(CallRecvDt)
                         && ic.CallSno == CallSNo).OrderByDescending(ic => ic.Datetime).FirstOrDefault();

            //if (ICInter != null)
            //{
            //    ////if (selectedConsigneeCd == ICInter.ConsigneeCd)
            //    ////{
            //    caseDetailIEModel.BK_NO = ICInter.BkNo;
            //    caseDetailIEModel.SET_NO = ICInter.SetNo;
            //    //caseDetailIEModel.Consignee = Convert.ToString(ICInter.ConsigneeCd);
            //    //caseDetailIEModel.QtyPassed = ICInter.QtyPassed;
            //    //caseDetailIEModel.QtyRejected = ICInter.QtyRejected;
            //    ////}
            //}
            //else
            //{
                if(string.IsNullOrEmpty(caseDetailIEModel.BK_NO) && string.IsNullOrEmpty(caseDetailIEModel.SET_NO))
                {
                    var dlt_IC = (from x in context.T17CallRegisters
                                  orderby Convert.ToInt32(x.SetNo) descending
                                  where x.BkNo.Trim() == ic_book.BkNo.Trim() && x.IeCd == IeCd
                                  select x).FirstOrDefault();

                    if (dlt_IC != null)
                    {
                        int setNo = Convert.ToInt32(dlt_IC.SetNo) + 1;

                        string incrementedSetNo = setNo.ToString("D3");
                        var ic_bookset = (from item in context.T10IcBooksets
                                          orderby item.IssueDt descending
                                          where item.BkNo.Trim().ToUpper() == dlt_IC.BkNo &&
                                                Convert.ToInt32(incrementedSetNo) >= Convert.ToInt32(item.SetNoFr) && Convert.ToInt32(incrementedSetNo) <= Convert.ToInt32(item.SetNoTo) &&
                                                item.IssueToIecd == dlt_IC.IeCd
                                          select item).FirstOrDefault();

                        if (ic_bookset != null)
                        {
                            caseDetailIEModel.BK_NO = ic_bookset.BkNo;
                            caseDetailIEModel.SET_NO = Convert.ToString(incrementedSetNo);
                        }
                        else
                        {
                            caseDetailIEModel.BK_NO = "";
                            caseDetailIEModel.SET_NO = "";
                        }
                    }
                    else
                    {
                        if(ic_book != null)
                        {
                            caseDetailIEModel.BK_NO = ic_book.BkNo;
                            caseDetailIEModel.SET_NO = Convert.ToString(ic_book.SetNoFr);
                        }
                        else
                        {
                            caseDetailIEModel.BK_NO = "";
                            caseDetailIEModel.SET_NO = "";
                        }
                    }
                }
            //}

            return caseDetailIEModel;
        }

        public List<DateWiseRecentInspectionModel> GetDateWiseRecentInspection(int IeCd, DateTime FromDate, DateTime ToDate)
        {
            List<DateWiseRecentInspectionModel> recentInspDetail = new List<DateWiseRecentInspectionModel>();
            var FrmDT = FromDate.ToString("dd/MM/yyyy");
            var ToDT = ToDate.ToString("dd/MM/yyyy");

            OracleParameter[] par = new OracleParameter[4];
            par[0] = new OracleParameter("P_FROMDATE", OracleDbType.Varchar2, FrmDT, ParameterDirection.Input);
            par[1] = new OracleParameter("P_TODATE", OracleDbType.Varchar2, ToDT, ParameterDirection.Input);
            par[2] = new OracleParameter("P_IECD", OracleDbType.Int32, IeCd, ParameterDirection.Input);
            par[3] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GET_DATE_WISE_RECENT_INSPECTION_API", par, 1);
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                recentInspDetail = JsonConvert.DeserializeObject<List<DateWiseRecentInspectionModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }).ToList();
            }
            return recentInspDetail;
        }

        public List<CompleteInspectionModel> GetCompleteInspection(int IeCd)
        {
            List<CompleteInspectionModel> completeInspList = new List<CompleteInspectionModel>();

            OracleParameter[] par = new OracleParameter[2];
            par[0] = new OracleParameter("P_IECD", OracleDbType.Int32, IeCd, ParameterDirection.Input);
            par[1] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GET_IE_COMPLETED_INSPECTION_API", par, 1);
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                completeInspList = JsonConvert.DeserializeObject<List<CompleteInspectionModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }).ToList();
            }
            return completeInspList;
        }
        #endregion

        #region Vendor Methods
        public List<VendorPedingInspectionModel> Get_Vendor_PendingInspection(int Vend_Cd, DateTime FromDate, DateTime ToDate)
        {
            List<VendorPedingInspectionModel> pendingInspList = new();
            var FrmDT = FromDate.ToString("dd/MM/yyyy");
            var ToDT = ToDate.ToString("dd/MM/yyyy");

            OracleParameter[] par = new OracleParameter[4];
            par[0] = new OracleParameter("P_VEND_CD", OracleDbType.Int32, Vend_Cd, ParameterDirection.Input);
            par[1] = new OracleParameter("P_FROMDATE", OracleDbType.Varchar2, FrmDT, ParameterDirection.Input);
            par[2] = new OracleParameter("P_TODATE", OracleDbType.Varchar2, ToDT, ParameterDirection.Input);
            par[3] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GET_VENDOR_PENDINGINSPECTION_API", par, 1);
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                pendingInspList = JsonConvert.DeserializeObject<List<VendorPedingInspectionModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }).ToList();
            }
            return pendingInspList;
        }

        public List<VendorPedingInspectionModel> Get_Pending_PO_For_Call(int Vend_Cd, DateTime FromDate, DateTime ToDate)
        {
            List<VendorPedingInspectionModel> pendingInspList = new();
            var FrmDT = FromDate.ToString("dd/MM/yyyy");
            var ToDT = ToDate.ToString("dd/MM/yyyy");

            OracleParameter[] par = new OracleParameter[4];
            par[0] = new OracleParameter("P_VEND_CD", OracleDbType.Int32, Vend_Cd, ParameterDirection.Input);
            par[1] = new OracleParameter("P_FROMDATE", OracleDbType.Varchar2, FrmDT, ParameterDirection.Input);
            par[2] = new OracleParameter("P_TODATE", OracleDbType.Varchar2, ToDT, ParameterDirection.Input);
            par[3] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GET_VENDOR_PENDING_PO_FOR_CALL_API", par);
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                pendingInspList = JsonConvert.DeserializeObject<List<VendorPedingInspectionModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }).ToList();
            }
            return pendingInspList;
        }
        #endregion

        #region CM Methods
        public List<RecentInspectionModel> Get_CM_RecentInspection(int CO_CD, int IE_CD, DateTime CurrDate)
        {
            List<RecentInspectionModel> recentInspList = new();
            OracleParameter[] par = new OracleParameter[4];
            par[0] = new OracleParameter("P_VISITDATE", OracleDbType.Varchar2, CurrDate.ToString("dd/MM/yyyy"), ParameterDirection.Input);
            par[1] = new OracleParameter("P_CO_CD", OracleDbType.Int32, CO_CD, ParameterDirection.Input);
            par[2] = new OracleParameter("P_IE_CD", OracleDbType.Int32, IE_CD, ParameterDirection.Input);
            par[3] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GET_CM_RECENTINSPECTION_API", par, 1);
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                recentInspList = JsonConvert.DeserializeObject<List<RecentInspectionModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }).ToList();
            }
            return recentInspList;
        }
        #endregion

        #region Client Methods
        public List<PendingInspectionModel> Get_Client_PendingInspection(string Rly_CD, string Rly_NonType, DateTime FromDate, DateTime ToDate)
        {
            List<PendingInspectionModel> pendingInspList = new();
            var FrmDT = FromDate.ToString("dd/MM/yyyy");
            var ToDT = ToDate.ToString("dd/MM/yyyy");

            OracleParameter[] par = new OracleParameter[5];
            par[0] = new OracleParameter("P_RLY_CD", OracleDbType.Varchar2, Rly_CD, ParameterDirection.Input);
            par[1] = new OracleParameter("P_RLY_NONTYPE", OracleDbType.Varchar2, Rly_NonType, ParameterDirection.Input);
            par[2] = new OracleParameter("P_FROMDATE", OracleDbType.Varchar2, FrmDT, ParameterDirection.Input);
            par[3] = new OracleParameter("P_TODATE", OracleDbType.Varchar2, ToDT, ParameterDirection.Input);
            par[4] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GET_CLIENT_PENDINGINSPECTION_API", par, 1);
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                pendingInspList = JsonConvert.DeserializeObject<List<PendingInspectionModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }).ToList();
            }
            return pendingInspList;
        }

        public List<PendingInspectionModel> Get_Client_Region_Wise_PendingInspection(string Rly_CD, string Rly_NonType, string PO_NO, string Region, DateTime FromDate, DateTime ToDate)
        {
            List<PendingInspectionModel> pendingInspList = new();
            var FrmDT = FromDate.ToString("dd/MM/yyyy");
            var ToDT = ToDate.ToString("dd/MM/yyyy");
            Region = string.IsNullOrEmpty(Region) ? null : Region;
            OracleParameter[] par = new OracleParameter[7];
            par[0] = new OracleParameter("P_RLY_CD", OracleDbType.Varchar2, Rly_CD, ParameterDirection.Input);
            par[1] = new OracleParameter("P_RLY_NONTYPE", OracleDbType.Varchar2, Rly_NonType, ParameterDirection.Input);
            par[2] = new OracleParameter("P_FROMDATE", OracleDbType.Varchar2, FrmDT, ParameterDirection.Input);
            par[3] = new OracleParameter("P_TODATE", OracleDbType.Varchar2, ToDT, ParameterDirection.Input);
            par[4] = new OracleParameter("P_PO_NO", OracleDbType.Varchar2, PO_NO, ParameterDirection.Input);
            par[5] = new OracleParameter("P_REGION", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            par[6] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GET_CLIENT_REGION_WISE_PENDINGINSPECTION_API", par, 1);
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                pendingInspList = JsonConvert.DeserializeObject<List<PendingInspectionModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }).ToList();
            }
            return pendingInspList;
        }
        #endregion


        public List<PhotosModel> GetDocRecordsList(int DocumentCategoryID, string ApplicationID, string WebRootPath)
        {
            var MainList = (from e in context.IbsAppdocuments
                            where e.Documentcategory == DocumentCategoryID && e.Applicationid.Contains(ApplicationID)
                            && e.Isdeleted != Convert.ToByte(true)
                            select new PhotosModel
                            {
                                ID = e.Id,
                                ApplicationID = e.Applicationid,
                                DocumentCategoryID = e.Documentcategory,
                                DocumentID = e.Documentid ?? 0,
                                OtherDocumentName = e.Otherdocumentname,
                                Imageurl = WebRootPath + e.Relativepath + "/" + e.Fileid,
                            }).ToList();
            return MainList;
        }

        public int DeleteSingleRecord(DeleteICPhotoRequestModel model)
        {
            int id = 0;
            IbsAppdocument objGNR_APPDocument = (from c in context.IbsAppdocuments
                                                 where c.Id == model.ID && c.Applicationid == model.ApplicationID
                                                       && c.Documentcategory == model.DocumentCategoryID && c.Documentid == model.DocumentID
                                                 select c).FirstOrDefault();
            if (objGNR_APPDocument != null)
            {
                objGNR_APPDocument.Isdeleted = Convert.ToByte(true);
                context.SaveChanges();
                id = objGNR_APPDocument.Id;
            }
            return id;
        }

        public int IcIntermediateSave(ICPhotoUploadRequestModel model, List<IFormFile> photos, IFormFile ICPhotoDigitalSign, IFormFile UploadTestPlan, IFormFile UploadICAnnexue1, IFormFile UploadICAnnexue2)
        {

            int consignee_cd = 0;
            ImageFiles filesimg = new ImageFiles();
            string formattedCallRecvDt = "";
            int id = 0;
            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    //if (model.CallRecvDt != null && model.CallRecvDt != DateTime.MinValue)
                    //{
                    //    DateTime parsedFromDate = DateTime.ParseExact(model.CallRecvDt.ToString().Replace("-", "/"), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                    //    formattedCallRecvDt = parsedFromDate.ToString("dd/MM/yyyy");
                    //}

                    if (model.DocBkNo != null && model.DocSetNo != null)
                    {
                        var FinalOrStage = context.T17CallRegisters
                                            .Where(cr => cr.CaseNo == model.CaseNo && cr.CallSno == model.CallSno && cr.CallRecvDt == model.CallRecvDt.Date)
                                            .Select(cr => cr.FinalOrStage)
                                            .FirstOrDefault() ?? "F";

                        var docSetNo = Convert.ToInt32(model.DocSetNo);

                        var bsCheckIC = (from a in context.T10IcBooksets
                                         where a.BkNo.Trim() == model.DocBkNo.Trim()
                                         && (docSetNo >= Convert.ToInt32(a.SetNoFr) && docSetNo <= Convert.ToInt32(a.SetNoTo))
                                         && a.IssueToIecd == Convert.ToInt32(model.IeCd)
                                         && (a.Ictype ?? "F") == FinalOrStage
                                         select a.IssueToIecd).FirstOrDefault();
                        if (bsCheckIC != null)
                        {
                            var query = context.IcIntermediates
                                    .Where(ici => ici.CaseNo == model.CaseNo &&
                                                  ici.CallRecvDt == model.CallRecvDt.Date &&
                                                  ici.CallSno == model.CallSno &&
                                                  ici.BkNo == model.DocBkNo &&
                                                  ici.SetNo == model.DocSetNo)
                                    .OrderBy(ici => ici.Datetime)
                                    .Select(ici => ici.ConsigneeCd)
                                    .FirstOrDefault();

                            consignee_cd = query;

                            if (consignee_cd > 0)
                            {
                                if (consignee_cd.ToString() != model.Consignee)
                                {
                                    model.AlertMsg = "Please Enter other book no. or set no. same is used for consignee " + consignee_cd + " !!!";
                                    id = 0;
                                    return id;
                                }
                            }

                            string FileName = model.CaseNo + "-" + model.DocBkNo + "-" + model.DocSetNo + "-";

                            //Add Code for Image Upload Code
                            if (photos.Count > 0)
                            {
                                filesimg.File_1 = photos.Count >= 1 ? FileName + "01.JPG" : "";
                                filesimg.File_2 = photos.Count >= 2 ? FileName + "02.JPG" : "";
                                filesimg.File_3 = photos.Count >= 3 ? FileName + "03.JPG" : "";
                                filesimg.File_4 = photos.Count >= 4 ? FileName + "04.JPG" : "";
                                filesimg.File_5 = photos.Count >= 5 ? FileName + "05.JPG" : "";
                                filesimg.File_6 = photos.Count >= 6 ? FileName + "06.JPG" : "";
                                filesimg.File_7 = photos.Count >= 7 ? FileName + "07.JPG" : "";
                                filesimg.File_8 = photos.Count >= 8 ? FileName + "08.JPG" : "";
                                filesimg.File_9 = photos.Count >= 9 ? FileName + "09.JPG" : "";
                                filesimg.File_10 = photos.Count >= 10 ? FileName + "10.JPG" : "";
                            }

                            var IcDetail = (from item in context.IcIntermediates
                                            where item.CaseNo == model.CaseNo.Trim() &&
                                                  item.CallSno == model.CallSno &&
                                                  item.CallRecvDt == model.CallRecvDt.Date &&//model.CallRecvDt.Date &&
                                                  item.ConsigneeCd == Convert.ToInt32(model.Consignee)
                                            select item).FirstOrDefault();
                            if (IcDetail == null)
                            {
                                var CallDetails = (from c in context.T18CallDetails
                                                   where c.CaseNo == model.CaseNo && c.CallRecvDt == model.CallRecvDt.Date
                                                   && c.CallSno == model.CallSno && c.ConsigneeCd == Convert.ToInt32(model.Consignee)
                                                   select c).ToList();
                                if (CallDetails.Count() > 0)
                                {
                                    foreach (var i in CallDetails)
                                    {
                                        IcIntermediate obj = new IcIntermediate();
                                        obj.CaseNo = model.CaseNo;
                                        obj.CallRecvDt = model.CallRecvDt;
                                        obj.CallSno = Convert.ToInt16(model.CallSno);
                                        obj.BkNo = model.DocBkNo;
                                        obj.SetNo = model.DocSetNo;
                                        obj.PoNo = model.PoNo;
                                        obj.ConsigneeCd = Convert.ToInt32(model.Consignee);
                                        obj.UserId = model.userId;
                                        obj.ItemSrnoPo = i.ItemSrnoPo;
                                        obj.ItemDescPo = i.ItemDescPo;
                                        obj.QtyPassed = i.QtyPassed;
                                        obj.QtyRejected = i.QtyRejected;
                                        obj.QtyDue = i.QtyDue;
                                        obj.IeCd = Convert.ToInt32(model.IeCd);
                                        obj.Datetime = DateTime.Now;
                                        obj.Createddate = DateTime.Now;
                                        obj.Createdby = model.User_Id;
                                        context.IcIntermediates.Add(obj);
                                        context.SaveChanges();
                                        id = 1;
                                    }
                                }
                            }
                            else
                            {
                                using (var command = context.Database.GetDbConnection().CreateCommand())
                                {
                                    bool wasOpen = command.Connection.State == System.Data.ConnectionState.Open;
                                    if (!wasOpen) command.Connection.Open();
                                    try
                                    {
                                        command.CommandText = "UPDATE IC_INTERMEDIATE SET BK_NO = '" + model.DocBkNo + "', SET_NO = '" + model.DocSetNo + "', QTY_PASSED = '" + model.QtyPassed + "', QTY_REJECTED = '" + model.QtyRejected + "',UPDATEDDATE=TO_date('" + DateTime.Now.ToString("dd/MM/yyyy") + "', 'dd/mm/yyyy') WHERE CASE_NO = '" + model.CaseNo + "' AND CALL_SNO = '" + model.CallSno + "' AND CALL_RECV_DT = TO_date('" + model.CallRecvDt.ToString("dd-MM-yy") + "', 'DD-MM-YY') AND CONSIGNEE_CD = " + Convert.ToInt32(model.Consignee);
                                        command.ExecuteNonQuery();
                                        id = 1;
                                    }
                                    finally
                                    {
                                        if (!wasOpen) command.Connection.Close();
                                    }
                                }
                            }

                            var recordExists = context.T49IcPhotoEncloseds.Where(x => x.CaseNo == model.CaseNo && x.BkNo == model.DocBkNo && x.SetNo == model.DocSetNo && x.CallSno == model.CallSno && x.CallRecvDt == model.CallRecvDt.Date).FirstOrDefault();
                            string PDFFineName = model.CaseNo.Trim() + "-" + model.DocBkNo + "-" + model.DocSetNo;
                            if (recordExists == null)
                            {
                                T49IcPhotoEnclosed obj = new T49IcPhotoEnclosed();

                                obj.CaseNo = model.CaseNo;
                                obj.CallRecvDt = model.CallRecvDt;
                                obj.CallSno = (short?)model.CallSno;
                                obj.BkNo = model.DocBkNo;
                                obj.SetNo = model.DocSetNo;
                                obj.ConsigneeCd = Convert.ToInt32(model.Consignee);
                                obj.Datetime = DateTime.Now;
                                obj.UserId = model.userId;
                                obj.File1 = filesimg.File_1;
                                obj.File2 = filesimg.File_2;
                                obj.File3 = filesimg.File_3;
                                obj.File4 = filesimg.File_4;
                                obj.File5 = filesimg.File_5;
                                obj.File6 = filesimg.File_6;
                                obj.File7 = filesimg.File_7;
                                obj.File8 = filesimg.File_8;
                                obj.File9 = filesimg.File_9;
                                obj.File10 = filesimg.File_10;
                                obj.IcPhoto = ICPhotoDigitalSign != null ? PDFFineName + ".pdf" : "";
                                obj.IcPhotoA1 = UploadICAnnexue1 != null ? PDFFineName + "-A1.PDF" : "";
                                obj.IcPhotoA2 = UploadICAnnexue2 != null ? PDFFineName + "-A2.PDF" : "";
                                context.T49IcPhotoEncloseds.Add(obj);
                                context.SaveChanges();
                            }
                            else
                            {
                                recordExists.CaseNo = model.CaseNo;
                                recordExists.CallRecvDt = model.CallRecvDt;
                                recordExists.CallSno = (short?)model.CallSno;
                                recordExists.BkNo = model.DocBkNo;
                                recordExists.SetNo = model.DocSetNo;
                                recordExists.ConsigneeCd = Convert.ToInt32(model.Consignee);
                                recordExists.Datetime = DateTime.Now;
                                recordExists.UserId = model.userId;
                                recordExists.File1 = filesimg.File_1;
                                recordExists.File2 = filesimg.File_2;
                                recordExists.File3 = filesimg.File_3;
                                recordExists.File4 = filesimg.File_4;
                                recordExists.File5 = filesimg.File_5;
                                recordExists.File6 = filesimg.File_6;
                                recordExists.File7 = filesimg.File_7;
                                recordExists.File8 = filesimg.File_8;
                                recordExists.File9 = filesimg.File_9;
                                recordExists.File10 = filesimg.File_10;
                                recordExists.IcPhoto = ICPhotoDigitalSign != null ? PDFFineName + ".pdf" : "";
                                recordExists.IcPhotoA1 = UploadICAnnexue1 != null ? PDFFineName + "-A1.PDF" : "";
                                recordExists.IcPhotoA2 = UploadICAnnexue2 != null ? PDFFineName + "-A2.PDF" : "";
                                context.SaveChanges();
                            }

                            var IcDetail1 = (from ic in context.IcIntermediates
                                             where ic.CaseNo == model.CaseNo && ic.BkNo == model.DocBkNo && ic.SetNo == model.DocSetNo && ic.ConsigneeCd == Convert.ToInt32(model.Consignee)
                                             select ic).FirstOrDefault();

                            if (IcDetail1 != null)
                            {
                                IcDetail1.File1 = filesimg.File_1;
                                IcDetail1.File2 = filesimg.File_2;
                                IcDetail1.File3 = filesimg.File_3;
                                IcDetail1.File4 = filesimg.File_4;
                                IcDetail1.File5 = filesimg.File_5;
                                IcDetail1.File6 = filesimg.File_6;
                                IcDetail1.File7 = filesimg.File_7;
                                IcDetail1.File8 = filesimg.File_8;
                                IcDetail1.File9 = filesimg.File_9;
                                IcDetail1.File10 = filesimg.File_10;
                                IcDetail.Updatedby = model.User_Id;
                                IcDetail.Updateddate = DateTime.Now;
                                context.SaveChanges();
                            }
                            model.AlertMsg = "Success";
                            id = 1;                            
                            trans.Commit();
                        }
                        else
                        {
                            model.AlertMsg = "Book No. and Set No. specified is not issued to You!!!";
                            id = 0;
                            trans.Rollback();
                            return id;
                        }
                    }
                    else
                    {
                        model.AlertMsg = "Please enter valid book no. and set no. !";
                        id = 0;
                        trans.Rollback();
                        return id;
                    }
                }
                catch (Exception ex)
                {
                    Common.AddException(ex.ToString(), ex.Message.ToString(), "Call_API", "ICPhotoUpload", 1, string.Empty);
                    model.AlertMsg = ex.Message.ToString();
                    id = 0;
                    trans.Rollback();
                }
            }
            return id;
        }

        public BookNoSetNoModel GetBkNoAndSetNoByConsignee(string CaseNo, DateTime? DesireDt, int CallSno, int selectedConsigneeCd, int IE_CD)
        {
            BookNoSetNoModel model = new BookNoSetNoModel();

            var ic_book = (from item in context.T10IcBooksets
                           orderby item.IssueDt descending
                           where item.IssueToIecd == IE_CD
                           select item).FirstOrDefault();

            var ICInter = context.IcIntermediates.Where(ic => ic.CaseNo == CaseNo.Trim() && ic.CallRecvDt == Convert.ToDateTime(DesireDt)
                         && ic.CallSno == CallSno).OrderByDescending(ic => ic.Datetime).ToList();

            if (ICInter != null)
            {
                foreach (var item in ICInter)
                {
                    if (selectedConsigneeCd == item.ConsigneeCd)
                    {
                        model.DocBkNo = item.BkNo;
                        model.DocSetNo = item.SetNo;
                    }
                }
            }
            else
            {
                var dlt_IC = (from x in context.IcIntermediates
                              orderby x.SetNo descending
                              where x.BkNo.Trim() == ic_book.BkNo.Trim() && x.IeCd == IE_CD
                              select x).FirstOrDefault();

                if (dlt_IC != null)
                {
                    int setNo = Convert.ToInt32(dlt_IC.SetNo) + 1;

                    string incrementedSetNo = setNo.ToString("D3");
                    var ic_bookset = (from item in context.T10IcBooksets
                                      orderby item.IssueDt descending
                                      where item.BkNo.Trim().ToUpper() == dlt_IC.BkNo &&
                                            Convert.ToInt32(incrementedSetNo) >= Convert.ToInt32(item.SetNoFr) && Convert.ToInt32(incrementedSetNo) <= Convert.ToInt32(item.SetNoTo) &&
                                            item.IssueToIecd == dlt_IC.IeCd
                                      select item).FirstOrDefault();

                    if (ic_bookset != null)
                    {
                        model.DocBkNo = ic_bookset.BkNo;
                        model.DocSetNo = Convert.ToString(incrementedSetNo);
                    }
                    else
                    {
                        model.DocBkNo = "";
                        model.DocSetNo = "";
                    }
                }
                else
                {
                    model.DocBkNo = ic_book.BkNo;
                    model.DocSetNo = Convert.ToString(ic_book.SetNoFr);
                }
            }

            return model;
        }
    }
}
