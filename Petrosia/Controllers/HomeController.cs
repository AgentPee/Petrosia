using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Petrosia.Models;

namespace Petrosia.Controllers;

public class HomeController : Controller
{
    private readonly UserManagement1Context _context;

    public HomeController(UserManagement1Context context)
    {
        _context = context;
    }
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

    public IActionResult Admin() 
    {
        var allAdmins = _context.Admins.ToList();
        return View(allAdmins);
    }

    public IActionResult SignUp()
    {
        return View();
    }
   
}

