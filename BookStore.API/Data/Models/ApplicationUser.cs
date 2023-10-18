using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BookStore.API.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        public string Password { get; internal set; }
        public string ConfirmPassword { get; internal set; }
        public string MobileNumber { get; internal set; }
    }
}
