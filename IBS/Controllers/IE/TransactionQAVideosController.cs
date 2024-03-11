using IBS.Interfaces.IE;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers.IE
{
    public class TransactionQAVideosController : BaseController
    {
        #region Variables
        private readonly ITransactionQAVideosRepository videoRepository;
        #endregion

        public TransactionQAVideosController(ITransactionQAVideosRepository _videoRepository)
        {
            videoRepository = _videoRepository;
        }

        public IActionResult QAVideoGeneral()
        {
            return View();
        }

        public IActionResult QAVideoMechanical()
        {
            return View();
        }

        public IActionResult QAVideoEletrical()
        {
            return View();
        }

        public IActionResult QAVideoCivil()
        {
            return View();
        }
    }
}
