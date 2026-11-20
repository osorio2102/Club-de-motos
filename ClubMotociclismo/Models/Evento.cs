using System.ComponentModel.DataAnnotations;

namespace ClubMotociclismo.Models;

// Un evento del club (rodada, taller o viaje). Se guarda en la tabla Eventos.
public class Evento
{
    public int Id { get; set; }

    [Required, StringLength(80)]
    public string Titulo { get; set; } = "";

    [Required, StringLength(20)]
    public string Tipo { get; set; } = "";

    [StringLength(300)]
    public string Descripcion { get; set; } = "";

    public DateTime Fecha { get; set; }

    [StringLength(80)]
    public string Lugar { get; set; } = "";
}
