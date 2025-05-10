using System.Collections.Generic;
using System.Linq;
using Petrosia.Models;

namespace Petrosia.Data
{
    public class ReviewRepository
    {
        private readonly UserManagement1Context _context;

        public ReviewRepository(UserManagement1Context context)
        {
            _context = context;
        }

        public List<Review> GetAll()
        {
            return _context.Reviews.OrderByDescending(r => r.SubmissionDate).ToList();
        }

        public void Add(Review review)
        {
            _context.Reviews.Add(review);
            _context.SaveChanges();
        }
    }
}