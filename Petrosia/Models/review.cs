using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Petrosia.Models
{
    public class Review
    {
        public int ReviewId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Stay date is required")]
        [Display(Name = "Date of Stay")]
        public DateTime StayDate { get; set; }

        [Required(ErrorMessage = "Rating is required")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }

        [Required(ErrorMessage = "Review title is required")]
        [StringLength(200, ErrorMessage = "Title cannot be longer than 200 characters")]
        [Display(Name = "Review Title")]
        public string ReviewTitle { get; set; }

        [Required(ErrorMessage = "Review content is required")]
        [StringLength(2000, ErrorMessage = "Review cannot be longer than 2000 characters")]
        [Display(Name = "Your Review")]
        public string ReviewText { get; set; }

        // Categories/tags that the user liked
        public string LikedCategories { get; set; }

        // Optional photo path
        public string PhotoPath { get; set; }

        // Date when the review was submitted
        public DateTime SubmissionDate { get; set; } = DateTime.Now;

        // Review status (for moderation)
        public string Status { get; set; } = "Pending";

        // For parsing the liked categories from form submission
        public List<string> GetLikedCategoriesAsList()
        {
            return string.IsNullOrEmpty(LikedCategories)
                ? new List<string>()
                : new List<string>(LikedCategories.Split(','));
        }

        public void SetLikedCategoriesFromList(List<string> categories)
        {
            LikedCategories = categories != null && categories.Count > 0
                ? string.Join(",", categories)
                : null;
        }
    }
}