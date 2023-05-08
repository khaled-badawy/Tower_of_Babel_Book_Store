using book_store.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using My_Final_Project.Models;

namespace book_store.Repositry
{
    public class AuthorRepositry: IAuthorRepositry
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly StoreContext StoreContext;
        public AuthorRepositry(StoreContext _storeContext, IWebHostEnvironment webHostEnvironment)
        {
            StoreContext = _storeContext;
            _webHostEnvironment = webHostEnvironment;
        }
        public List<Author> GetAll()
        {
            var AllAuthors = StoreContext.Authors.ToList();
            return AllAuthors;
        }
        public async Task New ([Bind("Name, BriefHistory, PublishCount")] Author author, IFormFile file)
        {
                if (file!=null)
                {
                    string fileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    author.Image = fileName;
                }
            StoreContext.Authors.Add(author);
            StoreContext.SaveChanges();
        }
        public Author GetById (int id)
        {
            return StoreContext.Authors.FirstOrDefault(a => a.Id == id);
        }
        public async Task Edit(int id, [Bind("Id,Name, BriefHistory, PublishCount")] Author Newauthor, IFormFile? file)
        {
            var oldAuthor = GetById(id);
            if (file != null)
            {
                //1. delete the existing img
                string fileNameDelete = oldAuthor.Image;
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
                oldAuthor.Image = fileNameCreate;
            }
            oldAuthor.Name = Newauthor.Name;
            oldAuthor.BriefHistory = Newauthor.BriefHistory;
            oldAuthor.PublishCount = Newauthor.PublishCount;
            StoreContext.SaveChanges();
        }
        public void Delete (int id)
        {
            var oldAuthor = GetById(id);

            string fileNameDelete = oldAuthor.Image;
            string filePathDelete = Path.Combine(_webHostEnvironment.WebRootPath, "Images", fileNameDelete);
            if (File.Exists(filePathDelete))
            {
                File.Delete(filePathDelete);
            }

            StoreContext.Authors.Remove(oldAuthor);
            StoreContext.SaveChanges();
        }
    }
}
