using IBSAPI.DataAccess;
using IBSAPI.Helper;
using IBSAPI.Interfaces;
using IBSAPI.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Linq;

namespace IBSAPI.Repositories
{
    public class VendorRepository : IVendorRepository
    {
        int e_status = 0;
        private readonly ModelContext context;
        public VendorRepository(ModelContext context)
        {
            this.context = context;
        }

        public List<CallRegiModel> GetCaseDetailsforvendor(int UserID)
        {
            List<CallRegiModel> lst = new();

            lst = (from x in context.T17CallRegisters
                  join y in context.T13PoMasters on x.CaseNo equals y.CaseNo
                  join p in context.V06Consignees on y.PurchaserCd equals p.ConsigneeCd
                   join v in context.T05Vendors on y.VendCd equals v.VendCd
                   join cs in context.T21CallStatusCodes on x.CallStatus equals cs.CallStatusCd
                   join ie in context.T09Ies on x.IeCd equals ie.IeCd
                   where x.MfgCd == UserID
                   select new CallRegiModel
                   {
                       CaseNo = x.CaseNo,
                       CallDate = x.Datetime,
                       CallSNo = x.CallSno,
                       Purchaser = p.Consignee,
                       Vendor = v.VendName,
                       PurchaseOrderDate=y.PoDt,
                       PurchaseOrderNo = y.PoNo,
                       CallStatus = cs.CallStatusDesc,
                       Region = x.RegionCode,
                       PlaceofInspection = x.MfgPlace,
                       ContactPersonName = ie.IeName,
                       ManufacturerEmail = ie.IeEmail,
                       PhoneNumber = ie.IePhoneNo,
                   }).ToList();
            return lst;
        }

        public List<CallRegiModel> GetCaseDetailsforClient(string UserID, string Organisation, string OrgnType)
        {
            List<CallRegiModel> lst = new();
            string threeMonthsAgo = DateTime.Now.AddMonths(-3).ToString("dd-MM-yy");

            lst = (from x in context.T17CallRegisters
                   join y in context.T13PoMasters on x.CaseNo equals y.CaseNo
                   join p in context.V06Consignees on y.PurchaserCd equals p.ConsigneeCd
                   join v in context.T05Vendors on y.VendCd equals v.VendCd
                   join cs in context.T21CallStatusCodes on x.CallStatus equals cs.CallStatusCd
                   join ie in context.T09Ies on x.IeCd equals ie.IeCd
                   where y.RlyCd == Organisation && y.RlyNonrly == OrgnType && x.CallRecvDt >= Convert.ToDateTime(threeMonthsAgo)
                   select new CallRegiModel
                   {
                       CaseNo = x.CaseNo,
                       CallDate = x.Datetime,
                       CallSNo = x.CallSno,
                       Purchaser = p.Consignee,
                       Vendor = v.VendName,
                       PurchaseOrderDate = y.PoDt,
                       PurchaseOrderNo = y.PoNo,
                       CallStatus = cs.CallStatusDesc,
                       Region = x.RegionCode,
                       PlaceofInspection = x.MfgPlace,
                       ContactPersonName = ie.IeName,
                       ManufacturerEmail = ie.IeEmail,
                       PhoneNumber = ie.IePhoneNo,
                   }).ToList();
            return lst;
        }

        public VenderCallRegisterModel FindByAddDetails(string CaseNo, DateTime? CallRecvDt, string CallStage, int UserId)
        {
            VenderCallRegisterModel model = new();
            var T13 = context.T13PoMasters.Where(x => x.CaseNo == CaseNo).FirstOrDefault();

            var T14 = context.T17CallRegisterSearchViews.Where(x => x.CaseNo == CaseNo).FirstOrDefault();
            if (T14 != null)
            {
                model.OnlineCallStatus = T14.OnlineCallStatus;
            }

            if (T13 == null)
                throw new Exception("Record Not found");
            else
            {
                model.CaseNo = T13.CaseNo;
                model.VendCd = (Convert.ToString(T13.VendCd) == "" || Convert.ToString(T13.VendCd) == null) ? "0" : Convert.ToString(T13.VendCd) == Convert.ToString(UserId) ? "2" : "1";
                model.InspectingAgency = T13.InspectingAgency;
                model.Remarks = T13.Remarks;
                model.RlyNonrly = T13.RlyNonrly;
                model.PoOrLetter = T13.PoOrLetter;
                model.PendingCharges = Convert.ToInt32(T13.PendingCharges);
            }
            var count = context.T17CallRegisters.Where(call => call.CaseNo == CaseNo && call.CallStatus == "M" && call.FinalOrStage == CallStage).Select(call => call.CaseNo).Count();

            //int result = count != null ? count : 0;
            model.MaxCount = count;

            string dp = "";
            if (model.InspectingAgency == "R" || model.InspectingAgency == "U")
            {
                var maxExtDelvDt = context.T15PoDetails.Where(T15 => T15.CaseNo == CaseNo).Max(T15 => (DateTime?)T15.ExtDelvDt);

                string resultDt = maxExtDelvDt != null ? maxExtDelvDt.Value.ToString("dd/MM/yyyy") : "01/01/2001";
                string ext_delv_dt = "";

                ext_delv_dt = resultDt;
                if (ext_delv_dt == "01/01/2001")
                {
                    dp = "2";
                }
                else
                {
                    System.DateTime w_dt1 = new System.DateTime(Convert.ToInt32(ext_delv_dt.Substring(6, 4)), Convert.ToInt32(ext_delv_dt.Substring(3, 2)), Convert.ToInt32(ext_delv_dt.Substring(0, 2)));
                    System.DateTime w_dt2 = new System.DateTime(Convert.ToInt32(Convert.ToString(CallRecvDt).Substring(6, 4)), Convert.ToInt32(Convert.ToString(CallRecvDt).Substring(3, 2)), Convert.ToInt32(Convert.ToString(CallRecvDt).Substring(0, 2)));
                    TimeSpan ts = w_dt1 - w_dt2;
                    int differenceInDays = ts.Days;
                    if (differenceInDays < 5)
                    {
                        dp = "0";
                    }
                    else
                    {
                        dp = "1";
                    }
                }
            }


            model.dp = dp;

            return model;
        }

        public string GetMatch(string CaseNo, string UserName)
        {
            string test = "";
            var item = context.T13PoMasters.Where(x => x.CaseNo == CaseNo).FirstOrDefault();
            if (item == null)
            {
                test = "0";
            }
            else if (item.VendCd == Convert.ToInt32(UserName))
            {
                test = "2";
            }
            else
            {
                test = "1";
            }
            return test;
        }

        public List<VenderCallRegisterModel> GetVenderListM(RequestVenderCallRegisterModel model)
        {
            string CallRecvDt1 = model.CallRecvDt.ToString("dd-MM-yy");

            string CaseNo1 = "";
            string CallSno1 = "";
            if (!string.IsNullOrEmpty(model.CaseNo))
            {
                CaseNo1 = Convert.ToString(model.CaseNo);
            }

            if (!string.IsNullOrEmpty(model.CallSno))
            {
                CallSno1 = Convert.ToString(model.CallRecvDt);
            }

            CaseNo1 = CaseNo1.ToString() == "" ? string.Empty : CaseNo1.ToString();
            CallSno1 = CallSno1.ToString() == "" ? string.Empty : CallSno1.ToString();
            
            OracleParameter[] par = new OracleParameter[4];
            par[0] = new OracleParameter("p_CNO", OracleDbType.Varchar2, CaseNo1, ParameterDirection.Input);
            par[1] = new OracleParameter("p_DT", OracleDbType.Date, Convert.ToDateTime(CallRecvDt1), ParameterDirection.Input);
            par[2] = new OracleParameter("p_CSNO", OracleDbType.Int32, CallSno1, ParameterDirection.Input);

            par[3] = new OracleParameter("p_result_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("SP_GET_CALL_DETAILS", par, 1);
            DataTable dt = ds.Tables[0];
            List<VenderCallRegisterModel> models = new();
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                models = JsonConvert.DeserializeObject<List<VenderCallRegisterModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }
            
            return models;
        }

        public VenderCallRegisterModel GetValidate(VenderCallRegisterModel model)
        {
            if (model.ActionType == "A")
            {
                model.callval = FindIeCODE(model);
            }

            GetDtList(model);
            return model;
        }

        int FindIeCODE(VenderCallRegisterModel model)
        {
            string department1 = string.Empty;
            int strval = 0;
            int Clustercode;
            int vcode = 0;
            string region = model.RegionCode.ToString();

            if (region == "N")
            {
                department1 = model.DepartmentCode;
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
                department1 = model.DepartmentCode;
            }

            vcode = Convert.ToInt32(model.MfgCd);
            var distinctClusterCodes = (from a in context.T100VenderClusters
                                        join b in context.T99ClusterMasters
                                        on a.ClusterCode equals b.ClusterCode
                                        where a.VendorCode == vcode && a.DepartmentName == department1 && b.RegionCode == region
                                        select b.ClusterCode).Distinct().ToList();



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
                        model.IeCd = ieCode;
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

                        if (callStatusCount >= 0)
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
                                    model.IeCd = ieCode;
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
                                        if (callStatusCount1 >= 0)
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
                                                        model.IeCd = Alt_ieCode;
                                                    }
                                                    else
                                                    {
                                                        var altIeTwo = context.T09Ies.Where(ie => ie.IeCd == ieCode).Select(ie => ie.AltIeTwo).FirstOrDefault();
                                                        if (altIeTwo != null)
                                                        {
                                                            //    strval = 0;
                                                            //}
                                                            //else
                                                            //{
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
                                                            if (callStatusCount2 >= 0)
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
                                                                            model.IeCd = Alt_ieCode_TWO;
                                                                        }
                                                                        else
                                                                        {
                                                                            var altIeThree = context.T09Ies.Where(ie => ie.IeCd == ieCode).Select(ie => ie.AltIeThree).FirstOrDefault();
                                                                            if (altIeThree != null)
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
                                                                                if (callStatusCount3 >= 0)
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
                                                                                                model.IeCd = Alt_ieCode_THREE;
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

        int GetDtList(VenderCallRegisterModel model)
        {
            int err = 0;
            List<VenderCallRegisterModel>? query = null;

            var ItemSrnoPo = (from a in context.T18CallDetails
                              where a.CaseNo == model.CaseNo && a.CallRecvDt == Convert.ToDateTime(model.CallRecvDt) && a.CallSno == Convert.ToInt16(model.CallSno)
                              select a.ItemSrnoPo).FirstOrDefault();

            query = (from t18 in context.T18CallDetails
                     join t06 in context.T06Consignees on t18.ConsigneeCd equals t06.ConsigneeCd
                     join t03 in context.T03Cities on t06.ConsigneeCity equals t03.CityCd
                     where t18.CaseNo == model.CaseNo && t18.CallRecvDt == Convert.ToDateTime(model.CallRecvDt) && t18.CallSno == Convert.ToInt16(model.CallSno)
                     select new VenderCallRegisterModel
                     {
                         ItemSrnoPo = t18.ItemSrnoPo,
                         ItemDescPo = t18.ItemDescPo,
                         QtyOrdered = t18.QtyOrdered,
                         CumQtyPrevOffered = t18.CumQtyPrevOffered,
                         CumQtyPrevPassed = t18.CumQtyPrevPassed,
                         QtyToInsp = t18.QtyToInsp,
                         QtyPassed = t18.QtyPassed,
                         QtyRejected = t18.QtyRejected,
                         QtyDue = t18.QtyDue,
                         Consignee = t06.ConsigneeCd + "-" +
                                                  t06.ConsigneeDesig + "/" +
                                                  t06.ConsigneeDept + "/" +
                                                  t06.ConsigneeFirm + "/" +
                                                  t06.ConsigneeAdd1 + "/" +
                                                  t03.Location + " : " + t03.City,
                         CaseNo = t18.CaseNo,
                         CallRecvDt = t18.CallRecvDt,
                         CallSno = t18.CallSno
                     }).ToList();

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
                                 where detail.CaseNo == model.CaseNo && detail.ItemSrno == srno
                                 select new
                                 {
                                     detail.ConsigneeCd,
                                     detail.Qty,
                                     detail.Value,
                                     EXT_DELV_DATE = detail.ExtDelvDt.HasValue ? detail.ExtDelvDt.Value.ToString("dd/MM/yyyy") : "01-01-2001"
                                 };
                    long ccd = 0;
                    foreach (var record in query1)
                    {
                        ccd = (long)record.ConsigneeCd;
                        wMat_value += Convert.ToDecimal((record.Value / record.Qty) * qtyOffNow);
                        model.wMat_value = wMat_value;
                        ext_delv_dt = record.EXT_DELV_DATE;
                    }
                    var CallDetails = context.T18CallDetails.Where(x => x.CaseNo == model.CaseNo && x.CallRecvDt == Convert.ToDateTime(model.CallRecvDt) && x.CallSno == model.CallSno && x.ItemSrnoPo == srno).FirstOrDefault();

                    if (CallDetails == null)
                    {
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
                    }
                    else
                    {
                        CallDetails.QtyToInsp = Convert.ToDecimal(qtyOffNow);
                        context.SaveChanges();
                        err = Convert.ToInt32(CallDetails.ItemSrnoPo);
                    }

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
                                       where t102.RegionCode == Convert.ToString(model.RegionCode)
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
                        model.IeCd = contAltIeCode;
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
                OracleParameter[] par = new OracleParameter[4];
                par[0] = new OracleParameter("p_CNO", OracleDbType.Varchar2, model.CaseNo, ParameterDirection.Input);
                par[1] = new OracleParameter("p_DT", OracleDbType.Date, Convert.ToDateTime(model.CallRecvDt), ParameterDirection.Input);
                par[2] = new OracleParameter("p_CSNO", OracleDbType.Int32, model.CallSno, ParameterDirection.Input);

                par[3] = new OracleParameter("p_result_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

                var ds = DataAccessDB.GetDataSet("SP_GET_CALL_DETAILS", par, 1);
                DataTable dt = ds.Tables[0];

                List<VenderCallRegisterModel> list = new();
                if (ds != null && ds.Tables.Count > 0)
                {
                    string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                    list = JsonConvert.DeserializeObject<List<VenderCallRegisterModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                }
                var query = list.AsQueryable();

                var results = query;
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
            if (ext_delv_dt == "01-01-2001")
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

        public string RegiserCallSave(VenderCallRegisterModel model)
        {
            string IE_name = null;
            int ie_officer_code = 0;
            string automaticCallMarked = null;

            string ID = "";
            int CD = 0;
            if (model.ActionType == "A")
            {
                int cmdCL = context.T17CallRegisters.Where(x => x.CaseNo == model.CaseNo && x.CallRecvDt == model.CallRecvDt && x.RegionCode == model.RegionCode).Count();
                if (cmdCL == 0)
                {
                    var w_item_rdso = "";
                    var w_vend_rdso = "";
                    var w_stag = "";
                    var w_stage_or_final = "";

                    CD = Convert.ToInt32(model.CallSno);
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
                    if (model.IsNewVender == true)
                    {
                        w_New_Vendor = "Y";
                    }
                    model.callval = FindIeCODE(model);
                    model.IeCd = model.callval;
                    if (model.callval == 0)
                    {
                        //DisplayAlert("Master data not entered.So please enter master data cluster/vender/ie");
                    }
                    else
                    {
                        var ieInfo = context.T09Ies.Where(ie => ie.IeCd == model.callval).Select(ie => new { IeName = ie.IeName, IeCoCode = ie.IeCoCd }).FirstOrDefault();
                        if (ieInfo != null)
                        {
                            string ieName = ieInfo.IeName;
                            int ieOfficerCode = Convert.ToInt32(ieInfo.IeCoCode);
                            IE_name = ieName;
                            ie_officer_code = ieOfficerCode;
                            automaticCallMarked = "Y";
                        }
                        model.IE_name = ieInfo.IeName;
                    }

                    string w_irfc_funded = "";
                    if (model.RlyNonrly == "R")
                    {
                        w_irfc_funded = Convert.ToString(model.IrfcFunded);
                    }
                    else
                    {
                        w_irfc_funded = "N";
                    }
                    if (model.callval == 0)
                    {
                        T17CallRegister obj = new T17CallRegister();
                        obj.CaseNo = model.CaseNo;
                        obj.CallRecvDt = Convert.ToDateTime(model.CallRecvDt);
                        obj.CallSno = CD;
                        obj.CallLetterNo = model.CallLetterNo;
                        obj.CallLetterDt = model.CallLetterDt;
                        obj.CallMarkDt = model.CallMarkDt;
                        obj.DepartmentCode = model.DepartmentCode;
                        obj.DtInspDesire = model.DtInspDesire;
                        obj.CallStatus = "M";
                        obj.CallStatusDt = model.CallStatusDt;
                        obj.CallRemarkStatus = model.CallRemarkStatus;
                        obj.CallInstallNo = model.CallInstallNo;
                        obj.RegionCode = model.RegionCode;
                        obj.MfgCd = model.MfgCd;
                        obj.UserId = model.UserId;
                        obj.Datetime = DateTime.Now;
                        obj.MfgPlace = model.VendAdd1;
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
                        obj.Isfinalizedstatus = model.IsFinalizedStatus == true ? "F" : "N";

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
                        obj.CallSno = CD;
                        obj.CallLetterNo = model.CallLetterNo;
                        obj.CallLetterDt = model.CallLetterDt;
                        obj.CallMarkDt = model.CallMarkDt;
                        obj.IeCd = model.callval;
                        obj.CoCd = ie_officer_code;
                        obj.AutomaticCall = automaticCallMarked;
                        obj.DepartmentCode = model.DepartmentCode;
                        obj.DtInspDesire = model.DtInspDesire;
                        obj.CallStatus = "M";
                        obj.CallStatusDt = model.CallStatusDt;
                        obj.CallRemarkStatus = model.CallRemarkStatus;
                        obj.CallInstallNo = model.CallInstallNo;
                        obj.RegionCode = model.RegionCode;
                        obj.MfgCd = model.MfgCd;
                        obj.UserId = model.UserId;
                        obj.Datetime = DateTime.Now;
                        obj.MfgPlace = model.VendAdd1;
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
                        obj.Isfinalizedstatus = model.IsFinalizedStatus == true ? "F" : "N";

                        obj.Createdby = model.Createdby;
                        obj.Createddate = DateTime.Now;
                        context.T17CallRegisters.Add(obj);
                        context.SaveChanges();
                        ID = obj.CaseNo;
                    }
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
                    GetCall.CallSno = Convert.ToInt32(model.CallSno);
                    GetCall.DtInspDesire = model.DtInspDesire;
                    GetCall.CallStatusDt = model.CallStatusDt;
                    GetCall.CallRemarkStatus = model.CallRemarkStatus;
                    GetCall.DepartmentCode = model.DepartmentCode;
                    GetCall.CallInstallNo = model.CallInstallNo;
                    GetCall.NewVendor = model.IsNewVender == true ? "Y" : "X";
                    GetCall.Isfinalizedstatus = model.IsFinalizedStatus == true ? "F" : "N";

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

        public string UpdateCallDetails(VenderCallRegisterModel model, int ItemSrnoPo)
        {
            string ID = "";
            var Details = context.T18CallDetails.Where(x => x.CaseNo == model.CaseNo && x.CallRecvDt == Convert.ToDateTime(model.CallRecvDt) && x.CallSno == Convert.ToInt32(model.CallSno) && x.ItemSrnoPo == ItemSrnoPo).FirstOrDefault();
            if (Details == null)
            {
                T18CallDetail T18 = new T18CallDetail();
                T18.CaseNo = model.CaseNo;
                T18.CallRecvDt = Convert.ToDateTime(model.CallRecvDt);
                T18.CallSno = Convert.ToInt32(model.CallSno);
                T18.ItemSrnoPo = ItemSrnoPo;
                T18.ItemDescPo = model.ItemDescPo;
                T18.CumQtyPrevOffered = 0;
                T18.CumQtyPrevPassed = 0;
                T18.QtyOrdered = model.QtyOrdered;
                T18.QtyToInsp = model.QtyToInsp;
                T18.QtyPassed = 0;
                T18.QtyRejected = 0;
                T18.QtyDue = 0;
                T18.ConsigneeCd = model.ConsigneeCd;
                T18.UserId = model.UserId;
                context.T18CallDetails.Add(T18);
                context.SaveChanges();
                ID = Convert.ToString(T18.ItemSrnoPo);
            }
            else
            {
                Details.QtyToInsp = model.QtyToInsp;
                context.SaveChanges();
                ID = Convert.ToString(Details.ItemSrnoPo);
            }
            return ID;
        }

        public int DetailsInsertUpdate(RequestUpdateManufacturerDetailsModel model)
        {
            int ID = 0;
            using (var dbContext = context.Database.GetDbConnection())
            {
                OracleParameter[] par = new OracleParameter[4];
                par[0] = new OracleParameter("p_MfgCd", OracleDbType.Varchar2, model.MfgCd, ParameterDirection.Input);
                par[1] = new OracleParameter("p_VendContactPer1", OracleDbType.Varchar2, model.VendContactPer, ParameterDirection.Input);
                par[2] = new OracleParameter("p_VendContactTel1", OracleDbType.Varchar2, model.VendContactTel, ParameterDirection.Input);
                par[3] = new OracleParameter("p_VendEmail", OracleDbType.Varchar2, model.VendEmail, ParameterDirection.Input);
                var ds = DataAccessDB.ExecuteNonQuery("SP_UPDATE_VENDOR_INFO", par, 1);
                ID = model.MfgCd;
            }
            return ID;
        }
    }
}
