
/* AuthController.Cs */


using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Petrosia.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

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
                Console.WriteLine(" ModelState is INVALID. Errors:");
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        Console.WriteLine($" Field: {state.Key}, Error: {error.ErrorMessage}");
                    }
                }
                return View(newGuest);
            }

            Console.WriteLine(" Model validation passed!");

            newGuest.Password = _passwordHasher.HashPassword(newGuest, newGuest.Password);
            Console.WriteLine($" Hashed Password: {newGuest.Password}");
            _context.Guests.Add(newGuest);

            try
            {
                await _context.SaveChangesAsync();
                Console.WriteLine(" User added successfully: " + newGuest.Email);
            }
            catch (Exception ex)
            {
                Console.WriteLine(" Error saving user: " + ex.Message);
            }

            return RedirectToAction("SignIn");
        }




        [HttpGet("SignIn")]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(string email, string password)
        {
            Console.WriteLine($" Login Attempt: Email={email}");

            var guest = _context.Guests.FirstOrDefault(u => u.Email == email);
            var admin = _context.Admins.FirstOrDefault(a => a.Email == email);

            if (guest == null && admin == null)
            {
                Console.WriteLine(" User not found.");
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return View();
            }

            // Log the stored hashed password
            var passwordHasher = new PasswordHasher<object>(); // Generic type to handle both Admin and Guest
            PasswordVerificationResult result;

            string storedPassword;
            string userRole;

            if (guest != null)
            {
                storedPassword = guest.Password;
                result = passwordHasher.VerifyHashedPassword(guest, storedPassword, password);
                userRole = "Guest";
            }
            else
            {
                storedPassword = admin.Password;
                result = passwordHasher.VerifyHashedPassword(admin, storedPassword, password);
                userRole = "Admin";
            }

            Console.WriteLine($" Stored Hashed Password: {storedPassword}");
            Console.WriteLine($" Entered Password: {password}");
            Console.WriteLine($" Password Verification Result: {result}");

            if (result == PasswordVerificationResult.Failed)
            {
                Console.WriteLine(" Incorrect password.");
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return View();
            }

            Console.WriteLine(" Login successful!");

            HttpContext.Session.SetString("UserEmail", email);
            HttpContext.Session.SetString("UserRole", userRole);
            HttpContext.Session.SetInt32("UserId", guest != null ? guest.GuestId : admin.AdminId);

            //admin token
            //var guest = _context.Guests.FirstOrDefault(g => g.Email == login.Email);
            //var admin = _context.Admins.FirstOrDefault(a => a.Email == login.Email);

            if (guest != null)
            {
                var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, guest.Email),
        new Claim(ClaimTypes.Role, "Guest") // Assign "Guest" role
    };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties();

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return RedirectToAction("Index", "Home");
            }
            else if (admin != null)
            {
                var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, admin.Email),
        new Claim(ClaimTypes.Role, "Admin") // Assign "Admin" role
    };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties();

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return RedirectToAction("Admin", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return View();
            }



            //return userRole == "Admin" ? RedirectToAction("Admin", "Home") : RedirectToAction("Dashboard", "Home");
        }

        [HttpGet("Logout")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet("AccessDenied")]
        public IActionResult AccessDenied()
        {
            return View();
        }


    }
}
