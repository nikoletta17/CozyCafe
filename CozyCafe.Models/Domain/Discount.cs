using CozyCafe.Models.Domain;
using System.ComponentModel.DataAnnotations;

public class Discount
{
    public int Id { get; set; }

    [Required]
    [MaxLength(20)]
    public required string Code { get; set; }

    [Required]
    [Range(1, 100)]
    public int Percent { get; set; }

    [Required]
    public DateTime ValidFrom { get; set; }

    [Required]
    public DateTime ValidTo { get; set; }

    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
