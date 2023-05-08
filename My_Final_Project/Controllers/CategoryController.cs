using Microsoft.AspNetCore.Mvc;
using book_store.Repositry;
using book_store.Service;
using book_store.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace book_store.Controllers
{
    public class CategoryController : Controller
    {
        public ICategoryService CatService;
        public CategoryController(ICategoryService _CatService)
        {
            CatService = _CatService;
        }

        public IActionResult Index()
        {
            var categories = CatService.GetAll();
            return View(categories);
        }

        #region admin
        [Authorize(Roles = "admin")]
        public IActionResult IndexAdmin()
        {
            return View(CatService.GetAll());
        }
        [Authorize(Roles = "admin")]
        public IActionResult Search(string search)
        {
            var categories = CatService.GetAll();

            if (!String.IsNullOrEmpty(search))
            {
                categories = categories.Where(a => a.Name.ToLower().Contains(search.ToLower())).ToList();
                ViewBag.SearchCategory = search;
            }

            return View("_CategoriesTablePartial", categories);
        }

        #region create
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([Bind("Name, Description")] Category newCategory, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                await CatService.Add(newCategory, file);
                return RedirectToAction("IndexAdmin");
            }
            else
            {
                return View(newCategory);
            }

        }
        #endregion

        [Authorize(Roles = "admin")]
        public IActionResult Edit(int id)
        {
            return View(CatService.Get(id));
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name, Description")] Category NewCategory, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                await CatService.Edit(id, NewCategory, file);
                return RedirectToAction("IndexAdmin");
            }
            else
            {
                return View(NewCategory);
            }
        }

        [Authorize(Roles = "admin")]
        public IActionResult Delete(int id)
        {
            CatService.Delete(id);

            List<Category> categories = CatService.GetAll();
            return RedirectToAction("IndexAdmin", categories);
        }
        #endregion

    }
}
