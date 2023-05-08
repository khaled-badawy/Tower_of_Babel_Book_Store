using book_store.Models;

namespace book_store.Service
{
    public interface IOrderItemService
    {
        public List<OrderedItem> GetAll();
        public OrderedItem Get(int id);
        public void Add(OrderedItem item);
        public void AddList(List<OrderedItem> item,int orderId);
        public void Update(OrderedItem item);
        public void Delete(int id);
        public OrderedItem ConvertToAddToCart(int bookId,int quantity);
        public List<OrderedItem> ConvertToAddToDb(List<OrderedItem> inCompleteItems, int orderId);
    }
}
