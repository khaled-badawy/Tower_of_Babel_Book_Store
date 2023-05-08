using book_store.Models;

namespace book_store.Repositry
{
    public interface IBookRepositry
    {
        public Book Get(int id);
        public List<Book> GetAll();
        public List<Book> GetAllNew();
        public List<Book> GetAllBest();
        public void Add(Book book);
        public void Update(Book book);
        public void SaveChanges();
        public void Delete(int id);
        public List<Book> GetAllInCategory(int id);
        public List<Book> PriceFilter(Double Price);
        public List<Book> AuthorFilter(string author);
    }
}
