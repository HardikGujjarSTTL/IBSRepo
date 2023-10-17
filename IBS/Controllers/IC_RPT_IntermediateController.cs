using IBS.DataAccess;
using IBS.Filters;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Xml;

namespace IBS.Controllers
{
    [Authorization]
    public class IC_RPT_IntermediateController : BaseController
    {
        #region Varible
        private readonly IIC_RPT_IntermediateRepository iC_RPT_IntermediateRepository;
        SessionHelper objSessionHelper = new SessionHelper();
        private readonly IWebHostEnvironment env;
        public IC_RPT_IntermediateController(IIC_RPT_IntermediateRepository _iC_RPT_IntermediateRepository, IWebHostEnvironment _env)
        {
            iC_RPT_IntermediateRepository = _iC_RPT_IntermediateRepository;
            this.env = _env;
        }
        #endregion

        [Authorization("IC_RPT_Intermediate", "Index", "view")]
        public IActionResult Index()
        {
            IC_RPT_IntermediateModel model = new();            
            var CASE_NO = "N21111089";
            var Call_Recv_dt = Convert.ToString("13/08/2022");
            var Call_SNO = "3";
            var CONSIGNEE_CD = "39";
            var ACTIONAR = "A";
            //if (Convert.ToString(Request.Query["CASE_NO"]) == null || Convert.ToString(Request.Query["CALL_RECV_DT"]) == null)
            //{
            //    CASE_NO = "";
            //    Call_Recv_dt = "";
            //    Call_SNO = "";
            //    CONSIGNEE_CD = "";
            //    ACTIONAR = "";
            //}
            //else
            //{
            //    CASE_NO = Convert.ToString(Request.Query["CASE_NO"]);
            //    Call_Recv_dt = Convert.ToString(Request.Query["CALL_RECV_DT"]);
            //    Call_SNO = Convert.ToString(Request.Query["CALL_SNO"]);
            //    CONSIGNEE_CD = Convert.ToString(Request.Query["CONSIGNEE_CD"]);
            //    ACTIONAR = Convert.ToString(Request.Query["ACTIONAR"]);
            //}

            if(Convert.ToString(Request.Query["filename"]) != null)
            {
                string filename = Convert.ToString(Request.Query["filename"]);

                XmlDocument XmlDoc = new XmlDocument();
                XmlDoc.Load(Path.Combine("IC_XML/" + filename + ".xml"));

                string dsic = "";
                XmlNode node;
                byte[] imageBytes;
                try
                {
                    node = XmlDoc.DocumentElement.SelectSingleNode("/response/data");
                    dsic = node.InnerText;
                    imageBytes = Convert.FromBase64String(node.InnerText);
                    FileStream fs = new FileStream(Path.Combine("..") + @"/BILL_IC/" + filename + ".PDF", FileMode.OpenOrCreate);
                    fs.Write(imageBytes, 0, imageBytes.Length);
                    fs.Close();
                }
                catch (Exception ex)
                {
                    node = XmlDoc.DocumentElement.SelectSingleNode("/request/data");
                    dsic = node.InnerText;
                    imageBytes = Convert.FromBase64String(node.InnerText);
                }

                //Response.Clear();
                //Response.Buffer = true;
                //Response.ContentType = "application/pdf";
                //Response.BinaryWrite(imageBytes);
                //Response.End();
            }
            
            model = iC_RPT_IntermediateRepository.AcceptedFun(CASE_NO, Call_Recv_dt, Call_SNO, CONSIGNEE_CD);
            model.ACTIONAR = ACTIONAR;
            model.CONSIGNEE_CD = CONSIGNEE_CD;

            //model = iC_RPT_IntermediateRepository.GetDetails(model.CASE_NO, model.Display_Call_Recv_dt, model.Call_SNO, model.ITEM_SRNO_PO, model.CONSIGNEE_CD);

            model.Region = Region;
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadPOAmendmentTable([FromBody] DTParameters dtParameters)
        {
            DTResult<PO_Amendments> dTResult = iC_RPT_IntermediateRepository.GetPOAmendment(dtParameters);
            objSessionHelper.lstPoAmendments = dTResult.data.ToList();
            return Json(dTResult);
        }

        public IActionResult GetPoAmendment(int id)
        {
            var data = objSessionHelper.lstPoAmendments.Where(x => Convert.ToInt32(x.Sno) == id).FirstOrDefault();
            return Json(data);
        }

        public IActionResult AcceptedFun(string Case_No, string Call_Recv_Dt, string Call_SNo, string Consignee_Cd)
        {
            var data = iC_RPT_IntermediateRepository.AcceptedFun(Case_No, Call_Recv_Dt, Call_SNo, Consignee_Cd);
            return Json(data);
        }

        public IActionResult GetVisitsChanges(string Case_No, string Call_Recv_Dt, string Call_SNo, string VisitDate)
        {
            VisitDate = iC_RPT_IntermediateRepository.GetVisitsChanges(Case_No, Call_Recv_Dt, Call_SNo, VisitDate);
            return Json(VisitDate);
        }


        public IActionResult FillItems(string Case_No, string Call_Recv_Dt, string Call_SNo, string Consignee_Cd)
        {
            var model = iC_RPT_IntermediateRepository.FillItems(Case_No, Call_Recv_Dt, Call_SNo, Consignee_Cd);
            return Json(model);
        }

        public IActionResult SetItemVal(string Case_No, string Call_Recv_Dt, string Call_SNo, string ITEM_SRNO_PO, string Consignee_Cd)
        {
            var model = iC_RPT_IntermediateRepository.GetDetails(Case_No, Call_Recv_Dt, Call_SNo, ITEM_SRNO_PO, Consignee_Cd);
            return Json(model);
        }


        public IActionResult SetAccepted(string Case_No, string Call_Recv_Dt, string Call_SNo, string Consignee_Cd)
        {
            var data = iC_RPT_IntermediateRepository.SetAccepted(Case_No, Call_Recv_Dt, Call_SNo, Consignee_Cd);
            return Json(data);
        }

        public IActionResult FillItemDropDown(string Case_No, string Call_Recv_Dt, string Call_SNo, string Consignee_Cd)
        {
            var data = iC_RPT_IntermediateRepository.GetItems(Case_No, Call_Recv_Dt, Call_SNo, Consignee_Cd);
            return Json(data);
        }

        [HttpPost]
        public IActionResult SaveDetails(IC_RPT_IntermediateModel model)
        {
            try
            {
                var result = true;
                model.IESTAMP_PATH = env.WebRootPath + "/IE_IMAGES/Default/Blank.jpg";
                model.IESTAMP2_PATH = env.WebRootPath + "/IE_IMAGES/Default/Blank.jpg";

                if (model.IE_STAMPS_DETAIL != "0") { model.IESTAMP_PATH = env.WebRootPath + "/" + Convert.ToString(GetUserInfo.IeCd) + model.IE_STAMPS_DETAIL + ".jpg"; }
                if (model.IE_STAMPS_DETAIL2 != "0") { model.IESTAMP2_PATH = env.WebRootPath + "/" + Convert.ToString(GetUserInfo.IeCd) + model.IE_STAMPS_DETAIL2 + ".jpg"; }

                result = iC_RPT_IntermediateRepository.SaveDetail(model, GetUserInfo);

                if (result)
                {
                    return Json(new { status = true, responseText = "Record Added Successfully." });
                }

                //    AlertAddSuccess("Record Added Successfully.");
                //else
                //    AlertDanger("Looks Like Something Went Wrong. Some Error Occurs...");
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "IC_RPT_Intermediate", "SaveDetails", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Looks Like Something Went Wrong. Some Error Occurs..." });
        }

        public IActionResult RefreshDetail(IC_RPT_IntermediateModel model)
        {
            try
            {
                var data = iC_RPT_IntermediateRepository.RefreshDetail(model, GetUserInfo);
                iC_RPT_IntermediateRepository.DeleteNotReq(model);
                if (data == "")
                {
                    return Json(new { status = true, responseText = "Your request has been accepted!" });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "IC_RPT_Intermediate", "RefereDetail", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Looks Like Something Went Wrong. Some Error Occurs..." });
        }

        public IActionResult SaveAmendment(string CaseNo, string Po_No, PO_Amendments model)
        {
            int res = 0;
            var Iecd = GetUserInfo.IeCd;
            try
            {
                List<PO_Amendments> lstPoAhm = objSessionHelper.lstPoAmendments;
                lstPoAhm.ForEach(x => x.IECD = Iecd);
                res = iC_RPT_IntermediateRepository.SaveAmendment(CaseNo, Po_No, model, lstPoAhm);
                if (res > 0)
                {
                    return Json(new { status = true, responseText = "PO Amendment Record Added Successfully." });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "IC_RPT_Intermediate", "SaveAmendment", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Looks Like Something Went Wrong. Some Error Occurs..." });
        }
    }
}
