using System.ComponentModel.DataAnnotations;

namespace CozyCafe.Models.DTO.ForUser
{
    public class CreateReviewDto
    {
        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [MaxLength(500)]
        public string Comment { get; set; } = string.Empty;

        [Required]
        public int MenuItemId { get; set; }
    }
}
