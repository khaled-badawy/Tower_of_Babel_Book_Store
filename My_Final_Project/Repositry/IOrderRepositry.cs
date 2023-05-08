using book_store.Models;

namespace book_store.Repositry
{
    public interface IOrderRepositry
    {
        public List<Order> GetAll();
        public List<Order> GetAllById(string userId);
        public Order Get(int id);
        public void Add(Order order);
        public void Update(Order order);
        public void Delete(int id);
    }
}
