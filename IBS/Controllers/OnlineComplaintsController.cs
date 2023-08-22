using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IBS.Controllers
{
    public class OnlineComplaintsController : BaseController
    {
        private readonly IOnlineComplaintsRepository _onlineComplaintsRepository;

        public OnlineComplaintsController(IOnlineComplaintsRepository onlineComplaintsRepository)
        {
            _onlineComplaintsRepository = onlineComplaintsRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        public ActionResult GetItems(string ItemSno, string bkno,string setno,string InspRegionDropdown)
        {
            var json = _onlineComplaintsRepository.GetItems(ItemSno,bkno,setno, InspRegionDropdown);

            return Json(json);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ComplaintsSave(OnlineComplaints onlineComplaints, IFormFile complaintFile)
        {
            string msg = "";
            try
            {
                 msg = _onlineComplaintsRepository.SaveComplaints(onlineComplaints,complaintFile);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "OnlineComplaints", "ComplaintsSave", 1, GetIPAddress());
            }
            return Json(new { status = true, responseText = msg });
        }
    }
}
