namespace CozyCafe.Models.DTO
{
    public class MenuItemDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public required string CategoryName { get; set; }
    }
}
