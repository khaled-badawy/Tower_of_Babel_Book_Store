using book_store.Models;
using book_store.Repositry;
using book_store.ViewModels;
using My_Final_Project.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace book_store.Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderItemsRepositry Items;
        private readonly IOrderRepositry ordersRepo;
        private readonly StoreContext bookContext;
        public OrderService(IOrderItemsRepositry items, IOrderRepositry orders, StoreContext _bookStore)
        {
            Items = items;
            this.ordersRepo = orders;
            this.bookContext = _bookStore;
        }
        public OrdersWithItems DetailedOrders()
        {
            OrdersWithItems myOrders = new OrdersWithItems();
            myOrders.allOrders = bookContext.Orders.Include(o => o.user).ToList();
            myOrders.allOrderedItems = Items.GetAll();
            return myOrders;
        }
        public List<Order> GetAll()
        {
            return ordersRepo.GetAll();
        }
        public List<Order> GetAllById(string userId)
        {
            return ordersRepo.GetAllById(userId);
        }
        public Order Get(int id)
        {
            return ordersRepo.Get(id);
        }
        public int Add(Order order)
        {
            ordersRepo.Add(order);
            return order.Id;
        }
        public void Update(Order order)
        {
            ordersRepo.Update(order);
        }
        public void Delete(int id)
        {
            ordersRepo.Delete(id);
        }
    }
}
