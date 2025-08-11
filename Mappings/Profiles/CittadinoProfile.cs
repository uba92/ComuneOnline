using AutoMapper;
using ComuneOnline.Models.DTOs;
using ComuneOnline.Models.Entities;

namespace ComuneOnline.Mappings.Profiles
{
    public class CittadinoProfile : Profile
    {
        public CittadinoProfile()
        {
            CreateMap<Cittadino, CittadinoDetailsDTO>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.Email) ? "Non disponibile" : src.Email))
                .ForMember(dest => dest.Telefono, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.Telefono) ? "Non disponibile" : src.Telefono));
        }
    }
}
