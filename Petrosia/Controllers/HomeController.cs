using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Petrosia.Models;

namespace Petrosia.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult About()
    {
        return View();
    }

    public IActionResult TouristSpot()
    {
        return View();
    }

    public IActionResult Gallery()
    {
        return View();
    }
}

