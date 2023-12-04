using IBS.Interfaces.IE;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers.IE
{
    public class Quality_Manual_ProceduresController : BaseController
    {
        #region Variables
        private readonly IQuality_Manual_ProceduresRepository qaRepository;
        #endregion

        public Quality_Manual_ProceduresController(IQuality_Manual_ProceduresRepository _qaRepository)
        {
            qaRepository = _qaRepository;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
