using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces.Inspection_Billing;
using IBS.Models;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace IBS.Repositories.Inspection_Billing
{
    public class AdministratorPurchaseOrderRepository : IAdministratorPurchaseOrderRepository
    {
        private readonly ModelContext context;
        private readonly IWebHostEnvironment env;
        private readonly IConfiguration _configuration;

        public AdministratorPurchaseOrderRepository(ModelContext context, IWebHostEnvironment _environment, IConfiguration configuration)
        {
            this.context = context;
            env = _environment;
            _configuration = configuration;
        }
        public AdministratorPurchaseOrderModel FindByID(string CaseNo)
        {
            AdministratorPurchaseOrderModel model = new();
            T13PoMaster POMaster = context.T13PoMasters.Where(x => x.CaseNo == CaseNo).FirstOrDefault();

            if (POMaster == null)
                throw new Exception("PO Record Not found");
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
                model.VendCd = POMaster.VendCd;
                model.RlyCd = POMaster.RlyCd;
                model.RegionCode = POMaster.RegionCode;
                model.UserId = POMaster.UserId;
                model.Datetime = POMaster.Datetime;
                model.Remarks = POMaster.Remarks;
                model.PoiCd = POMaster.PoiCd;
                model.L5noPo = POMaster.L5noPo;
                model.InspectingAgency = POMaster.InspectingAgency;
                model.Ispricevariation = Convert.ToBoolean(POMaster.Ispricevariation);
                model.Isstageinspection = Convert.ToBoolean(POMaster.Isstageinspection);
                model.Contractid = POMaster.Contractid;
                var T14 = context.T14aPoNonrlies.Where(x => x.CaseNo == POMaster.CaseNo).FirstOrDefault();
                if (T14 != null)
                {
                    model.ContractNo = T14.ContractNo;
                    model.ContractDt = T14.ContractDt;
                    model.ProjectRef = T14.ProjectRef;
                    model.MinFee = T14.MinFee;
                    model.MaxFee = T14.MaxFee;
                    model.WithServTax = T14.WithServTax;
                }
                return model;
            }
        }
        public DTResult<AdministratorPurchaseOrderListModel> GetPOMasterList(DTParameters dtParameters, string region_code, string RootHostName)
        {

            DTResult<AdministratorPurchaseOrderListModel> dTResult = new() { draw = 0 };
            IQueryable<AdministratorPurchaseOrderListModel>? query = null;

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
            string CaseNo = "", PoNo = "", PoDt = "", vend_name = "";
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
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["vend_name"]))
            {
                vend_name = Convert.ToString(dtParameters.AdditionalValues["vend_name"]);
            }
            OracleParameter[] par = new OracleParameter[9];
            par[0] = new OracleParameter("p_cs_no", OracleDbType.Varchar2, CaseNo.ToString() == "" ? DBNull.Value : CaseNo.ToString(), ParameterDirection.Input);
            par[1] = new OracleParameter("p_po_no", OracleDbType.Varchar2, PoNo.ToString() == "" ? DBNull.Value : PoNo.ToString(), ParameterDirection.Input);
            par[2] = new OracleParameter("p_po_date", OracleDbType.Varchar2, PoDt.ToString() == "" ? DBNull.Value : PoDt.ToString(), ParameterDirection.Input);
            par[3] = new OracleParameter("p_vend_name", OracleDbType.Varchar2, vend_name.ToString() == "" ? DBNull.Value : vend_name.ToString(), ParameterDirection.Input);
            par[4] = new OracleParameter("p_region_code", OracleDbType.Varchar2, region_code.ToString() == "" ? DBNull.Value : region_code.ToString(), ParameterDirection.Input);
            par[5] = new OracleParameter("p_page_start", OracleDbType.Int32, dtParameters.Start + 1, ParameterDirection.Input);
            par[6] = new OracleParameter("p_page_end", OracleDbType.Int32, (dtParameters.Start + dtParameters.Length), ParameterDirection.Input);
            par[7] = new OracleParameter("p_result_cursor", OracleDbType.RefCursor, ParameterDirection.Output);
            par[8] = new OracleParameter("p_result_records", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("SP_GET_Administrator_PO_DETAILS", par, 2);
            List<AdministratorPurchaseOrderListModel> list = new();
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                list = JsonConvert.DeserializeObject<List<AdministratorPurchaseOrderListModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                if (list.Count > 0)
                {
                    string HostUrl = _configuration.GetSection("AppSettings")["SiteUrl"];
                    if (RootHostName.Contains("14.143.90.241"))
                    {
                        HostUrl = HostUrl.Replace("192.168.0.101", "14.143.90.241");
                    }
                    foreach (var item in list)
                    {
                        string fpath = env.WebRootPath + Enums.GetEnumDescription(Enums.FolderPath.AdministratorPurchaseOrderCASE_NO) + "/" + item.PO_DOC.ToString();
                        string fpath1 = env.WebRootPath + Enums.GetEnumDescription(Enums.FolderPath.AdministratorPurchaseOrderCASE_NO) + "/" + item.PO_DOC1.ToString();
                        if (!File.Exists(fpath) && !File.Exists(fpath1))
                        {
                            item.IsFileExist = false;
                        }
                        else if (File.Exists(fpath))
                        {
                            item.IsFileExist = true;
                            item.IsPO_DOC = true;
                            item.IsPO_DOC1 = false;
                            item.PO_DOC = HostUrl + Enums.GetEnumDescription(Enums.FolderPath.AdministratorPurchaseOrderCASE_NO) + "/" + item.PO_DOC.ToString();
                        }
                        else if (File.Exists(fpath1))
                        {
                            item.IsFileExist = true;
                            item.IsPO_DOC = false;
                            item.IsPO_DOC1 = true;
                            item.PO_DOC1 = HostUrl + Enums.GetEnumDescription(Enums.FolderPath.AdministratorPurchaseOrderCASE_NO) + "/" + item.PO_DOC1.ToString();
                        }
                    }
                }
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
        public string POMasterDetailsInsertUpdate(AdministratorPurchaseOrderModel model)
        {
            string CaseNo = "";
            var POMaster = context.T13PoMasters.Find(model.CaseNo);
            #region POMaster save
            if (POMaster == null)
            {
                string date = model.PoDt.ToString().Substring(0, 10);
                OracleParameter[] par = new OracleParameter[3];
                par[0] = new OracleParameter("IN_REGION_CD", OracleDbType.Char, model.RegionCode, ParameterDirection.Input);
                par[1] = new OracleParameter("IN_PO_DT", OracleDbType.Varchar2, date, ParameterDirection.Input);
                par[2] = new OracleParameter("p_result_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

                var ds = DataAccessDB.GetDataSet("GENERATE_CASE_NO", par, 1);
                AdministratorPurchaseOrderModel model1 = new();
                if (ds != null && ds.Tables.Count > 0)
                {
                    string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                    model1 = JsonConvert.DeserializeObject<List<AdministratorPurchaseOrderModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }).FirstOrDefault();
                }

                T13PoMaster obj = new T13PoMaster();
                obj.CaseNo = model1.CaseNo.Trim();
                obj.PurchaserCd = model.PurchaserCd;
                obj.StockNonstock = model.StockNonstock;
                obj.PoOrLetter = model.PoOrLetter;
                obj.RlyNonrly = model.RlyNonrly;
                obj.PoNo = model.PoNo;
                obj.PoDt = model.PoDt;
                obj.RecvDt = model.RecvDt;
                obj.VendCd = model.VendCd;
                obj.RlyCd = model.RlyCd;
                obj.RegionCode = model.RegionCode;
                obj.Remarks = model.Remarks;
                obj.Datetime = DateTime.Now;
                obj.PoiCd = model.PoiCd;
                obj.UserId = model.UserId;
                obj.InspectingAgency = model.InspectingAgency;
                obj.L5noPo = model.L5noPo;
                obj.Createdby = model.Createdby;
                obj.Createddate = DateTime.Now;
                obj.Ispricevariation = Convert.ToByte(model.Ispricevariation);
                obj.Isstageinspection = Convert.ToByte(model.Isstageinspection);
                obj.Contractid = model.Contractid;
                context.T13PoMasters.Add(obj);
                context.SaveChanges();
                CaseNo = obj.CaseNo;
                if (model.RlyNonrly != "R")
                {
                    var T14 = context.T14aPoNonrlies.Where(x => x.CaseNo == obj.CaseNo).FirstOrDefault();
                    if (T14 == null)
                    {
                        T14aPoNonrly objT14 = new T14aPoNonrly();
                        objT14.CaseNo = obj.CaseNo;
                        objT14.ContractNo = model.ContractNo;
                        objT14.ContractDt = model.ContractDt;
                        objT14.ProjectRef = model.ProjectRef;
                        objT14.MinFee = model.MinFee;
                        objT14.MaxFee = model.MaxFee;
                        objT14.WithServTax = model.WithServTax;
                        context.T14aPoNonrlies.Add(objT14);
                        context.SaveChanges();
                    }
                }
            }
            else
            {
                POMaster.PurchaserCd = model.PurchaserCd;
                POMaster.StockNonstock = model.StockNonstock;
                POMaster.PoOrLetter = model.PoOrLetter;
                POMaster.RlyNonrly = model.RlyNonrly;
                POMaster.PoNo = model.PoNo;
                POMaster.PoDt = model.PoDt;
                POMaster.RecvDt = model.RecvDt;
                POMaster.VendCd = model.VendCd;
                POMaster.RlyCd = model.RlyCd;
                POMaster.RegionCode = model.RegionCode;
                POMaster.Remarks = model.Remarks;
                POMaster.Datetime = DateTime.Now;
                POMaster.PoiCd = model.PoiCd;
                POMaster.UserId = model.UserId;
                POMaster.InspectingAgency = model.InspectingAgency;
                POMaster.L5noPo = model.L5noPo;
                POMaster.Updatedby = model.Updatedby;
                POMaster.Updateddate = DateTime.Now;
                POMaster.Ispricevariation = Convert.ToByte(model.Ispricevariation);
                POMaster.Isstageinspection = Convert.ToByte(model.Isstageinspection);
                POMaster.Contractid = model.Contractid;
                context.SaveChanges();
                CaseNo = POMaster.CaseNo;

                if (model.RlyNonrly != "R")
                {
                    var T14 = context.T14aPoNonrlies.Where(x => x.CaseNo == POMaster.CaseNo).FirstOrDefault();
                    if (T14 == null)
                    {
                        T14aPoNonrly objT14 = new T14aPoNonrly();
                        objT14.CaseNo = POMaster.CaseNo;
                        objT14.ContractNo = model.ContractNo;
                        objT14.ContractDt = model.ContractDt;
                        objT14.ProjectRef = model.ProjectRef;
                        objT14.MinFee = model.MinFee;
                        objT14.MaxFee = model.MaxFee;
                        objT14.WithServTax = model.WithServTax;
                        context.T14aPoNonrlies.Add(objT14);
                        context.SaveChanges();
                    }
                    else
                    {
                        T14.ContractNo = model.ContractNo;
                        T14.ContractDt = model.ContractDt;
                        T14.ProjectRef = model.ProjectRef;
                        T14.MinFee = model.MinFee;
                        T14.MaxFee = model.MaxFee;
                        T14.WithServTax = model.WithServTax;
                        context.SaveChanges();
                    }
                }
            }
            #endregion
            return CaseNo;
        }
        public PO_MasterModel FindCaseNo(string CaseNo, int VendCd)
        {
            PO_MasterModel model = new();
            //T13PoMaster POMaster = context.T13PoMasters.Find(CaseNo);
            T80PoMaster POMaster = (from l in context.T80PoMasters
                                    where l.CaseNo == CaseNo && l.VendCd == VendCd
                                    select l).FirstOrDefault();

            if (POMaster == null)
                throw new Exception("Po Master Record Not found");
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

            var ds = DataAccessDB.GetDataSet("SP_GET_PO_DETAILSForAdministrator", par, 1);
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
            T15PoDetail POMastersDetails = (from pm in context.T15PoDetails
                                            where pm.CaseNo == CaseNo && pm.ItemSrno == Convert.ToByte(ITEM_SRNO)
                                            select pm).FirstOrDefault();
            if (POMastersDetails == null) { return false; }

            POMastersDetails.Isdeleted = Convert.ToByte(true);
            POMastersDetails.Updatedby = UserID.ToString();
            POMastersDetails.Updateddate = DateTime.Now;
            context.SaveChanges();
            return true;
        }
        public int GenerateITEM_SRNO(string CaseNo)
        {
            int maxSrNo = 1;
            int count = context.T15PoDetails.Where(x => x.CaseNo == CaseNo).Count();
            if (count > 0)
            {
                maxSrNo = (from pm in context.T15PoDetails
                           where pm.CaseNo == CaseNo
                           select pm.ItemSrno).Max() + 1;
            }
            return maxSrNo;
        }
        public PO_MasterDetailsModel FindPODetailsByID(string CASE_NO, string ITEM_SRNO)
        {
            PO_MasterDetailsModel model = new();
            T15PoDetail POMastersDetails = context.T15PoDetails.Where(x => x.CaseNo == CASE_NO && x.ItemSrno == Convert.ToByte(ITEM_SRNO)).FirstOrDefault();

            if (POMastersDetails == null)
                throw new Exception("Po Master Details Record Not found");
            else
            {
                model.CaseNo = POMastersDetails.CaseNo;
                model.ItemSrno = POMastersDetails.ItemSrno;
                model.PlNo = POMastersDetails.PlNo;
                //model.BpoCd = POMastersDetails.BpoCd;
                //model.Bpo = POMastersDetails.Bpo;
                model.ItemDesc = POMastersDetails.ItemDesc;
                model.ConsigneeCd = POMastersDetails.ConsigneeCd;
                //model.Consignee = POMastersDetails.Consignee;
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
                model.DrawingNo = POMastersDetails.DrawingNo;
                model.SpecificationNo = POMastersDetails.SpecificationNo;
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
        public PO_MasterModel alreadyExistT80_PO_MASTER(AdministratorPurchaseOrderModel model)
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
        public PO_MasterModel alreadyExistT13_PO_MASTER(AdministratorPurchaseOrderModel model)
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
        public int POMasterSubDetailsInsertUpdate(PO_MasterDetailsModel model)
        {
            int ItemSrno = 0;
            var t15PoDetail = context.T15PoDetails.Where(x => x.CaseNo == model.CaseNo && x.ItemSrno == model.ItemSrno).FirstOrDefault();
            if (t15PoDetail == null)
            {
                T15PoDetail obj = new T15PoDetail();
                obj.CaseNo = model.CaseNo;
                obj.DrawingNo = model.DrawingNo;
                obj.SpecificationNo = model.SpecificationNo;
                obj.ItemSrno = model.ItemSrno;
                obj.PlNo = model.PlNo;
                //obj.BpoCd = model.BpoCd;
                obj.ItemDesc = model.ItemDesc;
                obj.ConsigneeCd = model.ConsigneeCd;
                //obj.Bpo = model.Bpo;
                //obj.Consignee = model.Consignee;
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
                obj.Createdby = model.Createdby.ToString();
                obj.Createddate = DateTime.Now;
                context.T15PoDetails.Add(obj);
                context.SaveChanges();
                ItemSrno = Convert.ToInt32(obj.ItemSrno);
            }
            else
            {
                t15PoDetail.ItemDesc = model.ItemDesc;
                t15PoDetail.PlNo = model.PlNo;
                t15PoDetail.DrawingNo = model.DrawingNo;
                t15PoDetail.SpecificationNo = model.SpecificationNo;
                //t15PoDetail.BpoCd = model.BpoCd;
                t15PoDetail.Qty = model.Qty;
                t15PoDetail.ConsigneeCd = model.ConsigneeCd;
                //t15PoDetail.Bpo = model.Bpo;
                //t15PoDetail.Consignee = model.Consignee;
                t15PoDetail.UomCd = model.UomCd;
                t15PoDetail.BasicValue = model.BasicValue;
                t15PoDetail.Rate = model.Rate;
                t15PoDetail.ExciseType = model.ExciseType;
                t15PoDetail.ExcisePer = model.ExcisePer;
                t15PoDetail.Excise = model.Excise;
                t15PoDetail.SalesTaxPer = model.SalesTaxPer;
                t15PoDetail.SalesTax = model.SalesTax;
                t15PoDetail.DiscountType = model.DiscountType;
                t15PoDetail.DiscountPer = model.DiscountPer;
                t15PoDetail.Discount = model.Discount;
                t15PoDetail.OtChargePer = model.OtChargePer;
                t15PoDetail.OtChargeType = model.OtChargeType;
                t15PoDetail.OtherCharges = model.OtherCharges;
                t15PoDetail.Value = model.Value;
                t15PoDetail.DelvDt = model.DelvDt;
                t15PoDetail.ExtDelvDt = model.ExtDelvDt;
                t15PoDetail.UserId = model.UserId;
                t15PoDetail.Updatedby = model.Updatedby.ToString();
                t15PoDetail.Updateddate = DateTime.Now;
                context.SaveChanges();
                ItemSrno = Convert.ToInt32(t15PoDetail.ItemSrno);
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
                result[1] = ds.Tables[0].Rows[1]["OUT_CASE_NO"].ToString();
            }
            return result;
        }

        public DTResult<ConsigneeListModel> GetConsigneeDetaisList(DTParameters dtParameters)
        {

            DTResult<ConsigneeListModel> dTResult = new() { draw = 0 };
            IQueryable<ConsigneeListModel>? query = null;

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
            string CaseNo = "";
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["CaseNo"]))
            {
                CaseNo = Convert.ToString(dtParameters.AdditionalValues["CaseNo"]);
            }

            OracleParameter[] par = new OracleParameter[2];
            par[0] = new OracleParameter("p_case_no", OracleDbType.Varchar2, CaseNo.ToString() == "" ? DBNull.Value : CaseNo.ToString(), ParameterDirection.Input);
            par[1] = new OracleParameter("p_result_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GET_PO_BPO_DETAILS", par, 1);
            DataTable dt = ds.Tables[0];
            List<ConsigneeListModel> list = new();
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                list = JsonConvert.DeserializeObject<List<ConsigneeListModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }
            query = list.AsQueryable();
            dTResult.recordsTotal = query.Count();
            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.CASE_NO).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.CONSIGNEE_CD).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public bool ConsigneeDelete(string CASE_NO, string CONSIGNEE_CD, string BPO_CD)
        {
            T14PoBpo t14PoBpo = (from pm in context.T14PoBpos
                                 where pm.CaseNo == CASE_NO && pm.ConsigneeCd == Convert.ToInt32(CONSIGNEE_CD) && pm.BpoCd == BPO_CD
                                 select pm).FirstOrDefault();
            if (t14PoBpo == null) { return false; }

            context.T14PoBpos.Remove(t14PoBpo);
            context.SaveChanges();
            return true;
        }

        public ConsigneeModel FindConsigneeByID(string CaseNo, int Consignee_CD)
        {
            ConsigneeModel model = new();
            T14PoBpo t14PoBpo = context.T14PoBpos.Where(x => x.CaseNo == CaseNo && x.ConsigneeCd == Consignee_CD).FirstOrDefault();

            if (t14PoBpo == null)
                throw new Exception("Consignee Not found");
            else
            {
                model.CaseNo = t14PoBpo.CaseNo;
                model.ConsigneeCd = t14PoBpo.ConsigneeCd;
                model.BpoCd = t14PoBpo.BpoCd;
                return model;
            }
        }

        public string SaveConsignee(ConsigneeModel model)
        {
            string CaseNo = "";
            T14PoBpo t14PoBpo = context.T14PoBpos.Where(x => x.CaseNo == model.CaseNo && x.ConsigneeCd == model.ConsigneeCd).FirstOrDefault();
            if (t14PoBpo == null)
            {
                T14PoBpo obj = new T14PoBpo();
                obj.CaseNo = model.CaseNo;
                obj.ConsigneeCd = model.ConsigneeCd;
                obj.BpoCd = model.BpoCd;
                context.T14PoBpos.Add(obj);
                context.SaveChanges();
                CaseNo = obj.CaseNo;
            }
            else
            {
                t14PoBpo.BpoCd = model.BpoCd;
                context.SaveChanges();
                CaseNo = t14PoBpo.CaseNo;
            }
            return CaseNo;
        }

        public string UpdatePODate(AdministratorPurchaseOrderModel model)
        {
            string CaseNo = "";
            var POMaster = context.T13PoMasters.Find(model.CaseNo);
            if (POMaster != null)
            {
                POMaster.PoDt = model.PoDtNew;
                context.SaveChanges();
                CaseNo = POMaster.CaseNo;
            }
            return CaseNo;
        }
    }
}
