using System;
using System.Collections.Generic;
using HsqvLogistica.Models.Entities.Store;
using Microsoft.EntityFrameworkCore;

namespace HsqvLogistica.Data;

public partial class StoreDbContext : DbContext
{
    public StoreDbContext()
    {
    }

    public StoreDbContext(DbContextOptions<StoreDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Almacen> Almacens { get; set; }

    public virtual DbSet<Articulo> Articulos { get; set; }

    public virtual DbSet<GarantiaDetalle> GarantiaDetalles { get; set; }

    public virtual DbSet<Garantium> Garantia { get; set; }

    public virtual DbSet<Linea> Lineas { get; set; }

    public virtual DbSet<Motivo> Motivos { get; set; }

    public virtual DbSet<Movimiento> Movimientos { get; set; }

    public virtual DbSet<MovimientoDetalle> MovimientoDetalles { get; set; }

    public virtual DbSet<Pedido> Pedidos { get; set; }

    public virtual DbSet<PedidoDetalle> PedidoDetalles { get; set; }

    public virtual DbSet<TipoUsuario> TipoUsuarios { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("workstation id=DbStore.mssql.somee.com;packet size=4096;user id=adminEbueno81;pwd=ACEace11;data source=DbStore.mssql.somee.com;persist security info=False;initial catalog=DbStore;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Almacen>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Almacen__3214EC07DAF2A424");
        });

        modelBuilder.Entity<Articulo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Articulo__3214EC07953AB1D3");

            entity.Property(e => e.Activo).HasDefaultValue(true);

            entity.HasOne(d => d.IdLineaNavigation).WithMany(p => p.Articulos).HasConstraintName("FK_Articulo_Linea");
        });

        modelBuilder.Entity<GarantiaDetalle>(entity =>
        {
            entity.Property(e => e.Activo).HasDefaultValue(true);

            entity.HasOne(d => d.IdArticuloNavigation).WithMany(p => p.GarantiaDetalles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GarantiaDetalle_Articulo");

            entity.HasOne(d => d.IdGarantiaNavigation).WithMany(p => p.GarantiaDetalles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GarantiaDetalle_Garantia");
        });

        modelBuilder.Entity<Garantium>(entity =>
        {
            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.NroGuia).IsFixedLength();
            entity.Property(e => e.NroSerie).IsFixedLength();
        });

        modelBuilder.Entity<Linea>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Linea__3214EC071830FA29");
        });

        modelBuilder.Entity<Motivo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Motivo__3214EC077CE6925A");
        });

        modelBuilder.Entity<Movimiento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Movimien__3214EC07E628E668");

            entity.Property(e => e.Activo).HasDefaultValue(true);

            entity.HasOne(d => d.IdMotivoNavigation).WithMany(p => p.Movimientos).HasConstraintName("FK_Movimiento_Motivo");
        });

        modelBuilder.Entity<MovimientoDetalle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Movimien__3214EC07F13319ED");

            entity.HasOne(d => d.IdArticuloNavigation).WithMany(p => p.MovimientoDetalles).HasConstraintName("FK_MovimientoDetalle_Articulo");

            entity.HasOne(d => d.IdMovNavigation).WithMany(p => p.MovimientoDetalles).HasConstraintName("FK_MovimientoDetalle_Movimiento");
        });

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Pedido__3214EC07AD688CE3");

            entity.Property(e => e.Activo).HasDefaultValue(true);
        });

        modelBuilder.Entity<PedidoDetalle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PedidoDe__3214EC07477B5464");

            entity.HasOne(d => d.IdArticuloNavigation).WithMany(p => p.PedidoDetalles).HasConstraintName("FK_PedidoDetalle_Articulo");

            entity.HasOne(d => d.IdPedidoNavigation).WithMany(p => p.PedidoDetalles).HasConstraintName("FK_PedidoDetalle_Pedido");
        });

        modelBuilder.Entity<TipoUsuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TipoUsua__3214EC0776D6179D");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Usuario__3214EC070FA055A8");

            entity.HasOne(d => d.IdTipoNavigation).WithMany(p => p.Usuarios).HasConstraintName("FK_Usuario_TipoUsuario");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
