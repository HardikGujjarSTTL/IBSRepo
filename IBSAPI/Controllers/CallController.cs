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
        private readonly IConfiguration config;
        //private readonly IDocument iDocument;
        public IConfiguration Configuration { get; }
        #endregion

        public CallController(ICallRepository _callRepository, IWebHostEnvironment _environment,  IConfiguration configuration, IInspectionRepository _inspectionRepository, IConfiguration _config)
        {
            callRepository = _callRepository;
            env = _environment;
            //iDocument = _iDocumentRepository;
            Configuration = configuration;
            inspectionRepository = _inspectionRepository;
            config = _config;
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
                int PlanDHours = Convert.ToInt32(config.GetSection("MyAppSettings")["PlanDHours"]);
                int id = callRepository.SheduleInspection(sheduleInspectionRequestModel,PlanDHours);
                if (id == 999)
                {
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                        message = "Your Work Plan Cannot be Saved due to today after 3:00 clock can't saved. ",
                    };
                    return Ok(response);
                }
                else
                {
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
        public IActionResult ICPhotoUpload(string CaseNo, string DocBkNo, string DocSetNo, decimal? Latitude, decimal? Longitude,
            string Consignee,decimal? QtyPassed,decimal? QtyRejected,DateTime call_Recv_DT,int CallSno,string PoNo,
            int? IeCd,string userId
            , List<IFormFile> photos, IFormFile ICPhotoDigitalSign, IFormFile UploadTestPlan, IFormFile UploadICAnnexue1, IFormFile UploadICAnnexue2)
        {
            try
            {
                Common.AddException("1", "1", "Call_API", "ICPhotoUpload", 1, string.Empty);
                ICPhotoUploadRequestModel model = new ICPhotoUploadRequestModel();
                model.CaseNo=CaseNo;
                model.DocBkNo = DocBkNo;
                model.DocSetNo = DocSetNo;
                model.Consignee = Consignee;
                model.QtyPassed = QtyPassed;
                model.QtyRejected = QtyRejected;
                model.CallRecvDt = call_Recv_DT;
                model.CallSno = CallSno;
                model.PoNo = PoNo;
                model.IeCd = IeCd;
                model.userId = userId;
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
                            aPP.Latitude = Latitude;
                            aPP.Longitude = Longitude;
                            DocumentsList.Add(aPP);
                        }
                        i++;
                    }
                    
                    int retID =DocumentHelper.SaveICFiles(Convert.ToString(model.CaseNo), DocumentsList, Enums.GetEnumDescription(Enums.FolderPath.ICPHOTOS), env, null, FileName, string.Empty, 22, IsStaging);

                    int ICPhoto_Dig_SignDID = (int)Enums.DocumentCategory_CANRegisrtation.ICPhoto_Dig_Sign;
                    int Upload_Test_PlanDID = (int)Enums.DocumentCategory_CANRegisrtation.Upload_Test_Plan;
                    int Upload_IC_Annexue1DID = (int)Enums.DocumentCategory_CANRegisrtation.Upload_IC_Annexue1;
                    int Upload_IC_Annexue2DID = (int)Enums.DocumentCategory_CANRegisrtation.Upload_IC_Annexue2;
                    Common.AddException("2", "2", "Call_API", "ICPhotoUpload", 1, string.Empty);
                    if (ICPhotoDigitalSign.Length > 0)
                    {
                        if (ICPhotoDigitalSign.Name == "ICPhotoDigitalSign")
                        {
                            Common.AddException("3", "3", "Call_API", "ICPhotoUpload", 1, string.Empty);
                            DocumentsList = new List<APPDocumentDTO>();
                            APPDocumentDTO aPP = new APPDocumentDTO();
                            aPP.Documentid = (int)Enums.DocumentCategory_CANRegisrtation.ICPhoto_Dig_Sign;
                            aPP.FileName = ICPhotoDigitalSign.FileName;
                            aPP.formFile = ICPhotoDigitalSign;
                            DocumentsList.Add(aPP);
                            FileName = model.CaseNo + "-" + model.DocBkNo + "-" + model.DocSetNo + ".PDF";
                            DocumentHelper.SavePDFForCallFiles(Convert.ToString(model.CaseNo), DocumentsList, Enums.GetEnumDescription(Enums.FolderPath.BILLIC), env, null, FileName, string.Empty, ICPhoto_Dig_SignDID, IsStaging);
                            Common.AddException("4", "4", "Call_API", "ICPhotoUpload", 1, string.Empty);
                        }
                    }
                    if (UploadTestPlan.Length > 0)
                    {
                        if (UploadTestPlan.Name == "UploadTestPlan")
                        {
                            DocumentsList = new List<APPDocumentDTO>();
                            APPDocumentDTO aPP = new APPDocumentDTO();
                            aPP.Documentid = (int)Enums.DocumentCategory_CANRegisrtation.Upload_Test_Plan;
                            aPP.FileName = ICPhotoDigitalSign.FileName;
                            aPP.formFile = ICPhotoDigitalSign;
                            DocumentsList.Add(aPP);
                            FileName = model.CaseNo + "-" + model.DocBkNo + "-" + model.DocSetNo + ".PDF";
                            DocumentHelper.SavePDFForCallFiles(Convert.ToString(model.CaseNo), DocumentsList, Enums.GetEnumDescription(Enums.FolderPath.TESTPLAN), env, null, FileName, string.Empty, Upload_Test_PlanDID, IsStaging);
                        }
                    }
                    if (UploadICAnnexue1.Length > 0)
                    {
                        if (UploadICAnnexue1.Name == "UploadICAnnexue1")
                        {
                            DocumentsList = new List<APPDocumentDTO>();
                            APPDocumentDTO aPP = new APPDocumentDTO();
                            aPP.Documentid = (int)Enums.DocumentCategory_CANRegisrtation.Upload_IC_Annexue1;
                            aPP.FileName = ICPhotoDigitalSign.FileName;
                            aPP.formFile = ICPhotoDigitalSign;
                            DocumentsList.Add(aPP);
                            FileName = model.CaseNo + "-" + model.DocBkNo + "-" + model.DocSetNo + "-A1.PDF";
                            DocumentHelper.SavePDFForCallFiles(Convert.ToString(model.CaseNo), DocumentsList, Enums.GetEnumDescription(Enums.FolderPath.BILLIC), env, null, FileName, string.Empty, Upload_IC_Annexue1DID, IsStaging);
                        }
                    }
                    if (UploadICAnnexue2.Length > 0)
                    {
                        if (UploadICAnnexue2.Name == "UploadICAnnexue2")
                        {
                            DocumentsList = new List<APPDocumentDTO>();
                            APPDocumentDTO aPP = new APPDocumentDTO();
                            aPP.Documentid = (int)Enums.DocumentCategory_CANRegisrtation.Upload_IC_Annexue2;
                            aPP.FileName = ICPhotoDigitalSign.FileName;
                            aPP.formFile = ICPhotoDigitalSign;
                            DocumentsList.Add(aPP);
                            FileName = model.CaseNo + "-" + model.DocBkNo + "-" + model.DocSetNo + "-A2.PDF";
                            DocumentHelper.SavePDFForCallFiles(Convert.ToString(model.CaseNo), DocumentsList, Enums.GetEnumDescription(Enums.FolderPath.BILLIC), env, null, FileName, string.Empty, Upload_IC_Annexue2DID, IsStaging);
                        }
                    }

                    int id = inspectionRepository.CallStatusFilesSave(model);
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
                            message = "Something wrong"
                        };
                        return BadRequest(response);
                    }
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
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Call_API", "ICPhotoUpload", 1, string.Empty);
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
