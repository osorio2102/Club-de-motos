using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubMotociclismo.Models;

// Entidad "muchos" de la relación 1:N -> cada Miembro pertenece a un Plan
public class Miembro
{
    // Clave primaria
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(60, ErrorMessage = "Máximo 60 caracteres")]
    [Display(Name = "Nombre completo")]
    public string Nombre { get; set; } = "";

    [Required(ErrorMessage = "El correo es obligatorio")]
    [EmailAddress(ErrorMessage = "El correo no es válido")]
    [StringLength(100)]
    [Display(Name = "Correo electrónico")]
    public string Correo { get; set; } = "";

    [Required(ErrorMessage = "El teléfono es obligatorio")]
    [RegularExpression(@"^\d{7,10}$", ErrorMessage = "Escribe solo números (7 a 10 dígitos)")]
    [StringLength(10)]
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
    [StringLength(20)]
    [Display(Name = "Tipo de motocicleta")]
    public string TipoMoto { get; set; } = "";

    [Required(ErrorMessage = "El cilindraje es obligatorio")]
    [Range(50, 2000, ErrorMessage = "El cilindraje debe estar entre 50 y 2000 cc")]
    [Display(Name = "Cilindraje (cc)")]
    public int? Cilindraje { get; set; }

    // Agregada en la segunda migración (AgregarPlacaMiembro)
    [RegularExpression(@"^[A-Za-z]{3}\d{2}[A-Za-z]?$", ErrorMessage = "Formato de placa de moto: ABC12 o ABC12D")]
    [StringLength(6)]
    [Display(Name = "Placa")]
    public string? Placa { get; set; }

    // Clave foránea hacia la tabla Planes
    [Required(ErrorMessage = "Selecciona un plan")]
    [Display(Name = "Tipo de membresía")]
    public int? PlanId { get; set; }

    // Propiedad de navegación: el plan al que pertenece el miembro
    public Plan? Plan { get; set; }

    // Se calcula en el controlador (precio del plan + recargo por cilindraje)
    [Column(TypeName = "decimal(12,2)")]
    [Display(Name = "Cuota mensual")]
    public decimal CuotaMensual { get; set; }
}
