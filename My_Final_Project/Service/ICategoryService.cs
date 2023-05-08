using book_store.Models;
using book_store.Repositry;
using book_store.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace book_store.Service
{
    public interface ICategoryService
    {
        public List<Category> GetAll();
        public Category Get(int id);
        public Task Add([Bind("Name, Description")] Category newCategory, IFormFile file);
        public Task Edit(int id, [Bind("Id,Name, Description")] Category newCategory, IFormFile file);
        public void Delete(int id);
        public List<Book> GetNewBooks();
        public List<Book> GetBestBooks();
        public CategoryWithBooksViewModel ConvertToViewModel(List<Category> categories);
    }
}
