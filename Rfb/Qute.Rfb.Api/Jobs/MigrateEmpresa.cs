using Quartz;
using Qute.Rfb.Api.Services;

namespace Qute.Rfb.Api.Jobs;

public class MigrateEmpresa : IJob
{
    private readonly ILogger<MigrateEmpresa> _logger;
    private readonly RfbServices _rfb;

    public MigrateEmpresa(ILogger<MigrateEmpresa> logger, RfbServices rfb)
    {
        _logger = logger;
        _rfb = rfb;
    }

    public Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation($"Iniciando Migrate Empresa em {DateTimeOffset.Now}");
        _rfb.MigrateEmpresas();
        _logger.LogInformation($"Finalizando Migrate Empresa em {DateTimeOffset.Now}");

        return Task.CompletedTask;
    }
}
