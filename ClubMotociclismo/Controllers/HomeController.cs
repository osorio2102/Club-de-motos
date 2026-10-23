using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ClubMotociclismo.Models;

namespace ClubMotociclismo.Controllers;

public class HomeController : Controller
{
    // GET: /Home/Index -> pagina principal con los eventos
    public IActionResult Index()
    {
        // Solo mostramos los eventos que no han pasado, ordenados por fecha
        var proximos = DatosClub.Eventos
            .Where(e => e.Fecha >= DateTime.Today)
            .OrderBy(e => e.Fecha)
            .ToList();

        ViewBag.TotalMiembros = DatosClub.Miembros.Count;
        return View(proximos);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
