using Assignment.DTO.Login;
using Assignment.DTO.Register;
using Assignment.Models;
using AutoMapper;

namespace Assignment.Common
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //source- CreateCountryDto Destination-Country
            CreateMap<Register,CreateRegisterDto>().ReverseMap();
            CreateMap<Register,ShowDataDto>().ReverseMap();
            CreateMap<Register,RequestLoginDto>().ReverseMap();
            CreateMap<Register,UpdateDataDto>().ReverseMap();
            
        }
    }
}