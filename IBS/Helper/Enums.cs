using System.ComponentModel;

namespace IBS.Helper
{
    public class Enums
    {
        public enum UserTypes
        {
            [Description("Super User")] SuperUser = 0,
            [Description("User")] User = 1,
        }

        public enum MasterTypes
        {
            [Description("Bank")] Bank = 0,
            [Description("Billing Status")] Billing_Status = 1,
            [Description("Company")] Company = 2,
            [Description("Contact To")] Contact_To = 3,
            [Description("Insure Status")] Insure_Status = 4,
            [Description("Lender")] Lender = 5,
            [Description("Landlord Status")] Landlord_Status = 6,
            [Description("Manager")] Manager = 7,
            [Description("Payment Type")] Payment_Type = 8,
            [Description("Tenure")] Tenure = 9,
            [Description("Rent Frequency")] Rent_Frequency = 10,
            [Description("Invoice Category")] Invoice_Category = 11,
            [Description("Statement Format Type")] Statement_Format_Type = 12,
            [Description("Rent Type")] Rent_Type = 13,
            [Description("Letting Status")] Letting_Status = 14,
            [Description("Lease Type")] Lease_Type = 15,
            [Description("Account Status")] Account_Status = 16,
            [Description("DMS _DocCategory")] DMS_DocCategory = 17,
            [Description("Issue Type")] Issue_Type = 18,
            [Description("Task For")] Task_For = 19,
            [Description("Task Status")] Task_Status = 20,
            [Description("Account Type")] Account_Type = 21,
            [Description("Appointment Category")] Appointment_Category = 22,
        }
        public enum LandlordType
        {
            [Description("Management")] Management = 1,
            [Description("Non Management")] NonManagement = 2,
        }

        public enum PayInterestOnBal
        {
            [Description("None")] None = 1,
            [Description("Fixed")] Fixed = 2,
            [Description("Variable")] Variable = 3,
        }

        public enum PayInterestOnRent
        {
            [Description("None")] None = 1,
            [Description("Fixed")] Fixed = 2,
            [Description("Variable")] Variable = 3,
        }

        public enum LandlordAccountMode
        {
            [Description("Actual")] Actual = 1,
            [Description("Adjust")] Adjust = 2,
        }

        public enum ContactType
        {
            [Description("General Contact")] GeneralContact = 0,
            [Description("LandLord Contact")] LandLordContact = 1,
            [Description("Tenant Contact")] TenantContact = 2,
            [Description("Tenant Reference")] TenantReference = 3,
            [Description("Tenant Guarantor")] TenantGuarantor = 4,
            [Description("Tenant Mortgagee")] TenantMortgagee = 5,
            [Description("Property Contact")] PropertyContact = 6,
            [Description("Trustee Contact")] TrusteeContact = 7,
            [Description("DK Contact")] DKContact = 8,
            [Description("Solicitor Contact")] SolicitorContact = 9,
            [Description("Surveyor Contact")] SurveyorContact = 10,
            [Description("Insurance Contact")] InsuranceContact = 11,
            [Description("Auctioneer Contact")] AuctioneerContact = 12,
            [Description("Agency Contact")] AgencyContact = 13,
            [Description("Valuer Contact")] ValuerContact = 14,
        }


        public enum Tenure
        {
            [Description("Freehold")]
            Freehold = 1,
            [Description("Leasehold")]
            Leasehold = 2,
        }

        public enum InsureBy
        {
            [Description("Insured by Other")]
            InsuredbyOther = 1,
            [Description("Insured by Landlord")]
            InsuredbyLandlord = 2,
            [Description("Insured by Tenant")]
            InsuredbyTenant = 3,

        }

        public enum Permissions_Enum
        {
            ADD_EDIT_ACCESS,
            VIEW_ACCESS,
        }
        public enum Module_Name
        {
            ItmLandLord,
            ItmProperty,
            ItmTenant,
            ItmTrustee,
            ItmAgency,
            ItmUser,
            ItmTask,
            ItmDMS,
            ItmReport,
            ItmSystemSetup,
            ItmLetterGeneration,
            ItmAccount
        }
        public enum SUB_Module_Name
        {
            Landlord,
            LLStatement,
            LLAccounts,
            Property,
            CostForecast,
            TNLedger,
            Tenant,
            ChargingSchedule,
            PeriodicCharge,
            TNStatement,
            TNAccounts,
            Trustee,
            UBO,
            Director,
            Agency,
            AgencyProperty,
            UsersPermissions,
            UserAudit,
            Task,
            TaskReport,
            DocumentCategory,
            DMS,
            Reports,
            TrusteeRpt,
            Insurance,
            Auctioneer,
            Contacts,
            Maintenance,
            Solicitor,
            Surveyor,
            DKCompany,
            ImportData,
            Supplier,
            SendMail,
            Master,
            LettersList,
            ScannedLetters,
            UnscannedLetters,
            LetterGeneration,
            VATSummary,
            Invoice,
        }


        public enum CallDetailType
        {
            [Description("LandLord")] LandLord = 1,
        }


    }

    public class EnumUtility<T>
    {
        public static T GetEnumByKey(int key)
        {
            return (T)Enum.ToObject(typeof(T), key);
        }

        public static T GetEnumByValueString(string valueString)
        {
            return (T)Enum.Parse(typeof(T), valueString, true);
        }

        public static T GetEnumByValueId(int Id)
        {
            return (T)Enum.Parse(typeof(T), Id.ToString(), true);
        }

        public static int GetIdByValueString(string valueString)
        {
            return (int)Enum.Parse(typeof(T), valueString);
        }

        public static string GetValueById(int Id)
        {
            return (string)Enum.Parse(typeof(string), Id.ToString(), true);
        }

        public static string GetDescriptionByKey(int key)
        {
            Enum e = (Enum)Enum.ToObject(typeof(T), key);
            return e.Description();
        }

        public static T GetEnum(int Id)
        {
            T t;
            t = (T)Enum.ToObject(typeof(T), Id);
            return t;
        }

        public static T GetEnum(string emumString)
        {
            T t;
            t = (T)Enum.Parse(typeof(T), emumString);
            return t;
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
                objDropDown.Add(new TextValueDropDownDTO() { Value = RetVal[Key], Text = RetVal[Key] });
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
}
