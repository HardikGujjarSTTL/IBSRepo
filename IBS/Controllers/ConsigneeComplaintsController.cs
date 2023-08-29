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
    public class ConsigneeComplaintsController : BaseController
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
        public IActionResult Manage(string CASE_NO, string BK_NO,string SET_NO,string ComplaintId)
        {
            ConsigneeComplaints model = new();

            try
            {
                if(ComplaintId != "" && ComplaintId != null)
                {
                    model = consigneeComplaints.FindByCompID(ComplaintId);
                    ViewBag.Showcomplaint = true;
                }
                if (CASE_NO != null && BK_NO != null && SET_NO != null)
                {
                    model = consigneeComplaints.FindByID(CASE_NO, BK_NO, SET_NO);
                    ViewBag.Showcomplaint = false;
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult GetConsData([FromBody] DTParameters dtParameters)
        {
            DTResult<ConsigneeComplaints> dTResult = consigneeComplaints.GetDataListConsignee(dtParameters);
            return Json(dTResult);
        }
        [HttpPost]
        public IActionResult GetCompdata([FromBody] DTParameters dtParameters)
        {
            DTResult<ConsigneeComplaints> dTResult = consigneeComplaints.GetDataListComplaint(dtParameters);
            return Json(dTResult);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorization("ConsigneeComplaints", "Index", "edit")]
        public IActionResult ComplaintsDetailsSave(ConsigneeComplaints model)
        {
            try
            {
                string msg = "Complaints Inserted Successfully.";

                if (model.ComplaintId != null && model.ComplaintId != "")
                {
                    msg = "Complaints Updated Successfully.";
                }
                string i = consigneeComplaints.ComplaintsDetailsInsertUpdate(model);
                if (i != "")
                {
                    return Json(new { status = true, responseText = msg, redirectToIndex = true, alertMessage = msg });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "ConsigneeComplaints", "ComplaintsDetailsSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorization("ConsigneeComplaints", "Index", "edit")]
        public IActionResult ComplaintSaveChoice(ConsigneeComplaints model)
        {
            try
            {
                string msg = "";

                if (model.ComplaintId != null && model.ComplaintId != "")
                {
                    msg = "Data Updated Successfully.";
                }
                string i = consigneeComplaints.JIChoice(model);
                if (i != "")
                {
                    return Json(new { status = true, responseText = msg, redirectToIndex = true, alertMessage = msg });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "ConsigneeComplaints", "ComplaintSaveChoice", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorization("ConsigneeComplaints", "Index", "edit")]
        public IActionResult ComplaintCancelJI(ConsigneeComplaints model)
        {
            try
            {
                string msg = "";

                if (model.ComplaintId != null && model.ComplaintId != "")
                {
                    msg = "JI Cancel Successfully.";
                }
                string i = consigneeComplaints.CancelJI(model);
                if (i != "")
                {
                    return Json(new { status = true, responseText = msg, redirectToIndex = true, alertMessage = msg });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "ConsigneeComplaints", "ComplaintCancelJI", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorization("ConsigneeComplaints", "Index", "edit")]
        public IActionResult JIOutCome(ConsigneeComplaints model) 
        {
            try
            {
                string msg = "";

                if (model.ComplaintId != null && model.ComplaintId != "")
                {
                    msg = "Data Save Successfully.";
                }
                string i = consigneeComplaints.JIOutCome(model);
                if (i != "")
                {
                    return Json(new { status = true, responseText = msg, redirectToIndex = true, alertMessage = msg });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "ConsigneeComplaints", "ComplaintCancelJI", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
    }
}
