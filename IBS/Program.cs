using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Interfaces.Administration;
using IBS.Interfaces.IE;
using IBS.Interfaces.IE_Reports;
using IBS.Interfaces.Inspection_Billing;
using IBS.Interfaces.InspectionBilling;
using IBS.Interfaces.Reports;
using IBS.Interfaces.Vendor;
using IBS.Repositories;
using IBS.Repositories.IE_Report;
using IBS.Repositories.Inspection_Billing;
using IBS.Repositories.InspectionBilling;
using IBS.Repositories.Reports;
using IBS.Repositories.Vendor;
using Microsoft.EntityFrameworkCore;
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

builder.Services.AddHttpContextAccessor();

//builder.Services.AddAuthentication("CookieAuthentication")
//                 .AddCookie("CookieAuthentication", config =>
//                 {
//                     config.Cookie.Name = "UserLoginCookie"; // Name of cookie   
//                     config.LoginPath = "/Home/Index"; // Path for the redirect to user login page  
//                     config.AccessDeniedPath = "/User/UserAccessDenied";
//                 });

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
builder.Services.AddScoped<IOnlineComplaintsRepository, IBS.Repositories.OnlineComplaintsRepository>();


builder.Services.AddScoped<IBillRegisterRepository, IBS.Repositories.Reports.BillRegisterRepository>();

builder.Services.AddScoped<IDailyWorkPlanRepository, IBS.Repositories.IE.DailyWorkPlanRepository>();
builder.Services.AddScoped<IICPhotoEnclosedRepository, IBS.Repositories.IE.ICPhotoEnclosedRepository>();
builder.Services.AddScoped<IIEJIRemarksPendingRepository, IBS.Repositories.IE.IEJIRemarksPendingRepository>();
builder.Services.AddScoped<IComplaintApprovalRepository, IBS.Repositories.ComplaintApprovalRepository>();
builder.Services.AddScoped<ITransactionQAVideosRepository, IBS.Repositories.IE.TransactionQAVideosRepository>();

builder.Services.AddScoped<ICallRegisterIBRepository, IBS.Repositories.InspectionBilling.CallRegisterIBRepository>();


builder.Services.AddScoped<IConsigneeComplaintsRepository, IBS.Repositories.ConsigneeComplaintsRepository>();
builder.Services.AddScoped<INCRRegisterRepository, IBS.Repositories.NCRRegisterRepository>();
builder.Services.AddScoped<IUnitOfMeasurements, UnitOfMeasurements>();
builder.Services.AddScoped<IRitesDesignationMaster, RitesDesignationMaster>();
builder.Services.AddScoped<IRailwaysDirectory, RailwaysDirectory>();
builder.Services.AddScoped<IRly_Designation_Form, Rly_Designation_Form>();
builder.Services.AddScoped<IBankMaster, BankMaster>();
builder.Services.AddScoped<IAccountCodesDirectory, AccountCodesDirectory>();
builder.Services.AddScoped<IClientContractRepository, ClientContractRepository>();
builder.Services.AddScoped<IMasterItemsListForm, MasterItemsListForm>();
builder.Services.AddScoped<IConsignee_PMForm, Consignee_PMForm>();
builder.Services.AddScoped<IInspectionEngineers, InspectionEngineers>();
builder.Services.AddScoped<I_IE_CO_Form, IE_CO_Form>();
builder.Services.AddScoped<IBill_Paying_Officer_Form, Bill_Paying_Officer_Form>();
builder.Services.AddScoped<IClusterMaster, ClusterMaster>();
builder.Services.AddScoped<ILabBillingRepository, LabBillingRepository>();
builder.Services.AddScoped<IExpenditureRepository, ExpenditureRepository>();
builder.Services.AddScoped<ITechReferenceRepository, TechReferenceRepository>();
builder.Services.AddScoped<IHighlightsRepository, HighlightsRepository>();
builder.Services.AddScoped<IBillingOperatingTargetRepository, BillingOperatingRepository>();
builder.Services.AddScoped<IBillingAdjustmentRepository, BillingAdjustmentRepository>();
builder.Services.AddScoped<ILastYearOutstandingRepository, LastYearOutstandingRepository>();


builder.Services.AddScoped<IAddRecieptVoucher, AddRecieptVoucherRepository>();
builder.Services.AddScoped<IVendorDocumentRepository, VendorDocumentRepository>();
builder.Services.AddScoped<ISendMailRepository, SendMailRepository>();
builder.Services.AddScoped<ICentralQOIRepository, CentralQOIRepository>();
builder.Services.AddScoped<ICentralQOIIRepository, CentralQOIIRepository>();


#region Inspection and Billing
builder.Services.AddScoped<IHologramAccountalRepository, HologramAccountalRepository>();
builder.Services.AddScoped<IIC_ReceiptRepository, IC_ReceiptRepository>();
builder.Services.AddScoped<ICallMarkedOnlineRepository, CallMarkedOnlineRepository>();

#endregion
builder.Services.AddScoped<ICityMaster,CityMaster>();
builder.Services.AddScoped<I_IC_Bookset_Form,IC_Bookset_Form>();
builder.Services.AddScoped<IVendorCluster,VendorCluster>();
builder.Services.AddScoped<IHologramSearchForm,HologramSearchForm>();
builder.Services.AddScoped<I_IE_MaximumCallLimitForm,IE_MaximumCallLimitForm>();
builder.Services.AddScoped<IMasterItemsPLForm,MasterItemsPLForm>();
builder.Services.AddScoped<ICentralRejectionStatusRepository,CentralRejectionStatusRepository>();
builder.Services.AddScoped<ICheckPostingFormRepository,CheckPostingFormRepository>();
builder.Services.AddScoped<ISearchPaymentsRepository,SearchPaymentRepository>();
builder.Services.AddScoped<IEFTEntryRepository,EFTEntryRepository>();
builder.Services.AddScoped<IInterUnit_TransferRepository,InterUnit_TransferRepository>();



#region IE Report
builder.Services.AddScoped<IIE_PerfomanceRepository, IE_PerformanceRepository>();
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
builder.Services.AddScoped<ICityMaster, CityMaster>();
builder.Services.AddScoped<I_IC_Bookset_Form, IC_Bookset_Form>();
builder.Services.AddScoped<IVendorCluster, VendorCluster>();
builder.Services.AddScoped<IHologramSearchForm, HologramSearchForm>();
builder.Services.AddScoped<I_IE_MaximumCallLimitForm, IE_MaximumCallLimitForm>();
builder.Services.AddScoped<IMasterItemsPLForm, MasterItemsPLForm>();
builder.Services.AddScoped<IClientEntryForm, ClientEntryForm>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();
//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllerRoute(
//      name: "areas",
//      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
//    );
//});
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
