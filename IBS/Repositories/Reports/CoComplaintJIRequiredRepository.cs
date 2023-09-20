using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Interfaces.Reports;
using IBS.Interfaces.Transaction;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using static IBS.Helper.Enums;

namespace IBS.Repositories.Reports
{
    public class CoComplaintJIRequiredRepository : ICoComplaintJIRequiredRepository
    {
        private readonly ModelContext context;

        public CoComplaintJIRequiredRepository(ModelContext context)
        {
            this.context = context;
        }
        public JIRequiredReport GetJIComplaintsList(string FinancialYearsText, string FinancialYearsValue)
        {
            JIRequiredReport model = new();
            List<JIRequiredList> lstJIRequiredList = new();
            List<JIRequiredList> lstCompList = new();
            List<ComplaintJIIE> lstComplaintJIIE = new();
            DataSet dsA = null;
            DataSet dsB = null;
            DataSet dsC = null;
            DataTable dt = new DataTable();

            OracleParameter[] parameter = new OracleParameter[3];
            parameter[0] = new OracleParameter("p_Finyear_Value", OracleDbType.Varchar2, FinancialYearsValue, ParameterDirection.Input);
            parameter[1] = new OracleParameter("p_FinYear_Text", OracleDbType.Varchar2, FinancialYearsText, ParameterDirection.Input);
            parameter[2] = new OracleParameter("p_result", OracleDbType.RefCursor, ParameterDirection.Output);

            dsA = DataAccessDB.GetDataSet("JI_Complaint_Report_A", parameter, 1);

            OracleParameter[] parameter1 = new OracleParameter[2];
            parameter1[0] = new OracleParameter("p_Fin_Year", OracleDbType.Varchar2, FinancialYearsText, ParameterDirection.Input);
            parameter1[1] = new OracleParameter("p_result", OracleDbType.RefCursor, ParameterDirection.Output);

            dsB = DataAccessDB.GetDataSet("JI_Complaint_Report_B", parameter1, 1);

            OracleParameter[] parameter2 = new OracleParameter[3];
            parameter2[0] = new OracleParameter("p_Fin_Year", OracleDbType.Varchar2, FinancialYearsValue, ParameterDirection.Input);
            parameter2[1] = new OracleParameter("p_FinYear_Text", OracleDbType.Varchar2, FinancialYearsText, ParameterDirection.Input);
            parameter2[2] = new OracleParameter("p_result", OracleDbType.RefCursor, ParameterDirection.Output);

            dsC = DataAccessDB.GetDataSet("JI_Complaint_Report_C", parameter2, 1);

            if (dsA != null && dsA.Tables.Count > 0)
            {
                dt = dsA.Tables[0];
                List<JIRequiredList> listcong = dt.AsEnumerable().Select(row => new JIRequiredList
                {
                    Region = Convert.ToString(row["Region"]),
                    NO_OF_INSPECTION = Convert.ToDecimal(row["NO_OF_INSPECTION"]),
                    MATERIAL_VALUE = Convert.ToDecimal(row["MATERIAL_VALUE"]),
                    RECD = Convert.ToInt32(row["RECD"]),
                    FINALISED = Convert.ToInt32(row["FINALISED"]),
                    PENDING = Convert.ToInt32(row["PENDING"]),
                    ACCEPTED = Convert.ToInt32(row["ACCEPTED"]),
                    UPHELD = Convert.ToInt32(row["UPHELD"]),
                    SORTING = Convert.ToInt32(row["SORTING"]),
                    RECTIFICATION = Convert.ToInt32(row["RECTIFICATION"]),
                    PRICE_REDUCTION = Convert.ToInt32(row["PRICE_REDUCTION"]),
                    LIFTED_BEFORE_JI = Convert.ToInt32(row["LIFTED_BEFORE_JI"]),
                    NOT_ON_RITES_AC = Convert.ToInt32(row["NOT_ON_RITES_AC"]),
                    TRANSIT_DEMAGE = Convert.ToInt32(row["TRANSIT_DEMAGE"]),
                    UNSTAMPED = Convert.ToInt32(row["UNSTAMPED"]),
                }).ToList();
                foreach (var item in listcong)
                {
                    item.Total = item.UPHELD + item.SORTING + item.RECTIFICATION + item.PRICE_REDUCTION+ item.LIFTED_BEFORE_JI;
                }
                model.lstJIRequiredList = listcong;
            }

            if (dsB != null && dsB.Tables.Count > 0)
            {
                dt = dsB.Tables[0];
                List<JIRequiredList> listcong1 = dt.AsEnumerable().Select(row => new JIRequiredList
                {
                    Region = Convert.ToString(row["Region"]),
                    DEFECT_DESC = Convert.ToString(row["DEFECT_DESC"]),
                    UPHELD = Convert.ToInt32(row["UPHELD"]),
                    SORTING = Convert.ToInt32(row["SORTING"]),
                    RECTIFICATION = Convert.ToInt32(row["RECTIFICATION"]),
                    PRICE_REDUCTION = Convert.ToInt32(row["PRICE_REDUCTION"]),
                }).ToList();
                foreach (var item in listcong1)
                {
                    item.Total = item.UPHELD + item.SORTING + item.RECTIFICATION + item.PRICE_REDUCTION;
                }
                model.lstCompList = listcong1;
            }

            if (dsC != null && dsC.Tables.Count > 0)
            {
                dt = dsC.Tables[0];
                List<ComplaintJIIE> listcong = dt.AsEnumerable().Select(row => new ComplaintJIIE
                {
                    Region = Convert.ToString(row["Region"]),
                    S_Code = Convert.ToString(row["S_Code"]),
                    IE = Convert.ToString(row["IE"]),
                    NO_OF_INSPECTION = Convert.ToInt32(row["NO_OF_INSPECTION"]),
                    MATERIAL_VALUE = Convert.ToInt32(row["MATERIAL_VALUE"]),
                    RECD = Convert.ToInt32(row["RECD"]),
                    FINALISED = Convert.ToInt32(row["FINALISED"]),
                    PENDING = Convert.ToInt32(row["PENDING"]),
                    ACCEPTED = Convert.ToInt32(row["ACCEPTED"]),
                    UPHELD = Convert.ToInt32(row["UPHELD"]),
                    SORTING = Convert.ToInt32(row["SORTING"]),
                    RECTIFICATION = Convert.ToInt32(row["RECTIFICATION"]),
                    PRICE_REDUCTION = Convert.ToInt32(row["PRICE_REDUCTION"]),
                    LIFTED_BEFORE_JI = Convert.ToInt32(row["LIFTED_BEFORE_JI"]),
                    TRANSIT_DEMAGE = Convert.ToInt32(row["TRANSIT_DEMAGE"]),
                    UNSTAMPED = Convert.ToInt32(row["UNSTAMPED"]),
                    NOT_ON_RITES_AC = Convert.ToInt32(row["NOT_ON_RITES_AC"]),
                }).ToList();
                foreach (var item in listcong)
                {
                    item.Total = item.UPHELD + item.SORTING + item.RECTIFICATION + item.PRICE_REDUCTION+ item.LIFTED_BEFORE_JI;
                }
                model.lstComplaintJIIE = listcong;
            }

            return model;
        }
    }
}
