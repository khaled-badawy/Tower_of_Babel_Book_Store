using book_store.Models;
using Microsoft.AspNetCore.Mvc;

namespace book_store.Repositry
{
    public interface ICategoryRepositry
    {
        public List<Category> GetAll();
        public Category Get(int id);
        public Task Add([Bind("Name, Description")] Category newCategory, IFormFile file);
        public Task Edit(int id, [Bind("Id,Name, Description")] Category NewCategory, IFormFile file);
        public void Delete(int id);
    }
}
