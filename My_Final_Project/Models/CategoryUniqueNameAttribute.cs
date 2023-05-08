using book_store.Models;
using System.ComponentModel.DataAnnotations;

namespace My_Final_Project.Models
{
    public class CategoryUniqueNameAttribute : ValidationAttribute
    {
        StoreContext context = new StoreContext();
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string name = value.ToString();
            Category newCategory = (Category)validationContext.ObjectInstance;
            var category = context.Categories.FirstOrDefault(a => a.Name == name && a.Id != newCategory.Id);
            if (category != null)
            {
                return new ValidationResult("A Category with the same name already exists.");
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}
