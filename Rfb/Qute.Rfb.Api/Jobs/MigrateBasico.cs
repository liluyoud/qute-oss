using Quartz;
using Qute.Rfb.Api.Services;

namespace Qute.Rfb.Api.Jobs;

public class MigrateBasico : IJob
{
    private readonly ILogger<MigrateBasico> _logger;
    private readonly RfbServices _rfb;

    public MigrateBasico(ILogger<MigrateBasico> logger, RfbServices rfb)
    {
        _logger = logger;
        _rfb = rfb;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation($"Iniciando Migrate Basico em {DateTimeOffset.Now}");
        await _rfb.MigrateCnaes();
        await _rfb.MigrateMotivos();
        await _rfb.MigrateMunicipios();
        await _rfb.MigrateNaturezas();
        await _rfb.MigratePaises();
        await _rfb.MigrateQualificacoes();
        _logger.LogInformation($"Finalizando Migrate Basico em {DateTimeOffset.Now}");
    }
}
