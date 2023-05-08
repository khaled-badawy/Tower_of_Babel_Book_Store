using book_store.Models;

namespace book_store.Repositry
{
    public interface IOrderItemsRepositry
    {
        public List<OrderedItem> GetAll();
        public OrderedItem Get(int id);
        public void Add(OrderedItem item);
        public void AddList(List<OrderedItem> item);
        public void Update(OrderedItem item);
        public void Delete(int id);
    }
}
