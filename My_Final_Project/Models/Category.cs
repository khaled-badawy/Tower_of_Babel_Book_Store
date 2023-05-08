using My_Final_Project.Models;

namespace book_store.Models
{
    public class Category
    {
        public int Id { get; set; }
        [CategoryUniqueName]
        public string Name { get; set; }
        public string? Image { get; set; } 
        public string? Description { get; set; }
        public List<Book>? Books { get; set; }

        public List<OrderedItem>? items { get; set; }
    }
}
