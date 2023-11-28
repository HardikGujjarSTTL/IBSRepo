using IBS.Interfaces.InspectionBilling;
using Microsoft.AspNetCore.Mvc;
using IBS.Models;
using IBS.Repositories;

namespace IBS.Controllers.InspectionBilling
{
    public class BillAdjustmentsController : BaseController
    {
        #region Variables
        private readonly IBillAdjustmentsRepository billRepository;

        #endregion
        public BillAdjustmentsController(IBillAdjustmentsRepository _billRepository)
        {
            billRepository = _billRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetBillDetails(string BillNo)
        {
            try
            {
                InspectionCertModel model = new InspectionCertModel();
                if (BillNo != null)
                {
                    model = billRepository.FindByBillDetails(BillNo, Region);
                }
                if (model.BillNo != null)
                {
                    return PartialView("_BillDetails", model);
                }
                else
                {
                    return Json(new { status = false, responseText = "No Record Found!!!." });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "BillAdjustments", "GetBillDetails", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpPost]
        public IActionResult LoadTableDetails([FromBody] DTParameters dtParameters)
        {
            DTResult<InspectionCertModel> dTResult = billRepository.GetLoadTableDetails(dtParameters, Region);
            return Json(dTResult);
        }

        public IActionResult EditListDetails(string Caseno, DateTime Callrecvdt, int Callsno, int ItemSrnoPo)
        {
            try
            {
                InspectionCertModel model = new();
                model.Regioncode = Region;
                if (Caseno != null && Callrecvdt != null && Callsno > 0 && ItemSrnoPo > 0)
                {
                    model = billRepository.FindByItemID(Caseno, Callrecvdt, Callsno, ItemSrnoPo);
                }
                return PartialView("_EditListDetails", model);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "BillAdjustments", "EditListDetails", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpPost]
        public IActionResult UpdateCallDetails(InspectionCertModel model, int ItemSrnoPo)
        {
            try
            {
                string id = "";
                string msg = "Item Description Updated Successfully.";

                if (ItemSrnoPo > 0 && model.Caseno != null && model.Callrecvdt != null && model.Callsno > 0)
                {
                    id = billRepository.UpdateCallDetails(model, ItemSrnoPo);
                }

                if (id != null)
                {
                    return Json(new { status = true, responseText = msg });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "BillAdjustments", "UpdateCallDetails", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        public IActionResult PopUp(string BillNo)
        {
            try
            {
                ICPopUpModel model = new ICPopUpModel();
                if (BillNo != null)
                {
                    model = billRepository.FindByBillDetailsPopUp(BillNo, Region);
                }
                return PartialView("_PopUp", model);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "BillAdjustments", "PopUp", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpPost]
        public IActionResult BillUpdate(InspectionCertModel model)
        {
            try
            {
                string i = "";
                string msg = "";
                string myYear, myMonth, myDay;
                myYear = Convert.ToString(model.BillDt).Substring(6, 4);
                myMonth = Convert.ToString(model.BillDt).Substring(3, 2);
                myDay = Convert.ToString(model.BillDt).Substring(0, 2);
                string certdt = myYear + myMonth + myDay;
                //int idt = dt1.CompareTo(DateTime.Now.Date);

                string dt1 = Convert.ToDateTime(model.BillDt).ToString("dd/MM/yyyy");
                int idt = dt1.CompareTo(DateTime.Now.Date.ToString("dd/MM/yyyy"));

                model.Regioncode = Region;
                int fyr = billRepository.financial_year_check(model);
                if (fyr == 1)
                {
                    msg = "Bill must be generated within the same financial year in which IC was Issued!!!" + ". \\n(ie. Certificate Date & Bill Date shoud be in same financial year)";
                }
                else if (idt > 0)
                {
                    msg = "Bill Date Cannot be greater then Current Date!!!";
                }
                else if (model.BpoType == "R" && model.Au == "" && !(model.BpoRly == "RCF" || model.BpoRly == "ICF" || model.BpoRly == "RWF"))
                {
                    msg = "AU Cannot be Blank For Railways Bills, Kindly Update the AU for the BPO and then Generate the Bill!!!";
                }
                else
                {
                    model.UserId = Convert.ToString(UserId);

                    i = billRepository.BillUpdate(model, Region);
                    msg = "Update Successfully.";
                }
                if (model.UpdateStatus == "-1")
                {
                    msg = "Fee Details not available.";
                }
                else if (model.UpdateStatus == "-2")
                {
                    msg = "MINIMUM FEE PAYBLE IS GREATER THEN MAXIMUM FEE.";
                }
                else if (model.UpdateStatus == "-3")
                {
                    msg = "Unable to access Bill Master.";
                }
                else if (model.UpdateStatus == "-4")
                {
                    msg = "Unable to Insert New Bill No. in Bill Master.";
                }
                else if (model.UpdateStatus == "-5")
                {
                    msg = "Invalid Bill No. Passed as Parameter.";
                }
                else if (model.UpdateStatus == "-6")
                {
                    msg = "Unable to Insert Bill Details.";
                }
                else if (model.UpdateStatus == "-7")
                {
                    msg = "Error occured during updating Fee Details in Bill Master.";
                }
                else if (model.UpdateStatus == "-8")
                {
                    if (Convert.ToInt32(certdt) >= 20170701)
                    {
                        msg = "Unable to Select GST Tax Rates";
                    }
                    else
                    {
                        msg = "Unable to Select Service Tax Rates";
                    }
                }
                else
                {
                    if (model.TIFee < 1)
                    {
                        msg = "Zero Value BILL!!!";
                    }
                }
                if (msg != "")
                {
                    if (msg == "Update Successfully.")
                    {
                        if (i != "")
                        {
                            return Json(new { status = true, responseText = msg, Id = i });
                        }
                        else
                        {
                            msg = "This Call cannot be deleted. because IC is present for this call!!!";
                            return Json(new { status = false, responseText = msg, Id = i });
                        }
                    }
                    else
                    {
                        return Json(new { status = true, responseText = msg, Id = i });
                    }
                }

            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "BillAdjustments", "BillUpdate", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        //public IActionResult GetFeeCalculation(string Caseno, string Callrecvdt, int Callsno, string Consignee,string BillNo, decimal AdjustmentFee)
        //{
        //    DTResult<InspectionCertItemListModel> dTResult = billRepository.FindByFeesDetails(Caseno, Callrecvdt, Callsno, Consignee, BillNo, AdjustmentFee);
        //    return Json(dTResult);
        //}

        public IActionResult GetFeeCalculation(string Caseno, string Callrecvdt, int Callsno, string Consignee, string BillNo, decimal AdjustmentFee, int ConsigneeCd)
        {
            InspectionCertModel model = new();
            try
            {
                model = billRepository.FindByFeesDetails(Caseno, Callrecvdt, Callsno, Consignee, BillNo, AdjustmentFee, ConsigneeCd);
                return Json(new { status = true, responseText = model });
            }

            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "BillAdjustments", "UpdateCallDetails", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
    }
}
