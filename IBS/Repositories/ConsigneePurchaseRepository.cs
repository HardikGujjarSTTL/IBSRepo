using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Drawing;
using System.Xml.Linq;

namespace IBS.Repositories
{
    public class ConsigneePurchaseRepository : IConsigneePurchaseRepository
    {
        private readonly ModelContext context;

        public ConsigneePurchaseRepository(ModelContext context)
        {
            this.context = context;
        }

        public DTResult<ConsigneePurchaseMasterSearchModel> GetConsigneePurchaseList(DTParameters dtParameters)
        {
            DTResult<ConsigneePurchaseMasterSearchModel> dTResult = new() { draw = 0 };
            IQueryable<ConsigneePurchaseMasterSearchModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null && dtParameters.Order.Length > 0)
            {
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                if (string.IsNullOrEmpty(orderCriteria)) orderCriteria = "CONSIGNEE_CD";
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                orderCriteria = "CONSIGNEE_CD";
                orderAscendingDirection = true;
            }

            // string CONSIGNEE_CD = null, CONSIGNEE_DESIG = null, CONSIGNEE_FIRM = null, CITY = null, SAP_CUST_CD_CON = null, GSTIN_NO = null;

            string CONSIGNEE_CD = !string.IsNullOrEmpty(dtParameters.AdditionalValues["CONSIGNEE_CD"]) ? Convert.ToString(dtParameters.AdditionalValues["CONSIGNEE_CD"]) : null;
            string CONSIGNEE_DESIG = !string.IsNullOrEmpty(dtParameters.AdditionalValues["CONSIGNEE_DESIG"]) ? Convert.ToString(dtParameters.AdditionalValues["CONSIGNEE_DESIG"]) : null;
            string CONSIGNEE_FIRM = !string.IsNullOrEmpty(dtParameters.AdditionalValues["CONSIGNEE_FIRM"]) ? Convert.ToString(dtParameters.AdditionalValues["CONSIGNEE_FIRM"]) : null;
            string CITY = !string.IsNullOrEmpty(dtParameters.AdditionalValues["CITY"]) ? Convert.ToString(dtParameters.AdditionalValues["CITY"]) : null;
            string SAP_CUST_CD_CON = !string.IsNullOrEmpty(dtParameters.AdditionalValues["SAP_CUST_CD_CON"]) ? Convert.ToString(dtParameters.AdditionalValues["SAP_CUST_CD_CON"]) : null;
            string GSTIN_NO = !string.IsNullOrEmpty(dtParameters.AdditionalValues["GSTIN_NO"]) ? Convert.ToString(dtParameters.AdditionalValues["GSTIN_NO"]) : null;

            OracleParameter[] par = new OracleParameter[7];
            par[0] = new OracleParameter("P_CONSIGNEE_CD", OracleDbType.Varchar2, CONSIGNEE_CD, ParameterDirection.Input);
            par[1] = new OracleParameter("P_CONSIGNEE_DESIG", OracleDbType.Varchar2, CONSIGNEE_DESIG, ParameterDirection.Input);
            par[2] = new OracleParameter("P_CONSIGNEE_FIRM", OracleDbType.Varchar2, CONSIGNEE_FIRM, ParameterDirection.Input);
            par[3] = new OracleParameter("P_CITY", OracleDbType.Varchar2, CITY, ParameterDirection.Input);
            par[4] = new OracleParameter("P_SAP_CUST_CD_CON", OracleDbType.Varchar2, SAP_CUST_CD_CON, ParameterDirection.Input);
            par[5] = new OracleParameter("P_GSTIN_NO", OracleDbType.Varchar2, GSTIN_NO, ParameterDirection.Input);
            par[6] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("SP_GET_CONSIGNEE_PURCHASE_MASTER", par, 1);
            DataTable dt = ds.Tables[0];

            List<ConsigneePurchaseMasterSearchModel> list = dt.AsEnumerable().Select(row => new ConsigneePurchaseMasterSearchModel
            {
                ROWNO = Convert.ToInt32(row["ROWNO"]),
                CONSIGNEE_CD = row["CONSIGNEE_CD"].ToString(),
                CONSIGNEE_NAME = row["CONSIGNEE_NAME"].ToString(),
                DESIG_DESC = row["DESIG_DESC"].ToString(),
                CONSIGNEE_FIRM = row["CONSIGNEE_FIRM"].ToString(),
                CONSIGNEE_ADD1 = row["CONSIGNEE_ADD1"].ToString(),
                CONSIGNEE_CITY = row["CONSIGNEE_CITY"].ToString(),
                GSTIN_NO = row["GSTIN_NO"].ToString()
            }).ToList();

            query = list.AsQueryable();

            dTResult.recordsTotal = ds.Tables[0].Rows.Count;

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.CONSIGNEE_CD).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.CONSIGNEE_CITY).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = ds.Tables[0].Rows.Count;

            if (dtParameters.Length == -1) dtParameters.Length = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public ConsigneePurchaseModel FindByID(int Id)
        {
            ConsigneePurchaseModel model = new();
            T06Consignee consignee = context.T06Consignees.Find(Id);

            if (consignee == null)
                return model;
            else
            {
                model.ConsigneeCd = consignee.ConsigneeCd;
                model.ConsigneeType = consignee.ConsigneeType;
                model.ConsigneeRailwayCD = consignee.ConsigneeType == "R" ? consignee.ConsigneeFirm : "";
                model.FName = consignee.ConsigneeType != "R" ? consignee.ConsigneeFirm : "";
                model.ConsigneeDesig = consignee.ConsigneeType == "R" ? consignee.ConsigneeDesig : "";
                model.CSName = consignee.ConsigneeType != "R" ? consignee.ConsigneeDesig : "";
                model.ConsigneeDept = consignee.ConsigneeDept;
                model.ConsigneeCity = consignee.ConsigneeCity;
                model.ConsigneeState = Get_State(model.ConsigneeCity != null ? model.ConsigneeCity.ToString() : "");
                model.ConsigneeAdd1 = consignee.ConsigneeAdd1;
                model.ConsigneeAdd2 = consignee.ConsigneeAdd2;
                model.PinCode = consignee.PinCode;
                model.GstinNo = consignee.GstinNo;

                return model;
            }
        }

        public string Get_State(string city_CD)
        {
            var state = context.T92States
                        .Where(state => state.StateCd == context.T03Cities
                            .Where(city => city.CityCd == Convert.ToInt32(city_CD))
                            .Select(city => city.StateCd)
                            .FirstOrDefault())
                        .Select(state => Convert.ToString(state.StateCd).PadLeft(2, '0') + "-" + state.StateName)
                        .FirstOrDefault();
            return state;
        }

        public int SaveDetails(ConsigneePurchaseModel model)
        {
            if (model.ConsigneeCd == 0)
            {
                int maxID = context.T06Consignees.Max(x => x.ConsigneeCd) + 1;

                T06Consignee consignee = new()
                {
                    ConsigneeCd = maxID,
                    ConsigneeType = model.ConsigneeType,
                    ConsigneeDesig = model.ConsigneeType == "R" ? model.ConsigneeDesig : Common.ConvertToUpper(model.CSName),
                    ConsigneeDept = Common.ConvertToUpper(model.ConsigneeDept),
                    ConsigneeFirm = model.ConsigneeType == "R" ? model.ConsigneeRailwayCD : Common.ConvertToUpper(model.FName),
                    ConsigneeAdd1 = Common.ConvertToUpper(model.ConsigneeAdd1),
                    ConsigneeAdd2 = Common.ConvertToUpper(model.ConsigneeAdd2),
                    ConsigneeCity = model.ConsigneeCity,
                    GstinNo = Common.ConvertToUpper(model.GstinNo),
                    LegalName = model.ConsigneeType == "R" ? "MINISTRY OF RAILWAYS" : "",
                    PinCode = model.PinCode,
                    UserId = model.UserId,
                    Datetime = DateTime.Now,
                    Createdby = model.Createdby,
                    Createddate = DateTime.Now,
                };

                context.T06Consignees.Add(consignee);
                context.SaveChanges();
            }
            else
            {
                T06Consignee consignee = context.T06Consignees.Find(model.ConsigneeCd);

                if (consignee != null)
                {
                    consignee.ConsigneeType = model.ConsigneeType;
                    consignee.ConsigneeDesig = model.ConsigneeType == "R" ? model.ConsigneeDesig : Common.ConvertToUpper(model.CSName);
                    consignee.ConsigneeDept = Common.ConvertToUpper(model.ConsigneeDept);
                    consignee.ConsigneeFirm = model.ConsigneeType == "R" ? model.ConsigneeRailwayCD : Common.ConvertToUpper(model.FName);
                    consignee.ConsigneeAdd1 = Common.ConvertToUpper(model.ConsigneeAdd1);
                    consignee.ConsigneeAdd2 = Common.ConvertToUpper(model.ConsigneeAdd2);
                    consignee.ConsigneeCity = model.ConsigneeCity;
                    consignee.UserId = model.UserId;
                    consignee.Datetime = DateTime.Now;
                    consignee.GstinNo = Common.ConvertToUpper(model.GstinNo);
                    consignee.LegalName = model.ConsigneeType == "R" ? "MINISTRY OF RAILWAYS" : "";
                    consignee.PinCode = model.PinCode;
                    consignee.Updatedby = model.Updatedby;
                    consignee.Updateddate = DateTime.Now;

                    context.SaveChanges();
                }
            }

            return model.ConsigneeCd;
        }

        public bool Remove(int Id, int UserID)
        {
            T06Consignee consignee = context.T06Consignees.Find(Id);

            if (consignee == null) { return false; }

            consignee.Isdeleted = 1;
            consignee.Updatedby = UserID;
            consignee.Updateddate = DateTime.Now;

            context.SaveChanges();
            return true;
        }
    }
}
