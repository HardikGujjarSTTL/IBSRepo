using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces.Vendor;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data;
using System.Dynamic;
using System.Globalization;
using System.Reflection.Emit;

namespace IBS.Repositories.Vendor
{
    public class PurchesOrder1LOARepository : IPurchesOrder1LOARepository
    {
        private readonly ModelContext context;

        public PurchesOrder1LOARepository(ModelContext context)
        {
            this.context = context;
        }

        public PurchesOrder1LOAModel FindByID(string CaseNo)
        {
            PurchesOrder1LOAModel model = new();
            DataTable dt = new DataTable();
            string SetLetterNo = "";
            T13PoMaster POLetterValue = context.T13PoMasters.Find(CaseNo);
            if (POLetterValue.PoOrLetter == "P")
            {
                SetLetterNo = "0";
            }
            else if (POLetterValue.PoOrLetter == "L")
            {
                SetLetterNo = "2";
            }
            else
            {
                SetLetterNo = "1";
            }

            if (SetLetterNo == "2")
            {
                OracleParameter[] par = new OracleParameter[5];
                par[0] = new OracleParameter("p_CsNo", OracleDbType.Varchar2, CaseNo, ParameterDirection.Input);
                par[1] = new OracleParameter("p_PONo", OracleDbType.Varchar2, "", ParameterDirection.Input);
                par[2] = new OracleParameter("p_PODate", OracleDbType.Varchar2, "", ParameterDirection.Input);
                par[3] = new OracleParameter("p_VendName", OracleDbType.Varchar2, "", ParameterDirection.Input);
                par[4] = new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

                var ds = DataAccessDB.GetDataSet("SP_GET_PO_DETAILS_LIST", par, 1);
                dt = ds.Tables[0];

            }
            if (dt != null)
            {
                List<PurchesOrder1LOAModel> list = dt.AsEnumerable().Select(row => new PurchesOrder1LOAModel
                {
                    CaseNo = row["CASE_NO"].ToString(),
                    PoDt = Convert.ToDateTime(row["PO_DT"]),
                    PoNo = (row["PO_NO"]).ToString(),
                    VendName = (row["VEND_NAME"]).ToString(),
                }).ToList();
                if (list == null)
                    throw new Exception("Record Not found");
                else
                {
                    model.CaseNo = list[0].CaseNo;
                    model.PoDt = list[0].PoDt;
                    model.PoNo = list[0].PoNo;
                    model.VendName = list[0].VendName;
                    return model;
                }
            }
            return model;
        }

        public PurchesOrder1LOAModel FindByDetail(string CaseNo, byte ItemSrno, string type, string PoDt, int lstItemDesc)
        {
            PurchesOrder1LOAModel model = new();

            T13PoMaster POMaster = context.T13PoMasters.Where(x => x.CaseNo == CaseNo).FirstOrDefault();

            int DetailsItemCount = context.T15PoDetails.Where(x => x.CaseNo == CaseNo).Count() + 1;

            CaseNo = CaseNo.ToString() == "" ? string.Empty : CaseNo.ToString();
            //type = type.ToString() == "" ? string.Empty : type.ToString();
            //PoDt = Convert.ToDateTime(PoDt).ToString("dd/MM/yyyyy");
            T15PoDetail PoDetails = context.T15PoDetails.Where(x => x.CaseNo == CaseNo && x.ItemSrno == ItemSrno).FirstOrDefault();

            string dt = Convert.ToDateTime(PoDt).ToString("dd-MM-yyyy");
            DateTime parsedDate = DateTime.ParseExact(dt, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var rate_cr = context.T35RailPriceDetails.Where(p => p.RailId == lstItemDesc && p.PriceDateFr >= parsedDate && p.PriceDateTo <= parsedDate)
                .Select(p => (p.RailPricePerMt ?? 0) - (p.PackingCharge ?? 0))
                .FirstOrDefault();

            if (POMaster == null)
                throw new Exception("PO Record Not found");
            else
            {
                model.CaseNo = POMaster.CaseNo;
                model.ItemSrno = (byte)DetailsItemCount;
                model.PoDt = parsedDate;
                if (PoDetails != null)
                {
                    model.ItemDesc = PoDetails.ItemDesc;
                    model.ConsigneeCd = PoDetails.ConsigneeCd;
                    model.Qty = PoDetails.Qty;
                    model.BasicValue = PoDetails.BasicValue;
                    model.Rate = PoDetails.Rate;
                    model.UomCd = PoDetails.UomCd;
                    model.SalesTaxPer = PoDetails.SalesTaxPer;
                    model.SalesTax = PoDetails.SalesTax;
                    model.Excise = PoDetails.Excise;
                    model.ExcisePer = PoDetails.ExcisePer;
                    model.ExciseType = PoDetails.ExciseType;
                    model.Discount = PoDetails.Discount;
                    model.DiscountPer = PoDetails.DiscountPer;
                    model.DiscountType = PoDetails.DiscountType;
                    model.OtherCharges = PoDetails.OtherCharges;
                    model.OtChargePer = PoDetails.OtChargePer;
                    model.OtChargeType = PoDetails.OtChargeType;
                    model.Value = PoDetails.Value;
                    model.DelvDt = PoDetails.DelvDt;
                    model.ExtDelvDt = PoDetails.ExtDelvDt;
                }
                if (rate_cr != 0)
                {
                    model.Rate = Convert.ToDecimal(rate_cr);
                }
                return model;
            }
        }

        public DTResult<PurchesOrder1LOAModel> FindByUOMDetail(decimal id)
        {
            DTResult<PurchesOrder1LOAModel> dTResult = new() { draw = 0 };
            IQueryable<PurchesOrder1LOAModel>? query = null;

            query = from l in context.T04Uoms
                    where l.UomCd == id

                    select new PurchesOrder1LOAModel
                    {
                        UOMFactor = l.UomFactor
                    };

            dTResult.data = query;
            return dTResult;
        }

        public DTResult<PurchesOrder1LOAModel> GetDataList(DTParameters dtParameters)
        {

            DTResult<PurchesOrder1LOAModel> dTResult = new() { draw = 0 };
            IQueryable<PurchesOrder1LOAModel>? query = null;

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
                orderCriteria = "CaseNo";
                orderAscendingDirection = true;
            }

            string CaseNo = "", PoDt = "", PoNo = "", VendName = "";

            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["CaseNo"]))
            {
                CaseNo = Convert.ToString(dtParameters.AdditionalValues["CaseNo"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["PoDt"]))
            {
                PoDt = Convert.ToString(dtParameters.AdditionalValues["PoDt"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["PoNo"]))
            {
                PoNo = Convert.ToString(dtParameters.AdditionalValues["PoNo"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["VendName"]))
            {
                VendName = Convert.ToString(dtParameters.AdditionalValues["VendName"]);
            }

            CaseNo = CaseNo.ToString() == "" ? string.Empty : CaseNo.ToString();
            //PoDt = Convert.ToDateTime(PoDt).ToString("dd/MM/yyyyy");
            PoNo = PoNo.ToString() == "" ? string.Empty : PoNo.ToString();
            VendName = VendName.ToString() == "" ? string.Empty : VendName.ToString();

            OracleParameter[] par = new OracleParameter[5];
            par[0] = new OracleParameter("p_CsNo", OracleDbType.Varchar2, CaseNo, ParameterDirection.Input);
            par[1] = new OracleParameter("p_PONo", OracleDbType.Varchar2, PoNo, ParameterDirection.Input);
            par[2] = new OracleParameter("p_PODate", OracleDbType.Varchar2, PoDt, ParameterDirection.Input);
            par[3] = new OracleParameter("p_VendName", OracleDbType.Varchar2, VendName, ParameterDirection.Input);
            par[4] = new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("SP_GET_PO_DETAILS_LIST", par, 1);
            DataTable dt = ds.Tables[0];
            //PurchesOrder1LOAModel model = new();
            //if (ds != null && ds.Tables.Count > 0)
            //{
            //    string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
            //    model = JsonConvert.DeserializeObject<List<PurchesOrder1LOAModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }).FirstOrDefault();
            //}

            List<PurchesOrder1LOAModel> list = dt.AsEnumerable().Select(row => new PurchesOrder1LOAModel
            {
                CaseNo = row["CASE_NO"].ToString(),
                PoNo = row["PO_NO"].ToString(),
                PoDt = Convert.ToDateTime(row["PO_DT"]),
                RlyCd = row["RLY_CD"].ToString(),
                VendName = row["VEND_NAME"].ToString(),
                ConsigneeSName = row["CONSIGNEE_S_NAME"].ToString(),
                InspectingAgency = row["INSPECTING_AGENCY"].ToString(),
                PoDoc = row["PO_DOC"].ToString(),
                PoDoc1 = row["PO_DOC1"].ToString(),
            }).ToList();

            query = list.AsQueryable();

            dTResult.recordsTotal = ds.Tables[0].Rows.Count;

            //if (!string.IsNullOrEmpty(searchBy))
            //    query = query.Where(w => Convert.ToString(w.CaseNo).ToLower().Contains(searchBy.ToLower())
            //    || Convert.ToString(w.CaseNo).ToLower().Contains(searchBy.ToLower()) || Convert.ToString(w.CaseNo).ToLower().Contains(searchBy.ToLower())
            //    );

            dTResult.recordsFiltered = ds.Tables[0].Rows.Count;

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();
            //dTResult.data = list;
            //var filteredResults = query.Where(item => item.SomeProperty == someValue);

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public DTResult<PurchesOrder1LOAModel> GetPODataList(DTParameters dtParameters)
        {

            DTResult<PurchesOrder1LOAModel> dTResult = new() { draw = 0 };
            IQueryable<PurchesOrder1LOAModel>? query = null;

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
                orderCriteria = "CaseNo";
                orderAscendingDirection = true;
            }

            string CaseNo = "", PoDt = "", ItemSrno = "", type = "", ConsigneeCd = "", ItemDesc = "";

            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["CaseNo"]))
            {
                CaseNo = Convert.ToString(dtParameters.AdditionalValues["CaseNo"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["ItemSrno"]))
            {
                ItemSrno = Convert.ToString(dtParameters.AdditionalValues["ItemSrno"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["PoDt"]))
            {
                PoDt = Convert.ToString(dtParameters.AdditionalValues["PoDt"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["type"]))
            {
                type = Convert.ToString(dtParameters.AdditionalValues["type"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["ConsigneeCd"]))
            {
                ConsigneeCd = Convert.ToString(dtParameters.AdditionalValues["ConsigneeCd"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["ItemDesc"]))
            {
                ItemDesc = Convert.ToString(dtParameters.AdditionalValues["ItemDesc"]);
            }

            CaseNo = CaseNo.ToString() == "" ? string.Empty : CaseNo.ToString();
            PoDt = Convert.ToDateTime(PoDt).ToString("dd/MM/yyyyy");
            ItemSrno = ItemSrno.ToString() == "" ? string.Empty : ItemSrno.ToString();
            type = type.ToString() == "" ? string.Empty : type.ToString();
            ConsigneeCd = ConsigneeCd.ToString() == "" ? string.Empty : ConsigneeCd.ToString();
            ItemDesc = ItemDesc.ToString() == "" ? string.Empty : ItemDesc.ToString();


            query = from l in context.ViewT15PoDetails
                    where l.CaseNo == CaseNo && (l.ConsigneeCd == Convert.ToInt32(ConsigneeCd) || l.ConsigneeCd == null)
                    //&& l.ItemDesc == ItemDesc
                    select new PurchesOrder1LOAModel
                    {
                        CaseNo = l.CaseNo,
                        ItemSrno = l.ItemSrno,
                        ItemDesc = l.ItemDesc,
                        ConsigneeName = l.ConsigneeName,
                        Qty = l.Qty,
                        Rate = l.Rate,
                        Value = l.Value,
                        PoDt = Convert.ToDateTime(PoDt)
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

        public int DetailsUpdate(PurchesOrder1LOAModel model)
        {
            int Id = 0;
            var PoDetails = context.T15PoDetails.Find(model.CaseNo, model.ItemSrno);

            int CallCount = context.T18CallDetails.Where(x => x.CaseNo == model.CaseNo && x.ItemSrnoPo == model.ItemSrno).Count();
            var CallDetails = context.T18CallDetails.Where(x=>x.CaseNo == model.CaseNo && x.ItemSrnoPo == model.ItemSrno).FirstOrDefault();

            #region save
            if (PoDetails == null)
            {
                T15PoDetail obj = new T15PoDetail();
                obj.CaseNo = model.CaseNo;
                obj.ItemSrno = model.ItemSrno;
                obj.ItemDesc = model.ItemDesc;
                obj.ConsigneeCd = model.ConsigneeCd;
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

                context.T15PoDetails.Add(obj);
                context.SaveChanges();
                Id = Convert.ToInt32(obj.ItemSrno);
            }
            else
            {
                PoDetails.ItemDesc = model.ItemDesc;
                PoDetails.Qty = model.Qty;
                PoDetails.ConsigneeCd = model.ConsigneeCd;
                PoDetails.UomCd = model.UomCd;
                PoDetails.BasicValue = model.BasicValue;
                PoDetails.Rate = model.Rate;
                PoDetails.ExciseType = model.ExciseType;
                PoDetails.ExcisePer = model.ExcisePer;
                PoDetails.Excise = model.Excise;
                PoDetails.SalesTaxPer = model.SalesTaxPer;
                PoDetails.SalesTax = model.SalesTax;
                PoDetails.DiscountType = model.DiscountType;
                PoDetails.DiscountPer = model.DiscountPer;
                PoDetails.Discount = model.Discount;
                PoDetails.OtChargePer = model.OtChargePer;
                PoDetails.OtChargeType = model.OtChargeType;
                PoDetails.OtherCharges = model.OtherCharges;
                PoDetails.Value = model.Value;
                PoDetails.DelvDt = model.DelvDt;
                PoDetails.ExtDelvDt = model.ExtDelvDt;

                PoDetails.UserId = model.UserId;
                PoDetails.Updatedby = model.Updatedby;
                PoDetails.Updateddate = DateTime.Now;
                context.SaveChanges();
                Id = Convert.ToInt32(PoDetails.ItemSrno);
            }
            #endregion

            #region Call Details Update
            if(CallCount > 0)
            {
                if (CallDetails != null)
                {
                    CallDetails.ItemDescPo = model.ItemDesc;
                    CallDetails.QtyOrdered = model.Qty;
                    CallDetails.UserId = model.UserId;
                    context.SaveChanges();
                    Id = Convert.ToInt32(CallDetails.ItemSrnoPo);

                    var callRegister = context.T17CallRegisters.Find(model.CaseNo, model.ItemSrno);
                    if (callRegister.CaseNo == CallDetails.CaseNo && callRegister.CallRecvDt == CallDetails.CallRecvDt && callRegister.CallSno == CallDetails.CallSno)
                    {
                        if (callRegister != null)
                        {
                            CallDetails.ConsigneeCd = model.ConsigneeCd;
                            context.SaveChanges();
                        }
                    }
                }
            }
            #endregion
            return Id;
        }
    }
}
