using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces.Reports.OtherReports;
using IBS.Models.Reports;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace IBS.Repositories.Reports.OtherReports
{
    public class OtherReportsRepository : IOtherReportsRepository
    {
        private readonly ModelContext context;

        public OtherReportsRepository(ModelContext context)
        {
            this.context = context;
        }

        public ControllingOfficerIEModel GetControllingOfficerWiseIE(string Region)
        {
            ControllingOfficerIEModel model = new();
            List<ControllingOfficerModel> lstControllingOfficer = new();
            List<ControllingOfficerModel> lstCluster = new();
            List<IEModel> lstLocalIE = new();
            List<IEModel> lstOutstationIE = new();
            model.Region = EnumUtility<Enums.Region>.GetDescriptionByKey(Region);
            OracleParameter[] parameter = new OracleParameter[5];
            parameter[0] = new OracleParameter("P_REGION", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            parameter[1] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
            parameter[2] = new OracleParameter("P_TBL1_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
            parameter[3] = new OracleParameter("P_TBL2_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
            parameter[4] = new OracleParameter("P_TBL3_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            DataSet ds = DataAccessDB.GetDataSet("SP_GET_CONTROLLING_OFFICER_WISE_IE", parameter, 4);
            string serializeddt1 = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
            lstControllingOfficer = JsonConvert.DeserializeObject<List<ControllingOfficerModel>>(serializeddt1, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            model.lstControllingOfficer = lstControllingOfficer;

            string serializeddt2 = JsonConvert.SerializeObject(ds.Tables[1], Formatting.Indented);
            lstCluster = JsonConvert.DeserializeObject<List<ControllingOfficerModel>>(serializeddt2, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            model.lstCluster = lstCluster;

            string serializeddt3 = JsonConvert.SerializeObject(ds.Tables[2], Formatting.Indented);
            lstLocalIE = JsonConvert.DeserializeObject<List<IEModel>>(serializeddt3, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            model.lstLocalIE = lstLocalIE;

            string serializeddt4 = JsonConvert.SerializeObject(ds.Tables[3], Formatting.Indented);
            lstOutstationIE = JsonConvert.DeserializeObject<List<IEModel>>(serializeddt4, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            model.lstOutstationIE = lstOutstationIE;
            return model;
        }
    }
}
