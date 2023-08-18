
using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces.Vendor;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Dynamic;
using System.Globalization;
using System.Net.NetworkInformation;
using System.Security.Cryptography.Xml;

namespace IBS.Repositories.Vendor
{
    public class CallRegisterRepository : ICallRegisterRepository
    {
        private readonly ModelContext context;

        public CallRegisterRepository(ModelContext context)
        {
            this.context = context;
        }

        public CallRegisterModel FindByID(string CaseNo, string CallRecvDt, string CallSNo, string UserName)
        {
            CallRegisterModel model = new();
            DataTable dt = new DataTable();
            string PoNo = "", PoDt = "", VendName = "", CallLetterNo = "", VendCd = "";

            CaseNo = CaseNo.ToString() == "" ? string.Empty : CaseNo.ToString();
            DateTime? _CallRecvDt = CallRecvDt == "" ? null : DateTime.ParseExact(CallRecvDt, "dd-MM-yyyy", null);
            PoNo = PoNo.ToString() == "" ? string.Empty : PoNo.ToString();
            DateTime? _PoDt = PoDt == "" ? null : DateTime.ParseExact(PoDt, "dd-MM-yyyy", null);
            VendName = VendName.ToString() == "" ? string.Empty : VendName.ToString();
            VendCd = VendCd.ToString() == "" ? string.Empty : VendCd.ToString();
            CallSNo = CallSNo.ToString() == "" ? string.Empty : CallSNo.ToString();

            OracleParameter[] par = new OracleParameter[9];
            par[0] = new OracleParameter("p_case_no", OracleDbType.Varchar2, CaseNo, ParameterDirection.Input);
            par[1] = new OracleParameter("p_dt_of_receipt", OracleDbType.Date, _CallRecvDt, ParameterDirection.Input);
            par[2] = new OracleParameter("p_po_no", OracleDbType.Varchar2, PoNo, ParameterDirection.Input);
            par[3] = new OracleParameter("p_po_dt", OracleDbType.Date, _PoDt, ParameterDirection.Input);
            par[4] = new OracleParameter("p_vend_name", OracleDbType.Varchar2, VendName, ParameterDirection.Input);
            par[5] = new OracleParameter("p_call_let_no", OracleDbType.Varchar2, CallLetterNo, ParameterDirection.Input);
            par[6] = new OracleParameter("p_vend_cd", OracleDbType.Varchar2, UserName, ParameterDirection.Input);
            par[7] = new OracleParameter("p_callsno", OracleDbType.Varchar2, CallSNo, ParameterDirection.Input);
            par[8] = new OracleParameter("p_result_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("SP_GET_CALLREGISTER", par, 1);
            if (ds != null)
            {
                dt = ds.Tables[0];
                List<CallRegisterModel> list = dt.AsEnumerable().Select(row => new CallRegisterModel
                {
                    CaseNo = row["CaseNo"].ToString(),
                    PoNo = row["PoNo"].ToString(),
                    PoDt = Convert.ToDateTime(row["PoDt"]),
                    CallRecvDt = Convert.ToDateTime(row["CallRecvDt"]),
                    VendName = row["VendName"].ToString(),
                    CallInstallNo = Convert.ToInt32(row["CallInstallNo"]),
                    CallSNo = Convert.ToInt32(row["CallSNo"]),
                    CallStatus = row["CallStatus"].ToString(),
                    CallLetterNo = row["CallLetterNo"].ToString(),
                    Remarks = row["Remarks"].ToString(),
                    IE_SName = row["IE_SName"].ToString(),
                }).ToList();
                if (list == null)
                    throw new Exception("Record Not found");
                else
                {
                    model.CaseNo = list[0].CaseNo;
                    model.PoDt = list[0].PoDt;
                    model.PoNo = list[0].PoNo;
                    model.VendName = list[0].VendName;
                    model.CallLetterNo = list[0].CallLetterNo;
                    //return model;
                }
            }
            return model;
        }

        public CallRegisterModel FindManageByID(string CaseNo, string CallRecvDt, int CallSNo, int ItemSrNoPo, string UserName)
        {
            CallRegisterModel model = new();
            string dt = Convert.ToDateTime(CallRecvDt).ToString("dd-MM-yyyy");
            DateTime parsedDate = DateTime.ParseExact(dt, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var GetValuePO = (from v in context.T09Ies
                              join d in context.T17CallRegisters on v.IeCd equals d.IeCd
                              where d.CaseNo == CaseNo && d.CallRecvDt == parsedDate && d.CallSno == CallSNo
                              select new
                              {
                                  v,
                                  d
                              }
                  ).FirstOrDefault();

            var PODetails = context.T13PoMasters.Where(x => x.CaseNo == CaseNo).FirstOrDefault();

            var CallDetails = context.T18CallDetails.Where(x => x.CaseNo == CaseNo && x.CallRecvDt == parsedDate && x.CallSno == CallSNo && x.ItemSrnoPo == ItemSrNoPo).FirstOrDefault();

            if (GetValuePO == null)
                throw new Exception("Record Not found");
            else
            {
                model.IE_SName = GetValuePO.v.IeSname;
                if (PODetails != null)
                {
                    model.PoNo = PODetails.PoNo;
                    model.PoDt = PODetails.PoDt;
                }
            }
            if (CallDetails != null)
            {
                model.ItemSrNoPo = CallDetails.ItemSrnoPo;
                model.ItemDescPo = CallDetails.ItemDescPo;
                model.QtyOrdered = CallDetails.QtyOrdered;
                model.CumQtyPrevOffered = CallDetails.CumQtyPrevOffered;
                model.CumQtyPrevPassed = CallDetails.CumQtyPrevPassed;
                model.QtyToInsp = CallDetails.QtyToInsp;
            }


            return model;
        }

        public DTResult<CallRegisterModel> FindByModifyDetail(string CaseNo, string CallRecvDt, int CallSNo, string UserName)
        {
            DTResult<CallRegisterModel> dTResult = new() { draw = 0 };
            IQueryable<CallRegisterModel>? query = null;

            query = from p in context.T13PoMasters
                    join v in context.T05Vendors on p.VendCd equals v.VendCd
                    where p.CaseNo == CaseNo
                    select new CallRegisterModel
                    {
                        VendCd = Convert.ToString(v.VendCd),
                        VendInspStopped = v.VendInspStopped,
                        VendRemarks = v.VendRemarks,
                    };

            dTResult.data = query;
            return dTResult;
        }

        public DTResult<CallRegisterModel> FindMatchDetail(string CaseNo, string CallRecvDt, int CallSNo, string UserName)
        {
            DTResult<CallRegisterModel> dTResult = new() { draw = 0 };
            IQueryable<CallRegisterModel>? query = null;

            string dt = Convert.ToDateTime(CallRecvDt).ToString("dd-MM-yyyy");
            DateTime parsedDate = DateTime.ParseExact(dt, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            query = from c in context.T17CallRegisters
                    join p in context.T13PoMasters on c.CaseNo equals p.CaseNo
                    where p.CaseNo == CaseNo && c.CallRecvDt == parsedDate && c.CallSno == CallSNo
                    select new CallRegisterModel
                    {
                        VendCd = Convert.ToString(p.VendCd),
                        MfgCd = c.MfgCd,
                        CallStatus = c.CallStatus,
                    };

            dTResult.data = query;
            return dTResult;
        }

        public DTResult<CallRegisterModel> GetDataList(DTParameters dtParameters, string UserName)
        {

            DTResult<CallRegisterModel> dTResult = new() { draw = 0 };
            IQueryable<CallRegisterModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "" || orderCriteria == null)
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

            string CaseNo = "", CallRecvDt = "", PoNo = "", PoDt = "", VendName = "", CallLetterNo = "", VendCd = "", CallSNo = "";

            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["CaseNo"]))
            {
                CaseNo = Convert.ToString(dtParameters.AdditionalValues["CaseNo"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["CallRecvDt"]))
            {
                CallRecvDt = Convert.ToString(dtParameters.AdditionalValues["CallRecvDt"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["PoNo"]))
            {
                PoNo = Convert.ToString(dtParameters.AdditionalValues["PoNo"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["PoDt"]))
            {
                PoDt = Convert.ToString(dtParameters.AdditionalValues["PoDt"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["VendName"]))
            {
                VendName = Convert.ToString(dtParameters.AdditionalValues["VendName"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["CallLetterNo"]))
            {
                CallLetterNo = Convert.ToString(dtParameters.AdditionalValues["CallLetterNo"]);
            }

            CaseNo = CaseNo.ToString() == "" ? string.Empty : CaseNo.ToString();
            DateTime? _CallRecvDt = CallRecvDt == "" ? null : DateTime.ParseExact(CallRecvDt, "dd-MM-yyyy", null);
            PoNo = PoNo.ToString() == "" ? string.Empty : PoNo.ToString();
            DateTime? _PoDt = PoDt == "" ? null : DateTime.ParseExact(PoDt, "dd-MM-yyyy", null);
            VendName = VendName.ToString() == "" ? string.Empty : VendName.ToString();
            VendCd = VendCd.ToString() == "" ? string.Empty : VendCd.ToString();
            CallSNo = CallSNo.ToString() == "" ? string.Empty : CallSNo.ToString();

            OracleParameter[] par = new OracleParameter[9];
            par[0] = new OracleParameter("p_case_no", OracleDbType.Varchar2, CaseNo, ParameterDirection.Input);
            par[1] = new OracleParameter("p_dt_of_receipt", OracleDbType.Date, _CallRecvDt, ParameterDirection.Input);
            par[2] = new OracleParameter("p_po_no", OracleDbType.Varchar2, PoNo, ParameterDirection.Input);
            par[3] = new OracleParameter("p_po_dt", OracleDbType.Date, _PoDt, ParameterDirection.Input);
            par[4] = new OracleParameter("p_vend_name", OracleDbType.Varchar2, VendName, ParameterDirection.Input);
            par[5] = new OracleParameter("p_call_let_no", OracleDbType.Varchar2, CallLetterNo, ParameterDirection.Input);
            par[6] = new OracleParameter("p_vend_cd", OracleDbType.Varchar2, UserName, ParameterDirection.Input);
            par[7] = new OracleParameter("p_callsno", OracleDbType.Varchar2, CallSNo, ParameterDirection.Input);
            par[8] = new OracleParameter("p_result_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("SP_GET_CALLREGISTER", par, 1);
            DataTable dt = ds.Tables[0];

            CallRegisterModel model = new();
            List<CallRegisterModel> list = new();
            if (ds != null && ds.Tables.Count > 0)
            {
                //string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                //model = JsonConvert.DeserializeObject<List<BillRegisterModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }).FirstOrDefault();

                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                list = JsonConvert.DeserializeObject<List<CallRegisterModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }

            query = list.AsQueryable();

            dTResult.recordsTotal = ds.Tables[0].Rows.Count;

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.CaseNo).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = ds.Tables[0].Rows.Count;

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;
            //List<CallRegisterModel> list = new ();   
            //if (ds != null)
            //{
            //    list = dt.AsEnumerable().Select(row => new CallRegisterModel
            //    {
            //        CaseNo = row["CASE_NO"].ToString(),
            //        PoNo = row["PO_NO"].ToString(),
            //        PoDt = Convert.ToDateTime(row["PO_DT"]),
            //        CallRecvDt = Convert.ToDateTime(row["CALL_RECV_DT"]),
            //        VendName = row["VENDOR"].ToString(),
            //        CallInstallNo = Convert.ToInt32(row["CALL_INSTALL_NO"]),
            //        CallSNo = Convert.ToInt32(row["CALL_SNO"]),
            //        CallStatus = row["CALL_STATUS"].ToString(),
            //        CallLetterNo = row["CALL_LETTER_NO"].ToString(),
            //        Remarks = row["REMARKS"].ToString(),
            //        IE_SName = row["IE_SNAME"].ToString(),
            //    }).ToList();
            //}


            //query = list.AsQueryable();

            //dTResult.recordsTotal = ds.Tables[0].Rows.Count;

            //dTResult.recordsFiltered = ds.Tables[0].Rows.Count;

            //dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            //dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public DTResult<CallRegisterModel> GetCallDetailsList(DTParameters dtParameters)
        {

            DTResult<CallRegisterModel> dTResult = new() { draw = 0 };
            IQueryable<CallRegisterModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = false;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "ItemSrNoPo";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
            }
            else
            {
                orderCriteria = "ItemSrNoPo";
                orderAscendingDirection = true;
            }

            string CaseNo = "", CallRecvDt = "", CallSNo = "";

            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["CaseNo"]))
            {
                CaseNo = Convert.ToString(dtParameters.AdditionalValues["CaseNo"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["CallRecvDt"]))
            {
                CallRecvDt = Convert.ToString(dtParameters.AdditionalValues["CallRecvDt"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["CallSNo"]))
            {
                CallSNo = Convert.ToString(dtParameters.AdditionalValues["CallSNo"]);
            }

            CaseNo = CaseNo.ToString() == "" ? string.Empty : CaseNo.ToString();
            DateTime? _CallRecvDt = CallRecvDt == "" ? null : DateTime.ParseExact(CallRecvDt, "dd-MM-yyyy", null);
            CallSNo = CallSNo.ToString() == "" ? string.Empty : CallSNo.ToString();

            OracleParameter[] par = new OracleParameter[4];
            par[0] = new OracleParameter("p_CNO", OracleDbType.Varchar2, CaseNo, ParameterDirection.Input);
            par[1] = new OracleParameter("p_DT", OracleDbType.Date, _CallRecvDt, ParameterDirection.Input);
            par[2] = new OracleParameter("p_CSNO", OracleDbType.Int32, CallSNo, ParameterDirection.Input);

            par[3] = new OracleParameter("p_result_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("SP_GET_CALL_DETAILS", par, 1);
            DataTable dt = ds.Tables[0];

            List<CallRegisterModel> list = dt.AsEnumerable().Select(row => new CallRegisterModel
            {
                CallStatus = row["STATUS"].ToString(),
                ItemSrNoPo = Convert.ToInt32(row["ITEM_SRNO_PO"]),
                ItemDescPo = row["ITEM_DESC_PO"].ToString(),

                QtyOrdered = Convert.ToInt32(row["QTY_ORDERED"]),
                CumQtyPrevOffered = Convert.ToInt32(row["CUM_QTY_PREV_OFFERED"]),
                CumQtyPrevPassed = Convert.ToInt32(row["CUM_QTY_PREV_PASSED"]),
                QtyToInsp = Convert.ToInt32(row["QTY_TO_INSP"]),
                QtyPassed = Convert.ToInt32(row["QTY_PASSED"]),
                QtyRejected = Convert.ToInt32(row["QTY_REJECTED"]),
                QtyDue = Convert.ToInt32(row["QTY_DUE"]),
                Consignee = row["CONSIGNEE"].ToString(),
            }).ToList();

            query = list.AsQueryable();

            dTResult.recordsTotal = ds.Tables[0].Rows.Count;

            dTResult.recordsFiltered = ds.Tables[0].Rows.Count;

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public int DetailsSave(CallRegisterModel model, string UserName)
        {
            int Id = 0;
            //var GetValue = context.VendPoMaDetails.Find(CaseNo, CallRecvDt, CallSNo);
            int status = 0;
            decimal reader = 0;
            decimal qty = 0;

            var CallDetails = (from c in context.T17CallRegisters
                               join d in context.T18CallDetails on c.CaseNo equals d.CaseNo
                               where c.CaseNo.Equals(d.CaseNo) && c.CallRecvDt.Equals(d.CallRecvDt) && c.CallSno.Equals(d.CallSno)
                               && c.CaseNo == model.CaseNo && d.ItemSrnoPo == model.ItemSrNoPo && !new[] { "R", "C" }.Contains(c.CallStatus)
                               select new
                               {
                                   d.QtyToInsp
                               }
                  ).FirstOrDefault();
            reader = Convert.ToDecimal(CallDetails.QtyToInsp);
            if (reader > 0)
            {
                qty = (reader + Convert.ToDecimal(model.QtyToInsp)) - Convert.ToDecimal(model.QtyToInsp);
                if (reader > Convert.ToDecimal(model.QtyOrdered))
                {
                    status = 2;
                }
                else
                {
                    if (qty > Convert.ToDecimal(model.QtyOrdered))
                    {
                        status = 1;
                    }
                }
            }
            if (status != 2)
            {
                if (status == 1)
                {
                    return 1;
                }
                else
                {
                    var CallDetailsUpdate = context.T18CallDetails.Where(x => x.CaseNo == model.CaseNo && x.CallRecvDt == model.CallRecvDt && x.CallSno == model.CallSNo && x.ItemSrnoPo == model.ItemSrNoPo).FirstOrDefault();
                    
                    #region CallDetailsUpdate
                    if (CallDetailsUpdate != null)
                    {
                        CallDetailsUpdate.ItemDescPo = model.ItemDescPo;
                        CallDetailsUpdate.QtyOrdered = model.QtyOrdered;
                        CallDetailsUpdate.CumQtyPrevOffered = model.CumQtyPrevOffered;
                        CallDetailsUpdate.CumQtyPrevPassed = model.CumQtyPrevPassed;
                        CallDetailsUpdate.QtyToInsp = model.QtyToInsp;
                        CallDetailsUpdate.UserId = UserName;
                        CallDetailsUpdate.Datetime = DateTime.Now;
                        CallDetailsUpdate.Updatedby = model.Updatedby;
                        CallDetailsUpdate.Updateddate = DateTime.Now;
                        context.SaveChanges();
                        Id = Convert.ToInt32(3);
                    }
                    #endregion

                    var PODetailsUpdate = context.T15PoDetails.Where(x => x.CaseNo == model.CaseNo && x.ItemSrno == model.ItemSrNoPo).FirstOrDefault();

                    #region PODetailsUpdate 
                    if (PODetailsUpdate != null)
                    {
                        PODetailsUpdate.ItemDesc = model.ItemDescPo;
                        context.SaveChanges();
                        Id = Convert.ToInt32(3);
                    }
                    #endregion
                }
            }
            else if(status == 2)
            {
                return 2;
            }
            
            return Id;
        }
    }
}
