using ApplicationCore.DTOs;
using ApplicationCore.Entities;
using AutoMapper;

namespace Infrastructure.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Book - BookDTO
            CreateMap<Book, BookDTO>().ReverseMap();

            // Category - CategoryDTO
            CreateMap<Category, CategoryDTO>().ReverseMap();

            // Customer - CustomerDTO
            CreateMap<Customer, CustomerDTO>().ReverseMap();

            // Order - OrderDTO
            CreateMap<Order, OrderDTO>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Customer != null ? src.Customer.FullName : src.GuestFullName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Customer != null ? src.Customer.Email : src.GuestEmail))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Customer != null ? src.Customer.Phone : src.GuestPhone))
                .ForMember(dest => dest.CCCD, opt => opt.MapFrom(src => src.Customer != null ? src.Customer.CCCD : src.GuestCCCD))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Customer != null ? src.Customer.Address : src.GuestAddress));


            CreateMap<OrderDTO, Order>()
                .ForMember(dest => dest.GuestFullName, opt => opt.MapFrom(src => src.CustomerID != null ? null : src.FullName))
                .ForMember(dest => dest.GuestEmail, opt => opt.MapFrom(src => src.CustomerID != null ? null : src.Email))
                .ForMember(dest => dest.GuestPhone, opt => opt.MapFrom(src => src.CustomerID != null ? null : src.Phone))
                .ForMember(dest => dest.GuestCCCD, opt => opt.MapFrom(src => src.CustomerID != null ? null : src.CCCD))
                .ForMember(dest => dest.GuestAddress, opt => opt.MapFrom(src => src.CustomerID != null ? null : src.Address));

            // OrderDetail - OrderDetailDTO
            CreateMap<OrderDetail, OrderDetailDTO>()
                .ForMember(dest => dest.BookName, opt => opt.MapFrom(src => src.Book != null ? src.Book.BookName : null))
                .ReverseMap();

            // Payment - PaymentDTO

            // ShoppingCart - ShoppingCartDTO
            CreateMap<ShoppingCart, ShoppingCartDTO>()
                .ForMember(dest => dest.BookName, opt => opt.MapFrom(src => src.Book != null ? src.Book.BookName : null))
                .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src.Book != null ? src.Book.ImagePath : null))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Book != null ? src.Book.Price : 0))
                .ReverseMap();
        }
    }
}
