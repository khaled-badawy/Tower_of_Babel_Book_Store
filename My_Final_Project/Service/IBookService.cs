using book_store.Models;
using book_store.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace book_store.Service
{
    public interface IBookService
    {
        public Book Get(int id);
        public List<Book> GetAll();
        public Book GetwithSessionBind(int id);
        public List<Book> GetAllwithSessionBind();
        public List<Book> GetAllNew();
        public List<Book> GetAllBest();
        public void Add(Book book);
        public void Update(Book book);
        public void Delete(int id);
        public Task Create([Bind("Title, Edition, Description, Price, Quantity, AuthorId, CategoryId")] BooksEditing newBook, IFormFile? file);
        public BooksEditing AddingBook();
        public BooksEditing EditingBook(int id);
        public Task Edit(int id, [Bind("Id,Title, Edition, Description, Price, Quantity, AuthorId, CategoryId")] BooksEditing newBook, IFormFile? file);
        public List<Book> GetAllInCategory(int id);
        public List<Book> PriceFilter(Double Price);
        public List<Book> AuthorFilter(string author);
    }
}
