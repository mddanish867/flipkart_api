namespace BookStore.API.Data.Models
{
    public class Orders
    {
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal ProductDiscount { get; set; }
        public decimal ProductQuanitity { get; set; }
        public string ProductImageurl { get; set; }
        public string Status { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string Type { get; set; }
        public string Color { get; set; }
        public string Material { get; set; }
        public string Brand { get; set; }
        public string Size { get; set; }
        public decimal rating { get; set; }
        public int ProductId { get; set; }
        public string username { get; set; }
        public string ShippingCharge { get; set; }
        public int Count { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public decimal TotalAmount { get; set; }
        public string Name { get; set; }
        public string PaymentMode { get; set; }
        public DateTime PurchaseDate {get;set;}
        public string OrderTrackId { get; set; }
        public string errormessage { get; set; }
    }
}
