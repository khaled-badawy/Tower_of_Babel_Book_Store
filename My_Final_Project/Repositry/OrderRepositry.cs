using book_store.Models;
using Microsoft.EntityFrameworkCore;
using My_Final_Project.Models;

namespace book_store.Repositry
{
    public class OrderRepositry:IOrderRepositry
    {
        private readonly StoreContext StoreCon;
        public OrderRepositry(StoreContext storeContext)
        {
            this.StoreCon = storeContext;
        }
        public List<Order> GetAll()
        {
            return StoreCon.Orders.Include(o => o.items).Include(o => o.user).AsNoTracking().ToList();
        }
        public List<Order> GetAllById(string userId)
        {
            return StoreCon.Orders.Include(o => o.items).Include(o => o.user).Where(o => o.UserId == userId).AsNoTracking().ToList();
        }
        public Order Get(int id)
        {
            return StoreCon.Orders.Include(o => o.items).Include(o => o.user).AsNoTracking().FirstOrDefault(o => o.Id == id);
        }
        public void Add(Order order)
        {
            if (StoreCon.Orders.ToList().Count == 0)
            {
                order.Id = 1;
            }
            else
            {
                order.Id = StoreCon.Orders.Max(o => o.Id) +1;
            }
            StoreCon.Orders.Add(order);
            StoreCon.SaveChanges();
        }
        public void Update(Order order)
        {
            StoreCon.Orders.Update(order);
            StoreCon.SaveChanges();
        }
        public void Delete(int id)
        {
            Order orderToBeDeleted = Get(id);
            if (orderToBeDeleted != null)
            {
                StoreCon.Remove(orderToBeDeleted);
                StoreCon.SaveChanges();
            }
        }
    }
}
