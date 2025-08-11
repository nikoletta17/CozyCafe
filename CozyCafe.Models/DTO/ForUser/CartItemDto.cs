public class CartItemDto
{
    public int MenuItemId { get; set; }
    public required string MenuItemName { get; set; }
    public decimal UnitPrice { get; set; }  // Ціна за одиницю з опціями
    public decimal Price { get; set; }      // Загальна сума = UnitPrice * Quantity
    public int Quantity { get; set; }
    public string? ImageUrl { get; set; }
    public List<string> SelectedOptionNames { get; set; } = new();
}
