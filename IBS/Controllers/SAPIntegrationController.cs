using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.OleDb;


namespace IBS.Controllers
{
    public class SAPIntegrationController : BaseController
    {
        private readonly ISAPIntegrationRepository sapIntegrationRepository;
        private readonly IWebHostEnvironment env;

        public SAPIntegrationController(ISAPIntegrationRepository _sapIntegrationRepository, IWebHostEnvironment _environment)
        {
            sapIntegrationRepository = _sapIntegrationRepository;
            env = _environment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ExportExcelBPO(string BPO_Cd, string Type)
        {
            DataSet ds = new DataSet();
            string fileName = "";
            if (Type == "MultipleBPO")
            {
                fileName = "MultipleBPO";
                ds = sapIntegrationRepository.ExportExcelBPO(BPO_Cd);
            }
            else if (Type == "SelectiveBPO")
            {
                fileName = "SelectiveBPO";
                ds = sapIntegrationRepository.ExportExcelSelectiveBPO(BPO_Cd);
            }
            else if (Type == "ConsigneSelect")
            {
                fileName = "ConsigneSelect";
                ds = sapIntegrationRepository.ExportExcelConsigneSelect(BPO_Cd);
            }

            return File(Helpers.CreateExcelFile.ExportToExcelDownload(ds), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName +".xls");

        }

        [HttpPost]
        public JsonResult UploadFile(string Type)
        {
            DataSet output = new DataSet();
            try
            {
                var files = Request.Form.Files;

                if (files.Count > 0)
                {
                    var file = files[0];

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string path = env.WebRootPath + "/ReadWriteData/Files/TempUploadedFiles";
                    string DestinationPath = Path.Combine(path, fileName);

                    using (var fileStream = System.IO.File.Create(DestinationPath))
                    {
                        file.CopyTo(fileStream);
                    }

                    string excelCS = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={DestinationPath};Extended Properties='Excel 12.0 Xml;HDR=YES;IMEX=1';";
                    using (OleDbConnection connExcel = new OleDbConnection(excelCS))
                    {
                        connExcel.Open();
                        DataTable dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                        connExcel.Close();

                        using (OleDbDataAdapter da = new OleDbDataAdapter())
                        {
                            da.SelectCommand = new OleDbCommand($"SELECT * From [{dtExcelSchema.Rows[0]["TABLE_NAME"]}]");
                            da.SelectCommand.Connection = connExcel;

                            da.Fill(output);
                        }
                    }

                    return new JsonResult("IMM File Uploaded successfully!");
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "SAPIntegration", "UploadIMMFile", 1, string.Empty);
                return Json(ex);
            }

            return Json("Index");
        }


    }
}
