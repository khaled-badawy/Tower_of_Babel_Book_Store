using book_store.Models;
using Microsoft.AspNetCore.Mvc;

namespace book_store.Repositry
{
    public interface IReviewRepositry
    {
        public List<Review> GetAll();
        public void AddReview([Bind("textReview, rating")] Review rev, string id);
    }
}
