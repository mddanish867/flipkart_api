using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BookStore.API.Data.Models
{
    public class AddUser
    {
        //public int UserId { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
       
        public string Email { get; set; }
        
        public string Password { get; set; }
        
        public string ConfirmPassword { get; set; }
        public string MobileNumber { get; set; }
        public string errormessage { get; set; }
        
    }
}
