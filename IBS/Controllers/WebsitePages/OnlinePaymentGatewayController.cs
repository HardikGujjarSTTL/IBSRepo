using IBS.Interfaces.WebsitePages;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PuppeteerSharp.Media;
using PuppeteerSharp;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using IBS.Helper;
using System.Collections.Specialized;
using System.Globalization;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace IBS.Controllers.WebsitePages
{
    public class OnlinePaymentGatewayController : BaseController
    {
        #region Variables
        private readonly IOnlinePaymentGatewayRepository onlinePaymentGatewayRepository;
        private readonly IWebHostEnvironment env;
        private readonly IConfiguration config;
        #endregion
        public OnlinePaymentGatewayController(IOnlinePaymentGatewayRepository _onlinePaymentGatewayRepository, IWebHostEnvironment env, IConfiguration _config)
        {
            onlinePaymentGatewayRepository = _onlinePaymentGatewayRepository;
            this.env = env;
            config = _config;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult VerifyPayment(OnlinePaymentGateway model)
        {
            model = onlinePaymentGatewayRepository.VerifyByCaseNo(model);
            return Json(model);
        }

        public IActionResult PaymentIntegration(OnlinePaymentGateway model)
        {
            PayrequestModel.RootObject rt = new PayrequestModel.RootObject();
            PayrequestModel.MsgBdy mb = new PayrequestModel.MsgBdy();
            PayrequestModel.HeadDetails hd = new PayrequestModel.HeadDetails();
            PayrequestModel.MerchDetails md = new PayrequestModel.MerchDetails();
            PayrequestModel.PayDetails pd = new PayrequestModel.PayDetails();
            PayrequestModel.CustDetails cd = new PayrequestModel.CustDetails();
            PayrequestModel.Extras ex = new PayrequestModel.Extras();

            PayrequestModel.Payrequest pr = new PayrequestModel.Payrequest();
            hd.version = "OTSv1.1";
            hd.api = "AUTH";
            hd.platform = "FLASH";

            //md.merchId = "317159";
            md.merchId = config.GetSection("PaymentSetting")["merchId"];
            md.userId = config.GetSection("PaymentSetting")["merchId"];
            md.password = "Test@123";
            //md.merchTxnDate = "2023-12-12 20:46:00";
            md.merchTxnDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            md.merchTxnId = "test000123";

            pd.amount = Convert.ToString(model.Charges);
            pd.product = "NSE";
            pd.custAccNo = "213232323";
            pd.txnCurrency = "INR";

            cd.custEmail = model.Email;
            cd.custMobile = model.Mobile;

            ex.udf1 = "";
            ex.udf2 = "";
            ex.udf3 = "";
            ex.udf4 = "";
            ex.udf5 = "";

            pr.headDetails = hd;
            pr.merchDetails = md;
            pr.payDetails = pd;
            pr.custDetails = cd;
            pr.extras = ex;

            rt.payInstrument = pr;
            var json = JsonConvert.SerializeObject(rt);

            string passphrase = "A4476C2062FFA58980DC8F79EB6A799E";
            string salt = "A4476C2062FFA58980DC8F79EB6A799E";
            byte[] iv = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            int iterations = 65536;
            int keysize = 256;
            string hashAlgorithm = "SHA1";
            string Encryptval = Encrypt(json, passphrase, salt, iv, iterations);

            string testurleq = "https://caller.atomtech.in/ots/aipay/auth?merchId=317159&encData=" + Encryptval;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(testurleq);
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            request.Proxy.Credentials = CredentialCache.DefaultCredentials;
            Encoding encoding = new UTF8Encoding();
            byte[] data = encoding.GetBytes(json);
            request.ProtocolVersion = HttpVersion.Version11;
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = data.Length;
            Stream stream = request.GetRequestStream();
            stream.Write(data, 0, data.Length);
            stream.Close();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string jsonresponse = response.ToString();

            StreamReader reader = new StreamReader(response.GetResponseStream());
            string temp = null;
            string status = "";
            while ((temp = reader.ReadLine()) != null)
            {
                jsonresponse += temp;
            }
            var result = jsonresponse.Replace("System.Net.HttpWebResponse", "");

            var uri = new Uri("http://atom.in?" + result);
            var query = HttpUtility.ParseQueryString(uri.Query);

            string encData = query.Get("encData");
            string passphrase1 = "75AEF0FA1B94B3C10D4F5B268F757F11";
            string salt1 = "75AEF0FA1B94B3C10D4F5B268F757F11";
            string Decryptval = decrypt(encData, passphrase1, salt1, iv, iterations);
            PayverifyModel.Payverify objectres = new PayverifyModel.Payverify();
            objectres = JsonConvert.DeserializeObject<PayverifyModel.Payverify>(Decryptval);
            string txnMessage = objectres.responseDetails.txnMessage;

            string Tok_id = objectres.atomTokenId;
            var url = Request.Scheme + "://" + Request.Host.Value;
            model.LocalURL = url;

            model = onlinePaymentGatewayRepository.PaymentIntergreationSave(model);
            model.Tok_id = Tok_id;
            return Json(new { status = false, response = model });
        }

        public string Encrypt(string plainText, string passphrase, string salt, Byte[] iv, int iterations)
        {
            var plainBytes = Encoding.UTF8.GetBytes(plainText);
            string data = ByteArrayToHexString(Encrypt(plainBytes, GetSymmetricAlgorithm(passphrase, salt, iv, iterations))).ToUpper();
            return data;
        }

        public string decrypt(string plainText, string passphrase, string salt, Byte[] iv, int iterations)
        {
            byte[] str = HexStringToByte(plainText);

            string data1 = Encoding.UTF8.GetString(decrypt(str, GetSymmetricAlgorithm(passphrase, salt, iv, iterations)));
            return data1;
        }

        public byte[] Encrypt(byte[] plainBytes, SymmetricAlgorithm sa)
        {
            return sa.CreateEncryptor().TransformFinalBlock(plainBytes, 0, plainBytes.Length);

        }

        public byte[] decrypt(byte[] plainBytes, SymmetricAlgorithm sa)
        {
            return sa.CreateDecryptor().TransformFinalBlock(plainBytes, 0, plainBytes.Length);
        }

        public SymmetricAlgorithm GetSymmetricAlgorithm(string passphrase, string salt, Byte[] iv, int iterations)
        {
            var saltBytes = new byte[16];
            var ivBytes = new byte[16];
            Rfc2898DeriveBytes rfcdb = new System.Security.Cryptography.Rfc2898DeriveBytes(passphrase, Encoding.UTF8.GetBytes(salt), iterations, HashAlgorithmName.SHA512);
            saltBytes = rfcdb.GetBytes(32);
            var tempBytes = iv;
            Array.Copy(tempBytes, ivBytes, Math.Min(ivBytes.Length, tempBytes.Length));
            var rij = new RijndaelManaged(); //SymmetricAlgorithm.Create();
            rij.Mode = CipherMode.CBC;
            rij.Padding = PaddingMode.PKCS7;
            rij.FeedbackSize = 128;
            rij.KeySize = 128;

            rij.BlockSize = 128;
            rij.Key = saltBytes;
            rij.IV = ivBytes;
            return rij;
        }

        protected static byte[] HexStringToByte(string hexString)
        {
            try
            {
                int bytesCount = (hexString.Length) / 2;
                byte[] bytes = new byte[bytesCount];
                for (int x = 0; x < bytesCount; ++x)
                {
                    bytes[x] = Convert.ToByte(hexString.Substring(x * 2, 2), 16);
                }
                return bytes;
            }
            catch
            {
                throw;
            }
        }

        public static string ByteArrayToHexString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public IActionResult PaymentResponse(string mef_ref)
        {
            OnlinePaymentGateway model = new();
            byte[] iv = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            int iterations = 65536;
            int keysize = 256;
            string hashAlgorithm = "SHA1";
            string encdata = Request.Form["encdata"];
            string passphrase1 = "75AEF0FA1B94B3C10D4F5B268F757F11";
            string salt1 = "75AEF0FA1B94B3C10D4F5B268F757F11";
            string Decryptval = decrypt(encdata, passphrase1, salt1, iv, iterations);
            PayresponseModel.Rootobject root = new PayresponseModel.Rootobject();
            PayresponseModel.Parent objectres = new PayresponseModel.Parent();

            objectres = JsonConvert.DeserializeObject<PayresponseModel.Parent>(Decryptval);

            model.MER_TXN_REF = mef_ref;
            model.MERTXNID = objectres.payInstrument.merchDetails.merchTxnId;
            model.Charges = Convert.ToDecimal(objectres.payInstrument.payDetails.amount);
            model.Product = objectres.payInstrument.payDetails.product;
            DateTime txnCompleteDate = Convert.ToDateTime(objectres.payInstrument.payDetails.txnCompleteDate);
            model.TranDate = txnCompleteDate.ToString("dd/MM/yyyy");
            model.BankTXNID = objectres.payInstrument.payModeSpecificData.bankDetails.bankTxnId;
            model.BankName = objectres.payInstrument.payModeSpecificData.bankDetails.otsBankName;
            model.PaymentStatus = objectres.payInstrument.responseDetails.message;
            model.Email = objectres.payInstrument.custDetails.custEmail;
            model.Mobile = objectres.payInstrument.custDetails.custMobile;
            model.MerID= objectres.payInstrument.merchDetails.merchId;
            model.merchTxnDate = objectres.payInstrument.merchDetails.merchTxnDate;
            model.AtomTXNID = objectres.payInstrument.payDetails.atomTxnId;
            model.custAccNo = objectres.payInstrument.payDetails.custAccNo;
            model.BankID = objectres.payInstrument.payModeSpecificData.bankDetails.otsBankId;
            model.SubChannel = objectres.payInstrument.payModeSpecificData.subChannel[0];
            model.Description = objectres.payInstrument.responseDetails.description;
            model.StatusCode = objectres.payInstrument.responseDetails.statusCode;

            model = onlinePaymentGatewayRepository.PaymentResponseUpdate(model);
            GlobalDeclaration.OnlinePaymentResponse = model;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> GeneratePDF(OnlinePaymentGateway model)
        {
            string htmlContent = string.Empty;
            //OnlinePaymentGateway model = GlobalDeclaration.OnlinePaymentResponse;
            htmlContent = await this.RenderViewToStringAsync("/Views/OnlinePaymentGateway/PaymentResponsePDF.cshtml", model);

            await new BrowserFetcher().DownloadAsync();
            await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true,
                DefaultViewport = null
            });
            await using var page = await browser.NewPageAsync();
            await page.EmulateMediaTypeAsync(MediaType.Screen);
            await page.SetContentAsync(htmlContent);

            string cssPath = env.WebRootPath + "/css/report.css";

            AddTagOptions bootstrapCSS = new AddTagOptions() { Path = cssPath };
            await page.AddStyleTagAsync(bootstrapCSS);

            var pdfContent = await page.PdfStreamAsync(new PdfOptions
            {
                Landscape = true,
                Format = PaperFormat.Letter,
                PrintBackground = true
            });

            await browser.CloseAsync();

            return File(pdfContent, "application/pdf", Guid.NewGuid().ToString() + ".pdf");
        }
    }
}
