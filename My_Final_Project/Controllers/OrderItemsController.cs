using book_store.Models;
using book_store.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;
using System.Net;

namespace book_store.Controllers
{
    public class OrderItemsController : Controller
    {
        private readonly IOrderItemService itemService;
        private readonly IBookService bookService;
        public OrderItemsController(IOrderItemService _itemService, IBookService _bookService) {
            this.itemService = _itemService;
            this.bookService = _bookService;
        }
        [Authorize(Roles = "customer")]
        [AutoValidateAntiforgeryToken]
        public IActionResult AddToCart([FromQuery] int bookId, [FromQuery] int orderQuantity, [FromQuery] Guid bookcartid)
        {
            List<OrderedItem> orderItems;
            OrderedItem item = null;
            int itemCounter = 0;
            var book = bookService.Get(bookId);

            string orderItemsAsJson = HttpContext.Session.GetString("Cart");
            if (orderItemsAsJson != null)
            {
                orderItems = JsonConvert.DeserializeObject<List<OrderedItem>>(orderItemsAsJson);
            }
            else
            {
                orderItems = new List<OrderedItem>();
            }

            if (orderItems.Count == 0)
            {
                item = itemService.ConvertToAddToCart(bookId, orderQuantity);
                if (item.Quantity > book.Quantity)
                {
                    return RedirectToAction("outofstock", "book");
                }
                item.itemCartId = bookcartid;
                orderItems.Add(item);
                itemCounter = orderItems.Count;
            }
            else
            {
                for (int i = 0; i < orderItems.Count; i++)
                {
                    if (orderItems[i].itemCartId == bookcartid)
                    {
                        continue;
                    }
                    else
                    {
                        if (orderItems[i].BookId == bookId)
                        {  
                          orderItems[i].Quantity += orderQuantity;
                          if (orderItems[i].Quantity > book.Quantity)
                          {
                               return RedirectToAction("outofstock", "book");
                          }
                          orderItems[i].itemCartId = bookcartid;
                          orderItems[i].totalPrice = orderItems[i].UnitPrice * orderItems[i].Quantity;
                          orderItems[i].book = book;                  
                        }
                        else
                        {
                            var existingItem = orderItems.FirstOrDefault(i => i.itemCartId == bookcartid);
                            if (existingItem == null)
                            {
                                item = itemService.ConvertToAddToCart(bookId, orderQuantity);
                                item.book = book;
                                if (item.Quantity > book.Quantity)
                                {
                                    return RedirectToAction("outofstock", "book");
                                }
                                item.itemCartId = bookcartid;
                                orderItems.Add(item);
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                }
                itemCounter = orderItems.Count;
            }
            var itemAsJson = JsonConvert.SerializeObject(orderItems);
            HttpContext.Session.SetString("Cart", itemAsJson);
            HttpContext.Session.SetInt32("ItemsCount", itemCounter);
            if (item == null) 
            {
                var updatedItem = orderItems.FirstOrDefault(i=>i.BookId == bookId);
                return View(updatedItem);
            }
            else
            {
                return View(item);
            }
        }

        [Authorize(Roles = "customer")]
        [AutoValidateAntiforgeryToken]
        public IActionResult ViewCart()
        {
            List<OrderedItem> orderItems;

            string orderItemsAsJson = HttpContext.Session.GetString("Cart");
            if (orderItemsAsJson != null)
            {
                orderItems = JsonConvert.DeserializeObject<List<OrderedItem>>(orderItemsAsJson);
                for (int i = 0; i < orderItems.Count; i++)
                {
                    orderItems[i] = itemService.ConvertToAddToCart(orderItems[i].BookId, orderItems[i].Quantity);
                }
            }
            else
            {
                return RedirectToAction("Empty");
            }

            return View(orderItems);
        }
        [Authorize(Roles = "customer")]
        [AutoValidateAntiforgeryToken]
        public IActionResult Empty()
        {
            return View();
        }
    }
}
