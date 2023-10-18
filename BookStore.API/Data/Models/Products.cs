using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
namespace BookStore.API.Data.Models
{
    public class Products
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal ProductDiscount { get; set; }
        public int ProductQuanitity { get; set; }
        public DateTime AddedOn { get; set; }
        public string ProductImageurl { get; set; }
        public int Status { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public decimal rating { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public double PercDiscount { get; set; }
        public string Type { get; set; }
        public string Brands { get; set; }
        public string Material { get; set; }
        public string Sleeve { get; set; }
        public string Fabrick { get; set; }
        public string NeckType { get; set; }
        public string Pattern { get; set; }
        public string errormessage { get; set; }

    }
}
