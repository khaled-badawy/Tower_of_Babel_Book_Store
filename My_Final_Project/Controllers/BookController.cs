using book_store.Models;
using book_store.Service;
using book_store.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;

namespace book_store.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookService bookService;
        public BookController(IBookService _bookService)
        { 
            this.bookService = _bookService;
        }

        #region admin
        [Authorize(Roles = "admin")]
        public IActionResult IndexAdmin()
        {
            var books = bookService.GetAll();
            return View(books);
        }
        [Authorize(Roles = "admin")]
        public IActionResult Search(string search)
        {
            var books = bookService.GetAll();

            if (!String.IsNullOrEmpty(search))
            {
                books = books.Where(a => a.Title.ToLower().Contains(search.ToLower())).ToList();
                ViewBag.SearchBook = search;
            }

            return View("_BooksTablePartial", books);
        }

        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View(bookService.AddingBook());
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([Bind("Title, Edition, Description, Price, Quantity, AuthorId, CategoryId")] BooksEditing newBook, IFormFile? file)
        { 
            if(ModelState.IsValid)
            {
                await bookService.Create(newBook, file);
                return RedirectToAction("IndexAdmin");

            }
            else
            {
                return View(newBook);
            }
        }
        [Authorize(Roles = "admin")]
        public IActionResult Edit(int id)
        {
            return View(bookService.EditingBook(id));
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title, Edition, Description, Price, Quantity, AuthorId, CategoryId")] BooksEditing newBook, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                await bookService.Edit(id, newBook, file);
                return RedirectToAction("IndexAdmin");

            }
            else
            {
                return View(newBook);
            }
        }

        [Authorize(Roles = "admin")]
        public IActionResult Delete(int id)
        {
            bookService.Delete(id);

            List<Book> books = bookService.GetAll();
            return RedirectToAction("IndexAdmin", books);
        }

        #endregion

        public IActionResult Index()
        {
            var books = bookService.GetAllwithSessionBind();
            return View(books);
        }
        public IActionResult CategoryBooks(int id)
        {
            var books = bookService.GetAllInCategory(id);
            return View(books);
        }
        public IActionResult GetNew()
        {
            var newBooks = bookService.GetAllNew();
            return View(newBooks);
        }
        public IActionResult GetBest()
        {
            var bestBooks = bookService.GetAllBest();
            return View(bestBooks);
        }
        public IActionResult Details(int id)
        {
            var book = bookService.GetwithSessionBind(id);
            return View(book);
        }
        public IActionResult PriceFilter(Double Price)
        {
            var filteredbook = bookService.PriceFilter(Price);
            ViewBag.price = Price;
            return View(filteredbook);
        }
        public IActionResult UserSearch(string author)
        {

            author = author.ToLower();
            var authorbooks = bookService.AuthorFilter(author);

            ViewBag.SearchText = author;
            return View(authorbooks);
        }

        [HttpPost]
        public IActionResult Details([FromRoute] int id, int orderQuantity,[FromQuery]Guid bookcartid) {
            var book = bookService.Get(id);
            if (ModelState.IsValid)
            {
                return RedirectToAction("AddToCart", "OrderItems", new {bookId=id ,orderQuantity, bookcartid });
            }
            return View(book);
        }

        public IActionResult OutofStock() 
        {
            return View();
        }


















        //public IActionResult New()
        //{
        //    var newBooks = bookService.GetAllNew();
        //    return View(newBooks);
        //}
        //public IActionResult Best()
        //{
        //    var bestBooks = bookService.GetAllBest();
        //    return View(bestBooks);
        //}
        //public IActionResult Details(int id) 
        //{
        //    var book = bookService.Get(id);
        //    return View(book);
        //}
        //public IActionResult AddToCart([FromRoute]int id)
        //{
        //    List<Book> bookList;

        //    string booksListAsJson = HttpContext.Session.GetString("Cart");
        //    if (booksListAsJson!=null)
        //    {
        //        bookList = JsonConvert.DeserializeObject<List<Book>>(booksListAsJson);
        //    }
        //    else 
        //    {
        //        bookList = new List<Book>();
        //    }
        //    var book = bookService.Get(id); 
        //    bookList.Add(book);
        //    var BooksAsJson = JsonConvert.SerializeObject(bookList);
        //    HttpContext.Session.SetString("Cart", BooksAsJson);

        //    return RedirectToAction("Index","category");
        //}
        //public IActionResult ViewCart()
        //{
        //    List<Book> bookList;

        //    string booksListAsJson = HttpContext.Session.GetString("Cart");
        //    if (booksListAsJson != null)
        //    {
        //        bookList = JsonConvert.DeserializeObject<List<Book>>(booksListAsJson);
        //    }
        //    else
        //    {
        //        return Content("Cart is Empty");
        //    }
        //    return View(bookList);
        //}
    }
}
