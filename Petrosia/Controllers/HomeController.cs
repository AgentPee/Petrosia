using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

    //guest edit,create
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public IActionResult EditGuest(int id)
    {
        var guest = _context.Guests.FirstOrDefault(g => g.GuestId == id);
        if (guest == null) return NotFound();

        Console.WriteLine($"Selected GUestId: {guest.GuestId}");
        return View(guest);
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public IActionResult EditGuest(Guest model)
    {
        if (model == null || model.GuestId == 0)
        {
            return BadRequest("Invalid guest ID");
        }

        var guest = _context.Guests.FirstOrDefault(g => g.GuestId == model.GuestId);
        if (guest == null)
        {
            return NotFound();
        }

        // Update guest details
        guest.FirstName = model.FirstName;
        guest.LastName = model.LastName;
        guest.Email = model.Email;
        guest.PhoneNumber = model.PhoneNumber;
        guest.Address = model.Address;

        _context.SaveChanges();

        return RedirectToAction("Guest");
    }




    [Authorize(Roles = "Admin")]
    [HttpPost]
    public IActionResult DeleteGuest(int id)
    {
        var guest = _context.Guests.Find(id);
        if (guest == null)
        {
            return NotFound();
        }

        _context.Guests.Remove(guest);
        int rowsAffected = _context.SaveChanges();
        Console.WriteLine($"Rows Affected: {rowsAffected}");
        return RedirectToAction("Guest");
    }


    //admin edit, create
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public IActionResult EditAdmin(int id)
    {
        var admin = _context.Admins.FirstOrDefault(a => a.AdminId == id);
        if (admin == null) return NotFound();

        Console.WriteLine($"Selected AdminId: {admin.AdminId}");
        return View(admin);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public IActionResult EditAdmin(Admin model)
    {
        if (model == null || model.AdminId == 0)
        {
            return BadRequest("Invalid admin ID");
        }

        var admin = _context.Admins.FirstOrDefault(a => a.AdminId == model.AdminId);
        if (admin == null)
        {
            return NotFound();
        }

        // Update admin details
        admin.FirstName = model.FirstName;
        admin.LastName = model.LastName;
        admin.Email = model.Email;
        admin.PhoneNumber = model.PhoneNumber;
        //admin.Role = model.Role;

        _context.SaveChanges();

        return RedirectToAction("Guest");
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public IActionResult DeleteAdmin(int id)
    {
        var admin = _context.Admins.Find(id);
        if (admin == null)
        {
            return NotFound();
        }

        _context.Admins.Remove(admin);
        int rowsAffected = _context.SaveChanges();
        Console.WriteLine($"Rows Affected: {rowsAffected}");
        return RedirectToAction("Guest");
    }


}

