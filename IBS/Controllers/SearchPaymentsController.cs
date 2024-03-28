using IBS.Filters;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class SearchPaymentsController : BaseController
    {
        private readonly ISearchPaymentsRepository SearchPaymentsRepository;
        public SearchPaymentsController(ISearchPaymentsRepository _SearchPaymentsRepository)
        {
            SearchPaymentsRepository = _SearchPaymentsRepository;
        }

        [HttpPost]
        [Authorization("SearchPayments", "SearchPayment", "view")]

        [HttpPost]
        public IActionResult PaymentList([FromBody] DTParameters dtParameters)
        //public IActionResult PaymentList(string AMOUNT , string CASE_NO , string CHQ_NO , string BANK_NAME, string NARRATION, string CHQ_DT, string ACC_CD)
        {
            //DTParameters dtParameters = new DTParameters();
            //var Region = Region;
            //int Role = UserId;
            DTResult<SearchPaymentsModel> dTResult = SearchPaymentsRepository.GetSearchPayment(dtParameters, Region, UserId);
            //DTResult<SearchPaymentsModel> dTResult = SearchPaymentsRepository.GetSearchPayment(dtParameters, AMOUNT , CASE_NO , CHQ_NO, BANK_NAME, NARRATION , CHQ_DT, ACC_CD , region ,Role);
            return Json(dTResult);
        }

        [Authorization("SearchPayments", "SearchPayment", "view")]

        public IActionResult UpdatePayment()
        {
            return View();
        }

        [Authorization("SearchPayments", "SearchPayment", "view")]

        public IActionResult SearchPayment()
        {
            return View();
        }
    }
}
