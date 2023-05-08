using book_store.Models;
using Microsoft.EntityFrameworkCore;
using My_Final_Project.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace book_store.Repositry
{
    public class OrderItemsRepositry:IOrderItemsRepositry
    {
        private readonly StoreContext StoreContext;
        public OrderItemsRepositry(StoreContext storeContext)
        {
            this.StoreContext = storeContext;
        }
        public List<OrderedItem> GetAll()
        {  
           return StoreContext.Items.Include(o => o.book).Include(c => c.category).Include(c => c.order).ToList();

        }
        public OrderedItem Get(int id)
        {
            return StoreContext.Items.Include(o => o.book).Include(c => c.category).Include(c => c.order).FirstOrDefault(itm => itm.Id == id);
        }
        public void Add(OrderedItem item)
        {
            StoreContext.Items.Add(item);
            StoreContext.SaveChanges();
        }
        public void Update(OrderedItem item)
        {
            StoreContext.Items.Update(item);
            StoreContext.SaveChanges();
        }
        public void Delete(int id)
        {
            OrderedItem itemToBeDeleted = Get(id);
            if (itemToBeDeleted != null)
            {
                StoreContext.Remove(itemToBeDeleted);
                StoreContext.SaveChanges();
            }
        }
        public void AddList(List<OrderedItem> items)
        {
            StoreContext.Items.AddRange(items);
            StoreContext.SaveChanges();
        }

    }
}
