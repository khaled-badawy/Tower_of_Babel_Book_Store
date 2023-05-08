using book_store.Models;
using My_Final_Project.Models;

namespace book_store.ViewModels
{
    public class DashboardHomeVM
    {
        public List<Category> categories { get; set; }
        public List<OrderedItem> items { get; set; }
        public List<Order> orders { get; set; }
        public List<Book> books { get; set; }
        public double avgRev { get; set; }
        public List<ApplicationUser> users { get; set; }
    }
}
