using System.Diagnostics;
using book_store.Service;
using Microsoft.AspNetCore.Mvc;
using My_Final_Project.Models;

namespace My_Final_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICategoryService CatService;
        public HomeController(ILogger<HomeController> logger, ICategoryService _CatService)
        {
            _logger = logger;
            CatService = _CatService;
        }

        public IActionResult Index()
        {
            var categories = CatService.GetAll();
            var categoriesWithBooks = CatService.ConvertToViewModel(categories);
            return View(categoriesWithBooks);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}