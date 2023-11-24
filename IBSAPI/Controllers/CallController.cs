using IBSAPI.Helper;
using IBSAPI.Helpers;
using IBSAPI.Interfaces;
using IBSAPI.Models;
using IBSAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.Configuration;
using System.IO;

namespace IBSAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CallController : ControllerBase
    {
        #region Variable
        private readonly ICallRepository callRepository;
        private readonly IInspectionRepository inspectionRepository;
        private readonly IWebHostEnvironment env;
        //private readonly IDocument iDocument;
        public IConfiguration Configuration { get; }
        #endregion

        public CallController(ICallRepository _callRepository, IWebHostEnvironment _environment,  IConfiguration configuration, IInspectionRepository _inspectionRepository)
        {
            callRepository = _callRepository;
            env = _environment;
            //iDocument = _iDocumentRepository;
            Configuration = configuration;
            inspectionRepository = _inspectionRepository;
        }

        [HttpGet("GetCallList", Name = "GetCallList")]
        public IActionResult GetCallList()
        {
            try
            {
                List<CallListModel> callList = callRepository.GetCallList();
                if (callList.Count() > 0)
                {
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.SucessMessage,
                        message = "Data get successfully",
                        data = callList
                    };
                    return Ok(response);
                }
                else
                {
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                        message = "No Data Found",
                    };
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Call_API", "GetCallList", 1, string.Empty);
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                    message = ex.Message.ToString(),
                };
                return Ok(response);
            }
        }

        [HttpPost("SheduleInspection", Name = "SheduleInspection")]
        public IActionResult SheduleInspection([FromBody] SheduleInspectionRequestModel sheduleInspectionRequestModel)
        {
            try
            {
                int id = callRepository.SheduleInspection(sheduleInspectionRequestModel);
                if (id > 0)
                {
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.SucessMessage,
                        message = "Successfully"
                    };
                    return Ok(response);
                }
                else
                {
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                        message = "No Data Found",
                    };
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Call_API", "SheduleInspection", 1, string.Empty);
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                    message = ex.Message.ToString(),
                };
                return Ok(response);
            }
        }
        [HttpGet("Get_Call_Status_List", Name = "Get_Call_Status_List")]
        public IActionResult Get_Call_Status_List()
        {
            try
            {
                var statusList = callRepository.Get_Call_Status_List();
                if (statusList.Count() > 0)
                {
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.SucessMessage,
                        message = "Data get successfully",
                        data = statusList
                    };
                    return Ok(response);
                }
                else
                {
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                        message = "No Data Found",
                        data = statusList
                    };
                    return Ok(response);

                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Call_API", "Get_Call_Status_List", 1, string.Empty);
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                    message = ex.Message.ToString(),
                };
                return Ok(response);
            }
        }
        [HttpPost("CancelInspection", Name = "CancelInspection")]
        public IActionResult CancelInspection([FromBody] CancelInspectionRequestModel cancelInspectionRequestModel)
        {
            try
            {
                int id = callRepository.CancelInspection(cancelInspectionRequestModel.IeCd, cancelInspectionRequestModel.CaseNo, cancelInspectionRequestModel.PlanDt, cancelInspectionRequestModel.CallRecvDt, cancelInspectionRequestModel.CallSno);
                if (id > 0)
                {
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.SucessMessage,
                        message = "Successfully"
                    };
                    return Ok(response);
                }
                else
                {
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                        message = "No Data Found",
                    };
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Call_API", "CancelInspection", 1, string.Empty);
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                    message = ex.Message.ToString(),
                };
                return Ok(response);
            }
        }

        [HttpPost("ICPhotoUpload")]
        [Consumes("multipart/form-data")]
        public IActionResult ICPhotoUpload(string CaseNo, string DocBkNo, string DocSetNo, List<IFormFile> photos)
        {
            try
            {
                ICPhotoUploadRequestModel model = new ICPhotoUploadRequestModel();
                model.CaseNo=CaseNo;
                model.DocBkNo = DocBkNo;
                model.DocSetNo = DocSetNo;
                string IsStaging = Configuration["MyAppSettings:IsStaging"];
                if (photos != null && photos.Count > 0)
                {
                    List<string> uploadedFileNames = new List<string>();
                    var FileName = model.CaseNo + "-" + model.DocBkNo + "-" + model.DocSetNo;
                    List<APPDocumentDTO> DocumentsList = new List<APPDocumentDTO>();
                    int i = 1;
                    foreach (var photo in photos)
                    {
                        if (photo.Length > 0)
                        {
                            string fileExtension = Path.GetExtension(photo.FileName);
                            APPDocumentDTO aPP = new APPDocumentDTO();
                            Guid newGuid = Guid.NewGuid();
                            aPP.UniqueFileName = newGuid.ToString()+ fileExtension;
                            aPP.DocName = "IC Image " + i;
                            aPP.FileName = photo.FileName;
                            aPP.formFile = photo;
                            DocumentsList.Add(aPP);
                            
                            //string WebRootPath = "";
                            //if (Convert.ToBoolean(IsStaging) == true)
                            //{
                            //    WebRootPath = env.WebRootPath.Replace("IBS2API", "IBS2");
                            //    //WebRootPath = env.WebRootPath.Replace("IBSAPI", "IBS");
                            //}
                            //else
                            //{
                            //    WebRootPath = env.WebRootPath;
                            //}
                            //string TempFilePath = WebRootPath + Enums.GetEnumDescription(Enums.FolderPath.TempFilePath);
                            //string TempPath = Path.Combine(TempFilePath, aPP.UniqueFileName+ fileExtension);
                            //using (var fileStream = System.IO.File.Create(TempPath))
                            //{
                            //    photo.CopyTo(fileStream);
                            //}
                            //Common.AddException(TempPath, TempPath, "Call", "UploadAPI", 1, string.Empty);
                        }
                        i++;
                    }
                    
                    int retID =DocumentHelper.SaveICFiles(Convert.ToString(model.CaseNo), DocumentsList, Enums.GetEnumDescription(Enums.FolderPath.ICPHOTOS), env, null, FileName, string.Empty, 22, IsStaging);

                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.SucessMessage,
                        message = "Successfully"
                    };
                    return Ok(response);
                }
                else
                {
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                        message = "No file uploaded"
                    };
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                var response = new
                {
                    resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                    message = ex.Message.ToString(),
                };
                return Ok(response);
            }
        }

        [HttpPost("DeleteICPhoto", Name = "DeleteICPhoto")]
        public IActionResult DeleteICPhoto([FromBody] DeleteICPhotoRequestModel deleteICPhotoRequestModel)
        {
            try
            {
                int id = inspectionRepository.DeleteSingleRecord(deleteICPhotoRequestModel);
                if (id > 0)
                {
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.SucessMessage,
                        message = "Successfully"
                    };
                    return Ok(response);
                }
                else
                {
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                        message = "No Data Found",
                    };
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Call_API", "DeleteICPhoto", 1, string.Empty);
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
