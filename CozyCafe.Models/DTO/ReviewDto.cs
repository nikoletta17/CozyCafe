using System;

namespace CozyCafe.Models.DTO
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public required string UserFullName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
