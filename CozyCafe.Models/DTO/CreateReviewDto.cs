using System.ComponentModel.DataAnnotations;

namespace CozyCafe.Models.DTO
{
    public class CreateReviewDto
    {
        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [MaxLength(500)]
        public string? Comment { get; set; }

        [Required]
        public int MenuItemId { get; set; }
    }
}
