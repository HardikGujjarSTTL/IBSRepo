using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace IBS.Repositories
{
    public class DEOVendorPurchesOrderRepository : IDEOVendorPurchesOrderRepository
    {
        private readonly ModelContext context;
        private readonly IWebHostEnvironment env;
        private readonly IConfiguration _configuration;

        public DEOVendorPurchesOrderRepository(ModelContext context, IWebHostEnvironment _environment, IConfiguration configuration)
        {
            this.context = context;
            env = _environment;
            _configuration = configuration;
        }

        public DTResult<DEOVendorPurchesOrderModel> GetDataList(DTParameters dtParameters, string GetRegionCode, string RootHostName)
        {

            DTResult<DEOVendorPurchesOrderModel> dTResult = new() { draw = 0 };
            IQueryable<DEOVendorPurchesOrderModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "CaseNo";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "CaseNo";
                orderAscendingDirection = true;
            }

            string CaseNo = "", PoNo = "", PoDt = "";
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["CaseNo"]))
            {
                CaseNo = Convert.ToString(dtParameters.AdditionalValues["CaseNo"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["PoNo"]))
            {
                PoNo = Convert.ToString(dtParameters.AdditionalValues["PoNo"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["PoDt"]))
            {
                PoDt = Convert.ToString(dtParameters.AdditionalValues["PoDt"]);
            }

            OracleParameter[] par = new OracleParameter[8];
            
            par[0] = new OracleParameter("p_CASE_NO", OracleDbType.Varchar2, CaseNo.ToString() == "" ? DBNull.Value : CaseNo.ToString(), ParameterDirection.Input);
            par[1] = new OracleParameter("p_PO_NO", OracleDbType.Varchar2, PoNo.ToString() == "" ? DBNull.Value : PoNo.ToString(), ParameterDirection.Input);
            par[2] = new OracleParameter("p_PO_DT", OracleDbType.Varchar2, PoDt.ToString() == "" ? DBNull.Value : PoDt.ToString(), ParameterDirection.Input);
            par[3] = new OracleParameter("p_region_code", OracleDbType.Varchar2, GetRegionCode, ParameterDirection.Input);
            par[4] = new OracleParameter("p_page_start", OracleDbType.Int32, dtParameters.Start + 1, ParameterDirection.Input);
            par[5] = new OracleParameter("p_page_end", OracleDbType.Int32, (dtParameters.Start + dtParameters.Length), ParameterDirection.Input);
            par[6] = new OracleParameter("p_result_cursor", OracleDbType.RefCursor, ParameterDirection.Output);
            par[7] = new OracleParameter("p_result_records", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("SP_GET_DEO_VENDOR_PURCHASE_ORDERS", par, 2);
            List<DEOVendorPurchesOrderModel> list = new();
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                list = JsonConvert.DeserializeObject<List<DEOVendorPurchesOrderModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                if (list.Count > 0)
                {
                    string HostUrl = _configuration.GetSection("AppSettings")["SiteUrl"];
                    if (RootHostName.Contains("14.143.90.241"))
                    {
                        HostUrl = HostUrl.Replace("192.168.0.101", "14.143.90.241");
                    }
                    foreach (var item in list)
                    {
                        string fpath = env.WebRootPath + Enums.GetEnumDescription(Enums.FolderPath.VendorPO) + "/" + item.CaseNo.Trim().ToString()+ ".pdf";
                        if (!File.Exists(fpath))
                        {
                            item.IsFileExist = false;
                        }
                        else 
                        {
                            item.IsFileExist = true;
                            item.File = HostUrl + Enums.GetEnumDescription(Enums.FolderPath.VendorPO) + "/" + item.CaseNo.Trim().ToString() + ".pdf";
                        }
                    }
                }
            }
            int recordsTotal = 0;
            if (ds != null && ds.Tables[1].Rows.Count > 0)
            {
                recordsTotal = Convert.ToInt32(ds.Tables[1].Rows[0]["total_records"]);
            }
            query = list.AsQueryable();
            dTResult.recordsTotal = recordsTotal;
            dTResult.recordsFiltered = recordsTotal;
            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Select(p => p).ToList();
            dTResult.draw = dtParameters.Draw;

            //query = from p in context.T80PoMasters
            //        join v in context.T05Vendors on p.VendCd equals v.VendCd
            //        join c in context.T03Cities on v.VendCityCd equals c.CityCd
            //        where (p.Isdeleted == 0 || p.Isdeleted == null) && p.RealCaseNo == null && p.RegionCode == GetRegionCode
            //        //&& Convert.ToDateTime(p.RecvDt) > "01-12-2016"

            //        select new DEOVendorPurchesOrderModel
            //        {
            //            CaseNo = p.CaseNo,
            //            PurchaserCd = p.PurchaserCd,
            //            StockNonstock = p.StockNonstock,
            //            RlyNonrly = p.RlyNonrly.Equals("R") ? "Railway" : p.RlyNonrly.Equals("P") ? "Private" : p.RlyNonrly.Equals("S") ? "State Government" : p.RlyNonrly.Equals("F") ? "Foreign Railways" : p.RlyNonrly.Equals("U") ? "PSU" : p.RlyNonrly,
            //            PoOrLetter = p.PoOrLetter,
            //            PoNo = p.PoNo,
            //            PoDt = p.PoDt,
            //            RecvDt = p.RecvDt,
            //            VendCd = p.VendCd,
            //            RlyCd = p.RlyCd,
            //            RegionCode = p.RegionCode,
            //            UserId = p.UserId,
            //            Datetime = p.Datetime,
            //            Remarks = p.Remarks,
            //            PoiCd = p.PoiCd,
            //            Isdeleted = p.Isdeleted,
            //            Createddate = p.Createddate,
            //            //Createdby = p.Createdby,
            //            Updateddate = p.Updateddate,
            //            //Updatedby = p.Updatedby,
            //            //VendCdNavigation = v,
            //            VendName = v.VendName,
            //            RealCaseNo = p.RealCaseNo

            //        };

            //dTResult.recordsTotal = query.Count();

            //if (!string.IsNullOrEmpty(searchBy))
            //    query = query.Where(w => Convert.ToString(w.CaseNo).ToLower().Contains(searchBy.ToLower())
            //    || Convert.ToString(w.Remarks).ToLower().Contains(searchBy.ToLower())
            //    );

            //dTResult.recordsFiltered = query.Count();

            //dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            //dTResult.draw = dtParameters.Draw;

            return dTResult;
        }
    }
}
