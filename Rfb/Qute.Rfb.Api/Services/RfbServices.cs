using DotNetEnv;
using HtmlAgilityPack;
using System;

namespace Qute.Rfb.Api.Services;

public class RfbServices
{
    private readonly ILogger<RfbServices> _logger;
    private readonly IHttpClientFactory _http;
    private readonly IHostEnvironment _env;

    public RfbServices(ILogger<RfbServices> logger, IHttpClientFactory http, IHostEnvironment env)
    {
        _logger = logger;
        _http = http;
        _env = env;
    }

    public async Task<List<string>> GetFileNames()
    {
        var date = DateTime.Now.AddMonths(-1);
        var dateStr = date.ToString("yyyy-MM");
        var rfbUrl = Environment.GetEnvironmentVariable("RFB_URL") ?? "https://arquivos.receitafederal.gov.br/cnpj/dados_abertos_cnpj";
        string baseUrl = $"{rfbUrl}/{dateStr}";
        var fileNames = new List<string>();
        try
        {
            using var client = new HttpClient();
            var htmlContent = await client.GetStringAsync(baseUrl);

            // Carregar o HTML na estrutura do HtmlAgilityPack
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlContent);

            // Selecionar todos os elementos <a> com um atributo href
            var links = doc.DocumentNode.SelectNodes("//a[@href]");
            if (links != null)
            {
                // Filtrar os links que possuem nomes de arquivos e adicioná-los à lista
                fileNames = links
                    .Where(link => link.Attributes["href"].Value.Contains(".zip")) // Filtrar arquivos .zip
                    .Select(link => link.Attributes["href"].Value) // Pegar o valor do href
                    .ToList();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao obter os arquivos da RFB: {ex.Message}");
        }
        return fileNames;
    }

    public async Task DownloadBasicFiles()
    {
        var fileNames = await GetFileNames();
        fileNames = fileNames.Where(f => !f.Contains("Empresas") && !f.Contains("Estabelecimentos") && !f.Contains("Simples") && !f.Contains("Socios")).ToList();
        foreach (var fileName in fileNames)
        {
            await DownloadFile(fileName);
        }
    }

    private async Task DownloadFile(string fileName)
    {
        // diretório do zip
        string appDir = _env.ContentRootPath;
        string zipDir = Path.Combine(appDir, "downloads", "zip");
        if (!Directory.Exists(zipDir))
            Directory.CreateDirectory(zipDir);

        // url base do download
        var date = DateTime.Now.AddMonths(-1);
        var dateStr = date.ToString("yyyy-MM");
        var rfbUrl = Environment.GetEnvironmentVariable("RFB_URL") ?? "https://arquivos.receitafederal.gov.br/cnpj/dados_abertos_cnpj";
        string baseUrl = $"{rfbUrl}/{dateStr}";

        // arquivo origem e destino
        var fileFrom = $"{baseUrl}/{fileName}";
        var fileTo = Path.Combine(zipDir, fileName);

        using (var client = _http.CreateClient())
        {
            _logger.LogInformation($"Iniciando o download de {fileFrom}.");

            // Enviar o request e obter o tamanho do arquivo
            HttpResponseMessage response = await client.GetAsync(fileFrom, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            long? totalBytes = response.Content.Headers.ContentLength;

            // Abrir streams para leitura e gravação
            using (Stream contentStream = await response.Content.ReadAsStreamAsync(),
                          fileStream = new FileStream(fileTo, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
            {
                byte[] buffer = new byte[10485760]; // 10MB
                long totalBytesRead = 0;
                int bytesRead;
                double progressPercentage = 0;

                // Ler o conteúdo do arquivo em partes
                while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    // Escrever no arquivo local
                    await fileStream.WriteAsync(buffer, 0, bytesRead);

                    // Atualizar progresso
                    totalBytesRead += bytesRead;
                    if (totalBytes.HasValue)
                    {
                        progressPercentage = (double)totalBytesRead / totalBytes.Value * 100;
                        _logger.LogInformation($"Downloading {totalBytesRead / (1024 * 1024)} MB of {totalBytes / (1024 * 1024)} MB ({progressPercentage:F2}%)");
                    }
                    else
                    {
                        _logger.LogInformation($"Downloaded {totalBytesRead / (1024 * 1024)} MB");
                    }
                }

                _logger.LogInformation($"{fileFrom} downloaded.");
            }
        }
    }
}
