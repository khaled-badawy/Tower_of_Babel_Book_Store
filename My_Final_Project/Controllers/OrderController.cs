using book_store.Service;
using book_store.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Newtonsoft.Json;
using System.Security.Claims;
using My_Final_Project.Models;
using Microsoft.AspNetCore.Identity;

namespace book_store.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService orders;
        private readonly IOrderItemService itemsService;
        private readonly IBookService bookService;
        private readonly UserManager<ApplicationUser> userManager;
        public OrderController(IOrderService _orders, IOrderItemService _items, UserManager<ApplicationUser> _userManager, IBookService _bookService)
        {
            this.orders = _orders;
            this.itemsService = _items;
            this.userManager = _userManager;
            this.bookService = _bookService;
        }
        [Authorize(Roles = "customer")]
        [AutoValidateAntiforgeryToken]
        public IActionResult PlaceOrder() 
        {
            Order newOrder = new Order();
            
            List<OrderedItem> orderItems;

            string orderItemsAsJson = HttpContext.Session.GetString("Cart");
            if (orderItemsAsJson != null)
            {
                orderItems = JsonConvert.DeserializeObject<List<OrderedItem>>(orderItemsAsJson);
                foreach (var item in orderItems)
                {
                    item.book = bookService.Get(item.BookId);
                    newOrder.TotalPrice += item.totalPrice;
                }
                newOrder.TotalPrice += newOrder.ShippingCost;
            }
            else
            {
                return RedirectToAction("Empty","OrderItems");
            }
            newOrder.items = orderItems;
            return View(newOrder);
        }

        [HttpPost]
        [Authorize(Roles = "customer")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> PlaceOrder(Order order) 
        {
            List<OrderedItem> orderItems;

            string orderItemsAsJson = HttpContext.Session.GetString("Cart");
            if (orderItemsAsJson != null)
            {
                orderItems = JsonConvert.DeserializeObject<List<OrderedItem>>(orderItemsAsJson);
                order.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                order.user = await userManager.GetUserAsync(User);
                foreach (var item in orderItems)
                {
                    order.TotalPrice += item.totalPrice;
                }
                order.TotalPrice += order.ShippingCost;
                if (order.user!=null && order.UserId!=null)
                {
                    int orderId = orders.Add(order);
                    itemsService.AddList(orderItems, orderId);
                    HttpContext.Session.Clear();
                    return RedirectToAction("OrderSuccess");
                }
                order.items = orderItems;
                return View(order);
            }
            else
            {
                return RedirectToAction("Empty", "OrderItems");
            }
        }

        [Authorize(Roles = "customer")]
        [AutoValidateAntiforgeryToken]
        public IActionResult OrderSuccess() 
        { 
            return View(); 
        }
        [HttpPost]
        [Authorize(Roles = "customer")]
        [AutoValidateAntiforgeryToken]
        public IActionResult ClearCart() 
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Empty", "OrderItems");
        }
        [HttpPost]
        [Authorize(Roles = "customer")]
        [AutoValidateAntiforgeryToken]
        public IActionResult RemoveFromCart([FromQuery] int bookid, [FromQuery] int quantity)
        {
            Order order = new Order();
            List<OrderedItem> orderItems;

            string orderItemsAsJson = HttpContext.Session.GetString("Cart");
            if (orderItemsAsJson != null) 
            {
                orderItems =  JsonConvert.DeserializeObject<List<OrderedItem>>(orderItemsAsJson);
                var itemToRemove = orderItems.Find(i => i.BookId == bookid);
                if (itemToRemove != null) 
                {
                    orderItems.Remove(itemToRemove);
                    if (orderItems.Count ==0)
                    {
                        HttpContext.Session.Clear();
                        return RedirectToAction("Empty", "OrderItems");
                    }
                    foreach (var item in orderItems)
                    {
                        order.TotalPrice += item.totalPrice;
                    }
                    order.TotalPrice += order.ShippingCost;
                }
                for (int i = 0; i < orderItems.Count; i++)
                {
                    orderItems[i] = itemsService.ConvertToAddToCart(bookid, quantity);
                }
                order.items = orderItems;
                var itemAsJson = JsonConvert.SerializeObject(orderItems);
                HttpContext.Session.SetString("Cart", itemAsJson);
                return RedirectToAction("PlaceOrder", new { order });
            }
            else
            {
                return RedirectToAction("PlaceOrder", new { order });           
            }
        }

        [Authorize(Roles = "customer")]
        [AutoValidateAntiforgeryToken]
        public IActionResult ViewOrders()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) 
            { 
                return RedirectToAction("Empty","orderitems");
            }
            List<Order> orderList = orders.GetAllById(userId);
            return View(orderList);
        }

        #region admin
        [Authorize(Roles = "admin")]
        public IActionResult IndexAdmin()
        {
            return View(orders.DetailedOrders());
        }
        #endregion
    }
}
