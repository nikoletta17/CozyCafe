using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozyCafe.Models.DTO.Admin
{
    public class MenuItemFilterModel
    {
        public string? SearchTerm { get; set; }       // Пошук за назвою
        public int? CategoryId { get; set; }          // Фільтр за категорією
        public decimal? MinPrice { get; set; }        // Мінімальна ціна
        public decimal? MaxPrice { get; set; }        // Максимальна ціна
        public string? SortBy { get; set; }           // name, price_asc, price_desc
    }
}
