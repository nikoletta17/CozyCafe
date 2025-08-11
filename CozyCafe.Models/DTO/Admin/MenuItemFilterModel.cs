using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozyCafe.Models.DTO.Admin
{
    public class MenuItemFilterModel
    {
        public string? SearchTerm { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }  
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? SortBy { get; set; }
        public int Page { get; set; } = 1;           // поточна сторінка
        public int PageSize { get; set; } = 8;       // кылькысть елементів на сторінку
        public int TotalPages { get; set; }           // загальна кількість сторінок

    }

}
