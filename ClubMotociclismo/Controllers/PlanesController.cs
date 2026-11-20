using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClubMotociclismo.Data;
using ClubMotociclismo.Models;

namespace ClubMotociclismo.Controllers;

// CRUD de los planes de membresía (lado "uno" de la relación Plan 1:N Miembro)
public class PlanesController : Controller
{
    private readonly ClubContext _context;

    public PlanesController(ClubContext context)
    {
        _context = context;
    }

    // READ: GET /Planes -> lista de planes con cuántos miembros tiene cada uno
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var planes = await _context.Planes
            .Include(p => p.Miembros)
            .OrderBy(p => p.CuotaMensual)
            .ToListAsync();

        return View(planes);
    }

    // READ: GET /Planes/Detalles/2 -> el plan y sus miembros
    [HttpGet]
    public async Task<IActionResult> Detalles(int id)
    {
        var plan = await _context.Planes
            .Include(p => p.Miembros.OrderBy(m => m.Nombre))
            .FirstOrDefaultAsync(p => p.Id == id);

        if (plan == null)
        {
            return NotFound();
        }

        return View(plan);
    }

    // CREATE: GET /Planes/Crear
    [HttpGet]
    public IActionResult Crear()
    {
        return View(new Plan());
    }

    // CREATE: POST /Planes/Crear
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear([Bind("Nombre,CuotaMensual,Descripcion")] Plan plan)
    {
        await ValidarNombre(plan);

        if (!ModelState.IsValid)
        {
            return View(plan);
        }

        plan.Nombre = plan.Nombre.Trim();
        _context.Planes.Add(plan);
        await _context.SaveChangesAsync();

        TempData["Mensaje"] = $"Se creó el plan {plan.Nombre}.";
        return RedirectToAction(nameof(Index));
    }

    // UPDATE: GET /Planes/Editar/2
    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        var plan = await _context.Planes.FindAsync(id);

        if (plan == null)
        {
            return NotFound();
        }

        return View(plan);
    }

    // UPDATE: POST /Planes/Editar/2
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, [Bind("Id,Nombre,CuotaMensual,Descripcion")] Plan datos)
    {
        if (id != datos.Id)
        {
            return NotFound();
        }

        var plan = await _context.Planes
            .Include(p => p.Miembros)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (plan == null)
        {
            return NotFound();
        }

        await ValidarNombre(datos);

        if (!ModelState.IsValid)
        {
            return View(datos);
        }

        plan.Nombre = datos.Nombre.Trim();
        plan.Descripcion = datos.Descripcion;
        plan.CuotaMensual = datos.CuotaMensual;

        // Si cambia el precio, se recalcula la cuota de los miembros de este plan
        foreach (var miembro in plan.Miembros)
        {
            miembro.CuotaMensual = plan.CuotaMensual +
                (miembro.Cilindraje > DatosClub.LimiteAltoCilindraje ? DatosClub.RecargoAltoCilindraje : 0);
        }

        await _context.SaveChangesAsync();

        TempData["Mensaje"] = $"Se actualizó el plan {plan.Nombre}.";
        return RedirectToAction(nameof(Index));
    }

    // DELETE: GET /Planes/Eliminar/2
    [HttpGet]
    public async Task<IActionResult> Eliminar(int id)
    {
        var plan = await _context.Planes
            .Include(p => p.Miembros)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (plan == null)
        {
            return NotFound();
        }

        return View(plan);
    }

    // DELETE: POST /Planes/Eliminar/2
    [HttpPost, ActionName("Eliminar")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EliminarConfirmado(int id)
    {
        var plan = await _context.Planes
            .Include(p => p.Miembros)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (plan == null)
        {
            return RedirectToAction(nameof(Index));
        }

        // La clave foránea es Restrict: no dejamos huérfanos a los miembros
        if (plan.Miembros.Any())
        {
            TempData["Error"] = $"No se puede eliminar el plan {plan.Nombre} porque tiene {plan.Miembros.Count} miembro(s). Cámbialos de plan primero.";
            return RedirectToAction(nameof(Index));
        }

        _context.Planes.Remove(plan);
        await _context.SaveChangesAsync();

        TempData["Mensaje"] = $"Se eliminó el plan {plan.Nombre}.";
        return RedirectToAction(nameof(Index));
    }

    private async Task ValidarNombre(Plan plan)
    {
        if (string.IsNullOrWhiteSpace(plan.Nombre))
        {
            return;
        }

        var nombre = plan.Nombre.Trim().ToLower();
        if (await _context.Planes.AnyAsync(p => p.Nombre.ToLower() == nombre && p.Id != plan.Id))
        {
            ModelState.AddModelError("Nombre", "Ya existe un plan con ese nombre");
        }
    }
}
