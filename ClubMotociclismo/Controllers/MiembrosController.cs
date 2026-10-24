using Microsoft.AspNetCore.Mvc;
using ClubMotociclismo.Models;

// hola  estoy haciendo un commit 2
namespace ClubMotociclismo.Controllers;

public class MiembrosController : Controller
{
    // GET: /Miembros?tipoMoto=Naked -> lista de riders con filtro
    [HttpGet]
    public IActionResult Index(string tipoMoto = "Todas")
    {
        var lista = DatosClub.Miembros;

        if (tipoMoto != "Todas")
        {
            lista = lista.Where(m => m.TipoMoto == tipoMoto).ToList();
        }

        ViewBag.Filtro = tipoMoto;
        ViewBag.TiposMoto = DatosClub.TiposMoto;
        ViewBag.TotalCuotas = lista.Sum(m => m.CuotaMensual);
        return View(lista);
    }

    // GET: /Miembros/Inscripcion -> muestra el formulario vacio
    [HttpGet]
    public IActionResult Inscripcion()
    {
        CargarListas();
        var nuevo = new Miembro { FechaIngreso = DateTime.Today };
        return View(nuevo);
    }

    // POST: /Miembros/Inscripcion -> recibe los datos del formulario
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Inscripcion(Miembro miembro)
    {
        // Validaciones extra que no se pueden hacer solo con los atributos del modelo
        if (!DatosClub.TiposMoto.Contains(miembro.TipoMoto))
        {
            ModelState.AddModelError("TipoMoto", "Tipo de moto no válido");
        }

        if (!DatosClub.Planes.ContainsKey(miembro.Plan))
        {
            ModelState.AddModelError("Plan", "Plan no válido");
        }

        if (DatosClub.Miembros.Any(m => m.Correo.ToLower() == miembro.Correo?.ToLower()))
        {
            ModelState.AddModelError("Correo", "Ya existe un rider inscrito con ese correo");
        }

        if (miembro.FechaIngreso < DateTime.Today)
        {
            ModelState.AddModelError("FechaIngreso", "La fecha de ingreso no puede ser en el pasado");
        }

        // Si algo esta mal, volvemos a mostrar el formulario con los errores
        if (!ModelState.IsValid)
        {
            CargarListas();
            return View(miembro);
        }

        // Procesamos la informacion
        miembro.Nombre = miembro.Nombre.Trim();
        miembro.CuotaMensual = CalcularCuota(miembro.Plan, miembro.Cilindraje ?? 0);
        miembro.Id = DatosClub.Miembros.Max(m => m.Id) + 1;
        DatosClub.Miembros.Add(miembro);

        return RedirectToAction("Confirmacion", new { id = miembro.Id });
    }

    // GET: /Miembros/Confirmacion/4 -> resumen de la inscripcion
    [HttpGet]
    public IActionResult Confirmacion(int id)
    {
        var miembro = DatosClub.Miembros.FirstOrDefault(m => m.Id == id);

        if (miembro == null)
        {
            return NotFound();
        }

        return View(miembro);
    }

    // Precio del plan + 10.000 de recargo si la moto es de alto cilindraje (mas de 600 cc)
    private decimal CalcularCuota(string plan, int cilindraje)
    {
        decimal cuota = DatosClub.Planes[plan];

        if (cilindraje > 600)
        {
            cuota += 10000;
        }

        return cuota;
    }

    private void CargarListas()
    {
        ViewBag.TiposMoto = DatosClub.TiposMoto;
        ViewBag.Planes = DatosClub.Planes;
    }
}
