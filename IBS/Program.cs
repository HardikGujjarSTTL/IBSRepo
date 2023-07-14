using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Repositories;
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
builder.Services.AddScoped<IContractRepository, ContractRepository>();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
