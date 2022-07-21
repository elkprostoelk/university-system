using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using UniversitySystem.Api;

var builder = WebApplication.CreateBuilder(args);

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
var appConfiguration = new ConfigurationBuilder()
    .AddJsonFile($"appsettings.{environment}.json", optional: false)
    .Build();

var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.MSSqlServer(
        connectionString: appConfiguration.GetConnectionString("LoggerConnection"),
        sinkOptions: new MSSqlServerSinkOptions
        {
            TableName = "Logger",
            AutoCreateSqlTable = true,
            BatchPeriod = TimeSpan.FromSeconds(5)
        },
        restrictedToMinimumLevel: LogEventLevel.Error)
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.ConfigureServices(appConfiguration);
builder.Services.AddRsaAuthentication(appConfiguration);

var app = builder.Build();

app.ConfigureApp(app.Environment);
await app.DatabaseEnsureCreated();

await app.RunAsync();