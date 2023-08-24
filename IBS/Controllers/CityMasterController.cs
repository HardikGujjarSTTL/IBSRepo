using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class CityMasterController : BaseController
    {
        private readonly ICityRepository cityRepository;

        public CityMasterController(ICityRepository _cityRepository)
        {
            cityRepository = _cityRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Manage(int id)
        {
            CityMasterModel model = new();
            if (id > 0)
            {
                model = cityRepository.FindByID(id);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<CityMasterModel> dTResult = cityRepository.GetCityMasterList(dtParameters);
            return Json(dTResult);
        }

        [HttpPost]
        public IActionResult Manage(CityMasterModel model)
        {
            try
            {
                if (model.CityCd == 0)
                {
                    model.Createdby = UserId;
                    model.UserId = USER_ID.Substring(0, 8);
                    cityRepository.SaveDetails(model);
                    AlertAddSuccess("Record Added Successfully.");
                }
                else
                {
                    model.Updatedby = UserId;
                    model.UserId = USER_ID.Substring(0, 8);
                    cityRepository.SaveDetails(model);
                    AlertAddSuccess("Record Updated Successfully.");
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "CityMaster", "Manage", 1, GetIPAddress());
            }
            return View(model);
        }

        public IActionResult Delete(int id)
        {
            try
            {
                if (cityRepository.Remove(id))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "CityMaster", "Delete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
        }

    }
}
