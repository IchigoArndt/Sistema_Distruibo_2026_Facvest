using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using SD_Api_Extensions;
using SD_Api_Extensions.Settings;
using SD_Server.Infra.Data.Context;
using Serilog;
using Serilog.Events;
using SimpleInjector;
using SimpleInjector.Lifestyles;

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
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbServices(builder.Configuration); //Registra o DbContext e os repositórios e rodar o migrations;

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

app.MapScalarApiReference(opt => opt
    .WithTitle("SD_Server_Api")
    .WithTheme(ScalarTheme.DeepSpace)
    .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient));

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
