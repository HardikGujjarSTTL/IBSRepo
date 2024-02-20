using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

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
            T12BillPayingOfficer BPO = context.T12BillPayingOfficers.Where(x => x.BpoCd == BpoCd).FirstOrDefault();

            var BPOState = (from s in context.T92States
                            where s.StateCd == (from c in context.T03Cities
                                                where c.CityCd == BPO.BpoCityCd
                                                select c.StateCd).FirstOrDefault()
                            select Convert.ToString(s.StateCd).PadLeft(2, '0') + "-" + s.StateName
                         ).FirstOrDefault();


            if (BPO == null)
                return model;
            else
            {
                model.BpoCd = BPO.BpoCd;
                model.BpoRegion = BPO.BpoRegion;
                model.BpoType = BPO.BpoType;
                model.BpoName = BPO.BpoName;
                model.BpoRlylst = BPO.BpoType == "R" ? BPO.BpoRly : BPO.BpoRly;
                model.BpoRly = BPO.BpoType != "R" ? BPO.BpoRly : BPO.BpoRly;
                model.BpoAdd = BPO.BpoAdd;
                model.BpoCityCd = Convert.ToInt32(BPO.BpoCityCd);
                model.BpoCity = Convert.ToString(BPO.BpoCityCd);
                model.BillPassOfficer = BPO.BillPassOfficer;
                model.BpoFeeType = BPO.BpoFeeType;
                model.BpoFee = BPO.BpoFee;
                model.BpoTaxType = (BPO.BpoTaxType == "" || BPO.BpoTaxType == null) ? "X" : BPO.BpoTaxType;
                model.BpoFlg = BPO.BpoFlg;
                model.BpoAdvFlg = BPO.BpoAdvFlg;
                model.BpoLocCd = BPO.BpoLocCd;
                model.BpoOrgn = BPO.BpoOrgn;
                model.BpoAdd1 = BPO.BpoAdd1;
                model.BpoAdd2 = BPO.BpoAdd2;
                model.lblBpoState = Convert.ToString(BPOState);
                model.BpoState = BPO.BpoState;
                model.BpoPhone = BPO.BpoPhone;
                model.BpoFax = BPO.BpoFax;
                model.BpoEmail = BPO.BpoEmail;
                model.PayWindowId = BPO.PayWindowId;
                model.GstinNo = BPO.GstinNo;
                model.Au = BPO.Au;
                model.PinCode = BPO.PinCode;
                model.SapCustCdBpo = BPO.SapCustCdBpo;

                model.UserId = BPO.UserId;
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

            query = from l in context.ViewGetBpodetails
                    where (string.IsNullOrEmpty(BpoCd) || l.BpoCd == BpoCd)
                    && (string.IsNullOrEmpty(BpoName) || l.BpoName == BpoName)
                    && (string.IsNullOrEmpty(BpoRly) || l.BpoRly == BpoRly)
                    && (string.IsNullOrEmpty(BpoCity) || l.City.Equals(BpoCity))
                    && (string.IsNullOrEmpty(SapCustCdBpo) || l.SapCustCdBpo == SapCustCdBpo)
                    && (string.IsNullOrEmpty(GstinNo) || l.GstinNo == GstinNo)
                    && l.Isdeleted != 1
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
            var T12 = context.T12BillPayingOfficers.Where(x => x.BpoCd == Convert.ToString(BpoCd)).FirstOrDefault();
            if (T12 == null) { return false; }

            T12.Isdeleted = Convert.ToByte(true);
            T12.Updatedby = Convert.ToInt32(UserID);
            T12.Updateddate = DateTime.Now;
            context.SaveChanges();
            return true;
        }

        public string GetMaxBPOCd()
        {
            string BPOCd = "";

            using ModelContext context = new(DbContextHelper.GetDbContextOptions());
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                bool wasOpen = command.Connection.State == ConnectionState.Open;
                if (!wasOpen) command.Connection.Open();
                try
                {
                    //command.CommandText = "SELECT NVL(MAX(BANK_CD), 0) FROM T94_BANK WHERE BANK_CD < 990";
                    command.CommandText = "SELECT LPAD(to_number(NVL(MAX(NVL(BPO_CD,0)),0)+1),5,'0') FROM T12_BILL_PAYING_OFFICER";
                    BPOCd = Convert.ToString(command.ExecuteScalar());
                }
                finally
                {
                    if (!wasOpen) command.Connection.Close();
                }
            }
            return BPOCd;
        }

        public string BPOSave(Bill_Paying_Officer_FormModel model)
        {
            string BPOCd = "";
            if (model.BpoCd == null)
            {
                var Cnt = context.T12BillPayingOfficers.Where(x => x.BpoType == model.BpoType && x.BpoRly == (model.BpoType == "R" ? model.BpoRlylst : model.BpoRly) && x.BpoRegion == model.BpoRegion).Count();
                if(Cnt > 0)
                {
                    return "-1";
                }

                BPOCd = GetMaxBPOCd();

                T12BillPayingOfficer bpo = new()
                {
                    BpoCd = BPOCd,
                    BpoRegion = model.BpoRegion,
                    BpoType = model.BpoType,
                    BpoName = model.BpoName,

                    BpoRly = model.BpoType == "R" ? model.BpoRlylst : model.BpoRly,
                    BpoAdd = model.BpoAdd,
                    BpoCityCd = Convert.ToInt32(model.BpoCity),
                    BillPassOfficer = model.BillPassOfficer,
                    BpoFeeType = model.BpoFeeType,
                    BpoFee = model.BpoFee,

                    BpoTaxType = model.BpoTaxType == "X" ? "" : model.BpoTaxType,
                    BpoFlg = model.BpoFlg,
                    BpoAdvFlg = model.BpoAdvFlg,
                    BpoLocCd = model.BpoLocCd,
                    BpoOrgn = model.BpoOrgn,
                    BpoAdd1 = model.BpoAdd1,
                    BpoAdd2 = model.BpoAdd2,
                    BpoState = model.BpoState,
                    BpoPhone = model.BpoPhone,
                    BpoFax = model.BpoFax,
                    BpoEmail = model.BpoEmail,
                    PayWindowId = model.BpoType == "R" ? model.PayWindowId : null,
                    UserId = model.UserId,
                    Datetime = DateTime.Now.Date,
                    GstinNo = model.GstinNo,
                    Au = model.BpoType == "R" ? model.Au : null,
                    LegalName = model.BpoType == "R" ? "MINISTRY OF RAILWAYS" : null,
                    PinCode = model.PinCode,

                    Createdby = model.Createdby,
                    Createddate = DateTime.Now,
                };

                context.T12BillPayingOfficers.Add(bpo);
                context.SaveChanges();
            }
            else
            {
                T12BillPayingOfficer bpo = context.T12BillPayingOfficers.Find(model.BpoCd);

                if (bpo != null)
                {
                    bpo.BpoRegion = model.BpoRegion;
                    bpo.BpoType = model.BpoType;
                    bpo.BpoName = model.BpoName;
                    bpo.BpoRly = model.BpoType == "R" ? model.BpoRlylst : model.BpoRly;
                    bpo.BpoAdd = model.BpoAdd;
                    bpo.BpoCityCd = Convert.ToInt32(model.BpoCity);
                    bpo.BillPassOfficer = model.BillPassOfficer;
                    bpo.BpoFeeType = model.BpoFeeType;
                    bpo.BpoFee = model.BpoFee;
                    bpo.BpoTaxType = model.BpoTaxType == "X" ? "" : model.BpoTaxType;
                    bpo.BpoFlg = model.BpoFlg;
                    bpo.BpoAdvFlg = model.BpoAdvFlg;
                    bpo.BpoLocCd = model.BpoLocCd;
                    bpo.BpoOrgn = model.BpoOrgn;
                    bpo.BpoAdd1 = model.BpoAdd1;
                    bpo.BpoAdd2 = model.BpoAdd2;
                    bpo.BpoState = model.BpoState;
                    bpo.BpoPhone = model.BpoPhone;
                    bpo.BpoFax = model.BpoFax;
                    bpo.BpoEmail = model.BpoEmail;
                    bpo.PayWindowId = model.BpoType == "R" ? model.PayWindowId : null;
                    bpo.UserId = model.UserId;
                    bpo.Datetime = DateTime.Now.Date;
                    bpo.GstinNo = model.GstinNo.ToUpper();
                    bpo.Au = model.BpoType == "R" ? model.Au : null;
                    bpo.PinCode = model.PinCode;

                    bpo.Updatedby = model.Updatedby;
                    bpo.Updateddate = DateTime.Now;

                    context.SaveChanges();
                    BPOCd = model.BpoCd;
                }
            }
            return BPOCd;

        }

        public string GetState(int BpoCityCd)
        {
            var BPOState = (from s in context.T92States
                            where s.StateCd == (from c in context.T03Cities
                                                where c.CityCd == BpoCityCd
                                                select c.StateCd).FirstOrDefault()
                            select Convert.ToString(s.StateCd).PadLeft(2, '0') + "-" + s.StateName
                        ).FirstOrDefault();

            return BPOState;
        }
    }

}