using IBS.DataAccess;
using IBS.Filters;
using IBS.Interfaces;
using IBS.Interfaces.Vendor;
using IBS.Models;
using IBS.Repositories;
using IBS.Repositories.Vendor;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NuGet.Protocol.Plugins;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace IBS.Controllers
{
    public class ConsigneeComplaintsController : Controller
    {
        private readonly IConsigneeComplaintsRepository consigneeComplaints;
        public ConsigneeComplaintsController(IConsigneeComplaintsRepository _ConsigneeComplaintsRepository)
        {
            consigneeComplaints = _ConsigneeComplaintsRepository;
        }
        [Authorization("ConsigneeComplaints", "Index", "view")]
        public IActionResult Index()
        {
            return View();
        }
        [Authorization("ConsigneeComplaints", "Index", "view")]
        public IActionResult Manage(string CASE_NO, string BK_NO,string SET_NO)
        {
            ConsigneeComplaints model = new();

            try
            {
                if (CASE_NO != null && BK_NO != null && SET_NO != null)
                {
                    model = consigneeComplaints.FindByID(CASE_NO, BK_NO, SET_NO);
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            return View(model);
        }


        [HttpPost]
        public ActionResult GetCombinedData(string poNo, string poDt)
        {
            List<ConsigneeComplaints> dTResult = consigneeComplaints.GetDataListConsignee(poNo, poDt);
            List<ConsigneeComplaints> dTResult1 = consigneeComplaints.GetDataListComplaint(poNo, poDt);
            return Json(new { dTResult = dTResult, dTResult1 = dTResult1 });
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult ComplaintsDetailsSave(ConsigneeComplaints model)
        //{
        //    try
        //    {
        //        string msg = "Complaints Inserted Successfully.";

        //        //if (model.ComplaintId > 0)
        //        //{
        //        //    msg = "Complaints Updated Successfully.";
        //        //}
        //        int i = consigneeComplaints.ComplaintsDetailsInsertUpdate(model);
        //        if (i > 0)
        //        {
        //            return Json(new { status = true, responseText = msg });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //Common.AddException(ex.ToString(), ex.Message.ToString(), "Role", "RoleDetailsSave", 1, GetIPAddress());
        //    }
        //    return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        //}
    }
}
