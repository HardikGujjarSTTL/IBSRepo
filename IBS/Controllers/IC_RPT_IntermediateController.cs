using IBS.Filters;
using IBS.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    [Authorization]
    public class IC_RPT_IntermediateController : BaseController
    {
        private readonly IInterUnit_TransferRepository interunittransferrepository;
        public IC_RPT_IntermediateController(IInterUnit_TransferRepository _interunittransferrepository)
        {
            interunittransferrepository = _interunittransferrepository;
        }

        [Authorization("IC_RPT_Intermediate", "Index", "view")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
