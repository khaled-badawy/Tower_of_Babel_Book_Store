using book_store.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace My_Final_Project.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Address { get; set; }
        
        public Gender gender { get; set; } 
        public int Age { get; set; }
        public ICollection<Order>? Orders { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public enum Gender
        {
            male = 1,
            female = 2,
            other = 3
        }
    }
}
