using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Petrosia.Models; // Adjust namespace if needed
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Security.Cryptography;

namespace Petrosia.Controllers
{
    [Route("Auth")] // Base Route
    public class AuthController : Controller
    {
        private readonly UserManagement1Context _context;
        private readonly ILogger<AuthController> _logger;
        private readonly PasswordHasher<Guest> _passwordHasher = new PasswordHasher<Guest>();

        public AuthController(UserManagement1Context context, ILogger<AuthController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("SignUp")]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUpManual()
        {
            Console.WriteLine("Sign up started");

            var form = Request.Form;

            Console.WriteLine($"Raw Form Data: {string.Join(", ", form.Keys.Select(k => $"{k}={form[k]}"))}");

            Guest newGuest = new Guest
            {
                FirstName = form["FirstName"],
                LastName = form["LastName"],
                Email = form["Email"],
                PhoneNumber = form["PhoneNumber"],
                Address = form["Address"],
                Password = form["Password"]
            };

            Console.WriteLine($"Extracted Data: " +
                $"FirstName={newGuest.FirstName}, " +
                $"LastName={newGuest.LastName}, " +
                $"Email={newGuest.Email}, " +
                $"Phone={newGuest.PhoneNumber}, " +
                $"Address={newGuest.Address}, " +
                $"Password={newGuest.Password}"
            );

            if (!ModelState.IsValid)
            {
                Console.WriteLine("❌ ModelState is INVALID. Errors:");
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        Console.WriteLine($"⚠️ Field: {state.Key}, Error: {error.ErrorMessage}");
                    }
                }
                return View(newGuest);
            }

            Console.WriteLine("✅ Model validation passed!");

            newGuest.Password = HashPassword(newGuest.Password);
            _context.Guests.Add(newGuest);

            try
            {
                await _context.SaveChangesAsync();
                Console.WriteLine("🎉 User added successfully: " + newGuest.Email);
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Error saving user: " + ex.Message);
            }

            return RedirectToAction("SignIn");
        }

        // ✅ Fix: Define HashPassword method here
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        [HttpGet("SignIn")]
        public IActionResult SignIn()
        {
            return View();
        }
    }
}
