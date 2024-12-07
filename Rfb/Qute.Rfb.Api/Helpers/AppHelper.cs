using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Quartz;
using Scalar.AspNetCore;
using Qute.Rfb.Api.Contexts;
using Qute.Rfb.Api.Endpoints;
using Qute.Rfb.Api.Jobs;
using Qute.Rfb.Api.Services;

namespace Qute.Rfb.Api.Helpers;

public static class AppHelper
{
    public static void AddQuteServices(this IServiceCollection services, IConfiguration conf)
    {
        // http
        services.AddOpenApi();
        services.AddHttpClient();

        // load environment
        Env.Load();

        // postgres
        var postgresHost = Environment.GetEnvironmentVariable("POSTGRES_HOST") ?? "postgres";
        var postgresPort = Environment.GetEnvironmentVariable("POSTGRES_PORT") ?? "5432";
        var postgresDb = Environment.GetEnvironmentVariable("POSTGRES_DB") ?? "rfb";
        var postgresUser = Environment.GetEnvironmentVariable("POSTGRES_USER") ?? "postgres";
        var postgresPwd = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD") ?? "postgres";
        var postgresConnection = $"Host={postgresHost};Port={postgresPort};Database={postgresDb};Username={postgresUser};Password={postgresPwd}";
        services.AddDbContext<RfbContext>(options => options.UseNpgsql(postgresConnection, o => o.MigrationsAssembly("Qute.Rfb.Api")));

        // redis
        var redisHost = Environment.GetEnvironmentVariable("REDIS_HOST") ?? "redis";
        var redisPort = Environment.GetEnvironmentVariable("REDIS_PORT") ?? "6379";
        var redisUser = Environment.GetEnvironmentVariable("REDIS_USER") ?? null;
        var redisPassword = Environment.GetEnvironmentVariable("REDIS_PASSWORD") ?? null;
        var redisDb = Environment.GetEnvironmentVariable("REDIS_DB") ?? "0";
        var redisUrl = $"{redisHost}:{redisPort},defaultDatabase={redisDb}";
        redisUrl += redisUser != null ? $",user={redisUser}" : "";
        redisUrl += redisPassword != null ? $",password={redisPassword}" : "";
        services.AddStackExchangeRedisCache(options => options.Configuration = redisUrl);

        // services
        services.AddScoped<RfbServices>();

        // quartz
        services.AddQuartz(q =>
        {
            // keys
            var downloadBasicoKey = new JobKey("DownloadBasico");
            var downloadEmpresaKey = new JobKey("DownloadEmpresa");
            var downloadEstabelecimentoKey = new JobKey("DownloadEstabelecimento");
            var downloadSocioKey = new JobKey("DownloadSocio");
            
            var processFilesKey = new JobKey("ProcessFiles");

            var migrateBasicoKey = new JobKey("MigrateBasico");
            var migrateEmpresasKey = new JobKey("MigrateEmpresas");
            var migrateSimplesKey = new JobKey("MigrateSimples");
            var migrateEstabelecimentosKey = new JobKey("MigrateEstabelecimentos");
            var migrateSociosKey = new JobKey("MigrateSocios");

            // Jobs
            q.AddJob<DownloadBasico>(opts => opts.WithIdentity(downloadBasicoKey));
            q.AddJob<DownloadEmpresa>(opts => opts.WithIdentity(downloadEmpresaKey));
            q.AddJob<DownloadEstabelecimento>(opts => opts.WithIdentity(downloadEstabelecimentoKey));
            q.AddJob<DownloadSocio>(opts => opts.WithIdentity(downloadSocioKey));

            q.AddJob<ProcessFiles>(opts => opts.WithIdentity(processFilesKey));

            q.AddJob<MigrateBasico>(opts => opts.WithIdentity(migrateBasicoKey));
            q.AddJob<MigrateEmpresa>(opts => opts.WithIdentity(migrateEmpresasKey));
            q.AddJob<MigrateSimples>(opts => opts.WithIdentity(migrateSimplesKey));
            q.AddJob<MigrateEstabelecimento>(opts => opts.WithIdentity(migrateEstabelecimentosKey));
            q.AddJob<MigrateSocio>(opts => opts.WithIdentity(migrateSociosKey));

            var cronDownloadBasico = Environment.GetEnvironmentVariable("DOWNLOAD_BASICOS") ?? "0 0 5 * * ?"; // meia noite do dia 5 de cada mês
            var cronDownloadEmpresas = Environment.GetEnvironmentVariable("DOWNLOAD_EMPRESAS") ?? "0 1 5 * * ?"; // 1 da manhã do dia 5 de cada mês
            var cronDownloadEstabelecimentos = Environment.GetEnvironmentVariable("DOWNLOAD_ESTABELECIMENTOS") ?? "0 2 5 * * ?"; // 2 da mahã do dia 5 de cada mês
            var cronDownloadSocios = Environment.GetEnvironmentVariable("DOWNLOAD_SOCIOS") ?? "0 3 5 * * ?"; // 3 da manhã do dia 5 de cada mês
            var cronProcessaArquivos = Environment.GetEnvironmentVariable("PROCESSA_ARQUIVOS") ?? "0 0 6 * * ?"; // meia noite do dia 6 de cada mês
            var cronMigraBasico = Environment.GetEnvironmentVariable("MIGRA_BASICOS") ?? "0 1 6 * * ?"; // 1 da manhã do dia 6 de cada mês
            var cronMigraEmpresas = Environment.GetEnvironmentVariable("MIGRA_EMPRESAS") ?? "0 2 6 * * ?"; // 2 da manhã do dia 6 de cada mês
            var cronMigraSimples = Environment.GetEnvironmentVariable("MIGRA_SIMPLES") ?? "0 0 7 * * ?"; // meia noite do dia 7 de cada mês
            var cronMigraEstabelecimentos = Environment.GetEnvironmentVariable("MIGRA_ESTABELECIMENTOS") ?? "0 0 8 * * ?"; // meia noite do dia 8 de cada mês
            var cronMigraSocios = Environment.GetEnvironmentVariable("MIGRA_SOCIOS") ?? "0 0 9 * * ?"; // meia noite do dia 9 de cada mês

            // Triggers
            q.AddTrigger(opts => opts
                .ForJob(downloadBasicoKey) 
                .WithIdentity("DownloadBasico-Mensal") 
                .WithCronSchedule(cronDownloadBasico)); // meia noite do dia 5 de cada mês
            q.AddTrigger(opts => opts
                .ForJob(downloadEmpresaKey)
                .WithIdentity("DownloadEmpresa-Mensal")
                .WithCronSchedule(cronDownloadEmpresas)); // meia noite do dia 5 de cada mês
            q.AddTrigger(opts => opts
                .ForJob(downloadEstabelecimentoKey)
                .WithIdentity("DownloadEstabelecimento-Mensal")
                .WithCronSchedule(cronDownloadEstabelecimentos)); // meia noite do dia 5 de cada mês
            q.AddTrigger(opts => opts
                .ForJob(downloadSocioKey)
                .WithIdentity("DownloadSocio-Mensal")
                .WithCronSchedule(cronDownloadSocios)); // meia noite do dia 5 de cada mês

            q.AddTrigger(opts => opts
                .ForJob(processFilesKey)
                .WithIdentity("ProcessFiles-Mensal")
                .WithCronSchedule(cronProcessaArquivos)); // meia noite do dia 5 de cada mês

            q.AddTrigger(opts => opts
                .ForJob(migrateBasicoKey)
                .WithIdentity("MigrateBasico-Mensal")
                .WithCronSchedule(cronMigraBasico)); // meia noite do dia 5 de cada mês
            q.AddTrigger(opts => opts
                .ForJob(migrateEmpresasKey)
                .WithIdentity("MigrateEmpresa-Mensal")
                .WithCronSchedule(cronMigraEmpresas)); // meia noite do dia 5 de cada mês
            q.AddTrigger(opts => opts
                .ForJob(migrateSimplesKey)
                .WithIdentity("MigrateSimples-Mensal")
                .WithCronSchedule(cronMigraSimples)); // meia noite do dia 5 de cada mês
            q.AddTrigger(opts => opts
                .ForJob(migrateEstabelecimentosKey)
                .WithIdentity("MigrateEstabelecimentos-Mensal")
                .WithCronSchedule(cronMigraEstabelecimentos)); // meia noite do dia 5 de cada mês
            q.AddTrigger(opts => opts
                .ForJob(migrateSociosKey)
                .WithIdentity("MigrateSocios-Mensal")
                .WithCronSchedule(cronMigraSocios)); // meia noite do dia 5 de cada mês

        });
        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
    }

    public static void UseQute(this WebApplication app, IConfiguration conf)
    {
        var showApi = Environment.GetEnvironmentVariable("SHOW_API") ?? "false";
        if (app.Environment.IsDevelopment() || showApi == "true")
        {
            app.MapOpenApi();
            app.MapScalarApiReference(options =>
            {
                options.Title = "Qute Api Reference";
            });
        }

        // migration
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<RfbContext>();
                context.Database.Migrate();
            }
            catch (Exception ex)
            {
                // Log para erros durante a migração
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "Ocorreu um erro ao aplicar as migrações.");
            }
        }

        app.MapRfb();
        app.MapTables();
        app.MapJobs();
    }

}
