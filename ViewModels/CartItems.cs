using HTNLShop.Data;

namespace HTNLShop.ViewModels
{
    public class CartItems
    {
        public int CartId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }
        public string ProductName { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public decimal Pay => Quantity * Price;
         
    }
}
