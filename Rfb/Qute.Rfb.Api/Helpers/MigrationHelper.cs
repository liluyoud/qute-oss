using HtmlAgilityPack;
using Qute.Rfb.Shared.Enums;
using System.IO.Compression;

namespace Qute.Rfb.Api.Helpers;

public static class MigrationHelper
{
    public async static Task<List<string>> GetRfbFileNames(ILogger logger)
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
            logger.LogError($"Erro ao obter os arquivos da RFB: {ex.Message}");
        }

        return fileNames;
    }

    public static async Task DownloadRfbFiles(IHostEnvironment env, ILogger logger, RfbFileType fileType = RfbFileType.Basico)
    {
        // create zip directory
        string appDir = env.ContentRootPath;
        string zipDir = Path.Combine(appDir, "downloads", "zip");
        if (!Directory.Exists(zipDir))
            Directory.CreateDirectory(zipDir);

        var date = DateTime.Now.AddMonths(-1);
        var dateStr = date.ToString("yyyy-MM");
        var rfbUrl = Environment.GetEnvironmentVariable("RFB_URL") ?? "https://arquivos.receitafederal.gov.br/cnpj/dados_abertos_cnpj";
        string baseUrl = $"{rfbUrl}/{dateStr}";

        var fileNames = await GetRfbFileNames(logger);
        if (fileType == RfbFileType.Basico)
            fileNames = fileNames.Where(f => !f.Contains("Empresas") && !f.Contains("Estabelecimentos") && !f.Contains("Simples") && !f.Contains("Socios")).ToList();
        else if (fileType == RfbFileType.Empresas)
            fileNames = fileNames.Where(f => f.Contains("Empresas") || f.Contains("Simples")).ToList();
        else if (fileType == RfbFileType.Estabelecimentos)
            fileNames = fileNames.Where(f => f.Contains("Estabelecimentos")).ToList();
        else if (fileType == RfbFileType.Socios)
            fileNames = fileNames.Where(f => f.Contains("Socios")).ToList();
        foreach (var fileName in fileNames)
        {
            var fileUrl = $"{baseUrl}/{fileName}";
            var filePath = Path.Combine(zipDir, fileName);
            using var httpClient = new HttpClient();
            var fileBytes = await httpClient.GetByteArrayAsync(fileUrl);
            logger.LogInformation($"{fileUrl} downloaded.");
            await File.WriteAllBytesAsync(filePath, fileBytes);
            logger.LogInformation($"{filePath} created.");
        }
    }

    public static void ExtractRfbFiles(IHostEnvironment env, ILogger logger)
    {
        string appDir = env.ContentRootPath;
        string zipDir = Path.Combine(appDir, "downloads", "zip");
        if (!Directory.Exists(zipDir))
            Directory.CreateDirectory(zipDir);
        string csvDir = Path.Combine(appDir, "downloads", "csv");
        if (!Directory.Exists(csvDir))
            Directory.CreateDirectory(csvDir);

        string[] zipFiles = Directory.GetFiles(zipDir, "*.zip");
        // unzip files
        foreach (string file in zipFiles)
        {
            logger.LogInformation($"Processing {file}.");
            string csvFile = Path.Combine(csvDir, Path.GetFileNameWithoutExtension(file));
            ZipFile.ExtractToDirectory(file, csvFile);
            logger.LogInformation($"{file} unziped.");
        }
    }

    public static void MoveFilesToRootFolder(IHostEnvironment env, ILogger logger)
    {
        string appDir = Path.Combine(env.ContentRootPath, "downloads", "csv");
        if (Directory.Exists(appDir))
        {
            var subdirectories = Directory.GetDirectories(appDir);
            foreach (var subdirectory in subdirectories)
            {
                var files = Directory.GetFiles(subdirectory);
                foreach (var file in files)
                {
                    var destFile = Path.Combine(appDir, Path.GetFileName(file));
                    logger.LogInformation($"Moving {destFile}");
                    if (File.Exists(destFile))
                    {
                        File.Delete(destFile);
                    }
                    File.Move(file, destFile);
                }
                Directory.Delete(subdirectory, true);
            }
        }
    }
}
