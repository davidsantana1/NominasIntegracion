using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Nominas.Models;

namespace Nominas.DataAccess.Data;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

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

        modelBuilder.Entity<Departamento>()
            .HasMany(d => d.Empleados)
            .WithOne(e => e.Departamento)
            .HasForeignKey(e => e.DepartamentoId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Departamento>()
            .HasOne(d => d.ResponsableDeArea)
            .WithOne()
            .HasForeignKey<Departamento>(d => d.ResponsableDeAreaId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<RegistroTransaccion>()
            .HasOne(rt => rt.Empleado)
            .WithMany(e => e.RegistroTransacciones)
            .HasForeignKey(rt => rt.EmpleadoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Puesto>().HasData(
            new Puesto { Id = 1, Nombre = "Desarrollador", NivelDeRiesgo = "Medio", NivelMinimoSalario = 30000, NivelMaximoSalario = 60000 },
            new Puesto { Id = 2, Nombre = "Analista", NivelDeRiesgo = "Bajo", NivelMinimoSalario = 25000, NivelMaximoSalario = 50000 }
        );

        // Seed data for Empleados
        modelBuilder.Entity<Empleado>().HasData(
            new Empleado { Id = 1, Cedula = "001-1234567-8", Nombre = "Juan Pérez", DepartamentoId = 1, PuestoId = 1, SalarioMensual = 35000, NominaId = 1 },
            new Empleado { Id = 2, Cedula = "002-2345678-9", Nombre = "Ana Gómez", DepartamentoId = 2, PuestoId = 2, SalarioMensual = 28000, NominaId = 1 }
        );

        // Seed data for Departamentos
        modelBuilder.Entity<Departamento>().HasData(
            new Departamento { Id = 1, Nombre = "IT", UbicacionFisica = "Edificio A", ResponsableDeAreaId = 1 },
            new Departamento { Id = 2, Nombre = "Recursos Humanos", UbicacionFisica = "Edificio B", ResponsableDeAreaId = 2 }
        );

        modelBuilder.Entity<TipoDeDeduccion>().HasData(
            new TipoDeDeduccion { Id = 1, Nombre = "AFP", DependeDeSalario = true, Estado = true },
            new TipoDeDeduccion { Id = 2, Nombre = "ARS", DependeDeSalario = true, Estado = true },
            new TipoDeDeduccion { Id = 3, Nombre = "Seguro de Vida", DependeDeSalario = false, Estado = true },
            new TipoDeDeduccion { Id = 4, Nombre = "Préstamo Personal", DependeDeSalario = false, Estado = true }
        );

        // Datos iniciales para TipoDeIngreso
        modelBuilder.Entity<TipoDeIngreso>().HasData(
            new TipoDeIngreso { Id = 1, Nombre = "Bono de Desempeño", DependeDeSalario = true, Estado = true },
            new TipoDeIngreso { Id = 2, Nombre = "Gratificación Anual", DependeDeSalario = true, Estado = true },
            new TipoDeIngreso { Id = 3, Nombre = "Comisiones", DependeDeSalario = true, Estado = true },
            new TipoDeIngreso { Id = 4, Nombre = "Reembolso de Gastos", DependeDeSalario = false, Estado = true }
        );

        modelBuilder.Entity<RegistroTransaccion>().HasData(
    new RegistroTransaccion
    {
        Id = 1,
        EmpleadoId = 1, // Asegúrate de que este ID exista en la tabla Empleados
        Ingreso = 1000,
        Deduccion = 200,
        TipoTransaccion = TipoTransaccion.Ingreso,
        Fecha = new DateTime(2024, 9, 1),
        Monto = 800,
        Estado = "Aprobado"
    },
    new RegistroTransaccion
    {
        Id = 2,
        EmpleadoId = 2, // Asegúrate de que este ID exista en la tabla Empleados
        Ingreso = 1200,
        Deduccion = 150,
        TipoTransaccion = TipoTransaccion.Ingreso,
        Fecha = new DateTime(2024, 9, 5),
        Monto = 1050,
        Estado = "Aprobado"
    },
    new RegistroTransaccion
    {
        Id = 3,
        EmpleadoId = 1, // Asegúrate de que este ID exista en la tabla Empleados
        Ingreso = 500,
        Deduccion = 50,
        TipoTransaccion = TipoTransaccion.Ingreso,
        Fecha = new DateTime(2024, 9, 10),
        Monto = 450,
        Estado = "Pendiente"
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
