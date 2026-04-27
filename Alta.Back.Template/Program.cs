using Alta.Back.Lib.Middlewares;
using Alta.Back.Lib.Helpers;
using Alta.Back.Lib.Enums;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// 1. Configurar Ambiente
var envName = builder.Environment.EnvironmentName;
var ambienteAtual = AmbienteHelper.ObterAmbienteAtual(envName);

// 2. Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Serilog Config (Basic)
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .WriteTo.Console());

var app = builder.Build();

// 3. Pipeline
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

// Swagger: APENAS PARA AMBIENTES != DE PRODUCAO
if (ambienteAtual != AmbienteAplicacaoEnum.Producao)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Health Check via Controller (/health) or minimal map. 
// User demanded Rota de health check no swagger. Controller handles it.
// We can also map standard endpoint if needed, but redundant.

app.Run();
