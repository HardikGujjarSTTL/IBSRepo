using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces.Reports;
using IBS.Models;
using IBS.Models.Reports;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using static IBS.Helper.Enums;

namespace IBS.Repositories.Reports
{
    public class IEWiseTrainingReportRepository : IIEWiseTrainingReportRepository
    {
        private readonly ModelContext context;

        public IEWiseTrainingReportRepository(ModelContext context)
        {
            this.context = context;
        }

        public IEWiseTrainingReportModel GetIEWiseTrainingDetails(string IENAME, string TrainingArea, string Mechanical, string Electrical, string Civil, string Regular, string Deputaion, string Particularie, string ParticularArea, string Region)
        {
            IEWiseTrainingReportModel model = new();
            List<IEWISETRAINING> lstIEWISETRAINING = new();

            DataSet ds = null;
            DataTable dt = new DataTable();

            OracleParameter[] par = new OracleParameter[11];
            par[0] = new OracleParameter("p_region", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            par[1] = new OracleParameter("rdbMech", OracleDbType.Varchar2, Mechanical, ParameterDirection.Input);
            par[2] = new OracleParameter("rdbElec", OracleDbType.Varchar2, Electrical, ParameterDirection.Input);
            par[3] = new OracleParameter("rdbCiv", OracleDbType.Varchar2, Civil, ParameterDirection.Input);
            par[4] = new OracleParameter("rdbPIE", OracleDbType.Varchar2, Particularie, ParameterDirection.Input);
            par[5] = new OracleParameter("rdbRegular", OracleDbType.Varchar2, Regular, ParameterDirection.Input);
            par[6] = new OracleParameter("rdbDepu", OracleDbType.Varchar2, Deputaion, ParameterDirection.Input);
            par[7] = new OracleParameter("rdbPArea", OracleDbType.Varchar2, ParticularArea, ParameterDirection.Input);
            par[8] = new OracleParameter("lstIE", OracleDbType.Varchar2, IENAME, ParameterDirection.Input);
            par[9] = new OracleParameter("lstarea", OracleDbType.Varchar2,TrainingArea, ParameterDirection.Input);
            par[10] = new OracleParameter("p_result", OracleDbType.RefCursor, ParameterDirection.Output);

            ds = DataAccessDB.GetDataSet("GetIEWiseTrainingReports", par, 1);


            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
                List<IEWISETRAINING> listcong = dt.AsEnumerable().Select(row => new IEWISETRAINING
                {
                    NAME = Convert.ToString(row["NAME"]),
                    EMP_NO = Convert.ToString(row["EMP_NO"]),
                    CATEGORY = Convert.ToString(row["CATEGORY"]),
                    QUAL = Convert.ToString(row["QUAL"]),
                    T_TYPE = Convert.ToString(row["T_TYPE"]),
                    T_FEILD = Convert.ToString(row["T_FEILD"]),
                    COURSE_NAME = Convert.ToString(row["COURSE_NAME"]),
                    COURSE_INSTITUTE = Convert.ToString(row["COURSE_INSTITUTE"]),
                    C_DUR_FR = Convert.ToString(row["C_DUR_FR"]),
                    C_DUR_TO = Convert.ToString(row["C_DUR_TO"]),
                    CERTIFICATE = Convert.ToString(row["CERTIFICATE"]),
                    FEES = Convert.ToString(row["FEES"]),
                    VALIDITY = Convert.ToString(row["VALIDITY"]),
                }).ToList();
                model.lstIEWISETRAINING = listcong;
            }

            return model;
        }
    }
}
