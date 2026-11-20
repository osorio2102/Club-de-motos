using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ClubMotociclismo.Models;

// Entidad "uno" de la relación 1:N -> un Plan tiene muchos Miembros
public class Plan
{
    // Clave primaria (EF la reconoce por llamarse Id)
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre del plan es obligatorio")]
    [StringLength(30, ErrorMessage = "Máximo 30 caracteres")]
    [Display(Name = "Nombre del plan")]
    public string Nombre { get; set; } = "";

    [Required(ErrorMessage = "La cuota es obligatoria")]
    [Range(0, 1000000, ErrorMessage = "La cuota debe estar entre 0 y 1.000.000")]
    [Column(TypeName = "decimal(12,2)")]
    [Display(Name = "Cuota mensual")]
    public decimal CuotaMensual { get; set; }

    [StringLength(200, ErrorMessage = "Máximo 200 caracteres")]
    [Display(Name = "Descripción")]
    public string? Descripcion { get; set; }

    // Propiedad de navegación: todos los miembros que tienen este plan
    [ValidateNever]
    public ICollection<Miembro> Miembros { get; set; } = new List<Miembro>();
}
