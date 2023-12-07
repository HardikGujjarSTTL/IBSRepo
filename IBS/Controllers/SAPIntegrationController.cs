using IBS.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace IBS.Controllers
{
    public class SAPIntegrationController : BaseController
    {
        private readonly ISAPIntegrationRepository sapIntegrationRepository;

        public SAPIntegrationController(ISAPIntegrationRepository _sapIntegrationRepository)
        {
            sapIntegrationRepository = _sapIntegrationRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ExportExcelBPO(string BPO_Cd)
        {
            DataSet ds = sapIntegrationRepository.ExportExcelBPO(BPO_Cd);

            return File(Helpers.CreateExcelFile.ExportToExcelDownload(ds), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "YourFileName.xls");

        }

    }
}
