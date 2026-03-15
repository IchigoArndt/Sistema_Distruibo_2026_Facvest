using Scalar.AspNetCore;
using SD_Api_Extensions;
using SD_Server.Infra.Configurations;
using SimpleInjector;
using SimpleInjector.Lifestyles;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddDbServices(builder.Configuration);

//builder.Services.AddStaticFiles(); // Para servir arquivos estáticos (ex: imagens)

//builder.Host.UseSerilog();

// Configurações
EnviromentConfiguration.Configure(builder.Configuration);

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
