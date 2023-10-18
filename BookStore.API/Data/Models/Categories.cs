using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
namespace BookStore.API.Data.Models
{
    public class Categories
    {
        public int CategoryId { get; set; }
        public string Category { get; set; }
        public string errormessage { get; set; }

    }
}
