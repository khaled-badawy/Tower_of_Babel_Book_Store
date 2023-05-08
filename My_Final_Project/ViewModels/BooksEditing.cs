using book_store.Models;
using book_store.Repositry;
using book_store.Service;
using My_Final_Project.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace book_store.ViewModels
{
    public class BooksEditing
    {
        public List<Author> authors = new List<Author>();
        public List<Category> categories;
        public int Id { get; set; }
        [BookUniqueName]
        public string Title { get; set; }
        public string Edition { get; set; } = "1st Edition";
        public string Description { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Price must be at least one pound")]
        public double Price { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Quantity can't be by negative values")]
        public int Quantity { get; set; }
        public string? CoverImage { get; set; }
        public int AuthorId { get; set; }
        public int CategoryId { get; set; }
    }
}
