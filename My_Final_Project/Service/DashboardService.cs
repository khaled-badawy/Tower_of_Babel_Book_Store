using book_store.Models;
using book_store.Repositry;
using book_store.ViewModels;
using My_Final_Project.Models;
using My_Final_Project.Repositry;

namespace book_store.Service
{
    public class DashboardService : IDashboardService
    {
        private readonly StoreContext StoreContext;
        private readonly IBookRepositry BookRepo;
        private readonly IAuthorRepositry AuthorRepo;
        private readonly ICategoryService CatService;
        private readonly IReviewService revService;
        private readonly IOrderItemsRepositry itemsRepo;
        private readonly IOrderService orderService;
        private readonly IApplicationUserRepositry userRepo;

        public DashboardService(StoreContext storeContext, IBookRepositry bookRepo, IAuthorRepositry authorRepo, ICategoryService catRepo, IReviewService revService, IOrderItemsRepositry itemsRepo, IOrderService orderService, IApplicationUserRepositry userRepo)
        {
            StoreContext = storeContext;
            BookRepo = bookRepo;
            AuthorRepo = authorRepo;
            CatService = catRepo;
            this.revService = revService;
            this.itemsRepo = itemsRepo;
            this.orderService = orderService;
            this.userRepo = userRepo;
        }

        public DashboardHomeVM homeAdmin()
        {
            DashboardHomeVM home = new DashboardHomeVM();
            home.categories = CatService.GetAll();
            home.items = itemsRepo.GetAll();
            home.orders = orderService.GetAll();
            home.books = BookRepo.GetAll();
            home.avgRev = 0;
            home.users = userRepo.GetAll();


            return home;
        }
    }
}
