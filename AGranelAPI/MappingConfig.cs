using AGranelAPI.Models;
using AGranelAPI.Models.DTO;
using AutoMapper;

namespace AGranelAPI
{
    public class MappingConfig : Profile
    {
        public MappingConfig() 
        {
            CreateMap<User, UserLoginDTO>().ReverseMap();
            CreateMap<User, UserRegisterDTO>().ReverseMap();
            CreateMap<Producto, ProductoDTO>().ReverseMap();
            CreateMap<Producto, ProductoCreateDTO>().ReverseMap();
            CreateMap<Producto, ProductoUpdateDTO>().ReverseMap();
            CreateMap<Venta, VentaDTO>().ReverseMap();
            CreateMap<Venta, VentaCreateDTO>().ReverseMap();
            CreateMap<DetalleVenta, DetalleVentaCreateDTO>().ReverseMap();
            CreateMap<DetalleVenta, DetalleVentaDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
        }
    }
}
