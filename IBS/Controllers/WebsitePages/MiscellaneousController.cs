using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class MiscellaneousController : Controller
    {
        public IActionResult InfoInstructions()
        {
            return View();
        }
        public IActionResult PhotoGallery()
        {
            return View();
        }
        public IActionResult QaChannelYouTube()
        {
            return View();
        }
    }
}
