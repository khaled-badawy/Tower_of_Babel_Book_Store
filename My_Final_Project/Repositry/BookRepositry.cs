using book_store.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using My_Final_Project.Models;

namespace book_store.Repositry
{
    public class BookRepositry:IBookRepositry
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly StoreContext StoreContext;
        public BookRepositry(StoreContext _storeContext, IWebHostEnvironment webHostEnvironment) 
        {
            StoreContext= _storeContext;
            _webHostEnvironment= webHostEnvironment;
        }
        public Book Get(int id)
        {
            var book = StoreContext.Books.Include(b => b.Author).AsNoTracking().FirstOrDefault(x => x.Id == id);
            return book;
        }
        public List<Book> GetAll()
        {
            var allBooks = StoreContext.Books.Include(b => b.Author).Include(b => b.Category).AsNoTracking().ToList();
            return allBooks;
        }
        public List<Book> GetAllInCategory(int id)
        {
            return StoreContext.Books.Include(b => b.Author).Include(b => b.Category).Where(b => b.CategoryId == id).AsNoTracking().ToList();
        }
        public List<Book> GetAllNew()
        {
            return StoreContext.Books.Include(b => b.Author).Include(b => b.Category).Where(b => (int)b.BookStatus == 1).AsNoTracking().ToList();
        }
        public List<Book> GetAllBest()
        {
            return StoreContext.Books.Include(b => b.Author).Include(b => b.Category).Where(b => (int)b.BookStatus == 2).AsNoTracking().ToList();
        }
        public List<Book> PriceFilter(Double Price)
        {
            Price = Math.Floor(Price);
            var filterdbooks = StoreContext.Books.Include(b => b.Author).Include(b => b.Category).Where(p => p.Price < Price).ToList();
            return filterdbooks;
        }
        public List<Book> AuthorFilter(string author)
        {
            return StoreContext.Books.Include(b => b.Author).Include(b => b.Category).Where(b => b.Author.Name.ToLower().Contains(author)).ToList();
        }
        public void Add(Book book)
        {


        }
        public void Update(Book book)
        {
            StoreContext.Update(book);
            StoreContext.SaveChanges();
        }
        public void SaveChanges()
        {

            StoreContext.SaveChanges();
        }

        #region admin
        public void Delete(int id)
        {
            var oldBook = Get(id);

            string fileNameDelete = oldBook.CoverImage;
            string filePathDelete = Path.Combine(_webHostEnvironment.WebRootPath, "Images", fileNameDelete);
            if (File.Exists(filePathDelete))
            {
                File.Delete(filePathDelete);
            }

            StoreContext.Books.Remove(oldBook);
            StoreContext.SaveChanges();
        }
        #endregion
    }
}
