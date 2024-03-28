using IBS.Filters;
using IBS.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    [Authorization]
    public class ReceiptsRemitanceController : BaseController
    {
        #region Variables
        private readonly IReceiptsRemitanceRepository ReceiptsRemitanceRepository;
        private readonly IWebHostEnvironment env;
        private readonly IConfiguration config;
        #endregion
        public ReceiptsRemitanceController(IReceiptsRemitanceRepository _ReceiptsRemitanceRepository, IWebHostEnvironment _env, IConfiguration _config)
        {
            ReceiptsRemitanceRepository = _ReceiptsRemitanceRepository;
            this.env = _env;
            config = _config;
        }

        public IActionResult Index()
        {
            ViewBag.ReportUrl = config.GetSection("AppSettings")["ReportUrl"];
            ViewBag.Region = Region;
            return View();
        }

    }
}
