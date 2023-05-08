using book_store.Models;
using book_store.Repositry;
using book_store.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace book_store.Service
{
    public class CategoryService:ICategoryService
    {
        private readonly ICategoryRepositry categoryRepositry;
        private readonly IBookRepositry booksRepositry;
        public CategoryService(ICategoryRepositry _catRepo, IBookRepositry _bookRepo) 
        {
            categoryRepositry= _catRepo;
            booksRepositry= _bookRepo;
        }
        public CategoryWithBooksViewModel ConvertToViewModel(List<Category> categories)
        {
            CategoryWithBooksViewModel categoriesWithBooks = new CategoryWithBooksViewModel
            {
                BestBooks = GetBestBooks(),
                NewBooks = GetNewBooks(),
                Categories = categories
            };
            return categoriesWithBooks;
            
        }
        public void Delete(int id)
        {
            categoryRepositry.Delete(id);   
        }
        public Category Get(int id)
        {
            return categoryRepositry.Get(id);
        }
        public List<Category> GetAll()
        {
            return categoryRepositry.GetAll();
        }
        public List<Book> GetBestBooks()
        {
            return booksRepositry.GetAllBest();
        }
        public List<Book> GetNewBooks()
        {
            return booksRepositry.GetAllNew();
        }
        public async Task Add([Bind("Name, Description")] Category newCategory, IFormFile file)
        {
            await categoryRepositry.Add(newCategory, file);
        }
        public async Task Edit(int id, [Bind("Id,Name, Description")] Category NewCategory, IFormFile? file)
        {
            await categoryRepositry.Edit(id, NewCategory, file);
        }
    }
}
