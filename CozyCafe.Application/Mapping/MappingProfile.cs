using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CozyCafe.Models.Domain;
using CozyCafe.Models.DTO;

namespace CozyCafe.Application.Mapping
{
    public class MappingProfile: Profile
    {
        public MappingProfile() 
        {
            CreateMap<Cart, CartDto>();
            CreateMap<CartItem, CartItemDto>();

            CreateMap<MenuItem, MenuItemDto>();
            CreateMap<CreateMenuItemDto, MenuItem>();

            CreateMap<MenuItemOption, MenuItemOptionDto>();
            CreateMap<MenuItemOptionGroup, MenuItemOptionGroupDto>();

            CreateMap<Order, OrderDto>();
            CreateMap<OrderItem, OrderItemDto>();
            CreateMap<OrderItemOption, OrderItemOptionDto>();

            CreateMap<CreateOrderDto, Order>();
            CreateMap<CreateOrderItemDto, OrderItem>();

            CreateMap<Review, ReviewDto>();
            CreateMap<CreateReviewDto, Review>();

            CreateMap<Category, CategoryDto>(); 

            CreateMap<Discount, DiscountDto>(); 

        }
    }
}
