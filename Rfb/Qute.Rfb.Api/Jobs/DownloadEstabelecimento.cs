using Quartz;
using Qute.Rfb.Api.Services;

namespace Qute.Rfb.Api.Jobs;

public class DownloadEstabelecimento : IJob
{
    private readonly ILogger<DownloadEstabelecimento> _logger;
    private readonly RfbServices _rfb;

    public DownloadEstabelecimento(ILogger<DownloadEstabelecimento> logger, RfbServices rfb)
    {
        _logger = logger;
        _rfb = rfb;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation($"Iniciando Download Estabelecimento em {DateTimeOffset.Now}");
        await _rfb.DownloadEstabelecimentosFiles();
        _logger.LogInformation($"Finalizando Download Estabelecimento em {DateTimeOffset.Now}");
    }
}
