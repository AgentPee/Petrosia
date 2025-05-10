/* HomeController.Cs */

using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Petrosia.Models;
using Petrosia.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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
            try
            {
                // Fetch bookings directly from the Bookings table
                var bookings = _context.Bookings.ToList();

                ViewBag.TotalBookings = bookings.Count;
                ViewBag.TotalRevenue = bookings.Sum(b => b.TotalAmount);
                ViewBag.AverageStay = bookings.Any()
                    ? bookings.Average(b => (b.CheckOutDate - b.CheckInDate).Days)
                    : 0;

                ViewBag.RecentBookings = bookings
                    .OrderByDescending(b => b.BookingDate)
                    .Take(5)
                    .ToList();

                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Raa action: {ex.Message}");
                return StatusCode(500, "An error occurred while processing your request.");
            }
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
    if (!ModelState.IsValid)
    {
        foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
        {
            Console.WriteLine($"Validation error: {error.ErrorMessage}");
        }
        return BadRequest(ModelState);
    }

    Console.WriteLine($"Received booking: {JsonConvert.SerializeObject(booking)}");

    // Validate RoomTypeId
    if (booking.RoomTypeId <= 0)
    {
        return BadRequest(new { Error = "The RoomTypeId field is required and must be a valid integer." });
    }

    // Save booking
    _context.Bookings.Add(booking);
    _context.SaveChanges();

    return Ok("Booking successfully saved!");
}

        // ========================== View Bookings (Admin) ========================== //
        [Authorize(Roles = "Admin")]
        public IActionResult Bookings()
        {
            var bookings = _context.Bookings.ToList();
            return View(bookings);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult BookingDetails(int id)
        {
            var booking = _context.Bookings.Find(id);
            return booking != null ? View(booking) : NotFound();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult UpdateBookingStatus(int id, string status)
        {
            var booking = _context.Bookings.Find(id);
            if (booking == null) return NotFound();

            booking.Status = status;
            _context.SaveChanges();

            return RedirectToAction("BookingDetails", new { id = id });
        }


        // Testing booking (safe to remove)

        public IActionResult BookingTest()
        {
            // This action will serve the test page
            return View();
        }

        [HttpGet]
public IActionResult TestDatabaseConnection()
{
    try
    {
        // Test if we can access the database
        bool isConnected = _context.Database.CanConnect();

        // Get some basic stats to confirm tables exist
        int adminCount = _context.Admins.Count(); // Ensure Count() is invoked
        int guestCount = _context.Guests.Count(); // Ensure Count() is invoked

        // Check if Bookings table exists by trying to access it
        bool bookingsTableExists = true;
        try
        {
            _context.Bookings.Count(); // Ensure Count() is invoked
        }
        catch
        {
            bookingsTableExists = false;
        }

        return Json(new
        {
            success = isConnected,
            message = $"Database connection successful. Found {adminCount} admins and {guestCount} guests. Bookings table exists: {bookingsTableExists}"
        });
    }
    catch (Exception ex)
    {
        return Json(new
        {
            success = false,
            message = $"Error connecting to database: {ex.Message}"
        });
    }
}


        // ========================== Saving Booking ========================== //

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveBooking(Booking model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Validation error: {error.ErrorMessage}");
                }

                return BadRequest(ModelState);
            }

            _context.Bookings.Add(model);
            _context.SaveChanges();
            return Ok("Booking successfully saved!");
        }


        // ========================== Room Management ========================== //
        [Authorize(Roles = "Admin")]
        public IActionResult ManageRooms()
        {
            var rooms = _context.Rooms
                .Select(r => new
                {
                    r.RoomId,
                    r.RoomNumber,
                    r.RoomType,
                    r.Status,
                    GuestName = _context.Bookings
                        .Where(b => b.RoomTypeId == r.RoomTypeId && b.Status == "Checked-in")
                        .Select(b => b.FirstName + " " + b.LastName)
                        .FirstOrDefault()
                })
                .ToList();

            return View(rooms);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult UpdateRoomStatus(int roomId, string status)
        {
            var room = _context.Rooms.Find(roomId);
            if (room == null) return NotFound();

            room.Status = status;
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Room status updated successfully.";
            return RedirectToAction("ManageRooms");
        }

        // ========================== Review Management ========================== //
        [HttpGet]
        public IActionResult Far()
        {
            var reviews = _context.Reviews.OrderByDescending(r => r.SubmissionDate).ToList();
            ViewBag.Reviews = reviews;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitReview(Review review)
        {
            if (ModelState.IsValid)
            {
                review.SubmissionDate = DateTime.Now;
                _context.Reviews.Add(review);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Thank you for your review!";
                return RedirectToAction("Far");
            }

            // If invalid, re-fetch reviews and return to view
            ViewBag.Reviews = _context.Reviews.OrderByDescending(r => r.SubmissionDate).ToList();
            return View("Far", review);
        }

        




    }

}

