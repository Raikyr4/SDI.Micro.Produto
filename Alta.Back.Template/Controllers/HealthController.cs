using Alta.Back.Lib.Models;
using Alta.Back.Lib.Constants;
using Alta.Back.Template.Models.Dto.Output;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Net;

namespace Alta.Back.Template.Controllers;

[ApiController]
[Route("saude")]
public class HealthController : ControllerBase
{
    private readonly HealthCheckService _healthCheckService;

    public HealthController(HealthCheckService healthCheckService)
    {
        _healthCheckService = healthCheckService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(RetornoPadrao<HealthCheckResponse>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(RetornoPadrao<HealthCheckResponse>), (int)HttpStatusCode.ServiceUnavailable)]
    public async Task<IActionResult> Get()
    {
        var report = await _healthCheckService.CheckHealthAsync();
        
        var responseDto = new HealthCheckResponse
        {
            Status = report.Status.ToString(),
            TotalDuration = report.TotalDuration.ToString(),
            Entries = report.Entries.ToDictionary(
                e => e.Key,
                e => new HealthCheckEntryResponse
                {
                    Status = e.Value.Status.ToString(),
                    Description = e.Value.Description ?? string.Empty,
                    Duration = e.Value.Duration.ToString(),
                    Data = e.Value.Data
                })
        };

        var retorno = new RetornoPadrao<HealthCheckResponse>
        {
            Codigo = report.Status == HealthStatus.Healthy ? ConstantesCodigoSucessoPadrao.OperacaoRealizadaComSucesso : ConstantesCodigoRetornoPadrao.ErroInterno,
            Mensagem = report.Status == HealthStatus.Healthy ? "Sistema operando normalmente." : "Sistema com instabilidade.",
            StatusHttp = report.Status == HealthStatus.Healthy ? (int)HttpStatusCode.OK : (int)HttpStatusCode.ServiceUnavailable,
            Resultado = responseDto
        };

        return report.Status == HealthStatus.Healthy ? Ok(retorno) : StatusCode((int)HttpStatusCode.ServiceUnavailable, retorno);
    }
}
