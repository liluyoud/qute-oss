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
    public DbSet<MeiSimples> MeisSimples { get; set; } = default!;
    public DbSet<Estabelecimento> Estabelecimentos { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cnae>()
           .Property(e => e.Id)
           .ValueGeneratedNever();
        modelBuilder.Entity<Motivo>()
           .Property(e => e.Id)
           .ValueGeneratedNever();
        modelBuilder.Entity<Municipio>()
           .Property(e => e.Id)
           .ValueGeneratedNever();
        modelBuilder.Entity<NaturezaJuridica>()
           .Property(e => e.Id)
           .ValueGeneratedNever();
        modelBuilder.Entity<Pais>()
           .Property(e => e.Id)
           .ValueGeneratedNever();
        modelBuilder.Entity<Qualificacao>()
           .Property(e => e.Id)
           .ValueGeneratedNever();

        modelBuilder.Entity<Empresa>()
           .Property(e => e.Id)
           .ValueGeneratedNever();
        modelBuilder.Entity<MeiSimples>()
           .Property(e => e.Id)
           .ValueGeneratedNever();
        modelBuilder.Entity<Estabelecimento>()
            .HasKey(e => new { e.Cnpj, e.Ordem, e.Dv });

        base.OnModelCreating(modelBuilder);
    }
}
