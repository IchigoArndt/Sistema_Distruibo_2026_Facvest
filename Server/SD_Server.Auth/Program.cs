using Scalar.AspNetCore;
using SD_Api_Extensions;
using SD_Api_Extensions.Settings;
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
        configuration.WriteTo.MongoDB(mongo, "LogsAuth");
});

//builder.Services.AddDbServices(builder.Configuration);
//builder.Services.AddStaticFiles();

builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();

// Criação do container
var container = new Container();

container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

//container.Register<IFileConverter, FileConverter>(Lifestyle.Transient);

// Registrar serviços padrão
builder.Services.AddDefaultServices<Program>(container);

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      builder =>
                      {
                          builder.WithOrigins("*");
                          builder.AllowAnyMethod();
                          builder.AllowAnyHeader();
                      });
});

var app = builder.Build();

app.MapOpenApi();

// Configurar Scalar
app.MapScalarApiReference(opt => opt
        .WithTitle("SD_Server_Auth")
        .WithTheme(ScalarTheme.DeepSpace)
        .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient)
    );

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Configurar CORS (antes de endpoints)
app.UseCors(MyAllowSpecificOrigins);

//app.UseStaticFiles();

app.Run();
