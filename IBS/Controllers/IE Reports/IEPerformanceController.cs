using IBS.Interfaces.IE;
using IBS.Interfaces.IE_Reports;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;

namespace IBS.Controllers.IE_Reports
{
    public class IEPerformanceController : BaseController
    {
        #region Variables
        private readonly IIE_PerfomanceRepository iE_PerfomanceRepository;
        #endregion

        public IEPerformanceController(IIE_PerfomanceRepository _iE_PerfomanceRepository)
        {
            iE_PerfomanceRepository = _iE_PerfomanceRepository;
        }

        // GET: IEPerformanceController
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            var dTResult = new DTResult<IE_PerformanceModel>();
            var res = new IEPerformanceSummary();           
            try
            {
                var obj = new IEPerformanceFilter();
                obj.Region = Convert.ToString(GetUserInfo.Region);
                obj.UserName = Convert.ToString(GetUserInfo.UserName);
                obj.IE_CD = Convert.ToString(GetUserInfo.IeCd);
                if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["FromDate"]) || !string.IsNullOrEmpty(dtParameters.AdditionalValues["ToDate"]))
                {
                    //if (dtParameters.Start <= 0)
                    //{
                    //    res = iE_PerfomanceRepository.Get_IE_Performance_Summary(dtParameters, obj);
                    //}
                    dTResult = iE_PerfomanceRepository.Get_IE_Performance(dtParameters, obj);
                }
                else
                {
                    var list = new List<IE_PerformanceModel>();
                    dTResult.data = list;
                    dTResult.draw = 1;
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "HologramAccountal", "LoadTable", 1, GetIPAddress());
            }

            return Json(dTResult);
            //return Json(new
            //{
            //    _dTResult = dTResult
            //    //_rejection = res.Rejection,
            //    //_NoOfIcs = res.NoOfIcs,
            //    //_CallsWithin = res.CallsWithin,
            //    //_CallsBeyond = res.CallsBeyond,
            //    //_summaryList = res.IEPerSumFooter
            //});
        }

        [HttpPost]
        public IActionResult Get_IE_Performance_Summary(IEFromToDate model)
        {
            var res = new IEPerformanceSummary();            
            try
            {
                var obj = new IEPerformanceFilter();
                obj.Region = Convert.ToString(GetUserInfo.Region);
                obj.UserName = Convert.ToString(GetUserInfo.UserName);
                obj.IE_CD = Convert.ToString(GetUserInfo.IeCd);

                //res = iE_PerfomanceRepository.Get_IE_Performance_Summary(dtParameters, obj);
                res = iE_PerfomanceRepository.Get_IE_Performance_Summary(model, obj);

            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "HologramAccountal", "LoadTable", 1, GetIPAddress());
            }

            return Json(new
            {                
                _rejection = res.Rejection,
                _NoOfIcs = res.NoOfIcs,
                _CallsWithin = res.CallsWithin,
                _CallsBeyond = res.CallsBeyond,
                _summaryList = res.IEPerSumFooter
            });
        }
    }
}
