using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Nominas.Models;

namespace Nominas.DataAccess.Data;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<DetalleDeduccion>(entity =>
        {
            entity.HasNoKey();
            entity.ToView(null); // No se mapea a una tabla en la base de datos
        });

        modelBuilder.Entity<DetalleIngreso>(entity =>
        {
            entity.HasNoKey();
            entity.ToView(null); // No se mapea a una tabla en la base de datos
        });

        modelBuilder
            .Entity<Departamento>()
            .HasMany(d => d.Empleados)
            .WithOne(e => e.Departamento)
            .HasForeignKey(e => e.DepartamentoId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder
            .Entity<Departamento>()
            .HasOne(d => d.ResponsableDeArea)
            .WithOne()
            .HasForeignKey<Departamento>(d => d.ResponsableDeAreaId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relación Nomina -> Empleado (muchas nóminas por empleado)
        modelBuilder
            .Entity<Nomina>()
            .HasOne(n => n.Empleado)
            .WithMany() // no hay colección en Empleado
            .HasForeignKey(n => n.EmpleadoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder
            .Entity<RegistroTransaccion>()
            .HasOne(rt => rt.Empleado)
            .WithMany(e => e.RegistroTransacciones)
            .HasForeignKey(rt => rt.EmpleadoId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configuración de precisión para propiedades decimales
        modelBuilder.Entity<Empleado>(entity =>
        {
            entity.Property(e => e.SalarioMensual).HasPrecision(18, 2);
        });

        modelBuilder.Entity<Puesto>(entity =>
        {
            entity.Property(p => p.NivelMinimoSalario).HasPrecision(18, 2);
            entity.Property(p => p.NivelMaximoSalario).HasPrecision(18, 2);
        });

        modelBuilder.Entity<Nomina>(entity =>
        {
            entity.Property(n => n.SalarioBruto).HasPrecision(18, 2);
            entity.Property(n => n.TotalDeducciones).HasPrecision(18, 2);
            entity.Property(n => n.TotalIngresos).HasPrecision(18, 2);
            entity.Property(n => n.SalarioNeto).HasPrecision(18, 2);
        });

        modelBuilder.Entity<RegistroTransaccion>(entity =>
        {
            entity.Property(r => r.Ingreso).HasPrecision(18, 2);
            entity.Property(r => r.Deduccion).HasPrecision(18, 2);
            entity.Property(r => r.Monto).HasPrecision(18, 2);
        });

        modelBuilder.Entity<TipoDeDeduccion>(entity =>
        {
            entity.Property(t => t.Porcentaje).HasPrecision(5, 2);
        });

        modelBuilder.Entity<TipoDeIngreso>(entity =>
        {
            entity.Property(t => t.Porcentaje).HasPrecision(5, 2);
        });

        modelBuilder
            .Entity<Puesto>()
            .HasData(
                new Puesto
                {
                    Id = 1,
                    Nombre = "Desarrollador",
                    NivelDeRiesgo = "Medio",
                    NivelMinimoSalario = 30000,
                    NivelMaximoSalario = 60000,
                },
                new Puesto
                {
                    Id = 2,
                    Nombre = "Analista",
                    NivelDeRiesgo = "Bajo",
                    NivelMinimoSalario = 25000,
                    NivelMaximoSalario = 50000,
                }
            );

        // Seed data for Empleados
        modelBuilder
            .Entity<Empleado>()
            .HasData(
                new Empleado
                {
                    Id = 1,
                    Cedula = "001-1234567-8",
                    Nombre = "Juan Pérez",
                    DepartamentoId = 1,
                    PuestoId = 1,
                    SalarioMensual = 35000,
                    NominaId = 1,
                },
                new Empleado
                {
                    Id = 2,
                    Cedula = "002-2345678-9",
                    Nombre = "Ana Gómez",
                    DepartamentoId = 2,
                    PuestoId = 2,
                    SalarioMensual = 28000,
                    NominaId = 1,
                }
            );

        // Seed data for Departamentos
        modelBuilder
            .Entity<Departamento>()
            .HasData(
                new Departamento
                {
                    Id = 1,
                    Nombre = "IT",
                    UbicacionFisica = "Edificio A",
                    ResponsableDeAreaId = 1,
                },
                new Departamento
                {
                    Id = 2,
                    Nombre = "Recursos Humanos",
                    UbicacionFisica = "Edificio B",
                    ResponsableDeAreaId = 2,
                }
            );

        modelBuilder
            .Entity<TipoDeDeduccion>()
            .HasData(
                new TipoDeDeduccion
                {
                    Id = 1,
                    Nombre = "AFP",
                    DependeDeSalario = true,
                    Estado = true,
                },
                new TipoDeDeduccion
                {
                    Id = 2,
                    Nombre = "ARS",
                    DependeDeSalario = true,
                    Estado = true,
                },
                new TipoDeDeduccion
                {
                    Id = 3,
                    Nombre = "Seguro de Vida",
                    DependeDeSalario = false,
                    Estado = true,
                },
                new TipoDeDeduccion
                {
                    Id = 4,
                    Nombre = "Préstamo Personal",
                    DependeDeSalario = false,
                    Estado = true,
                }
            );

        // Datos iniciales para TipoDeIngreso
        modelBuilder
            .Entity<TipoDeIngreso>()
            .HasData(
                new TipoDeIngreso
                {
                    Id = 1,
                    Nombre = "Bono de Desempeño",
                    DependeDeSalario = true,
                    Estado = true,
                },
                new TipoDeIngreso
                {
                    Id = 2,
                    Nombre = "Gratificación Anual",
                    DependeDeSalario = true,
                    Estado = true,
                },
                new TipoDeIngreso
                {
                    Id = 3,
                    Nombre = "Comisiones",
                    DependeDeSalario = true,
                    Estado = true,
                },
                new TipoDeIngreso
                {
                    Id = 4,
                    Nombre = "Reembolso de Gastos",
                    DependeDeSalario = false,
                    Estado = true,
                }
            );

        modelBuilder
            .Entity<RegistroTransaccion>()
            .HasData(
                new RegistroTransaccion
                {
                    Id = 1,
                    EmpleadoId = 1, // Asegúrate de que este ID exista en la tabla Empleados
                    Ingreso = 1000,
                    Deduccion = 200,
                    TipoTransaccion = 0,
                    Fecha = new DateTime(2024, 9, 1),
                    Monto = 800,
                    Estado = "Aprobado",
                },
                new RegistroTransaccion
                {
                    Id = 2,
                    EmpleadoId = 2, // Asegúrate de que este ID exista en la tabla Empleados
                    Ingreso = 1200,
                    Deduccion = 150,
                    TipoTransaccion = 0,
                    Fecha = new DateTime(2024, 9, 5),
                    Monto = 1050,
                    Estado = "Aprobado",
                }
            );

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Departamento> Departamentos { get; set; }
    public DbSet<Empleado> Empleados { get; set; }
    public DbSet<Puesto> Puestos { get; set; }
    public DbSet<RegistroTransaccion> RegistroTransacciones { get; set; }
    public DbSet<TipoDeDeduccion> TiposDeDeducciones { get; set; }
    public DbSet<TipoDeIngreso> TiposDeIngreso { get; set; }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<Nomina> Nominas { get; set; }
}
