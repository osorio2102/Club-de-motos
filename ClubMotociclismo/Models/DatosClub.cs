namespace ClubMotociclismo.Models;

// Guarda los datos en memoria (no usamos base de datos todavía).
// Si se reinicia la aplicación, los miembros nuevos se pierden.
public static class DatosClub
{
    public static List<string> TiposMoto = new() { "Deportiva", "Touring", "Naked", "Enduro" };

    // Precio mensual de cada plan
    public static Dictionary<string, decimal> Planes = new()
    {
        { "Básica", 30000 },
        { "Gold", 60000 },
        { "Aventura", 90000 }
    };

    public static List<Evento> Eventos = new()
    {
        new Evento { Id = 1, Titulo = "Ruta a la montaña", Tipo = "Rodada", Descripcion = "Salida temprano para ver el amanecer y desayunar arriba.", Fecha = new DateTime(2026, 11, 8), Lugar = "Parque principal" },
        new Evento { Id = 2, Titulo = "Mantenimiento básico", Tipo = "Taller", Descripcion = "Aprende a revisar cadena, frenos y aceite de tu moto.", Fecha = new DateTime(2026, 11, 22), Lugar = "Sede del club" },
        new Evento { Id = 3, Titulo = "Viaje regional", Tipo = "Viaje", Descripcion = "Fin de semana rodando por los pueblos cercanos.", Fecha = new DateTime(2026, 12, 6), Lugar = "Terminal norte" }
    };

    public static List<Miembro> Miembros = new()
    {
        new Miembro { Id = 1, Nombre = "Andrés Gómez", Correo = "andres@correo.com", Telefono = "3001112233", Edad = 34, FechaIngreso = new DateTime(2026, 3, 15), TipoMoto = "Touring", Cilindraje = 650, Plan = "Gold", CuotaMensual = 60000 },
        new Miembro { Id = 2, Nombre = "Laura Pérez", Correo = "laura@correo.com", Telefono = "3104445566", Edad = 27, FechaIngreso = new DateTime(2026, 5, 2), TipoMoto = "Naked", Cilindraje = 300, Plan = "Básica", CuotaMensual = 30000 },
        new Miembro { Id = 3, Nombre = "Julián Torres", Correo = "julian@correo.com", Telefono = "3207778899", Edad = 41, FechaIngreso = new DateTime(2026, 8, 20), TipoMoto = "Enduro", Cilindraje = 450, Plan = "Aventura", CuotaMensual = 90000 }
    };
}
