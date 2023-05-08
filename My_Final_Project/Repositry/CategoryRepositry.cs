using book_store.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using My_Final_Project.Models;

namespace book_store.Repositry
{
    public class CategoryRepositry:ICategoryRepositry
    {
		private readonly IWebHostEnvironment _webHostEnvironment;
		private readonly StoreContext StoreContext;
        public CategoryRepositry(StoreContext _storeContext, IWebHostEnvironment webHostEnvironment)
        {
            StoreContext = _storeContext;
            _webHostEnvironment = webHostEnvironment;
        }
        public List<Category> GetAll()
        {
            return StoreContext.Categories.Include(c=>c.Books).ToList();
        }
        public Category Get(int id) 
        {
            return StoreContext.Categories.FirstOrDefault(c => c.Id == id);
        }
        public async Task Add([Bind("Name, Description")] Category newCategory, IFormFile file)
        {
            if (file != null)
            {
                string fileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                newCategory.Image = fileName;
            }
            StoreContext.Categories.Add(newCategory);
            StoreContext.SaveChanges();
        }
        public async Task Edit(int id, [Bind("Id,Name, Description")] Category NewCategory, IFormFile? file)
        {
            var oldCat = Get(id);
            if (file != null)
            {
                //1. delete the existing img
                string fileNameDelete = oldCat.Image;
                string filePathDelete = Path.Combine(_webHostEnvironment.WebRootPath, "Images", fileNameDelete);
                if (File.Exists(filePathDelete))
                {
                    File.Delete(filePathDelete);
                }

                //2. create the new img
                string fileNameCreate = Guid.NewGuid().ToString() + "_" + file.FileName;
                string filePathCreate = Path.Combine(_webHostEnvironment.WebRootPath, "Images", fileNameCreate);
                using (var fileStream = new FileStream(filePathCreate, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                //3. assign the new img
                oldCat.Image = fileNameCreate;
            }
            oldCat.Name = NewCategory.Name;
            oldCat.Description = NewCategory.Description;
            StoreContext.SaveChanges();
        }
        public void Delete(int id)
        {
            var oldCategory = Get(id);

            string fileNameDelete = oldCategory.Image;
            string filePathDelete = Path.Combine(_webHostEnvironment.WebRootPath, "Images", fileNameDelete);
            if (File.Exists(filePathDelete))
            {
                File.Delete(filePathDelete);
            }

            StoreContext.Categories.Remove(oldCategory);
            StoreContext.SaveChanges();
        }
    }
}
