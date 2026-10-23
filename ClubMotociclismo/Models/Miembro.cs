using System.ComponentModel.DataAnnotations;

namespace ClubMotociclismo.Models;

// Entidad principal: un rider inscrito en el club
public class Miembro
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(60, ErrorMessage = "Máximo 60 caracteres")]
    [Display(Name = "Nombre completo")]
    public string Nombre { get; set; } = "";

    [Required(ErrorMessage = "El correo es obligatorio")]
    [EmailAddress(ErrorMessage = "El correo no es válido")]
    [Display(Name = "Correo electrónico")]
    public string Correo { get; set; } = "";

    [Required(ErrorMessage = "El teléfono es obligatorio")]
    [RegularExpression(@"^\d{7,10}$", ErrorMessage = "Escribe solo números (7 a 10 dígitos)")]
    [Display(Name = "Teléfono")]
    public string Telefono { get; set; } = "";

    [Required(ErrorMessage = "La edad es obligatoria")]
    [Range(18, 80, ErrorMessage = "Debes tener entre 18 y 80 años")]
    public int? Edad { get; set; }

    [Required(ErrorMessage = "La fecha es obligatoria")]
    [DataType(DataType.Date)]
    [Display(Name = "Fecha de ingreso")]
    public DateTime? FechaIngreso { get; set; }

    [Required(ErrorMessage = "Selecciona el tipo de moto")]
    [Display(Name = "Tipo de motocicleta")]
    public string TipoMoto { get; set; } = "";

    [Required(ErrorMessage = "El cilindraje es obligatorio")]
    [Range(50, 2000, ErrorMessage = "El cilindraje debe estar entre 50 y 2000 cc")]
    [Display(Name = "Cilindraje (cc)")]
    public int? Cilindraje { get; set; }

    [Required(ErrorMessage = "Selecciona un plan")]
    [Display(Name = "Tipo de membresía")]
    public string Plan { get; set; } = "Básica";

    // Se calcula en el controlador
    public decimal CuotaMensual { get; set; }
}
