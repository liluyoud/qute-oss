using Quartz;
using Qute.Rfb.Api.Services;

namespace Qute.Rfb.Api.Jobs;

public class MigrateSocio : IJob
{
    private readonly ILogger<MigrateSocio> _logger;
    private readonly RfbServices _rfb;

    public MigrateSocio(ILogger<MigrateSocio> logger, RfbServices rfb)
    {
        _logger = logger;
        _rfb = rfb;
    }

    public Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation($"Iniciando Migrate Socios em {DateTimeOffset.Now}");
        _rfb.MigrateSocios();
        _logger.LogInformation($"Finalizando Migrate Socios em {DateTimeOffset.Now}");

        return Task.CompletedTask;
    }
}
