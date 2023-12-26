using IBS.Filters;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class Rly_Online_Check_Posting_FormController : BaseController
    {

        private readonly IRly_Online_Check_Posting_Form_Repository rlychkpostingrepository;
        public Rly_Online_Check_Posting_FormController(IRly_Online_Check_Posting_Form_Repository _rlychkpostingrepository)
        {
            rlychkpostingrepository = _rlychkpostingrepository;
        }

        public IActionResult GetValue(string BankNameDropdown, string CHQ_NO, string CHQ_DT)
        {
            string region = GetRegionCode;
            Rly_Online_Check_Posting_Form_Model dTResult = rlychkpostingrepository.GetTextboxValues(BankNameDropdown, CHQ_NO, CHQ_DT, region);
            return Json(dTResult);

        }



        [HttpPost]
        public IActionResult ChequePost([FromBody] DTParameters dtParameters)
        {
            string Region = GetRegionCode;
            DTResult<Rly_Online_Check_Posting_Form_Model> dTResult = rlychkpostingrepository.BillList(dtParameters, Region);

            return Json(dTResult);


        }
        //[Authorization("Rly_Online_Check_Posting_Form", "Submit", "Add")]

        [HttpPost]
        public IActionResult Submit([FromBody] RequestDataModel requestData)
        {
            string Uname = Convert.ToString(UserId);
            string i = rlychkpostingrepository.Submit(requestData, Uname);
            if (i == Convert.ToString(true))
            {
                return Json(i);
            }
            else
            {

                return null;

            }
        }
        [Authorization("Rly_Online_Check_Posting_Form", "Index", "view")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
