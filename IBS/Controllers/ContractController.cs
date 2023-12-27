using IBS.Filters;
using IBS.Helper;
using IBS.Helpers;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IBS.Controllers
{
    [Authorization]
    public class ContractController : BaseController
    {
        #region Variables
        private readonly IDocument iDocument;
        private readonly IWebHostEnvironment env;
        private readonly IContractRepository contractRepository;
        #endregion
        public ContractController(IDocument _iDocumentRepository, IWebHostEnvironment _environment, IContractRepository _contractRepository)
        {
            iDocument = _iDocumentRepository;
            env = _environment;
            contractRepository = _contractRepository;
        }
        [Authorization("Contract", "Index", "view")]
        public IActionResult Index()
        {
            return View();
        }
        [Authorization("Contract", "Index", "view")]
        public IActionResult Manage(int id)
        {
            ContractModel model = new();
            if (id > 0)
            {
                model = contractRepository.FindByID(id);
            }
            List<IBS_DocumentDTO> lstDocument = iDocument.GetRecordsList((int)Enums.DocumentCategory.Contract, Convert.ToString(id));
            FileUploaderDTO FileUploaderContract = new FileUploaderDTO();
            FileUploaderContract.Mode = (int)Enums.FileUploaderMode.Add_Edit;
            FileUploaderContract.IBS_DocumentList = lstDocument.Where(m => m.ID == (int)Enums.DocumentCategory_CANRegisrtation.Contract_Documents_If_Any).ToList();
            FileUploaderContract.OthersSection = false;
            FileUploaderContract.MaxUploaderinOthers = 5;
            FileUploaderContract.FilUploadMode = (int)Enums.FilUploadMode.Single;
            ViewBag.Contract_Documents_If_Any = FileUploaderContract;

            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<ContractModel> dTResult = contractRepository.GetContractList(dtParameters);
            return Json(dTResult);
        }
        [Authorization("Contract", "Index", "delete")]
        public IActionResult Delete(int id)
        {
            try
            {
                if (contractRepository.Remove(id, UserId))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Contract", "Delete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorization("Contract", "Index", "edit")]
        public IActionResult ContractDetailsSave(ContractModel model, IFormCollection FrmCollection)
        {
            try
            {
                string msg = "Contract Inserted Successfully.";
                if (model.ContractId > 0)
                {
                    msg = "Contract Updated Successfully.";
                    model.Updatedby = UserId;
                }
                model.Createdby = UserId;
                model.UserId = Convert.ToString(UserId);
                int id = contractRepository.ContractDetailsInsertUpdate(model);
                if (id > 0)
                {
                    if (!string.IsNullOrEmpty(FrmCollection["hdnUploadedDocumentList_tab-1"]))
                    {
                        int[] DocumentIds = { (int)Enums.DocumentCategory_CANRegisrtation.Address_Proof_Document, (int)Enums.DocumentCategory_CANRegisrtation.Contract_Documents_If_Any };
                        List<APPDocumentDTO> DocumentsList = JsonConvert.DeserializeObject<List<APPDocumentDTO>>(FrmCollection["hdnUploadedDocumentList_tab-1"]);
                        DocumentHelper.SaveFiles(Convert.ToString(id), DocumentsList, Enums.GetEnumDescription(Enums.FolderPath.ContractDocument), env, iDocument, "Contract", string.Empty, DocumentIds);
                    }
                    return Json(new { status = true, responseText = msg });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Contract", "ContractDetailsSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

    }
}
