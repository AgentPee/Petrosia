using System;

namespace Petrosia.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime StayDate { get; set; }
        public int Rating { get; set; }
        public string ReviewTitle { get; set; }
        public string ReviewText { get; set; }
        public string LikedCategories { get; set; }
        public DateTime SubmissionDate { get; set; }
    }
}