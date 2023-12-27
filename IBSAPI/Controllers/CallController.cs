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
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using static IBSAPI.Helper.Enums;

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

        public CallController(ICallRepository _callRepository, IWebHostEnvironment _environment, IConfiguration configuration, IInspectionRepository _inspectionRepository, IConfiguration _config)
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
                int id = callRepository.SheduleInspection(sheduleInspectionRequestModel, PlanDHours);
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
            string Consignee, decimal? QtyPassed, decimal? QtyRejected, DateTime call_Recv_DT, int CallSno, string PoNo,
            int? IeCd, string userId, int User_Id, string Call_Status, string ReasonFIFO, List<IFormFile> photos, IFormFile ICPhotoDigitalSign, IFormFile UploadTestPlan, IFormFile UploadICAnnexue1, IFormFile UploadICAnnexue2)
        {
            try
            {
                ICPhotoUploadRequestModel model = new ICPhotoUploadRequestModel();
                model.CaseNo = CaseNo;
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
                model.User_Id = User_Id;
                string IsStaging = Configuration["MyAppSettings:IsStaging"];
                if ((photos != null && photos.Count > 0 && (Call_Status == "A" || Call_Status == "R" || Call_Status == "G")) || (Call_Status != "A" || Call_Status != "R" || Call_Status != "G"))
                {
                    VenderCallStatusModel obj = new VenderCallStatusModel();
                    obj.CaseNo = CaseNo;
                    obj.DocBkNo = DocBkNo;
                    obj.DocSetNo = DocSetNo;
                    obj.Consignee = Consignee;
                    //obj.QtyPassed = QtyPassed;
                    //obj.QtyRejected = QtyRejected;
                    obj.CallRecvDt = call_Recv_DT;
                    obj.CallSno = CallSno;
                    obj.PoNo = PoNo;
                    //obj.IeCd = IeCd;
                    obj.UserId = User_Id;
                    obj.UserName = userId;
                    obj.CallStatus = Call_Status;
                    obj.ReasonFIFO = ReasonFIFO;

                    int id = 0;
                    if (Call_Status == "A" || Call_Status == "R" || Call_Status == "G")
                    {
                        id = inspectionRepository.IcIntermediateSave(model, photos, ICPhotoDigitalSign, UploadTestPlan, UploadICAnnexue1, UploadICAnnexue2);
                        if (id == 0)
                        {
                            var response = new
                            {
                                resultFlag = (int)Helper.Enums.ResultFlag.ValidationMessage,
                                message = model.AlertMsg
                            };
                            return Ok(response);
                        }
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
                                aPP.UniqueFileName = newGuid.ToString() + fileExtension;
                                aPP.DocName = "IC Image " + i;
                                aPP.FileName = photo.FileName;
                                aPP.formFile = photo;
                                aPP.Latitude = Latitude;
                                aPP.Longitude = Longitude;
                                DocumentsList.Add(aPP);
                            }
                            i++;
                        }
                        int retID = DocumentHelper.SaveICFiles(Convert.ToString(model.CaseNo), DocumentsList, Enums.GetEnumDescription(Enums.FolderPath.ICPHOTOS), env, null, FileName, string.Empty, 22, IsStaging);
                        AccepRejPDFSave(DocumentsList, model, IsStaging, ICPhotoDigitalSign, UploadTestPlan, UploadICAnnexue1, UploadICAnnexue2);
                    }

                    // Call Status Update not in Accept and Reject
                    if (Call_Status != "A" && Call_Status != "R" && Call_Status != "G")
                    {
                        var msg = callRepository.Save(obj, ICPhotoDigitalSign.Name);
                        if (obj.AlertMsg == "Success")
                        {
                            if (Call_Status == "T")//(Call_Status == "G" || Call_Status == "T")
                            {
                                //Save PDF for Call Status Accept & Reject
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
                                        aPP.UniqueFileName = newGuid.ToString() + fileExtension;
                                        aPP.DocName = "IC Image " + i;
                                        aPP.FileName = photo.FileName;
                                        aPP.formFile = photo;
                                        aPP.Latitude = Latitude;
                                        aPP.Longitude = Longitude;
                                        DocumentsList.Add(aPP);
                                    }
                                    i++;
                                }
                                int retID = DocumentHelper.SaveICFiles(Convert.ToString(model.CaseNo), DocumentsList, Enums.GetEnumDescription(Enums.FolderPath.ICPHOTOS), env, null, FileName, string.Empty, 22, IsStaging);
                                AccepRejPDFSave(DocumentsList, model, IsStaging, ICPhotoDigitalSign, UploadTestPlan, UploadICAnnexue1, UploadICAnnexue2);
                            }

                            if (obj.AlertMsg == "Success")
                            {
                                var response = new
                                {
                                    resultFlag = (int)Helper.Enums.ResultFlag.SucessMessage,
                                    message = "Record Inserted Successfully !!"
                                };
                                return Ok(response);
                            }
                            else
                            {
                                var response = new
                                {
                                    resultFlag = (int)Helper.Enums.ResultFlag.ValidationMessage,
                                    message = obj.AlertMsg
                                };
                                return Ok(response);
                            }
                        }
                    }
                    else if ((Call_Status == "A" || Call_Status == "R" || Call_Status == "G") && id > 0)
                    {

                        var msg = callRepository.CallStatusAcceptRej(obj);
                        if (obj.AlertMsg == "Success")
                        {
                            var StatusMsg = "Record Accepted Successfully !!";
                            if (Call_Status == "R") { StatusMsg = "Record Rejected Successfully !!"; }
                            else if (Call_Status == "G") { StatusMsg = "Record Stage Inspection Accepted Successfully !!"; }
                            var response = new
                            {
                                resultFlag = (int)Helper.Enums.ResultFlag.SucessMessage,
                                message = StatusMsg
                            };
                            return Ok(response);
                        }
                        else
                        {
                            var response = new
                            {
                                resultFlag = (int)Helper.Enums.ResultFlag.ValidationMessage,
                                message = obj.AlertMsg
                            };
                            return Ok(response);
                        }
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
                                message = "Something wrong"
                            };
                            return Ok(response);
                        }
                    }
                }
                else
                {
                    var response = new
                    {
                        resultFlag = (int)Helper.Enums.ResultFlag.ErrorMessage,
                        message = "No file uploaded"
                    };
                    return Ok(response);
                }
                return Ok();
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

        private string AccepRejPDFSave(List<APPDocumentDTO> DocumentsList, ICPhotoUploadRequestModel model, string IsStaging, IFormFile ICPhotoDigitalSign, IFormFile UploadTestPlan, IFormFile UploadICAnnexue1, IFormFile UploadICAnnexue2)
        {
            //string IsStaging = Configuration["MyAppSettings:IsStaging"];
            int ICPhoto_Dig_SignDID = (int)Enums.DocumentCategory_CANRegisrtation.ICPhoto_Dig_Sign;
            int Upload_Test_PlanDID = (int)Enums.DocumentCategory_CANRegisrtation.Upload_Test_Plan;
            int Upload_IC_Annexue1DID = (int)Enums.DocumentCategory_CANRegisrtation.Upload_IC_Annexue1;
            int Upload_IC_Annexue2DID = (int)Enums.DocumentCategory_CANRegisrtation.Upload_IC_Annexue2;
            var FileName = "";

            if (ICPhotoDigitalSign.Length > 0)
            {
                if (ICPhotoDigitalSign.Name == "ICPhotoDigitalSign")
                {
                    DocumentsList = new List<APPDocumentDTO>();
                    APPDocumentDTO aPP = new APPDocumentDTO();
                    aPP.Documentid = (int)Enums.DocumentCategory_CANRegisrtation.ICPhoto_Dig_Sign;
                    aPP.FileName = ICPhotoDigitalSign.FileName;
                    aPP.formFile = ICPhotoDigitalSign;
                    DocumentsList.Add(aPP);
                    FileName = model.CaseNo + "-" + model.DocBkNo + "-" + model.DocSetNo + ".PDF";
                    DocumentHelper.SavePDFForCallFiles(Convert.ToString(model.CaseNo), DocumentsList, Enums.GetEnumDescription(Enums.FolderPath.BILLIC), env, null, FileName, string.Empty, ICPhoto_Dig_SignDID, IsStaging);
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
            return "";
        }
    }
}
