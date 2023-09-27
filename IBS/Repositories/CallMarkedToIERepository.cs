using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using Newtonsoft.Json;
using NuGet.Protocol.Plugins;
using System.Data;

namespace IBS.Repositories
{
    public class CallMarkedToIERepository : ICallMarkedToIERepository
    {
        private readonly ModelContext context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CallMarkedToIERepository(ModelContext context)
        {
            this.context = context;
        }

        public DTResult<CallsMarkedToIEModel> GetDataList(DTParameters dtParameters, string GetRegionCode, string UserId, int GetIeCd)
        {

            DTResult<CallsMarkedToIEModel> dTResult = new() { draw = 0 };
            IQueryable<CallsMarkedToIEModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;
            string PType = Convert.ToString(dtParameters.AdditionalValues.ToArray().Where(x => x.Key == "PType").FirstOrDefault().Value);
            if (dtParameters.Order != null)
            {
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "Vendor";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "Vendor";
                orderAscendingDirection = true;
            }

            DateTime CallMarkDtMax = Convert.ToDateTime("21-06-2022");
            DateTime CallMarkDtMin = Convert.ToDateTime("21-07-2022");

            query = from l in context.CallsmarkedtoieViews
                    join r in context.T91Railways on l.RlyCd equals r.RlyCd
                    where l.RlyCd == r.RlyCd && l.RlyCd == "NR"
                    && l.CallMarkDt >= CallMarkDtMax && l.CallMarkDt <= CallMarkDtMin && l.IeCd == GetIeCd
                    //where (l.Isdeleted == 0 || l.Isdeleted == null)
                    orderby l.Vendor, l.CallMarkDt, l.DtInspDesire
                    select new CallsMarkedToIEModel
                    {
                        Vendor = l.Vendor,
                        NewVendor = l.NewVendor,
                        Consignee = l.Consignee,
                        ItemDescPo = l.ItemDescPo,
                        ExtDelvDt = l.ExtDelvDt,
                        DtInspDesire = l.DtInspDesire,
                        CallMarkDt = l.CallMarkDt,
                        //CallSno = l.CallSno,
                        callDocAny = "/RBS/Vendor/CALLS_DOCUMENTS/" + l.CaseNo + "-" + l.CallRecvDt.ToString().Substring(6, 4) + l.CallRecvDt.ToString().Substring(3, 2) + l.CallRecvDt.ToString().Substring(0, 2) + l.CallSno + ".PDF",
                        PoSource = l.PoSource.Equals("C") ? "https://www.ireps.gov.in/ireps/etender/pdfdocs/MMIS/PO/" + l.PoYr + "/" + r.ImmsRlyCd + "/" + l.PoNo + ".pdf" : "/RBS/CASE_NO/" + l.CaseNo + ".PDF",
                        CallStatus = l.CallStatus,
                        Remarks = l.Remarks,
                        PoNo = l.PoNo,
                        PoDt = l.PoDt,
                        MfgPers = l.MfgPers,
                        MfgPhone = l.MfgPhone,
                        UserId = l.UserId == UserId ? "" : l.UserId,
                        Datetime = l.Datetime,
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.Vendor).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public string GetFilePath(string fileName)
        {
            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "RBS/Vendor/CALLS_DOCUMENTS", fileName);

            if (System.IO.File.Exists(filePath))
            {
                return filePath;
            }
            else
            {
                return null;
            }
        }
        public CallsMarkedToIEModel GetReport(int GetIeCd, string UserId, string type)
        {
            CallsMarkedToIEModel model = new();
            List<CallsMarkedToIEModelList> ReportLst = new();

            DateTime CallRecvDt = Convert.ToDateTime("27-02-2012");

            DateTime currentDate = DateTime.Now;
            DateTime startDate = currentDate.AddMonths(-1);

            DateTime dtFr = startDate;
            DateTime dtTo = currentDate;

            if (type == "C")
            {
                ReportLst = (from l in context.CallsmarkedtoieViewNews
                             join r in context.T91Railways on l.RlyCd equals r.RlyCd
                             where ((l.CallMarkDt >= dtFr && l.CallMarkDt <= dtTo && !(l.CallStatus == "B" || l.CallStatus == "C")) || (l.CallStatus == "C" && l.CallRecvDt > CallRecvDt && l.DocsSubmitted == "X"))
                             && l.IeCd == GetIeCd
                             orderby l.CallMarkDt, l.CallSno, l.Vendor
                             select new CallsMarkedToIEModelList
                             {
                                 Vendor = l.Vendor,
                                 NewVendor = l.NewVendor,
                                 Consignee = l.Consignee,
                                 ItemDescPo = l.ItemDescPo,
                                 ExtDelvDt = l.ExtDelvDt,
                                 DtInspDesire = l.InspDesireDt,
                                 CallMarkDt = l.CallMarkDt,
                                 CallSno = l.CallSno,
                                 callDocAny = "/RBS/Vendor/CALLS_DOCUMENTS/" + l.CaseNo + "-" + l.CallRecvDt.ToString().Substring(6, 4) + l.CallRecvDt.ToString().Substring(3, 2) + l.CallRecvDt.ToString().Substring(0, 2) + l.CallSno + ".PDF",
                                 //callDocAny = GetFilePath("/RBS/Vendor/CALLS_DOCUMENTS/" + l.CaseNo + "-" + l.CallRecvDt.ToString().Substring(6, 4) + l.CallRecvDt.ToString().Substring(3, 2) + l.CallRecvDt.ToString().Substring(0, 2) + l.CallSno + ".PDF"),
                                 PoSource = l.PoSource.Equals("C") ? "https://www.ireps.gov.in/ireps/etender/pdfdocs/MMIS/PO/" + l.PoYr + "/" + r.ImmsRlyCd + "/" + l.PoNo + ".pdf" : "/RBS/CASE_NO/" + l.CaseNo + ".PDF",
                                 CallStatus = l.CallStatus,
                                 Remarks = l.Remarks,
                                 PoNo = l.PoNo,
                                 PoDt = l.PoDt,
                                 MfgPers = l.MfgPers,
                                 MfgPhone = l.MfgPhone,
                                 UserId = l.UserId == UserId ? "" : l.UserId,
                                 Datetime = l.Datetime,
                                 CaseNo = l.CaseNo,
                                 CallRecvDt = l.CallRecvDt,
                                 VendCd = l.VendCd,
                                 MfgCd = l.MfgCd,
                                 Manufacturer = l.Manufacturer,
                                 Source = l.Source,
                                 CallStatusFull = l.CallStatusFull,
                                 IeCd = l.IeCd,
                                 cnt = l.Count,
                             }).ToList();
            }
            else if (type == "V")
            {
                ReportLst = (from l in context.CallsmarkedtoieViewNews
                             join r in context.T91Railways on l.RlyCd equals r.RlyCd
                             where ((l.CallMarkDt >= dtFr && l.CallMarkDt <= dtTo && !(l.CallStatus == "B" || l.CallStatus == "C")) || (l.CallStatus == "C" && l.CallRecvDt > CallRecvDt && l.DocsSubmitted == "X"))
                             && l.IeCd == GetIeCd
                             orderby l.Vendor, l.CallMarkDt, l.CallSno
                             select new CallsMarkedToIEModelList
                             {
                                 Vendor = l.Vendor,
                                 NewVendor = l.NewVendor,
                                 Consignee = l.Consignee,
                                 ItemDescPo = l.ItemDescPo,
                                 ExtDelvDt = l.ExtDelvDt,
                                 DtInspDesire = l.InspDesireDt,
                                 CallMarkDt = l.CallMarkDt,
                                 CallSno = l.CallSno,
                                 callDocAny = "/RBS/Vendor/CALLS_DOCUMENTS/" + l.CaseNo + "-" + l.CallRecvDt.ToString().Substring(6, 4) + l.CallRecvDt.ToString().Substring(3, 2) + l.CallRecvDt.ToString().Substring(0, 2) + l.CallSno + ".PDF",
                                 PoSource = l.PoSource.Equals("C") ? "https://www.ireps.gov.in/ireps/etender/pdfdocs/MMIS/PO/" + l.PoYr + "/" + r.ImmsRlyCd + "/" + l.PoNo + ".pdf" : "/RBS/CASE_NO/" + l.CaseNo + ".PDF",
                                 CallStatus = l.CallStatus,
                                 Remarks = l.Remarks,
                                 PoNo = l.PoNo,
                                 PoDt = l.PoDt,
                                 MfgPers = l.MfgPers,
                                 MfgPhone = l.MfgPhone,
                                 UserId = l.UserId == UserId ? "" : l.UserId,
                                 Datetime = l.Datetime,
                                 CaseNo = l.CaseNo,
                                 VendCd = l.VendCd,
                                 MfgCd = l.MfgCd,
                                 Manufacturer = l.Manufacturer,
                                 Source = l.Source,
                                 CallStatusFull = l.CallStatusFull,
                                 IeCd = l.IeCd,
                                 cnt = l.Count,
                             }).ToList();
            }
            else
            {
                ReportLst = (from l in context.CallsmarkedtoieViewNews
                             join r in context.T91Railways on l.RlyCd equals r.RlyCd
                             where ((l.CallMarkDt >= dtFr && l.CallMarkDt <= dtTo && !(l.CallStatus == "B" || l.CallStatus == "C")) || (l.CallStatus == "C" && l.CallRecvDt > CallRecvDt && l.DocsSubmitted == "X"))
                             && l.IeCd == GetIeCd
                             orderby l.InspDesireDt, l.CallMarkDt, l.CallSno, l.Vendor
                             select new CallsMarkedToIEModelList
                             {
                                 Vendor = l.Vendor,
                                 NewVendor = l.NewVendor,
                                 Consignee = l.Consignee,
                                 ItemDescPo = l.ItemDescPo,
                                 ExtDelvDt = l.ExtDelvDt,
                                 DtInspDesire = l.InspDesireDt,
                                 CallMarkDt = l.CallMarkDt,
                                 CallSno = l.CallSno,
                                 callDocAny = "/RBS/Vendor/CALLS_DOCUMENTS/" + l.CaseNo + "-" + l.CallRecvDt.ToString().Substring(6, 4) + l.CallRecvDt.ToString().Substring(3, 2) + l.CallRecvDt.ToString().Substring(0, 2) + l.CallSno + ".PDF",
                                 PoSource = l.PoSource.Equals("C") ? "https://www.ireps.gov.in/ireps/etender/pdfdocs/MMIS/PO/" + l.PoYr + "/" + r.ImmsRlyCd + "/" + l.PoNo + ".pdf" : "/RBS/CASE_NO/" + l.CaseNo + ".PDF",
                                 CallStatus = l.CallStatus,
                                 Remarks = l.Remarks,
                                 PoNo = l.PoNo,
                                 PoDt = l.PoDt,
                                 MfgPers = l.MfgPers,
                                 MfgPhone = l.MfgPhone,
                                 UserId = l.UserId == UserId ? "" : l.UserId,
                                 Datetime = l.Datetime,
                                 CaseNo = l.CaseNo,
                                 VendCd = l.VendCd,
                                 MfgCd = l.MfgCd,
                                 Manufacturer = l.Manufacturer,
                                 Source = l.Source,
                                 CallStatusFull = l.CallStatusFull,
                                 IeCd = l.IeCd,
                                 cnt = l.Count,
                             }).ToList();
            }

            if (ReportLst != null)
            {
                string serializeddt = JsonConvert.SerializeObject(ReportLst, Formatting.Indented);
                ReportLst = JsonConvert.DeserializeObject<List<CallsMarkedToIEModelList>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                ReportLst.ToList().ForEach(i =>
                {
                    i.Vendor = Convert.ToString(i.Vendor);
                    i.NewVendor = Convert.ToString(i.NewVendor);
                    i.Consignee = Convert.ToString(i.Consignee);
                    i.ItemDescPo = Convert.ToString(i.ItemDescPo);
                    i.ExtDelvDt = Convert.ToDateTime(i.ExtDelvDt);
                    i.DtInspDesire = Convert.ToDateTime(i.DtInspDesire);
                    i.CallMarkDt = Convert.ToDateTime(i.CallMarkDt);
                    i.CallSno = Convert.ToInt16(i.CallSno);
                    i.callDocAny = Convert.ToString(i.callDocAny);
                    i.PoSource = Convert.ToString(i.PoSource);
                    i.CallStatus = Convert.ToString(i.CallStatus);
                    i.Remarks = Convert.ToString(i.Remarks);
                    i.PoNo = Convert.ToString(i.PoNo);
                    i.PoDt = Convert.ToDateTime(i.PoDt);
                    i.MfgPers = Convert.ToString(i.MfgPers);
                    i.MfgPhone = Convert.ToString(i.MfgPhone);
                    i.UserId = Convert.ToString(i.UserId);
                    i.Datetime = Convert.ToDateTime(i.Datetime);
                    i.CaseNo = Convert.ToString(i.CaseNo);
                    i.cnt = Convert.ToDecimal(i.cnt);
                });

                model.ReportLst = ReportLst;
            }
            return model;
        }
    }
}
