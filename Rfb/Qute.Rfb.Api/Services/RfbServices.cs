using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Qute.Rfb.Api.Contexts;
using Qute.Rfb.Api.Extensions;
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
                                        writer.Write(values[0].GetInteger()); // id
                                        writer.Write(values[1].GetString()); // nome
                                        writer.Write(values[2].GetIntegerOrNull()); // natureza_juridica_id
                                        writer.Write(values[3].GetIntegerOrNull()); // qualificacao_id
                                        writer.Write(values[4].GetDecimalOrNull()); // capital_social
                                        writer.Write(values[5].GetIntegerOrNull()); // porte
                                        writer.Write(values[6].GetStringOrNull()); // ente_federativo
                                        if (++i % 100000 == 0)
                                            _logger.LogInformation($"{i} empresas inseridas");
                                    }
                                }
                            }
                            writer.Complete();
                        }
                        transaction.Commit();
                        _logger.LogInformation($"Total de {i} empresas inseridas");
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

    public void MigrateEstabelecimentos()
    {
        var estabelecimentos = _context.Estabelecimentos.Count();
        if (estabelecimentos == 0)
        {
            var csvDir = Path.Combine(_env.ContentRootPath, "downloads", "csv");
            var files = Directory.GetFiles(csvDir, "*ESTABE*");
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
                        "COPY rfb_estabelecimentos (cnpj, ordem, dv, matriz_filial, nome_fantasia, " +
                        "situacao_cadastral, situacao_cadastral_data, motivo_id, " +
                        "cidade_exterior, pais_id, " +
                        "inicio_atividades, cnae_id, cnaes_secundarios, " +
                        "tipo_logradouro, logradouro, numero, complemento, bairro, cep, uf, municipio_id, " +
                        "ddd1, telefone1, ddd2, telefone2, ddd_fax, telefone_fax, email, " +
                        "situacao_especial, situacao_especial_data) FROM STDIN (FORMAT BINARY)"))
                        {
                            using (var reader = new StreamReader(file, Encoding.Latin1))
                            {
                                while (!reader.EndOfStream)
                                {
                                    var line = reader.ReadLine();
                                    var values = line?.Split(';');
                                    if (values != null && values.Length == 30)
                                    {
                                        writer.StartRow();
                                        writer.Write(values[0].GetInteger()); // cnpj
                                        writer.Write(values[1].GetShort()); // ordem
                                        writer.Write(values[2].GetByte()); // dv
                                        writer.Write(values[3].GetIntegerOrNull()); // matriz_filial
                                        writer.Write(values[4].GetStringOrNull()); // nome_fantasia
                                        writer.Write(values[5].GetIntegerOrNull()); // situacao_cadastral
                                        writer.Write(values[6].GetDateOnlyOrNull());  // situacao_cadastral_data
                                        writer.Write(values[7].GetIntegerOrNull()); // motivo_id
                                        writer.Write(values[8].GetStringOrNull()); // cidade_exterior
                                        writer.Write(values[9].GetIntegerOrNull()); // pais_id
                                        writer.Write(values[10].GetDateOnlyOrNull()); // inicio_atividades
                                        writer.Write(values[11].GetIntegerOrNull()); // cnae_id
                                        writer.Write(values[12].GetIntegerArrayOrNull()); // cnaes_secundarios
                                        writer.Write(values[13].GetStringOrNull()); // tipo_logradouro
                                        writer.Write(values[14].GetStringOrNull()); // logradouro
                                        writer.Write(values[15].GetStringOrNull()); // numero
                                        writer.Write(values[16].GetStringOrNull()); // complemento
                                        writer.Write(values[17].GetStringOrNull()); // bairro
                                        writer.Write(values[18].GetStringOrNull()); // cep
                                        writer.Write(values[19].GetStringOrNull()); // uf
                                        writer.Write(values[20].GetIntegerOrNull()); // municipio_id
                                        writer.Write(values[21].GetStringOrNull()); // ddd1
                                        writer.Write(values[22].GetStringOrNull()); // telefone1
                                        writer.Write(values[23].GetStringOrNull()); // ddd2
                                        writer.Write(values[24].GetStringOrNull()); // telefone2
                                        writer.Write(values[25].GetStringOrNull()); // ddd_fax
                                        writer.Write(values[26].GetStringOrNull()); // telefone_fax
                                        writer.Write(values[27].GetStringOrNull()?.ToLower()); // email
                                        writer.Write(values[28].GetStringOrNull()); // situacao_especial
                                        writer.Write(values[29].GetDateOnlyOrNull()); // situacao_especial_data
                                        if (++i % 100000 == 0)
                                            _logger.LogInformation($"{i} estabelecimentos inseridos");
                                    }
                                }
                            }
                            writer.Complete();
                        }
                        transaction.Commit();
                        _logger.LogInformation($"Total de {i} estabelecimentos inseridos");
                    }

                }
            }
        }
    }

    public void MigrateSimples()
    {
        var simples = _context.MeisSimples.Count();
        if (simples == 0)
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
                        _logger.LogInformation($"Total de {i} simples inseridos");
                    }
                }
            }
        }
    }

    public void MigrateSocios()
    {
        var socios = _context.Socios.Count();
        if (socios == 0)
        {
            var csvDir = Path.Combine(_env.ContentRootPath, "downloads", "csv");
            var files = Directory.GetFiles(csvDir, "*SOCIO*");
            var i = 0;
            foreach (var file in files)
            {
                using (var connection = new NpgsqlConnection(_context.Database.GetConnectionString()))
                {
                    connection.Open();

                    using (var transaction = connection.BeginTransaction())
                    {
                        using (var writer = connection.BeginBinaryImport(
                            "COPY rfb_socios (cnpj, tipo, nome, cpf_cnpj, qualificacao_id, data_entrada, pais_id, " +
                            "representante_cpf_cnpj, representante_nome, representante_qualificacao_id, faixa_etaria) FROM STDIN (FORMAT BINARY)"))
                        {
                            using (var reader = new StreamReader(file, Encoding.Latin1))
                            {
                                while (!reader.EndOfStream)
                                {
                                    var line = reader.ReadLine();
                                    var values = line?.Split(';');
                                    if (values != null && values.Length == 11)
                                    {
                                        writer.StartRow();
                                        writer.Write(values[0].GetInteger()); // cnjp
                                        writer.Write(values[1].GetIntegerOrNull()); // tipo
                                        writer.Write(values[2].GetStringOrNull()); // nome
                                        writer.Write(values[3].GetStringOrNull()); //  cpf_cnpj
                                        writer.Write(values[4].GetIntegerOrNull()); // qualificacao_id
                                        writer.Write(values[5].GetDateOnlyOrNull()); // data_entrada
                                        writer.Write(values[6].GetIntegerOrNull()); // pais_id
                                        writer.Write(values[7].GetStringOrNull()); // representante_cpf_cnpj
                                        writer.Write(values[8].GetStringOrNull()); // representante_nome
                                        writer.Write(values[9].GetIntegerOrNull()); // representante_qualificacao_id
                                        writer.Write(values[10].GetIntegerOrNull()); // faixa_etaria
                                        if (++i % 100000 == 0)
                                            _logger.LogInformation($"{i} socios inseridos");
                                    }
                                }
                            }
                            writer.Complete();
                        }
                        transaction.Commit();
                        _logger.LogInformation($"Total de {i} socios inseridos");
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
        var fileNames = new List<string>();
        fileNames.Add("Cnaes.zip");
        fileNames.Add("Empresas0.zip");
        fileNames.Add("Empresas1.zip");
        fileNames.Add("Empresas2.zip");
        fileNames.Add("Empresas3.zip");
        fileNames.Add("Empresas4.zip");
        fileNames.Add("Empresas5.zip");
        fileNames.Add("Empresas6.zip");
        fileNames.Add("Empresas7.zip");
        fileNames.Add("Empresas8.zip");
        fileNames.Add("Empresas9.zip");
        fileNames.Add("Estabelecimentos0.zip");
        fileNames.Add("Estabelecimentos1.zip");
        fileNames.Add("Estabelecimentos2.zip");
        fileNames.Add("Estabelecimentos3.zip");
        fileNames.Add("Estabelecimentos4.zip");
        fileNames.Add("Estabelecimentos5.zip");
        fileNames.Add("Estabelecimentos6.zip");
        fileNames.Add("Estabelecimentos7.zip");
        fileNames.Add("Estabelecimentos8.zip");
        fileNames.Add("Estabelecimentos9.zip");
        fileNames.Add("Motivos.zip");
        fileNames.Add("Municipios.zip");
        fileNames.Add("Naturezas.zip");
        fileNames.Add("Paises.zip");
        fileNames.Add("Qualificacoes.zip");
        fileNames.Add("Simples.zip");
        fileNames.Add("Socios0.zip");
        fileNames.Add("Socios1.zip");
        fileNames.Add("Socios2.zip");
        fileNames.Add("Socios3.zip");
        fileNames.Add("Socios4.zip");
        fileNames.Add("Socios5.zip");
        fileNames.Add("Socios6.zip");
        fileNames.Add("Socios7.zip");
        fileNames.Add("Socios8.zip");
        fileNames.Add("Socios9.zip");
        //var date = DateTime.Now.AddMonths(-1);
        //var dateStr = date.ToString("yyyy-MM");
        //var rfbUrl = Environment.GetEnvironmentVariable("RFB_URL") ?? "https://arquivos.receitafederal.gov.br/cnpj/dados_abertos_cnpj";
        //string baseUrl = $"{rfbUrl}/{dateStr}/";
        //try
        //{
        //    using var client = _http.CreateClient();
        //    var htmlContent = await client.GetStringAsync(baseUrl);

        //    // Carregar o HTML na estrutura do HtmlAgilityPack
        //    var doc = new HtmlDocument();
        //    doc.LoadHtml(htmlContent);

        //    // Selecionar todos os elementos <a> com um atributo href
        //    var links = doc.DocumentNode.SelectNodes("//a[@href]");
        //    if (links != null)
        //    {
        //        // Filtrar os links que possuem nomes de arquivos e adicioná-los à lista
        //        fileNames = links
        //            .Where(link => link.Attributes["href"].Value.Contains(".zip")) // Filtrar arquivos .zip
        //            .Select(link => link.Attributes["href"].Value) // Pegar o valor do href
        //            .ToList();
        //    }
        //}
        //catch (Exception ex)
        //{
        //    _logger.LogError($"Erro ao obter os arquivos da RFB: {ex.Message} \n {ex.InnerException}");
        //}
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
