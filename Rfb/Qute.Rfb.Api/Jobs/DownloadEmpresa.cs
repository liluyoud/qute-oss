using Quartz;
using Qute.Rfb.Api.Services;

namespace Qute.Rfb.Api.Jobs;

public class DownloadEmpresa : IJob
{
    private readonly ILogger<DownloadEmpresa> _logger;
    private readonly RfbServices _rfb;

    public DownloadEmpresa(ILogger<DownloadEmpresa> logger, RfbServices rfb)
    {
        _logger = logger;
        _rfb = rfb;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation($"Iniciando Download Empresa em {DateTimeOffset.Now}");
        await _rfb.DownloadBasicFiles();
        _logger.LogInformation($"Finalizando Download Empresa em {DateTimeOffset.Now}");
    }
}
