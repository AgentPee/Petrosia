using Petrosia.Models;

namespace Petrosia.Data
{
    public static class RoomSeeder
    {
        public static void SeedRooms(UserManagement1Context context)
        {
            if (context.Rooms.Any())
            {
                context.Rooms.RemoveRange(context.Rooms.ToList());
                context.SaveChanges();
            }

            var roomTypes = new List<(int RoomTypeId, string RoomType, decimal Price, int Capacity)>
            {
                (1, "Deluxe Single Room", 2500.00M, 1),
                (2, "Deluxe Double Room", 3200.00M, 2),
                (3, "Deluxe Twin Room", 3800.00M, 4),
                (4, "Junior Suite", 5500.00M, 4),
                (5, "Executive Suite", 7200.00M, 6)
            };

            var rooms = new List<Room>();

            foreach (var (roomTypeId, roomType, price, capacity) in roomTypes)
            {
                for (int i = 1; i <= 5; i++)
                {
                    rooms.Add(new Room
                    {
                        RoomId = roomTypeId * 100 + i, // Unique RoomId
                        RoomTypeId = roomTypeId,      // Shared RoomTypeId
                        RoomNumber = roomTypeId * 100 + i,
                        RoomType = roomType,
                        Capacity = capacity,
                        Price = price,
                        Status = "Available"
                    });
                }
            }

            context.Rooms.AddRange(rooms);
            context.SaveChanges();
        }
    }
}