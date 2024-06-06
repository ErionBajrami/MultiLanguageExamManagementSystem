using AutoMapper;
using MultiLanguageExamManagementSystem.Models.Dtos;
using MultiLanguageExamManagementSystem.Models.Entities;

namespace LifeEcommerce.Helpers
{
    public class AutoMapperConfigurations : Profile
    {
        public AutoMapperConfigurations() 
        {
            CreateMap<Language, LanguageDto>().ReverseMap();
            CreateMap<Country, CountryDto>().ReverseMap();
            CreateMap<LocalizationResource, LocalizationResourceDto>().ReverseMap();
        }
    }
}
