using IBS.DataAccess;
using IBS.Filters;
using IBS.Interfaces;
using IBS.Models;
using IBS.Models.Reports;
using IBS.Repositories;
using IBS.Repositories.Reports;
using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp.Media;
using PuppeteerSharp;
using System;
using System.Drawing;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IBS.Controllers
{
    [Authorization]
    public class ReceiptsRemitanceController : BaseController
    {
        #region Variables
        private readonly IReceiptsRemitanceRepository ReceiptsRemitanceRepository;
        private readonly IWebHostEnvironment env;
        #endregion
        public ReceiptsRemitanceController(IReceiptsRemitanceRepository _ReceiptsRemitanceRepository, IWebHostEnvironment _env)
        {
            ReceiptsRemitanceRepository = _ReceiptsRemitanceRepository;
            this.env = _env;
        }

        public IActionResult Index()
        {
            ViewBag.Region = Region;
            return View();
        }
        
    }
}
