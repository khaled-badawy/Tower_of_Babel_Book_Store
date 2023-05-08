using book_store.Models;
using book_store.Repositry;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace book_store.Controllers
{

    public class AuthorController : Controller
    {
        public readonly IAuthorRepositry author;
        public AuthorController(IAuthorRepositry author) 
        { 
            this.author = author;
        }
        [Authorize(Roles = "admin")]
        public IActionResult Search(string search)
        {
            var authors = author.GetAll();

            if (!String.IsNullOrEmpty(search))
            {
                authors = authors.Where(a => a.Name.ToLower().Contains(search.ToLower())).ToList();
                ViewBag.SearchString = search;
            }

            return View("_AuthorsTable", authors);
        }
        [Authorize(Roles = "admin")]
        public IActionResult IndexAdmin()
        {
            var authors = author.GetAll();
            return View(authors);
        }
        public IActionResult Index()
        {
            var authors = author.GetAll();
            return View(authors);
        }

        #region create
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([Bind("Name, BriefHistory, PublishCount")] Author Newauthor, IFormFile file)
        {
            if(ModelState.IsValid)
            {
                await author.New( Newauthor, file);
                return RedirectToAction("IndexAdmin");
            }
            else
            {
                return View(Newauthor);
            }

        }
        #endregion

        #region edit
        [Authorize(Roles = "admin")]
        public IActionResult Edit(int id)
        {
            var oldAuthor = author.GetById(id);
            var newAuthor = new Author();
            newAuthor.Id = oldAuthor.Id;
            newAuthor.BriefHistory = oldAuthor.BriefHistory;
            newAuthor.PublishCount = oldAuthor.PublishCount;
            newAuthor.Name = oldAuthor.Name;
            return View(newAuthor);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name, BriefHistory, PublishCount")] Author Newauthor, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                await author.Edit(id, Newauthor, file);
                return RedirectToAction("IndexAdmin");
            }
            else
            {
                return View(Newauthor);
            }
        }
        #endregion

        [Authorize(Roles = "admin")]
        public IActionResult Delete(int id)
        {
            author.Delete(id);

            List<Author> authors = author.GetAll(); 
            return RedirectToAction("IndexAdmin", authors);
        }
    }
}
