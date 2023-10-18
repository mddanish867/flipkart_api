using System.Drawing;

namespace BookStore.API.Data.Models
{
    public class PreOrders
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public string UserName { get; set; }
        public string errormessage { get; set; }

    }
}
