using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Qute.Rfb.Api.Contexts;
using Qute.Rfb.Api.Endpoints;

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

        app.MapRfbMigration();
    }

}
