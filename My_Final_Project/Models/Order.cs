using My_Final_Project.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace book_store.Models 
{
    public class Order
    {
        public int Id { get; set; }
        [Required]
        public string ShippingAddress { get; set; }
        public int ShippingCost { get; set; } = 80;
        [Range(80, int.MaxValue, ErrorMessage = "Price must be at least 80 pounds")]
        public double TotalPrice { get; set; }
        public string? Notes { get; set; }
        [EnumDataType(typeof(OrderStatus))]
        public OrderStatus status { get; set; } = OrderStatus.Pending;
        [ForeignKey("user")]
        public string UserId { get; set; }
        public ApplicationUser user { get; set; }
        public ICollection<OrderedItem>? items { get; set; }
        public enum OrderStatus
        {
            Pending = 1,
            Delivered = 2,
            Cancelled = 3
        }

    }

}

