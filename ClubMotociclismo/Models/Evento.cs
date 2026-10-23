namespace ClubMotociclismo.Models;

// Un evento del club (rodada, taller o viaje)
public class Evento
{
    public int Id { get; set; }
    public string Titulo { get; set; } = "";
    public string Tipo { get; set; } = "";
    public string Descripcion { get; set; } = "";
    public DateTime Fecha { get; set; }
    public string Lugar { get; set; } = "";
}
