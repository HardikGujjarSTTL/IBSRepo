using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Interfaces.Reports;
using IBS.Interfaces.Reports.ConsigneeComplaintReports;
using IBS.Models;
using IBS.Models.Reports;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Globalization;

namespace IBS.Repositories.Reports.ConsigneeComplaintReports
{
    public class ConsigneeCompReportRepository : IConsigneeCompReportRepository
    {
        private readonly ModelContext context;

        public ConsigneeCompReportRepository(ModelContext context)
        {
            this.context = context;
        }

        public string GetItems(string Clientwise)
        {
            List<SelectListItem> qry = null;
            if (Clientwise == "R")
            {
                qry = (from a in context.T91Railways
                       select new SelectListItem
                       {
                           Text = Convert.ToString(a.Railway),
                           Value = Convert.ToString(a.RlyCd)
                       }).OrderBy(c => c.Value).ToList();
            }
            else
            {
                qry = (from bpo in context.T12BillPayingOfficers
                       where bpo.BpoType == Clientwise
                       select new SelectListItem
                       {
                           Text = bpo.BpoOrgn,
                           Value = bpo.BpoRly.Trim().ToUpper()
                       })
                        .Distinct()
                        .OrderBy(item => item.Value)
                        .ToList();

            }
            string json = JsonConvert.SerializeObject(qry, Formatting.Indented);
            return json;
        }

        public HighValueInspReport GetHighValueInspdata(string month, string year, string valinsp, string FromDate, string ToDate, string ICDate, string BillDate, string formonth, string forperiod, string Region)
        {
            HighValueInspReport model = new();
            List<ValueInspList> lstValueInspList = new();

            DataSet ds = null;
            DataTable dt = new DataTable();

            model.month = month; model.year = year; model.valinsp = valinsp; model.FromDate = FromDate; model.ToDate = ToDate; model.ICDate = ICDate; model.BillDate = BillDate; model.formonth = formonth; model.forperiod = forperiod;

            OracleParameter[] par = new OracleParameter[7];
            par[0] = new OracleParameter("p_region", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            par[1] = new OracleParameter("p_rdoBill", OracleDbType.Varchar2, BillDate, ParameterDirection.Input);
            par[2] = new OracleParameter("p_rdoDt", OracleDbType.Varchar2, forperiod, ParameterDirection.Input);
            par[3] = new OracleParameter("p_fromdt", OracleDbType.Varchar2, FromDate, ParameterDirection.Input);
            par[4] = new OracleParameter("p_todate", OracleDbType.Varchar2, ToDate, ParameterDirection.Input);
            par[5] = new OracleParameter("p_monthyear", OracleDbType.Varchar2, year + month, ParameterDirection.Input);
            par[6] = new OracleParameter("p_result", OracleDbType.RefCursor, ParameterDirection.Output);

            ds = DataAccessDB.GetDataSet("GetNHighValueInspReport", par, 1);

            int recCount = 0;
            List<ValueInspList> listcong = new List<ValueInspList>();

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                if (recCount < Convert.ToInt32(valinsp))
                {
                    dt = ds.Tables[0];
                    ValueInspList valueInsp = new ValueInspList
                    {
                        // Populate the properties based on the row data
                        BillNo = row.Field<string>("BILL_NO"),
                        BillDate = DateTime.TryParseExact(row.Field<string>("BILL_DT"), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateValue)
                            ? dateValue
                            : (DateTime?)null,
                        CaseNo = row.Field<string>("CASE_NO"),
                        EngName = row.Field<string>("IE_NAME"),
                        VENDOR = row.Field<string>("VENDOR"),
                        CONSIGNEE = row.Field<string>("CONSIGNEE"),
                        ITEMDESC = row.Field<string>("ITEM_DESC"),
                        MATERIALVALUE = row.Field<decimal>("MATERIAL_VALUE").ToString(), // Convert to string if needed
                        PLNO = row.Field<string>("PL_NO"),
                        INSPFEE = row.Field<decimal>("INSP_FEE").ToString(), // Convert to string if needed
                        ICDate = DateTime.TryParseExact(row.Field<string>("IC_DT"), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateValues)
                        ? dateValues
                        : (DateTime?)null
                    };
                    listcong.Add(valueInsp);
                    recCount++;
                }
                else
                {
                    break;
                }
            }
            model.lstValueInspList = listcong;
            return model;
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
                    item.Total = item.UPHELD + item.SORTING + item.RECTIFICATION + item.PRICE_REDUCTION + item.LIFTED_BEFORE_JI;
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
                    TRANSIT_DEMAGE = Convert.ToInt32(row["TRANSIT_DEMAGE"]),
                    UNSTAMPED = Convert.ToInt32(row["UNSTAMPED"]),
                    NOT_ON_RITES_AC = Convert.ToInt32(row["NOT_ON_RITES_AC"]),
                }).ToList();
                foreach (var item in listcong)
                {
                    item.Total = item.UPHELD + item.SORTING + item.RECTIFICATION + item.PRICE_REDUCTION + item.LIFTED_BEFORE_JI;
                }
                model.lstComplaintJIIE = listcong;
            }

            return model;
        }

        public DefectCodeReport GetDefectCodeWiseData(DateTime FromDate, DateTime ToDate, string Region)
        {
            DefectCodeReport model = new();
            List<DefectCodeList> lstDefectCodeList = new();
            DataSet ds = null;
            DataTable dt = new DataTable();

            OracleParameter[] parameter = new OracleParameter[4];
            parameter[0] = new OracleParameter("p_From_date", OracleDbType.Varchar2, FromDate.ToString("dd/MM/yyyy"), ParameterDirection.Input);
            parameter[1] = new OracleParameter("p_To_date", OracleDbType.Varchar2, ToDate.ToString("dd/MM/yyyy"), ParameterDirection.Input);
            parameter[2] = new OracleParameter("p_Region", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            parameter[3] = new OracleParameter("p_result", OracleDbType.RefCursor, ParameterDirection.Output);

            ds = DataAccessDB.GetDataSet("Defect_Code_Wise_Report", parameter, 1);

            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
                List<DefectCodeList> listcong = dt.AsEnumerable().Select(row => new DefectCodeList
                {
                    Code = Convert.ToString(row["DEFECT_DESC"]),
                    Upheld = Convert.ToDecimal(row["UPHELD"]),
                    Sorting = Convert.ToDecimal(row["SORTING"]),
                    Rectification = Convert.ToDecimal(row["RECTIFICATION"]),
                    PriceReduction = Convert.ToDecimal(row["PRICE_REDUCTION"]),
                }).ToList();
                foreach (var item in listcong)
                {
                    item.Total = (decimal)item.Upheld + (decimal)item.Sorting + (decimal)item.Rectification + (decimal)item.PriceReduction;
                }
                model.lstDefectCodeList = listcong;
            }

            model.FromDate = FromDate;
            model.ToDate = ToDate;

            return model;
        }

        public ConsigneeCompReports GetCompPeriodData(string FromDate, string ToDate, string InspRegion, string JIInspRegion, string JIInspReqRegion, string actiondrp, string actioncodedrp, string actionjidrp,int IeCd)
        {
            ConsigneeCompReports model = new();
            List<ConsigneeComplaintsReportModel> lstConsigneeComplaints = new();

            DataSet ds = null;
            DataTable dt = new DataTable();

            model.FromDate = FromDate; model.ToDate = ToDate; model.actiondrp = actiondrp; model.actioncodedrp = actioncodedrp; model.actionjidrp = actionjidrp;

            OracleParameter[] parameter = new OracleParameter[10];
            parameter[0] = new OracleParameter("p_Fromdate", OracleDbType.Varchar2, FromDate, ParameterDirection.Input);
            parameter[1] = new OracleParameter("p_Todate", OracleDbType.Varchar2, ToDate, ParameterDirection.Input);
            parameter[2] = new OracleParameter("p_lstAction", OracleDbType.Varchar2, actiondrp, ParameterDirection.Input);
            parameter[3] = new OracleParameter("p_lstClassification", OracleDbType.Varchar2, actionjidrp, ParameterDirection.Input);
            parameter[4] = new OracleParameter("p_lstDefectCd", OracleDbType.Varchar2, actioncodedrp, ParameterDirection.Input);
            parameter[5] = new OracleParameter("P_INSPREGION", OracleDbType.Varchar2, InspRegion, ParameterDirection.Input);
            parameter[6] = new OracleParameter("P_JIREGION", OracleDbType.Varchar2, JIInspRegion, ParameterDirection.Input);
            parameter[7] = new OracleParameter("P_JIREQREGION", OracleDbType.Varchar2, JIInspReqRegion, ParameterDirection.Input);
            parameter[8] = new OracleParameter("P_IECD", OracleDbType.Varchar2, IeCd, ParameterDirection.Input);
            parameter[9] = new OracleParameter("p_result", OracleDbType.RefCursor, ParameterDirection.Output);
            ds = DataAccessDB.GetDataSet("compliants_Period_Report", parameter, 1);

            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
                List<ConsigneeComplaintsReportModel> listcong = dt.AsEnumerable().Select(row => new ConsigneeComplaintsReportModel
                {
                    IN_REGION = Convert.ToString(row["IN_REGION"]),
                    COMPLAINT_ID = Convert.ToString(row["COMPLAINT_ID"]),
                    JI_SNO = Convert.ToString(row["JI_SNO"]),
                    VENDOR = Convert.ToString(row["VENDOR"]),
                    PO_NO = Convert.ToString(row["PO_NO"]),
                    PO_DATE = Convert.ToString(row["PO_DATE"]),
                    BK_SET = Convert.ToString(row["BK_SET"]),
                    IC_DATE = Convert.ToString(row["IC_DATE"]),
                    ITEM_DESC = Convert.ToString(row["ITEM_DESC"]),
                    CONSIGNEE = Convert.ToString(row["CONSIGNEE"]),
                    IE_NAME = Convert.ToString(row["IE_NAME"]),
                    QTY_OFF = Convert.ToString(row["QTY_OFF"]),
                    QTY_REJECTED = Convert.ToString(row["QTY_REJECTED"]),
                    REJECTION_VALUE = Convert.ToString(row["REJECTION_VALUE"]),
                    DEPT = Convert.ToString(row["DEPT"]),
                    COMPLAINT_DATE = Convert.ToString(row["COMPLAINT_DATE"]),
                    REJECTIONMEMOPATH = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", Enums.GetEnumDescription(Enums.FolderPath.RejectionMemo), Convert.ToString(row["CASE_NO"]) + "-" + Convert.ToString(row["BK_NO"]) + "-" + Convert.ToString(row["SET_NO"])),// "/REJECTION_MEMO/" + Convert.ToString(row["CASE_NO"]) + "-" + Convert.ToString(row["BK_NO"]) + "-" + Convert.ToString(row["SET_NO"]),
                    REJECTION_REASON = Convert.ToString(row["REJECTION_REASON"]),
                    NO_JI_RES = Convert.ToString(row["NO_JI_RES"]),
                    JI_DATE = Convert.ToString(row["JI_DATE"]),
                    COMPLAINTSCASESPATH = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", Enums.GetEnumDescription(Enums.FolderPath.ComplaintCase), Convert.ToString(row["CASE_NO"]) + "-" + Convert.ToString(row["BK_NO"]) + "-" + Convert.ToString(row["SET_NO"])),//"/COMPLAINTS_CASES/" + Convert.ToString(row["CASE_NO"]) + "-" + Convert.ToString(row["BK_NO"]) + "-" + Convert.ToString(row["SET_NO"]),
                    STATUS = Convert.ToString(row["STATUS"]),
                    COMPLAINTSREPORTPATH = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", Enums.GetEnumDescription(Enums.FolderPath.COMPLAINTSREPORT), Convert.ToString(row["CASE_NO"]) + "-" + Convert.ToString(row["BK_NO"]) + "-" + Convert.ToString(row["SET_NO"])),//"/COMPLAINTS_REPORT/" + Convert.ToString(row["CASE_NO"]) + "-" + Convert.ToString(row["BK_NO"]) + "-" + Convert.ToString(row["SET_NO"]),
                    DEFECT_DESC = Convert.ToString(row["DEFECT_DESC"]),
                    JI_STATUS_DESC = Convert.ToString(row["JI_STATUS_DESC"]),
                    CONCLUSION_DATE = Convert.ToString(row["CONCLUSION_DATE"]),
                    CO_NAME = Convert.ToString(row["CO_NAME"]),
                    JI_IE_NAME = Convert.ToString(row["JI_IE_NAME"]),
                    ROOT_CAUSE_ANALYSIS = Convert.ToString(row["ROOT_CAUSE_ANALYSIS"]),
                    CHK_STATUS = Convert.ToString(row["CHK_STATUS"]),

                    TECH_REF = Convert.ToString(row["TECH_REF"]),
                    ACTION_PROPOSED = Convert.ToString(row["ACTION_PROPOSED"]),
                    ANY_OTHER = Convert.ToString(row["ANY_OTHER"]),
                    CAPA_STATUS = Convert.ToString(row["CAPA_STATUS"]),
                    DANDAR_STATUS = Convert.ToString(row["DANDAR_STATUS"]),

                    CASE_NO = Convert.ToString(row["CASE_NO"]),
                    BK_NO = Convert.ToString(row["BK_NO"]),
                    SET_NO = Convert.ToString(row["SET_NO"]),
                }).ToList();
                model.lstConsigneeComplaints = listcong;
            }

            return model;
        }

        public ConsigneeComplaints GetComplaintReportDetails(string JISNO, string Region)
        {
            ConsigneeComplaints model = new();
            List<ConsigneeReport> lstconsignee = new();

            DataSet ds = null;
            DataTable dt = new DataTable();

            model.JiSno = JISNO;

            OracleParameter[] par = new OracleParameter[2];
            par[0] = new OracleParameter("p_JI_SNO", OracleDbType.Varchar2, JISNO.ToString() == "" ? DBNull.Value : JISNO.ToString(), ParameterDirection.Input);
            par[1] = new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

            ds = DataAccessDB.GetDataSet("GetComplaintDetailsReport", par, 1);

            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
                List<ConsigneeReport> listcong = dt.AsEnumerable().Select(row => new ConsigneeReport
                {
                    JiSno = row.Field<string>("JI_SNO"),
                    ComplaintDate = DateTime.TryParseExact(row.Field<string>("COMPLAINT_DATE"), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateValue)
                    ? dateValue
                    : (DateTime?)null,
                    Consignee = row.Field<string>("CONSIGNEE"),
                    PO_NO = row.Field<string>("PO"),
                    ie_name = row.Field<string>("IE_NAME"),
                    InspRegion = row.Field<string>("INSP_REGION_NAME"),
                    IC_NO = row.Field<string>("IC"),
                    BK_NO = row.Field<string>("BK_NO") + "/" + row.Field<string>("SET_NO"),
                    VEND_NAME = row.Field<string>("VENDOR"),
                    ItemDesc = row.Field<string>("ITEM_DESC"),
                    QtyOffered = (decimal?)row.Field<double>("QTY_OFFERED"),
                    QtyRejected = (decimal?)row.Field<double>("QTY_REJECTED"),
                    RejectionReason = row.Field<string>("REJECTION_REASON"),
                    Remarks = row.Field<string>("IE_JI_REMARKS"),
                    JIDate = DateTime.TryParseExact(row.Field<string>("JI_DATE"), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateValues)
                    ? dateValues
                    : (DateTime?)null,
                    NoJiOther = row.Field<string>("ACTION"),
                    JiStatusDesc = row.Field<string>("JI_STATUS"),
                    CoName = row.Field<string>("CO_NAME"),
                }).ToList();
                model.lstconsignee = listcong;
            }

            return model;
        }

        public JIRequiredReport GetJIRequiredList(string FromDate, string ToDate, string AllCM, string AllIEs, string AllVendors, string AllClient, string AllConsignee, string Compact, string AwaitingJI, string JIConclusion, string JIConclusionfollowup,
           string JIconclusionreport, string JIDecidedDT, string All, string ParticularIEs, string IEWise, string CMWise, string VendorWise, string ClientWise, string ConsigneeWise, string FinancialYear, string ParticularCMs, string ParticularClients, string ParticularConsignee,
           string ParticularVendor, string Detailed, string FinancialYears, string ddlsupercm, string ddliename, string Clientwiseddl, string vendor, string Item, string consignee, string Region, string FinancialYearsvalue)
        {
            JIRequiredReport model = new();
            List<JIRequiredList> lstJIRequiredList = new();
            List<ConsigneeComplaintsReportModel> lstConsigneeComplaints = new();
            DataSet ds = null;
            DataTable dt = new DataTable();

            model.FromDate = FromDate; model.ToDate = ToDate; model.AllCM = AllCM; model.AllIEs = AllIEs; model.AllVendors = AllVendors; model.AllClient = AllClient; model.AllConsignee = AllConsignee;
            model.Compact = Compact; model.AwaitingJI = AwaitingJI; model.JIConclusion = JIConclusion; model.JIConclusionfollowup = JIConclusionfollowup; model.JIconclusionreport = JIconclusionreport; model.JIDecidedDT = JIDecidedDT;
            model.All = All; model.ParticularIEs = ParticularIEs; model.IEWise = IEWise; model.CMWise = CMWise; model.VendorWise = VendorWise; model.ClientWise = ClientWise; model.ConsigneeWise = ConsigneeWise;
            model.FinancialYear = FinancialYear; model.ParticularCMs = ParticularCMs; model.ParticularClients = ParticularClients; model.ParticularConsignee = ParticularConsignee; model.ParticularVendor = ParticularVendor; model.Detailed = Detailed;
            model.FinancialYears = FinancialYears; model.ddlsupercm = ddlsupercm; model.ddliename = ddliename; model.Clientwiseddl = Clientwiseddl; model.vendor = vendor; model.Item = Item; model.consignee = consignee; model.FinancialYearsValue = FinancialYearsvalue;


            if (Convert.ToBoolean(Compact) == true)
            {
                if (Convert.ToBoolean(IEWise) == true)
                {
                    ds = compliants_statement_IEWise(FromDate, ToDate, Region, FinancialYear, JIDecidedDT, ParticularIEs, AllIEs, ddliename, FinancialYears, FinancialYearsvalue);
                }
                else if (Convert.ToBoolean(VendorWise) == true)
                {
                    ds = compliants_statement_VendorWise(FromDate, ToDate, Region, FinancialYear, JIDecidedDT, AllVendors, ParticularVendor, vendor, FinancialYears, FinancialYearsvalue);
                }
                else if (Convert.ToBoolean(CMWise) == true)
                {
                    ds = compliants_statement_CMWise(FromDate, ToDate, Region, FinancialYear, JIDecidedDT, AllCM, ParticularCMs, ddlsupercm, FinancialYears, FinancialYearsvalue);
                }
                else if (Convert.ToBoolean(ClientWise) == true)
                {
                    ds = compliants_statement_ClientWise(FromDate, ToDate, Region, FinancialYear, JIDecidedDT, AllClient, ParticularClients, Clientwiseddl, FinancialYears, FinancialYearsvalue);
                }
                else if (Convert.ToBoolean(ConsigneeWise) == true)
                {
                    ds = compliants_statement_ConsigneeWise(FromDate, ToDate, Region, FinancialYear, JIDecidedDT, AllConsignee, ParticularConsignee, consignee, FinancialYears, FinancialYearsvalue);
                }
            }
            else
            {
                ds = ji_compliants_statement(FromDate, ToDate, Region, FinancialYear, JIDecidedDT, Compact, AwaitingJI, JIConclusion, JIConclusionfollowup, All, Detailed, FinancialYears, consignee, ddlsupercm, ddliename, vendor, Clientwiseddl, Item, CMWise, ClientWise, VendorWise, ConsigneeWise, IEWise);
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    List<ConsigneeComplaintsReportModel> listcong = dt.AsEnumerable().Select(row => new ConsigneeComplaintsReportModel
                    {
                        IN_REGION = Convert.ToString(row["IN_REGION"]),
                        COMPLAINT_ID = Convert.ToString(row["COMPLAINT_ID"]),
                        JI_SNO = Convert.ToString(row["JI_SNO"]),
                        VENDOR = Convert.ToString(row["VENDOR"]),
                        PO_NO = Convert.ToString(row["PO_NO"]),
                        PO_DATE = Convert.ToString(row["PO_DATE"]),
                        BK_SET = Convert.ToString(row["BK_SET"]),
                        IC_DATE = Convert.ToString(row["IC_DATE"]),
                        ITEM_DESC = Convert.ToString(row["ITEM_DESC"]),
                        CONSIGNEE = Convert.ToString(row["CONSIGNEE"]),
                        IE_NAME = Convert.ToString(row["IE_NAME"]),
                        QTY_OFF = Convert.ToString(row["QTY_OFF"]),
                        QTY_REJECTED = Convert.ToString(row["QTY_REJECTED"]),
                        REJECTION_VALUE = Convert.ToString(row["REJECTION_VALUE"]),
                        DEPT = Convert.ToString(row["DEPT"]),
                        COMPLAINT_DATE = Convert.ToString(row["COMPLAINT_DATE"]),
                        REJECTIONMEMOPATH = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", Enums.GetEnumDescription(Enums.FolderPath.RejectionMemo), Convert.ToString(row["CASE_NO"]) + "-" + Convert.ToString(row["BK_NO"]) + "-" + Convert.ToString(row["SET_NO"])),// "/REJECTION_MEMO/" + Convert.ToString(row["CASE_NO"]) + "-" + Convert.ToString(row["BK_NO"]) + "-" + Convert.ToString(row["SET_NO"]),
                        REJECTION_REASON = Convert.ToString(row["REJECTION_REASON"]),
                        NO_JI_RES = Convert.ToString(row["NO_JI_RES"]),
                        JI_DATE = Convert.ToString(row["JI_DATE"]),
                        COMPLAINTSCASESPATH = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", Enums.GetEnumDescription(Enums.FolderPath.ComplaintCase), Convert.ToString(row["CASE_NO"]) + "-" + Convert.ToString(row["BK_NO"]) + "-" + Convert.ToString(row["SET_NO"])),//"/COMPLAINTS_CASES/" + Convert.ToString(row["CASE_NO"]) + "-" + Convert.ToString(row["BK_NO"]) + "-" + Convert.ToString(row["SET_NO"]),
                        STATUS = Convert.ToString(row["STATUS"]),
                        COMPLAINTSREPORTPATH = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", Enums.GetEnumDescription(Enums.FolderPath.COMPLAINTSREPORT), Convert.ToString(row["CASE_NO"]) + "-" + Convert.ToString(row["BK_NO"]) + "-" + Convert.ToString(row["SET_NO"])),//"/COMPLAINTS_REPORT/" + Convert.ToString(row["CASE_NO"]) + "-" + Convert.ToString(row["BK_NO"]) + "-" + Convert.ToString(row["SET_NO"]),
                        DEFECT_DESC = Convert.ToString(row["DEFECT_DESC"]),
                        JI_STATUS_DESC = Convert.ToString(row["JI_STATUS_DESC"]),
                        CONCLUSION_DATE = Convert.ToString(row["CONCLUSION_DATE"]),
                        CO_NAME = Convert.ToString(row["CO_NAME"]),
                        JI_IE_NAME = Convert.ToString(row["JI_IE_NAME"]),
                        ROOT_CAUSE_ANALYSIS = Convert.ToString(row["ROOT_CAUSE_ANALYSIS"]),
                        CHK_STATUS = Convert.ToString(row["CHK_STATUS"]),

                        TECH_REF = Convert.ToString(row["TECH_REF"]),
                        ACTION_PROPOSED = Convert.ToString(row["ACTION_PROPOSED"]),
                        ANY_OTHER = Convert.ToString(row["ANY_OTHER"]),
                        CAPA_STATUS = Convert.ToString(row["CAPA_STATUS"]),
                        DANDAR_STATUS = Convert.ToString(row["DANDAR_STATUS"]),

                        CASE_NO = Convert.ToString(row["CASE_NO"]),
                        BK_NO = Convert.ToString(row["BK_NO"]),
                        SET_NO = Convert.ToString(row["SET_NO"]),


                    }).ToList();
                    model.lstConsigneeComplaints = listcong;

                }
                return model;
            }
            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
                List<JIRequiredList> list = dt.AsEnumerable().Select(row => new JIRequiredList
                {
                    IE = row.Field<string>("Name"),
                    NO_OF_INSPECTION = Convert.ToInt32(row.Field<decimal>("NO_OF_INSPECTION")),
                    MATERIAL_VALUE = row.Field<decimal>("MATERIAL_VALUE"),
                    RECD = Convert.ToInt32(row.Field<decimal>("RECD")),
                    FINALISED = Convert.ToInt32(row.Field<decimal>("FINALISED")),
                    PENDING = Convert.ToInt32(row.Field<decimal>("PENDING")),
                    ACCEPTED = Convert.ToInt32(row.Field<decimal>("ACCEPTED")),

                    UPHELD = Convert.ToInt32(row.Field<decimal>("UPHELD")),
                    SORTING = Convert.ToInt32(row.Field<decimal>("SORTING")),
                    RECTIFICATION = Convert.ToInt32(row.Field<decimal>("RECTIFICATION")),
                    PRICE_REDUCTION = Convert.ToInt32(row.Field<decimal>("PRICE_REDUCTION")),
                    LIFTED_BEFORE_JI = Convert.ToInt32(row.Field<decimal>("LIFTED_BEFORE_JI")),

                    TRANSIT_DEMAGE = Convert.ToInt32(row.Field<decimal>("TRANSIT_DEMAGE")),
                    UNSTAMPED = Convert.ToInt32(row.Field<decimal>("UNSTAMPED")),
                    NOT_ON_RITES_AC = Convert.ToInt32(row.Field<decimal>("NOT_ON_RITES_AC")),
                    Region = Region,
                    FromDate = Convert.ToDateTime(FromDate),
                    ToDate = Convert.ToDateTime(ToDate),

                }).ToList();
                foreach (var item in list)
                {
                    item.Total = item.UPHELD + item.SORTING + item.RECTIFICATION + item.PRICE_REDUCTION + item.LIFTED_BEFORE_JI;
                }
                model.lstJIRequiredList = list;
            }

            return model;
        }


        public DataSet compliants_statement_IEWise(string FromDateFor, string ToDateFor, string Region, string FinancialYear, string JIDecidedDT, string ParticularIEs, string AllIEs, string ddliename, string FinancialYears, string FinancialYearsvalue)
        {
            DataSet ds = null;
            if (Convert.ToBoolean(JIDecidedDT) == true)
            {
                if (Convert.ToBoolean(AllIEs) == true)
                {
                    ds = AllIE(FromDateFor, ToDateFor, Region, ddliename, FinancialYears, FinancialYearsvalue);
                }
                else if (Convert.ToBoolean(ParticularIEs) == true)
                {
                    ds = AllIE(FromDateFor, ToDateFor, Region, ddliename, FinancialYears, FinancialYearsvalue);
                }
            }
            else if (Convert.ToBoolean(FinancialYear) == true)
            {
                if (Convert.ToBoolean(AllIEs) == true)
                {
                    ds = AllIE(FromDateFor, ToDateFor, Region, ddliename, FinancialYears, FinancialYearsvalue);
                }
                else if (Convert.ToBoolean(ParticularIEs) == true)
                {
                    ds = AllIE(FromDateFor, ToDateFor, Region, ddliename, FinancialYears, FinancialYearsvalue);
                }
            }
            return ds;
        }

        public DataSet compliants_statement_VendorWise(string FromDate, string ToDate, string Region, string FinancialYear, string JIDecidedDT, string AllVendors, string ParticularVendor, string vendor, string FinancialYears, string FinancialYearsvalue)
        {
            DataSet ds = null;
            if (Convert.ToBoolean(JIDecidedDT) == true)
            {
                if (Convert.ToBoolean(AllVendors) == true)
                {
                    ds = AllVendor(FromDate, ToDate, Region, vendor, FinancialYears, FinancialYearsvalue);
                }
                else if (Convert.ToBoolean(ParticularVendor) == true)
                {
                    ds = AllVendor(FromDate, ToDate, Region, vendor, FinancialYears, FinancialYearsvalue);
                }
            }
            else if (Convert.ToBoolean(FinancialYear) == true)
            {
                if (Convert.ToBoolean(AllVendors) == true)
                {
                    ds = AllVendor(FromDate, ToDate, Region, vendor, FinancialYears, FinancialYearsvalue);
                }
                else if (Convert.ToBoolean(ParticularVendor) == true)
                {
                    ds = AllVendor(FromDate, ToDate, Region, vendor, FinancialYears, FinancialYearsvalue);
                }
            }
            return ds;
        }

        public DataSet compliants_statement_CMWise(string FromDate, string ToDate, string Region, string FinancialYear, string JIDecidedDT, string AllCM, string ParticularCMs, string ddlsupercm, string FinancialYears, string FinancialYearsvalue)
        {
            DataSet ds = null;
            if (Convert.ToBoolean(JIDecidedDT) == true)
            {
                if (Convert.ToBoolean(AllCM) == true)
                {
                    ds = ALLCM(FromDate, ToDate, Region, ddlsupercm, FinancialYears, FinancialYearsvalue);
                }
                else if (Convert.ToBoolean(ParticularCMs) == true)
                {
                    ds = ALLCM(FromDate, ToDate, Region, ddlsupercm, FinancialYears, FinancialYearsvalue);
                }
            }
            else if (Convert.ToBoolean(FinancialYear) == true)
            {
                if (Convert.ToBoolean(AllCM) == true)
                {
                    ds = ALLCM(FromDate, ToDate, Region, ddlsupercm, FinancialYears, FinancialYearsvalue);
                }
                else if (Convert.ToBoolean(ParticularCMs) == true)
                {
                    ds = ALLCM(FromDate, ToDate, Region, ddlsupercm, FinancialYears, FinancialYearsvalue);
                }
            }
            return ds;
        }

        public DataSet compliants_statement_ClientWise(string FromDate, string ToDate, string Region, string FinancialYear, string JIDecidedDT, string AllClient, string ParticularClients, string Clientwiseddl, string FinancialYears, string FinancialYearsvalue)
        {
            DataSet ds = null;
            if (Convert.ToBoolean(JIDecidedDT) == true)
            {
                if (Convert.ToBoolean(AllClient) == true)
                {
                    ds = ALLClient(FromDate, ToDate, Region, Clientwiseddl, FinancialYears, FinancialYearsvalue);
                }
                else if (Convert.ToBoolean(ParticularClients) == true)
                {
                    ds = ALLClient(FromDate, ToDate, Region, Clientwiseddl, FinancialYears, FinancialYearsvalue);
                }
            }
            else if (Convert.ToBoolean(FinancialYear) == true)
            {
                if (Convert.ToBoolean(AllClient) == true)
                {
                    ds = ALLClient(FromDate, ToDate, Region, Clientwiseddl, FinancialYears, FinancialYearsvalue);
                }
                else if (Convert.ToBoolean(ParticularClients) == true)
                {
                    ds = ALLClient(FromDate, ToDate, Region, Clientwiseddl, FinancialYears, FinancialYearsvalue);
                }
            }
            return ds;
        }

        public DataSet compliants_statement_ConsigneeWise(string FromDate, string ToDate, string Region, string FinancialYear, string JIDecidedDT, string AllConsignee, string ParticularConsignee, string consignee, string FinancialYears, string FinancialYearsvalue)
        {
            DataSet ds = null;
            if (Convert.ToBoolean(JIDecidedDT) == true)
            {
                if (Convert.ToBoolean(AllConsignee) == true)
                {
                    ds = ALLConsignee(FromDate, ToDate, Region, consignee, FinancialYears, FinancialYearsvalue);
                }
                else if (Convert.ToBoolean(ParticularConsignee) == true)
                {
                    ds = ALLConsignee(FromDate, ToDate, Region, consignee, FinancialYears, FinancialYearsvalue);
                }
            }
            else if (Convert.ToBoolean(FinancialYear) == true)
            {
                if (Convert.ToBoolean(AllConsignee) == true)
                {
                    ds = ALLConsignee(FromDate, ToDate, Region, consignee, FinancialYears, FinancialYearsvalue);
                }
                else if (Convert.ToBoolean(ParticularConsignee) == true)
                {
                    ds = ALLConsignee(FromDate, ToDate, Region, consignee, FinancialYears, FinancialYearsvalue);
                }
            }
            return ds;
        }

        public DataSet ji_compliants_statement(string FromDate, string ToDate, string Region, string FinancialYear, string JIDecidedDT, string Compact, string AwaitingJI, string JIConclusion, string JIConclusionfollowup, string All, string Detailed, string FinancialYears, string consignee, string ddlsupercm, string ddliename, string vendor, string Clientwiseddl, string Item, string CMWise, string ClientWise, string VendorWise, string ConsigneeWise, string IEWise)
        {
            DataSet ds = null;
            if (Convert.ToBoolean(JIDecidedDT) == true)
            {
                if (Convert.ToBoolean(Compact) == true)
                {
                    ds = ALLReportTye(FromDate, ToDate, Region, AwaitingJI, JIConclusion, JIConclusionfollowup, All, FinancialYears, consignee, ddlsupercm, ddliename, vendor, Clientwiseddl, Item, CMWise, ClientWise, VendorWise, ConsigneeWise, IEWise);
                }
                else if (Convert.ToBoolean(Detailed) == true)
                {
                    ds = ALLReportTye(FromDate, ToDate, Region, AwaitingJI, JIConclusion, JIConclusionfollowup, All, FinancialYears, consignee, ddlsupercm, ddliename, vendor, Clientwiseddl, Item, CMWise, ClientWise, VendorWise, ConsigneeWise, IEWise);
                }
            }
            else if (Convert.ToBoolean(FinancialYear) == true)
            {
                if (Convert.ToBoolean(Compact) == true)
                {
                    ds = ALLReportTye(FromDate, ToDate, Region, AwaitingJI, JIConclusion, JIConclusionfollowup, All, FinancialYears, consignee, ddlsupercm, ddliename, vendor, Clientwiseddl, Item, CMWise, ClientWise, VendorWise, ConsigneeWise, IEWise);
                }
                else if (Convert.ToBoolean(Detailed) == true)
                {
                    ds = ALLReportTye(FromDate, ToDate, Region, AwaitingJI, JIConclusion, JIConclusionfollowup, All, FinancialYears, consignee, ddlsupercm, ddliename, vendor, Clientwiseddl, Item, CMWise, ClientWise, VendorWise, ConsigneeWise, IEWise);
                }
            }
            return ds;
        }

        public DataSet AllIE(string FromDateFor, string ToDateFor, string Region, string ddliename, string FinancialYears, string FinancialYearsvalue)
        {
            DataSet ds = null;
            OracleParameter[] par = new OracleParameter[7];
            par[0] = new OracleParameter("p_frm_dt", OracleDbType.Varchar2, FromDateFor, ParameterDirection.Input);
            par[1] = new OracleParameter("p_to_dt", OracleDbType.Varchar2, ToDateFor, ParameterDirection.Input);
            par[2] = new OracleParameter("p_ie_cd", OracleDbType.Varchar2, ddliename, ParameterDirection.Input);
            par[3] = new OracleParameter("p_fin_year", OracleDbType.Varchar2, FinancialYears, ParameterDirection.Input);
            par[4] = new OracleParameter("p_finyear_value", OracleDbType.Varchar2, FinancialYearsvalue, ParameterDirection.Input);
            par[5] = new OracleParameter("p_region", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            par[6] = new OracleParameter("p_result", OracleDbType.RefCursor, ParameterDirection.Output);
            ds = DataAccessDB.GetDataSet("compliants_statement_AllIE_Date", par, 1);
            return ds;
        }

        public DataSet AllVendor(string FromDate, string ToDate, string Region, string vendor, string FinancialYears, string FinancialYearsvalue)
        {
            DataSet ds = null;
            OracleParameter[] par = new OracleParameter[7];
            par[0] = new OracleParameter("p_frmdt", OracleDbType.Varchar2, FromDate, ParameterDirection.Input);
            par[1] = new OracleParameter("p_todt", OracleDbType.Varchar2, ToDate, ParameterDirection.Input);
            par[2] = new OracleParameter("p_vend_cd", OracleDbType.Varchar2, vendor, ParameterDirection.Input);
            par[3] = new OracleParameter("p_finyear", OracleDbType.Varchar2, FinancialYears, ParameterDirection.Input);
            par[4] = new OracleParameter("p_finyear_value", OracleDbType.Varchar2, FinancialYearsvalue, ParameterDirection.Input);
            par[5] = new OracleParameter("p_region", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            par[6] = new OracleParameter("p_result", OracleDbType.RefCursor, ParameterDirection.Output);
            ds = DataAccessDB.GetDataSet("compliants_statement_AllVendor_Report", par, 1);
            return ds;
        }

        public DataSet ALLCM(string FromDate, string ToDate, string Region, string ddlsupercm, string FinancialYears, string FinancialYearsvalue)
        {
            DataSet ds = null;
            OracleParameter[] par = new OracleParameter[7];
            par[0] = new OracleParameter("p_frmdt", OracleDbType.Varchar2, FromDate, ParameterDirection.Input);
            par[1] = new OracleParameter("p_todt", OracleDbType.Varchar2, ToDate, ParameterDirection.Input);
            par[2] = new OracleParameter("p_CO", OracleDbType.Varchar2, ddlsupercm, ParameterDirection.Input);
            par[3] = new OracleParameter("p_finyear", OracleDbType.Varchar2, FinancialYears, ParameterDirection.Input);
            par[4] = new OracleParameter("p_finyear_value", OracleDbType.Varchar2, FinancialYearsvalue, ParameterDirection.Input);
            par[5] = new OracleParameter("p_region", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            par[6] = new OracleParameter("p_result", OracleDbType.RefCursor, ParameterDirection.Output);
            ds = DataAccessDB.GetDataSet("compliants_statement_AllCM_Report", par, 1);
            return ds;
        }

        public DataSet ALLClient(string FromDate, string ToDate, string Region, string Clientwiseddl, string FinancialYears, string FinancialYearsvalue)
        {
            DataSet ds = null;
            OracleParameter[] par = new OracleParameter[7];
            par[0] = new OracleParameter("p_frmdt", OracleDbType.Varchar2, FromDate, ParameterDirection.Input);
            par[1] = new OracleParameter("p_todt", OracleDbType.Varchar2, ToDate, ParameterDirection.Input);
            par[2] = new OracleParameter("p_bpo_rly", OracleDbType.Varchar2, Clientwiseddl, ParameterDirection.Input);
            par[3] = new OracleParameter("p_finyear", OracleDbType.Varchar2, FinancialYears, ParameterDirection.Input);
            par[4] = new OracleParameter("p_finyear_value", OracleDbType.Varchar2, FinancialYearsvalue, ParameterDirection.Input);
            par[5] = new OracleParameter("p_region", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            par[6] = new OracleParameter("p_result", OracleDbType.RefCursor, ParameterDirection.Output);
            ds = DataAccessDB.GetDataSet("compliants_statement_AllClient_Report", par, 1);
            return ds;
        }

        public DataSet ALLConsignee(string FromDate, string ToDate, string Region, string consignee, string FinancialYears, string FinancialYearsvalue)
        {
            DataSet ds = null;
            OracleParameter[] par = new OracleParameter[7];
            par[0] = new OracleParameter("p_frmdt", OracleDbType.Varchar2, FromDate, ParameterDirection.Input);
            par[1] = new OracleParameter("p_todt", OracleDbType.Varchar2, ToDate, ParameterDirection.Input);
            par[2] = new OracleParameter("p_consignee_cd", OracleDbType.Varchar2, consignee, ParameterDirection.Input);
            par[3] = new OracleParameter("p_finyear", OracleDbType.Varchar2, FinancialYears, ParameterDirection.Input);
            par[4] = new OracleParameter("p_finyear_value", OracleDbType.Varchar2, FinancialYearsvalue, ParameterDirection.Input);
            par[5] = new OracleParameter("p_region", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            par[6] = new OracleParameter("p_result", OracleDbType.RefCursor, ParameterDirection.Output);
            ds = DataAccessDB.GetDataSet("compliants_statement_AllConsignee_Report", par, 1);
            return ds;
        }

        public DataSet ALLReportTye(string FromDate, string ToDate, string Region, string AwaitingJI, string JIConclusion, string JIConclusionfollowup, string All, string FinancialYears, string consignee, string ddlsupercm, string ddliename, string vendor, string Clientwiseddl, string Item, string CMWise, string ClientWise, string VendorWise, string ConsigneeWise, string IEWise)
        {
            DataSet ds = null;
            OracleParameter[] par = new OracleParameter[21];
            par[0] = new OracleParameter("p_Frm_dt", OracleDbType.Varchar2, FromDate, ParameterDirection.Input);
            par[1] = new OracleParameter("p_To_dt", OracleDbType.Varchar2, ToDate, ParameterDirection.Input);
            par[2] = new OracleParameter("p_Co", OracleDbType.Varchar2, ddlsupercm, ParameterDirection.Input);
            par[3] = new OracleParameter("p_Ie", OracleDbType.Varchar2, ddliename, ParameterDirection.Input);
            par[4] = new OracleParameter("p_Vend_cd", OracleDbType.Varchar2, vendor, ParameterDirection.Input);
            par[5] = new OracleParameter("p_Consignee_cd", OracleDbType.Varchar2, consignee, ParameterDirection.Input);
            par[6] = new OracleParameter("p_Today_dt", OracleDbType.Varchar2, DateTime.Now.ToString("dd/MM/yyyy"), ParameterDirection.Input);
            par[7] = new OracleParameter("p_Fin_year", OracleDbType.Varchar2, FinancialYears, ParameterDirection.Input);
            par[8] = new OracleParameter("p_Bpo_rly", OracleDbType.Varchar2, Item, ParameterDirection.Input);
            par[9] = new OracleParameter("p_Client_type", OracleDbType.Varchar2, Clientwiseddl, ParameterDirection.Input);
            par[10] = new OracleParameter("p_Region", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            par[11] = new OracleParameter("p_Awaiting_ji", OracleDbType.Varchar2, AwaitingJI, ParameterDirection.Input);
            par[12] = new OracleParameter("p_Awaiting_conclusion", OracleDbType.Varchar2, JIConclusion, ParameterDirection.Input);
            par[13] = new OracleParameter("p_Awaiting_action", OracleDbType.Varchar2, JIConclusionfollowup, ParameterDirection.Input);
            par[14] = new OracleParameter("p_Awaiting_finalaction", OracleDbType.Varchar2, All, ParameterDirection.Input);
            par[15] = new OracleParameter("p_Cm_wise", OracleDbType.Varchar2, CMWise, ParameterDirection.Input);
            par[16] = new OracleParameter("p_Ie_wise", OracleDbType.Varchar2, IEWise, ParameterDirection.Input);
            par[17] = new OracleParameter("p_Vendor_wise", OracleDbType.Varchar2, VendorWise, ParameterDirection.Input);
            par[18] = new OracleParameter("p_Client_wise", OracleDbType.Varchar2, ClientWise, ParameterDirection.Input);
            par[19] = new OracleParameter("p_Consignee_wise", OracleDbType.Varchar2, ConsigneeWise, ParameterDirection.Input);
            par[20] = new OracleParameter("p_result", OracleDbType.RefCursor, ParameterDirection.Output);
            ds = DataAccessDB.GetDataSet("compliants_statement_reporttypes_Report", par, 1);
            return ds;
        }

    }
}
