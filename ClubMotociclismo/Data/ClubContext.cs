using Microsoft.EntityFrameworkCore;
using ClubMotociclismo.Models;

namespace ClubMotociclismo.Data;

// Puente entre las clases de C# y la base de datos de SQL Server
public class ClubContext : DbContext
{
    public ClubContext(DbContextOptions<ClubContext> options) : base(options)
    {
    }

    // Cada DbSet se convierte en una tabla
    public DbSet<Plan> Planes { get; set; }
    public DbSet<Miembro> Miembros { get; set; }
    public DbSet<Evento> Eventos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Relación 1:N -> un Plan tiene muchos Miembros, cada Miembro tiene un Plan.
        // Restrict: no se puede borrar un plan mientras tenga miembros.
        modelBuilder.Entity<Miembro>()
            .HasOne(m => m.Plan)
            .WithMany(p => p.Miembros)
            .HasForeignKey(m => m.PlanId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        // No se pueden repetir correos ni nombres de plan
        modelBuilder.Entity<Miembro>().HasIndex(m => m.Correo).IsUnique();
        modelBuilder.Entity<Plan>().HasIndex(p => p.Nombre).IsUnique();

        // Datos iniciales (se insertan con la migración)
        modelBuilder.Entity<Plan>().HasData(
            new Plan { Id = 1, Nombre = "Básica", CuotaMensual = 30000, Descripcion = "Rodadas grupales de fin de semana." },
            new Plan { Id = 2, Nombre = "Gold", CuotaMensual = 60000, Descripcion = "Rodadas, talleres de mantenimiento y descuentos en tiendas aliadas." },
            new Plan { Id = 3, Nombre = "Aventura", CuotaMensual = 90000, Descripcion = "Todo lo del plan Gold más viajes regionales con asistencia." }
        );

        modelBuilder.Entity<Miembro>().HasData(
            new Miembro { Id = 1, Nombre = "Andrés Gómez", Correo = "andres@correo.com", Telefono = "3001112233", Edad = 34, FechaIngreso = new DateTime(2026, 3, 15), TipoMoto = "Touring", Cilindraje = 650, PlanId = 2, CuotaMensual = 70000 },
            new Miembro { Id = 2, Nombre = "Laura Pérez", Correo = "laura@correo.com", Telefono = "3104445566", Edad = 27, FechaIngreso = new DateTime(2026, 5, 2), TipoMoto = "Naked", Cilindraje = 300, PlanId = 1, CuotaMensual = 30000 },
            new Miembro { Id = 3, Nombre = "Julián Torres", Correo = "julian@correo.com", Telefono = "3207778899", Edad = 41, FechaIngreso = new DateTime(2026, 8, 20), TipoMoto = "Enduro", Cilindraje = 450, PlanId = 3, CuotaMensual = 90000 }
        );

        modelBuilder.Entity<Evento>().HasData(
            new Evento { Id = 1, Titulo = "Ruta a la montaña", Tipo = "Rodada", Descripcion = "Salida temprano para ver el amanecer y desayunar arriba.", Fecha = new DateTime(2026, 11, 29), Lugar = "Parque principal" },
            new Evento { Id = 2, Titulo = "Mantenimiento básico", Tipo = "Taller", Descripcion = "Aprende a revisar cadena, frenos y aceite de tu moto.", Fecha = new DateTime(2026, 12, 6), Lugar = "Sede del club" },
            new Evento { Id = 3, Titulo = "Viaje regional", Tipo = "Viaje", Descripcion = "Fin de semana rodando por los pueblos cercanos.", Fecha = new DateTime(2026, 12, 13), Lugar = "Terminal norte" }
        );
    }
}
