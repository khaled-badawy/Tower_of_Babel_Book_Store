using book_store.Models;
using book_store.ViewModels;

namespace book_store.Service
{
    public interface IOrderService
    {
        public OrdersWithItems DetailedOrders();
        public List<Order> GetAll();
        public List<Order> GetAllById(string userId);
        public Order Get(int id);
        public int Add(Order order);
        public void Update(Order order);
        public void Delete(int id);
    }
}
