using IBS.Filters;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    [Authorization]
    public class HologramSearchFormController : BaseController
    {
        #region Variables
        private readonly IHologramSearchForm hologramSearchForm;
        #endregion
        public HologramSearchFormController(IHologramSearchForm _hologramSearchForm)
        {
            hologramSearchForm = _hologramSearchForm;
        }
        [Authorization("HologramSearchForm", "Index", "view")]
        public IActionResult Index()
        {
            var region = GetUserInfo.Region;
            ViewBag.Region = region;
            ViewBag.Role = GetUserInfo.RoleName;
            return View();
        }

        [Authorization("HologramSearchForm", "Index", "view")]
        public IActionResult Manage(string Hg_No_Fr, string Hg_No_To)
        {
            HologramSearchFormModel model = new();
            ViewBag.Role = GetUserInfo.RoleName;
            ViewBag.Region = GetUserInfo.Region;
            if (!string.IsNullOrEmpty(Hg_No_Fr) && !string.IsNullOrEmpty(Hg_No_To))
            {
                Hg_No_Fr = Hg_No_Fr.Substring(1, 7);
                Hg_No_To = Hg_No_To.Substring(1, 7);
                model = hologramSearchForm.FindByID(Hg_No_Fr, Hg_No_To, GetUserInfo.Region);
            }
            model.Region = Convert.ToString(GetUserInfo.Region);
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            var region = GetUserInfo.Region;
            DTResult<HologramSearchFormModel> dTResult = hologramSearchForm.GetHologramSearchFormList(dtParameters, region);
            return Json(dTResult);
        }

        [Authorization("HologramSearchForm", "Index", "delete")]
        public IActionResult Delete(string Hg_No_Fr, string Hg_No_To)
        {
            try
            {
                var model = new HologramSearchFormModel();
                model.HgNoFr = Hg_No_Fr.Substring(1, 7);
                model.HgNoTo = Hg_No_To.Substring(1, 7);
                model.HgRegion = GetUserInfo.Region;
                model.Updatedby = UserId;
                if (hologramSearchForm.Remove(model))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "HologramSearchForm", "Delete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorization("HologramSearchForm", "Index", "edit")]
        public IActionResult Manage(HologramSearchFormModel model)
        {
            try
            {
                ViewBag.Role = GetUserInfo.RoleName;
                ViewBag.Region = GetUserInfo.Region;
                model.HgRegion = Region;
                var chkDate = hologramSearchForm.CheckDate(Convert.ToString(model.HgIssueDt));
                if (chkDate == 1)
                {
                    var chkHNo = hologramSearchForm.CheckHologramNo(model);
                    if (chkHNo <= 0)
                    {
                        if (model.lblHgNoFr == null && model.lblHgNoTo == null)
                        {
                            var status = hologramSearchForm.IEIssueOrNot(Convert.ToString(model.HgIecd));
                            if (status == "W")
                            {
                                model.Createdby = GetUserInfo.UserID;
                                model.UserId = GetUserInfo.UserName;
                                int result = hologramSearchForm.SaveDetails(model);
                                if (result > 0)
                                {
                                    return Json(new { status = true, responseText = "Search of Hologram to IE Inserted Successfully." });
                                }
                                else
                                {
                                    AlertDanger();
                                }
                            }
                            else
                            {
                                return Json(new { status = false, responseText = "You Cannot Issue a New HOLOGRAM To a Retired or Left IE!!!" });
                            }
                        }
                        else
                        {
                            //var res = hologramSearchForm.CheckHologramCancel(model);
                            //if (res == 0)
                            //{
                            model.UserId = GetUserInfo.UserName;
                            model.Updatedby = GetUserInfo.UserID;
                            int result = hologramSearchForm.SaveDetails(model);
                            if (result > 0)
                            {
                                return Json(new { status = true, responseText = "Search of Hologram to IE Updated Successfully." });
                            }
                            else
                            {
                                AlertDanger();
                            }
                            //}
                            //else
                            //{
                            //    return Json(new { status = true, responseText = "The  Hologram No. is  Cancelled/Destroyed between the Range of Holgram No. From and Hologram No. To, soo u cannot modify it." });
                            //}
                        }
                    }
                    else
                    {
                        return Json(new { status = false, responseText = "Range of Entered Hologram No. From to Hologram No. To Already Present in Database!!!" });
                    }
                }
                else
                {
                    return Json(new { status = false, responseText = "The Date Of Issue To IE Cannot be greater then todays date" });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "HologramSearchForm", "Manage", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpPost]
        public IActionResult MatchHologram(string Hg_No_Fr, string Hg_No_To)
        {
            string errorMsg = "";
            Hg_No_Fr = Hg_No_Fr.Substring(1, 7);
            Hg_No_To = Hg_No_To.Substring(1, 7);
            var res = hologramSearchForm.MatchHologram(Hg_No_Fr, Hg_No_To, GetUserInfo.Region);
            if (res == 0)
            {
                errorMsg = "No Record Found of Entered Hologram No From. and Hologram No.To!!!";
                //return Json(new { status = false, responseText =  });
            }
            else if (res == 1)
            {
                errorMsg = "You Are Not Authorised to Update/Delete Hologram data Issued to Other Regions!!!";
                //return Json(new { status = false, responseText = "You Are Not Authorised to Update/Delete Hologram data Issued to Other Regions!!!" });
            }
            return Json(errorMsg);
        }
    }
}
