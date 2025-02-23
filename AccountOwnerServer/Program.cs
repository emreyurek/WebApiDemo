using AccountOwnerServer.Extensions;
using AutoMapper;
using Microsoft.AspNetCore.HttpOverrides;
using NLog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.ConfigureRepositoryWrapper();

builder.Services.ConfigureMsSqlContext(builder.Configuration);

LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
builder.Services.ConfigureLoggerService();

builder.Services.ConfigureCors();

builder.Services.ConfigureIISIntegration();

builder.Services.AddControllers();

var app = builder.Build();

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
