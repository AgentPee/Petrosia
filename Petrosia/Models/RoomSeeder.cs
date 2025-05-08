using Petrosia.Models;

namespace Petrosia.Data
{
    public static class RoomSeeder
    {
        public static void SeedRooms(UserManagement1Context context)
        {
            // Check if rooms already exist
            if (context.Rooms.Any())
            {
                // Clear existing rooms and their bookings
                context.Bookings.RemoveRange(context.Bookings.ToList());
                context.Rooms.RemoveRange(context.Rooms.ToList());
                context.SaveChanges();
            }

            var rooms = new List<Room>
            {
                // Deluxe Single Rooms (5 rooms)
                new Room { RoomNumber = 101, RoomType = "Deluxe Single Room", Capacity = 1, Price = 2500.00M, Status = "Available" },
                new Room { RoomNumber = 102, RoomType = "Deluxe Single Room", Capacity = 1, Price = 2500.00M, Status = "Available" },
                new Room { RoomNumber = 103, RoomType = "Deluxe Single Room", Capacity = 1, Price = 2500.00M, Status = "Available" },
                new Room { RoomNumber = 104, RoomType = "Deluxe Single Room", Capacity = 1, Price = 2500.00M, Status = "Available" },
                new Room { RoomNumber = 105, RoomType = "Deluxe Single Room", Capacity = 1, Price = 2500.00M, Status = "Available" },

                // Deluxe Double Rooms (5 rooms)
                new Room { RoomNumber = 201, RoomType = "Deluxe Double Room", Capacity = 2, Price = 3200.00M, Status = "Available" },
                new Room { RoomNumber = 202, RoomType = "Deluxe Double Room", Capacity = 2, Price = 3200.00M, Status = "Available" },
                new Room { RoomNumber = 203, RoomType = "Deluxe Double Room", Capacity = 2, Price = 3200.00M, Status = "Available" },
                new Room { RoomNumber = 204, RoomType = "Deluxe Double Room", Capacity = 2, Price = 3200.00M, Status = "Available" },
                new Room { RoomNumber = 205, RoomType = "Deluxe Double Room", Capacity = 2, Price = 3200.00M, Status = "Available" },

                // Deluxe Twin Rooms (5 rooms)
                new Room { RoomNumber = 301, RoomType = "Deluxe Twin Room", Capacity = 4, Price = 3800.00M, Status = "Available" },
                new Room { RoomNumber = 302, RoomType = "Deluxe Twin Room", Capacity = 4, Price = 3800.00M, Status = "Available" },
                new Room { RoomNumber = 303, RoomType = "Deluxe Twin Room", Capacity = 4, Price = 3800.00M, Status = "Available" },
                new Room { RoomNumber = 304, RoomType = "Deluxe Twin Room", Capacity = 4, Price = 3800.00M, Status = "Available" },
                new Room { RoomNumber = 305, RoomType = "Deluxe Twin Room", Capacity = 4, Price = 3800.00M, Status = "Available" },

                // Junior Suite Rooms (5 rooms)
                new Room { RoomNumber = 401, RoomType = "Junior Suite", Capacity = 4, Price = 5500.00M, Status = "Available" },
                new Room { RoomNumber = 402, RoomType = "Junior Suite", Capacity = 4, Price = 5500.00M, Status = "Available" },
                new Room { RoomNumber = 403, RoomType = "Junior Suite", Capacity = 4, Price = 5500.00M, Status = "Available" },
                new Room { RoomNumber = 404, RoomType = "Junior Suite", Capacity = 4, Price = 5500.00M, Status = "Available" },
                new Room { RoomNumber = 405, RoomType = "Junior Suite", Capacity = 4, Price = 5500.00M, Status = "Available" },

                // Executive Suite Rooms (5 rooms)
                new Room { RoomNumber = 501, RoomType = "Executive Suite", Capacity = 6, Price = 7200.00M, Status = "Available" },
                new Room { RoomNumber = 502, RoomType = "Executive Suite", Capacity = 6, Price = 7200.00M, Status = "Available" },
                new Room { RoomNumber = 503, RoomType = "Executive Suite", Capacity = 6, Price = 7200.00M, Status = "Available" },
                new Room { RoomNumber = 504, RoomType = "Executive Suite", Capacity = 6, Price = 7200.00M, Status = "Available" },
                new Room { RoomNumber = 505, RoomType = "Executive Suite", Capacity = 6, Price = 7200.00M, Status = "Available" }
            };

            context.Rooms.AddRange(rooms);
            context.SaveChanges();
        }
    }
}