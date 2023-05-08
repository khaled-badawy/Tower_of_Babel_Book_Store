using book_store.Models;
using book_store.Repositry;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Model.Tree;
using Newtonsoft.Json;


namespace book_store.Service
{
    public class OrderItemService:IOrderItemService
    {
        private readonly IOrderItemsRepositry orderItemRepositry;
        private readonly IBookRepositry bookRepo;
        public OrderItemService(IOrderItemsRepositry _orderItemRepositry, IBookRepositry _bookRepo) 
        { 
            this.orderItemRepositry = _orderItemRepositry;
            this.bookRepo = _bookRepo;
        }
        public List<OrderedItem> GetAll()
        {
            return orderItemRepositry.GetAll();
        }
        public OrderedItem Get(int id)
        {
           return orderItemRepositry.Get(id);
        }
        public void Add(OrderedItem item)
        {
            throw new NotImplementedException();
        }
        public void AddList(List<OrderedItem> items,int orderId)
        {
            items = ConvertToAddToDb(items, orderId);
            orderItemRepositry.AddList(items);
            foreach (var item in items)
            {
                var book = bookRepo.Get(item.BookId);
                if (book != null) 
                {
                    book.Quantity -= item.Quantity;
                    bookRepo.Update(book);
                }
            }
        }
        public void Update(OrderedItem item)
        {
            orderItemRepositry.Update(item);
        }
        public void Delete(int id)
        {
            orderItemRepositry.Delete(id);
        }
        public OrderedItem ConvertToAddToCart(int bookId,int quantity)
        {
                OrderedItem orderedItem = new OrderedItem();
                Book newBook = bookRepo.Get(bookId);
            if (newBook != null)
            {
                orderedItem.Quantity = quantity;
                orderedItem.UnitPrice = newBook.Price;
                orderedItem.totalPrice = newBook.Price * quantity;
                orderedItem.BookId = bookId;
                orderedItem.book = newBook;
            }
            return orderedItem;   
        }
        public List<OrderedItem> ConvertToAddToDb(List<OrderedItem> inCompleteItems, int orderId)
        {
            for (int i = 0; i < inCompleteItems.Count; i++)
            {
                if (i==0)
                {
                    inCompleteItems[i].OrderId = orderId;
                }
                else
                {
                    if (inCompleteItems[i].BookId == inCompleteItems[i-1].BookId && inCompleteItems[i].Quantity== inCompleteItems[i-1].Quantity)
                    {
                        continue;
                    }
                    else
                    {
                        inCompleteItems[i].OrderId = orderId;
                    }
                }
                inCompleteItems[i].book = bookRepo.Get(inCompleteItems[i].BookId); 
                inCompleteItems[i].categoryId = inCompleteItems[i].book.CategoryId;
                inCompleteItems[i].book = null;
            }

            return inCompleteItems;
        }
    }
}
