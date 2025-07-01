using AutoMapper;
using CozyCafe.Models.Domain.Admin;
using CozyCafe.Models.Domain.Common;
using CozyCafe.Models.Domain.ForUser;
using CozyCafe.Models.DTO.Admin;
using CozyCafe.Models.DTO.ForUser;

namespace CozyCafe.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Cart
            CreateMap<Cart, CartDto>();
            CreateMap<CartItem, CartItemDto>();

            // MenuItem
            CreateMap<MenuItem, MenuItemDto>();
            CreateMap<CreateMenuItemDto, MenuItem>();

            // MenuItem Options
            CreateMap<MenuItemOption, MenuItemOptionDto>();
            CreateMap<MenuItemOptionGroup, MenuItemOptionGroupDto>();

            // Orders (User)
            CreateMap<Order, OrderDto>();
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id)) // додаємо Id
                .ForMember(dest => dest.MenuItemName, opt => opt.MapFrom(src => src.MenuItem.Name));
            CreateMap<OrderItemOption, OrderItemOptionDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id)); // додаємо Id

            CreateMap<CreateOrderDto, Order>();
            CreateMap<CreateOrderItemDto, OrderItem>();

            // Reviews
            CreateMap<Review, ReviewDto>()
                .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => src.User.FullName));
            CreateMap<CreateReviewDto, Review>();

            // Discount & Category
            CreateMap<Category, CategoryDto>();
            CreateMap<Discount, DiscountDto>();

            // Orders (Admin)
            CreateMap<Order, AdminOrderDto>()
                .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.DiscountCode, opt => opt.MapFrom(src => src.Discount != null ? src.Discount.Code : null))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            CreateMap<OrderItem, AdminOrderItemDto>()
                .ForMember(dest => dest.MenuItemName, opt => opt.MapFrom(src => src.MenuItem.Name))
                .ForMember(dest => dest.SelectedOptions,
                    opt => opt.MapFrom(src => src.SelectedOptions.Select(o => o.MenuItemOption.Name).ToList()));
        }
    }
}
