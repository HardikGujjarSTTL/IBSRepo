using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class Print_Call_letter_Controller : BaseController
    {

        private readonly IPrint_Call_letter_Repository printcallletterrepository;
        public Print_Call_letter_Controller(IPrint_Call_letter_Repository _printcallletterrepository)
        {
            printcallletterrepository = _printcallletterrepository;
        }
        public IActionResult Index(string caseNo = "", string callRecvDate = "", string callSno = "")
        {
            //query1( caseNo ,  callRecvDate ,  callSno );
            //query2(caseNo, callRecvDate, callSno);
            Print_Call_letter_Model dTResult = printcallletterrepository.CombinedQuery(caseNo, callRecvDate, callSno);
            return View(dTResult);
        }

        //public IActionResult query1(string caseNo = "", string callRecvDate = "", string callSno = "")
        //{
        //    ViewBag.caseNo = caseNo;
        //    ViewBag.callRecvDate = callRecvDate;
        //    ViewBag.callSno = callSno;
        //   // DTResult<Print_Call_letter_Model> dTResult = printcallletterrepository.query1(caseNo, callRecvDate , callSno);
        //    return Json(dTResult);
        //}

        //public IActionResult query2(string caseNo = "", string callRecvDate = "", string callSno = "")
        //{
        //    ViewBag.caseNo = caseNo;
        //    ViewBag.callRecvDate = callRecvDate;
        //    ViewBag.callSno = callSno;
        //    DTResult<Print_Call_letter_Model> dTResult = printcallletterrepository.query2(caseNo, callRecvDate, callSno);
        //    return Json(dTResult);
        //}
    }
}
