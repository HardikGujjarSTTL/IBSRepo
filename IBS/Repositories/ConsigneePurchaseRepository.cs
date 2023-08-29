using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Drawing;

namespace IBS.Repositories
{
    public class ConsigneePurchaseRepository : IConsigneePurchaseRepository
    {
        private readonly ModelContext context;
        private readonly IConfiguration configuration;
        public ConsigneePurchaseRepository(ModelContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }


        public DTResult<ConsigneePurchaseMasterSearchModel> Get_Consignee_Purchase(DTParameters dtParameters)
        {
            DTResult<ConsigneePurchaseMasterSearchModel> dTResult = new() { draw = 0 };
            IQueryable<ConsigneePurchaseMasterSearchModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "ROWNO";
                }
                //orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "ROWNO";
                orderAscendingDirection = true;
            }

            string CONSIGNEE_CD = null, CONSIGNEE_DESIG = null, CONSIGNEE_FIRM = null, CITY = null, SAP_CUST_CD_CON = null, GSTIN_NO = null;

            CONSIGNEE_CD = !string.IsNullOrEmpty(dtParameters.AdditionalValues["CONSIGNEE_CD"]) ? Convert.ToString(dtParameters.AdditionalValues["CONSIGNEE_CD"]) : null;
            CONSIGNEE_DESIG = !string.IsNullOrEmpty(dtParameters.AdditionalValues["CONSIGNEE_DESIG"]) ? Convert.ToString(dtParameters.AdditionalValues["CONSIGNEE_DESIG"]) : null;
            CONSIGNEE_FIRM = !string.IsNullOrEmpty(dtParameters.AdditionalValues["CONSIGNEE_FIRM"]) ? Convert.ToString(dtParameters.AdditionalValues["CONSIGNEE_FIRM"]) : null;
            CITY = !string.IsNullOrEmpty(dtParameters.AdditionalValues["CITY"]) ? Convert.ToString(dtParameters.AdditionalValues["CITY"]) : null;
            SAP_CUST_CD_CON = !string.IsNullOrEmpty(dtParameters.AdditionalValues["SAP_CUST_CD_CON"]) ? Convert.ToString(dtParameters.AdditionalValues["SAP_CUST_CD_CON"]) : null;
            GSTIN_NO = !string.IsNullOrEmpty(dtParameters.AdditionalValues["GSTIN_NO"]) ? Convert.ToString(dtParameters.AdditionalValues["GSTIN_NO"]) : null;

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


            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.CONSIGNEE_CD).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.CONSIGNEE_CITY).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsTotal = ds.Tables[0].Rows.Count;
            dTResult.recordsFiltered = ds.Tables[0].Rows.Count;
            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();
            dTResult.draw = dtParameters.Draw;
            return dTResult;
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
        public ConsigneePurchaseModel Get_Consignee_Purchase_Detail(int Consignee_CD)
        {
            var result = new ConsigneePurchaseModel();
            result = (from a in context.T06Consignees
                      where a.ConsigneeCd == Consignee_CD
                      select new ConsigneePurchaseModel
                      {
                          ConsigneeCd = a.ConsigneeCd,
                          ConsigneeType = a.ConsigneeType,
                          ConsigneeDesig = a.ConsigneeType == "R" ? a.ConsigneeDesig : "",
                          ConsigneeDept = a.ConsigneeDept,
                          ConsigneeRailwayCD = a.ConsigneeType == "R" ? a.ConsigneeFirm : "",
                          ConsigneeAdd1 = a.ConsigneeAdd1,
                          ConsigneeAdd2 = a.ConsigneeAdd2,
                          ConsigneeCity = a.ConsigneeCity,
                          GstinNo = a.GstinNo,
                          SapCustCdCon = a.SapCustCdCon,
                          PinCode = a.PinCode,
                          CSName = a.ConsigneeType != "R" ? a.ConsigneeDesig : "",
                          FName = a.ConsigneeType != "R" ? a.ConsigneeFirm : "",
                      }).FirstOrDefault();
            
            return result;
        }

        public int ConsigneePurchaseDelete(int Consignee_CD)
        {
            var flag = 0;
            try
            {
                var _data = context.T06Consignees.Where(x => x.ConsigneeCd == Consignee_CD).Select(x => x).FirstOrDefault();
                if (_data != null)
                {
                    _data.Isdeleted = Convert.ToByte(1);
                    context.SaveChanges();
                    flag = 1;
                }
                flag = -1;
            }
            catch (Exception ex)
            {
                flag = 500;
            }
            return flag;
        }

        public int CongsigneePurchaseInsertUpdate(ConsigneePurchaseModel model, UserSessionModel uModel)
        {
            int ID = 0;
            var data = context.T06Consignees.Find(model.ConsigneeCd);
            string w_Legal_Name = "", rly = "", CName = "";
            if (model.ConsigneeType == "R")
            {
                w_Legal_Name = "MINISTRY OF RAILWAYS";
            }

            if (model.ConsigneeType == "R")
            {
                rly = model.ConsigneeRailwayCD;//lstRailwayCD.SelectedValue;
                CName = Convert.ToString(model.ConsigneeDesig);//lstCAName.SelectedValue;
            }
            else
            {
                rly = model.FName;//txtFName.Text;
                CName = model.CSName; //txtCSName.Text;
            }
            if (data == null)
            {
                int maxID = context.T06Consignees.Max(x => x.ConsigneeCd) + 1;
                T06Consignee obj = new T06Consignee();
                obj.ConsigneeCd = maxID;
                obj.ConsigneeType = model.ConsigneeType;
                obj.ConsigneeDesig = CName;
                obj.ConsigneeDept = model.ConsigneeDept;
                obj.ConsigneeFirm = rly;
                obj.ConsigneeAdd1 = model.ConsigneeAdd1;
                obj.ConsigneeAdd2 = model.ConsigneeAdd2;
                obj.ConsigneeCity = model.ConsigneeCity;
                obj.UserId = uModel.USER_ID;
                obj.Datetime = DateTime.Now;
                obj.GstinNo = model.GstinNo;
                obj.LegalName = w_Legal_Name;
                obj.PinCode = model.PinCode;
                obj.Createdby = uModel.UserID;
                obj.Createddate = DateTime.Now;
                context.T06Consignees.Add(obj);
                context.SaveChanges();
                ID = obj.ConsigneeCd;
            }
            else
            {
                data.ConsigneeType = model.ConsigneeType;               
                data.ConsigneeDesig = CName;
                data.ConsigneeDept = model.ConsigneeDept;
                data.ConsigneeFirm = rly;
                data.ConsigneeAdd1 = model.ConsigneeAdd1;
                data.ConsigneeAdd2 = model.ConsigneeAdd2;
                data.ConsigneeCity = model.ConsigneeCity;
                data.UserId = uModel.USER_ID;
                data.Datetime = DateTime.Now;
                data.GstinNo = model.GstinNo;
                data.LegalName = w_Legal_Name;
                data.PinCode = model.PinCode;
                data.Updatedby = uModel.UserID;
                data.Updateddate = DateTime.Now;
                context.SaveChanges();
                ID = data.ConsigneeCd;
            }
            return ID;
        }
    }
}
