/*Booking.cs*/


using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Petrosia.Models
{
    [Table("bookings")]
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }


       


        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }


        public int? GuestId { get; set; }  // Nullable
        public int? RoomId { get; set; }


        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(200)]
        public string Address { get; set; }

        [Required]
        [StringLength(50)]
        public string City { get; set; }

        [Required]
        [StringLength(20)]
        public string ZipCode { get; set; }

        [Required]
        [StringLength(50)]
        public string Country { get; set; }

        [Required]
        [StringLength(50)]
        public string RoomType { get; set; }


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


       

        [Required]
        public int NumberOfAdults { get; set; }

        public int NumberOfChildren { get; set; }

        [Required]
        public int NumberOfNights { get; set; }

        [StringLength(500)]
        public string SpecialRequests { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalAmount { get; set; }

        [Required]
        [StringLength(50)]
        public string PaymentMethod { get; set; }

        [Required]
        public DateTime BookingDate { get; set; }

        [StringLength(20)]
        public string BookingReference { get; set; }

        [StringLength(20)]
        public string Status { get; set; } = "Confirmed";

    }
}