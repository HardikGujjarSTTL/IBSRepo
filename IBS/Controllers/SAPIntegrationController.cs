using IBS.Interfaces;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost]
        public IActionResult ExportExcelBPO(string BPO_Cd)
        {
            sapIntegrationRepository.ExportExcelBPO(BPO_Cd);
            return View();
        }

    }
}
