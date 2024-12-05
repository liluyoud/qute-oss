using Microsoft.EntityFrameworkCore;
using Qute.Rfb.Shared.Entities;

namespace Qute.Rfb.Api.Contexts;

public class RfbContext(DbContextOptions<RfbContext> options) : DbContext(options)
{
    public DbSet<Cnae> Cnaes { get; set; } = default!;
    public DbSet<Motivo> Motivos { get; set; } = default!;
    public DbSet<Municipio> Municipios { get; set; } = default!;
    public DbSet<Pais> Paises { get; set; } = default!;
    public DbSet<NaturezaJuridica> NaturezasJuridicas { get; set; } = default!;
    public DbSet<Qualificacao> Qualificacoes { get; set; } = default!;
    public DbSet<Empresa> Empresas { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
