using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Petrosia.Models
{
    public class Room
    {
        [Key]

        public int RoomId { get; set; }

        // ✅ Add RoomNumber property
        [Required]
        public int RoomNumber { get; set; }

        [Required]
        public string RoomType { get; set; } = string.Empty;

        [Required]
        public int Capacity { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string Status { get; set; } = "Available";

        // ✅ Add GuestId (Nullable in case the room is unassigned)
        public int? GuestId { get; set; }

        // ✅ Navigation property for Guest
        public virtual Guest? Guest { get; set; }

        // ✅ Navigation property for Bookings
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

        // ✅ Navigation property for HouseKeepings
        public virtual ICollection<HouseKeeping> HouseKeepings { get; set; } = new List<HouseKeeping>();
    }
}

