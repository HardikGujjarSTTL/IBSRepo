﻿using IBS.Filters;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using IBS.Models.Reports;
using IBS.Repositories.Reports;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class ConsigneeCompPeriodController : BaseController
    {
        private readonly IConsigneeCompPeriodRepository consigneeCompPeriodRepository;
        public ConsigneeCompPeriodController(IConsigneeCompPeriodRepository _consigneeCompPeriodRepository)
        {
            consigneeCompPeriodRepository = _consigneeCompPeriodRepository;
        }
        [Authorization("ConsigneeCompPeriod", "Index", "view")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Manage(string FromDate, string ToDate, string Allregion, string regionorth, string regionsouth, string regioneast, string regionwest, string jiallregion,
            string jinorth, string jisourth, string jieast,string jiwest, string compallregion, string compyes, string compno, string cancelled, string underconsider, string allaction, string particilaraction, string actiondrp)
        {
            ConsigneeCompPeriodReport model = new()
            {
                FromDate = FromDate,
                ToDate = ToDate,
                Allregion = Allregion,
                regionorth = regionorth,
                regionsouth = regionsouth,
                regioneast = regioneast,
                regionwest = regionwest,
                jiallregion = jiallregion,
                jinorth = jinorth,
                jisourth = jisourth,
                jieast = jieast,
                jiwest = jiwest,
                compallregion = compallregion,
                compyes = compyes,
                compno = compno,
                cancelled = cancelled,
                underconsider = underconsider,
                allaction = allaction,
                particilaraction = particilaraction,
                actiondrp = actiondrp
            };
           model.ReportTitle = "Consignee Complaints";
            return View(model);
        }

        public IActionResult ComplaintsByPeriod(string FromDate, string ToDate, string Allregion, string regionorth, string regionsouth, string regioneast, string regionwest, string jiallregion,
            string jinorth, string jisourth, string jieast, string jiwest, string compallregion, string compyes, string compno, string cancelled, string underconsider, string allaction, string particilaraction, string actiondrp,
            string particilarcode, string particilarjicode, string actioncodedrp, string actionjidrp)
        {
            string region = "", jirequired = "";
            ConsigneeCompPeriodReport model = consigneeCompPeriodRepository.GetCompPeriodData(FromDate, ToDate, Allregion, regionorth, regionsouth, regioneast, regionwest, jiallregion,
             jinorth, jisourth, jieast, jiwest, compallregion, compyes, compno, cancelled, underconsider, allaction, particilaraction, actiondrp, actioncodedrp, actionjidrp);

            region = (Allregion == "true") ? "AllRegion" :
                     (regionorth == "true") ? "Northern Region" :
                     (regionsouth == "true") ? "Southern Region" :
                     (regioneast == "true") ? "Eastern Region" :
                     (regionwest == "true") ? "Western Region" :
                     "";

            jirequired = (compallregion == "true") ? "" :
                      (compyes == "true") ? "& JI Required" :
                      (compno == "true") ? "& JI Not Required" :
                      (cancelled == "true") ? " & JI Cancelled" :
                      (underconsider == "true") ? "& JI Under Consideration" :
                      "";

            ViewBag.Regions = region;
            ViewBag.JiRequiredStatus = jirequired;
            return PartialView(model);
        }
    }
}
