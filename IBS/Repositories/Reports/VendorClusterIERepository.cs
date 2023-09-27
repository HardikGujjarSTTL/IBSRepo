using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Interfaces.Reports;
using IBS.Interfaces.Transaction;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using static IBS.Helper.Enums;

namespace IBS.Repositories.Reports
{
    public class VendorClusterIERepository : IVendorClusterIERepository
    {
        private readonly ModelContext context;

        public VendorClusterIERepository(ModelContext context)
        {
            this.context = context;
        }

        public VendorClusterReportModel GetVendorClusterReport(string departreport,string Region)
        {
            VendorClusterReportModel model = new();
            List<VendorClusterList> lstVendorClusterList = new();

            DataSet ds = null;
            DataTable dt = new DataTable();

            OracleParameter[] par = new OracleParameter[3];
            par[0] = new OracleParameter("p_region", OracleDbType.Varchar2,Region, ParameterDirection.Input);
            par[1] = new OracleParameter("p_department", OracleDbType.Varchar2, departreport, ParameterDirection.Input);
            par[2] = new OracleParameter("p_ResultSet", OracleDbType.RefCursor, ParameterDirection.Output);

            ds = DataAccessDB.GetDataSet("GetVendorClusterAllReport", par, 1);
           

            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
                List<VendorClusterList> listcong = dt.AsEnumerable().Select(row => new VendorClusterList
                {
                    Department = Convert.ToString(row["DEPARTMENT"]),
                    Cluster_name = Convert.ToString(row["CLUSTER_NAME"]),
                    geographical_partition = Convert.ToString(row["geographical_partition"]),
                    Vend_cd = Convert.ToString(row["VEND_CD"]),
                    vendor = Convert.ToString(row["VEND_NAME"]),
                    vend_add1 = Convert.ToString(row["VEND_ADD1"]),
                    city = Convert.ToString(row["CITY"]),
                    Ie_name = Convert.ToString(row["IE_NAME"]),

                }).ToList();
                model.lstVendorClusterList = listcong;
            }

            return model;
        }
    }
}
