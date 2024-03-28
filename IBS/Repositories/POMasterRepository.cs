﻿using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace IBS.Repositories
{
    public class POMasterRepository : IPOMasterRepository
    {
        private readonly ModelContext context;

        public POMasterRepository(ModelContext context)
        {
            this.context = context;
        }
        public PO_MasterModel FindByID(string CaseNo)
        {
            PO_MasterModel model = new();
            T80PoMaster POMaster = context.T80PoMasters.Find(CaseNo);

            if (POMaster == null)
                throw new Exception("Po Master Record Not found");
            else
            {
                model.CaseNo = POMaster.CaseNo.Trim();
                model.PurchaserCd = POMaster.PurchaserCd;
                model.StockNonstock = POMaster.StockNonstock;
                model.RlyNonrly = POMaster.RlyNonrly;
                model.PoOrLetter = POMaster.PoOrLetter;
                model.PoNo = POMaster.PoNo;
                model.PoDt = POMaster.PoDt;
                model.RecvDt = POMaster.RecvDt;
                model.RlyCdDesc = POMaster.RlyCdDesc;
                model.VendCd = POMaster.VendCd;
                model.RlyCd = POMaster.RlyCd;
                model.RegionCode = POMaster.RegionCode;
                model.UserId = POMaster.UserId;
                model.Datetime = POMaster.Datetime;
                model.Remarks = POMaster.Remarks;
                model.PoiCd = POMaster.PoiCd;
                model.Ispricevariation = Convert.ToBoolean(POMaster.Ispricevariation);
                model.Isstageinspection = Convert.ToBoolean(POMaster.Isstageinspection);
                model.Contractid = POMaster.Contractid;
                model.RealCaseNo = POMaster.RealCaseNo;
                model.Purchaser = POMaster.Purchaser;
                //model.txtSPur = POMaster.Purchaser;
                model.TempPurchaser = POMaster.Purchaser;
                model.TempPoiCd = POMaster.PoiCd;
                return model;
            }
        }
        public DTResult<PO_MasterModel> GetPOMasterList(DTParameters dtParameters, int VendCd)
        {

            DTResult<PO_MasterModel> dTResult = new() { draw = 0 };
            IQueryable<PO_MasterModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;
            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "RealCaseNo";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                orderCriteria = "RealCaseNo";
                orderAscendingDirection = true;
            }

            string CaseNo = "", PoNo = "", PoDt = "";
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["CaseNo"]))
            {
                CaseNo = Convert.ToString(dtParameters.AdditionalValues["CaseNo"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["PoNo"]))
            {
                PoNo = Convert.ToString(dtParameters.AdditionalValues["PoNo"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["PoDt"]))
            {
                PoDt = Convert.ToString(dtParameters.AdditionalValues["PoDt"]);
            }

            OracleParameter[] par = new OracleParameter[8];
            par[0] = new OracleParameter("p_VendCd", OracleDbType.Int32, VendCd, ParameterDirection.Input);
            par[1] = new OracleParameter("p_CASE_NO", OracleDbType.Varchar2, CaseNo.ToString() == "" ? DBNull.Value : CaseNo.ToString(), ParameterDirection.Input);
            par[2] = new OracleParameter("p_PO_NO", OracleDbType.Varchar2, PoNo.ToString() == "" ? DBNull.Value : PoNo.ToString(), ParameterDirection.Input);
            par[3] = new OracleParameter("p_PO_DT", OracleDbType.Varchar2, PoDt.ToString() == "" ? DBNull.Value : PoDt.ToString(), ParameterDirection.Input);
            par[4] = new OracleParameter("p_page_start", OracleDbType.Int32, dtParameters.Start + 1, ParameterDirection.Input);
            par[5] = new OracleParameter("p_page_end", OracleDbType.Int32, (dtParameters.Start + dtParameters.Length), ParameterDirection.Input);
            par[6] = new OracleParameter("p_result_cursor", OracleDbType.RefCursor, ParameterDirection.Output);
            par[7] = new OracleParameter("p_result_records", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("SP_GET_POMASTERLIST_For_Vendor", par, 2);
            List<PO_MasterModel> list = new();
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                list = JsonConvert.DeserializeObject<List<PO_MasterModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            }
            int recordsTotal = 0;
            if (ds != null && ds.Tables[1].Rows.Count > 0)
            {
                recordsTotal = Convert.ToInt32(ds.Tables[1].Rows[0]["total_records"]);
            }
            query = list.AsQueryable();
            dTResult.recordsTotal = recordsTotal;
            dTResult.recordsFiltered = recordsTotal;
            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Select(p => p).ToList();
            dTResult.draw = dtParameters.Draw;
            return dTResult;
        }

        public DTResult<PO_MasterModel> GetPOMasterListForClient(DTParameters dtParameters, string rly_cd, string RlyNonrly)
        {

            DTResult<PO_MasterModel> dTResult = new() { draw = 0 };
            IQueryable<PO_MasterModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "RealCaseNo";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "RealCaseNo";
                orderAscendingDirection = true;
            }

            string CaseNo = "", PoNo = "", PoDt = "";
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["CaseNo"]))
            {
                CaseNo = Convert.ToString(dtParameters.AdditionalValues["CaseNo"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["PoNo"]))
            {
                PoNo = Convert.ToString(dtParameters.AdditionalValues["PoNo"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["PoDt"]))
            {
                PoDt = Convert.ToString(dtParameters.AdditionalValues["PoDt"]);
            }

            OracleParameter[] par = new OracleParameter[9];
            par[0] = new OracleParameter("p_rly_cd", OracleDbType.Varchar2, rly_cd.ToString() == "" ? DBNull.Value : rly_cd.ToString(), ParameterDirection.Input);
            par[1] = new OracleParameter("p_RlyNonrly", OracleDbType.Varchar2, RlyNonrly.ToString() == "" ? DBNull.Value : RlyNonrly.ToString(), ParameterDirection.Input);
            par[2] = new OracleParameter("p_CASE_NO", OracleDbType.Varchar2, CaseNo.ToString() == "" ? DBNull.Value : CaseNo.ToString(), ParameterDirection.Input);
            par[3] = new OracleParameter("p_PO_NO", OracleDbType.Varchar2, PoNo.ToString() == "" ? DBNull.Value : PoNo.ToString(), ParameterDirection.Input);
            par[4] = new OracleParameter("p_PO_DT", OracleDbType.Varchar2, PoDt.ToString() == "" ? DBNull.Value : PoDt.ToString(), ParameterDirection.Input);
            par[5] = new OracleParameter("p_page_start", OracleDbType.Int32, dtParameters.Start + 1, ParameterDirection.Input);
            par[6] = new OracleParameter("p_page_end", OracleDbType.Int32, (dtParameters.Start + dtParameters.Length), ParameterDirection.Input);
            par[7] = new OracleParameter("p_result_cursor", OracleDbType.RefCursor, ParameterDirection.Output);
            par[8] = new OracleParameter("p_result_records", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("SP_GET_POMASTERLIST_For_Client", par, 2);
            List<PO_MasterModel> list = new();
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                list = JsonConvert.DeserializeObject<List<PO_MasterModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            }
            int recordsTotal = 0;
            if (ds != null && ds.Tables[1].Rows.Count > 0)
            {
                recordsTotal = Convert.ToInt32(ds.Tables[1].Rows[0]["total_records"]);
            }
            query = list.AsQueryable();
            dTResult.recordsTotal = recordsTotal;
            dTResult.recordsFiltered = recordsTotal;
            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Select(p => p).ToList();
            dTResult.draw = dtParameters.Draw;


            //query = from POMaster in context.ViewPomasterlists
            //        where POMaster.Rlycds == rly_cd && POMaster.RlyNonrly == RlyNonrly
            //        && POMaster.Isdeleted != Convert.ToByte(true)
            //        select new PO_MasterModel
            //        {
            //            VendCd = POMaster.VendCd,
            //            CaseNo = POMaster.CaseNo.Trim(),
            //            PoNo = POMaster.PoNo,
            //            PoDtDate = Convert.ToDateTime(POMaster.PoDt).ToString("dd/MM/yyyy"),
            //            RlyCd = POMaster.RlyCd,
            //            VendorName = POMaster.VendName,
            //            ConsigneeSName = POMaster.ConsigneeSName,
            //            RealCaseNo = POMaster.RealCaseNo,
            //            Remarks = POMaster.Remarks,
            //            RlyNonrly = POMaster.RlyNonrly,
            //            MainrlyCd = POMaster.MainrlyCd,
            //        };


            //dTResult.recordsTotal = query.Count();

            //if (!string.IsNullOrEmpty(searchBy))
            //    query = query.Where(w => Convert.ToString(w.RealCaseNo).ToLower().Contains(searchBy.ToLower())
            //    || Convert.ToString(w.VendorName).ToLower().Contains(searchBy.ToLower())
            //    || Convert.ToString(w.ConsigneeSName).ToLower().Contains(searchBy.ToLower())
            //    || Convert.ToString(w.CaseNo).ToLower().Contains(searchBy.ToLower())
            //    );

            //dTResult.recordsFiltered = query.Count();

            //dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            //dTResult.draw = dtParameters.Draw;

            return dTResult;
        }
        public bool Remove(string CaseNo, int UserID)
        {
            var POMasters = context.T80PoMasters.Find(CaseNo);
            if (POMasters == null) { return false; }

            POMasters.Isdeleted = Convert.ToByte(true);
            POMasters.Updatedby = Convert.ToInt32(UserID);
            POMasters.Updateddate = DateTime.Now;
            context.SaveChanges();
            return true;
        }
        public PO_MasterModel alreadyExistT80_PO_MASTER(PO_MasterModel model)
        {
            PO_MasterModel models = new PO_MasterModel();
            T80PoMaster POMaster = (from m in context.T80PoMasters
                                    where m.PoNo == model.PoNo && m.PoDt == model.PoDt && m.RegionCode == model.RegionCode
                                    select m).FirstOrDefault();

            if (POMaster == null)
            {
                models = null;
                return models;
            }
            else
            {
                models.CaseNo = POMaster.CaseNo.Trim();
                models.PoDt = POMaster.PoDt;
                return models;
            }
        }
        public PO_MasterModel alreadyExistT13_PO_MASTER(PO_MasterModel model)
        {
            PO_MasterModel models = new PO_MasterModel();
            T13PoMaster POMaster = (from m in context.T13PoMasters
                                    where m.PoNo == model.PoNo && m.PoDt == model.PoDt && m.RegionCode == model.RegionCode
                                    select m).FirstOrDefault();

            if (POMaster == null)
            {
                models = null;
                return models;
            }
            else
            {
                models.CaseNo = POMaster.CaseNo.Trim();
                models.PoDt = POMaster.PoDt;
                return models;
            }
        }
        public string POMasterDetailsInsertUpdate(PO_MasterModel model)
        {
            string CaseNo = "";
            var POMaster = context.T80PoMasters.Find(model.CaseNo);
            #region POMaster save
            if (POMaster == null)
            {
                string date = model.PoDt.ToString().Substring(0, 10);
                OracleParameter[] par = new OracleParameter[3];
                par[0] = new OracleParameter("IN_REGION_CD", OracleDbType.Char, model.RegionCode, ParameterDirection.Input);
                par[1] = new OracleParameter("IN_PO_DT", OracleDbType.Varchar2, date, ParameterDirection.Input);
                par[2] = new OracleParameter("p_result_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

                var ds = DataAccessDB.GetDataSet("GENERATE_VEND_CASE_NO", par, 1);
                PO_MasterModel model1 = new();
                if (ds != null && ds.Tables.Count > 0)
                {
                    string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                    model1 = JsonConvert.DeserializeObject<List<PO_MasterModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }).FirstOrDefault();
                }

                T80PoMaster obj = new T80PoMaster();
                obj.CaseNo = model1.CaseNo.Trim();
                obj.PurchaserCd = model.PurchaserCd;
                obj.StockNonstock = model.StockNonstock;
                obj.PoOrLetter = model.PoOrLetter;
                obj.RlyNonrly = model.RlyNonrly;
                obj.PoNo = model.PoNo;
                obj.PoDt = model.PoDt;
                if (model.RecvDt != null)
                {
                    obj.RecvDt = model.RecvDt;
                }
                else
                {
                    obj.RecvDt = DateTime.Now;
                }
                obj.VendCd = model.VendCd;
                obj.RlyCd = model.RlyCd;
                obj.RlyCdDesc = model.RlyCdDesc;
                obj.RegionCode = model.RegionCode;
                obj.Remarks = model.Remarks;
                obj.Datetime = DateTime.Now;
                obj.PoiCd = model.PoiCd;
                if (model.PurchaserCd == 0)
                {
                    obj.Purchaser = model.Purchaser;
                }
                if (model.ClientUserID != null && model.ClientUserID != "")
                {
                    obj.Clientuserid = model.ClientUserID;
                }
                obj.Ispricevariation = Convert.ToByte(model.Ispricevariation);
                obj.Isstageinspection = Convert.ToByte(model.Isstageinspection);
                obj.Contractid = model.Contractid;
                obj.Createdby = model.Createdby;
                obj.Createddate = DateTime.Now;
                context.T80PoMasters.Add(obj);
                context.SaveChanges();
                CaseNo = obj.CaseNo;
            }
            else
            {
                POMaster.PurchaserCd = model.PurchaserCd;
                POMaster.StockNonstock = model.StockNonstock;
                POMaster.PoOrLetter = model.PoOrLetter;
                POMaster.RlyNonrly = model.RlyNonrly;
                POMaster.PoNo = model.PoNo;
                POMaster.PoDt = model.PoDt;
                if (model.RecvDt != null)
                {
                    POMaster.RecvDt = model.RecvDt;
                }
                else
                {
                    POMaster.RecvDt = DateTime.Now;
                }
                POMaster.VendCd = model.VendCd;
                POMaster.RlyCd = model.RlyCd;
                POMaster.RlyCdDesc = model.RlyCdDesc;
                //POMaster.RegionCode = model.RegionCode;
                POMaster.Remarks = model.Remarks;
                POMaster.Datetime = DateTime.Now;
                POMaster.PoiCd = model.PoiCd;
                POMaster.Ispricevariation = Convert.ToByte(model.Ispricevariation);
                POMaster.Isstageinspection = Convert.ToByte(model.Isstageinspection);
                POMaster.Contractid = model.Contractid;
                if (model.PurchaserCd == 0)
                {
                    POMaster.Purchaser = model.Purchaser;
                }
                POMaster.Updatedby = model.Updatedby;
                POMaster.Updateddate = DateTime.Now;
                context.SaveChanges();
                CaseNo = POMaster.CaseNo;
            }
            #endregion
            return CaseNo;
        }
        public PO_MasterModel FindCaseNo(string CaseNo, int VendCd)
        {
            PO_MasterModel model = new PO_MasterModel();
            T13PoMaster POMaster = (from l in context.T13PoMasters
                                    where l.CaseNo == CaseNo && l.VendCd == VendCd
                                    select l).FirstOrDefault();

            if (POMaster == null)
            {
                model = null;
                return model;
            }
            else
            {
                model.CaseNo = POMaster.CaseNo;
                model.PurchaserCd = POMaster.PurchaserCd;
                model.StockNonstock = POMaster.StockNonstock;
                model.RlyNonrly = POMaster.RlyNonrly;
                model.PoOrLetter = POMaster.PoOrLetter;
                model.PoNo = POMaster.PoNo;
                model.PoDt = POMaster.PoDt;
                model.RecvDt = POMaster.RecvDt;
                model.VendCd = POMaster.VendCd;
                model.RlyCd = POMaster.RlyCd;
                model.RegionCode = POMaster.RegionCode;
                model.UserId = POMaster.UserId;
                model.Datetime = POMaster.Datetime;
                model.Remarks = POMaster.Remarks;
                model.PoiCd = POMaster.PoiCd;
                model.Contractid = POMaster.Contractid;
                model.Isstageinspection = Convert.ToBoolean(POMaster.Isstageinspection);
                model.Ispricevariation = Convert.ToBoolean(POMaster.Ispricevariation);
                return model;
            }
        }

        public PO_MasterModel FindCaseNoForClient(string CaseNo)
        {
            PO_MasterModel model = new PO_MasterModel();
            T13PoMaster POMaster = (from l in context.T13PoMasters
                                    where l.CaseNo == CaseNo
                                    select l).FirstOrDefault();

            if (POMaster == null)
            {
                model = null;
                return model;
            }
            else
            {
                model.CaseNo = POMaster.CaseNo;
                model.PurchaserCd = POMaster.PurchaserCd;
                model.StockNonstock = POMaster.StockNonstock;
                model.RlyNonrly = POMaster.RlyNonrly;
                model.PoOrLetter = POMaster.PoOrLetter;
                model.PoNo = POMaster.PoNo;
                model.PoDt = POMaster.PoDt;
                model.RecvDt = POMaster.RecvDt;
                model.VendCd = POMaster.VendCd;
                model.RlyCd = POMaster.RlyCd;
                model.RegionCode = POMaster.RegionCode;
                model.UserId = POMaster.UserId;
                model.Datetime = POMaster.Datetime;
                model.Remarks = POMaster.Remarks;
                model.PoiCd = POMaster.PoiCd;
                model.Contractid = POMaster.Contractid;
                model.Isstageinspection = Convert.ToBoolean(POMaster.Isstageinspection);
                model.Ispricevariation = Convert.ToBoolean(POMaster.Ispricevariation);
                return model;
            }
        }
        public DTResult<PO_MasterDetailListModel> GetPOMasterDetailsList(DTParameters dtParameters)
        {

            DTResult<PO_MasterDetailListModel> dTResult = new() { draw = 0 };
            IQueryable<PO_MasterDetailListModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "CASE_NO";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "CASE_NO";
                orderAscendingDirection = true;
            }
            string CaseNo = dtParameters.AdditionalValues.ToArray().Where(x => x.Key == "CaseNo").FirstOrDefault().Value;

            OracleParameter[] par = new OracleParameter[2];
            par[0] = new OracleParameter("p_case_no", OracleDbType.Char, CaseNo, ParameterDirection.Input);
            par[1] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GET_PO_DETAILS", par, 1);
            List<PO_MasterDetailListModel> model = new List<PO_MasterDetailListModel>();
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                model = JsonConvert.DeserializeObject<List<PO_MasterDetailListModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }).ToList();
            }
            query = model.AsQueryable();

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.CASE_NO).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.ITEM_SRNO).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.ITEM_DESC).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }
        public bool RemovePODetails(string CaseNo, string ITEM_SRNO, int UserID)
        {
            T82PoDetail POMastersDetails = (from pm in context.T82PoDetails
                                            where pm.CaseNo == CaseNo && pm.ItemSrno == Convert.ToByte(ITEM_SRNO)
                                            select pm).FirstOrDefault();
            if (POMastersDetails == null) { return false; }

            POMastersDetails.Isdeleted = Convert.ToByte(true);
            POMastersDetails.Updatedby = Convert.ToInt32(UserID);
            POMastersDetails.Updateddate = DateTime.Now;
            context.SaveChanges();
            return true;
        }
        public int GenerateITEM_SRNO(string CaseNo)
        {
            int maxSrNo = 1;
            int count = context.T82PoDetails.Where(x => x.CaseNo == CaseNo).Count();
            if (count > 0)
            {
                maxSrNo = (from pm in context.T82PoDetails
                           where pm.CaseNo == CaseNo
                           select pm.ItemSrno).Max() + 1;
            }
            return maxSrNo;
        }
        public PO_MasterDetailsModel FindPODetailsByID(string CASE_NO, string ITEM_SRNO)
        {
            PO_MasterDetailsModel model = new();
            T82PoDetail POMastersDetails = context.T82PoDetails.Where(x => x.CaseNo == CASE_NO && x.ItemSrno == Convert.ToByte(ITEM_SRNO)).FirstOrDefault();

            if (POMastersDetails == null)
                throw new Exception("Po Master Details Record Not found");
            else
            {
                model.CaseNo = POMastersDetails.CaseNo;
                model.DrawingNo = POMastersDetails.DrawingNo;
                model.SpecificationNo = POMastersDetails.SpecificationNo;
                model.ItemSrno = POMastersDetails.ItemSrno;
                model.PlNo = POMastersDetails.PlNo;
                model.BpoCd = POMastersDetails.BpoCd;
                model.Bpo = POMastersDetails.Bpo;
                model.ItemDesc = POMastersDetails.ItemDesc;
                model.ConsigneeCd = POMastersDetails.ConsigneeCd;
                model.Consignee = POMastersDetails.Consignee;
                model.Qty = POMastersDetails.Qty;
                model.BasicValue = POMastersDetails.BasicValue;
                model.Rate = POMastersDetails.Rate;
                model.UomCd = POMastersDetails.UomCd;
                model.SalesTaxPer = POMastersDetails.SalesTaxPer;
                model.SalesTax = POMastersDetails.SalesTax;
                model.Excise = POMastersDetails.Excise;
                model.ExcisePer = POMastersDetails.ExcisePer;
                model.ExciseType = POMastersDetails.ExciseType;
                model.Discount = POMastersDetails.Discount;
                model.DiscountPer = POMastersDetails.DiscountPer;
                model.DiscountType = POMastersDetails.DiscountType;
                model.OtherCharges = POMastersDetails.OtherCharges;
                model.OtChargePer = POMastersDetails.OtChargePer;
                model.OtChargeType = POMastersDetails.OtChargeType;
                model.Value = POMastersDetails.Value;
                model.DelvDt = POMastersDetails.DelvDt;
                model.ExtDelvDt = POMastersDetails.ExtDelvDt;
                return model;
            }
        }
        public DTResult<PO_MasterDetailsModel> FindByUOMDetail(decimal id)
        {
            DTResult<PO_MasterDetailsModel> dTResult = new() { draw = 0 };
            IQueryable<PO_MasterDetailsModel>? query = null;
            query = from l in context.T04Uoms
                    where l.UomCd == id

                    select new PO_MasterDetailsModel
                    {
                        UOMFactor = l.UomFactor
                    };

            dTResult.data = query;
            return dTResult;
        }
        public int POMasterSubDetailsInsertUpdate(PO_MasterDetailsModel model)
        {
            int ItemSrno = 0;
            var t82PoDetail = context.T82PoDetails.Where(x => x.CaseNo == model.CaseNo && x.ItemSrno == model.ItemSrno).FirstOrDefault();
            if (t82PoDetail == null)
            {
                T82PoDetail obj = new T82PoDetail();
                obj.CaseNo = model.CaseNo;
                obj.DrawingNo = model.DrawingNo;
                obj.SpecificationNo = model.SpecificationNo;
                obj.ItemSrno = (byte)model.ItemSrno;
                obj.PlNo = model.PlNo;
                obj.BpoCd = model.BpoCd;
                obj.ItemDesc = model.ItemDesc;
                obj.ConsigneeCd = model.ConsigneeCd;
                obj.Bpo = model.Bpo;
                obj.Consignee = model.Consignee;
                obj.Qty = model.Qty;
                obj.Rate = model.Rate;
                obj.UomCd = model.UomCd;
                obj.BasicValue = model.BasicValue;
                obj.SalesTaxPer = model.SalesTaxPer;
                obj.SalesTax = model.SalesTax;
                obj.ExciseType = model.ExciseType;
                obj.ExcisePer = model.ExcisePer;
                obj.Excise = model.Excise;
                obj.DiscountType = model.DiscountType;
                obj.DiscountPer = model.DiscountPer;
                obj.Discount = model.Discount;
                obj.OtChargeType = model.OtChargeType;
                obj.OtChargePer = model.OtChargePer;
                obj.OtherCharges = model.OtherCharges;
                obj.Value = model.Value;
                obj.DelvDt = model.DelvDt;
                obj.ExtDelvDt = model.ExtDelvDt;
                obj.UserId = model.UserId;
                obj.Datetime = DateTime.Now;
                obj.Createdby = model.Createdby;
                obj.Createddate = DateTime.Now;
                context.T82PoDetails.Add(obj);
                context.SaveChanges();
                ItemSrno = Convert.ToInt32(obj.ItemSrno);
            }
            else
            {
                t82PoDetail.ItemDesc = model.ItemDesc;
                t82PoDetail.PlNo = model.PlNo;
                t82PoDetail.DrawingNo = model.DrawingNo;
                t82PoDetail.SpecificationNo = model.SpecificationNo;
                t82PoDetail.BpoCd = model.BpoCd;
                t82PoDetail.Qty = model.Qty;
                t82PoDetail.ConsigneeCd = model.ConsigneeCd;
                t82PoDetail.Bpo = model.Bpo;
                t82PoDetail.Consignee = model.Consignee;
                t82PoDetail.UomCd = model.UomCd;
                t82PoDetail.BasicValue = model.BasicValue;
                t82PoDetail.Rate = model.Rate;
                t82PoDetail.ExciseType = model.ExciseType;
                t82PoDetail.ExcisePer = model.ExcisePer;
                t82PoDetail.Excise = model.Excise;
                t82PoDetail.SalesTaxPer = model.SalesTaxPer;
                t82PoDetail.SalesTax = model.SalesTax;
                t82PoDetail.DiscountType = model.DiscountType;
                t82PoDetail.DiscountPer = model.DiscountPer;
                t82PoDetail.Discount = model.Discount;
                t82PoDetail.OtChargePer = model.OtChargePer;
                t82PoDetail.OtChargeType = model.OtChargeType;
                t82PoDetail.OtherCharges = model.OtherCharges;
                t82PoDetail.Value = model.Value;
                t82PoDetail.DelvDt = model.DelvDt;
                t82PoDetail.ExtDelvDt = model.ExtDelvDt;
                t82PoDetail.UserId = model.UserId;
                t82PoDetail.Updatedby = model.Updatedby;
                t82PoDetail.Updateddate = DateTime.Now;
                context.SaveChanges();
                ItemSrno = Convert.ToInt32(t82PoDetail.ItemSrno);
            }
            return ItemSrno;
        }
        public string UpdateRealCaseNo(DEOVendorPurchesOrderModel model)
        {
            string returnVal = "";
            var POMaster = context.T13PoMasters.Find(model.CaseNo);
            if (POMaster != null)
            {
                if (POMaster.PoNo == model.PoNo && POMaster.PoDt == model.PoDt && POMaster.RlyCd == model.RlyCd)
                {
                    var t80PoMasters = context.T80PoMasters.Find(model.CaseNo);
                    t80PoMasters.RealCaseNo = model.RealCaseNo;
                    t80PoMasters.Datetime = DateTime.Now;
                    context.SaveChanges();
                    returnVal = t80PoMasters.CaseNo;
                }
                else
                {
                    returnVal = "Not Match";
                }
            }
            else
            {
                returnVal = "Not Match";
            }
            return returnVal;
        }

        public string getVendorEmail(string CASE_NO)
        {
            string vendorEmail = "";
            OracleParameter[] par = new OracleParameter[2];
            par[0] = new OracleParameter("IN_CASE_NO", OracleDbType.Varchar2, CASE_NO, ParameterDirection.Input);
            par[1] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);
            var ds = DataAccessDB.GetDataSet("GET_VENDOR_INFO", par, 1);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                vendorEmail = ds.Tables[0].Rows[0]["VEND_EMAIL"].ToString();
            }
            return vendorEmail;
        }

        public string[] GenerateRealCaseNo(string REGION_CD, string CASE_NO, string USER_ID)
        {
            string[] result = new string[2];
            OracleParameter[] par = new OracleParameter[4];
            par[0] = new OracleParameter("IN_REGION_CD", OracleDbType.Char, REGION_CD, ParameterDirection.Input);
            par[1] = new OracleParameter("IN_TEMP_CASE_NO", OracleDbType.Char, CASE_NO, ParameterDirection.Input);
            par[2] = new OracleParameter("IN_TEMP_USER_ID", OracleDbType.Char, USER_ID, ParameterDirection.Input);
            par[3] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);
            var ds = DataAccessDB.GetDataSet("GENERATE_REAL_CASE_NO", par, 1);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                result[0] = ds.Tables[0].Rows[0]["ERR_CD"].ToString();
                result[1] = ds.Tables[0].Rows[0]["CASE_NO"].ToString();
            }
            return result;
        }

        public IBS_DocumentDTO FindAPPDocumentByID(string Applicationid, int DocumentID)
        {
            IBS_DocumentDTO aPPDocument = (from x in context.IbsAppdocuments
                                           where x.Applicationid == Convert.ToString(Applicationid)
                                           && x.Documentid == DocumentID
                                           select new IBS_DocumentDTO
                                           {
                                               ID = x.Id,
                                               DocumentCategory = x.Documentcategory,
                                               APPDocumentID = x.Id,
                                               ApplicationID = x.Applicationid,
                                               DocumentID = x.Documentid ?? 0,
                                               RelativePath = x.Relativepath,
                                               FileID = x.Fileid,
                                               Extension = x.Extension,
                                               FileDisplayName = x.Filedisplayname,
                                               IsOtherDoc = x.Isotherdoc,
                                               OtherDocumentName = x.Otherdocumentname,
                                           }).FirstOrDefault();
            return aPPDocument;
        }

        public int SaveAPPDocumentByID(IBS_DocumentDTO model)
        {
            if (model != null)
            {
                IbsAppdocument ibsAppdocuments = new IbsAppdocument();
                ibsAppdocuments.Applicationid = Convert.ToString(model.ApplicationID);
                ibsAppdocuments.Documentid = model.DocumentID;
                ibsAppdocuments.Relativepath = model.RelativePath;
                ibsAppdocuments.Fileid = model.FileID;
                ibsAppdocuments.Extension = model.Extension;
                ibsAppdocuments.Filedisplayname = model.FileDisplayName;
                ibsAppdocuments.Isotherdoc = model.IsOtherDoc;
                ibsAppdocuments.Otherdocumentname = model.DocumentName;
                ibsAppdocuments.Documentcategory = model.DocumentCategory;
                context.IbsAppdocuments.Add(ibsAppdocuments);
                context.SaveChanges();
                return ibsAppdocuments.Id;
            }
            else
            {
                return 0;
            }
        }
    }
}
