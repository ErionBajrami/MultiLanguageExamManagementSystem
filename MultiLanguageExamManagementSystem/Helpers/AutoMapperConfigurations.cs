using AutoMapper;
using MultiLanguageExamManagementSystem.Models.Dtos;
using MultiLanguageExamManagementSystem.Models.Entities;

namespace LifeEcommerce.Helpers
{
    public class AutoMapperConfigurations : Profile
    {
        public AutoMapperConfigurations() 
        {
            
            #region Language, LocalizationResource

            CreateMap<Language, LanguageRequestDTO>().ReverseMap();
            CreateMap<Language, LanguageResponseDTO>().ReverseMap();
            CreateMap<LocalizationResource, LocalizationResourceRequestDTO>().ReverseMap();
            CreateMap<LocalizationResource, LocalizationResourceResponseDTO>().ReverseMap();

            CreateMap<LocalizationResource, LocalizationResource>();
            

            #endregion

            #region Exam

            CreateMap<Exam, AddExamDTO>().ReverseMap();

            #endregion

            #region User
            

            #endregion
        }
    }
}
