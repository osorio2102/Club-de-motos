namespace ClubMotociclismo.Models;

// Datos fijos que no necesitan tabla propia.
// Los planes, miembros y eventos ahora se guardan en SQL Server (ver Data/ClubContext.cs).
public static class DatosClub
{
    public static readonly List<string> TiposMoto = new() { "Deportiva", "Touring", "Naked", "Enduro" };

    // Recargo mensual para motos de más de 600 cc
    public const decimal RecargoAltoCilindraje = 10000;
    public const int LimiteAltoCilindraje = 600;
}
