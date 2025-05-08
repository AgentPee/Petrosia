/* HomeController.Cs */

using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Petrosia.Models;
using Petrosia.Data;
using Microsoft.EntityFrameworkCore;

namespace Petrosia.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManagement1Context _context;

        public HomeController(UserManagement1Context context)
        {
            _context = context;
        }

        public IActionResult Index() => View();
        public IActionResult Booking() => View();
        public IActionResult About() => View();
        public IActionResult TouristSpot() => View();
        public IActionResult Accommodation() => View();
        public IActionResult RoomDetails(string roomType) => View(roomType);

        // ========================== Admin Dashboard ========================== //
        [Authorize(Roles = "Admin")]
        public IActionResult Admin() => View(_context.Admins.ToList());

        [Authorize(Roles = "Admin")]
        public IActionResult Guest() => View(_context.Guests.ToList());

        // ========================== Reports & Analytics ========================== //
        [Authorize(Roles = "Admin")]
        public IActionResult Raa()
        {
            // Get counts for dashboard stats
            int totalRooms = _context.Rooms.Count();
            int availableRooms = _context.Rooms.Count(r => r.Status == "Available");
            int occupiedRooms = _context.Rooms.Count(r => r.Status == "Used" || r.Status == "Booked");
            int totalGuests = _context.Guests.Count();
            int totalBookings = _context.Bookings.Count();

            // Calculate occupancy percentage
            double occupancyRate = totalRooms > 0 ? (double)occupiedRooms / totalRooms * 100 : 0;

            // Calculate total revenue (simplified version)
            decimal totalRevenue = 0;
            var bookings = _context.Bookings.Include(b => b.Room).ToList();
            foreach (var booking in bookings)
            {
                if (booking.Room != null)
                {
                    int stayDays = (booking.CheckOutDate - booking.CheckInDate).Days;
                    if (stayDays <= 0) stayDays = 1; // Minimum one day
                    totalRevenue += booking.Room.Price * stayDays;
                }
            }

            // Calculate average stay
            double averageStay = 0;
            if (bookings.Count > 0)
            {
                averageStay = bookings.Average(b => (b.CheckOutDate - b.CheckInDate).Days);
                if (averageStay <= 0) averageStay = 1; // Ensure minimum of 1 day
            }

            // Get recent bookings for table display
            var recentBookings = _context.Bookings
                .Include(b => b.Guest)
                .Include(b => b.Room)
                .OrderByDescending(b => b.BookingId)
                .Take(5)
                .ToList();

            // Pass data to view using ViewBag
            ViewBag.TotalRooms = totalRooms;
            ViewBag.AvailableRooms = availableRooms;
            ViewBag.OccupiedRooms = occupiedRooms;
            ViewBag.OccupancyRate = occupancyRate;
            ViewBag.TotalGuests = totalGuests;
            ViewBag.TotalBookings = totalBookings;
            ViewBag.TotalRevenue = totalRevenue;
            ViewBag.AverageStay = averageStay;
            ViewBag.RecentBookings = recentBookings;

            return View();
        }

        // ========================== Guest Management ========================== //
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult EditGuest(int id)
        {
            var guest = _context.Guests.Find(id);
            return guest != null ? View(guest) : NotFound();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult EditGuest(Guest model)
        {
            if (model == null || model.GuestId == 0) return BadRequest("Invalid guest ID");

            var guest = _context.Guests.Find(model.GuestId);
            if (guest == null) return NotFound();

            guest.FirstName = model.FirstName;
            guest.LastName = model.LastName;
            guest.Email = model.Email;
            guest.PhoneNumber = model.PhoneNumber;
            guest.Address = model.Address;

            _context.SaveChanges();
            TempData["SuccessMessage"] = "Guest details updated successfully.";
            return RedirectToAction("Guest");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult DeleteGuest(int id)
        {
            var guest = _context.Guests.Find(id);
            if (guest == null) return NotFound();

            _context.Guests.Remove(guest);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Guest deleted successfully.";
            return RedirectToAction("Guest");
        }

        // ========================== Admin Management ========================== //
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult EditAdmin(int id)
        {
            var admin = _context.Admins.Find(id);
            return admin != null ? View(admin) : NotFound();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult EditAdmin(Admin model)
        {
            if (model == null || model.AdminId == 0) return BadRequest("Invalid admin ID");

            var admin = _context.Admins.Find(model.AdminId);
            if (admin == null) return NotFound();

            admin.FirstName = model.FirstName;
            admin.LastName = model.LastName;
            admin.Email = model.Email;
            admin.PhoneNumber = model.PhoneNumber;

            _context.SaveChanges();
            TempData["SuccessMessage"] = "Admin details updated successfully.";
            return RedirectToAction("Admin");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult DeleteAdmin(int id)
        {
            var admin = _context.Admins.Find(id);
            if (admin == null) return NotFound();

            _context.Admins.Remove(admin);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Admin deleted successfully.";
            return RedirectToAction("Admin");
        }

        // ========================== Booking Management ========================== //
        [HttpPost]
        public IActionResult BookRoom([FromBody] Booking booking)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return Json(new { error = "Please log in to make a booking." });
                }

                var availableRoom = _context.Rooms
                    .Where(r => r.RoomType == booking.RoomType && r.Status == "Available")
                    .FirstOrDefault();

                if (availableRoom == null)
                {
                    return Json(new { error = $"No {booking.RoomType}s available." });
                }

                var guest = _context.Guests.FirstOrDefault(g => g.Email == User.Identity.Name);

                if (guest == null)
                {
                    return Json(new { error = "Guest profile not found." });
                }

                booking.GuestId = guest.GuestId;
                booking.RoomId = availableRoom.RoomId;

                availableRoom.Status = "Booked";
                _context.Bookings.Add(booking);
                _context.SaveChanges();

                return Json(new
                {
                    success = true,
                    message = "Booking successful!",
                    bookingId = booking.BookingId,
                    roomNumber = availableRoom.RoomNumber
                });
            }
            catch (Exception ex)
            {
                return Json(new { error = $"Booking failed: {ex.Message}" });
            }
        }

        // ========================== Reviews Management ========================== //
        [HttpPost]
        public IActionResult SubmitReview(string name, string email, int rating, string review)
        {
            // In a real application, you would save this to the database
            // For now, we'll just display a success message
            TempData["SuccessMessage"] = "Thank you for your review! It will be displayed after moderation.";

            // Redirect back to the About page
            return RedirectToAction("About");
        }

        // ========================== Utility Methods ========================== //
        [HttpGet]
        public IActionResult TestDatabaseConnection()
        {
            try
            {
                bool isConnected = _context.Database.CanConnect();
                return Json(new
                {
                    connected = isConnected,
                    message = isConnected ? "Database connection successful" : "Database connection failed"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    connected = false,
                    error = ex.Message
                });
            }
        }

        [HttpGet]
        public IActionResult SeedRooms()
        {
            try
            {
                RoomSeeder.SeedRooms(_context);
                return Json(new { success = true, message = "Rooms seeded successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }
    }

}

