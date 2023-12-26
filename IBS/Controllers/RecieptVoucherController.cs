using IBS.Filters;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using OfficeOpenXml;

namespace IBS.Controllers
{
    public class RecieptVoucherController : BaseController
    {
        private readonly IRecieptVoucherRepository recieptVoucherRepository;
        public RecieptVoucherController(IRecieptVoucherRepository _recieptVoucherRepository)
        {
            recieptVoucherRepository = _recieptVoucherRepository;
        }

        [Authorization("RecieptVoucher", "Index", "view")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Manage(string id)
        {
            RecieptVoucherModel model = new() { Region = Region };
            if (!string.IsNullOrEmpty(id))
            {
                model = recieptVoucherRepository.FindByID(id);
                model.Region = Region;

                model.lstVoucherDetails.ForEach(i =>
                {
                    i.BANK_NAME = recieptVoucherRepository.GetBankName(i.BANK_CD);
                    i.ACC_NAME = recieptVoucherRepository.GetAccountName(i.ACC_CD ?? 0);
                    i.BPO_NAME = i.BPO_CD != null ? recieptVoucherRepository.GetBPOName(i.BPO_CD) : "";
                    i.IsNew = false;
                });

                SetLstVoucherDetailsModel = model.lstVoucherDetails;
            }
            else
            {
                model.VCHR_DT = DateTime.Now.Date;
                model.VCHR_NO = recieptVoucherRepository.GenerateVoucherNo(Region, DateTime.Now.Date);

                if (Region == "N") model.BANK_CD = 53;
                else if (Region == "W") model.BANK_CD = 88;
                else if (Region == "E") model.BANK_CD = 85;
                else if (Region == "S") model.BANK_CD = 87;

                SetLstVoucherDetailsModel = new List<VoucherDetailsModel>();
            }

            ViewBag.RoleCD = GetAuthType;
            ViewBag.Region = Region;

            return View(model);
        }

        [HttpPost]
        public IActionResult Manage(RecieptVoucherModel model)
        {
            if (model.IsNew)
            {
                model.Createdby = UserId;
                model.UserName = USER_ID.Length > 8 ? USER_ID.Substring(0, 8) : USER_ID;
                model.lstVoucherDetails = GetLstVoucherDetailsModel;
                model.Region = GetRegionCode;
                if (model.lstVoucherDetails != null && model.lstVoucherDetails.Count > 0)
                {
                    foreach (var item in model.lstVoucherDetails)
                    {
                        if (item.BANK_CD == 0)
                        {
                            AlertAlreadyExist("Case No./ BPO is must. (By Order)");
                            return View(model);
                        }
                    }
                }

                recieptVoucherRepository.SaveDetails(model);
                AlertAddSuccess();
            }
            else
            {
                model.Updatedby = UserId;
                model.UserName = USER_ID.Substring(0, 8);
                model.lstVoucherDetails = GetLstVoucherDetailsModel;

                if (model.lstVoucherDetails != null && model.lstVoucherDetails.Count > 0)
                {
                    foreach (var item in model.lstVoucherDetails)
                    {
                        if (item.BANK_CD == 0)
                        {
                            AlertAlreadyExist("Case No./ BPO is must. (By Order)");
                            return View(model);
                        }
                    }
                }

                recieptVoucherRepository.SaveDetails(model);
                AlertUpdateSuccess();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<RecieptVoucherModel> dTResult = recieptVoucherRepository.GetVoucherList(dtParameters);
            return Json(dTResult);
        }

        [HttpPost]
        public IActionResult SaveVoucherDetails(int id, string ChequeNo, DateTime ChequeDate, int Bank_Cd, decimal? Amount, string SampleNo, int? AccountCode, string CaseNo, string BPO_Cd, string BPOType, string Narration, bool IsAdd)
        {
            List<VoucherDetailsModel> lst = GetLstVoucherDetailsModel;
            VoucherDetailsModel obj = new VoucherDetailsModel();

            if (lst == null) lst = new List<VoucherDetailsModel>();

            if (IsAdd)
            {
                if (lst.Count == 0) obj.ID = 1;
                else obj.ID = lst.Max(x => x.ID) + 1;
                obj.CHQ_NO = ChequeNo;
                obj.CHQ_DT = ChequeDate;
                obj.BANK_CD = Bank_Cd;
                obj.BANK_NAME = recieptVoucherRepository.GetBankName(Bank_Cd);
                obj.AMOUNT = Amount;
                obj.SAMPLE_NO = SampleNo;
                obj.ACC_CD = AccountCode;
                obj.ACC_NAME = recieptVoucherRepository.GetAccountName(AccountCode ?? 0);
                obj.CASE_NO = CaseNo;
                obj.BPO_CD = BPO_Cd;
                obj.BPO_NAME = recieptVoucherRepository.GetBPOName(BPO_Cd);
                obj.BPO_TYPE = BPOType;
                obj.NARRATION = Narration;
                lst.Add(obj);
            }
            else
            {
                obj = lst.Find(a => a.ID == id);
                if (obj != null)
                {
                    obj.CHQ_NO = ChequeNo;
                    obj.CHQ_DT = ChequeDate;
                    obj.BANK_CD = Bank_Cd;
                    obj.BANK_NAME = recieptVoucherRepository.GetBankName(Bank_Cd);
                    obj.AMOUNT = Amount;
                    obj.SAMPLE_NO = SampleNo;
                    obj.ACC_CD = AccountCode;
                    obj.ACC_NAME = recieptVoucherRepository.GetAccountName(AccountCode ?? 0);
                    obj.CASE_NO = CaseNo;
                    obj.BPO_CD = BPO_Cd;
                    obj.BPO_NAME = recieptVoucherRepository.GetBPOName(BPO_Cd);
                    obj.BPO_TYPE = BPOType;
                    obj.NARRATION = Narration;
                }
            }

            SetLstVoucherDetailsModel = lst;

            if (IsAdd)
            {
                return Json(new { status = 1, responseText = "Records has been added successfully." });

            }
            else
            {
                return Json(new { status = 1, responseText = "Records has been updated successfully." });
            }
        }

        public IActionResult GetVoucherDetails()
        {
            List<VoucherDetailsModel> lst = GetLstVoucherDetailsModel;
            if (lst == null) lst = new List<VoucherDetailsModel>();
            return PartialView("_VoucherDetails", lst);
        }

        public IActionResult GetVoucherDetailsByID(int id)
        {
            List<VoucherDetailsModel> lst = GetLstVoucherDetailsModel;

            if (GetLstVoucherDetailsModel != null)
            {
                VoucherDetailsModel obj = GetLstVoucherDetailsModel.Where(x => x.ID == id).FirstOrDefault();

                if (obj != null) return Json(new { status = 1, responseText = obj });
            }
            return Json(new { status = 0, responseText = "Something went Wrong!!" });
        }

        public IActionResult DeleteVoucherDetailsByID(int id)
        {
            List<VoucherDetailsModel> lst = GetLstVoucherDetailsModel;

            VoucherDetailsModel obj = lst.Find(a => a.ID == id);
            if (obj != null)
            {
                lst.Remove(obj);
                SetLstVoucherDetailsModel = lst;
                return Json(new { status = 1, responseText = "Records has been deleted successfully." });
            }

            return Json(new { status = 0, responseText = "Something went Wrong!!" });
        }

        [HttpPost]
        public IActionResult UploadExcelFile(IFormFile file)
        {
            try
            {
                List<VoucherDetailsModel> lst = GetLstVoucherDetailsModel;

                if (lst == null) lst = new List<VoucherDetailsModel>();

                if (file != null && file.Length > 0)
                {
                    using (var stream = new MemoryStream())
                    {
                        file.CopyTo(stream);
                        using (var package = new ExcelPackage(stream))
                        {
                            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                            var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                            if (worksheet != null)
                            {
                                int rowCount = worksheet.Dimension.Rows;
                                int colCount = worksheet.Dimension.Columns;
                                int ID = 1;

                                for (int row = 2; row <= rowCount; row++) // Assuming the data starts from the 2nd row (skip header row)
                                {
                                    if (lst.Count > 0) ID = lst.Max(x => x.ID) + 1;

                                    VoucherDetailsModel data = new VoucherDetailsModel
                                    {
                                        ID = ID,
                                        CHQ_NO = worksheet.Cells[row, 1].Value != null ? worksheet.Cells[row, 1].Value.ToString() : "",
                                        CHQ_DT = worksheet.Cells[row, 1].Value != null ? DateTime.FromOADate(long.Parse(worksheet.Cells[row, 2].Value.ToString())) : DateTime.Now.Date,
                                        AMOUNT = worksheet.Cells[row, 1].Value != null ? Convert.ToDecimal(worksheet.Cells[row, 3].Value) : null
                                    };
                                    lst.Add(data);
                                }

                                SetLstVoucherDetailsModel = lst;

                                return Json(new { status = 1, responseText = "Records has been added successfully." });

                            }
                        }
                    }
                }
                return Json(new { status = 0, responseText = "No file or empty file." });
            }
            catch (Exception ex)
            {
                return Json(new { status = 0, responseText = "Something went Wrong!!" });
            }
        }

        public IActionResult GetBPO(int Acc_cd, string BpoType, string BPO_cd)
        {
            return Json(recieptVoucherRepository.GetBPO(Acc_cd, BpoType, BPO_cd).ToList());
        }

        public IActionResult FindBPODetails(string CaseNo)
        {
            BPODetailsModel obj = recieptVoucherRepository.FindBPODetails(CaseNo);

            if (obj != null) return Json(new { status = 1, responseText = obj });

            return Json(new { status = 0, responseText = "Invalid Case No.!!!" });
        }
    }
}
