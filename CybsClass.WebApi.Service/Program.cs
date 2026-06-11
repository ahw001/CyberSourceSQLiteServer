using System;
using System.IO;
using System.Text.Json.Serialization;
using CybsClass.Cybersource.Authentication;
using CybsClass.EntityModels; // To use the AddB2cNorthwindContext method.
using CybsClass.WebApi.Service;
using CybsClass.WebApi.Service.ExceptionHandlers;
using CybsClass.WebApi.Service.Services;
using CybsClass.WebApi.Service.Services.CcTransatcionProcessing;
using CybsClass.WebApi.Service.Services.Configs;
using CybsClass.WebApi.Service.Services.TokenProcessing;
using CybsClass.WebApi.Service.Services.Utilities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Serilog;

const string LogRoot = @"C:\inetpub\logs\CybsApi";
const string CorsPolicy = "AppCors";

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("boardingSettings.json", optional: false, reloadOnChange: true);

// ------- Environment diagnostics (console only) -------
var env = builder.Environment;
Console.WriteLine($"[Env] ASPNETCORE_ENVIRONMENT = {env.EnvironmentName}");
Console.WriteLine($"[Env] ContentRootPath = {env.ContentRootPath}");
Console.WriteLine($"[Env] BaseDirectory = {AppContext.BaseDirectory}");

// ------- Decide if logging should be enabled -------
static bool CanWriteDirectory(string path, out string? error)
{
    try
    {
        Directory.CreateDirectory(path);
        var probe = Path.Combine(path, $".probe-{Guid.NewGuid():N}.tmp");
        using (var fs = new FileStream(probe, FileMode.CreateNew, FileAccess.Write, FileShare.Read))
        {
            fs.WriteByte(0x42);
        }
        File.Delete(probe);
        error = null;
        return true;
    }
    catch (Exception ex)
    {
        error = ex.Message;
        return false;
    }
}

var hasInetpubAccess = CanWriteDirectory(LogRoot, out var accessErr);
var loggingEnabled = hasInetpubAccess;

Console.WriteLine(loggingEnabled
    ? $"[Log] Using inetpub log root: {LogRoot}"
    : $"[Log] Logging disabled (cannot write {LogRoot}: {accessErr})");

// ------- Wire Serilog only if writable; else no logging at all -------
if (loggingEnabled)
{
    builder.Host.UseSerilog((ctx, _, lc) =>
    {
        Directory.CreateDirectory(LogRoot);

        // Serilog internal errors -> file (only when writable)
        Serilog.Debugging.SelfLog.Enable(m =>
        {
            try { File.AppendAllText(Path.Combine(LogRoot, "serilog-selflog.txt"), m); } catch { }
        });

        lc.MinimumLevel.Information()
          .Enrich.FromLogContext()
          .WriteTo.File(
              Path.Combine(LogRoot, "app-.log"),
              rollingInterval: RollingInterval.Day,
              retainedFileCountLimit: 30,
              shared: true);
    });
}
else
{
    // Remove all default Microsoft logging providers too (no logging at all)
    builder.Logging.ClearProviders();
}

// ------- CORS -------
var defaultAllowed = new[]
{
    "https://www.testccprocessor.com",
    "https://testccprocessor.com",
    "https://localhost:7173",
    "https://localhost:7133",
    "http://localhost:5173",
    "https://localhost:7290", // dev WASM (http)
    "http://localhost"        // IIS local
};

var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>() ?? defaultAllowed;

// ------- Services -------
builder.Services.Configure<AppSettings>(builder.Configuration);
builder.Services.AddSingleton<ConfigValues>();

builder.Services.AddProblemDetails();

builder.Services.AddScoped<CcAuthService>();
builder.Services.AddScoped<CallNtDecrypt>();

// Bind strongly typed settings (also available via IOptions<AppSettings>)
var appSettings = new AppSettings();
builder.Configuration.Bind(appSettings);

// Console diagnostics (safe details only)
Console.WriteLine($"P12 file = {appSettings.AuthCredentialFile.RestP12JwtCredential}");
Console.WriteLine($"KeyPass length = {appSettings.AuthCredentialFile.KeyPass?.Length ?? 0}");
Console.WriteLine($"IsPortfolioCredential = {appSettings.AuthCredentialFile.IsPortfolioCredential}");
Console.WriteLine($"MerchantID = {appSettings.AuthCredentialFile.MerchantID}");
Console.WriteLine($"KeyId = {appSettings.AuthSecretKey.KeyId}");
Console.WriteLine($"SharedSecret length = {appSettings.AuthSecretKey.SharedSecret?.Length ?? 0}");
Console.WriteLine($"BaseUrlAddress = {appSettings.BaseUrlAddress}");
Console.WriteLine($"BasePosUrlAddress = {appSettings.BasePosUrlAddress}");
Console.WriteLine($"AllowedHosts = {appSettings.AllowedHosts}");
Console.WriteLine($"AllowedOrigins = {string.Join(";", appSettings.Cors.AllowedOrigins ?? new List<string>())}");
Console.WriteLine($"AcceptanceMerchantId = {appSettings.AcceptanceDeviceInfo.AcceptanceMerchantId}");
Console.WriteLine($"AcceptanceSecret length = {appSettings.AcceptanceDeviceInfo.AcceptanceSecret?.Length ?? 0}");
Console.WriteLine($"AcceptanceDeviceSerialNumber = {appSettings.AcceptanceDeviceInfo.AcceptanceDeviceSerialNumber}");

builder.Services.AddAuthorization();
builder.Services.AddSignalR();

builder.Services.AddCustomRateLimiting(builder.Configuration);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext(s)
builder.Services.AddB2cNorthwindContext();

// System.Text.Json defaults
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// Exception handlers
builder.Services.AddExceptionHandler<TimeOutExceptionHandler>();
builder.Services.AddExceptionHandler<DefaultExceptionHandler>();

// CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicy, p =>
        p.WithOrigins(allowedOrigins)
         .SetIsOriginAllowedToAllowWildcardSubdomains()
         .AllowAnyHeader()
         .AllowAnyMethod()
         .AllowCredentials());
});

var app = builder.Build();

// Options snapshot
var settings = app.Services.GetRequiredService<IOptions<AppSettings>>().Value;
Console.WriteLine($"[Cfg] P12={settings.AuthCredentialFile.RestP12JwtCredential}, KeyPassLen={settings.AuthCredentialFile.KeyPass?.Length ?? 0}");

// Initialize Credentials
Credentials.Initialize(
    settings.AuthCredentialFile.RestP12JwtCredential!,
    settings.AuthCredentialFile.IsPortfolioCredential ?? "false",
    settings.AuthCredentialFile.MerchantID ?? "",
    settings.AuthCredentialFile.KeyPass!,
    settings.AuthSecretKey.KeyId!,
    settings.AuthSecretKey.SharedSecret!,
    settings.BaseUrlAddress ?? ""
);

// Initialize BoardingCredentials (JWT boarding auth)
BoardingCredentials.Initialize(
    settings.AuthCredentialFile.RestP12JwtCredential!,
    settings.AuthCredentialFile.IsPortfolioCredential ?? "false",
    settings.AuthCredentialFile.MerchantID ?? "",
    settings.AuthCredentialFile.KeyPass!,
    settings.AuthSecretKey.KeyId!,
    settings.AuthSecretKey.SharedSecret!,
    settings.BaseUrlAddress ?? ""
);

// Initialize MLE credentials (SJC encryption cert + response decryption key)
var mleCfg = new MleSettings();
app.Configuration.GetSection("MleSettings").Bind(mleCfg);
Console.WriteLine($"[MLE] ResponseMleKid = {mleCfg.ResponseMleKid}");
MleCredentials.Initialize(
    mleCfg.SjcCertificatePath ?? "",
    mleCfg.ResponseMleKeyPath ?? "",
    mleCfg.ResponseMleKid ?? "",
    mleCfg.ResponseMleKeyPass,
    mleCfg.LegacyMlePrivateKeyPath,
    mleCfg.LegacyMleKid
);

// Initialize BoardingConfig
var boardingSettings = new BoardingSettings();
app.Configuration.GetSection("BoardingSettings").Bind(boardingSettings);
BoardingConfig.Initialize(
    boardingSettings.BoardingPackageId ?? "",
    boardingSettings.CardProcessingTemplateId ?? "",
    boardingSettings.VirtualTerminalTemplateId ?? "",
    boardingSettings.TokenManagementTemplateId ?? "",
    boardingSettings.PayerAuthenticationTemplateId ?? ""
);

// Initialize URL endpoint method lookup
UrlEndpointConfig.Initialize(Path.Combine(app.Environment.ContentRootPath, "urlEndpoints.json"));

// ------- Middleware -------
if (loggingEnabled)
{
    app.UseSerilogRequestLogging();

    app.Lifetime.ApplicationStarted.Register(() =>
        Serilog.Log.Information("App started. Logging to {Path}", LogRoot));
}

app.UseExceptionHandler(); // uses registered IExceptionHandler implementations
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Optional CORS debug logging — only when logging is enabled
if (loggingEnabled)
{
    app.Use(async (ctx, next) =>
    {
        if (ctx.Request.Path.StartsWithSegments("/api"))
        {
            var log = ctx.RequestServices.GetRequiredService<ILoggerFactory>()
                      .CreateLogger("CorsDebug");
            log.LogInformation("CORS check: Origin={Origin}; Method={Method}; ACRM={ACRM}; ACRH={ACRH}",
                ctx.Request.Headers.Origin.ToString(),
                ctx.Request.Method,
                ctx.Request.Headers.AccessControlRequestMethod.ToString(),
                ctx.Request.Headers.AccessControlRequestHeaders.ToString());
        }
        await next();
    });
}

app.UseCors(CorsPolicy);

// ------- Optional static file serving for local dev (leave commented) -------
/*
if (app.Environment.IsDevelopment())
{
    var wasmPath = @"C:\Users\ahw00\OneDrive\Documents\Development\Publish\CybsWasm\wwwroot";
    app.MapWhen(context =>
        !context.Request.Path.StartsWithSegments("/api") &&
        !context.Request.Path.StartsWithSegments("/swagger") &&
        !context.Request.Path.StartsWithSegments("/hub"),
        subApp =>
        {
            subApp.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(wasmPath),
                ServeUnknownFileTypes = true,
                RequestPath = ""
            });

            subApp.Run(async context =>
            {
                context.Response.ContentType = "text/html";
                await context.Response.SendFileAsync(Path.Combine(wasmPath, "index.html"));
            });
        });
}
*/

// ------- Endpoints -------
app.MapGets(); // Default pageSize: 10.
app.MapPosts();
app.MapPuts();
app.MapDeletes();

app.MapPaymentCardInfoEndpoints();

app.MapGet("/api/test", (ILogger<Program> log) =>
{
    if (loggingEnabled)
    {
        log.LogInformation("Hit /test at {Utc}", DateTimeOffset.UtcNow);
        Serilog.Log.Information("Static Serilog also wrote from /test");
    }
    return Results.Ok(new { ok = true, message = loggingEnabled ? "logged" : "logging disabled" });
});

if (loggingEnabled)
{
    app.MapGet("/api/diag", () =>
    {
        var probe = Path.Combine(LogRoot, "probe.txt");
        File.AppendAllText(probe, $"{DateTimeOffset.UtcNow:O} /api/diag hit{Environment.NewLine}");
        return Results.Ok(new
        {
            LogRoot,
            Expect = Path.Combine(LogRoot, $"app-{DateTime.UtcNow:yyyy-MM-dd}.log"),
            Probe = probe
        });
    });

    app.Lifetime.ApplicationStopped.Register(Serilog.Log.CloseAndFlush);
}

app.MapGet("/exception", () => { throw new Exception(); });
app.MapGet("/timeout", () => { throw new TimeoutException(); });

app.MapGroup("/api/session").GroupSessionState();
app.MapGroup("/api/json").UtilityGroupEndPoints();
app.MapGroup("/api/semiintpos").SemiIntGroupPosEndPoints();
app.MapGroup("/api/cloudpos").GroupCloudPosEndPoints();
app.MapGroup("/api/payouts").GroupPayoutsEndPoints();
app.MapGroup("/api/paypal").GroupPayPalEndpoints();
app.MapGroup("/api/tokens").GroupTokens();
app.MapGroup("/api/unified").GroupUnifiedEndpoints();
app.MapGroup("/api/invoice").GroupInvoiceEndpoints();
app.MapGroup("/api/payerauth").GroupPayerAuthEndpoints();
app.MapAuthTransResponseEndpoints();
app.MapFollowOnTransResponseEndpoints();
app.MapIndividualTransactionEndpoints();
app.MapNetworkTokenInfoEndpoints();
app.MapOrderEndpoints();
app.MapOrderDetailEndpoints();
app.MapB2cCustomerEndpoints();
app.MapCategoryEndpoints();
app.MapSampleInvoiceDetailEndpoints();
app.MapPayerAuthCardSampleDatumEndpoints();
app.MapMerchantSampleDatumEndpoints();
app.MapMerchantBoardingEndpoints();
app.MapElectronicProductEndpoints();
app.MapBoardingDataEndpoints();

app.Run();
