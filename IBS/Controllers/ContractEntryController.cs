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
                List<IBS_DocumentDTO> lstDocumentUpload_Memo = iDocument.GetRecordsList((int)Enums.DocumentCategory.ContractEntryDoc, Convert.ToString(id));
                FileUploaderDTO FileUploaderUpload_Memo = new FileUploaderDTO();
                FileUploaderUpload_Memo.Mode = (int)Enums.FileUploaderMode.Add_Edit;
                FileUploaderUpload_Memo.IBS_DocumentList = lstDocumentUpload_Memo.Where(m => m.ID == (int)Enums.DocumentCategory_CANRegisrtation.Upload_Contract_Doc).ToList();
                FileUploaderUpload_Memo.OthersSection = false;
                FileUploaderUpload_Memo.MaxUploaderinOthers = 5;
                FileUploaderUpload_Memo.FilUploadMode = (int)Enums.FilUploadMode.Single;
                ViewBag.Upload_Contract_Doc = FileUploaderUpload_Memo;

                if (id > 0)
                {
                    model = contractEntryRepository.FindByID(id);
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
            if(clientType == "R") 
                filteredUsers = Common.GetRailwayWithCode();
            else
                filteredUsers = Common.GetClientName(clientType, searchTerm);

            var result = filteredUsers.Select(u => new
            {
                id = u.Value,
                text = u.Text
            });
            return Json(new { data = result });
        }

    }
}
