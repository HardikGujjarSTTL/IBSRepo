using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class Print_Bank_Statement_VoucherController : BaseController
    {
        private readonly IPrint_Bank_Statement_VoucherRepository printbankstatementvoucherrepository;

        public Print_Bank_Statement_VoucherController(IPrint_Bank_Statement_VoucherRepository _printbankstatementvoucherrepository)
        {
            printbankstatementvoucherrepository = _printbankstatementvoucherrepository;
        }


        public IActionResult Index(string VCHR_NO = "", string VCHR_DT = "")
        {
            string region = GetRegionCode;
            string Region = "";
            if (region == "N")
            {
                Region = "Northern Region";
            }
            else if (region == "S")
            {
                Region = "Southern Region";
            }
            else if (region == "E")
            {
                Region = "Eastern Region";
            }
            else if (region == "W")
            {
                Region = "Western Region";
            }
            else if (region == "C")
            {
                Region = "Central Region";
            }
            ViewBag.Region = Region;
            ViewBag.VCHR_NO = VCHR_NO;
            ViewBag.VCHR_DT = VCHR_DT;
            Print_Bank_Statement_VoucherModel model = printbankstatementvoucherrepository.ReportData(VCHR_NO, Region);
            return View(model);

        }
    }
}
