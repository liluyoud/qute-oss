using Quartz;
using Qute.Rfb.Api.Services;

namespace Qute.Rfb.Api.Jobs;

public class MigrateSimples : IJob
{
    private readonly ILogger<MigrateSimples> _logger;
    private readonly RfbServices _rfb;

    public MigrateSimples(ILogger<MigrateSimples> logger, RfbServices rfb)
    {
        _logger = logger;
        _rfb = rfb;
    }

    public Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation($"Iniciando Worker em {DateTimeOffset.Now}");
        _rfb.MigrateSimples();
        _logger.LogInformation($"Finalizando Worker em {DateTimeOffset.Now}");

        return Task.CompletedTask;
    }
}
