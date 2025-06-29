using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozyCafe.Models.DTO
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public required string  Name { get; set; }
    }
}
