/*Booking.cs*/


using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Petrosia.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        [Required]
        [ForeignKey("Guest")]
        public int GuestId { get; set; }

        [Required]
        [ForeignKey("Room")]
        public int RoomId { get; set; }

        [Required]
        public string RoomType { get; set; }

        [Required]
        [Range(1, 10)]
        public int Adults { get; set; }

        [Range(0, 5)]
        public int Children { get; set; }

        [Required]
        public DateTime CheckInDate { get; set; }

        [Required]
        public DateTime CheckOutDate { get; set; }

        public virtual Guest Guest { get; set; }
        public virtual Room Room { get; set; }
    }
}
