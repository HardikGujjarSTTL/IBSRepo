using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace IBS.Controllers
{
    public class MultipleFileUploadController : BaseController
    {
        private readonly IMultipleFileUploadRepository multipleFileUploadRepository;


        public MultipleFileUploadController(IMultipleFileUploadRepository _multipleFileUploadRepository)
        {
            multipleFileUploadRepository = _multipleFileUploadRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(MultipleFileUploadModel model)
        {
            var FileName = "";
            var Bill_NO = "";

            try
            {
                if (model.Files != null)
                {
                    if (model.Files.Count > 0 && model.Files.Count <= 50)
                    {
                        var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "/ReadWriteData/MultipleFileUpload");

                        if (!Directory.Exists(uploadDirectory))
                        {
                            Directory.CreateDirectory(uploadDirectory);
                        }

                        foreach (var file in model.Files)
                        {
                            if (file.Length > 0)
                            {
                                if (Path.GetExtension(file.FileName).ToLower() == ".pdf")
                                {
                                    var filePath = Path.Combine(uploadDirectory, file.FileName);
                                    FileName = "/ReadWriteData/MultipleFileUpload/" + file.FileName;
                                    Bill_NO = file.FileName.Split('.')[0];
                                    using (var stream = new FileStream(filePath, FileMode.Create))
                                    {
                                        await file.CopyToAsync(stream);
                                    }
                                }
                                else
                                {
                                    return Json(new { status = false, responseText = "Only PDF files are allowed." });
                                }
                            }

                            int CreatedBy = UserId;
                            int result = multipleFileUploadRepository.InsertPDFDetails(FileName, Bill_NO, CreatedBy);
                        }
                    }
                    else
                    {
                        return Json(new { status = false, responseText = "At a time only 50 pdf are upload." });
                    }
                }
                else
                {
                    return Json(new { status = false, responseText = "No files selected for upload." });
                }

            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "MultipleFileUpload", "Index", 1, GetIPAddress());
            }

            return Json(new { status = true, responseText = "File Uploaded !!" });
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<MultipleFileUploadModel> dTResult = multipleFileUploadRepository.GetDocList(dtParameters);
            return Json(dTResult);
        }
    }
}
