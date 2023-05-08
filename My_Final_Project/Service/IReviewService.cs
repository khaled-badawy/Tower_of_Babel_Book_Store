using book_store.Models;
using book_store.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace book_store.Service
{
    public interface IReviewService
    {
        public List<Review> GetAll();
        public ReviewAdmin SummaryReviews();
        public void AddReview([Bind("textReview, rating")] Review rev, string id);
    }
}
