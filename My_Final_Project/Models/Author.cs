using My_Final_Project.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace book_store.Models
{
    public class Author
    {
        public int Id { get; set; }
        [AuthorUniqueName]
        public string Name { get; set; }
        public string? Image { get; set; }
        public string? BriefHistory { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Publish Count must be at least one")]
        public int PublishCount { get; set; }
        public List<Book>? Books { get; set; }

    }
}
