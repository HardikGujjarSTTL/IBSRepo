using IBS.Filters;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
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
       

        public IActionResult PaymentList(string AMOUNT , string CASE_NO , string CHQ_NO , string BankNameDropdown , string NARRATION, string VCHR_DT, string ACC_CD)
            {
            DTParameters dtParameters = new DTParameters();
            var region = GetRegionCode;
            int Role = UserId;
           
             DTResult<SearchPaymentsModel> dTResult = SearchPaymentsRepository.GetSearchPayment(dtParameters, AMOUNT , CASE_NO , CHQ_NO, BankNameDropdown, NARRATION , VCHR_DT , ACC_CD , region ,Role);
            return Json(dTResult);
        }

        

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
