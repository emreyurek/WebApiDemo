using AccountOwnerServer.Extensions;
using AutoMapper;
using Contracts;
using Microsoft.AspNetCore.HttpOverrides;
using NLog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureActionFilters();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.ConfigureRepositoryWrapper();

builder.Services.ConfigureMsSqlContext(builder.Configuration);

LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
builder.Services.ConfigureLoggerService();

builder.Services.ConfigureCors();

builder.Services.ConfigureIISIntegration();

builder.Services.AddControllers(config =>
{
    config.RespectBrowserAcceptHeader = true;
    config.ReturnHttpNotAcceptable = true;
})
.AddNewtonsoftJson()  // default - json format 
.AddXmlDataContractSerializerFormatters();

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILoggerManager>();
app.ConfigureExceptionHandler(logger);

// Configure the HTTP request pipeline. Middleware
if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage(); // Shows detailed error messages
else
    app.UseHsts();  // Forces HTTP requests to HTTPS (making it more secure)

app.UseHttpsRedirection();

app.UseStaticFiles();  // It allows to serve static files (CSS, JS, images, etc.) from the wwwroot folder

app.UseForwardedHeaders(new ForwardedHeadersOptions // For Linux deployment
{
    ForwardedHeaders = ForwardedHeaders.All
});

app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
