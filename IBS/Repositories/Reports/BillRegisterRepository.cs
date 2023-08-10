using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces.Reports;
using IBS.Models;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Drawing;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace IBS.Repositories.Reports
{
    public class BillRegisterRepository : IBillRegisterRepository
    {
        private readonly ModelContext context;

        public BillRegisterRepository(ModelContext context)
        {
            this.context = context;
        }

        public DTResult<BillRegisterModel> GetDataList(DTParameters dtParameters)
        {

            DTResult<BillRegisterModel> dTResult = new() { draw = 0 };
            IQueryable<BillRegisterModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "" || orderCriteria == null)
                {
                    orderCriteria = "BILL_NO";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
            }
            else
            {
                orderCriteria = "BILL_NO";
                orderAscendingDirection = true;
            }

            string FromDt = "", ToDt = "", BillStatus = "", Region = "";
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["FromDt"]))
            {
                FromDt = Convert.ToString(dtParameters.AdditionalValues["FromDt"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["ToDt"]))
            {
                ToDt = Convert.ToString(dtParameters.AdditionalValues["ToDt"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["BillStatus"]))
            {
                BillStatus = Convert.ToString(dtParameters.AdditionalValues["BillStatus"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["Region"]))
            {
                Region = Convert.ToString(dtParameters.AdditionalValues["Region"]);
            }

            DateTime? _FromDt = FromDt == "" ? null : DateTime.ParseExact(FromDt, "dd/MM/yyyy", null);
            DateTime? _ToDt = ToDt == "" ? null : DateTime.ParseExact(ToDt, "dd/MM/yyyy", null);
            BillStatus = BillStatus.ToString() == "" ? string.Empty : BillStatus.ToString();
            Region = Region.ToString() == "" ? string.Empty : Region.ToString();

            OracleParameter[] par = new OracleParameter[5];
            par[0] = new OracleParameter("wDtFr", OracleDbType.Date, _FromDt, ParameterDirection.Input);
            par[1] = new OracleParameter("wDtTo", OracleDbType.Date, _ToDt, ParameterDirection.Input);
            par[2] = new OracleParameter("wbillstatus", OracleDbType.Varchar2, BillStatus, ParameterDirection.Input);
            par[3] = new OracleParameter("wregion", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            par[4] = new OracleParameter("p_result_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("SP_GET_BILL_REGITER_DETAILS", par, 1);
            DataTable dt = ds.Tables[0];


            BillRegisterModel model = new();
            List<BillRegisterModel> list = new();
            if (ds != null && ds.Tables.Count > 0)
            {
                //string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                //model = JsonConvert.DeserializeObject<List<BillRegisterModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }).FirstOrDefault();

                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                list = JsonConvert.DeserializeObject<List<BillRegisterModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }

            query = list.AsQueryable();

            dTResult.recordsTotal = ds.Tables[0].Rows.Count;

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.BILL_NO).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = ds.Tables[0].Rows.Count;

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public int DetailsSave(BillRegisterModel model, string commaSeparatedString, string Region)
        {
            int BID = 0;
            var BATCHID = "";
            string[] values = commaSeparatedString.Split(',');
            int maxRecordCount = context.SapBillregisterSends.Count();
            //if (maxRecordCount == 0)
            //{
            maxRecordCount = maxRecordCount + 1;
            BATCHID = Region + maxRecordCount.ToString().PadLeft(5, '0');
            //}
            //else
            //{
            //    BATCHID = Region + maxRecordCount.ToString().PadLeft(5, '0');
            //}

            var BillReg = context.SapBillregisterSends.Where(x => x.Batchid == BATCHID).FirstOrDefault();
            if (BillReg == null)
            {
                SapBillregisterSend obj = new SapBillregisterSend();
                obj.Batchid = BATCHID;
                obj.BatchDt = DateTime.Now;

                context.SapBillregisterSends.Add(obj);
                context.SaveChanges();
                string ID = new string(BATCHID.Where(char.IsDigit).ToArray());
                BID = Convert.ToInt32(ID);
                foreach (string value in values)
                {
                    var BillRegD = context.ViewGetBillregisterDtails.Where(x => x.BillNo == value).FirstOrDefault();
                    if (BillRegD != null)
                    {
                        SapBillregisterSendDetail objD = new SapBillregisterSendDetail();
                        objD.Batchid = BATCHID;
                        objD.BatchDt = DateTime.Now;
                        objD.BillNo = value;
                        objD.BillDt = BillRegD.BillDt;
                        objD.BkNo = BillRegD.BkNo;
                        objD.SetNo = BillRegD.SetNo;
                        objD.IcDt = BillRegD.IcDt;
                        objD.BillAmount = BillRegD.BillAmount;
                        objD.ServiceTax = BillRegD.ServiceTax;
                        objD.EduCess = BillRegD.EduCess;
                        objD.SheCess = BillRegD.SheCess;
                        objD.SwachhBharatCess = BillRegD.SwachhBharatCess;
                        objD.KrishiKalyanCess = BillRegD.KrishiKalyanCess;
                        objD.InspFee = BillRegD.InspFee;
                        objD.Igst = BillRegD.Igst;
                        objD.Sgst = BillRegD.Sgst;
                        objD.Cgst = BillRegD.Cgst;
                        objD.InvoiceNo = BillRegD.InvoiceNo;
                        objD.RecipientGstinNo = BillRegD.RecipientGstinNo;
                        objD.BpoType = BillRegD.BpoType;
                        objD.IrnNo = BillRegD.IrnNo;
                        objD.Senttosap = "Sent to SAP";
                        objD.Finalised = BillRegD.Finalised;
                        objD.AckDt = BillRegD.AckDt;
                        objD.DigBillGenDt = BillRegD.DigBillGenDt;
                        objD.Bpo = BillRegD.Bpo;
                        objD.MaterialValue = BillRegD.MaterialValue;
                        objD.State = BillRegD.State;
                        objD.CaseNo = BillRegD.CaseNo;

                        context.SapBillregisterSendDetails.Add(objD);
                        context.SaveChanges();
                        //string BillNo = new string(objD.BillNo.Where(char.IsDigit).ToArray());
                        //BID = Convert.ToInt32(BillNo);

                        var BillStatusUpdate = context.T22Bills.Where(x => x.BillNo == value).FirstOrDefault();
                        if (BillStatusUpdate != null)
                        {
                            BillStatusUpdate.SapStatus = "S";

                            context.SaveChanges();
                            string numeric = new string(BillStatusUpdate.BillNo.Where(char.IsDigit).ToArray());
                            BID = Convert.ToInt32(numeric);
                        }
                    }

                }

            }

            return BID;
        }
    }
}
