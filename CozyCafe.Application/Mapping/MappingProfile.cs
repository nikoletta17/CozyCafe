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
            CreateMap<Cart, CartDto>()
           .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            CreateMap<CartItem, CartItemDto>()
                .ForMember(dest => dest.MenuItemName, opt => opt.MapFrom(src => src.MenuItem!.Name))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.MenuItem!.Price))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.MenuItem!.ImageUrl));

            // MenuItem
            CreateMap<MenuItem, MenuItemDto>();
            CreateMap<CreateMenuItemDto, MenuItem>();

            // MenuItem Options
            CreateMap<MenuItemOption, MenuItemOptionDto>();
            CreateMap<MenuItemOptionGroup, MenuItemOptionGroupDto>();

            // Orders (User)
            CreateMap<Order, OrderDto>();
            CreateMap<OrderItem, OrderItemDto>()
                     .ForMember(d => d.MenuItemId, opt => opt.MapFrom(s => s.MenuItemId))
                     .ForMember(d => d.MenuItemName, opt => opt.MapFrom(s => s.MenuItem.Name));


            CreateMap<OrderItemOption, OrderItemOptionDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id)); // додаємо Id

            CreateMap<CreateOrderDto, Order>();
            CreateMap<CreateOrderItemDto, OrderItem>();

            // Reviews
            CreateMap<CreateReviewDto, Review>();
            CreateMap<Review, ReviewDto>()
                .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => src.User.FullName));

            // Discount & Category
            CreateMap<Category, CategoryDto>();
           
            // Orders (Admin)
            CreateMap<Order, AdminOrderDto>()
                .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            CreateMap<OrderItem, AdminOrderItemDto>()
                .ForMember(dest => dest.MenuItemName, opt => opt.MapFrom(src => src.MenuItem.Name))
                .ForMember(dest => dest.SelectedOptions,
                    opt => opt.MapFrom(src => src.SelectedOptions.Select(o => o.MenuItemOption.Name).ToList()));
        }
    }
}
