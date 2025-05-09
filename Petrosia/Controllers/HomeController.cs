using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Petrosia.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

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

        // ========================== Reports & Analytics ========================== //
        [Authorize(Roles = "Admin")]
        public IActionResult Raa()
        {
            // Get counts for dashboard stats
            int totalRooms = _context.Rooms.Count();
            int availableRooms = _context.Rooms.Count(r => r.Status == "Available");
            int occupiedRooms = _context.Rooms.Count(r => r.Status == "Used" || r.Status == "Booked");
            int totalBookings = _context.Bookings.Count();

            // Calculate occupancy percentage
            double occupancyRate = totalRooms > 0 ? (double)occupiedRooms / totalRooms * 100 : 0;

            // Calculate total revenue
            decimal totalRevenue = 0;
            var bookings = _context.Bookings.ToList();

            foreach (var booking in bookings)
            {
                if (booking.RoomId != null)
                {
                    var room = _context.Rooms.FirstOrDefault(r => r.RoomId == booking.RoomId);
                    if (room != null)
                    {
                        int stayDays = (booking.CheckOutDate - booking.CheckInDate).Days;
                        if (stayDays <= 0) stayDays = 1; // Minimum one day
                        totalRevenue += room.Price * stayDays; // Use Room.Price
                    }
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
            var recentBookings = bookings
                .OrderByDescending(b => b.BookingId)
                .Take(5)
                .Select(b => new
                {
                    b.BookingId,
                    FullName = $"{b.FirstName} {b.LastName}",
                    b.Email,
                    b.PhoneNumber,
                    b.CheckInDate,
                    b.CheckOutDate,
                    b.Status,
                    b.TotalAmount,
                    Room = _context.Rooms.FirstOrDefault(r => r.RoomId == b.RoomId) // Fetch Room details
                })
                .ToList();

            // Pass data to view using ViewBag
            ViewBag.TotalRooms = totalRooms;
            ViewBag.AvailableRooms = availableRooms;
            ViewBag.OccupiedRooms = occupiedRooms;
            ViewBag.OccupancyRate = occupancyRate;
            ViewBag.TotalBookings = totalBookings;
            ViewBag.TotalRevenue = totalRevenue;
            ViewBag.AverageStay = averageStay;
            ViewBag.RecentBookings = recentBookings;

            return View();
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
                if (!ModelState.IsValid)
                {
                    // Log the specific validation errors
                    foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                    {
                        Console.WriteLine($"Validation error: {error.ErrorMessage}");
                    }
                    return BadRequest(ModelState);
                }

                // Generate a unique booking reference
                booking.BookingReference = "PH" + DateTime.Now.ToString("yyyyMMdd") + new Random().Next(1000, 9999);

                // Save to database
                _context.Bookings.Add(booking);
                _context.SaveChanges();

                return Ok("Booking successful! Reference: " + booking.BookingReference);
            }
            catch (Exception ex)
            {
                // Log the detailed exception
                Console.WriteLine($"Error in BookRoom: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");

                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }

                return StatusCode(500, "An error occurred: " + ex.Message);
            }
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

        // ========================== Room Inventory Management ========================== //
        public async Task<IActionResult> RoomInventory()
        {
            var rooms = await _context.Rooms.ToListAsync();
            return View(rooms);
        }

        [HttpGet]
        public IActionResult AddRoom()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRoom(Room room)
        {
            if (ModelState.IsValid)
            {
                _context.Add(room);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(RoomInventory));
            }
            return View(room);
        }

        [HttpGet]
        public async Task<IActionResult> EditRoom(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            return View(room);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRoom(int id, Room room)
        {
            if (id != room.RoomId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(room);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomExists(room.RoomId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(RoomInventory));
            }
            return View(room);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room != null)
            {
                _context.Rooms.Remove(room);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(RoomInventory));
        }

        private bool RoomExists(int id)
        {
            return _context.Rooms.Any(e => e.RoomId == id);
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
                int adminCount = _context.Admins.Count();

                // Check if Bookings table exists by trying to access it
                bool bookingsTableExists = true;
                try
                {
                    _context.Bookings.Count();
                }
                catch
                {
                    bookingsTableExists = false;
                }

                return Json(new
                {
                    success = isConnected,
                    message = $"Database connection successful. Found {adminCount} admins. Bookings table exists: {bookingsTableExists}"
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
    }
}