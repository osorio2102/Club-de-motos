using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClubMotociclismo.Data;
using ClubMotociclismo.Models;

namespace ClubMotociclismo.Controllers;

// CRUD de miembros (riders) guardados en SQL Server con Entity Framework Core
public class MiembrosController : Controller
{
    private readonly ClubContext _context;

    // El DbContext llega por inyección de dependencias (registrado en Program.cs)
    public MiembrosController(ClubContext context)
    {
        _context = context;
    }

    // READ: GET /Miembros?tipoMoto=Naked&planId=2 -> lista con filtros
    [HttpGet]
    public async Task<IActionResult> Index(string tipoMoto = "Todas", int? planId = null)
    {
        // Include trae también los datos del Plan relacionado (JOIN)
        var consulta = _context.Miembros
            .Include(m => m.Plan)
            .AsQueryable();

        if (tipoMoto != "Todas")
        {
            consulta = consulta.Where(m => m.TipoMoto == tipoMoto);
        }

        if (planId.HasValue)
        {
            consulta = consulta.Where(m => m.PlanId == planId);
        }

        var lista = await consulta.OrderBy(m => m.Nombre).ToListAsync();

        ViewBag.Filtro = tipoMoto;
        ViewBag.PlanFiltro = planId;
        ViewBag.TiposMoto = DatosClub.TiposMoto;
        ViewBag.Planes = await _context.Planes.OrderBy(p => p.CuotaMensual).ToListAsync();
        ViewBag.TotalCuotas = lista.Sum(m => m.CuotaMensual);
        return View(lista);
    }

    // READ: GET /Miembros/Detalles/3
    [HttpGet]
    public async Task<IActionResult> Detalles(int id)
    {
        var miembro = await _context.Miembros
            .Include(m => m.Plan)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (miembro == null)
        {
            return NotFound();
        }

        return View(miembro);
    }

    // CREATE: GET /Miembros/Inscripcion -> formulario vacío
    [HttpGet]
    public async Task<IActionResult> Inscripcion()
    {
        await CargarListas();
        return View(new Miembro { FechaIngreso = DateTime.Today });
    }

    // CREATE: POST /Miembros/Inscripcion -> guarda en la base de datos
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Inscripcion(
        [Bind("Nombre,Correo,Telefono,Edad,FechaIngreso,TipoMoto,Cilindraje,Placa,PlanId")] Miembro miembro)
    {
        if (miembro.FechaIngreso < DateTime.Today)
        {
            ModelState.AddModelError("FechaIngreso", "La fecha de ingreso no puede ser en el pasado");
        }

        await ValidarMiembro(miembro);

        if (!ModelState.IsValid)
        {
            await CargarListas();
            return View(miembro);
        }

        var plan = await _context.Planes.FindAsync(miembro.PlanId);
        miembro.Nombre = miembro.Nombre.Trim();
        miembro.Correo = miembro.Correo.Trim();
        miembro.Placa = miembro.Placa?.Trim().ToUpper();
        miembro.CuotaMensual = CalcularCuota(plan!, miembro.Cilindraje ?? 0);

        _context.Miembros.Add(miembro);
        await _context.SaveChangesAsync(); // INSERT en SQL Server

        return RedirectToAction(nameof(Confirmacion), new { id = miembro.Id });
    }

    // GET: /Miembros/Confirmacion/4 -> resumen de la inscripción
    [HttpGet]
    public async Task<IActionResult> Confirmacion(int id)
    {
        var miembro = await _context.Miembros
            .Include(m => m.Plan)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (miembro == null)
        {
            return NotFound();
        }

        return View(miembro);
    }

    // UPDATE: GET /Miembros/Editar/3 -> formulario con los datos actuales
    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        var miembro = await _context.Miembros.FindAsync(id);

        if (miembro == null)
        {
            return NotFound();
        }

        await CargarListas();
        return View(miembro);
    }

    // UPDATE: POST /Miembros/Editar/3 -> actualiza el registro
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id,
        [Bind("Id,Nombre,Correo,Telefono,Edad,FechaIngreso,TipoMoto,Cilindraje,Placa,PlanId")] Miembro datos)
    {
        if (id != datos.Id)
        {
            return NotFound();
        }

        var miembro = await _context.Miembros.FindAsync(id);
        if (miembro == null)
        {
            return NotFound();
        }

        await ValidarMiembro(datos);

        if (!ModelState.IsValid)
        {
            await CargarListas();
            return View(datos);
        }

        var plan = await _context.Planes.FindAsync(datos.PlanId);

        miembro.Nombre = datos.Nombre.Trim();
        miembro.Correo = datos.Correo.Trim();
        miembro.Telefono = datos.Telefono;
        miembro.Edad = datos.Edad;
        miembro.FechaIngreso = datos.FechaIngreso;
        miembro.TipoMoto = datos.TipoMoto;
        miembro.Cilindraje = datos.Cilindraje;
        miembro.Placa = datos.Placa?.Trim().ToUpper();
        miembro.PlanId = datos.PlanId;
        miembro.CuotaMensual = CalcularCuota(plan!, datos.Cilindraje ?? 0);

        await _context.SaveChangesAsync(); // UPDATE en SQL Server

        TempData["Mensaje"] = $"Se actualizaron los datos de {miembro.Nombre}.";
        return RedirectToAction(nameof(Index));
    }

    // DELETE: GET /Miembros/Eliminar/3 -> pide confirmación
    [HttpGet]
    public async Task<IActionResult> Eliminar(int id)
    {
        var miembro = await _context.Miembros
            .Include(m => m.Plan)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (miembro == null)
        {
            return NotFound();
        }

        return View(miembro);
    }

    // DELETE: POST /Miembros/Eliminar/3 -> borra el registro
    [HttpPost, ActionName("Eliminar")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EliminarConfirmado(int id)
    {
        var miembro = await _context.Miembros.FindAsync(id);

        if (miembro != null)
        {
            _context.Miembros.Remove(miembro);
            await _context.SaveChangesAsync(); // DELETE en SQL Server
            TempData["Mensaje"] = $"Se eliminó a {miembro.Nombre} del club.";
        }

        return RedirectToAction(nameof(Index));
    }

    // Validaciones que dependen de la base de datos
    private async Task ValidarMiembro(Miembro miembro)
    {
        if (!string.IsNullOrEmpty(miembro.TipoMoto) && !DatosClub.TiposMoto.Contains(miembro.TipoMoto))
        {
            ModelState.AddModelError("TipoMoto", "Tipo de moto no válido");
        }

        if (miembro.PlanId.HasValue && !await _context.Planes.AnyAsync(p => p.Id == miembro.PlanId))
        {
            ModelState.AddModelError("PlanId", "Plan no válido");
        }

        if (!string.IsNullOrWhiteSpace(miembro.Correo))
        {
            var correo = miembro.Correo.Trim().ToLower();
            bool repetido = await _context.Miembros
                .AnyAsync(m => m.Correo.ToLower() == correo && m.Id != miembro.Id);

            if (repetido)
            {
                ModelState.AddModelError("Correo", "Ya existe un rider inscrito con ese correo");
            }
        }
    }

    // Precio del plan + recargo si la moto es de alto cilindraje (más de 600 cc)
    private static decimal CalcularCuota(Plan plan, int cilindraje)
    {
        decimal cuota = plan.CuotaMensual;

        if (cilindraje > DatosClub.LimiteAltoCilindraje)
        {
            cuota += DatosClub.RecargoAltoCilindraje;
        }

        return cuota;
    }

    // Listas para los <select> del formulario (los planes salen de la tabla Planes)
    private async Task CargarListas()
    {
        ViewBag.TiposMoto = DatosClub.TiposMoto;
        ViewBag.Planes = await _context.Planes.OrderBy(p => p.CuotaMensual).ToListAsync();
    }
}
