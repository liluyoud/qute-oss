using Quartz;
using Qute.Rfb.Api.Services;

namespace Qute.Rfb.Api.Jobs;

public class ProcessFiles : IJob
{
    private readonly ILogger<ProcessFiles> _logger;
    private readonly RfbServices _rfb;

    public ProcessFiles(ILogger<ProcessFiles> logger, RfbServices rfb)
    {
        _logger = logger;
        _rfb = rfb;
    }

    public Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation($"Iniciando Worker em {DateTimeOffset.Now}");
        _rfb.ExtractFiles();
        _rfb.MoveFiles();
        _logger.LogInformation($"Finalizando Worker em {DateTimeOffset.Now}");

        return Task.CompletedTask;
    }
}
