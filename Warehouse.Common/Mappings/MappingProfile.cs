using AutoMapper;
using Warehouse.Common.DTOs;
using Warehouse.Data.Models;

namespace Warehouse.Common.Mappings
{
  public class MappingProfile : Profile
  {
    public MappingProfile()
    {
      // Mappature esistenti
      CreateMap<Categories, DTOCategory>().ReverseMap();
      CreateMap<Cities, DTOCity>().ReverseMap();
      CreateMap<Orders, DTOOrder>()
          .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount)) // Mappatura esplicita
          .ReverseMap();
      CreateMap<Products, DTOProduct>().ReverseMap();
      CreateMap<Suppliers, DTOSupplier>().ReverseMap();
      CreateMap<Users, DTOUser>()
          .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt == default ? (DateTime?)null : src.CreatedAt))
          .ReverseMap();

      // Aggiunta mappatura Owners <-> DTOOwner
      CreateMap<Owners, DTOOwner>().ReverseMap();  // Se hai un DTO per Owners

      // Nuova mappatura Roles <-> DTORole
      CreateMap<Roles, DTORole>().ReverseMap();  // Aggiungi questa riga per mappare i ruoli

      // Aggiungi questa riga per mappare DTOCreateUser in Users
      CreateMap<DTOCreateUser, Users>()
          .ForMember(dest => dest.UserID, opt => opt.Ignore()) // Ignora UserID
          .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow)); // Imposta CreatedAt
    }
  }
}
