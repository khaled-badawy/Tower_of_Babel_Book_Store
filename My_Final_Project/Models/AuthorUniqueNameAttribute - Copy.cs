using book_store.Models;
using System.ComponentModel.DataAnnotations;

namespace My_Final_Project.Models
{
    public class AuthorUniqueNameAttribute : ValidationAttribute
    {
        StoreContext context = new StoreContext();
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string name = value.ToString();
            Author newAuthor = (Author)validationContext.ObjectInstance;
            var author = context.Authors.FirstOrDefault(a => a.Name == name && a.Id != newAuthor.Id);
            if (author != null)
            {
                return new ValidationResult("An Author with the same name already exists.");
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}
