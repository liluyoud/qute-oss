﻿using Quartz;
using Qute.Rfb.Api.Services;

namespace Qute.Rfb.Api.Jobs;

public class DownloadBasic : IJob
{
    private readonly ILogger<DownloadBasic> _logger;
    private readonly RfbServices _rfb;

    public DownloadBasic(ILogger<DownloadBasic> logger, RfbServices rfb)
    {
        _logger = logger;
        _rfb = rfb;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation($"Iniciando Worker em {DateTimeOffset.Now}");
        await _rfb.DownloadBasicFiles();
        _logger.LogInformation($"Finalizando Worker em {DateTimeOffset.Now}");
    }
}
