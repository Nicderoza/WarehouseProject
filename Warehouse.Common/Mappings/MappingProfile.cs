// Warehouse.Common.Mappings/MappingProfile.cs
using AutoMapper;
using Warehouse.Common.DTOs;
using Warehouse.Data.Models;
using System;
using System.Linq; // Necessario per .Sum()

namespace Warehouse.Common.Mappings
{
  public class MappingProfile : Profile
  {
    public MappingProfile()
    {
      CreateMap<Categories, DTOCategory>().ReverseMap();

      CreateMap<Orders, DTOOrder>()
                .ForMember(dest => dest.UserID, opt => opt.MapFrom(src => src.UserID))
                .ForMember(dest => dest.OrderStatusID, opt => opt.MapFrom(src => src.OrderStatusFkID))
                 .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.OrderStatus != null ? src.OrderStatus.StatusName : "Unknown"))
                 .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderItems))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
                .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.OrderDate));


      CreateMap<OrderItems, DTOOrderItem>()
    .ForMember(dest => dest.OrderItemID, opt => opt.MapFrom(src => src.OrderItemID))
    .ForMember(dest => dest.OrderID, opt => opt.MapFrom(src => src.OrderID))
    .ForMember(dest => dest.ProductID, opt => opt.MapFrom(src => src.ProductID))
    .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
    .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
    .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
    .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Product.Category.CategoryName))
    .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Product.Supplier.CompanyName));


      CreateMap<DTOOrderItem, OrderItems>()
    .ForMember(dest => dest.ProductID, opt => opt.MapFrom(src => src.ProductID))
    .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
    .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
    .ForMember(dest => dest.Product, opt => opt.Ignore());


      CreateMap<DTOOrderItemRequest, OrderItems>()
    .ForMember(dest => dest.ProductID, opt => opt.MapFrom(src => src.ProductID))
    .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
    .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.UnitPrice));



      CreateMap<Products, DTOProduct>().ReverseMap();
      CreateMap<Suppliers, DTOSupplier>().ReverseMap();
      CreateMap<Users, DTOUser>().ReverseMap();
      CreateMap<Roles, DTORole>().ReverseMap();

      CreateMap<DTOCreateUser, Users>()
          .ForMember(dest => dest.UserID, opt => opt.Ignore())
          .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

      CreateMap<CartItems, DTOCartItem>()
    .ForMember(dest => dest.CartItemID, opt => opt.MapFrom(src => src.ID))
    .ForMember(dest => dest.ProductID, opt => opt.MapFrom(src => src.ProductID != 0 ? src.ProductID : src.Product.ProductID))
    .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
    .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice))
    .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
    .ForMember(dest => dest.Subtotal, opt => opt.MapFrom(src => src.Quantity * src.UnitPrice));



      CreateMap<Cart, DTOCartResponse>()
    .ForMember(dest => dest.OrderID, opt => opt.MapFrom(src => src.OrderID))
    .ForMember(dest => dest.CartID, opt => opt.MapFrom(src => src.CartID))
    .ForMember(dest => dest.UserID, opt => opt.MapFrom(src => src.UserID))
    .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
    .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status))
    .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.CartItems));



      CreateMap<Categories, DTOCategory>().ReverseMap();
      CreateMap<Cities, DTOCity>().ReverseMap();
      CreateMap<Orders, DTOOrder>()
          .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount)) 
          .ReverseMap();
      CreateMap<Products, DTOProduct>().ReverseMap();
      CreateMap<Suppliers, DTOSupplier>().ReverseMap();
      CreateMap<Users, DTOUser>()
          .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt == default ? (DateTime?)null : src.CreatedAt))
          .ReverseMap();

      CreateMap<Roles, DTORole>().ReverseMap()
        .ForMember(dest => dest.Permissions, opt => opt.MapFrom(src => src.Permissions));

      CreateMap<Permissions, DTOPermission>();

      CreateMap<DTOCreateUser, Users>()
          .ForMember(dest => dest.UserID, opt => opt.Ignore()) 
          .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

      CreateMap<DTOOrderCreateRequest, Orders>()
    .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.Items));


      CreateMap<DTOOrderItemRequest, OrderItems>();


      CreateMap<DTOOrderCreateRequest, Orders>()
      .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.Items))
      .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(_ => DateTime.UtcNow));


      CreateMap<Products, DTOProduct>()
    .ForMember(dest => dest.ProductID, opt => opt.MapFrom(src => src.ProductID))
    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
    .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
    .ForMember(dest => dest.CategoryID, opt => opt.MapFrom(src => src.CategoryID))
    .ForMember(dest => dest.SupplierID, opt => opt.MapFrom(src => src.SupplierID))
    .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
    .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));


      CreateMap<DTOProduct, Products>()
    .ForMember(dest => dest.ProductID, opt => opt.MapFrom(src => src.ProductID))
    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
    .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
    .ForMember(dest => dest.CategoryID, opt => opt.MapFrom(src => src.CategoryID))
    .ForMember(dest => dest.SupplierID, opt => opt.MapFrom(src => src.SupplierID))
    .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
    .ForMember(dest => dest.OrderItems, opt => opt.Ignore()) // Di solito evitiamo mappaggio ciclico
    .ForMember(dest => dest.CartItems, opt => opt.Ignore())  // Idem
    .ForMember(dest => dest.Category, opt => opt.Ignore())
    .ForMember(dest => dest.Supplier, opt => opt.Ignore());


      CreateMap<Suppliers, DTOSupplier>()
    .ForMember(dest => dest.SupplierID, opt => opt.MapFrom(src => src.SupplierID))
    .ForMember(dest => dest.CityID, opt => opt.MapFrom(src => src.CityID))
    .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.CompanyName));


      CreateMap<DTOSupplier, Suppliers>()
    .ForMember(dest => dest.SupplierID, opt => opt.MapFrom(src => src.SupplierID))
    .ForMember(dest => dest.CityID, opt => opt.MapFrom(src => src.CityID))
    .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.CompanyName))
    .ForMember(dest => dest.Products, opt => opt.Ignore()) // evitare mapping ciclico
    .ForMember(dest => dest.UsersSuppliers, opt => opt.Ignore())
    .ForMember(dest => dest.City, opt => opt.Ignore())
    .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
    .ForMember(dest => dest.IsActive, opt => opt.Ignore());


      CreateMap<Cities, DTOCity>();
      CreateMap<DTOCity, Cities>();

      CreateMap<DTORegisterSupplier, Suppliers>()
    .ForMember(dest => dest.CityID, opt => opt.Ignore());


    }
  }
}
