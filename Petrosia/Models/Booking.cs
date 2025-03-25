using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Petrosia.Models
{
    public class Booking
    {

        public int Id { get; set; }

        [Required]
        public string RoomType { get; set; }

        [Range(1, 10, ErrorMessage = "Adults must be between 1 and 10.")]
        public int Adults { get; set; }

        [Range(0, 5, ErrorMessage = "Children must be between 0 and 5.")]
        public int Children { get; set; }

        [Key]
        public int BookingId { get; set; }

        [Required]
        public int GuestId { get; set; }

        [Required]
        public int RoomId { get; set; }

        [Required]
        public DateTime CheckInDate { get; set; }

        [Required]
        public DateTime CheckOutDate { get; set; }

        // ✅ Fix: Ensure correct namespace for Room
        public virtual Guest Guest { get; set; }
        public virtual Room Room { get; set; }
    }
}
