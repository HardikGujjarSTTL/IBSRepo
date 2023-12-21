using IBSAPI.Helper;
using IBSAPI.Interfaces;
using IBSAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace IBSAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InspectionController : ControllerBase
    {
        #region Varible
        private readonly IInspectionRepository inspectionRepository;
        private readonly IWebHostEnvironment env;
        public IConfiguration Configuration { get; }
        #endregion        
        public InspectionController(IInspectionRepository _inspectionRepository, IWebHostEnvironment _environment, IConfiguration configuration)
        {
            inspectionRepository = _inspectionRepository;
            env = _environment;
            Configuration = configuration;
        }

        #region IE Methods
        [HttpGet("Get_IE_TodayInspection", Name = "GetTodayInspection")]
        public IActionResult GetTodayInspection(int IeCd)
        {
            try
            {
                var result = inspectionRepository.GetToDayInspection(IeCd);
                if (result.Count() > 0)
                {
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.SucessMessage,
                        message = "Data get successfully",
                        totalRecord = result.Count(),
                        data = result
                    };
                    return Ok(response);
                }
                else
                {
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                        message = "No Data Found",
                        totalRecord = result.Count(),
                        data = result
                    };
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Inspection_API", "GetTodayInspection", 1, string.Empty);
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                    message = ex.Message.ToString(),
                };
                return Ok(response);
            }
        }

        [HttpGet("Get_IE_TomorrowInspection", Name = "GetTomorrowInspection")]
        public IActionResult GetTomorrowInspection(int IeCd)
        {
            try
            {
                var result = inspectionRepository.GetTomorrowInspection(IeCd);
                if (result.Count() > 0)
                {
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.SucessMessage,
                        message = "Data get successfully",
                        totalRecord = result.Count(),
                        data = result
                    };
                    return Ok(response);
                }
                else
                {
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                        message = "No Data Found",
                        totalRecord = result.Count(),
                        data = result
                    };
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Inspection_API", "GetTomorrowInspection", 1, string.Empty);
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                    message = ex.Message.ToString(),
                };
                return Ok(response);
            }
        }

        [HttpGet("Get_Case_Detail_For_IE", Name = "GetCaseDetailForIE")]
        public IActionResult GetCaseDetailForIE(string Case_No, DateTime CallRecvDt, int CallSNo, int IeCd)
        {
            try
            {
                CaseDetailIEModel result = inspectionRepository.GetCaseDetailForIE(Case_No, CallRecvDt, CallSNo, IeCd);
                if (result != null)
                {
                    int DocumentCategoryID = (int)Enums.DocumentCategory.ICPHOTOS;
                    int ICPhotoDigSignDCID = (int)Enums.DocumentCategory.ICPhotoDigSign;
                    int UploadTestPlanDCID = (int)Enums.DocumentCategory.UploadTestPlan;
                    int UploadICAnnexue1DCID = (int)Enums.DocumentCategory.UploadICAnnexue1;
                    int UploadICAnnexue2DCID = (int)Enums.DocumentCategory.UploadICAnnexue2;
                    string IsStaging = Configuration["MyAppSettings:IsStaging"];
                    string RootHostName = HttpContext.Request.Host.Value;
                    //string WebRootPath = "https://192.168.0.101/IBS2";
                    string WebRootPath = "https://" + RootHostName + "/IBS2";
                    //if (Convert.ToBoolean(IsStaging) == true)
                    //{
                    //    //WebRootPath = env.WebRootPath.Replace("IBS2API", "IBS2");
                    //    WebRootPath = env.WebRootPath.Replace("IBSAPI", "IBS");
                    //}
                    //else
                    //{
                    //    WebRootPath = env.WebRootPath;
                    //}
                    result.photosModel = inspectionRepository.GetDocRecordsList(DocumentCategoryID, Case_No, WebRootPath).OrderBy(x => x.OtherDocumentName).ToList();
                    PhotosModel photosModels = new PhotosModel();
                    List<PhotosModel> photosModels1 = new List<PhotosModel>();
                    photosModels = inspectionRepository.GetDocRecordsList(ICPhotoDigSignDCID, Case_No, WebRootPath).OrderBy(x => x.OtherDocumentName).FirstOrDefault();
                    if (photosModels != null)
                    {
                        photosModels1.Add(photosModels);
                    }
                    photosModels = inspectionRepository.GetDocRecordsList(UploadTestPlanDCID, Case_No, WebRootPath).OrderBy(x => x.OtherDocumentName).FirstOrDefault();
                    if (photosModels != null)
                    {
                        photosModels1.Add(photosModels);
                    }
                    photosModels = inspectionRepository.GetDocRecordsList(UploadICAnnexue1DCID, Case_No, WebRootPath).OrderBy(x => x.OtherDocumentName).FirstOrDefault();
                    if (photosModels != null)
                    {
                        photosModels1.Add(photosModels);
                    }
                    photosModels = inspectionRepository.GetDocRecordsList(UploadICAnnexue2DCID, Case_No, WebRootPath).OrderBy(x => x.OtherDocumentName).FirstOrDefault();
                    if (photosModels != null)
                    {
                        photosModels1.Add(photosModels);
                    }
                    result.pdfModel = photosModels1;
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.SucessMessage,
                        message = "Data get successfully",
                        data = result,
                    };
                    return Ok(response);
                }
                else
                {
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                        message = "No Data Found"
                    };
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Inspection_API", "GetCaseDetailForIE", 1, string.Empty);
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                    message = ex.Message.ToString(),
                };
                return Ok(response);
            }
        }

        [HttpGet("Get_IE_PendingInspection", Name = "GetPendingInspection")]
        public IActionResult GetPendingInspection(int IeCd, string Region, DateTime Date)
        {
            try
            {
                var result = inspectionRepository.GetPendingInspection(IeCd, Region, Date);
                if (result.Count() > 0)
                {
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.SucessMessage,
                        message = "Data get successfully",
                        totalRecord = result.Count(),
                        data = result
                    };
                    return Ok(response);
                }
                else
                {
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                        message = "No Data Found",
                        totalRecord = result.Count(),
                        data = result
                    };
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Inspection_API", "GetPendingInspection", 1, string.Empty);
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                    message = ex.Message.ToString(),
                };
                return Ok(response);
            }
        }

        [HttpGet("Get_Date_Wise_Recent_Inspection", Name = "GetDateWiseRecentInspection")]
        public IActionResult GetDateWiseRecentInspection(int IeCd, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                var result = inspectionRepository.GetDateWiseRecentInspection(IeCd, FromDate, ToDate);
                if (result.Count() > 0)
                {
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.SucessMessage,
                        message = "Data get successfully",
                        totalRecord = result.Count(),
                        data = result
                    };
                    return Ok(response);
                }
                else
                {
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                        message = "No Data Found",
                        totalRecord = result.Count(),
                        data = result
                    };
                    return Ok(response);
                }

            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Inspection_API", "GetDateWiseRecentInspection", 1, string.Empty);
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                    message = ex.Message.ToString(),
                };
                return Ok(response);
            }
        }

        [HttpGet("Get_IE_CompleteInspection", Name = "GetCompleteInspection")]
        public IActionResult GetCompleteInspection(int IeCd)
        {
            try
            {
                var result = inspectionRepository.GetCompleteInspection(IeCd);
                if (result.Count() > 0)
                {
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.SucessMessage,
                        message = "Data get successfully",
                        totalRecord = result.Count(),
                        data = result
                    };
                    return Ok(response);
                }
                else
                {
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                        message = "No Data Found",
                        totalRecord = result.Count(),
                        data = result
                    };
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Inspection_API", "GetCompleteInspection", 1, string.Empty);
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                    message = ex.Message.ToString(),
                };
                return Ok(response);
            }
        }
        #endregion

        #region Vendor Methods
        [HttpGet("Get_Vendor_PendingInspection", Name = "Get_Vendor_PendingInspection")]
        public IActionResult Get_Vendor_PendingInspection(int Vend_Cd)
        {
            try
            {
                DateTime currentDate = DateTime.Now;
                DateTime FromDate = currentDate.AddMonths(-3);
                FromDate = new DateTime(FromDate.Year, FromDate.Month, 1);
                var result = inspectionRepository.Get_Vendor_PendingInspection(Vend_Cd, FromDate, currentDate);
                if (result.Count() > 0)
                {
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.SucessMessage,
                        message = "Data get successfully",
                        totalRecord = result.Count(),
                        data = result,
                    };
                    return Ok(response);
                }
                else
                {
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                        message = "No Data Found",
                        totalRecord = result.Count(),
                        data = result
                    };
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Inspection_API", "Get_Vendor_PendingInspection", 1, string.Empty);
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                    message = ex.Message.ToString(),
                };
                return Ok(response);
            }
        }

        [HttpGet("Get_Pending_PO_For_Call", Name = "Get_Pending_PO_For_Call")]
        public IActionResult Get_Pending_PO_For_Call(int Vend_Cd)
        {
            try
            {
                DateTime currentDate = DateTime.Now;
                DateTime FromDate = currentDate.AddMonths(-3);
                FromDate = new DateTime(FromDate.Year, FromDate.Month, 1);
                var result = inspectionRepository.Get_Pending_PO_For_Call(Vend_Cd, FromDate, currentDate);
                if (result.Count() > 0)
                {
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.SucessMessage,
                        message = "Data get successfully",
                        totalRecord = result.Count(),
                        data = result,
                    };
                    return Ok(response);
                }
                else
                {
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                        message = "No Data Found",
                        totalRecord = result.Count(),
                        data = result
                    };
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Inspection_API", "Get_Vendor_PendingInspection", 1, string.Empty);
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                    message = ex.Message.ToString(),
                };
                return Ok(response);
            }
        }
        #endregion

        #region CM Methods
        [HttpGet("Get_CM_RecentInspection", Name = "Get_CM_RecentInspection")]
        public IActionResult Get_CM_RecentInspection(int Co_Cd, int IE_CD, DateTime CurrDate)
        {
            try
            {
                //var CurrDate = DateTime.Now;
                var result = inspectionRepository.Get_CM_RecentInspection(Co_Cd, IE_CD, CurrDate);
                if (result.Count() > 0)
                {
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.SucessMessage,
                        message = "Data get successfully",
                        totalRecord = result.Count(),
                        data = result
                    };
                    return Ok(response);
                }
                else
                {
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                        message = "No Data Found",
                        totalRecord = result.Count(),
                        data = result
                    };
                    return Ok(response);

                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Inspection_API", "Get_CM_RecentInspection", 1, string.Empty);
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                    message = ex.Message.ToString(),
                };
                return Ok(response);
            }
        }
        #endregion

        #region Client Methods
        [HttpGet("Get_Client_PendingInspection", Name = "Get_Client_PendingInspection")]
        public IActionResult Get_Client_PendingInspection(string Rly_CD, string Rly_NonType)
        {
            try
            {
                DateTime ToDate = DateTime.Now;
                DateTime FromDate = ToDate.AddMonths(-3);
                FromDate = new DateTime(FromDate.Year, FromDate.Month, 1);
                var result = inspectionRepository.Get_Client_PendingInspection(Rly_CD, Rly_NonType, FromDate, ToDate);
                if (result.Count() > 0)
                {
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.SucessMessage,
                        message = "Data get successfully",
                        totalRecord = result.Count(),
                        data = result,
                    };
                    return Ok(response);
                }
                else
                {
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                        message = "No Data Found",
                        totalRecord = result.Count(),
                        data = result
                    };
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Inspection_API", "Get_Client_PendingInspection", 1, string.Empty);
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                    message = ex.Message.ToString(),
                };
                return Ok(response);
            }
        }

        [HttpGet("Get_Client_Region_Wise_PendingInspection", Name = "Get_Client_Region_Wise_PendingInspection")]
        public IActionResult Get_Client_Region_Wise_PendingInspection(string Rly_CD, string Rly_NonType, string? PO_NO, string Region = "")
        {
            try
            {
                DateTime ToDate = DateTime.Now;
                DateTime FromDate = ToDate.AddMonths(-3);
                FromDate = new DateTime(FromDate.Year, FromDate.Month, 1);
                var result = inspectionRepository.Get_Client_Region_Wise_PendingInspection(Rly_CD, Rly_NonType, PO_NO, Region, FromDate, ToDate);
                if (result.Count() > 0)
                {
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.SucessMessage,
                        message = "Data get successfully",
                        totalRecord = result.Count(),
                        data = result,
                    };
                    return Ok(response);
                }
                else
                {
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                        message = "No Data Found",
                        totalRecord = result.Count(),
                        data = result
                    };
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Inspection_API", "Get_Client_Region_Wise_PendingInspection", 1, string.Empty);
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                    message = ex.Message.ToString(),
                };
                return Ok(response);
            }
        }
        #endregion

        [HttpGet("GetBkNoAndSetNoByConsignee", Name = "GetBkNoAndSetNoByConsignee")]
        public IActionResult GetBkNoAndSetNoByConsignee(string CaseNo, DateTime? call_Recv_DT, int CallSno, int Consignee, int IE_CD)
        {
            try
            {
                BookNoSetNoModel model = inspectionRepository.GetBkNoAndSetNoByConsignee(CaseNo, call_Recv_DT, CallSno, Consignee, IE_CD);
                if (model != null)
                {
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.SucessMessage,
                        data = model,
                    };
                    return Ok(response);
                }
                else
                {
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                        message = "No Data Found"
                    };
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Inspection_API", "GetBkNoAndSetNoByConsignee", 1, string.Empty);
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                    message = ex.Message.ToString(),
                };
                return Ok(response);
            }
        }

    }
}
