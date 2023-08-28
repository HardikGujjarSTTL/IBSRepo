using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class InterUnit_TransferController : BaseController
    {
        private readonly IInterUnit_TransferRepository interunittransferrepository;
        public InterUnit_TransferController(IInterUnit_TransferRepository _interunittransferrepository)
        {
            interunittransferrepository = _interunittransferrepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetValue(int BankNameDropdown, string CHQ_NO, string CHQ_DATE)
        {
            string region = GetRegionCode;
            InterUnit_TransferModel dTResult = interunittransferrepository.GetTextboxValues(BankNameDropdown, CHQ_NO, CHQ_DATE, region);
            return Json(dTResult);

        }

        public IActionResult GetJVValue(int BankNameDropdown, string CHQ_NO, string CHQ_DATE)
        {

            
            InterUnit_TransferModel dTResult = interunittransferrepository.GetJVvalues(BankNameDropdown, CHQ_NO, CHQ_DATE);
            return Json(dTResult);

        }

        [HttpPost]
        public IActionResult LoadGrid([FromBody] DTParameters dtParameters)
        {
            DTResult<InterUnit_TransferModel> dTResult = interunittransferrepository.BillList(dtParameters);

            return Json(dTResult);


        }

        [HttpPost]
        public JsonResult Insert_InterUnit()
        {
            try
            {
                InterUnit_TransferModel model = new InterUnit_TransferModel();
                model.VCHR_DT = Convert.ToString(Request.Form["VCHR_DT"]);
                model.BANK_CD = Convert.ToInt32(Request.Form["BANK_CD"]);   
                model.CHQ_DT = Request.Form["CHQ_DATE"];
                model.CHQ_NO = Convert.ToString(Request.Form["CHQ_NO"]);
                model.VCHR_NO = Convert.ToString(Request.Form["VCHR_NO"]);
                model.SNO = Convert.ToInt32(Request.Form["SNO"]);
                model.ACC_CD = Request.Form["ACC_CD"];
                model.AMOUNT = Convert.ToDecimal(Request.Form["AMOUNT"]);
                model.NARRATION = Request.Form["Narration"];
                model.JV_NO = Request.Form["JV_NO"];
                model.Action = Request.Form["Action"];

                var msg = "";
                var Uname = UserId.ToString();

                var region = GetRegionCode;

                if (model.JV_NO == null || model.JV_NO == "")
                {
                    bool i = interunittransferrepository.Save(model, region);
                    if (i == true)
                    {
                        return Json(new { status = true, responseText = msg });
                    }
                }
                else
                {
                    bool i = interunittransferrepository.modify(model, region);
                    if (i == true)
                    {
                        return Json(new { status = true, responseText = msg });
                    }

                }
              
            }
            catch (Exception ex)
            {
                return Json(false);
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        public JsonResult Delete_InterUnit()
        {

            try
            {
                InterUnit_TransferModel model = new InterUnit_TransferModel();
                model.VCHR_DT = Convert.ToString(Request.Form["VCHR_DT"]);
                model.BANK_CD = Convert.ToInt32(Request.Form["BANK_CD"]);
                model.CHQ_DT = Request.Form["CHQ_DATE"];
                model.CHQ_NO = Convert.ToString(Request.Form["CHQ_NO"]);
                model.VCHR_NO = Convert.ToString(Request.Form["VCHR_NO"]);
                model.SNO = Convert.ToInt32(Request.Form["SNO"]);
                model.ACC_CD = Request.Form["ACC_CD"];
                model.AMOUNT = Convert.ToDecimal(Request.Form["AMOUNT"]);
                model.NARRATION = Request.Form["Narration"];
                model.JV_NO = Request.Form["JV_NO"];
                model.AMT_TRANSFERRED = Convert.ToDecimal(Request.Form["AMT_TRANSFERRED"]);
                model.SUSPENSE_AMT = Convert.ToDecimal(Request.Form["SUSPENSE_AMT"]);
                var msg = "";
                var Uname = UserId.ToString();

                var region = GetRegionCode;

               
                    InterUnit_TransferModel i = interunittransferrepository.Del_Select(model);
                    if (i == null)
                    {
                        return Json(new { status = true, responseText = msg });
                    }
              

            }
            catch (Exception ex)
            {
                return Json(false);
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });

        }

    }
}
