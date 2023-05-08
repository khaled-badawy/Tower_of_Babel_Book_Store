using book_store.Models;
using book_store.Repositry;
using book_store.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using My_Final_Project.Models;

namespace book_store.Service
{
    public class BookService : IBookService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly StoreContext StoreContext;
        private readonly IBookRepositry BookRepo;
        private readonly IAuthorRepositry AuthorRepo;
        private readonly ICategoryRepositry CatRepo;
        public BookService(IBookRepositry booksRepositry, StoreContext storeContext, IWebHostEnvironment webHostEnvironment, IAuthorRepositry AuthorRepo, ICategoryRepositry CatRepo) 
        {
            BookRepo = booksRepositry;
            StoreContext = storeContext;
            _webHostEnvironment = webHostEnvironment;
            this.AuthorRepo = AuthorRepo;
            this.CatRepo = CatRepo;
        }
        public void Add(Book book)
        {
            throw new NotImplementedException();
        }
        public Book Get(int id)
        {
            
            var book = BookRepo.Get(id);
            var category = CatRepo.Get(book.CategoryId);
            book.Category = category;
            return book;
        }
        public List<Book> GetAll()
        {
            return BookRepo.GetAll();
        }
        public List<Book> GetAllBest()
        {
            return BookRepo.GetAllBest();
        }
        public List<Book> GetAllInCategory(int id)
        {
            return BookRepo.GetAllInCategory(id);
        }
        public List<Book> GetAllNew()
        {
           return BookRepo.GetAllNew();
        }
        public List<Book> PriceFilter(Double Price)
        {
            return BookRepo.PriceFilter(Price);
        }
        public List<Book> AuthorFilter(string author)
        {
            return BookRepo.AuthorFilter(author);
        }
        public void Update(Book book)
        {
            BookRepo.Update(book);
        }
        public void SaveChanges()
        {
            BookRepo.SaveChanges();
        }
        public Book GetwithSessionBind(int id)
        {
            var bookToBound = Get(id);
            bookToBound.BookCartId = Guid.NewGuid();
            return bookToBound;
        }
        public List<Book> GetAllwithSessionBind()
        {
            var booksToBound = GetAll();
            foreach (var item in booksToBound)
            {
                item.BookCartId = Guid.NewGuid();
            }
            return booksToBound;
        }

        #region admin
        public BooksEditing AddingBook()
        {
            BooksEditing newBook = new BooksEditing
            {
                authors = AuthorRepo.GetAll(),
                categories = CatRepo.GetAll(),
            };
            return newBook;
        }
        public async Task Create([Bind("Title, Edition, Description, Price, Quantity, AuthorId, CategoryId")] BooksEditing newBook, IFormFile? file)
        {
            var AddedBook = new Book();
            if (file != null)
            {
                string fileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                AddedBook.CoverImage = fileName;
            }
            AddedBook.Title = newBook.Title;
            AddedBook.Edition = newBook.Edition;
            AddedBook.Description = newBook.Description;
            AddedBook.Price = newBook.Price;
            AddedBook.Quantity = newBook.Quantity;
            AddedBook.AuthorId = newBook.AuthorId;
            AddedBook.CategoryId = newBook.CategoryId;
            StoreContext.Books.Add(AddedBook);
            StoreContext.SaveChanges();
        }

        public BooksEditing EditingBook(int id)
        {
            Book originalBook = BookRepo.Get(id);
            BooksEditing EditedBook = new BooksEditing
            {
                authors = AuthorRepo.GetAll(),
                categories = CatRepo.GetAll(),
            };
            EditedBook.Id = originalBook.Id;
            EditedBook.Title = originalBook.Title;
            EditedBook.Edition = originalBook.Edition;
            EditedBook.Description = originalBook.Description;
            EditedBook.Price = originalBook.Price;
            EditedBook.Quantity = originalBook.Quantity;
            EditedBook.AuthorId = originalBook.AuthorId;
            EditedBook.CoverImage = originalBook.CoverImage;
            EditedBook.CategoryId = originalBook.CategoryId;
            return EditedBook;
        }

        public async Task Edit(int id, [Bind("Id,Title, Edition, Description, Price, Quantity, AuthorId, CategoryId")] BooksEditing newBook, IFormFile? file)
        {
            Book originalBook = BookRepo.Get(id);
            if (file != null)
            {
                //1. delete the existing img
                string fileNameDelete = originalBook.CoverImage;
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
                originalBook.CoverImage = fileNameCreate;
            }
            originalBook.Title = newBook.Title;
            originalBook.Edition = newBook.Edition;
            originalBook.Description = newBook.Description;
            originalBook.Price = newBook.Price;
            originalBook.Quantity = newBook.Quantity;
            originalBook.AuthorId = newBook.AuthorId;
            originalBook.CategoryId = newBook.CategoryId;
            StoreContext.SaveChanges();
        }
        public void Delete(int id)
        {
            BookRepo.Delete(id);
        }

        #endregion

    }
}
