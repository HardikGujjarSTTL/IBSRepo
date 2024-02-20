using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Interfaces.Administration;
using IBS.Interfaces.Hub;
using IBS.Interfaces.IE;

using IBS.Interfaces.Inspection_Billing;
using IBS.Interfaces.InspectionBilling;
using IBS.Interfaces.Reports;
using IBS.Interfaces.Reports.Billing;
using IBS.Interfaces.Reports.ConsigneeComplaintReports;
using IBS.Interfaces.Reports.OtherReports;
using IBS.Interfaces.Reports.RealisationPayment;
using IBS.Interfaces.Transaction;
using IBS.Interfaces.Vendor;
using IBS.Interfaces.WebsitePages;
using IBS.Repositories;
using IBS.Repositories.Hub;
using IBS.Repositories.Inspection_Billing;
using IBS.Repositories.InspectionBilling;
using IBS.Repositories.Reports;
using IBS.Repositories.Reports.ConsigneeComplaintReports;
using IBS.Repositories.Reports.OtherReports;
using IBS.Repositories.Reports.RealisationPayment;
using IBS.Repositories.Transaction;
using IBS.Repositories.Vendor;
using IBS.Repositories.WebsitePages;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using System.Globalization;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ModelContext>(options => options.UseOracle(connectionString));

IMvcBuilder mvcBuilder = builder.Services.AddControllersWithViews();
// Add services to the container.

mvcBuilder.AddRazorRuntimeCompilation();

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

builder.Services.AddMvc(option => option.EnableEndpointRouting = false);

builder.Services.AddHttpContextAccessor();

var accessor = builder.Services.BuildServiceProvider().GetService<IHttpContextAccessor>();
SessionHelper.SetHttpContextAccessor(accessor);

builder.Services.AddSession(options => { options.IdleTimeout = TimeSpan.FromMinutes(60); });

builder.Services.AddDataProtection();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IVendorProfileRepository, VendorProfileRepository>();
builder.Services.AddScoped<IGeneralMessageRepository, GeneralMessageRepository>();
builder.Services.AddScoped<IMasterTableStatusRepository, MasterTableStatusRepository>();
builder.Services.AddScoped<IAllow_Old_Bill_DateRepository, Allow_Old_Bill_DateRepository>();
builder.Services.AddScoped<IIE_Instructions_AdminRepository, IE_Instructions_AdminRepository>();
builder.Services.AddScoped<IDownloadDocumentsRepository, DownloadDocumentsRepository>();
builder.Services.AddScoped<IPOMasterRepository, POMasterRepository>();
builder.Services.AddScoped<IContractRepository, ContractRepository>();
builder.Services.AddScoped<IDocument, Document>();
builder.Services.AddScoped<IDEOVendorPurchesOrderRepository, DEOVendorPurchesOrderRepository>();
builder.Services.AddScoped<IDEOCRISPurchesOrderRepository, DEOCRISPurchesOrderRepository>();
builder.Services.AddScoped<IMAapproveRepository, MAapproveRepository>();
builder.Services.AddScoped<IDEOCRISPurchesOrderWCaseNoRepository, DEOCRISPurchesOrderWCaseNoRepository>();
builder.Services.AddScoped<IIEMessageRepository, IEMessageRepository>();
builder.Services.AddScoped<ICallMarkedToIERepository, CallMarkedToIERepository>();
builder.Services.AddScoped<ILaboratoryMstRepository, LaboratoryMstRepository>();
builder.Services.AddScoped<IUploadDocRepository, IBS.Repositories.Administration.UploadDocRepository>();
builder.Services.AddScoped<IVendorCallRegisterRepository, IBS.Repositories.Vendor.VendorCallRegisterRepository>();
builder.Services.AddScoped<IPurchesOrder1LOARepository, IBS.Repositories.Vendor.PurchesOrder1LOARepository>();
builder.Services.AddScoped<ICallRegisterRepository, IBS.Repositories.Vendor.CallRegisterRepository>();
builder.Services.AddScoped<IDownloadInspFeeBillRepository, IBS.Repositories.Vendor.DownloadInspFeeBillRepository>();
builder.Services.AddScoped<IVendorCallsMarkedForSpecificPORepository, IBS.Repositories.Vendor.VendorCallsMarkedForSpecificPORepository>();
builder.Services.AddScoped<IVendorPOMARepository, IBS.Repositories.Vendor.VendorPOMARepository>();
//builder.Services.AddScoped<IOnlineComplaintsRepository, IBS.Repositories.OnlineComplaintsRepository>();
builder.Services.AddScoped<IOnlineComplaintsRepository, OnlineComplaintsRepository>();
builder.Services.AddScoped<IOnlinePaymentGatewayRepository, OnlinePaymentGatewayRepository>();
builder.Services.AddScoped<IFeedbackSuggestionRepository, FeedbackSuggestionRepository>();
builder.Services.AddScoped<IBillRegisterRepository, IBS.Repositories.Reports.BillRegisterRepository>();
builder.Services.AddScoped<IDailyWorkPlanRepository, IBS.Repositories.IE.DailyWorkPlanRepository>();
builder.Services.AddScoped<IICPhotoEnclosedRepository, IBS.Repositories.IE.ICPhotoEnclosedRepository>();
builder.Services.AddScoped<IIEJIRemarksPendingRepository, IBS.Repositories.IE.IEJIRemarksPendingRepository>();
builder.Services.AddScoped<IComplaintApprovalRepository, IBS.Repositories.ComplaintApprovalRepository>();
builder.Services.AddScoped<ITransactionQAVideosRepository, IBS.Repositories.IE.TransactionQAVideosRepository>();
builder.Services.AddScoped<IQuality_Manual_ProceduresRepository, IBS.Repositories.IE.Quality_Manual_ProceduresRepository>();

builder.Services.AddScoped<ICMDailyWorkPlanRepository, IBS.Repositories.CMDailyWorkPlanRepository>();

builder.Services.AddScoped<ICallRegisterIBRepository, IBS.Repositories.InspectionBilling.CallRegisterIBRepository>();
builder.Services.AddScoped<IInspectionCertRepository, IBS.Repositories.InspectionBilling.InspectionCertRepository>();
builder.Services.AddScoped<IBillAdjustmentsRepository, IBS.Repositories.InspectionBilling.BillAdjustmentsRepository>();
builder.Services.AddScoped<ISupplementaryBillRepository, IBS.Repositories.InspectionBilling.SupplementaryBillRepository>();

builder.Services.AddScoped<IBillRegisterRepository, IBS.Repositories.Reports.BillRegisterRepository>();
builder.Services.AddScoped<IBillRaisedRepository, IBS.Repositories.Reports.Billing.BillRaisedRepository>();
builder.Services.AddScoped<IFinanceReportsRepository, IBS.Repositories.Reports.FinanceReportsRepository>();

builder.Services.AddScoped<IMultipleFileUploadRepository, MultipleFileUploadRepository>();

builder.Services.AddScoped<IBillCheckPostingRepository, BillCheckPostingRepository>();

builder.Services.AddScoped<IRemitanceReportsRepository, IBS.Repositories.Reports.RemitanceReportsRepository>();

builder.Services.AddScoped<ISearchRepository, IBS.Repositories.SearchRepository>();
builder.Services.AddScoped<IContractEntryRepository, IBS.Repositories.ContractEntryRepository>();
builder.Services.AddScoped<IClientMasterRepository, IBS.Repositories.ClientMasterRepository>();
builder.Services.AddScoped<INonRlyClientMasterRepository, IBS.Repositories.NonRlyClientMasterRepository>();
builder.Services.AddScoped<IpfrmFromToDateRepository, IBS.Repositories.pfrmFromToDateRepository>();
builder.Services.AddScoped<IConsigneeComplaintsRepository, IBS.Repositories.ConsigneeComplaintsRepository>();
builder.Services.AddScoped<INCRRegisterRepository, IBS.Repositories.NCRRegisterRepository>();
builder.Services.AddScoped<IUnitOfMeasurementsRepository, UnitOfMeasurementsRepository>();
builder.Services.AddScoped<IRitesDesignationMasterRepository, RitesDesignationMasterRepository>();
builder.Services.AddScoped<IRailwaysDirectoryRepository, RailwaysDirectoryRepository>();
builder.Services.AddScoped<IRailwaysDesignationRepository, RailwaysDesignationRepository>();
builder.Services.AddScoped<IBankRepository, BankRepository>();
builder.Services.AddScoped<IAccountCodesDirectoryRepository, AccountCodesDirectoryRepository>();
builder.Services.AddScoped<IClientContractRepository, ClientContractRepository>();
builder.Services.AddScoped<IMasterItemsListForm, MasterItemsListForm>();
builder.Services.AddScoped<IConsignee_PMForm, Consignee_PMForm>();
builder.Services.AddScoped<IConsigneePurchaseRepository, ConsigneePurchaseRepository>();
builder.Services.AddScoped<IInspectionEngineers, InspectionEngineers>();
builder.Services.AddScoped<I_IE_CO_FormRepository, IE_CO_FormRepository>();
builder.Services.AddScoped<IBill_Paying_Officer_Form, Bill_Paying_Officer_Form>();
builder.Services.AddScoped<IClusterMasterRepository, ClusterMasterRepository>();
builder.Services.AddScoped<ILabBillingRepository, LabBillingRepository>();
builder.Services.AddScoped<IExpenditureRepository, ExpenditureRepository>();
builder.Services.AddScoped<ITechReferenceRepository, TechReferenceRepository>();
builder.Services.AddScoped<IHighlightsRepository, HighlightsRepository>();
builder.Services.AddScoped<IBillingOperatingTargetRepository, BillingOperatingRepository>();
builder.Services.AddScoped<IBillingAdjustmentRepository, BillingAdjustmentRepository>();
builder.Services.AddScoped<ILastYearOutstandingRepository, LastYearOutstandingRepository>();
builder.Services.AddScoped<IRecieptVoucherRepository, RecieptVoucherRepository>();
builder.Services.AddScoped<IInterUnitRecieptRepository, InterUnitRecieptRepository>();

builder.Services.AddScoped<IVendorDocumentRepository, VendorDocumentRepository>();
builder.Services.AddScoped<ISendMailRepository, SendMailRepository>();
builder.Services.AddScoped<ICentralQOIRepository, CentralQOIRepository>();
builder.Services.AddScoped<ICentralQOIIRepository, CentralQOIIRepository>();
#region Inspection and Billing
builder.Services.AddScoped<IHologramAccountalRepository, HologramAccountalRepository>();
builder.Services.AddScoped<IIC_ReceiptRepository, IC_ReceiptRepository>();
builder.Services.AddScoped<ICallMarkedOnlineRepository, CallMarkedOnlineRepository>();
#endregion
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<I_ICBooksetFormRepository, ICBooksetFormRepository>();
builder.Services.AddScoped<IVendorClusterRepository, VendorClusterRepository>();
builder.Services.AddScoped<IHologramSearchForm, HologramSearchForm>();
builder.Services.AddScoped<I_IE_MaximumCallLimitForm, IE_MaximumCallLimitForm>();
builder.Services.AddScoped<IMasterItemsPLFormRepository, MasterItemsPLFormRepository>();
builder.Services.AddScoped<ICentralRejectionStatusRepository, CentralRejectionStatusRepository>();
builder.Services.AddScoped<ICheckPostingFormRepository, CheckPostingFormRepository>();
builder.Services.AddScoped<ISearchPaymentsRepository, SearchPaymentRepository>();
builder.Services.AddScoped<IEFTEntryRepository, EFTEntryRepository>();
builder.Services.AddScoped<IInterUnit_TransferRepository, InterUnit_TransferRepository>();
builder.Services.AddScoped<IUnregisteredCallsRepository, UnregisteredCallsRepository>();
builder.Services.AddScoped<IInspectionFeeBillRepository, InspectionFeeBillRepository>();
builder.Services.AddScoped<ITDSEntryRepository, TDSEntryRepository>();
builder.Services.AddScoped<IIEClaimFormRepository, IEClaimFormRepository>();
builder.Services.AddScoped<IRly_Online_Check_Posting_Form_Repository, Rly_Online_Check_Posting_Form_Rpository>();
builder.Services.AddScoped<ICMIEWiseCancellationAcceptance_FormRepository, CMIEWiseCancellationAcceptance_FormRepository>();
builder.Services.AddScoped<IPrint_Call_letter_Repository, Print_Call_letter_Repository>();
builder.Services.AddScoped<ICallMarkedRepository, CallMarkedRepository>();
builder.Services.AddScoped<ICalls_Marked_ReportRepository, Calls_Marked_ReportRepository>();
builder.Services.AddScoped<ICall_Cancellation_FormRepository, Call_Cancellation_FormRepository>();
builder.Services.AddScoped<ICalls_Marked_For_Specific_PORepository, Calls_Marked_For_Specific_PORepository>();
builder.Services.AddScoped<ICallsReportRepository, CallsReportRepository>();
builder.Services.AddScoped<IDailyIEWiseCallsRepository, DailyIEWiseCallsRepository>();
builder.Services.AddScoped<IWriteOffEntryRepository, WriteOffEntryRepository>();
builder.Services.AddScoped<IPrint_Bank_Statement_VoucherRepository, Print_Bank_Statement_VoucherRepository>();
builder.Services.AddScoped<IClientRailwayRepository, IBS.Repositories.ClientRailwayRepository>();
builder.Services.AddScoped<IAllGeneratedBillsRepository, IBS.Repositories.AllGeneratedBillsRepository>();
builder.Services.AddScoped<ILabInvoiceRepository, IBS.Repositories.LabInvoiceRepository>();




#region IE Report

#endregion

builder.Services.AddScoped<IReportsRepository, ReportsRepository>();
builder.Services.AddScoped<ILabTDSEntryRepository, LabTDSEntryRepository>();
builder.Services.AddScoped<ILabRegFormRepository, LabRegiFormRepository>();
builder.Services.AddScoped<ILabRecieptVoucherRepository, LabRecieptVoRepository>();
builder.Services.AddScoped<ILabPaymentFormRepository, LabPaymentRRepository>();
builder.Services.AddScoped<ILabSampleInfoRepository, LabSampleRepository>();
builder.Services.AddScoped<ILabBillFinalisationRepository, LabBillFinalRepository>();
builder.Services.AddScoped<IVendorLabSampleInfoRepository, VendorLabSampleRepository>();
builder.Services.AddScoped<ILabPaymentListRepository, LabPaymentListRRepository>();
builder.Services.AddScoped<ILabInvoiceDownloadRepository, LabInvoiceDownloadRRepository>();
builder.Services.AddScoped<ILabSamplePaymentRptRepository, LabSamplePaymentRptRRepository>();
builder.Services.AddScoped<IClientCallStatusRepository, ClientCallStatusRRepository>();
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<I_ICBooksetFormRepository, ICBooksetFormRepository>();
builder.Services.AddScoped<IVendorRepository, VendorRepository>();
//builder.Services.AddScoped<IHologramSearchForm, HologramSearchForm>();
builder.Services.AddScoped<I_IE_MaximumCallLimitForm, IE_MaximumCallLimitForm>();
builder.Services.AddScoped<IMasterItemsPLFormRepository, MasterItemsPLFormRepository>();
builder.Services.AddScoped<IClientEntryForm, ClientEntryForm>();
builder.Services.AddScoped<ISpecificPOCallStatusRepository, SpecificPOCallStatusRRepository>();
builder.Services.AddScoped<ILabInvoiceRptRepository, LabInvoiceRptRRepository>();
builder.Services.AddScoped<IReturnedBillsRepository, ReturnedBillsRRepository>();
builder.Services.AddScoped<IBillFinalisationFormRepository, BillFinalisationFormRepository>();
builder.Services.AddScoped<IICCancellationRepository, ICCancellationRepository>();
builder.Services.AddScoped<ICallRemarkingRepository, CallRemarkingRepository>();
builder.Services.AddScoped<IVigilanceCaseMonitoringRepository, VigilanceCaseMonitoringRepository>();
builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();
builder.Services.AddScoped<IDownloadBillsRepository, DownloadBillsRRepository>();
builder.Services.AddScoped<IBillRemarksRepository, BillRemarksRRepository>();
builder.Services.AddScoped<IETrainingDetailsRepository, IETrainingDetailsRRepository>();
builder.Services.AddScoped<IAdministratorPurchaseOrderRepository, AdministratorPurchaseOrderRepository>();
builder.Services.AddScoped<ICentralRegionBillingInformationRepository, CentralRegionBillingInformationRepository>();
builder.Services.AddScoped<ISuperSurpirseFormRepository, SuperSurpirseFormRRepository>();
builder.Services.AddScoped<ICentralItemMasterRepository, CentralItemMasterRepository>();
builder.Services.AddScoped<IInspectionBillingDelayRepository, InspectionBillingDelayRepository>();
builder.Services.AddScoped<IConsigneeComplaintsReportRepository, ConsigneeComplaintsReportRepository>();

builder.Services.AddScoped<IRegionalHRDataOfIERepository, RegionalHRDataOfIERepository>();
builder.Services.AddScoped<ILabRegisterReportRepository, LabRegisterReportRRepository>();
builder.Services.AddScoped<ILabPerfomanceReportRepository, LabPerformanceReportRRepository>();
builder.Services.AddScoped<ILabPostingReportRepository, LabPostingReportRRepository>();
builder.Services.AddScoped<IOnlinePaymentReportRepository, OnlinePaymentReportRRepository>();
builder.Services.AddScoped<ILabInvoiceReportRepository, LabInvoiceReportRRepository>();
builder.Services.AddScoped<ILabSamInfoReportRepository, LabSamInfoReportRRepository>();

builder.Services.AddScoped<IManagementReportsRepository, ManagementReportsRepository>();
builder.Services.AddScoped<IPurchaseOrdersofSpecificValuesRepository, PurchaseOrdersofSpecificValuesRepository>();

builder.Services.AddScoped<IPCDOReportRepository, PCDOReportRepository>();
builder.Services.AddScoped<IProjectDetailsRepository, ProjectDetailsRepository>();

builder.Services.AddScoped<IRealisationPaymentRepository, RealisationPaymentRepository>();
builder.Services.AddScoped<IInspectionStatusRepository, InspectionStatusRRepository>();
builder.Services.AddScoped<ILabReportsRepository, LabReportsRRepository>();
builder.Services.AddScoped<IOtherReportsRepository, OtherReportsRepository>();
builder.Services.AddScoped<IConsigneeCompReportRepository, ConsigneeCompReportRepository>();
builder.Services.AddScoped<IReceiptsRemitanceRepository, ReceiptsRemitanceRRepository>();
builder.Services.AddScoped<IBillingRepository, BillingRRepository>();
builder.Services.AddScoped<IDailyIEWiseCallsRepository, DailyIEWiseCallsRepository>();
builder.Services.AddScoped<IMonthlyReportsRepository, MonthlyReportsRepository>();
builder.Services.AddScoped<IIC_RPT_IntermediateRepository, IC_RPT_IntermediateRepository>();
builder.Services.AddScoped<IBarcodeGeneration, BarcodeGenerationRepository>();
builder.Services.AddScoped<ILabSearchPaymentRepository, LabSearchPaymentsRepository>();
builder.Services.AddScoped<IBPOWiseOutstandingBillsRepository, BPOWiseOutRRepository>();

builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
builder.Services.AddScoped<ISAPIntegrationRepository, SAPIntegrationRepository>();
builder.Services.AddScoped<IHolidayMasterRepository, HolidayMasterRepository>();

// SignalR Class and Configuration
builder.Services.AddScoped<IChatRepository, ChatRepository>();
builder.Services.AddScoped<ChatHub>();

// SignalR Configuration
//builder.Services.AddSignalR();
builder.Services.AddSignalR(e =>
{
    e.MaximumReceiveMessageSize = 102400000;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


var supportedCultures = new[]
{
 new CultureInfo("en-GB")
};

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-GB"),
    // Formatting numbers, dates, etc.
    SupportedCultures = supportedCultures,
    // UI strings that we have localized.
    SupportedUICultures = supportedCultures
});

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// SignalR Configuration
app.MapHub<ChatHub>("/chatHub");

app.Run();

