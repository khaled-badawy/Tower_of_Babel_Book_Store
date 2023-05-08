using book_store.Models;
using Microsoft.AspNetCore.Mvc;
using My_Final_Project.Models;

namespace book_store.Repositry
{
    public class ReviewRepositry:IReviewRepositry
    {
        private readonly StoreContext StoreContext;
        public ReviewRepositry(StoreContext storeContext)
        {
            StoreContext = storeContext;
        }

        public List<Review> GetAll()
        {
            List<Review> allReviews = StoreContext.Reviews.ToList();
            return allReviews;
        }

        public void AddReview([Bind("textReview, rating")] Review rev, string id)
        {

            rev.userId = id;

            StoreContext.Reviews.Add(rev);
            StoreContext.SaveChanges();
        }
    }
}
