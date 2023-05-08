using book_store.Models;

namespace book_store.ViewModels
{
    public class CategoryWithBooksViewModel
    {
        public List<Book> NewBooks { get; set; }
        public List<Book> BestBooks { get; set; }
        public List<Category> Categories { get; set; }
    }
}
