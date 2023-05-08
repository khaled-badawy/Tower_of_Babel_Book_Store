using book_store.Models;
using book_store.Repositry;
using book_store.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace book_store.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IReviewService revService;
        public ReviewController(IReviewService revService)
        {
            this.revService = revService;
        }
        [Authorize(Roles = "admin")]
        public IActionResult IndexAdmin()
        {
            if (revService.GetAll().Count > 0)
            {
                return View(revService.SummaryReviews());
            }
            else
            {
                return View("NoReviews");
            }
        }
        [HttpPost]
        public IActionResult SavingReview([Bind("textReview, rating")] Review rev)
        {


            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            string userId = userIdClaim.Value;
            revService.AddReview(rev, userId);
            return RedirectToAction("Index", "Home");
        }

    }
}
