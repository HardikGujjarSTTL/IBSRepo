using IBS.Filters;
using IBS.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class ComplaintsJIRequiredReportController : BaseController
    {
        #region Variables
        private readonly IComplaintsJIRequiredReportRepository complaintsJIRequiredReportRepository;
        #endregion

        public ComplaintsJIRequiredReportController(IComplaintsJIRequiredReportRepository _complaintsJIRequiredReportRepository)
        {
            complaintsJIRequiredReportRepository = _complaintsJIRequiredReportRepository;
        }
        [Authorization("ComplaintsJIRequiredReport", "Index", "view")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
