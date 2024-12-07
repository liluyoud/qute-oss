using Quartz;
using Qute.Rfb.Api.Services;

namespace Qute.Rfb.Api.Jobs;

public class DownloadBasico : IJob
{
    private readonly ILogger<DownloadBasico> _logger;
    private readonly RfbServices _rfb;

    public DownloadBasico(ILogger<DownloadBasico> logger, RfbServices rfb)
    {
        _logger = logger;
        _rfb = rfb;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation($"Iniciando Download Basic em {DateTimeOffset.Now}");
        await _rfb.DownloadBasicFiles();
        _logger.LogInformation($"Finalizando Download Basic em {DateTimeOffset.Now}");
    }
}
