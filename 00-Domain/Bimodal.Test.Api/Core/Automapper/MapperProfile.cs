using AutoMapper;
using Bimodal.Test.Commands;
using Bimodal.Test.Common;
using Bimodal.Test.Database;

namespace Bimodal.Test.Api.Core.Automapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Customer, CustomerViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.DocumentNumber, opt => opt.MapFrom(src => src.Dni))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber));

            CreateMap<CreateCustomer, CustomerViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.DocumentNumber, opt => opt.MapFrom(src => src.DocumentNumber))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber));
        }
    }
}
