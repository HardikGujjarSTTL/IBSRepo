using IBS.Filters;
using IBS.Interfaces.Reports;
using IBS.Repositories.Reports;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers.Reports
{
    [Authorization]
    public class MonthlyReportsController : BaseController
    {
        private readonly IMonthlyReportsRepository monthlyReportsRepository;
        private readonly IWebHostEnvironment env;

        public MonthlyReportsController(IMonthlyReportsRepository _monthlyReportsRepository, IWebHostEnvironment _env)
        {
            monthlyReportsRepository = _monthlyReportsRepository;
            this.env = _env;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
