using IBS.Filters;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    [Authorization]
    public class IE_Instructions_AdminController : BaseController
    {

        #region Variables
        private readonly IIE_Instructions_AdminRepository instructionsRepository;
        #endregion
        public IE_Instructions_AdminController(IIE_Instructions_AdminRepository _instructionsRepository)
        {
            instructionsRepository = _instructionsRepository;
        }

        [Authorization("IE_Instructions_Admin", "IE_InstructionsMaster", "view")]
        public IActionResult IE_InstructionsMaster()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<IE_Instructions_AdminModel> dTResult = instructionsRepository.GetMessageList(dtParameters, GetRegionCode);
            return Json(dTResult);
        }

        [Authorization("IE_Instructions_Admin", "IE_InstructionsMaster", "delete")]
        public IActionResult Delete(int MessageID)
        {
            try
            {
                if (instructionsRepository.Remove(MessageID, GetRegionCode))
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
        public IActionResult IE_InstructionsManage(IE_Instructions_AdminModel model)
        {
            try
            {
                if (model.MessageId == 0)
                {
                    model.Createdby = USER_ID.Substring(0, 8);
                    model.UserId = USER_ID.Substring(0, 8);
                    instructionsRepository.SaveDetails(model, Region);
                    AlertAddSuccess("Record Added Successfully.");
                }
                else
                {
                    model.Updatedby = USER_ID.Substring(0, 8);
                    model.UserId = USER_ID.Substring(0, 8);
                    instructionsRepository.SaveDetails(model, Region);
                    AlertAddSuccess("Record Updated Successfully.");
                }
                return RedirectToAction("IE_InstructionsMaster");
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "IE_Instructions_Admin", "UserDetailsSave", 1, GetIPAddress());
            }
            return View(model);

        }

        [Authorization("IE_Instructions_Admin", "IE_InstructionsMaster", "view")]
        public IActionResult IE_InstructionsManage(int MessageID, string RegionCode)
        {
            IE_Instructions_AdminModel model = new();

            if (MessageID > 0)
            {
                model = instructionsRepository.FindByID(MessageID, RegionCode);

            }
            return View(model);
        }
    }
}
