using IBS.Helper;
using IBS.Interfaces.Reports;
using IBS.Models.Reports;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;
using IBS.Filters;
using IBS.Interfaces;

namespace IBS.Controllers
{
    public class WriteOffEntryController : BaseController
    {
        private readonly IWriteOffEntryRepository writeOffEntryRepository;

        public WriteOffEntryController(IWriteOffEntryRepository _writeOffEntryRepository)
        {
            writeOffEntryRepository = _writeOffEntryRepository;
        }

        [Authorization("IEICPhotoEnclosedReport", "Index", "view")]
        public IActionResult Index()
        {
            ViewBag.Region = Region;
            return View();
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            string Region = SessionHelper.UserModelDTO.Region;
            DTResult<WriteOffEntryModel> dTResult = writeOffEntryRepository.GetWriteOfEntryList(dtParameters);
            return Json(dTResult);
        }
    }
}
