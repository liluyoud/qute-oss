using Microsoft.EntityFrameworkCore;
using Qute.Rfb.Shared.Entities;

namespace Qute.Rfb.Api.Contexts;

public class RfbContext(DbContextOptions<RfbContext> options) : DbContext(options)
{
    public DbSet<Cnae> Cnaes { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
