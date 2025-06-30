using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CozyCafe.Models.Domain
{
    public class Order
    {
        public int Id { get; set; }

        public DateTime OrderedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public required string UserId { get; set; }

        public ApplicationUser? User { get; set; }

        public int? DiscountId { get; set; }
        public Discount? Discount { get; set; }

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

        [Required]
        [Range(0.01, 999999)]
        public decimal Total { get; set; }

        public enum OrderStatus
        {
            Pending,     //	Замовлення створене, очікує обробки
            Confirmed,   //	Замовлення підтверджене (оплата/перевірка)
            InProgress,  //Замовлення готується
            Completed,  //	Замовлення виконано
            Cancelled   //	Замовлення скасовано
        }

        [Required]
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
    }
}
