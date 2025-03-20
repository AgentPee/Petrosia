using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
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

    [Authorize(Roles = "Admin")]
    public IActionResult Admin() 
    {
        var allAdmins = _context.Admins.ToList();
        return View(allAdmins);
    }




    [Authorize(Roles = "Admin")]
    public IActionResult Guest()
    {
        var allGuests = _context.Guests.ToList();
        return View(allGuests);
    }

    
    public IActionResult Dashboard() 
    {
        return View();
    }

}

