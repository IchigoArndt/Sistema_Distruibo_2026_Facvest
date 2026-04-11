using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using SD_Api_Extensions;
using SD_Api_Extensions.Settings;
using SD_Server.Infra.Data.Context;
using Serilog;
using Serilog.Events;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

builder.Host.UseSerilog((_, _, configuration) =>
{
    configuration
        .MinimumLevel.Information()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        .MinimumLevel.Override("System", LogEventLevel.Warning)
        .Enrich.FromLogContext()
        .WriteTo.Console(
            outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} | {Level:u3} | {SourceContext} | {Message}{NewLine}{Exception}");

    var mongo = Environment.GetEnvironmentVariable("Connection_Mongo");
    if (!string.IsNullOrWhiteSpace(mongo))
        configuration.WriteTo.MongoDB(mongo, "Logs");
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbServices(builder.Configuration);

var jwtSecret = Environment.GetEnvironmentVariable("Secret")
    ?? builder.Configuration["AppSettings:JwtSecret"]
    ?? throw new InvalidOperationException("Secret JWT não configurado. Defina a variável de ambiente 'Secret' ou 'AppSettings:JwtSecret' no appsettings.");

var key = Encoding.UTF8.GetBytes(jwtSecret);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer           = true,
        ValidateAudience         = true,
        ValidateLifetime         = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer              = "MatrixCompetency",
        ValidAudience            = "MatrixCompetency",
        IssuerSigningKey         = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<SD_Server.Api.OpenApi.BearerSecuritySchemeTransformer>();
});

var container = new Container();
container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

builder.Services.AddDefaultServices<Program>(container);

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.AllowAnyOrigin();
            policy.AllowAnyMethod();
            policy.AllowAnyHeader();
        });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SdServerDbContext>();
    db.Database.Migrate();
}

app.MapOpenApi();

app.MapScalarApiReference(opt =>
{
    opt.WithTitle("SD_Server_Api")
       .WithTheme(ScalarTheme.DeepSpace)
       .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);

    opt.Authentication = new Scalar.AspNetCore.ScalarAuthenticationOptions
    {
        PreferredSecurityScheme = "Bearer"
    };
});

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
