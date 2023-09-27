using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;


namespace IBS.Controllers
{
    public class Call_Cancellation_FormController : BaseController
    {
        private readonly ICall_Cancellation_FormRepository callcancellationformrepository;
        public Call_Cancellation_FormController(ICall_Cancellation_FormRepository _callcancellationformrepository)
        {
            callcancellationformrepository = _callcancellationformrepository;
        }

        [HttpPost]
        public IActionResult Save(Call_Cancellation_FormModel model ,string selectedValues) 
        {
            var msg = "Successfull";
            string Uname = Convert.ToString(UserId);

            string i = callcancellationformrepository.SaveDetails(model ,selectedValues,Uname);
            if (i != null)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Delete(string caseNo, string calldate, string callsno)
        {
            string i = callcancellationformrepository.delete_details(caseNo, calldate, callsno);
            if (i != null)
            {
                var alert = "Deleted Successfully";
                return Json(new { status = false, alert = alert, responseText = "Oops Somthing Went Wrong !!" }); ;
            }
            return null;
        }
        public IActionResult Index(string caseNo = "", string callRecvDate = "", string callSno = "")
        {
            Call_Cancellation_FormModel dtresult = callcancellationformrepository.Combined(caseNo, callRecvDate, callSno);
            
            if(dtresult != null)
            {
                return View(dtresult);
                //return Json(dtresult);
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
                
    }
}
