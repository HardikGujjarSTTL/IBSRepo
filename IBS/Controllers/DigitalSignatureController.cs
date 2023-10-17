using IBS.Helper;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class DigitalSignatureController : BaseController
    {
        private readonly IWebHostEnvironment env;

        public DigitalSignatureController(IWebHostEnvironment _env)
        {
            env = _env;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Manage(IFormFile file)
        {
            try
            {
                DigitalSignatureModel model = new DigitalSignatureModel
                {
                    IsLeft = true,
                    IsMultipleSign = false,
                    PageNo = 1,
                };

                string path = Path.Combine(env.WebRootPath, "ReadWriteData");
                byte[] bytes = null;

                if (file != null && file.Length > 0)
                {
                    using (var stream = new MemoryStream())
                    {
                        file.CopyTo(stream);
                        bytes = stream.ToArray();
                    }

                    string filePath = env.WebRootPath + Enums.GetEnumDescription(Enums.FolderPath.DigiSignatureFiles);

                    if (!Directory.Exists(filePath)) Directory.CreateDirectory(filePath);

                    string fileName = DateTime.Now.Ticks.ToString() + ".pdf";

                    string fullPath = filePath + "/" + fileName;

                    // Below code is save file 
                    System.IO.File.WriteAllBytes(fullPath, bytes);

                    DigitalSigner.SetSignField(bytes, fullPath, model.IsMultipleSign, model.IsLeft, model.SearchText, out int counter, model.PageNo, model.Level, model.X1, model.Y1, model.X2, model.Y2);
                    DigitalSigner.SignPDF(DigitalSigner.getCertificate("minesh vinodchandra doshi"), fullPath, "", "", counter, model.PageNo);

                    string signedFileName = fileName.Replace(".pdf", "_Signed.pdf");

                    return Json(new { status = 1, responseText = signedFileName });
                }

                return Json(new { status = 0, responseText = "File not found!!" });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "BankMaster", "Manage", 1, GetIPAddress());
            }
            return Json(new { status = 0, responseText = "Something went Wrong!!" });
        }
    }
}
