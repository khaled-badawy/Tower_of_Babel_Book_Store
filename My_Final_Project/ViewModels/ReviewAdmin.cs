using book_store.Models;

namespace book_store.ViewModels
{
    public class ReviewAdmin
    {
        public double avgRatings { get; set; }
        public double oneStarRate { get; set; }
        public double twoStarRate { get; set; }
        public double threeStarRate { get; set; }
        public double fourStarRate { get; set; }
        public double fiveStarRate { get; set; }
        public double allReviews { get; set; }
        public List<Review> reviews { get; set; }
    }
}
