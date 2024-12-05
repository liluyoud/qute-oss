﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Qute.Rfb.Api.Contexts;

#nullable disable

namespace Qute.Rfb.Api.Migrations
{
    [DbContext(typeof(RfbContext))]
    [Migration("20241205050442_Cnaes")]
    partial class Cnaes
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Qute.Rfb.Shared.Entities.Cnae", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(7)
                        .HasColumnType("character varying(7)")
                        .HasColumnName("id");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("descricao");

                    b.HasKey("Id");

                    b.ToTable("rfb_cnaes");
                });
#pragma warning restore 612, 618
        }
    }
}
