using IBS.Helper;
using IBS.Interfaces;
using IBS.Interfaces.Vendor;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;

namespace IBS.Controllers.Vendor
{
    public class VendorCallRegisterController : BaseController
    {
        #region Variables
        private readonly IVendorCallRegisterRepository venderRepository;
        SessionHelper objSessionHelper = new SessionHelper();
        #endregion
        public VendorCallRegisterController(IVendorCallRegisterRepository _venderRepository)
        {
            venderRepository = _venderRepository;
        }

        public IActionResult VendorCallRegister()
        {
            VenderCallRegisterModel model = new();
            //model = venderRepository.FindByID(UserId);
            model.CDATE = DateTime.Now.ToString("dd-MM-yyyy");
            model.CDAY = DateTime.Now.DayOfWeek.ToString("D");
            model.CTYM = DateTime.Now.ToString("HH24MI");

            if (model.CDAY == "1")
            {
                model.CDATE = DateTime.Now.AddDays(1).ToString("dd-MM-yyyy");
            }
            if (model.CaseNo == null || model.CallRecvDt == null)
            {
                if (model.CDATE == "27-01-2023")
                {
                    model.CallRecvDt = Convert.ToDateTime("27-01-2023");
                }
                else if (model.CDATE == "15-08-2023")
                {
                    model.CallRecvDt = Convert.ToDateTime("16-08-2023");
                }
                else if (model.CDATE == "02-10-2023")
                {
                    model.CallRecvDt = Convert.ToDateTime("03-10-2023");
                }
                else
                {
                    model.CallRecvDt = Convert.ToDateTime(model.CDATE);
                }
            }
            else
            {

            }

            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<VenderCallRegisterModel> dTResult = venderRepository.GetUserList(dtParameters, UserName);
            return Json(dTResult);
        }

        public IActionResult VendorCallRegisterDetail(string Action, string CaseNo, string CallRecvDt, int CallSno)
        {
            VenderCallRegisterModel model = new();

            if (CaseNo != null && CallRecvDt != null && CallSno != null)
            {
                model = venderRepository.FindByID(CaseNo, CallRecvDt, CallSno, UserName);
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTableList([FromBody] DTParameters dtParameters)
        {
            DTResult<VenderCallRegisterModel> dTResult = venderRepository.GetVenderList(dtParameters, UserName);
            return Json(dTResult);
        }

        [HttpPost]
        public IActionResult SearchMenufacturer(int id, int MfgCd)
        {
            VenderCallRegisterModel model = new();

            if (MfgCd > 0)
            {
                model = venderRepository.FindByVenderDetail(MfgCd);
            }

            return View(model);
            //return RedirectToAction("VendorCallRegister");
        }

        [HttpPost]
        public IActionResult CheckSameVendor(int id)
        {
            VenderCallRegisterModel model = new();
            int MfgCd = Convert.ToInt32(UserName);
            if (MfgCd > 0)
            {
                model = venderRepository.FindByVenderDetail(MfgCd);
            }

            return View(model);
            //return RedirectToAction("VendorCallRegister");
        }

        public IActionResult GetVendorDetails(int id)
        {
            int MfgCd = Convert.ToInt32(UserName);

            DTResult<VenderCallRegisterModel> dTResult = venderRepository.FindByVenderDetail1(MfgCd);
            return Json(dTResult);
        }

        public IActionResult GetVendorDetailsClick(int id)
        {
            int MfgCd = id;

            DTResult<VenderCallRegisterModel> dTResult = venderRepository.FindByVenderDetail1(MfgCd);
            return Json(dTResult);
        }

        public IActionResult BindVendor(int id)
        {
            return Json(Common.GetVendor_City(id));
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult VoucherDetailsSave(AddRecieptVoucherModel model)
        //{
        //    try
        //    {
        //        string msg = "Voucher Inserted Successfully.";

        //        if (model.VCHR_NO != "")
        //        {
        //            msg = "Voucher Updated Successfully.";

        //        }
        //        int i = addVoucherRepository.VoucherDetailsSave(model, GetUserInfo.Region.ToString());
        //        if (i > 0)
        //        {
        //            return Json(new { status = true, responseText = msg });
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //         Common.AddException(ex.ToString(), ex.Message.ToString(), "VendorCallRegister", "VendorCallRegisterUpdate", 1, GetIPAddress());
        //    }
        //    return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        //}

        [HttpPost]
        public IActionResult UpdateDetails(VenderCallRegisterModel model)
        {
            try
            {
                string msg = "Message Inserted Successfully.";

                if (model.MfgCd > 0)
                {
                    msg = "Message Updated Successfully.";
                    //model.Updatedby = Convert.ToString(UserId);
                }
                //model.Createdby = Convert.ToString(UserId);

                int i = venderRepository.DetailsInsertUpdate(model);
                if (i > 0)
                {
                    return Json(new { status = true, responseText = msg, Id = i });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Update Manufacturer Details", "VendorCallRegister", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DetailsSave(VenderCallRegisterModel model)
        {
            try
            {
                string msg = "Message Inserted Successfully.";
                //if (model.MfgCd > 0)
                //{
                //    msg = "Message Updated Successfully.";
                //}

                int i = venderRepository.RegiserCallSave(model);
                if (i > 0)
                {
                    return Json(new { status = true, responseText = msg, Id = i });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "VenderCallRegisterModel", "DetailsSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        //[HttpPost]
        //public IActionResult VendorCallRegisterDetail([FromBody] VenderCallRegisterModel model)
        //{
        //    try
        //    {
        //        var entity = context.CallRegisterEntities.FirstOrDefault(e => e.CaseNumber == model.CaseNumber && e.CallReceiveDate == model.CallDate && e.CallSerialNumber == model.CallSerialNumber);

        //        if (entity == null)
        //        {
        //            // Handle the case when the record is not found
        //            return NotFound();
        //        }

        //        // Update the entity with the new values from the model
        //        entity.CallLetterNumber = model.CallNumber;
        //        entity.CallLetterDate = model.CallDate;
        //        entity.MarkDate = model.MarkDate;
        //        entity.InspectionDesireDate = model.InspectionDesireDate;
        //        entity.CallStatusDate = model.CallStatusDate;
        //        entity.CallRemarkStatus = model.CallRemarkStatus;
        //        entity.CallInstallNumber = model.CallInstallNumber;
        //        entity.Remarks = model.Remarks;
        //        entity.ManufacturerCode = model.ManufacturerCode;
        //        entity.ManufacturerPlace = model.ManufacturerPlace;
        //        entity.UserID = model.UserID;
        //        entity.DateTime = DateTime.Now;

        //        // Save changes to the database
        //        dbContext.SaveChanges();

        //        // Return a success response
        //        return Ok("Your Record Has Been Saved!!!");
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle exceptions, log the error, etc.
        //        return StatusCode(500, "An error occurred while updating the record.");
        //    }
        //}
    }
}
