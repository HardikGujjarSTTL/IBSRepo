using IBSAPI.DataAccess;
using IBSAPI.Interfaces;
using IBSAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ModelContext>(options => options.UseOracle(connectionString));

IMvcBuilder mvcBuilder = builder.Services.AddControllersWithViews();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITokenServices, TokenServices>();
builder.Services.AddScoped<ISendMailRepository, SendMailRepository>();
builder.Services.AddScoped<ICallRepository, CallRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IVendorRepository, VendorRepository>();
builder.Services.AddScoped<IInspectionRepository, InspectionRepository>();
builder.Services.AddScoped<IDashBoardRepository, DashBoardRepository>();

builder.Services.AddScoped<ICRISRepository, CRISRepository>();
builder.Services.AddScoped<IInvoices_SAPRepository, Invoices_SAPRepository>();

builder.Services.AddScoped<IChatRepository, ChatRepository>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "IBS2API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "IBS2API v1"));
    app.UseSwaggerUI();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("../swagger/v1/swagger.json", "IBS2API v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
