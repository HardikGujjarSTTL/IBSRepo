using IBS.Filters;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace IBS.Controllers
{
    [Authorization]
    public class IE_Instructions_AdminController : BaseController
    {

        #region Variables
        private readonly IIE_Instructions_AdminRepository ie_instructions_adminRepository;
        #endregion
        public IE_Instructions_AdminController(IIE_Instructions_AdminRepository _ie_instructions_adminRepository)
        {
            ie_instructions_adminRepository = _ie_instructions_adminRepository;
        }

        [Authorization("IE_Instructions_Admin", "IE_InstructionsMaster", "view")]
        public IActionResult IE_InstructionsMaster()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<IE_Instructions_AdminModel> dTResult = ie_instructions_adminRepository.GetMessageList(dtParameters, GetRegionCode);
            return Json(dTResult);
        }

        [Authorization("IE_Instructions_Admin", "IE_InstructionsMaster", "delete")]
        public IActionResult Delete(int MessageID)
        {
            try
            {
                if (ie_instructions_adminRepository.Remove(MessageID, GetRegionCode))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "IE Instruction Message", "Delete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("IE_InstructionsMaster");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorization("IE_Instructions_Admin", "IE_InstructionsMaster", "edit")]
        public IActionResult DetailsSave(IE_Instructions_AdminModel model)
        {
            try
            {
                string msg = "Message Inserted Successfully.";

                if (model.MessageId > 0)
                {
                    msg = "Message Updated Successfully.";
                    model.Updatedby = Convert.ToString(UserId);
                }
                model.Createdby = Convert.ToString(UserId);
                //model.MessageId = setMessageID;

                int i = ie_instructions_adminRepository.MessageDetailsInsertUpdate(model, GetRegionCode);
                if (i > 0)
                {
                    return Json(new { status = true, responseText = msg, Id = i });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "IE Instruction Message", "UserDetailsSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [Authorization("IE_Instructions_Admin", "IE_InstructionsMaster", "view")]
        public IActionResult IE_InstructionsManage(int MessageID, string RegionCode)
        {
            IE_Instructions_AdminModel model = new();

            if (MessageID > 0)
            {
                model = ie_instructions_adminRepository.FindByID(MessageID, RegionCode);

            }
            //else
            //{
            //    setMessageID = ie_instructions_adminRepository.FindByMaxID(MessageID, GetRegionCode);
            //}
            return View(model);
        }
    }
}
