using IBS.Filters;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class SearchController : BaseController
    {
        #region Variables
        private readonly ISearchRepository searchRepository;
        #endregion
        public SearchController(ISearchRepository _searchRepository)
        {
            searchRepository = _searchRepository;
        }
        [Authorization("Search", "Index", "view")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<Search> dTResult = searchRepository.GetSearchList(dtParameters);
            return Json(dTResult);
        }

        [HttpPost]
        public JsonResult GetBPOData(string Prefix)
        {
            List<BPOdata> dTResult = searchRepository.GetBPOList(Prefix);
            return Json(dTResult);
        }

        [HttpPost]
        public JsonResult GetConsigneeData(string Prefix)
        {
            List<Consignee> dTResult = searchRepository.GetConsigneeList(Prefix);
            return Json(dTResult);
        }
    }
}
