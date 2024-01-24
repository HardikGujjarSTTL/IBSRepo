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
        private readonly IConfiguration config;

        public IC_RPT_IntermediateController(IIC_RPT_IntermediateRepository _iC_RPT_IntermediateRepository, IWebHostEnvironment _env, IConfiguration _config)
        {
            iC_RPT_IntermediateRepository = _iC_RPT_IntermediateRepository;
            this.env = _env;
            config = _config;
        }
        #endregion

        //[Authorization("IC_RPT_Intermediate", "Index", "view")]
        public IActionResult Index()
        {
            ViewBag.ReportUrl = config.GetSection("AppSettings")["ReportUrl"];
            IC_RPT_IntermediateModel model = new();
            //var CASE_NO = "N21111089";
            //var Call_Recv_dt = Convert.ToString("13/08/2022");
            //var Call_SNO = "3";
            //var CONSIGNEE_CD = "39";
            //var ACTIONAR = "A";

            var CASE_NO = "";
            var Call_Recv_dt = "";
            var Call_SNO = "";
            var CONSIGNEE_CD = "";
            var ACTIONAR = "";
            if (Convert.ToString(Request.Query["CASE_NO"]) == null || Convert.ToString(Request.Query["CALL_RECV_DT"]) == null)
            {
                CASE_NO = "";
                Call_Recv_dt = "";
                Call_SNO = "";
                CONSIGNEE_CD = "";
                ACTIONAR = "";
            }
            else
            {
                CASE_NO = Convert.ToString(Request.Query["CASE_NO"]);
                Call_Recv_dt = Convert.ToString(Request.Query["CALL_RECV_DT"]);
                Call_SNO = Convert.ToString(Request.Query["CALL_SNO"]);
                CONSIGNEE_CD = Convert.ToString(Request.Query["CONSIGNEE_CD"]);
                ACTIONAR = Convert.ToString(Request.Query["ACTIONAR"]);
            }

            if (Convert.ToString(Request.Query["filename"]) != null)
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

        public IActionResult SaveAmendment(string CaseNo, string PO_NO, string Sno, string Amendments, string Date)
        {
            int res = 0;
            var Iecd = GetUserInfo.IeCd;
            try
            {
                List<PO_Amendments> lstPoAhm = objSessionHelper.lstPoAmendments;
                PO_Amendments model = new();
                model.Sno = Sno;
                model.Amendments = Amendments;
                model.Date = Date;
                model.IECD = Convert.ToString(Iecd);
                lstPoAhm.ForEach(x => x.IECD = Convert.ToString(Iecd));
                res = iC_RPT_IntermediateRepository.SaveAmendment(CaseNo, PO_NO, model, lstPoAhm, "Insert");
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

        public IActionResult DeletePOAmendment(string CaseNo, string PO_NO, string Sno)
        {
            int res = 0;
            var Iecd = GetUserInfo.IeCd;
            try
            {
                List<PO_Amendments> lstPoAhm = objSessionHelper.lstPoAmendments;
                PO_Amendments model = new();
                model.Sno = "";
                model.Amendments = "";
                model.Date = "";
                lstPoAhm.ForEach(x => x.IECD = Convert.ToString(Iecd));

                var data = lstPoAhm.Where(x => x.Sno == Sno).Select(x => x).FirstOrDefault();
                lstPoAhm.Remove(data);
                res = iC_RPT_IntermediateRepository.SaveAmendment(CaseNo, PO_NO, model, lstPoAhm, "Delete");
                if (res > 0)
                {
                    return Json(new { status = true, responseText = "PO Amendment Record Delete Successfully." });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "IC_RPT_Intermediate", "SaveAmendment", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Looks Like Something Went Wrong. Some Error Occurs..." });
        }

        public IActionResult PKIDATA(string file, string filename)
        {
            if (file != null)
            {
                string xmlResp = file;
                using (StreamWriter writetext = new StreamWriter(Path.Combine(env.WebRootPath + "/IC_XML/" + filename + ".xml")))
                {
                    writetext.WriteLine(xmlResp);
                }
            }
            return Json(filename);
        }

        public async Task<IActionResult> GetReportData(string CaseNO, string Call_Recv_Dt, string CallSNo, string Consignee_CD, string Region, string BkNo, string SetNo)
        {
            try
            {
                string base64String = string.Empty;
           
                var webServiceUrl = config.GetSection("AppSettings")["ReportUrl"];
                webServiceUrl = webServiceUrl.Replace("Default.aspx", "WebService1.asmx");

                string methodName = "GetReportData";

                var formData = new Dictionary<string, string>
                {
                    { "CaseNO", CaseNO },
                    { "Call_Recv_Dt", Call_Recv_Dt },
                    { "CallSNo", CallSNo },
                    { "Consignee_CD", Consignee_CD },
                    { "Region", Region },
                    { "BkNo", BkNo },
                    { "SetNo", SetNo },
                };

                using (var httpClient = new HttpClient())
                {
                    var encodedFormData = new FormUrlEncodedContent(formData);

                    var response = await httpClient.PostAsync($"{webServiceUrl}/{methodName}", encodedFormData);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(responseContent);

                        base64String = xmlDoc.InnerText.Replace("\"", "");

                    }
                }

                if (!string.IsNullOrEmpty(base64String))
                {
                    string xmlData = GenerateDigitalSignatureXML(base64String);

                    return Json(new { status = 1, responseText = xmlData });
                }
                else
                {
                    return Json(new { status = 0, responseText = "Something went wrong!!" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = 0, responseText = ex.Message.ToString() });
            }

        }

        public string GenerateDigitalSignatureXML(string base64String)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);

            XmlNode requestNode = doc.CreateElement("request");
            doc.AppendChild(requestNode);

            XmlNode commandNode = doc.CreateElement("command");
            commandNode.AppendChild(doc.CreateTextNode("pkiNetworkSign"));
            requestNode.AppendChild(commandNode);

            XmlNode tsNode = doc.CreateElement("ts");
            string tym = DateTime.Now.ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffffffzzz");
            tsNode.AppendChild(doc.CreateTextNode(tym));
            requestNode.AppendChild(tsNode);
            Random random = new Random();
            string otp = Convert.ToString(random.Next(1000, 9999));

            XmlNode txnNode = doc.CreateElement("txn");
            txnNode.AppendChild(doc.CreateTextNode(otp));
            requestNode.AppendChild(txnNode);

            XmlNode certNode = doc.CreateElement("certificate");
            requestNode.AppendChild(certNode);

            XmlNode nameNode1 = doc.CreateElement("attribute");
            XmlAttribute nameNode1Attr = doc.CreateAttribute("name");
            nameNode1Attr.Value = "CN";
            nameNode1.Attributes.Append(nameNode1Attr);
            certNode.AppendChild(nameNode1);

            XmlNode nameNode2 = doc.CreateElement("attribute");
            XmlAttribute nameNode2Attr = doc.CreateAttribute("name");
            nameNode2Attr.Value = "O";
            nameNode2.Attributes.Append(nameNode2Attr);
            certNode.AppendChild(nameNode2);

            XmlNode nameNode3 = doc.CreateElement("attribute");
            XmlAttribute nameNode3Attr = doc.CreateAttribute("name");
            nameNode3Attr.Value = "OU";
            nameNode3.Attributes.Append(nameNode3Attr);
            certNode.AppendChild(nameNode3);

            XmlNode nameNode4 = doc.CreateElement("attribute");
            XmlAttribute nameNode4Attr = doc.CreateAttribute("name");
            nameNode4Attr.Value = "T";
            nameNode4.Attributes.Append(nameNode4Attr);
            certNode.AppendChild(nameNode4);

            XmlNode nameNode5 = doc.CreateElement("attribute");
            XmlAttribute nameNode5Attr = doc.CreateAttribute("name");
            nameNode5Attr.Value = "E";
            nameNode5.Attributes.Append(nameNode5Attr);
            certNode.AppendChild(nameNode5);

            XmlNode nameNode6 = doc.CreateElement("attribute");
            XmlAttribute nameNode6Attr = doc.CreateAttribute("name");
            nameNode6Attr.Value = "SN";
            nameNode6.Attributes.Append(nameNode6Attr);
            certNode.AppendChild(nameNode6);

            XmlNode nameNode7 = doc.CreateElement("attribute");
            XmlAttribute nameNode7Attr = doc.CreateAttribute("name");
            nameNode7Attr.Value = "CA";
            nameNode7.Attributes.Append(nameNode7Attr);
            certNode.AppendChild(nameNode7);

            XmlNode nameNode8 = doc.CreateElement("attribute");
            XmlAttribute nameNode8Attr = doc.CreateAttribute("name");
            nameNode8Attr.Value = "TC";
            nameNode8.Attributes.Append(nameNode8Attr);
            nameNode8.AppendChild(doc.CreateTextNode("SG"));
            certNode.AppendChild(nameNode8);

            XmlNode nameNode9 = doc.CreateElement("attribute");
            XmlAttribute nameNode9Attr = doc.CreateAttribute("name");
            nameNode9Attr.Value = "AP";
            nameNode9.Attributes.Append(nameNode9Attr);
            nameNode9.AppendChild(doc.CreateTextNode("1"));
            certNode.AppendChild(nameNode9);

            XmlNode nameNode10 = doc.CreateElement("attribute");
            XmlAttribute nameNode10Attr = doc.CreateAttribute("name");
            nameNode10Attr.Value = "VD";
            nameNode10.Attributes.Append(nameNode10Attr);
            certNode.AppendChild(nameNode10);

            XmlNode fileNode = doc.CreateElement("file");
            requestNode.AppendChild(fileNode);

            XmlNode nameNode11 = doc.CreateElement("attribute");
            XmlAttribute nameNode11Attr = doc.CreateAttribute("name");
            nameNode11Attr.Value = "type";
            nameNode11.Attributes.Append(nameNode11Attr);
            nameNode11.AppendChild(doc.CreateTextNode("pdf"));
            fileNode.AppendChild(nameNode11);

            XmlNode pdfNode = doc.CreateElement("pdf");
            requestNode.AppendChild(pdfNode);

            XmlNode pageNode = doc.CreateElement("page");
            pageNode.AppendChild(doc.CreateTextNode("1"));
            pdfNode.AppendChild(pageNode);

            XmlNode coodNode = doc.CreateElement("cood");
            coodNode.AppendChild(doc.CreateTextNode("400,45"));
            pdfNode.AppendChild(coodNode);

            XmlNode sizeNode = doc.CreateElement("size");
            sizeNode.AppendChild(doc.CreateTextNode("165,60"));

            pdfNode.AppendChild(sizeNode);

            XmlNode dataNode = doc.CreateElement("data");
            dataNode.AppendChild(doc.CreateTextNode(base64String));
            requestNode.AppendChild(dataNode);

            StringWriter sw = new StringWriter();
            XmlTextWriter tx = new XmlTextWriter(sw);
            doc.WriteTo(tx);

            return sw.ToString();
        }

    }
}
