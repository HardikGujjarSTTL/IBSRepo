using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;


namespace IBS.Controllers
{
	public class RailwaysDirectoryController : BaseController
	{
		private readonly IRailwaysDirectory railwaysDirectory;
        public RailwaysDirectoryController(IRailwaysDirectory _railwaysDirectory)
        {
            railwaysDirectory = _railwaysDirectory;
		}
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Manage(int id)
        {
            RailwaysDirectoryModel model = new();
            if (id > 0)
            {
                model = railwaysDirectory.FindByID(id);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<RailwaysDirectoryModel> dTResult = railwaysDirectory.GetRMList(dtParameters);
            return Json(dTResult);
        }
        public IActionResult Delete(int id)
        {
            try
            {
                if (railwaysDirectory.Remove(id, UserId))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "RailwaysDirectory", "Delete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RailwaysDirectoryDetailsSave(RailwaysDirectoryModel model)
        {
            try
            {
                string msg = "RailwaysDirectory Inserted Successfully.";

                if (model.Id > 0)
                {
                    msg = "RailwaysDirectory Updated Successfully.";
                    model.Updatedby = UserId;
                }
                model.Createdby = UserId;
                int i = railwaysDirectory.RailwaysDirectoryDetailsInsertUpdate(model);
                if (i > 0)
                {
                    return Json(new { status = true, responseText = msg });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "RailwaysDirectory", "RailwaysDirectoryDetailsSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

    }
}



