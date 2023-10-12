using IBS.Filters;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    [Authorization]
    public class IC_RPT_IntermediateController : BaseController
    {
        #region Varible
        private readonly IIC_RPT_IntermediateRepository iC_RPT_IntermediateRepository;
        public IC_RPT_IntermediateController(IIC_RPT_IntermediateRepository _iC_RPT_IntermediateRepository)
        {
            iC_RPT_IntermediateRepository = _iC_RPT_IntermediateRepository;
        }
        #endregion

        [Authorization("IC_RPT_Intermediate", "Index", "view")]
        public IActionResult Index()
        {
            IC_RPT_IntermediateModel model = new();
            //model.CASE_NO = "N22061024";
            //model.Call_Recv_dt = "23/08/2022";
            //model.Call_SNO = "99";
            //model.CONSIGNEE_CD = "3895";
            //model.ACTIONAR = "A";

            model.CASE_NO = "N21111089";
            model.Call_Recv_dt = Convert.ToDateTime("13/08/2022");
            model.Call_SNO = "3";
            model.CONSIGNEE_CD = "39";
            model.ACTIONAR = "A";


            model = iC_RPT_IntermediateRepository.GetDetails(model.CASE_NO, model.Display_Call_Recv_dt, model.Call_SNO, model.ITEM_SRNO_PO, model.CONSIGNEE_CD);

            model.Region = Region;
            return View(model);
        }

        public IActionResult FillItems()
        {
            IC_RPT_IntermediateModel model = new();

            return View("Index", model);
        }
    }
}
