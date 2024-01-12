using IBS.Filters;
using IBS.Helper;
using IBS.Helpers;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace IBS.Controllers
{
    public class ContractEntryController : BaseController
    {
        private readonly IContractEntryRepository contractEntryRepository;
        private readonly IDocument iDocument;
        private readonly IWebHostEnvironment env;
        SessionHelper objSessionHelper = new SessionHelper();

        public ContractEntryController(IDocument _iDocumentRepository, IWebHostEnvironment _environment, IContractEntryRepository _contractEntryRepository)
        {
            iDocument = _iDocumentRepository;
            env = _environment;
            contractEntryRepository = _contractEntryRepository;
        }
        [Authorization("ContractEntry", "Index", "view")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<ContractEntry> dTResult = contractEntryRepository.GetContractList(dtParameters);
            return Json(dTResult);
        }

        [Authorization("ContractEntry", "Index", "view")]
        public IActionResult Manage(int id)
        {
            ContractEntry model = new();
            try
            {
                var AppID = id > 0 ? Convert.ToString(id) : "";
                List<IBS_DocumentDTO> lstDocumentUpload_Memo = new List<IBS_DocumentDTO>();
                FileUploaderDTO FileUploaderUpload_Memo = new FileUploaderDTO();

                lstDocumentUpload_Memo = iDocument.GetRecordsList((int)Enums.DocumentCategory.ContractEntryDoc, AppID);
                FileUploaderUpload_Memo.Mode = (int)Enums.FileUploaderMode.Add_Edit;
                FileUploaderUpload_Memo.IBS_DocumentList = lstDocumentUpload_Memo.Where(m => m.ID == (int)Enums.DocumentCategory_CANRegisrtation.Upload_Contract_Doc).ToList();
                FileUploaderUpload_Memo.OthersSection = false;
                FileUploaderUpload_Memo.MaxUploaderinOthers = 5;
                FileUploaderUpload_Memo.FilUploadMode = (int)Enums.FilUploadMode.Single;
                ViewBag.Upload_Contract_Doc = FileUploaderUpload_Memo;
                if (id > 0)
                {
                    model = contractEntryRepository.FindByID(id);
                    objSessionHelper.lstContractEntryList = model.lstContractEntryList;
                }
                else
                {
                    objSessionHelper.lstContractEntryList = null;
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "ContractEntry", "Manage", 1, GetIPAddress());
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorization("ContractEntry", "Index", "edit")]
        public IActionResult ContractDetailsSave(ContractEntry model, IFormCollection FrmCollection)
        {
            try
            {
                string msg = "Contract Inserted Successfully.";

                if (model.ID > 0)
                {
                    msg = "Contract Updated Successfully.";
                    model.UpdatedBy = UserId;
                }
                model.CreatedBy = UserId;
                if (objSessionHelper.lstContractEntryList != null)
                {
                    model.lstContractEntryList = objSessionHelper.lstContractEntryList;
                }
                int i = contractEntryRepository.ContractDetailsInsertUpdate(model);
                if (i > 0)
                {
                    if (!string.IsNullOrEmpty(FrmCollection["hdnUploadedDocumentList_tab-1"]))
                    {
                        int[] DocumentIds = { (int)Enums.DocumentCategory_CANRegisrtation.Upload_Contract_Doc };
                        List<APPDocumentDTO> DocumentsList = JsonConvert.DeserializeObject<List<APPDocumentDTO>>(FrmCollection["hdnUploadedDocumentList_tab-1"]);
                        DocumentHelper.SaveFiles(Convert.ToString(model.ID), DocumentsList, Enums.GetEnumDescription(Enums.FolderPath.ContractEntry), env, iDocument, "CONTRACT_ENTRY", string.Empty, DocumentIds);
                    }
                    return Json(new { status = true, responseText = msg });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "ContractEntry", "ContractDetailsSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        public IActionResult Delete(int id)
        {
            try
            {
                if (contractEntryRepository.Remove(id, UserId))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "ContractEntry", "Delete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
        }

        public IActionResult SearchClient(string clientType, string searchTerm)
        {
            List<SelectListItem> filteredUsers = new List<SelectListItem>();
            if (clientType == "R")
                filteredUsers = Common.GetRailwayWithCode(searchTerm);
            else
                filteredUsers = Common.GetClientName(clientType, searchTerm);

            var result = filteredUsers.Select(u => new
            {
                id = u.Value,
                text = u.Text
            });
            return Json(new { data = result });
        }

        [HttpPost]
        public IActionResult SaveMValue(ContractEntryList model)
        {
            try
            {
                List<ContractEntryList> lstContractEntryList = objSessionHelper.lstContractEntryList == null ? new List<ContractEntryList>() : objSessionHelper.lstContractEntryList;
                lstContractEntryList.RemoveAll(x => x.Id == Convert.ToInt32(model.Id));
                if (model.Id > 0)
                {
                    model.Id = model.Id;
                }
                else
                {
                    model.Id = lstContractEntryList.Count > 0 ? (lstContractEntryList.OrderByDescending(a => a.Id).FirstOrDefault().Id) + 1 : 1;
                }
                lstContractEntryList.Add(model);
                objSessionHelper.lstContractEntryList = lstContractEntryList;
                return Json(new { status = true, responseText = "Material Value Added Successfully." });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "ContractEntry ", "SaveMValue", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpPost]
        public IActionResult LoadDataTable([FromBody] DTParameters dtParameters)
        {
            List<ContractEntryList> lstContractEntryList = new List<ContractEntryList>();
            if (objSessionHelper.lstContractEntryList != null)
            {
                lstContractEntryList = objSessionHelper.lstContractEntryList;
            }

            DTResult<ContractEntryList> dTResult = contractEntryRepository.GetValueList(dtParameters, lstContractEntryList);
            return Json(dTResult);
        }

        [HttpGet]
        public IActionResult EditMValue(string id)
        {
            try
            {
                ContractEntryList MValue = objSessionHelper.lstContractEntryList.Where(x => x.Id == Convert.ToInt32(id)).FirstOrDefault();
                return Json(new { status = true, list = MValue });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "ContractEntry", "EditMValue", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpPost]
        public IActionResult DeleteMValue(string id)
        {
            try
            {
                List<ContractEntryList> lstContractEntryList = objSessionHelper.lstContractEntryList == null ? new List<ContractEntryList>() : objSessionHelper.lstContractEntryList;
                lstContractEntryList.RemoveAll(x => x.Id == Convert.ToInt32(id));
                objSessionHelper.lstContractEntryList = lstContractEntryList;
                return Json(new { status = true, responseText = "Material Value Deleted Successfully" });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "ContractEntry", "DeleteMValue", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
    }
}
