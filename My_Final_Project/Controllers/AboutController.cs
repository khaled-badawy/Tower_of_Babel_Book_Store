using book_store.Models;
using book_store.Service;
using book_store.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using static System.Reflection.Metadata.BlobBuilder;

namespace book_store.Controllers
{
    public class AboutController : Controller
    {
        public ActionResult Us()
        {
            return View();
        }
    }
}
