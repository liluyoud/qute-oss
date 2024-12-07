using Quartz;
using Qute.Rfb.Api.Services;

namespace Qute.Rfb.Api.Jobs;

public class DownloadSocio : IJob
{
    private readonly ILogger<DownloadSocio> _logger;
    private readonly RfbServices _rfb;

    public DownloadSocio(ILogger<DownloadSocio> logger, RfbServices rfb)
    {
        _logger = logger;
        _rfb = rfb;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation($"Iniciando Download Socio em {DateTimeOffset.Now}");
        await _rfb.DownloadSociosFiles();
        _logger.LogInformation($"Finalizando Download Socio em {DateTimeOffset.Now}");
    }
}
