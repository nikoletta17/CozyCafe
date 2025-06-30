namespace CozyCafe.Models.DTO
{
    public class CartItemDto
    {
        public int MenuItemId { get; set; }
        public required string MenuItemName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string? ImageUrl { get; set; }
    }
}
