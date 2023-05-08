using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using My_Final_Project.Controllers;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using My_Final_Project.Models;
using Newtonsoft.Json;

namespace book_store.Models 
{ 
public class Book
{
        [NotMapped]
        public Guid BookCartId { get; set; } = Guid.NewGuid();
        public int Id { get; set; }
        [BookUniqueName]
        public string Title { get; set; }
        public string Edition { get; set; } = "1st Edition";
        public string Description { get; set; }
        [EnumDataType(typeof(Status))]
        public Status BookStatus { get; set; } = Status.Default;
        [Range(1, int.MaxValue, ErrorMessage = "Price must be at least one pound")]
        public double Price { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Quantity can't be by negative values")]
        public int Quantity { get; set; }
        public string? CoverImage { get; set; }
        [ForeignKey("Author")]
        public int AuthorId { get; set; }
        [JsonIgnore]
        public Author Author { get; set; }
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        [JsonIgnore]
        public Category Category { get; set; }
        public ICollection<OrderedItem>? items { get; set; }
        public enum Status
        {
            New = 1,
            Best = 2,
            Default = 3,
        }

    }


}

