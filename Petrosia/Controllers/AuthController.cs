using Microsoft.AspNetCore.Mvc;
using Petrosia.Models; // Adjust namespace if needed
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Petrosia.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManagement1Context _context; // Ensure this matches your DbContext

        public AuthController(UserManagement1Context context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(Guest newGuest)
        {
            if (!ModelState.IsValid)
            {
                return View(newGuest);
            }

            // Debugging output
            Console.WriteLine("Received SignUp Request for: " + newGuest.Email);

            // Check if email already exists
            if (_context.Guests.Any(u => u.Email == newGuest.Email))
            {
                Console.WriteLine("Email already exists: " + newGuest.Email);
                ModelState.AddModelError("Email", "Email is already registered.");
                return View(newGuest);
            }

            // Hash the password
            newGuest.Password = HashPassword(newGuest.Password);
            _context.Guests.Add(newGuest);

            try
            {
                await _context.SaveChangesAsync(); ;
                Console.WriteLine("User added successfully: " + newGuest.Email);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving user: " + ex.Message);
            }

            return RedirectToAction("SignIn");
        }


        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
    }
}
