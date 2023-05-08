using My_Final_Project.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace book_store.Models
{
    public class Review
    {
            public int Id { get; set; }
            public string textReview { get; set; }
            public int rating { get; set; }

            [ForeignKey("user")]
            public string userId { get; set; }
            public ApplicationUser user { get; set; }
    }
}
