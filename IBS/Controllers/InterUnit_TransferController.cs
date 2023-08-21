using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class InterUnit_TransferController : BaseController
    {
        private readonly IInterUnit_TransferRepository interunittransferrepository;
        public InterUnit_TransferController(IInterUnit_TransferRepository _interunittransferrepository)
        {
            interunittransferrepository = _interunittransferrepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetValue(int BankNameDropdown, string CHQ_NO, string CHQ_DATE)
        {
            string region = GetRegionCode;
            InterUnit_TransferModel dTResult = interunittransferrepository.GetTextboxValues(BankNameDropdown, CHQ_NO, CHQ_DATE, region);
            return Json(dTResult);

        }

        public IActionResult GetJVValue(int BankNameDropdown, string CHQ_NO, string CHQ_DATE)
        {

            
            InterUnit_TransferModel dTResult = interunittransferrepository.GetJVvalues(BankNameDropdown, CHQ_NO, CHQ_DATE);
            return Json(dTResult);

        }

        [HttpPost]
        public IActionResult LoadGrid([FromBody] DTParameters dtParameters)
        {
            DTResult<InterUnit_TransferModel> dTResult = interunittransferrepository.BillList(dtParameters);

            return Json(dTResult);


        }

        

    }
}
