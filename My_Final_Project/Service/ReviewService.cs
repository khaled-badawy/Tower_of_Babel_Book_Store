using book_store.Models;
using book_store.Repositry;
using book_store.ViewModels;
using Microsoft.AspNetCore.Mvc;
using My_Final_Project.Models;

namespace book_store.Service
{
    public class ReviewService:IReviewService
    {
        private readonly StoreContext StoreContext;
        private readonly IReviewRepositry revRepo;
        public ReviewService(StoreContext storeContext, IReviewRepositry revRepo)
        {
            StoreContext = storeContext;
            this.revRepo = revRepo;
        }
        public List<Review> GetAll()
        {
            return revRepo.GetAll();
        }
        public ReviewAdmin SummaryReviews()
        {
            List<Review> reviews = GetAll();
            ReviewAdmin sumReviews = new ReviewAdmin();
            sumReviews.allReviews = GetAll().Count;
            sumReviews.avgRatings = Math.Round(GetAll().Average(r => r.rating), 2);
            sumReviews.oneStarRate = Math.Round((GetAll().Where(r => r.rating == 1).Count() / sumReviews.allReviews) * 100, 1);
            sumReviews.twoStarRate = Math.Round((GetAll().Where(r => r.rating == 2).Count() / sumReviews.allReviews) * 100, 1);
            sumReviews.threeStarRate = Math.Round((GetAll().Where(r => r.rating == 3).Count() / sumReviews.allReviews) * 100, 1);
            sumReviews.fourStarRate = Math.Round((GetAll().Where(r => r.rating == 4).Count() / sumReviews.allReviews) * 100, 1);
            sumReviews.fiveStarRate = Math.Round((GetAll().Where(r => r.rating == 5).Count() / sumReviews.allReviews) * 100, 1);

            sumReviews.reviews = reviews;
            return sumReviews;
        }
        public void AddReview([Bind("textReview, rating")] Review rev, string id)
        {
            revRepo.AddReview(rev, id);
        }
    }
}
