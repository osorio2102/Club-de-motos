using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClubMotociclismo.Data;
using ClubMotociclismo.Models;

namespace ClubMotociclismo.Controllers;

public class HomeController : Controller
{
    private readonly ClubContext _context;

    public HomeController(ClubContext context)
    {
        _context = context;
    }

    // GET: /Home/Index -> pagina principal con los eventos
    public async Task<IActionResult> Index()
    {
        // Solo mostramos los eventos que no han pasado, ordenados por fecha
        var proximos = await _context.Eventos
            .Where(e => e.Fecha >= DateTime.Today)
            .OrderBy(e => e.Fecha)
            .ToListAsync();

        ViewBag.TotalMiembros = await _context.Miembros.CountAsync();
        ViewBag.TotalPlanes = await _context.Planes.CountAsync();
        return View(proximos);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
