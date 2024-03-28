using IBS.Filters;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IBS.Controllers
{
    public class WriteOffEntryController : BaseController
    {
        private readonly IWriteOffEntryRepository writeOffEntryRepository;

        public WriteOffEntryController(IWriteOffEntryRepository _writeOffEntryRepository)
        {
            writeOffEntryRepository = _writeOffEntryRepository;
        }

        [Authorization("WriteOffEntry", "Index", "view")]
        public IActionResult Index()
        {
            ViewBag.Region = Region;
            return View();
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<WriteOffEntryModel> dTResult = writeOffEntryRepository.GetWriteOfEntryList(dtParameters, Region);
            return Json(dTResult);
        }

        [HttpPost]
        [Authorization("WriteOffEntry", "Index", "edit")]
        public IActionResult UpdateWriteAmt([FromForm] List<UpdateDataModel> dataArr)
        {
            try
            {
                if (dataArr != null && dataArr.Count > 0)
                {
                    WriteOfMaster model = new WriteOfMaster();
                    model.CreatedBy = UserId;
                    model.CreatedDT = DateTime.Now;

                    writeOffEntryRepository.UpdateWriteAmtDetails(dataArr, model);
                    AlertAddSuccess("Record Updated Successfully.");
                    return Json(new { success = true });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "WriteOffEntry", "UpdateWriteAmt", 1, GetIPAddress());
            }
            return Json(new { success = false });
        }

        public IActionResult getBPO(string SBPO)
        {
            try
            {
                List<SelectListItem> objList = Common.GetBillPayingOfficerUsingSBPO(SBPO);
                var result = objList.Select(u => new
                {
                    id = u.Value,
                    text = u.Text
                });
                return Json(new { data = result });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "AdministratorPurchaseOrder", "getBPO", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
    }
}
