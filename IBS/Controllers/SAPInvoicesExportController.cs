using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Net;
using System.Web;

namespace IBS.Controllers
{
    public class SAPInvoicesExportController : BaseController
    {
        private readonly ISAPInvoicesExportRepository sapIntegrationRepository;

        public SAPInvoicesExportController(ISAPInvoicesExportRepository _sapIntegrationRepository)
        {
            sapIntegrationRepository = _sapIntegrationRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ExportToExcel(DateTime? FromDt, DateTime? ToDt)
        {
            DataSet ds = new DataSet();
            string FileName = "SAPInvoiceList" + Guid.NewGuid().ToString();

            MyData invoices = new MyData();
            string apiUrl = "https://ibs2.php-staging.com/ibs2api/Invoices_SAP/getinvoicelist?from=" + FromDt + "&to=" + ToDt;

            try
            {
                string jsonData;
                using (var webClient = new WebClient())
                {
                    jsonData = webClient.DownloadString(apiUrl);
                }
                invoices = JsonConvert.DeserializeObject<MyData>(jsonData);

                string json = JsonConvert.SerializeObject(invoices.data);
                DataTable dt = JsonConvert.DeserializeObject<DataTable>(json);
                dt.TableName = "Invoices"; 
                ds.Tables.Add(dt);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching or processing data: " + ex.Message);
                throw;
            }

            return File(Helpers.CreateExcelFile.ExportToExcelDownload(ds), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", FileName + ".xls"); ;
        }
    }
}
