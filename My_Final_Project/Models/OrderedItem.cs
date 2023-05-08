using My_Final_Project.Models;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace book_store.Models
{
    public class OrderedItem
    {
        [NotMapped]
        public Guid itemCartId { get; set; }
        public int Id { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Qty must be at least 1")]
        public int Quantity { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Price must be at least 1 Pound")]
        public double UnitPrice { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Price must be at least 1 Pound")]
        public double totalPrice { get; set; }
        public string? Description { get; set; }
        [ForeignKey("order")]
        public int OrderId { get; set; }
        
        public Order? order { get; set; }
        [ForeignKey("book")]
        public int BookId { get; set; }
        [JsonIgnore]
        public Book? book { get; set; }
        [ForeignKey("category")]
        public int categoryId { get; set; }
        [JsonIgnore]
        public Category category { get; set; }

    }
}
