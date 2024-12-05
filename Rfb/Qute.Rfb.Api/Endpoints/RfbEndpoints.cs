using Microsoft.Extensions.Caching.Distributed;
using Microsoft.EntityFrameworkCore;
using Qute.Shared.Extensions;
using Qute.Rfb.Api.Contexts;
using Qute.Rfb.Shared.Entities;

namespace Qute.Rfb.Api.Endpoints;

public static class RfbEndpoints
{
    public static IEndpointRouteBuilder MapRfb(this IEndpointRouteBuilder app)
    {
        var migration = app.MapGroup("rfb").WithTags("RFB");

        migration.MapGet("cnaes", async (string identificador, RfbContext context, IDistributedCache cache) =>
        {
            var cnaes = await cache.GetOrCreateAsync($"qute-cnaes", async () =>
            {
                return await context.Cnaes.ToListAsync();
            }, TimeSpan.FromMinutes(30));
            return cnaes;
        })
        .WithName("cnaes")
        .WithSummary("Todos os CNAES")
        .WithDescription("Retorna todos os CNAES")
        .Produces<List<Cnae>?>(200);


        return app;
    }
}
