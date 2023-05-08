using book_store.Models;

namespace book_store.ViewModels
{
    public class OrdersWithItems
    {
        public List<Order> allOrders { get; set; }
        public List<OrderedItem> allOrderedItems { get; set; }  
    }
}
