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
            if (model.Files != null && model.Files.Count > 0)
            {
                foreach (var file in model.Files)
                {
                    if (file.Length > 0)
                    {
                        if (Path.GetExtension(file.FileName).ToLower() == ".pdf")
                        {
                            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ReadWriteData/MultipleFileUpload", file.FileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }
                        }
                        else
                        {
                            AlertDanger("Only PDF files are allowed .");
                        }
                    }
                }
            }
            return RedirectToAction("Index");
        }


    }
    



}
