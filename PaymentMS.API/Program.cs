using GetnetProvider.Models;
using GetnetProvider.Services;
using Microsoft.EntityFrameworkCore;
using PaymentMS.API.Services;
using PaymentMS.Domain.Contracts.Services;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;

// Add services to the container.
var connectionString = configuration.GetConnectionString("Development");
services.AddDbContext<global::PaymentMS.Data.AppDbContext>((Microsoft.EntityFrameworkCore.DbContextOptionsBuilder options)
    => options.UseSqlite(connectionString));

//builder.Services.AddDatabaseDeveloperPageExceptionFilter();
services.AddControllers();
//services.AddControllers().AddNewtonsoftJson(options =>
//    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
//);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddTransient<IGetnetAuthenticationService, GetnetAuthenticationService>();
services.AddTransient<IGetnetCustomerService, GetnetCustomerService>();

services.AddTransient<ICustomerService, CustomerService>();

services.Configure<GetnetSettings>(settings =>
{
    settings.SellerId = configuration.GetSection("PaymentProviders").GetSection("Getnet")["SellerId"];
    settings.ClientId = configuration.GetSection("PaymentProviders").GetSection("Getnet")["ClientId"];
    settings.ClientSecret = configuration.GetSection("PaymentProviders").GetSection("Getnet")["ClientSecret"];
    settings.ApiUrl = configuration.GetSection("PaymentProviders").GetSection("Getnet")["ApiUrl"];
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
