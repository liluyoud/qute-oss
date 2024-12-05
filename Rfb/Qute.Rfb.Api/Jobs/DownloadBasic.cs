using DotNetEnv;
using Quartz;
using Qute.Rfb.Api.Services;

namespace Qute.Rfb.Api.Jobs;

public class DownloadBasic : IJob
{
    private readonly ILogger<DownloadBasic> _logger;
    private readonly IHttpClientFactory _http;
    private readonly IHostEnvironment _env;
    private readonly RfbServices _rfb;

    public DownloadBasic(ILogger<DownloadBasic> logger, IHttpClientFactory http, IHostEnvironment env, RfbServices rfb)
    {
        _logger = logger;
        _http = http;
        _env = env;
        _rfb = rfb;
    }

    public Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation($"Worker executando em: {DateTimeOffset.Now}");

        var client = _http.CreateClient();

        string appDir = _env.ContentRootPath;
        string zipDir = Path.Combine(appDir, "downloads", "zip");
        if (!Directory.Exists(zipDir))
            Directory.CreateDirectory(zipDir);



        return Task.CompletedTask;
    }
}
