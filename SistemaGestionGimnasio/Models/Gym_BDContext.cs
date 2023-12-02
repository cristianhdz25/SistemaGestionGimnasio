using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SistemaGestionGimnasio.Models
{
    public partial class Gym_BDContext : DbContext
    {
        public Gym_BDContext()
        {
        }

        public Gym_BDContext(DbContextOptions<Gym_BDContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cliente> Clientes { get; set; } = null!;
        public virtual DbSet<Empleado> Empleados { get; set; } = null!;
        public virtual DbSet<Membresium> Membresia { get; set; } = null!;
        public virtual DbSet<Usuario> Usuarios { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("server=localhost; database=Gym_BD; integrated security=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.ClienteCedula)
                    .HasName("PK__Cliente__BCD6305719CEB455");

                entity.ToTable("Cliente");

                entity.Property(e => e.ClienteCedula)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PrimerApellido)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SegundoApellido)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Teléfono)
                    .HasMaxLength(15)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Empleado>(entity =>
            {
                entity.HasKey(e => e.EmpleadoCedula)
                    .HasName("PK__Empleado__A47363FB785A34F2");

                entity.ToTable("Empleado");

                entity.Property(e => e.EmpleadoCedula)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Horario)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PrimerApellido)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Salario).HasColumnType("numeric(10, 2)");

                entity.Property(e => e.SegundoApellido)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Teléfono)
                    .HasMaxLength(15)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Membresium>(entity =>
            {
                entity.Property(e => e.ClienteCedula)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaInicio).HasColumnType("date");

                entity.Property(e => e.FechaVencimiento).HasColumnType("date");

                entity.Property(e => e.Precio).HasColumnType("numeric(10, 2)");

                entity.Property(e => e.TipoMembresia)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.ClienteCedulaNavigation)
                    .WithMany(p => p.Membresia)
                    .HasForeignKey(d => d.ClienteCedula)
                    .HasConstraintName("FK__Membresia__Clien__4D94879B");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("Usuario");

                entity.Property(e => e.Contrasenia)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NombreUsuario)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
