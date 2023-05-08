using book_store.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace My_Final_Project.Models
{
    public class BookUniqueNameAttribute : ValidationAttribute
    {
        StoreContext context = new StoreContext();
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string name = value.ToString();
            BooksEditing newBook = (BooksEditing)validationContext.ObjectInstance;
            var book = context.Books.FirstOrDefault(a => a.Title == name && a.Id != newBook.Id);
            if (book != null)
            {
                return new ValidationResult("A book with the same name already exists.");
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}
