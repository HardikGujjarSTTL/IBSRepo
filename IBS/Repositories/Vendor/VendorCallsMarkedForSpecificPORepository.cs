using IBS.DataAccess;
using IBS.Interfaces.Vendor;
using IBS.Models;
using System.Diagnostics.Metrics;
using System.Drawing;

namespace IBS.Repositories.Vendor
{
    public class VendorCallsMarkedForSpecificPORepository : IVendorCallsMarkedForSpecificPORepository
    {
        private readonly ModelContext context;

        public VendorCallsMarkedForSpecificPORepository(ModelContext context)
        {
            this.context = context;
        }

        public DTResult<VendorCallsMarkedForSpecificPOModel> GetDataList(DTParameters dtParameters, string UserName)
        {
            DTResult<VendorCallsMarkedForSpecificPOModel> dTResult = new() { draw = 0 };
            IQueryable<VendorCallsMarkedForSpecificPOModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "PoNo";
                }
                //orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
                orderAscendingDirection = true;
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "PoNo";
                orderAscendingDirection = true;
            }
            string RlyNorly = "", RlyCd = "", PoDt = "";
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["RlyNorly"]))
            {
                RlyNorly = Convert.ToString(dtParameters.AdditionalValues["RlyNorly"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["RlyCd"]))
            {
                RlyCd = Convert.ToString(dtParameters.AdditionalValues["RlyCd"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["PoDt"]))
            {
                PoDt = Convert.ToString(dtParameters.AdditionalValues["PoDt"]);
            }

            RlyNorly = RlyNorly.ToString() == "" ? string.Empty : RlyNorly.ToString();
            RlyCd = RlyCd.ToString() == "" ? string.Empty : RlyCd.ToString();
            DateTime? _PoDt = PoDt == "" ? null : DateTime.ParseExact(PoDt, "dd/MM/yyyy", null);

            query = from l in context.ViewVendorCallsMarkedForSpecificPos
                    where l.RlyNonrly == RlyNorly && l.RlyCd == RlyCd && l.PoDt == _PoDt && l.VendCd == Convert.ToInt32(UserName)
                    select new VendorCallsMarkedForSpecificPOModel
                    {
                        PoNo = l.PoNo,
                        L5NoPo = l.L5noPo,
                        PoDt = l.PoDt,
                        RlyNorly = l.RlyNonrly,
                        RlyCd = l.RlyCd
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.PoNo).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public DTResult<VendorCallsForSpecificPOReportModel> GetDataReportCallList(DTParameters dtParameters, string UserName)
        {
            DTResult<VendorCallsForSpecificPOReportModel> dTResult = new() { draw = 0 };
            IQueryable<VendorCallsForSpecificPOReportModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "PoNo";
                }
                //orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
                orderAscendingDirection = true;
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "PoNo";
                orderAscendingDirection = true;
            }
            string L5NoPo = "", RlyNorly = "", RlyCd = "", PoDt = "";
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["L5NoPo"]))
            {
                L5NoPo = Convert.ToString(dtParameters.AdditionalValues["L5NoPo"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["RlyNorly"]))
            {
                RlyNorly = Convert.ToString(dtParameters.AdditionalValues["RlyNorly"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["RlyCd"]))
            {
                RlyCd = Convert.ToString(dtParameters.AdditionalValues["RlyCd"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["PoDt"]))
            {
                PoDt = Convert.ToString(dtParameters.AdditionalValues["PoDt"]);
            }

            L5NoPo = L5NoPo.ToString() == "" ? string.Empty : L5NoPo.ToString();
            RlyNorly = RlyNorly.ToString() == "" ? string.Empty : RlyNorly.ToString();
            RlyCd = RlyCd.ToString() == "" ? string.Empty : RlyCd.ToString();
            DateTime? _PoDt = PoDt == "" ? null : DateTime.ParseExact(PoDt, "dd-MM-yyyy", null);

            string basePath_IC = Path.Combine(Directory.GetCurrentDirectory(), "RBS", "BILL_IC");
            string basePath_PLAN = Path.Combine(Directory.GetCurrentDirectory(), "RBS", "TESTPLAN");


            var FilePath = context.ViewCalldetailsforspecificpoReports.Where(x=>x.L5noPo == L5NoPo && x.RlyNonrly == RlyNorly && x.RlyCd == RlyCd && x.PoDt == _PoDt && x.VendCd == Convert.ToInt32(UserName)).FirstOrDefault();

            string jpgPath = Path.Combine(basePath_IC, $"{FilePath.IcPhoto}.JPG");
            string pdfPath = Path.Combine(basePath_IC, $"{FilePath.IcPhoto}.PDF");

            string fpath3_jpg = Path.Combine(basePath_PLAN, $"{FilePath.IcPhoto}.JPG");
            string fpath3_pdf = Path.Combine(basePath_PLAN, $"{FilePath.IcPhoto}.PDF");

            string fpath1_jpg = Path.Combine(basePath_IC, $"{FilePath.IcPhotoA1}.JPG");
            string fpath1_pdf = Path.Combine(basePath_IC, $"{FilePath.IcPhotoA1}.PDF");
            string fpath2_jpg = Path.Combine(basePath_IC, $"{FilePath.IcPhotoA2}.JPG");
            string fpath2_pdf = Path.Combine(basePath_IC, $"{FilePath.IcPhotoA2}.PDF");

            query = from l in context.ViewCalldetailsforspecificpoReports
                    where l.L5noPo == L5NoPo && l.RlyNonrly == RlyNorly && l.RlyCd == RlyCd && l.PoDt == _PoDt && l.VendCd == Convert.ToInt32(UserName)
                    select new VendorCallsForSpecificPOReportModel
                    {
                        Vendor = l.VendCd == l.MfgCd ? l.Vendor : (l.Vendor + l.Manufacturer),
                        Manufacturer = l.Manufacturer,
                        VendCd = l.VendCd,
                        MfgCd = l.MfgCd,
                        Consignee = l.Consignee,
                        ItemDescPo = l.Count > 1 ? l.ItemDescPo + "<span style=\"color:red\">Qty.</span>\r\n" + l.QtyToInsp + "and more items as per call" : l.ItemDescPo + "Qty." + l.QtyToInsp,
                        QtyToInsp = l.QtyToInsp,
                        CallMarkDt = l.CallMarkDt,
                        IeName = l.IeName,
                        IePhoneNo = l.IePhoneNo,
                        PoNo = l.PoNo,
                        PoDt = l.PoDt,
                        CaseNo = l.CaseNo,
                        Remark = l.Remark,
                        CallStatus = l.CallStatus,
                        Colour = l.Colour,
                        MfgPers = l.MfgPers,
                        MfgPhone = l.MfgPhone,
                        CallSno = l.CallSno,
                        Hologram = l.Hologram,

                        IcPhoto = jpgPath == null && pdfPath == null ? "" : jpgPath != null ? "/RBS/BILL_IC/" + l.IcPhoto + ".JPG" : pdfPath != null ? "/RBS/BILL_IC/" + l.IcPhoto + ".PDF" 
                        : fpath3_jpg != null ? "/RBS/TESTPLAN/" + l.IcPhoto + ".JPG" : fpath3_pdf != null ? "/RBS/TESTPLAN/" + l.IcPhoto + ".PDF" 
                        : fpath1_jpg != null ? "/RBS/BILL_IC/" + l.IcPhotoA1 + ".JPG" : fpath1_pdf != null ? "/RBS/BILL_IC/" + l.IcPhotoA1 + ".PDF"
                        : fpath2_jpg != null ? "/RBS/BILL_IC/" + l.IcPhotoA2 + ".JPG" : fpath2_pdf != null ? "/RBS/BILL_IC/" + l.IcPhotoA2 + ".PDF" : "",

                        //IcPhoto = "/RBS/BILL_IC/" + l.IcPhoto + ".JPG",
                        //IcPhotoA1 = "/RBS/BILL_IC/" + l.IcPhotoA1 + ".JPG" + "IC Annex 1",
                        //IcPhotoA2 = "/RBS/BILL_IC/" +l.IcPhotoA2 + ".JPG" + "IC Annex 2",
                        Count = l.Count,
                        CoName = l.CoName,
                        L5NoPo = l.L5noPo,
                        RlyNorly = l.RlyNonrly,
                        RlyCd = l.RlyCd,
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.PoNo).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public DTResult<VendorCallsMarkedForSpecificICModel> GetDataReportICList(DTParameters dtParameters, string UserName)
        {
            DTResult<VendorCallsMarkedForSpecificICModel> dTResult = new() { draw = 0 };
            IQueryable<VendorCallsMarkedForSpecificICModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "PoNo";
                }
                //orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
                orderAscendingDirection = true;
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "PoNo";
                orderAscendingDirection = true;
            }
            string L5NoPo = "", RlyNorly = "", RlyCd = "", PoDt = "";
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["L5NoPo"]))
            {
                L5NoPo = Convert.ToString(dtParameters.AdditionalValues["L5NoPo"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["RlyNorly"]))
            {
                RlyNorly = Convert.ToString(dtParameters.AdditionalValues["RlyNorly"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["RlyCd"]))
            {
                RlyCd = Convert.ToString(dtParameters.AdditionalValues["RlyCd"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["PoDt"]))
            {
                PoDt = Convert.ToString(dtParameters.AdditionalValues["PoDt"]);
            }

            L5NoPo = L5NoPo.ToString() == "" ? string.Empty : L5NoPo.ToString();
            RlyNorly = RlyNorly.ToString() == "" ? string.Empty : RlyNorly.ToString();
            RlyCd = RlyCd.ToString() == "" ? string.Empty : RlyCd.ToString();
            DateTime? _PoDt = PoDt == "" ? null : DateTime.ParseExact(PoDt, "dd-MM-yyyy", null);

            string basePath_IC = Path.Combine(Directory.GetCurrentDirectory(), "RBS", "BILL_IC");
            string basePath_PLAN = Path.Combine(Directory.GetCurrentDirectory(), "RBS", "TESTPLAN");


            var FilePath = context.ViewCalldetailsforspecificpoReports.Where(x => x.L5noPo == L5NoPo && x.RlyNonrly == RlyNorly && x.RlyCd == RlyCd && x.PoDt == _PoDt && x.VendCd == Convert.ToInt32(UserName)).FirstOrDefault();

            string fpath = Path.Combine(basePath_IC, $"{FilePath.IcPhoto}");

            string fpath1 = Path.Combine(basePath_IC, $"{FilePath.IcPhotoA1}");
            string fpath2 = Path.Combine(basePath_IC, $"{FilePath.IcPhotoA2}");
            string fpath3 = Path.Combine(basePath_PLAN, $"{FilePath.IcPhoto}");

            query = from l in context.ViewVendorCallsMarkedForSpecificIcs
                    where l.L5noPo == L5NoPo && l.RlyNonrly == RlyNorly && l.RlyCd == RlyCd && l.PoDt == _PoDt && l.VendCd == Convert.ToInt32(UserName)
                    select new VendorCallsMarkedForSpecificICModel
                    {
                        Purchaser = l.Purchaser,
                        PoNo = l.PoNo,
                        PoDt = l.PoDt,
                        IcNo = l.IcNo,
                        IcDt = l.IcDt,
                        BkNo = l.BkNo,
                        SetNo = l.SetNo,
                        BillNo = l.BillNo,
                        IeName = l.IeName,
                        Vendor = l.Vendor,
                        ItemDescPo = l.ItemDescPo,
                        QtyToInsp = l.QtyToInsp,
                        QtyPassed = l.QtyPassed,
                        QtyRejected = l.QtyRejected,
                        QtyPassReject = l.QtyPassed + "/" + l.QtyRejected,
                        Hologram = l.Hologram,

                        //IcPhoto = fpath == null ? "" : fpath != null ? "/RBS/BILL_IC/" + l.IcPhoto : fpath3 != null ? "/RBS/TESTPLAN/" + l.IcPhoto 
                        //: fpath1 != null ? "/RBS/BILL_IC/" + l.IcPhotoA1 : fpath2 != null ? "/RBS/BILL_IC/" + l.IcPhotoA2 : "",

                        IcPhoto = l.IcPhoto,
                        IcPhotoA1 = l.IcPhotoA1,
                        IcPhotoA2 = l.IcPhotoA2,
                        L5noPo = l.L5noPo,
                        RlyNonrly = l.RlyNonrly,
                        RlyCd = l.RlyCd,
                        VendCd = l.VendCd,
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.PoNo).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public DTResult<VendorCallsMarkedForSpecificICSubModel> GetDataReportICSubList(DTParameters dtParameters, string UserName)
        {
            DTResult<VendorCallsMarkedForSpecificICSubModel> dTResult = new() { draw = 0 };
            IQueryable<VendorCallsMarkedForSpecificICSubModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "" || orderCriteria == null)
                {
                    orderCriteria = "PoNo";
                }
                //orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
                orderAscendingDirection = true;
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "PoNo";
                orderAscendingDirection = true;
            }
            string L5NoPo = "", RlyNorly = "", RlyCd = "", PoDt = "";
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["L5NoPo"]))
            {
                L5NoPo = Convert.ToString(dtParameters.AdditionalValues["L5NoPo"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["RlyNorly"]))
            {
                RlyNorly = Convert.ToString(dtParameters.AdditionalValues["RlyNorly"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["RlyCd"]))
            {
                RlyCd = Convert.ToString(dtParameters.AdditionalValues["RlyCd"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["PoDt"]))
            {
                PoDt = Convert.ToString(dtParameters.AdditionalValues["PoDt"]);
            }

            L5NoPo = L5NoPo.ToString() == "" ? string.Empty : L5NoPo.ToString();
            RlyNorly = RlyNorly.ToString() == "" ? string.Empty : RlyNorly.ToString();
            RlyCd = RlyCd.ToString() == "" ? string.Empty : RlyCd.ToString();
            DateTime? _PoDt = PoDt == "" ? null : DateTime.ParseExact(PoDt, "dd-MM-yyyy", null);

            query = from l in context.ViewVendorCallsMarkedForSpecificIcSubs
                    where l.L5noPo == L5NoPo && l.RlyNonrly == RlyNorly && l.RlyCd == RlyCd && l.PoDt == _PoDt && l.VendCd == Convert.ToInt32(UserName)
                    select new VendorCallsMarkedForSpecificICSubModel
                    {
                        Vendor = l.Vendor,
                        Manufacturer = l.Manufacturer,
                        VendCd = l.VendCd,
                        MfgCd = l.MfgCd,
                        Consignee = l.Consignee,
                        ItemDescPo = l.ItemDescPo,
                        QtyToInsp = l.QtyToInsp,
                        CallMarkDt = l.CallMarkDt,
                        IeName = l.IeName,
                        IePhoneNo = l.IePhoneNo,
                        PoNo = l.PoNo,
                        PoDt = l.PoDt,
                        CaseNo = l.CaseNo,
                        Remark = l.Remark,
                        CallStatus = l.CallStatus,
                        Colour = l.Colour,
                        MfgPers = l.MfgPers,
                        MfgPhone = l.MfgPhone,
                        CallSno = l.CallSno,
                        Hologram = l.Hologram,
                        IcPhoto = l.IcPhoto,
                        IcPhotoA1 = l.IcPhotoA1,
                        IcPhotoA2 = l.IcPhotoA2,
                        Count = l.Count,
                        CoName = l.CoName,
                        L5noPo = l.L5noPo,
                        RlyNonrly = l.RlyNonrly,
                        RlyCd = l.RlyCd,
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.PoNo).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

    }
}
