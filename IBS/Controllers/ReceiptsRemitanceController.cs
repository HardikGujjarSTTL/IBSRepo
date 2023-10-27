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
