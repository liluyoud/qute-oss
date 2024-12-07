using Quartz;
using Qute.Rfb.Api.Services;

namespace Qute.Rfb.Api.Jobs;

public class MigrateEstabelecimento : IJob
{
    private readonly ILogger<MigrateEstabelecimento> _logger;
    private readonly RfbServices _rfb;

    public MigrateEstabelecimento(ILogger<MigrateEstabelecimento> logger, RfbServices rfb)
    {
        _logger = logger;
        _rfb = rfb;
    }

    public Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation($"Iniciando Migrate Estabelecimento em {DateTimeOffset.Now}");
        _rfb.MigrateEstabelecimentos();
        _logger.LogInformation($"Finalizando Migrate Estabelecimento em {DateTimeOffset.Now}");

        return Task.CompletedTask;
    }
}
