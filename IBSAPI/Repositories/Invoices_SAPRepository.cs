using IBSAPI.DataAccess;
using IBSAPI.Helper;
using IBSAPI.Interfaces;
using IBSAPI.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace IBSAPI.Repositories
{
    public class Invoices_SAPRepository : IInvoices_SAPRepository
    {

        private readonly ModelContext context;

        public Invoices_SAPRepository(ModelContext context)
        {
            this.context = context;
        }

        public List<Invoices_SAPModel> FindInvoiceList(DateTime frmdt, DateTime todt)
        {
            List<Invoices_SAPModel> lst = new();
            OracleParameter[] par = new OracleParameter[3];
            par[0] = new OracleParameter("p_start_date", OracleDbType.Varchar2, Convert.ToDateTime(frmdt).ToString("dd/MM/yyyy"), ParameterDirection.Input);
            par[1] = new OracleParameter("p_end_date", OracleDbType.Varchar2, Convert.ToDateTime(todt).ToString("dd/MM/yyyy"), ParameterDirection.Input);
            par[2] = new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GET_INVOICE_SAP_LIST", par, 1);
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                lst = dt.AsEnumerable().Select(row => new Invoices_SAPModel
                {
                    InvoiceType01 = Convert.ToString(row["INVOICE_TYPE_01"]),
                    Customer01 = Convert.ToString(row["CUSTOMER_01"]),
                    DocumentDate = Convert.ToString(row["DOCUMENT_DATE"]),
                    Reference01 = Convert.ToString(row["REFERENCE_01"]),
                    PostingDate = Convert.ToString(row["POSTING_DATE"]),
                    DocumentType01 = Convert.ToString(row["DOCUMENT_TYPE_01"]),
                    Amount01 = Convert.ToInt32(row["AMOUNT_01"]),
                    Currency = Convert.ToString(row["CURRENCY"]),
                    CalculateTax01 = Convert.ToString(row["CALCULATE_TAX_01"]),
                    DateOfCompletion01 = Convert.ToString(row["DATE_OF_COMPLETION_01"]),
                    InvoiceType02 = Convert.ToString(row["INVOICE_TYPE_02"]),
                    Customer02 = Convert.ToString(row["CUSTOMER_02"]),
                    Reference02 = Convert.ToString(row["REFERENCE_02"]),
                    DocumentType02 = Convert.ToString(row["DOCUMENT_TYPE_02"]),
                    Amount02 = Convert.ToDecimal(row["AMOUNT_02"]),
                    CalculateTax02 = Convert.ToString(row["CALCULATE_TAX_02"]),
                    BusinessPlace01 = Convert.ToString(row["BUSINESS_PLACE_01"]),
                    SectionCode01 = Convert.ToString(row["SECTION_CODE_01"]),
                    Text01 = Convert.ToString(row["TEXT_01"]),
                    DateOfCompletion02 = Convert.ToString(row["DATE_OF_COMPLETION_02"]),
                    GstPartner01 = Convert.ToString(row["GST_PARTNER_01"]),
                    PlaceOfSupply01 = Convert.ToString(row["PLACE_OF_SUPPLY_01"]),
                    InvoiceType03 = Convert.ToString(row["INVOICE_TYPE_03"]),
                    Customer03 = Convert.ToString(row["CUSTOMER_03"]),
                    Reference03 = Convert.ToString(row["REFERENCE_03"]),
                    DocumenyType03 = Convert.ToString(row["DOCUMENY_TYPE_03"]),
                    Amount03 = Convert.ToDecimal(row["AMOUNT_03"]),
                    CalculateTax03 = Convert.ToString(row["CALCULATE_TAX_03"]),
                    BusinessPlace02 = Convert.ToString(row["BUSINESS_PLACE_02"]),
                    SectionCode02 = Convert.ToString(row["SECTION_CODE_02"]),
                    Text02 = Convert.ToString(row["TEXT_02"]),
                    DateOfCompletion03 = Convert.ToString(row["DATE_OF_COMPLETION_03"]),
                    GstPartner02 = Convert.ToString(row["GST_PARTNER_02"]),
                    PlaceOfSupply02 = Convert.ToString(row["PLACE_OF_SUPPLY_02"]),
                    GlAccount = Convert.ToString(row["GL_ACCOUNT"]),
                    Amount04 = Convert.ToDecimal(row["AMOUNT_04"]),
                    GstTaxCode = Convert.ToString(row["GST_TAX_CODE"]),
                    Text03 = Convert.ToString(row["TEXT_03"]),
                    Wbs = Convert.ToString(row["WBS"]),
                    HsnSacCode = Convert.ToString(row["HSN_SAC_CODE"]),
                }).ToList();
            }
            return lst;
        }
    }
}
