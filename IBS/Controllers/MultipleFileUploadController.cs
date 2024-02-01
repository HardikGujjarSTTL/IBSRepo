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
        public async Task<IActionResult> Index(MultipleFileUploadModel model)
        {
            //if (model.Files != null && model.Files.Count > 0)
            //{
            //    foreach (var file in model.Files)
            //    {
            //        if (file.Length > 0)
            //        {
            //            if (Path.GetExtension(file.FileName).ToLower() == ".pdf")
            //            {
            //                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ReadWriteData/MultipleFileUpload", file.FileName);

            //                using (var stream = new FileStream(filePath, FileMode.Create))
            //                {
            //                    await file.CopyToAsync(stream);
            //                }
            //            }
            //            else
            //            {
            //                AlertDanger("Only PDF files are allowed .");
            //            }
            //        }
            //    }
            //}
            //return RedirectToAction("Index");
            var FileName = "";
            var Bill_NO = "";
            if (model.Files != null && model.Files.Count > 0)
            {
                var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ReadWriteData/MultipleFileUpload");

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
                            FileName = "wwwroot/ReadWriteData/MultipleFileUpload/" + file.FileName;
                            Bill_NO = file.FileName.Substring(0, 10);
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }
                        }
                        else
                        {
                            AlertDanger("Only PDF files are allowed.");
                        }
                    }

                    int CreatedBy = UserId;
                    int result = multipleFileUploadRepository.InsertPDFDetails(FileName, Bill_NO, CreatedBy);
                }
            }
            else
            {
                AlertDanger("No files selected for upload.");
            }

            return RedirectToAction("Index");

        }


    }
    



}
