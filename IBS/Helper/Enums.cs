using IBS.Models;
using IBS.Models.Reports;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Reflection;

namespace IBS.Helper
{
    public class Enums
    {
        public enum FileUploaderMode : int
        {
            View = 1,
            Add_Edit = 2
        }

        public enum FilUploadMode
        {
            Single = 1,
            Multiple = 2,
        }

        public enum FolderPath
        {
            [Description("/ReadWriteData/Files/TempUploadedFiles")]
            TempFilePath = 1,
            [Description("/ReadWriteData/Files/UserRegistration")]
            UserRegistration = 2,
            [Description("/ReadWriteData/Files/AdminUserUploadDoc")]
            AdminUserUploadDoc = 3,
            [Description("/ReadWriteData/Files/Vendor")]
            Vendor = 4,
            [Description("/ReadWriteData/Files/VendorDocument")]
            VendorDocument = 5,
            [Description("/ReadWriteData/MA")]
            VendorMADocument = 6,
            [Description("/ReadWriteData/Files/ContractDocument")]
            ContractDocument = 7,
            [Description("/ReadWriteData/Files/TechnicalReferences")]
            TechnicalReferencesDoc = 8,
            [Description("/ReadWriteData/MASTER_ITEMS_CHECKSHEETS")]
            MasterItemDoc = 9,
            [Description("/ReadWriteData/CALLS_DOCUMENTS")]
            CallRegistrationDoc = 10,
            [Description("/ReadWriteData/Files/AdministratorPurchaseOrder")]
            AdministratorPurchaseOrder = 13,
            [Description("/ReadWriteData/Files/Online_Comp_Document")]
            OnlineComplaints = 11,
            [Description("/ReadWriteData/Files/Complaint_Case")]
            ComplaintCase = 12,
            [Description("/ReadWriteData/IE/SIGNATURE/FULL")]
            IEFullSignature = 13,
            [Description("/ReadWriteData/IE/SIGNATURE/INITIALS")]
            IEInitials = 14,
            [Description("/ReadWriteData/Files/ContractEntry")]
            ContractEntry = 15,
            [Description("/ReadWriteData/Files/Rejection_Memo")]
            RejectionMemo = 16,
            [Description("/ReadWriteData/Files/Complaints_Report")]
            COMPLAINTSREPORT = 17,
            [Description("/ReadWriteData/Files/Complaints_Tech_Ref")]
            ComplaintTechRef = 18,
            [Description("/ReadWriteData/Files/ICCancellation")]
            ICCancellation = 19,
            [Description("/ReadWriteData/Files/PurchaseOrderForm")]
            PurchaseOrderForm = 20,
            [Description("/ReadWriteData/Files/INVOICE_SUPP_DOCS")]
            ICDocument = 21,
            [Description("/ReadWriteData/CASE_NO")]
            CaseNo = 22,
            [Description("/ReadWriteData/LAB")]
            Lab = 23,
            [Description("/ReadWriteData/IC_PHOTOS")]
            ICPHOTOS = 24,
            [Description("/ReadWriteData/BILL_IC")]
            BILLIC = 25,
            [Description("/ReadWriteData/TESTPLAN")]
            TESTPLAN = 26,
        }

        public enum DocumentCategory : int
        {
            UserRegi = 1,
            AdminUserUploadDoc = 2,
            Vendor = 3,
            VendorDocument = 4,
            VendorMADoc = 5,
            Contract = 6,
            TechnicalReferences = 8,
            MasterItemDoc = 7,
            CallRegistrationDoc = 10,
            //AdministratorPurchaseOrder =13,
            OnlineComplaints = 11,
            ConsigneeComplaints = 12,
            IEFullSignature = 14,
            IEInitials = 15,
            ContractEntryDoc = 16,
            ICCancellation = 17,
            PurchaseOrderForm = 18,
            ICDocument = 21,
            ICPHOTOS = 22,
            ICPhotoDigSign = 23,
            UploadTestPlan = 24,
            UploadICAnnexue1 = 25,
            UploadICAnnexue2 = 26,
        }

        public enum DocumentCategory_CANRegisrtation : int
        {
            Address_Proof_Document = 5,
            Profile_Picture = 6,
            Document_Vendor_manufacturer_created = 23,
            Inernal_Records = 45,
            Firm_Certificate_Like_RDSO_Approval_Type_test_etc = 47,
            Raw_Material_Invoice = 48,
            Calibration_Records = 49,
            Contract_Documents_If_Any = 9,
            Upload_Tech_Ref = 11,
            Upload_Tech_Ref_Reply = 12,
            Upload_Rejection_Memo = 50,
            Upload_JI_Case = 51,
            Upload_JI_Report = 52,
            Upload_Tech_Ref1 = 53,
            Upload_Contract_Doc = 16,
            IC_Photos_Upload1 = 22,
            IC_Photos_Upload2 = 24,
            IC_Photos_Upload3 = 25,
            IC_Photos_Upload4 = 26,
            IC_Photos_Upload5 = 27,
            IC_Photos_Upload6 = 28,
            IC_Photos_Upload7 = 29,
            IC_Photos_Upload8 = 30,
            IC_Photos_Upload9 = 31,
            IC_Photos_Upload10 = 32,
            ICPhoto_Dig_Sign = 33,
            Upload_Test_Plan = 34,
            Upload_IC_Annexue1 = 35,
            Upload_IC_Annexue2 = 36,
        }

        public enum DocumentICCancellation : int
        {
            FIR_Upload = 55,
        }

        public enum DocumentPurchaseOrderForm : int
        {
            Upload_a_scanned_copy_of_Purchase_Order = 54,
            DrawingSpecification = 56,
            Amendment = 57,
            ParentLOA = 58,
        }

        public enum DocumentCategory_AdminUserUploadDoc : int
        {
            Browse_the_Document_to_Upload = 7,
            CallRegistrationDoc = 13,
            IEFullSignature = 14,
            IEInitials = 15,
            ICDocument = 2
        }

        public enum DocumentCategory_VendorMADoc : int
        {
            VendorMADoc = 8,
        }

        public enum DocumentCategory_MasterDoc : int
        {
            MasterItemDoc = 10,
        }

        public static string GetEnumDescription(object enumValue)
        {
            string defDesc = string.Empty;
            FieldInfo fi = enumValue.GetType().GetField(enumValue.ToString());

            if (null != fi)
            {
                object[] attrs = fi.GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (attrs != null && attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;
            }

            return defDesc;
        }

        public enum RegionType
        {
            [Description("East Zone")]
            East_Zone = 1,
            [Description("West Zone")]
            West_Zone = 2,
            [Description("North Zone")]
            North_Zone = 3,
            [Description("South Zone")]
            South_Zone = 4,
        }

        public enum YesNoCommon
        {
            [Description("Yes")]
            Yes = 0,
            [Description("No")]
            No = 1,
        }

        public enum COType
        {
            [Description("CM")]
            C,
            [Description("DFO")]
            D,
        }

        public enum COStatus
        {
            [Description("Working")]
            W,
            [Description("Retired")]
            R,
            [Description("Transferred")]
            T,
            [Description("Left/Repatriated")]
            L,
        }

        public enum IEPosting
        {
            [Description("Local")]
            LC,
            [Description("Outstation")]
            OU,
            [Description("Liaison Officer")]
            LO,
        }

        public enum IEStatus
        {
            [Description("Working")]
            W,
            [Description("Retired")]
            R,
            [Description("Transferred")]
            T,
            [Description("Left/Repatriated")]
            L,
        }

        public enum IEJobType
        {
            [Description("Regular")]
            R,
            [Description("Deputation")]
            D,
            [Description("Contract")]
            C,
        }

        public enum BPOFeeType
        {
            [Description("Man days Basis")]
            D,
            [Description("Hourly Basis")]
            H,
            [Description("Lump sum")]
            L,
            [Description("Percentage Basis")]
            P,
        }

        public enum BPOTaxType
        {
            [Description("Fee Inclusive Service Tax")]
            I,
            [Description("Tax/VAT Charged separately")]
            X,
        }

        public enum BPOFlag
        {
            [Description("FA & CAO")]
            F,
            [Description("AO�s")]
            A,
            [Description("DFM�s")]
            D,
            [Description("DEE/DCEE etc.")]
            M,
            [Description("Workshop")]
            S,
        }

        public enum BPOAdvFlag
        {
            [Description("Advance bill to be raised")]
            A,
            [Description("Otherwise")]
            N,
        }

        public enum AdvanceBill
        {
            [Description("Advance bill to be raised")]
            A,
            [Description("No Advance bill")]
            X,
        }


        public enum ActiveInActive
        {
            [Description("Active")]
            A,
            [Description("InActive")]
            I,
        }

        public enum UserType
        {
            [Description("User")]
            U,
            [Description("CM")]
            C,
            [Description("GM")]
            G,
            [Description("SBU HEAD")]
            S,
        }

        public enum PoOrLetter
        {
            [Description("Purchase Order")]
            P,
            [Description("Letter of Offer")]
            L,
        }

        public enum StockNonstock
        {
            [Description("Stock")]
            S,
            [Description("Non-Stock")]
            N,
        }

        public enum ServTax
        {
            [Description("Service Tax to be Charged on Fee")]
            Y,
            [Description("Fee is Inclusive of Service Tax")]
            N,
        }

        public enum ClientType
        {
            [Description("Railways")]
            R,
            [Description("Private")]
            P,
            [Description("PSU")]
            U,
            [Description("State Govt")]
            S,
            [Description("Foreign Railways")]
            F,
        }

        public enum RegionCode
        {
            [Description("NORTHERN REGION")]
            N,
            [Description("EASTERN REGION")]
            E,
            [Description("WESTERN REGION")]
            W,
            [Description("SOUTHERN REGION")]
            S,
            [Description("CENTRAL REGION")]
            C,
            [Description("CO QA DIVISION")]
            Q,
        }

        public enum DiscountType
        {
            [Description("Percentage")]
            P,
            [Description("Lumpsum")]
            L,
            [Description("Per No.")]
            N,
        }

        public enum ExciseType
        {
            [Description("Percentage")]
            P,
            [Description("Lumpsum")]
            L,
        }

        public enum VendorStatus
        {
            [Description("Active")]
            A,
            [Description("Banned/BlackListed")]
            B,
            [Description("Re-Instated")]
            R,
        }

        public enum OnlineCallStatus
        {
            [Description("No")]
            N,
            [Description("Yes")]
            Y,
        }

        public enum Department
        {
            [Description("Mechanical")]
            M,
            [Description("Electrical")]
            E,
            [Description("Civil")]
            C,
            [Description("Textiles")]
            T,
            [Description("M & P")]
            Z,
        }

        public enum Region
        {
            [Description("Northern Region")]
            N,
            [Description("Southern Region")]
            S,
            [Description("Eastern Region")]
            E,
            [Description("Westrern Region")]
            W,
            [Description("Central Region")]
            C,
            [Description("QA Corporate")]
            Q,
        }

        public enum BookSubmitted
        {
            [Description("Submitted & Completed")]
            Y,
            [Description("Submitted but not Completed")]
            S,
            [Description("Not Submitted but Completed")]
            C,
            [Description("NO")]
            N,
        }

        public enum Sector
        {
            [Description("All")]
            A,
            [Description("Railways")]
            R,
            [Description("Private")]
            P,
            [Description("PSU")]
            U,
            [Description("State Govt.")]
            S,
            [Description("Foreign Railways")]
            F,
        }

        public enum IcStatus
        {
            [Description("Cancelled")]
            C,
            [Description("Missing")]
            M,
            [Description("Lost / Theft")]
            L,
        }

        public enum ActionProposed
        {
            [Description("No Action Required")]
            N,
            [Description("Warning Letter")]
            W,
            [Description("Minor Penalty")]
            I,
            [Description("Major Penalty")]
            J,
            [Description("Others")]
            O,
        }

        public enum ConsigneeType
        {
            [Description("Railway")]
            R,
            [Description("Private")]
            P,
            [Description("Foreign Railway")]
            F,
            [Description("PSU")]
            U,
            [Description("State Government")]
            S,
        }

        public enum FPart
        {
            [Description("Final")]
            F,
            [Description("Part")]
            P,
        }

        public enum TAXType
        {
            [Description("Fee Inclusive Service Tax")]
            I,
            [Description("Service Tax Charged separately")]
            X,
            [Description("No Service Tax(RITES Billing)")]
            N,
            [Description("Fee Inclusive of Service Tax (Don't Print S.Tax in Bill)")]
            D,
        }

        public enum TaxType_GST
        {
            [Description("IGST @ 18%")]
            I,
            [Description("CGST @ 9% & SGST @ 9%")]
            C,
            [Description("NO GST")]
            X,
            [Description("Fee Inclusive of IGST @ 18%")]
            Y,
            [Description("Fee Inclusive of CGST @ 9% & SGST @ 9%")]
            Z,
        }

        public enum Criteria
        {
            [Description("PO Date")]
            P,
            [Description("Date of Reciept of PO in RITES")]
            R,
        }

        public enum RailTypes
        {
            [Description("Railway")]
            R,
            [Description("Private")]
            P,
            [Description("PSU")]
            U,
            [Description("State Government")]
            S,
        }

        public enum ScopeOfsector
        {
            [Description("(IAF 12) Chemical/Paints")]
            A,
            [Description("(IAF 14b) Plastics Pipes & Fittings")]
            B,
            [Description("(IAF 16) Cement Pipes, AC Pressue Pipes & PCC Poles")]
            C,
            [Description("(IAF 17b)  Rails, CI/DI Pipes, Steel Tubes and Fittings, Seamless Blocl/Galvanised, Valves")]
            D,
            [Description("(IAF 19a) Conductor, Cables, Power Transformers, CT/PT Fans, Relay, Panel, DG set, Alternator, Energy Meter")]
            E,
            [Description("(IAF 22) Railway Rolling Stock")]
            F,
            [Description("(IAF 28) Water Supply")]
            G,
            [Description("(IAF 28) Construction")]
            H,
            [Description("IAF 07) Paper for Printing")]
            I,
            [Description("(IAF 09) Printed Tickes & Ruled Papers")]
            J,
            [Description("Others")]
            O,
        }

        public enum Status
        {
            [Description("Pending")]
            P,
            [Description("Accepted")]
            A,
            [Description("Rejected")]
            R,
        }

        public enum CallsStatus
        {
            [Description("Pending")]
            P,
            [Description("Approved")]
            A,
            [Description("Rejected")]
            R,
        }

        public enum ManagementReportsTitle
        {
            [Description("IE Performance")] IE_X,
            [Description("Cluster Wise Performance Report")] CLUSTER_X,
            [Description("IC Submission Report")] ICSUBMIT,
            [Description("Pending IC's Against Calls where Material has been Sccepted or Rejected")] CALLSWITHOUTIC,
            [Description("CO Wise Super Surprise Summary")] SUPSURPSUMM,
            [Description("Overdue/Pending Calls")] PENDING_CALLS,
            [Description("CM and IE wise IC issued but not recieved")] COUNTIC,
            [Description("Call Details Dashborad")] CALL_DETAILS,
            [Description("Region Wise Billing Summary")] RWB,
            [Description("Region Wise Comparison of Outstanding")] R,
            [Description("Super Surprise Details")] SUPSUR,
            [Description("Online Consignee Rejection Report")] CONSIGN_REJECT,
            [Description("Outstanding of One Region Over Other")] X,
            [Description("Rejection Details Client Wise")] CLIENTWISEREJ,
            [Description("Format for Monthly Non Conformity Report")] NON_CONFORMITY,
            [Description("Tentative Inspection Fee Wise Pending Call")] HIGHVALUE,
            [Description("Call Remarking Detail")] REMARKING,
            [Description("NCR Controlling Wise Report")] C,
            [Description("NCR IE Wise Report")] I,
            [Description("IE Wise Training Details")] IEWISET,
            [Description("Ongoing Contracts")] ONGCON,
            [Description("Contracts")] CONTRACT,
            [Description("Cluster vendor & IE mapping")] CLUSVENDOR,
            [Description("IE's Alternate IE mapping")] IEALTER,
            [Description("Vendor's Performance Report")] VENDPER,
            [Description("Vendor's FeedBack Report")] VENDFEED,
            [Description("Period Wise Progress Of Checksheet")] CHECK,
            [Description("Period Wise Technical Reference")] TECH,
            [Description("Daily IE Work Plan Report")] U,
            [Description("Daily IE Work Plan Exception Report")] E,
            [Description("Download / View Photo Uploaded")] IEICPHOTO,
            [Description("Consignee Complaints For The Period")] CCU,
            [Description("Summary Of Consignee Complaints where JI Required")] COMPJI,
            [Description("JI CONSIGNEE COMPLAINTS")] CORP,
            [Description("Top N HIGH VALUE INSPECTIONS")] TOPNHIGH,
            [Description("Top JI Report")] TOPJI,
            [Description("DEFECT CODE WISE ANALYSIS OF COMPLAINTS")] DCWACOMPS,
            [Description("CONSIGNEE COMPLAINTS")] COCOMPJI,
        }
    }

    public class EnumUtility<T>
    {
        public static IEnumerable<SelectListItem> GetEnumList(Type EnumerationType)
        {
            List<SelectListItem> objDropDown = new();
            Dictionary<int, string> RetVal = new();
            var AllItems = Enum.GetValues(EnumerationType);

            foreach (var CurrentItem in AllItems)
            {
                DescriptionAttribute[] DescAttribute = (DescriptionAttribute[])EnumerationType.GetField(CurrentItem.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
                string Description = DescAttribute.Length > 0 ? DescAttribute[0].Description : CurrentItem.ToString();
                RetVal.Add((int)CurrentItem, Description);
            }

            foreach (int Key in RetVal.Keys)
            {
                objDropDown.Add(new SelectListItem() { Value = Key.ToString(), Text = RetVal[Key] });
            }

            return objDropDown;
        }

        public static IEnumerable<DropDownDTO> GetEnumDropDownIntValue(Type EnumerationType)
        {
            List<DropDownDTO> objDropDown = new();
            Dictionary<int, string> RetVal = new();
            var AllItems = Enum.GetValues(EnumerationType);

            foreach (var CurrentItem in AllItems)
            {
                DescriptionAttribute[] DescAttribute = (DescriptionAttribute[])EnumerationType.GetField(CurrentItem.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
                string Description = DescAttribute.Length > 0 ? DescAttribute[0].Description : CurrentItem.ToString();
                RetVal.Add((int)CurrentItem, Description);
            }

            foreach (int Key in RetVal.Keys)
            {
                objDropDown.Add(new DropDownDTO() { Value = Key, Text = RetVal[Key] });
            }

            return objDropDown;
        }

        public static IEnumerable<TextValueDropDownDTO> GetEnumDropDownStringValue(Type EnumerationType)
        {
            List<TextValueDropDownDTO> objDropDown = new();
            Dictionary<string, string> RetVal = new();
            var AllItems = Enum.GetValues(EnumerationType);

            foreach (var CurrentItem in AllItems)
            {
                DescriptionAttribute[] DescAttribute = (DescriptionAttribute[])EnumerationType.GetField(CurrentItem.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
                string Description = DescAttribute.Length > 0 ? DescAttribute[0].Description : CurrentItem.ToString();
                RetVal.Add(CurrentItem.ToString(), Description);
            }
            foreach (string Key in RetVal.Keys)
            {
                objDropDown.Add(new TextValueDropDownDTO() { Value = Key, Text = RetVal[Key] });
            }

            return objDropDown;
        }

        public static string GetDescriptionByKey(int key)
        {
            Enum e = (Enum)Enum.ToObject(typeof(T), key);
            return e.Description();
        }

        public static string GetDescriptionByKey(string key)
        {
            if (string.IsNullOrEmpty(key)) return string.Empty;
            Enum e = (Enum)Enum.Parse(typeof(T), key);
            return e.Description();
        }
    }

    public static class EnumExtensions
    {
        public static string Description(this Enum enumValue)
        {
            var enumType = enumValue.GetType();
            var field = enumType.GetField(enumValue.ToString());
            var attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length == 0
                ? enumValue.ToString()
                : ((DescriptionAttribute)attributes[0]).Description;
        }
    }

    public class DropDownDTO
    {
        public int Value { get; set; }
        public string Text { get; set; }
        public string Group { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class TextValueDropDownDTO
    {
        public string Value { get; set; }
        public string Text { get; set; }
    }

    public static class GlobalDeclaration
    {
        public static IEPerformanceModel IEPerformance { get; set; }

        public static ConsigneeComplaints ConsigneeComplaint { get; set; }

        public static ClusterPerformanceModel ClusterPerformance { get; set; }

        public static RWBSummaryModel RWBSummary { get; set; }

        public static RWCOModel RWCO { get; set; }

        public static ICSubmissionModel ICSubmission { get; set; }

        public static PendingICAgainstCallsModel PendingICAgainstCalls { get; set; }

        public static SuperSurpriseDetailsModel SuperSurpriseDetails { get; set; }

        public static SuperSurpriseSummaryModel SuperSurpriseSummary { get; set; }

        public static NCRReport NCRReports { get; set; }

        public static JIRequiredReport JIRequiredReports { get; set; }

        public static ConsigneeCompReports ConsigneeCompPeriod { get; set; }

        public static DefectCodeReport DefectCodeReports { get; set; }

        public static HighValueInspReport HighValueInspReports { get; set; }

        public static VendorClusterReportModel VendorClusterReport { get; set; }

        public static IEAlterMappingReportModel IEAlterMappingReport { get; set; }

        public static VendorFeedbackReportModel VendorFeedbackReport { get; set; }

        public static ControllingOfficerIEModel ControllingOfficerIE { get; set; }

        public static OngoingContrcatsReportModel OngoingContrcatsReport { get; set; }

        public static ContractReportModel ContractReport { get; set; }

        public static IEWiseTrainingReportModel IEWiseTrainingReport { get; set; }

        public static VendorPerformanceReportModel VendorPerformanceReport { get; set; }

        public static PeriodWiseChecksheetReportModel PeriodWiseChecksheetReport { get; set; }

        public static ConsignRejectModel ConsignReject { get; set; }

        public static OutstandingOverRegionModel OutstandingOverRegion { get; set; }

        public static ClientWiseRejectionModel ClientWiseRejection { get; set; }

        public static CoIeWiseCallsModel CoIeWiseCalls { get; set; }

        public static PeriodWiseTechnicalRefReportModel PeriodWiseTechnicalRefReport { get; set; }

        public static DailyIECMWorkPlanReportModel DailyIECMWorkPlanReport { get; set; }

        public static NonConformityModel NonConformity { get; set; }

        public static PendingCallsModel PendingCalls { get; set; }

        public static ICIssuedNotReceivedModel ICIssuedNotReceived { get; set; }

        public static TentativeInspectionFeeWisePendingCallsModel TentativeInspectionFeeWisePendingCalls { get; set; }

        public static Models.Reports.CallRemarkingModel CallRemarking { get; set; }

        public static CallDetailsDashboradModel CallDetailsDashborad { get; set; }

        public static AllICStatusModel AllICStatus { get; set; }
        public static ReInspectionICsModel ReInspectionICs { get; set; }
        public static IEICPhotoEnclosedModelReport IEICPhotoEnclosedModel { get; set; }
        public static ICUnbilledModel ICUnbilled { get; set; }
        public static IE7thCopyListModel IE7thCopyList { get; set; }
        public static ICIssuedNotReceivedReportModel ICIssuedNotReceivedReport{ get; set; }
        public static ICStatusModel ICStatus { get; set; }
        public static PendingJICasesReportModel PendingJICasesReport { get; set; }
        public static IEDairyModel IEDairy { get;set; }
        public static IEWorkPlanModel IEWorkPlan { get; set; }
    }
}
