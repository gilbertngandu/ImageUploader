using Asp.Versioning;
using ImageUpload.Api.Extensions;
using ImageUpload.Api.Registration;
using ImageUploader.Common.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using System.Net;

const string ApiName = "ImageUploader API";
const string AzureAd = "AzureAd";

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;
var azureConfig = config.GetSection(AzureAd);
var apiScopeUrl = $"api://{azureConfig["ClientId"]}/{azureConfig["Scopes"]}";

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
// Configure logging levels
var loggerFactory = LoggerFactory.Create(logging =>
{
    logging.ClearProviders(); // Clear the default logging providers
    logging.AddConsole();     // Add Console logging provider
    logging.AddDebug();       // Add Debug logging provider
    // Add more logging providers if needed
});
services.AddSingleton(loggerFactory);
services.AddLogging();

services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(azureConfig,
    subscribeToJwtBearerMiddlewareDiagnosticsEvents: true);
services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddAuthorization();
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = ApiName, Version = "v1" });
    // Configure OAuth2 for Azure AD
    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            Implicit = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri($"https://login.microsoftonline.com/{azureConfig["TenantId"]}/oauth2/v2.0/authorize"),
                TokenUrl = new Uri($"https://login.microsoftonline.com/{azureConfig["TenantId"]}/oauth2/v2.0/token"),
                Scopes = new Dictionary<string, string>
                {
                  { apiScopeUrl, "Access the API as a user" }
                }
            }
        }
    });
    // Add the OAuth2 requirement
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme {
                    Reference = new OpenApiReference {
                        Type = ReferenceType.SecurityScheme,
                        Id = "oauth2"
                    },
                    Scheme = "oauth2",
                    Name = "oauth2",
                    In = ParameterLocation.Header
                    },
                    new List <string> ()
                }
            });
}
);

services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1);
    options.ReportApiVersions = true;
}).AddMvc();
services.RegisterDependencies(config);
var app = builder.Build();

//app.UseRateLimit(10);
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", ApiName);
    c.OAuthClientId(azureConfig["ClientId"]);
    c.OAuthClientSecret(azureConfig["ClientSecret"]); // Required for some flows
    c.OAuthUseBasicAuthenticationWithAccessCodeGrant();
});
app.UseExceptionHandler(e =>
{
    e.Run(async context =>
    {
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerPathFeature?.Error;
        context.Response.ContentType = "application/json";
        var errorResponse = new ApiResponse<object>(exception?.Message,
            (HttpStatusCode)context.Response.StatusCode);
        await context.Response.WriteAsJsonAsync(errorResponse);
    });
});
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();