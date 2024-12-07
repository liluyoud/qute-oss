using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Qute.Rfb.Api.Contexts;
using Qute.Rfb.Api.Helpers;
using Qute.Rfb.Shared.Entities;
using System.IO.Compression;
using System.Text;

namespace Qute.Rfb.Api.Services;

public class RfbServices
{
    private readonly ILogger<RfbServices> _logger;
    private readonly IHttpClientFactory _http;
    private readonly IHostEnvironment _env;
    private readonly RfbContext _context;

    public RfbServices(ILogger<RfbServices> logger, IHttpClientFactory http, IHostEnvironment env, RfbContext context)
    {
        _logger = logger;
        _http = http;
        _env = env;
        _context = context;
    }

    public void MigrateEmpresas()
    {
        var empresas = _context.Empresas.Count();
        if (empresas == 0)
        {
            var csvDir = Path.Combine(_env.ContentRootPath, "downloads", "csv");
            var files = Directory.GetFiles(csvDir, "*.EMPRECSV");
            var i = 0;
            foreach (var file in files)
            {
                using (var connection = new NpgsqlConnection(_context.Database.GetConnectionString()))
                {
                    connection.Open();

                    // criando uma tabela temporária
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = @"
                            CREATE TEMP TABLE temp_rfb_empresas (
                                id integer,
                                nome character varying(255),
                                natureza_juridica_id integer,
                                qualificacao_id integer,
                                capital_social numeric,
                                porte integer,
                                ente_federativo character varying(255)
                            );
                        ";
                        command.ExecuteNonQuery();
                    }

                    // copiando os dados para a tabela temporária
                    using (var transaction = connection.BeginTransaction())
                    {
                        using (var writer = connection.BeginBinaryImport(
                            "COPY temp_rfb_empresas (id, nome, natureza_juridica_id, qualificacao_id, capital_social, porte, ente_federativo) FROM STDIN (FORMAT BINARY)"))
                        {
                            using (var reader = new StreamReader(file, Encoding.Latin1))
                            {
                                while (!reader.EndOfStream)
                                {
                                    var line = reader.ReadLine();
                                    var values = line?.Split(';');
                                    if (values != null && values.Length == 7)
                                    {
                                        writer.StartRow();
                                        //_logger.LogInformation($"Empresa: {values[0]}");
                                        writer.Write(values[0].GetInteger());
                                        writer.Write(values[1].GetString());
                                        writer.Write(values[2].GetIntegerOrNull());
                                        writer.Write(values[3].GetIntegerOrNull());
                                        writer.Write(values[4].GetDecimalOrNull());
                                        writer.Write(values[5].GetIntegerOrNull());
                                        writer.Write(values[6].GetStringOrNull());
                                        if (++i % 100000 == 0)
                                            _logger.LogInformation($"{i} empresas inseridas");
                                    }
                                }
                            }
                            writer.Complete();
                        }
                        transaction.Commit();
                    }

                    // inserindo os dados na tabela final
                    using (var command = connection.CreateCommand())
                    {
                        _logger.LogInformation($"Copiando empresas de TEMP");
                        
                        command.CommandTimeout = 1800; // 30 minutos
                        command.CommandText = @"
                            INSERT INTO rfb_empresas (id, nome, natureza_juridica_id, qualificacao_id, capital_social, porte, ente_federativo)
                            SELECT id, nome, natureza_juridica_id, qualificacao_id, capital_social, porte, ente_federativo
                            FROM temp_rfb_empresas
                            ON CONFLICT (id) DO NOTHING;
                        ";
                        command.ExecuteNonQuery();
                    }

                }
            }
        }
    }

    public void MigrateSimples()
    {
        var empresas = _context.Empresas.Count();
        if (empresas == 0)
        {
            var csvDir = Path.Combine(_env.ContentRootPath, "downloads", "csv");
            var files = Directory.GetFiles(csvDir, "*SIMPLES*");
            var i = 0;
            foreach (var file in files)
            {
                using (var connection = new NpgsqlConnection(_context.Database.GetConnectionString()))
                {
                    connection.Open();

                    // copiando os dados para a tabela temporária
                    using (var transaction = connection.BeginTransaction())
                    {
                        using (var writer = connection.BeginBinaryImport(
                            "COPY rfb_simples (id, simples_opcao, simples_inclusao, simples_exclusao, mei_opcao, mei_inclusao, mei_exclusao) FROM STDIN (FORMAT BINARY)"))
                        {
                            using (var reader = new StreamReader(file, Encoding.Latin1))
                            {
                                while (!reader.EndOfStream)
                                {
                                    var line = reader.ReadLine();
                                    var values = line?.Split(';');
                                    if (values != null)
                                    {
                                        writer.StartRow();
                                        //_logger.LogInformation($"Empresa: {values[0]}");
                                        writer.Write(values[0].GetInteger());
                                        writer.Write(values[1].GetBooleanOrNull());
                                        writer.Write(values[2].GetDateOnlyOrNull());
                                        writer.Write(values[3].GetDateOnlyOrNull());
                                        writer.Write(values[4].GetBooleanOrNull());
                                        writer.Write(values[5].GetDateOnlyOrNull());
                                        writer.Write(values[6].GetDateOnlyOrNull());
                                        if (++i % 100000 == 0)
                                            _logger.LogInformation($"{i} simples inseridos");
                                    }
                                }
                            }
                            writer.Complete();
                        }
                        transaction.Commit();
                    }
                }
            }
        }
    }

    public async Task MigrateCnaes()
    {
        var cnaes = await _context.Cnaes.ToListAsync();
        if (cnaes.Count == 0)
        {
            var csvDir = Path.Combine(_env.ContentRootPath, "downloads", "csv");
            var file = Directory.GetFiles(csvDir, "*CNAE*").FirstOrDefault();

            if (file != null)
            {
                var list = new List<Cnae>();
                using (var reader = new StreamReader(file, Encoding.Latin1))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line?.Split(';');
                        if (values != null && values.Length == 2)
                        {
                            var item = new Cnae
                            {
                                Id = values[0].GetInteger(),
                                Descricao = values[1].GetString()
                            };
                            list.Add(item);
                        }
                    }
                }
                await _context.Cnaes.AddRangeAsync(list);
                await _context.SaveChangesAsync();
            }
        }
    }

    public async Task MigrateMotivos()
    {
        var motivos = await _context.Motivos.ToListAsync();
        if (motivos.Count == 0)
        {
            var csvDir = Path.Combine(_env.ContentRootPath, "downloads", "csv");
            var file = Directory.GetFiles(csvDir, "*.MOTICSV").FirstOrDefault();

            if (file != null)
            {
                var list = new List<Motivo>();
                using (var reader = new StreamReader(file, Encoding.Latin1))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line?.Split(';');
                        if (values != null && values.Length == 2)
                        {
                            var item = new Motivo
                            {
                                Id = values[0].GetInteger(),
                                Descricao = values[1].GetString()
                            };
                            list.Add(item);
                        }
                    }
                }
                await _context.Motivos.AddRangeAsync(list);
                await _context.SaveChangesAsync();
            }

        }
    }

    public async Task MigrateMunicipios()
    {
        var municipios = await _context.Municipios.ToListAsync();
        if (municipios.Count == 0)
        {
            var csvDir = Path.Combine(_env.ContentRootPath, "downloads", "csv");
            var file = Directory.GetFiles(csvDir, "*.MUNICCSV").FirstOrDefault();

            if (file != null)
            {
                var list = new List<Municipio>();
                using (var reader = new StreamReader(file, Encoding.Latin1))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line?.Split(';');
                        if (values != null && values.Length == 2)
                        {
                            var item = new Municipio
                            {
                                Id = values[0].GetInteger(),
                                Nome = values[1].GetString()
                            };
                            list.Add(item);
                        }
                    }
                }
                await _context.Municipios.AddRangeAsync(list);
                await _context.SaveChangesAsync();
            }
        }
    }

    public async Task MigrateNaturezas()
    {
        var naturezasJuridicas = await _context.NaturezasJuridicas.ToListAsync();
        if (naturezasJuridicas.Count == 0)
        {
            var csvDir = Path.Combine(_env.ContentRootPath, "downloads", "csv");
            var file = Directory.GetFiles(csvDir, "*.NATJUCSV").FirstOrDefault();

            if (file != null)
            {
                var list = new List<NaturezaJuridica>();
                using (var reader = new StreamReader(file, Encoding.Latin1))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line?.Split(';');
                        if (values != null && values.Length == 2)
                        {
                            var item = new NaturezaJuridica
                            {
                                Id = values[0].GetInteger(),
                                Descricao = values[1].GetString()
                            };
                            list.Add(item);
                        }
                    }
                }
                await _context.NaturezasJuridicas.AddRangeAsync(list);
                await _context.SaveChangesAsync();
            }
        }
    }

    public async Task MigratePaises()
    {
        var paises = await _context.Paises.ToListAsync();
        if (paises.Count == 0)
        {
            var csvDir = Path.Combine(_env.ContentRootPath, "downloads", "csv");
            var file = Directory.GetFiles(csvDir, "*.PAISCSV").FirstOrDefault();

            if (file != null)
            {
                var list = new List<Pais>();
                using (var reader = new StreamReader(file, Encoding.Latin1))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line?.Split(';');
                        if (values != null && values.Length == 2)
                        {
                            var item = new Pais
                            {
                                Id = values[0].GetInteger(),
                                Nome = values[1].GetString()
                            };
                            list.Add(item);
                        }
                    }
                }
                await _context.Paises.AddRangeAsync(list);
                await _context.SaveChangesAsync();
            }
        }
    }

    public async Task MigrateQualificacoes()
    {
        var qualificacoes = await _context.Qualificacoes.ToListAsync();
        if (qualificacoes.Count == 0)
        {
            var csvDir = Path.Combine(_env.ContentRootPath, "downloads", "csv");
            var file = Directory.GetFiles(csvDir, "*.QUALSCSV").FirstOrDefault();

            if (file != null)
            {
                var list = new List<Qualificacao>();
                using (var reader = new StreamReader(file, Encoding.Latin1))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line?.Split(';');
                        if (values != null && values.Length == 2)
                        {
                            var item = new Qualificacao
                            {
                                Id = values[0].GetInteger(),
                                Descricao = values[1].GetString()
                            };
                            list.Add(item);
                        }
                    }
                }
                await _context.Qualificacoes.AddRangeAsync(list);
                await _context.SaveChangesAsync();
            }
        }
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

    public async Task DownloadEmpresaFiles()
    {
        var fileNames = await GetFileNames();
        fileNames = fileNames.Where(f => f.Contains("Empresas") || !f.Contains("Simples")).ToList();
        foreach (var fileName in fileNames)
        {
            await DownloadFile(fileName);
        }
    }

    public async Task DownloadEstabelecimentosFiles()
    {
        var fileNames = await GetFileNames();
        fileNames = fileNames.Where(f => f.Contains("Estabelecimentos")).ToList();
        foreach (var fileName in fileNames)
        {
            await DownloadFile(fileName);
        }
    }

    public async Task DownloadSociosFiles()
    {
        var fileNames = await GetFileNames();
        fileNames = fileNames.Where(f => f.Contains("Socios")).ToList();
        foreach (var fileName in fileNames)
        {
            await DownloadFile(fileName);
        }
    }

    public void ExtractFiles()
    {
        string appDir = _env.ContentRootPath;
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
            _logger.LogInformation($"Processing {file}.");
            string csvFile = Path.Combine(csvDir, Path.GetFileNameWithoutExtension(file));
            ZipFile.ExtractToDirectory(file, csvFile);
            _logger.LogInformation($"{file} unziped.");
        }
    }

    public void MoveFiles()
    {
        string appDir = Path.Combine(_env.ContentRootPath, "downloads", "csv");
        if (Directory.Exists(appDir))
        {
            var subdirectories = Directory.GetDirectories(appDir);
            foreach (var subdirectory in subdirectories)
            {
                var files = Directory.GetFiles(subdirectory);
                foreach (var file in files)
                {
                    var destFile = Path.Combine(appDir, Path.GetFileName(file));
                    _logger.LogInformation($"Moving {destFile}");
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
