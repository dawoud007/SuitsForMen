using AutoMapper;
using ElectronicsShop_service.Dtos;
using ElectronicsShop_service.Models;

namespace ElectronicsShop_service.MapperProfiles;
public class ShopMappings : Profile
{
    public ShopMappings()
    {
        CreateMap<Shop, ShopDto>()
 
        .ReverseMap();
    }
}