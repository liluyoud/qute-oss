using Quartz;

namespace Qute.Rfb.Api.Endpoints;

public static class JobEndpoints
{
    public static IEndpointRouteBuilder MapJobs(this IEndpointRouteBuilder app)
    {
        var migration = app.MapGroup("jobs").WithTags("Jobs");

        #region downloads
        migration.MapGet("download/basicos", async (ISchedulerFactory schedulerFactory) =>
        {
            // Obtém o scheduler
            var scheduler = await schedulerFactory.GetScheduler();

            // Cria o identificador do job
            var jobKey = new JobKey("DownloadBasico");
            if (!await scheduler.CheckExists(jobKey))
                return Results.BadRequest("Job não encontrado.");

            // Executar o job em background
            _ = Task.Run(() => scheduler.TriggerJob(jobKey));

            return Results.Ok("Job executado com sucesso.");
        })
        .WithName("JobDownloadBasico")
        .WithSummary("Download Básicos")
        .WithDescription("Download arquivos básicos")
        .Produces(200);

        migration.MapGet("download/empresas", async (ISchedulerFactory schedulerFactory) =>
        {
            // Obtém o scheduler
            var scheduler = await schedulerFactory.GetScheduler();

            // Cria o identificador do job
            var jobKey = new JobKey("DownloadEmpresa");
            if (!await scheduler.CheckExists(jobKey))
                return Results.BadRequest("Job não encontrado.");

            // Executar o job em background
            _ = Task.Run(() => scheduler.TriggerJob(jobKey));

            return Results.Ok("Job executado com sucesso.");
        })
        .WithName("JobDownloadEmpresa")
        .WithSummary("Download Empresas")
        .WithDescription("Download arquivos das empresas")
        .Produces(200);

        migration.MapGet("download/estabelecimentos", async (ISchedulerFactory schedulerFactory) =>
        {
            // Obtém o scheduler
            var scheduler = await schedulerFactory.GetScheduler();

            // Cria o identificador do job
            var jobKey = new JobKey("DownloadEstabelecimento");
            if (!await scheduler.CheckExists(jobKey))
                return Results.BadRequest("Job não encontrado.");

            // Executar o job em background
            _ = Task.Run(() => scheduler.TriggerJob(jobKey));

            return Results.Ok("Job executado com sucesso.");
        })
        .WithName("JobDownloadEstabelecimento")
        .WithSummary("Download Estabelecimentos")
        .WithDescription("Download arquivos dos estabelecimentos")
        .Produces(200);

        migration.MapGet("download/socios", async (ISchedulerFactory schedulerFactory) =>
        {
            // Obtém o scheduler
            var scheduler = await schedulerFactory.GetScheduler();

            // Cria o identificador do job
            var jobKey = new JobKey("DownloadSocio");
            if (!await scheduler.CheckExists(jobKey))
                return Results.BadRequest("Job não encontrado.");

            // Executar o job em background
            _ = Task.Run(() => scheduler.TriggerJob(jobKey));

            return Results.Ok("Job executado com sucesso.");
        })
        .WithName("JobDownloadSocio")
        .WithSummary("Download Sócios")
        .WithDescription("Download arquivos dos sócios")
        .Produces(200);

        migration.MapGet("process/files", async (ISchedulerFactory schedulerFactory) =>
        {
            // Obtém o scheduler
            var scheduler = await schedulerFactory.GetScheduler();

            // Cria o identificador do job
            var jobKey = new JobKey("ProcessFiles");
            if (!await scheduler.CheckExists(jobKey))
                return Results.BadRequest("Job não encontrado.");

            // Executar o job em background
            _ = Task.Run(() => scheduler.TriggerJob(jobKey));

            return Results.Ok("Job executado com sucesso.");
        })
        .WithName("JobProcessFiles")
        .WithSummary("Processa Arquivos")
        .WithDescription("Extrair e mover os arquvivos baixados")
        .Produces(200);
        #endregion

        #region migrations
        migration.MapGet("migrate/basico", async (ISchedulerFactory schedulerFactory) =>
        {
            // Obtém o scheduler
            var scheduler = await schedulerFactory.GetScheduler();

            // Cria o identificador do job
            var jobKey = new JobKey("MigrateBasico");
            if (!await scheduler.CheckExists(jobKey))
                return Results.BadRequest("Job não encontrado.");

            // Executar o job em background
            _ = Task.Run(() => scheduler.TriggerJob(jobKey));

            return Results.Ok("Job executado com sucesso.");
        })
        .WithName("JobMigrateBasico")
        .WithSummary("Migra Basicos")
        .WithDescription("Migra as tabelas básicas da receita")
        .Produces(200);

        migration.MapGet("migrate/empresas", async (ISchedulerFactory schedulerFactory) =>
        {
            // Obtém o scheduler
            var scheduler = await schedulerFactory.GetScheduler();

            // Cria o identificador do job
            var jobKey = new JobKey("MigrateEmpresas");
            if (!await scheduler.CheckExists(jobKey))
                return Results.BadRequest("Job não encontrado.");

            // Executar o job em background
            _ = Task.Run(() => scheduler.TriggerJob(jobKey));

            return Results.Ok("Job executado com sucesso.");
        })
        .WithName("JobMigrateEmpresas")
        .WithSummary("Migra Empresas")
        .WithDescription("Migra os dados dos arquivos de empresas para a tabela")
        .Produces(200);

        migration.MapGet("migrate/simples", async (ISchedulerFactory schedulerFactory) =>
        {
            // Obtém o scheduler
            var scheduler = await schedulerFactory.GetScheduler();

            // Cria o identificador do job
            var jobKey = new JobKey("MigrateSimples");
            if (!await scheduler.CheckExists(jobKey))
                return Results.BadRequest("Job não encontrado.");

            // Executar o job em background
            _ = Task.Run(() => scheduler.TriggerJob(jobKey));

            return Results.Ok("Job executado com sucesso.");
        })
        .WithName("JobMigrateSimples")
        .WithSummary("Migra Simples")
        .WithDescription("Migra os dados dos simples para a tabela")
        .Produces(200);

        migration.MapGet("migrate/estabelecimentos", async (ISchedulerFactory schedulerFactory) =>
        {
            // Obtém o scheduler
            var scheduler = await schedulerFactory.GetScheduler();

            // Cria o identificador do job
            var jobKey = new JobKey("MigrateEstabelecimentos");
            if (!await scheduler.CheckExists(jobKey))
                return Results.BadRequest("Job não encontrado.");

            // Executar o job em background
            _ = Task.Run(() => scheduler.TriggerJob(jobKey));

            return Results.Ok("Job executado com sucesso.");
        })
        .WithName("JobMigrateEstabelecimentos")
        .WithSummary("Migra Estabelecimentos")
        .WithDescription("Migra os dados dos estabelcimentos para a tabela")
        .Produces(200);

        migration.MapGet("migrate/socios", async (ISchedulerFactory schedulerFactory) =>
        {
            // Obtém o scheduler
            var scheduler = await schedulerFactory.GetScheduler();

            // Cria o identificador do job
            var jobKey = new JobKey("MigrateSocios");
            if (!await scheduler.CheckExists(jobKey))
                return Results.BadRequest("Job não encontrado.");

            // Executar o job em background
            _ = Task.Run(() => scheduler.TriggerJob(jobKey));

            return Results.Ok("Job executado com sucesso.");
        })
        .WithName("JobMigrateSocios")
        .WithSummary("Migra Sócios")
        .WithDescription("Migra os dados dos sócios para a tabela")
        .Produces(200);

        #endregion

        return app;
    }
}
