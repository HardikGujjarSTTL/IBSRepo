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
            var region = GetUserInfo.Region;
            ViewBag.Region = region;
            return View();
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            var region = GetUserInfo.Region;
            DTResult<Search> dTResult = searchRepository.GetSearchList(dtParameters, region);
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

        [HttpPost]
        public JsonResult GetVendorData(string Prefix)
        {
            List<VendorCls> dTResult = searchRepository.GetVendorList(Prefix);
            return Json(dTResult);
        }
    }
}
